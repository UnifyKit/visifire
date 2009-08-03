using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Specialized;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Visifire.Charts;
using Visifire.Commons;


namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests for performance
    /// </summary>
    [TestClass]
    public class PerformanceTests : SilverlightControlTest
    {

        #region ColumnChartPerformanceTest
        /// <summary>
        /// Column chart performance test
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void ColumnChartPerformanceTest()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestingLineChartWithMarker
        /// <summary>
        /// Testing Line chart with Markers
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void TestingLineChartWithMarker()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Line;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestingLineChartWithOutMarker
        /// <summary>
        /// Testing Line chart without Markers
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void TestingLineChartWithOutMarker()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Line;
                dataSeries.MarkerEnabled = false;
                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region BarChartPerformanceChart
        /// <summary>
        /// Bar chart performance test
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void BarChartPerformanceChart()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;
                dataSeries.ShowInLegend = false;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(10, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region TestingChartWithoutBevel
        /// <summary>
        /// Testing chart without bevel
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void TestingChartWithoutBevel()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.Bevel = false;
                dataSeries.RenderAs = RenderAs.Column;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        #region UpdatingPropertyTest
        /// <summary>
        /// Updating DataPoint property test at runtime
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void UpdatingPropertyTest()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg1 = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataPoint.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
                    {
                        totalDuration = 0;
                        start = DateTime.UtcNow;

                        (sender as DataPoint).YValue = 80;

                        EnqueueConditional(() => { return ObservableObject._isPropertyChangedFired; });

                        EnqueueCallback(() =>
                        {
                            if (chart.ChartArea._renderCount > 0)
                            {
                                DateTime end = DateTime.UtcNow;
                                totalDuration = (end - start).TotalSeconds;
                                _htmlElement3.SetProperty("value", "Total Updation Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount + " Click here to exit.");
                            }

                            EnqueueCallback(() =>
                            {
                                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.PerformanceTest_OnClick));
                            });
                        });

                    };
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });
         
            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg1);
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetProperty("value", "Click on any DataPoint to update YValue");
                _htmlElement3.SetStyleAttribute("width", "900px");
                _htmlElement3.SetStyleAttribute("top", "560px");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);

            });
        }
        #endregion 

        #region TesingChartWithoutAxis
        /// <summary>
        /// Testing chart without axis
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void TesingChartWithoutAxis()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");
            if (_htmlElement1 != null && _htmlElement2 != null)
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            }

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.Enabled = false;
            chart.AxesX.Add(axisX);

            Axis axisY = new Axis();
            axisY.Enabled = false;
            chart.AxesY.Add(axisY);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;

                for (Int32 i = 0; i < 1000; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation : " + msg);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueCallback(() =>
            {
                EnqueueTestComplete();
            });
        }
        #endregion

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerformanceTest_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement3);             
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        #region private Data
        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement1;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement2;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement3;

        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}