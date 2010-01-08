using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace WPFVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.AxisLabels class 
    /// </summary>
    [TestClass]
    public class AxisLabelsTest
    {
        #region CheckDefaultPropertyValue
        /// <summary>
        /// Check the default value of Angle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Angle.")]
        [Owner("[....]")]
        public void AngleDefaultValue()
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
                Assert.AreEqual(Double.NaN, chart.AxesX[0].AxisLabels.Angle);
                Assert.AreEqual(Double.NaN, chart.AxesY[0].AxisLabels.Angle);
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
        public void IntervalDefaultValue()
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
                Assert.AreEqual(Double.NaN, chart.AxesX[0].AxisLabels.Interval);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontFamily. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontFamily.")]
        [Owner("[....]")]
        public void FontFamilyDefaultValue()
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
                Assert.AreEqual(new FontFamily("Verdana"), chart.AxesX[0].AxisLabels.FontFamily);
                Assert.AreEqual(new FontFamily("Verdana"), chart.AxesY[0].AxisLabels.FontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontSize. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontSize.")]
        [Owner("[....]")]
        public void FontSizeDefaultValue()
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
                Assert.AreEqual(10, chart.AxesX[0].AxisLabels.FontSize);
                Assert.AreEqual(10, chart.AxesY[0].AxisLabels.FontSize);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontStyle.")]
        [Owner("[....]")]
        public void FontStyleDefaultValue()
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
                Assert.AreEqual(FontStyles.Normal, chart.AxesX[0].AxisLabels.FontStyle);
                Assert.AreEqual(FontStyles.Normal, chart.AxesY[0].AxisLabels.FontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontWeight. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontWeight.")]
        [Owner("[....]")]
        public void FontWeightDefaultValue()
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
                Assert.AreEqual(FontWeights.Normal, chart.AxesX[0].AxisLabels.FontWeight);
                Assert.AreEqual(FontWeights.Normal, chart.AxesY[0].AxisLabels.FontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of FontColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontColor.")]
        [Owner("[....]")]
        public void FontColorDefaultValue()
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
                Assert.IsNull(chart.AxesX[0].AxisLabels.FontColor);
                Assert.IsNull(chart.AxesY[0].AxisLabels.FontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        /// <summary>
        /// Check the default value of Rows. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Rows.")]
        [Owner("[....]")]
        public void RowsDefaultValue()
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
                Assert.AreEqual(0, chart.AxesX[0].AxisLabels.Rows);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        ///// <summary>
        ///// Check the default value of TextWrap (Currently not in use).
        ///// </summary> 
        //[TestMethod]
        //[Description("Check the default value of TextWrap (Currently not in use).")]
        //[Owner("[....]")]
        //public void TextWrapDefaultValue()
        //{
        //    Chart chart = new Chart();
        //    chart.Width = 400;
        //    chart.Height = 300;

        //    Common.CreateAndAddDefaultDataSeries(chart);

        //    isLoaded = false;
        //    chart.Loaded += new RoutedEventHandler(chart_Loaded);

        //    Window window = new Window();
        //    window.Content = chart;
        //    window.Show();
        //    if (isLoaded)
        //    {
        //        Assert.AreEqual(TextWrapping.Wrap, chart.AxesX[0].AxisLabels.TextWrap);
        //    }

        //    window.Dispatcher.InvokeShutdown();
        //    window.Close();
        //}

        #endregion

        #region CheckNewPropertyValue
        
        /// <summary>
        /// Check the New value for Angle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Angle.")]
        [Owner("[....]")]
        public void CheckAngleNewPropertyValue()
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
                _axisX.AxisLabels.Angle = -30;
                _axisY.AxisLabels.Angle = 30;
                Assert.AreEqual(-30, _axisX.AxisLabels.Angle);
                Assert.AreEqual(30, _axisY.AxisLabels.Angle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value for Opacity.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Opacity.")]
        [Owner("[....]")]
        public void CheckOpacityNewPropertyValue()
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
                _axisX.AxisLabels.Opacity = 0.5;
                _axisY.AxisLabels.Opacity = 0.5;
                Assert.AreEqual(0.5, _axisX.AxisLabels.Opacity, Common.HighPrecisionDelta);
                Assert.AreEqual(0.5, _axisY.AxisLabels.Opacity, Common.HighPrecisionDelta);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the New value for FontSize.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontSize.")]
        [Owner("[....]")]
        public void CheckFontSizeNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisLabels.FontSize = 14;
                _axisY.AxisLabels.FontSize = 14;
                Assert.AreEqual(14, _axisX.AxisLabels.FontSize);
                Assert.AreEqual(14, _axisY.AxisLabels.FontSize);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the New value for FontFamily.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontFamily.")]
        [Owner("[....]")]
        public void CheckFontFamilyNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisLabels.FontFamily = new FontFamily("Arial");
                _axisY.AxisLabels.FontFamily = new FontFamily("Arial");
                Assert.AreEqual(new FontFamily("Arial"), _axisX.AxisLabels.FontFamily);
                Assert.AreEqual(new FontFamily("Arial"), _axisY.AxisLabels.FontFamily);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the New value for FontColor.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontColor.")]
        [Owner("[....]")]
        public void CheckFontColorNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
                _axisY.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), _axisX.AxisLabels.FontColor);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), _axisY.AxisLabels.FontColor);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the New value for FontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontStyle.")]
        [Owner("[....]")]
        public void CheckFontStyleNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisLabels.FontStyle = FontStyles.Italic;
                _axisY.AxisLabels.FontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, _axisX.AxisLabels.FontStyle);
                Assert.AreEqual(FontStyles.Italic, _axisY.AxisLabels.FontStyle);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the New value for FontWeight.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontWeight.")]
        [Owner("[....]")]
        public void CheckFontWeightNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisLabels.FontWeight = FontWeights.Bold;
                _axisY.AxisLabels.FontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, _axisX.AxisLabels.FontWeight);
                Assert.AreEqual(FontWeights.Bold, _axisY.AxisLabels.FontWeight);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the New value for Rows.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Rows.")]
        [Owner("[....]")]
        public void CheckRowsNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            chart.AxesX.Add(_axisX);

            Common.CreateAndAddDefaultDataSeries(chart);

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                _axisX.AxisLabels.Rows = 2;
                Assert.AreEqual(2, _axisX.AxisLabels.Rows);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }


        /// <summary>
        /// Check the New value for TextWrap.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for TextWrap.")]
        [Owner("[....]")]
        public void CheckTextWrapNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            _axisX = new Axis();
            chart.AxesX.Add(_axisX);

            DataSeries dataSeries = new DataSeries();
            DataPoint dataPoint = new DataPoint();

            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "VisifireSilverlight" + i;
                datapoint.YValue = rand.Next(0, 100);
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
                _axisX.AxisLabels.TextWrap = 0.5;
                Assert.AreEqual(0.5, _axisX.AxisLabels.TextWrap);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #region AxisLabelTest
        /// <summary>
        /// Test number of AxisLabel added to Chart
        /// </summary>
        [TestMethod]
        [Description("")]
        public void TestNumberOfAxisLabel()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 15; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.AxisXLabel = "Visifire" + i;
                datapoint.YValue = rand.Next(0, 100);
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
                Assert.AreEqual(15, chart.AxesX[0].AxisLabels.AxisLabelList.Count);
            }

            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region AxisLabel Performance Test
        /// <summary>
        /// Test number of AxisLabel added to Chart
        /// </summary>
        [TestMethod]
        [Description("")]
        public void CheckAxisLabelsAutoPlacement1()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;

            chart.AxesX.Add(axisX);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            Int32 numberOfDataPoint = 0;
            Common.AssertAverageDuration(20, 1, delegate
            {

                for (Int32 i = 0; i < 60; i++)
                {
                    DataPoint datapoint = new DataPoint();
                    datapoint.AxisXLabel = "Label" + i;
                    datapoint.XValue = i + 1;
                    datapoint.YValue = rand.Next(0, 100);
                    dataSeries.DataPoints.Add(datapoint);
                    numberOfDataPoint++;
                }
                chart.Series.Add(dataSeries);

            });
        }

        #endregion

        #region Performance and Stress

        /// <summary> 
        /// Stress test AxisLabel creation.
        /// </summary>
        [TestMethod]
        [Description("Stress test AxisLabel creation.")]
        [Owner("[....]")]
        //[Ignore] //Test fails on slower machines and needs to be rewritten to consider that.
        public void CheckAxisLabelsAutoPlacement()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Axis axisX = new Axis();
            axisX.Interval = 1;

            chart.AxesX.Add(axisX);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            Random rand = new Random();

            Int32 numberOfDataPoint = 0;
            Common.AssertAverageDuration(200, 1, delegate
            {
                for (Int32 i = 0; i < 500; i++)
                {
                    DataPoint datapoint = new DataPoint();
                    datapoint.AxisXLabel = "Visifire Label" + i;
                    datapoint.YValue = rand.Next(0, 100);
                    dataSeries.DataPoints.Add(datapoint);
                    numberOfDataPoint++;
                }

                chart.Series.Add(dataSeries);

            });
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
