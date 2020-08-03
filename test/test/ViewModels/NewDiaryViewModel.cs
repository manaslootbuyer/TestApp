using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmAspire;
using Plugin.Media;
using test.Resources;
using Xamarin.Forms;

namespace test.ViewModels
{
    public class NewDiaryViewModel : AppViewModel
    {

        #region Commands
        public RelayCommand AddPhotoCommand { get; set; }
        #endregion

        private List<ImageSource> _photoImageSource;
        public List<ImageSource> PhotoImageSource
        {
            get => _photoImageSource;
            set => SetProperty(ref _photoImageSource, value);
        }


        private bool _photoImageSources;
        public bool PhotoImageSources
        {
            get => _photoImageSources;
            set => SetProperty(ref _photoImageSources, value);
        }
        //todo use xamarin essential to retrieve location address
        private string _location= "Retrieving current location...";
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }
        public NewDiaryViewModel()
        {
            PhotoImageSource = new List<ImageSource>();
            AddPhotoCommand = new RelayCommand(async () => await AddPhoto());
        }

        public async Task AddPhoto()
        {
          var options = await Page.DisplayActionSheet(Strings.AddAPhoto, Strings.Cancel, "", new string[] { Strings.TakePhoto, Strings.AddGallery });
            await CrossMedia.Current.Initialize();


            if (options== "Take Photo")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    //todo transfer on string resx
                    await Page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = DateTime.Now.ToLongDateString()
                });

                var src = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
                PhotoImageSource.Add(src);
                PhotoImageSources = !PhotoImageSources;
            }
            else if (options == "Add from gallery")
            {

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    //todo transfer on string resx
                    await Page.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                });

               var src= ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                PhotoImageSource.Add(src);
                PhotoImageSources = !PhotoImageSources;
                var test = PhotoImageSource.Count();
            }
               
        }
        public override void Init()
        {
            base.Init();
        }

       
    }
}

