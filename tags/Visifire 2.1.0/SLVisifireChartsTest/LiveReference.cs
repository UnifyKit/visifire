﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;

namespace SLVisifireChartsTest
{
    /// <summary>
    /// LiveReference is used to track elements that are added to the testing 
    /// surface canvas so they can automatically be removed once the test is
    /// complete.
    /// </summary> 
    public sealed class LiveReference : SilverlightTest, IDisposable
    {
        private LiveReference() { }

        private SilverlightTest _testBase;

        /// <summary>
        /// Element added to the testing surface canvas.
        /// </summary> 
        public UIElement Element { get; private set; }

        /// <summary> 
        /// Initializes a new instance of the LiveReference class.
        /// </summary>
        /// <param name="element"> 
        /// Element to add to the testing surface canvas.
        /// </param>
        public LiveReference(SilverlightTest silverlightTest, UIElement element)
        {
            _testBase = silverlightTest;
            
            Assert.IsNotNull(_testBase);
            Assert.IsNotNull(_testBase.TestPanel);
            Assert.IsNotNull(_testBase.TestPanel);

            Element = element;
            TestPanel.Children.Add(Element); 
        }

        /// <summary>
        /// Remove the element from the testing surface canvas when finished
        /// </summary> 
        public void Dispose()
        {
            Assert.IsNotNull(Element);
            Assert.IsNotNull(_testBase);
            Assert.IsNotNull(_testBase.TestPanel);
            Assert.IsNotNull(_testBase.TestPanel);
            bool removed = _testBase.TestPanel.Children.Remove(Element);
            Assert.IsTrue(removed);
        }
    }
}
