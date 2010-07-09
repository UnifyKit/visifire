using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.PlotArea class 
    /// </summary>
    [TestClass]
    public class PlotAreaTest
    {
        #region CheckPlotAreaDefaultPropertyValue
        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        public void BevelDefaultValue()
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
                Assert.IsFalse(chart.PlotArea.Bevel);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of BorderColor
        /// </summary>
        [TestMethod]
        public void BorderColorDefaultValue()
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
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.PlotArea.BorderColor);
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
                Assert.AreEqual(new Thickness(0), chart.PlotArea.BorderThickness);
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
            if (_isLoaded)
            {
                Assert.IsNull(chart.PlotArea.Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Href
        /// </summary>
        [TestMethod]
        public void HrefDefaultValue()
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
                Assert.IsNull(chart.PlotArea.Href);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of HrefTarget
        /// </summary>
        [TestMethod]
        public void HrefTargetDefaultValue()
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
                Assert.AreEqual(Visifire.Commons.HrefTargets._self, chart.PlotArea.HrefTarget);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LightingEnabled
        /// </summary>
        [TestMethod]
        public void LightingEnabledDefaultValue()
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
                Assert.IsFalse(chart.PlotArea.LightingEnabled);
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
                Assert.IsTrue((Boolean)chart.PlotArea.ShadowEnabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of CornerRadius
        /// </summary>
        [TestMethod]
        public void CornerRadiusDefaultValue()
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
                Assert.AreEqual(new CornerRadius(0), chart.PlotArea.CornerRadius);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ToolTipText
        /// </summary>
        [TestMethod]
        public void ToolTipTextDefaultValue()
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
                Assert.IsNotNull(chart.PlotArea.ToolTipText);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region CheckPlotAreaNewPropertyValue
        
        /// <summary>
        /// Check the new value of Bevel. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Bevel.")]
        [Owner("[....]")]
        public void BevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.Background = new SolidColorBrush(Colors.Red);
                chart.PlotArea.Bevel = true;
                Assert.IsTrue(chart.PlotArea.Bevel);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Opacity. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Opacity.")]
        [Owner("[....]")]
        public void OpacityNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.Opacity = 0.5;
                Assert.AreEqual(0.5, chart.PlotArea.Opacity, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Bevel. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of BorderColor.")]
        [Owner("[....]")]
        public void BorderColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.BorderThickness = new Thickness(1);
                chart.PlotArea.BorderColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.PlotArea.BorderColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of BorderThickness. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of BorderThickness.")]
        [Owner("[....]")]
        public void BorderThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.BorderThickness = new Thickness(2);
                Assert.AreEqual(new Thickness(2), chart.PlotArea.BorderThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Background. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Background.")]
        [Owner("[....]")]
        public void BackgroundNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.Background = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.PlotArea.Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Href. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Href.")]
        [Owner("[....]")]
        public void HrefNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", chart.PlotArea.Href);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of HrefTarget. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of HrefTarget.")]
        [Owner("[....]")]
        public void HrefTargetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.HrefTarget = HrefTargets._blank;
                Assert.AreEqual(HrefTargets._blank, chart.PlotArea.HrefTarget);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }


        /// <summary>
        /// Check the new value of LightingEnabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LightingEnabled.")]
        [Owner("[....]")]
        public void LightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.Background = new SolidColorBrush(Colors.Red);
                chart.PlotArea.LightingEnabled = true;
                Assert.IsTrue(chart.PlotArea.LightingEnabled);
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
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.ShadowEnabled = true;
                Assert.IsTrue((Boolean)chart.PlotArea.ShadowEnabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of CornerRadius. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of CornerRadius.")]
        [Owner("[....]")]
        public void CornerRadiusNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.BorderThickness = new Thickness(1);
                chart.PlotArea.CornerRadius = new CornerRadius(5);
                Assert.AreEqual(new CornerRadius(5), chart.PlotArea.CornerRadius);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ToolTipText.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ToolTipText.")]
        [Owner("[....]")]
        public void ToolTipTextNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
               
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.PlotArea.ToolTipText = "ToolTip";
                Assert.AreEqual("ToolTip", chart.PlotArea.ToolTipText);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region TestPlotAreaSerialization

        /// <summary>
        /// Testing PlotArea Serialization
        /// </summary>
        [TestMethod]
        public void TestPlotAreaSerialization()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            PlotArea plotArea = new PlotArea();
            plotArea.Background = new SolidColorBrush(Colors.Aqua);
            chart.PlotArea = plotArea;

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
                MessageBox.Show(XamlWriter.Save(plotArea));
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
