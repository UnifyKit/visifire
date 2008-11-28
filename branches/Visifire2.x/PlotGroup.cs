#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;

#else
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
#endif

namespace Visifire.Charts
{
    internal class PlotGroup
    {
        #region Public Methods
        /// <summary>
        /// Creates a instance of PlotGroup and initializes the various lists
        /// </summary>
        /// <param name="renderAs"></param>
        /// <param name="axisX"></param>
        /// <param name="axisY"></param>
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
        /// Stores the maximum XValue for the group
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
        /// Stores the minimum XValue for the group
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
        /// Store the calculated minimum difference value for the XValues in this group
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
        /// <param name="values"></param>
        /// <returns></returns>
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

            for (Int32 i = 0; i < distinctValues.Length - 1; i++)
            {
                // get the smallest difference between two successive elements
                minDiff = Math.Min(minDiff, Math.Abs(distinctValues[i] - distinctValues[i + 1]));
            }

            // return the minimu difference
            return minDiff;
        }

        #endregion

        #region Internal Methods
        /// <summary>
        /// Updates all properties of this class by calculating each property.
        /// </summary>
        internal void Update()
        {
            

            // List to store a concatinated set of DataPoints from all DataSeries in this group
            List<DataPoint> dataPoints = new List<DataPoint>();

            // Populates the list with DataPoints with all availabel DataPoints from all DataSeries
            // Also set the plotGropu reference to the current plot group
            foreach (DataSeries dataSeries in DataSeriesList)
            {
                // check if data series is enabled
                if (dataSeries.Enabled == true)
                {
                    List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.DataPoints select datapoint).ToList();

                    // Concatinate the lists of DataPoints
                    dataPoints.InsertRange(dataPoints.Count, enabledDataPoints);

                    // set the plot group reference
                    dataSeries.PlotGroup = this;
                }
            }

            // variable to temporarily store the stacked Data content
            XWiseStackedData xWiseData;

            // Populates the Xwise sorted Stacked data list with entries from 
            // all the datapoints from all DataSeries from this group
            foreach (DataPoint dataPoint in dataPoints)
            {

                if (XWiseStackedDataList.ContainsKey(dataPoint.XValue))
                {
                    // gets the existing  node
                    xWiseData = XWiseStackedDataList[dataPoint.XValue];
                }
                else
                {
                    // Creates a new node
                    xWiseData = new XWiseStackedData();
                    XWiseStackedDataList.Add(dataPoint.XValue, xWiseData);
                }

                // add the datapoint to a node
                AddXWiseStackedDataEntry(ref xWiseData, dataPoint);
            }

            // Get a list of all XValues,YValues and ZValues from all DataPoints from all the DataSeries in this Group
            var xValues = (from dataPoint in dataPoints where !Double.IsNaN(dataPoint.XValue) select dataPoint.XValue).Distinct();
            var yValues = (from dataPoint in dataPoints where !Double.IsNaN(dataPoint.YValue) select dataPoint.YValue).Distinct();
            var zValues = (from dataPoint in dataPoints where !Double.IsNaN(dataPoint.ZValue) select dataPoint.ZValue).Distinct();

            // Calculate max value
            MaximumX = (xValues.Count() > 0) ? (xValues).Max() : 0;
            MaximumZ = (zValues.Count() > 0) ? (zValues).Max() : 0;

            // Calculate min value
            MinimumX = (xValues.Count() > 0) ? (xValues).Min() : 0;
            MinimumZ = (zValues.Count() > 0) ? (zValues).Min() : 0;

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
                    MaximumY = (yValues.Count() > 0) ? (yValues).Max() : 0;
                    MinimumY = (yValues.Count() > 0) ? (yValues).Min() : 0;
                    break;

                case RenderAs.StackedArea:
                case RenderAs.StackedBar:
                case RenderAs.StackedColumn:
                    {
                        var positiveYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.PositiveYValueSum;
                        var negativeYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.NegativeYValueSum;
                        MaximumY = (positiveYValue.Count() > 0) ? (positiveYValue).Max() : 0;
                        MinimumY = (negativeYValue.Count() > 0) ? (negativeYValue).Min() : 0;
                    }
                    break;

                case RenderAs.StackedArea100:
                case RenderAs.StackedBar100:
                case RenderAs.StackedColumn100:
                    {
                        var positiveYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.PositiveYValueSum;
                        var negativeYValue = from xwisedata in XWiseStackedDataList.Values select xwisedata.NegativeYValueSum;
                        MaximumY = (positiveYValue.Count() > 0) ? (positiveYValue).Max() : 0;
                        MinimumY = (negativeYValue.Count() > 0) ? (negativeYValue).Min() : 0;
                    }
                    // Since for stacked chart the Maximum can't be greater than 100 or less then 0
                    // Check and set appropriate limit
                    if (MaximumY > 0) MaximumY = 100;
                    else MaximumY = 0;

                    // Since for stacked chart the Minimum can't be greater than 0 or less then -100
                    // Check and set appropriate limit
                    if (MinimumY >= 0) MinimumY = 0;
                    else MinimumY = -100;

                    break;
            }

            // Calculates and sets the min difference for XValues
            MinDifferenceX = GetMinDifference(xValues.ToArray());
        }
        #endregion

    }

}
