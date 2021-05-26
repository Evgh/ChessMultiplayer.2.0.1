using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using ChessMultiplayer.ViewModels;

namespace ChessMultiplayer.Views.Converters
{
    public class CapcakeColorConverterPro : IValueConverter
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
                    case PositionVM.CheckState.CanMove: return Color.PeachPuff;
                    case PositionVM.CheckState.CanBeat: return Color.PeachPuff;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Purple;
                    case PositionVM.CheckState.Selected: return Color.LimeGreen;
                    case PositionVM.CheckState.CanMove: return Color.Purple;
                    case PositionVM.CheckState.CanBeat: return Color.Purple;

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
