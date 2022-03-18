using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using App5.Entities;
using App5.repo;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace App5.viewmodel
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static string[] rus = new[] {"Белый", "Красный", "Синий", "Желтый"};
        public static string[] eng = new[] {"WHITE", "RED", "BLUE", "YELLOW"};
        public static Color[] colors = new[] {Color.FromArgb(248, 248, 248), Color.FromArgb(255, 193, 171), Color.LightBlue, Color.LightYellow};

        public ICommand AttachGeolocationCommand { get; set; }
        
        private string[] fontSizes = new[]
            {"18", "20", "22", "24", "25", "26", "27", "28", "29", "30"};
        
        private bool _showsFontPicker;
        private TextElementViewModel _selectedTextElement;
        public ObservableCollection<TextElementViewModel> TextElements { get; set; }
        public ObservableCollection<ImageElementViewModel> ImageElements { get; set; }
        
        public Note Note { get; set; }
        
        CancellationTokenSource cts;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            App.Service.UpdateEntity(Note);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void AttachGeolocation()
        {
            try
            {
                var whenInUse = await  Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                var always = await Permissions.RequestAsync<Permissions.LocationAlways>();
            
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Note.latitude = location.Latitude;
                    Note.longitude = location.Longitude;
                    OnPropertyChanged("HasLocation");
                    OnPropertyChanged("LocationText");
                    // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
        public NoteViewModel(Note note)
        {
            Note = note;

            AttachGeolocationCommand = new Command(AttachGeolocation);
            
            TextElements = new ObservableCollection<TextElementViewModel>();
            foreach (var noteElement in note.textElements)
            {
                TextElements.Add(new TextElementViewModel(noteElement));
            }
            
            ImageElements = new ObservableCollection<ImageElementViewModel>();
            foreach (var imageElement in note.imageElements)
            {
                ImageElements.Add(new ImageElementViewModel(imageElement));
            }
        }

        public bool HasLocation
        {
            get
            {
                return Note.latitude != null && Note.longitude != null;
            }
        }

        public string LocationText
        {
            get
            {
                return $"Latitude: {Note.latitude}, Longitude: {Note.longitude}";
            }
        }

        public void CreateTextElement(double x, double y)
        {
            var noteElement = App.Service.CreateTextElement(Note, (long) x - 20, (long) y - 20, int.Parse(AvailableFontSizes[0]));
            TextElements.Add(new TextElementViewModel(noteElement));
        }

        public async void CreateImageElement(double x, double y)
        {
            var fileResult = await MediaPicker.CapturePhotoAsync();

            var fileInfo = App.Service.SaveImage(fileResult.FullPath);

            // img.Source = ImageSource.FromFile(fileResult.FullPath);

            var noteElement = App.Service.CreateImageElement(Note, (long) x - 20, (long) y - 20, fileInfo);
            ImageElements.Add(new ImageElementViewModel(noteElement));
        }

        public string Id
        {
            get { return Note.id; }
        }

        public string Title
        {
            get { return Note.title; }
            set
            {
                if (Note.title != value)
                {
                    Note.title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public Color Color
        {
            get
            {
                var i = Array.IndexOf(eng, Note.color);
                return colors[i];
            }
            set
            {
                var i = Array.IndexOf(colors, value);
                if (Note.color != eng[i])
                {
                    Note.color = eng[i];
                    OnPropertyChanged("Color");
                }
            }
        }

        public string ColorTitle
        {
            get
            {
                var i = Array.IndexOf(eng, Note.color);
                return rus[i];
            }
            set
            {
                var i = Array.IndexOf(rus, value);
                if (Note.color != eng[i])
                {
                    Note.color = eng[i];
                    OnPropertyChanged("Color");
                }
            }
        }

        public string[] ColorTitlesRus
        {
            get { return rus; }
        }

        public bool ShowsFontPicker
        {
            get => _showsFontPicker;
            set
            {
                if (_showsFontPicker != value)
                {
                    _showsFontPicker = value;
                    OnPropertyChanged("ShowsFontPicker");
                }
            }
        }
        
        public string[] AvailableFontSizes
        {
            get{ return fontSizes; }
        }
        
        public TextElementViewModel SelectedTextElement
        {
            get => _selectedTextElement;
            set
            {
                if (_selectedTextElement != value)
                {
                    _selectedTextElement = value;
                    OnPropertyChanged("SelectedTextElement");
                    ShowsFontPicker = _selectedTextElement != null;
                }
            }
        }
    }
}