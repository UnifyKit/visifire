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
    public class FinancialChartsTest : SilverlightControlTest
    {
        #region CandleStickProperties

        #region CheckCandleStickDefaultPropertyValue

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.IsTrue((Boolean)chart.Series[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.IsFalse((Boolean)chart.Series[0].ShadowEnabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of PriceUpColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickPriceUpColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Color.FromArgb((Byte)0xFF, (Byte)0xA8, (Byte)0xD4, (Byte)0x4F)), chart.Series[0].PriceUpColor);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of PriceDownColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickPriceDownColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Color.FromArgb((Byte)0xFF, (Byte)0xDD, (Byte)0x00, (Byte)0x00)), chart.Series[0].PriceDownColor);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.View3D = true;
                    Assert.IsTrue(chart.View3D);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShadowEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickShadowEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].ShadowEnabled = true;
                    Assert.IsTrue((Boolean)chart.Series[0].ShadowEnabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of PriceUpColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickPriceUpColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].PriceUpColor = new SolidColorBrush(Colors.Blue);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Blue), chart.Series[0].PriceUpColor);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of PriceDownColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CandleStickPriceDownColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.CandleStick;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].PriceDownColor = new SolidColorBrush(Colors.DarkGray);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.DarkGray), chart.Series[0].PriceDownColor);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.IsTrue((Boolean)chart.Series[0].Enabled);
                });

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    Assert.IsFalse((Boolean)chart.Series[0].ShadowEnabled);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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
        /// Check the new value of ShadowEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void StockShadowEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateDefaultDataSeries4FinancialCharts(chart);
            chart.Series[0].RenderAs = RenderAs.Stock;

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.Series[0].ShadowEnabled = true;
                    Assert.IsTrue((Boolean)chart.Series[0].ShadowEnabled);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
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

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.Series[0].DataPoints[3].Href = "http://www.visifire.com",
                () => chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget));
            EnqueueTestComplete();
        }
        #endregion

        #endregion

        #region TestSingleDataPoint

        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointCandleStick()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.CandleStick;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { 20, 28, 42, 10 } });
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestSingleDataPointStock()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.CandleStick;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { 20, 28, 42, 10 } });
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        #endregion

        #region TestNullDataPoint

        [TestMethod]
        [Asynchronous]
        public void TestNullDataPointCandleStick()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.CandleStick;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestNullDataPointStock()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Stock;
            chart.Series.Add(dataSeries);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0, chart.Series[0].DataPoints.Count));
            EnqueueTestComplete();
        }

        #endregion

        #region TestNegativeXValuesValues

        [TestMethod]
        [Asynchronous]
        public void TestNegativeXValuesInCandleStick()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;

                for (Int32 i = 0; i < 12; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.XValue = -5 + i;
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestNegativeXValuesInStock()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;

                for (Int32 i = 0; i < 12; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.XValue = -5 + i;
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestSameXValuesValues

        [TestMethod]
        [Asynchronous]
        public void TestSameXValuesInCandleStick()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;

                Double open = rand.Next(50, 60);
                Double close = open + rand.Next(-10, 10);
                Double high = (open > close ? open : close) + 10;
                Double low = (open < close ? open : close) - 5;

                DataPoint dp = new DataPoint();
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestSameXValuesInStock()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;

                Double open = rand.Next(50, 60);
                Double close = open + rand.Next(-10, 10);
                Double high = (open > close ? open : close) + 10;
                Double low = (open < close ? open : close) - 5;

                DataPoint dp = new DataPoint();
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValues = new Double[] { open, close, high, low } });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestDateTimeValues

        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValuesInCandleStick()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;
                dataSeries.XValueType = ChartValueTypes.DateTime;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 1, 2, 8), YValues = new Double[] { 10, 20, 40, 5 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1, 3, 9, 20), YValues = new Double[] { 20, 30, 35, 10 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 1, 8, 22, 40), YValues = new Double[] { 22, 10, 28, 8 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 1, 9, 7, 24), YValues = new Double[] { 5, 8, 12, 2 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 1, 6, 8, 22), YValues = new Double[] { 18, 24, 38, 18 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 12, 1, 8, 20, 58), YValues = new Double[] { 30, 42, 48, 28 } });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestDateTimeValuesInStock()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;
                dataSeries.XValueType = ChartValueTypes.DateTime;

                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 1, 1, 1, 2, 8), YValues = new Double[] { 10, 20, 40, 5 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 2, 1, 3, 9, 20), YValues = new Double[] { 20, 30, 35, 10 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 3, 1, 8, 22, 40), YValues = new Double[] { 22, 10, 28, 8 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 4, 1, 9, 7, 24), YValues = new Double[] { 5, 8, 12, 2 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 5, 1, 6, 8, 22), YValues = new Double[] { 18, 24, 38, 18 } });
                dataSeries.DataPoints.Add(new DataPoint() { XValue = new DateTime(2001, 12, 1, 8, 20, 58), YValues = new Double[] { 30, 42, 48, 28 } });
                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestLabelText

        [TestMethod]
        [Asynchronous]
        public void TestLabelTextInCandleStick()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;
                dataSeries.LabelText = "Open: #Open, Close: #Close\nHigh: #High, Low: #Low";

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
                }
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLabelTextInStock()
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
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;
                dataSeries.LabelText = "Open: #Open, Close: #Close\nHigh: #High, Low: #Low";

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
                }
            });

            EnqueueTestComplete();
        }

        #endregion

        #region TestNegativeValues

        [TestMethod]
        [Asynchronous]
        public void TestNegativeValueInCandleStick()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(-50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestNegativeValueInStock()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(-50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }


        #endregion

        #region TestLessYValues

        [TestMethod]
        [Asynchronous]
        public void TestLessYValuesInCandleStick()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestLessYValuesInStock()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(-50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }


        #endregion

        #region TestMoreYValues

        [TestMethod]
        [Asynchronous]
        public void TestMoreYValuesInCandleStick()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestMoreYValuesInStock()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;

                for (Int32 i = 0; i < 6; i++)
                {
                    Double open = rand.Next(-50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low, low };
                    dataSeries.DataPoints.Add(dp);
                }

                chart.Series.Add(dataSeries);
            });

            EnqueueTestComplete();
        }


        #endregion

        #region TestMultiSeries

        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesInCandleStick()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 j = 0; j < 3; j++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.CandleStick;

                    for (Int32 i = 0; i < 6; i++)
                    {
                        Double open = rand.Next(50, 60);
                        Double close = open + rand.Next(-10, 10);
                        Double high = (open > close ? open : close) + 10;
                        Double low = (open < close ? open : close) - 5;

                        DataPoint dp = new DataPoint();
                        dp.YValues = new Double[] { open, close, high, low, low };
                        dataSeries.DataPoints.Add(dp);
                    }

                    chart.Series.Add(dataSeries);
                }
            });

            EnqueueTestComplete();
        }

        [TestMethod]
        [Asynchronous]
        public void TestMultiSeriesInStock()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueDelay(_sleepTime);

            EnqueueCallback(() =>
            {
                for (Int32 j = 0; j < 3; j++)
                {
                    DataSeries dataSeries = new DataSeries();
                    dataSeries.RenderAs = RenderAs.CandleStick;

                    for (Int32 i = 0; i < 6; i++)
                    {
                        Double open = rand.Next(50, 60);
                        Double close = open + rand.Next(-10, 10);
                        Double high = (open > close ? open : close) + 10;
                        Double low = (open < close ? open : close) - 5;

                        DataPoint dp = new DataPoint();
                        dp.YValues = new Double[] { open, close, high, low, low };
                        dataSeries.DataPoints.Add(dp);
                    }

                    chart.Series.Add(dataSeries);
                }
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
        public void CandleStickStressTest()
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
            String msg = Common.AssertAverageDuration(500, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.CandleStick;

                for (Int32 i = 0; i < 2000; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
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
        public void StockStressTest()
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
            String msg = Common.AssertAverageDuration(500, 1, delegate
            {
                dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Stock;

                for (Int32 i = 0; i < 2000; i++)
                {
                    Double open = rand.Next(50, 60);
                    Double close = open + rand.Next(-10, 10);
                    Double high = (open > close ? open : close) + 10;
                    Double low = (open < close ? open : close) - 5;

                    DataPoint dp = new DataPoint();
                    dp.YValues = new Double[] { open, close, high, low };
                    dataSeries.DataPoints.Add(dp);
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
        private void Exit_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            try
            {
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement1);
                System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement2);
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
        /// Chart reference
        /// </summary>
        private Chart _chart = null;

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

        #endregion
    }
}