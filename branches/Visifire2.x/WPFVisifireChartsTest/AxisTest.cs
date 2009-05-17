using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Axis class 
    /// </summary>
    [TestClass]
    public class AxisTest
    {

        #region CheckChartAxisInXaml

        /// <summary>
        /// Check the Axis in Chart via XAML Markup
        /// </summary>
        [TestMethod]
        public void ChartAxisViaXaml()
        {
            Object result = XamlReader.Load(new XmlTextReader(new StringReader(Resource.Chart_AxisXaml)));
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Convert.ToString(1), chart.AxesX[0].AxisMinimum);
                Assert.AreEqual(Convert.ToString(100), chart.AxesX[0].AxisMaximum);
                Assert.AreEqual(Convert.ToString(0), chart.AxesY[0].AxisMinimum);
                Assert.AreEqual(Convert.ToString(200), chart.AxesY[0].AxisMaximum);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        #endregion

        #region CheckDefaultPropertyValues
        /// <summary>
        /// Check the default value of Background. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Background.")]
        [Owner("[....]")]
        public void BackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].Background);
                Assert.IsNull(chart.AxesY[0].Background);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ScrollBarOffset for vertical chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ScrollBarOffset.")]
        [Owner("[....]")]
        public void ScrollBarOffsetVerticalChartDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Double.NaN, chart.AxesX[0].ScrollBarOffset);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        ///// <summary>
        ///// Check the default value of Scale.
        ///// </summary> 
        //[TestMethod]
        //[Description("Check the default value of Scale.")]
        //[Owner("[....]")]
        //public void ScaleDefaultValue()
        //{
        //    Chart chart = new Chart();
        //    chart.Width = 400;
        //    chart.Height = 300;

        //    Common.CreateAndAddDefaultDataSeries(chart);

        //    _isLoaded = false;
        //    chart.Loaded += new RoutedEventHandler(chart_Loaded);

        //    Window window = new Window();
        //    window.Content = chart;
        //    window.Show();
        //    if (_isLoaded)
        //    {
        //        Assert.AreEqual(Double.NaN, chart.AxesX[0].Scale);
        //    }

        //    window.Dispatcher.InvokeShutdown();
        //    window.Close();
        //}

        /// <summary>
        /// Check the default value of Href.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Href.")]
        [Owner("[....]")]
        public void HrefAndHrefTargetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(_axisX.Href);
                Assert.IsNull(_axisY.Href);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ScrollBarOffset for horizontal chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ScrollBarOffset.")]
        [Owner("[....]")]
        public void ScrollBarOffsetHorizontalChartDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Double.NaN, chart.AxesX[0].ScrollBarOffset);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Enabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Enabled.")]
        [Owner("[....]")]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.AxesX[0].Enabled);
                Assert.IsTrue((Boolean)chart.AxesY[0].Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LineColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of LineColor.")]
        [Owner("[....]")]
        public void LineColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesX[0].LineColor);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesY[0].LineColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LineStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of LineStyle.")]
        [Owner("[....]")]
        public void LineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, chart.AxesX[0].LineStyle);
                Assert.AreEqual(LineStyles.Solid, chart.AxesY[0].LineStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LineThickness.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of LineThickness.")]
        [Owner("[....]")]
        public void LineThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0.5, chart.AxesX[0].LineThickness);
                Assert.AreEqual(0.5, chart.AxesY[0].LineThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Prefix.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Prefix.")]
        [Owner("[....]")]
        public void PrefixDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].Prefix);
                Assert.IsNull(chart.AxesY[0].Prefix);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Suffix.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Suffix.")]
        [Owner("[....]")]
        public void SuffixDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].Suffix);
                Assert.IsNull(chart.AxesY[0].Suffix);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ScalingSet.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ScalingSet.")]
        [Owner("[....]")]
        public void ScalingSetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].ScalingSet);
                Assert.IsNull(chart.AxesY[0].ScalingSet);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of StartFromZero.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of StartFromZero.")]
        [Owner("[....]")]
        public void StartFromZeroDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse((Boolean)chart.AxesX[0].StartFromZero);
                Assert.IsTrue((Boolean)chart.AxesY[0].StartFromZero);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Title.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Title.")]
        [Owner("[....]")]
        public void TitleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].Title);
                Assert.IsNull(chart.AxesY[0].Title);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontColor.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontColor.")]
        [Owner("[....]")]
        public void TitleFontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].TitleFontColor);
                Assert.IsNull(chart.AxesY[0].TitleFontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontFamily.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontFamily.")]
        [Owner("[....]")]
        public void TitleFontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(new FontFamily("Verdana"), chart.AxesX[0].TitleFontFamily);
                Assert.AreEqual(new FontFamily("Verdana"), chart.AxesY[0].TitleFontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontSize.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontSize.")]
        [Owner("[....]")]
        public void TitleFontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(11, chart.AxesX[0].TitleFontSize, Common.HighPrecisionDelta);
                Assert.AreEqual(11, chart.AxesY[0].TitleFontSize, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontStyle.")]
        [Owner("[....]")]
        public void TitleFontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontStyles.Normal, chart.AxesX[0].TitleFontStyle);
                Assert.AreEqual(FontStyles.Normal, chart.AxesY[0].TitleFontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of TitleFontWeight.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontWeight.")]
        [Owner("[....]")]
        public void TitleFontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(FontWeights.Normal, chart.AxesX[0].TitleFontWeight);
                Assert.AreEqual(FontWeights.Normal, chart.AxesY[0].TitleFontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Interval. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Interval.")]
        [Owner("[....]")]
        public virtual void IntervalDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Double.NaN, chart.AxesX[0].Interval);
                Assert.AreEqual(Double.NaN, chart.AxesY[0].Interval);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ValueFormatString. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ValueFormatString.")]
        [Owner("[....]")]
        public virtual void ValueFormatStringDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual("###,##0.##", chart.AxesX[0].ValueFormatString);
                Assert.AreEqual("###,##0.##", chart.AxesY[0].ValueFormatString);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of AxisType.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of AxisType.")]
        [Owner("[....]")]
        public void AxisTypeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(AxisTypes.Primary, chart.AxesX[0].AxisType);
                Assert.AreEqual(AxisTypes.Primary, chart.AxesY[0].AxisType);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Minimum.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of AxisOrientation.")]
        [Owner("[....]")]
        public void AxisOrientationDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Orientation.Horizontal, chart.AxesX[0].AxisOrientation);
                Assert.AreEqual(Orientation.Vertical, chart.AxesY[0].AxisOrientation);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckNewPropertyValues

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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Background = new SolidColorBrush(Colors.Blue);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), _axisX.Background);
                _axisY.Background = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), _axisY.Background);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Opacity = 0.5;
                Assert.AreEqual(0.5, _axisX.Opacity, Common.HighPrecisionDelta);
                _axisY.Opacity = 0.5;
                Assert.AreEqual(0.5, _axisY.Opacity, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        ///// <summary>
        ///// Check the new value of Scale.
        ///// </summary> 
        //[TestMethod]
        //[Description("Check the new value of Scale.")]
        //[Owner("[....]")]
        //public void ScaleNewValue()
        //{
        //    Chart chart = new Chart();
        //    chart.Width = 400;
        //    chart.Height = 300;

        //    _axisX = new Axis();
        //    chart.AxesX.Add(_axisX);

        //    Common.CreateAndAddDefaultDataSeries(chart);

        //    _isLoaded = false;
        //    chart.Loaded += new RoutedEventHandler(chart_Loaded);

        //    Window window = new Window();
        //    window.Content = chart;
        //    window.Show();
        //    if (_isLoaded)
        //    {
        //        _axisX.Scale = 3;
        //        Assert.AreEqual(3, _axisX.Scale, Common.HighPrecisionDelta);
        //    }


        //    window.Dispatcher.InvokeShutdown();
        //    window.Close();
        //}

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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Padding = new Thickness(10);
                Assert.AreEqual(new Thickness(10), _axisX.Padding);
                _axisY.Padding = new Thickness(10);
                Assert.AreEqual(new Thickness(10), _axisY.Padding);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Cursor = Cursors.Arrow;
                Assert.AreEqual(Cursors.Arrow, _axisX.Cursor);
                _axisY.Cursor = Cursors.Arrow;
                Assert.AreEqual(Cursors.Arrow, _axisY.Cursor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        /// <summary>
        /// Check the new value of ScrollBarOffset for vertical chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ScrollBarOffset.")]
        [Owner("[....]")]
        public void ScrollBarOffsetVerticalChartNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeriesForScrolling(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.ScrollBarOffset = 1;
                Assert.AreEqual(1, _axisX.ScrollBarOffset);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ScrollBarOffset for horizontal chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ScrollBarOffset.")]
        [Owner("[....]")]
        public void ScrollBarOffsetHorizontalChartNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeriesForScrolling(chart);
            chart.Series[0].RenderAs = RenderAs.Bar;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.ScrollBarOffset = 0;
                Assert.AreEqual(0, _axisX.ScrollBarOffset);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Enabled = false;
                Assert.IsFalse((Boolean)_axisX.Enabled);
                _axisY.Enabled = false;
                Assert.IsFalse((Boolean)_axisY.Enabled);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ValueFormatString.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ValueFormatString.")]
        [Owner("[....]")]
        public void ValueFormatStringNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.ValueFormatString = "#0.#'%'";
                Assert.AreEqual("#0.#'%'", _axisX.ValueFormatString);
                _axisY.ValueFormatString = "#0.#'%'";
                Assert.AreEqual("#0.#'%'", _axisY.ValueFormatString);
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
        public void HrefAndHrefTargetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", _axisX.Href);
                _axisY.Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", _axisY.Href);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.LineColor = new SolidColorBrush(Colors.Green);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), _axisX.LineColor);
                _axisY.LineColor = new SolidColorBrush(Colors.Green);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), _axisY.LineColor);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.LineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, _axisX.LineStyle);
                _axisY.LineStyle = LineStyles.Dotted;
                Assert.AreEqual(LineStyles.Dotted, _axisY.LineStyle);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.LineThickness = 2;
                Assert.AreEqual(2, _axisX.LineThickness);
                _axisY.LineThickness = 2;
                Assert.AreEqual(2, _axisY.LineThickness);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Prefix.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Prefix.")]
        [Owner("[....]")]
        public void PrefixNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Prefix = "$";
                Assert.AreEqual("$", _axisX.Prefix);
                _axisY.Prefix = "$";
                Assert.AreEqual("$", _axisY.Prefix);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Suffix.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Suffix.")]
        [Owner("[....]")]
        public void SuffixNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Suffix = "%";
                Assert.AreEqual("%", _axisX.Suffix);
                _axisY.Suffix = "%";
                Assert.AreEqual("%", _axisY.Suffix);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ScalingSet.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ScalingSet.")]
        [Owner("[....]")]
        public void ScalingSetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisY = new Axis();
            chart.AxesY.Add(_axisY);

            DataSeries dataSeries = new DataSeries();
            DataPoint dataPoint = new DataPoint();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "a" + i;
                datapoint.YValue = rand.Next(1000, 2000);
                datapoint.XValue = i + 1;
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisY.ScalingSet = "1024,KB;1024,MB;1024,GB";
                Assert.AreEqual("1024,KB;1024,MB;1024,GB", _axisY.ScalingSet);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of StartFromZero.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of StartFromZero.")]
        [Owner("[....]")]
        public void StartFromZeroNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.StartFromZero = true;
                Assert.IsTrue((Boolean)_axisX.StartFromZero);
                _axisY.StartFromZero = false;
                Assert.IsFalse((Boolean)_axisY.StartFromZero);
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
        public void TitleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Title = "AxisXTitle";
                Assert.AreEqual("AxisXTitle", _axisX.Title);
                _axisY.Title = "AxisYTitle";
                Assert.AreEqual("AxisYTitle", _axisY.Title);
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";
            _axisY = new Axis();
            _axisY.Title = "AxisY";
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.TitleFontColor = new SolidColorBrush(Colors.Magenta);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), _axisX.TitleFontColor);
                _axisY.TitleFontColor = new SolidColorBrush(Colors.Magenta);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), _axisY.TitleFontColor);
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";
            _axisY = new Axis();
            _axisY.Title = "AxisY";
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.TitleFontFamily = new FontFamily("MS Trebuchet");
                Assert.AreEqual(new FontFamily("MS Trebuchet"), _axisX.TitleFontFamily);
                _axisY.TitleFontFamily = new FontFamily("MS Trebuchet");
                Assert.AreEqual(new FontFamily("MS Trebuchet"), _axisY.TitleFontFamily);
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";
            _axisY = new Axis();
            _axisY.Title = "AxisY";
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.TitleFontSize = 16;
                Assert.AreEqual(16, _axisX.TitleFontSize);
                _axisY.TitleFontSize = 16;
                Assert.AreEqual(16, _axisY.TitleFontSize);
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";
            _axisY = new Axis();
            _axisY.Title = "AxisY";
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.TitleFontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, _axisX.TitleFontStyle);
                _axisY.TitleFontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, _axisY.TitleFontStyle);
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";
            _axisY = new Axis();
            _axisY.Title = "AxisY";
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.TitleFontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, _axisX.TitleFontWeight);
                _axisY.TitleFontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, _axisY.TitleFontWeight);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.Interval = 2;
                Assert.AreEqual(2, _axisX.Interval);
                _axisY.Interval = 5;
                Assert.AreEqual(5, _axisY.Interval);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Maximum.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisMaximum.")]
        [Owner("[....]")]
        public void AxisMaximumNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisMaximum = 50;
                Assert.AreEqual(50, _axisX.AxisMaximum);
                _axisY.AxisMaximum = 100;
                Assert.AreEqual(100, _axisY.AxisMaximum);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Minimum.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisMinimum.")]
        [Owner("[....]")]
        public void AxisMinimumNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisMinimum = 1;
                Assert.AreEqual(1, _axisX.AxisMinimum);
                _axisY.AxisMinimum = 10;
                Assert.AreEqual(10, _axisY.AxisMinimum);
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisType = AxisTypes.Primary;
                Assert.AreEqual(AxisTypes.Primary, _axisX.AxisType);
                _axisY.AxisType = AxisTypes.Secondary;
                Assert.AreEqual(AxisTypes.Secondary, _axisY.AxisType);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of AxisOrientation.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisOrientation.")]
        [Owner("[....]")]
        public void AxisOrientationNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisOrientation = Orientation.Horizontal;
                Assert.AreEqual(Orientation.Horizontal, _axisX.AxisOrientation);
                _axisY.AxisOrientation = Orientation.Vertical;
                Assert.AreEqual(Orientation.Vertical, _axisY.AxisOrientation);
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
            

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.ToolTipText = "AxisX";
                Assert.AreEqual("AxisX", _axisX.ToolTipText);
                _axisY.ToolTipText = "AxisY";
                Assert.AreEqual("AxisY", _axisY.ToolTipText);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestAxisCollectionChanged Event

        /// <summary>
        /// Testing the Axis collection changed event.
        /// </summary>
        [TestMethod]
        [Description("Testing the Axis collection changed event.")]
        public void TestingChartAxisCollectionChangedEvent()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _isLoaded = false;

            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "a" + i;
                datapoint.YValue = rand.Next(0, 900);
                datapoint.XValue = i + 1;
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            Axis axis = new Axis();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                axis.Title = "Visifire Axis Title";
                axis.AxisType = AxisTypes.Secondary;
                axis.AxisMaximum = 1000;
                axis.AxisMinimum = 1;
                chart.AxesY = new AxisCollection();
                chart.AxesY.Add(axis);

                dataSeries.AxisYType = AxisTypes.Secondary;
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

        /// <summary>
        /// AxisX reference
        /// </summary>
        private Axis _axisX;

        /// <summary>
        /// AxisY reference
        /// </summary>
        private Axis _axisY;


        #endregion
    }
}
