using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CVIssueApp
{
    public class StringToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object displayValue;

            displayValue = null;
            if (value != null)
            {
                if (DateTime.TryParse(value.ToString(), out DateTime dateVal))
                {
                    displayValue = dateVal;
                }
            }
           
            return displayValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

