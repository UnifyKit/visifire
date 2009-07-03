using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Windows.Browser;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    [TestClass]
    public class FunnelChartTests:SilverlightControlTest
    {
        #region StreamLineFunnelProperties

        #region CheckFunnelDefaultPropertyValue

        /// <summary>
        /// Check the default value of MinPointHeight
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineMinPointHeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.AreEqual(Double.NaN, chart.Series[0].MinPointHeight);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.IsTrue((Boolean)chart.Series[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LabelStyles.OutSide, (LabelStyles)chart.Series[0].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelLineEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Series[0].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelLineStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, (LineStyles)chart.Series[0].LabelLineStyle));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].Bevel));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LightingEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Exploded
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineExplodedDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.Series[0].Exploded));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShowInLegend
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineShowInLegendDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].ShowInLegend));
            EnqueueTestComplete();
        }

        #endregion

        #region CheckFunnelNewPropertyValue
        /// <summary>
        /// Check the new value of MinPointHeight
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineMinPointHeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].YValue = 1;
                    chart.Series[0].DataPoints[4].YValue = 5;
                    chart.Series[0].MinPointHeight = 20;
                    Assert.AreEqual(20, chart.Series[0].MinPointHeight);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of View3D
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineView3DNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.View3D = true;
                    Assert.IsTrue(chart.View3D);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of UniqueColors
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineUniqueColorsNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.UniqueColors = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        if (i < 4)
                            Common.AssertBrushesAreEqual(chart.Series[0].DataPoints[i].Color, chart.Series[0].DataPoints[i + 1].Color);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Color in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineColorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Color = new SolidColorBrush(Colors.Red);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.Series[0].Color);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Color in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineColorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        chart.Series[0].DataPoints[i].Color = new SolidColorBrush(Colors.Yellow);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Yellow), chart.Series[0].DataPoints[i].Color);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Cursor in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineCursorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Cursor = Cursors.Hand;
                    Assert.AreEqual(Cursors.Hand, chart.Series[0].Cursor);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Cursor in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineCursorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        chart.Series[0].DataPoints[i].Cursor = Cursors.Hand;
                        Assert.AreEqual(Cursors.Hand, chart.Series[0].DataPoints[i].Cursor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Enabled in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Enabled = false;
                    Assert.IsFalse((Boolean)chart.Series[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Enabled in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].Enabled = false;
                    Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Opacity in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineOpacityInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Opacity = 0.5;
                    Assert.AreEqual(0.5, chart.Series[0].Opacity);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Opacity in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineOpacityInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].Opacity = 0.5;
                    Assert.AreEqual(0.5, chart.Series[0].DataPoints[2].Opacity, Common.HighPrecisionDelta);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LegendText in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLegendTextInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].LegendText = "Legend";
                    Assert.AreEqual("Legend", chart.Series[0].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LegendText in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLegendTextInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].LegendText = "Legend";
                    Assert.AreEqual("Legend", chart.Series[0].DataPoints[2].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].DataPoints[2].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new` value of LabelLineEnabled in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelLineEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineEnabled in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].LabelLineEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].DataPoints[3].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelLineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, (LineStyles)chart.Series[0].LabelLineStyle));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].LabelLineStyle = LineStyles.Dotted,
                () => Assert.AreEqual(LineStyles.Dotted, (LineStyles)chart.Series[0].DataPoints[3].LabelLineStyle));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineThicknessInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelLineThickness = 2,
                () => Assert.AreEqual(2, chart.Series[0].LabelLineThickness));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLabelLineThicknessInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].LabelLineThickness = 3,
                () => Assert.AreEqual(3, chart.Series[0].DataPoints[3].LabelLineThickness));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineBevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Bevel = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].Bevel));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LightingEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineLightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LightingEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Exploded in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineExplodedInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Exploded = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].Exploded));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Exploded in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineExplodedInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].Exploded = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].DataPoints[2].Exploded));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineShowInLegendInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].ShowInLegend = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].ShowInLegend));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineShowInLegendInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].ShowInLegend = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].ShowInLegend));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Href in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineHrefInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Href = "http://www.visifire.com",
                () => Assert.AreEqual("http://www.visifire.com", chart.Series[0].Href));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Href in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineHrefInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => Assert.AreEqual("http://www.visifire.com", chart.Series[0].DataPoints[3].Href));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineHrefTargetInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Href = "http://www.visifire.com",
                () => chart.Series[0].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].HrefTarget));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StreamLineHrefTargetInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget));
            EnqueueTestComplete();
        }
        #endregion

        #endregion

        #region SectionFunnelProperties

        #region CheckFunnelDefaultPropertyValue

        /// <summary>
        /// Check the default value of MinPointHeight
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionMinPointHeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.AreEqual(Double.NaN, chart.Series[0].MinPointHeight);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.IsTrue((Boolean)chart.Series[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LabelStyles.OutSide, (LabelStyles)chart.Series[0].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelLineEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Series[0].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelLineStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, (LineStyles)chart.Series[0].LabelLineStyle));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].Bevel));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LightingEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Exploded
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionExplodedDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.Series[0].Exploded));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShowInLegend
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionShowInLegendDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].ShowInLegend));
            EnqueueTestComplete();
        }

        #endregion

        #region CheckFunnelNewPropertyValue
        /// <summary>
        /// Check the new value of MinPointHeight
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionMinPointHeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].YValue = 1;
                    chart.Series[0].DataPoints[4].YValue = 5;
                    chart.Series[0].MinPointHeight = 20;
                    Assert.AreEqual(20, chart.Series[0].MinPointHeight);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of UniqueColors
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionUniqueColorsNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.UniqueColors = false;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        if (i < 4)
                            Common.AssertBrushesAreEqual(chart.Series[0].DataPoints[i].Color, chart.Series[0].DataPoints[i + 1].Color);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of View3D
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionView3DNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.View3D = true;
                    Assert.IsTrue(chart.View3D);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Color in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionColorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Color = new SolidColorBrush(Colors.Red);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.Series[0].Color);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Color in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionColorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        chart.Series[0].DataPoints[i].Color = new SolidColorBrush(Colors.Yellow);
                        Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Yellow), chart.Series[0].DataPoints[i].Color);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Cursor in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionCursorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Cursor = Cursors.Hand;
                    Assert.AreEqual(Cursors.Hand, chart.Series[0].Cursor);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Cursor in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionCursorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    for (Int32 i = 0; i < 5; i++)
                    {
                        chart.Series[0].DataPoints[i].Cursor = Cursors.Hand;
                        Assert.AreEqual(Cursors.Hand, chart.Series[0].DataPoints[i].Cursor);
                    }
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Enabled in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Enabled = false;
                    Assert.IsFalse((Boolean)chart.Series[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Enabled in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].Enabled = false;
                    Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Opacity in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionOpacityInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].Opacity = 0.5;
                    Assert.AreEqual(0.5, chart.Series[0].Opacity);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Opacity in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionOpacityInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].Opacity = 0.5;
                    Assert.AreEqual(0.5, chart.Series[0].DataPoints[2].Opacity, Common.HighPrecisionDelta);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LegendText in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLegendTextInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].LegendText = "Legend";
                    Assert.AreEqual("Legend", chart.Series[0].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LegendText in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLegendTextInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].LegendText = "Legend";
                    Assert.AreEqual("Legend", chart.Series[0].DataPoints[2].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].DataPoints[2].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new` value of LabelLineEnabled in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelLineEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineEnabled in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].LabelLineEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].DataPoints[3].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelLineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, (LineStyles)chart.Series[0].LabelLineStyle));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].LabelLineStyle = LineStyles.Dotted,
                () => Assert.AreEqual(LineStyles.Dotted, (LineStyles)chart.Series[0].DataPoints[3].LabelLineStyle));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineThicknessInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelLineThickness = 2,
                () => Assert.AreEqual(2, chart.Series[0].LabelLineThickness));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLabelLineThicknessInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].LabelLineThickness = 3,
                () => Assert.AreEqual(3, chart.Series[0].DataPoints[3].LabelLineThickness));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionBevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Bevel = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].Bevel));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LightingEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionLightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LightingEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Exploded in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionExplodedInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Exploded = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].Exploded));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Exploded in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionExplodedInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].Exploded = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].DataPoints[2].Exploded));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionShowInLegendInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].ShowInLegend = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].ShowInLegend));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionShowInLegendInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].ShowInLegend = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].ShowInLegend));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Href in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionHrefInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Href = "http://www.visifire.com",
                () => Assert.AreEqual("http://www.visifire.com", chart.Series[0].Href));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Href in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionHrefInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => Assert.AreEqual("http://www.visifire.com", chart.Series[0].DataPoints[3].Href));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionHrefTargetInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].Href = "http://www.visifire.com",
                () => chart.Series[0].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].HrefTarget));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void SectionHrefTargetInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget));
            EnqueueTestComplete();
        }
        #endregion

        #endregion

        #region TestFunnelEvents in DataSeries

        [TestMethod]
        [Asynchronous]
        public void TestFunnelEventsInDataSeries()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Visifire" + i;
                dataPoint.YValue = rand.Next(10, 100);
                dataSeries.DataPoints.Add(dataPoint);

            }
            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            dataSeries.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "DataSeries RenderAs: " + (sender as DataPoint).Parent.RenderAs + "MouseLeftButtonUp event fired");
            };

            dataSeries.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "DataSeries RenderAs: " + (sender as DataPoint).Parent.RenderAs + " MouseEnter event fired");
            };

            dataSeries.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement1.SetProperty("value", "Click here to exit. MouseLeave event fired");
            };

            dataSeries.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement1.SetProperty("value", "DataSeries RenderAs: " + (sender as DataPoint).Parent.RenderAs + " MouseLeftButtonDown event fired");
            };

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Mouse over the DataPoints");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }

        #endregion

        #region TestFunnelEvents in DataPoints

        [TestMethod]
        [Asynchronous]
        public void TestFunnelEventsInDataPoints()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = false;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "Visifire" + i;
                dataPoint.YValue = rand.Next(10, 100);
                dataPoint.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseLeftButtonUp event fired");
                };

                dataPoint.MouseEnter += delegate(Object sender, MouseEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseEnter event fired");
                };

                dataPoint.MouseLeave += delegate(Object sender, MouseEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "Click here to exit. MouseLeave event fired");
                };

                dataPoint.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseLeftButtonDown event fired");
                };

                dataPoint.MouseMove += delegate(Object sender, MouseEventArgs e)
                {
                    _htmlElement1.SetProperty("value", "DataPoint YValue: " + (sender as DataPoint).InternalYValue + " MouseMove event fired");
                };

                dataSeries.DataPoints.Add(dataPoint);
            }
            chart.Series.Add(dataSeries);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<HtmlEventArgs>(this.HtmlElement_OnClick));
            });

            _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
            _htmlElement1.SetStyleAttribute("width", "900px");
            _htmlElement1.SetProperty("value", "Check Mouse events for DataPoints");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
        }

        #endregion

        #region TestSingleDataPoint

        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 20 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 20 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        #endregion

        #region TestNullDataPoint

        [TestMethod]
        [Asynchronous]
        public void TestNullDataPointStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestNullDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        #endregion

        #region TestZeroValueDataPoint

        [TestMethod]
        [Asynchronous]
        public void TestZeroXValueDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 0, YValue = 20 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestZeroYValueDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(3, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestZeroYValueDataPointSection3D()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(3, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestZeroYValueDataPointStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(3, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestZeroYValueDataPointStreamLine3D()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            chart.Series.Add(dataSeries);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(3, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        #endregion

        #region TestNegativeValue

        [TestMethod]
        [Asynchronous]
        public void TestNegativeValueInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.StreamLineFunnel;

                    for (Int32 i = 0; i < 10; i++)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = "Visifire";
                        dataPoint.YValue = rand.Next(-100, 100);
                        dataSeries.DataPoints.Add(dataPoint);
                    }

                    chart.Series.Add(dataSeries);
                });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestNegativeValueInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestAllNegativeValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, -10);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestAllNegativeValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, -10);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region ExplodedFunnelChecking
        /// <summary>
        /// Testing Pie Chart exploded
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ExplodedFunnelChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 350;

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            Random rand = new Random();

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;

            for (Int32 i = 0; i < 10; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.YValue = rand.Next(100, 500);
                dataSeries.DataPoints.Add(dataPoint);

            }
            _chart.Series.Add(dataSeries);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here to View3D(true/false).");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Switch StreamLine/Section.");
                _htmlElement3 = Common.GetDisplayMessageButton(_htmlElement3);
                _htmlElement3.SetStyleAttribute("top", "560px");
                _htmlElement3.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement3);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.View3D_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.SwitchDataSeries_OnClick));
            });

            EnqueueCallback(() =>
            {
                _htmlElement3.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.Exit_OnClick));
            });

        }
        #endregion

        #region TestLabelText

        [TestMethod]
        [Asynchronous]
        public void TestLabelTextInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "Visifire Chart, #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLabelTextInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "Visifire Chart, #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLongLabelTextIn2DStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "Visifire Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLongLabelTextIn2DSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "Visifire Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLongLabelTextIn3DStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "Visifire Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLongLabelTextIn3DSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "Visifire Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestDecimalValues

        [TestMethod]
        [Asynchronous]
        public void TestDecimalValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 2.8345 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 0.8679 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 6.2387 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 20.8156 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 8.0002 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 16.888 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestDecimalValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 2.8345 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 0.8679 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 6.2387 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 20.8156 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 8.0002 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 16.888 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestNegativeXValues

        [TestMethod]
        [Asynchronous]
        public void TestNegativeXValueInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -1, YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -2, YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -3, YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -4, YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -5, YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -6, YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestNegativeXValueInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -1, YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -2, YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -3, YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -4, YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -5, YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = -6, YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestSameXValues

        [TestMethod]
        [Asynchronous]
        public void TestSameXValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestSameXValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestDateValues

        [TestMethod]
        [Asynchronous]
        public void TestDateValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "#XValue, #YValue";
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1), YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1), YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 1), YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 1), YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 1), YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 12, 1), YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestDateValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "#XValue, #YValue";
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1), YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1), YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 1), YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 1), YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 1), YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 12, 1), YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestDateTimeValues

        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "#XValue, #YValue";
                dataSeries.XValueType = ChartValueTypes.DateTime;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 1, 2, 8), YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1, 3, 9, 20), YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 1, 8, 22, 40), YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 1, 9, 7, 24), YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 1, 6, 8, 22), YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 12, 1, 8, 20, 58), YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "#XValue, #YValue";
                dataSeries.XValueType = ChartValueTypes.DateTime;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 1, 2, 8), YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1, 3, 9, 20), YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 1, 8, 22, 40), YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 1, 9, 7, 24), YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 1, 6, 8, 22), YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 12, 1, 8, 20, 58), YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestTimeValues

        [TestMethod]
        [Asynchronous]
        public void TestTimeValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "#XValue, #YValue";
                dataSeries.XValueType = ChartValueTypes.Time;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 1, 2, 8), YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 3, 9, 20), YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 8, 22, 40), YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 9, 7, 24), YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 6, 8, 22), YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 8, 20, 58), YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestTimeValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "#XValue, #YValue";
                dataSeries.XValueType = ChartValueTypes.Time;
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 1, 2, 8), YValue = 2.84 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 3, 9, 20), YValue = 12.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 8, 22, 40), YValue = 6.35 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 9, 7, 24), YValue = 20.86 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 6, 8, 22), YValue = 8.02 });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 8, 20, 58), YValue = 16.88 });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region StressTestMinPointHeight

        [TestMethod]
        [Asynchronous]
        public void StressTestMinPointHeightInStreamLine()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 350;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 160 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 2.8 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 10 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 28 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 120 });
                _chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here for MinPointHeight value at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueSleep(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        [TestMethod]
        [Asynchronous]
        public void StressTestMinPointHeightInSection()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            _chart = new Chart();
            _chart.Width = 400;
            _chart.Height = 350;
            _chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            _chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(_chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 200 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 160 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 2.8 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 10 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 28 });
                dataSeries.DataPoints.Add(new DataPoint() { YValue = 120 });
                _chart.Series.Add(dataSeries);
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", "Click here for MinPointHeight value at realtime.");
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueSleep(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement1.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement2_OnClick));
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        #endregion

        #region TestLegendAlignment
        /// <summary>
        /// Test Legend Alignment in StreamLine
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestLegendAlignmentInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            TestPanel.Children.Add(chart);

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                legend.VerticalAlignment = VerticalAlignment.Bottom;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                legend.VerticalAlignment = VerticalAlignment.Center;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                legend.VerticalAlignment = VerticalAlignment.Top;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Center;
                legend.VerticalAlignment = VerticalAlignment.Top;
            });

            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Right;
                legend.VerticalAlignment = VerticalAlignment.Top;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Right;
                legend.VerticalAlignment = VerticalAlignment.Center;
            });

            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Right;
                legend.VerticalAlignment = VerticalAlignment.Bottom;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Center;
                legend.VerticalAlignment = VerticalAlignment.Bottom;
            });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Test Legend Alignment in Section
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestLegendAlignmentInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            TestPanel.Children.Add(chart);

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                legend.VerticalAlignment = VerticalAlignment.Bottom;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                legend.VerticalAlignment = VerticalAlignment.Center;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Left;
                legend.VerticalAlignment = VerticalAlignment.Top;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Center;
                legend.VerticalAlignment = VerticalAlignment.Top;
            });

            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Right;
                legend.VerticalAlignment = VerticalAlignment.Top;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Right;
                legend.VerticalAlignment = VerticalAlignment.Center;
            });

            EnqueueSleep(1000);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Right;
                legend.VerticalAlignment = VerticalAlignment.Bottom;
            });

            EnqueueSleep(_sleepTime);

            EnqueueCallback(() =>
            {
                legend.HorizontalAlignment = HorizontalAlignment.Center;
                legend.VerticalAlignment = VerticalAlignment.Bottom;
            });

            EnqueueTestComplete();
        }
        #endregion

        #region Performance Tests
        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void StreamLineFunnelStressTest()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            Int32 numberofDataPoint = 0;

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.ShowInLegend = false;

                for (Int32 i = 0; i < 100; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg + " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueSleep(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }

        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [Asynchronous]
        public void SectionFunnelStressTest()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            Random rand = new Random();

            Int32 numberofDataPoint = 0;

            Double totalDuration = 0;
            DateTime start = DateTime.UtcNow;
            String msg = Common.AssertAverageDuration(200, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.ShowInLegend = false;

                for (Int32 i = 0; i < 100; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "a" + i;
                    dataPoint.YValue = rand.Next(-200, 200);
                    dataSeries.DataPoints.Add(dataPoint);
                    numberofDataPoint++;
                }
                numberOfSeries++;
                chart.Series.Add(dataSeries);
            });

            EnqueueConditional(() => { return _isLoaded; });

            EnqueueCallback(() =>
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;
            });

            EnqueueCallback(() =>
            {
                _htmlElement1 = Common.GetDisplayMessageButton(_htmlElement1);
                _htmlElement1.SetStyleAttribute("width", "900px");
                _htmlElement1.SetProperty("value", dataSeries.RenderAs + " chart with " + numberOfSeries + " DataSeries having " + numberofDataPoint + " DataPoints. Total Chart Loading Time: " + totalDuration + "s. Number of Render Count: " + chart.ChartArea._renderCount);
                _htmlElement2 = Common.GetDisplayMessageButton(_htmlElement2);
                _htmlElement2.SetStyleAttribute("top", "540px");
                _htmlElement2.SetProperty("value", "Total Calculation: " + msg + " Click here to exit.");
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement2);
            });

            EnqueueSleep(_sleepTime);
            EnqueueCallback(() =>
            {
                _htmlElement2.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.HtmlElement_OnClick));
            });
        }
        #endregion

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlElement_OnClick(object sender, HtmlEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();

            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement3);
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HtmlElement2_OnClick(object sender, HtmlEventArgs e)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1500);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            _chart.Series[0].MinPointHeight = rand.Next(0, 100);
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View3D_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            if (_chart.View3D)
                _chart.View3D = false;
            else
                _chart.View3D = true;

            _htmlElement3.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchDataSeries_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            if (_chart.Series[0].RenderAs == RenderAs.StreamLineFunnel)
                _chart.Series[0].RenderAs = RenderAs.SectionFunnel;
            else
                _chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            _htmlElement3.SetProperty("value", "Number of Render Count: " + _chart.ChartArea._renderCount + ". Click here to exit.");
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            try
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement3);
                System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
            }
            catch { }
        }

        /// <summary>
        /// Event handler for loaded event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _isLoaded = true;
        }

        #region Private Data

        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;
        
        /// <summary>
        /// Chart 
        /// </summary>
        private Chart _chart;

        // Create a new instance of Random class
        private Random rand = new Random();

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        /// <summary>
        /// Dispatch Timer
        /// </summary>
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        
        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement1;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement2;

        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement3;

        #endregion
    }
}