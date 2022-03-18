using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using App5.components;
using App5.Entities;
using App5.PageElements;
using App5.repo;
using App5.viewmodel;
using SQLiteNetExtensions.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace App5
{
    public partial class NoteListPage : ContentPage
    {
        public static int MOBILE_BREAKPOINT = 600;

        public bool isMobile = false;
        
        public EditorContext EditorContext;

        public NoteListViewModel NoteListViewModel;
        

        public NoteListPage(string noteId)
        {
            InitializeComponent();

            NoteListViewModel = new NoteListViewModel();
            NoteListViewModel.PropertyChanged += NoteListViewModel_PropertyChanged;

            BindingContext = NoteListViewModel;
            
            if (noteId != null)
            {
                Preferences.Set(App.NOTE_ID_KEY, noteId);
                NoteListViewModel.OpenNote(noteId);
            }
            
            EditorContext = new EditorContext();
            EditorContext.SelectTypeChanged += (sender, args) =>
            {
                if (EditorContext.SelectedType == SelectedType.NONE)
                {
                    TypeElementPicker.SelectedItem = null;
                }
            };
            
            TouchEffect touchEffect = new TouchEffect
            {
                Capture = true
            };
            touchEffect.Clicked += (sender, args) =>
            {
                switch (EditorContext.State)
                {
                    case EditorState.NONE:

                        if (EditorContext.SelectedType == SelectedType.TEXT)
                        {
                            NoteListViewModel.SelectedNote.CreateTextElement(args.Location.X, args.Location.Y);
                        }

                        if (EditorContext.SelectedType == SelectedType.IMAGE)
                        {
                            NoteListViewModel.SelectedNote.CreateImageElement(args.Location.X, args.Location.Y);
                        }

                        EditorContext.SelectedType = SelectedType.NONE;

                        break;
                }
            };
            NoteField.Effects.Add(touchEffect);
            
            NoteListViewModel.Navigation = Navigation;
            var TitleView = new Label()
            {
                Text = "Заметки",
                FontSize = 22,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            NavigationPage.SetTitleView(this, TitleView);
        }

        private void NoteListViewModel_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            string propertyName = args.PropertyName;

            if (propertyName == nameof(NoteListViewModel.SelectedNote) && NoteListViewModel.SelectedNote != null)
            {
                NoteListViewModel.SelectedNote.TextElements.ForEach(addTextElement);
                NoteListViewModel.SelectedNote.TextElements.CollectionChanged += (sender1, args1) =>
                {
                    NoteField.Children.Clear();
                    NoteListViewModel.SelectedNote.TextElements.ForEach(addTextElement);
                    NoteListViewModel.SelectedNote.ImageElements.ForEach(addImageElement);
                };
                
                NoteListViewModel.SelectedNote.ImageElements.ForEach(addImageElement);
                NoteListViewModel.SelectedNote.ImageElements.CollectionChanged += (sender1, args1) =>
                {
                    NoteField.Children.Clear();
                    NoteListViewModel.SelectedNote.TextElements.ForEach(addTextElement);
                    NoteListViewModel.SelectedNote.ImageElements.ForEach(addImageElement);
                };
            }

            
        }

        private void addImageElement(ImageElementViewModel imageElementViewModel)
        {
            var image = new Image();
            image.Source = imageElementViewModel.ImageSource;
            image.BindingContext = imageElementViewModel;
            image.SetBinding(Image.TranslationXProperty, "Position.Item1", BindingMode.OneWay);
            image.SetBinding(Image.TranslationYProperty, "Position.Item2", BindingMode.OneWay);

            /*image.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Image.TranslationX) ||
                    args.PropertyName == nameof(Image.TranslationY))
                {
                    
                }
            };*/
            
            TouchEffect touchEffect = new TouchEffect
            {
                Capture = true,
            };
            touchEffect.Moved += (sender, args) =>
            {
                image.TranslationX += args.Item1;
                image.TranslationY += args.Item2;
            };
            touchEffect.Realeased += (sender, args) =>
            {
                imageElementViewModel.Position = new Tuple<double, double>(image.TranslationX, image.TranslationY);
            };
            image.Effects.Add(touchEffect);
            
            NoteField.Children.Add(image);
        }

        private void addTextElement(TextElementViewModel textElementViewModel)
        {
            var textElementView = new TextElementView();

            textElementView.BindingContext = textElementViewModel;
            textElementView.SetBinding(TextElementView.TextProperty, "Text", BindingMode.TwoWay);
            textElementView.SetBinding(TextElementView.FontSizeProperty, "FontSize", BindingMode.TwoWay);
            textElementView.SetBinding(TextElementView.PositionProperty, "Position", BindingMode.TwoWay);

            textElementView.Clicked += (sender, args) =>
            {
                var editableText = (TextElementView) sender;

                if (EditorContext.currentItem != null &&
                    EditorContext.currentItem != editableText)
                {
                    EditorContext.currentItem.ToLabel();
                    EditorContext.currentItem.Unselct();
                    EditorContext.State = EditorState.NONE;
                }

                switch (EditorContext.State)
                {
                    case EditorState.NONE:
                        EditorContext.currentItem = editableText;
                        editableText.Select();
                        EditorContext.State = EditorState.SELECTED;
                        NoteListViewModel.SelectedNote.SelectedTextElement = textElementViewModel;
                        break;
                    case EditorState.EDITING:

                        break;
                    case EditorState.SELECTED:
                        editableText.ToEditor();
                        editableText.Unselct();
                        EditorContext.State = EditorState.EDITING;
                        break;
                }
            };

            textElementView.Unfocused += (sender, args) =>
            {
                var editableText = (TextElementView) sender;
                if (EditorContext.currentItem == editableText)
                {
                    EditorContext.State = EditorState.NONE;
                    EditorContext.currentItem.ToLabel();
                    EditorContext.currentItem = null;
                    NoteListViewModel.SelectedNote.SelectedTextElement = null;
                }
            };

            NoteField.Children.Add(textElementView);
        }

        private void ToggleNavigationDrawer(object sender, EventArgs e)
        {
            NavigationDrawer.togleNavDrawer();
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            NoteListViewModel.ClickNote(e.SelectedItem);
        }

        private void NotePage_OnSizeChanged(object sender, EventArgs e)
        {
            isMobile = this.Width < MOBILE_BREAKPOINT;
            NavigationDrawer.redraw(isMobile, this.Width);
        }

        private void TypeElementPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (TypeElementPicker.SelectedIndex != -1)
            {
                var selectedName = TypeElementPicker.Items[TypeElementPicker.SelectedIndex];
                SelectedType o = (SelectedType) Enum.Parse(typeof(SelectedType), selectedName.ToUpper());
                EditorContext.SelectedType = o;
            }
        }
    }
}