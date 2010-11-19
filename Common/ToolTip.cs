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

#if WPF

    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.ComponentModel;

#else

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Collections.Generic;

#endif

using System.Windows.Input;
using System.Windows.Shapes;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.ToolTip class
    /// </summary>
#if SL &&!WP
    [System.Windows.Browser.ScriptableType]
#endif
    public class ToolTip : Visifire.Commons.VisifireElement
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.ToolTip class
        /// </summary>
        public ToolTip()
        {   
            Text = "";

            // Apply default style from generic
#if WPF     
            if (!_defaultStyleKeyApplied)
            {   
                DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
                _defaultStyleKeyApplied = true;
            }
#else
            DefaultStyleKey = typeof(ToolTip);
#endif
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes (such as a rebuilding layout pass) call System.Windows.Controls.Control.ApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _borderElement = GetTemplateChild("ToolTipBorder") as Border;
            _toolTipTextBlock = GetTemplateChild("ToolTipTextBlock") as TextBlock;
            _callOutPath = GetTemplateChild("CallOut") as Path;

            if(_toolTipTextBlock != null)
                _toolTipTextBlock.LineHeight = (Int32)(_toolTipTextBlock.FontSize * 1.4); 

            this.Visibility = Visibility.Collapsed;
        }

        #endregion
        
        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.ToolTip.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ToolTip.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(ToolTip),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ToolTip.Text dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ToolTip.Text dependency property.
        /// </returns>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
            ("Text",
            typeof(String),
            typeof(ToolTip),
            new PropertyMetadata(OnTextPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ToolTip.CornerRadius dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ToolTip.CornerRadius dependency property.
        /// </returns>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register
            ("CornerRadius",
            typeof(CornerRadius),
            typeof(ToolTip),
            null);

        /// <summary>
        /// Identifies the Visifire.Charts.ToolTip.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ToolTip.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(ToolTip),
            null);

        /// <summary>
        /// Visifire chart control reference
        /// </summary>
#if WPF
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Visifire.Commons.VisifireControl Chart
        {
            get;
            set;
        }

        /// <summary>
        /// Enables or disables the ToolTip
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null)
                {
                    if (Chart != null)
                        return Chart.ToolTipEnabled;
                    else
                        return true;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
                }
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Text property of ToolTip
        /// </summary>
        public String Text
        {   
            get
            {
                return (GetValue(TextProperty) == null) ? "" : (String)GetValue(TextProperty);
            }
            set
            {
                String val = Visifire.Commons.ObservableObject.GetFormattedMultilineText(value);
                SetValue(TextProperty, val);
            }
        }

        /// <summary>
        /// Get or set the CornerRadius property of ToolTip
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Get or set the FontColor of ToolTip Text
        /// </summary>
        public Brush FontColor
        {
            get
            {
                return (Brush)GetValue(FontColorProperty);
            }
            set
            {
                _isAutoGeneratedFontColor = false;
                SetValue(FontColorProperty, value);
            }
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        internal Point CallOutStartPoint
        {
            set
            {
                if (_callOutPath != null)
                {
                    ((_callOutPath.Data as PathGeometry).Figures[0] as PathFigure).StartPoint = value;
                }
            }
        }

        internal Point CallOutMidPoint
        {
            set
            {
                if (_callOutPath != null)
                {
                    (((_callOutPath.Data as PathGeometry).Figures[0] as PathFigure).Segments[0] as LineSegment).Point = value;
                }
            }
        }

        internal Point CallOutEndPoint
        {
            set
            {   
                if (_callOutPath != null)
                {
                    (((_callOutPath.Data as PathGeometry).Figures[0] as PathFigure).Segments[1] as LineSegment).Point = value;
                }
            }
        }

        internal Visibility CallOutVisiblity
        {
            set
            {
                if (_callOutPath != null)
                {
                    _callOutPath.Visibility = value;
                }
            }
            get
            {
                return (_callOutPath != null)? _callOutPath.Visibility: Visibility.Collapsed;
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// EnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolTip t = d as ToolTip;

            if (t._borderElement != null)
            {
                if ((Boolean)e.NewValue)
                    t._borderElement.Visibility = t.Visibility;
                else
                    t._borderElement.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// TextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolTip tootlTip = d as ToolTip;

            if (tootlTip._toolTipTextBlock != null)
                tootlTip._toolTipTextBlock.Text = (String)e.NewValue;

            if (tootlTip.Chart != null)
            {
                tootlTip.MaxWidth = tootlTip.Chart.ActualWidth;

                if (tootlTip._toolTipTextBlock != null)
                    tootlTip._toolTipTextBlock.MaxWidth = tootlTip.Chart.ActualWidth - 4;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Show ToolTip
        /// </summary>
        internal void Show()
        {
            if ((Boolean)Enabled)
            {
                if (Text == String.Empty)
                    Hide();
                else
                    this.Visibility = Visibility.Visible;
            }
            else
                Hide();
        }

        /// <summary>
        /// Hide ToolTip
        /// </summary>
        internal void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data
        
        /// <summary>
        /// Border element of ToolTip
        /// </summary>
        internal Border _borderElement;

        /// <summary>
        /// TextBlock element of ToolTip
        /// </summary>
        private TextBlock _toolTipTextBlock;
        internal Path _callOutPath;
        internal Boolean _isAutoGenerated;
        internal Boolean _isAutoGeneratedFontColor = true;

#if WPF
        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif

        #endregion
    }
}