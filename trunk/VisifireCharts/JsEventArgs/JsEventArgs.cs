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
using System.Windows;
using System.Windows.Input;
using System.Windows.Browser;
using Visifire.Charts;

namespace VisifireCharts
{
    [ScriptableType]
    public class JsEventArgs : EventArgs
    {
        #region Public Method

        public JsEventArgs(FrameworkElement chart, FrameworkElement element, EventArgs e, String eventName)
        {
            // ControlId = "NOT SET";
            Event = eventName;
            ChartName = chart.Name;
            Element = element.GetType().Name;

            MouseX = Double.NaN;
            MouseY = Double.NaN;

            if(eventName != "MouseLeave")
                GetMousePosition(chart, e);
        }

        #endregion
        
        #region Public Properties

            public String ControlId { get; set; }
            
            public String Event { get; set; }

            public String ChartName { get; set; }
            public String Element { get; set; }

            public Double MouseX { get; set; }
            public Double MouseY { get; set; }

        #endregion 

        #region Private Method

            private void GetMousePosition(UIElement chart, EventArgs e)
            {
                if (e.GetType().Name == "MouseButtonEventArgs")
                {
                    Point position = (e as MouseButtonEventArgs).GetPosition(chart);
                    MouseX = position.X;
                    MouseY = position.Y;
                }
                else if (e.GetType().Name == "MouseEventArgs")
                {
                    Point position = (e as MouseEventArgs).GetPosition(chart);
                    MouseX = position.X;
                    MouseY = position.Y;
                }
            }
            
        #endregion
    }
}