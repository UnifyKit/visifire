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
    /// This class runs the unit tests Visifire.Charts.ChartGrid class 
    /// </summary>
    [TestClass]
    public class ChartGridTest
    {
        #region CheckChartGridDefaultPropertyValue
        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);

            Common.CreateAndAddDefaultDataSeries(chart);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)grid.Enabled);
                Assert.IsTrue((Boolean)grid.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LineThickness
        /// </summary>
        [TestMethod]
        public void LineThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0.25, grid.LineThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LineColor
        /// </summary>
        [TestMethod]
        public void LineColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), grid.LineColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LineStyle
        /// </summary>
        [TestMethod]
        public void LineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, grid.LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of InterlacedColor
        /// </summary>
        [TestMethod]
        public void InterlacedColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(grid.InterlacedColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region CheckChartGridNewPropertyValue
        /// <summary>
        /// Check the new value of Enabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Enabled.")]
        [Owner("[....]")]
        public void EnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            Axis axis = new Axis();
            ChartGrid grid = new ChartGrid();
            axis.Grids.Add(grid);
            chart.AxesX.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                grid.Enabled = true;
                Assert.IsTrue((Boolean)grid.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Interval. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Interval.")]
        [Owner("[....]")]
        public void IntervalNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                grid.Interval = 20;
                Assert.AreEqual(20, grid.Interval);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LineColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineColor.")]
        [Owner("[....]")]
        public void LineColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                grid.LineColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), grid.LineColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LineThickness. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineThickness.")]
        [Owner("[....]")]
        public void LineThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                grid.LineThickness = 1;
                Assert.AreEqual(1, grid.LineThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LineStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineStyle.")]
        [Owner("[....]")]
        public void LineStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                grid.LineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, grid.LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of InterlacedColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of InterlacedColor.")]
        [Owner("[....]")]
        public void InterlacedColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            ChartGrid grid = new ChartGrid();
            Axis axis = new Axis();
            axis.Grids.Add(grid);
            chart.AxesY.Add(axis);
            
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                grid.InterlacedColor = new SolidColorBrush(Colors.Gray);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), grid.InterlacedColor);
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
