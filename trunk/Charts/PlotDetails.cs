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

    /// <summary>
    /// Contains information about how the Chart is to be plotted. Like AxisOrientation, etc.
    /// </summary>
    internal class PlotDetails
    {
        #region Public Methods
        public PlotDetails()
        {
            Plots = new System.Collections.Generic.List<Plot>();
            _axisLabels = new System.Collections.Generic.Dictionary<Double, String>();

            SetDefaults();
        }

        #endregion Public Methods

        #region Private Methods
        
        private void SetDefaults()
        {
            // AxisOrientation should be Undefined by default. Else it'll not run properly.
            AxisOrientation = AxisOrientation.Undefined;

            _maxAxisXDataValue = Double.NaN;
            _minAxisXDataValue = Double.NaN;
            _maxAxisYDataValue = Double.NaN;
            _minAxisYDataValue = Double.NaN;

            MaxDataPoints = 0;

            TotalNoOfSeries = 0;

            
        }

        private void FindMaxMinAxisXValues()
        {
            _maxAxisXDataValue = Double.NegativeInfinity;
            _minAxisXDataValue = Double.PositiveInfinity;

            foreach (Plot plot in Plots)
            {
                _maxAxisXDataValue = Math.Max(_maxAxisXDataValue, plot.MaxAxisXValue);

                _minAxisXDataValue = Math.Min(_minAxisXDataValue, plot.MinAxisXValue);
            }

        }

        private void FindMaxMinAxisYValues(AxisY axisY)
        {
            _maxAxisYDataValue = Double.NegativeInfinity;
            _minAxisYDataValue = Double.PositiveInfinity;

            foreach (Plot plot in Plots)
            {
                if (plot.AxisY.GetHashCode().Equals(axisY.GetHashCode()))
                {
                    _maxAxisYDataValue = Math.Max(_maxAxisYDataValue, plot.MaxAxisYValue);

                    _minAxisYDataValue = Math.Min(_minAxisYDataValue, plot.MinAxisYValue);
                }
            }
        }
        internal void FindMaxMinForAxis(Axes axes)
        {
            if (axes.GetType().Name == "AxisX")
                FindMaxMinAxisXValues();
            else if (axes.GetType().Name == "AxisY")
                FindMaxMinAxisYValues(axes as AxisY);
        }
        
        #endregion Private Methods
        
        #region Internal Methods
        
        internal AxisOrientation AxisOrientation
        {
            get;
            set;
        }

        internal Double TotalNoOfSeries
        {
            get;
            set;
        }

        internal Double MaxAxisXDataValue
        {
            get
            {
                return _maxAxisXDataValue;
            }

            set
            {
                _maxAxisXDataValue = value;
            }
        }

        internal Double MaxAxisYDataValue
        {
            get
            {
                return _maxAxisYDataValue;
            }

            set
            {
                _maxAxisYDataValue = value;
            }
        }

        internal Double MinAxisXDataValue
        {
            get
            {
                return _minAxisXDataValue;
            }

            set
            {
                _minAxisXDataValue = value;
            }
        }

        internal Double MinAxisYDataValue
        {
            get
            {
                return _minAxisYDataValue;
            }

            set
            {
                _minAxisYDataValue = value;
            }
        }

        internal Int32 MaxDataPoints
        {
            get;
            set;
        }

        #endregion Internal Methods
        
        #region Data
        /// <summary>
        /// Collection of Plot. 
        /// For example, Plots Collection can have a Plot with ChartTpye Bar and one Plot with ChartType Line. Then it becomes
        /// a combinatioinal chart.
        /// </summary>
        internal System.Collections.Generic.List<Plot> Plots
        {
            get;
            set;
        }

        internal System.Collections.Generic.Dictionary<Double,String> AxisLabels
        {
            get
            {
                return _axisLabels;
            }
            set
            {
                _axisLabels = value;
            }
        }

        internal Boolean AllAxisLabels
        {
            get
            {
                return (_axisLabels.Count == MaxDataPoints);
            }
            set
            {
                _allAxisLabels = value;
            }
        }

        private System.Collections.Generic.Dictionary<Double, String> _axisLabels;
        private Boolean _allAxisLabels;
        private Double _maxAxisYDataValue;
        private Double _minAxisYDataValue;
        private Double _maxAxisXDataValue;
        private Double _minAxisXDataValue;

        #endregion Data
    }
}
