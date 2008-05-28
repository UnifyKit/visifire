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

using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Visifire.Commons;

namespace Visifire.Charts
{
    public abstract class Axes : VisualObject
    {
        #region Public Methods
        /// <summary>
        /// Constructor for the axes class
        /// </summary>
        public Axes()
        {
            
        }

        /// <summary>
        /// This function adds a axis line to the children list
        /// </summary>
        public new void Render()
        {
            if(Enabled)
                this.Children.Add(_line);
        }

        /// <summary>
        /// Sets the width of the Axis
        /// </summary>
        public override void SetWidth()
        {
            if (AxisOrientation == AxisOrientation.Bar)
            {
                if (!Enabled)
                {
                    this.Width = 0;
                    return;
                }
                if (_parent.View3D)
                {
                    if (this.GetType().Name == "AxisX")
                        this.Width = (Double)AxisLabels.GetValue(LeftProperty) + AxisLabels.Width + _majorTicks.TickLength + PlankThickness + _parent.Padding;
                    else
                        this.Width = (Double)AxisLabels.GetValue(LeftProperty) + AxisLabels.Width + _parent.AxisX.MajorTicks.TickLength;
                }
                else
                {
                    this.Width = (Double)AxisLabels.GetValue(LeftProperty) + AxisLabels.Width + _majorTicks.TickLength;
                }
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                this.Width = _parent.PlotArea.Width;
            }
        }

        /// <summary>
        /// Sets the height of the axis
        /// </summary>
        public override void SetHeight()
        {

            if (AxisOrientation == AxisOrientation.Bar)
            {
                this.Height = _parent.PlotArea.Height;
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                if (!Enabled)
                {
                    // If axis is not enabled then reset height and return
                    this.Height = 0;
                    return;
                }

                if (_parent.View3D)
                {
                    if (this.GetType().Name == "AxisX")
                        this.Height = AxisLabels.Height + _majorTicks.TickLength + PlankThickness;
                    else
                        this.Height = (Double)AxisLabels.GetValue(TopProperty) + AxisLabels.Height + _parent.Padding;
                }
                else
                {
                    this.Height = AxisLabels.Height + _majorTicks.TickLength / 2;
                }

                // if Axis Title is given by user the increase the Axis height to include it
                if (_titleTextBlock.Text.Length != 0)
                    this.Height = this.Height + _titleTextBlock.ActualHeight;
            }
        }

        /// <summary>
        /// Sets the left of the axis
        /// </summary>
        public override void SetLeft()
        {
            if (AxisOrientation == AxisOrientation.Bar)
            {
                Double tempLeft = _parent.Padding;

                SetValue(LeftProperty, _parent._innerBounds.Left);

                // if there is overflow due to long labels then adjust position accordingly
                if (_parent.LabelPaddingLeft > (Double)this.GetValue(LeftProperty) + (Double)this.GetValue(WidthProperty))
                    this.SetValue(LeftProperty, _parent.LabelPaddingLeft - (Double)this.GetValue(WidthProperty) + _parent.Padding);
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                this.SetValue(LeftProperty, _parent.PlotArea.GetValue(LeftProperty));

            }
        }

        /// <summary>
        /// Sets the top of the axis
        /// </summary>
        public override void SetTop()
        {
            Double tempTop = _parent.Height - _parent.Padding;

            if (AxisOrientation == AxisOrientation.Bar)
            {
                this.SetValue(TopProperty, _parent.PlotArea.GetValue(TopProperty));

                // if there is overflow due to long labels then adjust position accordingly
                if (_parent.LabelPaddingTop > (Double)this.GetValue(TopProperty))
                    this.SetValue(TopProperty, _parent.LabelPaddingTop);
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {

                SetValue(TopProperty, _parent._innerBounds.Bottom - Height - _parent.Padding);

                // if there is overflow due to long labels then adjust position accordingly
                if (_parent.LabelPaddingBottom > (_parent.Height - (Double)this.GetValue(TopProperty)))
                    this.SetValue(TopProperty, tempTop - _parent.LabelPaddingBottom - _parent.Padding);
            }

        }

        /// <summary>
        /// This function is used to prepare the object before rendering
        /// </summary>
        public override void Init()
        {
            // Checks if the parent for this object is valid
            ValidateParent();

            // sets a default name to the object
            SetName();

            // sets tag for all visual elements for this class
            SetTags();

            //Sets the default font size
            if (Double.IsNaN(_titleFontSize))
                _titleTextBlock.FontSize = CalculateTitleFontSize();

            // finds all the legend elements and makes all the references
            CreateReferences();

            // Set specific settings for axisX
            if (this.GetType().Name == "AxisX") 
                AxisXSpecificSettings();

            // Set axis limit 
            if ( Double.IsInfinity(MaxDataValue) || Double.IsInfinity(MinDataValue) )
            {
                // if AxisLabel limit is Not already available generate temporary limits
                // This occours if there are no DataSeries
                if (this.GetType().Name == "AxisX")
                {
                    Double xInterval = (Double.IsNaN(_interval) ? ChartConstants.AxisX.NoData.Interval : _interval);
                    Double maxXValue = (Double.IsNaN(AxisMaximum) ? ChartConstants.AxisX.NoData.MaxValue : AxisMaximum - xInterval);
                    Double minXValue = (Double.IsNaN(AxisMinimum) ? ChartConstants.AxisX.NoData.MinValue : AxisMinimum + xInterval);
                    
                    _axisManager = new AxisManager((Decimal)maxXValue, (Decimal)minXValue, StartFromZero,true);
                    _axisManager.Interval = (Decimal)xInterval;
                }
                else
                {
                    Double yInterval = (Double.IsNaN(_interval) ? ChartConstants.AxisY.NoData.Interval: _interval);
                    Double maxYValue = (Double.IsNaN(AxisMaximum) ? ChartConstants.AxisY.NoData.MaxValue : AxisMaximum - yInterval);
                    Double minYValue = (Double.IsNaN(AxisMinimum) ? ChartConstants.AxisY.NoData.MinValue : AxisMinimum + yInterval);

                    _axisManager = new AxisManager((Decimal)maxYValue, (Decimal)minYValue, StartFromZero,false);
                    _axisManager.Interval = (Decimal)yInterval;
                }
            }
            else
            {
                // If axis limit is available then use it
                _axisManager = new AxisManager((Decimal)MaxDataValue, (Decimal)MinDataValue, StartFromZero, this.GetType().Name == "AxisX");
            }

            // if user has given interval then apply it
            if (Interval > 0)
                AxisManager.Interval = (Decimal)Interval;

            // apply current IncludeZero setting
            AxisManager.IncludeZero = IncludeZero;

            // if user has given axis minimum limit then apply it
            if (!Double.IsNaN(AxisMinimum))
                AxisManager.AxisMinimumValue = (Decimal)AxisMinimum;

            // if user has given axis maximum limit then apply it
            if (!Double.IsNaN(AxisMaximum))
                AxisManager.AxisMaximumValue = (Decimal)AxisMaximum;

            // calculate the axis parameters
            AxisManager.Calculate();

            // Settings specific to axis y
            if (this.GetType().Name == "AxisY")
            {
                AxisMinimum = (Double)AxisManager.GetAxisMinimumValue();
                AxisMaximum = (Double)AxisManager.GetAxisMaximumValue();
            }

            // initialize child elements
            AxisLabels.Init();
            MajorGrids.Init();
            MajorTicks.Init();

            // Attach toop tip and hyperlink
            AttachHref();
            AttachToolTip();
        }

        #endregion Public Methods

        #region Public Properties

        #region Title Font Properties
        /// <summary>
        /// Title for the Axis
        /// </summary>
        public String Title
        {
            get
            {
                return _titleTextBlock.Text;
            }
            set
            {
                _titleTextBlock.Text = value;
            }
        }

        /// <summary>
        /// Font color for the axis Title
        /// </summary>
        public Brush TitleFontColor
        {
            get
            {
                if (_titleFontColor != null)
                    return _titleFontColor;
                else
                    return GetFromTheme("TitleFontColor") as Brush;
            }
            set
            {
                _titleFontColor = value;
                _titleTextBlock.Foreground = Cloner.CloneBrush(_titleFontColor);
            }

        }

        /// <summary>
        /// Fint size of the axis Title
        /// </summary>
        public Double TitleFontSize
        {
            get
            {
                return _titleFontSize;
            }
            set
            {
                _titleFontSize = value;
                _titleTextBlock.FontSize = value;
            }
        }

        /// <summary>
        /// Font styles like (Normal,Oblique,Italic) for the axis Title
        /// </summary>
        public String TitleFontStyle
        {
            get
            {
                return _titleTextBlock.FontStyle.ToString();
            }
            set
            {
                _titleTextBlock.FontStyle = Converter.StringToFontStyle(value);
            }
        }

        /// <summary>
        /// Font weight for axis Title. Like (Thin,ExtraLight,Light,Normal,Medium,SemiBold,Bold,ExtraBold,Black,ExtraBlack)
        /// </summary>
        public String TitleFontWeight
        {
            get
            {
                return _titleTextBlock.FontWeight.ToString();
            }

            set
            {
                _titleTextBlock.FontWeight = Converter.StringToFontWeight(value);
            }
        }

        /// <summary>
        /// Font family for the axis Title. Like (Verdana, ...)
        /// </summary>
        public String TitleFontFamily
        {
            get
            {
                return _titleTextBlock.FontFamily.ToString();
            }
            set
            {
                _titleTextBlock.FontFamily = new FontFamily(value);
            }
        }

        #endregion Title Font Properties

        /// <summary>
        /// To set how frequently the Ticks, Grid Lines and Axis Labels have to appear
        /// </summary>
        public Double Interval
        {
            get
            {
                if (!Double.IsNaN(_interval))
                    return _interval;
                else
                    return (Double)AxisManager.GetInterval();
            }
            set
            {
                _interval = value;
            }
        }

        /// <summary>
        /// To fix the number of Ticks, Grid Lines and Axis Labels that have to appear
        /// </summary>
        public Int32 NumberOfIntervals
        {
            get
            {
                if (_noOfIntervals > 0)
                    return _noOfIntervals;
                else
                    return (Int32)AxisManager.GetNoOfIntervals();
            }
            set
            {
                _noOfIntervals = value;
            }
        }

        /// <summary>
        /// This decides how the given String gets formatted
        /// </summary>
        public String ValueFormatString
        {
            get
            {
                return _valueFormatString;
            }
            set
            {
                _valueFormatString = value;
            }
        }

        /// <summary>
        /// Any prefix to be added to the axis labels
        /// </summary>
        public String Prefix
        {
            get;
            set;
        }

        /// <summary>
        /// Any suffix to be added to the axis labels
        /// </summary>
        public String Suffix
        {
            get;
            set;
        }

        /// <summary>
        /// To set the angle by which the axis labels should tilt
        /// </summary>
        public Double LabelAngle
        {
            get
            {
                if (!Double.IsNaN(_labelAngle))
                    return _labelAngle;
                else
                    return 0;
            }
            set
            {
                _labelAngle = value;
            }
        }

        /// <summary>
        /// Scaling pairs will be in this format 1024,KB;1024,MB;1024,GB
        /// </summary>
        public String ScalingSets
        {
            set
            {
                String[] pairs = value.Split(';');
                Double scale = 1;
                Double parsedValue;
                
                for (Int32 i = 0; i < pairs.Length; i++)
                {
                    String[] sets = pairs[i].Split(',');
                    if (sets.Length != 2) continue;

                    parsedValue = Double.Parse(sets[0]);
                    _scaleValues.Add(parsedValue * scale);
                    _scaleUnits.Add(sets[1]);
                    scale *= parsedValue;
                    
                }
                ScalingEnabled = true;
            }
        }

        public Boolean ScalingEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// To force the inclusion of zero in the axis
        /// </summary>
        public Boolean IncludeZero
        {
            get;
            set;
        }

        public Boolean StartFromZero
        {
            get;

            set;

        }

        /// <summary>
        /// This property can set the alternating color Single color or Radial/Linear gradient type
        /// </summary>
        public String InterlacedColor
        {
            set
            {
                InterlacedBackground = Parser.ParseColor(value);
            }
        }

        #region Line Properties

        
        public String LineColor
        {
            set
            {
                LineBackground = Parser.ParseColor(value);
            }
        }

        public Double LineThickness
        {
            get
            {
                if (!Double.IsNaN(_lineThickness))
                    return _lineThickness;
                else if (GetFromTheme("LineThickness") != null)
                    return Convert.ToDouble(GetFromTheme("LineThickness"));
                else
                    return ChartConstants.Axes.Default.LineThickness;
            }
            set
            {
                _lineThickness = value;
            }
        }

        public String LineStyle
        {
            get
            {
                if (!String.IsNullOrEmpty(_lineStyle))
                    return _lineStyle;
                else
                    return Convert.ToString(GetFromTheme("LineStyle"));
            }

            set
            {
                _lineStyle = value;
            }
        }
        #endregion Line Properties

        public Double AxisMinimum
        {
            get
            {
                return _axisMinmum;
            }
            set
            {
                _axisMinmum = value;
            }
        }

        public Double AxisMaximum
        {
            get
            {
                return _axisMaximum;
            }
            set
            {
                _axisMaximum = value;
            }
        }

        #endregion Public Properties

        #region Internal Properties
        /// <summary>
        /// The color that must appear alternatively between grid lines
        /// </summary>
        internal Brush InterlacedBackground
        {
            get
            {
                if (_interlaceBackground != null)
                    return _interlaceBackground;
                else
                    return GetFromTheme("InterlacedColor") as Brush;
            }
            set
            {
                _interlaceBackground = value;
            }
        }

        internal Brush LineBackground
        {
            get
            {
                if (_lineBackground != null)
                    return _lineBackground;
                else
                    return GetFromTheme("LineColor") as Brush;
            }
            set
            {
                _lineBackground = value;
            }
        }

        internal AxisOrientation AxisOrientation
        {
            get
            {
                return _axisOrientation;
            }

            set
            {
                _axisOrientation = value;
            }
        }

        internal AxisLabels AxisLabels
        {
            get
            {
                return _axisLabels;
            }
            set
            {
                _axisLabels = value;
            }
        }

        internal AxisManager AxisManager
        {
            get
            {
                return _axisManager;
            }
        }

        internal MajorTicks MajorTicks
        {
            get
            {
                return _majorTicks;
            }
            set
            {
                _majorTicks = value;
            }
        }

        internal MajorGrids MajorGrids
        {
            get
            {
                return _majorGrids;
            }
            set
            {
                _majorGrids = value;
            }
        }

        internal Double MaxDataValue
        {
            get
            {
                return _maxDataValue;
            }
            set
            {
                _maxDataValue = value;
            }
        }

        internal Double MinDataValue
        {
            get
            {
                return _minDataValue;
            }
            set
            {
                _minDataValue = value;
            }
        }

        internal Double PlankThickness
        {
            get;
            set;
        }
        #endregion Internal properties

        #region Protected Methods
        /// <summary>
        /// This function is used to set the default values to the various attributes
        /// </summary>
        protected override void SetDefaults()
        {
            base.SetDefaults();

            // Initialize all local data(Axis Line)
            _line = new Line();

            // Axis title
            _titleTextBlock = new TextBlock();

            // default format String
            _valueFormatString = ChartConstants.Axes.Default.ValueFormatString;

            // default axis line color
            _lineBackground = null;
            _titleFontColor = null;

            // set title text to null
            Title = null;

            // default axis line tickness
            LineThickness = Double.NaN;

            // Number of intervals is set to a invalid value so that it can be verified later
            NumberOfIntervals = -1;

            Interval = Double.NaN;
            AxisMaximum = Double.NaN;
            AxisMinimum = Double.NaN;
            
            _labelAngle = Double.NaN;
            
            _titleFontSize = Double.NaN;
            
            Enabled = true;

            if (this.GetType().Name == "AxisY")
            {
                StartFromZero = true;
            }
            else
            {
                StartFromZero = false;
            }

            this.SetValue(ZIndexProperty, ChartConstants.Axes.Default.ZIndex);
        }

        #endregion Protected Methods

        #region Private Methods
        
        /// <summary>
        /// Generates font size for title . currently not used
        /// </summary>
        /// <returns></returns>
        private Int32 CalculateTitleFontSize()
        {
            Int32[] fontSizes = { 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };
            Double _parentSize = (Parent as Chart).Width * (Parent as Chart).Height;
            Int32 i = (Int32)(Math.Ceiling(((_parentSize + 10000) / 115000)));
            i = (i >= fontSizes.Length ? fontSizes.Length - 1 : i);
            return fontSizes[i];
        }

        private void CreateReferences()
        {
            foreach (FrameworkElement child in this.Children)
            {
                switch (child.GetType().Name)
                {
                    case "AxisLabels":
                        _axisLabels = child as AxisLabels;
                        break;

                    case "MajorTicks":
                        _majorTicks = child as MajorTicks;
                        break;

                    case "MajorGrids":

                        _majorGrids = child as MajorGrids;
                        break;
                }
            }

            if (AxisLabels == null)
            {
                _axisLabels = new AxisLabels();
                this.Children.Add(_axisLabels);
            }

            if (MajorTicks == null)
            {
                _majorTicks = new MajorTicks();
                this.Children.Add(_majorTicks);
            }

            if (MajorGrids == null)
            {
                _majorGrids = new MajorGrids();
                if (this.GetType().Name == "AxisX")
                    _majorGrids.Enabled = false;
                this.Children.Add(_majorGrids);
            }
        }

        
        /// <summary>
        /// Validate Parent element and assign it to _parent.
        /// Parent element should be a Charts element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            if (this.Parent is Chart)
                _parent = this.Parent as Chart;
            else
                throw new Exception(this + "Parent should be a Chart");
        }

        private void AxisXSpecificSettings()
        {

            // if axis is axisX then this sets the 3D depth of the plank
            if (_parent.View3D)
            {
                Double h = _parent.Height;
                Double w = _parent.Width;
                if (_parent.PlotDetails.AxisOrientation == AxisOrientation.Column)
                {
                    Double horizontalDepthFactor = ChartConstants.Plank.HorizontalDepthFactor;
                    Double horizontalThicknessFactor = ChartConstants.Plank.HorizontalThicknessFactor;
                    MajorTicks.TickLength = (h > w ? w : h) * (horizontalDepthFactor * _parent.Count);
                    PlankThickness = (h > w ? w : h) * (horizontalThicknessFactor);
                }
                else
                {
                    Double verticalDepthFactor = ChartConstants.Plank.VerticalDepthFactor;
                    Double verticalThicknessFactor = ChartConstants.Plank.VerticalThicknessFactor;
                    MajorTicks.TickLength = (h > w ? w : h) * (verticalDepthFactor * _parent.Count);
                    PlankThickness = (h > w ? w : h) * (verticalThicknessFactor);
                }
            }
        }

        /// <summary>
        /// Function returns Maxximum value in the set of min differences of each plot type
        /// </summary>
        /// <returns></returns>
        private Double GetMaxOfMinDifference()
        {
            Double max = 0;
            foreach (Plot p in _parent.PlotDetails.Plots)
            {
                max = Math.Max(max, p.MinDifference);
            }
            return max;
        }

        private Brush GetDefaultFontColor()
        {
            Brush fontColorBrush = null;
            Double intensity;

            if (this.Background == null)
            {
                if (_parent.Background == null)
                {
                    fontColorBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    intensity = Parser.GetBrushIntensity(_parent.Background);
                    fontColorBrush = Parser.GetDefaultFontColor(intensity);
                }
            }
            else
            {
                intensity = Parser.GetBrushIntensity(this.Background);
                fontColorBrush = Parser.GetDefaultFontColor(intensity);
            }

            return fontColorBrush;
        }

        private void SetTags()
        {
            _line.Tag = this.Name;
            _titleTextBlock.Tag = this.Name;
        }
        #endregion Private Methods

        #region Internal Methods

        /// <summary>
        /// Returns formatted text with Prefix and Suffix added to it
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal String GetFormattedText(Double value)
        {
            String str = value.ToString();
            if (ScalingEnabled && _scaleValues.Count>0 && _scaleUnits.Count>0 )
            {
                Double sValue = _scaleValues[0];
                String sUnit = _scaleUnits[0];
                for (Int32 i = 0; i < _scaleValues.Count; i++)
                {
                    if ((Math.Abs(value) / _scaleValues[i]) < 1)
                    {
                        break;
                    }
                    sValue = _scaleValues[i];
                    sUnit = _scaleUnits[i];
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

        internal void SetAxisLimits()
        {
            if (this.GetType().Name == "AxisX" && !StartFromZero)
            {
                Decimal gap;
                Double min = GetMaxOfMinDifference();
                Double temp;
                Decimal tempGap;
                if (_parent.TotalSiblings > 0)
                {
                    if (Double.IsNaN(min) || min == 0)
                    {
                        gap = (Decimal)Math.Abs((DoubleToPixel((Double)(AxisManager.Interval + AxisManager.AxisMinimumValue)) - DoubleToPixel((Double)AxisManager.AxisMinimumValue)) / _parent.TotalSiblings);
                    }
                    else
                    {
                        temp = DoubleToPixel((Double)((min < (Double)_parent.AxisX.Interval) ? min : (Double)AxisManager.Interval) + (Double)AxisManager.AxisMinimumValue);
                        gap = (Decimal)Math.Abs(temp - DoubleToPixel((Double)AxisManager.AxisMinimumValue)) / _parent.TotalSiblings;
                    }

                    tempGap = (Decimal)Math.Abs(DoubleToPixel((Double)AxisManager.GetMinimumDataValue()) - _parent.AxisX.DoubleToPixel((Double)AxisManager.AxisMinimumValue));

                    tempGap *= 2;
                    if (gap > tempGap)
                        gap = tempGap;

                    if (AxisOrientation == AxisOrientation.Column)
                    {
                        if (Double.IsNaN(AxisMinimum))
                            AxisManager.AxisMinimumValue = (Decimal)PixelToDouble(DoubleToPixel((Double)(AxisManager.GetMinimumDataValue())) - (Double)gap * 1.1 * _parent.TotalSiblings / 2);
                        else
                            AxisManager.AxisMinimumValue = (Decimal)AxisMinimum;
                    }
                    else if (AxisOrientation == AxisOrientation.Bar)
                    {
                        if(Double.IsNaN(AxisMinimum))
                            AxisManager.AxisMinimumValue = (Decimal)PixelToDouble(DoubleToPixel((Double)(AxisManager.GetMinimumDataValue())) + (Double)gap * 1.1 * _parent.TotalSiblings / 2);
                        else
                            AxisManager.AxisMinimumValue = (Decimal)AxisMinimum;
                    }
                }


                // This is a modification
                // This was done so that the space left by the left most datapoint and plot area must 
                // be same as the space left by the rightmost datapoint and plot area
                if(Double.IsNaN(AxisMaximum))
                {
                    if (AxisManager.AxisMaximumValue - AxisManager.GetMaximumDataValue() >= AxisManager.GetMinimumDataValue() - AxisManager.AxisMinimumValue)
                    {

                        // This part makes the gaps equal
                        AxisManager.AxisMaximumValue = AxisManager.GetMaximumDataValue() + AxisManager.GetMinimumDataValue() - AxisManager.AxisMinimumValue;
                    }
                }
                else
                {
                    AxisManager.AxisMaximumValue = (Decimal)AxisMaximum;
                }

            }



            AxisMinimum = (Double)AxisManager.GetAxisMinimumValue();
            AxisMaximum = (Double)AxisManager.GetAxisMaximumValue();
        }

        internal void PlaceTitle()
        {
            if (!Enabled) return;

            this.Children.Add(_titleTextBlock);

            if (_titleFontColor == null)
            {
                _titleTextBlock.Foreground = GetDefaultFontColor();
            }

            RotateTransform rt = new RotateTransform();
            Double offset = _parent.View3D ? _parent.AxisX.MajorTicks.TickLength : 0;

            if (AxisOrientation == AxisOrientation.Bar)
            {
                rt.Angle = -90;
                _titleTextBlock.RenderTransform = rt;

                _titleTextBlock.SetValue(TopProperty, (Double)this.GetValue(HeightProperty) / 2 + _titleTextBlock.ActualWidth / 2 + offset);
                _titleTextBlock.SetValue(LeftProperty, 0);
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                rt.Angle = 0;
                _titleTextBlock.RenderTransform = rt;

                _titleTextBlock.Margin = new Thickness(0);
                _titleTextBlock.SetValue(TopProperty, this.Height - _titleTextBlock.ActualHeight);
                _titleTextBlock.SetValue(LeftProperty, this.Width / 2 - _titleTextBlock.ActualWidth / 2);

            }
        }

        internal void FixTitleSize()
        {
            if (this.AxisOrientation == AxisOrientation.Bar)
            {
                _titleTextBlock.Width = _parent.Height * 0.8;
            }
            else
            {
                _titleTextBlock.Width = _parent.Width * 0.8;
            }
            _titleTextBlock.TextWrapping = TextWrapping.Wrap;
        }

        internal void DrawAxisLine()
        {
            if (AxisOrientation == AxisOrientation.Bar)
            {
                _line.X1 = 0 + (Double)this.GetValue(WidthProperty);
                _line.X2 = 0 + (Double)this.GetValue(WidthProperty);
                _line.Y1 = 0;
                _line.Y2 = (Double)this.GetValue(HeightProperty);
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                _line.X1 = 0;
                _line.X2 = 0 + (Double)this.GetValue(WidthProperty);
                _line.Y1 = 0;
                _line.Y2 = 0;

            }

            if (LineBackground == null)
                _line.Stroke = new SolidColorBrush(Colors.Gray);
            else
                _line.Stroke = Cloner.CloneBrush(LineBackground);


            _line.StrokeThickness = LineThickness;

            _line.StrokeDashArray = Parser.GetStrokeDashArray(this._lineStyle);
            
        }

        internal Double DoubleToPixel(Double value)
        {
            Double pixel;
            Double max = (Double)AxisManager.GetAxisMaximumValue();
            Double min = (Double)AxisManager.GetAxisMinimumValue();
            if (AxisOrientation == AxisOrientation.Bar)
            {
                pixel = this.Height - ((this.Height / (max - min)) * (value - min));
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                pixel = (this.Width / (max - min)) * (value - min);
            }
            else
                pixel = 0;

            return pixel;
        }

        internal Double PixelToDouble(Double value)
        {
            Double res;
            Double max = (Double)AxisManager.GetAxisMaximumValue();
            Double min = (Double)AxisManager.GetAxisMinimumValue();
            if (AxisOrientation == AxisOrientation.Bar)
            {
                res = min + (1 - value / this.Height) * (max - min) ;
            }
            else if (AxisOrientation == AxisOrientation.Column)
            {
                res = (max - min) / this.Width * value + min;
            }
            else
                res = 0;

            return res;
        }

        #endregion Internal Methods

        #region Data

        private Double _interval;
        internal Double _labelAngle;
        private Double _maxDataValue;
        private Double _minDataValue;
        private Double _axisMinmum;
        private Double _axisMaximum;

        private Int32 _noOfIntervals;

        private String _valueFormatString;

        internal Chart _parent;
        private Line _line;
        private Double _lineThickness;
        private Brush _lineBackground;
        private String _lineStyle;
        private AxisManager _axisManager;

        private AxisOrientation _axisOrientation;
        internal TextBlock _titleTextBlock;

        private AxisLabels _axisLabels;
        private MajorTicks _majorTicks;
        private MajorGrids _majorGrids;

        private Brush _interlaceBackground;
        private Brush _titleFontColor;
        private Double _titleFontSize;

        private List<Double> _scaleValues = new List<Double>();
        private List<String> _scaleUnits = new List<String>();
        #endregion Data
    }
}
