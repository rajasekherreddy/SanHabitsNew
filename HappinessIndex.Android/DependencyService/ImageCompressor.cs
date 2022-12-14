using System;
using Android.Graphics;
using HappinessIndex.DependencyService;
using HappinessIndex.Droid.DependencyService;

[assembly: Xamarin.Forms.Dependency(typeof(ImageCompressor))]
namespace HappinessIndex.Droid.DependencyService
{
    public class ImageCompressor : IImageCompressor
    {
        public async void Compress(string path)
        {
            using (System.IO.Stream stream = System.IO.File.Create(path))
            {
                Bitmap bitmap = await BitmapFactory.DecodeStreamAsync(stream);
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 30, stream);
            }
        }
    }
}