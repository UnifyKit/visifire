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

namespace Visifire.Charts
{

    public enum OCHL
    {   
        Open = 0,
        Close = 1,
        High = 2,
        Low = 3
    }

    /// <summary>
    /// Export file types
    /// </summary>
    public enum ExportType
    {   
        Jpg = 0,
        Bmp = 1
    }
    
    /// <summary>
    /// Fill types
    /// </summary>
    public enum FillType
    {   
        Solid, Hollow
    }

    /// <summary>
    /// Modes of selection
    /// </summary>
    public enum SelectionModes
    {
        Single = 0,
        Multiple = 1
    }

    /// <summary>
    /// Axis types of a series
    /// </summary>
    public enum ChartValueTypes
    {
        Auto = 0,
        Numeric = 1,
        Date = 2,
        DateTime = 3,
        Time = 4
    }

    /// <summary>
    /// Specifies an interval type.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public enum IntervalTypes
    {
        /// <summary>
        /// Automatically determined by the ISeriesHost control.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// The interval type is numerical.
        /// </summary>
        Number = 1,

        /// <summary>
        /// The interval type is years.
        /// </summary>
        Years = 2,

        /// <summary>
        /// The interval type is months.
        /// </summary>
        Months = 3,

        /// <summary>
        /// The interval type is weeks.
        /// </summary>
        Weeks = 4,

        /// <summary>
        /// The interval type is days.
        /// </summary>
        Days = 5,

        /// <summary>
        /// The interval type is hours.
        /// </summary>
        Hours = 6,

        /// <summary>
        /// The interval type is minutes.
        /// </summary>
        Minutes = 7,

        /// <summary>
        /// The interval type is seconds.
        /// </summary>
        Seconds = 8,

        /// <summary>
        /// The interval type is milliseconds.
        /// </summary>
        Milliseconds = 9,
    }

    /// <summary>
    /// Defines the type of the axis
    /// </summary>
    public enum AxisTypes 
    { 
        Primary = 0, 
        Secondary = 1 
    }

    /// <summary>
    /// Defines relative placement type for the axis elements
    /// </summary>
    internal enum PlacementTypes 
    {
        Top = 0,
        Left = 1,
        Right = 2,
        Bottom = 3 
    }

    /// <summary>
    /// Defines enum for use with axis manager
    /// </summary>
    internal enum MantissaOrExponent 
    { 
        Mantissa = 1, 
        Exponent = 2 
    }
    
    /// <summary>
    /// This property is used to select the chart type
    /// </summary>
    public enum RenderAs
    {
        Column = 0,
        Line = 1,
        Pie = 2,
        Bar = 3,
        Area = 4,
        Doughnut = 5,
        StackedColumn = 6,
        StackedColumn100 = 7,
        StackedBar = 8,
        StackedBar100 = 9,
        StackedArea = 10,
        StackedArea100 = 11,
        Bubble = 12,
        Point = 13,
        StreamLineFunnel = 14,
        SectionFunnel = 15,
        Stock = 16,
        CandleStick = 17,
        StepLine = 18
    }

    /// <summary>
    /// Styles for labels
    /// </summary>
    public enum LabelStyles
    {
        OutSide = 0, 
        Inside = 1 
    }

    /// <summary>
    /// Styles for line
    /// </summary>
    public enum LineStyles
    {
        Solid = 0,
        Dashed = 1,
        Dotted = 2
    }

    /// <summary>
    /// Defines the various chart orientations
    /// </summary>
    internal enum ChartOrientationType 
    {
        Undefined = 0,     // Undefined - not yet assigned 
        Vertical = 1,      // Vertical - charts of type point, bubble line, column(all similar types), area(all similar types)
        Horizontal = 2,    // Horizontal - charts of type bar(all similar types)
        NoAxis = 3         // NoAxis - charts of type pie or doughnut
    }

    /// <summary>
    /// Axis representations as X wise or Y wise
    /// </summary>
    internal enum AxisRepresentations 
    { 
        AxisX = 0, 
        AxisY = 1 
    };

    /// <summary>
    /// Visifire elements
    /// </summary>
    public enum Elements
    {   
        Chart = 0, 
        Title = 1, 
        DataSeries = 2, 
        DataPoint = 3, 
        AxisX = 4, 
        AxisY = 5, 
        Legend = 6
    }

    /// <summary>
    /// Layout of legends
    /// </summary>
    public enum Layouts
    {   
        Auto = 0,
        FlowLayout = 1,
        GridLayout = 2
    }

    public enum VcProperties
    {
        None,
        Angle,
        AxesX,
        AxesY,
        AxisLabels,
        AxisMaximum,
        AxisMinimum,
        AxisType,
        AxisXLabel,
        AxisXType,
        AxisYType,
        Background,
        Bevel,
        BorderColor,
        BorderStyle,
        BorderThickness,
        Color,
        ColorSet,
        CornerRadius,
        ClosestPlotDistance,
        Cursor,
        DataPoints,
        DockInsidePlotArea,
        Enabled,
        Exploded,
        EntryMargin,
        EndValue,
        FontColor,
        FontFamily,
        FontSize,
        FontStyle,
        FontWeight,
        Grids,
        HorizontalAlignment,
        Href,
        HrefTarget,
        IncludeZero,
        InterlacedColor,
        Interval,
        IntervalType,
        IncludeYValueInLegend,
        IncludePercentageInLegend,
        LabelBackground,
        LabelEnabled,
        LabelFontColor,
        LabelFontFamily,
        LabelFontSize,
        LabelFontStyle,
        LabelFontWeight,
        LabelLineColor,
        LabelLineEnabled,
        LabelLineStyle,
        LabelLineThickness,
        LabelMargin,
        LabelStyle,
        LabelText,
        LabelAngle,
        Legend,
        LegendText,
        LightingEnabled,
        LineColor,
        LineStyle,
        LineThickness,
        Margin,
        MarkerBorderColor,
        MarkerBorderThickness,
        MarkerColor,
        MarkerEnabled,
        MarkerScale,
        MarkerSize,
        MarkerType,
        MaxWidth,
        MaxHeight,
        MinWidth,
        MinHeight,
        MinPointHeight,
        MouseEvent,
        MovingMarkerEnabled,
        Opacity,
        Orientation,
        Padding,
        Prefix,
        PriceDownColor,
        PriceUpColor,
        RadiusX,
        RadiusY,
        RenderAs,
        Reversed,
        Rows,
        Series,
        ScalingSet,
        ShadowEnabled,
        ShowInLegend,
        StartAngle,
        StartFromZero,
        Suffix,
        ScrollBarScale,
        ScrollBarSize,
        ScrollBarOffset,
        StartValue,
        Text,
        TextAlignment,
        TextWrap,
        TickLength,
        Ticks,
        Title,
        TitleAlignmentX,
        TitleBackground,
        TitleFontColor,
        TitleFontFamily,
        TitleFontSize,
        TitleFontStyle,
        TitleFontWeight,
        TitleTextAlignment,
        ToolTipText,
        TrendLines,
        Value,
        ValueFormatString,
        VerticalAlignment,
        XValue,
        XValueFormatString,
        XValues,
        XValueType,
        YValue,
        YValueFormatString,
        YValues,
        ZIndex,
        ZValue,
        ZValueFormatString,
    }
}

namespace Visifire.Commons
{
    /// <summary>
    /// Href target window types
    /// </summary>
    public enum HrefTargets
    {
        _self = 0,
        _blank = 1,
        _media = 2,
        _parent = 3,
        _search = 4,
        _top = 5
    }

    /// <summary>
    /// Types of marker
    /// </summary>
    public enum MarkerTypes
    {
        Circle = 0, 
        Square = 1, 
        Triangle = 2, 
        Cross = 3, 
        Diamond = 4, 
        Line = 5
    }
    
    /// <summary>
    /// Style of _axisIndicatorBorderElement
    /// </summary>
    public enum BorderStyles
    {   
        Solid = 0,
        Dashed = 1,
        Dotted = 2
    }

    /// <summary>
    /// Pie face Types
    /// </summary>
    public enum PieFaceTypes
    {
        Top=0, Bottom=1, Left=2, Right=3, CurvedSurface=4
    }

}