using MvvmAspire;
using MvvmAspire.Services;
using MvvmAspire.Unity;
using test.Services;
using test.ViewModels;
using test.Views;
using Unity;
using Unity.Lifetime;

namespace test
{
    public static class AppBootstrap
    {
        static UnityContainer container;

        public static UnityDependencyResolver Init(bool forBackgroundService = false)
        {
            container = new UnityContainer();
            var resolver = new UnityDependencyResolver(container);

            if (!forBackgroundService)
            {
                Resolver.SetResolver(resolver);
                container.RegisterInstance<INavigation>(RegisterNavigation());
                container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
                container.RegisterType<IDiaryService, DiaryService>(new ContainerControlledLifetimeManager());
                RegisterMapping();
            }
            return resolver;
        }

        public static void RegisterOverrides() => RegisterOverrides(Resolver.Current);

        public static void RegisterOverrides(IDependencyResolver resolver) => resolver.GetContainer();

        /// <summary>
        /// Registration of Navigation MVVM
        /// </summary>
        /// <returns></returns>
        public static INavigation RegisterNavigation()
        {
            var navigation = new XamarinFormsNavigation(() => App.Current);
         
         navigation.Register<NewDiaryViewModel, NewDiaryPage>();
   
            return navigation;
        }

        /// <summary>
        /// Mapping DataModels
        /// </summary>
        static void RegisterMapping()
        {

        }
    }
}
