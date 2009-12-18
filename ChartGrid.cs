#if WPF

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;


#endif
using Visifire.Commons;
using Visifire.Charts;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.ChartGrid class. It contains grids of axis.
    /// </summary>
    public class ChartGrid : ObservableObject
    {

        #region Public Methods

        /// <summary>
        ///  Initializes a new instance of the Visifire.Charts.ChartGrid class
        /// </summary>
        public ChartGrid()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartGrid), new FrameworkPropertyMetadata(typeof(ChartGrid)));
                _defaultStyleKeyApplied = true;

            }
#else
            DefaultStyleKey = typeof(ChartGrid);
#endif

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.Interval dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.Interval dependency property.
        /// </returns>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(ChartGrid),
            new PropertyMetadata(OnIntervalPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(ChartGrid),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
             ("LineColor",
             typeof(Brush),
             typeof(ChartGrid),
             new PropertyMetadata(OnLineColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.LineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.LineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register
            ("LineStyle",
            typeof(LineStyles),
            typeof(ChartGrid),
            new PropertyMetadata(OnLineStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Double),
            typeof(ChartGrid),
            new PropertyMetadata(0.25, OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.InterlacedColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.InterlacedColor dependency property.
        /// </returns>
        public static readonly DependencyProperty InterlacedColorProperty = DependencyProperty.Register(
            "InterlacedColor",
            typeof(Brush),
            typeof(ChartGrid),
            new PropertyMetadata(OnInterlacedColorPropertyChanged));

        /// <summary>
        /// Get or set the grid interval
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> Interval
        {
            get
            {
                if ((Nullable<Double>)GetValue(IntervalProperty) == null && ParentAxis != null)
                    return ParentAxis.InternalInterval;
                else
                    return (Nullable<Double>)GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

       /// <summary>
       /// ToolTipText property
       /// ( NotImplemented )
       /// </summary>
       [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
       public override String ToolTipText
       {
           get
           {
               throw new NotImplementedException("ToolTipText property for ChartGrid is not implemented");
           }
           set
           {
               throw new NotImplementedException("ToolTipText property for ChartGrid is not implemented");
           }
       }

       /// <summary>
       /// Get or set the grid line style
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
       /// Get or set the grid interlaced color
        /// </summary>
       public Brush InterlacedColor
       {
           get { return (Brush)GetValue(InterlacedColorProperty); }
           set { SetValue(InterlacedColorProperty, value); }
       }
       
       /// <summary>
       /// Get or set the grid line thickness 
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
       /// Enables or disables grid lines
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
       /// Get or set the grid line color
       /// </summary>
       public Brush LineColor
       {
           get
           {
               return (GetValue(LineColorProperty) != null) ? (Brush)GetValue(LineColorProperty) : Graphics.GRAY_BRUSH;
           }
           set
           {
               SetValue(LineColorProperty, value);
           }
       }

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties
        
        /// <summary>
        /// Visual element for grid
        /// </summary>
        internal Canvas Visual
        {
            get;
            set;
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
        /// Minimum data value for the axis
        /// </summary>
        internal Double DataMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum data value for the axis
        /// </summary>
        internal Double DataMaximum
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the width of the grid canvas, will be used only with the Horizontal axis
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
        /// Get or set the height of the grid canvas, will be used ony with the vertical axis 
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
        /// Placement decides how the grids have to be positioned 
        /// </summary>
        internal PlacementTypes Placement
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

        /// <summary>
        /// Chart grid storyboard
        /// </summary>
        internal Storyboard Storyboard
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods
        
       /// <summary>
        /// Creates grids and also position them appropriately
       /// </summary>
       /// <param name="animationEnabled">Whether animation is enabled</param>
       /// <param name="animationDuration">Animation duration</param>
        private void CreateAndPositionChartGrid(bool animationEnabled, Double animationDuration)
        {
            Double interval = (Double)Interval; // Interval  for the chart grid
            Decimal index =0;// = (Decimal)Minimum;   // starting point for the loop that generates grids
            Decimal minVal = (Decimal)Minimum;  // smallest value from where the grid must be drawn
            Decimal maxVal = (Decimal)Maximum;  // largest value from where the grid must be drawn

            // gap between two intervals
            Decimal gap = (Decimal)interval;// +(((Nullable<Double>)GetValue(IntervalProperty) == null) ? ParentAxis.SkipOffset : 0); 
            
            //Int32 count = 0;                    // counts the number of lines required for alternate colored bands
            Int32 countRectangles = 0;          // counts the number of color bands for animating them alternately in opposite direction
            Double position = 0;                // value of the line position for the running loop cycle
            Double prevPosition = 0;            // value of the line position for the previous position

            
            // if axisX and the first data point is in a gap between the prescribed interval, the index must dateTime from the 
            // datapoint rather than the axis minimum
            // if ((DataMinimum - interval) < Minimum && ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
            //    index = (Decimal)DataMinimum;

            if (ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
            {
                if (Double.IsNaN((Double)ParentAxis.AxisMinimumNumeric))
                {
                    if (ParentAxis.XValueType != ChartValueTypes.Numeric)
                    {
                        minVal = (Decimal)ParentAxis.FirstLabelPosition;
                    }
                    else
                    {
                        if ((DataMinimum - Minimum) / interval >= 1)
                            minVal = (Decimal)(DataMinimum - Math.Floor((DataMinimum - Minimum) / interval) * interval);
                        else
                            minVal = (Decimal)DataMinimum;
                    }
                }
            }

            //index = minval;
            // maxVal = maxVal + gap / 1000;

#if WPF
            if (Storyboard != null && Storyboard.GetValue(Storyboard.TargetProperty) != null)
                Storyboard.Stop();
#else       
            if (Storyboard != null)
                Storyboard.Stop();
#endif

            InterlacedRectangles = new List<Rectangle>();
            InterlacedLines = new List<Line>();

            if (minVal != maxVal)
            {
                InterlacedRectangles = new List<Rectangle>();
                Decimal xValue;

                for (xValue = minVal; xValue <= maxVal; )
                {
                    Line line = new Line();
                    InterlacedLines.Add(line);
                    line.Stroke = LineColor;
                    line.StrokeThickness = LineThickness;
                    line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

                    line.Width = Width;
                    line.Height = Height;

                    switch (Placement)
                    {
                        case PlacementTypes.Top:
                        case PlacementTypes.Bottom:

                            position = Graphics.ValueToPixelPosition(0, Width, Minimum, Maximum, (Double)xValue);

                            line.X1 = position;
                            line.X2 = position;
                            line.Y1 = 0;
                            line.Y2 = Height;

                            if (index % 2 == 1)
                            {
                                Rectangle rectangle = new Rectangle();

                                if (animationEnabled)
                                {
                                    ScaleTransform scaleTransform;
                                    if (countRectangles % 2 == 0)
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                        rectangle.RenderTransformOrigin = new Point(0.5, 1);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, animationDuration));
                                    }
                                    else
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                        rectangle.RenderTransformOrigin = new Point(0.5, 0);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, animationDuration));
                                    }
                                    rectangle.RenderTransform = scaleTransform;
                                }

                                countRectangles++;

                                rectangle.Width = Math.Abs(position - prevPosition);
                                rectangle.Height = Height;
                                rectangle.Fill = InterlacedColor;
                                rectangle.SetValue(Canvas.LeftProperty, prevPosition);
                                rectangle.SetValue(Canvas.TopProperty, (Double)0);
                                rectangle.SetValue(Canvas.ZIndexProperty, (Int32)(-countRectangles));
                                Visual.Children.Add(rectangle);
                                InterlacedRectangles.Add(rectangle);
                            }

                            break;

                        case PlacementTypes.Left:
                        case PlacementTypes.Right:

                            position = Graphics.ValueToPixelPosition(Height, 0, Minimum, Maximum, (Double)xValue);

                            if (position == 0)
                                position += this.LineThickness;

                            line.X1 = 0;
                            line.X2 = Width;
                            line.Y1 = position;
                            line.Y2 = position;

                            if (index % 2 == 1)
                            {
                                Rectangle rectangle = new Rectangle();

                                if (animationEnabled)
                                {
                                    ScaleTransform scaleTransform;

                                    if (countRectangles % 2 == 0)
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                        rectangle.RenderTransformOrigin = new Point(0, 0.5);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, animationDuration));
                                    }
                                    else
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                        rectangle.RenderTransformOrigin = new Point(1, 0.5);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, animationDuration));
                                    }

                                    rectangle.RenderTransform = scaleTransform;
                                }

                                countRectangles++;
                                rectangle.Width = Width;
                                rectangle.Height = Math.Abs(position - prevPosition);
                                rectangle.Fill = InterlacedColor;
                                rectangle.SetValue(Canvas.LeftProperty, (Double)0);
                                rectangle.SetValue(Canvas.TopProperty, position);
                                rectangle.SetValue(Canvas.ZIndexProperty, (Int32)(-countRectangles));
                                Visual.Children.Add(rectangle);
                                InterlacedRectangles.Add(rectangle);
                            }

                            break;
                    }
                    
                    Visual.Children.Add(line);
                    prevPosition = position;

                    index += (ParentAxis.SkipOffset + 1);

                    if (ParentAxis.IsDateTimeAxis)
                    {
                        DateTime dt = DateTimeHelper.UpdateDate(ParentAxis.FirstLabelDate, (Double)(index * gap), ParentAxis.InternalIntervalType);
                        Decimal oneUnit = (Decimal)DateTimeHelper.DateDiff(dt, ParentAxis.FirstLabelDate, ParentAxis.MinDateRange, ParentAxis.MaxDateRange, ParentAxis.InternalIntervalType, ParentAxis.XValueType);

                        xValue = minVal + oneUnit;
                    }
                    else
                    {
                        xValue = minVal + index * gap;
                    }

                }

                if (index % 2 == 1)
                {
                    Rectangle rectangle = new Rectangle();
                    ScaleTransform scaleTransform = null;
                    switch (Placement)
                    {
                        case PlacementTypes.Top:
                        case PlacementTypes.Bottom:
                            rectangle.Width = Math.Abs(Width - position);
                            rectangle.Height = Height;

                            if (animationEnabled)
                            {
                                if (countRectangles % 2 == 0)
                                {
                                    scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                    rectangle.RenderTransformOrigin = new Point(0.5, 1);
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, animationDuration));
                                }
                                else
                                {
                                    scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                    rectangle.RenderTransformOrigin = new Point(0.5, 0);
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, animationDuration));
                                }
                            }

                            rectangle.SetValue(Canvas.LeftProperty, position);
                            rectangle.SetValue(Canvas.TopProperty, (Double)0);
                            break;

                        case PlacementTypes.Left:
                        case PlacementTypes.Right:
                            rectangle.Width = Width;
                            rectangle.Height = Math.Abs(position);

                            if (animationEnabled)
                            {
                                if (countRectangles % 2 == 0)
                                {
                                    scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                    rectangle.RenderTransformOrigin = new Point(0, 0.5);
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, animationDuration));
                                }
                                else
                                {
                                    scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                    rectangle.RenderTransformOrigin = new Point(1, 0.5);
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, animationDuration));
                                }
                            }

                            rectangle.SetValue(Canvas.LeftProperty, (Double)0);
                            rectangle.SetValue(Canvas.TopProperty, (Double)0);
                            break;
                    }

                    rectangle.RenderTransform = scaleTransform;
                    rectangle.Fill = InterlacedColor;
                    rectangle.SetValue(Canvas.ZIndexProperty, (Int32)(-countRectangles));
                    Visual.Children.Add(rectangle);
                    InterlacedRectangles.Add(rectangle);
                }
            }

            Visual.Width = Width;
            Visual.Height = Height;
        }

        /// <summary>
        /// Generates a Double animation sequence with fixed parameters
        /// </summary>
        /// <param name="target">Animation target object</param>
        /// <param name="property">Animation target property</param>
        /// <param name="from">Start value</param>
        /// <param name="to">End value</param>
        /// <param name="begin">Animation begin time</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>DoubleAnimationUsingKeyFrames</returns>
        private DoubleAnimationUsingKeyFrames CreateDoubleAnimation(DependencyObject target, String property, Double from, Double to, Double begin, Double duration)
        {
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            da.BeginTime = TimeSpan.FromSeconds(begin);
#if WPF
            target.SetValue(NameProperty, target.GetType().Name + Guid.NewGuid().ToString().Replace('-', '_'));
            Storyboard.SetTargetName(da, target.GetValue(NameProperty).ToString());

            Chart._rootElement.RegisterName((string)target.GetValue(NameProperty), target);

#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));

            SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
            keyFrame.KeySpline = new KeySpline() { ControlPoint1 = new Point(0, 0), ControlPoint2 = new Point(1, 1) };
            keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            keyFrame.Value = from;
            da.KeyFrames.Add(keyFrame);

            keyFrame = new SplineDoubleKeyFrame();
            keyFrame.KeySpline = new KeySpline() { ControlPoint1 = new Point(0.5, 0), ControlPoint2 = new Point(0.5, 1) };
            keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration));
            keyFrame.Value = to;
            da.KeyFrames.Add(keyFrame);

            return da;
        }
        
        /// <summary>
        /// IntervalProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged(VcProperties.Interval);
        }

        /// <summary>
        /// EnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged(VcProperties.Enabled);
        }

        /// <summary>
        /// LineColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.UpdateVisual(VcProperties.LineColor, e.NewValue);
        }

        /// <summary>
        /// LineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.UpdateVisual(VcProperties.LineStyle, e.NewValue);
        }

        /// <summary>
        /// LineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.UpdateVisual(VcProperties.LineThickness, e.NewValue);
        }

        /// <summary>
        /// InterlacedColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnInterlacedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.UpdateVisual(VcProperties.InterlacedColor, e.NewValue);
        }
                
        #endregion

        #region Private Properties
        
        /// <summary>
        /// Identifies the Visifire.Charts.ChartGrid.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.ChartGrid.ToolTipText dependency property.
        /// </returns>
        private new static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(ChartGrid),
            null);

        /// <summary>
        /// List of interlaced rectangles
        /// </summary>
        private List<Rectangle> InterlacedRectangles
        {
            get;
            set;
        }

        /// <summary>
        /// List of interlaced lines
        /// </summary>
        private List<Line> InterlacedLines
        {
            get;
            set;
        }
        
        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates the visual element for chart grid
        /// </summary>
        /// <param name="width">ChartVisualCanvas width</param>
        /// <param name="height">ChartVisualCanvas height</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <param name="animationDuration">Animation duration</param>
        internal void CreateVisualObject(Double width, Double height, bool animationEnabled, Double animationDuration)
        {   
            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            if (Visual == null)
                Visual = new Canvas();
            else
                Visual.Children.Clear();

            Width = width;
            Height = height;

            if (animationEnabled)
            {
                Storyboard = new Storyboard();

                ScaleTransform st = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
                Visual.RenderTransformOrigin = new Point(0.5, 0.5);
                Visual.RenderTransform = st;

                if (Placement == PlacementTypes.Top || Placement == PlacementTypes.Bottom)
                    Storyboard.Children.Add(CreateDoubleAnimation(st, "(ScaleTransform.ScaleY)", 0, 1, 0, animationDuration));
                else
                    Storyboard.Children.Add(CreateDoubleAnimation(st, "(ScaleTransform.ScaleX)", 0, 1, 0, animationDuration));
            }

            CreateAndPositionChartGrid(animationEnabled, animationDuration);

            Visual.Opacity = this.Opacity;
        }
        
        /// <summary>
        /// Update visual used for partial update
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(VcProperties propertyName, object value)
        {
            if (Visual != null)
            {
                foreach (Rectangle rec in InterlacedRectangles)
                    rec.Fill = InterlacedColor;

                foreach (Line line in InterlacedLines)
                {
                    line.Stroke = LineColor;
                    line.StrokeThickness = LineThickness;
                    line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);
                }
            }
            else
                FirePropertyChanged(propertyName);
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        /// <summary>
        /// Set the width of the grid canvas, will be used only with the Horizontal axis
        /// </summary>
        private Double _width;

        /// <summary>
        /// Set the height of the grid canvas, will be used ony with the vertical axis 
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
