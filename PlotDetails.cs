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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;


#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Diagnostics;
#endif

namespace Visifire.Charts
{
    /// <summary>
    /// Containes all the details about the data required for various plotting puposes
    /// </summary>
    internal class PlotDetails
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.PlotDetails class
        /// </summary>
        public PlotDetails(Chart chart)
        {
            // Create a plot groups list
            this.PlotGroups = new List<PlotGroup>();

            // To store all the axisX labels for the primary axisX;
            this.AxisXPrimaryLabels = new Dictionary<Double, String>();

            // To store all the axisX labels for the secondary axisX;
            this.AxisXSecondaryLabels = new Dictionary<Double, String>();

            // Store the chart reference
            this.Chart = chart;

            // Set default chart orientation
            this.ChartOrientation = ChartOrientationType.Undefined;

            // Validate XValue type of the DataPoint and DataSeries
            SetDataPointsNameAndValidateDataPointXValueType();

            // Calculate all the required details
            this.Calculate();
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
        /// List of all DataPoints in chart
        /// </summary>
        internal List<DataPoint> ListOfAllDataPoints
        {
            get;
            set;
        }

        /// <summary>
        /// List of different types of plot groups based on RenderAs, AxisXType and AxisYType
        /// </summary>
        internal List<PlotGroup> PlotGroups
        {
            get;
#if DEBUG
            set;
#else
            private set;
#endif
        }

        /// <summary>
        /// Reference to the chart element
        /// </summary>
        internal Chart Chart
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the overall orientation of the chart
        /// </summary>
        internal ChartOrientationType ChartOrientation
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the number of divisions to Divide the height/width available for each datapoint while rendering in multiseries combinations. 
        /// Also used for calculating axisX limits
        /// </summary>
        internal Int32 DrawingDivisionFactor
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the value which will be used to calculate the thickness in the case of 3D charts
        /// </summary>
        internal Int32 Layer3DCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Dictionary that stores the Series Drawing Indexes, the Key is the DataSeries itself
        /// </summary>
        internal Dictionary<DataSeries, Int32> SeriesDrawingIndex
        {
            get;
            set;
        }

        /// <summary>
        /// List of unique axisX primary labels
        /// </summary>
        internal Dictionary<Double, String> AxisXPrimaryLabels
        {
            get;
            private set;
        }

        /// <summary>
        /// List of unique axisX secondary labels
        /// </summary>
        internal Dictionary<Double, String> AxisXSecondaryLabels
        {
            get;
            private set;
        }

        /// <summary>
        /// Whether all primary axis labels are present
        /// </summary>
        internal Boolean IsAllPrimaryAxisXLabelsPresent
        {
            get;
            set;
        }
        /// <summary>
        /// Whether all secondary axis labels are present
        /// </summary>
        internal Boolean IsAllSecondaryAxisXLabelsPresent
        {
            get;
            set;
        }
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Validate XValue type of the DataPoint and DataSeries
        /// </summary>
        private void SetDataPointsNameAndValidateDataPointXValueType()
        {
            Int32 dsIndex = 0;
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                foreach (DataPoint dp in ds.InternalDataPoints)
                {
                    if (ds.XValueType == ChartValueTypes.Auto)
                    {   
                        ds.InternalXValueType = (dp.XValueType == ChartValueTypes.DateTime) ? ChartValueTypes.Date : dp.XValueType;
                    }
                    else if ((ds.XValueType == ChartValueTypes.Date || ds.XValueType == ChartValueTypes.DateTime || ds.XValueType == ChartValueTypes.Time)
                        && dp.XValueType != ChartValueTypes.DateTime)
                    {
                        throw new Exception("Error occurred due to incorrect XValue format. XValue can be Double or DateTime for all DataPoints in a DataSeries.");
                    }
                    else if (ds.XValueType == ChartValueTypes.Numeric && dp.XValueType != ChartValueTypes.Numeric)
                    {
                        throw new Exception("Error occurred due to incorrect XValue format. XValue can be Double or DateTime for all DataPoints in a DataSeries according to XValueType of the DataSeries.");
                    }
                }

                dsIndex++;
            }
        }

        /// <summary>
        /// Calculate PlotDetails
        /// </summary>
        private void Calculate()
        {
            // Create Axis incase if it doesnt exist
            CreateMissingAxes();

            Axis axisXSecondary = GetAxisXFromChart(Chart, AxisTypes.Secondary);

            if (axisXSecondary != null)
                throw new Exception("Note: Secondary AxisX is not yet supported in Visifire.");

            Axis axisX = GetAxisXFromChart(Chart, AxisTypes.Primary);

            // Generate XValues for DataTime axis
            GenerateXValueForDataTimeAxis(axisX);

            // Identifies the various plot groups and populates the list
            PopulatePlotGroups();

            SetTrendLineValues(axisX);

            // Generates a index set that identifies the order in which the series must be drawn(layering order)
            SeriesDrawingIndex = GenerateDrawingOrder();

            // Create list of datapoints from all series
            CreateListOfDataPoints();

            // Gets a unique set of axis labels by axisX type
            AxisXPrimaryLabels = GetAxisXLabels(AxisTypes.Primary);
            AxisXSecondaryLabels = GetAxisXLabels(AxisTypes.Secondary);

            // Set Labels count state
            SetLabelsCountState();
        }

        /// <summary>
        /// Get List of DateTime from all DataSeries
        /// </summary>
        private List<DateTime> GetListOfXValue(Axis axis)
        {

            List<DateTime> xValuesAsDateTimeList = new List<DateTime>();

            int dataSeriesIndex = 0, dataPointIndex = 0;

            foreach (DataSeries ds in Chart.InternalSeries)
            {
                dataPointIndex = 0;

                foreach (DataPoint dp in ds.InternalDataPoints)
                {

                    if (dp.InternalXValueAsDateTime == null)
                    {
                        throw new Exception("Wrong property value as date for XValue in DataPoint position " + dataPointIndex.ToString() + " and DataSeries position " + dataSeriesIndex.ToString());
                    }

                    try
                    {   
                        if (dp.XValueType == ChartValueTypes.Numeric)
                            throw new Exception();

                        if (ds.InternalXValueType == ChartValueTypes.Auto || ds.InternalXValueType == ChartValueTypes.Date)
                        {
                            dp.InternalXValueAsDateTime = new DateTime(dp.InternalXValueAsDateTime.Date.Year, dp.InternalXValueAsDateTime.Date.Month, dp.InternalXValueAsDateTime.Date.Day);
                            xValuesAsDateTimeList.Add(dp.InternalXValueAsDateTime);
                        }
                        else if (ds.InternalXValueType == ChartValueTypes.DateTime)
                        {
                            // if (axis.IntervalType == IntervalTypes.Years || axis.IntervalType == IntervalTypes.Months || axis.IntervalType == IntervalTypes.Weeks || axis.IntervalType == IntervalTypes.Days)
                            //    xValuesAsDateTimeList.Add((DateTime)dp.InternalXValueAsDateTime.Date);
                            // else
                            xValuesAsDateTimeList.Add((DateTime)dp.InternalXValueAsDateTime);
                        }
                        else if (ds.InternalXValueType == ChartValueTypes.Time)
                        {
                            System.Diagnostics.Debug.WriteLine(dp.InternalXValueAsDateTime.TimeOfDay.ToString());
                            dp.InternalXValueAsDateTime = DateTime.Parse("12/30/1899 " + dp.InternalXValueAsDateTime.TimeOfDay.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            xValuesAsDateTimeList.Add(dp.InternalXValueAsDateTime);
                        }
                    }
                    catch
                    {
                        throw new Exception("Wrong property value as date for XValue in DataPoint position " + dataPointIndex.ToString() + " and DataSeries position " + dataSeriesIndex.ToString());
                    }

                    dataPointIndex++;
                }

                dataSeriesIndex++;
            }

            return xValuesAsDateTimeList;
        }

        /// <summary>
        /// Whether axis is a date time axis.
        /// Also set up common XValue Type for axis
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        private Boolean CheckIsDateTimeAxis(Axis axis)
        {
            Boolean isDateTimeAxis = false;                             // Whether axisX is  DateTime axis
            axis.XValueType = ChartValueTypes.Numeric;
            Boolean isAnyDateTimeSeriesExist = false;

            int seriesCount = (from dataSeries in Chart.InternalSeries
                               where dataSeries.InternalXValueType == ChartValueTypes.Date
                               select dataSeries).Count();

            isAnyDateTimeSeriesExist = seriesCount > 0 ? true : false;

            if (seriesCount == Chart.InternalSeries.Count) // Is chart having a DateTime Axis
            {
                isDateTimeAxis = true;
                axis.XValueType = ChartValueTypes.Date;
            }
            else
            {
                seriesCount = (from dataSeries in Chart.InternalSeries
                               where dataSeries.InternalXValueType == ChartValueTypes.DateTime
                               select dataSeries).Count();

                isAnyDateTimeSeriesExist = seriesCount > 0 ? true : isAnyDateTimeSeriesExist;

                if (seriesCount == Chart.InternalSeries.Count) // Is chart having a DateTime Axis
                {
                    isDateTimeAxis = true;
                    axis.XValueType = ChartValueTypes.DateTime;
                }
                else
                {
                    seriesCount = (from dataSeries in Chart.InternalSeries
                                   where dataSeries.InternalXValueType == ChartValueTypes.Time
                                   select dataSeries).Count();

                    isAnyDateTimeSeriesExist = seriesCount > 0 ? true : isAnyDateTimeSeriesExist;

                    if (seriesCount == Chart.InternalSeries.Count) // Is chart having a DateTime Axis
                    {
                        isDateTimeAxis = true;
                        axis.XValueType = ChartValueTypes.Time;
                    }
                }
            }

            if (!isDateTimeAxis && isAnyDateTimeSeriesExist)
            {
                throw new Exception("Error occurred, different types of XValue type found in DataSeries. Check for property value applied for XValueType property of different DataSeries.");
            }

            return isDateTimeAxis;
        }

       /// <summary>
       /// Set TrendLine values
       /// </summary>
       /// <param name="axisX">Axis</param>
        private void SetTrendLineValues(Axis axisX)
        {
            if (axisX.IsDateTimeAxis)
            {
                foreach (TrendLine trendLine in Chart.TrendLines)
                {
                    if ((Boolean)trendLine.Enabled &&
                        ((trendLine.Orientation == Orientation.Vertical && axisX.PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
                        || (trendLine.Orientation == Orientation.Horizontal && axisX.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                        )
                    )
                        trendLine.InternalNumericValue = DateTimeHelper.DateDiff(trendLine.InternalDateValue, axisX.MinDate, axisX.MinDateRange, axisX.MaxDateRange, axisX.InternalIntervalType, axisX.XValueType);
                }
            }

        }

        /// <summary>
        /// Reset DateTime for AxisMinimum and Axis AxisMaximum
        /// </summary>
        /// <param name="axis"></param>
        private void ResetDateTime4AxisMinimumAndMaximum(Axis axis)
        {
            if (axis.AxisMinimum != null)
            {
                if (axis._axisMinimumValueType == ChartValueTypes.Numeric)
                    throw new Exception("AxisMinimum should have a value of type Date/DateTime/Time");
 
                if (axis.XValueType == ChartValueTypes.Date)
                    axis.AxisMinimumDateTime = new DateTime(axis.AxisMinimumDateTime.Year, axis.AxisMinimumDateTime.Month, axis.AxisMinimumDateTime.Day);
                else if (axis.XValueType == ChartValueTypes.Time)
                    axis.AxisMinimumDateTime = DateTime.Parse("12/30/1899 " + axis.AxisMinimumDateTime.TimeOfDay.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }

            if (axis.AxisMaximum != null)
            {
                if (axis._axisMaximumValueType == ChartValueTypes.Numeric)
                    throw new Exception("AxisMaximum should have a value of type Date/DateTime/Time");
                
                if (axis.XValueType == ChartValueTypes.Date)
                    axis.AxisMaximumDateTime = new DateTime(axis.AxisMaximumDateTime.Year, axis.AxisMaximumDateTime.Month, axis.AxisMaximumDateTime.Day);
                else if (axis.XValueType == ChartValueTypes.Time)
                    axis.AxisMaximumDateTime = DateTime.Parse("12/30/1899 " + axis.AxisMaximumDateTime.TimeOfDay.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Generates XValues for DataTime axis
        /// </summary>
        private void GenerateXValueForDataTimeAxis(Axis axisX)
        {
            if (axisX != null)
            {
                axisX._isDateTimeAutoInterval = false;// Minimum difference between two DataTimes
                axisX.IsDateTimeAxis = CheckIsDateTimeAxis(axisX);

                if (axisX.IsDateTimeAxis)
                {
                    DateTime minDate, maxDate; // Min and max date
                    TimeSpan minDateRange, maxDateRange;

                    List<DateTime> xValuesAsDateTimeList = GetListOfXValue(axisX);

                    ResetDateTime4AxisMinimumAndMaximum(axisX);

                    if (xValuesAsDateTimeList.Count != 0)
                    {
                        DateTimeHelper.CalculateMinMaxDate(xValuesAsDateTimeList, out minDate, out maxDate, out minDateRange, out maxDateRange);

                        IntervalTypes autoIntervalType = axisX.InternalIntervalType;
                        Double maxInterval = (axisX.XValueType == ChartValueTypes.Date || axisX.XValueType == ChartValueTypes.Time) ? 8 : 8;
                        if (minDate != maxDate)
                        {
                            axisX.InternalInterval = DateTimeHelper.CalculateAutoInterval(Chart.ActualWidth, Chart.ActualHeight, axisX.AxisOrientation, minDate, maxDate, out autoIntervalType, maxInterval, axisX.XValueType);
                        }
                        else
                        {
                            if (axisX.XValueType != ChartValueTypes.Time)
                            {
                                //if (axisX.AxisMinimum == null)
                                //{
                                //    minDate = minDate.AddDays(-1);
                                //    autoIntervalType = IntervalTypes.Days;
                                //}
                                if (axisX.AxisMinimum == null)
                                {
                                    if (axisX.InternalIntervalType == IntervalTypes.Hours)
                                    {
                                        minDate = minDate.AddHours(-1);
                                        autoIntervalType = axisX.InternalIntervalType;
                                    }
                                    else if (axisX.InternalIntervalType == IntervalTypes.Minutes)
                                    {
                                        minDate = minDate.AddMinutes(-1);
                                        autoIntervalType = axisX.InternalIntervalType;
                                    }
                                    else if (axisX.InternalIntervalType == IntervalTypes.Seconds)
                                    {
                                        minDate = minDate.AddSeconds(-1);
                                        autoIntervalType = axisX.InternalIntervalType;
                                    }
                                    else if (axisX.InternalIntervalType == IntervalTypes.Milliseconds)
                                    {
                                        minDate = minDate.AddMilliseconds(-1);
                                        autoIntervalType = axisX.InternalIntervalType;
                                    }
                                    else
                                    {
                                        minDate = minDate.AddDays(-1);
                                        autoIntervalType = IntervalTypes.Days;
                                    }
                                }
                                else
                                {
                                    TimeSpan minDateDifference = minDate.Subtract((DateTime)axisX.AxisMinimumDateTime);

                                    if (minDateDifference.TotalDays <= 30)
                                    {
                                        autoIntervalType = IntervalTypes.Days;
                                        minDate = minDate.AddDays(-1);
                                    }
                                    else if ((minDateDifference.TotalDays / 30) < 12)
                                    {
                                        autoIntervalType = IntervalTypes.Months;
                                        minDate = minDate.AddMonths(-1);
                                    }
                                    else if (minDateDifference.TotalDays / 365.242199 > 0)
                                    {
                                        autoIntervalType = IntervalTypes.Years;
                                        minDate = minDate.AddYears(-1);
                                    }
                                    else if (minDateDifference.TotalHours > 0)
                                    {
                                        autoIntervalType = IntervalTypes.Hours;
                                        minDate = minDate.AddHours(-1);
                                    }
                                    else if (minDateDifference.TotalMinutes > 0)
                                    {
                                        autoIntervalType = IntervalTypes.Minutes;
                                        minDate = minDate.AddMinutes(-1);
                                    }
                                    else if (minDateDifference.TotalSeconds > 0)
                                    {
                                        autoIntervalType = IntervalTypes.Seconds;
                                        minDate = minDate.AddSeconds(-1);
                                    }
                                    else if (minDateDifference.TotalMilliseconds > 0)
                                    {
                                        autoIntervalType = IntervalTypes.Milliseconds;
                                        minDate = minDate.AddMilliseconds(-1);
                                    }
                                }
                            }

                            axisX.InternalInterval = 1;
                            axisX._isDateTimeAutoInterval = true;

                        }

                        //if (minDate == maxDate)
                        //{
                        // //if (axisX.XValueType != ChartValueTypes.Time)
                        // // minDate = minDate.AddDays(-1);
                        // autoIntervalType = IntervalTypes.Days;
                        //}

                        if (axisX.XValueType == ChartValueTypes.DateTime || axisX.XValueType == ChartValueTypes.Date)
                        {
                            if (axisX.IntervalType == IntervalTypes.Auto || Double.IsNaN((Double)axisX.Interval))
                            {
                                axisX._isDateTimeAutoInterval = true;
                                axisX.InternalIntervalType = autoIntervalType;
                            }
                            else
                            {
                                Boolean isAcceptAutoIntervalType = false;

                                if (axisX.XValueType == ChartValueTypes.Date && (
                                axisX.IntervalType == IntervalTypes.Hours || axisX.IntervalType == IntervalTypes.Minutes
                                || axisX.IntervalType == IntervalTypes.Seconds || axisX.IntervalType == IntervalTypes.Milliseconds
                                || axisX.IntervalType == IntervalTypes.Auto))
                                    isAcceptAutoIntervalType = true;

                                if (axisX.IntervalType == IntervalTypes.Auto)
                                {
                                    isAcceptAutoIntervalType = true;
                                }

                                if (axisX.Interval == null || Double.IsNaN((Double)axisX.Interval))
                                {
                                    if (axisX.IntervalType == IntervalTypes.Years)
                                    {
                                        if (autoIntervalType == IntervalTypes.Months || autoIntervalType == IntervalTypes.Weeks
                                        || autoIntervalType == IntervalTypes.Days || autoIntervalType == IntervalTypes.Hours
                                        || autoIntervalType == IntervalTypes.Minutes || autoIntervalType == IntervalTypes.Seconds
                                        || autoIntervalType == IntervalTypes.Milliseconds
                                        )
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Months)
                                    {
                                        if (autoIntervalType == IntervalTypes.Weeks || autoIntervalType == IntervalTypes.Days
                                        || autoIntervalType == IntervalTypes.Hours || autoIntervalType == IntervalTypes.Minutes
                                        || autoIntervalType == IntervalTypes.Seconds || autoIntervalType == IntervalTypes.Milliseconds
                                        )
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Weeks)
                                    {
                                        if (autoIntervalType == IntervalTypes.Days || autoIntervalType == IntervalTypes.Hours
                                        || autoIntervalType == IntervalTypes.Minutes || autoIntervalType == IntervalTypes.Seconds
                                        || autoIntervalType == IntervalTypes.Milliseconds
                                        )
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Days)
                                    {
                                        if (autoIntervalType == IntervalTypes.Days || autoIntervalType == IntervalTypes.Hours
                                        || autoIntervalType == IntervalTypes.Minutes || autoIntervalType == IntervalTypes.Seconds
                                        || autoIntervalType == IntervalTypes.Milliseconds
                                        )
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Hours)
                                    {
                                        if (autoIntervalType == IntervalTypes.Minutes || autoIntervalType == IntervalTypes.Seconds
                                        || autoIntervalType == IntervalTypes.Milliseconds
                                        )
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Minutes)
                                    {
                                        if (autoIntervalType == IntervalTypes.Seconds || autoIntervalType == IntervalTypes.Milliseconds)
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Seconds)
                                    {
                                        if (autoIntervalType == IntervalTypes.Milliseconds)
                                            isAcceptAutoIntervalType = true;
                                    }
                                    else if (axisX.IntervalType == IntervalTypes.Milliseconds)
                                    {
                                        if (autoIntervalType == IntervalTypes.Milliseconds)
                                            isAcceptAutoIntervalType = true;
                                    }

                                    axisX._isDateTimeAutoInterval = true;
                                }

                                if (axisX.InternalIntervalType != autoIntervalType)
                                {
                                    axisX.InternalInterval = UpdateTimeIterval(autoIntervalType, axisX.InternalIntervalType, axisX.InternalInterval);
                                }

                                if (isAcceptAutoIntervalType)
                                    axisX.InternalIntervalType = autoIntervalType;
                            }


                            if (axisX.AxisMinimum == null)
                            {
                                //DateTime newMinDate = DateTimeHelper.AlignDateTime(minDate, axisX.InternalInterval, axisX.InternalIntervalType);

                                //if (newMinDate != minDate)
                                //{
                                // // Chart.InternalSeries[0].IsNotificationEnable = false;
                                // // Chart.InternalSeries[0].InternalDataPoints.Add(new DataPoint() { XValueType = ChartValueTypes.DateTime, InternalXValueAsDateTime = newMinDate, Enabled = false, Parent = Chart.InternalSeries[0] });
                                // // Chart.InternalSeries[0].IsNotificationEnable = true;

                                // minDate = newMinDate;
                                //}
                            }
                        }

                        if (axisX.XValueType == ChartValueTypes.Time)
                        {
                            if (axisX.IntervalType == IntervalTypes.Auto || Double.IsNaN((Double)axisX.Interval))
                            {
                                axisX._isDateTimeAutoInterval = true;
                                axisX.InternalIntervalType = autoIntervalType;
                            }
                            else
                            {
                                if (axisX.IntervalType == IntervalTypes.Years || axisX.IntervalType == IntervalTypes.Months
                                || axisX.IntervalType == IntervalTypes.Weeks || axisX.IntervalType == IntervalTypes.Days
                                || axisX.IntervalType == IntervalTypes.Auto)
                                {
                                    if (autoIntervalType == IntervalTypes.Years || autoIntervalType == IntervalTypes.Months
                                    || autoIntervalType == IntervalTypes.Weeks || autoIntervalType == IntervalTypes.Days)
                                        axisX.InternalIntervalType = IntervalTypes.Hours;
                                    else
                                        axisX.InternalIntervalType = autoIntervalType;
                                }
                                else
                                {
                                    if (axisX.Interval == null || Double.IsNaN((Double)axisX.Interval))
                                    {
                                        if ((axisX.InternalIntervalType == IntervalTypes.Hours && maxDateRange.Hours == 0)
                                        || (axisX.InternalIntervalType == IntervalTypes.Minutes && maxDateRange.Minutes == 0)
                                        || (axisX.InternalIntervalType == IntervalTypes.Seconds && maxDateRange.Seconds == 0))
                                        {
                                            axisX.InternalIntervalType = autoIntervalType;
                                        }
                                    }
                                }

                                if (axisX.Interval == null || Double.IsNaN((Double)axisX.Interval))
                                {
                                    if (axisX.InternalIntervalType != autoIntervalType && minDate.TimeOfDay != maxDate.TimeOfDay)
                                    {
                                        axisX.InternalInterval = UpdateTimeIterval(autoIntervalType, axisX.InternalIntervalType, axisX.InternalInterval);

                                    }
                                    else
                                        axisX.InternalInterval = 1;

                                    axisX._isDateTimeAutoInterval = true;
                                }
                                else
                                    axisX.InternalInterval = (Double)axisX.Interval;
                            }

                            if (axisX.InternalIntervalType == IntervalTypes.Hours || axisX.InternalIntervalType == IntervalTypes.Minutes || axisX.InternalIntervalType == IntervalTypes.Seconds || axisX.InternalIntervalType == IntervalTypes.Milliseconds)
                            {
                                if (axisX.AxisMinimum == null)
                                {
                                    //DateTime newMinDate = DateTimeHelper.AlignDateTime(minDate, axisX.InternalInterval, axisX.InternalIntervalType);

                                    //if (newMinDate != minDate && minDate.TimeOfDay != maxDate.TimeOfDay)
                                    //{
                                    // // Chart.InternalSeries[0].IsNotificationEnable = false;
                                    // // Chart.InternalSeries[0].InternalDataPoints.Add(new DataPoint() { XValueType = ChartValueTypes.DateTime, InternalXValueAsDateTime = newMinDate, Enabled = false, Parent = Chart.InternalSeries[0] });
                                    // // Chart.InternalSeries[0].IsNotificationEnable = true;

                                    // minDate = newMinDate;
                                    //}
                                }
                            }
                        }

                        axisX.MinDate = minDate;
                        axisX.MaxDate = maxDate;
                        axisX.MinDateRange = minDateRange;
                        axisX.MaxDateRange = maxDateRange;

                        if (Chart.InternalSeries.Count == 1 && Chart.InternalSeries[0].InternalDataPoints.Count == 1)
                        {
                            axisX._isAllXValueZero = false;
                            DataPoint dp = Chart.InternalSeries[0].InternalDataPoints[0] as DataPoint;
                            dp.InternalXValue = DateTimeHelper.DateDiff((DateTime)dp.InternalXValueAsDateTime, minDate, minDateRange, maxDateRange, axisX.InternalIntervalType, axisX.XValueType);

                            if (dp.InternalXValue == 0)
                            {
                                axisX._isAllXValueZero = true;
                                dp.InternalXValue = 1;
                            }
                        }
                        else
                        {
                            axisX._isAllXValueZero = true;

                            foreach (DataSeries ds in Chart.InternalSeries)
                            {
                                foreach (DataPoint dp in ds.InternalDataPoints)
                                {
                                    dp.InternalXValue = DateTimeHelper.DateDiff((DateTime)dp.InternalXValueAsDateTime, minDate, minDateRange, maxDateRange, axisX.InternalIntervalType, axisX.XValueType);

                                    if (dp.InternalXValue == 0 && axisX._isAllXValueZero == true)
                                        axisX._isAllXValueZero = true;
                                    else
                                        axisX._isAllXValueZero = false;

                                    System.Diagnostics.Debug.WriteLine("XValue =" + dp.InternalXValue.ToString());
                                }
                            }

                            if (axisX._isAllXValueZero == true)
                            {
                                foreach (DataSeries ds in Chart.InternalSeries)
                                {
                                    foreach (DataPoint dp in ds.InternalDataPoints)
                                    {
                                        dp.InternalXValue = 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
                
        /// <summary>
        /// Update interval in terms of time according to interval type
        /// </summary>
        /// <param name="autoIntervalType">Auto interval type</param>
        /// <param name="currentIntervalTypes">Current interval type</param>
        /// <param name="intervalToUpdate">Interval to update</param>
        /// <returns>Returns interval in terms of current interval type</returns>
        private Double UpdateTimeIterval(IntervalTypes autoIntervalType, IntervalTypes currentIntervalTypes, Double intervalToUpdate)
        {
            if (autoIntervalType == IntervalTypes.Hours)
            {
                if (currentIntervalTypes == IntervalTypes.Minutes)
                    intervalToUpdate = intervalToUpdate * 60;
                else if (currentIntervalTypes == IntervalTypes.Seconds)
                    intervalToUpdate = intervalToUpdate * 60 * 60;
                else if (currentIntervalTypes == IntervalTypes.Milliseconds)
                    intervalToUpdate = intervalToUpdate * 60 * 60 * 1000;
            }
            else if (autoIntervalType == IntervalTypes.Minutes)
            {
                if (currentIntervalTypes == IntervalTypes.Hours)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate / 60);
                if (currentIntervalTypes == IntervalTypes.Seconds)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate * 60);
                else if (currentIntervalTypes == IntervalTypes.Milliseconds)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate * 60 * 1000);
            }
            else if (autoIntervalType == IntervalTypes.Seconds)
            {
                if (currentIntervalTypes == IntervalTypes.Hours)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate / 3600);
                if (currentIntervalTypes == IntervalTypes.Minutes)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate / 60);
                else if (currentIntervalTypes == IntervalTypes.Milliseconds)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate * 1000);
            }
            else if (autoIntervalType == IntervalTypes.Milliseconds)
            {
                if (currentIntervalTypes == IntervalTypes.Hours)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate / 3600000);
                if (currentIntervalTypes == IntervalTypes.Minutes)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate / 60000);
                else if (currentIntervalTypes == IntervalTypes.Seconds)
                    intervalToUpdate = Math.Ceiling(intervalToUpdate / 1000);
            }

            return intervalToUpdate;
        }


        /// <summary>
        /// Set labels count state
        /// </summary>
        private void SetLabelsCountState()
        {
            //Get all dataPoints with series reference to dataseries
            List<DataPoint> dataPointsListPri = new List<DataPoint>();
            List<DataPoint> dataPointsListSec = new List<DataPoint>();

            // Populates the list with InternalDataPoints with all availabel InternalDataPoints from all DataSeries
            foreach (DataSeries dataSeries in Chart.InternalSeries)
            {
                List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.InternalDataPoints select datapoint).ToList(); // where datapoint.Enabled == true
                // Concatinate the lists of InternalDataPoints if the axis type matches
                if (dataSeries.AxisXType == AxisTypes.Primary)
                    dataPointsListPri.InsertRange(dataPointsListPri.Count, enabledDataPoints);
                else
                    dataPointsListSec.InsertRange(dataPointsListSec.Count, enabledDataPoints);
            }

            var uniqueLabels4PrimaryAxisX = (from dataPoint in dataPointsListPri group dataPoint by dataPoint.InternalXValue);

            IsAllPrimaryAxisXLabelsPresent = (AxisXPrimaryLabels.Count == uniqueLabels4PrimaryAxisX.Count());

            var uniqueLabels4SecondaryAxisX = (from dataPoint in dataPointsListSec group dataPoint by dataPoint.InternalXValue);

            IsAllSecondaryAxisXLabelsPresent = (AxisXSecondaryLabels.Count == uniqueLabels4SecondaryAxisX.Count());

        }

        /// <summary>
        /// Create missing axis
        /// </summary>
        private void CreateMissingAxes()
        {
            ChartOrientationType chartOrientation = GetChartOrientation();

            Orientation axisXOrientation = (chartOrientation == ChartOrientationType.Vertical) ? Orientation.Horizontal : Orientation.Vertical;
            Orientation axisYOrientation = (chartOrientation == ChartOrientationType.Vertical) ? Orientation.Vertical : Orientation.Horizontal;

            #region Check primary Axis X

            if (GetCountOfSeriesUsingAxisXPrimary() > 0)
            {
                Axis axisX = GetAxisXFromChart(Chart, AxisTypes.Primary);
                if (axisX == null)
                {
                    axisX = new Axis();
                    axisX._isAutoGenerated = true;
                    axisX.Chart = Chart;
                    axisX.AxisOrientation = axisXOrientation;
                    axisX.AxisType = AxisTypes.Primary;
                    axisX.PlotDetails = this;
                    axisX.AxisRepresentation = AxisRepresentations.AxisX;
                    Chart.InternalAxesX.Add(axisX);
                    Chart.AxesX.Add(axisX);
                }
                else
                {
                    axisX.AxisOrientation = axisXOrientation;
                    axisX.AxisType = AxisTypes.Primary;
                    axisX.PlotDetails = this;
                    axisX.AxisRepresentation = AxisRepresentations.AxisX;
                }
            }
            else
            {
                Axis axisX = GetAxisXFromChart(Chart, AxisTypes.Primary);
                while (axisX != null)
                {
                    Chart.InternalAxesX.Remove(axisX);
                    axisX = GetAxisXFromChart(Chart, AxisTypes.Primary);
                }
            }

            #endregion

            #region Check Secondary axis X

            if (GetCountOfSeriesUsingAxisXSecondary() > 0)
            {
                Axis axisX = GetAxisXFromChart(Chart, AxisTypes.Secondary);

                if (axisX == null)
                {
                    axisX = new Axis();
                    axisX._isAutoGenerated = true;
                    axisX.Chart = Chart;

                    axisX.AxisOrientation = axisXOrientation;
                    axisX.AxisType = AxisTypes.Secondary;
                    axisX.PlotDetails = this;
                    axisX.AxisRepresentation = AxisRepresentations.AxisX;
                    Chart.InternalAxesX.Add(axisX);
                    Chart.AxesX.Add(axisX);
                }
                else
                {
                    axisX.AxisOrientation = axisXOrientation;
                    axisX.AxisType = AxisTypes.Secondary;
                    axisX.PlotDetails = this;
                    axisX.AxisRepresentation = AxisRepresentations.AxisX;
                }
            }
            else
            {
                Axis axisX = GetAxisXFromChart(Chart, AxisTypes.Secondary);
                while (axisX != null)
                {
                    Chart.InternalAxesX.Remove(axisX);
                    axisX = GetAxisXFromChart(Chart, AxisTypes.Secondary);
                }
            }

            #endregion

            #region Check primary Axis Y

            if (GetCountOfSeriesUsingAxisYPrimary() > 0)
            {
                Axis axisY = GetAxisYFromChart(Chart, AxisTypes.Primary);

                if (axisY == null)
                {
                    axisY = new Axis();
                    axisY._isAutoGenerated = true;
                    axisY.Chart = Chart;
                    axisY.AxisOrientation = axisYOrientation;
                    axisY.AxisType = AxisTypes.Primary;
                    axisY.PlotDetails = this;
                    axisY.AxisRepresentation = AxisRepresentations.AxisY;
                    Chart.InternalAxesY.Add(axisY);
                    Chart.AxesY.Add(axisY);
                }
                else
                {
                    axisY.AxisOrientation = axisYOrientation;
                    axisY.AxisType = AxisTypes.Primary;
                    axisY.PlotDetails = this;
                    axisY.AxisRepresentation = AxisRepresentations.AxisY;
                }
            }
            else
            {
                Axis axisY = GetAxisYFromChart(Chart, AxisTypes.Primary);
                while (axisY != null)
                {
                    Chart.InternalAxesY.Remove(axisY);
                    axisY = GetAxisYFromChart(Chart, AxisTypes.Primary);
                }
            }

            #endregion

            #region Check Secondary axis Y

            if (GetCountOfSeriesUsingAxisYSecondary() > 0)
            {
                Axis axisY = GetAxisYFromChart(Chart, AxisTypes.Secondary);
                if (axisY == null)
                {
                    axisY = new Axis() { AxisOrientation = axisYOrientation, AxisType = AxisTypes.Secondary };
                    axisY._isAutoGenerated = true;
                    axisY.Chart = Chart;
                    axisY.AxisOrientation = axisYOrientation;
                    axisY.AxisType = AxisTypes.Secondary;
                    axisY.PlotDetails = this;
                    axisY.AxisRepresentation = AxisRepresentations.AxisY;
                    Chart.InternalAxesY.Add(axisY);
                    Chart.AxesY.Add(axisY);
                }
                else
                {
                    axisY.AxisOrientation = axisYOrientation;
                    axisY.AxisType = AxisTypes.Secondary;
                    axisY.PlotDetails = this;
                    axisY.AxisRepresentation = AxisRepresentations.AxisY;
                }

            }
            else
            {
                Axis axisY = GetAxisYFromChart(Chart, AxisTypes.Secondary);

                if (axisY != null)
                {
                    Chart.InternalAxesY.Remove(axisY);
                    axisY = GetAxisYFromChart(Chart, AxisTypes.Secondary);
                }
            }

            #endregion
        }

        /// <summary>
        /// Create default legends
        /// </summary>
        private void CreateLegends()
        {
            foreach (Legend oldLegends in Chart.Legends)
            {
                oldLegends.Entries.Clear();
                oldLegends.Visual = null;
            }

            List<DataSeries> SeriesToBeShownInLegend;

            if (Chart.InternalSeries.Count > 1)
                SeriesToBeShownInLegend = (from entry in Chart.InternalSeries where entry.Enabled == true && (Boolean)entry.ShowInLegend == true select entry).ToList();
            else
                SeriesToBeShownInLegend = Chart.InternalSeries;

            Legend legend = null;
            if (SeriesToBeShownInLegend.Count > 0)
            {
                //List<DataSeries> SeriesWithNoReferingLegend = (from entry in SeriesToBeShownInLegend where String.IsNullOrEmpty(entry.Legend) select entry).ToList();
                List<DataSeries> SeriesWithReferingLegend = (from entry in SeriesToBeShownInLegend where !String.IsNullOrEmpty(entry.Legend) select entry).ToList();

                //if (SeriesWithNoReferingLegend.Count > 0)
                //{
                //    legend = new Legend() { _isAutoGenerated = true };
                //    legend.Chart = Chart;
                //    Chart.Legends.Add(legend);
                //    legend.SetValue(FrameworkElement.NameProperty, "Legend" + Chart.Legends.IndexOf(legend));

                //    foreach (DataSeries series in SeriesWithNoReferingLegend)
                //    {
                //        series.InternalLegendName = legend.Name;
                //    }
                //}

                if (SeriesWithReferingLegend.Count > 0)
                {
                    foreach (DataSeries series in SeriesWithReferingLegend)
                    {
                        var legends = (from entry in Chart.Legends where entry.Name == series.Legend select entry);

                        if (legends.Count() == 0)
                        {
                            legend = new Legend() { _isAutoGenerated = true };
                            legend.Chart = Chart;
                            legend.SetValue(FrameworkElement.NameProperty, series.Legend);
                            Chart.Legends.Add(legend);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get orientation of Chart
        /// </summary>
        /// <returns>ChartOrientationType</returns>
        private ChartOrientationType GetChartOrientation()
        {
            ChartOrientationType chartOrientation = ChartOrientationType.Undefined;
            foreach (DataSeries dataSeries in Chart.InternalSeries)
            {
                if (chartOrientation == ChartOrientationType.Undefined)
                    chartOrientation = GetChartOrientation(dataSeries.RenderAs);
                else if (chartOrientation == GetChartOrientation(dataSeries.RenderAs))
                    continue;
                else
                    throw new Exception("Invalid chart combination");
            }

            return chartOrientation;
        }

        /// <summary>
        /// Creates and updates the plot groups
        /// </summary>
        private void PopulatePlotGroups()
        {
            // Creates any required legends
            CreateLegends();

            // From the series generate groups based on RenderAs, AxisXType,AxisYType
            var plotGroupsData = (from dataSeries in Chart.InternalSeries
                                  group dataSeries by
                                      new
                                      {
                                          dataSeries.RenderAs,
                                          dataSeries.AxisXType,
                                          dataSeries.AxisYType
                                      });

            // Populate the plot groups by checking for validity of charts
            foreach (var plotGroup in plotGroupsData)
            {
                // Get the overall orientation of the chart
                ChartOrientationType plotGroupChartOrientation = GetChartOrientation(plotGroup.Key.RenderAs);

                // Retrieve the reference for the axisX and AxisY
                Axis axisX = GetAxisXFromChart(Chart, plotGroup.Key.AxisXType);
                Axis axisY = GetAxisYFromChart(Chart, plotGroup.Key.AxisYType);

                // Perfrom tasks based on orientation setting
                if (ChartOrientation == ChartOrientationType.Undefined)
                {
                    // if orientation is not set then set it first
                    ChartOrientation = plotGroupChartOrientation;

                    // create and update a plot group and add to the plot group list
                    AddToPlotGroupsList(plotGroup.Key.RenderAs, axisX, axisY, plotGroup.ToList());
                }
                else if (ChartOrientation == plotGroupChartOrientation)
                {
                    // if orientation is already set and the current group also is of the same orientation then
                    // create and update a plot group and add to the plot group list
                    AddToPlotGroupsList(plotGroup.Key.RenderAs, axisX, axisY, plotGroup.ToList());
                }
                else
                {   
                    // if the chart orientation do not match then assert.
                    Debug.Assert(false, "Invalid chart combination. See Documentation for Combination Charts.");
                    throw new Exception("Invalid chart combination");
                }
            }

            // Select DataSeries and group them by RenderAs type
            var seriesGroupByRenderAs = (from series in Chart.InternalSeries group series by series.RenderAs);

            // Apply sibling count based on the chart types
            foreach (var seriesGroup in seriesGroupByRenderAs)
            {
                // Convert the group to a list
                List<DataSeries> seriesList = seriesGroup.ToList();

                // Get the number of series within the list
                Int32 seriesCount = seriesList.Count;

                // Set the count as series count for all series within the list
                seriesList.ForEach(delegate(DataSeries dataSeries) { dataSeries.SeriesCountOfSameRenderAs = seriesCount; });
            }

            // If the chart contains charts of type Bar, StackedBar, StackedBar100
            if (ChartOrientation == ChartOrientationType.Horizontal)
            {
                // Get the count of number of series with RenderAs Bar
                Int32 countOfBarCharts = GetSeriesCountByRenderAs(RenderAs.Bar);

                // Get the count of number of plot groups with RenderAs type as StackedBar
                Int32 countOfStackedBarGroups = GetPlotGroupCountByRenderAs(RenderAs.StackedBar);

                // Get the count of number of plot groups with RenderAs type as StackedBar100
                Int32 countOfStackedBar100Groups = GetPlotGroupCountByRenderAs(RenderAs.StackedBar100);

                // Set a count of siblings by selecting the maximum out of the 3 counts
                DrawingDivisionFactor = Math.Max(countOfBarCharts, Math.Max(countOfStackedBarGroups, countOfStackedBar100Groups));
            }
            else if (ChartOrientation == ChartOrientationType.Vertical)
            {
                // If the chart contains any chart other than bar type or pie or doughnut

                // Get the count of number of series with RenderAs Column
                Int32 countOfColumnCharts = GetSeriesCountByRenderAs(RenderAs.Column);

                // Get the count of number of plot groups with RenderAs type as StackedColumn
                Int32 countOfStackedColumnGroups = GetPlotGroupCountByRenderAs(RenderAs.StackedColumn);

                // Get the count of number of plot groups with RenderAs type as StackedColumn100
                Int32 countOfStackedColumn100Groups = GetPlotGroupCountByRenderAs(RenderAs.StackedColumn100);

                Int32 countOfStockChart = GetPlotGroupCountByRenderAs(RenderAs.Stock);
                if (countOfStockChart > 1)
                    countOfStockChart = 1;

                Int32 countOfCandleStickCharts = GetPlotGroupCountByRenderAs(RenderAs.CandleStick);
                if (countOfCandleStickCharts > 1)
                    countOfCandleStickCharts = 1;

                countOfStockChart = (countOfStockChart == 0) ? countOfCandleStickCharts : countOfStockChart;

                // Set a count of siblings by selecting the maximum out of the 3 counts
                DrawingDivisionFactor = Math.Max(countOfColumnCharts, Math.Max(countOfStackedColumnGroups, countOfStackedColumn100Groups));

                DrawingDivisionFactor = Math.Max(DrawingDivisionFactor, countOfStockChart);
            }
            else if (ChartOrientation == ChartOrientationType.NoAxis)
            {
                // if chart type is NoAxis then set sibling count as zero
                DrawingDivisionFactor = 0;
            }
        }

        /// <summary>
        /// Creates and adds a PlotGroup to the PlotGroups list
        /// </summary>
        /// <param name="renderAs">RenderAs type of the PlotGroup</param>
        /// <param name="axisX">axisX reference for the PlotGroup</param>
        /// <param name="axisY">AxisY reference for the PlotGroup</param>
        /// <param name="series">List of DataSeries belonging to the PlotGroup</param>
        private void AddToPlotGroupsList(RenderAs renderAs, Axis axisX, Axis axisY, List<DataSeries> series)
        {
            // Create a new PlotGroup
            PlotGroup plotGroupEntry = new PlotGroup(renderAs, axisX, axisY);

            // Assign the series list to the PlotGroup
            plotGroupEntry.DataSeriesList = series;

            // refresh or update the PlotGroup details
            plotGroupEntry.Update();

            // Add the PlotGroup the PlotGroups
            PlotGroups.Add(plotGroupEntry);
        }

        /// <summary>
        /// Converts RenderAs to chart orientation type
        /// </summary>
        /// <param name="renderAs"></param>
        /// <returns></returns>
        private ChartOrientationType GetChartOrientation(RenderAs renderAs)
        {
            ChartOrientationType chartOrientation;
            switch (renderAs)
            {
                case RenderAs.Area:
                case RenderAs.Bubble:
                case RenderAs.Column:
                case RenderAs.Line:
                case RenderAs.Stock:
                case RenderAs.CandleStick:
                case RenderAs.Point:
                case RenderAs.StackedArea:
                case RenderAs.StackedColumn:
                case RenderAs.StackedArea100:
                case RenderAs.StackedColumn100:
                    chartOrientation = ChartOrientationType.Vertical;
                    break;

                case RenderAs.Bar:
                case RenderAs.StackedBar:
                case RenderAs.StackedBar100:
                    chartOrientation = ChartOrientationType.Horizontal;
                    break;

                case RenderAs.Pie:
                case RenderAs.Doughnut:
                case RenderAs.SectionFunnel:
                case RenderAs.StreamLineFunnel:
                    chartOrientation = ChartOrientationType.NoAxis;
                    break;

                default:
                    chartOrientation = ChartOrientationType.Undefined;
                    break;
            }

            return chartOrientation;
        }

        /// <summary>
        /// Returns the axis reference for a axis by selecting it from the axis collection in the chart
        /// </summary>
        /// <param name="chart">Reference to the chart</param>
        /// <param name="axisType">Axis type to be selected</param>
        /// <returns>Axis</returns>
        internal Axis GetAxisXFromChart(Chart chart, AxisTypes axisType)
        {
            var axisList = from axis in chart.InternalAxesX where axis.AxisType == axisType select axis;
            if (axisList.Count() > 0)
                return (axisList).Last();
            else return null;
        }

        /// <summary>
        /// Returns the axis reference for a axis by selecting it from the axis collection in the chart
        /// </summary>
        /// <param name="chart">Reference to the chart</param>
        /// <param name="axisType">Axis type to be selected</param>
        /// <returns>Axis</returns>
        internal Axis GetAxisYFromChart(Chart chart, AxisTypes axisType)
        {
            var axisList = from axis in chart.InternalAxesY where axis.AxisType == axisType select axis;

            if (axisList.Count() > 0)
                return (axisList).Last();
            else return null;
        }

        /// <summary>
        /// Sets an incremental ZIndex to the series
        /// </summary>
        private void SetIncrementalZIndexForSeries()
        {
            Int32 index =0;

            foreach (DataSeries dataSeries in Chart.InternalSeries)
            {
                dataSeries.InternalZIndex = dataSeries.InternalZIndex - Chart.InternalSeries.Count;
                dataSeries.InternalZIndex += index++;
            }
        }

        /// <summary>
        /// Calculates and returns the drawing order for all the series for a given chart
        /// </summary>
        /// <returns>A dictionary containing DataSeries as key and its corresponding drawing index</returns>
        private Dictionary<DataSeries, Int32> GenerateDrawingOrder()
        {
            // set an incremental ZIndex levels for all series
            SetIncrementalZIndexForSeries();

            // These charts will be drawn in the same plane hence the depth for each chart type increases by one
            Int32 layer3DCount = 0;
            layer3DCount += (GetSeriesCountByRenderAs(RenderAs.Column) > 0) ? 1 : 0;
            layer3DCount += (GetSeriesCountByRenderAs(RenderAs.StackedColumn) > 0) ? 1 : 0;
            layer3DCount += (GetSeriesCountByRenderAs(RenderAs.StackedColumn100) > 0) ? 1 : 0;
            layer3DCount += (GetSeriesCountByRenderAs(RenderAs.Bar) > 0) ? 1 : 0;
            layer3DCount += (GetSeriesCountByRenderAs(RenderAs.StackedBar) > 0) ? 1 : 0;
            layer3DCount += (GetSeriesCountByRenderAs(RenderAs.StackedBar100) > 0) ? 1 : 0;
            //layer3DCount += (GetSeriesCountByRenderAs(RenderAs.Bubble) > 0) ? 1 : 0;

            // Each Area chart has to be drawn in a different plane hence the depth incereases 
            // by the amount equal to the number of area charts
            layer3DCount += GetSeriesCountByRenderAs(RenderAs.Area);
            //layer3DCount += GetSeriesCountByRenderAs(RenderAs.Line);
            //layer3DCount += GetSeriesCountByRenderAs(RenderAs.Point);
            //layer3DCount += GetSeriesCountByRenderAs(RenderAs.Bubble);

            // These charts are drawn on a different plane for each plot group hence the depth increases 
            // by the amount equal to the number of plots for these charts
            layer3DCount += GetPlotGroupCountByRenderAs(RenderAs.StackedArea);
            layer3DCount += GetPlotGroupCountByRenderAs(RenderAs.StackedArea100);

            // Set the depth factor as the depth count
            Layer3DCount = layer3DCount;
            
            // Functions to select the key and value from the grouped list obtained from LINQ.
            Func<IGrouping<DataSeries, Int32>, DataSeries> KeySelector = delegate(IGrouping<DataSeries, Int32> entry) { return entry.Key; };
            Func<IGrouping<DataSeries, Int32>, Int32> ElementSelector = delegate(IGrouping<DataSeries, Int32> entry) { return entry.Last(); };

            // create a dictionary of seriesIndex by converting the result into a dictionary
            Dictionary<DataSeries, Int32> sortedSeriesIndexGroupedBySeries = (from series in Chart.InternalSeries orderby series.InternalZIndex group series.InternalZIndex by series).ToDictionary(KeySelector, ElementSelector);

            // Generate index for each chart type
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.Column, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedColumn, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedColumn100, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.Bar, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedBar, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedBar100, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.Line, sortedSeriesIndexGroupedBySeries);

            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.Point, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.Stock, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.CandleStick, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.Bubble, sortedSeriesIndexGroupedBySeries);

            // Generate index for each chart type based on the AxisXType and AxisYType
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea, AxisTypes.Primary, AxisTypes.Primary, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea, AxisTypes.Primary, AxisTypes.Secondary, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea, AxisTypes.Secondary, AxisTypes.Primary, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea, AxisTypes.Secondary, AxisTypes.Secondary, sortedSeriesIndexGroupedBySeries);
            
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea100, AxisTypes.Primary, AxisTypes.Primary, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea100, AxisTypes.Primary, AxisTypes.Secondary, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea100, AxisTypes.Secondary, AxisTypes.Primary, sortedSeriesIndexGroupedBySeries);
            sortedSeriesIndexGroupedBySeries = GenerateIndexByRenderAs(RenderAs.StackedArea100, AxisTypes.Secondary, AxisTypes.Secondary, sortedSeriesIndexGroupedBySeries);
            
            // create a List out of the result obtained by sorting the seriesIndex by ZIndex value
            List<KeyValuePair<DataSeries, Int32>> seriesIndexList = (from entry in sortedSeriesIndexGroupedBySeries orderby entry.Value select entry).ToList();

            // Temporary list to swap lists
            List<KeyValuePair<DataSeries, Int32>> seriesIndexTempList = new List<KeyValuePair<DataSeries, Int32>>();

            // This list will accumilte the final index for each DataSeries
            List<KeyValuePair<DataSeries, Int32>> seriesIndexFinal = new List<KeyValuePair<DataSeries, Int32>>();
            Int32 drawingIndex = 0;     // Decides which chart shoud come in front (rendering will be done from series with lowest index )
            Int32 lowestIndex;          // used to get the lowest index for a chart type
            Boolean ignore = false;     // is used to indicate whether the series has any affect on index or not

            // This is a array of ignorable render as types while calculating drawing index
            RenderAs[] ignorableCharts = { RenderAs.Line, RenderAs.Point, RenderAs.Bubble, RenderAs.Stock, RenderAs.CandleStick };

            // repeat the loop until the seriesIndexList becomes empty
            while (seriesIndexList.Count > 0)
            {
                // Do not ignore any series type ( this will change based on chart type later )
                ignore = false;

                // Get the lowest seriesIndex from the seriesIndexList
                lowestIndex = (from series in seriesIndexList select series.Value).Min();

                // Select all series with the lowest index
                var seriesWithLowestIndex = from series in seriesIndexList where series.Value == lowestIndex select series.Key;

                // Insert the series with lowest index into the final index list, Update the index by setting it to the drawingIndex
                seriesIndexFinal.InsertRange(seriesIndexFinal.Count, (from series in seriesWithLowestIndex select new KeyValuePair<DataSeries, Int32>(series, drawingIndex)).ToList());

                // Get the count of series with RenderAs value in the ignorable charts list
                Int32 ignorableChartCount = (from series in seriesWithLowestIndex where ignorableCharts.Contains(series.RenderAs) select series.RenderAs).Count();

                // If the count of ignorable charts is gretaer than 0 then set ignore flag to true
                ignore = (ignorableChartCount > 0) ? true : false;

                // Get a list of series which are not same as the lowest index
                var seriesWithoutLowestIndex = from series in seriesIndexList where series.Value != lowestIndex select series;

                // Generate a list of series whose index is not same as the lowest index
                seriesIndexTempList = seriesWithoutLowestIndex.ToList();

                // Clear the list so that a updated list can be assigned to it
                seriesIndexList.Clear();

                // Assign the reference of the updated list to the setiesIndex
                seriesIndexList = seriesIndexTempList;

                // do not increase the drawing index
                if (!ignore)
                    drawingIndex++;
            }

            // Functions to select the key and value from the grouped list obtained from LINQ.
            Func<KeyValuePair<DataSeries, Int32>, DataSeries> KeySelector2 = delegate(KeyValuePair<DataSeries, Int32> entry) { return entry.Key; };
            Func<KeyValuePair<DataSeries, Int32>, Int32> ElementSelector2 = delegate(KeyValuePair<DataSeries, Int32> entry) { return entry.Value; };

            // Generate a dictionary from the result obtained from the LINQ.
            sortedSeriesIndexGroupedBySeries = seriesIndexFinal.ToDictionary(KeySelector2, ElementSelector2);

            // Return the generated dictionary 
            return sortedSeriesIndexGroupedBySeries;
        }

        /// <summary>
        /// From a given list all series with a particular RenderAs type are selected.
        /// From such a list the largest index is obtainded. This index is applied to all series with same RenderAs type
        /// </summary>
        /// <param name="renderAs">RenderAs type for selecting the series</param>
        /// <param name="seriesIndexDictionary">List which will be used to select the series</param>
        /// <returns>Return the updated list</returns>
        private Dictionary<DataSeries, Int32> GenerateIndexByRenderAs(RenderAs renderAs, Dictionary<DataSeries, Int32> seriesIndexDictionary)
        {
            // Get all series of a particular render as type
            var seriesByRenderAs = from entry in seriesIndexDictionary where entry.Key.RenderAs == renderAs select entry;

            // If there atleast one series in the list then continue to update the list
            if (seriesByRenderAs.Count() > 0)
            {
                // Get the highest index fron the seriesByRenderAs list
                Int32 highestIndex = (from entry in seriesByRenderAs select entry.Value).Max();

                // Convert list to array
                KeyValuePair<DataSeries, Int32>[] seriesArray = seriesByRenderAs.ToArray();

                // Update the series index
                for (Int32 i = 0; i < seriesArray.Length; i++)
                    seriesIndexDictionary[seriesArray[i].Key] = highestIndex;
            }

            // Return the updated list
            return seriesIndexDictionary;
        }

        /// <summary>
        /// From a given list all series with a particular RenderAs type and same reference to axisX and axisY are selected.
        /// From such a list the largest index is obtainded. This index is applied to all series with same RenderAs type
        /// </summary>
        /// <param name="renderAs">RenderAs type for selecting the series</param>
        /// <param name="axisXType">Reference to the axisX</param>
        /// <param name="axisYType">Reference to the axisY</param>
        /// <param name="seriesIndexDictionary">List which will be used to select the series</param>
        /// <returns>Return the updated list</returns>
        private Dictionary<DataSeries, Int32> GenerateIndexByRenderAs(RenderAs renderAs, AxisTypes axisXType, AxisTypes axisYType, Dictionary<DataSeries, Int32> seriesIndexDictionary)
        {
            // Get all series of a particular render as typeand same reference to axisX and axisY
            var seriesByRenderAs = from entry in seriesIndexDictionary
                                   where entry.Key.RenderAs == renderAs && entry.Key.AxisXType == axisXType && entry.Key.AxisYType == axisYType
                                   select entry;

            // If there atleast one series in the list then continue to update the list
            if (seriesByRenderAs.Count() > 0)
            {
                // Get the highest index fron the seriesByRenderAs list
                Int32 highestIndex = (from entry in seriesByRenderAs select entry.Value).Max();

                // Convert list to array
                KeyValuePair<DataSeries, Int32>[] seriesArray = seriesByRenderAs.ToArray();

                // Update the series index
                for (Int32 i = 0; i < seriesArray.Length; i++)
                    seriesIndexDictionary[seriesArray[i].Key] = highestIndex;
            }

            // Return the updated list
            return seriesIndexDictionary;
        }

        /// <summary>
        /// Returns a count of DataSeries for a particulat render as type
        /// </summary>
        internal Int32 GetSeriesCountByRenderAs(RenderAs renderAs)
        {
            return (from series in Chart.InternalSeries where series.RenderAs == renderAs select series).Count();
        }

        /// <summary>
        /// Returns a count of DataSeries for a particulat render as type
        /// </summary>
        internal Int32 GetPlotGroupCountByRenderAs(RenderAs renderAs)
        {
            return (from plotGroup in PlotGroups where plotGroup.RenderAs == renderAs && plotGroup.IsEnabled select plotGroup).Count();
        }

        /// <summary>
        /// Create list of datapoints from all series
        /// </summary>
        private void CreateListOfDataPoints()
        {
            // List of all datapoints in the chart
            ListOfAllDataPoints = new List<DataPoint>();

            // Populates the list with InternalDataPoints with all availabel InternalDataPoints from all DataSeries
            foreach (DataSeries dataSeries in Chart.InternalSeries)
            {
                // Concatinate the lists of InternalDataPoints if the axis type matches
                if (dataSeries.Enabled == true)
                {
                    List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.InternalDataPoints select datapoint).ToList(); //where datapoint.Enabled == true 

                    ListOfAllDataPoints.InsertRange(ListOfAllDataPoints.Count, enabledDataPoints);
                }
            }
        }

        /// <summary>
        /// Generates AxisLabels for this PlotGroup and returns a dictionary
        /// that holds InternalXValue as key, AxisLabel as value
        /// </summary>
        private Dictionary<Double, String> GetAxisXLabels(AxisTypes axisXType)
        {
            // List of all datapoints in the chart
            List<DataPoint> listOfAllDataPoints = (from dataPoint in ListOfAllDataPoints where dataPoint.Parent.AxisXType == axisXType select dataPoint).ToList();


            //// Populates the list with InternalDataPoints with all availabel InternalDataPoints from all DataSeries
            //foreach (DataSeries dataSeries in Chart.InternalSeries)
            //{
            //    // Concatinate the lists of InternalDataPoints if the axis type matches
            //    if (dataSeries.AxisXType == axisXType && dataSeries.Enabled == true)
            //    {
            //        List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.InternalDataPoints select datapoint).ToList(); //where datapoint.Enabled == true 

            //        listOfAllDataPoints.InsertRange(listOfAllDataPoints.Count, enabledDataPoints);
            //    }
            //}

            // Contains a table which hold unique XValues and all the Axis Labels availabel for each XVAlue
            var uniqueXValueDataPoints = (from dataPoint in listOfAllDataPoints where !String.IsNullOrEmpty(dataPoint.AxisXLabel) orderby dataPoint.InternalXValue group dataPoint.AxisXLabel by dataPoint.InternalXValue);

            // A function to select a appropriate key for creating the final dictionary
            Func<IGrouping<Double, String>, Double> GetXValue = delegate(IGrouping<Double, String> entry) { return entry.Key; };

            // A function to get the last axis label for a particular InternalXValue for an available set of axis labels
            Func<IGrouping<Double, String>, String> GetAxisLabel = delegate(IGrouping<Double, String> entry) { return entry.Last(); };

            // Generates the dictionary with InternalXValue as key, AxisLabel as value
            return uniqueXValueDataPoints.ToDictionary(GetXValue, GetAxisLabel);
        }


        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the maximum data value for axisX, no need to check primary or secondary
        /// </summary>
        /// <returns>Returns the maximum data value as Double</returns>
        internal Double GetAxisXMaximumDataValue(Axis axisX)
        {
            return (from plotData in PlotGroups
                    where (!Double.IsNaN(plotData.MaximumX) && plotData.AxisX == axisX)
                    select plotData.MaximumX).Max();
        }

        /// <summary>
        /// Returns the maximum data value for AxisY, no need to check primary or secondary
        /// </summary>
        /// <param name="axisY"></param>
        /// <returns>Returns the maximum data value as Double</returns>
        internal Double GetAxisYMaximumDataValue(Axis axisY)
        {
            return (from plotData in PlotGroups
                    where (!Double.IsNaN(plotData.MaximumY) && plotData.AxisY == axisY)
                    select plotData.MaximumY).Max();
        }

        /// <summary>
        /// Returns the minimum data value for axisX, no need to check primary or secondary
        /// </summary>
        /// <param name="axisX"></param>
        /// <returns>Returns the minimum data value as Double</returns>
        internal Double GetAxisXMinimumDataValue(Axis axisX)
        {
            return (from plotData in PlotGroups
                    where (!Double.IsNaN(plotData.MinimumX) && plotData.AxisX == axisX)
                    select plotData.MinimumX).Min();
        }

        /// <summary>
        /// Returns the minimum data value for AxisY, no need to check primary or secondary
        /// </summary>
        /// <param name="axisY"></param>
        /// <returns>Returns the minimum data value as Double</returns>
        internal Double GetAxisYMinimumDataValue(Axis axisY)
        {
            return (from plotData in PlotGroups
                    where (!Double.IsNaN(plotData.MinimumY) && plotData.AxisY == axisY)
                    select plotData.MinimumY).Min();
        }

        /// <summary>
        /// Returns the maximum ZValue
        /// </summary>
        /// <returns>Returns the maximum ZValue as Double</returns>
        internal Double GetMaximumZValue()
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinimumZ)
                    select plotData.MaximumZ).Max();
        }


        /// <summary>
        /// Returns the minimum ZValue
        /// </summary>
        /// <returns>Returns the minimum ZValue as Double</returns>
        internal Double GetMinimumZValue()
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinimumZ)
                    select plotData.MinimumZ).Min();
        }

        /// <summary>
        /// Returns the maximum value from a given set of minimum differences
        /// </summary>
        /// <returns>Double</returns>
        internal Double GetMaxOfMinDifferencesForXValue()
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinDifferenceX)
                    select plotData.MinDifferenceX).Max();
        }

        /// <summary>
        /// Returns the maximum value from a given set of minimum differences by renderas type
        /// </summary>
        /// <returns>Double</returns>
        internal Double GetMaxOfMinDifferencesForXValue(RenderAs renderAs)
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinDifferenceX) && plotData.RenderAs == renderAs
                    select plotData.MinDifferenceX).Max();
        }

        /// <summary>
        /// Returns the minimum value from a given set of minimum differences
        /// </summary>
        /// <returns>Double</returns>
        internal Double GetMinOfMinDifferencesForXValue()
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinDifferenceX)
                    
                    select plotData.MinDifferenceX).Min();
        }

        /// <summary>
        /// Returns the minimum value from a given set of minimum differences by render as type
        /// </summary>
        /// <param name="renderAs">RenderAs</param>
        /// <returns>Double</returns>
        internal Double GetMinOfMinDifferencesForXValue(RenderAs renderAs)
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinDifferenceX) && plotData.RenderAs == renderAs
                    select plotData.MinDifferenceX).Min();
        }

        /// <summary>
        /// Returns the minimum value from a given set of minimum differences by render as types
        /// </summary>
        /// <param name="renderAs">RenderAs</param>
        /// <returns>Double</returns>
        internal Double GetMinOfMinDifferencesForXValue(params RenderAs[] renderAs)
        {
            return (from plotData in PlotGroups
                    where !Double.IsNaN(plotData.MinDifferenceX) && renderAs.Contains(plotData.RenderAs)
                    select plotData.MinDifferenceX).Min();
        }

        /// <summary>
        /// Returns a series list based on the render as type
        /// </summary>
        /// <param name="renderAs">RenderAs</param>
        /// <returns>List of dataseries</returns>
        internal List<DataSeries> GetSeriesListByRenderAs(RenderAs renderAs)
        {
            return (from series in Chart.InternalSeries where series.Enabled == true && series.RenderAs == renderAs select series).ToList();
        }

        /// <summary>
        /// Returns series count using secondary axis-x
        /// </summary>
        /// <returns>Series count</returns>
        internal Int32 GetCountOfSeriesUsingAxisXSecondary()
        {
            return (from series in Chart.InternalSeries where series.AxisXType == AxisTypes.Secondary select series).Count();
        }

        /// <summary>
        /// Get count of series using primary axis-x
        /// </summary>
        /// <returns>Series count</returns>
        internal Int32 GetCountOfSeriesUsingAxisXPrimary()
        {
            return (from series in Chart.InternalSeries where series.AxisXType == AxisTypes.Primary select series).Count();
        }

        /// <summary>
        /// Return series count using secondary axis-y
        /// </summary>
        /// <returns>Series count</returns>
        internal Int32 GetCountOfSeriesUsingAxisYSecondary()
        {
            return (from series in Chart.InternalSeries where series.AxisYType == AxisTypes.Secondary select series).Count();
        }

        /// <summary>
        /// Returns series count using primary axis-y 
        /// </summary>
        /// <returns>Series count</returns>
        internal Int32 GetCountOfSeriesUsingAxisYPrimary()
        {
            return (from series in Chart.InternalSeries where series.AxisYType == AxisTypes.Primary select series).Count();
        }

        /// <summary>
        /// Returns true if series count is equal to no of dataSeries having renderAs type StackedArea100 
        /// or StackedBar100 or StackedColumn100, else false
        /// </summary>
        /// <returns>Boolean</returns>
        internal Boolean GetStacked100OverrideState()
        {
            RenderAs[] stacked100Types = { RenderAs.StackedArea100, RenderAs.StackedBar100, RenderAs.StackedColumn100 };

            Int32 countOfStack100Types = (from series in Chart.InternalSeries where stacked100Types.Contains(series.RenderAs) select series).Count();

            return (countOfStack100Types == Chart.InternalSeries.Count());

        }

        /// <summary>
        /// Returns datapoints grouped by InternalXValue
        /// </summary>
        /// <param name="renderAs">RenderAs</param>
        /// <returns>Dictionary[Double, SortedDataPoints]</returns>
        internal Dictionary<Double, SortDataPoints> GetDataPointsGroupedByXValue(RenderAs renderAs)
        {
            List<PlotGroup> selectedGroup = (from plotGroup in PlotGroups where plotGroup.RenderAs == renderAs select plotGroup).ToList();

            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (PlotGroup plotGroup in selectedGroup)
            {
                foreach (DataSeries dataSeries in plotGroup.DataSeriesList)
                {
                    if (dataSeries.Enabled == true)
                    {
                        List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.InternalDataPoints where datapoint.Enabled == true select datapoint).ToList();

                        dataPoints.InsertRange(dataPoints.Count, enabledDataPoints);
                    }
                }
            }

            var dataPointsGroupedByXValues = (from datapoint in dataPoints group datapoint by datapoint.InternalXValue);

            Dictionary<Double, SortDataPoints> entries = new Dictionary<Double, SortDataPoints>();

            foreach (var groupEntry in dataPointsGroupedByXValues)
            {
                List<DataPoint> positiveDataPoints = (from data in groupEntry where data.InternalYValue > 0 select data).ToList();
                List<DataPoint> negativeDataPoints = (from data in groupEntry where data.InternalYValue <= 0 select data).ToList();

                entries.Add(groupEntry.Key, new SortDataPoints(positiveDataPoints, negativeDataPoints));
            }

            return entries;
        }

        /// <summary>
        /// Returns datapoints grouped by InternalXValue
        /// </summary>
        /// <param name="renderAs">dataseries renderas type</param>
        /// <param name="axisX">axis-x</param>
        /// <param name="axisY">axis-y</param>
        /// <returns>Returns datapoints grouped by InternalXValue</returns>
        internal Dictionary<Double, SortDataPoints> GetDataPointsGroupedByXValue(RenderAs renderAs, Axis axisX, Axis axisY)
        {
            List<PlotGroup> selectedGroup = (from plotGroup in PlotGroups where plotGroup.RenderAs == renderAs && plotGroup.AxisX == axisX && plotGroup.AxisY == axisY select plotGroup).ToList();

            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (PlotGroup plotGroup in selectedGroup)
            {
                foreach (DataSeries dataSeries in plotGroup.DataSeriesList)
                {
                    if (dataSeries.Enabled == true)
                    {
                        List<DataPoint> enabledDataPoints = (from datapoint in dataSeries.InternalDataPoints where datapoint.Enabled == true select datapoint).ToList();

                        dataPoints.InsertRange(dataPoints.Count, enabledDataPoints);
                    }
                }
            }

            var dataPointsGroupedByXValues = (from datapoint in dataPoints group datapoint by datapoint.InternalXValue);

            Dictionary<Double, SortDataPoints> entries = new Dictionary<Double, SortDataPoints>();

            foreach (var groupEntry in dataPointsGroupedByXValues)
            {
                List<DataPoint> positiveDataPoints = (from data in groupEntry where data.InternalYValue > 0 select data).ToList();
                List<DataPoint> negativeDataPoints = (from data in groupEntry where data.InternalYValue <= 0 select data).ToList();

                entries.Add(groupEntry.Key, new SortDataPoints(positiveDataPoints, negativeDataPoints));
            }

            return entries;
        }

        /// <summary>
        /// Returns minimum interval of axis-x
        /// </summary>
        /// <returns>Axis</returns>
        internal Axis GetAxisXMinimumInterval()
        {
            Double minInterval = (Double)(from axis in Chart.InternalAxesX select axis.InternalInterval).Min();

            return (from axis in Chart.InternalAxesX where axis.InternalInterval == minInterval select axis).First();

        }

        /// <summary>
        /// Returns division factor (Number of distinct parent) from sorted datapoints set
        /// </summary>
        /// <param name="sortedSet">SortedDataPoints</param>
        /// <returns>Int32</returns>
        internal Int32 GetDivisionFactor(SortDataPoints sortedSet)
        {
            List<DataPoint> lists = new List<DataPoint>();

            lists.InsertRange(lists.Count, sortedSet.Positive);
            lists.InsertRange(lists.Count, sortedSet.Negative);

            // Number of distinct parent 
            return (from entry in lists group entry by entry.Parent).Count();
        }

        /// <summary>
        /// Returns dataseries list from sorted datapoints
        /// </summary>
        /// <param name="sortedSet">SortedDataPoints</param>
        /// <returns>List of dataseries</returns>
        internal List<DataSeries> GetSeriesFromSortedPoints(SortDataPoints sortedSet)
        {
            List<DataPoint> lists = new List<DataPoint>();

            lists.InsertRange(lists.Count, sortedSet.Positive);
            lists.InsertRange(lists.Count, sortedSet.Negative);

            return (from entry in lists select entry.Parent).Distinct().ToList();
        }

        /// <summary>
        /// Returns the list of Series belonging to a DataPoint value
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <returns>List of dataseries</returns>
        internal List<DataSeries> GetSeriesFromDataPoint(DataPoint dataPoint)
        {
            List<DataSeries> lists = new List<DataSeries>();

            //Boolean IsDataSeriesExist = false;
            foreach (DataSeries ds in Chart.InternalSeries)
            {
                if (ds.RenderAs == dataPoint.Parent.RenderAs && ds.Enabled == true)
                {
                    //foreach (DataPoint dp in ds.InternalDataPoints)
                    //{
                    //    if (dp.InternalXValue == dataPoint.InternalXValue)
                    //    {
                    //        IsDataSeriesExist = true;
                    //        break;
                    //    }
                    //    else
                    //        IsDataSeriesExist = false;
                    //}

                    if (ds.InternalDataPoints.Count > 0)
                        lists.Add(ds);
                }
            }

            return lists;
        }

        /// <summary>
        /// Returns max division factor, 
        /// Where division factor is the number of distinct parent(dataseries) from sorted datapoints set
        /// </summary>
        /// <param name="sortedDataPointList">Dictionary of SortedDataPoints</param>
        /// <returns>Int32</returns>
        internal Int32 GetMaxDivision(Dictionary<Double, SortDataPoints> sortedDataPointList)
        {
            List<Double> values = sortedDataPointList.Keys.ToList();

            Int32 factor = 0;

            foreach (Double value in values)
            {
                factor = Math.Max(factor, GetDivisionFactor(sortedDataPointList[value]));
            }

            return factor;
        }

        /// <summary>
        /// Returns absolute sum of datapoints
        /// </summary>
        /// <param name="dataPoints">List of datapoints</param>
        /// <returns>Double</returns>
        internal Double GetAbsoluteSumOfDataPoints(List<DataPoint> dataPoints)
        {
            if (dataPoints.Count > 0 && (dataPoints[0].Parent.RenderAs == RenderAs.SectionFunnel || dataPoints[0].Parent.RenderAs == RenderAs.StreamLineFunnel))
                return (from dataPoint in dataPoints where !Double.IsNaN(dataPoint.InternalYValue) && dataPoint.InternalYValue >=0 select Math.Abs(dataPoint.InternalYValue)).Sum();
            else
                return (from dataPoint in dataPoints where !Double.IsNaN(dataPoint.InternalYValue) select Math.Abs(dataPoint.InternalYValue)).Sum();
        }

        /// <summary>
        /// Returns datapoint value in stacked order (positive to negative order)
        /// </summary>
        /// <param name="plotGroup">PlotGroup</param>
        /// <returns>Dictionary[Double, List[Double]] </returns>
        internal Dictionary<Double, List<Double>> GetDataPointValuesInStackedOrder(PlotGroup plotGroup)
        {
            Double[] xValues = plotGroup.XWiseStackedDataList.Keys.ToArray();
            Array.Sort(xValues);

            Dictionary<Double, List<Double>> dataPointsInStackOrder = new Dictionary<Double, List<Double>>();

            for (Int32 i = 0; i < xValues.Length; i++)
            {
                var yValuePositive = (from entry in plotGroup.XWiseStackedDataList[xValues[i]].Positive group entry.InternalYValue by entry.Parent);
                var yValueNegative = (from entry in plotGroup.XWiseStackedDataList[xValues[i]].Negative group entry.InternalYValue by entry.Parent);

                Double[] indexedYValues = new Double[yValuePositive.Count() + yValueNegative.Count()];
                for (Int32 index = 0; index < indexedYValues.Length; index++)
                    indexedYValues[index] = 0;

                foreach (var entry in yValuePositive)
                {
                    Int32 index = plotGroup.DataSeriesList.IndexOf(entry.Key);

                    if (index < indexedYValues.Count())
                        indexedYValues[index] += entry.Sum();
                }

                foreach (var entry in yValueNegative)
                {
                    Int32 index = plotGroup.DataSeriesList.IndexOf(entry.Key);

                    if (index < indexedYValues.Count())
                        indexedYValues[index] += entry.Sum();
                }


                dataPointsInStackOrder.Add(xValues[i], indexedYValues.ToList());
            }

            return dataPointsInStackOrder;
        }

        /// <summary>
        /// Returns datapoint in stacked order (positive to negative order)
        /// </summary>
        /// <param name="plotGroup">PlotGroup</param>
        /// <returns>Dictionary[Double, List[DataPoint]]</returns>
        internal Dictionary<Double, List<DataPoint>> GetDataPointInStackOrder(PlotGroup plotGroup)
        {
            Double[] xValues = plotGroup.XWiseStackedDataList.Keys.ToArray();
            Array.Sort(xValues);

            Dictionary<Double, List<DataPoint>> dataPointsInStackOrder = new Dictionary<Double, List<DataPoint>>();

            Int32 enabledSeriesCount = (from series in plotGroup.DataSeriesList where series.Enabled == true select series).Count();

            for (Int32 i = 0; i < xValues.Length; i++)
            {
                var yPositiveDataPoints = (from entry in plotGroup.XWiseStackedDataList[xValues[i]].Positive group entry by entry.Parent);
                var yNegativeDataPoints = (from entry in plotGroup.XWiseStackedDataList[xValues[i]].Negative group entry by entry.Parent);

                DataPoint[] indexedDataPoints = new DataPoint[yPositiveDataPoints.Count() + yNegativeDataPoints.Count()];

                foreach (var entry in yPositiveDataPoints)
                {
                    Int32 index = plotGroup.DataSeriesList.IndexOf(entry.Key);

                    if (index < indexedDataPoints.Count())
                        indexedDataPoints[index] = entry.First();
                }

                foreach (var entry in yNegativeDataPoints)
                {
                    Int32 index = plotGroup.DataSeriesList.IndexOf(entry.Key);

                    if (index < indexedDataPoints.Count())
                        indexedDataPoints[index] = entry.First();
                }

                dataPointsInStackOrder.Add(xValues[i], indexedDataPoints.ToList());
            }

            return dataPointsInStackOrder;
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        #endregion
    }
}
