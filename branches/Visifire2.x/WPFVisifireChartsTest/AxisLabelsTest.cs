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
    /// Summary description for AxisLabelsTest
    /// </summary>
    [TestClass]
    public class AxisLabelsTest
    {
        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isLoaded = true;
        }

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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                Assert.AreEqual(1, chart.AxesX[0].AxisLabels.Rows);
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

        #region CheckAngleNewPropertyValue
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
                axisX.AxisLabels.Angle = -30;
                axisY.AxisLabels.Angle = 30;
                Assert.AreEqual(-30, axisX.AxisLabels.Angle);
                Assert.AreEqual(30, axisY.AxisLabels.Angle);
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

            axisX = new Axis();
            axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.FontSize = 14;
                axisY.AxisLabels.FontSize = 14;
                Assert.AreEqual(14, axisX.AxisLabels.FontSize);
                Assert.AreEqual(14, axisY.AxisLabels.FontSize);
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

            axisX = new Axis();
            axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.FontFamily = new FontFamily("Arial");
                axisY.AxisLabels.FontFamily = new FontFamily("Arial");
                Assert.AreEqual(new FontFamily("Arial"), axisX.AxisLabels.FontFamily);
                Assert.AreEqual(new FontFamily("Arial"), axisY.AxisLabels.FontFamily);
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

            axisX = new Axis();
            axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
                axisY.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), axisX.AxisLabels.FontColor);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), axisY.AxisLabels.FontColor);
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

            axisX = new Axis();
            axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.FontStyle = FontStyles.Italic;
                axisY.AxisLabels.FontStyle = FontStyles.Italic;
                Assert.AreEqual(FontStyles.Italic, axisX.AxisLabels.FontStyle);
                Assert.AreEqual(FontStyles.Italic, axisY.AxisLabels.FontStyle);
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

            axisX = new Axis();
            axisY = new Axis();

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.FontWeight = FontWeights.Bold;
                axisY.AxisLabels.FontWeight = FontWeights.Bold;
                Assert.AreEqual(FontWeights.Bold, axisX.AxisLabels.FontWeight);
                Assert.AreEqual(FontWeights.Bold, axisY.AxisLabels.FontWeight);
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

            axisX = new Axis();
            chart.AxesX.Add(axisX);

            Common.CreateAndAddDefaultDataSeries(chart);

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.Rows = 2;
                Assert.AreEqual(2, axisX.AxisLabels.Rows);
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

            axisX = new Axis();
            chart.AxesX.Add(axisX);

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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
            {
                axisX.AxisLabels.TextWrap = TextWrapping.Wrap;
                Assert.AreEqual(TextWrapping.Wrap, axisX.AxisLabels.TextWrap);
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

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (isLoaded)
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

        #region Private Data

        bool isLoaded = false;
        const int sleepTime = 3000;

        Axis axisX = new Axis();
        Axis axisY = new Axis();

        #endregion
    }
}
