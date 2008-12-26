using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Collections;
using Visifire.Charts;

namespace SLVisifireChartsTest
{
    [TestClass]
    public class MajorTicksTest : SilverlightControlTest
    {
        #region Test default property value
        /// <summary>
        /// Check LineThickness default value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDefaultLineThickness()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(0.5, chart.AxesX[0].Ticks[0].LineThickness),
                () => Assert.AreEqual(0.5, chart.AxesY[0].Ticks[0].LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check LineStyle default value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDefaultLineStyle()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(LineStyles.Solid, chart.AxesX[0].Ticks[0].LineStyle),
                () => Assert.AreEqual(LineStyles.Solid, chart.AxesY[0].Ticks[0].LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check LineColor default value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDefaultLineColor()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesX[0].Ticks[0].LineColor),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Gray), chart.AxesY[0].Ticks[0].LineColor));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check LineColor default value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDefaultInterval()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.AxesX[0].Ticks[0].Interval));

            EnqueueTestComplete();
        }
        #endregion

        #region Test new property value
        /// <summary>
        /// Check LineThickness new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckNewLineThickness()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].Ticks[0].LineThickness = 2,
                () => Assert.AreEqual(2, chart.AxesX[0].Ticks[0].LineThickness),
                () => chart.AxesY[0].Ticks[0].LineThickness = 2,
                () => Assert.AreEqual(2, chart.AxesY[0].Ticks[0].LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check LineStyle new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckNewLineStyle()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].Ticks[0].LineStyle = LineStyles.Dashed,
                () => Assert.AreEqual(LineStyles.Dashed, chart.AxesX[0].Ticks[0].LineStyle),
                () => chart.AxesY[0].Ticks[0].LineStyle = LineStyles.Dotted,
                () => Assert.AreEqual(LineStyles.Dotted, chart.AxesY[0].Ticks[0].LineStyle));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check LineColor new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckNewLineColor()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].Ticks[0].LineColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.AxesX[0].Ticks[0].LineColor),
                () => chart.AxesY[0].Ticks[0].LineColor = new SolidColorBrush(Colors.Red),
                () => Common.AssertBrushesAreEqual(new SolidColorBrush(Colors.Red), chart.AxesY[0].Ticks[0].LineColor));


            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Interval new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckNewInterval()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.AxesX[0].Ticks[0].Interval = 2;
                    Assert.AreEqual(2, chart.AxesX[0].Ticks[0].Interval);
                    chart.AxesY[0].Ticks[0].Interval = 25;
                    Assert.AreEqual(25, chart.AxesY[0].Ticks[0].Interval);
                });
            EnqueueTestComplete();
        }
        #endregion

        #region Private Data
        private const int sleepTime = 1000;
        #endregion
    }
}