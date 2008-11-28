﻿using System;
using System.Windows.Input;
using System.Windows;

#if SL
using System.Windows.Browser;
#endif
namespace Visifire.Commons
{
    public abstract class VisifireElement: System.Windows.Controls.ContentControl
    {
        public VisifireElement()
        {

        }

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

                if(EventChanged != null)
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

        /// <summary>
        /// Property change event
        /// </summary>
        internal event EventHandler EventChanged;

        #endregion


        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Object">Object with which event is attached</param>
        /// <param name="Sender">Sender will be passed as sender whiling firing event</param>
        /// <param name="Visual">Visual object with which event will be attached</param>
        public static void AttachEvents2Visual(VisifireElement Object, VisifireElement Sender, FrameworkElement Visual)
        {
            if (Visual == null)
                return;

            if (Object._onMouseEnter != null)
                Visual.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Object._onMouseEnter(Sender, e);
                };

            if (Object._onMouseLeave != null)
            {
                Visual.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    Object._onMouseLeave(Sender, e);
                };
            }

            if (Object._onMouseLeftButtonDown != null)
                Visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    Object._onMouseLeftButtonDown(Sender, e);
                };

            if (Object._onMouseLeftButtonUp != null)
                Visual.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {   
                    Object._onMouseLeftButtonUp(Sender, e);
                };

            if (Object._onMouseMove != null)
                Visual.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    Object._onMouseMove(Sender, e);
                };
        }
        
        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Visual"></param>
        public static void AttachEvents2Visual(VisifireElement Object, FrameworkElement Visual)
        {
            if (Visual != null)
                AttachEvents2Visual(Object, Object, Visual);
        }
        
        internal event MouseButtonEventHandler _onMouseLeftButtonDown;       // Handler for MouseLeftButtonDown event
        internal event MouseButtonEventHandler _onMouseLeftButtonUp;         // Handler for MouseLeftButtonUp event
        internal event EventHandler<MouseEventArgs> _onMouseEnter;           // Handler for MouseEnter event
        internal event EventHandler<MouseEventArgs> _onMouseLeave;           // Handler for MouseLeave event
        internal event EventHandler<MouseEventArgs> _onMouseMove;            // Handler for MouseMove event
    }
}