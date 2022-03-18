using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App5.Entities;
using App5.model;
using App5.repo;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;

namespace App5.viewmodel
{
    public class ImageElementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ImageElement ImageElement;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            App.Service.UpdateEntity(ImageElement);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ImageElementViewModel(ImageElement imageElement)
        {
            ImageElement = imageElement;
        }
        

        public Tuple<double, double> Position
        {
            get { return new Tuple<double, double>(ImageElement.x, ImageElement.y); }
            set
            {
                if (ImageElement.x != (long) value.Item1 ||
                    ImageElement.y != (long) value.Item2)
                {
                    ImageElement.x = (long) value.Item1;
                    ImageElement.y = (long) value.Item2;
                    OnPropertyChanged("Position");
                }
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                return App.Service.getImageSource(ImageElement.image);
            }
        }
        

    }
}