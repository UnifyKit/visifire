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
using System.Diagnostics;
#endif
using Visifire.Commons;

namespace Visifire.Charts
{

    public class Ticks : ObservableObject
    {

        #region Public Methods

        public Ticks()
        {
            SetDefaults();
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Ticks), new FrameworkPropertyMetadata(typeof(Ticks)));
                _defaultStyleKeyApplied = true;
            }
            
            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);

#else
            DefaultStyleKey = typeof(Ticks);
#endif
        }

        private void SetDefaults()
        {
            //if(LineColor == null)
            //    LineColor = new SolidColorBrush(Colors.LightGray);
            
            //LineThickness = 1;
            //LineStyle = LineStyles.Solid;
            //Interval = Double.NaN;
            //Enabled = true;
        }

        /// <summary>
        /// Creates the visual element for the Major ticks
        /// </summary>
        public void CreateVisualObject()
        {
            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }
            Visual = new Canvas();
            ApplyVisualProperty();
            CreateAndPositionMajorTicks();

        }
        #endregion

        #region Public Properties

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

        private static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(Ticks),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks tick = d as Ticks;
            tick.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// Major Tick interval
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

        private static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(Ticks),
            new PropertyMetadata(OnIntervalPropertyChanged));

        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged("Interval");
        }

        /// <summary>
        /// Major Tick LineColor
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

        private static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
            ("LineColor",
            typeof(Brush),
            typeof(Ticks),
            new PropertyMetadata(OnLineColorPropertyChanged));

        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged("LineColor");
        }

        /// <summary>
        /// Major Tick LineThickness
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

        private static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Double),
            typeof(Ticks),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged("LineThickness");
        }

        /// <summary>
        /// Major Tick LineStyle
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
            typeof(Ticks),
            new PropertyMetadata(OnLineStylePropertyChanged));

        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged("LineStyle");
        }

        /// <summary>
        /// Length of the ticks
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

        private static readonly DependencyProperty TickLengthProperty = DependencyProperty.Register
            ("TickLength",
            typeof(Double),
            typeof(Ticks),
            new PropertyMetadata(OnTickLengthPropertyChanged));

        private static void OnTickLengthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Ticks ticks = d as Ticks;
            ticks.FirePropertyChanged("TickLength");
        }

        /// <summary>
        /// Visual element for major ticks
        /// </summary>
        public Canvas Visual
        {
            get;
            private set;
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
        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private void ApplyVisualProperty()
        {
            Visual.Opacity = this.Opacity;
        }

        /// <summary>
        /// Creates the major ticks and also positions them appropriately
        /// </summary>
        private void CreateAndPositionMajorTicks()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Calculate interval
            Double interval =(Double) Interval;

            Decimal index = (Decimal)Minimum;
            Decimal minval = (Decimal)Minimum;
            Decimal maxVal = (Decimal)Maximum;
            Decimal gap = (Decimal)interval + +(((Nullable<Double>)GetValue(IntervalProperty) == null) ? ParentAxis.SkipOfset : 0);
            Int32 count = 0;
            Double position;

            if ((DataMinimum - interval) < Minimum && ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
                index = (Decimal)DataMinimum;

            minval = index;
            maxVal = maxVal + gap / 1000;
            if (minval != maxVal)
            {
                for (; index <= maxVal; index = minval + (++count) * gap)
                {
                    Line line = new Line();

                    line.Stroke = LineColor;
                    line.StrokeThickness = LineThickness;
                    line.StrokeDashArray = GetDashArray(LineStyle);

                    switch (Placement)
                    {
                    case PlacementTypes.Top:
                        position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, (Double)index);

                        if(Double.IsNaN(position))
                            return;

                        line.X1 = position;
                        line.X2 = position;
                        line.Y1 = 0;
                        line.Y2 = TickLength;
                        break;

                    case PlacementTypes.Bottom:
                        position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, (Double)index);

                        if (Double.IsNaN(position))
                            return;

                        line.X1 = position;
                        line.X2 = position;
                        line.Y1 = 0;
                        line.Y2 = TickLength;
                        break;

                    case PlacementTypes.Left:
                    case PlacementTypes.Right:
                        position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, (Double)index);

                        if (Double.IsNaN(position))
                            return;

                        line.X1 = 0;
                        line.X2 = TickLength;
                        line.Y1 = position;
                        line.Y2 = position;
                        break;

                    }

                    Visual.Children.Add(line);

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
        private static Boolean _defaultStyleKeyApplied = false;
#endif

        #endregion
    }
}
