using System;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using Visifire.Charts;
using Visifire.Commons;

internal abstract class Graph
{
    /// <summary>
    /// Update marker and legend for dataPoint
    /// </summary>
    /// <param name="Value">Color value</param>
    public static void UpdateMarkerAndLegend(DataPoint dataPoint, VcProperties propertyName, object newValue)
    {   
        Marker marker = dataPoint.Marker;

        if (marker != null && marker.Visual != null && (Boolean)dataPoint.MarkerEnabled)
        {
            if (dataPoint.Parent.RenderAs == RenderAs.Point)
            {
                marker.FillColor = (Brush)newValue;

                if (marker.MarkerType != MarkerTypes.Cross)
                {
                    if (dataPoint.BorderColor != null)
                        marker.BorderColor = dataPoint.BorderColor;
                }
                else
                    marker.BorderColor = (Brush)newValue;

                // Marker.UpdateMarker();
            }
            else
                marker.BorderColor = (Brush)newValue;

            if (!dataPoint.Selected)
                marker.UpdateMarker();
        }

        // Marker displaied in Marker
        marker = dataPoint.LegendMarker;

        if (marker != null && marker.Visual != null)
        {   
            marker.BorderColor = (Brush)newValue;
            RenderAs renderAs = dataPoint.Parent.RenderAs;

            switch (renderAs)
            {   
                case RenderAs.Line:
                case RenderAs.CandleStick:
                case RenderAs.Stock:

                    if ((marker.Visual as Grid).Parent != null && (((marker.Visual as Grid).Parent as Canvas).Children[0] as Line) != null)
                        (((marker.Visual as Grid).Parent as Canvas).Children[0] as Line).Stroke = (Brush)newValue;

                    break;

                default:
                    marker.FillColor = (Brush)newValue;
                    break;
            }

            marker.UpdateMarker();
        }
    }

}