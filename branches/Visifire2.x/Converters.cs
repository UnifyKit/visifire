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
using System.ComponentModel;
using Visifire.Charts;
using System.Globalization;
using System.Reflection;

namespace Visifire.Commons
{
    /// <summary>
    /// Visifire.Commons.Converters class
    /// </summary>
    public class Converters
    {
        public class ValueConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if ((String)value == String.Empty)
                    return Double.NaN;
                else
                    return Double.Parse((String)value, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

#if SL
        /// <summary>
        /// NullableDouble converter
        /// </summary>
        public class NullableDoubleConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<Double> data = new Nullable<Double>(System.Convert.ToDouble(value,CultureInfo.InvariantCulture));
                return data;
            }
        }

        /// <summary>
        /// NullableInt32 converter
        /// </summary>
        public class NullableInt32Converter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<Int32> data = new Nullable<Int32>(System.Convert.ToInt32(value,System.Globalization.CultureInfo.InvariantCulture));
                return data;
            }
        }

        /// <summary>
        /// NullableThickness converter
        /// </summary>
        public class NullableThicknessConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                String[] thickness = value.ToString().Split(',');

                Thickness markerBorderThickness;

                if (thickness.Length == 1)
                {
                    markerBorderThickness = new Thickness(Convert.ToDouble(thickness[0],System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(thickness[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(thickness[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(thickness[0],System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (thickness.Length == 4)
                {
                    markerBorderThickness = new Thickness(Convert.ToDouble(thickness[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(thickness[1],System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(thickness[2],System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(thickness[3],System.Globalization.CultureInfo.InvariantCulture));
                }
                else
                    throw new NotSupportedException("Incorrect property value of property type Thickness");

                return markerBorderThickness;
            }
        }

        /// <summary>
        /// NullableLabelStyles converter
        /// </summary>
        public class NullableLabelStylesConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {   
                Nullable<LabelStyles> data = new Nullable<LabelStyles>((LabelStyles)Enum.Parse(typeof(LabelStyles), value.ToString(), true));
                return data;
            }
        }

        /// <summary>
        /// NullableHrefTargets converter
        /// </summary>
        public class NullableHrefTargetsConverter : TypeConverter
        {   
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<HrefTargets> data = new Nullable<HrefTargets>((HrefTargets)Enum.Parse(typeof(HrefTargets), value.ToString(), true));
                return data;
            }
        }

        /// <summary>
        /// NullableMarkerTypes converter
        /// </summary>
        public class NullableMarkerTypesConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<MarkerTypes> data = new Nullable<MarkerTypes>((MarkerTypes)Enum.Parse(typeof(MarkerTypes), value.ToString(), true));
                return data;
            }
        }

        /// <summary>
        /// NullableLineStyles converter
        /// </summary>
        public class NullableLineStylesConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<LineStyles> data = new Nullable<LineStyles>((LineStyles)Enum.Parse(typeof(LineStyles), value.ToString(), true));
                return data;
            }
        }

        /// <summary>
        /// NullableBorderStyles converter
        /// </summary>
        public class NullableBorderStylesConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<BorderStyles> data = new Nullable<BorderStyles>((BorderStyles)Enum.Parse(typeof(BorderStyles), value.ToString(), true));
                return data;
            }
        }

        /// <summary>
        /// CornerRadius converter
        /// </summary>
        public class CornerRadiusConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {          
                String[] radius = value.ToString().Split(',');

                CornerRadius cornerRadius;

                if (radius.Length == 1)
                {
                    cornerRadius = new CornerRadius(Convert.ToDouble(radius[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(radius[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(radius[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(radius[0], System.Globalization.CultureInfo.InvariantCulture));
                }
                else if (radius.Length == 4)
                {
                    cornerRadius = new CornerRadius(Convert.ToDouble(radius[0], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(radius[1], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(radius[2], System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(radius[3], System.Globalization.CultureInfo.InvariantCulture));
                }
                else
                    throw new NotSupportedException("Incorrect property value of property CornerRadius");

                return cornerRadius;
            }
        }

        /// <summary>
        /// FontStyle converter
        /// </summary>
        public class FontStyleConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return typeof(String) == sourceType;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                String fontStyleString = value as String;
                PropertyInfo fontProperty = typeof(FontStyles).GetProperty(fontStyleString);
                FontStyleMethod fontStyleMethod = (FontStyleMethod)Delegate.CreateDelegate(typeof(FontStyleMethod), fontProperty.GetGetMethod());
                return fontStyleMethod();
            }
            private delegate FontStyle FontStyleMethod();
        }

        /// <summary>
        /// FontWeight converter
        /// </summary>
        public class FontWeightConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return typeof(String) == sourceType;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                String fontWeightString = value as String;
                PropertyInfo fontProperty = typeof(FontWeights).GetProperty(fontWeightString);
                FontWeightMethod fontWeightMethod = (FontWeightMethod)Delegate.CreateDelegate(typeof(FontWeightMethod), fontProperty.GetGetMethod());
                return fontWeightMethod();
            }
            private delegate FontWeight FontWeightMethod();
        }
#endif

    }
}
