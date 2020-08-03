using System;
using System.Collections.Generic;
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

            if(e.PropertyName == "PhotoImageSources")
            {

                PhotoHolder.Children.Clear();
                foreach(var imgSrc in _vm.PhotoImageSource)
                {
                    PhotoHolder.Children.Add(new Image
                    {
                        HeightRequest = 100,
                        WidthRequest = 100,
                        Aspect = Aspect.AspectFill,
                        Source = imgSrc
                    });
                }
                
            }
        }


    }
}
