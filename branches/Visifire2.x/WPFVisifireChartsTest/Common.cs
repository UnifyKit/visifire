using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Globalization;
using System.Windows.Shapes;
using Visifire.Charts;
using Visifire.Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Common testing utilities and constants.
    /// </summary> 
    public static class Common
    {
        public const int NumberOfInstancesForStressScenarios = 500;
        /// <summary>
        /// Delta used for high precision assertions.
        /// </summary> 
        public const double HighPrecisionDelta = 0.000000000001;

        /// <summary> 
        /// Standard number of iterations to execute timing tests. 
        /// </summary>
        public const int NumberOfIterationsForTiming = 10;

        /// <summary>
        /// Get the template parts declared on a type. 
        /// </summary> 
        /// <param name="controlType">Type with template parts defined.</param>
        /// <returns>Template parts defined on the type.</returns> 
        public static IDictionary<String, Type> GetTemplateParts(this Type controlType)
        {
            Dictionary<String, Type> templateParts = new Dictionary<String, Type>();
            foreach (Attribute attribute in controlType.GetCustomAttributes(typeof(TemplatePartAttribute), false))
            {
                TemplatePartAttribute templatePart = attribute as TemplatePartAttribute;
                if (templatePart != null)
                {
                    templateParts.Add(templatePart.Name, templatePart.Type);
                }
            }
            return templateParts;
        }

        /// <summary>
        /// Assert that a template part is defined.
        /// </summary>
        /// <param name="templateParts">Template parts defined on a type.</param> 
        /// <param name="name">Name of the template part.</param>
        /// <param name="type">Type of the template part.</param>
        public static void AssertTemplatePartDefined(this IDictionary<String, Type> templateParts, string name, Type type)
        {
            Assert.IsNotNull(templateParts);
            Assert.IsTrue(templateParts.ContainsKey(name),
                "No template part named {0} was defined!", name);
            Assert.AreEqual(type, templateParts[name],
                "\nThe template part {0} is of type {1}, not {2}!", name, templateParts[name].FullName, type.FullName);
        }

        public static bool AreBrushesEqual(Brush first, Brush second)
        {
            // If the default comparison is true, that's good enough.
            if (object.Equals(first, second))
            {
                return true;
            }

            // Do a field by field comparison if they're not the same reference
            // 
            SolidColorBrush firstSolidColorBrush = first as SolidColorBrush;
            if (firstSolidColorBrush != null)
            {
                SolidColorBrush secondSolidColorBrush = second as SolidColorBrush;
                if (secondSolidColorBrush != null)
                {
                    return object.Equals(firstSolidColorBrush.Color, secondSolidColorBrush.Color);
                }
            }

            return false;
        }

        public static void AssertBrushesAreEqual(Brush expected, Brush actual)
        {
            if (!AreBrushesEqual(expected, actual))
            {
                throw new AssertFailedException(string.Format(CultureInfo.InvariantCulture,
                   "Brushes are not equal.  Expected:{0}.  Actual:{1}.",
                   expected,
                   actual));
            }
        }

        /// <summary>
        /// Measure the duration of an action in milliseconds using a low 
        /// accuracy timing method.
        /// </summary>
        /// <param name="action">Action to measure.</param> 
        /// <returns>Duration of the action in milliseconds.</returns> 
        public static double MeasureTestDuration(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            DateTime start = DateTime.UtcNow;
            action();
            DateTime end = DateTime.UtcNow;

            return (end - start).TotalMilliseconds;
        }

        /// <summary>
        /// Ensure the action completes within the specified duration (in
        /// milliseconds). 
        /// </summary>
        /// <param name="duration">
        /// Maximum allowable duration of the action in milliseconds. 
        /// </param>
        /// <param name="action">
        /// Action to complete within the specified duration. 
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Most tests are still unwritten.")]
        public static void AssertTestDuration(double duration, Action action)
        {
            double actualDuration = MeasureTestDuration(action);
            if (actualDuration > duration)
            {
                throw new AssertFailedException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Action completed in {0}ms ({2}ms longer than the maximum allowable {1}ms).",
                    actualDuration,
                    duration,
                    actualDuration - duration));
            }
        }

        /// <summary>
        /// Ensure the action completes within the specified average duration 
        /// (in milliseconds) across the desired number of iterations.
        /// </summary>
        /// <param name="duration"> 
        /// Maximum allowable average duration of the action in milliseconds. 
        /// </param>
        /// <param name="action"> 
        /// Action to complete within the specified duration.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Most tests are still unwritten.")]
        public static void AssertAverageDuration(double duration, Action action)
        {
            AssertAverageDuration(duration, NumberOfIterationsForTiming, action);
        }

        /// <summary> 
        /// Ensure the action completes within the specified average duration
        /// (in milliseconds) across the desired number of iterations.
        /// </summary> 
        /// <param name="duration">
        /// Maximum allowable average duration of the action in milliseconds.
        /// </param> 
        /// <param name="iterations"> 
        /// Number of iterations to measure the test duration.
        /// </param> 
        /// <param name="action">
        /// Action to complete within the specified duration.
        /// </param> 
        public static String AssertAverageDuration(double duration, int iterations, Action action)
        {
            if (iterations <= 0)
            {
                throw new ArgumentException("iterations must be greater than 0.", "iterations");
            }

            double totalDuration = 0;
            for (int i = 0; i < iterations; i++)
            {
                totalDuration += MeasureTestDuration(action);
            }

            double averageDuration = totalDuration / ((double)iterations);
            if (averageDuration > duration)
            {
                throw new AssertFailedException(string.Format(
                    CultureInfo.InvariantCulture,
                    "Action completed in {0}ms on average over {2} iterations ({3}ms longer than the maximum allowable {1}ms).",
                    averageDuration,
                    duration,
                    iterations,
                    averageDuration - duration));
            }
            else
            {
                return (string.Format(CultureInfo.InvariantCulture,
                              "Action completed in {0}ms on average over {2} iterations ({3}ms faster than the maximum allowable {1}ms).",
                              averageDuration,
                              duration,
                              iterations,
                              duration - averageDuration));
            }
        }

        public static void CreateAndAddDefaultDataSeries(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {   
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "a" + i;
                datapoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);
        }

        public static void CreateAndAddDefaultDataSeriesForScrolling(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 20; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "a" + i;
                datapoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);
        }

        public static void CreateDefaultAxis(Chart chart)
        {
            Axis axisX = new Axis();
            Axis axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);
        }
    }
}
