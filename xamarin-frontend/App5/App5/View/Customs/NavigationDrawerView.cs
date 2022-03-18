using System;
using Xamarin.Forms;

namespace App5.components
{
    public class NavigationDrawerView : StackLayout
    {
        private bool _isMobile;
        private bool _isNavDrawerOpen = true;
        private double _parentWidth;
        
        public static readonly BindableProperty HeaderOpenedProperty =
            BindableProperty.Create(nameof(HeaderOpened), typeof(DataTemplate), typeof(ItemsView<StackLayout>), null);
        
        public static readonly BindableProperty HeaderClosedProperty =
            BindableProperty.Create(nameof(HeaderClosed), typeof(DataTemplate), typeof(ItemsView<StackLayout>), null);
        
        public DataTemplate HeaderOpened
        {
            get => (DataTemplate)GetValue(HeaderOpenedProperty);
            set => SetValue(HeaderOpenedProperty, value);
        }

        public DataTemplate HeaderClosed
        {
            get => (DataTemplate)GetValue(HeaderClosedProperty);
            set => SetValue(HeaderClosedProperty, value);
        }

        private View _openedHeader;
        private View _closedHeader;

        public NavigationDrawerView()
        {
            Children.Insert(0, new StackLayout());
        }
        
        public void redraw(bool isMobile, double parentWidth)
        {
            _openedHeader = ((ViewCell) HeaderOpened.CreateContent()).View;
            _closedHeader = ((ViewCell) HeaderClosed.CreateContent()).View;
            
            var wasMobile = _isMobile;
            _isMobile = isMobile;
            _parentWidth = parentWidth;
            
            redrawNavDrawer();

            if (!wasMobile && isMobile)
            {
                closeNavDrawer();
            }
            
            if (wasMobile && !isMobile)
            {
                openNavDrawer();
            }
        }
        
        public void togleNavDrawer()
        {
            if (_isNavDrawerOpen)
            {
                closeNavDrawer();
            }
            else
            {
                openNavDrawer();
            }
        } 
        
        public void redrawNavDrawer()
        {
            if (_isNavDrawerOpen)
            {
                openNavDrawer();
            }
            else
            {
                closeNavDrawer();
            }
        }
        
        private void closeNavDrawer()
        {
            
            Children.RemoveAt(0);
            Children.Insert(0, _closedHeader);
            _isNavDrawerOpen = false;
            WidthRequest = 100;
        }
        
        private void openNavDrawer()
        {
            _isNavDrawerOpen = true;
            
            Children.RemoveAt(0);
            Children.Insert(0, _openedHeader);

            if (_isMobile)
            {
                WidthRequest = 400;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else
            {
                WidthRequest = Math.Max(300, Math.Min(_parentWidth * 0.3, 400));
            }
        }
        
    }
}