using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Legend class 
    /// </summary>
    [TestClass]
    public class LegendTest
    {
        #region CheckLegendDefaultPropertyValue
        /// <summary>
        /// Check the default value of Href
        /// </summary>
        [TestMethod]
        public void HrefDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            
            if (_isLoaded)
            {
                Assert.IsNull(chart.Legends[0].Href);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Visifire.Commons.HrefTargets._self, chart.Legends[0].HrefTarget);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelMargin
        /// </summary>
        [TestMethod]
        public void LabelMarginDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(6, chart.Legends[0].LabelMargin);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(4), chart.Legends[0].Padding);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of HorizontalAlignment
        /// </summary>
        [TestMethod]
        public void HorizontalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(HorizontalAlignment.Center, chart.Legends[0].HorizontalAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of VerticalAlignment
        /// </summary>
        [TestMethod]
        public void VerticalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(VerticalAlignment.Bottom, chart.Legends[0].VerticalAlignment);
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

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), chart.Legends[0].BorderColor);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new Thickness(0.50), chart.Legends[0].BorderThickness);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.White), chart.Legends[0].Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of DockInsidePlotArea
        /// </summary>
        [TestMethod]
        public void DockInsidePlotAreaDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse(chart.Legends[0].DockInsidePlotArea);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Legends[0].Enabled);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.Legends[0].FontColor);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new FontFamily("Arial"), chart.Legends[0].FontFamily);
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

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(10, chart.Legends[0].FontSize, Common.HighPrecisionDelta);
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

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontStyles.Normal, chart.Legends[0].FontStyle);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontWeights.Normal, chart.Legends[0].FontWeight);
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

            CreateAndAddDefaultDataSeries(chart);
 
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue(chart.Legends[0].LightingEnabled);
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

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new CornerRadius(1), chart.Legends[0].CornerRadius);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Title
        /// </summary>
        [TestMethod]
        public void TitleDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.Legends[0].Title);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleALignmentX
        /// </summary>
        [TestMethod]
        public void TitleAlignmentXDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(HorizontalAlignment.Stretch, chart.Legends[0].TitleAlignmentX);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleTextALignment
        /// </summary>
        [TestMethod]
        public void TitleTextAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(TextAlignment.Center, chart.Legends[0].TitleTextAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleBackground
        /// </summary>
        [TestMethod]
        public void TitleBackgroundDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.LightGray), chart.Legends[0].TitleBackground);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontColor
        /// </summary>
        [TestMethod]
        public void TitleFontColorDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.Legends[0].TitleFontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontFamily
        /// </summary>
        [TestMethod]
        public void TitleFontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new FontFamily("Arial"), chart.Legends[0].TitleFontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontSize
        /// </summary>
        [TestMethod]
        public void TitleFontSizeDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(11, chart.Legends[0].TitleFontSize);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontStyle
        /// </summary>
        [TestMethod]
        public void TitleFontStyleDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontStyles.Normal, chart.Legends[0].TitleFontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontWeight
        /// </summary>
        [TestMethod]
        public void TitleFontWeightDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontWeights.Normal, chart.Legends[0].TitleFontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of EntryMargin
        /// </summary>
        [TestMethod]
        public void EntryMarginDefaultValue()
        {
            Chart chart = new Chart();
            
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(3, chart.Legends[0].EntryMargin);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region CheckNewPropertyValue
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.HrefTarget = HrefTargets._blank;
                legend.Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", legend.Href);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelMargin. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LabelMargin.")]
        [Owner("[....]")]
        public void LabelMarginNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.LabelMargin = 10;
                Assert.AreEqual(10, legend.LabelMargin);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of EntryMargin. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of EntryMargin.")]
        [Owner("[....]")]
        public void EntryMarginNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.LabelMargin = 10;
                Assert.AreEqual(10, legend.LabelMargin);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of VerticalAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of VerticalAlignment.")]
        [Owner("[....]")]
        public void VerticalAlignmentNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.VerticalAlignment = VerticalAlignment.Top;
                Assert.AreEqual(VerticalAlignment.Top, legend.VerticalAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of HorizontalAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of HorizontalAlignment.")]
        [Owner("[....]")]
        public void HorizontalAlignmentNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                Assert.AreEqual(HorizontalAlignment.Left, legend.HorizontalAlignment);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }


        /// <summary>
        /// Check the new value of HorizontalAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of BorderThickness.")]
        [Owner("[....]")]
        public void BorderThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.BorderColor = new SolidColorBrush(Colors.Red);
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.BorderThickness = new Thickness(2);
                Assert.AreEqual(new Thickness(2), legend.BorderThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of BorderColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of BorderColor.")]
        [Owner("[....]")]
        public void BorderColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.BorderThickness = new Thickness(1);
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.BorderColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.BorderColor);
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart); 

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Background = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Background. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of DockInsidePlotArea.")]
        [Owner("[....]")]
        public void DockInsidePlotAreaNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.DockInsidePlotArea = true;
                Assert.IsTrue(legend.DockInsidePlotArea);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Cursor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Cursor.")]
        [Owner("[....]")]
        public void CursorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Cursor = Cursors.Hand;
                Assert.AreEqual(Cursors.Hand, legend.Cursor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Enabled = false;
                Assert.AreEqual(false, legend.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontColor.")]
        [Owner("[....]")]
        public void FontColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.FontColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.FontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontSize. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontSize.")]
        [Owner("[....]")]
        public void FontSizeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.FontSize = 14;
                Assert.AreEqual(14, legend.FontSize, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontFamily. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontFamily.")]
        [Owner("[....]")]
        public void FontFamilyNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.FontFamily = new FontFamily("Times New Roman");
                Assert.AreEqual(new FontFamily("Times New Roman"), legend.FontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontStyle.")]
        [Owner("[....]")]
        public void FontStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.FontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, legend.FontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of FontWeight. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontWeight.")]
        [Owner("[....]")]
        public void FontWeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.FontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, legend.FontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of lightingEnabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LightingEnabled.")]
        [Owner("[....]")]
        public void LightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Background = new SolidColorBrush(Colors.Red);
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.LightingEnabled = true;
                Assert.IsTrue(legend.LightingEnabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Padding. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Padding.")]
        [Owner("[....]")]
        public void PaddingNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Padding = new Thickness(12);
                Assert.AreEqual(new Thickness(12), legend.Padding);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Margin. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Margin.")]
        [Owner("[....]")]
        public void MarginNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Margin = new Thickness(10);
                Assert.AreEqual(new Thickness(10), legend.Margin);
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.CornerRadius = new CornerRadius(5);
                Assert.AreEqual(new CornerRadius(5), legend.CornerRadius);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Title. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Title.")]
        [Owner("[....]")]
        public void TitleRadiusNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Title = "Legend";
                Assert.AreEqual("Legend", legend.Title);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// Check the new value of TitleAlignmentX. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleAlignmentX.")]
        [Owner("[....]")]
        public void TitleAlignmentXNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleAlignmentX = HorizontalAlignment.Right;
                Assert.AreEqual(HorizontalAlignment.Right, legend.TitleAlignmentX);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of TitleBackground. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleBackground.")]
        [Owner("[....]")]
        public void TitleBackgroundNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleBackground = new SolidColorBrush(Colors.Gray);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), legend.TitleBackground);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of TitleFontColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontColor.")]
        [Owner("[....]")]
        public void TitleFontColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleFontColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.TitleFontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of TitleFontSize. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontSize.")]
        [Owner("[....]")]
        public void TitleFontSizeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleFontSize = 15;
                Assert.AreEqual(15, legend.TitleFontSize);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of TitleFontFamily. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontFamily.")]
        [Owner("[....]")]
        public void TitleFontFamilyNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleFontFamily = new FontFamily("Times New Roman");
                Assert.AreEqual(new FontFamily("Times New Roman"), legend.TitleFontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of TitleFontStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontStyle.")]
        [Owner("[....]")]
        public void TitleFontStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleFontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, legend.TitleFontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of TitleFontWeight. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontWeight.")]
        [Owner("[....]")]
        public void TitleFontWeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.TitleFontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, legend.TitleFontWeight);
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);
             
            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.ToolTipText = "Legend ToolTip";
                Assert.AreEqual("Legend ToolTip", legend.ToolTipText);
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                legend.Opacity = 0.5;
                Assert.AreEqual(0.5, legend.Opacity, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region TestLegendSerialization

        /// <summary>
        /// Testing Legend Serialization
        /// </summary>
        [TestMethod]
        public void TestLegendSerialization()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Legend legend = new Legend();
            legend.Background = new SolidColorBrush(Colors.Aqua);
            chart.Legends.Add(legend);

            DataSeries ds = new DataSeries();
            ds.ShowInLegend = true;
            DataPoint dp = new DataPoint();
            dp.YValue = 20;
            ds.DataPoints.Add(dp);
            chart.Series.Add(ds);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                MessageBox.Show(XamlWriter.Save(legend));
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        /// <summary>
        /// Create DataSeries with Legend
        /// </summary>
        /// <param name="chart"></param>
        public void CreateAndAddDefaultDataSeries(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.ShowInLegend = true;

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "a" + i;
                dataPoint.YValue = rand.Next(0, 100);
                dataPoint.XValue = i + 1;
                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);
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
