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
    public static class JsHelper
    {
        internal static void SetProperty(VisifireElement sender, String propertyName, String value)
        {
            if (propertyName == "Canvas.ZIndex")
            {
                sender.SetValue(Canvas.ZIndexProperty, Convert.ToInt32(value, CultureInfo.InvariantCulture));
                if ((sender as ObservableObject) != null)
                    (sender as ObservableObject).FirePropertyChanged("ZIndex");
                return;
            }

            System.Reflection.PropertyInfo[] propArray = sender.GetType().GetProperties();
            var obj = from property in propArray
                      where (property.Name == propertyName)
                      select property;

            Chart chart;

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

                System.Reflection.PropertyInfo property = obj.First<System.Reflection.PropertyInfo>();

                if (property.PropertyType.Name == "Brush")
                    property.SetValue(sender, ((Brush)System.Windows.Markup.XamlReader.Load(value)), null);
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
