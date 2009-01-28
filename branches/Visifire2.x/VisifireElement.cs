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

    public abstract class VisifireElement: System.Windows.Controls.Control
    {
        public VisifireElement()
        {

        }
        
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

        internal static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(VisifireElement),
            new PropertyMetadata(ToolTipTextPropertyChanged));

        internal static void ToolTipTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VisifireElement).OnToolTipTextPropertyChanged((String)e.NewValue);
        }

        internal virtual void OnToolTipTextPropertyChanged(String NewValue)
        {

        }

        public virtual string TextParser(String unParsed)
        {
            return unParsed;
        }
        
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
        
        private VisifireControl _control;
        private VisifireElement _element;
        private FrameworkElement _visualObject;
        
        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="element">FrameworkElement</param>
        /// <param name="toolTipText">Tooltip text</param>
        internal void AttachToolTip(VisifireControl Control, VisifireElement Element, FrameworkElement VisualObject)
        {
            if (VisualObject == null || Control == null || Element == null)
                return;

            _control = Control;
            _element = Element;
            _visualObject = VisualObject;

            VisualObject.MouseMove += new MouseEventHandler(VisualObject_MouseMove);
            VisualObject.MouseEnter += new MouseEventHandler(VisualObject_MouseEnter);

            // Hide ToolTip on mouse out from the chart element
            VisualObject.MouseLeave += new MouseEventHandler(VisualObject_MouseLeave);
        }

        internal void DetachToolTip(FrameworkElement VisualObject)
        {
            if (VisualObject != null)
            {
                VisualObject.MouseMove -= new MouseEventHandler(VisualObject_MouseMove);
                VisualObject.MouseEnter -= new MouseEventHandler(VisualObject_MouseEnter);

                // Hide ToolTip on mouse out from the chart element
                VisualObject.MouseLeave -= new MouseEventHandler(VisualObject_MouseLeave);
            }
        }

        void VisualObject_MouseLeave(object sender, MouseEventArgs e)
        {
            String text = _element.TextParser(_element.ToolTipText);
            // Control._toolTip.Text = "";
            if (_control._toolTip.Text == text)
                _control._toolTip.Hide();
        }

        void VisualObject_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;
            UpdateToolTip(sender, e);
        }

        void VisualObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;
            UpdateToolTip(sender, e);
        }

        private void UpdateToolTip(object sender, MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("_element----" + _element.GetType().ToString());

            if (!String.IsNullOrEmpty(_control.ToolTipText) && _control != _element)
                return;

            if (_element.ToolTipText == null)
                {
                    // Control._toolTip.Hide();
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

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Visual"></param>
        public static void AttachEvents2Visual(VisifireElement Object, FrameworkElement Visual)
        {
            if (Visual != null)
                AttachEvents2Visual(Object, Object, Visual);
        }

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

            Visual.MouseEnter += delegate(object sender, MouseEventArgs e)
            {
                if (Object._onMouseEnter != null)
                    Object._onMouseEnter(Sender, e);
            };

            Visual.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                if (Object._onMouseLeave != null)
                    Object._onMouseLeave(Sender, e);
            };

            Visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                if (Object._onMouseLeftButtonDown != null)
                    Object._onMouseLeftButtonDown(Sender, e);
            };
            
            Visual.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
            {
                if (Object._onMouseLeftButtonUp != null)
                    Object._onMouseLeftButtonUp(Sender, e);
            };
            
            Visual.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (Object._onMouseMove != null)
                    Object._onMouseMove(Sender, e);
            };
        }

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Object">Object with which event is attached</param>
        /// <param name="Sender">Sender will be passed as sender whiling firing event</param>
        /// <param name="Visual">Visual object with which event will be attached</param>
        public static void AttachEvents2Visual4MouseDownEvent(VisifireElement Object, VisifireElement Sender, FrameworkElement Visual)
        {
            if (Visual == null)
                return;

#if WPF
            Visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                if (Object._onMouseLeftButtonDown != null)
                    Object._onMouseLeftButtonDown(Sender, e);
            };
#endif
        }

        

#if SL
        /// <summary>
        /// Sets value for specific property of chart
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

        internal event MouseButtonEventHandler _onMouseLeftButtonDown;       // Handler for MouseLeftButtonDown event
        internal event MouseButtonEventHandler _onMouseLeftButtonUp;         // Handler for MouseLeftButtonUp event
        internal event EventHandler<MouseEventArgs> _onMouseEnter;           // Handler for MouseEnter event
        internal event EventHandler<MouseEventArgs> _onMouseLeave;           // Handler for MouseLeave event
        internal event EventHandler<MouseEventArgs> _onMouseMove;            // Handler for MouseMove event
    }
}