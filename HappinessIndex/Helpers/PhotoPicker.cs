using System;
using System.IO;
using System.Threading.Tasks;
using HappinessIndex.DependencyService;
using HappinessIndex.Resx;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HappinessIndex.Helpers
{
    public static class PhotoPicker
    {
        public async static Task<MediaFile> Pick()
        {
            var expectation = await Application.Current.MainPage.DisplayActionSheet(AppResources.UploadImage, AppResources.Cancel, null, AppResources.TakeAPicture, AppResources.PickAPhotoFromAlbum);

            MediaFile mediaFile = null;

            if (expectation == AppResources.PickAPhotoFromAlbum)
            {
                var storagePermission = Xamarin.Forms.Device.RuntimePlatform == "iOS" ?
                            await Permissions.CheckStatusAsync<Permissions.Photos>() : await Permissions.CheckStatusAsync<Permissions.StorageRead>();


                if (storagePermission != PermissionStatus.Granted)
                {
                    storagePermission = Xamarin.Forms.Device.RuntimePlatform == "iOS" ? await Permissions.RequestAsync<Permissions.Photos>() :
                        await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                if (storagePermission == PermissionStatus.Granted)
                {
                    if (CrossMedia.Current.IsPickPhotoSupported)
                    {
                        if (Device.RuntimePlatform == "iOS")
                        {
                            mediaFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                            {
                                PhotoSize = PhotoSize.Medium,
                            });
                        }
                        else
                        {
                            mediaFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                            {
                                CompressionQuality = 50,
                                PhotoSize = PhotoSize.Medium,
                            });
                        }

                        //mediaFile = MoveFromTempToPictures(mediaFile);
                    }
                }
            }
            else if (expectation == AppResources.TakeAPicture)
            {
                var cameraPermission =
             await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (cameraPermission != PermissionStatus.Granted)
                {
                    cameraPermission = await Permissions.RequestAsync<Permissions.Camera>();
                }

                if (cameraPermission == PermissionStatus.Granted)
                {
                    if (CrossMedia.Current.IsCameraAvailable)
                    {
                        if (Device.RuntimePlatform == "iOS")
                        {
                            mediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                            {

                            });
                        }
                        else
                        {
                            mediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                            {
                                CompressionQuality = 50
                            });
                        }

                        //mediaFile = MoveFromTempToPictures(mediaFile);
                    }
                }
            }
            else if (expectation == AppResources.Cancel)
            {
                return null;
            }

            if (mediaFile == null)
            {
                return null;
            }

            //highlights.Photo = mediaFile.Path;

            if (Device.RuntimePlatform == "iOS")
            {
                Xamarin.Forms.DependencyService.Resolve<IImageCompressor>().Compress(mediaFile.Path);
            }
            return mediaFile;
        }

        private static MediaFile MoveFromTempToPictures(MediaFile mediaFile)
        {
            if (mediaFile == null) return null;

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                AppSettings.JournalDate.ToString("MM-dd-yyyy"));

            if (Directory.Exists(folderPath))
            {
                var files = Directory.GetFiles(folderPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, AppSettings.JournalDate.ToString("hh:mm:ss") + Path.GetExtension(mediaFile.Path));

            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Move(mediaFile.Path, filePath);

            mediaFile = new MediaFile(filePath, null);
            return mediaFile;
        }
    }
}