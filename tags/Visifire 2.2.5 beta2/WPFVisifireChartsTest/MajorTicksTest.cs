using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Ticks class 
    /// </summary>
    [TestClass]
    public class TicksTest
    {
        #region Test default property value
         /// <summary>
        /// Check Enabled default value
        /// </summary>
        [TestMethod]
        public void CheckDefaultEnabled()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)tick.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LineThickness default value
        /// </summary>
        [TestMethod]
        public void CheckDefaultLineThickness()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0.5, tick.LineThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LineStyle default value
        /// </summary>
        [TestMethod]
        public void CheckDefaultLineStyle()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, tick.LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LineColor default value
        /// </summary>
        [TestMethod]
        public void CheckDefaultLineColor()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), tick.LineColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region Test new property value
        /// <summary>
        /// Check Enabled new value
        /// </summary>
        [TestMethod]
        public void CheckNewEnabled()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                tick.Enabled = true;
                Assert.IsTrue((Boolean)tick.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Opacity new value
        /// </summary>
        [TestMethod]
        public void CheckNewOpacity()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                tick.Opacity = 0.5;
                Assert.AreEqual(0.5, tick.Opacity, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LineThickness new value
        /// </summary>
        [TestMethod]
        public void CheckNewLineThickness()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                tick.LineThickness = 2;
                Assert.AreEqual(2, tick.LineThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LineStyle new value
        /// </summary>
        [TestMethod]
        public void CheckNewLineStyle()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                tick.LineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, tick.LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check LineColor new value
        /// </summary>
        [TestMethod]
        public void CheckNewLineColor()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                tick.LineColor = new SolidColorBrush(Colors.Blue);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), tick.LineColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check Interval new value
        /// </summary>
        [TestMethod]
        public void CheckNewInterval()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            Ticks tick = new Ticks();
            Axis axis = new Axis();
            axis.Ticks.Add(tick);
            chart.AxesX.Add(axis);
              
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                tick.Interval = 2;
                Assert.AreEqual(2, tick.Interval);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region TestTicksSerialization

        /// <summary>
        /// Testing Ticks Serialization
        /// </summary>
        [TestMethod]
        public void TestTicksSerialization()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Axis axis = new Axis();
            Ticks ticks = new Ticks();
            ticks.LineColor = new SolidColorBrush(Colors.Blue);
            axis.Ticks.Add(ticks);
            chart.AxesX.Add(axis);

            DataSeries ds = new DataSeries();
            DataPoint dp = new DataPoint();
            dp.YValue = 20;
            ds.DataPoints.Add(dp);
            chart.Series.Add(ds);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                MessageBox.Show(XamlWriter.Save(ticks));
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
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

        #endregion
    }
}
