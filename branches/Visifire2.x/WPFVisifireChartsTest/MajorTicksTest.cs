using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Summary description for MajorTicksTest
    /// </summary>
    [TestClass]
    public class MajorTicksTest
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                tick.Enabled = true;
                Assert.IsTrue((Boolean)tick.Enabled);
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
              
            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                tick.Interval = 2;
                Assert.AreEqual(2, tick.Interval);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region Private Data

        bool isLoaded = false;

        #endregion
    }
}
