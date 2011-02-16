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
using System.Windows.Shapes;
using System.Globalization;
using Visifire.Charts;
using Visifire.Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Browser;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// Common testing utilities and constants.
    /// </summary> 
    public static class Common
    {
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

            if (first.GetType().Name == "SolidColorBrush")
            {
                // Do a field by field comparison if they're not the same reference
                SolidColorBrush firstSolidColorBrush = first as SolidColorBrush;
                if (firstSolidColorBrush != null)
                {
                    SolidColorBrush secondSolidColorBrush = second as SolidColorBrush;
                    if (secondSolidColorBrush != null)
                    {
                        return object.Equals(firstSolidColorBrush.Color, secondSolidColorBrush.Color);
                    }
                }
            }
            else
            {
                // Do a field by field comparison if they're not the same reference
                LinearGradientBrush firstLinearColorBrush = first as LinearGradientBrush;
                if (firstLinearColorBrush != null)
                {
                    LinearGradientBrush secondLinearColorBrush = second as LinearGradientBrush;
                    if (secondLinearColorBrush != null)
                    {
                        Boolean stop1 = object.Equals(firstLinearColorBrush.GradientStops[0].Color, secondLinearColorBrush.GradientStops[0].Color);
                        Boolean stop2 = object.Equals(firstLinearColorBrush.GradientStops[1].Color, secondLinearColorBrush.GradientStops[1].Color);
                        return object.Equals(stop1, stop2);
                    }
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
        /// Create a reference to the element that will be added to the testing
        /// surface and then removed when the reference is disposed.
        /// </summary> 
        /// <param name="element">Element create the reference for.</param> 
        /// <returns>LiveReference to track the element.</returns>
        public static LiveReference CreateLiveReference(this UIElement element, Microsoft.Silverlight.Testing.SilverlightTest testReference)
        {
            return new LiveReference(testReference, element);
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
                    "Actions completed in {0}ms on average over {2} iterations ({3}ms longer than the maximum allowable {1}ms).",
                    averageDuration,
                    duration,
                    iterations,
                    averageDuration - duration));
            }
            else
            {
                return (string.Format(CultureInfo.InvariantCulture,
                          "Actions completed in {0}ms on average over {2} iterations ({3}ms faster than the maximum allowable {1}ms).",
                          averageDuration,
                          duration,
                          iterations,
                          duration - averageDuration));
            }

        }

        /// <summary>
        /// Create and add DataSeries
        /// </summary>
        /// <param name="chart">Chart</param>
        public static void CreateAndAddDefaultDataSeries(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "abc" + i;
                datapoint.YValue = rand.Next(0, 100);
                datapoint.XValue = i + 1;
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);
        }

        /// <summary>
        /// Create and add DataSeries
        /// </summary>
        /// <param name="chart">Chart</param>
        public static void CreateAndAddDefaultDataSeriesWithLargeNoOfDps(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            Random rand = new Random();

            for (Int32 i = 0; i < 1000; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "abc" + i;
                datapoint.YValue = rand.Next(0, 100);
                datapoint.XValue = i + 1;
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);
        }


        /// <summary>
        /// Create and add DataSeries
        /// </summary>
        /// <param name="chart">Chart</param>
        public static void CreateAndAddDefaultDataSeries4Sampling(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            Random rand = new Random();

            for (Int32 i = 0; i < 50; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "abc" + i;
                datapoint.YValue = rand.Next(0, 100);
                datapoint.XValue = i + 1;
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);
        }

        public static void CreateAndAddDefaultDateTimeAxis(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();
            Random rn = new Random(DateTime.Now.Second);

            DateTime dateTime = DateTime.Now;

            for (int i = 0; i < 30; i++)
            {

                DataPoint datapoint = new DataPoint();
                datapoint.XValue = dateTime;
                datapoint.YValue = rn.Next(10, 100);
                dataSeries.DataPoints.Add(datapoint);
                dateTime = dateTime.AddDays(1);
            }
            chart.Series.Add(dataSeries);
        }

        public static void CreateDefaultDataSeries4FinancialCharts(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            Random rand = new Random();

            for (Int32 i = 0; i < 6; i++)
            {
                Double open = rand.Next(50, 60);
                Double close = open + rand.Next(-10, 10);
                Double high = (open > close ? open : close) + 10;
                Double low = (open < close ? open : close) - 5;

                DataPoint dp = new DataPoint();
                dp.YValues = new Double[] { open, close, high, low };
                dataSeries.DataPoints.Add(dp);
            }

            chart.Series.Add(dataSeries);
        }

        /// <summary>
        /// Create and add DataSeries to scrollable chart
        /// </summary>
        /// <param name="chart">Chart</param>
        public static void CreateAndAddDataSeriesWithMoreDataPoints(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 25; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "a" + i;
                datapoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);
        }

        /// <summary>
        /// Create default Axes
        /// </summary>
        /// <param name="chart">Chart</param>
        public static void CreateDefaultAxis(Chart chart)
        {
            Axis axisX = new Axis();
            Axis axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);
        }

        /// <summary>
        /// Create a html button to display custom messages
        /// </summary>
        /// <param name="htmlElement"></param>
        /// <returns></returns>
        public static System.Windows.Browser.HtmlElement GetDisplayMessageButton(System.Windows.Browser.HtmlElement htmlElement)
        {
            htmlElement = System.Windows.Browser.HtmlPage.Document.CreateElement("input");
            htmlElement.SetProperty("id", "inputText");
            htmlElement.SetProperty("type", "button");
            htmlElement.SetStyleAttribute("border", "solid 1px black");
            htmlElement.SetStyleAttribute("position", "absolute");
            htmlElement.SetStyleAttribute("width", "900px");
            htmlElement.SetStyleAttribute("height", "20px");
            htmlElement.SetStyleAttribute("top", "520px");
            htmlElement.SetStyleAttribute("left", "0px");

            return htmlElement;
        }
        
        public static void SetSLPluginHeight(Double height)
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", height.ToString() + "px");
        }

        public static void OnTestCompleted(SilverlightControlTest testClass)
        {
           
            testClass.EnqueueTestComplete();
          
            RemoveMessageButton(TestHtmlButton);

           
        }

        public static void EnableAutoTimerCallBack(SilverlightControlTest testClass, TimeSpan interval, TimeSpan duration, params Action[] actions)
        {
            TestingTimer.Interval = interval;
            TestingTimer.Duration = duration;

            TestingTimer.Tick += delegate
            {
                testClass.EnqueueCallback(actions);
            };

            TestingTimer.OnStop += delegate
            {   
                OnTestCompleted(testClass);
            };

            testClass.EnqueueCallback(() => TestingTimer.Start());
        }

        public static void AddMessageButton(SilverlightControlTest testClass, String content)
        {
            testClass.EnqueueCallback(() =>
            {   
                TestHtmlButton = Common.GetDisplayMessageButton(TestHtmlButton);
                TestHtmlButton.SetStyleAttribute("width", "900px");
                TestHtmlButton.SetProperty("value", content);
                TestHtmlButton.AttachEvent("onclick", delegate(object obj, HtmlEventArgs args) { OnTestCompleted(testClass); });
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(TestHtmlButton);
            });
        }

        public static void RemoveMessageButton(HtmlElement button)
        {
            try
            {   
                if (button != null)
                {
                    System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(button);
                    System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(button);
                    System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(button);
                }
                System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
            }
            catch { }
        }

        private static HtmlElement TestHtmlButton;
        private static TestingTimer TestingTimer = new TestingTimer();

     
    }
    
    public class TestingTimer : System.Windows.Threading.DispatcherTimer
    {   
        public Nullable<TimeSpan> Duration
        {
            get;
            set;
        }

        public new event EventHandler Tick;
        public event EventHandler OnStop;

         public new void Stop()
         {
             base.Stop();

             if(IsRunning)
                OnStop(this, null);

             IsRunning = false;
         }

        public new void Start()
        {
            _timerStartTime = DateTime.Now;
            base.Start();
            IsRunning = true;
            base.Tick -= TestingTimer_Tick;
            base.Tick += new EventHandler(TestingTimer_Tick);
        }

        void TestingTimer_Tick(object sender, EventArgs e)
        {
            Tick(sender, e);

            if (Duration != null && (DateTime.Now - _timerStartTime) >= Duration)
                this.Stop();
        }

        DateTime _timerStartTime;
        
        public Boolean IsRunning;
    }
}
