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

namespace Visifire.Charts
{
    internal class ElementPositionData
    {
        #region Public Methods

        public ElementPositionData()
        {
        }

        public ElementPositionData(FrameworkElement element, Double angle1, Double angle2)
        {
            Element = element;
            StartAngle = angle1;
            StopAngle = angle2;
        }

        public ElementPositionData(ElementPositionData m)
        {
            Element = m.Element;
            StartAngle = m.StartAngle;
            StopAngle = m.StopAngle;
        }

        #endregion Public Methods

        #region Static Methods

        public static Int32 CompareAngle(ElementPositionData a, ElementPositionData b)
        {
            Double angle1 = (a.StartAngle + a.StopAngle) / 2;
            Double angle2 = (b.StartAngle + b.StopAngle) / 2;
            return angle1.CompareTo(angle2);
        }

        #endregion Static Methods

        #region Public Properties

        public FrameworkElement Element
        {
            get;
            set;
        }

        public Double StartAngle
        {
            get;
            set;
        }

        public Double StopAngle
        {
            get;
            set;
        }

        #endregion
    }
}
