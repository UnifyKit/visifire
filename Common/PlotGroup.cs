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

#if WPF

using System;
using System.Collections.Generic;
using System.Windows.Documents;
#else
using System;
using System.Collections.Generic;
#endif
using System.Linq;
using System.Windows;

namespace Visifire.Charts
{
    /// <summary>
    /// One or more than one DataSeries are grouped into a PlotGroup
    /// </summary>
    internal class PlotGroup
    {   
        #region Public Methods
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.PlotGroup class.
        /// </summary>
        /// <param name="renderAs">RenderAs</param>
        /// <param name="axisX">axisX</param>
        /// <param name="axisY">AxisY</param>
        public PlotGroup(RenderAs renderAs, Axis axisX, Axis axisY)
        {
            DataSeriesList = new List<DataSeries>();
            
            XWiseStackedDataList = new Dictionary<Double, XWiseStackedData>();

            RenderAs = renderAs;
            AxisX = axisX;
            AxisY = axisY;
        }

        public Double GetLimitingYValue()
        {
            Double limitingYValue = 0;

            if (AxisY != null)
            {
                if (AxisY.InternalAxisMinimum > 0)
                    limitingYValue = (Double)AxisY.InternalAxisMinimum;

                if (AxisY.InternalAxisMaximum < 0)
                    limitingYValue = (Double)AxisY.InternalAxisMaximum;
            }

            return limitingYValue;
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion
        
        #region Internal Properties

        /// <summary>
        /// PlotGroup is enabled if atleast one dataSeries in DataSeriesList is enabled
        /// </summary>
        internal bool IsEnabled
        {
            get
            {
                foreach (DataSeries ds in DataSeriesList)
                {
                    if ((Boolean)ds.Enabled) return true; 
                }

                return false;
            }
        }
        
        /// <summary>
        /// Reference to the X-Axis for this group
        /// </summary>
        internal Axis AxisX
        {
            get;
            private set;
        }

        /// <summary>
        /// Reference to the Y-Axis for this group
        /// </summary>
        internal Axis AxisY
        {
            get;
            private set;
        }

        /// <summary>
        /// Chart type that will be rendered by this PlotGroup
        /// </summary>
        internal RenderAs RenderAs
        {
            get;
            private set;
        }

        /// <summary>
        /// List of DataSeries that belong to this group
        /// </summary>
        internal List<DataSeries> DataSeriesList
        {
            get;
            set;
        }


        /// <summary>
        /// Data stored in a sorted order for displaying stacked chart types
        /// </summary>
        internal Dictionary<Double, XWiseStackedData> XWiseStackedDataList
        {
            get;
#if DEBUG
            set;
#else
            private set;
#endif
        }

        /// <summary>
        /// Stores the maximum InternalXValue for the group
        /// </summary>
        internal Double MaximumX
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the maximum YValue for the gourp
        /// </summary>
        internal Double MaximumY
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the maximum ZValue for the group
        /// </summary>
        internal Double MaximumZ
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the minimum InternalXValue for the group
        /// </summary>
        internal Double MinimumX
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the minimum YValue for the group
        /// </summary>
        internal Double MinimumY
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the minimum ZValue for the group
        /// </summary>
        internal Double MinimumZ
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the calculated minimum difference value for the XValues in this group
        /// </summary>
        internal Double MinDifferenceX
        {
            get;
#if DEBUG
            set;
#else
            private set;
#endif
        }


        
        internal Double DrawingIndex;

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods
        /// <summary>
        /// Selectively adds the DataPoint to the Positive and Negative list of datapoints under a PlotGroup
        /// </summary>
        /// <param name="xWiseData">Reference to the XWise data node</param>
        /// <param name="dataPoint">DataPoint to be added to the node</param>
#if DEBUG
        internal 
#else
        private
#endif
        void AddXWiseStackedDataEntry(ref XWiseStackedData xWiseData, DataPoint dataPoint)
        {

            if (dataPoint.YValue >= 0)
            {
                xWiseData.Positive.Add(dataPoint);
            }
            else
            {
                xWiseData.Negative.Add(dataPoint);
            }
        }

        /// <summary>
        /// Gets minimum difference from Double value array.
        /// </summary>
        /// <param name="values">Array of values</param>
        /// <returns>Double</returns>
#if DEBUG
        internal Double GetMinDifference(Double[] values)
#else
        private Double GetMinDifference(Double[] values)
#endif
        {
            // initialize minimum difference value with a large value
            Double minDiff = Double.MaxValue;

            Double[] distinctValues = values.Distinct().ToArray();
            // get unique values and then sort it
            Array.Sort(distinctValues);

            if (distinctValues.Length <= 1 )
            {
                return Double.PositiveInfinity;
            }

            for (Int32 i = 0; i < distinctValues.Length - 1; i++)
            {
                // get the smallest difference between two successive elements
                minDiff = Math.Min(minDiff, Math.Abs(distinctValues[i] - distinctValues[i + 1]));
            }

            return minDiff;
        }

        #endregion

        #region Internal Methods



        private void CreateXWiseStackedDataEntry(ref List<DataPoint> dataPointsInCurrentPlotGroup, params RenderAs[] chartTypes)
        {
            XWiseStackedDataList.Clear();

            // variable to temporarily store the stacked Data content
            XWiseStackedData xWiseData;

            // Populates the Xwise sorted Stacked data list with entries from 
            // all the datapoints from all DataSeries from this group
            foreach (DataPoint dataPoint in dataPointsInCurrentPlotGroup)
            {
                // Check whether the DataPoint is under the list of RenderAs
                if (!chartTypes.Any(w => w == dataPoint.Parent.RenderAs))
                    continue;
                
                if (XWiseStackedDataList.ContainsKey(dataPoint.InternalXValue))
                {   
                    // gets the existing  node
                    xWiseData = XWiseStackedDataList[dataPoint.InternalXValue];
                }
                else
                {   
                    // Creates a new node
                    xWiseData = new XWiseStackedData();
                    XWiseStackedDataList.Add(dataPoint.InternalXValue, xWiseData);
                }

                // add the datapoint to a node
                AddXWiseStackedDataEntry(ref xWiseData, dataPoint);
            }
        } 
        
        /// <summary>
        /// This function will find the dependent variable types from the current PlotGroup.
        /// </summary>
        /// <returns></returns>
        internal List<Type> GetDependentVariableTypes()
        {
            List<Type> types = new List<Type>();
            var dataSeriesCount = (from dataSeries in DataSeriesList
                                   where dataSeries.RenderAs == RenderAs.CandleStick
                                   || dataSeries.RenderAs == RenderAs.Stock
                                   select dataSeries).Count();

            if (dataSeriesCount == DataSeriesList.Count)
                types.Add(typeof(List<Double>));
            else
            {
                types.Add(typeof(Double));
                var bubbleSeriesCount = (from dataSeries in DataSeriesList
                                         where dataSeries.RenderAs == RenderAs.Bubble
                                         select dataSeries).Count();
                if (bubbleSeriesCount > 0)
                    types.Add(typeof(Double));
            }

            return types;
        }

        public void LoadYValues(List<DataSeries> DataSeriesList)
        {
            foreach (DataSeries dataSeries in DataSeriesList)
            {
                dataSeries.PlotGroup = this;

                if (dataSeries.RenderAs == RenderAs.Stock || dataSeries.RenderAs == RenderAs.CandleStick)
                {
                    foreach (DataPoint dp in dataSeries.InternalDataPoints)
                    {
                        if (dp.YValues != null)
                        {
                            _yValues.AddRange(dp.YValues);
                        }

                    }
                }
            }
        }

        public void CalculateMinYValueWithInAXValueRange(Double minXValue, Double maxXValue, out Double minimumY)
        {
            switch (RenderAs)
            {   
                case RenderAs.StackedArea:
                case RenderAs.StackedBar:
                case RenderAs.StackedColumn:
                    {
                        CreateXWiseStackedDataEntry(ref _dataPointsInCurrentPlotGroup, RenderAs.StackedColumn, RenderAs.StackedBar, RenderAs.StackedArea);

                        Double[] xValuesInViewPort = RenderHelper.GetXValuesUnderViewPort(XWiseStackedDataList.Keys.ToList(), AxisX, AxisY, false);

                        var secectedValues = (from xWiseData in XWiseStackedDataList where xValuesInViewPort.Contains(xWiseData.Key) select xWiseData.Value);
                        var negativeYValue = from xwisedata in secectedValues select xwisedata.NegativeYValueSum;

                        minimumY = (negativeYValue.Count() > 0) ? (negativeYValue).Max() : 0;
                    }

                    break;

                case RenderAs.StackedArea100:
                case RenderAs.StackedBar100:
                case RenderAs.StackedColumn100:
                    {
                        CreateXWiseStackedDataEntry(ref _dataPointsInCurrentPlotGroup, RenderAs.StackedColumn100, RenderAs.StackedBar100, RenderAs.StackedArea100);

                        Double[] xValuesInViewPort = RenderHelper.GetXValuesUnderViewPort(XWiseStackedDataList.Keys.ToList(), AxisX, AxisY, false);

                        var secectedValues = (from xWiseData in XWiseStackedDataList where xValuesInViewPort.Contains(xWiseData.Key) select xWiseData.Value);
                        var negativeYValue = from xwisedata in secectedValues select xwisedata.NegativeYValueSum;

                        minimumY = (negativeYValue.Count() > 0) ? (negativeYValue).Min() : 0;

                        // Since for stacked chart the Minimum can't be greater than 0 or less then -100
                        // Check and set appropriate limit
                        minimumY = (minimumY >= 0) ? 0 : -100;
                    }

                    break;

                case RenderAs.CandleStick:
                case RenderAs.Stock:

                    List<DataPoint> dataPointsInViewPort = RenderHelper.GetDataPointsUnderViewPort(_dataPointsInCurrentPlotGroup, true);
                    minimumY = (from dp in dataPointsInViewPort where dp.YValues != null select dp.YValues.Min()).Min();

                    break;
                default:

                    List<DataPoint> dataPointsInViewPort1 = RenderHelper.GetDataPointsUnderViewPort(_dataPointsInCurrentPlotGroup, true);
                    var yValues = (from dp in dataPointsInViewPort1 select dp.YValue);

                    if (yValues.Count() > 0)
                        minimumY = yValues.Min();
                    else
                        minimumY = Double.NaN;

                    break;
            };
        }

        public void CalculateMaxYValueWithInAXValueRange(Double minXValue, Double maxXValue, out Double maximumY)
        {
            switch (RenderAs)
            {
                case RenderAs.StackedArea:
                case RenderAs.StackedBar:
                case RenderAs.StackedColumn:
                    {
                        CreateXWiseStackedDataEntry(ref _dataPointsInCurrentPlotGroup, RenderAs.StackedColumn, RenderAs.StackedBar, RenderAs.StackedArea);

                        Double[] xValuesInViewPort = RenderHelper.GetXValuesUnderViewPort(XWiseStackedDataList.Keys.ToList(), AxisX, AxisY, true);

                        var secectedValues = (from xWiseData in XWiseStackedDataList where xValuesInViewPort.Contains(xWiseData.Key) select xWiseData.Value);
                        var positiveYValue = from xwisedata in secectedValues select xwisedata.PositiveYValueSum;

                        maximumY = (positiveYValue.Count() > 0) ? (positiveYValue).Max() : 0;

                    }

                    break;

                case RenderAs.StackedArea100:
                case RenderAs.StackedBar100:
                case RenderAs.StackedColumn100:
                    {
                        CreateXWiseStackedDataEntry(ref _dataPointsInCurrentPlotGroup, RenderAs.StackedColumn100, RenderAs.StackedBar100, RenderAs.StackedArea100);

                        Double[] xValuesInViewPort = RenderHelper.GetXValuesUnderViewPort(XWiseStackedDataList.Keys.ToList(), AxisX, AxisY, true);

                        var secectedValues = (from xWiseData in XWiseStackedDataList where xValuesInViewPort.Contains(xWiseData.Key) select xWiseData.Value);
                        var positiveYValue = from xwisedata in secectedValues select xwisedata.PositiveYValueSum;
                       
                        maximumY = (positiveYValue.Count() > 0) ? (positiveYValue).Max() : 0;

                        // Since for stacked chart the Maximum can't be greater than 100 or less then 0
                        // Check and set appropriate limit
                        maximumY = (maximumY > 0) ? 100 : 0;
                    }

                    break;

                case RenderAs.CandleStick:
                case RenderAs.Stock:

                    List<DataPoint> dataPointsInViewPort = RenderHelper.GetDataPointsUnderViewPort(_dataPointsInCurrentPlotGroup, true);

                    maximumY = (from dp in dataPointsInViewPort where dp.YValues != null select dp.YValues.Max()).Max();
                    break;
                default:

                    List<DataPoint> dataPointsInViewPort1 = RenderHelper.GetDataPointsUnderViewPort(_dataPointsInCurrentPlotGroup, true);

                    var yValues = (from dp in dataPointsInViewPort1 select dp.YValue);

                    if (yValues.Count() > 0)
                        maximumY = yValues.Max();
                    else
                        maximumY = Double.NaN;

                    break;
            };
        }

        private static void CalculateMinMaxOfASplineCurve(DataSeries dataSeries, ref Double chartSpecificMinX, ref Double chartSpecificMaxX, ref Double chartSpecificMinY, ref Double chartSpecificMaxY)
        {
            if (dataSeries.RenderAs == RenderAs.Spline)
            {
                List<Point> knotPoints = (from dataPoint in dataSeries.InternalDataPoints where !Double.IsNaN(dataPoint.InternalXValue) && !Double.IsNaN(dataPoint.YValue) select new Point(dataPoint.InternalXValue, dataPoint.YValue)).ToList();

                if (knotPoints.Count() > 0)
                {
                    // Get Bezier Spline Control Points.
                    Point[] controlPoints1, controlPoints2;
                    Bezier.GetCurveControlPoints(2, knotPoints.ToArray(), out controlPoints1, out controlPoints2);

                    if (controlPoints1 != null && controlPoints2 != null)
                    {
                        // Calculate Max and min value among control points of Bezier segment.
                        if (controlPoints1.Count() > 0)
                        {
                            chartSpecificMinX = Math.Min(Double.IsNaN(chartSpecificMinX) ? Double.MaxValue : chartSpecificMinX, (from point in controlPoints1 select point.X).Min());
                            chartSpecificMinY = Math.Min(Double.IsNaN(chartSpecificMinY) ? Double.MaxValue : chartSpecificMinY, (from point in controlPoints1 select point.Y).Min());

                            chartSpecificMaxX = Math.Max(Double.IsNaN(chartSpecificMaxX) ? Double.MinValue : chartSpecificMaxX, (from point in controlPoints1 select point.X).Max());
                            chartSpecificMaxY = Math.Max(Double.IsNaN(chartSpecificMaxY) ? Double.MinValue : chartSpecificMaxY, (from point in controlPoints1 select point.Y).Max());
                        }

                        if (controlPoints2.Count() > 0)
                        {
                            chartSpecificMinX = Math.Min(chartSpecificMinX, (from point in controlPoints2 select point.X).Min());
                            chartSpecificMinY = Math.Min(chartSpecificMinY, (from point in controlPoints2 select point.Y).Min());

                            chartSpecificMaxX = Math.Max(chartSpecificMaxX, (from point in controlPoints2 select point.X).Max());
                            chartSpecificMaxY = Math.Max(chartSpecificMaxY, (from point in controlPoints2 select point.Y).Max());
                        }
                    }
                }
            }
            else
            {
                chartSpecificMinX = Double.NaN; // Min XValue specific to chart type. 
                chartSpecificMaxX = Double.NaN; // Min XValue specific to chart type. 
                chartSpecificMinY = Double.NaN; // Min YValue specific to chart type. 
                chartSpecificMaxY = Double.NaN; // Min YValue specific to chart type. 
            }
        }

        /// <summary>
        /// Updates all properties of this class by calculating each property.
        /// </summary>
        public void Update(VcProperties property, object oldValue, object newValue)
        {
            Double chartSpecificMinX = Double.NaN; // Min XValue specific to chart type. 
            Double chartSpecificMaxX = Double.NaN; // Min XValue specific to chart type. 
            Double chartSpecificMinY = Double.NaN; // Min YValue specific to chart type. 
            Double chartSpecificMaxY = Double.NaN; // Min YValue specific to chart type. 

            // List to store a concatinated set of InternalDataPoints from all DataSeries in this group
            

            // Populates the list with InternalDataPoints with all availabel InternalDataPoints from all DataSeries
            // Also set the plotGroup reference to the current plot group
            
            //List<DataPoint> _dataPointsInCurrentPlotGroup = new List<DataPoint>();
            if (property == VcProperties.None || property == VcProperties.DataPoints)
            {
                _dataPointsInCurrentPlotGroup = new List<DataPoint>();

                foreach (DataSeries dataSeries in DataSeriesList)
                {
                    if (dataSeries.RenderAs == RenderAs.Spline)
                        CalculateMinMaxOfASplineCurve(dataSeries, ref chartSpecificMinX, ref chartSpecificMaxX, ref chartSpecificMinY, ref chartSpecificMaxY);

                    System.Diagnostics.Debug.WriteLine("MinY=" + chartSpecificMinY.ToString() + " ,MaxY=" + chartSpecificMaxY.ToString());

                    // Concatinate the lists of InternalDataPoints
                    _dataPointsInCurrentPlotGroup.InsertRange(_dataPointsInCurrentPlotGroup.Count, dataSeries.InternalDataPoints);
                    
                    // set the plot group reference
                    dataSeries.PlotGroup = this;
                }
            }

            // Get a list of all XValues,YValues and ZValues from all InternalDataPoints from all the DataSeries in this Group

            if (property == VcProperties.None || property == VcProperties.XValue || property == VcProperties.DataPoints || property == VcProperties.Series)
            {
                _xValues = (from dataPoint in _dataPointsInCurrentPlotGroup where !Double.IsNaN(dataPoint.InternalXValue) select dataPoint.InternalXValue).Distinct().ToList();

                if (!Double.IsNaN(chartSpecificMinX))
                    _xValues.Add(chartSpecificMinX);

                if (!Double.IsNaN(chartSpecificMaxX))
                    _xValues.Add(chartSpecificMaxX);

                // Calculate max XValue
                MaximumX = (_xValues.Count() > 0) ? (_xValues).Max() : 0;

                // Calculate min XValue
                MinimumX = (_xValues.Count() > 0) ? (_xValues).Min() : 0;

                if (RenderAs == RenderAs.Polar)
                {
                    if (_xValues.Count > 0 && MaximumX < 360)
                        MaximumX = 360;

                    if (_xValues.Count > 0 && MinimumX < 0)
                        MinimumX = 0;
                }

                // Calculates and sets the min difference for XValues
                MinDifferenceX = GetMinDifference(_xValues.ToArray());
            }

            if (property == VcProperties.None || property == VcProperties.DataPoints || property == VcProperties.Series || property == VcProperties.ZValue)
            {
                if (property == VcProperties.ZValue)
                {
                    Double value = (Double)newValue;

                    MaximumZ = value > MaximumZ ? value : MaximumZ;
                    MinimumZ = value < MinimumZ ? value : MinimumZ;
                }
                else //if (listOfDataPointsFromAllSeries.Count > 0 && listOfDataPointsFromAllSeries[0].Parent.RenderAs == RenderAs.Bubble)
                {
                    if(GetDependentVariableTypes().Count == 2)
                        _zValues = (from dataPoint in _dataPointsInCurrentPlotGroup where !Double.IsNaN(dataPoint.ZValue) select dataPoint.ZValue).Distinct().ToArray();

                    MaximumZ = (_zValues != null && _zValues.Count() > 0) ? (_zValues).Max() : 0;
                    MinimumZ = (_zValues != null && _zValues.Count() > 0) ? (_zValues).Min() : 0;
                }
            }

            if (property == VcProperties.None || property == VcProperties.DataPoints || property == VcProperties.YValues || property == VcProperties.YValue || property == VcProperties.XValue)
            {
                Double maxY = Double.NaN, minY = Double.NaN;

                if ( RenderAs == RenderAs.Spline || property == VcProperties.None || property == VcProperties.DataPoints)
                {
                    List<Type> dependentVariableTypes = GetDependentVariableTypes();

                    if (dependentVariableTypes.Count == 1 && dependentVariableTypes[0] == typeof(List<Double>))
                        _yValues = new List<Double>();
                    else
                        _yValues = (from dataPoint in _dataPointsInCurrentPlotGroup where !Double.IsNaN(dataPoint.YValue) select dataPoint.YValue).ToList();

                    if (!Double.IsNaN(chartSpecificMinY))
                        _yValues.Add(chartSpecificMinY);

                    if (!Double.IsNaN(chartSpecificMaxY))
                        _yValues.Add(chartSpecificMaxY);

                    foreach (DataPoint dp in _dataPointsInCurrentPlotGroup)
                    {
                        if (dp.Parent.RenderAs == RenderAs.CandleStick || dp.Parent.RenderAs == RenderAs.Stock)
                        {
                            if (dp.YValues != null)
                            {
                                _yValues.Add(dp.YValues.Max());
                                _yValues.Add(dp.YValues.Min());
                            }

                        }
                    }

                    //_yValues.AddRange(yValuesList);
                }
                else if (property == VcProperties.YValue)
                {
                    _yValues.Remove((Double)oldValue);
                    _yValues.Add((Double)newValue);
                }
                else if (property == VcProperties.YValues)
                {
                    if (oldValue != null)
                    {
                        _yValues.Remove(((Double[])oldValue).Max());
                        _yValues.Remove(((Double[])oldValue).Min());
                    }
                    if(newValue != null)
                    {
                        _yValues.Add(maxY = ((Double[])newValue).Max());
                        _yValues.Add(minY = ((Double[])newValue).Min());
                    }
                }
                

                // variables to store the yValuee sum in case of stacked type charts
                // var positiveYValue;
                // var negativeYValue;

                // Calculating Max and Min YValue based on chart type
                switch (RenderAs)
                {   
                    case RenderAs.Area:
                    case RenderAs.Bar:
                    case RenderAs.Bubble:
                    case RenderAs.Column:
                    case RenderAs.Doughnut:
                    case RenderAs.Line:
                    case RenderAs.StepLine:
                    case RenderAs.Spline:
                    case RenderAs.Pie:
                    case RenderAs.Point:
                    case RenderAs.Stock:
                    case RenderAs.CandleStick:
                    case RenderAs.SectionFunnel:
                    case RenderAs.StreamLineFunnel:
                    case RenderAs.Radar:
                    case RenderAs.Polar:

                        if (property == VcProperties.YValue)
                        {
                            Double value = (Double)newValue;

                            if (value > MaximumY)
                            {
                                MaximumY = value;

                                if (_yValues.Count() > 0)
                                    MinimumY = _yValues.Min();
                            }
                            else if (value < MinimumY)
                            {
                                MinimumY = value;

                                if (_yValues.Count() > 0)
                                    MaximumY = _yValues.Max();
                            }
                            else
                            {   
                                if (_yValues.Count() > 0)
                                {
                                    MaximumY = _yValues.Max();
                                    MinimumY = _yValues.Min();
                                }
                                else
                                {
                                    MaximumY = 0;
                                    MinimumY = 0;
                                }

                                MaximumY = MaximumY;
                            }

                        }
                        else if (property == VcProperties.YValues)
                        {
                            if (maxY > MaximumY)
                                MaximumY = maxY;
                            else if (minY < MinimumY)
                                MinimumY = minY;
                            else
                            {

                                if (_yValues.Count() > 0)
                                {
                                    MaximumY = _yValues.Max();
                                    MinimumY = _yValues.Min();
                                }
                                else
                                {   
                                    MaximumY = 0;
                                    MinimumY = 0;
                                }
                            }
                        }
                        else
                        {
                            MaximumY = (_yValues.Count() > 0) ? _yValues.Max() : 0;
                            MinimumY = (_yValues.Count() > 0) ? _yValues.Min() : 0;
                        }

                        break;

                    case RenderAs.StackedArea:
                    case RenderAs.StackedBar:
                    case RenderAs.StackedColumn:
                        {
                            CreateXWiseStackedDataEntry(ref _dataPointsInCurrentPlotGroup, RenderAs.StackedColumn, RenderAs.StackedBar, RenderAs.StackedArea);

                            //if (property == VcProperties.YValue)
                            //{
                            //    Double value = (Double)newValue;
                            //    MaximumY = value > MaximumY ? value : MaximumY;
                            //    MinimumY = value < MinimumY ? value : MinimumY;
                            //}
                            //else
                            {   
                                var positiveYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.PositiveYValueSum;
                                var negativeYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.NegativeYValueSum;

                                MaximumY = (positiveYValue.Count() > 0) ? (positiveYValue).Max() : 0;
                                MinimumY = (negativeYValue.Count() > 0) ? (negativeYValue).Min() : 0;
                            }
                        }
                        break;

                    case RenderAs.StackedArea100:
                    case RenderAs.StackedBar100:
                    case RenderAs.StackedColumn100:
                        {
                            CreateXWiseStackedDataEntry(ref _dataPointsInCurrentPlotGroup, RenderAs.StackedColumn100, RenderAs.StackedBar100, RenderAs.StackedArea100);
                            
                            //if (property == VcProperties.YValue)
                            //{   
                            //    Double value = (Double)newValue;
                            //    MaximumY = value > MaximumY ? value : MaximumY;
                            //    MinimumY = value < MinimumY ? value : MinimumY;
                            //}
                            //else
                            {
                                var positiveYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.PositiveYValueSum;
                                var negativeYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.NegativeYValueSum;

                                MaximumY = (positiveYValue.Count() > 0) ? (positiveYValue).Max() : 0;
                                MinimumY = (negativeYValue.Count() > 0) ? (negativeYValue).Min() : 0;
                            }

                            // Since for stacked chart the Maximum can't be greater than 100 or less then 0
                            // Check and set appropriate limit
                            MaximumY = (MaximumY > 0) ? 100 : 0;

                            // Since for stacked chart the Minimum can't be greater than 0 or less then -100
                            // Check and set appropriate limit
                            MinimumY = (MinimumY >= 0) ? 0 : -100;
                        }

                        break;
                }


                System.Diagnostics.Debug.WriteLine("XXXX--MinY=" + MinimumY.ToString() + " , XXX-MaxY=" + MaximumY.ToString());
            }

        }


        #endregion

        List<Double> _xValues;
        Double[] _zValues;
        List<Double> _yValues;
        List<DataPoint> _dataPointsInCurrentPlotGroup;
    }

}
