using System;
using System.Collections.Generic;
using Microsoft.Silverlight.Testing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    //[TestClass]
    public class AxisLabelsTest : SilverlightControlTest
    {

        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region CheckDefaultPropertyValues
        /// <summary>
        /// Check the default value of Angle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Angle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AngleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].AxisLabels.Angle),
                () => Assert.AreEqual(Double.NaN, chart.AxesY[0].AxisLabels.Angle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Interval. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Interval.")]
        [Owner("[....]")]
        [Asynchronous]
        public void IntervalDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].AxisLabels.Interval));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontSize. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of FontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(10, chart.AxesX[0].AxisLabels.FontSize),
                () => Assert.AreEqual(10, chart.AxesY[0].AxisLabels.FontSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Rows. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Rows.")]
        [Owner("[....]")]
        [Asynchronous]
        public void RowsDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.AxesX[0].AxisLabels.Rows));

            EnqueueTestComplete();
        }

        ///// <summary>
        ///// Check the default value of TextWrap (Currently not in use).
        ///// </summary> 
        //[TestMethod]
        //[Description("Check the default value of TextWrap (Currently not in use).")]
        //[Owner("[....]")]
        //[Asynchronous]
        //public void TextWrapDefaultValue()
        //{
        //    Chart chart = new Chart();
        //    chart.Width = 400;
        //    chart.Height = 300;

        //    Common.CreateAndAddDefaultDataSeries(chart);
        //    EnqueueSleep(sleepTime);

        //    CreateAsyncTask(chart,
        //        () => Assert.AreEqual(TextWrapping.Wrap, chart.AxesX[0].AxisLabels.TextWrap));

        //    EnqueueTestComplete();
        //}

        #endregion

        #region CheckAngleNewPropertyValue
        /// <summary>
        /// Check the New value for Angle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Angle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckAngleNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisLabels.Angle = -30;
            axisY.AxisLabels.Angle = 30;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(-30, axisX.AxisLabels.Angle),
                () => Assert.AreEqual(30, axisY.AxisLabels.Angle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontSize.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckFontSizeNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisLabels.FontSize = 14;
            axisY.AxisLabels.FontSize = 14;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(14, axisX.AxisLabels.FontSize),
                () => Assert.AreEqual(14, axisY.AxisLabels.FontSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontFamily.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontFamily.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckFontFamilyNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisLabels.FontFamily = new FontFamily("Arial");
            axisY.AxisLabels.FontFamily = new FontFamily("Arial");

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(new FontFamily("Arial"), axisX.AxisLabels.FontFamily),
                () => Assert.AreEqual(new FontFamily("Arial"), axisY.AxisLabels.FontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontColor.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckFontColorNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
            axisY.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), axisX.AxisLabels.FontColor),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), axisY.AxisLabels.FontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckFontStyleNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisLabels.FontStyle = FontStyles.Italic;
            axisY.AxisLabels.FontStyle = FontStyles.Italic;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontStyles.Italic, axisX.AxisLabels.FontStyle),
                () => Assert.AreEqual(FontStyles.Italic, axisY.AxisLabels.FontStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for FontWeight.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for FontWeight.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckFontWeightNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisLabels.FontWeight = FontWeights.Bold;
            axisY.AxisLabels.FontWeight = FontWeights.Bold;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontWeights.Bold, axisX.AxisLabels.FontWeight),
                () => Assert.AreEqual(FontWeights.Bold, axisY.AxisLabels.FontWeight));

            EnqueueTestComplete();
        }
        /// <summary>
        /// Check the New value for Rows.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for Rows.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckRowsNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();

            axisX.AxisLabels.Rows = 2;
            chart.AxesX.Add(axisX);

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(2, axisX.AxisLabels.Rows));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the New value for TextWrap.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value for TextWrap.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckTextWrapNewPropertyValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            axisX = new Axis();

            axisX.AxisLabels.TextWrap = TextWrapping.Wrap;
            axisX.AxisLabels.Angle = 0;
            axisX.Interval = 1;
            axisX.AxisLabels.Rows = 1;
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

            CreateAsyncTest(chart,
                () => Assert.AreEqual(TextWrapping.Wrap, axisX.AxisLabels.TextWrap));

            EnqueueTestComplete();
        }
        #endregion

        #region AxisLabelTest
        /// <summary>
        /// Test number of AxisLabel added to Chart
        /// </summary>
        [TestMethod]
        [Description("")]
        [Asynchronous]
        public void TestNumberOfAxisLabel()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            for (Int32 i = 0; i < 15; i++)
            {
                DataPoint datapoint = new DataPoint();
                datapoint.XValue = i + 1;
                datapoint.AxisXLabel = "Visifire" + i;
                datapoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(datapoint);
            }
            chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                Assert.AreEqual(15, chart.AxesX[0].AxisLabels.AxisLabelList.Count);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region AxisLabel Performance Test
        /// <summary>
        /// Test number of AxisLabel added to Chart
        /// </summary>
        [TestMethod]
        [Description("")]
        [Asynchronous]
        public void CheckAxisLabelsAutoPlacement1()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
            chart.AxesX.Add(axisX);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;

            Random rand = new Random();

            Int32 numberOfDataPoint = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(25, 1, delegate
            {
                for (Int32 i = 0; i < 60; i++)
                {
                    DataPoint datapoint = new DataPoint();
                    datapoint.AxisXLabel = "Label" + i;
                    datapoint.YValue = rand.Next(0, 100);
                    dataSeries.DataPoints.Add(datapoint);
                    numberOfDataPoint++;
                }
                chart.Series.Add(dataSeries);

            });

            EnqueueConditional(() => { return isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                htmlElement1 = Common.GetDisplayMessageButton(htmlElement1);
                htmlElement1.SetStyleAttribute("width", "900px");
                htmlElement1.SetProperty("value", numberOfDataPoint + " AxisLabels are added. Click here to exit.");
                htmlElement2 = Common.GetDisplayMessageButton(htmlElement2);
                htmlElement2.SetStyleAttribute("top", "540px");
                htmlElement2.SetProperty("value", msg + " Total Chart Loading Time: " + totalDuration + "s");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(htmlElement2);
            });

            EnqueueCallback(() =>
            {
                htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        /// <summary> 
        /// Stress test AxisLabel creation.
        /// </summary>
        [TestMethod]
        [Description("Stress test AxisLabel creation.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CheckAxisLabelsAutoPlacement2()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            chart.Loaded += new System.Windows.RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Axis axisX = new Axis();
            axisX.AxisLabels.FontColor = new SolidColorBrush(Colors.Red);
            chart.AxesX.Add(axisX);

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            
            Random rand = new Random();

            Int32 numberOfDataPoint = 0;
            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(50, 1, delegate
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

            EnqueueConditional(() => { return isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;

                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                htmlElement1 = Common.GetDisplayMessageButton(htmlElement1);
                htmlElement1.SetStyleAttribute("width", "900px");
                htmlElement1.SetProperty("value", numberOfDataPoint + " AxisLabels are added. Click here to exit.");
                htmlElement2 = Common.GetDisplayMessageButton(htmlElement2);
                htmlElement2.SetStyleAttribute("top", "540px");
                htmlElement2.SetProperty("value", msg + " Total Chart Loading Time: " + totalDuration + "s");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(htmlElement2);
            });

            EnqueueCallback(() =>
            {
                htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        void HtmlElement_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        #endregion

        #region Private Data

        const int sleepTime = 1000;
        System.Windows.Browser.HtmlElement htmlElement1;
        System.Windows.Browser.HtmlElement htmlElement2;
        Axis axisX = new Axis();
        Axis axisY = new Axis();
        Boolean isLoaded = false;
        #endregion
    }
}
