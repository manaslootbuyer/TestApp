using System;
using System.Collections.Generic;
using test.Resources;
using test.ViewModels;
using Xamarin.Forms;

namespace test.Views
{
    public partial class NewDiaryPage : ContentPage
    {
        NewDiaryViewModel _vm;
        public NewDiaryPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _vm = (NewDiaryViewModel)this.BindingContext;
            _vm.PropertyChanged +=Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if(e.PropertyName == "PhotoImageSource")
            {

                PhotoHolder.Children.Clear();
                foreach(var imgSrc in _vm.PhotoImageSources)
                {
                    PhotoHolder.Children.Add(PhotoGrid(imgSrc));
                }
            }
        }


         Grid PhotoGrid(ImageSource imgSrc)
        {
            var parentGrid = new Grid
            {

                HeightRequest = 120,
                WidthRequest = 105,
            };


            var grid = new Grid
            {
                HeightRequest = 85,
                WidthRequest = 85,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            var img = new Image
            {
                HeightRequest = 85,
                WidthRequest = 85,
                Aspect = Aspect.AspectFill,
                Source = imgSrc
            };

            var imgRemove = new Image
            {
                HeightRequest = 25,
                WidthRequest = 25,
                Source = Images.RemoveIco,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,

            };
            grid.Children.Add(img);
            parentGrid.Children.Add(grid);
            parentGrid.Children.Add(imgRemove);

            return parentGrid;
        }


    }
}
