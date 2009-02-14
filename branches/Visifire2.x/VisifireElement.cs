﻿/*   
    Copyright (C) 2008 Webyog Softworks Private Limited

    This file is a part of Visifire Charts.
 
    Visifire is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Visifire Charts.  If not, see <http://www.gnu.org/licenses/>.
  
    If GPL is not suitable for your products or company, Webyog provides Visifire 
    under a flexible commercial license designed to meet your specific usage and 
    distribution requirements. If you have already obtained a commercial license 
    from Webyog, you can use this file under those license terms.
    
*/

using System;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using Visifire.Charts;

#if SL
    using System.Windows.Browser;
#endif

namespace Visifire.Commons
{
    /// <summary>
    /// Base class for all Visifire elements including chart
    /// </summary>
    public abstract class VisifireElement: System.Windows.Controls.Control
    {   
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Commons.VisifireElement class
        /// </summary>
        public VisifireElement()
        {
        }

#if SL

        /// <summary>
        /// Sets value for specific property of chart.
        /// This function is used for setting property from JavaScript only
        /// </summary>
        /// <param name="propertyName">Name of the property as String</param>
        /// <param name="value">Property Value as String</param>
        [System.Windows.Browser.ScriptableMember()]
        public void SetPropertyFromJs(String propertyName, String value)
        {
            JsHelper.SetProperty(this, propertyName, value);
        }

#endif

        #endregion

        #region Public Properties

        /// <summary>
        /// Name of the object
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new String Name
        {
            get
            {
                return (String)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }

        /// <summary>
        /// ToolTipText Property
        /// </summary>
        public virtual String ToolTipText
        {
            get
            {
                return (String)GetValue(ToolTipTextProperty);
            }
            set
            {
                SetValue(ToolTipTextProperty, value);
            }
        }

        /// <summary>
        /// Identifies the Visifire.Charts.VisifireElement.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.VisifireElement.ToolTipText dependency property.
        /// </returns>
        public static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(VisifireElement),
            new PropertyMetadata(ToolTipTextPropertyChanged));

        /// <summary>
        /// ToolTipText property changed Event handler
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        internal static void ToolTipTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VisifireElement).OnToolTipTextPropertyChanged((String)e.NewValue);
        }

        /// <summary>
        /// OnToolTipTextPropertyChanged call back virtual function
        /// </summary>
        /// <param name="newValue"></param>
        internal virtual void OnToolTipTextPropertyChanged(String newValue)
        {

        }

        /// <summary>
        /// TextParser is used to pars text
        /// </summary>
        /// <param name="unParsed">String unParsed</param>
        /// <returns>parsed as string</returns>
        public virtual string TextParser(String unParsed)
        {
            return unParsed;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event handler for the MouseLeftButtonDown event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event MouseButtonEventHandler MouseLeftButtonDown
        {
            remove
            {
                _onMouseLeftButtonDown -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseLeftButtonDown += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseLeftButtonUp event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event MouseButtonEventHandler MouseLeftButtonUp
        {
            remove
            {
                _onMouseLeftButtonUp -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseLeftButtonUp += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }


        /// <summary>
        /// Event handler for the MouseEnter event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<MouseEventArgs> MouseEnter
        {
            remove
            {
                _onMouseEnter -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseEnter += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseLeave event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<MouseEventArgs> MouseLeave
        {
            remove
            {
                _onMouseLeave -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseLeave += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseMove event 
        /// </summary>
#if SL
        [ScriptableMember]
#endif
        public new event EventHandler<MouseEventArgs> MouseMove
        {
            remove
            {
                _onMouseMove -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseMove += value;
                
                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// MouseEnter event handler for href
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseEventArgs</param>
        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as FrameworkElement).Cursor = Cursors.Hand;
        }

        /// <summary>
        /// MouseLeftButtonUp event handler for href
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
#if WPF
            System.Diagnostics.Process.Start("explorer.exe", _tempHref);
#else
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(VisifireControl.GetAbsolutePath(_tempHref)), _tempHrefTarget.ToString());
#endif
        }

        /// <summary>
        /// MouseMove event handler for displaying tooltip
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseEventArgs</param>
        private void VisualObject_MouseLeave(object sender, MouseEventArgs e)
        {
            String text = _element.TextParser(_element.ToolTipText);
            if (_control._toolTip.Text == text)
                _control._toolTip.Hide();
        }

        /// <summary>
        /// MouseEnter event handler for displaying tooltip
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseEventArgs</param>
        private void VisualObject_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;

            UpdateToolTip(sender, e);
        }

        /// <summary>
        /// MouseMove event handler for displaying tooltip
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseEventArgs</param>
        private void VisualObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;
            UpdateToolTip(sender, e);
        }

        /// <summary>
        /// Update TextProperty of the tooltip element from ToolTipTextProperty of VisifireElement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateToolTip(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;

            if (_element.ToolTipText == null)
            {
                return;
            }
            else
            {
                String text = _element.TextParser(_element.ToolTipText);

                if (!String.IsNullOrEmpty(text))
                {
                    _control._toolTip.Text = text;

                    if (_control.ToolTipEnabled)
                        _control._toolTip.Show();

                    (_control as Chart).UpdateToolTipPosition(sender, e);
                }
            }
        }

        #endregion

        #region Private Events

        /// <summary>
        /// Handler for MouseLeftButtonDown event
        /// </summary>
        private event MouseButtonEventHandler _onMouseLeftButtonDown;

        /// <summary>
        /// Handler for MouseLeftButtonUp event
        /// </summary>
        private event MouseButtonEventHandler _onMouseLeftButtonUp; 
        
        /// <summary>
        /// Handler for MouseEnter event
        /// </summary>
        private event EventHandler<MouseEventArgs> _onMouseEnter;

        /// <summary>
        /// Handler for MouseLeave event
        /// </summary>
        private event EventHandler<MouseEventArgs> _onMouseLeave;  
         
        /// <summary>
        /// Handler for MouseMove event
        /// </summary>
        private event EventHandler<MouseEventArgs> _onMouseMove;
        
        #endregion

        #region Internal Methods

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Object">obj with which event is attached</param>
        /// <param name="Visual">FrameworkElement</param>
        internal static void AttachEvents2Visual(VisifireElement obj, FrameworkElement visual)
        {   
            if (visual != null)
                AttachEvents2Visual(obj, obj, visual);
        }

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Object">obj with which event is attached</param>
        /// <param name="Sender">sender will be passed to the event-handler</param>
        /// <param name="Visual">visual object with which event will be attached</param>
        internal static void AttachEvents2Visual(VisifireElement obj, VisifireElement senderElement, FrameworkElement visual)
        {
            if (visual == null)
                return;

            visual.MouseEnter += delegate(object sender, MouseEventArgs e)
            {
                if (obj._onMouseEnter != null)
                    obj._onMouseEnter(senderElement, e);
            };

            visual.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                if (obj._onMouseLeave != null)
                    obj._onMouseLeave(senderElement, e);
            };

            visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                if (obj._onMouseLeftButtonDown != null)
                    obj._onMouseLeftButtonDown(senderElement, e);
            };

            visual.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
            {
                if (obj._onMouseLeftButtonUp != null)
                    obj._onMouseLeftButtonUp(senderElement, e);
            };

            visual.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (obj._onMouseMove != null)
                    obj._onMouseMove(senderElement, e);
            };
        }

        /// <summary>
        /// Attach MouseDownEvent event to a visual
        /// </summary>
        /// <param name="Object">obj with which event is attached</param>
        /// <param name="Sender">sender will be passed as sender while firing event</param>
        /// <param name="Visual">visual object with which event will be attached</param>
        internal static void AttachEvents2Visual4MouseDownEvent(VisifireElement obj, VisifireElement senderElement, FrameworkElement visual)
        {
            if (visual == null)
                return;
#if WPF
            visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                if (obj._onMouseLeftButtonDown != null)
                    obj._onMouseLeftButtonDown(senderElement, e);
            };
#endif
        }

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="elements">FrameworkElements list</param>
        /// <param name="toolTipText">Tooltip text</param>
        public void AttachToolTip(VisifireControl control, ObservableObject element, System.Collections.Generic.List<FrameworkElement> visualElements)
        {
            // Show ToolTip on mouse move over the chart element
            foreach (FrameworkElement visualElement in visualElements)
                AttachToolTip(control, element, visualElement);
        }

        /// <summary>
        /// Attach Href with a framework element
        /// </summary>
        /// <param name="Control">Chart Control</param>
        /// <param name="Element">FrameworkElement</param>
        /// <param name="Href">Href</param>
        /// <param name="HrefTarget">HrefTarget</param>
        internal void AttachHref(VisifireControl control, FrameworkElement element, String href, HrefTargets hrefTarget)
        {
            if (element == null)
                return;

            if (!String.IsNullOrEmpty(href))
            {
                _tempHref = href;
                _tempHrefTarget = hrefTarget;
                element.MouseEnter -= Element_MouseEnter;
                element.MouseEnter += Element_MouseEnter;

                element.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
                element.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
            }
        }

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="element">FrameworkElement</param>
        /// <param name="toolTipText">Tooltip text</param>
        internal void AttachToolTip(VisifireControl control, VisifireElement element, FrameworkElement visualObject)
        {
            if (visualObject == null || control == null || element == null)
                return;

            _control = control;
            _element = element;
            _visualObject = visualObject;

            visualObject.MouseMove += new MouseEventHandler(VisualObject_MouseMove);
            visualObject.MouseEnter += new MouseEventHandler(VisualObject_MouseEnter);

            // Hide ToolTip on mouse leave from the chart element
            visualObject.MouseLeave += new MouseEventHandler(VisualObject_MouseLeave);
        }

        /// <summary>
        /// Detach ToolTip from a visual element
        /// </summary>
        /// <param name="VisualObject"></param>
        internal void DetachToolTip(FrameworkElement visualObject)
        {
            if (visualObject != null)
            {
                visualObject.MouseMove -= new MouseEventHandler(VisualObject_MouseMove);
                visualObject.MouseEnter -= new MouseEventHandler(VisualObject_MouseEnter);

                // Hide ToolTip on mouse leave from the chart element
                visualObject.MouseLeave -= new MouseEventHandler(VisualObject_MouseLeave);
            }
        }

        #endregion

        #region Internal Events

        /// <summary>
        /// EventChanged event is firef if any event is attached
        /// </summary>
        internal event EventHandler EventChanged;

        #endregion

        #region Data

        /// <summary>
        /// Visifire Control reference
        /// </summary>
        private VisifireControl _control;

        /// <summary>
        /// Visifire element reference
        /// </summary>
        private VisifireElement _element;

        /// <summary>
        /// Visual object reference
        /// </summary>
        private FrameworkElement _visualObject;

        /// <summary>
        /// Temp Href 
        /// </summary>
        internal String _tempHref;

        /// <summary>
        /// Temp HrefTarget
        /// </summary>
        internal HrefTargets _tempHrefTarget;
                
        #endregion
    }
}