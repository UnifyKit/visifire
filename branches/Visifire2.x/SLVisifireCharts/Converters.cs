using System;
using System.Windows;
using System.ComponentModel;
using Visifire.Charts;
using System.Globalization;
using System.Reflection;

namespace Visifire.Commons
{
    public class Converters
    {
        
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

        public class NullableInt32Converter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                Nullable<Int32> data = new Nullable<Int32>(System.Convert.ToInt32(value));
                return data;
            }
        }

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
                    markerBorderThickness = new Thickness(Convert.ToDouble(thickness[0]), Convert.ToDouble(thickness[0]), Convert.ToDouble(thickness[0]), Convert.ToDouble(thickness[0]));
                }
                else if (thickness.Length == 4)
                {
                    markerBorderThickness = new Thickness(Convert.ToDouble(thickness[0]), Convert.ToDouble(thickness[1]), Convert.ToDouble(thickness[2]), Convert.ToDouble(thickness[3]));
                }
                else
                    throw new NotSupportedException("Incorrect property value of property type Thickness");

                return markerBorderThickness;
            }
        }

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
                    cornerRadius = new CornerRadius(Convert.ToDouble(radius[0]), Convert.ToDouble(radius[0]), Convert.ToDouble(radius[0]), Convert.ToDouble(radius[0]));
                }
                else if (radius.Length == 4)
                {
                    cornerRadius = new CornerRadius(Convert.ToDouble(radius[0]), Convert.ToDouble(radius[1]), Convert.ToDouble(radius[2]), Convert.ToDouble(radius[3]));
                }
                else
                    throw new NotSupportedException("Incorrect property value of property CornerRadius");

                return cornerRadius;
            }
        }

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
    }
}
