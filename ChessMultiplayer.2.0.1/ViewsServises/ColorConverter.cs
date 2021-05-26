using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using ChessMultiplayer.ViewModels;

namespace ChessMultiplayer.Views
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PositionVM.CheckColor)value;

            if (color.Type == PositionVM.CheckType.White)
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.PeachPuff;
                    case PositionVM.CheckState.Selected: return Color.LimeGreen; 
                    case PositionVM.CheckState.CanMove: return Color.White;
                    case PositionVM.CheckState.CanBeat: return Color.Tomato;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Purple;
                    case PositionVM.CheckState.Selected: return Color.LimeGreen;
                    case PositionVM.CheckState.CanMove: return Color.MediumOrchid;
                    case PositionVM.CheckState.CanBeat: return Color.DarkRed;

                }
            }
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now.ToString("dd.MM.yyyy");
        }
    }


    public class ColorConverterD : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PositionVM.CheckColor)value;

            if (color.Type == PositionVM.CheckType.White)
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.DarkSeaGreen;
                    case PositionVM.CheckState.Selected: return Color.Gold;
                    case PositionVM.CheckState.CanMove: return Color.PaleGreen;
                    case PositionVM.CheckState.CanBeat: return Color.Tomato;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Navy;
                    case PositionVM.CheckState.Selected: return Color.Gold;
                    case PositionVM.CheckState.CanMove: return Color.RoyalBlue;
                    case PositionVM.CheckState.CanBeat: return Color.DarkRed;

                }
            }
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now.ToString("dd.MM.yyyy");
        }
    }

    public class ProfessionalColorConverterD : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PositionVM.CheckColor)value;

            if (color.Type == PositionVM.CheckType.White)
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.DarkSeaGreen;
                    case PositionVM.CheckState.Selected: return Color.Gold;
                    case PositionVM.CheckState.CanMove: return Color.DarkSeaGreen;
                    case PositionVM.CheckState.CanBeat: return Color.DarkSeaGreen;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Navy;
                    case PositionVM.CheckState.Selected: return Color.Gold;
                    case PositionVM.CheckState.CanMove: return Color.Navy;
                    case PositionVM.CheckState.CanBeat: return Color.Navy;

                }
            }
            return Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}
