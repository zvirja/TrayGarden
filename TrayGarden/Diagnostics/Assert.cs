using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Helpers;

namespace TrayGarden.Diagnostics
{
    public static class Assert
    {
        public static void ArgumentNotNull(object argument, string arumentName)
        {
            if (!(argument != null))
            {
                throw new ArgumentNullException("The {0} argument is null".FormatWith(new object[] { arumentName }));
            }
        }

        public static void ArgumentNotNullOrEmpty(string argument, string arumentName)
        {
            ArgumentNotNull(argument, arumentName);
            if (argument == string.Empty)
            {
                throw new ArgumentNullException("The {0} argument is an empty string".FormatWith(new object[] { arumentName }));
            }
        }

        public static void IsNotNull(object @object, string message, bool isError = true)
        {
            if (@object == null)
            {
                throw (isError ? new InvalidOperationException(message) : new Exception(message));
            }
        }

        public static void IsNotNullOrEmpty(string condition, string message, bool isError = true)
        {
            IsTrue(!string.IsNullOrEmpty(condition), message, isError);
        }

        public static void IsTrue(bool condition, string message, bool isError = true)
        {
            if (!condition)
            {
                throw (isError ? new InvalidOperationException(message) : new Exception(message));
            }
        }
    }

}
