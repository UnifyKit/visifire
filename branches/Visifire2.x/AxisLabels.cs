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

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.AxisLabels class
        /// </summary>
        public AxisLabels()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(AxisLabels), new FrameworkPropertyMetadata(typeof(AxisLabels)));
                _defaultStyleKeyApplied = true;

            }
#else
            DefaultStyleKey = typeof(AxisLabels);
#endif

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Interval dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Interval dependency property.
        /// </returns>
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register
            ("Interval",
            typeof(Nullable<Double>),
            typeof(AxisLabels),
            new PropertyMetadata(OnIntervalPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Angle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Angle dependency property.
        /// </returns>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register
            ("Angle",
            typeof(Nullable<Double>),
            typeof(AxisLabels),
            new PropertyMetadata(OnAnglePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(AxisLabels),
            new PropertyMetadata(OnEnabledPropertyChanged));
        
#if WPF
        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontFamily dependency property.
        /// </returns>
        private new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
            ("FontFamily",
            typeof(FontFamily),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontFamilyPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontColorPropertyChanged));
        
#if WPF

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontStyle dependency property.
        /// </returns>
        private new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontWeight dependency property.
        /// </returns>
        private new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.FontSize dependency property.
        /// </returns>
        private new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(AxisLabels),
            new PropertyMetadata(OnFontSizePropertyChanged));
#endif
        
        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.TextWrap dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.TextWrap dependency property.
        /// </returns>
        public static readonly DependencyProperty TextWrapProperty = DependencyProperty.Register
            ("TextWrap",
            typeof(TextWrapping),
            typeof(AxisLabels),
            new PropertyMetadata(OnTextWrapPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.AxisLabels.Rows dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.Rows dependency property.
        /// </returns>
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register
            ("Rows",
            typeof(Nullable<Int32>),
            typeof(AxisLabels),
            new PropertyMetadata(OnRowsPropertyChanged));

        /// <summary>
        /// ToolTipText property
        /// ( NotImplemented )
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override String ToolTipText
        {
            get
            {
                throw new NotImplementedException("ToolTipText property for AxisLabels is not implemented");
            }
            set
            {
                throw new NotImplementedException("ToolTipText property for AxisLabels is not implemented");
            }
        }

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

        /// <summary>
        /// Font family of AxisLabels 
        /// </summary>
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

        /// <summary>
        /// Set the Color for the text in AxisLabels 
        /// </summary>
        public Brush FontColor
        {
            get
            {
                return (Brush)GetValue(FontColorProperty);
            }
            set
            {
                SetValue(FontColorProperty, value);
            }
        }

        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.UpdateVisual("FontColor", e.NewValue);
        }

        /// <summary>
        /// Set the Styles for the text like "Italic" or "Normal"
        /// </summary>
#if WPF
         [TypeConverter(typeof(System.Windows.FontStyleConverter))]
#endif
        public new FontStyle FontStyle
        {
            get
            {
                return (FontStyle)(GetValue(FontStyleProperty));
            }
            set
            {
#if SL          
                if (FontStyle != value)
                {
                    SetValue(FontStyleProperty, value);
                    UpdateVisual("FontStyle", value);
                }
#else
                 SetValue(FontStyleProperty, value);
#endif
            }
        }
        
        /// <summary>
        /// Sets how the font appears. It takes values like "Bold", "Normal", "Black" etc
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
#endif
        public new FontWeight FontWeight
        {
            get
            {
                return (FontWeight)(GetValue(FontWeightProperty));
            }
            set
            {

#if SL          
                if (FontWeight != value)
                {
                    SetValue(FontWeightProperty, value);
                    UpdateVisual("FontWeight", value);
                }
#else
                SetValue(FontWeightProperty, value);
#endif

            }
        }

        /// <summary>
        /// Font Size of axis labels
        /// </summary>
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

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        /// <summary>
        /// Visual element for axis labels
        /// </summary>
        internal Canvas Visual
        {
            get;
            private set;
        }

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
        /// Visual minimum for the axis
        /// </summary>
        internal Double DataMinimum
        {
            get;
            set;
        }

        /// <summary>
        /// Visual maximum for the axis
        /// </summary>
        internal Double DataMaximum
        {
            get;
            set;
        }

        /// <summary>
        /// Set the width of the axis labels canvas. will be used only with the Horizontal axis
        /// </summary>
        internal new Double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Set the height of the axis labels canvas. will be used ony with the vertical axis 
        /// </summary>
        internal new Double Height
        {
            get;
            set;
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

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Event handler attached with Interval property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Interval");
        }

        /// <summary>
        /// Event handler attached with Angle property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Angle");
        }
        
        /// <summary>
        /// Event handler attached with Enabled property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Enabled");
        }

#if WPF

        /// <summary>
        /// Event handler attached with FontFamily property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("FontFamily");
        }

        /// <summary>
        /// Event handler attached with FontStyle property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.UpdateVisual("FontStyle", e.NewValue);
        }

        /// <summary>
        /// Event handler attached with FontWeight property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.UpdateVisual("FontWeight", e.NewValue);
        }
        
        /// <summary>
        /// Event handler attached with FontSize property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("FontSize");
        }
#endif

        /// <summary>
        /// Event handler attached with TextWrap property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextWrapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("TextWrap");
        }

        /// <summary>
        /// Event handler attached with Rows property changed event of AxisLabels elements
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AxisLabels axisLabels = d as AxisLabels;
            axisLabels.FirePropertyChanged("Rows");
        }
        
        /// <summary>
        /// Create a instance of a Visifire.Charts.AxisLabel
        /// </summary>
        /// <param name="text">Text as string</param>
        /// <returns>AxisLabel</returns>
        private AxisLabel CreateLabel(String text)
        {
            AxisLabel label = new AxisLabel();
            label.Text = text;
            label.Placement = this.Placement;
            return label;
        }

        /// <summary>
        /// Calculate auto interval
        /// </summary>
        /// <param name="CurrentInterval">Current interval</param>
        /// <param name="AxisWidth">Width of the axis</param>
        /// <param name="NoOfLabels">Number of labels</param>
        /// <param name="Angle">Angle of labels</param>
        /// <param name="Rows">Number of rows</param>
        /// <returns></returns>
        private Double CalculateAutoInterval(Double CurrentInterval, Double AxisWidth, Int32 NoOfLabels, Double Angle, Int32 Rows)
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

        /// <summary>
        /// Apply font properties of axislabels
        /// </summary>
        /// <param name="label">AxisLabel</param>
        private void ApplyAxisLabelFontProperties(AxisLabel label)
        {
            //Set size affecting font properties
            label.FontSize = FontSize;
            label.FontColor = Charts.Chart.CalculateFontColor((Chart as Chart), FontColor, false);
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
                
        /// <summary>
        /// Calculate auto font size
        /// </summary>
        /// <param name="initialFontSize">Double</param>
        /// <param name="width">Double</param>
        /// <returns></returns>
        private Double AutoAdjustFontSize(Double initialFontSize, Double width)
        {
            Double minimumFontSize = 8;
            Double fontSize = initialFontSize;
            TextBlock textBlock = new TextBlock();

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

        /// <summary>
        /// Calculate number of rows for axislabels
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Set properties of a textblock
        /// </summary>
        /// <param name="textBlock">TextBlock</param>
        /// <returns>TextBlock</returns>
        private TextBlock SetFontProperties(TextBlock textBlock)
        {
            textBlock.FontSize = FontSize;
            /* set other font properties */
            textBlock.FontFamily = FontFamily;
            textBlock.FontStyle = FontStyle;
            textBlock.FontWeight = FontWeight;
            
            return textBlock;
        }

        /// <summary>
        /// Get max height of axislabels
        /// </summary>
        /// <returns>Double</returns>
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

        /// <summary>
        /// Calculate default values for angle and rows of axis
        /// </summary>
        private void CalculateHorizontalDefaults()
        {
            IsNotificationEnable = false;

            Double width = Double.IsNaN(Width) ? 0 : Width;
            Double height = Double.IsNaN(Height) ? 0 : Height;
            Double max = Math.Max(width, height);
            if (Double.IsNaN(FontSize) || FontSize <= 0)
            {
                Double initialFontSize = CalculateFontSize(max);
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
        
        /// <summary>
        /// Calculate skip offset for axis labels
        /// </summary>
        /// <param name="noOfRows">Number of rows</param>
        /// <param name="angle">Rotation angle of labels</param>
        /// <param name="axisWidth">Width of the axis</param>
        /// <returns></returns>
        private Int32 CalculateSkipOffset(Int32 noOfRows, Double angle, Double axisWidth)
        {
            Int32 skipOffset = 0;             // Skip offset
            Boolean overlap = true;

            Double interval = (Double)((Double.IsNaN((Double)Interval) && Interval <= 0) ? Interval : ParentAxis.InternalInterval);
            //Double interval = (Double)((Double.IsNaN((Double)Interval) || Interval <= 0) ? ParentAxis.InternalInterval : Interval);
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
                else
                    skipOffset++;
            }

            return skipOffset;
        }

        /// <summary>
        /// Return formatted string from a given value depending upon scaling set and value format string
        /// </summary>
        /// <param name="value">Double value</param>
        /// <returns>String</returns>
        private String GetFormattedString(Double value)
        {
            return (ParentAxis != null)? ParentAxis.GetFormattedString(value) : value.ToString();
        }

        /// <summary>
        ///  Calculate default values for vertical axis
        /// </summary>
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
        /// Identifies the Visifire.Charts.AxisLabels.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.AxisLabels.ToolTipText dependency property.
        /// </returns>
        private new static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(AxisLabels),
            null);
            
        #endregion

        #region Internal Methods

        /// <summary>
        /// Update visual used for partial update
        /// </summary>
        /// <param name="PropertyName">Name of the property</param>
        /// <param name="Value">Value of the property</param>
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

        /// <summary>
        /// Creates a visual object for the element
        /// </summary>
        internal void CreateVisualObject()
        {
            // Create a new 
            Visual = new Canvas();

            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

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

        #region Internal Events

        #endregion

        #region Data
        
        /// <summary>
        /// Saved max row height
        /// </summary>
        private Double _maxRowHeight;

        /// <summary>
        /// Saved font size of axislabels
        /// </summary>
        private Double _savedFontSize;

        /// <summary>
        /// Saved number of rows
        /// </summary>
        private Int32 _savedRows;

        /// <summary>
        /// Saved old angle
        /// </summary>
        private Double _savedAngle;

        /// <summary>
        /// Whether the axis need to redraw
        /// </summary>
        private Boolean _isRedraw;

        /// <summary>
        /// Parent axis
        /// </summary>
        private Axis _parent;

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif
        #endregion
    }
}