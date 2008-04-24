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
    /// AxisOrientation is classified into 4 general categories.
    /// Orientation / Presence of AxisX and AxisY depends on this value.
    /// Axes will not be present if AxisOrientation is Pie
    /// </summary>
    internal enum AxisOrientation { Undefined = 0, Bar = 1, Column = 2, Pie = 3 };

    /// <summary>
    /// Contains information about how the Chart is to be plotted. Like AxisOrientation, etc.
    /// </summary>
    internal class PlotDetails
    {
        #region Public Methods
        public PlotDetails()
        {
            Plots = new System.Collections.Generic.List<Plot>();
            _axisLabels = new System.Collections.Generic.Dictionary<double, string>();

            SetDefaults();
        }

        #endregion Public Methods

        #region Private Methods
        
        private void SetDefaults()
        {
            // AxisOrientation should be Undefined by default. Else it'll not run properly.
            AxisOrientation = AxisOrientation.Undefined;

            _maxAxisXValue = Double.NaN;
            _minAxisXValue = Double.NaN;
            _maxAxisYValue = Double.NaN;
            _minAxisYValue = Double.NaN;

            MaxDataPoints = 0;

            TotalNoOfSeries = 0;

            
        }

        private void FindMaxMin()
        {
            _maxAxisXValue = Double.NegativeInfinity;
            _minAxisXValue = Double.PositiveInfinity;
            _maxAxisYValue = Double.NegativeInfinity;
            _minAxisYValue = Double.PositiveInfinity;

            foreach (Plot plot in Plots)
            {
                if (_maxAxisXValue < plot.MaxAxisXValue)
                    _maxAxisXValue = plot.MaxAxisXValue;

                if (_minAxisXValue > plot.MinAxisXValue)
                    _minAxisXValue = plot.MinAxisXValue;

                if (_maxAxisYValue < plot.MaxAxisYValue)
                    _maxAxisYValue = plot.MaxAxisYValue;

                if (_minAxisYValue > plot.MinAxisYValue)
                    _minAxisYValue = plot.MinAxisYValue;
            }
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

        internal Double MaxAxisXValue
        {
            get
            {
                if (Double.IsNaN(_maxAxisXValue))
                    FindMaxMin();

                return _maxAxisXValue;
            }

            set
            {
                _maxAxisXValue = value;
            }
        }

        internal Double MaxAxisYValue
        {
            get
            {
                if (Double.IsNaN(_maxAxisYValue))
                    FindMaxMin();

                return _maxAxisYValue;
            }

            set
            {
                _maxAxisYValue = value;
            }
        }

        internal Double MinAxisXValue
        {
            get
            {
                if (Double.IsNaN(_minAxisXValue))
                    FindMaxMin();

                return _minAxisXValue;
            }

            set
            {
                _minAxisXValue = value;
            }
        }

        internal Double MinAxisYValue
        {
            get
            {
                if (Double.IsNaN(_minAxisYValue))
                    FindMaxMin();

                return _minAxisYValue;
            }

            set
            {
                _minAxisYValue = value;
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
        public System.Collections.Generic.List<Plot> Plots
        {
            get;
            set;
        }

        public System.Collections.Generic.Dictionary<Double,String> AxisLabels
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

        public Boolean AllAxisLabels
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
        private Double _maxAxisYValue;
        private Double _minAxisYValue;
        private Double _maxAxisXValue;
        private Double _minAxisXValue;

        #endregion Data
    }
}
