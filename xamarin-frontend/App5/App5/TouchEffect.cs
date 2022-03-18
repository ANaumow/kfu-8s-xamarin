using System;
using App5.PageElements;
using Xamarin.Forms;

namespace App5
{
    public class TouchEffect : RoutingEffect
    {
        
        public bool _isBeingDragged;
        public long _touchId;
        Point _startPoint;
        private Point _currentPoint;

        public event EventHandler<Tuple<double, double>> Moved;
        public event EventHandler<TouchActionEventArgs> Realeased;
        public event EventHandler<TouchActionEventArgs> Clicked; 
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("XamarinDocs.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_isBeingDragged)
                    {
                        _isBeingDragged = true;
                        _touchId = args.Id;
                        _startPoint = new Point
                        {
                            X = args.Location.X,
                            Y = args.Location.Y
                        };

                        _currentPoint = new Point
                        {
                            X = args.Location.X,
                            Y = args.Location.Y
                        };


                        System.Diagnostics.Debug.WriteLine("Pressed");
                        System.Diagnostics.Debug.WriteLine(_startPoint.X);
                    }

                    break;

                case TouchActionType.Moved:
                    if (_isBeingDragged && _touchId == args.Id)
                    {
                        var deltaX = args.Location.X - _startPoint.X;
                        var deltaY = args.Location.Y - _startPoint.Y;
                        
                        Moved?.Invoke(element, new Tuple<double, double>(deltaX, deltaY));

                        _currentPoint.X = args.Location.X;
                        _currentPoint.Y = args.Location.Y;
                    }

                    break;

                case TouchActionType.Released:
                    if (_isBeingDragged && _touchId == args.Id)
                    {
                        _isBeingDragged = false;

                        if (_currentPoint.X == _startPoint.X &&
                            _currentPoint.Y == _startPoint.Y)
                        {
                            Clicked?.Invoke(element, args);
                        }
                        else
                        {
                            Realeased?.Invoke(element, args);
                        }
                    }

                    break;
                default:
                    break;
            }
        }
    }
}