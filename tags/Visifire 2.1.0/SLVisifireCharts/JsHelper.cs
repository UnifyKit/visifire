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
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using Visifire.Charts;
using System.Linq;
using System.Globalization;
using Visifire.Commons;

namespace Visifire.Commons
{
    /// <summary>
    /// JavaScript helper class
    /// </summary>
    public static class JsHelper
    {
        /// <summary>
        /// Set property from js
        /// </summary>
        /// <param name="sender">Object reference as sender</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">value of the property</param>
        internal static void SetProperty(VisifireElement sender, String propertyName, String value)
        {
            Chart chart;    // Chart 

            System.Reflection.PropertyInfo[] propArray = sender.GetType().GetProperties();

            // Find properties using LINQ
            var obj = from property in propArray
                      where (property.Name == propertyName)
                      select property;

            if ((sender as ObservableObject) != null)
                chart = (sender as ObservableObject).Chart as Chart;
            else if ((sender.GetType().Equals(typeof(Visifire.Charts.ToolTip))))
                chart = (sender as Visifire.Charts.ToolTip).Chart as Chart;
            else
                chart = sender as Chart;

            try
            {
                if (obj.Count<System.Reflection.PropertyInfo>() == 0)
                {
                    throw new Exception("Property not found.");
                }

                // Get the property from reflection
                System.Reflection.PropertyInfo property = obj.First<System.Reflection.PropertyInfo>();

                // Set the value of the property of the sender object
                if (property.PropertyType.Name == "Brush")
                    property.SetValue(sender, ((Brush)System.Windows.Markup.XamlReader.Load(value)), null);
                else if (property.PropertyType.Equals(typeof(Cursor)))
                {
                    Cursor cursor = null;
                    switch(value)
                    {
                        case "None":
                            cursor = Cursors.None;
                            break;

                        case "Wait":
                            cursor = Cursors.Wait;
                            break;

                        case "Hand":
                            cursor = Cursors.Hand;
                            break;

                        case "Eraser":
                            cursor = Cursors.Eraser;
                            break;

                        case "IBeam":
                            cursor = Cursors.IBeam;
                            break;

                        case "SizeNS":
                            cursor = Cursors.SizeNS;
                            break;

                        case "SizeWE":
                            cursor = Cursors.SizeWE;
                            break;

                        case "Stylus":
                            cursor = Cursors.Stylus;
                            break;

                        case "Arrow":
                            cursor = Cursors.Arrow;
                            break;
                    }

                    property.SetValue(sender, cursor, null);
                }
                else if (property.PropertyType.Equals(typeof(FontFamily)))
                {
                    FontFamily ff = new FontFamily(value);
                    property.SetValue(sender, ff, null);
                }
                else if (property.PropertyType.Equals(typeof(FontStyle)))
                {
                    Visifire.Commons.Converters.FontStyleConverter fsc = new Visifire.Commons.Converters.FontStyleConverter();
                    property.SetValue(sender, fsc.ConvertFrom(null, CultureInfo.InvariantCulture, value), null);
                }
                else if (property.PropertyType.Equals(typeof(Nullable<FontStyle>)))
                {
                    Visifire.Commons.Converters.FontStyleConverter fsc = new Visifire.Commons.Converters.FontStyleConverter();
                    property.SetValue(sender, (Nullable<FontStyle>)fsc.ConvertFrom(null, CultureInfo.InvariantCulture, value), null);
                }
                else if (property.PropertyType.Equals(typeof(FontWeight)))
                {
                    Visifire.Commons.Converters.FontWeightConverter fwc = new Visifire.Commons.Converters.FontWeightConverter();
                    property.SetValue(sender, fwc.ConvertFrom(null, CultureInfo.InvariantCulture, value), null);
                }
                else if (property.PropertyType.Equals(typeof(Nullable<FontWeight>)))
                {
                    Visifire.Commons.Converters.FontWeightConverter fwc = new Visifire.Commons.Converters.FontWeightConverter();
                    property.SetValue(sender, (Nullable<FontWeight>)fwc.ConvertFrom(null, CultureInfo.InvariantCulture, value), null);
                }
                else if (property.PropertyType.Equals(typeof(Nullable<Boolean>)))
                    property.SetValue(sender, new Nullable<Boolean>(Convert.ToBoolean(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.Equals(typeof(Nullable<Double>)))
                    property.SetValue(sender, new Nullable<Double>(Convert.ToDouble(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.BaseType.Equals(typeof(Enum)))
                    property.SetValue(sender, Enum.Parse(property.PropertyType, value, true), null);
                else if (property.PropertyType.Equals(typeof(Nullable<Thickness>)))
                    property.SetValue(sender, new Nullable<Thickness>(new Thickness(Convert.ToDouble(value, CultureInfo.InvariantCulture))), null);
                else if (property.PropertyType.Equals(typeof(Thickness)))
                    property.SetValue(sender, new Thickness(Convert.ToDouble(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.Equals(typeof(Nullable<CornerRadius>)))
                    property.SetValue(sender, new Nullable<CornerRadius>(new CornerRadius(Convert.ToDouble(value, CultureInfo.InvariantCulture))), null);
                else if (property.PropertyType.Equals(typeof(CornerRadius)))
                    property.SetValue(sender, new CornerRadius(Convert.ToDouble(value, CultureInfo.InvariantCulture)), null);
                else if (property.PropertyType.Equals(typeof(Nullable<LabelStyles>)))
                    property.SetValue(sender, Enum.Parse(typeof(LabelStyles), value, true), null);
                else if(property.PropertyType.Equals(typeof(Nullable<LineStyles>)))
                    property.SetValue(sender, Enum.Parse(typeof(LineStyles), value, true), null);
                else if (property.PropertyType.Equals(typeof(Nullable<MarkerTypes>)))
                    property.SetValue(sender, Enum.Parse(typeof(MarkerTypes), value, true), null);
                else if (property.PropertyType.Equals(typeof(Nullable<BorderStyles>)))
                    property.SetValue(sender, Enum.Parse(typeof(BorderStyles), value, true), null);
                else if (property.PropertyType.Equals(typeof(Nullable<HrefTargets>)))
                    property.SetValue(sender, Enum.Parse(typeof(HrefTargets), value, true), null);
                else if (property.PropertyType.Equals(typeof(Nullable<Int32>)))
                    property.SetValue(sender, new Nullable<Int32>(Convert.ToInt32(value, CultureInfo.InvariantCulture)), null);
                else
                    property.SetValue(sender, Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture), null);

                if ((chart as Chart).LoggerWindow != null)
                    (chart as Chart).LoggerWindow.Visibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                if ((chart as Chart).LoggerWindow == null)
                {
                    // If Log viewer is not present create it.
                    chart.CreateLogViewer();

                    if ((chart as Chart).LoggerWindow == null)
                        throw new Exception(e.Message);
                }

                if (chart.LogLevel == 1)
                    chart.LoggerWindow.Visibility = Visibility.Visible;
                else
                    chart.Visibility = Visibility.Collapsed;

                chart.LoggerWindow.Log("\n\nError Message:\n");

                // Log InnerException
                if (e.InnerException != null)
                {
                    chart.LoggerWindow.LogLine(e.InnerException.Message);
                }

                String s = String.Format(@"Unable to update {0} property. ({1})", propertyName, e.Message);

                chart.LoggerWindow.LogLine(s);

                // Exception is thrown to JavaScript
                throw new Exception((chart as Chart).LoggerWindow.logger.Text);
            }
        }
    }
}
