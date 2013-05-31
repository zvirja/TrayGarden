using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Helpers
{
    public static class StringHelper
    {
        public static bool NotNullNotEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string FormatWith(this string format, params object[] @params)
        {
            return string.Format(format, @params);
        }

    }
}