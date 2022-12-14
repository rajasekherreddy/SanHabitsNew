using System;
using Xamarin.Forms;

namespace HappinessIndex.Controls
{
    public class DataTemplateView : ContentView
    {
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly BindableProperty ContentTemplateProperty =
                        BindableProperty.Create("ContentTemplate", typeof(DataTemplate), typeof(DataTemplateView), null, BindingMode.Default, null, OnContentTemplatePropertyChanged);

        static void OnContentTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != null)
            {
                DataTemplate dataTemplate = newValue as DataTemplate;
                (bindable as DataTemplateView).Content = dataTemplate.CreateContent() as View;
            }
        }
    }
}