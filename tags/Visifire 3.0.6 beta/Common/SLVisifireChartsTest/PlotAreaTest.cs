using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.PlotArea class 
    /// </summary>
    [TestClass]
    public class PlotAreaTest : SilverlightControlTest
    {
        #region CheckPlotAreaDefaultPropertyValue
        /// <summary>
        /// Check the default value of Bevel
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void BevelDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse(chart.PlotArea.Bevel));

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
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.PlotArea.BorderColor));

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
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new Thickness(0), chart.PlotArea.BorderThickness));

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
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.PlotArea.Background));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of Href
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void HrefDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNull(chart.PlotArea.Href));

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
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(Visifire.Commons.HrefTargets._self, chart.PlotArea.HrefTarget));

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
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse(chart.PlotArea.LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ShadowEnabled
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ShadowEnabledDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsFalse(chart.PlotArea.ShadowEnabled));

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
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(new CornerRadius(0), chart.PlotArea.CornerRadius));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the default value of ToolTipText
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void ToolTipTextDefaultValue()
        {
            Chart chart = new Chart();
            chart.AnimationEnabled = false;
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsNotNull(chart.PlotArea.ToolTipText));

            EnqueueTestComplete();
        }
        #endregion

        #region CheckPlotAreaNewPropertyValue

        /// <summary>
        /// Check the new value of Bevel. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of Bevel.")]
        [Owner("[....]")]
        [Asynchronous]
        public void BevelNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            chart.PlotArea.Background = new SolidColorBrush(Colors.Red);
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.Bevel = true,
                () => Assert.IsTrue(chart.PlotArea.Bevel));

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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.InternalOpacity = 0.5,
                () => Assert.AreEqual(0.5, chart.PlotArea.InternalOpacity, Common.HighPrecisionDelta));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of Bevel. 
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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            chart.PlotArea.BorderThickness = new Thickness(1);
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.BorderColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.PlotArea.BorderColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of BorderThickness. 
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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.BorderThickness = new Thickness(2),
                () => Assert.AreEqual(new Thickness(2), chart.PlotArea.BorderThickness));

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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.Background = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.PlotArea.Background));

            EnqueueTestComplete();
        }


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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.Href = "http://www.visifire.com",
                () => Assert.AreEqual("http://www.visifire.com", chart.PlotArea.Href));

            EnqueueTestComplete();
        }


        /// <summary>
        /// Check the new value of HrefTarget. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of HrefTarget.")]
        [Owner("[....]")]
        [Asynchronous]
        public void HrefTargetNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.HrefTarget = HrefTargets._blank,
                () => Assert.AreEqual(HrefTargets._blank, chart.PlotArea.HrefTarget));

            EnqueueTestComplete();
        }


        /// <summary>
        /// Check the new value of LightingEnabled. 
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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            chart.PlotArea.Background = new SolidColorBrush(Colors.Red);
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.LightingEnabled = true,
                () => Assert.IsTrue(chart.PlotArea.LightingEnabled));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check the new value of ShadowEnabled. 
        /// </summary> 
        [TestMethod]
        [Description("Check the new value of ShadowEnabled.")]
        [Owner("[....]")]
        [Asynchronous]
        public void ShadowEnabledNewValue()
        {
            Chart chart = new Chart();
            chart.Width = 400;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.ShadowEnabled = true,
                () => Assert.IsTrue(chart.PlotArea.ShadowEnabled));

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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            chart.PlotArea.BorderThickness = new Thickness(1);

            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.CornerRadius = new CornerRadius(5),
                () => Assert.AreEqual(new CornerRadius(5), chart.PlotArea.CornerRadius));

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
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            chart.PlotArea = new PlotArea();
            EnqueueDelay(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.PlotArea.ToolTipText = "ToolTip",
                () => Assert.AreEqual("ToolTip", chart.PlotArea.ToolTipText));

            EnqueueTestComplete();
        }
        #endregion

        #region Private Data
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTasks or Enqueue callbacks. 
        /// </summary>
        private const int _sleepTime = 1000;

        #endregion
    }
}
