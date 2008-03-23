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
    /// <summary>
    /// This class stores all related DataSeries and how they should be plotted.
    /// For example 3 series may be combined to give one MultiSeriesBar
    /// </summary>
    internal class Plot
    {
        #region Public Methods
        public Plot()
        {
            _dataSeries = new System.Collections.Generic.List<DataSeries>();
            SetDefaults();
        }

        // Type of the chart
        public ChartTypes ChartType
        {
            get
            {
                return _chartType;
            }
            set
            {
                _chartType = value;
            }
        }
        // Collection of related DataSeries for the ChartType
        public System.Collections.Generic.List<DataSeries> DataSeries
        {
            get
            {
                return _dataSeries;
            }
            set
            {
                _dataSeries = value;
            }
        }

        public System.Collections.Generic.Dictionary<Double, Point> TopBottom
        {
            get;
            set;
        }

        public System.Collections.Generic.Dictionary<Double, Double> YValueSum
        {
            get;
            set;
        }

        #endregion Public Methods

        #region Private Methods

        private void SetDefaults()
        {
            _minDifference = Double.NaN;

            _maxAxisXValue = Double.NaN;
            _minAxisXValue = Double.NaN;
            _maxAxisYValue = Double.NaN;
            _minAxisYValue = Double.NaN;
        }

        private void FindMaxMin()
        {
            _maxAxisXValue = Double.NegativeInfinity;
            _minAxisXValue = Double.PositiveInfinity;
            _maxAxisYValue = Double.NegativeInfinity;
            _minAxisYValue = Double.PositiveInfinity;

            if (ChartType == ChartTypes.Area || ChartType == ChartTypes.Bar ||
                ChartType == ChartTypes.Column || ChartType == ChartTypes.Line ||
                ChartType == ChartTypes.Point || ChartType == ChartTypes.Pie || ChartType == ChartTypes.Doughnut)
            {
                foreach (DataSeries child in _dataSeries)
                {
                    foreach (DataPoint item in child.DataPoints)
                    {
                        if (!Double.IsNaN(item.XValue))
                        {
                            if (_maxAxisXValue < item.XValue)
                                _maxAxisXValue = item.XValue;

                            if (_minAxisXValue > item.XValue)
                                _minAxisXValue = item.XValue;
                        }

                        if (_maxAxisYValue < item.YValue)
                            _maxAxisYValue = item.YValue;

                        if (_minAxisYValue > item.YValue)
                            _minAxisYValue = item.YValue;
                    }

                }
            }

            else if (ChartType == ChartTypes.StackedColumn || ChartType == ChartTypes.StackedArea
                     || ChartType == ChartTypes.StackedBar)
            {
                System.Collections.Generic.Dictionary<Double, Point> minMax = new System.Collections.Generic.Dictionary<double, Point>();


                // Point.X corresponds to minimum;
                // Point.Y corresponds to maximum;

                foreach (DataSeries dataseries in _dataSeries)
                {
                    foreach (DataPoint datapoint in dataseries.DataPoints)
                    {
                        if (!minMax.ContainsKey(datapoint.XValue))
                        {
                            minMax[datapoint.XValue] = new Point(0.00000001, 0);

                        }

                        Point point = minMax[datapoint.XValue];

                        if (datapoint.YValue > 0)
                            point.Y += datapoint.YValue;
                        else
                            point.X += datapoint.YValue;

                        minMax[datapoint.XValue] = point;

                        if (_maxAxisYValue < point.Y)
                            _maxAxisYValue = point.Y;

                        if (_minAxisYValue > point.X)
                            _minAxisYValue = point.X;

                        if (_maxAxisXValue < datapoint.XValue)
                            _maxAxisXValue = datapoint.XValue;

                        if (_minAxisXValue > datapoint.XValue)
                            _minAxisXValue = datapoint.XValue;
                    }
                }

            }
            else if (ChartType == ChartTypes.StackedColumn100 || ChartType == ChartTypes.StackedArea100
                    || ChartType == ChartTypes.StackedBar100)
            {
                YValueSum = new System.Collections.Generic.Dictionary<double, double>();

                _maxAxisYValue = 100;
                _minAxisYValue = 0.000001;


                foreach (DataSeries dataseries in _dataSeries)
                {
                    foreach (DataPoint datapoint in dataseries.DataPoints)
                    {


                        if (!YValueSum.ContainsKey(datapoint.XValue))
                        {
                            YValueSum[datapoint.XValue] = Math.Abs(datapoint.YValue);
                        }
                        else
                        {
                            YValueSum[datapoint.XValue] = YValueSum[datapoint.XValue] + Math.Abs(datapoint.YValue);
                        }


                        if (datapoint.YValue >= 0)
                            _maxAxisYValue = 99.99999;
                        else
                            _minAxisYValue = -99.9999;

                        if (_maxAxisXValue < datapoint.XValue)
                            _maxAxisXValue = datapoint.XValue;

                        if (_minAxisXValue > datapoint.XValue)
                            _minAxisXValue = datapoint.XValue;
                    }
                }
            }
        }

        private void CalcMinDifference()
        {
            int totalDataPoints = 0;

            foreach (DataSeries ds in _dataSeries)
            {
                foreach (DataPoint dp1 in ds.DataPoints)
                {
                    totalDataPoints++;

                    foreach (DataPoint dp2 in ds.DataPoints)
                    {
                        if (dp2 == dp1)
                            continue;

                        if (!Double.IsNaN(_minDifference))
                        {
                            if (Math.Abs(dp2.XValue - dp1.XValue) < _minDifference)
                                if (Math.Abs(dp2.XValue - dp1.XValue) != 0)
                                    _minDifference = Math.Abs(dp2.XValue - dp1.XValue);
                        }
                        else
                            if (Math.Abs(dp2.XValue - dp1.XValue) != 0)
                                _minDifference = Math.Abs(dp2.XValue - dp1.XValue);
                    }
                }
            }
            if (totalDataPoints == _dataSeries.Count)
            {
                _minDifference = 0;
            }
        }   

        #endregion Private Methods

        #region Internal Properties
        internal Double MaxAxisXValue
        {
            get
            {
                if(Double.IsNaN(_maxAxisXValue))
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

        internal Double MinDifference
        {
            get
            {
                if (Double.IsNaN(_minDifference))
                    CalcMinDifference();

                return _minDifference;
            }
        }

        #endregion Internal Properties

        #region Data
        private Double _minDifference;

        

        private ChartTypes _chartType;
        private System.Collections.Generic.List<DataSeries> _dataSeries;

        
    

        private Double _maxAxisYValue;
        private Double _minAxisYValue;
        private Double _maxAxisXValue;
        private Double _minAxisXValue;
        #endregion Data
    }
}