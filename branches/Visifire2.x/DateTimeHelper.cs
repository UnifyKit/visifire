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
using System.Net;
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
    /// Halper class for date and time calculation
    /// </summary>
    /// <summary>
    /// Visifire.Charts.DateTimeHelper class
    /// </summary>
    internal class DateTimeHelper
    {
        #region Public Methods

        #endregion

        #region Public Properties

        /// <summary>
        /// Get actual interval type according to minimum difference between two DataTimes from a sorted list of DateTimes
        /// </summary>
        /// <param name="minDateRange">minimum difference between two DataTimes</param>
        /// <returns>IntervalTypes</returns>
        public static IntervalTypes GetAutoIntervalType(TimeSpan minDateDifference, TimeSpan maxDateDifference, IntervalTypes currentIntervalTypes)
        {
            if (maxDateDifference.Hours != 0)
                return IntervalTypes.Hours;
            else if (maxDateDifference.Minutes != 0)
                return IntervalTypes.Minutes;
            else if (maxDateDifference.Seconds != 0)
                return IntervalTypes.Seconds;
            else if (maxDateDifference.Milliseconds != 0)
                return IntervalTypes.Milliseconds;
            else // if (minDateRange.Days != 0)
                return IntervalTypes.Days;
        }

        /// <summary>
        /// Converts XValue to DateTime
        /// </summary>
        /// <param name="minDate">Min DateTime Value</param>
        /// <param name="XValue">XValue</param>
        /// <param name="intervalTypes">IntervalTypes</param>
        /// <returns>DateTime</returns>
        public static DateTime XValueToDateTime(DateTime minDate, Double XValue, IntervalTypes intervalTypes)
        {
            DateTime returnDate = minDate;
            TimeSpan timespan = new TimeSpan();
            
            switch (intervalTypes)
            {   
                case IntervalTypes.Years:
                    returnDate = minDate.AddMinutes((XValue / 0.000001902587519025875));
                    break;

                case IntervalTypes.Months:
                    returnDate = minDate.AddHours((XValue / 0.00137));
                    break;

                case IntervalTypes.Weeks:
                    returnDate = minDate.AddDays(XValue * 7);
                    break;

                case IntervalTypes.Days:
                    returnDate = minDate.AddDays(XValue);
                    break;

                case IntervalTypes.Hours:
                    returnDate = minDate.AddHours(XValue);
                    break;

                case IntervalTypes.Minutes:
                    returnDate = minDate.AddMinutes(XValue);
                    break;

                case IntervalTypes.Seconds:
                    returnDate = minDate.AddSeconds(XValue);
                    break;

                case IntervalTypes.Milliseconds:
                    returnDate = minDate.AddMilliseconds(XValue);
                    break;
            }

            return returnDate;
        }
        
        /// <summary>
        /// Get Date difference in days
        /// </summary>
        /// <param name="dateTime1">First Date</param>
        /// <param name="dateTime2">Second Date</param>
        /// <param name="minDateRange">Minimum TimeSpan between two dates in list of dates from InternalDataPoints interval</param>
        /// <param name="intervalTypes">Type of interval is applied</param>
        /// <returns>Date Difference</returns>
        public static Double DateDiff(DateTime dateTime1, DateTime dateTime2, TimeSpan minDateDifference, TimeSpan maxDateDifference, IntervalTypes intervalTypes, ChartValueTypes xValueType)
        {
            TimeSpan timespan = dateTime1.Subtract(dateTime2);
            Double retVal = 1;

            CALCULATE_WITH_NEW_INTERVAL_TYPE:

            switch (intervalTypes)
            {   
                case IntervalTypes.Auto:
                    intervalTypes = GetAutoIntervalType(minDateDifference, maxDateDifference, intervalTypes);
                    goto CALCULATE_WITH_NEW_INTERVAL_TYPE;

                case IntervalTypes.Years:

                    // Double noOfYears = (DateTime.IsLeapYear(dateToUpdate.Year) ? 366 : 365) * increment;
                    // return new DateTime(dateToUpdate.Year + (int) increment,1,1,0,0,0,0, DateTimeKind.Unspecified);
                    // 1 minutes (y / year) = 0.000001902587519025875 months (mth)
                    retVal = 0.000001902587519025875 * timespan.TotalMinutes;
                    //retVal = Math.Abs(dateTime1.Date.Year - dateTime2.Date.Year);
                    break;

                case IntervalTypes.Months:

                    //if (xValueType == ChartValueTypes.Date)
                    //    retVal = (dateTime1.Year * 12 + dateTime1.Month) - (dateTime2.Year * 12 + dateTime2.Month);
                    //else
                    {
                        // 1 hour (h / hr) = 0.00 137 months (mth)
                        retVal = 0.00137 * timespan.TotalHours;
                    }

                    break;

                case IntervalTypes.Weeks:
                    retVal = timespan.TotalDays / 7;
                    break;

                case IntervalTypes.Days:
                    retVal = timespan.TotalDays;
                    break;

                case IntervalTypes.Hours:
                    retVal = timespan.TotalHours;
                    break;

                case IntervalTypes.Minutes:
                    retVal = timespan.TotalMinutes;
                    break;

                case IntervalTypes.Seconds:
                    retVal = timespan.TotalSeconds;
                    break;

                case IntervalTypes.Milliseconds:
                    retVal = timespan.TotalMilliseconds;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// Calculate Min Max date from a List of dates
        /// </summary>
        /// <param name="dateTimeList">List of DateTimes</param>
        /// <param name="minDate">out min Date</param>
        /// <param name="maxDate">out Max Date</param>
        /// <param name="minDateRange">Minimum date difference</param>
        /// <param name="maxDateRange">Maximum date difference</param>
        public static void CalculateMinMaxDate(System.Collections.Generic.List<DateTime> dateTimeList, out DateTime minDate, out DateTime maxDate, out TimeSpan minDateRange, out TimeSpan maxDateRange)
        {   
            dateTimeList.Sort();

            minDateRange = new TimeSpan();
            maxDateRange = new TimeSpan();

            minDate = dateTimeList[0];

            if (dateTimeList.Count > 0)
            {
                maxDate = dateTimeList[dateTimeList.Count - 1];

                if (dateTimeList.Count >= 2)
                {
                    minDateRange = dateTimeList[1].Subtract(minDate);
                    maxDateRange = maxDate.Subtract(minDate);
                }
            }
            else
                maxDate = minDate;
        }

        /// <summary>
        /// Recalculates a DateTime interval obtained from maximum and minimum DateTime.
        /// </summary>
        /// <param name="width">Width of available space</param>
        /// <param name="height">Height of available space</param>
        /// <param name="axisOrientation">Axis orientation</param>
        /// <param name="minDateTime">Minimum DateTime value</param>
        /// <param name="maxDateTime">Maximum DateTime Value</param>
        /// <param name="type">Current Interval type</param>
        /// <param name="maxInterval">Max Interval</param>
        /// <returns>Calculated auto interval</returns>
        public static double CalculateAutoInterval(Double width, Double height, Orientation axisOrientation, DateTime minDateTime, DateTime maxDateTime, out IntervalTypes type, Double maxInterval)
        {   
            TimeSpan timeSpan = maxDateTime.Subtract(minDateTime);

            // Algorithm will interval close to 10.
            // We need to align the time span for PrefferedNumberOfIntervals
            double maxIntervals = axisOrientation == Orientation.Horizontal ? maxInterval * 0.8 : maxInterval;
            double rangeMultiplicator = (axisOrientation == Orientation.Horizontal ? width : height) / (200 * 10 / maxIntervals);
            
            timeSpan = new TimeSpan((long)((double)timeSpan.Ticks / rangeMultiplicator));

            // TotalMinutes
            double totalMinutes = timeSpan.TotalMinutes;

            // For Range less than 60 seconds interval is 5 sec
            if (totalMinutes <= 1.0)
            {   
                // Milli Seconds
                double milliSeconds = timeSpan.TotalMilliseconds;
                if (milliSeconds <= 10)
                {
                    type = IntervalTypes.Milliseconds;
                    return 1;
                }
                if (milliSeconds <= 50)
                {
                    type = IntervalTypes.Milliseconds;
                    return 4;
                }
                if (milliSeconds <= 200)
                {
                    type = IntervalTypes.Milliseconds;
                    return 20;
                }
                if (milliSeconds <= 500)
                {
                    type = IntervalTypes.Milliseconds;
                    return 50;
                }

                // Seconds
                double seconds = timeSpan.TotalSeconds;

                if (seconds <= 7)
                {
                    type = IntervalTypes.Seconds;
                    return 1;
                }
                else if (seconds <= 15)
                {
                    type = IntervalTypes.Seconds;
                    return 2;
                }
                else if (seconds <= 30)
                {
                    type = IntervalTypes.Seconds;
                    return 5;
                }
                else if (seconds <= 60)
                {
                    type = IntervalTypes.Seconds;
                    return 10;
                }
            }
            else if (totalMinutes <= 2.0)
            {
                // Range less than 120 seconds interval is 10 sec
                type = IntervalTypes.Seconds;
                return 20;
            }
            else if (totalMinutes <= 3.0)
            {
                // Range less than 180 seconds interval is 30 sec
                type = IntervalTypes.Seconds;
                return 30;
            }
            else if (totalMinutes <= 10)
            {
                // Range less than 10 minutes interval is 1 min
                type = IntervalTypes.Minutes;
                return 1;
            }
            else if (totalMinutes <= 20)
            {
                // Range less than 20 minutes interval is 1 min
                type = IntervalTypes.Minutes;
                return 2;
            }
            else if (totalMinutes <= 60)
            {
                // Range less than 60 minutes interval is 5 min
                type = IntervalTypes.Minutes;
                return 5;
            }
            else if (totalMinutes <= 120)
            {
                // Range less than 120 minutes interval is 10 min
                type = IntervalTypes.Minutes;
                return 10;
            }
            else if (totalMinutes <= 180)
            {
                // Range less than 180 minutes interval is 30 min
                type = IntervalTypes.Minutes;
                return 30;
            }
            else if (totalMinutes <= 60 * 12)
            {
                // Range less than 12 hours interval is 1 hour
                type = IntervalTypes.Hours;
                return 1;
            }
            else if (totalMinutes <= 60 * 24)
            {
                // Range less than 24 hours interval is 4 hour
                type = IntervalTypes.Hours;
                return 4;
            }
            else if (totalMinutes <= 60 * 24 * 2)
            {
                // Range less than 2 days interval is 6 hour
                type = IntervalTypes.Hours;
                return 6;
            }
            else if (totalMinutes <= 60 * 24 * 3)
            {
                // Range less than 3 days interval is 12 hour
                type = IntervalTypes.Hours;
                return 12;
            }
            else if (totalMinutes <= 60 * 24 * 10)
            {
                // Range less than 10 days interval is 1 day
                type = IntervalTypes.Days;
                return 1;
            }
            else if (totalMinutes <= 60 * 24 * 20)
            {
                // Range less than 20 days interval is 2 day
                type = IntervalTypes.Days;
                return 2;
            }
            else if (totalMinutes <= 60 * 24 * 30)
            {
                // Range less than 30 days interval is 3 day
                type = IntervalTypes.Days;
                return 3;
            }
            else if (totalMinutes <= 60 * 24 * 30.5 * 2)
            {
                // Range less than 2 months interval is 1 week
                type = IntervalTypes.Weeks;
                return 1;
            }
            else if (totalMinutes <= 60 * 24 * 30.5 * 5)
            {
                // Range less than 5 months interval is 2weeks
                type = IntervalTypes.Weeks;
                return 2;
            }
            else if (totalMinutes <= 60 * 24 * 30.5 * 12)
            {
                // Range less than 12 months interval is 1 month
                type = IntervalTypes.Months;
                return 1;
            }
            else if (totalMinutes <= 60 * 24 * 30.5 * 24)
            {
                // Range less than 24 months interval is 3 month
                type = IntervalTypes.Months;
                return 3;
            }
            else if (totalMinutes <= 60 * 24 * 30.5 * 48)
            {
                // Range less than 48 months interval is 6 months 
                type = IntervalTypes.Months;
                return 6;
            }

            // Range more than 48 months interval is year 
            type = IntervalTypes.Years;
            double years = totalMinutes / 60 / 24 / 365;
            if (years < 5)
            {
                return 1;
            }
            else if (years < 10)
            {
                return 2;
            }

            // Make a correction of the interval
            return Math.Floor(years / 5);
        }
        
        /// <summary>
        /// Update date with particular interval type
        /// </summary>
        /// <param name="dateToUpdate">Date to update</param>
        /// <param name="increment">Increment value</param>
        /// <param name="autoIntervalType">Type of the interval</param>
        /// <returns></returns>
        public static DateTime UpdateDate(DateTime dateToUpdate, Double increment, IntervalTypes intervalType)
        {   
            switch (intervalType)
            {
                case IntervalTypes.Years:
                    dateToUpdate = dateToUpdate.AddYears((int)Math.Floor(increment));
                    TimeSpan span = TimeSpan.FromDays(365.0 * (increment - Math.Floor(increment)));
                    
                    dateToUpdate = dateToUpdate.Add(span);

                    return dateToUpdate;

                case IntervalTypes.Months:

                    // Special case handling when current date point
                    // to the last day of the month
                    bool lastMonthDay = false;

                    if (dateToUpdate.Day == DateTime.DaysInMonth(dateToUpdate.Year, dateToUpdate.Month))
                    {
                        lastMonthDay = true;
                    }

                    // Add specified amount of months
                    dateToUpdate = dateToUpdate.AddMonths((int)Math.Floor(increment));
                    span = TimeSpan.FromDays(30.0 * (increment - Math.Floor(increment)));

                    // Check if last month of the day was used
                    if (lastMonthDay && span.Ticks == 0)
                    {
                        // Make sure the last day of the month is selected
                        int daysInMobth = DateTime.DaysInMonth(dateToUpdate.Year, dateToUpdate.Month);
                        dateToUpdate = dateToUpdate.AddDays(daysInMobth - dateToUpdate.Day);
                    }

                    dateToUpdate = dateToUpdate.Add(span);

                    return dateToUpdate;

                case IntervalTypes.Weeks:
                    return dateToUpdate.AddDays(7 * increment);

                case IntervalTypes.Hours:
                    return dateToUpdate.AddHours(increment);

                case IntervalTypes.Minutes:
                    return dateToUpdate.AddMinutes(increment);

                case IntervalTypes.Seconds:
                    return dateToUpdate.AddSeconds(increment);

                case IntervalTypes.Milliseconds:
                    return dateToUpdate.AddMilliseconds(increment);

                default:
                    return dateToUpdate.AddDays(increment);
            }
        }

        /// <summary>
        /// Adjusts the beginning of the first interval depending on the type and size
        /// </summary>
        /// <param name="dateTime">Original dateTime point</param>
        /// <param name="intervalSize">Interval size</param>
        /// <param name="type">Type of the interval (Month, Year, ...)</param>
        /// <returns>
        /// Adjusted DateTime
        /// </returns>
        internal static DateTime AlignDateTime(DateTime dateTime, double intervalSize, IntervalTypes type)
        {
            // Get the beginning of the interval depending on type
            DateTime newStartDate = dateTime;

            // Adjust the months interval depending on size
            if (intervalSize > 0.0 && intervalSize != 1.0)
            {
                if (type == IntervalTypes.Months && intervalSize <= 12.0 && intervalSize > 1)
                {
                    // Make sure that the beginning is aligned correctly for cases like quarters and half years
                    DateTime resultDate = newStartDate;
                    DateTime sizeAdjustedDate = new DateTime(newStartDate.Year, 1, 1, 0, 0, 0);
                    while (sizeAdjustedDate < newStartDate)
                    {   
                        resultDate = sizeAdjustedDate;
                        sizeAdjustedDate = sizeAdjustedDate.AddMonths((int)intervalSize);
                    }

                    newStartDate = resultDate;
                    return newStartDate;
                }
            }

            // Check interval type
            switch (type)
            {
                case IntervalTypes.Years:
                    int year = (int)((newStartDate.Year / intervalSize) * intervalSize);
                    if (year <= 0)
                    {
                        year = 1;
                    }
                    newStartDate = new DateTime(year, 1, 1, 0, 0, 0, 0);
                    break;

                case IntervalTypes.Months:

                    int month = (int)((newStartDate.Month / intervalSize) * intervalSize);

                    if (month <= 0)
                    {
                        month = 1;
                    }

                    newStartDate = new DateTime(newStartDate.Year, month, 1, 0, 0, 0);
                    break;

                case IntervalTypes.Days:

                    int day = (int)((newStartDate.Day / intervalSize) * intervalSize);

                    if (day <= 0)
                    {
                        day = 1;
                    }

                    newStartDate = new DateTime(newStartDate.Year, newStartDate.Month, day, 0, 0, 0);

                    break;

                case IntervalTypes.Hours:

                    int hour = (int)((newStartDate.Hour / intervalSize) * intervalSize);

                    newStartDate = new DateTime(
                        newStartDate.Year,
                        newStartDate.Month,
                        newStartDate.Day,
                        hour,
                        0,
                        0);
                    break;

                case IntervalTypes.Minutes:

                    int minute = (int)((newStartDate.Minute / intervalSize) * intervalSize);

                    newStartDate = new DateTime(
                        newStartDate.Year,
                        newStartDate.Month,
                        newStartDate.Day,
                        newStartDate.Hour,
                        minute,
                        0);
                    break;

                case IntervalTypes.Seconds:

                    int second = (int)((newStartDate.Second / intervalSize) * intervalSize);

                    newStartDate = new DateTime(
                        newStartDate.Year,
                        newStartDate.Month,
                        newStartDate.Day,
                        newStartDate.Hour,
                        newStartDate.Minute,
                        second,
                        0);

                    break;

                case IntervalTypes.Milliseconds:

                    int milliseconds = (int)((newStartDate.Millisecond / intervalSize) * intervalSize);

                    newStartDate = new DateTime(
                        newStartDate.Year,
                        newStartDate.Month,
                        newStartDate.Day,
                        newStartDate.Hour,
                        newStartDate.Minute,
                        newStartDate.Second,
                        milliseconds);

                    break;

                case IntervalTypes.Weeks:

                    // Elements that have interval set to weeks should be aligned to the nearest dateTime of week no matter how many weeks is the interval.
                    newStartDate = newStartDate.AddDays(-((int)newStartDate.DayOfWeek));
                    newStartDate = new DateTime(
                        newStartDate.Year,
                        newStartDate.Month,
                        newStartDate.Day,
                        0,
                        0,
                        0);

                    break;
            }

            return newStartDate;
        }

        #endregion
    }
}