using System;
using MvvmAspire;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace test
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var navigation = Resolver.Get<MvvmAspire.Services.INavigation>();
            MainPage = ((MvvmAspire.Services.XamarinFormsNavigation)navigation).NavigationPage;
            MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.Black);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
