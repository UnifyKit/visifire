using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Markup;
using Visifire.Commons;
using Visifire.Charts;
using System.Linq;
using System.IO;
using System.Xml;
using System.Globalization;
#if SL
using System.Windows.Browser;
#endif

namespace Visifire.Commons
{
    /// <summary>
    /// ObservableObject Implements INotifyPropertyChanged Interface
    /// </summary>
    public abstract class ObservableObject : Control, INotifyPropertyChanged
    {
        public ObservableObject()
            : base()
        {
            IsNotificationEnable = true;
        }

        #region Public Methods

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="element">FrameworkElement</param>
        /// <param name="toolTipText">Tooltip text</param>
        public static void AttachToolTip(VisifireControl Control, FrameworkElement Element, String ToolTipText)
        {
            if (Element == null)
                return;

            if (!String.IsNullOrEmpty(ToolTipText))
            {
                // Show ToolTip on mouse move over the chart element
                Element.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Control._toolTipTextBlock.Text = ToolTipText;

                    if (Control.ToolTipEnabled)
                        Control._toolTip.Visibility = Visibility.Visible;
                };

                // Hide ToolTip on mouse out from the chart element
                Element.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    Control._toolTip.Visibility = Visibility.Collapsed;
                };
            }
        }

        /// <summary>
        /// Clone a object to a new Object
        /// </summary>
        /// <typeparam name="T">System.Type</typeparam>
        /// <param name="source">Source Type</param>
        /// <returns>Specified Type</returns>
        public static T Clone<T>(T source)
        {
            T cloned = (T)Activator.CreateInstance(source.GetType());

            foreach (System.Reflection.PropertyInfo curPropInfo in source.GetType().GetProperties())
            {
                if (curPropInfo.GetGetMethod() != null && (curPropInfo.GetSetMethod() != null))
                {
                    // Handle Non-indexer properties
                    if (curPropInfo.Name != "Item")
                    {
                        // get property from source
                        object getValue = curPropInfo.GetGetMethod().Invoke(source, new object[] { });

                        // clone if needed
                        if (getValue != null && getValue is DependencyObject)
                            getValue = Clone((DependencyObject)getValue);

                        // set property on cloned
                        curPropInfo.GetSetMethod().Invoke(cloned, new object[] { getValue });

                    }
                    else  // handle indexer
                    {
                        // get count for indexer
                        int numberofItemInColleciton = (int)curPropInfo.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(source, new object[] { });

                        // run on indexer
                        for (int i = 0; i < numberofItemInColleciton; i++)
                        {
                            // get item through Indexer
                            object getValue = curPropInfo.GetGetMethod().Invoke(source, new object[] { i });

                            // clone if needed
                            if (getValue != null && getValue is DependencyObject)
                                getValue = Clone((DependencyObject)getValue);

                            // add item to collection
                            curPropInfo.ReflectedType.GetMethod("Add").Invoke(cloned, new object[] { getValue });
                        }

                    }
                }
            }

          
            return cloned;
        }

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="elements">FrameworkElements list</param>
        /// <param name="toolTipText">Tooltip text</param>
        public static void AttachToolTip(VisifireControl Control, List<FrameworkElement> Elements, String ToolTipText)
        {
            if (!String.IsNullOrEmpty(ToolTipText))
            {
                // Show ToolTip on mouse move over the chart element
                foreach (FrameworkElement element in Elements)
                {
                    if (element != null)
                    {
                        element.MouseEnter += delegate(object sender, MouseEventArgs e)
                        {
                            Control._toolTipTextBlock.Text = ToolTipText;

                            if(Control.ToolTipEnabled)
                                Control._toolTip.Visibility = Visibility.Visible;
                        };

                        // Hide ToolTip on mouse out from the chart element
                        element.MouseLeave += delegate(object sender, MouseEventArgs e)
                        {
                            Control._toolTip.Visibility = Visibility.Collapsed;
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Attach Href with a framework element
        /// </summary>
        /// <param name="Control">Chart Control</param>
        /// <param name="Element">FrameworkElement</param>
        /// <param name="Href">Href</param>
        /// <param name="HrefTarget">HrefTarget</param>
        public static void AttachHref(VisifireControl Control, FrameworkElement Element, String Href, HrefTargets HrefTarget)
        {
            if (Element == null)
                return;

            if (!String.IsNullOrEmpty(Href))
            {
                Element.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Element.Cursor = Cursors.Hand;
                };

                Element.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
#if WPF
                    System.Diagnostics.Process.Start("explorer.exe", Href);
#else
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(VisifireControl.GetAbsolutePath(Href)), HrefTarget.ToString());
#endif
                };
            }
        }

        /// <summary>
        /// Detaches event handler
        /// </summary>
        /// <param name="Handler"></param>
        public void DetachEvents(EventHandler Handler)
        {
            if (Handler != null)
            {
                Delegate[] invList = Handler.GetInvocationList();

                foreach (EventHandler handler in invList)
                {
                    Handler -= handler;
                }
            }
        }

        /// <summary>
        /// Apply specific style from theme
        /// </summary>
        /// <param name="Control">Control</param>
        /// <param name="KeyName">Style key name</param>
        public void ApplyStyleFromTheme(VisifireControl Control, String KeyName)
        {
            Chart chart = Control as Chart;

            if (Style == null)
                if (chart.StyleDictionary != null)
                {
                    Style myStyle = chart.StyleDictionary[KeyName] as Style;

                    if (myStyle != null)
                        Style = myStyle;
                }
        }

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Object">Object with which event is attached</param>
        /// <param name="Sender">Sender will be passed as sender whiling firing event</param>
        /// <param name="Visual">Visual object with which event will be attached</param>
        public static void AttachEvents2Visual(ObservableObject Object, ObservableObject Sender, FrameworkElement Visual)
        {
            if (Visual == null)
                return;

            if (Object._onMouseEnter != null)
                Visual.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    if (Sender._target != null)
                        Object._onMouseEnter(Sender._target, e);
                    else
                        Object._onMouseEnter(Sender, e);
                };

            if (Object._onMouseLeave != null)
                Visual.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    if (Sender._target != null)
                        Object._onMouseLeave(Sender._target, e);
                    else
                        Object._onMouseLeave(Sender, e);
                };

            if (Object._onMouseLeftButtonDown != null)
                Visual.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (Sender._target != null)
                        Object._onMouseLeftButtonDown(Sender._target, e);
                    else
                        Object._onMouseLeftButtonDown(Sender, e);
                };
 
            if (Object._onMouseLeftButtonUp != null)
                Visual.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    if (Sender._target != null)
                        Object._onMouseLeftButtonUp(Sender._target, e);
                    else
                        Object._onMouseLeftButtonUp(Sender, e);
                };

            if (Object._onMouseMove != null)
                Visual.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    if (Sender._target != null)
                        Object._onMouseMove(Sender._target, e);
                    else
                        Object._onMouseMove(Sender, e);
                };
        }

        /// <summary>
        /// Attach events to a visual
        /// </summary>
        /// <param name="Visual"></param>
        public static void AttachEvents2Visual(ObservableObject Object, FrameworkElement Visual)
        {
            if (Visual != null)
                AttachEvents2Visual(Object, Object, Visual);
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
            if (propertyName == "Canvas.ZIndex")
            {
                SetValue(Canvas.ZIndexProperty, Convert.ToInt32(value, CultureInfo.InvariantCulture));
                FirePropertyChanged("ZIndex");
                return;
            }

            Object element = this;

            Chart chart = Chart as Chart;
            //chart.RENDER_LOCK = false;

            System.Reflection.PropertyInfo[] propArray = element.GetType().GetProperties();
            var obj = from property in propArray
                      where (property.Name == propertyName)
                      select property;

            try
            {
                if (obj.Count<System.Reflection.PropertyInfo>() == 0)
                {
                    throw new Exception("Property not found.");
                }


                System.Reflection.PropertyInfo property = obj.First<System.Reflection.PropertyInfo>();

                if (property.PropertyType.Name == "Brush")
                    property.SetValue(element, ((Brush)System.Windows.Markup.XamlReader.Load(value)), null);
                else if (property.PropertyType.Equals(typeof(FontFamily)))
                {
                    FontFamily ff = new FontFamily(value);
                    property.SetValue(element, ff, null);
                }
                else if (property.PropertyType.Equals(typeof(FontStyle)))
                {
                    Visifire.Commons.Converters.FontStyleConverter fsc = new Visifire.Commons.Converters.FontStyleConverter();
                    property.SetValue(element, fsc.ConvertFrom(null, CultureInfo.InvariantCulture, value), null);
                }
                else if (property.PropertyType.Equals(typeof(FontWeight)))
                {
                    Visifire.Commons.Converters.FontWeightConverter fwc = new Visifire.Commons.Converters.FontWeightConverter();
                    property.SetValue(element, fwc.ConvertFrom(null, CultureInfo.InvariantCulture, value), null);
                }
                else if (property.PropertyType.Equals(typeof(Nullable<Boolean>)))
                    property.SetValue(element, new Nullable<Boolean>(Convert.ToBoolean(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.Equals(typeof(Nullable<Double>)))
                    property.SetValue(element, new Nullable<Double>(Convert.ToDouble(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.BaseType.Equals(typeof(Enum)))
                    property.SetValue(element, Enum.Parse(property.PropertyType, value, true), null);
                else if (property.PropertyType.Equals(typeof(Thickness)))
                    property.SetValue(element, new Thickness(Convert.ToDouble(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.Equals(typeof(CornerRadius)))
                    property.SetValue(element, new CornerRadius(Convert.ToDouble(value, CultureInfo.InvariantCulture)), null);
                else
                    property.SetValue(element, Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture), null);

                if (chart.LoggerWindow != null)
                    chart.LoggerWindow.Visibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                if (chart.LoggerWindow == null)
                {
                    // If Log viewer is not present create it.
                    chart.CreateLogViewer();

                    if (chart.LogLevel == 1)
                        chart.LoggerWindow.Visibility = Visibility.Visible;
                    else
                    {
                        chart.Visibility = Visibility.Collapsed;
                    }
                }

                chart.LoggerWindow.Log("\n\nError Message:\n");

                // Log InnerException
                if (e.InnerException != null)
                {
                    chart.LoggerWindow.LogLine(e.InnerException.Message);
                }

                String s = String.Format(@"Unable to update {0} property. ({1})", propertyName, e.Message);

                chart.LoggerWindow.LogLine(s);

                // Exception is thrown to JavaScript
                throw new Exception(chart.LoggerWindow.logger.Text);
            }
        }
#endif


        #endregion

        #region Public Properties

        /// <summary>
        /// Index property of a ObservableObject
        /// </summary>
        public Int32 Index
        {
            get;
            set;
        }

        /// <summary>
        /// ToolTip text
        /// </summary>
        public virtual String ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set
            {
                _toolTipText = value;
                FirePropertyChanged("ToolTipText");
            }
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
                _onMouseLeftButtonDown += value;
            }
            add
            {
                _onMouseLeftButtonDown += value;
                FirePropertyChanged("OnMouseLeftButtonDown");
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
            }
            add
            {
                _onMouseLeftButtonUp += value;
                FirePropertyChanged("OnMouseLeftButtonUp");
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
            }
            add
            {
                _onMouseEnter += value;
                FirePropertyChanged("OnMouseEnter");
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
            }
            add
            {
                _onMouseLeave += value;
                FirePropertyChanged("OnMouseLeave");
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
            }
            add
            {
                _onMouseMove += value;
                FirePropertyChanged("OnMouseMove");
            }
        }

        /// <summary>
        /// Property change event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        internal MouseButtonEventHandler InternalMouseLeftButtonUp
        {
            get
            {
                return _onMouseLeftButtonUp;
            }
            set
            {
                _onMouseLeftButtonUp = value;
            }
        }

        internal MouseButtonEventHandler InternalMouseLeftButtonDown
        {
            get 
            { 
                return _onMouseLeftButtonDown; 
            }
            set 
            { 
                _onMouseLeftButtonDown = value; 
            }
        }
        
        internal EventHandler<MouseEventArgs> InternalMouseEnter
        {
            get
            {
                return _onMouseEnter;
            }
            set
            {
                _onMouseEnter = value;
            }
        }

        internal EventHandler<MouseEventArgs> InternalMouseLeave
        {
            get
            {
                return _onMouseLeave;
            }
            set
            {
                _onMouseLeave = value;
            }
        }

        /// <summary>
        /// Event handler for the MouseMove event 
        /// </summary>
        internal EventHandler<MouseEventArgs> InternalMouseMove
        {
            get
            {
                return _onMouseMove;
            }
            set
            {
                _onMouseMove = value;
            }
        }

        #region Protected Methods

        /// <summary>
        /// Check whether the Property value is changed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Property Name</param>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        /// <returns></returns>
        protected bool CheckPropertyChanged<T>(string propertyName, ref T oldValue, ref T newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if ((oldValue == null && newValue != null) || !oldValue.Equals((T)newValue))
            {
                oldValue = newValue;

                FirePropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Check whether the Property value is changed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Property Name</param>
        /// <param name="oldValue">Old property value</param>
        /// <param name="newValue">New property value</param>
        /// <returns></returns>
        protected bool CheckPropertyChanged<T>(string propertyName, T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if ((oldValue == null && newValue != null) || !oldValue.Equals((T)newValue))
            {
                oldValue = newValue;

                FirePropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Fire property change event
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null && IsNotificationEnable)
            {
#if SL
                if (IsInDesignMode)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                    if (Chart != null)
                    {
                        if ((Chart as Chart).IsTemplateApplied)
                        {
                            if (Application.Current != null && Application.Current.RootVisual != null && Application.Current.RootVisual.Dispatcher != null)
                            {
                                System.Windows.Threading.Dispatcher currentDispatcher = Application.Current.RootVisual.Dispatcher;
                                if (currentDispatcher.CheckAccess())
                                    (Chart as Chart).Render();
                                else
                                    currentDispatcher.BeginInvoke(new Action<String>(FirePropertyChanged), propertyName);
                            }
                            else // if we did not get the Dispatcher throw an exception
                            {
                                throw new InvalidOperationException("This object must be initialized after that the RootVisual has been loaded");
                            }
                        }
                    }
#else           
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
#endif
            }
        }

        #endregion

        #region Internal Properties

#if WPF
        internal Boolean IsInDesignMode
        {
            get
            {
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            }
        }
#else
        internal Boolean IsInDesignMode
        {
            get
            {
                return !System.Windows.Browser.HtmlPage.IsEnabled;
            }
        }
#endif
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        #endregion

        #region Private Properties

#if WPF
        private new String ToolTip
        {
            get;
            set;
        }
#endif

        #endregion

        #region Internal Property

        /// <summary>
        /// Visifire Control reference
        /// </summary>
        internal VisifireControl Chart
        {
            get;
            set;
        }

        /// <summary>
        /// Property change event will be fired only if value is True
        /// </summary>
        internal Boolean IsNotificationEnable
        {
            get;
            set;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Formats newline character
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formated Text</returns>
        internal static String GetFormattedMultilineText(String text)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            String[] split = { "\\n" };
            String[] lines = text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            String multiLineText = "";
            foreach (String line in lines)
            {
                if (line.EndsWith("\\"))
                {
                    multiLineText += line + "n";
                }
                else
                {
                    multiLineText += line + "\n";
                }
            }

            if (text.EndsWith("\\n"))
                return multiLineText;
            else
                return multiLineText.Substring(0, multiLineText.Length - 1);
        }

        /// <summary>
        /// Returns dash array for line
        /// </summary>
        /// <param name="lineStyle">LineStyle as LineStyles</param>
        /// <returns>DashArray as DoubleCollection</returns>
        internal DoubleCollection GetDashArray(LineStyles lineStyle)
        {
            if (lineStyle.ToString() == null) return null;

            DoubleCollection dashArray = new DoubleCollection();

            switch (lineStyle)
            {
                case LineStyles.Solid:
                    dashArray = null;
                    break;

                case LineStyles.Dashed:
                    dashArray.Clear();
                    dashArray.Add(4);
                    dashArray.Add(4);
                    dashArray.Add(4);
                    dashArray.Add(4);
                    break;

                case LineStyles.Dotted:
                    dashArray.Clear();
                    dashArray.Add(1);
                    dashArray.Add(2);
                    dashArray.Add(1);
                    dashArray.Add(2);
                    break;
            }

            return dashArray;
        }

        /// <summary>
        /// Returns dash array for border
        /// </summary>
        /// <param name="borderStyle">borderStyle as BorderStyles</param>
        /// <returns>DashArray as DoubleCollection</returns>
        internal DoubleCollection GetDashArray(BorderStyles borderStyle)
        {
            if (borderStyle.ToString() == null) return null;

            DoubleCollection dashArray = new DoubleCollection();

            switch (borderStyle)
            {
                case BorderStyles.Solid:
                    dashArray = null;
                    break;

                case BorderStyles.Dashed:
                    dashArray.Clear();
                    dashArray.Add(4);
                    dashArray.Add(4);
                    dashArray.Add(4);
                    dashArray.Add(4);
                    break;

                case BorderStyles.Dotted:
                    dashArray.Clear();
                    dashArray.Add(1);
                    dashArray.Add(2);
                    dashArray.Add(1);
                    dashArray.Add(2);
                    break;

            }

            return dashArray;
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        private String _toolTipText;
        internal event MouseButtonEventHandler _onMouseLeftButtonDown;       // Handler for MouseLeftButtonDown event
        internal event MouseButtonEventHandler _onMouseLeftButtonUp;         // Handler for MouseLeftButtonUp event
        internal event EventHandler<MouseEventArgs> _onMouseEnter;           // Handler for MouseEnter event
        internal event EventHandler<MouseEventArgs> _onMouseLeave;           // Handler for MouseLeave event
        internal event EventHandler<MouseEventArgs> _onMouseMove;            // Handler for MouseMove event
        internal ObservableObject _target;                                   // Reference of the TargetObject for event
        #endregion
    }
}