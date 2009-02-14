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
    /// <summary>
    /// Defines the type of the axis
    /// </summary>
    public enum AxisTypes 
    { 
        Primary, 
        Secondary 
    }

    /// <summary>
    /// Defines relative placement type for the axis elements
    /// </summary>
    internal enum PlacementTypes 
    {
        Top,
        Left,
        Right,
        Bottom 
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
        Point = 13
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
    /// defines the various chart orientations
    /// </summary>
    internal enum ChartOrientationType 
    {
        Undefined,     // Undefined - not yet assigned 
        Vertical,      // Vertical - charts of type point, bubble line, column(all similar types), area(all similar types)
        Horizontal,    // Horizontal - charts of type bar(all similar types)
        NoAxis         // NoAxis - charts of type pie or doughnut
    }

    /// <summary>
    /// Axis representations as X wise or Y wise
    /// </summary>
    internal enum AxisRepresentations 
    { 
        AxisX, 
        AxisY 
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
        FlowLayout = 0,
        Gridlayout = 1
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
    /// Style of border
    /// </summary>
    public enum BorderStyles
    {   
        Solid = 0,
        Dashed = 1,
        Dotted = 2
    }
}