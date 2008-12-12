    #if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using System.Collections.ObjectModel;

#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

#endif
using Visifire.Commons;

namespace Visifire.Charts
{
#if SL
    [System.Windows.Browser.ScriptableType]
#endif
    
    public class Axis : ObservableObject
    {   
        #region Public Methods

        public Axis()
        {
            SetDefaults();

            // Initialize list of ChartGrids list as Grids
            Grids = new ObservableCollection<ChartGrid>();

            // Initialize list of Ticks list 
            Ticks = new ObservableCollection<Ticks>();

            // Create AxisLebels element
            AxisLabels = new AxisLabels();

            Grids.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Grids_CollectionChanged);

            Ticks.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Ticks_CollectionChanged);
        }

        void Ticks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (Ticks tick in e.NewItems)
                    {
                        if (Chart != null)
                            tick.Chart = Chart;

                        tick.PropertyChanged -= tick_PropertyChanged;
                        tick.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(tick_PropertyChanged);
                    }
                }
            }

            this.FirePropertyChanged("Ticks");
        }

        void Grids_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (ChartGrid grid in e.NewItems)
                    {
                        if (Chart != null)
                            grid.Chart = Chart;

                        grid.PropertyChanged -= grid_PropertyChanged;
                        grid.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(grid_PropertyChanged);
                    }
                }
            }

            this.FirePropertyChanged("Grids");
        }

        void grid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged(e.PropertyName);
        }

        void tick_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged(e.PropertyName);
        }

        internal Double InternalAxisMinimum
        {
            get;
            set;
        }

        internal Double InternalAxisMaximum
        {
            get;
            set;
        }

        internal Double InternalInterval
        {
            get;
            set;
        }
            

        private void SetDefaults()
        {
           // AxisMaximum = Double.NaN;
           // AxisMinimum = Double.NaN;
        }

        /// <summary>
        /// Collection of grids
        /// </summary>
        public ObservableCollection<ChartGrid> Grids
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of Ticks for an axis
        /// </summary>
        public ObservableCollection<Ticks> Ticks
        {
            get;
            set;
        }

        /// <summary>
        /// Enabled property
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
            typeof(Axis),
            new PropertyMetadata(OnEnabledPropertyChanged));
            
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("Enabled");
        }
        
        /// <summary>
        /// Creates the visual element for the Axis
        /// </summary>
        public void CreateVisualObject(Chart Chart, String AxisXorY)
        {
            IsNotificationEnable = false;

            AxisLabels.IsNotificationEnable = false;
            AxisLabels.Chart = Chart;

            if (AxisXorY == "AxesX")
                AxisLabels.ApplyStyleFromTheme(Chart, "AxisXLabels");
            else if (AxisXorY == "AxesY")
                AxisLabels.ApplyStyleFromTheme(Chart, "AxisYLabels");

            // Create visual elements
            Visual = new StackPanel();
            
            ApplyVisualProperty();
            InternalStackPanel = new StackPanel();
            Visual.Background = Color;

            if (AxisXorY == "AxesX")
            {
                if (AxisOrientation == Orientation.Vertical)
                {
                    if (AxisType == AxisTypes.Primary)
                    {
                        ScrollBarElement = Chart._leftAxisScrollBar;
                    }
                    else
                    {
                        ScrollBarElement = Chart._rightAxisScrollBar;
                    }
                   
                }
                else
                {
                    if (AxisType == AxisTypes.Primary)
                    {
                        ScrollBarElement = Chart._bottomAxisScrollBar;
                    }
                    else
                    {
                        ScrollBarElement = Chart._topAxisScrollBar;
                    }
                }
            }
            else if (AxisXorY == "AxesY")
            {
                if (this.AxisOrientation == Orientation.Vertical)
                {
                    if (AxisType == AxisTypes.Primary)
                    {
                        ScrollBarElement = Chart._leftAxisScrollBar;
                    }
                    else
                    {
                        ScrollBarElement = Chart._rightAxisScrollBar;
                    }
                }
                else
                {
                    if (AxisType == AxisTypes.Primary)
                    {
                        ScrollBarElement = Chart._bottomAxisScrollBar;
                    }
                    else
                    {
                        ScrollBarElement = Chart._topAxisScrollBar;
                    }

                }
            }

           // ScrollBarElement = new ScrollBar();

            ScrollViewerElement = new ScrollViewer() { Padding = new Thickness(0), BorderThickness = new Thickness(0) };

            AxisTitleElement = new Title();
            ApplyTitleProperties();

            //Set scroll viewer parameters
            ScrollViewerElement.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ScrollViewerElement.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            // Get the minimum and maximum value dependeing on the axis representation value
            Minimum = AxisRepresentation == AxisRepresentations.AxisX ? PlotDetails.GetAxisXMinimumDataValue(this) : PlotDetails.GetAxisYMinimumDataValue(this);
            Maximum = AxisRepresentation == AxisRepresentations.AxisX ? PlotDetails.GetAxisXMaximumDataValue(this) : PlotDetails.GetAxisYMaximumDataValue(this);   

            Boolean overflowValidity = (AxisRepresentation == AxisRepresentations.AxisX);
            Boolean stackingOverride = PlotDetails.GetStacked100OverrideState();

            // Create and initialize the AxisManagers
            AxisManager = new AxisManager(Maximum, Minimum, (Boolean) StartFromZero, overflowValidity, stackingOverride);

            // Set the include zero state
            AxisManager.IncludeZero = IncludeZero;

            // if interval is greater than zero then set the interval of the axis manager
            if (Interval > 0 || !Double.IsNaN((Double)Interval))
            {
                AxisManager.Interval = (Double)Interval;
                InternalInterval = (Double)Interval;
            }

            // settings specific to axis X
            if (AxisRepresentation == AxisRepresentations.AxisX)
            {
                Double interval = GenerateDefaultInterval();
                if (interval > 0 || !Double.IsNaN(interval))
                {
                    AxisManager.Interval = interval;
                    InternalInterval = interval;
                }
            }

            // set the axis maximum value if user has provided it
            if (!Double.IsNaN((Double)AxisMaximum))
            {
                AxisManager.AxisMaximumValue = (Double)AxisMaximum;
                InternalAxisMaximum = (Double)AxisMaximum;
            }

            // set the axis minimum value if the user has provided it
            if (!Double.IsNaN((Double)AxisMinimum))
            {
                AxisManager.AxisMinimumValue = (Double)AxisMinimum;
                InternalAxisMinimum = (Double)AxisMinimum;
            }

            // Calculate the various parameters for creating the axis
            AxisManager.Calculate();

            // Set axis specific limits based on axis limits.
            if (AxisRepresentation == AxisRepresentations.AxisX && !(Boolean)StartFromZero)
                if (!SetAxesXLimits())
                    return;

            // Settings specific to axis y
            if (this.AxisRepresentation == AxisRepresentations.AxisY && this.AxisType == AxisTypes.Primary)
            {
                // Set the internal axis limits the one obtained from axis manager
                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;

            }
            else if (this.AxisRepresentation == AxisRepresentations.AxisY && this.AxisType == AxisTypes.Secondary)
            {
                var axisYPrimary = (from axis in Chart.InternalAxesY where axis.AxisRepresentation == AxisRepresentations.AxisY && axis.AxisType == AxisTypes.Primary select axis);

                Axis primaryAxisY = null;
                if (axisYPrimary.Count() > 0)
                    primaryAxisY = axisYPrimary.First();

                if (Double.IsNaN((double)Interval) && primaryAxisY != null)
                {
                    // number of interval in the primary axis

                    Double primaryAxisIntervalCount = ((double)primaryAxisY.InternalAxisMaximum - (double)primaryAxisY.InternalAxisMinimum) / (double)primaryAxisY.InternalInterval;

                    // This will set the internal overriding flag
                    AxisManager.AxisMinimumValue = AxisManager.AxisMinimumValue;
                    AxisManager.AxisMaximumValue = AxisManager.AxisMaximumValue;

                    // This interval will reflect the interval in primary axis it is not same as that of primary axis
                    AxisManager.Interval = (AxisManager.AxisMaximumValue - AxisManager.AxisMinimumValue) / primaryAxisIntervalCount;

                    AxisManager.Calculate();
                }

                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;
            }
            else
            {
                // Set the internal axis limits the one obtained from axis manager
                InternalAxisMaximum = AxisManager.AxisMaximumValue;
                InternalAxisMinimum = AxisManager.AxisMinimumValue;
            }

            InternalInterval = AxisManager.Interval;

            // set the params to create Axis Labels
            AxisLabels.Maximum = AxisManager.AxisMaximumValue;
            AxisLabels.Minimum = AxisManager.AxisMinimumValue;
            AxisLabels.DataMaximum = Maximum;
            AxisLabels.DataMinimum = Minimum;
            AxisLabels.ParentAxis = this;
            AxisLabels.Padding = this.Padding;

            if (Ticks.Count == 0)
                Ticks.Add(new Ticks());

            foreach (Ticks tick in Ticks)
            {   
                tick.IsNotificationEnable = false;

                if (AxisXorY == "AxesX")
                    tick.ApplyStyleFromTheme(Chart, "AxisXTicks");
                else if (AxisXorY == "AxesY")
                    tick.ApplyStyleFromTheme(Chart, "AxisYTicks");

                tick.Maximum = AxisManager.AxisMaximumValue;
                tick.Minimum = AxisManager.AxisMinimumValue;
                tick.DataMaximum = Maximum;
                tick.DataMinimum = Minimum;
                tick.TickLength = 5;
                tick.ParentAxis = this;

                tick.IsNotificationEnable = true;
            }

            if (Grids.Count == 0 && AxisXorY != "AxesX")
                Grids.Add(new ChartGrid());

            foreach (ChartGrid grid in Grids)
            {
                grid.IsNotificationEnable = false;

                grid.Maximum = AxisManager.AxisMaximumValue;
                grid.Minimum = AxisManager.AxisMinimumValue;
                grid.DataMaximum = Maximum;
                grid.DataMinimum = Minimum;
                grid.ParentAxis = this;

                grid.IsNotificationEnable = true;
            }

            if (AxisRepresentation == AxisRepresentations.AxisX)
            {
                if (AxisType == AxisTypes.Primary)
                {
                    AxisLabels.AxisLabelContentDictionary = PlotDetails.AxisXPrimaryLabels;
                    AxisLabels.AllAxisLabels = PlotDetails.IsAllPrimaryAxisXLabelsPresent;
                }
                else
                {
                    AxisLabels.AxisLabelContentDictionary = PlotDetails.AxisXSecondaryLabels;
                    AxisLabels.AllAxisLabels = PlotDetails.IsAllSecondaryAxisXLabelsPresent;
                }
            }

            foreach (Ticks tick in Ticks)
            {
                tick.IsNotificationEnable = false;

                tick.AllAxisLabels = AxisLabels.AllAxisLabels;
                tick.AxisLabelsDictionary = AxisLabels.AxisLabelContentDictionary;

                tick.IsNotificationEnable = true;
            }

            // set the placement order based on the axis orientation
            switch (AxisOrientation)
            {   
                case Orientation.Horizontal:
                    ApplyHorizontalAxisSettings();
                    break;

                case Orientation.Vertical:
                    ApplyVerticalAxisSettings();
                    break;
            }

            IsNotificationEnable = true;
            AxisLabels.IsNotificationEnable = true;

            if (!(Boolean)this.Enabled)
                Visual.Visibility = Visibility.Collapsed;

        }

        Double GenerateDefaultInterval()
        {
            if (AxisType == AxisTypes.Primary)
            {
                if (Double.IsNaN((Double)Interval) && Double.IsNaN((Double)AxisLabels.Interval) && PlotDetails.IsAllPrimaryAxisXLabelsPresent)
                {
                    List<Double> uniqueXValues = (from entry in PlotDetails.AxisXPrimaryLabels orderby entry.Key select entry.Key).ToList();

                    if (uniqueXValues.Count > 0)
                    {   
                        Double minDiff = Double.MaxValue;

                        for (Int32 i = 0; i < uniqueXValues.Count - 1; i++)
                        {
                            minDiff = Math.Min(minDiff, Math.Abs(uniqueXValues[i] - uniqueXValues[i + 1]));
                        }

                        if (minDiff != Double.MaxValue)
                            return minDiff;
                    }
                }
            }
            else
            {
                if (Double.IsNaN((Double)Interval) && Double.IsNaN((Double)AxisLabels.Interval) && PlotDetails.IsAllSecondaryAxisXLabelsPresent)
                {
                    List<Double> uniqueXValues = (from entry in PlotDetails.AxisXSecondaryLabels orderby entry.Key select entry.Key).ToList();

                    if (uniqueXValues.Count > 0)
                    {
                        Double minDiff = Double.MaxValue;

                        for (Int32 i = 0; i < uniqueXValues.Count - 1; i++)
                        {
                            minDiff = Math.Min(minDiff, Math.Abs(uniqueXValues[i] - uniqueXValues[i + 1]));
                        }

                        if (minDiff != Double.MaxValue)
                            return minDiff;
                    }
                }
            }

            return (Double)Interval;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Href target property
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

        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(HrefTargets),
            typeof(Axis),
            new PropertyMetadata(OnHrefTargetChanged));

        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("HrefTarget");
        }

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

        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnHrefChanged));

        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("Href");
        }


        /// <summary>
        /// Sets the color of the Axis
        /// </summary>
        public Brush Color
        {
            get
            {
                return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        private static readonly DependencyProperty ColorProperty = DependencyProperty.Register
            ("Color",
            typeof(Brush),
            typeof(Axis),
            new PropertyMetadata(OnColorPropertyChanged));

        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            //axis.FirePropertyChanged("Color");
            if(axis.Visual != null)
                axis.Visual.Background = (Brush)e.NewValue;
            else
                axis.FirePropertyChanged("Color");
        }

        /// <summary>
        /// Sets the interval for all the axis elements
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> Interval
        {
            get
            {
                if ((Nullable<Double>)GetValue(IntervalProperty) == null)
                    return Double.NaN;
                else
                    return (Nullable<Double>)GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
                _axisIntervalOverride = (Double)value;
            }
        }

        private static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(Axis),
            new PropertyMetadata(OnIntervalPropertyChanged));

        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("Interval");
        }

        #region LineProperties

        /// <summary>
        /// Sets the Color of the axis line
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
            typeof(Axis),
            new PropertyMetadata(OnLineColorPropertyChanged));

        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("LineColor");
        }

        /// <summary>
        /// Sets the thickness of the axis line
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
            typeof(Axis),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("LineThickness");
        }

        /// <summary>
        /// Sets the style of the axis line. It takes values like "Dashed", "Dotted" etc
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
            typeof(Axis),
            new PropertyMetadata(OnLineStylePropertyChanged));

        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("LineStyle");
        }

        #endregion

        #region TitleProperties

        /// <summary>
        /// Sets the title for the axis
        /// </summary>
        public String Title
        {
            get
            {
                return (String)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        private static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            ("Title",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnTitlePropertyChanged));

        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("Title");
        }

        /// <summary>
        /// Sets the font color for axis title
        /// </summary>
        public Brush TitleFontColor
        {
            get
            {
                return (Brush)GetValue(TitleFontColorProperty);
            }
            set
            {
                SetValue(TitleFontColorProperty, value);
            }
        }

        private static readonly DependencyProperty TitleFontColorProperty = DependencyProperty.Register
            ("TitleFontColor",
            typeof(Brush),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontColorPropertyChanged));

        private static void OnTitleFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("TitleFontColor");
        }

        /// <summary>
        /// Sets the font family for axis title
        /// </summary>
        public FontFamily TitleFontFamily
        {
            get
            {
                return (FontFamily)GetValue(TitleFontFamilyProperty);
            }
            set
            {
                SetValue(TitleFontFamilyProperty, value);
            }
        }

        private static readonly DependencyProperty TitleFontFamilyProperty = DependencyProperty.Register
            ("TitleFontFamily",
            typeof(FontFamily),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontFamilyPropertyChanged));

        private static void OnTitleFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("TitleFontFamily");
        }

        /// <summary>
        /// Sets the font size for axis title
        /// </summary>
        public Double TitleFontSize
        {
            get
            {
                return (Double)GetValue(TitleFontSizeProperty);
            }
            set
            {
                SetValue(TitleFontSizeProperty, value);
            }
        }

        private static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register
            ("TitleFontSize",
            typeof(Double),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontSizePropertyChanged));

        private static void OnTitleFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("TitleFontSize");
        }

        /// <summary>
        /// Sets the font style for axis title
        /// </summary>
        public FontStyle TitleFontStyle
        {
            get
            {
                return (FontStyle)GetValue(TitleFontStyleProperty);
            }
            set
            {
                SetValue(TitleFontStyleProperty, value);
            }
        }

        private static readonly DependencyProperty TitleFontStyleProperty = DependencyProperty.Register
            ("TitleFontStyle",
            typeof(FontStyle),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontStylePropertyChanged));

        private static void OnTitleFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("TitleFontStyle");
        }

        /// <summary>
        /// Sets the font weight for axis title
        /// </summary>
        public FontWeight TitleFontWeight
        {
            get
            {
                return (FontWeight)GetValue(TitleFontWeightProperty);
            }
            set
            {
                SetValue(TitleFontWeightProperty, value);
            }
        }

        private static readonly DependencyProperty TitleFontWeightProperty = DependencyProperty.Register
            ("TitleFontWeight",
            typeof(FontWeight),
            typeof(Axis),
            new PropertyMetadata(OnTitleFontWeightPropertyChanged));

        private static void OnTitleFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("TitleFontWeight");
        }

        #endregion

        /// <summary>
        /// Sets or Gets the axis type (Primary or Secondary)
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

        private static readonly DependencyProperty AxisTypeProperty = DependencyProperty.Register
            ("AxisType",
            typeof(AxisTypes),
            typeof(Axis),
            new PropertyMetadata(OnAxisTypePropertyChanged));

        private static void OnAxisTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("AxisType");
        }

        /// <summary>
        /// Returns the visual element for the Axis
        /// </summary>
        internal StackPanel Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the maximum value for the Axis
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> AxisMaximum
        {
            get
            {
                if ((Nullable<Double>)GetValue(AxisMaximumProperty) == null)
                    return Double.NaN;
                else
                    return (Nullable<Double>)GetValue(AxisMaximumProperty);
            }
            set
            {
                SetValue(AxisMaximumProperty, value);
                //if(!_isRedraw)
                //    _axisMaxOverride = (Double)value;
            }
        }

        private static readonly DependencyProperty AxisMaximumProperty = DependencyProperty.Register
            ("AxisMaximum",
            typeof(Nullable<Double>),
            typeof(Axis),
            new PropertyMetadata(OnAxisMaximumPropertyChanged));

        private static void OnAxisMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("AxisMaximum");
        }

        /// <summary>
        /// Get or set the minimum value for the axis
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> AxisMinimum
        {
            get
            {   
                if((Nullable<Double>)GetValue(AxisMinimumProperty) == null)
                    return Double.NaN;
                else
                   return (Nullable<Double>)GetValue(AxisMinimumProperty);
            }
            set
            {
                SetValue(AxisMinimumProperty, value);
                //if (!_isRedraw)
                //    _axisMinOverride = (Double)value;
            }
        }

        private static readonly DependencyProperty AxisMinimumProperty = DependencyProperty.Register
            ("AxisMinimum",
            typeof(Nullable<Double>),
            typeof(Axis),
            new PropertyMetadata(OnAxisMinimumPropertyChanged));

        private static void OnAxisMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("AxisMinimum");
        }

        /// <summary>
        /// Include zero within the axis range
        /// </summary>
        public Boolean IncludeZero
        {
            get
            {
                return (Boolean)GetValue(IncludeZeroProperty);
            }
            set
            {
                SetValue(IncludeZeroProperty, value);
            }
        }

        private static readonly DependencyProperty IncludeZeroProperty = DependencyProperty.Register
            ("IncludeZero",
            typeof(Boolean),
            typeof(Axis),
            new PropertyMetadata(OnIncludeZeroPropertyChanged));

        private static void OnIncludeZeroPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("IncludeZero");
        }

        /// <summary>
        /// Forces the axis to include zero or atleast have either Axisminimum or AxisMaximum as zero
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> StartFromZero
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(StartFromZeroProperty) == null)
                {
                    if (AxisRepresentation == AxisRepresentations.AxisY)
                        return true;
                    else
                        return false;
                }
                else
                    return (Nullable<Boolean>)GetValue(StartFromZeroProperty);
            }
            set
            {
                SetValue(StartFromZeroProperty, value);
            }
        }

        private static readonly DependencyProperty StartFromZeroProperty = DependencyProperty.Register
            ("StartFromZero",
            typeof(Nullable<Boolean>),
            typeof(Axis),
            new PropertyMetadata(OnStartFromZeroPropertyChanged));

        private static void OnStartFromZeroPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("StartFromZero");
        }

        /// <summary>
        /// Gets or sets the prefix for the axis labels in this axis
        /// </summary>
        public String Prefix
        {
            get
            {
                return (String)GetValue(PrefixProperty);
            }
            set
            {
                SetValue(PrefixProperty, value);
            }
        }

        private static readonly DependencyProperty PrefixProperty = DependencyProperty.Register
            ("Prefix",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnPrefixPropertyChanged));

        private static void OnPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("Prefix");
        }

        /// <summary>
        /// Gets or sets the suffix for the labels used in the axis
        /// </summary>
        public String Suffix
        {
            get
            {
                return (String)GetValue(SuffixProperty);
            }
            set
            {
                SetValue(SuffixProperty, value);
            }
        }

        private static readonly DependencyProperty SuffixProperty = DependencyProperty.Register
            ("Suffix",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnSuffixPropertyChanged));

        private static void OnSuffixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("Suffix");
        }

        /// <summary>
        /// Gets or sets the scaling values for the axis
        /// </summary>
        public String ScalingSet
        {
            get
            {
                return (String)GetValue(ScalingSetProperty);
            }
            set
            {
                SetValue(ScalingSetProperty, value);
                ParseScalingSets(value);
            }
        }

        private static readonly DependencyProperty ScalingSetProperty = DependencyProperty.Register
            ("ScalingSet",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnScalingSetPropertyChanged));

        private static void OnScalingSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("ScalingSet");
        }

        /// <summary>
        /// Gets or sets the format string that must be used with the axis labels
        /// </summary>
        public String ValueFormatString
        {
            get
            {
                return String.IsNullOrEmpty((String)GetValue(ValueFormatStringProperty)) ? "###,##0.##" : (String)GetValue(ValueFormatStringProperty);
            }
            set
            {
                SetValue(ValueFormatStringProperty, value);
            }
        }

        private static readonly DependencyProperty ValueFormatStringProperty = DependencyProperty.Register
            ("ValueFormatString",
            typeof(String),
            typeof(Axis),
            new PropertyMetadata(OnValueFormatStringPropertyChanged));

        private static void OnValueFormatStringPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            axis.FirePropertyChanged("ValueFormatString");
        }

        #endregion

        #region Hidden Control Properties

        private new Brush Background
        {
            get;
            set;
        }

        private new static DependencyProperty BackgroundProperty = DependencyProperty.Register
            ("Background",
            typeof(Brush),
            typeof(Axis),
            null);

        private new FontFamily FontFamily
        {
            get;
            set;
        }

        private new static DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(Axis),
            null);

        private new Double FontSize
        {
            get;
            set;
        }

        private new static DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(Axis),
            null);

        private new FontStretch FontStretch
        {
            get;
            set;
        }

        private new static DependencyProperty FontStretchProperty = DependencyProperty.Register
            ("FontStretch",
            typeof(FontStretch),
            typeof(Axis),
            null);

        private new FontStyle FontStyle
        {
            get;
            set;
        }

        private new static DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(Axis),
            null);

        private new FontWeight FontWeight
        {
            get;
            set;
        }

        private new static DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(Axis),
            null);

        private new Brush Foreground
        {
            get;
            set;
        }

        private new static DependencyProperty ForegroundProperty = DependencyProperty.Register
            ("Foreground",
            typeof(Brush),
            typeof(Axis),
            null);

        private new Thickness BorderThickness
        {
            get;
            set;
        }

        private new static DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(Axis),
            null);

        private new Brush BorderBrush
        {
            get;
            set;
        }

        private new static DependencyProperty BorderBrushProperty = DependencyProperty.Register
            ("BorderBrush",
            typeof(Brush),
            typeof(Axis),
            null);

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Element MajorGrid element
        /// </summary>
        internal ChartGrid MajorGridsElement
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the scroll bar
        /// </summary>
        internal ScrollBar ScrollBarElement
        {
            get;
            set;
        }

        /// <summary>
        /// Axis ScrollViewer element
        /// </summary>
        internal ScrollViewer ScrollViewerElement
        {
            get;
            set;
        }


        /// <summary>
        /// Sets or gets the major ticks
        /// </summary>
        internal Ticks MajorTicksElement
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the axis title element
        /// </summary>
        internal Title AxisTitleElement
        {
            get;
            set;
        }

        /// <summary>
        /// AxisManager of the Axis
        /// </summary>
        internal AxisManager AxisManager
        {
            get;
            set;
        }

        /// <summary>
        /// Scrollable size of PlotArea
        /// </summary>
        internal Double ScrollableSize
        {
            get;
            set;
        }

        /// <summary>
        /// Line for Axis
        /// </summary>
        internal Line AxisLine
        {
            get;
            set;
        }

        /// <summary>
        /// Set or Gets the axis orientation
        /// Vertical = AxisY for all types except bar
        /// Horizontal = AxesX for types except bar
        /// </summary>
        internal Orientation AxisOrientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }

        /// <summary>
        /// Axis representation
        /// </summary>
        internal AxisRepresentations AxisRepresentation
        {
            get;
            set;
        }

        /// <summary>
        /// Details about plot groups
        /// </summary>
        internal PlotDetails PlotDetails
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the width of the axis
        /// </summary>
        internal new Double Width
        {
            get;
            set;
        }

        /// <summary>
        /// sets or gets the height of the axis
        /// </summary>
        internal new Double Height
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the data maximum for the axis
        /// </summary>
#if DEBUG
        public Double Maximum
#else
        internal Double Maximum
#endif
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the data minimum for the axis
        /// </summary>
#if DEBUG
        public Double Minimum
#else
        internal Double Minimum
#endif
        {
            get;
            set;
        }

        internal List<Double> ScaleValues
        {
            get
            {
                return _scaleValues;
            }
        }

        internal List<String> ScaleUnits
        {
            get
            {
                return _scaleUnits;
            }
        }

        internal Double StartOffset
        {
            get;
            set;
        }

        internal Double EndOffset
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Applies axis settings for horizontal type axis
        /// </summary>
#if DEBUG
        internal void ApplyHorizontalAxisSettings()
#else
        private void ApplyHorizontalAxisSettings()
#endif
        {
            // Apply  settings based on the axis type
            switch (AxisType)
            {
                case AxisTypes.Primary:
                    ApplyHorizontalPrimaryAxisSettings();
                    break;
                case AxisTypes.Secondary:
                    ApplyHorizontalSecondaryAxisSettings();
                    break;
            }
        }

        /// <summary>
        /// Applies setting for vertical type axis
        /// </summary>
        private void ApplyVerticalAxisSettings()
        {
            // Apply  settings based on the axis type
            switch (AxisType)
            {
                case AxisTypes.Primary:
                    ApplyVerticalPrimaryAxisSettings();
                    break;
                case AxisTypes.Secondary:
                    ApplyVerticalSecondaryAxisSettings();
                    break;
            }
        }

        /// <summary>
        /// Applies setting for primary vertical axis (Primary axis Y or Primary axis X in Bar)
        /// </summary>
        private void ApplyVerticalPrimaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.HorizontalAlignment = HorizontalAlignment.Left;
            Visual.VerticalAlignment = VerticalAlignment.Stretch;
            Visual.Orientation = Orientation.Horizontal;

            //Visual.Background = new SolidColorBrush(Colors.Red);

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Stretch;
            InternalStackPanel.Orientation = Orientation.Horizontal;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Width = e.NewSize.Width;
            };

            ScrollViewerElement.VerticalAlignment = VerticalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Vertical;
            ScrollBarElement.Width = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Left;
            AxisLabels.Height = ScrollableSize;

            // Set axis line parameters
            AxisLine = new Line() { Y1 = StartOffset, Y2 = Height - EndOffset, X1 = LineThickness / 2, X2 = LineThickness / 2, Width = LineThickness, Height = this.Height };
            AxisLine.StrokeThickness = LineThickness;
            AxisLine.Stroke = LineColor;
            AxisLine.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            // Set the parameters for the major ticks
            foreach (Ticks tick in Ticks)
            {
                tick.Placement = PlacementTypes.Left;
                tick.Height = ScrollableSize;
            }

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Left;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Left;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Center;

            
            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();
            
            //MajorTicksElement.CreateVisualObject();

UP:

            AxisTitleElement.Margin = new Thickness(4);
            AxisTitleElement.CreateVisualObject();

            if (AxisTitleElement.TextBlockDesiredSize.Width > Height && Height != 0)
            {
                if (AxisTitleElement.FontSize == 4)
                    goto DOWN;
                AxisTitleElement.IsNotificationEnable = false;
                AxisTitleElement.FontSize -= 1;
                AxisTitleElement.IsNotificationEnable = true;
                goto UP;
            }

DOWN:

            // Place the visual elements in the axis stack panel
            if (!String.IsNullOrEmpty(Title))
                Visual.Children.Add(AxisTitleElement.Visual);
            
            if (AxisLabels.Visual != null)
            {
                if (Height == ScrollableSize)
                {
                    if (AxisLabels.Visual != null)
                        Visual.Children.Add(AxisLabels.Visual);
                }
                else
                {
                    InternalStackPanel.Children.Add(AxisLabels.Visual);
                }
            }

            foreach (Ticks tick in Ticks)
            {
                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Height == ScrollableSize)
                        Visual.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);
                }
            }

            if (Height != ScrollableSize)
            {
                ScrollViewerElement.Content = InternalStackPanel;
                Visual.Children.Add(ScrollViewerElement);
            }

            //  Visual.Children.Add(ScrollBarElement);
            Visual.Children.Add(AxisLine);

        }
        internal Int32 SkipOfset
        {
            get;
            set;
        }
        private void ApplyTitleProperties()
        {
            #region Apply AxisTitle Properties

            if (this.TitleFontFamily != null)
                AxisTitleElement.FontFamily = this.TitleFontFamily;

            if (this.TitleFontSize != 0)
                AxisTitleElement.FontSize = this.TitleFontSize;

            if (this.TitleFontStyle != null)
                AxisTitleElement.FontStyle = this.TitleFontStyle;

            if (this.TitleFontWeight != null)
                AxisTitleElement.FontWeight = this.TitleFontWeight;

            if (!String.IsNullOrEmpty(this.Title) && String.IsNullOrEmpty(AxisTitleElement.Text))
                AxisTitleElement.Text = this.Title;

            //if (this.TitleFontColor != null)
            AxisTitleElement.FontColor = Graphics.ApplyAutoFontColor((Chart as Chart), this.TitleFontColor, false);

            #endregion
        }
        
        /// <summary>
        /// Applies setting for secondary vertical axis (Secondary axis Y or Secondary axis X in Bar)
        /// </summary>
        private void ApplyVerticalSecondaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.HorizontalAlignment = HorizontalAlignment.Right;
            Visual.VerticalAlignment = VerticalAlignment.Stretch;
            Visual.Orientation = Orientation.Horizontal;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Right;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Stretch;
            InternalStackPanel.Orientation = Orientation.Horizontal;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Width = e.NewSize.Width;
            };

            ScrollViewerElement.VerticalAlignment = VerticalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Vertical;
            ScrollBarElement.Width = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Right;
            AxisLabels.Height = ScrollableSize;

            // Set axis line parameters
            AxisLine = new Line() { Y1 = StartOffset, Y2 = Height - EndOffset, X1 = LineThickness / 2, X2 = LineThickness / 2, Width = LineThickness, Height = this.Height };
            AxisLine.StrokeThickness = LineThickness;
            AxisLine.Stroke = LineColor;
            AxisLine.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            // Set the parameters for the major ticks
            foreach (Ticks tick in Ticks)
            {
                tick.Placement = PlacementTypes.Right;
                tick.Height = ScrollableSize;
            }

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Right;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Right;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Center;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();
                        
            // Place the visual elements in the axis stack panel
            Visual.Children.Add(AxisLine);
            
            foreach (Ticks tick in Ticks)
            {
                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Height == ScrollableSize)
                        Visual.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);
                }
            }

            if (Height == ScrollableSize)
            {
                if (AxisLabels.Visual != null)
                    Visual.Children.Add(AxisLabels.Visual);
            }
            else
            {
                InternalStackPanel.Children.Add(AxisLabels.Visual);

                ScrollViewerElement.Content = InternalStackPanel;

                Visual.Children.Add(ScrollViewerElement);
            }

        UP2:

            AxisTitleElement.Margin = new Thickness(4);
            AxisTitleElement.CreateVisualObject();

            if (AxisTitleElement.TextBlockDesiredSize.Width > Height && Height != 0)
            {
                if (AxisTitleElement.FontSize == 4)
                    goto DOWN2;
                AxisTitleElement.IsNotificationEnable = false;
                AxisTitleElement.FontSize -= 1;
                AxisTitleElement.IsNotificationEnable = true;
                goto UP2;
            }

        DOWN2:

            if (!String.IsNullOrEmpty(Title))
                Visual.Children.Add(AxisTitleElement.Visual);
        }

        /// <summary>
        /// Applies setting for primary horizontal axis (Primary axis X or Primary axis Y in Bar)
        /// </summary>
        private void ApplyHorizontalPrimaryAxisSettings()
        {   
            // Set the parameters fo the Axis Stack panel
            Visual.HorizontalAlignment = HorizontalAlignment.Stretch;
            Visual.VerticalAlignment = VerticalAlignment.Bottom;
            Visual.Orientation = Orientation.Vertical;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            InternalStackPanel.Orientation = Orientation.Vertical;

            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Height = Math.Max(e.NewSize.Height, InternalStackPanel.ActualHeight);
            };

            ScrollViewerElement.HorizontalAlignment = HorizontalAlignment.Stretch;
            
            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Horizontal;
            ScrollBarElement.Height = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Bottom;
            AxisLabels.Width = ScrollableSize;

            // Set axis line parameters
            AxisLine = new Line() { X1 = StartOffset, X2 = Width - EndOffset, Y1 = LineThickness / 2, Y2 = LineThickness / 2, Width = this.Width, Height = LineThickness };
            AxisLine.StrokeThickness = LineThickness;
            AxisLine.Stroke = LineColor;
            AxisLine.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            // Set the parameters for the major ticks
            foreach (Ticks tick in Ticks)
            {
                tick.Placement = PlacementTypes.Bottom;
                tick.Width = ScrollableSize;
            }

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Bottom;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Center;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Bottom;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();
            //MajorTicksElement.CreateVisualObject();

            //AxisTitleElement.Margin = new Thickness(4);
            //AxisTitleElement.CreateVisualObject();

            // Place the visual elements in the axis stack panel
            Visual.Children.Add(AxisLine);
           // Visual.Children.Add(ScrollBarElement);

            foreach (Ticks tick in Ticks)
            {
                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if(Width == ScrollableSize)
                        Visual.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);
                }
            }

            if (Width == ScrollableSize)
            {
                if(AxisLabels.Visual != null)
                    Visual.Children.Add(AxisLabels.Visual);
            }
            else
            {   
                if(AxisLabels.Visual != null)
                    InternalStackPanel.Children.Add(AxisLabels.Visual);

                ScrollViewerElement.Content = InternalStackPanel;
                Visual.Children.Add(ScrollViewerElement);
            }

        UPX1:

            AxisTitleElement.Margin = new Thickness(4);
            AxisTitleElement.CreateVisualObject();

            if (AxisTitleElement.TextBlockDesiredSize.Width > Width && Width != 0)
            {
                if (AxisTitleElement.FontSize == 4)
                    goto DOWNX1;
                AxisTitleElement.IsNotificationEnable = false;
                AxisTitleElement.FontSize -= 1;
                AxisTitleElement.IsNotificationEnable = true;
                goto UPX1;
            }

        DOWNX1:

            if(!String.IsNullOrEmpty(Title))
                Visual.Children.Add(AxisTitleElement.Visual);

        }

        /// <summary>
        /// Applies setting for secondary horizontal axis (Secondary axis X or Secondary axis Y in Bar)
        /// </summary>
        private void ApplyHorizontalSecondaryAxisSettings()
        {
            // Set the parameters fo the Axis Stack panel
            Visual.HorizontalAlignment = HorizontalAlignment.Stretch;
            Visual.VerticalAlignment = VerticalAlignment.Top;
            Visual.Orientation = Orientation.Vertical;

            InternalStackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            InternalStackPanel.VerticalAlignment = VerticalAlignment.Top;
            InternalStackPanel.Orientation = Orientation.Vertical;


            InternalStackPanel.SizeChanged += delegate(object sender, SizeChangedEventArgs e)
            {
                ScrollViewerElement.Height = e.NewSize.Height;
            };

            ScrollViewerElement.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Set the parameters for the scroll bar
            ScrollBarElement.Orientation = Orientation.Horizontal;
            ScrollBarElement.Height = 10;

            // Set the parameters for the axis labels
            AxisLabels.Placement = PlacementTypes.Top;
            AxisLabels.Width = ScrollableSize;

            // Set axis line parameters
            AxisLine = new Line() { X1 = StartOffset, X2 = Width-EndOffset, Y1 = LineThickness / 2, Y2 = LineThickness / 2, Width = this.Width, Height = LineThickness };
            AxisLine.StrokeThickness = LineThickness;
            AxisLine.Stroke = LineColor;
            AxisLine.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            // Set the parameters for the major ticks
            foreach (Ticks tick in Ticks)
            {
                tick.Placement = PlacementTypes.Top;
                tick.Width = ScrollableSize;
            }

            // Set parameters for the Major Grids
            foreach (ChartGrid grid in Grids)
                grid.Placement = PlacementTypes.Top;

            // Set the alignement for the axis Title
            AxisTitleElement.HorizontalAlignment = HorizontalAlignment.Center;
            AxisTitleElement.VerticalAlignment = VerticalAlignment.Top;

            // Generate the visual object for the required elements
            AxisLabels.CreateVisualObject();

            //MajorTicksElement.CreateVisualObject();

            AxisTitleElement.Margin = new Thickness(4);
            AxisTitleElement.CreateVisualObject();
            
            // Place the visual elements in the axis stack panel
            if (!String.IsNullOrEmpty(Title))
                Visual.Children.Add(AxisTitleElement.Visual);

            if (AxisLabels.Visual != null)
            {
                if (Width == ScrollableSize)
                    Visual.Children.Add(AxisLabels.Visual);
                else
                    InternalStackPanel.Children.Add(AxisLabels.Visual);
            }

            foreach (Ticks tick in Ticks)
            {
                tick.CreateVisualObject();
                if (tick.Visual != null)
                {
                    if (Width == ScrollableSize)
                        Visual.Children.Add(tick.Visual);
                    else
                        InternalStackPanel.Children.Add(tick.Visual);
                }
            }

            if (Width != ScrollableSize)
            {
                ScrollViewerElement.Content = InternalStackPanel;

                Visual.Children.Add(ScrollViewerElement);
            }

            Visual.Children.Add(AxisLine);
        }

        /// <summary>
        /// Sets the axis limits considering the widths of the columns that will be drawn in the chart
        /// </summary>
        private bool SetAxesXLimits()
        {
            if (PlotDetails.DrawingDivisionFactor > 0)
            {
                if (!SetAxisLimitForMinimumGap())
                    return false;
            }

            MatchLeftAndRightGaps();
            return true;
        }

        /// <summary>
        /// Sets the limits such that the gap between plot area and the Columns will be minimum
        /// </summary>
        private bool SetAxisLimitForMinimumGap()
        {
            Double minimumDifference = PlotDetails.GetMaxOfMinDifferencesForXValue();
            Double gap;
            Double minValue;

            if (Double.IsNaN(minimumDifference) || minimumDifference <= 0)
            {
                minValue = AxisManager.Interval;
            }
            else
            {
                minValue = ((minimumDifference < AxisManager.Interval) ? minimumDifference : AxisManager.Interval);
            }

            Double dataAxisDifference = Math.Abs(AxisManager.AxisMinimumValue - Minimum) * 2;
            Double dataMinimumGap = 0;

            if (PlotDetails.ChartOrientation == ChartOrientationType.Vertical)
            {
                gap = Graphics.ValueToPixelPosition(0, Width, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, minValue + AxisManager.AxisMinimumValue) / PlotDetails.DrawingDivisionFactor;
                dataMinimumGap = Graphics.ValueToPixelPosition(0, Width, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, dataAxisDifference + AxisManager.AxisMinimumValue);
            }
            else
            {
                gap = Graphics.ValueToPixelPosition(0, Height, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, minValue + AxisManager.AxisMinimumValue) / PlotDetails.DrawingDivisionFactor;
                dataMinimumGap = Graphics.ValueToPixelPosition(0, Height, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, dataAxisDifference + AxisManager.AxisMinimumValue);
            }

            gap = Math.Min(Math.Abs(gap), Math.Abs(dataMinimumGap)) * 1.1 * PlotDetails.DrawingDivisionFactor / 2;

            if (gap == 0)
                if (dataAxisDifference != 0)
                    return false;
            
            if (AxisOrientation == Orientation.Horizontal)
            {
                if (Double.IsNaN((Double)AxisMinimum))
                {
                    Double value = Graphics.ValueToPixelPosition(0, Width, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, Minimum);
                    AxisManager.AxisMinimumValue = Graphics.PixelPositionToValue(0, Width, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, value - gap);
                }
                else
                {
                    AxisManager.AxisMinimumValue = (Double)AxisMinimum;
                }
            }
            else
            {
                if (Double.IsNaN((Double)AxisMinimum))
                {
                    Double value = Graphics.ValueToPixelPosition(0, Height, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, Minimum);
                    AxisManager.AxisMinimumValue = Graphics.PixelPositionToValue(0, Height, AxisManager.AxisMinimumValue, AxisManager.AxisMaximumValue, value - gap);
                }
                else
                {
                    AxisManager.AxisMinimumValue = (Double)AxisMinimum;
                }
            }
            return true;
        }

        private void MatchLeftAndRightGaps()
        {
            if (Double.IsNaN((Double)AxisMaximum))
            {
                if ((AxisManager.AxisMaximumValue - Maximum) >= (Minimum - AxisManager.AxisMinimumValue))
                {
                    // This part makes the gaps equal
                    AxisManager.AxisMaximumValue = Maximum + Minimum - AxisManager.AxisMinimumValue;
                }
            }
            else
            {
                AxisManager.AxisMaximumValue = (Double)AxisMaximum;
            }
        }

        /// <summary>
        /// Convert scaling sets from string to unit and value array
        /// </summary>
        /// <param name="scalingSets"></param>
        private void ParseScalingSets(String scalingSets)
        {
            if (String.IsNullOrEmpty(scalingSets)) return;

            // scaling sets are available in the form of value,unit;value,unit;value,unit;
            String[] pairs = scalingSets.Split(';');

            // Since scale has to be successively multiplied initialize it to 1
            Double scale = 1;

            // variable to store the parsed double value
            Double parsedValue;

            _scaleUnits = new List<String>();
            _scaleValues = new List<Double>();

            for (Int32 i = 0; i < pairs.Length; i++)
            {
                // split the individual pairs available in "value,unit" form
                String[] sets = pairs[i].Split(',');

                // if either of value or unit is missing then throw this exception
                if (sets.Length != 2)
                    throw new Exception("Invalid scaling set parameters. should be of the form value,unit;value,unit;...");

                // parse the value part of the string as double
                parsedValue = Double.Parse(sets[0], CultureInfo.InvariantCulture);

                // multiply the scale with the parsed value and store it
                scale *= parsedValue;
                _scaleValues.Add(scale);

                // store the unit in the units list
                _scaleUnits.Add(sets[1]);
            }
        }

        private void ApplyVisualProperty()
        {
            Visual.Cursor = (Cursor == null) ? Cursors.Arrow : Cursor;
            AttachHref(Chart, Visual, Href, HrefTarget);
            AttachToolTip(Chart, Visual, ToolTipText);
            AttachEvents2Visual(this, this.Visual);
            Visual.Opacity = this.Opacity;
            
        }
        
        #endregion

        #region Private Properties
        /// <summary>
        /// Sets or Gets the AxisLabels
        /// </summary>

        public AxisLabels AxisLabels
        {
            get
            {
                return (AxisLabels)GetValue(AxisLabelsProperty);
            }
            set
            {   
                SetValue(AxisLabelsProperty, value);
            }
        }

        public static DependencyProperty AxisLabelsProperty = DependencyProperty.Register
            ("AxisLabels",
            typeof(AxisLabels),
            typeof(Axis),
            new PropertyMetadata(OnAxisLabelsPropertyChanged));

        private static void OnAxisLabelsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Axis axis = d as Axis;
            
            if(axis.Chart != null)
                axis.AxisLabels.Chart = axis.Chart;

            axis.AxisLabels.Parent = axis;
            axis.AxisLabels.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(AxisLabels_PropertyChanged);
            axis.FirePropertyChanged("AxisLabels");
        }

        static void AxisLabels_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (sender as AxisLabels).Parent.FirePropertyChanged(e.PropertyName);
        }

        private StackPanel InternalStackPanel
        {
            get;
            set;
        }

        #endregion

        #region Internal Methods
        internal String GetFormattedString(Double value)
        {
            String str = value.ToString();
            if (ScaleValues != null && ScaleUnits != null)
            {
                String sUnit = ScaleUnits[0];
                Double sValue = ScaleValues[0];
                for (Int32 i = 0; i < ScaleValues.Count; i++)
                {
                    if ((Math.Abs(value) / ScaleValues[i]) < 1)
                    {
                        break;
                    }
                    sValue = ScaleValues[i];
                    sUnit = ScaleUnits[i];
                }
                str = Prefix + (value / sValue).ToString(ValueFormatString) + sUnit + Suffix;
            }
            else
            {
                str = value.ToString(ValueFormatString);
                str = Prefix + str + Suffix;
            }
            return str;
        }
        #endregion

        #region Internal Events

        #endregion

        #region Data

        private Orientation _orientation;           // To store the orientation (AxesX or AxisY)
        private List<String> _scaleUnits;           // To store the units extracted from the scaling sets
        private List<Double> _scaleValues;          // To store the values extracted from the scaling sets
        private Double _axisIntervalOverride;       // To check if user has set the axis interval or not 

        #endregion
    }
}
