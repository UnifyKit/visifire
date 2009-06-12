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
    /// Summary description for FunnelChartTests
    /// </summary>
    [TestClass]
    public class FunnelChartTests
    {
        #region StreamLineFunnelProperties

        #region CheckFunnelDefaultPropertyValue

        /// <summary>
        /// Check the default value of MinPointHeight
        /// </summary>
        [TestMethod]
        public void StreamLineMinPointHeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Double.NaN, chart.Series[0].MinPointHeight);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        public void StreamLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].Enabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelEnabled
        /// </summary>
        [TestMethod]
        public void StreamLineLabelEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelStyle
        /// </summary>
        [TestMethod]
        public void StreamLineLabelStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LabelStyles.OutSide, (LabelStyles)chart.Series[0].LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelLineEnabled
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelLineStyle
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, (LineStyles)chart.Series[0].LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        public void StreamLineBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;
            
            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].Bevel);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LightingEnabled
        /// </summary>
        [TestMethod]
        public void StreamLineLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].LightingEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        public void StreamLineShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse((Boolean)chart.Series[0].ShadowEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Exploded
        /// </summary>
        [TestMethod]
        public void StreamLineExplodedDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse((Boolean)chart.Series[0].Exploded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ShowInLegend
        /// </summary>
        [TestMethod]
        public void StreamLineShowInLegendDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckFunnelNewPropertyValue
        /// <summary>
        /// Check the new value of MinPointHeight
        /// </summary>
        [TestMethod]
        public void StreamLineMinPointHeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].YValue = 1;
                chart.Series[0].DataPoints[4].YValue = 5;
                chart.Series[0].MinPointHeight = 20;
                Assert.AreEqual(20, chart.Series[0].MinPointHeight);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
            
        }

        /// <summary>
        /// Check the new value of View3D
        /// </summary>
        [TestMethod]
        public void StreamLineView3DNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.View3D = true;
                Assert.IsTrue(chart.View3D);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Color in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineColorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Color = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.Series[0].Color);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Color in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineColorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    chart.Series[0].DataPoints[i].Color = new SolidColorBrush(Colors.Yellow);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Yellow), chart.Series[0].DataPoints[i].Color);
                }
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Cursor in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineCursorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Cursor = Cursors.Hand;
                Assert.AreEqual(Cursors.Hand, chart.Series[0].Cursor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Cursor in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineCursorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    chart.Series[0].DataPoints[i].Cursor = Cursors.Hand;
                    Assert.AreEqual(Cursors.Hand, chart.Series[0].DataPoints[i].Cursor);
                }
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Enabled in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Enabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].Enabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Enabled in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].Enabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].Enabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Opacity in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineOpacityInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Opacity = 0.5;
                Assert.AreEqual(0.5, chart.Series[0].Opacity, Common.HighPrecisionDelta);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Opacity in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineOpacityInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].Opacity = 0.5;
                Assert.AreEqual(0.5, chart.Series[0].DataPoints[2].Opacity, Common.HighPrecisionDelta);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LegendText in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineLegendTextInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LegendText = "Legend";
                Assert.AreEqual("Legend", chart.Series[0].LegendText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LegendText in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineLegendTextInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].LegendText = "Legend";
                Assert.AreEqual("Legend", chart.Series[0].DataPoints[2].LegendText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineLabelEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineLabelEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].LabelEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineLabelStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelStyle = LabelStyles.Inside;
                Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineLabelStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].LabelStyle = LabelStyles.Inside;
                Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].DataPoints[2].LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new` value of LabelLineEnabled in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelLineEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineEnabled in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].LabelLineEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[3].LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelLineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, (LineStyles)chart.Series[0].LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].LabelLineStyle = LineStyles.Dotted;
                Assert.AreEqual(LineStyles.Dotted, (LineStyles)chart.Series[0].DataPoints[3].LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineThicknessInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelLineThickness = 2;
                Assert.AreEqual(2, chart.Series[0].LabelLineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineLabelLineThicknessInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].LabelLineThickness = 3;
                Assert.AreEqual(3, chart.Series[0].DataPoints[3].LabelLineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Bevel
        /// </summary>
        [TestMethod]
        public void StreamLineBevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Bevel = false;
                Assert.IsFalse((Boolean)chart.Series[0].Bevel);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LightingEnabled
        /// </summary>
        [TestMethod]
        public void StreamLineLightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LightingEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].LightingEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Exploded in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineExplodedInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Exploded = true;
                Assert.IsTrue((Boolean)chart.Series[0].Exploded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Exploded in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineExplodedInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].Exploded = true;
                Assert.IsTrue((Boolean)chart.Series[0].DataPoints[2].Exploded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineShowInLegendInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].ShowInLegend = false;
                Assert.IsFalse((Boolean)chart.Series[0].ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineShowInLegendInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].ShowInLegend = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Href in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineHrefInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", chart.Series[0].Href);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Href in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineHrefInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", chart.Series[0].DataPoints[3].Href);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataSeries
        /// </summary>
        [TestMethod]
        public void StreamLineHrefTargetInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Href = "http://www.visifire.com";
                chart.Series[0].HrefTarget = HrefTargets._blank;
                Assert.AreEqual(HrefTargets._blank, chart.Series[0].HrefTarget);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataPoint
        /// </summary>
        [TestMethod]
        public void StreamLineHrefTargetInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.StreamLineFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].Href = "http://www.visifire.com";
                chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank;
                Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #endregion

        #region SectionFunnelProperties

        #region CheckFunnelDefaultPropertyValue

        /// <summary>
        /// Check the default value of MinPointHeight
        /// </summary>
        [TestMethod]
        public void SectionMinPointHeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(Double.NaN, chart.Series[0].MinPointHeight);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        public void SectionEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].Enabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelEnabled
        /// </summary>
        [TestMethod]
        public void SectionLabelEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelStyle
        /// </summary>
        [TestMethod]
        public void SectionLabelStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LabelStyles.OutSide, (LabelStyles)chart.Series[0].LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelLineEnabled
        /// </summary>
        [TestMethod]
        public void SectionLabelLineEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LabelLineStyle
        /// </summary>
        [TestMethod]
        public void SectionLabelLineStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(LineStyles.Solid, (LineStyles)chart.Series[0].LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        public void SectionBevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].Bevel);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of LightingEnabled
        /// </summary>
        [TestMethod]
        public void SectionLightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].LightingEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        public void SectionShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse((Boolean)chart.Series[0].ShadowEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of Exploded
        /// </summary>
        [TestMethod]
        public void SectionExplodedDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsFalse((Boolean)chart.Series[0].Exploded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the default value of ShowInLegend
        /// </summary>
        [TestMethod]
        public void SectionShowInLegendDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.IsTrue((Boolean)chart.Series[0].ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region CheckFunnelNewPropertyValue
        /// <summary>
        /// Check the new value of MinPointHeight
        /// </summary>
        [TestMethod]
        public void SectionMinPointHeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].YValue = 1;
                chart.Series[0].DataPoints[4].YValue = 5;
                chart.Series[0].MinPointHeight = 20;
                Assert.AreEqual(20, chart.Series[0].MinPointHeight);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of View3D
        /// </summary>
        [TestMethod]
        public void SectionView3DNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.View3D = true;
                Assert.IsTrue(chart.View3D);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Color in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionColorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Color = new SolidColorBrush(Colors.Red);
                Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.Series[0].Color);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Color in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionColorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    chart.Series[0].DataPoints[i].Color = new SolidColorBrush(Colors.Yellow);
                    Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Yellow), chart.Series[0].DataPoints[i].Color);
                }
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Cursor in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionCursorInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Cursor = Cursors.Hand;
                Assert.AreEqual(Cursors.Hand, chart.Series[0].Cursor);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
            
        }

        /// <summary>
        /// Check the new value of Cursor in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionCursorInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                for (Int32 i = 0; i < 5; i++)
                {
                    chart.Series[0].DataPoints[i].Cursor = Cursors.Hand;
                    Assert.AreEqual(Cursors.Hand, chart.Series[0].DataPoints[i].Cursor);
                }
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Enabled in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Enabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].Enabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Enabled in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].Enabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].Enabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Opacity in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionOpacityInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Opacity = 0.5;
                Assert.AreEqual(0.5, chart.Series[0].Opacity, Common.HighPrecisionDelta);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Opacity in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionOpacityInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].Opacity = 0.5;
                Assert.AreEqual(0.5, chart.Series[0].DataPoints[2].Opacity, Common.HighPrecisionDelta);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LegendText in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionLegendTextInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LegendText = "Legend";
                Assert.AreEqual("Legend", chart.Series[0].LegendText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LegendText in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionLegendTextInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].LegendText = "Legend";
                Assert.AreEqual("Legend", chart.Series[0].DataPoints[2].LegendText);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataSeries
        /// </summary>
        [TestMethod]
        public void SectionLabelEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelEnabled for DataPoint
        /// </summary>
        [TestMethod]
        public void SectionLabelEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].LabelEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].LabelEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionLabelStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelStyle = LabelStyles.Inside;
                Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelStyle in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionLabelStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].LabelStyle = LabelStyles.Inside;
                Assert.AreEqual(LabelStyles.Inside, (LabelStyles)chart.Series[0].DataPoints[2].LabelStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new` value of LabelLineEnabled in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionLabelLineEnabledInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelLineEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineEnabled in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionLabelLineEnabledInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].LabelLineEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[3].LabelLineEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
            
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionLabelLineStyleInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelLineStyle = LineStyles.Dashed;
                Assert.AreEqual(LineStyles.Dashed, (LineStyles)chart.Series[0].LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineStyle in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionLabelLineStyleInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].LabelLineStyle = LineStyles.Dotted;
                Assert.AreEqual(LineStyles.Dotted, (LineStyles)chart.Series[0].DataPoints[3].LabelLineStyle);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionLabelLineThicknessInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LabelLineThickness = 2;
                Assert.AreEqual(2, chart.Series[0].LabelLineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LabelLineThickness in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionLabelLineThicknessInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].LabelLineThickness = 3;
                Assert.AreEqual(3, chart.Series[0].DataPoints[3].LabelLineThickness);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Bevel
        /// </summary>
        [TestMethod]
        public void SectionBevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;
            
            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Bevel = false;
                Assert.IsFalse((Boolean)chart.Series[0].Bevel);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of LightingEnabled
        /// </summary>
        [TestMethod]
        public void SectionLightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].LightingEnabled = false;
                Assert.IsFalse((Boolean)chart.Series[0].LightingEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ShadowEnabled
        /// </summary>
        [TestMethod]
        public void SectionShadowEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].ShadowEnabled = true;
                Assert.IsTrue((Boolean)chart.Series[0].ShadowEnabled);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Exploded in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionExplodedInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Exploded = true;
                Assert.IsTrue((Boolean)chart.Series[0].Exploded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Exploded in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionExplodedInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].Exploded = true;
                Assert.IsTrue((Boolean)chart.Series[0].DataPoints[2].Exploded);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionShowInLegendInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].ShowInLegend = false;
                Assert.IsFalse((Boolean)chart.Series[0].ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of ShowInLegend in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionShowInLegendInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[2].ShowInLegend = false;
                Assert.IsFalse((Boolean)chart.Series[0].DataPoints[2].ShowInLegend);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Href in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionHrefInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", chart.Series[0].Href);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of Href in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionHrefInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].Href = "http://www.visifire.com";
                Assert.AreEqual("http://www.visifire.com", chart.Series[0].DataPoints[3].Href);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataSeries
        /// </summary>
        [TestMethod]
        public void SectionHrefTargetInDataSeriesNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].Href = "http://www.visifire.com";
                chart.Series[0].HrefTarget = HrefTargets._blank;
                Assert.AreEqual(HrefTargets._blank, chart.Series[0].HrefTarget);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        /// <summary>
        /// Check the new value of HrefTarget in DataPoint
        /// </summary>
        [TestMethod]
        public void SectionHrefTargetInDataPointNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Common.CreateAndAddDefaultDataSeries(chart);
            chart.Series[0].RenderAs = RenderAs.SectionFunnel;

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                chart.Series[0].DataPoints[3].Href = "http://www.visifire.com";
                chart.Series[0].DataPoints[3].HrefTarget = HrefTargets._blank;
                Assert.AreEqual(HrefTargets._blank, chart.Series[0].DataPoints[3].HrefTarget);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
        #endregion

        #endregion

        #region TestSingleDataPoint

        //[TestMethod]
        public void TestSingleDataPointStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 20 });
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(1, chart.Series[0].DataPoints.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestSingleDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 1, YValue = 20 });
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(1, chart.Series[0].DataPoints.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestNullDataPoint

        [TestMethod]
        public void TestNullDataPointStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.StreamLineFunnel;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0, chart.Series[0].DataPoints.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestNullDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0, chart.Series[0].DataPoints.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestZeroValueDataPoint

        [TestMethod]
        public void TestZeroXValueDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { XValue = 0, YValue = 20 });
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0, chart.Series[0].DataPoints.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestZeroYValueDataPointSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.SectionFunnel;
            dataSeries.DataPoints.Add(new DataPoint() { YValue = 0 });
            chart.Series.Add(dataSeries);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                Assert.AreEqual(0, chart.Series[0].DataPoints.Count);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestNegativeValue

        [TestMethod]
        public void TestNegativeValueInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestNegativeValueInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestLabelText

        [TestMethod]
        public void TestLabelTextInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestLabelTextInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestLongLabelTextIn2DStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "Visifire StreamLine Funnel Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestLongLabelTextIn2DSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "Visifire Section Funnel Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestLongLabelTextIn3DStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.StreamLineFunnel;
                dataSeries.LabelText = "Visifire StreamLine Funnel Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestLongLabelTextIn3DSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.SectionFunnel;
                dataSeries.LabelText = "Visifire Section Funnel Chart, AxisXLabel = #AxisXLabel, #YValue = #YValue";

                for (Int32 i = 0; i < 10; i++)
                {
                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = "Visifire";
                    dataPoint.YValue = rand.Next(-100, 100);
                    dataSeries.DataPoints.Add(dataPoint);
                }

                chart.Series.Add(dataSeries);
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestDecimalValues

        [TestMethod]
        public void TestDecimalValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestDecimalValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestNegativeXValues

        [TestMethod]
        public void TestNegativeXValueInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestNegativeXValueInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestSameXValues

        [TestMethod]
        public void TestSameXValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestSameXValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestDateValues

        [TestMethod]
        public void TestDateValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestDateValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestDateTimeValues

        [TestMethod]
        public void TestDateTimeValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestDateTimeValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region TestTimeValues

        [TestMethod]
        public void TestTimeValuesInStreamLine()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        [TestMethod]
        public void TestTimeValuesInSection()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            Random rand = new Random();

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
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
            }
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }

        #endregion

        #region Performance Tests
        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void StreamLineFunnelStressTest()
        {
            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n" + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }

        /// <summary>
        /// Performance and Stress
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void SectionFunnelStressTest()
        {
            Int32 numberOfSeries = 0;
            DataSeries dataSeries = null;

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 350;
            chart.View3D = true;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

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

            Window window = new Window();
            window.Content = chart;
            window.Show();
            if (_isLoaded)
            {
                DateTime end = DateTime.UtcNow;
                totalDuration = (end - start).TotalSeconds;

                MessageBox.Show("Total Chart Loading Time: " + totalDuration + "s" + "\n" + "Number of Render Count: " + chart.ChartArea._renderCount + "\n" + "Series Calculation: " + msg);
            }
            window.Dispatcher.InvokeShutdown();
        }
        #endregion

        void timer_Tick(object sender, EventArgs e)
        {
            chart.Series[0].MinPointHeight = rand.Next(0, 100);
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
        private Chart chart;

        // Create a new instance of Random class
        private Random rand = new Random();

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}
