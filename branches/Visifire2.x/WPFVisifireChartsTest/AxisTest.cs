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
    /// Summary description for AxisTest
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(1, chart.AxesX[0].AxisMinimum);
                Assert.AreEqual(100, chart.AxesX[0].AxisMaximum);
                Assert.AreEqual(0, chart.AxesY[0].AxisMinimum);
                Assert.AreEqual(200, chart.AxesY[0].AxisMaximum);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();

        }

        #endregion

        #region CheckDefaultPropertyValues
        /// <summary>
        /// Check the default value of Color. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Color.")]
        [Owner("[....]")]
        public void ColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.IsNull(chart.AxesX[0].Color);
                Assert.IsNull(chart.AxesY[0].Color);
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(Double.NaN, chart.AxesX[0].Interval);
                Assert.AreEqual(Double.NaN, chart.AxesY[0].Interval);
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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
        /// Check the new value of Color. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Color.")]
        [Owner("[....]")]
        public void ColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.Color = new SolidColorBrush(Colors.Blue);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), axisX.Color);
                axisY.Color = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), axisY.Color);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.Enabled = false;
                Assert.IsFalse((Boolean)axisX.Enabled);
                axisY.Enabled = false;
                Assert.IsFalse((Boolean)axisY.Enabled);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.LineColor = new SolidColorBrush(Colors.Green);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), axisX.LineColor);
                axisY.LineColor = new SolidColorBrush(Colors.Green);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), axisY.LineColor);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.LineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, axisX.LineStyle);
                axisY.LineStyle = LineStyles.Dotted;
                Assert.AreEqual(LineStyles.Dotted, axisY.LineStyle);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.LineThickness = 2;
                Assert.AreEqual(2, axisX.LineThickness);
                axisY.LineThickness = 2;
                Assert.AreEqual(2, axisY.LineThickness);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.Prefix = "$";
                Assert.AreEqual("$", axisX.Prefix);
                axisY.Prefix = "$";
                Assert.AreEqual("$", axisY.Prefix);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.Suffix = "%";
                Assert.AreEqual("%", axisX.Suffix);
                axisY.Suffix = "%";
                Assert.AreEqual("%", axisY.Suffix);
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

            axisY = new Axis();
            chart.AxesY.Add(axisY);

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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisY.ScalingSet = "1024,KB;1024,MB;1024,GB";
                Assert.AreEqual("1024,KB;1024,MB;1024,GB", axisY.ScalingSet);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.StartFromZero = true;
                Assert.IsTrue((Boolean)axisX.StartFromZero);
                axisY.StartFromZero = false;
                Assert.IsFalse((Boolean)axisY.StartFromZero);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.Title = "AxisXTitle";
                Assert.AreEqual("AxisXTitle", axisX.Title);
                axisY.Title = "AxisYTitle";
                Assert.AreEqual("AxisYTitle", axisY.Title);
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

            axisX = new Axis();
            axisX.Title = "AxisX";
            axisY = new Axis();
            axisY.Title = "AxisY";
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.TitleFontColor = new SolidColorBrush(Colors.Magenta);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), axisX.TitleFontColor);
                axisY.TitleFontColor = new SolidColorBrush(Colors.Magenta);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), axisY.TitleFontColor);
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

            axisX = new Axis();
            axisX.Title = "AxisX";
            axisY = new Axis();
            axisY.Title = "AxisY";
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.TitleFontFamily = new FontFamily("MS Trebuchet");
                Assert.AreEqual(new FontFamily("MS Trebuchet"), axisX.TitleFontFamily);
                axisY.TitleFontFamily = new FontFamily("MS Trebuchet");
                Assert.AreEqual(new FontFamily("MS Trebuchet"), axisY.TitleFontFamily);
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

            axisX = new Axis();
            axisX.Title = "AxisX";
            axisY = new Axis();
            axisY.Title = "AxisY";
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.TitleFontSize = 16;
                Assert.AreEqual(16, axisX.TitleFontSize);
                axisY.TitleFontSize = 16;
                Assert.AreEqual(16, axisY.TitleFontSize);
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

            axisX = new Axis();
            axisX.Title = "AxisX";
            axisY = new Axis();
            axisY.Title = "AxisY";
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.TitleFontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, axisX.TitleFontStyle);
                axisY.TitleFontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, axisY.TitleFontStyle);
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

            axisX = new Axis();
            axisX.Title = "AxisX";
            axisY = new Axis();
            axisY.Title = "AxisY";
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.TitleFontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, axisX.TitleFontWeight);
                axisY.TitleFontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, axisY.TitleFontWeight);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.Interval = 2;
                Assert.AreEqual(2, axisX.Interval);
                axisY.Interval = 5;
                Assert.AreEqual(5, axisY.Interval);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisMaximum = 50;
                Assert.AreEqual(50, axisX.AxisMaximum);
                axisY.AxisMaximum = 100;
                Assert.AreEqual(100, axisY.AxisMaximum);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisMinimum = 1;
                Assert.AreEqual(1, axisX.AxisMinimum);
                axisY.AxisMinimum = 10;
                Assert.AreEqual(10, axisY.AxisMinimum);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisType = AxisTypes.Primary;
                Assert.AreEqual(AxisTypes.Primary, axisX.AxisType);
                axisY.AxisType = AxisTypes.Secondary;
                Assert.AreEqual(AxisTypes.Secondary, axisY.AxisType);
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

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisOrientation = Orientation.Horizontal;
                Assert.AreEqual(Orientation.Horizontal, axisX.AxisOrientation);
                axisY.AxisOrientation = Orientation.Vertical;
                Assert.AreEqual(Orientation.Vertical, axisY.AxisOrientation);
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
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.ToolTipText = "AxisX";
                Assert.AreEqual("AxisX", axisX.ToolTipText);
                axisY.ToolTipText = "AxisY";
                Assert.AreEqual("AxisY", axisY.ToolTipText);
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

            isLoaded = false;

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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axis.Title = "Visifire Axis Title";
                axis.AxisType = AxisTypes.Secondary;
                axis.AxisMaximum = 1000;
                axis.AxisMinimum = 1;
                chart.AxesY = new ObservableCollection<Axis>();
                chart.AxesY.Add(axis);

                dataSeries.AxisYType = AxisTypes.Secondary;
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

        const int sleepTime = 2000;
        bool isLoaded = false;

        Axis axisX;
        Axis axisY;

        #endregion
    }
}
