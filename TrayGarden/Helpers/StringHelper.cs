using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Helpers
{
    internal static class StringHelper
    {
        public static bool NotNullNotEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}