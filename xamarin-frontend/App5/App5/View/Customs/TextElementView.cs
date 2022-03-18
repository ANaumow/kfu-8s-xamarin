using System;
using App5.Entities;
using App5.repo;
using Xamarin.Forms;

namespace App5.PageElements
{
    public class TextElementView : AbsoluteLayout
    {
        private bool _isEditing = false;

        private Editor Editor;
        private Label Label;

        public event EventHandler<EventArgs> Clicked;
        public event EventHandler<EventArgs> Unfocused;

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TextElementView), propertyChanged:
                (bindable, value, newValue) =>
                {
                    var textElementView = ((TextElementView) bindable);
                    var fontSize = ((double) newValue);
                    textElementView.Label.FontSize = fontSize;
                    textElementView.Editor.FontSize = fontSize;
                });

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(TextElementView), propertyChanged:
                (bindable, value, newValue) =>
                {
                    var textElementView = ((TextElementView) bindable);
                    var text = ((string) newValue);
                    textElementView.Label.Text = text;
                    textElementView.Editor.Text = text;
                });

        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(Tuple<double, double>), typeof(TextElementView),
                propertyChanged: (bindable, value, newValue) =>
                {
                    var textElementView = ((TextElementView) bindable);
                    var position = ((Tuple<double, double>) newValue);
                    textElementView.TranslationX = position.Item1;
                    textElementView.TranslationY = position.Item2;
                });


        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double FontSize
        {
            get { return (double) GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Tuple<double, double> Position
        {
            get { return (Tuple<double, double>) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public TextElementView()
        {
            TouchEffect touchEffect = new TouchEffect
            {
                Capture = true,
            };
            touchEffect.Moved += (sender, args) =>
            {
                TranslationX += args.Item1;
                TranslationY += args.Item2;
            };
            touchEffect.Clicked += (sender, args) =>
            {
                Clicked?.Invoke(sender, args);
            };
            touchEffect.Realeased += (sender, args) =>
            {
                Position = new Tuple<double, double>(TranslationX, TranslationY);
            };
            Effects.Add(touchEffect);

            Editor = new Editor() {AutoSize = EditorAutoSizeOption.TextChanges};
            Editor.Unfocused += (sender, args) =>
            {
                Text = Editor.Text;
                Unfocused.Invoke(this, args);
            };
            
            Label = new Label() {Padding = 8};

            Children.Add(_isEditing ? (View)Editor : (View)Label);
        }

        public void ToLabel()
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                Label.Text = Text;
                Children.Add(Label);
                Label.HeightRequest = Editor.Height;
                Label.WidthRequest = Editor.Width;
                Label.FontSize = Editor.FontSize;
                Children.Remove(Editor);
            });
        }

        public void ToEditor()
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                Children.Add(Editor);
                Editor.Focus();
                Children.Remove(Label);
            });
        }

        public void Select()
        {
            Label.Focus();
            Label.BackgroundColor = Color.Bisque;
        }

        public void Unselct()
        {
            Label.BackgroundColor = Color.Transparent;
        }
    }
}