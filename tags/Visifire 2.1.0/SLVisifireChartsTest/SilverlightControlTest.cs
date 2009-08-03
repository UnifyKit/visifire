using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SLVisifireChartsTest
{
    /// <summary> 
    /// Unit tests for Silverlight controls. 
    /// </summary>
    public abstract class SilverlightControlTest : SilverlightTest
    {
        /// <summary>
        /// Number of milliseconds to wait between actions in CreateAsyncTest. 
        /// </summary>
        public static int VisualDelayInMilliseconds = 1000;
        protected const int NumberOfInstancesForStressScenarios = 200;

        protected void CreateAsyncTask(FrameworkElement element, params Action[] actions)
        {
            Assert.IsNotNull(element);
            actions = actions ?? new Action[] { };

            // Add a handler to determine when the element is loaded 
            bool isLoaded = false;
            element.Loaded += delegate { isLoaded = true; };

            // Add the element to the test surface and wait until it's loaded
            TestPanel.Children.Add(element);
            EnqueueConditional(() => isLoaded);

            // Perform the test actions
            foreach (Action action in actions)
            {
                Action capturedAction = action;
                EnqueueCallback(() => capturedAction());
                EnqueueSleep(VisualDelayInMilliseconds);
            }

            // Remove the element from the test surface and finish the test
            EnqueueCallback(() => TestPanel.Children.Remove(element));
        }

        protected void CreateAsyncTest(FrameworkElement element, params Action[] actions)
        {
            CreateAsyncTask(element, actions);
            EnqueueTestComplete();

        }
    }
}
