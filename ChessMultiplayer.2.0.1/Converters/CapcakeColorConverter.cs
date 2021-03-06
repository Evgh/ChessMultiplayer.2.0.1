using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using ChessMultiplayer.ViewModels;

namespace ChessMultiplayer.Converters
{
    public class CapcakeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PositionVM.CheckColor)value;

            switch (color.State)
            {
                case PositionVM.CheckState.Selected: return Color.LimeGreen;
                case PositionVM.CheckState.Check: return Color.Coral;
            }

            if (color.Type == PositionVM.CheckType.White)
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.PeachPuff;
                    case PositionVM.CheckState.CanMove: return Color.White;
                    case PositionVM.CheckState.CanBeat: return Color.Tomato;

                    case PositionVM.CheckState.Evolutionate: return Color.LightGoldenrodYellow;
                    case PositionVM.CheckState.Сastling: return Color.LightGoldenrodYellow;
                }
            }
            else
            {
                switch (color.State)
                {
                    case PositionVM.CheckState.Simple: return Color.Purple;
                    case PositionVM.CheckState.CanMove: return Color.MediumOrchid;
                    case PositionVM.CheckState.CanBeat: return Color.DarkRed;

                    case PositionVM.CheckState.Evolutionate: return Color.MediumPurple;
                    case PositionVM.CheckState.Сastling: return Color.MediumPurple;
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
