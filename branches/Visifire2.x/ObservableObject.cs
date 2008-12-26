using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using Visifire.Charts;
using System.Linq;
using System.Globalization;

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
                Element.MouseMove += delegate(object sender, MouseEventArgs e)
                {
                    if (Control.ToolTipEnabled)
                        Control._toolTip.Show();
                    (Control as Chart).UpdateToolTipPosition(sender, e);
                };

                // Show ToolTip on mouse move over the chart element
                Element.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Control._toolTip.Text = ToolTipText;

                    if (Control.ToolTipEnabled)
                        Control._toolTip.Show();

                   (Control as Chart).UpdateToolTipPosition(sender, e);
                };

                // Hide ToolTip on mouse out from the chart element
                Element.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    Control._toolTip.Hide();
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
                Element.MouseEnter -= delegate(object sender, MouseEventArgs e)
                {
                    Element.Cursor = Cursors.Hand;
                };

                Element.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    Element.Cursor = Cursors.Hand;
                };

                Element.MouseLeftButtonUp -= delegate(object sender, MouseButtonEventArgs e)
                {
#if WPF
                    System.Diagnostics.Process.Start("explorer.exe", Href);
#else
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(VisifireControl.GetAbsolutePath(Href)), HrefTarget.ToString());
#endif
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
        /// Apply specific style from theme
        /// </summary>
        /// <param name="Control">Control</param>
        /// <param name="KeyName">Style key name</param>
        public void ApplyStyleFromTheme(VisifireControl Control, String KeyName)
        {
            bool oldIsNotificationEnable = IsNotificationEnable;
            IsNotificationEnable = false;

            Chart chart = Control as Chart;

            if (Style == null)
                if (chart.StyleDictionary != null)
                {
                    Style myStyle = chart.StyleDictionary[KeyName] as Style;

                    if (myStyle != null)
                        Style = myStyle;
                }

            IsNotificationEnable = oldIsNotificationEnable;
        }

        
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
        //public virtual String ToolTipText
        //{
        //    get
        //    {   
        //        return _toolTipText;
        //    }
        //    set
        //    {   
        //        _toolTipText = value;
        //        FirePropertyChanged("ToolTipText");
        //    }
        //}
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
            typeof(ObservableObject),
            new PropertyMetadata(OnFixedDataPointsChanged));

        private static void OnFixedDataPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            ObservableObject c = d as ObservableObject;
            c.FirePropertyChanged("ToolTipText");
        }


        #endregion

        #region Public Event

            public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        internal virtual void UpdateVisual(String PropertyName, object Value)
        {

        }
        
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
        /// Fire property change event
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        internal void FirePropertyChanged(string propertyName)
        {
            _isPropertyChangedFired = false; // Used for testing

            if (this.PropertyChanged != null && this.IsNotificationEnable)
            {
#if SL
                if (IsInDesignMode)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                else if (Chart != null && (Chart as Chart).IsTemplateApplied)
                {
                    if (Application.Current != null && Application.Current.RootVisual != null && Application.Current.RootVisual.Dispatcher != null)
                    {
                        System.Windows.Threading.Dispatcher currentDispatcher = Application.Current.RootVisual.Dispatcher;

                            if (currentDispatcher.CheckAccess() && (Chart as Chart).IsRenderCallAllowed)
                                (Chart as Chart).CallRender();
                            else
                                currentDispatcher.BeginInvoke(new Action<String>(FirePropertyChanged), propertyName);
                    }
                    else // if we did not get the Dispatcher throw an exception
                    {
                        throw new InvalidOperationException("This object must be initialized after that the RootVisual has been loaded");
                    }

                    _isPropertyChangedFired = true;   // Used for testing
                }
#else
                if (Chart != null && (Chart as Chart).IsRenderCallAllowed)
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
        internal static Boolean IsInDesignMode
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
#if SL
        [System.Windows.Browser.ScriptableMember]
#else
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public VisifireControl Chart
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

        #endregion

        #region Internal Events

        #endregion

        #region Data

        //private String _toolTipText;
        internal static Boolean _isPropertyChangedFired = false;   // Used for testing

       #endregion
    }
}