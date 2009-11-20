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
using System.Windows.Shapes;

#else

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

#endif

using Visifire.Commons;

namespace Visifire.Charts
{
    /// <summary>
    /// Ticks of axis
    /// </summary>
    public class Ticks : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.Ticks class
        /// </summary>
        public Ticks()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Ticks), new FrameworkPropertyMetadata(typeof(Ticks)));
                _defaultStyleKeyApplied = true;
            }
#else
            DefaultStyleKey = typeof(Ticks);
#endif
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.TickLength dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.TickLength dependency property.
        /// </returns>
        public static readonly DependencyProperty TickLengthProperty = DependencyProperty.Register
        ("TickLength",
        typeof(Double),
        typeof(Ticks),
        new PropertyMetadata((Double)5, OnTickLengthPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.LineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.LineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register
            ("LineStyle",
            typeof(LineStyles),
            typeof(Ticks),
            new PropertyMetadata(OnLineStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Double),
            typeof(Ticks),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
            ("LineColor",
            typeof(Brush),
            typeof(Ticks),
            new PropertyMetadata(OnLineColorPropertyChanged));


        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.Interval dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.Interval dependency property.
        /// </returns>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(Ticks),
            new PropertyMetadata(OnIntervalPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(Ticks),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Enables or disables Major Tick 
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
        /// Get or set the Major Tick interval
        /// </summary>
#if SL  
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif  
        public Nullable<Double> Interval
        {
            get
            {
                if ((Nullable<Double>)GetValue(IntervalProperty) == null)
                    return ParentAxis.InternalInterval;
                else
                    return (Nullable<Double>) GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        /// <summary>
        /// Get or set the ToolTipText property
        /// ( NotImplemented )
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override String ToolTipText
        {
            get
            {
                throw new NotImplementedException("ToolTipText property for Ticks is not implemented");
            }
            set
            {
                throw new NotImplementedException("ToolTipText property for Ticks is not implemented");
            }
        }
        
        /// <summary>
        /// Get or set the Major Tick LineColor
        /// </summary>
        public Brush LineColor
        {
            get
            {
                return (Brush)GetValue(LineColorProperty);
            }
            set
            {
                SetValue(LineColorProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the Major Tick LineThickness
        /// </summary>
        public Double LineThickness
        {
            get
            {
                return (Double)GetValue(LineThicknessProperty);
            }
            set
            {
                SetValue(LineThicknessProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the Major Tick LineStyle
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
        /// Get or set the Length of the ticks
        /// </summary>
        public Double TickLength
        {
            get
            {
                return (Double)GetValue(TickLengthProperty);
            }
            set
            {
                SetValue(TickLengthProperty, value);
            }
        }

        #region Hidden ControlProperties

        private new Brush Background { get; set; }
        private new Brush BorderBrush { get; set; }
        private new Thickness BorderThickness { get; set; }
        private new FontFamily FontFamily { get; set; }
        private new Double FontSize { get; set; }
        private new FontStyle FontStyle { get; set; }
        private new FontStretch FontStretch { get; set; }
        private new FontWeight FontWeight { get; set; }
        private new Brush Foreground  { get; set; }

        #endregion

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Visual element for Major Ticks
        /// </summary>
        internal Canvas Visual
        {
            get;
            private set;
        }

        /// <summary>
        /// Actual minimum value of the axis
        /// </summary>
        internal Double Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Actual maximum value of the axis
        /// </summary>
        internal Double Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// Minimum value for the axis
        /// </summary>
        internal Double DataMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum value for the axis
        /// </summary>
        internal Double DataMaximum
        {
            get;
            set;
        }

        /// <summary>
        /// Set the width of the Major Tick canvas, will be used only with the Horizontal axis
        /// </summary>
        internal new Double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Set the height of the Major Tick canvas, will be used ony with the vertical axis 
        /// </summary>
        internal new Double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// Placement decides how the ticks have to be positioned 
        /// </summary>
        internal PlacementTypes Placement
        {
            get;
            set;
        }

        /// <summary>
        /// Dictionary of AxisLabels
        /// </summary>
        internal Dictionary<Double, String> AxisLabelsDictionary
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether all unique XValues have labels or not
        /// </summary>
        internal Boolean AllAxisLabels
        {
            get;
            set;
        }

        /// <summary>
        /// Parent axis reference
        /// </summary>
        internal Axis ParentAxis
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
            Ticks tick = d as Ticks;
            tick.FirePropertyChanged(VcProperties.Enabled);
        }

        /// <summary>
        /// IntervalProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged(VcProperties.Interval);
        }

        /// <summary>
        /// LineColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.UpdateVisual(VcProperties.LineColor, e.NewValue);
        }

        /// <summary>
        /// LineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.UpdateVisual(VcProperties.LineThickness, e.NewValue);
        }

        /// <summary>
        /// LineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.UpdateVisual(VcProperties.LineStyle, e.NewValue);
        }

        /// <summary>
        /// TickLengthProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTickLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged(VcProperties.TickLength);
        }

        /// <summary>
        /// Creates the major ticks and also positions them appropriately
        /// </summary>
        private void CreateAndPositionMajorTicks()
        {   
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Calculate interval
            Double interval = (Double)Interval;

            Decimal index = 0;
            Decimal minval = (Decimal)Minimum;
            Decimal maxVal = (Decimal)Maximum;
           // Decimal gap = (Decimal)interval + (((Nullable<Double>)GetValue(IntervalProperty) == null) ? ParentAxis.SkipOfset : 0);
            Decimal gap = (Decimal)interval;

            Double position;

            if (ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
            {
                if (Double.IsNaN((Double)ParentAxis.AxisMinimumNumeric))
                {
                    if (ParentAxis.XValueType != ChartValueTypes.Numeric)
                    {
                        minval = (Decimal)ParentAxis.FirstLabelPosition;
                    }
                    else
                    {
                        if ((DataMinimum - Minimum) / interval >= 1)
                            minval = (Decimal)(DataMinimum - Math.Floor((DataMinimum - Minimum) / interval) * interval);
                        else
                            minval = (Decimal)DataMinimum;
                    }
                }
            }

            //minval = index;
            //maxVal = maxVal + gap / 1000;
            if (minval != maxVal)
            {
                Decimal xValue;

                for (xValue = minval; xValue <= maxVal; )
                {
                    Line line = new Line();

                    line.Stroke = LineColor;
                    line.StrokeThickness = LineThickness;
                    line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

                    switch (Placement)
                    {
                        case PlacementTypes.Top:
                            position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, (Double)xValue);

                            if (Double.IsNaN(position))
                                return;

                            line.X1 = position;
                            line.X2 = position;
                            line.Y1 = 0;
                            line.Y2 = TickLength;
                            break;

                        case PlacementTypes.Bottom:
                            position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, (Double)xValue);

                            if (Double.IsNaN(position))
                                return;

                            line.X1 = position;
                            line.X2 = position;
                            line.Y1 = 0;
                            line.Y2 = TickLength;
                            break;

                        case PlacementTypes.Left:
                        case PlacementTypes.Right:
                            position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, (Double)xValue);

                            if (Double.IsNaN(position))
                                return;

                            line.X1 = 0;
                            line.X2 = TickLength;
                            line.Y1 = position;
                            line.Y2 = position;
                            break;

                    }

                    System.Diagnostics.Debug.WriteLine("XValue=" + xValue.ToString());

                    Visual.Children.Add(line);

                    index += (ParentAxis.SkipOffset +1);

                    if (ParentAxis.IsDateTimeAxis)
                    {
                        DateTime dt = DateTimeHelper.UpdateDate(ParentAxis.FirstLabelDate, (Double)(index * gap), ParentAxis.InternalIntervalType);
                        Decimal oneUnit = (Decimal)DateTimeHelper.DateDiff(dt, ParentAxis.FirstLabelDate, ParentAxis.MinDateRange, ParentAxis.MaxDateRange, ParentAxis.InternalIntervalType, ParentAxis.XValueType);

                        xValue = minval + oneUnit;
                    }
                    else
                    {
                        xValue = minval + index * gap;
                    }
                }
            }
            switch (Placement)
            {
                case PlacementTypes.Top:
                case PlacementTypes.Bottom:
                    Visual.Width = Width;
                    Visual.Height = TickLength;
                    break;

                case PlacementTypes.Left:
                case PlacementTypes.Right:
                    Visual.Height = Height;
                    Visual.Width = TickLength;
                    break;

            }
        }

        #endregion

        #region Private Properties
        
        /// <summary>
        /// Identifies the Visifire.Charts.Ticks.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.Ticks.ToolTipText dependency property.
        /// </returns>
        private new static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(Ticks),
            null);
        
        #endregion
        
        #region Internal Methods
        
        /// <summary>
        /// Set the parameters for the Major Ticks
        /// </summary>
        /// <param name="placementTypes">placementType</param>
        /// <param name="width">Width of ticks</param>
        internal void SetParms(PlacementTypes placementTypes, Double width, Double height)
        {   
            Placement = placementTypes;

            if (!Double.IsNaN(width))
                Width = width;

            if (!Double.IsNaN(height))
                Height = height;
        }

        /// <summary>
        /// Creates the visual element for the Major Ticks
        /// </summary>
        internal void CreateVisualObject()
        {
            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            Visual = new Canvas();
            Visual.Opacity = this.Opacity;
            CreateAndPositionMajorTicks();
        }

        /// <summary>
        /// UpdateVisual is used for partial update
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(VcProperties property, object value)
        {   
            if (Visual != null)
                foreach (Line line in Visual.Children)
                {    
                    line.Stroke = LineColor;
                    line.StrokeThickness = LineThickness;
                    line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);
                }
            else
                FirePropertyChanged(property);
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        /// <summary>
        /// Set the width of the Major Tick canvas, will be used only with the Horizontal axis
        /// </summary>
        private Double _width;

        /// <summary>
        ///  Set the height of the Major Tick canvas, will be used ony with the vertical axis
        /// </summary>
        private Double _height;

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif

        #endregion
    }
}
