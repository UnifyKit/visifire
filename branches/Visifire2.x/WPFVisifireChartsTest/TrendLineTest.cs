using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.TrendLine class 
    /// </summary>
    [TestClass]
    public class TrendLineTest
    {
        #region CheckTrendLineDefaultPropertyValue
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

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)trendLine.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Orientation
        /// </summary>
        [TestMethod]
        public void OrientationDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Orientation.Horizontal, trendLine.Orientation);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of AxisType
        /// </summary>
        [TestMethod]
        public void AxisTypeDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(AxisTypes.Primary, trendLine.AxisType);
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), trendLine.LineColor);
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(2, trendLine.LineThickness);
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, trendLine.LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        public void ShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse(trendLine.ShadowEnabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region CheckTrendLineNewPropertyValue
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.Enabled = true;
                Assert.IsTrue((Boolean)trendLine.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Orientation. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Orientation.")]
        [Owner("[....]")]
        public void OrientationNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.Value = 3;
                trendLine.Orientation = Orientation.Vertical;
                Assert.AreEqual(Orientation.Vertical, trendLine.Orientation);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of AxisType. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisType.")]
        [Owner("[....]")]
        public void AxisTypeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].AxisYType = AxisTypes.Secondary;
                trendLine.AxisType = AxisTypes.Secondary;
                Assert.AreEqual(AxisTypes.Secondary, trendLine.AxisType);
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.LineColor = new SolidColorBrush(Colors.Cyan);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Cyan), trendLine.LineColor);
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.LineThickness = 5;
                Assert.AreEqual(5, trendLine.LineThickness);
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

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);
                 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.LineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, trendLine.LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ShadowEnabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ShadowEnabled.")]
        [Owner("[....]")]
        public void ShadowEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.ShadowEnabled = true;
                Assert.IsTrue(trendLine.ShadowEnabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Value. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Value.")]
        [Owner("[....]")]
        public void ValueNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            TrendLine trendLine = TrendLineToTest;
            chart.TrendLines.Add(trendLine);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                trendLine.Orientation = Orientation.Vertical;
                trendLine.Value = 2;
                Assert.AreEqual(2, trendLine.Value);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region TestTrendLinesCollectionChanged
        /// <summary>
        /// Check the TrendLines collection changed. 
        /// </summary> 
        [TestMethod]
        [Description("Check the TrendLines collection changed.")]
        [Owner("[....]")]
        public void TestTrendLinesCollectionChanged()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Int32 trendLinesAdded = 0;

            _isLoaded = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            chart.TrendLines.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    trendLinesAdded += e.NewItems.Count;
                    Assert.AreEqual(1, e.NewItems.Count);
                }
            };

            TrendLine trendLine = new TrendLine();
            trendLine.Orientation = Orientation.Horizontal;
            trendLine.Value = 30;
            chart.TrendLines.Add(trendLine);

            trendLine = new TrendLine();
            trendLine.Orientation = Orientation.Vertical;
            trendLine.Value = 3;
            chart.TrendLines.Add(trendLine);

            trendLine = new TrendLine();
            trendLine.Orientation = Orientation.Vertical;
            trendLine.Value = 4;
            chart.TrendLines.Add(trendLine);

            trendLine = new TrendLine();
            trendLine.Orientation = Orientation.Horizontal;
            trendLine.Value = 60;
            chart.TrendLines.Add(trendLine);

            trendLine = new TrendLine();
            trendLine.Orientation = Orientation.Horizontal;
            trendLine.Value = 80;
            chart.TrendLines.Add(trendLine);

            trendLine = new TrendLine();
            trendLine.Orientation = Orientation.Vertical;
            trendLine.Value = 1;
            chart.TrendLines.Add(trendLine);
        
            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(6, trendLinesAdded);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        /// <summary>
        /// Instances that should be used across all TrendLine tests.
        /// </summary>
        private TrendLine TrendLineToTest
        {
            get { return new TrendLine() { Value = 60, Orientation = Orientation.Horizontal }; }
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
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}
