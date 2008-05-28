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


namespace Visifire.Commons
{
    public class ArcSegmentParams : PathGeometryParams
    {
        #region Public Methods
        public ArcSegmentParams(Size size, Double rotation, Boolean isLargeArc, SweepDirection sweepDirection, Point endPoint)
            : base(endPoint)
        {
            Size = size;
            RotationAngle = rotation;
            IsLargeArc = isLargeArc;
            SweepDirection = sweepDirection;
        }
        #endregion Public Methods

        #region Public Properties

        public Size Size
        {
            get;
            set;
        }

        public Double RotationAngle
        {
            get;
            set;
        }

        public Boolean IsLargeArc
        {
            get;
            set;
        }

        public SweepDirection SweepDirection
        {
            get;
            set;
        }

        #endregion Public 

        
    }
}
