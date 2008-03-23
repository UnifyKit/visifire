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

namespace Visifire.Charts
{
    /// <summary>
    /// AxisManager class calculates the max value, min value, interval and 'number of intervals'
    /// of the axis.
    /// </summary>

    internal class AxisManager
    {

        #region Public Methods

        #region "Constructor"
        /// <summary>
        /// AxisManager class calculates the max value, min value, interval and 'number of intervals'
        /// of the axis.
        /// </summary>
        /// <param name="maxValue">Maximum Value.</param>
        /// <param name="minValue">Minimum Value.</param>

        public AxisManager(Decimal maxValue, Decimal minValue,Boolean startFromZero)
        {
            if (maxValue < minValue)
                throw (new ArgumentException("Invalid Argument:: Maximum Data value should be always greater than the minimum data value."));
            this._max = maxValue;
            this._min = minValue;

            if (startFromZero)
            {
                if (minValue >= 0) AxisMinimumValue = 0;
                else if (maxValue < 0) AxisMaximumValue = 0;
            }
            
        }
        #endregion

        /// <summary>
        /// Returns the calculated Maximum value of the axis.
        /// </summary>

        public Decimal GetAxisMaximumValue()
        {
            return this._axisMaximumValue;
        }

        /// <summary>
        /// Returns the calculated Minimum value of the axis.
        /// </summary>

        public Decimal GetAxisMinimumValue()
        {
            return this._axisMinimumValue;
        }

        /// <summary>
        /// Returns the smallest datavalue on the Axis
        /// </summary>
        /// <returns></returns>
        public Decimal GetMinimumDataValue()
        {
            return this._min;
        }

        /// <summary>
        /// Returns the smallest datavalue on the axis
        /// </summary>
        /// <returns></returns>
        public Decimal GetMaximumDataValue()
        {
            return this._max;
        }

        /// <summary>
        /// Returns 1unit interval size.
        /// </summary>

        public Decimal GetInterval()
        {
            return this._interval;
        }

        /// <summary>
        /// Returns the number of Intervals in the calculated range.
        /// </summary>

        public Int16 GetNoOfIntervals()
        {
            return (Int16)((this._axisMaximumValue - this._axisMinimumValue) / this._interval);
        }

        /// <summary>
        /// Function calculates the max value, min value, interval and number of intervals of the axis.
        /// </summary>

        public void Calculate()
        {
            Int16 loop = 0;                 // No of iteration.
            Int32 maxMagnitude;             // Magnitude of max data value.         
            Int32 minMagnitude;             // Magnitude of min data value.         
            Int32 magnitude;                // Magnitude of max/min data value.
            Decimal nextInterval;           // Next calculated interval size from the old interval size.
            Decimal tempAxisMaximumValue;   // Calculated maximum value of the axis.
            Decimal tempAxisMinimumValue;   // Calculated minimum value of the axis.


            // Handle values less than 10
            if (this._max < 10 && this._min >= 0)
                this._maxNoOfInterval = (Int16)(_max + 1);

            // Only one value presents to calculate the range.
            if (this._max == this._min)
            {
                CalculateSingle();  // Calculation for single value.
                return;
            }

            // Max is rounded to the nearest power of 10.
            maxMagnitude = OrderOfMagnitude(this._max);

            // Min is rounded to the nearest power of 10.
            minMagnitude = OrderOfMagnitude(this._min);

            // The maximum magnitude need to chose, in order to calculate the initial
            // interval size having maximum the value.
            magnitude = (maxMagnitude > minMagnitude) ? maxMagnitude : minMagnitude;

            // Interval needs to be sinking towards the power of 10. 
            // Initially maximum interval is chosen.
            if (this._overrideInterval)
                nextInterval = this._interval;
            else
                nextInterval = (Decimal)Math.Pow(10, magnitude + 1);

            // Rounding down the axisMaximumValue if necessary.
            if (this._overrideAxisMaximumValue)
                tempAxisMaximumValue = this._axisMaximumValue;
            else
                tempAxisMaximumValue = RoundAxisMaximumValue(this._max, nextInterval);

            // Rounding up the axisMinimumValue if necessary.            
            if (this._overrideAxisMinimumValue)
                tempAxisMinimumValue = this._axisMinimumValue;
            else
                tempAxisMinimumValue = RoundAxisMinimumValue(this._min, nextInterval);

            this._interval = nextInterval;
            this._axisMaximumValue = tempAxisMaximumValue;
            this._axisMinimumValue = tempAxisMinimumValue;

            // Next intervals will be calculated inside loop in iterative way.
            // Top value will be rounded down as much as possible.
            // Bottom value will be rounded up as much as possible.
            // So, In each pass in while loop calculates the new reduced interval.
            // which helps to calculate the new maximum and minimum value for axis.
            while (++loop < 100)
            {
                Int16 nextNoOfInterval;   // Number of interval increased in iterative way.     

                // Try to minimize the Interval Value if possible.
                if (!this._overrideInterval)
                    nextInterval = ReduceInterval(nextInterval);

                // If next interval is undesirable then stop farther calculation.
                if (nextInterval == 0)
                    break;

                // Rounding down the axisMaximumValue if necessary.
                if (!this._overrideAxisMaximumValue)
                    tempAxisMaximumValue = RoundAxisMaximumValue(this._max, nextInterval);

                // Rounding down the axisMinimumValue if necessary.
                if (!this._overrideAxisMinimumValue)
                    tempAxisMinimumValue = RoundAxisMinimumValue(this._min, nextInterval);

                // Calculate the number of interval.
                nextNoOfInterval = (Int16)((tempAxisMaximumValue - tempAxisMinimumValue) / nextInterval);

                // Number of interval cannot exceed the user expected no of interval.
                if (nextNoOfInterval > this._maxNoOfInterval)
                    break;

                this._axisMaximumValue = tempAxisMaximumValue;
                this._axisMinimumValue = tempAxisMinimumValue;
                this._interval = nextInterval;

            }
            
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Expected maximum number of intervals.
        /// </summary>

        public Int16 MaximumNoOfInterval
        {
            set
            {
                if (value < 0)
                    throw (new Exception("Invalid property value:: Expected number of intervals should be positive."));
                else
                    if (value > 100)
                        throw (new Exception("Property out of range:: Expected number of intervals should be less than or equals to 1000."));


                this._maxNoOfInterval = value;
            }
        }

        /// <summary>
        /// Write only property used to include zero in the axis range.
        /// </summary>

        public Boolean IncludeZero
        {
            set
            {
                // If zero is included need to set min value as 0.
                if (value == true && this._min > 0)
                    this._min = 0;
            }
        }

        /// <summary>
        /// Write only property to Override the axis maximum value.
        /// </summary>

        public Decimal AxisMaximumValue
        {
            get
            {
                return this._axisMaximumValue;
            }
            set
            {
                if (value < this._max)
                    throw (new Exception("Invalid property value:: Maximum axis value should be greater than or equals to the max data value."));

                this._axisMaximumValue = value;
                this._overrideAxisMaximumValue = true;
            }
        }

        /// <summary>
        /// Write only property to override the Number of interval.
        /// </summary>

        public Decimal Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                if (value < 0)
                    throw (new Exception("Invalid property value:: Interval size should be positive always."));

                this._interval = value;
                this._overrideInterval = true;
            }
        }

        /// <summary>
        /// Write only property to override the axis minimum value.
        /// </summary>

        public Decimal AxisMinimumValue
        {
            set
            {
                if (value > this._min)
                    throw (new Exception("Invalid property value:: Minimum axis value should be less than or equals to the min data value."));

                this._axisMinimumValue = value;
                this._overrideAxisMinimumValue = true;
            }
            get
            {
                return this._axisMinimumValue;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Removes decimal point from a decimal number.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>

        private Int64 RemoveDecimalPoint(Decimal number)
        {
            // Number is already is an integer.
            if ((Int64)(number) == number)
                return (Int64)(number);
            else
                // Multiply 10 to move the decimal point to the one digit right.
                while ((Int64)(number) != number)
                    number = number * 10;

            return (Int64)(number);

        }

        /// <summary>
        /// Finds the position of the decimal point in decimal number.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>

        private Int16 IndexOfDecimalPoint(Decimal number)
        {
            Int16 count = 0;                  // local variable as counter.

            // While number is not an integer.
            while ((Int64)(number) != number)
            {
                count++;
                number = number * 10;
            }

            return count;

        }

        /// <summary>
        /// Remove trailing from an integer.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>

        private Int64 RemoveZeroFromInt(Int64 number)
        {
            // While the number is divide by 10.
            while ((number % 10) == 0)
                number = number / 10;

            return number;

        }

        /// <summary>
        /// Calculate the number of zeros at the end of a number.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>

        private Int16 NoOfZeroAtEndInInt(Int64 number)
        {
            Int16 count = 0;            // Keep track the no of zeros.

            while ((number % 10) == 0)
            {
                count++;
                number = number / 10;
            }

            return count;

        }

        /// <summary>
        /// Calculate the mantissa or exponent of decimal number.
        /// </summary>
        /// <param name="mantissaOrExponent">According to the argument mantissa or exponent will be returned.</param>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns mantissa or exponent.</returns>

        private Int64 GetMantissaOrExponent(MantissaOrExponent mantissaOrExponent, Decimal number)
        {
            if (mantissaOrExponent == MantissaOrExponent.Exponent)
            {
                Int16 exponent;
                exponent = NoOfZeroAtEndInInt(RemoveDecimalPoint(number));
                exponent -= IndexOfDecimalPoint(number);
                return (Int64)exponent;
            }
            else
            {
                Int64 mantissa;
                mantissa = RemoveZeroFromInt(RemoveDecimalPoint(number));
                return mantissa;
            }


        }

        /// <summary>
        /// Finds the order of magnitude of a number.
        /// Note: A number rounded to the nearest power of 10 is called an order of magnitude.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>

        private Int32 OrderOfMagnitude(Decimal number)
        {
            Int64 mantissa;                       // Mantissa of number.
            Int64 exponent;                       // Exponent of number.

            if (number == 0)
                return 0;

            mantissa = GetMantissaOrExponent(MantissaOrExponent.Mantissa, number);
            exponent = GetMantissaOrExponent(MantissaOrExponent.Exponent, number);

            return mantissa.ToString().Length + (Int32)(exponent - 1);

        }

        /// <summary>
        /// Rounding down the value of axis maximum value. 
        /// </summary>
        /// <param name="axisMaxValue">Axis maximum value.</param>
        /// <param name="intervalValue">interval value.</param>
        /// <returns></returns>

        private Decimal RoundAxisMaximumValue(Decimal axisMaxValue, Decimal intervalValue)
        {
            axisMaxValue = axisMaxValue / intervalValue;
            axisMaxValue = Decimal.Floor(axisMaxValue);
            axisMaxValue = (axisMaxValue + 1) * intervalValue;

            return axisMaxValue;

        }

        /// <summary>
        /// Rounding Up the value of axis minimum value. 
        /// </summary>
        /// <param name="axisMinValue">Axis minimum value.</param>
        /// <param name="intervalValue">interval value.</param>

        private Decimal RoundAxisMinimumValue(Decimal axisMinValue, Decimal intervalValue)
        {
            axisMinValue = axisMinValue / intervalValue;
            axisMinValue = (Decimal)Math.Ceiling((Double)axisMinValue);
            axisMinValue = (axisMinValue - 1) * intervalValue;

            return axisMinValue;

        }

        /// <summary>
        /// Try to minimize the Interval Value if possible.
        /// </summary>
        /// <param name="intervalValue">Interval value.</param>
        /// <returns>Reduced interval.</returns>

        private Decimal ReduceInterval(Decimal intervalValue)
        {
            Int64 mantissa;                       // Mantissa of interval value.

            mantissa = GetMantissaOrExponent(MantissaOrExponent.Mantissa, intervalValue);

            // Easily understandable numbers by human brain are 5, 2 and 1 or power of 5 or 2 or 1.
            // Point to be noted: A number is divisible by its own mantissa. 
            if (mantissa == 5)
                return (intervalValue * 2 / 5);
            else if (mantissa == 2)
                return (intervalValue * 1 / 2);
            else if (mantissa == 1)
                return (intervalValue * 5 / 10);
            else
                return 0;

        }



        /// <summary>
        /// Function calculates the max value, min value, interval and number of intervals of the axis
        /// for a single value range.
        /// </summary>

        private void CalculateSingle()
        {
            Int16 loop = 0;        // No of iteration.
            Int64 magnitude;       // Magnitude of max/min value.
            Decimal nextInterval;  // Next Calculated interval from the old interval.

            // If the max and min both are same and equals to zero then the best range is 0 to 1. 
            if (_max == 0)
            {
                this._axisMaximumValue = 1;
                this._axisMinimumValue = 0;
                this._interval = 1;

                return;
            }

            // Max is rounded to the nearest power of 10.
            magnitude = OrderOfMagnitude(this._max);

            // Interval needs to be sinking towards the power of 10. 
            // Initially maximum interval is chosen.
            if (this._overrideInterval)
                nextInterval = this._interval;
            else
                nextInterval = (Decimal)Math.Pow(10, magnitude);

            // Rounding down the axisMaximumValue if necessary.
            if (!this._overrideAxisMaximumValue)
                this._axisMaximumValue = RoundAxisMaximumValue(this._max, nextInterval);

            // Rounding down the axisMaximumValue if necessary.
            if (!this._overrideAxisMinimumValue)
                this._axisMinimumValue = RoundAxisMinimumValue(this._max, nextInterval);

            this._interval = nextInterval;

            // Next intervals will be calculated inside loop in iterative way. 
            while (loop++ < 100)
            {
                Int16 nextNoOfInterval;                        // Number of interval.

                // Try to minimize the Interval Value if possible.
                if (!this._overrideInterval)
                    nextInterval = ReduceInterval(nextInterval);

                // If next interval is undesirable then stop farther calculation.
                if (nextInterval == 0)
                    break;

                // Calculate number of interval.
                nextNoOfInterval = (Int16)((this._axisMaximumValue - this._axisMinimumValue) / nextInterval);

                // Number of interval cannot exceed the user expected no of interval.
                if (nextNoOfInterval > _maxNoOfInterval)
                    break;

                this._interval = nextInterval;

            }

        }

        #endregion

        #region Data
        // input parameters.
        private Decimal _min;                      // Min data value.
        private Decimal _max;                      // Max data value.
        private Int16 _maxNoOfInterval = 10;       // Maximum number of intervals.

        // values calculated by this class.
        private Decimal _interval;                 // The interval size.
        private Decimal _axisMaximumValue;         // Calculated Maximum value of the axis.
        private Decimal _axisMinimumValue;         // Calculated Minimum value of the axis.

        // Member variables used to keep track about override operation.
        private Boolean _overrideAxisMaximumValue = false;
        private Boolean _overrideAxisMinimumValue = false;
        private Boolean _overrideInterval = false;

        #endregion Data

        private enum MantissaOrExponent
        {
            Mantissa = 1, Exponent = 2
        }
    }

}