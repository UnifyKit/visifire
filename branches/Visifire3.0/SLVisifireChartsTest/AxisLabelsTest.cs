using System;
using System.Collections.Generic;
using Microsoft.Silverlight.Testing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.AxisLabels class 
    /// </summary>
    [TestClass]
    public class AxisLabelsTest : SilverlightControlTest
    {
        #region CheckDefaultPropertyValues
        /// <summary>
        /// Check the default value of Angle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Angle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AngleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].AxisLabels.Angle),
                () => Assert.AreEqual(Double.NaN, chart.AxesY[0].AxisLabels.Angle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Interval. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Interval.")]
        [Owner("[....]")]
        [Asynchronous]
        public void IntervalDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].AxisLabels.Interval));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontSize. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(10, chart.AxesX[0].AxisLabels.FontSize),
                () => Assert.AreEqual(10, chart.AxesY[0].AxisLabels.FontSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Rows. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Rows.")]
        [Owner("[....]")]
        [Asynchronous]
        public void RowsDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.AxesX[0].AxisLabels.Rows));

            EnqueueTestComplete();
        }

        #endregion

        #region CheckNewPropertyValue
        
        /// <summary>
        /// Check the New value for Angle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Angle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AngleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.Angle = -30;
            _axisY.AxisLabels.Angle = 30;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(-30, _axisX.AxisLabels.Angle),
                () => Assert.AreEqual(30, _axisY.AxisLabels.Angle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for Interval.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Interval.")]
        [Owner("[....]")]
        [Asynchronous]
        public void IntervalNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.Interval = 2;
            _axisY.AxisLabels.Interval = 20;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(2, _axisX.AxisLabels.Interval),
                () => Assert.AreEqual(20, _axisY.AxisLabels.Interval));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for Opacity.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Opacity.")]
        [Owner("[....]")]
        [Asynchronous]
        public void OpacityNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.Opacity = 0.5;
            _axisY.AxisLabels.Opacity = 0.5;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(0.5, _axisX.AxisLabels.Opacity, Common.HighPrecisionDelta),
                () => Assert.AreEqual(0.5, _axisY.AxisLabels.Opacity, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontSize.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontSizeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.FontSize = 14;
            _axisY.AxisLabels.FontSize = 14;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(14, _axisX.AxisLabels.FontSize),
                () => Assert.AreEqual(14, _axisY.AxisLabels.FontSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontFamily.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontFamily.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontFamilyNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.FontFamily = new FontFamily("Arial");
            _axisY.AxisLabels.FontFamily = new FontFamily("Arial");

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(new FontFamily("Arial"), _axisX.AxisLabels.FontFamily),
                () => Assert.AreEqual(new FontFamily("Arial"), _axisY.AxisLabels.FontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontColor.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
            _axisY.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), _axisX.AxisLabels.FontColor),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), _axisY.AxisLabels.FontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.FontStyle = FontStyles.Italic;
            _axisY.AxisLabels.FontStyle = FontStyles.Italic;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontStyles.Italic, _axisX.AxisLabels.FontStyle),
                () => Assert.AreEqual(FontStyles.Italic, _axisY.AxisLabels.FontStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontWeight.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontWeight.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontWeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisLabels.FontWeight = FontWeights.Bold;
            _axisY.AxisLabels.FontWeight = FontWeights.Bold;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontWeights.Bold, _axisX.AxisLabels.FontWeight),
                () => Assert.AreEqual(FontWeights.Bold, _axisY.AxisLabels.FontWeight));

            EnqueueTestComplete();
        }
        /// <summary>
        /// Check the New value for Rows.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Rows.")]
        [Owner("[....]")]
        [Asynchronous]
        public void RowsNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();

            _axisX.AxisLabels.Rows = 2;
            chart.AxesX.Add(_axisX);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueDelay(_sleepTime);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(2, _axisX.AxisLabels.Rows));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for TextWrap.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for TextWrap.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TextWrapNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();

            _axisX.AxisLabels.TextWrap = 0.5;
            _axisX.AxisLabels.Angle = 0;
            _axisX.Interval = 1;
            _axisX.AxisLabels.Rows = 1;
            chart.AxesX.Add(_axisX);

            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "VisifireSilverlight" + i;
                datapoint.YValue = rand.Next(0, 100);
                datapoint.XValue = i + 1;
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(0.5, _axisX.AxisLabels.TextWrap, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }
        #endregion

        #region AxisLabelTest
        /// <summary>
        /// Test number of AxisLabel added to Chart
        /// </summary>
        [TestMethod]
        [Description("")]
        [Asynchronous]
        public void TestNumberOfAxisLabel()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 15; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.XValue = i + 1;
                datapoint.AxisXLabel = "Visifire" + i;
                datapoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(datapoint);
            }
            chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                Assert.AreEqual(15, chart.AxesX[0].AxisLabels.AxisLabelList.Count);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region AxisLabel Performance Tests
        /// <summary>
        /// Test number of AxisLabel added to Chart
        /// </summary>
        [TestMethod]
        [Description("")]
        [Asynchronous]
        public void CheckAxisLabelsAutoPlacement1()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
            chart.AxesX.Add(axisX);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            Int32 numberOfDataPoint = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(25, 1, delegate
            {
                for (Int32 i = 0; i < 60; i++)
                {
                    DataPoint datapoint = new DataPoint();
                    datapoint.AxisXLabel = "Label" + i;
                    datapoint.YValue = rand.Next(0, 100);
                    dataSeries.DataPoints.Add(datapoint);
                    numberOfDataPoint++;
                }
                chart.Series.Add(dataSeries);

            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", numberOfDataPoint + " AxisLabels are added. Click here to exit.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", msg + " Total Chart Loading Time: " + totalDuration + "s");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        /// <summary> 
        /// Stress test AxisLabel creation.
        /// </summary>
        [TestMethod]
        [Description("Stress test AxisLabel creation.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckAxisLabelsAutoPlacement2()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
            chart.AxesX.Add(axisX);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            
            Random rand = new Random();

            Int32 numberOfDataPoint = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(80, 1, delegate
            {
                for (Int32 i = 0; i < 500; i++)
                {
                    DataPoint datapoint = new DataPoint();
                    datapoint.AxisXLabel = "Visifire Label" + i;
                    datapoint.YValue = rand.Next(0, 100);
                    dataSeries.DataPoints.Add(datapoint);
                    numberOfDataPoint++;
                }

                chart.Series.Add(dataSeries);

            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", numberOfDataPoint + " AxisLabels are added. Click here to exit.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", msg + " Total Chart Loading Time: " + totalDuration + "s");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        #endregion

        #region TestAxisDecimalIntervalWithAxisXLabel
        /// <summary>
        /// Test Axis Interval in decimal if AxisXLabel is set in DataPoints
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestAxisDecimalIntervalWithAxisXLabel()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            for (Int32 i = 0; i < 10; i++)
                dataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = "Visifire", YValue = rand.Next(10, 100) });
            chart.Series.Add(dataSeries);

            EnqueueCallback(() =>
            {
                Axis axis = new Axis();
                axis.Interval = 0.1;
                chart.AxesX.Add(axis);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region TestBiggerAxisLabels
        /// <summary>
        /// Test bigger AxisLabels
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestBiggerAxisLabels()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;
                for (Int32 i = 0; i < 6; i++)
                    dataSeries.DataPoints.Add(new DataPoint() { AxisXLabel = "Visifire Chart Visifire Chart Visifire Chart", YValue = rand.Next(10, 100) });
                chart.Series.Add(dataSeries);
            });

            EnqueueDelay(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HtmlElement_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        #region Private Data
        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement1;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement2;
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 2000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        /// <summary>
        /// axisX reference
        /// </summary>
        private Axis _axisX;

        /// <summary>
        /// AxisY reference
        /// </summary>
        private Axis _axisY;

        #endregion
    }
}
