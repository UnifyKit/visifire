#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.ComponentModel;


#else

using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

#endif
using System.Windows.Data;
using Visifire.Commons;

namespace Visifire.Charts
{
    public class AxisLabels : ObservableObject
    {

        #region Public Methods
        public AxisLabels()
        {

#if SL
            Binding binding = new Binding("FontFamily");
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            base.SetBinding(FontFamilyProperty, binding);
#else
            Binding binding = new Binding("InternalFontStyle");
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            SetBinding(FontStyleProperty, binding);

            binding = new Binding("InternalFontWeight");
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            SetBinding(FontWeightProperty, binding);
#endif

#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(AxisLabels), new FrameworkPropertyMetadata(typeof(AxisLabels)));
                _defaultStyleKeyApplied = true;

            }

            //object dsp = this.GetValue(FrameworkElement.DefaultStyleKeyProperty);
            //Style = (Style)Application.Current.FindResource(dsp);

#else
            DefaultStyleKey = typeof(AxisLabels);
#endif

        }

        /// <summary>
        /// Creates a visual object for the element
        /// </summary>
        public void CreateVisualObject()
        {
            if (!(Boolean)Enabled)
            {
                return;
            }

            // Create a new 
            Visual = new Canvas();
            
            // Create new Labels list
            AxisLabelList = new List<AxisLabel>();

            // List to store the values for which the labels are created
            LabelValues = new List<Double>();

            if (FontSize > _savedFontSize || Angle != _savedAngle || Rows != _savedRows)
                _isRedraw = false;
            
            // check if this is a first time draw or a redraw
            if (_isRedraw)
            {
                // if redraw then restore the original values
                Angle = _savedAngle;
                FontSize = _savedFontSize;
                Rows = _savedRows;
            }
            else
            {
                // Preserve the original values for future use
                _savedAngle = (Double)Angle;
                _savedFontSize = FontSize;
                _savedRows = (Int32)Rows;
                _isRedraw = true;
            }

            // create the required labels
            CreateLabels();

            // Set the position of the labels
            SetLabelPosition();

        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Axis Labels interval
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
            }
        }

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(AxisLabels),
            new PropertyMetadata(OnIntervalPropertyChanged));

        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Interval");
        }

        /// <summary>
        /// Sets or Gets the axis label angle
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> Angle
        {
            get
            {
                if ((Nullable<Double>)GetValue(AngleProperty) == null)
                    return Double.NaN;
                else
                    return (Nullable<Double>)GetValue(AngleProperty);
            }
            set
            {
                SetValue(AngleProperty, value);
            }
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register
            ("Angle",
            typeof(Nullable<Double>),
            typeof(AxisLabels),
            new PropertyMetadata(OnAnglePropertyChanged));

        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Angle");
        }

        /// <summary>
        /// Enables or disables AxisLabels 
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


        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(AxisLabels),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Enabled");
        }

        /// <summary>
        /// Visual element for axis labels
        /// </summary>
        public Canvas Visual
        {
            get;
#if DEBUG
            internal set;
#else
            private set;
#endif
        }

        // [System.ComponentModel.TypeConverter(typeof(System.Windows.Media.FontFamilyConverter))]
        public new FontFamily FontFamily
        {
            get
            {
                if ((FontFamily)GetValue(FontFamilyProperty) == null)
                    return new FontFamily("Arial");
                else
                    return (FontFamily)GetValue(FontFamilyProperty);
            }
            set
            {

#if SL
                if (FontFamily != value)
                {
                    SetValue(FontFamilyProperty, value);
                    FirePropertyChanged("FontFamily");
                }
#else
                SetValue(FontFamilyProperty, value);
#endif
            }
        }

#if WPF
        private new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("FontFamily");
        }
#endif

        /// <summary>
        /// Set the Color for the text in AxisLabels 
        /// </summary>
        public Brush FontColor
        {
            get
            {
                //return ((Brush)GetValue(FontColorProperty) != null)?(Brush)GetValue(FontColorProperty): new SolidColorBrush(Colors.Black);
                return (Brush)GetValue(FontColorProperty);
            }
            set
            {
                SetValue(FontColorProperty, value);
            }
        }

        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontColorPropertyChanged));

        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            //axisLabels.FirePropertyChanged("FontColor");
            axisLabels.UpdateVisual("FontColor", e.NewValue);
        }

        /// <summary>
        /// Set the Styles for the text like "Italic" or "Normal"
        /// </summary>
#if SL
        
        public new FontStyle FontStyle
        {
            get
            {
                return (FontStyle)(GetValue(FontStyleProperty));
            }
            set
            {
                if (FontStyle != value)
                {
                    SetValue(FontStyleProperty, value);
                    //FirePropertyChanged("FontStyle");
                    UpdateVisual("FontStyle", value);
                }
            }
        }
#else

        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
        internal FontStyle InternalFontStyle
        {
            get
            {
                return (FontStyle)(GetValue(InternalFontStyleProperty));
            }
            set
            {
                SetValue(InternalFontStyleProperty, value);
            }
        }

        private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
            ("InternalFontStyle",
            typeof(FontStyle),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontStylePropertyChanged));

        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            //axisLabels.FirePropertyChanged("FontStyle");
            axisLabels.UpdateVisual("FontStyle", e.NewValue);
        }
#endif

        /// <summary>
        /// Sets how the font appears. It takes values like "Bold", "Normal", "Black" etc
        /// </summary>
#if SL
       
        public new FontWeight FontWeight
        {
            get
            {
                return (FontWeight)(GetValue(FontWeightProperty));
            }
            set
            {
                if (FontWeight != value)
                {
                    SetValue(FontWeightProperty, value);
                    // FirePropertyChanged("FontWeight");
                    UpdateVisual("FontWeight", value);
                }
            }
        }

#else   
        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
        internal FontWeight InternalFontWeight
        {   
            get
            {
                return (FontWeight)(GetValue(InternalFontWeightProperty));
            }
            set
            {
                SetValue(InternalFontWeightProperty, value);
            }
        }

        private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
            ("InternalFontWeight",
            typeof(FontWeight),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
             AxisLabels axisLabels = d as AxisLabels;
             // axisLabels.FirePropertyChanged("FontWeight");
             axisLabels.UpdateVisual("FontWeight", e.NewValue);
        }
#endif

        public new Double FontSize
        {
            get
            {
                return (Double)GetValue(FontSizeProperty);
            }
            set
            {

#if SL
                if (FontSize != value)
                {
                    SetValue(FontSizeProperty, value);
                    FirePropertyChanged("FontSize");
                }
#else
                SetValue(FontSizeProperty, value);
#endif
            }
        }

#if WPF
        private new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontSizePropertyChanged));
            
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("FontSize");
        }
#endif

        /// <summary>
        /// Sets the maximum width of the labels relative to chart size
        /// </summary>
        internal TextWrapping TextWrap
        {
            get
            {
                return (TextWrapping)GetValue(TextWrapProperty);
            }
            set
            {
                SetValue(TextWrapProperty, value);
            }
        }

        public static readonly DependencyProperty TextWrapProperty = DependencyProperty.Register
            ("TextWrap",
            typeof(TextWrapping),
            typeof(AxisLabels),
            new PropertyMetadata(OnTextWrapPropertyChanged));

        private static void OnTextWrapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("TextWrap");
        }

        /// <summary>
        /// Number of rows of the axis labels
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableInt32Converter))]
#endif
        public Nullable<Int32> Rows
        {
            get
            {
                return ((Nullable<Int32>)GetValue(RowsProperty) == null) ? 0 : ((Nullable<Int32>)GetValue(RowsProperty));
            }
            set
            {
                SetValue(RowsProperty, value);
            }
        }

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register
            ("Rows",
            typeof(Nullable<Int32>),
            typeof(AxisLabels),
            new PropertyMetadata(OnRowsPropertyChanged));

        private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Rows");
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
        /// Set the width of the axis labels canvas. will be used only with the Horizontal axis
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
        /// Set the height of the axis labels canvas. will be used ony with the vertical axis 
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
        /// Placement decides how the labels have to be positioned 
        /// </summary>
        internal PlacementTypes Placement
        {
            get;
            set;
        }

        /// <summary>
        /// Reference to the dictionary in the PlotDetails that contains the required labels
        /// </summary>
        internal Dictionary<Double, String> AxisLabelContentDictionary
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
        /// Reference to the axis which holds this AxisLabels
        /// </summary>
        internal Axis ParentAxis
        {
            get;
            set;
        }

        /// <summary>
        /// Number of pixels by which the top of the axislabels has overshot the actual canvas top
        /// </summary>
        internal Double TopOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of pixels by which the bottom of the axislabels has overshot the actual canvas bottom
        /// </summary>
        internal Double BottomOverflow
        {
            get;
            private set;
        }
        /// <summary>
        /// Number of pixels by which the left of the axislabels has overshot the actual canvas left
        /// </summary>
        internal Double LeftOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of pixels by which the right of the axislabels has overshot the actual canvas right
        /// </summary>
        internal Double RightOverflow
        {
            get;
            private set;
        }

        /// <summary>
        /// Parent of DataPoints 
        /// </summary>
        public new Axis Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                System.Diagnostics.Debug.Assert(typeof(Axis).Equals(value.GetType()), "Unknown Parent", "DataPoint should have DataSeries as Parent");
                _parent = value;
            }
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private AxisLabel CreateLabel(String text)
        {
            AxisLabel label = new AxisLabel();
            label.Text = text;
            label.Placement = this.Placement;
            return label;
        }
        
        public Double CalculateAutoInterval(Double CurrentInterval, Double AxisWidth, Int32 NoOfLabels, Double Angle, Int32 Rows)
        {
            Double retVal = 1;
            Angle = Double.IsNaN(Angle)? 0 :Angle;
            CalculateHorizontalOverflow();

            return retVal;
        }

        /// <summary>
        /// Creates a set of labels
        /// </summary>
        private Boolean CreateLabels()
        {
            // Calculate interval
            Double interval = (Double)Interval;

            if (Double.IsNaN(interval) || interval <= 0)
                interval = ParentAxis.InternalInterval;
            
            // Set the begining of the axis labels same as that of the Axis Minimum.
            Decimal index = (Decimal)Minimum;

            // initialize the loop begin and end parameters
            Decimal minval = (Decimal)Minimum;
            Decimal maxVal = (Decimal)Maximum;

            // initialize the loop index increment values
            Decimal gap = (Decimal)interval;
            Int32 count = 0;

            // if the axis labels belong to axis x
            if (ParentAxis.AxisRepresentation == AxisRepresentations.AxisX)
            {   
                // if the data minimum - interval is less than the actual minimum
                if ((DataMinimum - interval) < Minimum)
                    index = (Decimal)DataMinimum;
                    
                if (AllAxisLabels && AxisLabelContentDictionary.Count > 0)
                {   
                    Dictionary<Double, String>.Enumerator enumerator = AxisLabelContentDictionary.GetEnumerator();
                    enumerator.MoveNext();

                    Int32 dictionaryIndex = 0;
                    index = (Decimal)enumerator.Current.Key;

                    for (; dictionaryIndex < AxisLabelContentDictionary.Count - 1; dictionaryIndex++)
                    {   
                        enumerator.MoveNext();
                        index = Math.Min(index, (Decimal)enumerator.Current.Key);
                    }

                    enumerator.Dispose();
                }   
                
                minval = index;

                if (minval != maxVal)
                {   
                    for (; index <= maxVal; index = minval + (++count) * gap)
                    {
                        if ((AllAxisLabels) && (AxisLabelContentDictionary.Count > 0) && (index > (Decimal)DataMaximum))
                            continue;

                        String labelContent = "";

                        if (AxisLabelContentDictionary.ContainsKey((Double)index))
                        {
                            if (ParentAxis.AxisOrientation == Orientation.Vertical)
                                labelContent = GetFormattedMultilineText(AutoFormatMultilineText(AxisLabelContentDictionary[(Double)index]));
                            else
                                labelContent = GetFormattedMultilineText(AxisLabelContentDictionary[(Double)index]);
                        }
                        else
                            labelContent = GetFormattedString((Double)index);
                        
                        AxisLabel label = CreateLabel(labelContent);

                        AxisLabelList.Add(label);
                        LabelValues.Add((Double)index);
                    }
                }
                else
                {   
                    return false;
                }
            }
            else
            {   
                if (minval != maxVal)
                {   
                    AxisLabel label;

                    // Create and save the first label
                    label = CreateLabel(GetFormattedString(Minimum));
                    AxisLabelList.Add(label);
                    LabelValues.Add(Minimum);

                    // Shift the maximum inwards so that the last label is not created
                    // the reason for this is some times due to double approximation the last label doesnt get created
                    // thats why the last label is explicitly created and hence must not be created in the following loop
                    maxVal = (Decimal)(Maximum - interval / 2);

                    //Create and save intermediate labels
                    for (index = minval + (++count) * gap; index <= maxVal; index = minval + (++count) * gap)
                    {
                        label = CreateLabel(GetFormattedString((Double)index));
                        AxisLabelList.Add(label);
                        LabelValues.Add((Double)index);
                    }

                    //create and save the last label
                    label = CreateLabel(GetFormattedString(Maximum));
                    AxisLabelList.Add(label);
                    LabelValues.Add(Maximum);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Auto formats the AxisLabel text if bigger for Vertical charts
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private String AutoFormatMultilineText(String text)
        {
            String multiLineText = "";
            Int32 charCount = 0;
            foreach (Char c in text)
            {
                if (c != ' ')
                {
                    charCount++;
                    multiLineText += c;
                }
                else if (charCount >= 15)
                {
                    multiLineText += "\n";
                    charCount = 0;
                }
                else
                    multiLineText += c;
            }

            multiLineText = GetFormattedMultilineText(multiLineText);
            return multiLineText;
        }

        /// <summary>
        /// Sets the position of labels based on the placement type
        /// </summary>
        private void SetLabelPosition()
        {
            switch (Placement)
            {
                case PlacementTypes.Top:
                    PositionLabelsTop();
                    break;
                case PlacementTypes.Left:
                    PositionLabelsLeft();
                    break;
                case PlacementTypes.Right:
                    PositionLabelsRight();
                    break;
                case PlacementTypes.Bottom:
                    PositionLabelsBottom();
                    break;
            }
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear above the plot area
        /// </summary>
        private void PositionLabelsTop()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the width is valid or not
            if (Double.IsNaN(Width) || Width <= 0)
                return;

            // set the width of the canvas
            Visual.Width = Width;

            // Variable to calculate the height of the visual canvas
            Double height = 0;

            // Calculate Default placement values
            CalculateHorizontalDefaults();

            // Calculate the height of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label);

                // set the label position
                label.Position = new Point(0, 0);

                // create the label visual element
                label.CreateVisualObject();

                // get the max height of the labels
                height = Math.Max(Math.Max(height, label.ActualHeight), _maxRowHeight);

            }

            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate vertical Position
                Double top = 0;
                if (GetAngle() != 0)
                {
                    top = Math.Abs((label.ActualTextHeight / 2) * Math.Sin(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(position, height * (Int32)Rows - top - ((i % (Int32)Rows) * _maxRowHeight) + Padding.Top);

                // Create the visual element again
                label.CreateVisualObject();

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

            }

            // set the height of the visual canvas
            Visual.Height = height * (Int32)Rows + Padding.Top;

            // calculate the overflow due to this set of axis labels
            CalculateHorizontalOverflow();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the bottom of the plot area
        /// </summary>
        private void PositionLabelsLeft()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the height is valid or not
            if (Double.IsNaN(Height) || Height <= 0)
                return;

            // set the height of the canvas
            Visual.Height = Height;

            // Variable to calculate the height of the visual canvas
            Double width = 0;

            //Calculate Defaults for the vertical axis
            CalculateVerticalDefaults();

            // Calculate the width of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                // set size affecting pararameters
                ApplyAxisLabelFontProperties(label);

                // set the label position
                label.Position = new Point(0, 0);

                // create the label visual element
                label.CreateVisualObject();

                // get the max width of the labels
                width = Math.Max(width, label.ActualWidth);
            }

            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate horizontal Position
                Double left = 0;
                if (GetAngle() != 0)
                {
                    left = Math.Abs((label.ActualTextHeight / 2) * Math.Cos(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(width - left + Padding.Left, position);

                // Create the visual element again
                label.CreateVisualObject();

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

            }

            // set the width of the visual canvas
            Visual.Width = width + Padding.Left;

            // calculate the overflow due to this set of axis labels
            CalculateVerticalOverflow();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the right of the plot area
        /// </summary>
        private void PositionLabelsRight()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the height is valid or not
            if (Double.IsNaN(Height) || Height <= 0)
                return;

            // set the height of the canvas
            Visual.Height = Height;

            // Variable to calculate the height of the visual canvas
            Double width = 0;

            //Calculate Defaults for the vertical axis
            CalculateVerticalDefaults();

            // Calculate the width of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label);

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate horizontal Position
                Double left = 0;
                if (GetAngle() != 0)
                {
                    left = Math.Abs((label.ActualTextHeight / 2) * Math.Cos(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(0, position);

                // Create the visual element again
                label.CreateVisualObject();

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

                // get the max width of the labels
                width = Math.Max(width, label.ActualWidth);
            }

            // set the width of the visual canvas
            Visual.Width = width + Padding.Right;

            // calculate the overflow due to this set of axis labels
            CalculateVerticalOverflow();
        }
         


        private void ApplyAxisLabelFontProperties(AxisLabel label)
        {
            //Set size affecting font properties
            label.FontSize = FontSize;
            label.FontColor = Graphics.ApplyAutoFontColor((Chart as Chart), FontColor, false);
            label.FontFamily = FontFamily;
            label.FontStyle = FontStyle;
            label.FontWeight = FontWeight;
            label.Angle = GetAngle();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the left of the plot area
        /// </summary>
        private void PositionLabelsBottom()
        {
            Double startOffset = Double.IsNaN(ParentAxis.StartOffset) ? 0 : ParentAxis.StartOffset;
            Double endOffset = Double.IsNaN(ParentAxis.EndOffset) ? 0 : ParentAxis.EndOffset;

            // Check if the width is valid or not
            if (Double.IsNaN(Width) || Width <= 0)
                return;

            // set the width of the canvas
            Visual.Width = Width;

            // Variable to calculate the height of the visual canvas
            Double height = 0;

            // Calculate Default placement values
            CalculateHorizontalDefaults();

            // Calculate the height of the labels and position them
            for (Int32 i = 0; i < AxisLabelList.Count; i += (ParentAxis.SkipOfset + 1))
            {
                AxisLabel label = AxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label);

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate vertical Position
                Double top = 0;
                if (GetAngle() != 0)
                {
                    top = Math.Abs((label.ActualTextHeight / 2) * Math.Sin(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                // Set the new position
                label.Position = new Point(position, top + ((i % (Int32)Rows) * _maxRowHeight));

                // Create the visual element again
                label.CreateVisualObject();

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

                // get the max height of the labels
                height = Math.Max(Math.Max(height, label.ActualHeight), _maxRowHeight);
            }

            // set the height of the visual canvas
            Visual.Height = height * (Int32)Rows + Padding.Bottom;

            // calculate the overflow due to this set of axis labels
            CalculateHorizontalOverflow();
        }

        /// <summary>
        /// This is for axis with placement setting as top or bottom
        /// </summary>
        /// <returns></returns>
        private void CalculateHorizontalOverflow()
        {   
            // Check if the label list contains any labels or not (if not then set the overflow to 0)
            if (AxisLabelList.Count > 0)
            {
                LeftOverflow = (from axisLabel in AxisLabelList select axisLabel.ActualLeft).Min();
                RightOverflow = (from axisLabel in AxisLabelList select (axisLabel.ActualLeft + axisLabel.ActualWidth)).Max() - Width;
            }
            else
            {
                LeftOverflow = 0;
                RightOverflow = 0;
            }

            // if over flow is negative only then an actual overflow has ocured
            if ((Boolean)ParentAxis.Enabled)
                LeftOverflow = LeftOverflow > 0 ? 0 : Math.Abs(LeftOverflow);
            else
                LeftOverflow = 0;

            // if over flow is positive only then an actual overflow has ocured
            RightOverflow = RightOverflow < 0 ? 0 : RightOverflow;

            // For top or bottom these will remain zero
            TopOverflow = 0;
            BottomOverflow = 0;
        }

        /// <summary>
        /// This is for axis with placement setting as left or right
        /// </summary>
        private void CalculateVerticalOverflow()
        {
            // Check if the label list contains any labels or not (if not then set the overflow to 0)
            if (AxisLabelList.Count > 0)
            {
                TopOverflow = (from axisLabel in AxisLabelList select axisLabel.ActualTop).Min();
                BottomOverflow = ((from axisLabel in AxisLabelList select (axisLabel.ActualTop + axisLabel.ActualHeight)).Max()) - Height;
            }
            else
            {   
                TopOverflow = 0;
                BottomOverflow = 0;
            }
            
            // if over flow is negative only then an actual overflow has ocured
            TopOverflow = TopOverflow > 0 ? 0 : Math.Abs(TopOverflow);

            // if over flow is positive only then an actual overflow has ocured
            BottomOverflow = BottomOverflow < 0 ? 0 : BottomOverflow;

            // For left or right these will remain zero
            LeftOverflow = 0;
            RightOverflow = 0;
        }

        /// <summary>
        /// returns the proper angle value for calculation
        /// </summary>
        private Double GetAngle()
        {
            return Double.IsNaN((Double)this.Angle) ? 0 : (Double)this.Angle;
        }

        /// <summary>
        /// Calculates default font size based on a scaling criteria
        /// </summary>
        private Double CalculateFontSize(Double area)
        {
            if (Double.IsNaN(FontSize) || FontSize <= 0)
            {
                return Graphics.DefaultFontSizes[1];
            }
            else
                return FontSize;
        }

        
        private Double AutoAdjustFontSize(Double initialFontSize, Double width)
        {
            Double minimumFontSize = 8;
            Double fontSize = initialFontSize;

            TextBlock textBlock = new TextBlock();
            /* set other font properties here */

            Double labelsWidth = 0;
            Size textBlockSize;
            for (; fontSize > minimumFontSize; fontSize -= 2)
            {
                textBlock.FontSize = fontSize;
                labelsWidth = 0;

                foreach (AxisLabel label in AxisLabelList)
                {
                    textBlock.Text = " " + label.Text + " ";
                    textBlockSize = Graphics.CalculateTextBlockSize(AxisLabel.GetRadians(GetAngle()), textBlock);
                    labelsWidth += textBlockSize.Width;
                }

                if (labelsWidth <= width)
                    break;
            }

            return fontSize;
        }

        private Int32 CalculateNumberOfRows()
        {
            if (Rows <= 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock = SetFontProperties(textBlock);

                //Calculate interval
                Double interval = (Double)((Double.IsNaN((Double)Interval) && Interval <= 0) ? Interval : ParentAxis.InternalInterval);

                Double pixelInterval = Graphics.ValueToPixelPosition(0, Width, Minimum, Maximum, interval + Minimum);
                List<Double> labelWidths = new List<Double>();
                Double maxRowHeight = 0;
                Size textBlockSize;

                foreach (AxisLabel label in AxisLabelList)
                {
                    textBlock.Text = " " + label.Text + " ";
                    textBlockSize = Graphics.CalculateTextBlockSize(AxisLabel.GetRadians(GetAngle()), textBlock);
                    maxRowHeight = Math.Max(maxRowHeight, textBlockSize.Height);
                    labelWidths.Add(textBlockSize.Width);
                }

                _maxRowHeight = maxRowHeight;

                Boolean overlap;
                Int32 rows;
                for (rows = 1; rows <= 3; rows++)
                {
                    overlap = false;
                    for (Int32 i = 0; i < labelWidths.Count - rows; i++)
                    {
                        Double labelFittingSize = labelWidths[i] / 2 + labelWidths[i + rows] / 2;
                        if (labelFittingSize > pixelInterval * rows)
                        {
                            overlap = true;
                            break;
                        }
                    }
                    if (!overlap)
                        break;
                }

                return rows;
            }
            else
                return (Int32)Rows;
        }

        private TextBlock SetFontProperties(TextBlock textBlock)
        {
            textBlock.FontSize = FontSize;
            /* set other font properties */
            textBlock.FontFamily = FontFamily;
            textBlock.FontStyle = FontStyle;
            textBlock.FontWeight = FontWeight;
            return textBlock;
        }

        private Double GetMaxHeight()
        {
            TextBlock textBlock = new TextBlock();

            textBlock = SetFontProperties(textBlock);

            Double maxRowHeight = 0;
            Size textBlockSize;

            Int32 labelIndex = 0;
            for (labelIndex = 0; labelIndex < AxisLabelList.Count ; labelIndex += (ParentAxis.SkipOfset + 1))
            {
                AxisLabel label = AxisLabelList[labelIndex];
                textBlock.Text = label.Text;
                textBlockSize = Graphics.CalculateTextBlockSize(AxisLabel.GetRadians(GetAngle()), textBlock);
                maxRowHeight = Math.Max(maxRowHeight, textBlockSize.Height);
            }

            return maxRowHeight;
        }

        private void CalculateHorizontalDefaults()
        {
            IsNotificationEnable = false;

            Double width = Double.IsNaN(Width) ? 0 : Width;
            Double height = Double.IsNaN(Height) ? 0 : Height;
            Double max = Math.Max(width, height);
            if (Double.IsNaN(FontSize) || FontSize <= 0)
            {
                Double initialFontSize = CalculateFontSize(max);
                //_fontSize = AutoAdjustFontSize(initialFontSize, width);
                FontSize = initialFontSize;
            }
            if (Rows <= 0)
            {
                Int32 rows = CalculateNumberOfRows();

                if (rows > 2 && Double.IsNaN((Double)Angle))
                {
                    Rows = 1;
                    Angle = -45;

                    if (Double.IsNaN((Double)ParentAxis.Interval) && Double.IsNaN((Double)Interval))
                        ParentAxis.SkipOfset = CalculateSkipOffset((int)Rows, (Double)Angle, Width);
                    else
                        ParentAxis.SkipOfset = 0;
                }
                else
                {
                    
                    Rows = rows;
                    
                }
            }
            else
            {   
                Int32 rows = CalculateNumberOfRows();

                if (rows > 2 && Double.IsNaN((Double)Angle))
                {
                    Angle = -45;
                }
            }

            _maxRowHeight = GetMaxHeight();

            IsNotificationEnable = true;

        }

        private Int32 CalculateSkipOffset(Int32 NoOfRows, Double Angle,Double AxisWidth)
        {
            Int32 skipOffset = 0;             // Skip offset
            Boolean overlap = true;

            Double interval = (Double)((Double.IsNaN((Double)Interval) && Interval <= 0) ? Interval : ParentAxis.InternalInterval);
            Double pixelInterval;
            TextBlock textBlock = new TextBlock();
            textBlock = SetFontProperties(textBlock);
            textBlock.Text = "ABCD";

#if WPF
            textBlock.Measure(new Size(Double.MaxValue, Double.MaxValue));
#endif
                        
            while(overlap)
            {   
                pixelInterval = Graphics.ValueToPixelPosition(0, Width, Minimum, Maximum, interval + skipOffset + Minimum);

                if (pixelInterval >= textBlock.ActualHeight)
                {
                    overlap = false;
                }

                skipOffset++;
            }

            return skipOffset;
        }

        private String GetFormattedString(Double value)
        {
            String str = value.ToString();
            if (ParentAxis.ScaleValues != null && ParentAxis.ScaleUnits != null)
            {
                String sUnit = ParentAxis.ScaleUnits[0];
                Double sValue = ParentAxis.ScaleValues[0];
                for (Int32 i = 0; i < ParentAxis.ScaleValues.Count; i++)
                {
                    if ((Math.Abs(value) / ParentAxis.ScaleValues[i]) < 1)
                    {
                        break;
                    }
                    sValue = ParentAxis.ScaleValues[i];
                    sUnit = ParentAxis.ScaleUnits[i];
                }
                str = ParentAxis.Prefix + (value / sValue).ToString(ParentAxis.ValueFormatString) + sUnit + ParentAxis.Suffix;
            }
            else
            {
                str = value.ToString(ParentAxis.ValueFormatString);
                str = ParentAxis.Prefix + str + ParentAxis.Suffix;
            }
            return str;
        }

        private void CalculateVerticalDefaults()
        {
            Double width = Double.IsNaN(Width) ? 0 : Width;
            Double height = Double.IsNaN(Height) ? 0 : Height;
            Double max = Math.Max(width, height);
            if (Double.IsNaN(FontSize) || FontSize <= 0)
            {
                FontSize = CalculateFontSize(max);
            }
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// List of axis labels
        /// </summary>

        internal List<AxisLabel> AxisLabelList
        {
            get;
            set;
        }

        /// <summary>
        /// List of position values for the labels
        /// </summary>
        internal List<Double> LabelValues
        {
            get;
            set;
        }

        #endregion

        #region Internal Methods

        internal override void UpdateVisual(string PropertyName, object Value)
        {
            if (Visual != null)
            {
                foreach (AxisLabel axisLabel in AxisLabelList)
                {
                    ApplyAxisLabelFontProperties(axisLabel);
                    axisLabel.ApplyProperties(axisLabel);
                }
            }
            else
                FirePropertyChanged(PropertyName);
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        private Double _minimum;
        private Double _maximum;
        private Double _dataMinimum;
        private Double _dataMaximum;
        private Double _width;
        private Double _height;

        private Double _maxRowHeight;

        private Double _savedFontSize;
        private Int32 _savedRows;
        private Double _savedAngle;
        private Boolean _isRedraw;
        private Axis _parent;

#if WPF
        static Boolean _defaultStyleKeyApplied;            // Default Style key
#endif
        #endregion
    }
}