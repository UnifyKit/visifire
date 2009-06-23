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
    public class FinancialChartsTest:SilverlightControlTest
    {
        #region CandleStickProperties

        #region CheckCandleStickDefaultPropertyValue

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickLabelEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickLabelStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickLabelLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Series[0].LabelLineEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShowInLegend
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickShowInLegendDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.Series[0].ShowInLegend));
            EnqueueTestComplete();
        }

        #endregion

        #region CheckCandleStickNewPropertyValue

        /// <summary>
        /// Check the new value of View3D
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickView3DNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickColorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickColorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickCursorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickCursorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickOpacityInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickOpacityInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickLegendTextInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;
            chart.Series[0].ShowInLegend = true;

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
        public void CandleStickLegendTextInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].LegendText = "Legend";
                    chart.Series[0].DataPoints[2].ShowInLegend = true;
                    Assert.AreEqual("Legend", chart.Series[0].DataPoints[2].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickLabelEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelEnabled = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickLabelEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelEnabled = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].DataPoints[2].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickLabelStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickLabelStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].DataPoints[2].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickBevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate{
                    chart.Series[0].Bevel = false;
                    Assert.IsFalse((Boolean)chart.Series[0].Bevel);
                });
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LightingEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickLightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LightingEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickShowInLegendInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickShowInLegendInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickHrefInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickHrefInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickHrefTargetInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

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
        public void CandleStickHrefTargetInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget));
            EnqueueTestComplete();
        }
        #endregion

        #endregion

        #region SectionStockProperties

        #region CheckStockDefaultPropertyValue

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockLabelEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockLabelStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LabelStyles.OutSide, (LabelStyles)chart.Series[0].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShowInLegend
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockShowInLegendDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse((Boolean)chart.Series[0].ShowInLegend));
            EnqueueTestComplete();
        }

        #endregion

        #region CheckStockNewPropertyValue
       
        /// <summary>
        /// Check the new value of UniqueColors
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockUniqueColorsNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.UniqueColors = false;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockView3DNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockColorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockColorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockCursorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockCursorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockOpacityInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockOpacityInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockLegendTextInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;
            chart.Series[0].ShowInLegend = true;

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
        public void StockLegendTextInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].DataPoints[2].LegendText = "Legend";
                    chart.Series[0].DataPoints[2].ShowInLegend = true;
                    Assert.AreEqual("Legend", chart.Series[0].DataPoints[2].LegendText);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockLabelEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LabelEnabled = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataPoint
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockLabelEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelEnabled = true,
                () => Assert.IsTrue((Boolean)chart.Series[0].DataPoints[2].LabelEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockLabelStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockLabelStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[2].LabelStyle = LabelStyles.Inside,
                () => Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].DataPoints[2].LabelStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockBevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockLightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].LightingEnabled = false,
                () => Assert.IsFalse((Boolean)chart.Series[0].LightingEnabled));
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataSeries
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockShowInLegendInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockShowInLegendInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockHrefInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockHrefInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockHrefTargetInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

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
        public void StockHrefTargetInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget));
            EnqueueTestComplete();
        }
        #endregion

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