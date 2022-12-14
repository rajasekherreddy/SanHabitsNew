using System;
using Foundation;
using HappinessIndex.DependencyService;
using HappinessIndex.iOS.DependencyService;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ImageCompressor))]
namespace HappinessIndex.iOS.DependencyService
{
    public class ImageCompressor : IImageCompressor
    {
        public void Compress(string path)
        {
            UIImage image = new UIImage(path);
            NSData data = image.AsJPEG(0.3f);
            data.Save(path, false);
        }
    }
}