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
    public class AxisY : Axes
    {
        #region Public Methods

        public AxisY()
        {
        }

        public override void Init()
        {
            ValidateParent();

            if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
                AxisOrientation = AxisOrientation.Column;
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
                AxisOrientation = AxisOrientation.Bar;

            MaxDataValue = _parent.PlotDetails.MaxAxisYValue;
            MinDataValue = _parent.PlotDetails.MinAxisYValue;

            base.Init();
        }
        
        #endregion Public Methods



        #region Private Methods
                
        private void ValidateParent()
        {
            if (this.Parent is Chart)
                _parent = this.Parent as Chart;
            else
                throw new Exception(this + "Parent should be a Chart");
        }

        protected override void SetDefaults()
        {
            base.SetDefaults();
        }

        #endregion Private Methods

        

    }
}
