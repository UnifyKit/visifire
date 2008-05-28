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
using System.Windows.Documents;

namespace Visifire.Charts
{
    public class ChartConstants
    {
        public struct AxisX
        {
            public struct NoData
            {
                public static Double Interval = 1;
                public static Double MaxValue = 9;
                public static Double MinValue = 1;
            }
        }

        public struct AxisY
        {
            public struct NoData
            {
                public static Double Interval = 10;
                public static Double MaxValue = 90;
                public static Double MinValue = 10;
            }

        }

        public struct Axes
        {
            public struct Default
            {
                public static Double LineThickness = 0.25;
                public static String ValueFormatString = "###,##0.##";
                public static Int32 ZIndex = 2;
            }
        }

        public struct Plank
        {
            public static Double HorizontalDepthFactor = 0.04;
            public static Double HorizontalThicknessFactor = 0.03;

            public static Double VerticalDepthFactor = 0.015;
            public static Double VerticalThicknessFactor = 0.025;
        }

        public struct Interactivity
        {
            public static Double BeginTime = 0;
            public static Double Duration = 0.15;

        }

    }
}
