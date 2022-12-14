using System;
using HappinessIndex.Resx;
using Xamarin.Forms;

namespace HappinessIndex.Controls
{
    public class SliderExt : Slider
    {
        public bool ShowLabel { get; set; }

        public bool IsNegative { get; set; }

        private string tooltipDisplay;

        public string TooltipDisplay
        {
            get => tooltipDisplay; set
            {
                if (tooltipDisplay == value) return;
                tooltipDisplay = value;
                OnPropertyChanged();
            }
        }

        private Color tooltipColor = Color.Red;

        public Color TooltipColor
        {
            get => tooltipColor; set
            {
                if (tooltipColor == value) return;
                tooltipColor = value;
                OnPropertyChanged();
            }
        }

        public SliderExt()
        {
            ValueChanged += SliderExt_ValueChanged;
        }

        private void SliderExt_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newValue = Math.Round(e.NewValue);

            if (IsNegative)
            {
                if (newValue == 0)
                {
                    TooltipDisplay = AppResources.None;
                    App.Current.MainPage.Resources.TryGetValue("BackgroundDarkColor", out object themeColor);
                    if(themeColor != null)
                    {
                        TooltipColor = (Color)themeColor;
                    } 
                }
                else if (newValue < 4)
                {
                    TooltipDisplay = AppResources.Low;
                    TooltipColor = Color.Red;
                }
                else if (newValue < 7)
                {
                    TooltipDisplay = AppResources.Medium;
                    TooltipColor = Color.Red;
                }
                else if (newValue < 9)
                {
                    TooltipDisplay = AppResources.High;
                    TooltipColor = Color.Red;
                }
                else
                {
                    TooltipDisplay = AppResources.Extreme;
                    TooltipColor = Color.Red;
                }
            }
            else
            {
                if (newValue > 8)
                {
                    TooltipDisplay = AppResources.Excellent;
                    App.Current.Resources.TryGetValue("BackgroundDarkColor", out object themeColor);
                    if (themeColor != null)
                    {
                        TooltipColor = (Color)themeColor;
                    }
                }
                else if (newValue > 5)
                {
                    TooltipDisplay = AppResources.Good;
                    App.Current.Resources.TryGetValue("BackgroundDarkColor", out object themeColor);
                    if (themeColor != null)
                    {
                        TooltipColor = (Color)themeColor;
                    }
                }
                else if (newValue > 3)
                {
                    TooltipDisplay = AppResources.Fair;
                    TooltipColor = Color.FromHex("#e07919");
                }
                else if (newValue > 0)
                {
                    TooltipDisplay = AppResources.CouldBeBetter;
                    TooltipColor = Color.FromHex("#e07919");
                }
                else
                {
                    TooltipDisplay = AppResources.Terrible;
                    TooltipColor = Color.Red;
                }
            }
        }
    }
}