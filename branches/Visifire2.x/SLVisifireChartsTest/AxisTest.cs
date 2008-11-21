using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Silverlight.Testing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{

    [TestClass]
    public class AxisTest : SilverlightControlTest
    {

        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            isLoaded = true;
        }

        #region CheckChartAxisInXaml

        /// <summary>
        /// Check the Axis in Chart via XAML Markup
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ChartAxisViaXaml()
        {
            Object result = XamlReader.Load(Resource.Chart_AxisXaml);
            Assert.IsInstanceOfType(result, typeof(Chart));

            Chart chart = result as Chart;

            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, chart.AxesX[0].AxisMinimum),
                () => Assert.AreEqual(100, chart.AxesX[0].AxisMaximum),
                () => Assert.AreEqual(0, chart.AxesY[0].AxisMinimum),
                () => Assert.AreEqual(200, chart.AxesY[0].AxisMaximum));

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }

        #endregion

        #region CheckDefaultPropertyValues

        /// <summary>
        /// Check the default value of Color. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Color.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesX[0].Color),
                () => Assert.IsNull(chart.AxesY[0].Color));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of LineColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesX[0].LineColor),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesY[0].LineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of LineStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, chart.AxesX[0].LineStyle),
                () => Assert.AreEqual(LineStyles.Solid, chart.AxesY[0].LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LineThickness.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of LineThickness.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0.5, chart.AxesX[0].LineThickness),
                () => Assert.AreEqual(0.5, chart.AxesY[0].LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Prefix.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Prefix.")]
        [Owner("[....]")]
        [Asynchronous]
        public void PrefixDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesY[0].Prefix),
                () => Assert.IsNull(chart.AxesY[0].Prefix));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Suffix.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Suffix.")]
        [Owner("[....]")]
        [Asynchronous]
        public void SuffixDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesX[0].Suffix),
                () => Assert.IsNull(chart.AxesY[0].Suffix));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ScalingSet.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ScalingSet.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ScalingSetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesX[0].ScalingSet),
                () => Assert.IsNull(chart.AxesY[0].ScalingSet));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of StartFromZero.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of StartFromZero.")]
        [Owner("[....]")]
        [Asynchronous]
        public void StartFromZeroDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.AxesX[0].StartFromZero),
                () => Assert.IsTrue((Boolean)chart.AxesY[0].StartFromZero));


            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Title.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Title.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesX[0].Title),
                () => Assert.IsNull(chart.AxesY[0].Title));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontColor.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesX[0].TitleFontColor),
                () => Assert.IsNull(chart.AxesY[0].TitleFontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontFamily.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontFamily.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new FontFamily("Verdana"), chart.AxesX[0].TitleFontFamily),
                () => Assert.AreEqual(new FontFamily("Verdana"), chart.AxesY[0].TitleFontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontSize.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(11, chart.AxesX[0].TitleFontSize, Common.HighPrecisionDelta),
                () => Assert.AreEqual(11, chart.AxesY[0].TitleFontSize, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontStyles.Normal, chart.AxesX[0].TitleFontStyle),
                () => Assert.AreEqual(FontStyles.Normal, chart.AxesY[0].TitleFontStyle));


            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontWeight.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of TitleFontWeight.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontWeights.Normal, chart.AxesX[0].TitleFontWeight),
                () => Assert.AreEqual(FontWeights.Normal, chart.AxesY[0].TitleFontWeight));

            EnqueueTestComplete();
        }


        /// <summary>
        /// Check the default value of Interval. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Interval.")]
        [Owner("[....]")]
        [Asynchronous]
        public virtual void IntervalDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].Interval),
                () => Assert.AreEqual(Double.NaN, chart.AxesY[0].Interval));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of AxisType. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of AxisType.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisTypeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(AxisTypes.Primary, chart.AxesX[0].AxisType),
                () => Assert.AreEqual(AxisTypes.Primary, chart.AxesY[0].AxisType));


            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Minimum.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of AxisOrientation.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisOrientationDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Orientation.Horizontal, chart.AxesX[0].AxisOrientation),
                () => Assert.AreEqual(Orientation.Vertical, chart.AxesY[0].AxisOrientation));


            EnqueueTestComplete();
        }


        #endregion

        #region CheckNewPropertyValues

        /// <summary>
        /// Check the new value of Color. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Color.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ColorNewValue()
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

            CreateAsyncTest(chart,
                () => axisX.Color = new SolidColorBrush(Colors.Blue),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), axisX.Color),
                () => axisY.Color = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), axisY.Color));
        }

        /// <summary>
        /// Check the new value of LineColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.LineThickness = 1;

            axisY = new Axis();
            axisY.LineThickness = 1;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.LineColor = new SolidColorBrush(Colors.Green),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), axisX.LineColor),
                () => axisY.LineColor = new SolidColorBrush(Colors.Green),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), axisY.LineColor));
        }

        /// <summary>
        /// Check the new value of LineStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.LineColor = new SolidColorBrush(Colors.Black);
            axisX.LineThickness = 1;

            axisY = new Axis();
            axisY.LineColor = new SolidColorBrush(Colors.Black);
            axisY.LineThickness = 1;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.LineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, axisX.LineStyle),
                () => axisY.LineStyle = LineStyles.Dotted,
                () => Assert.AreEqual(LineStyles.Dotted, axisY.LineStyle));
        }

        /// <summary>
        /// Check the new value of LineThickness.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LineThickness.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LineThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.LineColor = new SolidColorBrush(Colors.Black);

            axisY = new Axis();
            axisY.LineColor = new SolidColorBrush(Colors.Black);

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.LineThickness = 2,
                () => Assert.AreEqual(2, axisX.LineThickness),
                () => axisY.LineThickness = 2,
                () => Assert.AreEqual(2, axisY.LineThickness));
        }

        /// <summary>
        /// Check the new value of Prefix.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Prefix.")]
        [Owner("[....]")]
        [Asynchronous]
        public void PrefixNewValue()
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

            CreateAsyncTest(chart,
                () => axisX.Prefix = "$",
                () => Assert.AreEqual("$", axisX.Prefix),
                () => axisY.Prefix = "$",
                () => Assert.AreEqual("$", axisY.Prefix));
        }

        /// <summary>
        /// Check the new value of Suffix.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Suffix.")]
        [Owner("[....]")]
        [Asynchronous]
        public void SuffixNewValue()
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

            CreateAsyncTest(chart,
                () => axisX.Suffix = "%",
                () => Assert.AreEqual("%", axisX.Suffix),
                () => axisY.Suffix = "%",
                () => Assert.AreEqual("%", axisY.Suffix));
        }

        /// <summary>
        /// Check the new value of ScalingSet.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ScalingSet.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ScalingSetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisY = new Axis();
            chart.AxesX.Add(axisX);
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

            CreateAsyncTest(chart,
                () => axisY.ScalingSet = "1024,KB;1024,MB;1024,GB",
                () => Assert.AreEqual("1024,KB;1024,MB;1024,GB", axisY.ScalingSet));
        }

        /// <summary>
        /// Check the new value of StartFromZero.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of StartFromZero.")]
        [Owner("[....]")]
        [Asynchronous]
        public void StartFromZeroNewValue()
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

            CreateAsyncTest(chart,
                () => axisX.StartFromZero = true,
                () => Assert.IsTrue((Boolean)axisX.StartFromZero),
                () => axisY.StartFromZero = false,
                () => Assert.IsFalse((Boolean)axisY.StartFromZero));
        }

        /// <summary>
        /// Check the new value of Title.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Title.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleNewValue()
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

            CreateAsyncTest(chart,
                () => axisX.Title = "AxisXTitle",
                () => Assert.AreEqual("AxisXTitle", axisX.Title),
                () => axisY.Title = "AxisYTitle",
                () => Assert.AreEqual("AxisYTitle", axisY.Title));
        }

        /// <summary>
        /// Check the new value of TitleFontColor.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.Title = "AxisX";

            axisY = new Axis();
            axisY.Title = "AxisX";

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.TitleFontColor = new SolidColorBrush(Colors.Magenta),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), axisX.TitleFontColor),
                () => axisY.TitleFontColor = new SolidColorBrush(Colors.Magenta),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), axisY.TitleFontColor));
        }

        /// <summary>
        /// Check the new value of TitleFontFamily.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontFamily.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontFamilyNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.Title = "AxisX";

            axisY = new Axis();
            axisY.Title = "AxisX";

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.TitleFontFamily = new FontFamily("MS Trebuchet"),
                () => Assert.AreEqual(new FontFamily("MS Trebuchet"), axisX.TitleFontFamily),
                () => axisY.TitleFontFamily = new FontFamily("MS Trebuchet"),
                () => Assert.AreEqual(new FontFamily("MS Trebuchet"), axisY.TitleFontFamily));
        }

        /// <summary>
        /// Check the new value of TitleFontSize.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontSizeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.Title = "AxisX";

            axisY = new Axis();
            axisY.Title = "AxisX";

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.TitleFontSize = 16,
                () => Assert.AreEqual(16, axisX.TitleFontSize, Common.HighPrecisionDelta),
                () => axisY.TitleFontSize = 16,
                () => Assert.AreEqual(16, axisY.TitleFontSize, Common.HighPrecisionDelta));
        }

        /// <summary>
        /// Check the new value of TitleFontStyle.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.Title = "AxisX";

            axisY = new Axis();
            axisY.Title = "AxisX";

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.TitleFontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, axisX.TitleFontStyle),
                () => axisY.TitleFontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, axisY.TitleFontStyle));
        }

        /// <summary>
        /// Check the new value of TitleFontWeight.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleFontWeight.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleFontWeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisX.Title = "AxisX";

            axisY = new Axis();
            axisY.Title = "AxisX";

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => axisX.TitleFontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, axisX.TitleFontWeight),
                () => axisY.TitleFontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, axisY.TitleFontWeight));
        }

        /// <summary>
        /// Check the new value of Interval.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Interval.")]
        [Owner("[....]")]
        [Asynchronous]
        public void IntervalNewValue()
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

            CreateAsyncTest(chart,
               () => axisX.Interval = 2,
               () => Assert.AreEqual(2, axisX.Interval),
               () => axisY.Interval = 5,
               () => Assert.AreEqual(5, axisY.Interval));
        }

        /// <summary>
        /// Check the new value of Maximum.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisMaximum.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisMaximumNewValue()
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

            CreateAsyncTest(chart,
               () => axisX.AxisMaximum = 50,
               () => Assert.AreEqual(50, axisX.AxisMaximum),
               () => axisY.AxisMaximum = 100,
               () => Assert.AreEqual(100, axisY.AxisMaximum));
        }

        /// <summary>
        /// Check the new value of Minimum.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisMinimum.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisMinimumNewValue()
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

            CreateAsyncTest(chart,
                () => axisX.AxisMinimum = 1,
                () => Assert.AreEqual(1, axisX.AxisMinimum),
                () => axisY.AxisMinimum = 10,
                () => Assert.AreEqual(10, axisY.AxisMinimum));
        }

        /// <summary>
        /// Check the new value of AxisType.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisType.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisTypeNewValue()
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

            CreateAsyncTest(chart,
               () => axisX.AxisType = AxisTypes.Primary,
               () => Assert.AreEqual(AxisTypes.Primary, axisX.AxisType),
               () => axisY.AxisType = AxisTypes.Secondary,
               () => Assert.AreEqual(AxisTypes.Secondary, axisY.AxisType));
        }

        /// <summary>
        /// Check the new value of AxisOrientation.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of AxisOrientation.")]
        [Owner("[....]")]
        [Asynchronous]
        public void AxisOrientationNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            axisX = new Axis();
            axisY = new Axis();

            axisX.AxisOrientation = Orientation.Horizontal;

            axisY.AxisOrientation = Orientation.Vertical;

            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => axisX.AxisOrientation = Orientation.Horizontal,
               () => Assert.AreEqual(Orientation.Horizontal, axisX.AxisOrientation),
               () => axisY.AxisOrientation = Orientation.Vertical,
               () => Assert.AreEqual(Orientation.Vertical, axisY.AxisOrientation));
        }

        /// <summary>
        /// Check the new value of ToolTipText.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ToolTipText.")]
        [Owner("[....]")]
        [Asynchronous]
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

            CreateAsyncTest(chart,
               () => axisX.ToolTipText = "AxisX",
               () => Assert.AreEqual("AxisX", axisX.ToolTipText),
               () => axisY.ToolTipText = "AxisY",
               () => Assert.AreEqual("AxisY", axisY.ToolTipText));
        }

        #endregion

        #region CheckAxisLabelMultiLineText
        /// <summary>
        /// Check the AxisLabels text with multiple lines
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckAxisLabelMultiLineText()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();
            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Column;
            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Visifire AxisLabels test with multiline text";
                dataPoint.YValue = rand.Next(0, 100);
                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    dataSeries.DataPoints[i].AxisXLabel = "Visifire AxisLabels test\nwith multiline text";
                }
            });

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }
        #endregion


        #region TestNewAxisAdded
        /// <summary>
        /// Testing the Axis added.
        /// </summary>
        [TestMethod]
        [Description("Testing the Axis added.")]
        [Asynchronous]
        public void TestingChartAxisChanged()
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
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                axisY = DefaultAxisToTest;
                axisY.AxisType = AxisTypes.Secondary;
                axisY.AxisMaximum = 1000;
                axisY.AxisMinimum = 1;
                chart.AxesY.Add(axisY);
                dataSeries.AxisYType = AxisTypes.Secondary;
            });

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region ExistingAxisChangeTest
        /// <summary>
        /// Testing the existing axis change.
        /// </summary>
        [TestMethod]
        [Description("Testing the existing axis change.")]
        [Asynchronous]
        public void TestingExistingAxisChange()
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
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return isLoaded; });
            EnqueueSleep(sleepTime);

            EnqueueCallback(() =>
            {
                axisY = chart.AxesY[0] as Axis;
                axisY.Title = "Visifire Axis Title";
                axisY.AxisType = AxisTypes.Secondary;
                chart.AxesY.Add(axisY);
                dataSeries.AxisYType = AxisTypes.Secondary;
            });

            EnqueueSleep(sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        private Axis DefaultAxisToTest
        {
            get { return new Axis(); }
        }
        

        #region Private Data

        const int sleepTime = 2000;
        bool isLoaded = false;

        Axis axisX = new Axis();
        Axis axisY = new Axis();

        #endregion
    }
}
