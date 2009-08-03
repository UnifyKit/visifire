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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;


#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;

#endif
using Visifire.Commons;

namespace Visifire.Charts
{

    /// <summary>
    /// TrendLine can be used to draw lines to indicate significance of certain points
    /// </summary>
    public class TrendLine : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.TrendLine class
        /// </summary>
        public TrendLine()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(
            "Enabled",
            typeof(Nullable<Boolean>),
            typeof(TrendLine),
            new PropertyMetadata(OnEnabledPropertyChanged));

#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register(
            "LineColor",
            typeof(Brush),
            typeof(TrendLine),
            new PropertyMetadata(OnLineColorPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
            "LineThickness",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.LineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.LineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle",
            typeof(LineStyles),
            typeof(TrendLine),
            new PropertyMetadata(OnLineStylePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register(
            "ShadowEnabled",
            typeof(bool),
            typeof(TrendLine),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Value dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Value dependency property.
        /// </returns>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(OnValuePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register(
            "HrefTarget",
            typeof(HrefTargets),
            typeof(TrendLine),
            new PropertyMetadata(OnHrefTargetChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register(
            "Href",
            typeof(String),
            typeof(TrendLine),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.AxisType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.AxisType dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisTypeProperty = DependencyProperty.Register(
            "AxisType",
            typeof(AxisTypes),
            typeof(TrendLine),
            new PropertyMetadata(OnAxisTypePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.TrendLine.Orientation dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.TrendLine.Orientation dependency property.
        /// </returns>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(TrendLine),
            new PropertyMetadata(Orientation.Horizontal, OnOrientationPropertyChanged));

        /// <summary>
        /// Enables or disables the TrendLine
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {
            get
            {   
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null)
                    return true;
                else
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Opacity property
        /// </summary>
        public new Double Opacity
        {
            get
            {
                return (Double)GetValue(OpacityProperty);
            }
            set
            {
#if SL
                if (Opacity != value)
                {
                    SetValue(OpacityProperty, value);
                    FirePropertyChanged("Opacity");
                }
#else
                SetValue(OpacityProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Cursor property
        /// </summary>
        public new Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                if (base.Cursor != value)
                {
                    base.Cursor = value;
                    FirePropertyChanged("Cursor");
                }
            }
        }

        /// <summary>
        /// Get or set the Color of the TrendLine
        /// </summary>
        public Brush LineColor
        {
            get 
            {
                return (Brush)GetValue(LineColorProperty) == null ? new SolidColorBrush(Colors.Red) : (Brush)GetValue(LineColorProperty);
            }
            set
            { 
                SetValue(LineColorProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Line thickness of TrendLine
        /// </summary>
        public Double LineThickness
        {
            get 
            { 
                return (Double)GetValue(LineThicknessProperty) == 0 ? 2 : (Double)GetValue(LineThicknessProperty);
            }
            set
            { 
                SetValue(LineThicknessProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Line style of TrendLine
        /// </summary>
        public LineStyles LineStyle
        {
            get 
            { 
                return (LineStyles)GetValue(LineStyleProperty);
            }
            set 
            { 
                SetValue(LineStyleProperty, value); 
            }
        }

        /// <summary>
        /// Whether shadow is enebled for the TrendLine
        /// </summary>
        public bool ShadowEnabled
        {
            get 
            { 
                return (bool)GetValue(ShadowEnabledProperty);
            }
            set 
            { 
                SetValue(ShadowEnabledProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Value of the TrendLine
        /// </summary>
        public Double Value
        {
            get 
            { 
                return (Double)GetValue(ValueProperty); 
            }
            set 
            { 
                SetValue(ValueProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the AxisType of the TrendLine
        /// </summary>
        public AxisTypes AxisType
        {
            get 
            { 
                return (AxisTypes)GetValue(AxisTypeProperty); 
            }
            set 
            { 
                SetValue(AxisTypeProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the Orientation of the TrendLine. 
        /// Whether the TrendLine should be vertically oriented or horizontally oriented
        /// </summary>
        public Orientation Orientation
        {
            get 
            { 
                return (Orientation)GetValue(OrientationProperty); 
            }
            set 
            { 
                SetValue(OrientationProperty, value); 
            }
        }

        /// <summary>
        /// Get or set the HrefTarget property of TrendLine
        /// </summary>
        public HrefTargets HrefTarget
        {
            get
            {
                return (HrefTargets)GetValue(HrefTargetProperty);
            }
            set
            {
                SetValue(HrefTargetProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Href property of TrendLine
        /// </summary>
        public String Href
        {
            get
            {
                return (String)GetValue(HrefProperty);
            }
            set
            {
                SetValue(HrefProperty, value);
            }
        }

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Axis reference
        /// </summary>
        internal Axis ReferingAxis
        {
            get;
            set;
        }

        /// <summary>
        /// TrendLine canvas
        /// </summary>
        internal Canvas Visual
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Line visual
        /// </summary>
        private Line Line
        {
            get;
            set;
        }

        /// <summary>
        /// Shadow visual
        /// </summary>
        private Line Shadow
        {
            get;
            set;
        }

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
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Enabled");
        }

#if WPF
        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Opacity");
        }
#endif
        
        /// <summary>
        /// LineColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual("LineColor", e.NewValue);
        }

        /// <summary>
        /// LineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            //source.FirePropertyChanged("LineThickness");
            source.UpdateVisual("LineThickness", e.NewValue);
        }

        /// <summary>
        /// LineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual("LineStyle", e.NewValue);
        }

        /// <summary>
        /// ShadowEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.UpdateVisual("ShadowEnabled", e.NewValue);
        }

        /// <summary>
        /// ValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Value");
        }

        /// <summary>
        /// AxisTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("AxisType");
        }

        /// <summary>
        /// OrientationProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Orientation");
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;
            trendLine.FirePropertyChanged("HrefTarget");
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine trendLine = d as TrendLine;
            trendLine.FirePropertyChanged("Href");
        }

        /// <summary>
        /// Apply properties of TrendLine to line visual and line-shadow visual
        /// </summary>
        private void ApplyProperties()
        {
            Line.Stroke = LineColor;
            Line.StrokeThickness = LineThickness;
            Line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);
            
            Shadow.StrokeThickness = LineThickness + 2;
            Shadow.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            Shadow.Stroke = new SolidColorBrush(Colors.LightGray);
            Shadow.Opacity = 0.7;

            Shadow.StrokeDashCap = PenLineCap.Round;
            Shadow.StrokeLineJoin = PenLineJoin.Round;
            Shadow.Visibility = ShadowEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// UpdateVisual is used for partial rendering
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(string propertyName, object value)
        {
            if (Line == null || Shadow == null)
                FirePropertyChanged(propertyName);
            else
                ApplyProperties();
        }

        /// <summary>
        /// Create visual objects for TrendLine
        /// </summary>
        /// <param name="width">Width of the ChartCanvas</param>
        /// <param name="height">Height of the ChartCanvas</param>
        internal void CreateVisualObject(Double width, Double height)
        {
            if (ReferingAxis == null || !(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            Visual = new Canvas();
            Visual.Opacity = this.Opacity;
            Visual.Cursor = this.Cursor;
            Double shadowThickness = LineThickness + 2;
            Line = new Line();
            Shadow = new Line();

            ApplyProperties();

            switch (Orientation)
            {   
                case Orientation.Vertical:
                    Line.Y1 = 0;
                    Line.Y2 = height;
                    Line.X1 = Graphics.ValueToPixelPosition(0,
                                        width,
                                        (Double)ReferingAxis.InternalAxisMinimum,
                                        (Double)ReferingAxis.InternalAxisMaximum,
                                        Value);
                    Line.X2 = Line.X1;
                    Visual.Height = height;
                    Visual.Width = shadowThickness;
                    break;
                case Orientation.Horizontal:
                    Line.X1 = 0;
                    Line.X2 = width;
                    Line.Y1 = Graphics.ValueToPixelPosition(height,
                                        0,
                                        (Double)ReferingAxis.InternalAxisMinimum,
                                        (Double)ReferingAxis.InternalAxisMaximum,
                                        Value);
                    Line.Y2 = Line.Y1;
                    Visual.Height = shadowThickness;
                    Visual.Width = width;
                    break;

            }

            if (Orientation == Orientation.Horizontal)
            {
                Shadow.X1 = Line.X1;
                Shadow.X2 = Line.X2 + 3;
                Shadow.Y1 = Line.Y1 + 3;
                Shadow.Y2 = Line.Y2 + 3;
            }
            else
            {
                Shadow.X1 = Line.X1 + 3;
                Shadow.X2 = Line.X2 + 3;
                Shadow.Y1 = Line.Y1 + 3;
                Shadow.Y2 = Line.Y2;
            }

            Visual.Children.Add(Shadow);
            Visual.Children.Add(Line);

            AttachToolTip(ReferingAxis.Chart, this, Line);
            AttachHref(ReferingAxis.Chart, Line, Href, HrefTarget);
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }
}
