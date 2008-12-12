namespace Visifire.Charts
{
    /// <summary>
    /// Defines the type of the axis
    /// </summary>
    public enum AxisTypes { Primary, Secondary }

    /// <summary>
    /// Defines relative placement type for the axis elements
    /// </summary>
    internal enum PlacementTypes {Top,Left,Right,Bottom }

    /// <summary>
    /// Defines enum for use with axis manager
    /// </summary>
    internal enum MantissaOrExponent { Mantissa = 1, Exponent = 2 }

    public enum AnimationType
    {
        Type1, Type2, Type3, Type4, Type5, Type6
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
        Point = 13
    }

    public enum LabelStyles
    {
        OutSide = 0, Inside = 1 
    }

    public enum LineStyles
    {
        Solid = 0,
        Dashed = 1,
        Dotted = 2
    }

    /// <summary>
    /// defines the various chart orientations
    /// </summary>
    internal enum ChartOrientationType {
          Undefined,     // Undefined - not yet assigned 
          Vertical,      // Vertical - charts of type point, bubble line, column(all similar types), area(all similar types)
          Horizontal,    // Horizontal - charts of type bar(all similar types)
          NoAxis         // NoAxis - charts of type pie or doughnut
    }

    /// <summary>
    /// Axis representations as X wise or Y wise
    /// </summary>
    internal enum AxisRepresentations { AxisX, AxisY };

    public enum Elements
    {   
        Chart = 0, Title = 1, DataSeries = 2, DataPoint = 3, AxisX = 4, AxisY = 5, Legend = 6
    }

    public enum LegendLayouts
    {
        FlowLayout = 0,
        Gridlayout = 1
    }
}

namespace Visifire.Commons
{
    public enum HrefTargets
    {
        _self = 0, _blank = 1, _media = 2, _parent = 3, _search = 4, _top = 5
    }

    public enum MarkerTypes
    {
        Circle = 0, Square = 1, Triangle = 2, Cross = 3, Diamond = 4, Line = 5
    }
    
    public enum BorderStyles
    {   
        Solid = 0,
        Dashed = 1,
        Dotted = 2
    }

    //public enum ColorSetNames
    //{
    //    None = 0,
    //    DarkShades = 1,
    //    Visifire2 = 2,
    //    SandyShades = 3,
    //    Caravan = 4,
    //    Picasso = 5,
    //    DullShades = 6,
    //    Visifire1 = 7,
    //    VisiBlue = 8,
    //    VisiRed = 9,
    //    VisiGreen = 10,
    //    VisiViolet = 11,
    //    VisiAqua = 12,
    //    VisiOrange = 13,
    //    VisiGray = 14,
    //}

}