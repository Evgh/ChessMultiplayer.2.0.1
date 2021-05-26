using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using Xamarin.Forms;
using ChessMultiplayer.Models;

namespace ChessMultiplayer.Converters
{
    class MoveParameterToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((MoveParameters)value).ActionNum;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MoveParameters() { ActionNum = (int)value };
        }
    }
}
