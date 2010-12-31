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
    #if SL && !WP
        using System.Windows.Browser;
    #endif
#endif

using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Visifire.Commons
{
    /// <summary>
    /// Base class for all Visifire elements including chart
    /// </summary>
    public abstract class VisifireElement : System.Windows.Controls.Control
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
        /// 
#if !WP
        [System.Windows.Browser.ScriptableMember()]
#endif
        public void SetPropertyFromJs(String propertyName, String value)
        {
#if !WP
            JsHelper.SetProperty(this, propertyName, value);
#endif
        }

#endif

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="element">Object reference</param>
        /// <param name="visualElements">FrameworkElements list</param>
        public void AttachToolTip(VisifireControl control, ObservableObject element, System.Collections.Generic.List<FrameworkElement> visualElements)
        {
            // Show ToolTip on mouse move over the chart element
            foreach (FrameworkElement visualElement in visualElements)
                AttachToolTip(control, element, visualElement);
        }
        
        #endregion

        #region Public Properties

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
            new PropertyMetadata("", ToolTipTextPropertyChanged));

        /// <summary>
        /// TextParser is used to parse text
        /// </summary>
        /// <param name="unParsed">String unParsed</param>
        /// <returns>parsed as string</returns>
        public virtual string TextParser(String unParsed)
        {
            return unParsed;
        }

        /// <summary>
        /// TextParser is used to parse ToolTipText
        /// </summary>
        /// <param name="unParsed">String unParsed</param>
        /// <returns>parsed as string</returns>
        public virtual string ParseToolTipText(String unParsed)
        {
            return TextParser(unParsed);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event handler for the MouseLeftButtonDown event 
        /// </summary>
#if SL &&!WP
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
#if SL &&!WP
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

#if WPF
        /// <summary>
        /// Event handler for the MouseRightButtonDown event 
        /// </summary>
        public new event MouseButtonEventHandler MouseRightButtonDown
        {
            remove
            {
                _onMouseRightButtonDown -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseRightButtonDown += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }

        /// <summary>
        /// Event handler for the MouseRightButtonUp event 
        /// </summary>
        public new event MouseButtonEventHandler MouseRightButtonUp
        {
            remove
            {
                _onMouseRightButtonUp -= value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
            add
            {
                _onMouseRightButtonUp += value;

                if (EventChanged != null)
                    EventChanged(this, null);
            }
        }
#endif

        /// <summary>
        /// Event handler for the MouseEnter event 
        /// </summary>
#if SL &&!WP
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
#if SL &&!WP
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
#if SL &&!WP
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

        /// <summary>
        /// Update TextProperty of the tooltip element from ToolTipTextProperty of VisifireElement
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseEventArgs</param>
        protected void UpdateToolTip(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;

            if (_element.ToolTipText == null)
            {
                return;
            }
            else
            {
                String text = _element.ParseToolTipText(_element.ToolTipText);

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
#elif !WP
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
            String text = _element.ParseToolTipText(_element.ToolTipText);
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

            _control._toolTip.CallOutVisiblity = Visibility.Collapsed;

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

            _control._toolTip.CallOutVisiblity = Visibility.Collapsed;

            UpdateToolTip(sender, e);
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

#if WPF
        /// <summary>
        /// Handler for MouseRightButtonDown event
        /// </summary>
        private event MouseButtonEventHandler _onMouseRightButtonDown;

        /// <summary>
        /// Handler for MouseRightButtonUp event
        /// </summary>
        private event MouseButtonEventHandler _onMouseRightButtonUp;
#endif

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
        /// If you want the event-handler to be executed before all other attached event-handler
        /// with this event then you can attach a delegate using AttachEventWithHighPriority 
        /// method. 
        /// Note: Currently it supports MouseLeftButtonUp event only.
        /// </summary>
        /// <param name="dlg"></param>
        /// <param name="eventType"></param>
        internal void AttachEventWithHighPriority(String eventType, Delegate dlg)
        {
            if (_onMouseLeftButtonUp != null)
            {
                // Get Invocation List
                Delegate[] delegates = _onMouseLeftButtonUp.GetInvocationList();

                // Detach delegates
                foreach (Delegate deleg in delegates)
                    MouseLeftButtonUp -= (MouseButtonEventHandler)deleg;

                // Attach delegate
                MouseLeftButtonUp += (MouseButtonEventHandler)dlg;

                // Attach all detached delegates
                foreach (Delegate deleg in delegates)
                    MouseLeftButtonUp += (MouseButtonEventHandler)deleg;
            }
            else
            {
                MouseLeftButtonUp += (MouseButtonEventHandler)dlg;
            }
        }

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
        /// Attach events to a visual
        /// </summary>
        /// <param name="obj">Object with which event is attached</param>
        /// <param name="visual">FrameworkElement</param>
        internal static void AttachEvents2Visual(VisifireElement obj, FrameworkElement visual)
        {   
            if (visual != null)
                AttachEvents2Visual(obj, obj, visual);
        }

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="obj">Object with which event is attached</param>
        /// <param name="senderElement">sender will be passed to the event-handler</param>
        /// <param name="visual">visual object with which event will be attached</param>
        internal static void AttachEvents2Visual(VisifireElement chartElement, VisifireElement senderElement, FrameworkElement visual)
        {
            if (visual == null)
                return;
            object eventHandler = null;

            // Attach event for MouseEnter
            eventHandler = chartElement.GetMouseEnterEventHandler();

            if (eventHandler != null)
                visual.MouseEnter += delegate(object sender, MouseEventArgs e)
                {   
                    chartElement.FireMouseEnterEvent(senderElement, e);
                };

            // Attach event for MouseLeave
            eventHandler = chartElement.GetMouseLeaveEventHandler();

            if (eventHandler != null)
                visual.MouseLeave += delegate(object sender, MouseEventArgs e)
                {   
                    chartElement.FireMouseLeaveEvent(senderElement, e);
                };

            // Attach event for MouseLeftButtonDown
            eventHandler = chartElement.GetMouseLeftButtonDownEventHandler();

            if (eventHandler != null)
                visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    chartElement.FireMouseLeftButtonDownEvent(senderElement, e);
                };

            // Attach event for MouseLeftButtonUp
            eventHandler = chartElement.GetMouseLeftButtonUpEventHandler();
            
            if (eventHandler != null)
                visual.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    chartElement.FireMouseLeftButtonUpEvent(senderElement, e);
                };

            // Attach event for MouseMove
            eventHandler = chartElement.GetMouseMoveEventHandler();

            if (eventHandler != null)
                visual.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    chartElement.FireMouseMoveEvent(senderElement, e);
                };

            #region RightMouseButtonEvents4WPF
#if WPF     
            // Attach event for MouseLeftButtonDown
            eventHandler = chartElement.GetMouseRightButtonDownEventHandler();

            if (eventHandler != null)
                visual.MouseRightButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    chartElement.FireMouseRightButtonDownEvent(senderElement, e);
                };

            // Attach event for MouseRightButtonUp
            eventHandler = chartElement.GetMouseRightButtonUpEventHandler();

            if (eventHandler != null)
                visual.MouseRightButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    chartElement.FireMouseRightButtonUpEvent(senderElement, e);
                };
#endif
            #endregion
        }

#if WPF
        internal virtual void FireMouseRightButtonDownEvent(object sender, object e)
        {
            if(_onMouseRightButtonDown != null)
                _onMouseRightButtonDown(sender, e as MouseButtonEventArgs);
        }

        internal virtual void FireMouseRightButtonUpEvent(object sender, object e)
        {
            if(_onMouseRightButtonUp != null)
                _onMouseRightButtonUp(sender, e as MouseButtonEventArgs);
        }
#endif

        internal virtual void FireMouseLeftButtonDownEvent(object sender, object e)
        {
            if (_onMouseLeftButtonDown != null)
                _onMouseLeftButtonDown(sender, e as MouseButtonEventArgs);
        }

        internal virtual void FireMouseLeftButtonUpEvent(object sender, object e)
        {
            if (_onMouseLeftButtonUp != null)
                _onMouseLeftButtonUp(sender, e as MouseButtonEventArgs);
        }

        internal virtual void FireMouseMoveEvent(object sender, object e)
        {
            if (_onMouseMove != null)
                _onMouseMove(sender, e as MouseEventArgs);
        }

        internal virtual void FireMouseEnterEvent(object sender, object e)
        {
            if (_onMouseEnter != null)
                _onMouseEnter(sender, e as MouseEventArgs);
        }

        internal virtual void FireMouseLeaveEvent(object sender, object e)
        {
            if (_onMouseLeave != null)
                _onMouseLeave(sender, e as MouseEventArgs);
        }

        internal virtual object GetMouseLeftButtonDownEventHandler()
        {
            return this._onMouseLeftButtonDown;
        }

        internal virtual object GetMouseLeftButtonUpEventHandler()
        {
            return this._onMouseLeftButtonUp;
        }
        
        internal virtual object GetMouseMoveEventHandler()
        {
            return this._onMouseMove;
        }

        internal virtual object GetMouseEnterEventHandler()
        {
            return this._onMouseEnter;
        }

        internal virtual object GetMouseLeaveEventHandler()
        {
            return this._onMouseLeave;
        }
        
        /// <summary>
        /// Attach events to a Area visual
        /// </summary>
        /// <param name="obj">Object with which event is attached</param>
        /// <param name="senderElement">sender will be passed to the event-handler</param>
        /// <param name="visual">visual object with which event will be attached</param>
        internal static void AttachEvents2AreaVisual(VisifireElement obj, VisifireElement senderElement, FrameworkElement visual)
        {
            if (visual == null)
                return;

            if (obj._onMouseEnter != null)
                visual.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    if (obj._onMouseEnter != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseEnter != null)
                                dp._onMouseEnter(dp, e);
                        }
                        else
                            obj._onMouseEnter(dp, e);
                    }
                };

            if (obj._onMouseLeave != null)
                visual.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    if (obj._onMouseLeave != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseLeave != null)
                                dp._onMouseLeave(dp, e);
                        }
                        else
                            obj._onMouseLeave(dp, e);
                    }
                };

            object eventHandler;
            eventHandler = obj._onMouseLeftButtonDown;

            if (eventHandler != null)
                visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (obj._onMouseLeftButtonDown != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseButtonEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseButtonEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseLeftButtonDown != null)
                                dp._onMouseLeftButtonDown(dp, e);
                        }
                        else
                            obj._onMouseLeftButtonDown(dp, e);
                    }
                };

            eventHandler = obj._onMouseLeftButtonUp;

            if (eventHandler != null)
                visual.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (obj._onMouseLeftButtonUp != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseButtonEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseButtonEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseLeftButtonUp != null)
                                dp._onMouseLeftButtonUp(dp, e);
                        }
                        else
                            obj._onMouseLeftButtonUp(dp, e);
                    }
                };

            eventHandler = obj._onMouseMove;

            if (eventHandler != null)
                visual.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    if (obj._onMouseMove != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseMove != null)
                                dp._onMouseMove(dp, e);
                        }
                        else
                            obj._onMouseMove(dp, e);
                    }
                };


            #region RightMouseButtonEvents4WPF
#if WPF
            object eventHandler4RightMouseButton;
            eventHandler4RightMouseButton = obj._onMouseRightButtonDown;

            if (eventHandler4RightMouseButton != null)
                visual.MouseRightButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (obj._onMouseRightButtonDown != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseButtonEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseButtonEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseRightButtonDown != null)
                                dp._onMouseRightButtonDown(dp, e);
                        }
                        else
                            obj._onMouseRightButtonDown(dp, e);
                    }
                };

            eventHandler4RightMouseButton = obj._onMouseRightButtonUp;

            if (eventHandler4RightMouseButton != null)
                visual.MouseRightButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (obj._onMouseRightButtonUp != null)
                    {
                        DataPoint dp = null;
                        if (obj.GetType().Equals(typeof(DataSeries)))
                            dp = (obj as DataSeries).GetNearestDataPointOnMouseButtonEvent(e);
                        else
                            dp = (obj as DataPoint).Parent.GetNearestDataPointOnMouseButtonEvent(e);

                        if (obj.GetType().Equals(typeof(DataPoint)))
                        {
                            if ((dp as VisifireElement)._onMouseRightButtonUp != null)
                                dp._onMouseRightButtonUp(dp, e);
                        }
                        else
                            obj._onMouseRightButtonUp(dp, e);
                    }
                };
#endif
            #endregion
        }

        /// <summary>
        /// Attach MouseDownEvent event to a visual
        /// </summary>
        /// <param name="obj">obj with which event is attached</param>
        /// <param name="senderElement">sender will be passed as sender while firing event</param>
        /// <param name="visual">visual object with which event will be attached</param>
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
        /// Attach Href with a framework elements
        /// </summary>
        /// <param name="control">Chart Control</param>
        /// <param name="visualElements">List of FrameworkElement</param>
        /// <param name="href">Href</param>
        /// <param name="hrefTarget">HrefTarget</param>
        internal void AttachHref(VisifireControl control, System.Collections.Generic.List<FrameworkElement> visualElements, String href, HrefTargets hrefTarget)
        {
            // Attach Href with a framework elements
            foreach (FrameworkElement visualElement in visualElements)
                AttachHref(control, visualElement, href, hrefTarget);
        }

        /// <summary>
        /// Attach Href with a framework element
        /// </summary>
        /// <param name="control">Chart Control</param>
        /// <param name="element">FrameworkElement</param>
        /// <param name="href">Href</param>
        /// <param name="hrefTarget">HrefTarget</param>
        internal void AttachHref(VisifireControl control, FrameworkElement visualElement, String href, HrefTargets hrefTarget)
        {
            if (visualElement == null)
                return;

            if (!String.IsNullOrEmpty(href))
            {
                _tempHref = href;
                _tempHrefTarget = hrefTarget;
                visualElement.MouseEnter -= Element_MouseEnter;
                visualElement.MouseEnter += Element_MouseEnter;

                visualElement.MouseLeftButtonUp -= Element_MouseLeftButtonUp;
                visualElement.MouseLeftButtonUp += new MouseButtonEventHandler(Element_MouseLeftButtonUp);
            }
        }

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="element">FrameworkElement</param>
        /// <param name="visualObject">FrameworlElement</param>
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
        /// <param name="VisualObject">FrameworkElement</param>
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

#if WPF
        internal virtual object GetMouseRightButtonDownEventHandler()
        {
            return this._onMouseRightButtonDown;
        }

        internal virtual object GetMouseRightButtonUpEventHandler()
        {
            return this._onMouseRightButtonUp;
        }
#endif

        internal virtual void ClearInstanceRefs()
        {
            //_onMouseLeftButtonDown = null;
            //_onMouseLeftButtonUp = null;
            //_onMouseMove = null;
            //_onMouseEnter = null;
            //_onMouseLeave = null;
        }

        #endregion

        #region Internal Events

        /// <summary>
        /// EventChanged event is fired if any event is attached or detached
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