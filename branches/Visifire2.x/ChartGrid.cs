#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;

#endif
using Visifire.Commons;

namespace Visifire.Charts
{

    public class ChartGrid : ObservableObject
    {

        #region Public Methods

        public ChartGrid()
        {
            SetDefaults();

#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartGrid), new FrameworkPropertyMetadata(typeof(ChartGrid)));
                _defaultStyleKeyApplied = true;

            }

            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);

#else
            DefaultStyleKey = typeof(ChartGrid);
#endif

        }

        private void SetDefaults()
        {
            //Interval = Double.NaN;
            LineStyle = LineStyles.Solid;
            LineThickness = Double.NaN;
            //  Enabled = true;
        }

        /// <summary>
        /// Creates the visual element for the Major ticks
        /// </summary>
        public void CreateVisualObject(Double width, Double height, bool animationEnabled, Double duration)
        {
            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            Visual = new Canvas();

            Width = width;
            Height = height;

            if (animationEnabled)
            {
                Storyboard = new Storyboard();
                //Visual.Resources.Add(Storyboard.GetHashCode().ToString(), Storyboard);

                ScaleTransform st = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
                Visual.RenderTransformOrigin = new Point(0.5, 0.5);
                Visual.RenderTransform = st;

                if (Placement == PlacementTypes.Top || Placement == PlacementTypes.Bottom)
                    Storyboard.Children.Add(CreateDoubleAnimation(st, "(ScaleTransform.ScaleY)", 0, 1, 0, duration));
                else
                    Storyboard.Children.Add(CreateDoubleAnimation(st, "(ScaleTransform.ScaleX)", 0, 1, 0, duration));
            }

            CreateAndPositionChartGrid(animationEnabled, duration);
            ApplyVisualProperty();
        }


        #endregion

        #region Public Properties
        /// <summary>
        /// Major grid Interval
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> Interval
        {
            get
            {
                //return (Double.IsNaN((Double)GetValue(IntervalProperty))) ? ParentAxis.Interval : (Double)GetValue(IntervalProperty);
                if ((Nullable<Double>)GetValue(IntervalProperty) == null)
                    return ParentAxis.InternalInterval;
                else
                    return (Nullable<Double>)GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        private static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(ChartGrid),
            new PropertyMetadata(OnIntervalPropertyChanged));

        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged("Interval");
        }

        /// <summary>
        /// Enables or disables Major grid 
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


        private static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(ChartGrid),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// Major grid LineColor
        /// </summary>
        public Brush LineColor
        {
            get
            {
                return (GetValue(LineColorProperty) != null) ? (Brush)GetValue(LineColorProperty) : new SolidColorBrush(Colors.Gray);
            }
            set
            {
                SetValue(LineColorProperty, value);
            }
        }

        private static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
            ("LineColor",
            typeof(Brush),
            typeof(ChartGrid),
            new PropertyMetadata(OnLineColorPropertyChanged));

        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged("LineColor");
        }

        /// <summary>
        /// Major grid LineStyle
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

        private static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register
            ("LineStyle",
            typeof(LineStyles),
            typeof(ChartGrid),
            new PropertyMetadata(OnLineStylePropertyChanged));

        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged("LineStyle");
        }

        /// <summary>
        /// Major grid LineThickness
        /// </summary>
        public Double LineThickness
        {
            get
            {
                return (!Double.IsNaN((Double)GetValue(LineThicknessProperty))) ? (Double)GetValue(LineThicknessProperty) : 0.25;
            }
            set
            {
                SetValue(LineThicknessProperty, value);
            }
        }

        private static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Double),
            typeof(ChartGrid),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged("LineThickness");
        }

        #region InterlacedColor
        public Brush InterlacedColor
        {
            get { return (Brush)GetValue(InterlacedColorProperty); }
            set { SetValue(InterlacedColorProperty, value); }
        }
        public static readonly DependencyProperty InterlacedColorProperty =
            DependencyProperty.Register(
            "InterlacedColor",
            typeof(Brush),
            typeof(ChartGrid),
            new PropertyMetadata(OnInterlacedColorPropertyChanged));

        private static void OnInterlacedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartGrid chartGrid = d as ChartGrid;
            chartGrid.FirePropertyChanged("InterlacedColor");
        }

        #endregion  InterlacedColor


        /// <summary>
        /// Visual element for major ticks
        /// </summary>
        public Canvas Visual
        {
            get;
            private set;
        }
        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties
        /// <summary>
        /// Actual minimum value of the axis
        /// </summary>
        internal Double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
            }
        }

        /// <summary>
        /// Actual maximum value of the axis
        /// </summary>
        internal Double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
            }
        }

        /// <summary>
        /// Visual minimum for the axis
        /// </summary>
        internal Double DataMinimum
        {
            get
            {
                return _dataMinimum;
            }
            set
            {
                _dataMinimum = value;
            }
        }

        /// <summary>
        /// Visual maximum for the axis
        /// </summary>
        internal Double DataMaximum
        {
            get
            {
                return _dataMaximum;
            }
            set
            {
                _dataMaximum = value;
            }
        }

        /// <summary>
        /// Set the width of the major tick canvas. will be used only with the Horizontal axis
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
        /// Set the height of the major tick canvas. will be used ony with the vertical axis 
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

        internal Dictionary<Double, String> AxisLabelsDictionary
        {
            get;
            set;
        }

        internal Boolean AllAxisLabels
        {
            get;
            set;
        }

        internal Axis ParentAxis
        {
            get;
            set;
        }

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
        /// Creates the major ticks and also positions them appropriately
        /// </summary>
        private void CreateAndPositionChartGrid(bool animationEnabled, Double duration)
        {
            Double interval = (Double)Interval; // Interval  for the chart grid
            Decimal index = (Decimal)Minimum; // starting point for the loop that generates grids
            Decimal minval = (Decimal)Minimum; // smallest value from where the grid must be drawn
            Decimal maxVal = (Decimal)Maximum; // largest value from where the grid must be drawn
            Decimal gap = (Decimal)interval +( ((Nullable<Double>)GetValue(IntervalProperty) == null) ? ParentAxis.SkipOfset : 0); // gap between two intervals
            Int32 count = 0; // counts the number of lines required for alternate colored bands
            Int32 countRectangles = 0; // counts the number of color bands for animating them alternately in opposite direction
            Double position = 0; // value of the line position for the running loop cycle
            Double prevPosition = 0; // value of the line position for the previous position

            // if axis X then and the First data point is in a gap between the prescribed interval the index must start from the 
            // datapoint rather than the axis minimum
            if ((DataMinimum - interval) < Minimum && ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
                index = (Decimal)DataMinimum;

            minval = index;
            maxVal = maxVal + gap / 1000;

            if(Storyboard != null)
                Storyboard.Stop();

            if (minval != maxVal)
            {
                for (; index <= maxVal; index = minval + (++count) * gap)
                {
                    Line line = new Line();

                    line.Stroke = LineColor;
                    line.StrokeThickness = LineThickness;
                    line.StrokeDashArray = GetDashArray(LineStyle);

                    line.Width = Width;
                    line.Height = Height;

                    switch (Placement)
                    {
                        case PlacementTypes.Top:
                        case PlacementTypes.Bottom:
                            position = Graphics.ValueToPixelPosition(0, Width, Minimum, Maximum, (Double)index);
                            line.X1 = position;
                            line.X2 = position;
                            line.Y1 = 0;
                            line.Y2 = Height;

                            if (count % 2 == 1)
                            {
                                Rectangle rectangle = new Rectangle();
                                if (animationEnabled)
                                {
                                    ScaleTransform scaleTransform;
                                    if (countRectangles % 2 == 0)
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                        rectangle.RenderTransformOrigin = new Point(0.5, 1);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, duration));
                                    }
                                    else
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                        rectangle.RenderTransformOrigin = new Point(0.5, 0);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, duration));
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
                            }
                            break;

                        case PlacementTypes.Left:
                        case PlacementTypes.Right:
                            position = Graphics.ValueToPixelPosition(Height, 0, Minimum, Maximum, (Double)index);
                            if (position == 0) position += this.LineThickness;
                            line.X1 = 0;
                            line.X2 = Width;
                            line.Y1 = position;
                            line.Y2 = position;
                            if (count % 2 == 1)
                            {
                                Rectangle rectangle = new Rectangle();
                                if (animationEnabled)
                                {
                                    ScaleTransform scaleTransform;
                                    if (countRectangles % 2 == 0)
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                        rectangle.RenderTransformOrigin = new Point(0, 0.5);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, duration));
                                    }
                                    else
                                    {
                                        scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                        rectangle.RenderTransformOrigin = new Point(1, 0.5);
                                        Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, duration));
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
                            }
                            break;

                    }


                    Visual.Children.Add(line);
                    prevPosition = position;
                }
                if (count % 2 == 1)
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
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5,duration));
                                }
                                else
                                {
                                    scaleTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 0 };
                                    rectangle.RenderTransformOrigin = new Point(0.5, 0);
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleY)", 0, 1, 0.5, duration));
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
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, duration));
                                }
                                else
                                {
                                    scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 1 };
                                    rectangle.RenderTransformOrigin = new Point(1, 0.5);
                                    Storyboard.Children.Add(CreateDoubleAnimation(scaleTransform, "(ScaleTransform.ScaleX)", 0, 1, 0.5, duration));
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


                }
            }

            Visual.Width = Width;
            Visual.Height = Height;

        }

        private void ApplyVisualProperty()
        {
            Visual.Opacity = this.Opacity;
        }
        /// <summary>
        /// Generates a Double animation sequence with fixed parameters
        /// </summary>
        DoubleAnimationUsingKeyFrames CreateDoubleAnimation(DependencyObject target, String property, Double from, Double to, Double begin, Double duration)
        {
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            da.BeginTime = TimeSpan.FromSeconds(begin);
#if WPF
            target.SetValue(NameProperty, target.GetType().Name + target.GetHashCode().ToString());
            Storyboard.SetTargetName(da, target.GetValue(NameProperty).ToString());

            if (NameScope.GetNameScope(this) == null)
                NameScope.SetNameScope(this, new NameScope());
            this.RegisterName((string)target.GetValue(NameProperty), target);

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
        #endregion

        #region Private Properties
        #endregion

        #region Internal Methods

        #endregion

        #region Internal Events

        #endregion

        #region Data
        private Double _maximum;
        private Double _minimum;
        private Double _width;
        private Double _height;
        private Double _dataMinimum;
        private Double _dataMaximum;

#if WPF
        private static Boolean _defaultStyleKeyApplied;            // Default Style key
#endif
        #endregion
    }
}
