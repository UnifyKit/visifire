using System;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests for performance
    /// </summary>
    [TestClass]
    public class PerformanceTests
    {
        #region ColumnChartPerformanceTest
        /// <summary>
        /// Column chart performance test
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void ColumnChartPerformanceTest()
        {
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(100, 1, delegate
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

            window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n"  + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

        #region BarChartPerformanceTest
        /// <summary>
        /// Bar chart performance test
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void BarChartPerformanceTest()
        {
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axisX = new Axis();
            axisX.Interval = 1;
            chart.AxesX.Add(axisX);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(100, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Bar;

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

            window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n"  + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

        #region TestingChartWithoutAxis
        /// <summary>
        /// Testing chart without axis
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestingChartWithoutAxis()
        {
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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

            String msg = Common.AssertAverageDuration(100, 1, delegate
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

            window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n"  + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

        #region LineChartWithMarkers
        /// <summary>
        /// Testing Line chart with Markers
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void LineChartWithMarkers()
        {
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(100, 1, delegate
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

            window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n"  + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

        #region LineChartWithOutMarkers
        /// <summary>
        /// Testing Line chart without Markers
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void LineChartWithOutMarkers()
        {
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(100, 1, delegate
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

            window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n"  + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

        #region TestingChartWithoutBevel
        /// <summary>
        /// Testing chart without bevel
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestingChartWithoutBevel()
        {
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;
            Int32 numberofDataPoint = 0;

            String msg = Common.AssertAverageDuration(100, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;
                dataSeries.Bevel = false;
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

            window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n"  + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

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
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;
        
        /// <summary>
        /// Reference for Window
        /// </summary>
        private Window window;

        #endregion
    }
}
