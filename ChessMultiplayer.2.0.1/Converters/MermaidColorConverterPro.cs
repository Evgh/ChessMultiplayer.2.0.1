using System;
using System.Globalization;
using Xamarin.Forms;
using ChessMultiplayer.ViewModels;

namespace ChessMultiplayer.Converters
{
    public class MermaidColorConverterPro : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PositionVM.CheckColor)value;

            switch (color.State)
            {
                case PositionVM.CheckState.Selected: return Color.Gold;
                case PositionVM.CheckState.Check: return Color.Coral;
            }

            if (color.Type == PositionVM.CheckType.White)
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.DarkSeaGreen;
                    case PositionVM.CheckState.CanMove: return Color.DarkSeaGreen;
                    case PositionVM.CheckState.CanBeat: return Color.DarkSeaGreen;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Navy;
                    case PositionVM.CheckState.CanMove: return Color.Navy;
                    case PositionVM.CheckState.CanBeat: return Color.Navy;
                    case PositionVM.CheckState.Evolutionate: return Color.Navy;
                    case PositionVM.CheckState.Сastling: return Color.Navy;
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
