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
    public abstract class ObservableObject : VisifireElement, INotifyPropertyChanged
    {
        public ObservableObject()
            : base()
        {
            this.EventChanged += delegate
            {
                FirePropertyChanged("MouseEvent");
            };

            IsNotificationEnable = true;
        }

        #region Public Methods

        /// <summary>
        /// Attach tooltip with a framework element
        /// </summary>
        /// <param name="control">Control reference</param>
        /// <param name="element">FrameworkElement</param>
        /// <param name="toolTipText">Tooltip text</param>
        internal static void AttachToolTip(VisifireControl Control, FrameworkElement Element, String ToolTipText)
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
                    AttachToolTip(Control, element, ToolTipText);
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
                }

                if (chart.LogLevel == 1)
                    chart.LoggerWindow.Visibility = Visibility.Visible;
                else
                    chart.Visibility = Visibility.Collapsed;
               

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

        #region Public Event

            public event PropertyChangedEventHandler PropertyChanged;

        #endregion

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
                                    (Chart as Chart).CallRender();
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

       #endregion
    }
}