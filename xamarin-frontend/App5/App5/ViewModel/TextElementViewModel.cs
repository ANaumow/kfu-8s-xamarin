using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App5.Entities;
using App5.repo;
using SQLiteNetExtensions.Extensions;

namespace App5.viewmodel
{
    public class TextElementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Service Service;

        public TextElement TextElement;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            App.Service.UpdateEntity(TextElement);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TextElementViewModel(TextElement textElement)
        {
            TextElement = textElement;
        }
        
        public double FontSize
        {
            get { return TextElement.fontSize; }
            set
            {
                TextElement.fontSize = (int) value;
                OnPropertyChanged("FontSize");
                OnPropertyChanged("FontSizeTitle");
            }
        }

        public string Text
        {
            get { return TextElement.text; }
            set
            {
                if (TextElement.text != value)
                {
                    TextElement.text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public Tuple<double, double> Position
        {
            get { return new Tuple<double, double>(TextElement.x, TextElement.y); }
            set
            {
                TextElement.x = (long) value.Item1;
                TextElement.y = (long) value.Item2;
                OnPropertyChanged("Position");
            }
        }

        public string FontSizeTitle
        {
            get { return ((int) FontSize) + ""; }
            set
            {
                if (value == null)
                    return;
                
                FontSize = double.Parse(value);
                OnPropertyChanged("FontSizeTitle");
            }
        }

    }
}