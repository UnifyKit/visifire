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
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Visifire.Commons
{
    public class Themes
    {
        #region Public Methods
        public Themes()
        {
            _themes = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>>>();
            System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>> theme;
            System.Collections.Generic.Dictionary<String, Object> elmProperties;
            
            #region Theme1
            theme = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>();
            _themes["Theme1"] = theme;
          
            // Properties for Chart
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["Chart"] = elmProperties;
            elmProperties["Background"] = new SolidColorBrush(Colors.White);
            elmProperties["BorderThickness"] = .5;
            elmProperties["BorderColor"] = new SolidColorBrush(Colors.Black);
            elmProperties["ColorSet"] = "Visifire1";
            elmProperties["AnimationType"] = "Type1";
            elmProperties["AnimationDuration"] = 1.25;
            elmProperties["AnimationEnabled"] = true;

            // Properties for DataSeries
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["DataSeries"] = elmProperties;
            elmProperties["Bevel"] = true;
            elmProperties["LightingEnabled"] = true;
            elmProperties["MarkerBorderThickness"] = 1;

            // Properties for AxisX
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["AxisX"] = elmProperties;
            elmProperties["LineThickness"] = 0.5;
            elmProperties["LineColor"] = Parser.ParseColor("#333333");

            // Properties for AxisY
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["AxisY"] = elmProperties;
            elmProperties["LineThickness"] = 0.5;
            elmProperties["LineColor"] = Parser.ParseColor("#333333");

            
            #endregion Theme1

            #region Theme2
            theme = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>();
            _themes["Theme2"] = theme;

            // Properties for Chart
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["Chart"] = elmProperties;
            elmProperties["Background"] = new SolidColorBrush(Colors.White);
            elmProperties["BorderThickness"] = 0.5;
            elmProperties["BorderColor"] = new SolidColorBrush(Colors.Black);
            elmProperties["ColorSet"] = "Visifire2";
            elmProperties["AnimationType"] = "Type3";
            elmProperties["AnimationEnabled"] = true;
            elmProperties["AnimationDuration"] = 1.25;

            // Properties for DataSeries
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["DataSeries"] = elmProperties;
            elmProperties["Bevel"] = false;
            elmProperties["LightingEnabled"] = true;
            elmProperties["MarkerBorderThickness"] = 1;

            // Properties for AxisX
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["AxisX"] = elmProperties;
            elmProperties["LineThickness"] = 0.5;
            elmProperties["LineColor"] = Parser.ParseColor("#333333");

            // Properties for AxisY
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["AxisY"] = elmProperties;
            elmProperties["LineThickness"] = 0.5;
            elmProperties["LineColor"] = Parser.ParseColor("#333333");

            #endregion Theme2

            #region Theme3
            theme = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>();
            _themes["Theme3"] = theme;

            // Properties for Chart
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["Chart"] = elmProperties;
            elmProperties["Background"] = new SolidColorBrush(Colors.Black);
            elmProperties["BorderThickness"] = 0.0;
            elmProperties["ColorSet"] = "Visifire1";
            elmProperties["Bevel"] = false;
            elmProperties["AnimationType"] = "Type2";
            elmProperties["AnimationEnabled"] = true;

            // Properties for Plotarea
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["PlotArea"] = elmProperties;
            elmProperties["Background"] = new SolidColorBrush(Colors.Black);
            elmProperties["BorderThickness"] = 0.0;
            elmProperties["LightingEnabled"] = false;
            elmProperties["Bevel"] = false;

            // Properties for DataSeries
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["DataSeries"] = elmProperties;
            elmProperties["RadiusX"] = 0;
            elmProperties["RadiusY"] = 0;
            elmProperties["Bevel"] = true;
            elmProperties["MarkerBorderThickness"] = 1;


            // Properties for AxisX
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["AxisX"] = elmProperties;
            elmProperties["LineThickness"] = 0.5;

            // Properties for AxisY
            elmProperties = new System.Collections.Generic.Dictionary<string, Object>();
            theme["AxisY"] = elmProperties;
            elmProperties["LineThickness"] = 0.5;
            #endregion Theme3

        }

        #endregion Public Methods

        #region Public Properties
        public System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>> this[String str]
        {
            get
            {
                return _themes[str];
            }
        }
        
        public System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, System.Collections.Generic.Dictionary<String, Object>>> _themes;  // Collection of Theme
        #endregion Public Methods
    }
}
