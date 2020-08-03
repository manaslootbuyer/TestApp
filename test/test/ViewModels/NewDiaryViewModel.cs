using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MvvmAspire;
using Plugin.Media;
using test.Models;
using test.Resources;
using test.Services;
using Xamarin.Forms;

namespace test.ViewModels
{
    public class NewDiaryViewModel : AppViewModel
    {

        #region Commands
        public RelayCommand AddPhotoCommand { get; set; }
        public RelayCommand SubmitCommand { get; set; }
        #endregion

        private List<ImageSource> _photoImageSources;
        public List<ImageSource> PhotoImageSources
        {
            get => _photoImageSources;
            set => SetProperty(ref _photoImageSources, value);
        }


        private bool _photoImageSource;
        public bool PhotoImageSource
        {
            get => _photoImageSource;
            set => SetProperty(ref _photoImageSource, value);
        }
        //todo use xamarin essential to retrieve location address
        private string _location= "Retrieving current location...";
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        IDiaryService _diaryService;

        UserCommand _userCommand;
        public NewDiaryViewModel(IDiaryService diaryService)
        {
            _diaryService = diaryService;
            PhotoImageSources = new List<ImageSource>();
            AddPhotoCommand = new RelayCommand(async () => await AddPhoto());
            SubmitCommand = new RelayCommand(async () => await Submit());
        }

        public async Task Submit()
        {
            SubmitCommand.CanRun = false;

            if (PhotoImageSources != null && PhotoImageSources.Any())
            {
                ShowMainLoader = true;
                //todo create api with list of image
                var byteImage = ImageSourceToByteArray(PhotoImageSources.FirstOrDefault());

                _userCommand = new UserCommand
                {
                    //todo cannot post the base64 the fake api returning paylood too large
                    Name = "Payload Too Large " //Convert.ToBase64String(byteImage)
                };
                var user = await _diaryService.AddDiaryAsync(_userCommand);
                if (user != null){

                    await Page.DisplayAlert("", Strings.SavedDiary, "Ok");
                }

                ShowMainLoader = false;
            }
            else
            {
                await Page.DisplayAlert("", Strings.PhotoRequired, "Ok");
            }
            SubmitCommand.CanRun = true;
        }

        public async Task AddPhoto()
        {
            AddPhotoCommand.CanRun = false;
          var options = await Page.DisplayActionSheet(Strings.AddAPhoto, Strings.Cancel, "", new string[] { Strings.TakePhoto, Strings.AddGallery });
            await CrossMedia.Current.Initialize();


            if (options== "Take Photo")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    //todo transfer on string resx
                    await Page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    AddPhotoCommand.CanRun = true;
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
                PhotoImageSources.Add(src);
                PhotoImageSource = !PhotoImageSource;
            }
            else if (options == "Add from gallery")
            {

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    //todo transfer on string resx
                    await Page.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    AddPhotoCommand.CanRun = true;
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

                PhotoImageSources.Add(src);
                PhotoImageSource = !PhotoImageSource;
                var test = PhotoImageSources.Count();
            }
            AddPhotoCommand.CanRun = true;
        }
        public override void Init()
        {
            base.Init();
        }
        private byte[] ImageSourceToByteArray(ImageSource source)
        {
            StreamImageSource streamImageSource = (StreamImageSource)source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;

            byte[] b;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                b = ms.ToArray();
            }

            return b;
        }

    }
}

