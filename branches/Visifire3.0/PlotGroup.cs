﻿/*   
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
using System.Linq;
using System.Windows.Documents;


#else
using System;
using System.Collections.Generic;
using System.Linq;

#endif

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
            if (dataPoint.InternalYValue >= 0)
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

        private void CreateXWiseStackedDataEntry(ref List<DataPoint> listOfDataPointsFromAllSeries)
        {
            XWiseStackedDataList.Clear();

            // variable to temporarily store the stacked Data content
            XWiseStackedData xWiseData;

            // Populates the Xwise sorted Stacked data list with entries from 
            // all the datapoints from all DataSeries from this group
            foreach (DataPoint dataPoint in listOfDataPointsFromAllSeries)
            {   
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
        /// Updates all properties of this class by calculating each property.
        /// </summary>
        public void Update(ref List<DataPoint> listOfDataPointsFromAllSeries, VcProperties property, object newValue)
        {
            // List to store a concatinated set of InternalDataPoints from all DataSeries in this group
            

            // Populates the list with InternalDataPoints with all availabel InternalDataPoints from all DataSeries
            // Also set the plotGropu reference to the current plot group
            
            /*-------------------------------------------------
            
            List<DataPoint> dataPoints = new List<DataPoint>();
            
            foreach (DataSeries dataSeries in DataSeriesList)
            {
                // check if data series is enabled
                // if (dataSeries.Enabled == true)
                {
                    List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.InternalDataPoints select datapoint).ToList();

                    // Concatinate the lists of InternalDataPoints
                    dataPoints.InsertRange(dataPoints.Count, enabledDataPoints);

                    // set the plot group reference
                    dataSeries.PlotGroup = this;
                }
            }
            
            ---------------------------------------------------------
            */
            
            
            
            //-------------------------------------------------------

            // Get a list of all XValues,YValues and ZValues from all InternalDataPoints from all the DataSeries in this Group

            if (property == VcProperties.None || property == VcProperties.XValue || property == VcProperties.DataPoints || property == VcProperties.Series)
            {
                _xValues = (from dataPoint in listOfDataPointsFromAllSeries where !Double.IsNaN(dataPoint.InternalXValue) select dataPoint.InternalXValue).Distinct().ToArray();

                // Calculate max XValue
                MaximumX = (_xValues.Count() > 0) ? (_xValues).Max() : 0;

                // Calculate min XValue
                MinimumX = (_xValues.Count() > 0) ? (_xValues).Min() : 0;

                // Calculates and sets the min difference for XValues
                MinDifferenceX = GetMinDifference(_xValues);
            }

            if (property == VcProperties.None || property == VcProperties.DataPoints || property == VcProperties.Series || property == VcProperties.ZValue)
            {
                if (property == VcProperties.ZValue)
                {   
                    Double value = (Double)newValue;

                    MaximumZ = value > MaximumZ ? value : MaximumZ;
                    MinimumZ = value < MinimumZ ? value : MinimumZ;
                }
                else if(listOfDataPointsFromAllSeries[0].Parent.RenderAs == RenderAs.Bubble)
                {   
                    _zValues = (from dataPoint in listOfDataPointsFromAllSeries where !Double.IsNaN(dataPoint.ZValue) select dataPoint.ZValue).Distinct().ToArray();
                    
                    MaximumZ = (_zValues.Count() > 0) ? (_zValues).Max() : 0;
                    MinimumZ = (_zValues.Count() > 0) ? (_zValues).Min() : 0;
                }
            }

            if (property == VcProperties.None || property == VcProperties.DataPoints || property == VcProperties.YValues || property == VcProperties.YValue)
            {
                if(property != VcProperties.YValue)
                    _yValues = (from dataPoint in listOfDataPointsFromAllSeries where !Double.IsNaN(dataPoint.InternalYValue) select dataPoint.InternalYValue).Distinct().ToList();

                // List<Double> yValuesList = new List<double>();
                if (property == VcProperties.None || property == VcProperties.YValues)
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
                    case RenderAs.Pie:
                    case RenderAs.Point:
                    case RenderAs.Stock:
                    case RenderAs.CandleStick:
                    case RenderAs.SectionFunnel:
                    case RenderAs.StreamLineFunnel:

                        if (property == VcProperties.YValue)
                        {
                            Double value =(Double)newValue;
                            MaximumY = value > MaximumY ? value : MaximumY;
                            MinimumY = value < MinimumY ? value : MinimumY;
                        }
                        else
                        {
                            MaximumY = (_yValues.Count() > 0) ? (_yValues).Max() : 0;
                            MinimumY = (_yValues.Count() > 0) ? (_yValues).Min() : 0;
                        }

                        break;

                    case RenderAs.StackedArea:
                    case RenderAs.StackedBar:
                    case RenderAs.StackedColumn:
                        {
                            CreateXWiseStackedDataEntry(ref listOfDataPointsFromAllSeries);

                            if (property == VcProperties.YValue)
                            {
                                Double value = (Double)newValue;
                                MaximumY = value > MaximumY ? value : MaximumY;
                                MinimumY = value < MinimumY ? value : MinimumY;
                            }
                            else
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
                            CreateXWiseStackedDataEntry(ref listOfDataPointsFromAllSeries);

                            if (property == VcProperties.YValue)
                            {
                                Double value = (Double)newValue;
                                MaximumY = value > MaximumY ? value : MaximumY;
                                MinimumY = value < MinimumY ? value : MinimumY;
                            }
                            else
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
            }
        }


        #endregion

        Double[] _xValues;
        Double[] _zValues;
        List<Double> _yValues;
    }

}
