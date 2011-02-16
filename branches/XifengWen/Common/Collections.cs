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

using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace Visifire.Charts
{
    /// <summary>
    /// Collection of Inline
    /// </summary>
    public class InlinesCollection : ObservableCollection<Inline> { }
    
    /// <summary>
    /// Collection of DataSeries
    /// </summary>
    public class DataSeriesCollection : ObservableCollection<DataSeries> { }

    /// <summary>
    /// Collection of DataPoint
    /// </summary>
    public class DataPointCollection : ObservableCollection<DataPoint> { }

    /// <summary>
    /// Collection of Axis
    /// </summary>
    public class AxisCollection : ObservableCollection<Axis> { }

    /// <summary>
    /// Collection of CustomLabels
    /// </summary>
    public class CustomAxisLabelsCollection : ObservableCollection<CustomAxisLabels> { }

    /// <summary>
    /// Collection of CustomLabel
    /// </summary>
    public class CustomAxisLabelCollection : ObservableCollection<CustomAxisLabel> { }

    /// <summary>
    /// Collection of Title
    /// </summary>
    public class TitleCollection : ObservableCollection<Title> { }
    
    /// <summary>
    /// Collection of Legend
    /// </summary>
    public class LegendCollection : ObservableCollection<Legend> { }
    
    /// <summary>
    /// Collection of TrendLine
    /// </summary>
    public class TrendLineCollection : ObservableCollection<TrendLine> { }
    
    /// <summary>
    /// Collection of ChartGrid
    /// </summary>
    public class ChartGridCollection : ObservableCollection<ChartGrid> {  }

    /// <summary>
    /// Collection of Ticks
    /// </summary>
    public class TicksCollection : ObservableCollection<Ticks> { }

    /// <summary>
    /// Collection of ToolTips
    /// </summary>
    public class ToolTipCollection : ObservableCollection<Visifire.Charts.ToolTip> {  }

    /// <summary>
    /// Collection of DataMapping
    /// </summary>
    public class DataMappingCollection : ObservableCollection<DataMapping> { }
}