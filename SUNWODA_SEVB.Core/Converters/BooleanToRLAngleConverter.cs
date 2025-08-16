using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SUNWODA_SEVB.Core.Converters
{
    public class BooleanToRLAngleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "AngleLeftSolid" : "AngleRightSolid";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() is string stringValue)
            {
                if (stringValue == "AngleLeftSolid")
                {
                    return true;
                }
                else if(stringValue == "AngleRightSolid")
                {
                    return false;
                }
            }
            return false;
        }
    }
}
