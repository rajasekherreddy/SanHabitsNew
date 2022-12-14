using System;
using System.Collections.Generic;
using HappinessIndex.Resx;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace HappinessIndex.Helpers
{
    public static class ColorHelper
    {
        static ChartColorCollection defaultColors;

        public static ChartColorCollection DefaultColors
        {
            get
            {
                if (defaultColors == null)
                {
                    defaultColors = new ChartColorCollection() {
                    Color.FromHex("#68B9C0"),
                    Color.FromHex("#90D585"),
                    Color.FromHex("#F3C151"),
                    Color.FromHex("#F37F64"),
                    Color.FromHex("#8F97A4"),
                    Color.FromHex("#DAC096"),
                    Color.FromHex("#76846E"),
                    Color.FromHex("#90D585"),
                    Color.FromHex("#68B9C0")
                };
                }
                return defaultColors;
            }
        }

        public static Color GetColor(int ID)
        {
            if (ID % 12 == 1)
            {
                return Color.FromHex("#266489");
            }
            if (ID % 12 == 2)
            {
                return Color.FromHex("#68B9C0");
            }
            if (ID % 12 == 3)
            {
                return Color.FromHex("#90D585");
            }
            if (ID % 12 == 4)
            {
                return Color.FromHex("#F3C151");
            }
            if (ID % 12 == 5)
            {
                return Color.FromHex("#F37F64");
            }
            if (ID % 12 == 6)
            {
                return Color.FromHex("#424856");
            }
            if (ID % 12 == 7)
            {
                return Color.FromHex("#8F97A4");
            }
            if (ID % 12 == 8)
            {
                return Color.FromHex("#DAC096");
            }
            if (ID % 12 == 9)
            {
                return Color.FromHex("#76846E");
            }
            if (ID % 12 == 10)
            {
                return Color.FromHex("#424856");
            }
            if (ID % 12 == 11)
            {
                return Color.FromHex("#90D585");
            }
            if (ID % 12 == 0)
            {
                return Color.FromHex("#68B9C0");
            }
            return Color.FromHex("#424856");
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (1 - red) * correctionFactor + red;
                green = (1 - green) * correctionFactor + green;
                blue = (1 - blue) * correctionFactor + blue;
            }

            return Color.FromRgb(red, green, blue);
        }

        public static Color GetColorOf(double value, double minValue, double maxValue)
        {
            value = value / (maxValue - minValue);

            var red = 0.56;
            var green = 0.76;
            var blue = 0.28;

            return new Color(red * (2f * value), green - green * value, blue);
        }
    }
}