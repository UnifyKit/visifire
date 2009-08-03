using System;
using System.Globalization;

namespace Visifire.Charts
{
    /// <summary>
    /// AxisManager class calculates the max value, min value, interval and number of intervals
    /// of the axis.
    /// </summary>
    internal class AxisManager
    {
        #region Public Methods

        #region "Constructor"

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.AxisManager class
        /// </summary>
        /// <param name="maxValue">Maximum Value.</param>
        /// <param name="minValue">Minimum Value.</param>
        /// <param name="startFromZero">Makes sure that the zero is included in the axis range</param>
        /// <param name="allowLimitOverflow">Applies limits so that axis range doesn't cross it</param>
        public AxisManager(Double maxValue, Double minValue, Boolean startFromZero, Boolean allowLimitOverflow, Boolean stackingOverride, AxisRepresentations axisRepresentation)
        {
            if (maxValue < minValue)
                throw (new ArgumentException("Invalid Argument:: Maximum Data value should be always greater than the minimum data value."));
            this._max = (Decimal)maxValue;
            this._min = (Decimal)minValue;

            AxisRepresentation = axisRepresentation;

            if (startFromZero)
            {
                if (minValue >= 0) AxisMinimumValue = 0;
                else if (maxValue <= 0) AxisMaximumValue = 0;
            }
            if (!allowLimitOverflow)
            {
                if (minValue == 0) AxisMinimumValue = 0;
                if (maxValue == 0) AxisMaximumValue = 0;
            }
            if (!allowLimitOverflow & stackingOverride)
            {
                AxisMaximumValue = maxValue;
                AxisMinimumValue = minValue;
            }
        }
        #endregion

        /// <summary>
        /// Function calculates the max value, min value, interval and number of intervals of the axis.
        /// </summary>
        public void Calculate()
        {   
            Int32 loop = 0;                 // No of iteration.
            Int32 maxMagnitude;             // Magnitude of max data value.         
            Int32 minMagnitude;             // Magnitude of min data value.         
            Int32 magnitude;                // Magnitude of max/min data value.
            Decimal nextInterval;           // Next calculated interval size from the old interval size.
            Decimal tempAxisMaximumValue;   // Calculated maximum value of the axis.
            Decimal tempAxisMinimumValue;   // Calculated minimum value of the axis.
            
            // Handle values less than 10 and Greater than 2
            if (AxisRepresentation == AxisRepresentations.AxisX)
            {
                if (this._max < 10 && this._min >= 0)
                    this._maxNoOfInterval = (Int32)(_max + 1);
            }
            else
            {
                if (_max > 2 && this._max < 10 && this._min >= 0)
                    this._maxNoOfInterval = (Int32)(_max + 1);
            }
            
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
                Int32 nextNoOfInterval;   // Number of interval increased in iterative way.     

                // Try to minimize the Interval Value if possible.
                if (!this._overrideInterval)
                    nextInterval = ReduceInterval(nextInterval);

                // If next interval is undesirable then stop further calculation.
                if (nextInterval == 0)
                    break;

                // Rounding down the axisMaximumValue if necessary.
                if (!this._overrideAxisMaximumValue)
                    tempAxisMaximumValue = RoundAxisMaximumValue(this._max, nextInterval);

                // Rounding down the axisMinimumValue if necessary.
                if (!this._overrideAxisMinimumValue)
                    tempAxisMinimumValue = RoundAxisMinimumValue(this._min, nextInterval);

                // Calculate the number of interval.
                nextNoOfInterval = (Int32)((tempAxisMaximumValue - tempAxisMinimumValue) / nextInterval);

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

        public AxisRepresentations AxisRepresentation
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the maximum number of intervals.
        /// </summary>
        public Int32 MaximumNoOfInterval
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
            get
            {
                return GetNoOfIntervals();
            }
        }

        /// <summary>
        /// Write-only property used to include zero in the axis range.
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
        /// Get or set the axis maximum value.
        /// </summary>
        public Double AxisMaximumValue
        {
            get
            {
                return (Double)this._axisMaximumValue;
            }
            set
            {
                this._axisMaximumValue = (Decimal)value;
                this._overrideAxisMaximumValue = true;
            }
        }

        /// <summary>
        /// Get or set the interval.
        /// </summary>
        public Double Interval
        {
            get
            {
                return (Double)_interval;
            }

            set
            {
                if (value < 0)
                    throw (new Exception("Invalid property value:: Interval size should be positive always."));

                this._interval = (Decimal)value;
                this._overrideInterval = true;
            }
        }

        /// <summary>
        /// Get or set the axis minimum value.
        /// </summary>
        public Double AxisMinimumValue
        {
            set
            {
                this._axisMinimumValue = (Decimal)value;
                this._overrideAxisMinimumValue = true;
            }
            get
            {
                return (Double)this._axisMinimumValue;
            }
        }

        /// <summary>
        /// Get or set minimum data value
        /// </summary>
        public Double MinimumValue
        {
            get
            {
                return (Double)_min;
            }
            set
            {
                _min = (Decimal)value;
            }
        }

        /// <summary>
        /// Get or set maximum data value
        /// </summary>
        public Double MaximumValue
        {
            get
            {
                return (Double)_max;
            }
            set
            {
                _max = (Decimal)value;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the number of Intervals in the calculated range.
        /// </summary>
        private Int32 GetNoOfIntervals()
        {
            return (Int32)((this._axisMaximumValue - this._axisMinimumValue) / this._interval);
        }

        /// <summary>
        /// Removes decimal point from a decimal number.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>
        private Int64 RemoveDecimalPoint(Decimal number)
        {
            // Number is already an integer.
            if ((Int64)(number) == number)
                return (Int64)(number);
            else
                // Multiply 10 to move the decimal point to one digit right.
                while ((Int64)(number) != number)
                    number = number * 10;

            return (Int64)(number); 

        }

        /// <summary>
        /// Finds the position of the decimal point in decimal number.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>
        private Int32 IndexOfDecimalPoint(Decimal number)
        {
            Int32 count = 0;                  // local variable as counter.
            
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
            // While the number is divided by 10.
            while ((number % 10) == 0)
                number = number / 10;

            return number;

        }

        /// <summary>
        /// Calculate the number of zeros at the end of a number.
        /// </summary>
        /// <param name="number">Number used for calculation.</param>
        /// <returns>Returns an integer.</returns>
        private Int32 NoOfZeroAtEndInInt(Int64 number)
        {
            Int32 count = 0;            // Keep track the no of zeros.

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
                Int32 exponent;
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

            return mantissa.ToString(CultureInfo.InvariantCulture).Length + (Int32)(exponent - 1);

        }

        /// <summary>
        /// Rounding down the value of axis maximum value. 
        /// </summary>
        /// <param name="axisMaxValue">Axis maximum value.</param>
        /// <param name="intervalValue">Interval value.</param>
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
        /// <param name="intervalValue">Interval value.</param>
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
            Int32 loop = 0;        // No of iteration.
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
                Int32 nextNoOfInterval;                        // Number of interval.

                // Try to minimize the Interval Value if possible.
                if (!this._overrideInterval)
                    nextInterval = ReduceInterval(nextInterval);

                // If next interval is undesirable then stop further calculation.
                if (nextInterval == 0)
                    break;

                // Calculate number of interval.
                nextNoOfInterval = (Int32)((this._axisMaximumValue - this._axisMinimumValue) / nextInterval);

                // Number of interval cannot exceed the user expected no of interval.
                if (nextNoOfInterval > _maxNoOfInterval)
                    break;

                this._interval = nextInterval;

            }

        }

        #endregion

        #region Data
        // Input parameters.
        private Decimal _min;                      // Min data value.
        private Decimal _max;                      // Max data value.
        private Int32 _maxNoOfInterval = 10;       // Maximum number of intervals.

        // Values calculated by this class.
        private Decimal _interval;                 // The interval size.
        private Decimal _axisMaximumValue;         // Calculated Maximum value of the axis.
        private Decimal _axisMinimumValue;         // Calculated Minimum value of the axis.

        // Member variables used to keep track about override operation.
        private Boolean _overrideAxisMaximumValue = false;
        private Boolean _overrideAxisMinimumValue = false;
        private Boolean _overrideInterval = false;

        #endregion Data

    }

}
