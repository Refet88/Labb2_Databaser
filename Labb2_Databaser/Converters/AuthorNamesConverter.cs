using Labb2_Databaser.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Labb2_Databaser.Converters
{
    public class AuthorNamesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection<FörfattareBöcker> författareBöcker)
            {
                return string.Join(", ", författareBöcker.Select(a => $"{a.Författare.Förnamn} {a.Författare.Efternamn}"));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
