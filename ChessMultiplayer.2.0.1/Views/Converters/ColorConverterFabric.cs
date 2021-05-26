using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace ChessMultiplayer.Views.Converters
{
    public static class ColorConverterFabric
    {
        public enum ColorConverter { Capcake, CapcakePro, Mermaid, MermaidPro };

        public static IValueConverter GetColorConverter(ColorConverter type)
        {
            switch (type)
            {
                case ColorConverter.Capcake: return new CapcakeColorConverter();
                case ColorConverter.CapcakePro: return new CapcakeColorConverterPro();
                case ColorConverter.Mermaid: return new MermaidColorConverter();
                case ColorConverter.MermaidPro: return new MermaidColorConverterPro();
                default: return null;
            }
        }
    }
}
