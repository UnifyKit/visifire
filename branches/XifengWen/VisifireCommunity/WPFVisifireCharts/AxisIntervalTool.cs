using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Visifire.Charts
{
    #region Nortek
    public static class AxisIntervalTool
    {
        private static Int32 OrderOfMagnitude(Decimal number)
        {
            Decimal mantissa;                       // Mantissa of number.
            Decimal exponent;                       // Exponent of number.

            if (number == 0)
                return 0;

            mantissa = GetMantissaOrExponent(MantissaOrExponent.Mantissa, number);
            exponent = GetMantissaOrExponent(MantissaOrExponent.Exponent, number);


            if (number > 0)
            {
                return mantissa.ToString(System.Globalization.CultureInfo.InvariantCulture).Length + (Int32)(exponent - 1);
            }
            else
            {
                return (mantissa.ToString(System.Globalization.CultureInfo.InvariantCulture).Length + (Int32)(exponent - 1)) - 1;
            }

        }

        private static Decimal GetMantissaOrExponent(MantissaOrExponent mantissaOrExponent, Decimal number)
        {
            if (mantissaOrExponent == MantissaOrExponent.Exponent)
            {
                Decimal exponent;
                exponent = NoOfZeroAtEndInInt(RemoveDecimalPoint(number));
                exponent -= IndexOfDecimalPoint(number);
                return exponent;
            }
            else
            {
                Decimal mantissa;
                mantissa = RemoveZeroFromInt(RemoveDecimalPoint(number));
                return mantissa;
            }
        }
        private static Int32 NoOfZeroAtEndInInt(Decimal number)
        {
            Int32 count = 0;            // Keep track the no of zeros.

            while ((number % 10) == 0)
            {
                count++;
                number = number / 10;
            }

            return count;

        }
        private static Decimal RemoveDecimalPoint(Decimal number)
        {
            // Number is already an integer.
            if (IsInterger(number))
                return number;
            else
                // Multiply 10 to move the decimal point to one digit right.
                while (!IsInterger(number))
                    number = number * 10;

            return number;

        }
        public static Boolean IsInterger(Decimal number)
        {
            return (Decimal.Truncate(number) == number);
        }

        private static Decimal RemoveZeroFromInt(Decimal number)
        {
            // While the number is divided by 10.
            while ((number % 10) == 0)
                number = number / 10;

            return Decimal.Truncate(number);
        }

        private static Int32 IndexOfDecimalPoint(Decimal number)
        {
            Int32 count = 0;                  // local variable as counter.

            // While number is not an integer.
            while (!IsInterger(number))
            {
                count++;
                number = number * 10;
            }

            return count;

        }

        /// <summary>
        /// Rounding down the value of axis maximum value. 
        /// </summary>
        /// <param name="axisMaxValue">Axis maximum value.</param>
        /// <param name="intervalValue">Interval value.</param>
        /// <returns></returns>
        private static Decimal RoundAxisMaximumValue(Decimal axisMaxValue, Decimal intervalValue)
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
        private static Decimal RoundAxisMinimumValue(Decimal axisMinValue, Decimal intervalValue)
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
        private static Decimal ReduceInterval(Decimal intervalValue)
        {
            Decimal mantissa; // Mantissa of interval value.

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
        /*
         * Default maxNoOfInterval is 10 
         * */
        public static double GenerateDefaultInterval(ref decimal max, ref decimal min, bool axisMaxFixed, bool axisMinFixed)
        {
            int noOfInterval;
            return GenerateDefaultInterval(ref  max, ref  min, axisMaxFixed, axisMinFixed, 10, out noOfInterval);
        }

        public static double GenerateDefaultInterval(ref decimal max, ref decimal min, bool axisMaxFixed, bool axisMinFixed, int maxNoOfInterval)
        {
            int noOfInterval;
            return GenerateDefaultInterval(ref  max, ref  min, axisMaxFixed, axisMinFixed, maxNoOfInterval, out noOfInterval);
        }

        //, bool axisMaxFixed, bool axisMinFixed
        public static double GenerateDefaultInterval(ref decimal max, ref decimal min, bool axisMaxFixed, bool axisMinFixed, int maxNoOfInterval, out int noOfInterval)
        {
            int loop = 0;
            noOfInterval = 0;
            decimal origianMax = max;
            decimal origianMin = min;
            int maxMagnitude = OrderOfMagnitude(origianMax);
            int minMagnitude = OrderOfMagnitude(origianMin);
            int magnitude = (maxMagnitude > minMagnitude) ? maxMagnitude : minMagnitude;

            decimal nextInterval = (Decimal)Math.Pow(10, magnitude + 1);

            decimal tempAxisMaximumValue = RoundAxisMaximumValue(origianMax, nextInterval);
            decimal tempAxisMinimumValue = RoundAxisMinimumValue(origianMin, nextInterval);
            decimal interval = origianMax - origianMin;


            if (axisMaxFixed)
            {
                tempAxisMaximumValue = max;
            }

            if (axisMinFixed)
            {
                tempAxisMinimumValue = min;
            }

            while (++loop < 100)
            {
                Int32 nextNoOfInterval;   // Number of interval increased in iterative way.     
                nextInterval = ReduceInterval(nextInterval);
                // If next interval is undesirable then stop further calculation.
                if (nextInterval == 0)
                {
                    break;
                }
                if (!axisMaxFixed)
                {
                    tempAxisMaximumValue = RoundAxisMaximumValue(origianMax, nextInterval);
                }

                if (!axisMinFixed)
                {
                    tempAxisMinimumValue = RoundAxisMinimumValue(origianMin, nextInterval);
                }

                // Calculate the number of interval.
                nextNoOfInterval = (Int32)((tempAxisMaximumValue - tempAxisMinimumValue) / nextInterval);

                // Number of interval cannot exceed the user expected no of interval.
                if (nextNoOfInterval > maxNoOfInterval)
                    break;
                noOfInterval = nextNoOfInterval;
                max = tempAxisMaximumValue;
                min = tempAxisMinimumValue;
                interval = nextInterval;
            }
            return (double)interval;
        }

        public static ValidTimeAxisSettings GenerateDefaultDateTimeInterval(DateTime start, DateTime end, int maxNoOfInterval)
        {
            if (end > start)
            {
                TimeSpan timeDiff = TimeSpan.FromTicks(end.Ticks - start.Ticks);
                TimeSpan tempIntervalUnit = TimeSpan.FromMilliseconds(timeDiff.TotalMilliseconds / maxNoOfInterval);
                ValidTimeAxisSettings minValidTimeSpan = null;
                ValidTimeAxisSettings maxValidTimeSpan = null;
                int i = 0;
                foreach (ValidTimeAxisSettings ts in ValidTimeAxisSettingList.Instance)
                {
                    if (ts.TimeSpan >= tempIntervalUnit)
                    {
                        maxValidTimeSpan = ts;
                        if (i != 0)
                        {
                            minValidTimeSpan = ValidTimeAxisSettingList.Instance[i - 1];
                        }
                        else
                        {
                            minValidTimeSpan = maxValidTimeSpan;
                        }
                        break;
                    }
                    i++;
                }

                if (minValidTimeSpan == null || maxValidTimeSpan == null)
                {
                    throw new Exception("ValidTimeSpanIndexers has no item,please add!");
                }

                if ((maxValidTimeSpan.TimeSpan - tempIntervalUnit) < (tempIntervalUnit - minValidTimeSpan.TimeSpan))
                {
                    return maxValidTimeSpan;
                }
                else
                {
                    return minValidTimeSpan;
                }
            }
            else
            {
                throw new Exception();
            }
        }
    }
    public static class ValidTimeAxisSettingList
    {
        private static List<ValidTimeAxisSettings> myList = new List<ValidTimeAxisSettings>();
        public static List<ValidTimeAxisSettings> Instance
        {
            get { return myList; }
        }
        static ValidTimeAxisSettingList()
        {
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Seconds, 10d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Seconds, 20d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Seconds, 30d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Minutes, 1d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Minutes, 2d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Minutes, 5d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Minutes, 10d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Minutes, 30d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Hours, 1d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Hours, 2d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Hours, 3d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Hours, 6d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Hours, 12d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Days, 1d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Days, 2d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Weeks, 1d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Weeks, 2d));
            myList.Add(new ValidTimeAxisSettings(Visifire.Charts.IntervalTypes.Weeks, 4d));
        }
    }

    public class ValidTimeAxisSettings
    {
        TimeSpan timeSpan;
        Visifire.Charts.IntervalTypes intervalType;
        double internval;
        string dateFormat;

        public string DateFormat
        {
            get { return dateFormat; }
        }
        public TimeSpan TimeSpan
        {
            get { return timeSpan; }
        }

        public Visifire.Charts.IntervalTypes IntervalType
        {
            get { return intervalType; }
        }

        public double Internval
        {
            get { return internval; }
        }

        public ValidTimeAxisSettings(Visifire.Charts.IntervalTypes intervalType, double interval)
        {
            if (interval >= 0)
            {
                switch (intervalType)
                {
                    case Visifire.Charts.IntervalTypes.Seconds:
                        timeSpan = TimeSpan.FromSeconds(interval);
                        dateFormat = "dd.MM mm:ss";
                        break;

                    case Visifire.Charts.IntervalTypes.Minutes:
                        timeSpan = TimeSpan.FromMinutes(interval);
                        dateFormat = "dd.MM HH:mm";
                        break;

                    case Visifire.Charts.IntervalTypes.Hours:
                        timeSpan = TimeSpan.FromHours(interval);
                        dateFormat = "dd.MM HH:mm";
                        break;

                    case Visifire.Charts.IntervalTypes.Days:
                        timeSpan = TimeSpan.FromDays(interval);
                        dateFormat = "dd/MM HH:mm";
                        break;

                    case Visifire.Charts.IntervalTypes.Weeks:
                        timeSpan = TimeSpan.FromDays(interval * 7);
                        dateFormat = "MM.dd.yy";
                        break;
                    default:
                        throw new Exception("Please specify valid Interval Type: Seconds, Minutes, Hours, Days, or Weeks");
                }
                this.intervalType = intervalType;
                this.internval = interval;
            }
            else
            {
                throw new Exception("Interval must be set larger than 0");
            }
        }
    }

    #endregion
}
