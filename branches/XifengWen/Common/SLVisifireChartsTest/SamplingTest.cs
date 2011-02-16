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
using System.Windows.Browser;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.TrendLine class 
    /// </summary>
    [TestClass]
    public class SamplingTest : SilverlightControlTest
    {
        /// <summary>
        /// Adding DataPoints at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void AddDataPointsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
            chart.SamplingThreshold = 8;

            EnqueueDelay(_sleepTime);

            Common.EnableAutoTimerCallBack(this, new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 5),
               () =>
               {
                   
                   chart.Series[0].DataPoints.Add(new DataPoint() { YValue = Graphics.RAND.Next(0, 100) });
               }
            );

            TestPanel.Children.Add(chart);
        }

        /// <summary>
        /// Update YVlaue of DataPoints at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void UpdateYValueAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
            chart.SamplingThreshold = 5;
          
            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;
            Common.EnableAutoTimerCallBack(this, new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0, 5),
               () =>
               {
                   chart.Series[0].DataPoints[Graphics.RAND.Next(0, 3)].YValue = Graphics.RAND.Next(0, 100);
                   EnqueueDelay(1000);
                   chart.Series[0].DataPoints[Graphics.RAND.Next(0, 3)].Color = new SolidColorBrush(Colors.Red);
                   
               }
            );
            
        }
        /// <summary>
        /// Check all charts with  and without Sampling 
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckSamplingWithAllChartsAtRealtime2()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreatAChartWithSingleSeries4DateTimeAxis();

            TestPanel.Children.Add(chart);

            chart.SmartLabelEnabled = true;
            chart.Series[0].LabelAngle = 45;
            chart.Series[0].RenderAs = RenderAs.Line;
            chart.ToolTipEnabled = true;
            chart.Series[0].LabelEnabled = true;
            chart.SamplingThreshold = 10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check all charts with  and without Sampling for logarithmic axis
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckSamplingWithAllChartsAtRealtime1()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
       
            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;
            chart.Series[0].LabelEnabled = true;
            chart.SamplingThreshold =10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine;  chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel; chart.Series[0].LabelText = "#XValue"; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Apply Custom ColorSet to chart with and without sampling
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckCustomColorSetForAllChartsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
           
            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;
            chart.SamplingThreshold =10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area; chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar; chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea; chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine;  chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;  chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Update DataPoint color at real time with and without sampling
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDataPointColorForAllChartsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.QuickLine;
            chart.SamplingThreshold = 10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area; chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Red); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Blue); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble; chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Orange); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column;   chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Green); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut;   chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Purple); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine; chart.Series[0].DataPoints[2].Color = new SolidColorBrush(Colors.Blue); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Brown); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Cyan); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.DarkGray); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar;   chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Magenta); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Orange); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid;  chart.Series[0].DataPoints[1].Color = new SolidColorBrush(Colors.Red); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar;   chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Yellow); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Red); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Blue); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea;   chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Orange); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea100;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Green); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Purple); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar100;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Blue); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn;   chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Brown); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn100;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Brown); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine;  chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Brown); });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel; chart.Series[0].DataPoints[0].Color = new SolidColorBrush(Colors.Brown); });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Apply Viwe3D property on all charts with and without sampling
        /// </summary>
        [TestMethod]
        [Asynchronous]
      
        public void CheckView3DForAllChartsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();

            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.QuickLine;
            chart.SamplingThreshold = 20;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar;chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea100;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar100;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn100;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }
   

        /// <summary>
        /// Update axis properties at real time with and without sampling
        /// </summary>
        [TestMethod]
        [Asynchronous]

        public void CheckAxisUpdateForAllChartsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
            EnqueueDelay(2000);
            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;

            chart.SamplingThreshold = 10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area; chart.AxesY[0].AxisMinimum = -50; chart.AxesY[0].AxisMaximum = 100; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; chart.Series[0].AxisYType = AxisTypes.Primary; chart.AxesY[0].AxisType = AxisTypes.Primary; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column; chart.AxesY[0].AxisMinimum = -30; chart.AxesY[0].AxisMaximum = 50; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; chart.Series[0].AxisYType = AxisTypes.Secondary; chart.AxesY[0].AxisType = AxisTypes.Secondary; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point; chart.AxesY[0].AxisMinimum = 0; chart.AxesY[0].AxisMaximum = 150; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea100; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar100; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn100; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = -10; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = 10; });
            EnqueueTestComplete();
        }
        /// <summary>
        /// Test sampling for Financial charts
        /// </summary>
        [TestMethod]
        [Asynchronous]
       
        public void CheckSamplingforFinancialchartsAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreatAChartWithSingleSeries4FinancialChart();

            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Stock;
            chart.SamplingThreshold = 5;

            EnqueueCallback(() => { chart.SamplingThreshold = 5; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.SamplingThreshold = 0; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.SamplingThreshold = 10; });
            EnqueueDelay(1000);
            EnqueueTestComplete();

        }

        /// <summary>
        /// Test Custom color set at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckCustomColorSetAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();

            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;
            chart.SamplingThreshold = 5;

            EnqueueCallback(() => { chart.ColorSet = "MyColorSet1"; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.ColorSet = "MyColorSet2"; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Test IndicatorEnabled at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckIndicatorEnabledAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();

            TestPanel.Children.Add(chart);
            
            chart.Series[0].RenderAs = RenderAs.Column;
            chart.IndicatorEnabled = true;
            chart.SamplingThreshold = 5;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area;  chart.IndicatorEnabled = true;});
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar;  chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble;  chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column;  chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine;  chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point; chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar;  chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid; chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar;chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline;  chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea;  chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar;  chart.IndicatorEnabled = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn;  chart.IndicatorEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine;  chart.IndicatorEnabled = false;});
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Test MovingMarkerEnabled at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]

        public void CheckMovingMarkerEnabledAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
         
            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;
           
            chart.SamplingThreshold = 10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.Series[0].MovingMarkerEnabled = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.Series[0].MovingMarkerEnabled = false ; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine; chart.Series[0].MovingMarkerEnabled = true; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Test Legend for Single Series at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]

        public void CheckLegendAtRealtime()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
            chart.SamplingThreshold = 10;

            TestPanel.Children.Add(chart);

            chart.Series[0].RenderAs = RenderAs.Column;
            chart.Series[0].ShowInLegend = true;
            chart.Series[0].IncludePercentageInLegend = true;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area;  chart.View3D = true; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine; chart.View3D = false; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar;  chart.View3D = true;  chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea100;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar;  chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar100; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn;  chart.View3D = false;  chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn100;  chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;  chart.View3D = true; });
            EnqueueTestComplete();
            EnqueueDelay(1000);
        }

        /// <summary>
        /// Test Legend for Single Series at real time
        /// </summary>
        [TestMethod]
        [Asynchronous]

        public void CheckZoomingAndScrolling()
        {
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();

            TestPanel.Children.Add(chart);
            chart.Series[0].RenderAs = RenderAs.Column;
            chart.ZoomingEnabled = true;
           
            chart.SamplingThreshold = 10;

            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Area; chart.View3D = true; chart.ZoomingEnabled = true; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bar; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Bubble; chart.ZoomingEnabled = true; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Column; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Doughnut; chart.ZoomingEnabled = true; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.QuickLine; chart.ZoomingEnabled = false; chart.View3D = false; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.ZoomingEnabled = true; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pie; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Point; chart.ZoomingEnabled = true; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Radar; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Line; chart.ZoomingEnabled = true; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Pyramid; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Polar; chart.View3D = true; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.SectionFunnel; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.Spline; chart.ZoomingEnabled = true; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedArea100; chart.ZoomingEnabled = false; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar; chart.ZoomingEnabled = true; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedBar100; chart.ZoomingEnabled = false; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn; chart.ZoomingEnabled = true; chart.View3D = false; chart.Series[0].DataPoints[Graphics.RAND.Next(0, 16)].YValue = Double.NaN; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StackedColumn100; chart.ZoomingEnabled = false; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StepLine; chart.ZoomingEnabled = false; chart.View3D = false; });
            EnqueueDelay(1000);
            EnqueueCallback(() => { chart.Series[0].RenderAs = RenderAs.StreamLineFunnel; chart.View3D = true; });
            EnqueueDelay(1000);
            EnqueueTestComplete();
        }


      
        
        /// <summary>
        /// Test a chart with 
        /// SamplingThreshold =0
        /// SamplingThreshold <0
        /// SamplingThreshold <Actual Number Of Points
        /// SamplingThreshold >Actual Number Of Points
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SamplingThresholdInputValue()
        {   
            Common.SetSLPluginHeight(500);

            Chart chart = CreateAChartWithSingleSeries();
            chart.Series[0].RenderAs = RenderAs.Line;

            Common.EnableAutoTimerCallBack(this, new TimeSpan(0, 0, 0, 0, 500), new TimeSpan(0, 0,6),
               () =>
               {
                   try
                   {
                       chart.SamplingThreshold = 0;
                   }
                   catch
                   {
                       Assert.Fail("Setting SamplingThreshold = 0, fails");
                   }
               },
               () =>
               {
                   try
                   {
                       chart.SamplingThreshold = -1;
                       Assert.Fail("Setting SamplingThreshold = -1, failed");
                   }
                   catch
                   {
                   }
               },
               
                () =>
                {
                    try
                    {
                        chart.SamplingThreshold = 30;
                        Assert.Fail("Setting SamplingThreshold = Actual NO of points, failed");
                    }
                    catch
                    {
                    }
                }, 
                () =>
                {
                    try
                    {
                        chart.SamplingThreshold =5;
                        Assert.Fail("Setting SamplingThreshold =5, failed");
                    }
                    catch
                    {
                    }
                }
            );

            TestPanel.Children.Add(chart);
          
        }
        
        /// <summary>
        /// Create a chart for Numeric Axis
        /// </summary>
        /// <returns></returns>
        public Chart CreateAChartWithSingleSeries()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            Axis axisx = new Axis();
            axisx.AxisLabels = new AxisLabels();
            axisx.AxisLabels.Angle = 45;
            chart.AxesX.Add(axisx);
            
            
            Legend legend = new Legend();
            legend.HorizontalAlignment = HorizontalAlignment.Left;
            legend.VerticalAlignment = VerticalAlignment.Center;
            chart.Legends.Add(legend);

            Common.CreateAndAddDefaultDataSeries4Sampling(chart);
           
            chart.ColorSets = new ColorSets();
            //COLOR SET 1:
            ColorSet ct = new ColorSet();
            ct.Id = "MyColorSet1";
            ct.Brushes.Add(new SolidColorBrush(Colors.Blue));
            ct.Brushes.Add(new SolidColorBrush(Colors.Brown));
            ct.Brushes.Add(new SolidColorBrush(Colors.Cyan));
            ct.Brushes.Add(new SolidColorBrush(Colors.DarkGray));
            ct.Brushes.Add(new SolidColorBrush(Colors.Gray));
            ct.Brushes.Add(new SolidColorBrush(Colors.Green));

            // Add ColorSet to ColorSets collection
            chart.ColorSets.Add(ct);

            //COLOR SET 2:
            ColorSet ct1 = new ColorSet();
            ct1.Id = "MyColorSet2";
            ct1.Brushes.Add(new SolidColorBrush(Colors.Red));
            ct1.Brushes.Add(new SolidColorBrush(Colors.Blue));
            ct1.Brushes.Add(new SolidColorBrush(Colors.Green));
            ct1.Brushes.Add(new SolidColorBrush(Colors.Orange));
            ct1.Brushes.Add(new SolidColorBrush(Colors.Purple));
            ct1.Brushes.Add(new SolidColorBrush(Colors.Black));
            // Create new instance of ColorSets class
            chart.ColorSets.Add(ct1);

            return chart;
        }
        /// <summary>
        /// Create chart for Financial charts.
        /// </summary>
        /// <returns></returns>
        public Chart CreatAChartWithSingleSeries4FinancialChart()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);

            return chart;
        }
        /// <summary>
        /// Create chart from DateTime Axis
        /// </summary>
        /// <returns></returns>
        public Chart CreatAChartWithSingleSeries4DateTimeAxis()
        {
            Chart chart = new Chart();
            chart.Width = 600;
            chart.Height = 300;
            chart.ScrollingEnabled = false;

            Common.CreateAndAddDefaultDateTimeAxis(chart);

             return chart;
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
        }
        
        #region Private Data
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        
        #endregion
    }
}
