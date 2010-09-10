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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

#else

using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#endif

using Visifire.Commons;
using System.Windows.Data;
using System.Collections.Generic;
using System.ComponentModel;
using Visifire.Commons.Controls;
using System.Collections.Specialized;

namespace Visifire.Charts
{
    public class CustomAxisLabels : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.CustomAxisLabels class
        /// </summary>
        public CustomAxisLabels()
        {
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomAxisLabels), new FrameworkPropertyMetadata(typeof(CustomAxisLabels)));
                _defaultStyleKeyApplied = true;

            }
#else
            DefaultStyleKey = typeof(CustomAxisLabels);
#endif

            WidthOfACharacter = Double.NaN;
            InternalAngle = Double.NaN;
            _tag = new ElementData() { Element = this };

            // Initialize CustomAxisLabels list
            Labels = new CustomAxisLabelCollection();

            Labels.CollectionChanged += new NotifyCollectionChangedEventHandler(Labels_CollectionChanged);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _rootElement = GetTemplateChild(RootElementName) as Canvas;

            foreach (CustomAxisLabel label in Labels)
            {
#if WPF
                if (IsInDesignMode)
                    ObservableObject.RemoveElementFromElementTree(label);
#endif

                if (!_rootElement.Children.Contains(label))
                    _rootElement.Children.Add(label);
            }
        }

        public override void Bind()
        {
#if SL
            Binding b = new Binding("FontSize");
            b.Source = this;
            this.SetBinding(InternalFontSizeProperty, b);

            b = new Binding("FontFamily");
            b.Source = this;
            this.SetBinding(InternalFontFamilyProperty, b);

            b = new Binding("FontStyle");
            b.Source = this;
            this.SetBinding(InternalFontStyleProperty, b);

            b = new Binding("FontWeight");
            b.Source = this;
            this.SetBinding(InternalFontWeightProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);

            b = new Binding("MaxWidth");
            b.Source = this;
            this.SetBinding(InternalMaxWidthProperty, b);

            b = new Binding("MaxHeight");
            b.Source = this;
            this.SetBinding(InternalMaxHeightProperty, b);

            b = new Binding("MinWidth");
            b.Source = this;
            this.SetBinding(InternalMinWidthProperty, b);

            b = new Binding("MinHeight");
            b.Source = this;
            this.SetBinding(InternalMinHeightProperty, b);
#endif
        }

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.LineEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.LineEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LineEnabledProperty = DependencyProperty.Register
            ("LineEnabled",
            typeof(Boolean),
            typeof(CustomAxisLabels),
            new PropertyMetadata(true, OnLineEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.LineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.LineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register
            ("LineThickness",
            typeof(Double),
            typeof(CustomAxisLabels),
            new PropertyMetadata(0.25, OnLineThicknessPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.LineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.LineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LineColorProperty = DependencyProperty.Register
            ("LineColor",
            typeof(Brush),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnLineColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.TextAlignment dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.TextAlignment dependency property.
        /// </returns>
        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register
            ("TextAlignment",
            typeof(TextAlignment),
            typeof(CustomAxisLabels),
            new PropertyMetadata(TextAlignment.Left, OnTextAlignmentPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty FontColorProperty = DependencyProperty.Register
            ("FontColor",
            typeof(Brush),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.Angle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.Angle dependency property.
        /// </returns>
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register
            ("Angle",
            typeof(Nullable<Double>),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnAnglePropertyChanged));

#if WPF

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontStyle dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register
            ("FontStyle",
            typeof(FontStyle),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontWeight dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
            ("FontWeight",
            typeof(FontWeight),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontSize dependency property.
        /// </returns>
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
            ("FontSize",
            typeof(Double),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontSizePropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.TextWrap dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.TextWrap dependency property.
        /// </returns>
        public static readonly DependencyProperty TextWrapProperty = DependencyProperty.Register
            ("TextWrap",
            typeof(Double),
            typeof(CustomAxisLabels),
            new PropertyMetadata(Double.NaN, OnTextWrapPropertyChanged));

        /// <summary>
        /// ToolTipText property
        /// ( NotImplemented )
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override String ToolTipText
        {
            get
            {
                throw new NotImplementedException("ToolTipText property for CustomAxisLabels is not implemented");
            }
            set
            {
                throw new NotImplementedException("ToolTipText property for CustomAxisLabels is not implemented");
            }
        }

        /// <summary>
        /// Get or set the maximum width of the labels relative to chart size. Value range is 0 - 1.
        /// </summary>
        public Double TextWrap
        {
            get
            {
                return (Double)GetValue(TextWrapProperty);
            }
            set
            {
                SetValue(TextWrapProperty, value);
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
                    InternalOpacity = value;
                    SetValue(OpacityProperty, value);
                    FirePropertyChanged(VcProperties.Opacity);
                }
#else
                SetValue(OpacityProperty, value);
#endif
            }
        }

        /// <summary>
        /// Enables or disables custom axis labels
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
        /// Enabled or disables the line marker for custom axis lables
        /// </summary>
        public Boolean LineEnabled
        {
            get
            {
                return (Boolean)GetValue(LineEnabledProperty);
            }
            set
            {
                SetValue(LineEnabledProperty, value);
            }
        }

        /// <summary>
        /// Get or set the color for the line thickness in custom axis labels
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
        /// Get or set the color for the line in custom axis labels
        /// </summary>
        public Brush LineColor
        {
            get
            {
                if (GetValue(LineColorProperty) == null)
                    return FontColor;
                else
                    return (Brush)GetValue(LineColorProperty);
            }
            set
            {
                SetValue(LineColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the alignment for the text in custom axis labels 
        /// </summary>
        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Get or set the font family of custom axis labels
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
                    InternalFontFamily = value;
                    SetValue(FontFamilyProperty, value);
                    FirePropertyChanged(VcProperties.FontFamily);
                }
#else           
                SetValue(FontFamilyProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the color for the text in custom axis labels
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

        /// <summary>
        /// Get or set the styles for the text like "Italic" or "Normal"
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
                if (InternalFontStyle != value)
                {
                    InternalFontStyle = value;
                    SetValue(FontStyleProperty, value);
                    UpdateVisual(VcProperties.FontStyle, value);
                }
#else
                 SetValue(FontStyleProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set how the font appears. It takes values like "Bold", "Normal", "Black" etc
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
                    InternalFontWeight = value;
                    SetValue(FontWeightProperty, value);
                    UpdateVisual(VcProperties.FontWeight, value);
                }
#else
                SetValue(FontWeightProperty, value);
#endif

            }
        }

        /// <summary>
        /// Get or set the font size for custom axis labels
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
                    InternalFontSize = value;
                    SetValue(FontSizeProperty, value);
                    FirePropertyChanged(VcProperties.FontSize);
                }
#else
                SetValue(FontSizeProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Angle for custom axis labels
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> Angle
        {
            get
            {
                if ((Nullable<Double>)GetValue(AngleProperty) == null)
                    return InternalAngle;
                else
                    return (Nullable<Double>)GetValue(AngleProperty);
            }
            set
            {
                SetValue(AngleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the parent as custom axis labels
        /// </summary>
        public new Axis Parent
        {
            get
            {
                return _parent;
            }
            internal set
            {
                System.Diagnostics.Debug.Assert(typeof(Axis).Equals(value.GetType()), "Unknown Parent", "CustomAxisLabels should have Axis as Parent");
                _parent = value;
            }
        }


        /// <summary>
        /// Collection of CustomAxisLabels
        /// </summary>
        public CustomAxisLabelCollection Labels
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Event handler attached with LineColor property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.UpdateVisual(VcProperties.LineColor, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with LineThickness property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.UpdateVisual(VcProperties.LineThickness, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with FontColor property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.UpdateVisual(VcProperties.FontColor, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with Enabled property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.FirePropertyChanged(VcProperties.Enabled);
        }

        /// <summary>
        /// Event handler attached with LineEnabled property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLineEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.FirePropertyChanged(VcProperties.LineEnabled);
        }

        /// <summary>
        /// Event handler attached with TextAlignment property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextAlignmentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.FirePropertyChanged(VcProperties.TextAlignment);
        }

        /// <summary>
        /// Event handler attached with FontFamily property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;

            if (e.NewValue == null || e.OldValue == null)
            {
                customAxisLabels.InternalFontFamily = (FontFamily)e.NewValue;
                customAxisLabels.FirePropertyChanged(VcProperties.FontFamily);
            }
            else if (e.NewValue.ToString() != e.OldValue.ToString())
            {
                customAxisLabels.InternalFontFamily = (FontFamily)e.NewValue;
                customAxisLabels.FirePropertyChanged(VcProperties.FontFamily);
            }
        }

        /// <summary>
        /// Event handler attached with FontStyle property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalFontStyle = (FontStyle)e.NewValue;
            customAxisLabels.UpdateVisual(VcProperties.FontStyle, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with FontWeight property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalFontWeight = (FontWeight)e.NewValue;
            customAxisLabels.UpdateVisual(VcProperties.FontWeight, e.NewValue);
        }

        /// <summary>
        /// Event handler attached with FontSize property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalFontSize = (Double)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.FontSize);
        }

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalOpacity = (Double)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.Opacity);
        }

        /// <summary>
        /// Event handler manages MaxHeight property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMaxHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalMaxHeight = (Double)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.MaxHeight);
        }

        /// <summary>
        /// Event handler manages MinHeight property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMinHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalMinHeight = (Double)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.MinHeight);
        }

        /// <summary>
        /// Event handler manages MaxWidth property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMaxWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalMaxWidth = (Double)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.MaxWidth);
        }

        /// <summary>
        /// Event handler manages MinWidth property change event of axis
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMinWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalMinWidth = (Double)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.MinWidth);
        }

        /// <summary>
        /// Event handler attached with TextWrap property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnTextWrapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;

            if ((Double)e.NewValue < 0 || (Double)e.NewValue > 1)
                throw new Exception("Wrong property value. Range of TextWrapProperty varies from 0 to 1.");

            customAxisLabels.FirePropertyChanged(VcProperties.TextWrap);
        }

        /// <summary>
        /// Event handler attached with Angle property changed event of CustomAxisLabels element
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomAxisLabels customAxisLabels = d as CustomAxisLabels;
            customAxisLabels.InternalAngle = (Nullable<Double>)e.NewValue;
            customAxisLabels.FirePropertyChanged(VcProperties.Angle);
        }

        private void Labels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (CustomAxisLabel customLabel in e.NewItems)
                    {
                        customLabel.Parent = this;

                        if (Chart != null)
                            customLabel.Chart = Chart;

                        if (String.IsNullOrEmpty((String)customLabel.GetValue(NameProperty)))
                        {
                            customLabel.Name = "CustomAxisLabel" + (this.Labels.Count - 1).ToString() + "_" + Guid.NewGuid().ToString().Replace('-', '_');
                        }

                        customLabel.PropertyChanged -= CustomLabel_PropertyChanged;
                        customLabel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(CustomLabel_PropertyChanged);
                    }
                }
            }

            FirePropertyChanged(VcProperties.CustomAxisLabel);
        }

        /// <summary>
        /// CustomLabel property changed event handler
        /// </summary>
        /// <param name="sender">DataSeries</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        private void CustomLabel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.FirePropertyChanged((VcProperties)Enum.Parse(typeof(VcProperties), e.PropertyName, true));
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

            if (!Double.IsNaN(TextWrap))
            {
                Double MaxLabelWidth = (ParentAxis.PlotDetails.ChartOrientation == ChartOrientationType.Vertical) ? Chart.ActualHeight : Chart.ActualWidth;
                label.MaxWidth = MaxLabelWidth * TextWrap;
            }
            else
                label.MaxWidth = Double.NaN;

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
        /// <returns>Double</returns>
        private Double CalculateAutoInterval(Double CurrentInterval, Double AxisWidth, Int32 NoOfLabels, Double Angle, Int32 Rows)
        {
            Double retVal = 1;
            Angle = Double.IsNaN(Angle) ? 0 : Angle;
            //CalculateHorizontalOverflow();

            return retVal;
        }

        /// <summary>
        /// Creates a set of labels
        /// </summary>
        private Boolean CreateLabels()
        {
            if (!Double.IsNaN(TextWrap))
                CalculatAvgWidthOfAChar();

            Double index = 0;

            foreach (CustomAxisLabel customAxisLabel in Labels)
            {
                String labelContent = GetFormattedMultilineText(customAxisLabel.Text);

                AxisLabel label = CreateLabel(labelContent);

                customAxisLabel.Chart = Chart;

                if (ParentAxis.IsDateTimeAxis)
                {
                    Double numericFromValue = DateTimeHelper.DateDiff(Convert.ToDateTime(customAxisLabel.From), Parent.FirstLabelDate, ParentAxis.MinDateRange, ParentAxis.MaxDateRange, ParentAxis.InternalIntervalType, ParentAxis.XValueType);
                    Double numericToValue = DateTimeHelper.DateDiff(Convert.ToDateTime(customAxisLabel.To), Parent.FirstLabelDate, ParentAxis.MinDateRange, ParentAxis.MaxDateRange, ParentAxis.InternalIntervalType, ParentAxis.XValueType);

                    index = (numericFromValue + numericToValue) / 2;

                    label.From = numericFromValue;
                    label.To = numericToValue;
                }
                else
                {
                    if (ParentAxis.AxisRepresentation == AxisRepresentations.AxisY && ParentAxis.Logarithmic)
                    {
                        index = (Math.Log(Convert.ToDouble(customAxisLabel.From), ParentAxis.LogarithmBase) + Math.Log(Convert.ToDouble(customAxisLabel.To), ParentAxis.LogarithmBase)) / 2;

                        label.From = Math.Log(Convert.ToDouble(customAxisLabel.From), ParentAxis.LogarithmBase);
                        label.To = Math.Log(Convert.ToDouble(customAxisLabel.To), ParentAxis.LogarithmBase);
                    }
                    else
                    {
                        index = (Convert.ToDouble(customAxisLabel.From) + Convert.ToDouble(customAxisLabel.To)) / 2;

                        label.From = Convert.ToDouble(customAxisLabel.From);
                        label.To = Convert.ToDouble(customAxisLabel.To);
                    }
                }

                if (label.From < label.To)
                {
                    if (label.To > Parent.InternalAxisMaximum)
                        label.To = Parent.InternalAxisMaximum;

                    if (label.From < Parent.InternalAxisMinimum)
                        label.From = Parent.InternalAxisMinimum;

                    index = (label.From + label.To) / 2;
                }
                else
                {
                    if (label.From > Parent.InternalAxisMaximum)
                        label.From = Parent.InternalAxisMaximum;

                    if (label.To < Parent.InternalAxisMinimum)
                        label.To = Parent.InternalAxisMinimum;

                    index = (label.From + label.To) / 2;
                }

                AxisLabelList.Add(label);

                CustomAxisLabelList.Add(customAxisLabel);


                LabelValues.Add((Double)index);
            }

            return true;
        }

        /// <summary>
        /// Calculate average width of a character
        /// </summary>
        /// <returns></returns>
        private void CalculatAvgWidthOfAChar()
        {
            AxisLabel label = new AxisLabel();
            label.Text = "ABCDabcd01";
            ApplyAxisLabelFontProperties(label, null);
            label.CreateVisualObject(null);
            WidthOfACharacter = label.ActualTextWidth / 10;
        }

        /// <summary>
        /// Sets the position of labels based on the placement type
        /// </summary>
        private void SetLabelPosition()
        {
            switch (Placement)
            {
                case PlacementTypes.Left:
                    PositionLabelsLeft();
                    break;
                case PlacementTypes.Bottom:
                    PositionLabelsBottom();
                    break;
                case PlacementTypes.Right:
                    PositionLabelsRight();
                    break;
                case PlacementTypes.Top:
                    PositionLabelsTop();
                    break;
            }
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the left of the plot area
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
            if (ParentAxis.PlotDetails.ChartOrientation == ChartOrientationType.Horizontal)
                CalculateHorizontalDefaults(Height, Width);
            else
                CalculateVerticalDefaults();

            // Calculate the width of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i += (ParentAxis.SkipOffset + 1))
            {
                AxisLabel label = AxisLabelList[i];

                CustomAxisLabel customLabel = CustomAxisLabelList[i];

                // set size affecting pararameters
                ApplyAxisLabelFontProperties(label, customLabel);

                // set the label position
                label.Position = new Point(0, 0);

                // create the label visual element
                label.CreateVisualObject(_tag);

                customLabel.TextElement = label.Visual;

                Double totalAxisLabelWidth = 0;

                if (InternalMinWidth != 0 && InternalMinWidth > label.ActualWidth)
                    totalAxisLabelWidth = InternalMinWidth;
                else
                    totalAxisLabelWidth = label.ActualWidth;

                if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth < totalAxisLabelWidth)
                {
                    label.ActualWidth = InternalMaxWidth;
                }
                else
                    label.ActualWidth = totalAxisLabelWidth;

                // get the max width of the labels
                width = Math.Max(width, label.ActualWidth);
            }

            width += (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0) + LEFT_TOP_PADDING;

            //for (Int32 i = 0; i < AxisLabelList.Count; i++)
            for (Int32 i = 0; i < AxisLabelList.Count; i += (ParentAxis.SkipOffset + 1))
            {
                AxisLabel label = AxisLabelList[i];

                CustomAxisLabel customLabel = CustomAxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate horizontal Position
                Double left = 1;

                if (GetAngle() != 0)
                {
                    left = Math.Abs((label.ActualTextHeight / 2) * Math.Cos(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                Double leftPos = width - left + Padding.Left - LEFT_TOP_PADDING;

                // Set the new position
                label.Position = new Point(leftPos - (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0), position);

                // Create the visual element again
                label.SetPosition();

                Double fromPosition = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, label.From);
                Double toPosition = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, label.To);

                if (LineEnabled)
                {
                    Path path = GetPath(new Point(leftPos, fromPosition), new Point(leftPos, toPosition), Placement, customLabel);
                    
                    customLabel.CustomLabelPath = path;
                    
                    Visual.Children.Add(path);
                }

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);
            }

            // set the width of the visual canvas
            Visual.Width = width + Padding.Left;

            // calculate the overflow due to this set of axis labels
            CalculateVerticalOverflow();
        }

        /// <summary>
        /// Apply font properties of CustomAxisLabels
        /// </summary>
        /// <param name="label">AxisLabel</param>
        private void ApplyAxisLabelFontProperties(AxisLabel label, CustomAxisLabel customAxisLabel)
        {
            //Set size affecting font properties
            label.FontSize = InternalFontSize;
            if(customAxisLabel != null)
                label.FontColor = Charts.Chart.CalculateFontColor((Chart as Chart), ParentAxis.Background, customAxisLabel.FontColor, false);
            label.FontFamily = InternalFontFamily;
            label.FontStyle = InternalFontStyle;
            label.FontWeight = InternalFontWeight;
            label.TextAlignment = TextAlignment;
            label.Angle = GetAngle();
        }

        private Path GetPath(Point startPoint, Point endPoint, PlacementTypes placementType, CustomAxisLabel customLabel)
        {
            Path path = new System.Windows.Shapes.Path();
            path.Stroke = Charts.Chart.CalculateFontColor((Chart as Chart), ParentAxis.Background, customLabel.LineColor, false);
            path.StrokeThickness = (Double) customLabel.LineThickness;

            PathGeometry geometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = startPoint;

            PointCollection collection = new PointCollection();

            if (placementType == PlacementTypes.Bottom)
            {
                collection.Add(new Point(startPoint.X, startPoint.Y + (LineEnabled ? LINE_HEIGHT : 0)));
                collection.Add(new Point(endPoint.X, endPoint.Y + (LineEnabled ? LINE_HEIGHT : 0)));
                collection.Add(endPoint);
            }
            else if (placementType == PlacementTypes.Left)
            {
                collection.Add(new Point(startPoint.X - (LineEnabled ? LINE_HEIGHT : 0), startPoint.Y));
                collection.Add(new Point(endPoint.X - (LineEnabled ? LINE_HEIGHT : 0), endPoint.Y));
                collection.Add(endPoint);
            }
            else if (placementType == PlacementTypes.Right)
            {
                collection.Add(new Point(startPoint.X + (LineEnabled ? LINE_HEIGHT : 0), startPoint.Y));
                collection.Add(new Point(endPoint.X + (LineEnabled ? LINE_HEIGHT : 0), endPoint.Y));
                collection.Add(endPoint);
            }
            else if (placementType == PlacementTypes.Top)
            {
                collection.Add(new Point(startPoint.X, startPoint.Y - (LineEnabled ? LINE_HEIGHT : 0)));
                collection.Add(new Point(endPoint.X, endPoint.Y - (LineEnabled ? LINE_HEIGHT : 0)));
                collection.Add(endPoint);
            }

            foreach (Point point in collection)
            {
                LineSegment segment = new LineSegment();
                segment.Point = point;
                pathFigure.Segments.Add(segment);
            }

            geometry.Figures.Add(pathFigure);
            path.Data = geometry;

            return path;
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
            CalculateHorizontalDefaults(Width, Height);

            Double internalRows = 1;

            // Calculate the height of the labels
            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                CustomAxisLabel customLabel = CustomAxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label, customLabel);

                // set the label position
                label.Position = new Point(0, 0);

                // create the label visual element
                label.CreateVisualObject(_tag);

                Double totalAxisLabelHeight = 0;
                if (InternalMinHeight != 0 && InternalMinHeight > label.ActualHeight)
                    totalAxisLabelHeight = InternalMinHeight;
                else
                    totalAxisLabelHeight = label.ActualHeight;

                if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight < totalAxisLabelHeight)
                {
                    label.ActualHeight = InternalMaxHeight;
                }
                else
                    label.ActualHeight = totalAxisLabelHeight;

                // get the max height of the labels
                height = Math.Max(height, label.ActualHeight);

                if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight >= label.ActualHeight && InternalMaxHeight > _maxRowHeight)
                {
                    height = Math.Max(height, _maxRowHeight);
                }
            }

            height += (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0) + LEFT_TOP_PADDING;

            for (Int32 i = 0; i < AxisLabelList.Count; i++)
            {
                AxisLabel label = AxisLabelList[i];

                CustomAxisLabel customLabel = CustomAxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate vertical Position
                Double top = 0;
                if (GetAngle() != 0)
                {
                    top = Math.Abs((label.ActualTextHeight / 2) * Math.Sin(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                Double topPos = height * (Int32)internalRows - top - ((i % (Int32)internalRows) * _maxRowHeight) + Padding.Top - LEFT_TOP_PADDING;

                // Set the new position
                label.Position = new Point(position, topPos - (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0));

                // Create the visual element again
                label.SetPosition();

                Double fromPosition = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, label.From);
                Double toPosition = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, label.To);

                if (LineEnabled)
                {
                    Path path = GetPath(new Point(fromPosition, topPos), new Point(toPosition, topPos), Placement, customLabel);

                    customLabel.CustomLabelPath = path;

                    Visual.Children.Add(path);
                }

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);
            }

            // set the height of the visual canvas
            Visual.Height = height * (Int32)internalRows + Padding.Top;

            // calculate the overflow due to this set of axis labels
            CalculateHorizontalOverflow();
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

                CustomAxisLabel customLabel = CustomAxisLabelList[i];

                //Set size affecting font properties
                ApplyAxisLabelFontProperties(label, customLabel);

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, LabelValues[i]);

                // Create the visual element again
                label.CreateVisualObject(_tag);

                customLabel.TextElement = label.Visual;

                //Calculate horizontal Position
                Double left = 1;
                if (GetAngle() != 0)
                {
                    left = Math.Abs((label.ActualTextHeight / 2) * Math.Cos(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                Double leftPos = left + Padding.Left + LEFT_TOP_PADDING;

                // Set the new position
                label.Position = new Point(leftPos + (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0), position);

                // Create the visual element again
                label.SetPosition();

                Double fromPosition = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, label.From);
                Double toPosition = Graphics.ValueToPixelPosition(Height - endOffset, startOffset, Minimum, Maximum, label.To);

                if (LineEnabled)
                {
                    Path path = GetPath(new Point(leftPos, fromPosition), new Point(leftPos, toPosition), Placement, customLabel);

                    customLabel.CustomLabelPath = path;

                    Visual.Children.Add(path);
                }

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);

                Double totalAxisLabelWidth = 0;
                if (InternalMinWidth != 0 && InternalMinWidth > label.ActualWidth)
                    totalAxisLabelWidth = InternalMinWidth;
                else
                    totalAxisLabelWidth = label.ActualWidth;

                if (!Double.IsPositiveInfinity(InternalMaxWidth) && InternalMaxWidth < totalAxisLabelWidth)
                {
                    label.ActualWidth = InternalMaxWidth;
                }
                else
                    label.ActualWidth = totalAxisLabelWidth;

                // get the max width of the labels
                width = Math.Max(width, label.ActualWidth);

            }

            // set the width of the visual canvas
            Visual.Width = width + Padding.Right + (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0) + LEFT_TOP_PADDING;

            // calculate the overflow due to this set of axis labels
            CalculateVerticalOverflow();
        }

        /// <summary>
        /// Labels will be positioned for the axis that will appear to the bottom of the plot area
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

            //// Variable to calculate the height of the visual canvas
            Double height = 0;

            // Calculate Default placement values
            CalculateHorizontalDefaults(Width, Height);

            Double internalRows = 1;

            // Calculate the height of the labels and position them
            for (Int32 i = 0; i < AxisLabelList.Count; i += (ParentAxis.SkipOffset + 1))
            {
                AxisLabel label = AxisLabelList[i];

                CustomAxisLabel customLabel = CustomAxisLabelList[i];

                // get the position of the label
                Double position = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, LabelValues[i]);

                //Calculate vertical Position
                Double top = 0;

                if (GetAngle() != 0)
                {
                    top = Math.Abs((label.ActualTextHeight / 2) * Math.Sin(Math.PI / 2 - AxisLabel.GetRadians(GetAngle())));
                }

                Double totalAxisLabelHeight = 0;
                if (InternalMinHeight != 0 && InternalMinHeight > label.ActualHeight)
                    totalAxisLabelHeight = InternalMinHeight;
                else
                    totalAxisLabelHeight = label.ActualHeight;

                if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight < totalAxisLabelHeight)
                {
                    label.ActualHeight = InternalMaxHeight;
                }
                else
                    label.ActualHeight = totalAxisLabelHeight;

                // get the max height of the labels
                height = Math.Max(height, label.ActualHeight);

                if (Double.IsPositiveInfinity(InternalMaxHeight))
                {
                    height = Math.Max(height, _maxRowHeight);
                }

                if (!Double.IsPositiveInfinity(InternalMaxHeight) && InternalMaxHeight >= label.ActualHeight && InternalMaxHeight > _maxRowHeight)
                {
                    height = Math.Max(height, _maxRowHeight);
                }

                Double topPos = top + ((i % internalRows) * _maxRowHeight) + LEFT_TOP_PADDING;

                // Set the new position
                label.Position = new Point(position, topPos + (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0));

                // Create the visual element again
                label.SetPosition();

                Double fromPosition = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, label.From);
                Double toPosition = Graphics.ValueToPixelPosition(startOffset, Width - endOffset, Minimum, Maximum, label.To);

                if (LineEnabled)
                {
                    Path path = GetPath(new Point(fromPosition, topPos), new Point(toPosition, topPos), Placement, customLabel);

                    customLabel.CustomLabelPath = path;

                    Visual.Children.Add(path);
                }

                // add the element to the visual canvas
                Visual.Children.Add(label.Visual);
            }

            // set the height of the visual canvas
            Visual.Height = height + Padding.Bottom + (LineEnabled ? LINE_HEIGHT + LINE_AND_LABEL_GAP : 0) + LEFT_TOP_PADDING;
            CalculateHorizontalOverflow();
        }

        /// <summary>
        /// This is for axis with placement setting as top or bottom
        /// </summary>
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

            if ((Boolean)(Chart as Chart).ScrollingEnabled && Width > ParentAxis.Width)
            {
                LeftOverflow = 0;
                RightOverflow = 0;
            }
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

            if ((Boolean)(Chart as Chart).ScrollingEnabled && Height > ParentAxis.Height)
            {
                TopOverflow = 0;
                BottomOverflow = 0;
            }
        }

        /// <summary>
        /// Returns the proper angle value for calculation
        /// </summary>
        private Double GetAngle()
        {
            return Double.IsNaN((Double)this.InternalAngle) ? 0 : (Double)this.InternalAngle;
        }

        /// <summary>
        /// Calculates default font size based on a scaling criteria
        /// </summary>
        /// <param name="area">Double</param>
        /// <returns>FontSize as Double</returns>
        private Double CalculateFontSize(Double area)
        {
            if (Double.IsNaN(InternalFontSize) || InternalFontSize <= 0)
            {
                return Graphics.DefaultFontSizes[1];
            }
            else
                return InternalFontSize;
        }

        /// <summary>
        /// Get max height of CustomAxisLabels
        /// </summary>
        private Double CalculateMaxHeightOfLabels()
        {
            return CalculateMaxHeightOfLabels(null);
        }

        /// <summary>
        /// Get max height of CustomAxisLabels
        /// </summary>
        /// <param name="labelWidths">If List of Widths are required. You can just pass a ref of a list of Double</param>
        /// <returns></returns>
        private Double CalculateMaxHeightOfLabels(List<Double> labelWidths)
        {
            Double maxRowHeight = 0;

            // Calculate MaxHeight of CustomAxisLabels
            for (Int32 labelIndex = 0; labelIndex < AxisLabelList.Count; labelIndex += (ParentAxis.SkipOffset + 1))
            {
                AxisLabel label = AxisLabelList[labelIndex];

                CustomAxisLabel customLabel = CustomAxisLabelList[labelIndex];

                ApplyAxisLabelFontProperties(label, customLabel);

                label.CreateVisualObject(null);

                customLabel.TextElement = label.Visual;

                maxRowHeight = Math.Max(maxRowHeight, label.ActualHeight);

                if (labelWidths != null)
                    labelWidths.Add(label.ActualWidth);
            }

            return maxRowHeight;
        }

        /// <summary>
        /// Calculate default values for angle and rows of axis
        /// </summary>
        private void CalculateHorizontalDefaults(Double widthOfAxis, Double heightOfAxis)
        {
            IsNotificationEnable = false;

            widthOfAxis = Double.IsNaN(widthOfAxis) ? 0 : widthOfAxis;
            heightOfAxis = Double.IsNaN(heightOfAxis) ? 0 : heightOfAxis;

            Double max = Math.Max(widthOfAxis, heightOfAxis);

            if (Double.IsNaN(InternalFontSize) || InternalFontSize <= 0)
            {
                Double initialFontSize = CalculateFontSize(max);
                InternalFontSize = initialFontSize;
            }

            _maxRowHeight = CalculateMaxHeightOfLabels();

            IsNotificationEnable = true;
        }

        /// <summary>
        ///  Calculate default values for vertical axis
        /// </summary>
        private void CalculateVerticalDefaults()
        {
            Double width = Double.IsNaN(Width) ? 0 : Width;
            Double height = Double.IsNaN(Height) ? 0 : Height;
            Double max = Math.Max(width, height);
            if (Double.IsNaN(InternalFontSize) || InternalFontSize <= 0)
            {
                InternalFontSize = CalculateFontSize(max);
            }
        }

        #endregion

        #region Internal Properties

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
        /// MaxHeight of the Axis
        /// </summary>
        internal Double InternalMaxHeight
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMaxHeight) ? GetValue(MaxHeightProperty) : _internalMaxHeight);
            }
            set
            {
                _internalMaxHeight = value;
            }
        }

        /// <summary>
        /// MaxWidth of the Axis
        /// </summary>
        internal Double InternalMaxWidth
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMaxWidth) ? GetValue(MaxWidthProperty) : _internalMaxWidth);
            }
            set
            {
                _internalMaxWidth = value;
            }
        }

        /// <summary>
        /// MinHeight of the Axis
        /// </summary>
        internal Double InternalMinHeight
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMinHeight) ? GetValue(MinHeightProperty) : _internalMinHeight);
            }
            set
            {
                _internalMinHeight = value;
            }
        }

        /// <summary>
        /// MinWidth of the Axis
        /// </summary>
        internal Double InternalMinWidth
        {
            get
            {
                return (Double)(Double.IsNaN(_internalMinWidth) ? GetValue(MinWidthProperty) : _internalMinWidth);
            }
            set
            {
                _internalMinWidth = value;
            }
        }

        /// <summary>
        /// Get or set the FontFamily property of title
        /// </summary>
        internal FontFamily InternalFontFamily
        {
            get
            {
                FontFamily retVal;

                if (_internalFontFamily == null)
                    retVal = (FontFamily)GetValue(FontFamilyProperty);
                else
                    retVal = _internalFontFamily;

                return (retVal == null) ? new FontFamily("Verdana") : retVal;
            }
            set
            {

                _internalFontFamily = value;
            }
        }

        /// <summary>
        /// Get or set the FontSize property of title
        /// </summary>
        internal Double InternalFontSize
        {
            get
            {
                return (Double)(Double.IsNaN(_internalFontSize) ? GetValue(FontSizeProperty) : _internalFontSize);
            }
            set
            {
                _internalFontSize = value;
            }
        }

        /// <summary>
        /// Get or set the FontStyle property of title text
        /// </summary>
#if WPF
        [TypeConverter(typeof(System.Windows.FontStyleConverter))]
#endif
        internal FontStyle InternalFontStyle
        {
            get
            {
                return (FontStyle)((_internalFontStyle == null) ? GetValue(FontStyleProperty) : _internalFontStyle);
            }
            set
            {
                _internalFontStyle = value;
            }
        }

        /// <summary>
        /// Get or set the FontWeight property of title text
        /// </summary>
#if WPF
         [System.ComponentModel.TypeConverter(typeof(System.Windows.FontWeightConverter))]
#endif
        internal FontWeight InternalFontWeight
        {
            get
            {
                return (FontWeight)((_internalFontWeight == null) ? GetValue(FontWeightProperty) : _internalFontWeight);
            }
            set
            {
                _internalFontWeight = value;
            }
        }

        /// <summary>
        /// Get or set the Opacity property
        /// </summary>
        internal Double InternalOpacity
        {
            get
            {
                return (Double)(Double.IsNaN(_internalOpacity) ? GetValue(OpacityProperty) : _internalOpacity);
            }
            set
            {
                _internalOpacity = value;
            }
        }


        /// <summary>
        /// Average width of a character after applying 
        /// </summary>
        internal Double WidthOfACharacter
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the axis labels angle
        /// </summary>
        internal Nullable<Double> InternalAngle
        {
            get;
            set;
        }

        internal Canvas Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Number of pixels by which the top of the axis labels has overshot the actual canvas top
        /// </summary>
        internal Double TopOverflow
        {
            get;
            set;
        }

        /// <summary>
        /// Number of pixels by which the bottom of the axislabels has overshot the actual canvas bottom
        /// </summary>
        internal Double BottomOverflow
        {
            get;
            set;
        }

        /// <summary>
        /// Number of pixels by which the left of the axislabels has overshot the actual canvas left
        /// </summary>
        internal Double LeftOverflow
        {
            get;
            set;
        }

        /// <summary>
        /// Number of pixels by which the right of the axislabels has overshot the actual canvas right
        /// </summary>
        internal Double RightOverflow
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the width of the axis labels canvas, will be used only with the Horizontal axis
        /// </summary>
        internal new Double Width
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the height of the axis labels canvas, will be used ony with the vertical axis 
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
        /// Reference to the axis which holds this axis labels
        /// </summary>
        internal Axis ParentAxis
        {
            get;
            set;
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
        /// List of custom axis labels
        /// </summary>
        internal List<CustomAxisLabel> CustomAxisLabelList
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

        #region Private Properties

#if SL

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontSize dependency property.
        /// </returns>
        private static readonly DependencyProperty
            InternalFontSizeProperty = DependencyProperty.Register
            ("InternalFontSize",
            typeof(Double),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontSizePropertyChanged));

        /// Identifies the Visifire.Charts.CustomAxisLabels.FontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontFamily dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontFamilyProperty = DependencyProperty.Register
            ("InternalFontFamily",
            typeof(FontFamily),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontFamilyPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontStyle dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontStyleProperty = DependencyProperty.Register
            ("InternalFontStyle",
            typeof(FontStyle),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.FontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.FontWeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalFontWeightProperty = DependencyProperty.Register
            ("InternalFontWeight",
            typeof(FontWeight),
            typeof(CustomAxisLabels),
            new PropertyMetadata(OnFontWeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.Opacity dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalOpacityProperty = DependencyProperty.Register
            ("InternalOpacity",
            typeof(Double),
            typeof(CustomAxisLabels),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.MaxHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.MaxHeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMaxHeightProperty = DependencyProperty.Register
           ("InternalMaxHeight",
           typeof(Double),
           typeof(CustomAxisLabels),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.MaxWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.MaxWidth dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMaxWidthProperty = DependencyProperty.Register
           ("InternalMaxWidth",
           typeof(Double),
           typeof(CustomAxisLabels),
           new PropertyMetadata(Double.PositiveInfinity, OnMaxWidthPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.MinHeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.MinHeight dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMinHeightProperty = DependencyProperty.Register
           ("InternalMinHeight",
           typeof(Double),
           typeof(CustomAxisLabels),
           new PropertyMetadata(OnMinHeightPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.MinWidth dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.MinWidth dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalMinWidthProperty = DependencyProperty.Register
           ("InternalMinWidth",
           typeof(Double),
           typeof(CustomAxisLabels),
           new PropertyMetadata(OnMinWidthPropertyChanged));


#endif

        /// <summary>
        /// Identifies the Visifire.Charts.CustomAxisLabels.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.CustomAxisLabels.ToolTipText dependency property.
        /// </returns>
        private new static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register
            ("ToolTipText",
            typeof(String),
            typeof(CustomAxisLabels),
            null);

        #endregion

        #region Internal Methods

        /// <summary>
        /// Update visual used for partial update
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal override void UpdateVisual(VcProperties propertyName, object value)
        {
            if (Visual != null)
            {
                //foreach (AxisLabel axisLabel in AxisLabelList)
                //{
                //    ApplyAxisLabelFontProperties(axisLabel);
                //    axisLabel.ApplyProperties(axisLabel);
                //}

                for (Int32 i = 0; i < AxisLabelList.Count; i++)
                {
                    AxisLabel label = AxisLabelList[i];

                    CustomAxisLabel customLabel = CustomAxisLabelList[i];

                    ApplyAxisLabelFontProperties(label, customLabel);
                    label.ApplyProperties(label);
                }
            }
            else
                FirePropertyChanged(propertyName);
        }

        /// <summary>
        /// Creates a visual object for the element
        /// </summary>
        internal void CreateVisualObject()
        {
            // Create a new Canvas
            Visual = new Canvas();
           
            if (!(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            // Create new Labels list
            AxisLabelList = new List<AxisLabel>();

            CustomAxisLabelList = new List<CustomAxisLabel>();

            // List to store the values for which the labels are created
            LabelValues = new List<Double>();

            if (InternalFontSize != _savedFontSize || InternalAngle != _savedAngle)
                _isRedraw = false;

            // check if this is a first time draw or a redraw
            if (_isRedraw)
            {
                // if redraw then restore the original values
                InternalAngle = _savedAngle;
                InternalFontSize = _savedFontSize;
            }
            else
            {
                // Preserve the original values for future use
                _savedAngle = (Double)InternalAngle;
                _savedFontSize = InternalFontSize;
                _isRedraw = true;
            }

            // create the required labels
            CreateLabels();

            // Set the position of the labels
            SetLabelPosition();

            Visual.Opacity = InternalOpacity;
        }

        #endregion

        #region Internal Events

        #endregion

        #region Data

        internal const string RootElementName = "RootElement";
        internal Canvas _rootElement;

        /// <summary>
        /// Constant gap between CustomAxisLabels
        /// </summary>
        private const Double CONSTANT_GAP_BETWEEN_AXISLABELS = 4;

        private const Double LINE_HEIGHT = 5;

        private const Double LINE_AND_LABEL_GAP = 5;

        private const Double LEFT_TOP_PADDING = 3;

        /// <summary>
        /// Saved max row height
        /// </summary>
        private Double _maxRowHeight;

        /// <summary>
        /// Saved font size of CustomAxisLabels
        /// </summary>
        private Double _savedFontSize;

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

        private Double _internalFontSize = Double.NaN;
        private FontFamily _internalFontFamily = null;
        internal Brush InternalFontColor;
        Nullable<FontStyle> _internalFontStyle = null;
        Nullable<FontWeight> _internalFontWeight = null;
        Double _internalOpacity = Double.NaN;

        Double _internalMaxHeight = Double.NaN;
        Double _internalMaxWidth = Double.NaN;
        Double _internalMinHeight = Double.NaN;
        Double _internalMinWidth = Double.NaN;

        ElementData _tag;

#if WPF

        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;
#endif
        #endregion

    }
}