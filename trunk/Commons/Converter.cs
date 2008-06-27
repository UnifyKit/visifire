/*
    Copyright (C) 2008 Webyog Softworks Private Limited

    This file is a part of Visifire Charts.
 
    Visifire is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Visifire Charts.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL is not suitable for your products or company, Webyog provides Visifire 
    under a flexible commercial license designed to meet your specific usage and 
    distribution requirements. If you have already obtained a commercial license 
    from Webyog, you can use this file under those license terms.
 
*/


using System;
using System.Windows;
using System.Windows.Media;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace Visifire.Commons
{
    public static class Converter
    {
        #region Static Methods

        public static DoubleCollection ArrayToCollection(Double[] arr)
        {
            DoubleCollection doubleCollection = new DoubleCollection();
            foreach (Double dbl in arr)
            {
                doubleCollection.Add(dbl);
            }

            return doubleCollection;
        }

        public static PointCollection ArrayToCollection(Point[] arr)
        {
            PointCollection pointCollection = new PointCollection();
            foreach (Point point in arr)
            {
                pointCollection.Add(point);
            }

            return pointCollection;
        }

        public static FontWeight StringToFontWeight(String fontWeightString)
        {

            PropertyInfo fontProperty = typeof(FontWeights).GetProperty(fontWeightString);
            FontWeightMethod fontWeightMethod = (FontWeightMethod)Delegate.CreateDelegate(typeof(FontWeightMethod), fontProperty.GetGetMethod());
            return fontWeightMethod();

        }

        public static FontStyle StringToFontStyle(String fontStyleString)
        {
            PropertyInfo fontProperty = typeof(FontStyles).GetProperty(fontStyleString);
            FontStyleMethod fontStyleMethod = (FontStyleMethod)Delegate.CreateDelegate(typeof(FontStyleMethod), fontProperty.GetGetMethod());
            return fontStyleMethod();
        }

        #endregion Static Methods

        #region Delegate

        private delegate FontWeight FontWeightMethod();

        private delegate FontStyle FontStyleMethod();

        #endregion Delegate

    }

    public sealed partial class DoubleConverter : TypeConverter
    {
        public override object ConvertFromString(string text)
        {
            return Double.Parse(text, CultureInfo.InvariantCulture);
        }

        public override object ConvertFrom(object value)
        {
            return Double.Parse(value.ToString(), CultureInfo.InvariantCulture);
        }

    }
}
