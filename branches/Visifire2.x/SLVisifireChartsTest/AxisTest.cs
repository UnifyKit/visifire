using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Silverlight.Testing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Axis class 
    /// </summary>
    [TestClass]
    public class AxisTest : SilverlightControlTest
    {
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

            EnqueueSleep(_sleepTime);
            EnqueueTestComplete();
        }

        #endregion

        #region CheckDefaultPropertyValues

        /// <summary>
        /// Check the default value of Background. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Background.")]
        [Owner("[....]")]
        [Asynchronous]
        public void BackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.AxesX[0].Background),
                () => Assert.IsNull(chart.AxesY[0].Background));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ScrollBarOffset for vertical chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ScrollBarOffset.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ScrollBarOffsetVerticalChartDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDataSeriesWithMoreDataPoints(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].ScrollBarOffset));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ScrollBarOffset for horizontal chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ScrollBarOffset.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ScrollBarOffsetHorizontalChartDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDataSeriesWithMoreDataPoints(chart);
            chart.Series[0].RenderAs = RenderAs.Bar;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Double.NaN, chart.AxesX[0].ScrollBarOffset));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default property value
        /// </summary>
        [TestMethod]
        [Description("Check the default value of Href.")]
        [Asynchronous]
        public void HrefAndHrefTargetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => Assert.AreEqual(null, _axisX.Href),
                () => Assert.AreEqual(HrefTargets._self, _axisX.HrefTarget),
                () => Assert.AreEqual(null, _axisY.Href),
                () => Assert.AreEqual(HrefTargets._self, _axisY.HrefTarget));
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0.5, chart.AxesX[0].LineThickness),
                () => Assert.AreEqual(0.5, chart.AxesY[0].LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ValueFormatString.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of ValueFormatString.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ValueFormatStringDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => Assert.AreEqual("###,##0.##", _axisX.ValueFormatString),
               () => Assert.AreEqual("###,##0.##", _axisY.ValueFormatString));
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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
            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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
            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Orientation.Horizontal, chart.AxesX[0].AxisOrientation),
                () => Assert.AreEqual(Orientation.Vertical, chart.AxesY[0].AxisOrientation));


            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Enabled.
        /// </summary> 
        [TestMethod]
        [Description("Check the default value of Enabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);
            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.AxesX[0].Enabled),
                () => Assert.IsTrue((Boolean)chart.AxesY[0].Enabled));


            EnqueueTestComplete();
        }


        #endregion

        #region CheckNewPropertyValues

        /// <summary>
        /// Check the new value of Background. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Background.")]
        [Owner("[....]")]
        [Asynchronous]
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

            CreateAsyncTest(chart,
                () => _axisX.Background = new SolidColorBrush(Colors.Blue),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), _axisX.Background),
                () => _axisY.Background = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), _axisY.Background));
        }


        /// <summary>
        /// Check the Enabled property value
        /// </summary>
        [TestMethod]
        [Description("Check the new value of Enabled.")]
        [Asynchronous]
        public void EnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Enabled = false,
                () => Assert.IsFalse((Boolean)_axisX.Enabled),
                () => _axisY.Enabled = false,
                () => Assert.IsFalse((Boolean)_axisY.Enabled));

        }
        
        /// <summary>
        /// Check the Opacity property value
        /// </summary>
        [TestMethod]
        [Description("Check the new value of Opacity.")]    
        [Asynchronous]
        public void OpacityNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Opacity = 0.5,
                () => Assert.AreEqual(0.5, _axisX.Opacity, Common.HighPrecisionDelta),
                () => _axisY.Opacity = 0.5,
                () => Assert.AreEqual(0.5, _axisY.Opacity, Common.HighPrecisionDelta));
        }

        /// <summary>
        /// Check the Padding property value
        /// </summary>
        [TestMethod]
        [Description("Check the new value of Padding.")]
        [Asynchronous]
        public void PaddingNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Padding = new Thickness(10),
                () => Assert.AreEqual(new Thickness(10), _axisX.Padding),
                () => _axisY.Padding = new Thickness(10),
                () => Assert.AreEqual(new Thickness(10), _axisY.Padding));
        }

        /// <summary>
        /// Check the Href property value
        /// </summary>
        [TestMethod]
        [Description("Check the new value of Href.")]
        [Asynchronous]
        public void HrefAndHrefTargetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Href = "http://www.visifire.com",
                () => _axisX.HrefTarget=HrefTargets._blank,
                () => Assert.AreEqual("http://www.visifire.com", _axisX.Href),
                () => _axisY.Href = "http://www.visifire.com",
                () => _axisY.HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual("http://www.visifire.com", _axisY.Href));
        }

        /// <summary>
        /// Check the Cursor property value
        /// </summary>
        [TestMethod]
        [Description("Check the new value of Cursor.")]
        [Asynchronous]
        public void CursorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Cursor = Cursors.Hand,
                () => Assert.AreEqual(Cursors.Hand, _axisX.Cursor),
                () => _axisY.Cursor = Cursors.Hand,
                () => Assert.AreEqual(Cursors.Hand, _axisY.Cursor));
        }

        /// <summary>
        /// Check the new value of ScrollBarOffset with vertical chart. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ScrollBarOffset.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ScrollBarOffsetVerticalChartNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDataSeriesWithMoreDataPoints(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].ScrollBarOffset = 1,
                () => Assert.AreEqual(1, chart.AxesX[0].ScrollBarOffset));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ScrollBarOffset with horizontal chart.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ScrollBarOffset.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ScrollBarOffsetHorizontalChartNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Common.CreateAndAddDataSeriesWithMoreDataPoints(chart);
            chart.Series[0].RenderAs = RenderAs.Bar;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].ScrollBarOffset = 0,
                () => Assert.AreEqual(0, chart.AxesX[0].ScrollBarOffset));

            EnqueueTestComplete();
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

            _axisX = new Axis();
            _axisX.LineThickness = 1;

            _axisY = new Axis();
            _axisY.LineThickness = 1;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.LineColor = new SolidColorBrush(Colors.Green),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), _axisX.LineColor),
                () => _axisY.LineColor = new SolidColorBrush(Colors.Green),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Green), _axisY.LineColor));
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

            _axisX = new Axis();
            _axisX.LineColor = new SolidColorBrush(Colors.Black);
            _axisX.LineThickness = 1;

            _axisY = new Axis();
            _axisY.LineColor = new SolidColorBrush(Colors.Black);
            _axisY.LineThickness = 1;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.LineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, _axisX.LineStyle),
                () => _axisY.LineStyle = LineStyles.Dotted,
                () => Assert.AreEqual(LineStyles.Dotted, _axisY.LineStyle));
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

            _axisX = new Axis();
            _axisX.LineColor = new SolidColorBrush(Colors.Black);

            _axisY = new Axis();
            _axisY.LineColor = new SolidColorBrush(Colors.Black);

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.LineThickness = 2,
                () => Assert.AreEqual(2, _axisX.LineThickness),
                () => _axisY.LineThickness = 2,
                () => Assert.AreEqual(2, _axisY.LineThickness));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Prefix = "$",
                () => Assert.AreEqual("$", _axisX.Prefix),
                () => _axisY.Prefix = "$",
                () => Assert.AreEqual("$", _axisY.Prefix));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Suffix = "%",
                () => Assert.AreEqual("%", _axisX.Suffix),
                () => _axisY.Suffix = "%",
                () => Assert.AreEqual("%", _axisY.Suffix));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
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

            CreateAsyncTest(chart,
                () => _axisY.ScalingSet = "1024,KB;1024,MB;1024,GB",
                () => Assert.AreEqual("1024,KB;1024,MB;1024,GB", _axisY.ScalingSet));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.StartFromZero = true,
                () => Assert.IsTrue((Boolean)_axisX.StartFromZero),
                () => _axisY.StartFromZero = false,
                () => Assert.IsFalse((Boolean)_axisY.StartFromZero));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.Title = "AxisXTitle",
                () => Assert.AreEqual("AxisXTitle", _axisX.Title),
                () => _axisY.Title = "AxisYTitle",
                () => Assert.AreEqual("AxisYTitle", _axisY.Title));
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";

            _axisY = new Axis();
            _axisY.Title = "AxisX";

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.TitleFontColor = new SolidColorBrush(Colors.Magenta),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), _axisX.TitleFontColor),
                () => _axisY.TitleFontColor = new SolidColorBrush(Colors.Magenta),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Magenta), _axisY.TitleFontColor));
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";

            _axisY = new Axis();
            _axisY.Title = "AxisX";

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.TitleFontFamily = new FontFamily("MS Trebuchet"),
                () => Assert.AreEqual(new FontFamily("MS Trebuchet"), _axisX.TitleFontFamily),
                () => _axisY.TitleFontFamily = new FontFamily("MS Trebuchet"),
                () => Assert.AreEqual(new FontFamily("MS Trebuchet"), _axisY.TitleFontFamily));
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";

            _axisY = new Axis();
            _axisY.Title = "AxisX";

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.TitleFontSize = 16,
                () => Assert.AreEqual(16, _axisX.TitleFontSize, Common.HighPrecisionDelta),
                () => _axisY.TitleFontSize = 16,
                () => Assert.AreEqual(16, _axisY.TitleFontSize, Common.HighPrecisionDelta));
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";

            _axisY = new Axis();
            _axisY.Title = "AxisX";

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.TitleFontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, _axisX.TitleFontStyle),
                () => _axisY.TitleFontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, _axisY.TitleFontStyle));
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

            _axisX = new Axis();
            _axisX.Title = "AxisX";

            _axisY = new Axis();
            _axisY.Title = "AxisX";

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.TitleFontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, _axisX.TitleFontWeight),
                () => _axisY.TitleFontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, _axisY.TitleFontWeight));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => _axisX.Interval = 2,
               () => Assert.AreEqual(2, _axisX.Interval),
               () => _axisY.Interval = 5,
               () => Assert.AreEqual(5, _axisY.Interval));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => _axisX.AxisMaximum = 50,
               () => Assert.AreEqual(50, _axisX.AxisMaximum),
               () => _axisY.AxisMaximum = 100,
               () => Assert.AreEqual(100, _axisY.AxisMaximum));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
                () => _axisX.AxisMinimum = 1,
                () => Assert.AreEqual(1, _axisX.AxisMinimum),
                () => _axisY.AxisMinimum = 10,
                () => Assert.AreEqual(10, _axisY.AxisMinimum));
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

            _axisX = new Axis();
            _axisY = new Axis();
            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => _axisX.AxisType = AxisTypes.Primary,
               () => Assert.AreEqual(AxisTypes.Primary, _axisX.AxisType),
               () => _axisY.AxisType = AxisTypes.Secondary,
               () => Assert.AreEqual(AxisTypes.Secondary, _axisY.AxisType));
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

            _axisX = new Axis();
            _axisY = new Axis();

            _axisX.AxisOrientation = Orientation.Horizontal;

            _axisY.AxisOrientation = Orientation.Vertical;

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => _axisX.AxisOrientation = Orientation.Horizontal,
               () => Assert.AreEqual(Orientation.Horizontal, _axisX.AxisOrientation),
               () => _axisY.AxisOrientation = Orientation.Vertical,
               () => Assert.AreEqual(Orientation.Vertical, _axisY.AxisOrientation));
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

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => _axisX.ToolTipText = "AxisX",
               () => Assert.AreEqual("AxisX", _axisX.ToolTipText),
               () => _axisY.ToolTipText = "AxisY",
               () => Assert.AreEqual("AxisY", _axisY.ToolTipText));
        }

        /// <summary>
        /// Check the new value of ValueFormatString.
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ValueFormatString.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ValueFormatStringNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            _axisX = new Axis();
            _axisY = new Axis();

            chart.AxesX.Add(_axisX);
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            CreateAsyncTest(chart,
               () => _axisX.ValueFormatString = "#0.#'%'",
               () => Assert.AreEqual("#0.#'%'", _axisX.ValueFormatString),
               () => _axisY.ValueFormatString = "#0.#'%'",
               () => Assert.AreEqual("#0.#'%'", _axisY.ValueFormatString));
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

            _isLoaded = false;
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

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    dataSeries.DataPoints[i].AxisXLabel = "Visifire AxisLabels test\nwith multiline text";
                }
            });

            EnqueueSleep(_sleepTime);
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
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _axisY = DefaultAxisToTest;
                _axisY.AxisType = AxisTypes.Secondary;
                _axisY.AxisMaximum = 1000;
                _axisY.AxisMinimum = 1;
                chart.AxesY.Add(_axisY);
                dataSeries.AxisYType = AxisTypes.Secondary;
            });

            EnqueueSleep(_sleepTime);
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
                dataSeries.DataPoints.Add(datapoint);
            }

            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _axisY = chart.AxesY[0] as Axis;
                _axisY.Title = "Visifire Axis Title";
                _axisY.AxisType = AxisTypes.Secondary;
                chart.AxesY.Add(_axisY);
                dataSeries.AxisYType = AxisTypes.Secondary;
            });

            EnqueueSleep(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        #region AxisEventTesting
        /// <summary>
        /// Testing events in Axis
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void AxisEventChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            TestPanel.Children.Add(chart);

            _axisX = new Axis();
            chart.AxesX.Add(_axisX);
            _axisY = new Axis();
            chart.AxesY.Add(_axisY);

            Common.CreateAndAddDefaultDataSeries(chart);

            _axisX.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseEnter event fired");
            };
            _axisY.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseEnter event fired");
            };

            _axisX.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseLeave event fired");
            };
            _axisY.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseLeave event fired");
            };

            _axisX.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseLeftButtonUp event fired");
            };
            _axisY.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseLeftButtonUp event fired");
            };

            _axisX.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseLeftButtonDown event fired");
            };
            _axisY.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Axis MouseLeftButtonDown event fired");
            };

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);

        }

        #endregion

        /// <summary>
        /// Gets a default instance of Axis to test.
        /// </summary>
        private Axis DefaultAxisToTest
        {
            get { return new Axis(); }
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

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HtmlElement_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        #region Private Data
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 2000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        /// <summary>
        /// axisX reference
        /// </summary>
        private Axis _axisX;

        /// <summary>
        /// AxisY reference
        /// </summary>
        private Axis _axisY;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement1;

        #endregion
    }
}
