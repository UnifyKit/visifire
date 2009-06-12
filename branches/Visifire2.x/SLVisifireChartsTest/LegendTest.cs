using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Legend class 
    /// </summary>
    [TestClass]
    public class LegendTest : SilverlightControlTest
    {
        #region CheckLegendDefaultPropertyValue
        /// <summary>
        /// Check the default value of Href
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void HrefDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Legends[0].Href));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of HrefTarget
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void HrefTargetDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Visifire.Commons.HrefTargets._self, chart.Legends[0].HrefTarget));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LabelMargin
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LabelMarginDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(6, chart.Legends[0].LabelMargin));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Padding
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void PaddingDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new Thickness(4), chart.Legends[0].Padding));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of HorizontalAlignment
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void HorizontalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(HorizontalAlignment.Center, chart.Legends[0].HorizontalAlignment));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of VerticalAlignment
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void VerticalAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(VerticalAlignment.Bottom, chart.Legends[0].VerticalAlignment));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of BorderColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void BorderColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Black), chart.Legends[0].BorderColor));

            EnqueueTestComplete();
        }
        
        /// <summary>
        /// Check the default value of BorderThickness
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void BorderThicknessDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new Thickness(0), chart.Legends[0].BorderThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Background
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void BackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Transparent), chart.Legends[0].Background));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of DockInsidePlotArea
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void DockInsidePlotAreaDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse(chart.Legends[0].DockInsidePlotArea));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Enabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void EnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.Legends[0].Enabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void FontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Legends[0].FontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontFamily
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void FontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new FontFamily("Arial"), chart.Legends[0].FontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontSize
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void FontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(11, chart.Legends[0].FontSize, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void FontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontStyles.Normal, chart.Legends[0].FontStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of FontWeight
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void FontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontWeights.Normal, chart.Legends[0].FontWeight));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of LightingEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LightingEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue(chart.Legends[0].LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of CornerRadius
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CornerRadiusDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            //Legend legend = new Legend();
            //legend.SetValue(Control.NameProperty, "Legend0");
            //chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new CornerRadius(1), chart.Legends[0].CornerRadius));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Title
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Legends[0].Title));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleALignmentX
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleAlignmentXDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(HorizontalAlignment.Stretch, chart.Legends[0].TitleAlignmentX));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleTextALignment
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleTextAlignmentDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(TextAlignment.Center, chart.Legends[0].TitleTextAlignment));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleBackground
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleBackgroundDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.LightGray), chart.Legends[0].TitleBackground));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontColor
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleFontColorDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.Legends[0].TitleFontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontFamily
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleFontFamilyDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new FontFamily("Arial"), chart.Legends[0].TitleFontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontSize
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleFontSizeDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(12, chart.Legends[0].TitleFontSize));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontStyle
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleFontStyleDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontStyles.Normal, chart.Legends[0].TitleFontStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of TitleFontWeight
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TitleFontWeightDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(FontWeights.Normal, chart.Legends[0].TitleFontWeight));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of EntryMargin
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void EntryMarginDefaultValue()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(3, chart.Legends[0].EntryMargin));

            EnqueueTestComplete();
        }
        #endregion

        #region CheckNewPropertyValue

        /// <summary>
        /// Check the new value of Href. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Href.")]
        [Owner("[....]")]
        [Asynchronous]
        public void HrefNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.HrefTarget = HrefTargets._blank,
                () => legend.Href = "http://www.visifire.com",
                () => Assert.AreEqual("http://www.visifire.com", legend.Href));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Opacity. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Opacity.")]
        [Owner("[....]")]
        [Asynchronous]
        public void OpacityNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.Opacity = 0.5,
                () => Assert.AreEqual(0.5, legend.Opacity, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Cursor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Cursor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CursorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.Cursor = Cursors.Hand,
                () => Assert.AreEqual(Cursors.Hand, legend.Cursor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of LabelMargin. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LabelMargin.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LabelMarginNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            
            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.LabelMargin = 10,
                () => Assert.AreEqual(10, legend.LabelMargin));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of EntryMargin. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of EntryMargin.")]
        [Owner("[....]")]
        [Asynchronous]
        public void EntryMarginNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.LabelMargin = 10,
                () => Assert.AreEqual(10, legend.LabelMargin));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of VerticalAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of VerticalAlignment.")]
        [Owner("[....]")]
        [Asynchronous]
        public void VerticalAlignmentNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            
            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.VerticalAlignment = VerticalAlignment.Top,
                () => Assert.AreEqual(VerticalAlignment.Top, legend.VerticalAlignment));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of HorizontalAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of HorizontalAlignment.")]
        [Owner("[....]")]
        [Asynchronous]
        public void HorizontalAlignmentNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.HorizontalAlignment = HorizontalAlignment.Left,
                () => Assert.AreEqual(HorizontalAlignment.Left, legend.HorizontalAlignment));

            EnqueueTestComplete();
        }


        /// <summary>
        /// Check the new value of HorizontalAlignment. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of BorderThickness.")]
        [Owner("[....]")]
        [Asynchronous]
        public void BorderThicknessNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.BorderThickness = new Thickness(2),
                () => Assert.AreEqual(new Thickness(2), legend.BorderThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of BorderColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of BorderColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void BorderColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.BorderThickness = new Thickness(1);
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.BorderColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.BorderColor));

            EnqueueTestComplete();
        }

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

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.Background = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.Background));

            EnqueueSleep(_sleepTime);
            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Background. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of DockInsidePlotArea.")]
        [Owner("[....]")]
        [Asynchronous]
        public void DockInsidePlotAreaNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.DockInsidePlotArea = true,
                () => Assert.IsTrue(legend.DockInsidePlotArea));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Enabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Enabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void EnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);
            
            CreateAsyncTask(chart,
                () => legend.Enabled = false,
                () => Assert.AreEqual(false, legend.Enabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of FontColor. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontColor.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontColorNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.FontColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.FontColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of FontSize. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontSize.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontSizeNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.FontSize = 14,
                () => Assert.AreEqual(14, legend.FontSize, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of FontFamily. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontFamily.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontFamilyNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.FontFamily = new FontFamily("Times New Roman"),
                () => Assert.AreEqual(new FontFamily("Times New Roman"), legend.FontFamily));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of FontStyle. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontStyle.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontStyleNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.FontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, legend.FontStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of FontWeight. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of FontWeight.")]
        [Owner("[....]")]
        [Asynchronous]
        public void FontWeightNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.FontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, legend.FontWeight));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of lightingEnabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of LightingEnabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void LightingEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Background = new SolidColorBrush(Colors.Red);
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.LightingEnabled = true,
                () => Assert.IsTrue(legend.LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Padding. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Padding.")]
        [Owner("[....]")]
        [Asynchronous]
        public void PaddingNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.Padding = new Thickness(12),
                () => Assert.AreEqual(new Thickness(12), legend.Padding));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Margin. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Margin.")]
        [Owner("[....]")]
        [Asynchronous]
        public void MarginNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.Margin = new Thickness(10),
                () => Assert.AreEqual(new Thickness(10), legend.Margin));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of CornerRadius. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of CornerRadius.")]
        [Owner("[....]")]
        [Asynchronous]
        public void CornerRadiusNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.BorderThickness = new Thickness(1);
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.CornerRadius = new CornerRadius(5),
                () => Assert.AreEqual(new CornerRadius(5), legend.CornerRadius));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Title. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Title.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleRadiusNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.Title = "Legend",
                () => Assert.AreEqual("Legend", legend.Title));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of TitleAlignmentX. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleAlignmentX.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleAlignmentXNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleAlignmentX = HorizontalAlignment.Right,
                () => Assert.AreEqual(HorizontalAlignment.Right, legend.TitleAlignmentX));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of TitleBackground. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of TitleBackground.")]
        [Owner("[....]")]
        [Asynchronous]
        public void TitleBackgroundNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleBackground = new SolidColorBrush(Colors.Gray),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), legend.TitleBackground));

            EnqueueTestComplete();
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleFontColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), legend.TitleFontColor));

            EnqueueTestComplete();
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleFontSize = 15,
                () => Assert.AreEqual(15, legend.TitleFontSize));

            EnqueueTestComplete();
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleFontFamily = new FontFamily("Times New Roman"),
                () => Assert.AreEqual(new FontFamily("Times New Roman"), legend.TitleFontFamily));

            EnqueueTestComplete();
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleFontStyle = FontStyles.Italic,
                () => Assert.AreEqual(FontStyles.Italic, legend.TitleFontStyle));

            EnqueueTestComplete();
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.TitleFontWeight = FontWeights.Bold,
                () => Assert.AreEqual(FontWeights.Bold, legend.TitleFontWeight));

            EnqueueTestComplete();
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
            

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            CreateAsyncTask(chart,
                () => legend.ToolTipText = "Legend ToolTip",
                () => Assert.AreEqual("Legend ToolTip", legend.ToolTipText));

            EnqueueTestComplete();
        }

        #endregion

        #region TestLegendAlignment
        /// <summary>
        /// Testing Legend Alignment
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestLegendAlignment()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.View3D = true;
            

            TestPanel.Children.Add(chart);

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            EnqueueSleep(1000);

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

        #region LegendEventTesting
        /// <summary>
        /// Testing Evnets in Title
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void LegendEventChecking()
        {
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "400px");

            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;

            TestPanel.Children.Add(chart);

            Legend legend = new Legend();
            legend.SetValue(FrameworkElement.NameProperty, "Legend0");
            legend.Title = "Legend";
            chart.Legends.Add(legend);

            CreateAndAddDefaultDataSeries(chart, legend);

            legend.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement.SetProperty("value", "Legend MouseEnter event fired");
            };

            legend.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                _htmlElement.SetProperty("value", "Legend MouseLeave event fired");
            };

            legend.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement.SetProperty("value", "Legend MouseLeftButtonUp event fired");
            };

            legend.MouseLeftButtonDown += delegate(Object sender, MouseButtonEventArgs e)
            {
                _htmlElement.SetProperty("value", "Legend MouseLeftButtonDown event fired");
            };

            EnqueueCallback(() =>
            {
                _htmlElement.AttachEvent("onclick", new EventHandler<System.Windows.Browser.HtmlEventArgs>(this.LegendTest_OnClick));
            });

            _htmlElement = Common.GetDisplayMessageButton(_htmlElement);
            _htmlElement.SetStyleAttribute("width", "900px");
            _htmlElement.SetProperty("value", "Click here to exit.");
            System.Windows.Browser.HtmlPage.Document.Body.AppendChild(_htmlElement);

        }
        #endregion

        #region TestMultipleLegends
        /// <summary>
        /// Test multiple Legends
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void TestMultipleLegends()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            _isLoaded = false;
            chart.Loaded += new RoutedEventHandler(chart_Loaded);

            Random rand = new Random();

            TestPanel.Children.Add(chart);

            EnqueueConditional(() => { return _isLoaded; });
            EnqueueSleep(_sleepTime);

            Legend legend1 = new Legend();
            legend1.Name = "Legend1";
            legend1.Background = new SolidColorBrush(Colors.Red);
            chart.Legends.Add(legend1);
            Legend legend2 = new Legend();
            legend2.Name = "Legend2";
            legend1.Background = new SolidColorBrush(Colors.Green);
            chart.Legends.Add(legend2);
         
            EnqueueCallback(() =>
            {
                DataSeries dataSeries = new DataSeries();
                dataSeries.Legend = "Legend1";
                for (Int32 i = 0; i < 6; i++)
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = i + 1, YValue = rand.Next(10, 100) });
                chart.Series.Add(dataSeries);

                dataSeries = new DataSeries();
                dataSeries.Legend = "Legend2";
                for (Int32 i = 0; i < 6; i++)
                    dataSeries.DataPoints.Add(new DataPoint() { XValue = i + 1, YValue = rand.Next(10, 100) });
                chart.Series.Add(dataSeries);

            });

            EnqueueSleep(_sleepTime);
            EnqueueTestComplete();
        }
        #endregion

        /// <summary>
        /// Create DataSeries
        /// </summary>
        /// <param name="chart"></param>
        public void CreateAndAddDefaultDataSeries(Chart chart)
        {
            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.ShowInLegend = true;

            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.AxisXLabel = "a" + i;
                dataPoint.YValue = rand.Next(0, 100);
                dataPoint.XValue = i + 1;
                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);
        }

        /// <summary>
        /// Create DataSeries with refering legend
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="legend"></param>
        public void CreateAndAddDefaultDataSeries(Chart chart, Legend legend)
        {
            DataSeries dataSeries = new DataSeries();

            dataSeries.RenderAs = RenderAs.Column;
            dataSeries.Legend = legend.Name;
            dataSeries.ShowInLegend = true;
            
            Random rand = new Random();

            for (Int32 i = 0; i < 5; i++)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.ShowInLegend = true;
                dataPoint.AxisXLabel = "a" + i;
                dataPoint.YValue = rand.Next(0, 100);
                dataPoint.XValue = i + 1;
                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);
        }

        /// <summary>
        /// Event handler for click event of the Html element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LegendTest_OnClick(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            EnqueueTestComplete();
            System.Windows.Browser.HtmlPage.Document.Body.RemoveChild(_htmlElement);
            System.Windows.Browser.HtmlPage.Plugin.SetStyleAttribute("height", "100%");
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

        #region Private Data
        /// <summary>
        /// Html element reference
        /// </summary>
        private System.Windows.Browser.HtmlElement _htmlElement;

        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 2000;

        /// <summary>
        /// Whether the chart is loaded
        /// </summary>
        private bool _isLoaded = false;

        #endregion
    }
}
