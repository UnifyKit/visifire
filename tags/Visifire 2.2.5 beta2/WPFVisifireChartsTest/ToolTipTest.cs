using System;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// Summary description for ToolTipTest
    /// </summary>
    [TestClass]
    public class ToolTipTest
    {
        #region CheckToolTipDefaultPropertyValue
        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.ToolTips[0].Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontColor
        /// </summary>
        [TestMethod]
        public void FontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Color.FromArgb(0xFF, 0x2E, 0x2D, 0x2D)), chart.ToolTips[0].FontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontSize
        /// </summary>
        [TestMethod]
        public void FontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
           
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(12, chart.ToolTips[0].FontSize, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontFamily
        /// </summary>
        [TestMethod]
        public void FontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new FontFamily("Portable User Interface"), chart.ToolTips[0].FontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontStyle
        /// </summary>
        [TestMethod]
        public void FontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontStyles.Normal, chart.ToolTips[0].FontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontWeight
        /// </summary>
        [TestMethod]
        public void FontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontWeights.Normal, chart.ToolTips[0].FontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Background
        /// </summary>
        [TestMethod]
        public void BackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
          
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();

            LinearGradientBrush linearGrad = new LinearGradientBrush();
            linearGrad.GradientStops = new GradientStopCollection();
            GradientStop gradientStop = new GradientStop();
            gradientStop.Color = Color.FromArgb(0xFF, 0xC8, 0xC8, 0xC4);
            gradientStop.Offset = 0.156;
            linearGrad.GradientStops.Add(gradientStop);
            gradientStop = new GradientStop();
            gradientStop.Color = Color.FromArgb(0xF0, 0xFF, 0xFF, 0xFF);
            gradientStop.Offset = 1;
            linearGrad.GradientStops.Add(gradientStop);
            linearGrad.Opacity = 0.9;
            linearGrad.StartPoint = new Point(0.5, 0);
            linearGrad.StartPoint = new Point(0.5, 1);

            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(linearGrad, chart.ToolTips[0].Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of BorderBrush
        /// </summary>
        [TestMethod]
        public void BorderBrushDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
          
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00)), chart.ToolTips[0].BorderBrush);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of BorderThickness
        /// </summary>
        [TestMethod]
        public void BorderThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(0.25, 0.25, 0.25, 1), chart.ToolTips[0].BorderThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Padding
        /// </summary>
        [TestMethod]
        public void PaddingDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(10, 5, 10, 3), chart.ToolTips[0].Padding);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckToolTipNewPropertyValue
        /// <summary>
        /// Check the new value of Enabled
        /// </summary>
        [TestMethod]
        public void EnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].Enabled = false;
                Assert.IsFalse((Boolean)chart.ToolTips[0].Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontColor
        /// </summary>
        [TestMethod]
        public void FontColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].FontColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.ToolTips[0].FontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontSize
        /// </summary>
        [TestMethod]
        public void FontSizeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].FontSize = 18;
                Assert.AreEqual(18, chart.ToolTips[0].FontSize, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontFamily
        /// </summary>
        [TestMethod]
        public void FontFamilyNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].FontFamily = new FontFamily("Arial");
                Assert.AreEqual(new FontFamily("Arial"), chart.ToolTips[0].FontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontStyle
        /// </summary>
        [TestMethod]
        public void FontStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].FontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, chart.ToolTips[0].FontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontWeight
        /// </summary>
        [TestMethod]
        public void FontWeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].FontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, chart.ToolTips[0].FontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Background
        /// </summary>
        [TestMethod]
        public void BackgroundNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();

            LinearGradientBrush linearGrad = new LinearGradientBrush();
            linearGrad.GradientStops = new GradientStopCollection();
            GradientStop gradientStop = new GradientStop();
            gradientStop.Color = Color.FromArgb(0xFF, 0xD8, 0xD8, 0xC8);
            gradientStop.Offset = 0.156;
            linearGrad.GradientStops.Add(gradientStop);
            gradientStop = new GradientStop();
            gradientStop.Color = Color.FromArgb(0xF0, 0xEE, 0xEF, 0xFE);
            gradientStop.Offset = 1;
            linearGrad.GradientStops.Add(gradientStop);
            linearGrad.Opacity = 0.9;
            linearGrad.StartPoint = new Point(0.5, 0);
            linearGrad.StartPoint = new Point(0.5, 1);

            if (_isLoaded)
            {
                chart.ToolTips[0].Background = linearGrad;
                Common.AssertBrushesAreEqual(linearGrad, chart.ToolTips[0].Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of BorderBrush
        /// </summary>
        [TestMethod]
        public void BorderBrushNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].BorderBrush = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.ToolTips[0].BorderBrush);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of BorderThickness
        /// </summary>
        [TestMethod]
        public void BorderThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].BorderThickness = new Thickness(2);
                Assert.AreEqual(new Thickness(2), chart.ToolTips[0].BorderThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Padding
        /// </summary>
        [TestMethod]
        public void PaddingNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.ToolTips[0].Padding = new Thickness(2);
                Assert.AreEqual(new Thickness(2), chart.ToolTips[0].Padding);
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
