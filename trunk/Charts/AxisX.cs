/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
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
    public enum AxisTypes{Value,Category,Auto};

    public class AxisX : Axes
    {
        #region Public Methods

        public AxisX()
        {
            
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public override void Init()
        {
            ValidateParent();


            if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Bar)
            {
                AxisOrientation = AxisOrientation.Bar;
                MaxDataValue = _parent.PlotDetails.MaxAxisXValue;
                MinDataValue = _parent.PlotDetails.MinAxisXValue;

            }
            else if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
            {
                AxisOrientation = AxisOrientation.Column;
                MaxDataValue = _parent.PlotDetails.MaxAxisXValue;
                MinDataValue = _parent.PlotDetails.MinAxisXValue;
            }

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