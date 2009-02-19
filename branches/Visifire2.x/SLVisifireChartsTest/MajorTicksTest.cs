﻿using System;
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
    /// <summary>
    /// This class runs the unit tests Visifire.Charts.Ticks class 
    /// </summary>
    [TestClass]
    public class TicksTest : SilverlightControlTest
    {
        #region CheckingDefaultPropertyValue
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.AreEqual(1, chart.AxesX[0].Ticks[0].Interval));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Enabled default value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckDefaultEnabled()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => Assert.IsTrue((Boolean)chart.AxesX[0].Ticks[0].Enabled));

            EnqueueTestComplete();
        }
        #endregion

        #region CheckingNewPropertyValue

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

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].Ticks[0].LineThickness = 2,
                () => Assert.AreEqual(2, chart.AxesX[0].Ticks[0].LineThickness),
                () => chart.AxesY[0].Ticks[0].LineThickness = 2,
                () => Assert.AreEqual(2, chart.AxesY[0].Ticks[0].LineThickness));

            EnqueueTestComplete();
        }

        /// <summary>
        /// Check Opacity new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckNewOpacity()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                () => chart.AxesX[0].Ticks[0].Opacity = 0.5,
                () => Assert.AreEqual(0.5, chart.AxesX[0].Ticks[0].Opacity, Common.HighPrecisionDelta));

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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

            EnqueueSleep(_sleepTime);
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

        /// <summary>
        /// Check Enabled new value
        /// </summary>
        [TestMethod]
        [Asynchronous]
        public void CheckNewEnabled()
        {
            Chart chart = new Chart();
            chart.Width = 500;
            chart.Height = 300;
            chart.AnimationEnabled = false;

            Common.CreateAndAddDefaultDataSeries(chart);

            EnqueueSleep(_sleepTime);
            CreateAsyncTask(chart,
                delegate
                {
                    chart.AxesX[0].Ticks[0].Enabled = false;
                    Assert.IsFalse((Boolean)chart.AxesX[0].Ticks[0].Enabled);
                    chart.AxesY[0].Ticks[0].Enabled = false;
                    Assert.IsFalse((Boolean)chart.AxesY[0].Ticks[0].Enabled);
                });
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
