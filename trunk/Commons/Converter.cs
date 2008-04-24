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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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

        public static FontWeight StringToFontWeight(String fws)
        {
            switch (fws.ToLower())
            {
                case "black":
                    return FontWeights.Black;
                case "bold":
                    return FontWeights.Bold;
                case "extrablack":
                    return FontWeights.ExtraBlack;
                case "extrabold":
                    return FontWeights.ExtraBold;
                case "extralight":
                    return FontWeights.ExtraLight;
                case "light":
                    return FontWeights.Light;
                case "medium":
                    return FontWeights.Medium;
                case "normal":
                    return FontWeights.Normal;
                case "semibold":
                    return FontWeights.SemiBold;
                case "thin":
                    return FontWeights.Thin;
                default:
                    return FontWeights.Normal;
            }
        }

        public static FontStyle StringToFontStyle(String fss)
        {
            switch (fss.ToLower())
            {
                case "normal":
                    return FontStyles.Normal;
                case "italic":
                    return FontStyles.Italic;
                default:
                    return FontStyles.Normal;
            }
        }
        #endregion Static Methods
    }
}
