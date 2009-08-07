/*   
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
        /// <summary>
        /// Initializes a new instance of the Visifire.Commons.ObservableObject class
        /// </summary>
        public ObservableObject()
            : base()
        {
            // Attach event handler with EventChanged event of VisifireElement
            // But do not attach the event for PlotArea beacuse PlotArea has overridden EventChanged as internal event
            if (!this.GetType().Equals(typeof(PlotArea)))
            {
                // Attach event handler with EventChanged event of VisifireElement
                EventChanged += delegate
                {
                    FirePropertyChanged("MouseEvent");
                };
            }

            IsNotificationEnable = true;
        }

        #region Public Methods

        /// <summary>
        /// Apply specific style from theme
        /// </summary>
        /// <param name="Control">Control</param>
        /// <param name="KeyName">Style key name</param>
        public void ApplyStyleFromTheme(VisifireControl control, String keyName)
        {
            bool oldIsNotificationEnable = IsNotificationEnable;
            IsNotificationEnable = false;

            Chart chart = control as Chart;
            if (chart.StyleDictionary != null)
            {
#if SL
                if (Style == null)
                {   
                    Style myStyle = chart.StyleDictionary[keyName] as Style;
                    
                    if (myStyle != null)
                        Style = myStyle;
                }
#else

                Style myStyle = chart.StyleDictionary[keyName] as Style;

                System.Diagnostics.Debug.WriteLine(keyName);
                if (myStyle != null)
                {
                    if((Chart as Chart)._isThemeChanged)
                        Style = myStyle;
                    else if(Style == null)
                         Style = myStyle;
                }

#endif
            }

            IsNotificationEnable = oldIsNotificationEnable;
        }
                
        #endregion

        #region Public Properties
               
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
        /// Get or set the ToolTipText for the DataPoint
        /// </summary>
        public override String ToolTipText
        {
            get
            {
                if ((Chart != null && !String.IsNullOrEmpty((Chart as Chart).ToolTipText)))
                    return null;
                else
                    return (String)GetValue(ToolTipTextProperty);
            }
            set
            {
                SetValue(ToolTipTextProperty, value);
            }
        }

        #endregion

        #region Public Event
            
        /// <summary>
        /// Event PropertyChanged will be fired if any property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
            
        #endregion

        #region Protected Methods

        /// <summary>
        /// UpdateVisual is used for partial rendering
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal virtual void UpdateVisual(String propertyName, object value)
        {

        }
        
        /// <summary>
        /// Check whether the Property value is changed
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
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
                else if (Chart != null && (Chart as Chart)._isTemplateApplied)
                {   
                    if (Application.Current != null && Application.Current.RootVisual != null && Application.Current.RootVisual.Dispatcher != null)
                    {
                        System.Windows.Threading.Dispatcher currentDispatcher = Application.Current.RootVisual.Dispatcher;

                            if (currentDispatcher.CheckAccess())
                                (Chart as Chart).InvokeRender();
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
                if (Chart != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
#endif
            }
        }

        #endregion

        #region Internal Properties

#if WPF

        /// <summary>
        /// Whether the application is in design mode
        /// </summary>
        internal Boolean IsInDesignMode
        {
            get
            {
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            }
        }
#else
        /// <summary>
        /// Whether the application is in design mode
        /// </summary>
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
        /// <summary>
        /// Overrides the tooltip property of control
        /// </summary>
        private new String ToolTip
        {
            get;
            set;
        }
#endif

        #endregion

        #region Internal Property

        /// <summary>
        /// Property change event will be fired if value is True
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

        /// <summary>
        /// Whether the PropertyChanged event is fired
        /// </summary>
        internal static Boolean _isPropertyChangedFired = false; 

        /// <summary>
        /// Whether this object is automatically generated while rendering. 
        /// And it is used while creating auto elements during render
        /// </summary>
        internal Boolean _isAutoGenerated;

      
       #endregion
    }
}