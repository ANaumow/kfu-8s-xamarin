using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using App5.Entities;
using App5.repo;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;

namespace App5.viewmodel
{
    public class NoteListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        private NoteViewModel _selectedNote;
        private ObservableCollection<NoteViewModel> _allNotes = new ObservableCollection<NoteViewModel>();

        public ICommand CreateNoteCommand { protected set; get; }
        public ICommand GoToAccountCommand { protected set; get; }
        public ICommand DeleteSelectedNoteCommand  { protected set; get; }
        public ICommand PullAccountCommand { protected set; get; }

        public ObservableCollection<NoteViewModel> AllNotes
        {
            get => _allNotes;
            set => _allNotes = value;
        }

        public NoteListViewModel()
        {
            CreateNoteCommand = new Command(CreateNote);
            GoToAccountCommand = new Command(GoToAcoount);
            DeleteSelectedNoteCommand = new Command(DeleteSelectedNote);
            PullAccountCommand = new Command(App.Service.StartAccountPullingSync);

            UpdateData();
            App.Service.AccoundUpdated += (sender, args) => UpdateData();
        }

        private void UpdateData()
        {
            AllNotes.Clear();
            foreach (var note in App.Service.GetAllNotes())
            {
                AllNotes.Add(new NoteViewModel(note));
                OnPropertyChanged("AllNotes");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DeleteSelectedNote()
        {
            App.Service.DeleteNote(SelectedNote.Note);
            if (!AllNotes.Remove(SelectedNote))
            {
                throw new Exception();
            }
            NavigateToNote(null);
            OnPropertyChanged("AllNotes");
            OnPropertyChanged("SelectedNote");
            OnPropertyChanged("IsNoteSelected");
        } 
        
        public void OpenNote(string noteId)
        {
            try
            {
                _selectedNote = AllNotes.Where((note => note.Id == noteId)).First();
                OnPropertyChanged("SelectedNote");
                OnPropertyChanged("IsNoteSelected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ClickNote(object o)
        {
            if (o is NoteViewModel noteViewModel)
                NavigateToNote(noteViewModel.Id);
        }

        public void CreateNote()
        {
            var note = App.Service.CreateNote();
            NavigateToNote(note.id);
        }
        
        private void NavigateToNote(string noteId)
        {
            Navigation.PushAsync(new NoteListPage(noteId), false);
        } 

        public void GoToAcoount()
        {
            Navigation.PushAsync(new AccountPage());
        }

        public bool IsNoteSelected
        {
            get => SelectedNote != null;
        }

        public NoteViewModel SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (_selectedNote != value )
                {
                    if (value != null)
                    {
                        NavigateToNote(value.Id);
                    }
                    else
                    {
                        NavigateToNote(null);
                    }
                    OnPropertyChanged("SelectedNote");
                    OnPropertyChanged("IsNoteSelected");
                }
            }
        }

        public List<string> NoteEementTypes
        {
            get => new List<string>(new[] {"text", "image"});
        }
    }
}