using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WPFVisifireChartsTest
{
    public class WPFControlTest
    {
        protected const int NumberOfInstancesForStressScenarios = 500;

        //public static bool AreBrushesEqual(Brush first, Brush second)
        //{
        //    // If the default comparison is true, that's good enough.
        //    if (object.Equals(first, second))
        //    {
        //        return true;
        //    }

        //    // Do a field by field comparison if they're not the same reference
        //    // 
        //    SolidColorBrush firstSolidColorBrush = first as SolidColorBrush;
        //    if (firstSolidColorBrush != null)
        //    {
        //        SolidColorBrush secondSolidColorBrush = second as SolidColorBrush;
        //        if (secondSolidColorBrush != null)
        //        {
        //            return object.Equals(firstSolidColorBrush.Color, secondSolidColorBrush.Color);
        //        }
        //    }

        //    return false;
        //}

        //public static void AssertBrushesAreEqual(Brush expected, Brush actual)
        //{
        //    if (!AreBrushesEqual(expected, actual))
        //    {
        //        throw new AssertFailedException(string.Format(CultureInfo.InvariantCulture,
        //           "Brushes are not equal.  Expected:{0}.  Actual:{1}.",
        //           expected,
        //           actual));
        //    }
        //}
    }
}
