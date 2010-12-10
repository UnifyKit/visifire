﻿/*   
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

#if WPF

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

#else

using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#endif

using Visifire.Commons;
using System.Windows.Data;
using System.Collections.Generic;
using System.ComponentModel;
using Visifire.Commons.Controls;
using System.Collections.Specialized;

#if SL && !WP
using System.Windows.Browser;
#endif

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.DataPoint class
    /// </summary>
#if SL &&!WP
    [System.Windows.Browser.ScriptableType]
#endif
    public class DataPoint : ObservableObject
    {
        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the Visifire.Charts.DataPoint class
        /// </summary>
        public DataPoint()
        {   
            ToolTipText = String.Empty;
            InternalXValue = Double.NaN;
            XValueType = ChartValueTypes.Numeric;
            
            // Apply default style from generic
#if WPF
            if (!_defaultStyleKeyApplied)
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPoint), new FrameworkPropertyMetadata(typeof(DataPoint)));
                _defaultStyleKeyApplied = true;
            }
#else
            
            DefaultStyleKey = typeof(DataPoint);
#endif

        }

        public override void Bind()
        {
#if SL
            base.Bind();
            
            Binding b = new Binding("BorderThickness");
            b.Source = this;
            this.SetBinding(InternalBorderThicknessProperty, b);

            b = new Binding("Opacity");
            b.Source = this;
            this.SetBinding(InternalOpacityProperty, b);

#endif
        }

        /// <summary>
        /// Set DateTime in AxisXLabel
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="axis">Axis</param>
        /// <param name="label">Axis labels</param>
        private String FormatDate4Labels(DateTime dt, Axis axis)
        {
            // String valueFormatString = axis.XValueType == ChartValueTypes.Date ? "M/d/yyyy" : axis.XValueType == ChartValueTypes.Time ? "h:mm:ss tt" : "M/d/yyyy h:mm:ss tt";

            String valueFormatString = axis.XValueType == ChartValueTypes.Date ? "d" :
                axis.XValueType == ChartValueTypes.Time ? "t" : "G";
            valueFormatString = String.IsNullOrEmpty(Parent.XValueFormatString) ? valueFormatString : Parent.XValueFormatString;

            return axis.AddPrefixAndSuffix(dt.ToString(valueFormatString, System.Globalization.CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// TextParser is used to parse ToolTipText
        /// </summary>
        /// <param name="unParsed">String unParsed</param>
        /// <returns>parsed as string</returns>
        public override string ParseToolTipText(string unParsed)
        {
            return _parsedToolTipText;
        }

        /// <summary>
        /// OnToolTipTextPropertyChanged call back virtual function
        /// </summary>
        /// <param name="newValue">New ToolTip value</param>
        internal override void OnToolTipTextPropertyChanged(string newValue)
        {
            // base.OnToolTipTextPropertyChanged(newValue);

            if (Chart != null && _parsedToolTipText != null)
            {
                _parsedToolTipText = TextParser(newValue);
            }
        }
        
        /// <summary>
        /// Already parsed content
        /// </summary>
        internal String _parsedToolTipText = null;

        /// <summary>
        /// TextParser is used to parse text
        /// </summary>
        /// <param name="unParsed">String unParsed</param>
        /// <returns>parsed as string</returns>
        public override String TextParser(String unParsed)
        {
            if (string.IsNullOrEmpty(unParsed) || Enabled == false)
                return "";

            String str = new String(unParsed.ToCharArray());
            if (str.Contains("##XValue"))
                str = str.Replace("##XValue", "#XValue");
            else if (str.Contains("#XValue"))
            {
                if (String.IsNullOrEmpty(_parent.XValueFormatString))
                {
                    if (Parent.PlotGroup != null)
                    {
                        if ((Chart as Chart).ChartArea.AxisX != null && (Chart as Chart).ChartArea.AxisX.XValueType != ChartValueTypes.Numeric)
                            str = str.Replace("#XValue", FormatDate4Labels(Convert.ToDateTime(InternalXValueAsDateTime), (Chart as Chart).ChartArea.AxisX));
                        else if ((this.Parent.RenderAs == RenderAs.Pie || this.Parent.RenderAs == RenderAs.Doughnut || this.Parent.RenderAs == RenderAs.SectionFunnel || this.Parent.RenderAs == RenderAs.StreamLineFunnel || this.Parent.RenderAs == RenderAs.Pyramid) && (Parent.InternalXValueType != ChartValueTypes.Numeric))
                        {
                            str = str.Replace("#XValue", FormatDate4Labels(Convert.ToDateTime(InternalXValueAsDateTime), Parent.PlotGroup.AxisX));
                        }
                        else
                            str = str.Replace("#XValue", Parent.PlotGroup.AxisX.GetFormattedString(InternalXValue));
                    }
                }
                else
                    if (Parent.PlotGroup != null)
                    {
                        if ((Chart as Chart).ChartArea.AxisX != null && (Chart as Chart).ChartArea.AxisX.XValueType != ChartValueTypes.Numeric)
                            str = str.Replace("#XValue", FormatDate4Labels(Convert.ToDateTime(InternalXValueAsDateTime), (Chart as Chart).ChartArea.AxisX));
                        else if ((this.Parent.RenderAs == RenderAs.Pie || this.Parent.RenderAs == RenderAs.Doughnut || this.Parent.RenderAs == RenderAs.SectionFunnel || this.Parent.RenderAs == RenderAs.StreamLineFunnel || this.Parent.RenderAs == RenderAs.Pyramid) && (Parent.InternalXValueType != ChartValueTypes.Numeric))
                        {
                            str = str.Replace("#XValue", FormatDate4Labels(Convert.ToDateTime(InternalXValueAsDateTime), Parent.PlotGroup.AxisX));
                        }
                        else
                            str = str.Replace("#XValue", InternalXValue.ToString(Parent.XValueFormatString));
                    }
            }

            if (str.Contains("##YValue"))
                str = str.Replace("##YValue", "#YValue");
            else if (str.Contains("#YValue"))
            {
                if (String.IsNullOrEmpty(_parent.YValueFormatString))
                {
                    if (Parent.PlotGroup != null)
                        str = str.Replace("#YValue", Parent.PlotGroup.AxisY.GetFormattedString(YValue));
                }
                else
                    str = str.Replace("#YValue", YValue.ToString(Parent.YValueFormatString));
            }

            if (str.Contains("##ZValue"))
                str = str.Replace("##ZValue", "#ZValue");
            else if (str.Contains("#ZValue"))
                str = str.Replace("#ZValue", ZValue.ToString(Parent.ZValueFormatString));

            if (str.Contains("##Series"))
                str = str.Replace("##Series", "#Series");
            else if (str.Contains("#Series"))
            {
                if (Parent._isAutoName)
                {
                    String[] s = Parent.Name.Split('_');
                    str = str.Replace("#Series", s[0]);
                }
                else
                    str = str.Replace("#Series", Parent.Name);
            }

            if (str.Contains("##AxisXLabel"))
                str = str.Replace("##AxisXLabel", "#AxisXLabel");
            else if (str.Contains("#AxisXLabel"))
                str = str.Replace("#AxisXLabel", String.IsNullOrEmpty(AxisXLabel) ? GetAxisXLabelString() : AxisXLabel);

            if (str.Contains("##Percentage"))
                str = str.Replace("##Percentage", "#Percentage");
            else if(str.Contains("#Percentage"))
                str = str.Replace("#Percentage", Percentage().ToString("#0.##"));


            if (str.Contains("##Sum"))
                str = str.Replace("##Sum", "#Sum");
            else if (str.Contains("#Sum"))
            {
                if (Parent.PlotGroup != null && Parent.PlotGroup.XWiseStackedDataList != null && Parent.PlotGroup.XWiseStackedDataList.ContainsKey(InternalXValue))
                {
                    Double sum = 0;
                    sum += Parent.PlotGroup.XWiseStackedDataList[InternalXValue].PositiveYValueSum;
                    sum += Parent.PlotGroup.XWiseStackedDataList[InternalXValue].NegativeYValueSum;
                    str = str.Replace("#Sum", Parent.PlotGroup.AxisY.GetFormattedString(sum));  //_stackSum[XValue].X contains sum of all data points with same X value
                }
            }

            if (_parent.RenderAs == RenderAs.Stock || _parent.RenderAs == RenderAs.CandleStick)
            {
                if (str.Contains("##High"))
                    str = str.Replace("##High", "#High");
                else if(str.Contains("#High"))
                {
                    if (String.IsNullOrEmpty(_parent.YValueFormatString))
                    {
                        if (Parent.PlotGroup != null && YValues != null && YValues.Length > 2)
                            str = str.Replace("#High", Parent.PlotGroup.AxisY.GetFormattedString(YValues[2]));
                        else
                            str = str.Replace("#High", "");
                    }
                    else
                    {
                        if (YValues != null && YValues.Length > 2)
                            str = str.Replace("#High", YValues[2].ToString(Parent.YValueFormatString));
                        else
                            str = str.Replace("#High", "");
                    }
                }


                if (str.Contains("##Low"))
                    str = str.Replace("##Low", "#Low");
                else if (str.Contains("#Low"))
                {
                    if (String.IsNullOrEmpty(_parent.YValueFormatString))
                    {
                        if (Parent.PlotGroup != null && YValues != null && YValues.Length > 3)
                            str = str.Replace("#Low", Parent.PlotGroup.AxisY.GetFormattedString(YValues[3]));
                        else
                            str = str.Replace("#Low", "");
                    }
                    else
                    {
                        if (YValues != null && YValues.Length > 3)
                            str = str.Replace("#Low", YValues[3].ToString(Parent.YValueFormatString));
                        else
                            str = str.Replace("#Low", "");
                    }
                }

                if (str.Contains("##Open"))
                    str = str.Replace("##Open", "#Open");
                else if (str.Contains("#Open"))
                {
                    if (String.IsNullOrEmpty(_parent.YValueFormatString))
                    {
                        if (Parent.PlotGroup != null && YValues != null && YValues.Length > 0)
                            str = str.Replace("#Open", Parent.PlotGroup.AxisY.GetFormattedString(YValues[0]));
                        else
                            str = str.Replace("#Open", "");
                    }
                    else
                    {
                        if (YValues != null && YValues.Length > 0)
                            str = str.Replace("#Open", YValues[0].ToString(Parent.YValueFormatString));
                        else
                            str = str.Replace("#Open", "");
                    }
                }


                if (str.Contains("##Close"))
                    str = str.Replace("##Close", "#Close");
                else if (str.Contains("#Close"))
                {
                    if (String.IsNullOrEmpty(_parent.YValueFormatString))
                    {
                        if (Parent.PlotGroup != null && YValues != null && YValues.Length > 1)
                            str = str.Replace("#Close", Parent.PlotGroup.AxisY.GetFormattedString(YValues[1]));
                        else
                            str = str.Replace("#Close", "");
                    }
                    else
                    {
                        if (YValues != null && YValues.Length > 1)
                            str = str.Replace("#Close", YValues[1].ToString(Parent.YValueFormatString));
                        else
                            str = str.Replace("#Close", "");
                    }
                }
            }

            return GetFormattedMultilineText(str);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.Selected dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.Selected dependency property.
        /// </returns>
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register
            ("Selected",
            typeof(Boolean),
            typeof(DataPoint),
            new PropertyMetadata(OnSelectedChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.HrefTarget dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.HrefTarget dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefTargetProperty = DependencyProperty.Register
            ("HrefTarget",
            typeof(Nullable<HrefTargets>),
            typeof(DataPoint),
            new PropertyMetadata(OnHrefTargetChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.Href dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.Href dependency property.
        /// </returns>
        public static readonly DependencyProperty HrefProperty = DependencyProperty.Register
            ("Href",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnHrefChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.Opacity dependency property.
        /// </returns>
        public new static readonly DependencyProperty OpacityProperty = DependencyProperty.Register
            ("Opacity",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.YValue dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.YValue dependency property.
        /// </returns>
        public static readonly DependencyProperty YValueProperty = DependencyProperty.Register
            ("YValue",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(Double.NaN, OnYValuePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.YValues dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.YValues dependency property.
        /// </returns>
        public static readonly DependencyProperty YValuesProperty = DependencyProperty.Register
           ("YValues",
           typeof(Double[]),
           typeof(DataPoint),
           new PropertyMetadata(null, OnYValuesPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.XValue dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.XValue dependency property.
        /// </returns>
        public static readonly DependencyProperty XValueProperty = DependencyProperty.Register
            ("XValue",
            typeof(Object),
            typeof(DataPoint),
            new PropertyMetadata(OnXValuePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.ZValue dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.ZValue dependency property.
        /// </returns>
        public static readonly DependencyProperty ZValueProperty = DependencyProperty.Register
            ("ZValue",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(Double.NaN, OnZValuePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.AxisXLabel dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.AxisXLabel dependency property.
        /// </returns>
        public static readonly DependencyProperty AxisXLabelProperty = DependencyProperty.Register
            ("AxisXLabel",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnAxisXLabelPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.Color dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.Color dependency property.
        /// </returns>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register
             ("Color",
             typeof(Brush),
             typeof(DataPoint),
             new PropertyMetadata(OnColorPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.StickColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.StickColor dependency property.
        /// </returns>
        public static readonly DependencyProperty StickColorProperty = DependencyProperty.Register
             ("StickColor",
             typeof(Brush),
             typeof(DataPoint),
             new PropertyMetadata(OnStickColorPropertyChanged));

        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.Enabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.Enabled dependency property.
        /// </returns>
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnEnabledPropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.Exploded dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.Exploded dependency property.
        /// </returns>
        public static readonly DependencyProperty ExplodedProperty = DependencyProperty.Register
            ("Exploded",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnExplodedPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LightingEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LightingEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LightingEnabledProperty = DependencyProperty.Register
            ("LightingEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnLightingEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.ShadowEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.ShadowEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty ShadowEnabledProperty = DependencyProperty.Register
            ("ShadowEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.ShowInLegend dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.ShowInLegend dependency property.
        /// </returns>
        public static readonly DependencyProperty ShowInLegendProperty = DependencyProperty.Register
            ("ShowInLegend",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnShowInLegendPropertychanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LegendText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LegendText dependency property.
        /// </returns>
        public static readonly DependencyProperty LegendTextProperty = DependencyProperty.Register
            ("LegendText",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnLegendTextPropertychanged));
        
#if WPF

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.BorderThickness dependency property.
        /// </returns>
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register
            ("BorderThickness",
            typeof(Thickness),
            typeof(DataPoint),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));
#endif

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.BorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.BorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register
            ("BorderColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnBorderColorPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.BorderStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.BorderStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty BorderStyleProperty = DependencyProperty.Register
            ("BorderStyle",
            typeof(Nullable<BorderStyles>),
            typeof(DataPoint),
            new PropertyMetadata(OnBorderStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.RadiusX dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.RadiusX dependency property.
        /// </returns>
        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register
            ("RadiusX",
            typeof(Nullable<CornerRadius>),
            typeof(DataPoint),
            new PropertyMetadata(OnRadiusXPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.RadiusY dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.RadiusY dependency property.
        /// </returns>
        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register
            ("RadiusY",
            typeof(Nullable<CornerRadius>),
            typeof(DataPoint),
            new PropertyMetadata(OnRadiusYPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelEnabledProperty = DependencyProperty.Register
            ("LabelEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelText dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register
            ("LabelText",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelTextPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelFontFamily dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelFontFamily dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontFamilyProperty = DependencyProperty.Register
            ("LabelFontFamily",
            typeof(FontFamily),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontFamilyPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelFontSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelFontSize dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register
             ("LabelFontSize",
             typeof(Nullable<Double>),
             typeof(DataPoint),
             new PropertyMetadata(OnLabelFontSizePropertyChanged));
            
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelFontColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelFontColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontColorProperty = DependencyProperty.Register
            ("LabelFontColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontColorPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelFontWeight dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelFontWeight dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register
            ("LabelFontWeight",
            typeof(Nullable<FontWeight>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontWeightPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelFontStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelFontStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelFontStyleProperty = DependencyProperty.Register
            ("LabelFontStyle",
            typeof(Nullable<FontStyle>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelFontStylePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelBackground dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelBackground dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register
            ("LabelBackground",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelBackgroundPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register
            ("LabelStyle",
            typeof(Nullable<LabelStyles>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelAngle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelAngle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelAngleProperty = DependencyProperty.Register
            ("LabelAngle",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(Double.NaN, OnLabelAnglePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelLineEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelLineEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineEnabledProperty = DependencyProperty.Register
            ("LabelLineEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelLineColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelLineColor dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineColorProperty = DependencyProperty.Register
            ("LabelLineColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineColorPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelLineThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelLineThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineThicknessProperty = DependencyProperty.Register
            ("LabelLineThickness",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineThicknessPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.LabelLineStyle dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.LabelLineStyle dependency property.
        /// </returns>
        public static readonly DependencyProperty LabelLineStyleProperty = DependencyProperty.Register
            ("LabelLineStyle",
            typeof(Nullable<LineStyles>),
            typeof(DataPoint),
            new PropertyMetadata(OnLabelLineStylePropertyChanged));

        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerEnabled dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerEnabled dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerEnabledProperty = DependencyProperty.Register
            ("MarkerEnabled",
            typeof(Nullable<Boolean>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerEnabledPropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerType dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerType dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerTypeProperty = DependencyProperty.Register
            ("MarkerType",
            typeof(Nullable<MarkerTypes>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerTypePropertyChanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerBorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerBorderThickness dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerBorderThicknessProperty = DependencyProperty.Register
            ("MarkerBorderThickness",
            typeof(Nullable<Thickness>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerBorderThicknessPropertychanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerBorderColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerBorderColor dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerBorderColorProperty = DependencyProperty.Register
           ("MarkerBorderColor",
           typeof(Brush),
           typeof(DataPoint),
           new PropertyMetadata(OnMarkerBorderColorPropertychanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerSize dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerSize dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerSizeProperty = DependencyProperty.Register
            ("MarkerSize",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerSizePropertychanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerColor dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerColor dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerColorProperty = DependencyProperty.Register
            ("MarkerColor",
            typeof(Brush),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerColorPropertychanged));
        
        /// <summary>
        /// Identifies the Visifire.Charts.DataPoint.MarkerScale dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataPoint.MarkerScale dependency property.
        /// </returns>
        public static readonly DependencyProperty MarkerScaleProperty = DependencyProperty.Register
            ("MarkerScale",
            typeof(Nullable<Double>),
            typeof(DataPoint),
            new PropertyMetadata(OnMarkerScalePropertychanged));

        /// <summary>
        /// Identifies the Visifire.Charts.VisifireElement.ToolTipText dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.VisifireElement.ToolTipText dependency property.
        /// </returns>
        public static new readonly DependencyProperty NameProperty = DependencyProperty.Register
            ("Name",
            typeof(String),
            typeof(DataPoint),
            new PropertyMetadata(String.Empty));

        /// <summary>
        /// Name of the object
        /// </summary>
#if (SL &&!WP)
        [ScriptableMember]
#endif
        public new String Name
        {
            get
            {
                return (String)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }
       
        /// <summary>
        /// Get or set the HrefTarget property of DataPoint
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableHrefTargetsConverter))]
#endif
        public Nullable<HrefTargets> HrefTarget
        {   
            get
            {
                if ((Nullable<HrefTargets>)GetValue(HrefTargetProperty) == null && _parent != null)
                    return _parent.HrefTarget;
                else
                    return (Nullable<HrefTargets>)GetValue(HrefTargetProperty);
            }
            set
            {
                SetValue(HrefTargetProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Selected property of DataPoint
        /// </summary>
        public Boolean Selected
        {
            get
            {
                if (Parent != null)
                    return ((Boolean)GetValue(SelectedProperty) & Parent.SelectionEnabled);
                else
                    return (Boolean)GetValue(SelectedProperty);
            }
            set
            {
                SetValue(SelectedProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Href property of DataPoint
        /// </summary>
        public String Href
        {
            get
            {
                if (String.IsNullOrEmpty((String)GetValue(HrefProperty)) && _parent != null)
                    return (String)Parent.GetValue(DataSeries.HrefProperty);
                else
                    return (String)GetValue(HrefProperty);
            }
            set
            {
                SetValue(HrefProperty, value);
            }
        }

        /// <summary>
        /// Get or set the Opacity property
        /// </summary>
        public new Double Opacity
        {
            get
            {
                return (Double)GetValue(OpacityProperty);
            }
            set
            {
#if SL
                if (Opacity != value)
                {   
                    InternalOpacity = value;
                    SetValue(OpacityProperty, value);
                    InvokeUpdateVisual(VcProperties.Opacity, value);
                    // FirePropertyChanged(VcProperties.Opacity);
                }
#else
                SetValue(OpacityProperty, value);
#endif
            }
        }

        /// <summary>
        /// Get or set the Cursor property
        /// </summary>
        public new Cursor Cursor
        {
            get
            {
                if (base.Cursor == null)
                    return _parent.Cursor;
                else
                    return base.Cursor;
            }
            set
            {
                if (base.Cursor != value)
                {
                    base.Cursor = value;

                    if (Faces != null || (this.Parent != null && this.Parent.Faces != null) || this.Marker != null)
                        SetCursor2DataPointVisualFaces();
                    else
                        InvokeUpdateVisual(VcProperties.Cursor, value);
                        //FirePropertyChanged(VcProperties.Cursor);
                }
            }
        }

        /// <summary>
        /// Get or set the value that will appear on Y-Axis for all charts.
        /// In the case of Pie and Doughnut, the YValue will be considered for calculating the percentages.
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(Converters.ValueConverter))]
        public Double YValue
        {   
            get
            {
                return (Double)GetValue(YValueProperty);
            }
            set
            {
                _oldYValue = (Double)GetValue(YValueProperty);
                SetValue(YValueProperty, value);
            }
        }

        [System.ComponentModel.TypeConverter(typeof(Converters.DoubleArrayConverter))]
        public Double[] YValues
        {
            get
            {
                return (Double[]) GetValue(YValuesProperty);
            }
            set
            {
                SetValue(YValuesProperty, value);
            }
        }

        internal Double[] InternalYValues
        {   
            get
            {
                if (YValues != null && Chart != null && Parent != null)
                {
                    Chart chart = Chart as Chart;
                    Double[] convertedYValues = new Double[4];
                    convertedYValues[0] = (YValues.Length > 0 ? ConvertYValue2LogarithmicValue(chart, YValues[0], Parent.AxisYType) : Double.NaN);
                    convertedYValues[1] = (YValues.Length > 1 ? ConvertYValue2LogarithmicValue(chart, YValues[1], Parent.AxisYType) : Double.NaN);
                    convertedYValues[2] = (YValues.Length > 2 ? ConvertYValue2LogarithmicValue(chart, YValues[2], Parent.AxisYType) : Double.NaN);
                    convertedYValues[3] = (YValues.Length > 3 ? ConvertYValue2LogarithmicValue(chart, YValues[3], Parent.AxisYType) : Double.NaN);

                    Double[] newYValues = (from value in convertedYValues where !Double.IsNaN(value) select value).ToArray();

                    return newYValues;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Get the YValue that will be used for Internal purpose.
        /// </summary>
        //[System.ComponentModel.TypeConverter(typeof(Converters.ValueConverter))]
        internal Double InternalYValue
        {
            get
            {
                return ConvertYValue2LogarithmicValue(this);
            }
        }

        /// <summary>
        /// This property is used to store the clipped width of a DataPoint in Pie and Doughnut charts.
        /// </summary>
        internal Double ClippedWidth4Pie
        {
            get;
            set;
        }

        /// <summary>
        /// Modify YValue for Logarithmic scale
        /// </summary>
        /// <param name="yValue"></param>
        /// <returns></returns>
        internal static Double ConvertYValue2LogarithmicValue(DataPoint dataPoint)
        {   
            Axis axis = null;

            Double value = 0;
            if (dataPoint.Parent != null && dataPoint.Parent.PlotGroup != null)
                axis = dataPoint.Parent.PlotGroup.AxisY;

            Chart chart = dataPoint.Chart as Chart;

            if (chart != null && chart.PlotDetails != null && chart.PlotDetails.ChartOrientation != ChartOrientationType.NoAxis
                && axis != null && axis.Logarithmic)
                value = Math.Log(dataPoint.YValue, axis.LogarithmBase);
            else
                value = dataPoint.YValue;

            return value;
        }

        /// <summary>
        /// Get or set the value that will appear on X-Axis for all charts. 
        /// </summary>
#if SL || WP
        [System.ComponentModel.TypeConverter(typeof(Converters.ObjectConverter))]
#endif
        public Object XValue
        {   
            get
            {
                return GetValue(XValueProperty);
            }
            set
            {
                SetValue(XValueProperty, value);
            }
        }

        /// <summary>
        /// Type of scale used in axis
        /// </summary>
        internal ChartValueTypes XValueType
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the value that will appear for bubble charts only
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(Converters.ValueConverter))]
        public Double ZValue
        {
            get
            {
                return (Double)GetValue(ZValueProperty);
            }
            set
            {
                SetValue(ZValueProperty, value);
            }
        }
        
        /// <summary>
        /// Label to be placed for the corresponding XValue in the AxisX
        /// </summary>
        public String AxisXLabel
        {
            get
            {
                return (String)GetValue(AxisXLabelProperty);
            }
            set
            {
                SetValue(AxisXLabelProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the color of the DataPoint
        /// </summary>
        public Brush Color
        {
            get
            {
                if ((Brush)GetValue(ColorProperty) == null)
                {
                    Chart chart =(Chart as Chart);

                    if (_parent == null)
                        return _internalColor;
                    else
                        return (_parent.Color == null) ? _internalColor : _parent.Color;
                }
                else
                    return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the high-low line color of CandleStick DataPoint
        /// </summary>
        public Brush StickColor
        {
            get
            {
                if ((Brush)GetValue(StickColorProperty) == null && _parent != null)
                    return _parent.StickColor;
                else
                    return (Brush)GetValue(StickColorProperty);
            }
            set
            {
                SetValue(StickColorProperty, value);
            }
        }

        /// <summary>
        /// Enables or disables the DataPoint
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {   
            get
            {
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null && _parent != null)
                    return Parent.Enabled;
                else
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the Exploded property. This is used in Pie/Doughnut, Funnel and Pyramid charts.
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Exploded
        {
            get
            {
                return (GetValue(ExplodedProperty) == null) ? ((_parent != null) ? _parent.Exploded : false) : (Boolean)GetValue(ExplodedProperty);
            }
            set
            {
                SetValue(ExplodedProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LightingEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> LightingEnabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(LightingEnabledProperty) == null && _parent != null)
                {
                    return _parent.LightingEnabled;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(LightingEnabledProperty);
                }
            }
            set
            {
                SetValue(LightingEnabledProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the ShadowEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> ShadowEnabled
        {
            get
            {

                if ((Nullable<Boolean>)GetValue(ShadowEnabledProperty) == null && _parent != null)
                {
                    return _parent.ShadowEnabled;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(ShadowEnabledProperty);
                }
                
            }
            set
            {
                SetValue(ShadowEnabledProperty, value);
            }
        }
        
        #region Label Properties

        /// <summary>
        /// Get or set the LabelEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> LabelEnabled
        {
            get
            {
                if (GetValue(LabelEnabledProperty) == null && _parent != null)
                    return _parent.LabelEnabled;
                else
                    return (Nullable<Boolean>)GetValue(LabelEnabledProperty);
            }
            set
            {
                SetValue(LabelEnabledProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LabelText property
        /// </summary>
        public String LabelText
        {
            get
            {
                if (String.IsNullOrEmpty((String)GetValue(LabelTextProperty)) && _parent != null)
                    return _parent.LabelText;
                else
                    return (String)GetValue(LabelTextProperty);
            }
            set
            {   
                SetValue(LabelTextProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LabelFontFamily property
        /// </summary>
        public FontFamily LabelFontFamily
        {
            get
            {
                if (GetValue(LabelFontFamilyProperty) == null && _parent != null)
                    return _parent.LabelFontFamily;
                else
                    return (FontFamily)GetValue(LabelFontFamilyProperty);
            }
            set
            {
                SetValue(LabelFontFamilyProperty, value);
            }
        }
        
        /// <summary>
        /// GEt or set the LabelFontSize property
        /// </summary>
#if SL
       [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LabelFontSize
        {
            get
            {
                if ((Nullable<Double>)GetValue(LabelFontSizeProperty) != null)
                    return (Nullable<Double>)GetValue(LabelFontSizeProperty);
                else if (_parent != null && _parent.LabelFontSize != 0)
                        return _parent.LabelFontSize;
                else
                    return 10;
            }
            set
            {
                SetValue(LabelFontSizeProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LabelFontColor property
        /// </summary>
        public Brush LabelFontColor
        {
            get
            {
                if (GetValue(LabelFontColorProperty) == null && _parent != null)
                    return _parent.LabelFontColor;
                else
                    return (Brush)GetValue(LabelFontColorProperty);
                    
            }
            set
            {
                SetValue(LabelFontColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LabelFontWeight property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontWeightConverter))]
#endif 
        public Nullable<FontWeight> LabelFontWeight
        {
            get
            {
                if ((Nullable<FontWeight>)GetValue(LabelFontWeightProperty) == null && _parent != null)
                   return _parent.LabelFontWeight;
                else
                    return (Nullable<FontWeight>)GetValue(LabelFontWeightProperty);
                    
            }
            set
            {
                SetValue(LabelFontWeightProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LabelFontStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Visifire.Commons.Converters.FontStyleConverter))]
#endif
        public Nullable<FontStyle> LabelFontStyle
        {
            get
            {
                if ((Nullable<FontStyle>)GetValue(LabelFontStyleProperty) == null && _parent != null)
                    return _parent.LabelFontStyle;
                else
                    return (Nullable<FontStyle>)GetValue(LabelFontStyleProperty);
                    
            }
            set
            {
                SetValue(LabelFontStyleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LabelBackground property
        /// </summary>
        public Brush LabelBackground
        {
            get
            {
                if (GetValue(LabelBackgroundProperty) == null && _parent != null)
                    return _parent.LabelBackground;
                else
                    return (Brush)GetValue(LabelBackgroundProperty);
                    
            }
            set
            {
                SetValue(LabelBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LabelAngle property
        /// </summary>
        public Double LabelAngle
        {
            get
            {
                if (Double.IsNaN((Double)GetValue(LabelAngleProperty)) && _parent != null)
                    return _parent.LabelAngle;
                else
                    return (Double)GetValue(LabelAngleProperty);
            }
            set
            {

                if (value > 90 || value < -90)
                    throw (new Exception("Invalid property value:: LabelAngle should be greater than -90 and less than 90."));

                SetValue(LabelAngleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LabelStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableLabelStylesConverter))]
#endif
        public Nullable<LabelStyles> LabelStyle
        {
            get
            {
                if ((Nullable<LabelStyles>)GetValue(LabelStyleProperty) == null && _parent != null)
                {
                    IsLabelStyleSet = false;
                    return _parent.LabelStyle;
                }
                else
                {
                    IsLabelStyleSet = true;
                    return (Nullable<LabelStyles>)GetValue(LabelStyleProperty);
                }

            }
            set
            {
                SetValue(LabelStyleProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LabelLineEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> LabelLineEnabled
        {
            get
            {
                if (LabelEnabled != null && (Boolean)LabelEnabled)
                {
                    Nullable<Boolean> retVal = null;
                    if ((Nullable<Boolean>)GetValue(LabelLineEnabledProperty) == null)
                    {
                        if (_parent != null)
                        {
                            if ((_parent.RenderAs == RenderAs.Pie || _parent.RenderAs == RenderAs.Doughnut || _parent.RenderAs == RenderAs.StreamLineFunnel || _parent.RenderAs == RenderAs.SectionFunnel || _parent.RenderAs == RenderAs.Pyramid) && (LabelStyle == LabelStyles.OutSide))
                                //retVal = (_parent != null) ? _parent.LabelLineEnabled : null;
                                retVal = (_parent.LabelLineEnabled != null) ? _parent.LabelLineEnabled : true;
                            else
                                retVal = false;
                        }
                        else
                            retVal = false;
                    }
                    else
                        retVal = (Nullable<Boolean>)GetValue(LabelLineEnabledProperty);

                    //if(retVal == null && _parent != null)
                    //    retVal = ((_parent.RenderAs == RenderAs.Pie || _parent.RenderAs == RenderAs.Doughnut) && (LabelStyle == LabelStyles.OutSide)) ? true : false;

                    return retVal;
                }

                return false;
            }
            set
            {
                SetValue(LabelLineEnabledProperty, value);
            }
        }
        //public Nullable<Boolean> LabelLineEnabled
        //{
        //    get
        //    {
        //        if (LabelEnabled != null && (Boolean)LabelEnabled)
        //        {   
        //            Nullable<Boolean> retVal = null;
        //            if ((Nullable<Boolean>)GetValue(LabelLineEnabledProperty) == null)
        //            {
        //                if (_parent != null)
        //                {
        //                    if ((_parent.RenderAs == RenderAs.Pie || _parent.RenderAs == RenderAs.Doughnut) && (LabelStyle == LabelStyles.OutSide))
        //                        retVal = (_parent.LabelLineEnabled != null) ? _parent.LabelLineEnabled : true;
        //                    else if (Chart != null && (Chart as Chart).PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
        //                        retVal = (_parent.LabelLineEnabled != null) ? _parent.LabelLineEnabled : true;
        //                    else
        //                        retVal = false;
        //                }
        //                else
        //                    retVal = false;
        //            }
        //            else
        //                retVal = (Nullable<Boolean>)GetValue(LabelLineEnabledProperty);

        //            return retVal;
        //        }

        //        return false;
        //    }
        //    set
        //    {
        //        SetValue(LabelLineEnabledProperty, value);
        //    }
        //}

        
        /// <summary>
        /// Get or set the LabelLineColor property
        /// </summary>
        public Brush LabelLineColor
        {
            get
            {   
                if (GetValue(LabelLineColorProperty) == null && _parent != null)
                    return _parent.LabelLineColor;
                else
                    return (Brush)GetValue(LabelLineColorProperty);
            }
            set
            {
                SetValue(LabelLineColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the LabelLineThickness property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> LabelLineThickness
        {
            get
            {
                if ((Nullable<Double>)GetValue(LabelLineThicknessProperty) != null)
                    return (Double)GetValue(LabelLineThicknessProperty);
                else if (_parent != null && _parent.LabelLineThickness != 0)
                    return _parent.LabelLineThickness;
                else
                    return 0.5;
            }
            set
            {
                SetValue(LabelLineThicknessProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LabelLineStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableLineStylesConverter))]
#endif
        public Nullable<LineStyles> LabelLineStyle
        {
            get
            {
                if ((Nullable<LineStyles>)GetValue(LabelLineStyleProperty) == null && _parent != null)
                    return _parent.LabelLineStyle;
                else
                    return (Nullable<LineStyles>)GetValue(LabelLineStyleProperty);
            }
            set
            {
                SetValue(LabelLineStyleProperty, value);
            }
        }
        
        #endregion

        #region Marker Properties

        /// <summary>
        /// Get or set the MarkerEnabled property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> MarkerEnabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(MarkerEnabledProperty) == null && _parent != null)
                    return _parent.MarkerEnabled;
                else
                    return (Nullable<Boolean>)GetValue(MarkerEnabledProperty);
            }
            set
            {   
                SetValue(MarkerEnabledProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerStyle property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableMarkerTypesConverter))]
#endif
        public Nullable<MarkerTypes> MarkerType
        {
            get
            {
                if ((Nullable<MarkerTypes>)GetValue(MarkerTypeProperty) == null && _parent != null)
                    return _parent.MarkerType;
                else
                   return (Nullable<MarkerTypes>)GetValue(MarkerTypeProperty);
            }
            set
            {
                SetValue(MarkerTypeProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerBorderThickness property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableThicknessConverter))]
#endif
        public Nullable<Thickness> MarkerBorderThickness
        {
            get
            {
                if ((Nullable<Thickness>)GetValue(MarkerBorderThicknessProperty) == null)
                {
                    if (_parent == null)
                        return new Thickness(0);

                    if (_parent.MarkerBorderThickness == null)
                    {
                        if (this.Parent.RenderAs == RenderAs.Point && this.MarkerType == MarkerTypes.Cross)
                            return new Thickness(1);
                        else if (this.Parent.RenderAs == RenderAs.Bubble || (this.Parent.RenderAs == RenderAs.Point && this.MarkerType != MarkerTypes.Cross))
                            return new Thickness(0);
                        else
                            return new Nullable<Thickness>(new Thickness((Double)MarkerSize / 6));
                    }
                    else
                        return _parent.MarkerBorderThickness;
                }
                else
                    return (Nullable<Thickness>)GetValue(MarkerBorderThicknessProperty);

            }
            set
            {
                SetValue(MarkerBorderThicknessProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerBorderColor property
        /// </summary>
        public Brush MarkerBorderColor
        {
            get
            {
                if (GetValue(MarkerBorderColorProperty) != null)
                {
                    return (Brush)GetValue(MarkerBorderColorProperty);
                }
                else
                {
                    if (_parent == null)
                        return null;

                    if (_parent.MarkerBorderColor != null)
                        return (_parent.MarkerBorderColor);
                    else
                        return (_internalColor == null) ? _parent._internalColor : _internalColor;
                }
            }
            set
            {   
                SetValue(MarkerBorderColorProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerSize property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> MarkerSize
        {
            get
            {
                if ((Nullable<Double>)GetValue(MarkerSizeProperty) == null && _parent != null)
                    return _parent.MarkerSize;
                else
                    return (Nullable<Double>)GetValue(MarkerSizeProperty);
            }
            set
            {   
                SetValue(MarkerSizeProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerColor property
        /// </summary>
        public Brush MarkerColor
        {
            get
            {
                if (GetValue(MarkerColorProperty) == null && _parent != null)
                {
                    if (_parent.MarkerColor == null)
                        return Graphics.WHITE_BRUSH;
                    else
                        return _parent.MarkerColor;
                }
                else
                    return (Brush)GetValue(MarkerColorProperty);
            }
            set
            {
                SetValue(MarkerColorProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the MarkerScale property
        /// </summary>
#if SL
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableDoubleConverter))]
#endif
        public Nullable<Double> MarkerScale
        {
            get
            {
                if ((Nullable<Double>)GetValue(MarkerScaleProperty) == null && _parent != null)
                    return _parent.MarkerScale;
                else
                    return (Nullable<Double>)GetValue(MarkerScaleProperty);
            }
            set
            {
                SetValue(MarkerScaleProperty, value);
            }
        }
        
        #endregion Marker Properties

        /// <summary>
        /// Get or set the ToolTipText for the DataPoint
        /// </summary>
        public override String ToolTipText
        {   
            get
            {   
                if ((Chart != null && !String.IsNullOrEmpty((Chart as Chart).ToolTipText)))
                    return null;

                if ((String)GetValue(ToolTipTextProperty) == String.Empty && _parent != null)
                    return _parent.ToolTipText;
                else
                    return (String)GetValue(ToolTipTextProperty);
            }
            set
            {
                SetValue(ToolTipTextProperty, value);
            }
        }

        /// <summary>
        /// Get or set the ShowInLegend property
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> ShowInLegend
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(ShowInLegendProperty) == null && _parent != null)
                {
                    return _parent.ShowInLegend;
                }
                else
                {
                    return (Nullable<Boolean>)GetValue(ShowInLegendProperty);
                }
            }
            set
            {
                SetValue(ShowInLegendProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the LegendText
        /// </summary>
        public String LegendText
        {   
            get
            {   
                if (String.IsNullOrEmpty((String)GetValue(LegendTextProperty)) && _parent != null)
                {
                    if (String.IsNullOrEmpty(_parent.LegendText))
                        if (this.Parent.RenderAs == RenderAs.Pie || this.Parent.RenderAs == RenderAs.Doughnut || this.Parent.RenderAs == RenderAs.SectionFunnel || this.Parent.RenderAs == RenderAs.StreamLineFunnel || this.Parent.RenderAs == RenderAs.Pyramid)
                        {
                            if (Parent.InternalXValueType != ChartValueTypes.Numeric)
                                return this.TextParser("#XValue");
                            else
                                return this.TextParser("#AxisXLabel");
                        }
                        else
                        {
                            if(_isAutoName)
                            {
                                String[] s = Name.Split('_');
                                return s[0];
                            }
                            else
                                return Name;
                        }
                    else
                        return _parent.LegendText;
                }
                else
                {
                    return (String)GetValue(LegendTextProperty);
                }
            }
            set
            {
                SetValue(LegendTextProperty, value);
            }
        }
        
        /// <summary>
        /// Get or set the BorderThickness property
        /// </summary>
        public new Thickness BorderThickness
        {
            get
            {
                Thickness retVal = (Thickness)GetValue(BorderThicknessProperty);

                if (retVal == new Thickness(0, 0, 0, 0))
                    retVal = (_parent == null) ? retVal : _parent.InternalBorderThickness;

                return retVal;
            }
            set
            {
#if SL
                if (BorderThickness != value)
                {   
                    InternalBorderThickness = value;
                    SetValue(BorderThicknessProperty, value);
                    InvokeUpdateVisual(VcProperties.BorderThickness, value);
                    // FirePropertyChanged(VcProperties.BorderThickness);
                }
#else           
                SetValue(BorderThicknessProperty, value);
#endif
            }
        }
        
        /// <summary>
        /// Get or set the BorderColor property
        /// </summary>
        public Brush BorderColor
        {
            get
            {   
                if (GetValue(BorderColorProperty) == null && _parent != null)
                {
                    return _parent.BorderColor;
                }
                else
                {
                    return (Brush)GetValue(BorderColorProperty);
                }
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        /// <summary>
        /// Get or set the BorderStyle property
        /// </summary>
#if SL  
        [System.ComponentModel.TypeConverter(typeof(Converters.NullableBorderStylesConverter))]
#endif
        public Nullable<BorderStyles> BorderStyle
        {   
            get
            {
                if ((Nullable<BorderStyles>)GetValue(BorderStyleProperty) == null && _parent != null)
                {
                    return _parent.BorderStyle;
                }
                else
                {
                    return (Nullable<BorderStyles>)GetValue(BorderStyleProperty); 
                }
            }
            set
            {
                SetValue(BorderStyleProperty, value);
            }
        }

        /// <summary>
        /// Get or set the RadiusX property
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public Nullable<CornerRadius> RadiusX
        {
            get
            {
                if ((Nullable<CornerRadius>)GetValue(RadiusXProperty) == null && _parent != null)
                   return _parent.RadiusX; 
                else
                    return (Nullable<CornerRadius>)GetValue(RadiusXProperty);
            }
            set
            {
                SetValue(RadiusXProperty, value);
            }
        }

        /// <summary>
        /// Get or set the RadiusY property
        /// </summary>
#if WPF
        [System.ComponentModel.TypeConverter(typeof(System.Windows.CornerRadiusConverter))]
#else
        [System.ComponentModel.TypeConverter(typeof(Converters.CornerRadiusConverter))]
#endif
        public Nullable<CornerRadius> RadiusY
        {
            get
            {
                if ((Nullable<CornerRadius>)GetValue(RadiusYProperty) == null && _parent != null)
                    return _parent.RadiusY;
                else
                    return (Nullable<CornerRadius>)GetValue(RadiusYProperty);
            }
            set
            {
                SetValue(RadiusYProperty, value);
            }
        }
        
        /// <summary>
        /// Parent of InternalDataPoints
        /// </summary>
        public new DataSeries Parent
        {
            get
            {   
                return _parent;
            }
            internal set
            {
                System.Diagnostics.Debug.Assert(typeof(DataSeries).Equals(value.GetType()), "Unknown Parent", "DataPoint should have DataSeries as Parent"); 
                _parent = value;
            }
        }

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        internal void InvokeUpdateVisual(VcProperties property, object newValue)
        {
            if (IsNotificationEnable)
            {
                if (Parent == null || !ValidatePartialUpdate(Parent.RenderAs, property))
                    return;
                UpdateVisual(property, newValue, false);
               // Chart.Dispatcher.BeginInvoke(new Action<VcProperties, object>(UpdateVisual), new object[] { property, newValue });
            }
        }

        /// <summary>
        /// Partial update of color property for not supported partial update tchart type
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        internal void PartialUpdateOfColorProperty(Brush newValue)
        {
             #region Color

                    if (Faces != null && Faces.Parts != null)
                    {
                        Brush value = (newValue != null) ? newValue : Color;

                        switch (Parent.RenderAs)
                        {   
                            case RenderAs.Pie:
                            case RenderAs.Doughnut:

                                SectorChartShapeParams pieParams = (SectorChartShapeParams)this.VisualParams;

                                if (pieParams != null)
                                {
                                    pieParams.Background = (Brush)value;

                                    if (!(Parent.Chart as Chart).View3D)
                                    {
                                        if (Faces.Parts.Count > 0 && Faces.Parts[0] != null)
                                            (Faces.Parts[0] as Shape).Fill = (Boolean)Parent.LightingEnabled ? Graphics.GetLightingEnabledBrush((Brush)value, "Radial", null) : (Brush)value;

                                        if (Faces.Parts.Count > 1 && Faces.Parts[1] != null)
                                            (Faces.Parts[1] as Shape).Fill = (pieParams.StartAngle > Math.PI * 0.5 && pieParams.StartAngle <= Math.PI * 1.5) ? PieChart.GetDarkerBevelBrush(pieParams.Background, pieParams.StartAngle * 180 / Math.PI + 135) : PieChart.GetLighterBevelBrush(pieParams.Background, -pieParams.StartAngle * 180 / Math.PI);

                                        if (Faces.Parts.Count > 2 && Faces.Parts[2] != null)
                                            (Faces.Parts[2] as Shape).Fill = (pieParams.StopAngle > Math.PI * 0.5 && pieParams.StopAngle <= Math.PI * 1.5) ? PieChart.GetLighterBevelBrush(pieParams.Background, pieParams.StopAngle * 180 / Math.PI + 135) : PieChart.GetDarkerBevelBrush(pieParams.Background, -pieParams.StopAngle * 180 / Math.PI);

                                        if (Faces.Parts.Count > 3 && Faces.Parts[3] != null)
                                            (Faces.Parts[3] as Shape).Fill = (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI) ? PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(-0.745, -0.85), Graphics.GenerateDoubleCollection(0, 1)) : (Faces.Parts[3] as Shape).Fill = PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(0.745, -0.99), Graphics.GenerateDoubleCollection(0, 1));

                                        if (Parent.RenderAs == RenderAs.Doughnut && Faces.Parts.Count > 4 && Faces.Parts[4] != null)
                                            (Faces.Parts[4] as Shape).Fill = (pieParams.MeanAngle > 0 && pieParams.MeanAngle < Math.PI) ? PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(-0.745, -0.85), Graphics.GenerateDoubleCollection(0, 1)) : (Faces.Parts[4] as Shape).Fill = PieChart.GetCurvedBevelBrush(pieParams.Background, pieParams.MeanAngle * 180 / Math.PI + 90, Graphics.GenerateDoubleCollection(0.745, -0.99), Graphics.GenerateDoubleCollection(0, 1));
                                    }
                                    else
                                    {
                                        //foreach (FrameworkElement fe in Faces.Parts)
                                        //    if (fe != null) (fe as Shape).Fill = pieParams.Lighting ? Graphics.GetLightingEnabledBrush(pieParams.Background, "Radial", null) : pieParams.Background;

                                        int i = 0;
                                        foreach (FrameworkElement fe in Faces.Parts)
                                        {
                                            PieFaceTypes pieFaceType = (PieFaceTypes)Enum.Parse(typeof(PieFaceTypes), Enum.GetName(typeof(PieFaceTypes), i), true);
                                            if (fe != null)
                                                (fe as Shape).Fill = PieChart.Get3DFaceColor(Parent.RenderAs, pieParams.Lighting, pieParams.Background, pieFaceType, pieParams.StartAngle, pieParams.StopAngle, pieParams.TiltAngle);
                                            i++;

                                            if (i >= 4)
                                                i = 4;
                                        }
                                    }
                                }

                                break;

                            case RenderAs.SectionFunnel:
                            case RenderAs.StreamLineFunnel:
                            
                                TriangularChartSliceParms funnelSliceParms = (TriangularChartSliceParms)this.VisualParams;

                                if (funnelSliceParms != null)
                                {
                                    foreach (Shape path in Faces.Parts)
                                    {
                                        FunnelChart.ReCalculateAndApplyTheNewBrush(path, (Brush)value, (Boolean)LightingEnabled, (Parent.Chart as Chart).View3D, funnelSliceParms);
                                    }
                                }
                                break;

                            case RenderAs.Pyramid:
                                TriangularChartSliceParms pyramidSliceParms = (TriangularChartSliceParms)this.VisualParams;

                                if (pyramidSliceParms != null)
                                {
                                    foreach (Shape path in Faces.Parts)
                                    {
                                        PyramidChart.ReCalculateAndApplyTheNewBrush(path, (Brush)value, (Boolean)LightingEnabled, (Parent.Chart as Chart).View3D, pyramidSliceParms);
                                    }
                                }

                                break;
                        }
                    }

                    UpdateMarkerAndLegend(this, newValue);

                    #endregion
        }

        /// <summary>
        /// Reset VisualParams for non-partial update chart types
        /// </summary>
        private void ResetVisualParams4NonPartialUpdateChartTypes()
        {
            if (Parent != null)
            {
                foreach (DataPoint dp in Parent.InternalDataPoints)
                {
                    if (Double.IsNaN(dp.InternalYValue))
                        VisualParams = null;
                }
            }
        }

        /// <summary>
        /// UpdateVisual is used for partial rendering
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <returns>Is need to update all DataPoints on axis change</returns>
        internal Boolean UpdateVisual(VcProperties property, object newValue, Boolean recursive)
        {
            Chart chart = Chart as Chart;
                       
            if (IsInDesignMode)
            {
                if(chart != null)
                    chart.RENDER_LOCK = false;

                FirePropertyChanged(property);

                return false;
            }

            if (chart != null && chart.IS_FULL_RENDER_PENDING)
                return false;

            /* Don’t proceed for the chart types which do not support partial update of properties
             * (Except Color property).
             */
            if (!recursive && NonPartialUpdateChartTypes(Parent.RenderAs))
            {
                if (property == VcProperties.Color)
                    PartialUpdateOfColorProperty(newValue as Brush);
                else
                {
                    ResetVisualParams4NonPartialUpdateChartTypes();
                    FirePropertyChanged(property);
                }

                return false;
            }

            /* If there is more than one DataSeries, and IncludePercentageInLegend property or 
               IncludeYValueInLegend property are set to true Legends need to update according to new YValue. 
               So charts need to re-render. 
             */

            if (((property == VcProperties.YValue || property == VcProperties.Enabled) && (Parent.IncludePercentageInLegend || Parent.IncludeYValueInLegend) && (Boolean)Parent.ShowInLegend)
            || (Parent != null && !Parent.InternalDataPoints.Contains(this)))
            {
                FirePropertyChanged(property);
                return false;
            }

            //if ((property == VcProperties.YValue || property == VcProperties.Enabled) && (Parent.IncludePercentageInLegend || Parent.IncludeYValueInLegend) && (Boolean)Parent.ShowInLegend)
            //{   
            //    FirePropertyChanged(property);
            //    return false;
            //}

            if (chart != null)
                chart._internalPartialUpdateEnabled = true;

            if (!chart.PARTIAL_DP_RENDER_LOCK || recursive)
            {   
                Boolean updateAllDpsOnAxisChange = false;
                Boolean renderAxis = false;
                PlotGroup plotGroup = Parent.PlotGroup;
                AxisRepresentations axisRepresentation = AxisRepresentations.AxisX;

                /* Line and bubble are first while updating DataPoints one by one. So to take advantage of updating DataPoints one by one
                conditions are written below */
                if (Parent.RenderAs == RenderAs.Spline || Parent.RenderAs == RenderAs.QuickLine
                    || (Parent.RenderAs != RenderAs.Line && Parent.RenderAs != RenderAs.StepLine) 
                    || ((Parent.RenderAs == RenderAs.Line || Parent.RenderAs == RenderAs.StepLine) && chart.AnimatedUpdate == false))
                {
                    if ((!recursive && (property == VcProperties.YValue) && (Parent.RenderAs == RenderAs.Spline || Parent.RenderAs == RenderAs.QuickLine))
                    ||
                    (!recursive && (property == VcProperties.YValue) && (chart.PlotDetails.ListOfAllDataPoints.Count > 1000 || !(Boolean)chart.AnimatedUpdate))
                    ||
                    (!recursive && property == VcProperties.XValue && (!RenderHelper.IsLineCType(Parent) || Parent.RenderAs != RenderAs.Bubble || Parent.RenderAs != RenderAs.Point)))
                    {   
                        chart.PARTIAL_DP_RENDER_LOCK = true;
                        chart._partialRenderBlockedCount = 0;
                        chart._datapoint2UpdatePartially = new Dictionary<DataPoint, VcProperties>();
                        chart._datapoint2UpdatePartially.Add(this, property);
                    }
                    else
                        chart.PARTIAL_DP_RENDER_LOCK = false;
                }
                
                if (!chart.PARTIAL_DP_RENDER_LOCK || recursive)
                {
                    if (Parent.RenderAs == RenderAs.Area)
                    {
                        if (property == VcProperties.BorderThickness || property == VcProperties.BorderColor || property == VcProperties.BorderStyle)
                            return false;
                    }

                    if (chart.PlotDetails == null || plotGroup == null)
                        return true;

                    if (property == VcProperties.YValue || property == VcProperties.YValues)
                    {
                        Double OldMaxYValue = chart.PlotDetails.GetAxisYMaximumDataValue(plotGroup.AxisY);
                        Double OldMinYValue = chart.PlotDetails.GetAxisYMinimumDataValue(plotGroup.AxisY);

                        //Object oldValue = null;
                        //if (Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.Stock)
                        //    oldValue = ConvertActualOldYValueToLogarithmicValue(_oldYValues);
                        //else
                        //    oldValue = ConvertActualOldYValueToLogarithmicValue(_oldYValue);

                        Object oldValue = (Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.Stock) ? (Object)_oldYValues : _oldYValue;

                        chart.PlotDetails.ReCreate(this, ElementTypes.DataPoint, property, oldValue, newValue);

                        Double NewMaxYValue = chart.PlotDetails.GetAxisYMaximumDataValue(plotGroup.AxisY);
                        Double NewMinYValue = chart.PlotDetails.GetAxisYMinimumDataValue(plotGroup.AxisY);

                        System.Diagnostics.Debug.WriteLine("OldAxisMaxY = " + OldMaxYValue.ToString() + " OldAxisMinY=" + OldMinYValue.ToString());
                        System.Diagnostics.Debug.WriteLine("NewAxisMaxY = " + NewMaxYValue.ToString() + " NewAxisMinY=" + NewMinYValue.ToString());

                        if (plotGroup.AxisY.AxisMinimum == null || plotGroup.AxisY.AxisMaximum == null)
                        {
                            if (NewMaxYValue != OldMaxYValue || (Double)NewMinYValue != OldMinYValue)
                            {
                                renderAxis = true;
                                axisRepresentation = AxisRepresentations.AxisY;
                                System.Diagnostics.Debug.WriteLine("RenderAxis1 =" + renderAxis.ToString());
                            }
                        }

                        if (plotGroup.AxisY.ViewportRangeEnabled)
                        {
                            renderAxis = true;
                            axisRepresentation = AxisRepresentations.AxisY;
                        }
                    }
                    else if (property == VcProperties.XValue)
                    {
                        Double OldMaxXValue = chart.PlotDetails.GetAxisXMaximumDataValue(plotGroup.AxisX);
                        Double OldMinXValue = chart.PlotDetails.GetAxisXMinimumDataValue(plotGroup.AxisX);
                        Double OldMaxYValue = chart.PlotDetails.GetAxisYMaximumDataValue(plotGroup.AxisY);
                        Double OldMinYValue = chart.PlotDetails.GetAxisYMinimumDataValue(plotGroup.AxisY);

                        chart.PlotDetails.ReCreate(this, ElementTypes.DataPoint, property, null, newValue);

                        Double NewMaxXValue = chart.PlotDetails.GetAxisXMaximumDataValue(plotGroup.AxisX);
                        Double NewMinXValue = chart.PlotDetails.GetAxisXMinimumDataValue(plotGroup.AxisX);
                        Double NewMaxYValue = chart.PlotDetails.GetAxisYMaximumDataValue(plotGroup.AxisY);
                        Double NewMinYValue = chart.PlotDetails.GetAxisYMinimumDataValue(plotGroup.AxisY);

                        //System.Diagnostics.Debug.WriteLine("OldAxisMaxX = " + OldAxisMaxX.ToString() + " OldAxisMinX=" + OldAxisMinX.ToString());
                        //System.Diagnostics.Debug.WriteLine("NewAxisMaxX = " + NewAxisMaxX.ToString() + " NewAxisMinX=" + NewAxisMinX.ToString());

                        if (plotGroup.AxisX.AxisMinimum == null || plotGroup.AxisX.AxisMaximum == null)
                        {
                            if (NewMaxXValue != OldMaxXValue || (Double)NewMinXValue != OldMinXValue)
                            {
                                renderAxis = true;
                                axisRepresentation = AxisRepresentations.AxisX;
                            }
                        }

                        if (plotGroup.AxisY.AxisMinimum == null || plotGroup.AxisY.AxisMaximum == null)
                        {
                            if (NewMaxYValue != OldMaxYValue || (Double)NewMinYValue != OldMinYValue)
                            {
                                renderAxis = true;
                                axisRepresentation = AxisRepresentations.AxisY;
                            }
                        }
                    }

                    // renderAxis = false;
                    Double oldZeroBaseLineY = 0, oldZeroBaseLineX = 0;
                    if (renderAxis == true)
                    {
                        if (property == VcProperties.YValue)
                            oldZeroBaseLineY = plotGroup.AxisY._zeroBaseLinePixPosition;
                        else if (property == VcProperties.XValue)
                            oldZeroBaseLineX = plotGroup.AxisX._zeroBaseLinePixPosition;
                    }
                    System.Diagnostics.Debug.WriteLine("RenderAxis2 =" + renderAxis.ToString());
                    chart.ChartArea.PrePartialUpdateConfiguration(this, ElementTypes.DataPoint, property, null, null, false, false, renderAxis, axisRepresentation, true);

                    if (property == VcProperties.YValue)
                    {
                        if (plotGroup.AxisY._oldInternalAxisMinimum == plotGroup.AxisY.InternalAxisMinimum &&
                            plotGroup.AxisY._oldInternalAxisMaximum == plotGroup.AxisY.InternalAxisMaximum)
                            renderAxis = false;
                    }
                    else if (property == VcProperties.XValue)
                    {
                        if (plotGroup.AxisX._oldInternalAxisMinimum == plotGroup.AxisX.InternalAxisMinimum &&
                            plotGroup.AxisX._oldInternalAxisMaximum == plotGroup.AxisX.InternalAxisMaximum)
                            renderAxis = false;
                    }

                    if (renderAxis == true)
                    {   
                        if (property == VcProperties.YValue)
                        {   
                            if (oldZeroBaseLineY == plotGroup.AxisY._zeroBaseLinePixPosition)
                                renderAxis = false;

                        }
                        else if (property == VcProperties.XValue)
                        {
                            if (oldZeroBaseLineX == plotGroup.AxisX._zeroBaseLinePixPosition)
                                renderAxis = false;
                        }

                        //if (renderAxis == false)
                            updateAllDpsOnAxisChange = true;
                    }
                }
                
                Parent._isZooming = false;

                System.Diagnostics.Debug.WriteLine("RenderAxis3 =" + renderAxis.ToString());
                System.Diagnostics.Debug.WriteLine("updateAllDpsOnAxisChange =" + updateAllDpsOnAxisChange.ToString());
                if (chart.PARTIAL_DP_RENDER_LOCK)
                {   
                    chart.Dispatcher.BeginInvoke(new Action<Chart, VcProperties, object, Boolean>(RenderHelper.UpdateVisualObject), new object[] { chart, property, newValue , true });
                    chart.Dispatcher.BeginInvoke(new Action(ActivePartialUpdateRenderLock));

                    // RenderHelper.UpdateVisualObject(chart, property, newValue);
                }
                else if (updateAllDpsOnAxisChange)
                {
                    if (recursive && (!(Boolean)chart.AnimatedUpdate || chart.PlotDetails.ListOfAllDataPoints.Count > 1000))
                    {
                        if (property == VcProperties.YValue)
                        {
                            if (Double.IsNaN((Double)newValue))
                            {
                                Faces = null;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        foreach (DataSeries ds in chart.InternalSeries)
                        {
                            foreach (DataPoint dp in ds.InternalDataPoints)
                            {
                                if (Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.Stock)
                                    dp._oldYValues = (Double[])ConvertActualOldYValueToLogarithmicValue(dp._oldYValues);
                                else
                                    dp._oldYValue = (Double)ConvertActualOldYValueToLogarithmicValue(dp._oldYValue);

                                RenderHelper.UpdateVisualObject(ds.RenderAs, dp, property, newValue, !updateAllDpsOnAxisChange);

                                if (ds.RenderAs == RenderAs.StackedArea || ds.RenderAs == RenderAs.StackedArea100)
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.Stock)
                        _oldYValues = (Double[])ConvertActualOldYValueToLogarithmicValue(_oldYValues);
                    else
                        _oldYValue = (Double)ConvertActualOldYValueToLogarithmicValue(_oldYValue);

                    RenderHelper.UpdateVisualObject(Parent.RenderAs, this, property, newValue, renderAxis);
                }

                #region Update list of selected DataPoints

                if (Parent.ListOfSelectedDataPoints != null)
                {
                    if (Selected)
                    {
                        if (!Parent.ListOfSelectedDataPoints.Contains(this))
                            Parent.ListOfSelectedDataPoints.Add(this);
                    }
                    else
                    {
                        if (Parent.ListOfSelectedDataPoints.Contains(this))
                            Parent.ListOfSelectedDataPoints.Remove(this);
                    }
                }

                #endregion

                if (property == VcProperties.Color && Parent.RenderAs != RenderAs.QuickLine)
                    UpdateLegendMarker(this, (Brush)newValue);

                if(Parent != null)
                    chart.Dispatcher.BeginInvoke(new Action<DataPoint>(Parent.AttachOrDetachInteractivity4DataPoint), new object[] { this });

                // chart._renderLock = false;
            }
            else
            {
                if (!chart._datapoint2UpdatePartially.Keys.Contains(this))
                {   
                    chart._datapoint2UpdatePartially.Add(this, property);
                    chart._partialRenderBlockedCount++;
                }
            }

            return false;
        }
        
        #endregion

        private Object ConvertActualOldYValueToLogarithmicValue(Object oldValue)
        {
            Chart chart = Chart as Chart;

            if (Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.Stock)
            {
                if (oldValue != null)
                {
                    Double[] convertedYValues = new Double[4];
                    convertedYValues[0] = (((Double[])oldValue).Length > 0 ? ConvertYValue2LogarithmicValue(chart, ((Double[])oldValue)[0], Parent.AxisYType) : Double.NaN);
                    convertedYValues[1] = (((Double[])oldValue).Length > 1 ? ConvertYValue2LogarithmicValue(chart, ((Double[])oldValue)[1], Parent.AxisYType) : Double.NaN);
                    convertedYValues[2] = (((Double[])oldValue).Length > 2 ? ConvertYValue2LogarithmicValue(chart, ((Double[])oldValue)[2], Parent.AxisYType) : Double.NaN);
                    convertedYValues[3] = (((Double[])oldValue).Length > 3 ? ConvertYValue2LogarithmicValue(chart, ((Double[])oldValue)[3], Parent.AxisYType) : Double.NaN);

                    oldValue = (from value in convertedYValues where !Double.IsNaN(value) select value).ToArray();
                }
            }
            else
                oldValue = ConvertYValue2LogarithmicValue(chart, Convert.ToDouble(oldValue), Parent.AxisYType);

            return oldValue;
        }

        public void ActivePartialUpdateRenderLock()
        {
            if(Chart != null)
                (Chart as Chart).PARTIAL_DP_RENDER_LOCK = false;
            // Visifire.Profiler.Profiler.End("Render");
            // Visifire.Profiler.Profiler.Report("Render", true, false);
        }

        #region Internal Properties

#if SL
        /// <summary>
        /// Identifies the Visifire.Charts.Axis.Opacity dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.Opacity dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalOpacityProperty = DependencyProperty.Register
            ("InternalOpacity",
            typeof(Double),
            typeof(DataPoint),
            new PropertyMetadata(1.0, OnOpacityPropertyChanged));


        /// <summary>
        /// Identifies the Visifire.Charts.Title.BorderThickness dependency property.
        /// </summary>
        /// <returns>
        /// The identifier for the Visifire.Charts.DataSeries.BorderThickness dependency property.
        /// </returns>
        private static readonly DependencyProperty InternalBorderThicknessProperty = DependencyProperty.Register
            ("InternalBorderThickness",
            typeof(Thickness),
            typeof(DataPoint),
            new PropertyMetadata(OnBorderThicknessPropertyChanged));

#endif

        /// <summary>
        /// Get or set the BorderThickness of title
        /// </summary>
        internal Thickness InternalBorderThickness
        {
            get
            {
                Thickness retVal = (Thickness)((_borderThickness == null) ? GetValue(BorderThicknessProperty) : _borderThickness);

                if (retVal == new Thickness(0, 0, 0, 0))
                    retVal = (_parent == null) ? retVal : _parent.InternalBorderThickness;

                return retVal;
            }
            set
            {
                _borderThickness = value;
            }
        }

        /// <summary>
        /// Get or set the Opacity property
        /// </summary>
        internal Double InternalOpacity
        {
            get
            {
                return (Double)(Double.IsNaN(_internalOpacity) ? GetValue(OpacityProperty) : _internalOpacity);
            }
            set
            {
                _internalOpacity = value;
            }
        }

        /// <summary>
        /// StoryBoard attached with the DataPoint for animation
        /// </summary>
        internal Storyboard Storyboard
        {
            get;
            set;
        }

        /// <summary>
        /// StoryBoard attached with the DataPoint for animation
        /// </summary>
        internal Storyboard StoryboardZValueAni
        {
            get;
            set;
        } 
        
        
        /// <summary>
        /// InternalXValue used for internally generated XValue of type double
        /// </summary>
        internal Double InternalXValue
        {
            get;
            set;
        }

        /// <summary>
        /// InternalXValue used for internally generated XValue of type DateTime
        /// </summary>
        internal DateTime InternalXValueAsDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// InternalXValue used for internally generated XValue of type DateTime
        /// </summary>
        internal DateTime ActualXValueAsDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Marker which appears in Legend
        /// </summary>
        internal Marker LegendMarker
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set geometric faces of DataPoint
        /// </summary>
        internal Faces Faces
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set geometric faces of DataPoint
        /// </summary>
        internal Faces ShadowFaces
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set label visual of the DataPoint
        /// </summary>
        internal FrameworkElement LabelVisual
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Marker associated with DataPoint
        /// </summary>
        internal Marker Marker
        {
            get;
            set;
        }

        internal Boolean IsTopOfStack
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Line for label
        /// (For Pie / Doughnut)
        /// </summary>
        internal Path LabelLine;
        
        /// <summary>
        /// Get or set Storyboard for explode animation
        /// </summary>
        internal Storyboard ExplodeAnimation
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set Storyboard for unexplode animation
        /// </summary>
        internal Storyboard UnExplodeAnimation
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set current visual parameters
        /// </summary>
        internal object VisualParams
        {
            get;
            set;
        }

        internal Boolean IsLabelStyleSet
        {
            get;
            set;
        }

        #endregion

        #region Private Eelegates

        #endregion

        #region Private Properties

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        private new Brush BorderBrush
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// SelectedProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            DataPoint dataPoint = d as DataPoint;            
            ApplySelectionChanged(dataPoint, (Boolean)e.NewValue);
        }

        private static void ApplySelectionChanged(DataPoint dataPoint, Boolean selectedValue)
        {
            if (dataPoint.Parent == null)
                return;

            Boolean selected = selectedValue & dataPoint.Parent.SelectionEnabled;

            if (dataPoint.IsNotificationEnable)
            {
                if (dataPoint.Parent.ListOfSelectedDataPoints != null)
                    dataPoint.Parent.ListOfSelectedDataPoints.Add(dataPoint);
            }

            if (selected)
            {
                dataPoint.Select(true);

                if(dataPoint.Parent.SelectionMode == SelectionModes.Single ||  dataPoint.Parent.RenderAs == RenderAs.SectionFunnel || dataPoint.Parent.RenderAs == RenderAs.StreamLineFunnel || dataPoint.Parent.RenderAs == RenderAs.Pyramid)
                   dataPoint.DeSelectOthers();
            }
            else
                dataPoint.DeSelect(dataPoint , true, true);
        }

        /// <summary>
        /// HrefTargetProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.HrefTarget, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.HrefTarget);
        }

        /// <summary>
        /// HrefProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnHrefChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;

            dataPoint.InvokeUpdateVisual(VcProperties.Href, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.Href);
        }

//#if WPF
//        /// <summary>
//        /// OpacityProperty changed call back function
//        /// </summary>
//        /// <param name="d">DependencyObject</param>
//        /// <param name="e">DependencyPropertyChangedEventArgs</param>
//        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            DataPoint dataPoint = d as DataPoint;

//            dataPoint.InvokeUpdateVisual(VcProperties.Opacity, e.NewValue);
            
//            // dataPoint.FirePropertyChanged(VcProperties.Opacity);
//        }
//#endif

        /// <summary>
        /// OpacityProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnOpacityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.Opacity, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.Opacity);
        }

        /// <summary>
        /// Modify YValue for Logarithmic scale
        /// </summary>
        /// <param name="yValue"></param>
        /// <returns></returns>
        internal static Double ConvertYValue2LogarithmicValue(Chart chart, Double yValue, AxisTypes axisType)
        {
            if (chart != null && chart.ChartArea != null)
            {
                if (chart.ChartArea.AxisY != null && chart.ChartArea.AxisY.AxisType == axisType)
                {
                    if (chart.ChartArea.AxisY.Logarithmic)
                    {
                        yValue = Math.Log(yValue, chart.ChartArea.AxisY.LogarithmBase);
                    }
                }
                else if (chart.ChartArea.AxisY2 != null && chart.ChartArea.AxisY2.AxisType == axisType)
                {
                    if (chart.ChartArea.AxisY2.Logarithmic)
                    {
                        yValue = Math.Log(yValue, chart.ChartArea.AxisY2.LogarithmBase);
                    }
                }
            }

            return yValue;
        }

        /// <summary>
        /// Converts Logarithmic YValue to Actual YValue
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="logValue"></param>
        /// <returns></returns>
        internal static Double ConvertLogarithmicValue2ActualValue(Chart chart, Double logValue, AxisTypes axisType)
        {
            if (chart != null && chart.ChartArea != null)
            {
                if (chart.ChartArea.AxisY != null && chart.ChartArea.AxisY.AxisType == axisType)
                {
                    if (chart.ChartArea.AxisY.Logarithmic)
                    {
                        logValue = Math.Pow(chart.ChartArea.AxisY.LogarithmBase, logValue);
                    }
                }
                else if (chart.ChartArea.AxisY2 != null && chart.ChartArea.AxisY2.AxisType == axisType)
                {
                    if (chart.ChartArea.AxisY2.Logarithmic)
                    {
                        logValue = Math.Pow(chart.ChartArea.AxisY2.LogarithmBase, logValue);
                    }
                }
            }

            return logValue;
        }     

        /// <summary>
        /// YValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnYValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            Double newYValue = (Double) e.NewValue;// dataPoint.ConvertYValue2LogarithmicValue((Double)e.NewValue);
            Double oldYValue = (Double) e.OldValue;// dataPoint.ConvertYValue2LogarithmicValue((Double)e.OldValue);
            
            if (dataPoint.Chart == null || (dataPoint.Chart as Chart).ChartArea == null)
                dataPoint._oldYValue = newYValue;
            else
            {
                dataPoint._oldYValue = Double.IsNaN(oldYValue) ? dataPoint._oldYValue : oldYValue;
                dataPoint._oldYValue = Double.IsNaN(dataPoint._oldYValue) ? 0 : dataPoint._oldYValue;

                if ((dataPoint.Chart as Chart).Series.Count == 1 &&
                    ((Boolean)dataPoint.Parent.ShowInLegend &&
                    (dataPoint.Parent.IncludeYValueInLegend || dataPoint.Parent.IncludePercentageInLegend)))
                {
                    dataPoint.FirePropertyChanged(VcProperties.YValue);
                }
                else
                {
                    if (!(dataPoint.Parent.RenderAs.Equals(RenderAs.CandleStick) || dataPoint.Parent.RenderAs.Equals(RenderAs.Stock)))
                        dataPoint.InvokeUpdateVisual(VcProperties.YValue, newYValue);
                }
            }
        }

        private static void OnYValuesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint._oldYValues = (Double[]) e.OldValue;

            if (dataPoint.Parent != null)
            {   
                if (dataPoint.Parent.RenderAs.Equals(RenderAs.CandleStick) || dataPoint.Parent.RenderAs.Equals(RenderAs.Stock))
                    dataPoint.InvokeUpdateVisual(VcProperties.YValues, e.NewValue);
            }
            else
                dataPoint.InvokeUpdateVisual(VcProperties.YValues, e.NewValue);
        }

        /// <summary>
        /// XValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnXValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;

            // Double / Int32 value entered in Managed Code
            //if (e.NewValue.GetType().Equals(typeof(Double)) || e.NewValue.GetType().Equals(typeof(Int32)))
            //{
            //    dataPoint.InternalXValue = Convert.ToDouble(e.NewValue);
            //    dataPoint.XValueType = ChartValueTypes.Numeric;
            //}

            if (e.NewValue == null)
                return;

            // DateTime value entered in Managed Code
            if ((e.NewValue.GetType().Equals(typeof(DateTime))))
            {
                dataPoint.InternalXValueAsDateTime = (DateTime)e.NewValue;
                dataPoint.ActualXValueAsDateTime  = (DateTime)e.NewValue;
                dataPoint.XValueType = ChartValueTypes.DateTime;
            }
            // Double / Int32 / DateTime entered in XAML
            else if ((e.NewValue.GetType().Equals(typeof(String))))
            {
                DateTime dateTimeresult;
                Double doubleResult;

                if (String.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    dataPoint.InternalXValue = Double.NaN;
                    dataPoint.XValueType = ChartValueTypes.Numeric;
                }
                // Double entered in XAML
                else if (Double.TryParse((string)e.NewValue, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowExponent, System.Globalization.CultureInfo.InvariantCulture, out doubleResult))
                {
                    dataPoint.InternalXValue = doubleResult;
                    dataPoint.XValueType = ChartValueTypes.Numeric;
                }
                // DateTime entered in XAML
                else if (DateTime.TryParse((string)e.NewValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTimeresult))
                {
                    dataPoint.InternalXValueAsDateTime = dateTimeresult;
                    dataPoint.ActualXValueAsDateTime = dateTimeresult;
                    dataPoint.XValueType = ChartValueTypes.DateTime;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Input for XValue");
                    throw new Exception("Invalid Input for XValue");
                }
            }
            else
            {
                try
                {
                    Object propertyValue = Convert.ChangeType(e.NewValue, typeof(System.Double), System.Globalization.CultureInfo.InvariantCulture);

                    dataPoint.InternalXValue = Convert.ToDouble(propertyValue);
                    dataPoint.XValueType = ChartValueTypes.Numeric;
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Invalid Input for XValue");
                    throw new Exception("Invalid Input for XValue");
                }
            }

            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Invalid Input for XValue");
            //    throw new Exception("Invalid Input for XValue");
            //}

            dataPoint.InvokeUpdateVisual(VcProperties.XValue, e.NewValue);

            // dataPoint.InvokeUpdateVisual("XValue", e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.XValue");
        }
        
        /// <summary>
        /// ZValueProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnZValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.ZValue, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.ZValue);
        }
        
        /// <summary>
        /// AxisXLabelProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnAxisXLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            // dataPoint.InvokeUpdateVisual(VcProperties.AxisXLabel, e.NewValue);
            dataPoint.FirePropertyChanged(VcProperties.AxisXLabel);
        }
        
        /// <summary>
        /// ColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.Color, e.NewValue);
        }

        /// <summary>
        /// StickColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnStickColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.StickColor, e.NewValue);
        }
        
        /// <summary>
        /// EnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.Enabled, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.Enabled);
        }
        
        /// <summary>
        /// ExplodedProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnExplodedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            DataPoint dataPoint = d as DataPoint;
            DataSeries dataSeries = dataPoint.Parent;

            if (dataSeries == null)
                return;

            Chart chart = dataSeries.Chart as Chart;

            if (dataPoint.IsNotificationEnable && chart != null && chart.ChartArea != null
                && chart.ChartArea.PlotDetails != null
                && chart.ChartArea.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
            {
                if (chart.ChartArea._isAnimationFired && (dataPoint.Parent.RenderAs == RenderAs.SectionFunnel || dataPoint.Parent.RenderAs == RenderAs.StreamLineFunnel || dataPoint.Parent.RenderAs == RenderAs.Pyramid))
                {
                    if (dataPoint.Parent.Exploded == false)
                        dataPoint.ExplodeOrUnexplodeAnimation();
                }
                else
                {
                    if (chart.InternalAnimationEnabled && !chart.ChartArea._isAnimationFired)
                        return;

                    dataPoint.ExplodeOrUnexplodeAnimation();
                }
            }
        }

        /// <summary>
        /// LightingEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLightingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LightingEnabled, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LightingEnabled);
        }

        /// <summary>
        /// ShadowEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.ShadowEnabled, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.ShadowEnabled);
        }
        
        /// <summary>
        /// LabelEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelEnabled, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelEnabled);
        }
        
        /// <summary>
        /// LabelTextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelText, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.LabelText);
        }

        /// <summary>
        /// LabelFontFamilyProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontFamilyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelFontFamily, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.LabelFontFamily);
        }
        
        /// <summary>
        /// LabelFontSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelFontSize, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelFontSize);
        }
        
        /// <summary>
        /// LabelFontColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelFontColor, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.LabelFontColor);
        }
        
        /// <summary>
        /// LabelFontWeightProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontWeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelFontWeight, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelFontWeight);
        }
        
        /// <summary>
        /// LabelFontStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelFontStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelFontStyle, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelFontStyle);
        }

        /// <summary>
        /// LabelBackgroundProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelBackground, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelBackground);
        }

        /// <summary>
        /// LabelStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelStyle, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelStyle);
        }

        /// <summary>
        /// LabelAngleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.FirePropertyChanged(VcProperties.LabelAngle);
        }

        /// <summary>
        /// LabelLineEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelLineEnabled, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelLineEnabled);
        }

        /// <summary>
        /// LabelLineColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelLineColor, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelLineColor);
        }

        /// <summary>
        /// LabelLineThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelLineThickness, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelLineThickness);
        }

        /// <summary>
        /// LabelLineStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLabelLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LabelLineStyle, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LabelLineStyle);
        }

        /// <summary>
        /// MarkerEnabledProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerEnabled, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.MarkerEnabled);
        }

        /// <summary>
        /// MarkerTypeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerType, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.MarkerType);
        }
        
        /// <summary>
        /// MarkerBorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerBorderThicknessPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerBorderThickness, e.NewValue);
            //dataPoint.UpdateMarker();
        }
        
        /// <summary>
        /// MarkerBorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerBorderColorPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerBorderColor, e.NewValue);
            //dataPoint.UpdateMarker();
        }
        
        /// <summary>
        /// MarkerSizeProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerSizePropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerSize, e.NewValue);
            //dataPoint.FirePropertyChanged(VcProperties.MarkerSize);
        }

        /// <summary>
        /// MarkerColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerColorPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerColor, e.NewValue);
            // dataPoint.UpdateMarker();
        }

        /// <summary>
        /// MarkerScaleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnMarkerScalePropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.MarkerScale, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.MarkerScale);
        }
        
        /// <summary>
        /// ShowInLegendProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnShowInLegendPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            //dataPoint.InvokeUpdateVisual(VcProperties.ShowInLegend, e.NewValue);
            dataPoint.FirePropertyChanged(VcProperties.ShowInLegend);
        }
        
        /// <summary>
        /// LegendTextProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnLegendTextPropertychanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.LegendText, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.LegendText);
        }
        
//#if WPF

//        /// <summary>
//        /// BorderThicknessProperty changed call back function
//        /// </summary>
//        /// <param name="d">DependencyObject</param>
//        /// <param name="e">DependencyPropertyChangedEventArgs</param>
//        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            DataPoint dataPoint = d as DataPoint;
//            dataPoint.InvokeUpdateVisual(VcProperties.BorderThickness, e.NewValue);
//            // dataPoint.FirePropertyChanged(VcProperties.BorderThickness);
//        }

//#endif

        /// <summary>
        /// BorderThicknessProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.BorderThickness, e.NewValue);
            dataPoint.FirePropertyChanged(VcProperties.BorderThickness);
        }

        /// <summary>
        /// BorderColorProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.BorderColor, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.BorderColor);
        }
        
        /// <summary>
        /// BorderStyleProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.BorderStyle, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.BorderStyle);
        }

        /// <summary>
        /// RadiusXProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRadiusXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.RadiusX, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.RadiusX);
        }
        
        /// <summary>
        /// RadiusYProperty changed call back function
        /// </summary>
        /// <param name="d">DependencyObject</param>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        private static void OnRadiusYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPoint dataPoint = d as DataPoint;
            dataPoint.InvokeUpdateVisual(VcProperties.RadiusY, e.NewValue);
            // dataPoint.FirePropertyChanged(VcProperties.RadiusY);
        }

        internal void DeSelectOthers()
        {
            if (Parent != null)
            {
                foreach (DataPoint dp in Parent.DataPoints)
                {
                    if (dp != this)
                        dp.DeSelect(dp, false, true);
                }
            }
        }

        /// <summary>
        /// Select a DataPoint visually
        /// </summary>
        /// <param name="selfDeselect">Whether to allow any property change</param>
        internal void Select(Boolean allowPropertyChange)
        {
            if (Faces != null)
            {
                if(allowPropertyChange)
                    UpdateExplodedPropertyForSelection(true, true);

                foreach (Shape shape in Faces.BorderElements)
                {   
                    BorderStyles borderStyle = BorderStyles.Dashed;
                    Double borderThickness = 1.5;

                    if(Parent != null)
                    {   
                        if((Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut) && (Chart as Chart).View3D)
                        {   
                            borderStyle = BorderStyles.Dotted;
                        }

                        if ((Parent.RenderAs == RenderAs.Column || Parent.RenderAs == RenderAs.Bar) && !(Chart as Chart).View3D)
                        {
                            borderThickness = 1.5;
                        }
                    }
                    
                    Brush borderColor;

                    if (Parent.RenderAs == RenderAs.Stock)
                    {
                        borderColor = StockChart.ReCalculateAndApplyTheNewBrush(shape, Color, (Boolean)LightingEnabled);
                    }
                    else
                        borderColor = Visifire.Charts.Chart.CalculateDataPointLabelFontColor(Chart as Chart, this, null, LabelStyles.OutSide);

                    InteractivityHelper.ApplyBorderEffect(shape, borderStyle, borderThickness, borderColor);            
                }
            }

            if (Parent != null && Marker != null && (RenderHelper.IsLineCType(Parent) || RenderHelper.IsAreaCType(Parent)))
            {               
                if (allowPropertyChange)
                    UpdateExplodedPropertyForSelection(true, true);

                if (InteractivityHelper.SELECTED_MARKER_BORDER_COLOR == null)
                    InteractivityHelper.SELECTED_MARKER_BORDER_COLOR = Graphics.RED_BRUSH;

                if (InteractivityHelper.SELECTED_MARKER_FILL_COLOR == null)
                    InteractivityHelper.SELECTED_MARKER_FILL_COLOR = Graphics.ORANGE_BRUSH;

                if ((!Double.IsNaN(_originalMarkerSize.Width) && !Double.IsNaN(_originalMarkerSize.Height)) && new Size(Marker.MarkerShape.Width, Marker.MarkerShape.Height) != _originalMarkerSize)
                {
                    Marker.MarkerShape.Width = _originalMarkerSize.Width;
                    Marker.MarkerShape.Height = _originalMarkerSize.Height;
                }

                _originalMarkerSize = new Size(Marker.MarkerShape.Width, Marker.MarkerShape.Height);

                InteractivityHelper.ApplyBorderEffect(Marker.MarkerShape, BorderStyles.Solid, InteractivityHelper.SELECTED_MARKER_BORDER_COLOR, 1.2, 2.4, InteractivityHelper.SELECTED_MARKER_FILL_COLOR);
                Marker.MarkerShape.Margin = new Thickness(- 1.2, -1.2,0,0);

                if (RenderHelper.IsLineCType(Parent))
                    LineChart.SelectMovingMarker(this);
            }
        }

        /// <summary>
        /// Deselect a DataPoint visually
        /// </summary>
        /// <param name="dataPoint">DataPoint</param>
        /// <param name="selfDeselect">Whether the action taken by the selected or deselected DataPoint to update its own Exploded property</param>
        /// <param name="selfDeselect">Whether to allow any property change</param>
        internal void DeSelect(DataPoint dataPoint, Boolean selfDeSelect, Boolean allowPropertyChange)
        {
            if (Faces != null)
            {
                if (allowPropertyChange)
                   UpdateExplodedPropertyForSelection(false, selfDeSelect);

                foreach (Shape shape in Faces.BorderElements)
                {
                    if (dataPoint.Parent != null && (dataPoint.Parent.RenderAs == RenderAs.Pie || dataPoint.Parent.RenderAs == RenderAs.Doughnut))
                        InteractivityHelper.RemoveBorderEffect(shape, (BorderStyles)dataPoint.BorderStyle, ((Thickness)InternalBorderThickness).Left, BorderColor);
                    else
                    {
                        Brush borderColor;

                        if (Parent.RenderAs == RenderAs.Stock)
                            borderColor = StockChart.ReCalculateAndApplyTheNewBrush(shape, dataPoint.Color, (Boolean) dataPoint.LightingEnabled);
                        else
                            borderColor = BorderColor;

                        InteractivityHelper.RemoveBorderEffect(shape, (BorderStyles)dataPoint.BorderStyle, ((Thickness)InternalBorderThickness).Left, borderColor);
                    }
                    
                    if (allowPropertyChange)
                    {   
                        IsNotificationEnable = false;
                        Selected = false;
                        IsNotificationEnable = true;
                    }
                }
            }

            if (Parent != null && Marker != null && (RenderHelper.IsLineCType(Parent) || RenderHelper.IsAreaCType(Parent)))
            {
                if (allowPropertyChange)
                    UpdateExplodedPropertyForSelection(false, selfDeSelect);

                if (Marker != null)
                {
                    if ((Boolean)MarkerEnabled)
                    {
                        InteractivityHelper.RemoveBorderEffect(Marker.MarkerShape, (BorderStyles)dataPoint.BorderStyle, Marker.BorderThickness, Marker.BorderColor, Marker.MarkerFillColor, Marker.MarkerSize.Width * Marker.ScaleFactor, Marker.MarkerSize.Height * Marker.ScaleFactor);
                        Marker.MarkerShape.Margin = new Thickness(0, 0, 0, 0);
                    }
                    else
                    {
                        InteractivityHelper.RemoveBorderEffect(Marker.MarkerShape, (BorderStyles)dataPoint.BorderStyle, Marker.BorderThickness, Marker.BorderColor, Marker.MarkerFillColor, Marker.MarkerSize.Width * Marker.ScaleFactor, Marker.MarkerSize.Height * Marker.ScaleFactor);
                        Marker.MarkerShape.Margin = new Thickness(0, 0, 0, 0);
                        Marker.MarkerShape.Fill = new SolidColorBrush(Colors.Transparent);
                    }
                }

                if (allowPropertyChange)
                {
                    IsNotificationEnable = false;
                    Selected = false;
                    IsNotificationEnable = true;
                }
            }
        }

        /// <summary>
        /// Update exploded property for selection
        /// </summary>
        /// <param name="exploded">Exploded</param>
        /// <param name="selfAction">Whether the action taken by the selected or deselected DataPoint to update its own Exploded property</param>
        private void UpdateExplodedPropertyForSelection(Boolean exploded, Boolean selfAction)
        {   
            // if (Parent != null && Parent.Chart != null && (Parent.Chart as Chart).ChartArea != null && (Parent.Chart as Chart).ChartArea.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
            {
                if (Parent.RenderAs == RenderAs.SectionFunnel || Parent.RenderAs == RenderAs.StreamLineFunnel || Parent.RenderAs == RenderAs.Pyramid)
                {
                    if (Exploded != exploded)
                    {
                        // For self action we need to update Exploded property 
                        if (selfAction) 
                            Exploded = exploded;
                        else // If it is not a self action we need to update Exploded property with out firing any event 
                        {
                            IsNotificationEnable = false;
                            Exploded = exploded;
                            IsNotificationEnable = true;
                        }                           
                   }
                }
                else
                {
                    if (Exploded != exploded)
                    {
                        Exploded = exploded;
                    }
                }
            }
        }

        /// <summary>
        /// Returns cursor type of the DataPoint
        /// </summary>
        /// <returns></returns>
        private Cursor GetCursor()
        {
            if (this.Cursor == null)
            {
                if (_parent != null && _parent.Cursor == null)
                    return Cursors.Arrow;
                else
                    return _parent.Cursor;
            }
            else
            {
                return this.Cursor;
            }
        }

        /// <summary>
        /// MouseLeftButtonUp event handler for handling interactive animation
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">MouseButtonEventArgs</param>
        private void Visual_ExplodeUnExplode(object sender, MouseButtonEventArgs e)
        {
            InteractiveAnimation(false, true);
        }

        /// <summary>
        /// Event handler attached with Completed event of the storyboard for explode animation
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">EventArgs</param>
        private void ExplodeAnimation_Completed(object sender, EventArgs e)
        {
            _interactiveExplodeState = true;
            _interativityAnimationState = false;
            Chart._rootElement.IsHitTestVisible = true;

            if (Parent.RenderAs == RenderAs.SectionFunnel || Parent.RenderAs == RenderAs.StreamLineFunnel || Parent.RenderAs == RenderAs.Pyramid)
            {
                TriangularChartSliceParms[] funnelSlices = Parent.VisualParams as TriangularChartSliceParms[];

                if (funnelSlices != null && this.Faces != null && Faces.Visual != null)
                {   
                    Double height = (Faces.Visual.Parent as FrameworkElement).Height;

                    //Boolean allUnExploaded = true;

                    //foreach (DataPoint dp in Parent.DataPoints)
                    //{
                    //    if (dp.Exploded == true)
                    //    {
                    //        allUnExploaded = false;
                    //        break;
                    //    }
                    //}

                    //if(allUnExploaded == true)
                        FunnelChart.ArrangeLabels(funnelSlices, Double.NaN, height);
                    //else
                       // FunnelChart.ArrangeLabelsOnExpload(funnelSlices, this, Double.NaN, height);
                }
            }
        }

        /// </summary>
        /// Event handler attached with Completed event of the storyboard for unexplode animation
        /// </summary>
        /// <param name="sender">FrameworkElement</param>
        /// <param name="e">EventArgs</param>
        private void UnExplodeAnimation_Completed(object sender, EventArgs e)
        {
            _interactiveExplodeState = false;
            _interativityAnimationState = false;
        }

        /// <summary>
        /// Returns label text for AxisX
        /// </summary>
        /// <returns>String</returns>
        private String GetAxisXLabelString()
        {
            String labelString = "";
            Chart chart = Chart as Chart;

            if(chart.PlotDetails.ChartOrientation == ChartOrientationType.NoAxis)
            {
                if(String.IsNullOrEmpty(this.AxisXLabel))
                {
                    if (Parent.PlotGroup.AxisX.IsDateTimeAxis)
                        labelString = AxisLabels.FormatDate(this.InternalXValueAsDateTime, Parent.PlotGroup.AxisX);
                    else
                        labelString = Parent.PlotGroup.AxisX.GetFormattedString(InternalXValue);
                }
                else
                    labelString = this.AxisXLabel;
            }
            else
            {   
                if (Parent.PlotGroup != null && Parent.PlotGroup.AxisX != null && Parent.PlotGroup.AxisX.AxisLabels != null)
                {
                    if (Parent.PlotGroup.AxisX.AxisLabels.AxisLabelContentDictionary != null &&
                    Parent.PlotGroup.AxisX.AxisLabels.AxisLabelContentDictionary.ContainsKey(InternalXValue))
                    {
                        labelString = Parent.PlotGroup.AxisX.AxisLabels.AxisLabelContentDictionary[InternalXValue];
                    }
                    else
                    {
                        if (Parent.RenderAs == RenderAs.Radar)
                            labelString = Parent.PlotGroup.AxisX.GetFormattedString(InternalXValue + 1);
                        else
                        {
                            if (Parent.PlotGroup.AxisX.IsDateTimeAxis)
                                labelString = AxisLabels.FormatDate(this.InternalXValueAsDateTime, Parent.PlotGroup.AxisX);
                            else
                                labelString = Parent.PlotGroup.AxisX.GetFormattedString(InternalXValue);
                        }
                    }
                }
                else
                    labelString = this.AxisXLabel;
            }

            return labelString;
        }

        /// <summary>
        /// Explode or unexplode animation for Pie and Doughnut
        /// </summary>
        internal void ExplodeOrUnexplodeAnimation()
        {
            if ((Boolean)Exploded)
            {
                if (this.UnExplodeAnimation != null)
                    this.UnExplodeAnimation.Stop();

                if (this.ExplodeAnimation != null)
                {
                    try
                    {
                        _isAlreadyExploded = true;
#if WPF             
                        this.ExplodeAnimation.Begin(Chart._rootElement, true);
#else
                        this.ExplodeAnimation.Begin();

#endif
                        if (!(Chart as Chart).ChartArea._isFirstTimeRender && Parent != null && (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut))
                            if (!(Boolean)(Chart as Chart).AnimatedUpdate)
                            {
#if WPF
                                this.ExplodeAnimation.SkipToFill(Chart._rootElement);
#else
                                this.ExplodeAnimation.SkipToFill();
#endif
                            }
                    }
                    catch
                    {
                        _isAlreadyExploded = false;
                    }
                }
                else
                {
                    PieDouExplodeWithoutAnimation();
                    _isAlreadyExploded = true;
                }

            }
            else if (_isAlreadyExploded == true)
            {
                UnExplodeFunnelSlices();

                if (this.ExplodeAnimation != null)
                    this.ExplodeAnimation.Stop();

                if (this.UnExplodeAnimation != null)
                {
                    _isAlreadyExploded = false;
#if WPF                 
                        this.UnExplodeAnimation.Begin(Chart._rootElement, true);
#else
                    this.UnExplodeAnimation.Begin();
#endif
                }
                else
                    PieDouUnExplodeWithoutAnimation();

                _isExplodeRunning = false;
            }

            if (!_isExplodeRunning && (Boolean)Exploded)
            {
                if (!(Chart as Chart).ChartArea._isFirstTimeRender && Parent != null && !(Chart as Chart).ChartArea._isDefaultInteractivityAllowed && (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut))
                {
                    if (ExplodeAnimation != null)
                    {
#if WPF                 
                        this.ExplodeAnimation.Begin(Chart._rootElement, true);
#else
                        this.ExplodeAnimation.Begin();
#endif
                        _isExplodeRunning = true;
                    }
                    else
                    {
                        PieDouExplodeWithoutAnimation();
                        _isExplodeRunning = false;
                    }
                }
            }
        }

        private void UnExplodeFunnelSlices()
        {
            if (Parent.RenderAs == RenderAs.SectionFunnel || Parent.RenderAs == RenderAs.StreamLineFunnel || Parent.RenderAs == RenderAs.Pyramid)
            {
                if (this.UnExplodeAnimation != null)
                {
                    this.UnExplodeAnimation.Completed -= ExplodeAnimation_Completed;
                    this.UnExplodeAnimation = null;
                }

                this.UnExplodeAnimation = new Storyboard();

                foreach (TriangularChartSliceParms funnleSlice in (Parent.VisualParams as TriangularChartSliceParms[]))
                {
                    this.UnExplodeAnimation = FunnelChart.CreateUnExplodingAnimation(Parent, this, this.UnExplodeAnimation, funnleSlice.DataPoint.Faces.Visual as Panel, funnleSlice.Top);
                }

                this.UnExplodeAnimation.Completed += ExplodeAnimation_Completed;
            }
        }

        /// <summary>
        /// Calculate percentage value among InternalDataPoints
        /// </summary>
        /// <returns>Double</returns>
        private Double Percentage()
        {   
            Double percentage = 0;

            if (Parent.RenderAs == RenderAs.Column || Parent.RenderAs == RenderAs.Bar
                || Parent.RenderAs == RenderAs.Area || RenderHelper.IsLineCType(Parent) || Parent.RenderAs == RenderAs.SectionFunnel || Parent.RenderAs == RenderAs.Pyramid
                || Parent.RenderAs == RenderAs.Point || Parent.RenderAs == RenderAs.Bubble
                || Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut)
            {   
                if ((Parent.Chart as Chart).PlotDetails != null)
                {
                    Double sum = (Parent.Chart as Chart).PlotDetails.GetAbsoluteSumOfDataPoints(Parent.InternalDataPoints.ToList());
                    if (sum > 0) percentage = ((YValue / sum) * 100);
                    else percentage = 0;
                }
            }
            else if (Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.Stock)
            {
                if ((Parent.Chart as Chart).PlotDetails != null)
                {
                    Double sum = (Parent.Chart as Chart).PlotDetails.GetAbsoluteSumOfDataPoints(Parent.InternalDataPoints.ToList());
                    if (sum > 0)
                    {
                        if (YValues != null && YValues.Length > 1)
                            percentage = ((YValues[1] / sum) * 100);
                        else
                            percentage = 0;
                    }
                    else
                        percentage = 0;
                }
            }
            else if (Parent.RenderAs == RenderAs.StreamLineFunnel)
            {
                percentage = ((YValue / Parent.PlotGroup.MaximumY) * 100);
            }
            else if (Parent.RenderAs == RenderAs.StackedArea || Parent.RenderAs == RenderAs.StackedBar
                || Parent.RenderAs == RenderAs.StackedColumn || Parent.RenderAs == RenderAs.StackedArea100
                || Parent.RenderAs == RenderAs.StackedBar100 || Parent.RenderAs == RenderAs.StackedColumn100)
            {
                percentage = YValue / Parent.PlotGroup.XWiseStackedDataList[InternalXValue].AbsoluteYValueSum * 100;// _stackSum[XValue].Y Contains Absolute sum
            }

            return percentage;
        }

        /// <summary>
        /// Update marker and legend for dataPoint
        /// </summary>
        /// <param name="Value">Color value</param>
        internal static void UpdateMarkerAndLegend(DataPoint dataPoint, object colorValue)
        {   
            Marker marker = dataPoint.Marker;

            if (marker != null && marker.Visual != null && (Boolean)dataPoint.MarkerEnabled)
            {   
                if (dataPoint.Parent.RenderAs == RenderAs.Point)
                {
                    marker.MarkerFillColor = dataPoint.Color; // (Brush)colorValue;

                    if (marker.MarkerType != MarkerTypes.Cross)
                    {
                        if (dataPoint.BorderColor != null)
                            marker.BorderColor = dataPoint.BorderColor;
                    }
                    else
                        marker.BorderColor = dataPoint.Color; // (Brush)colorValue;(Brush)colorValue;

                    // Marker.UpdateMarker();
                }
                else
                    marker.BorderColor = (dataPoint.GetValue(DataPoint.MarkerBorderColorProperty) as Brush == null) ? ((colorValue != null) ? colorValue as Brush : dataPoint.MarkerBorderColor) : dataPoint.MarkerBorderColor; // (Brush)colorValue;

                if (!dataPoint.Selected)
                    marker.UpdateMarker();
            }

            UpdateLegendMarker(dataPoint, colorValue as Brush);
        }

        internal static void UpdateLegendMarker(DataPoint dataPoint, Brush colorValue)
        {   
            Brush newValue = (colorValue != null) ? colorValue : dataPoint._internalColor;

            // Marker displaied in Marker
            Marker marker = dataPoint.LegendMarker;

            if (marker != null && marker.Visual != null)
            {
                RenderAs renderAs = dataPoint.Parent.RenderAs;

                switch (renderAs)
                {
                    case RenderAs.Line:
                    case RenderAs.StepLine:
                    case RenderAs.Spline:
                    case RenderAs.QuickLine:
                    case RenderAs.CandleStick:
                    case RenderAs.Stock:

                        marker.BorderColor = Graphics.GetLightingEnabledBrush((Brush)newValue, "Linear", new Double[] { 0.65, 0.55 });

                        if ((marker.Visual as Grid).Parent != null && (((marker.Visual as Grid).Parent as Canvas).Children[0] as Line) != null)
                            (((marker.Visual as Grid).Parent as Canvas).Children[0] as Line).Stroke = Graphics.GetLightingEnabledBrush((Brush)newValue, "Linear", new Double[] { 0.65, 0.55 }); ;

                        break;

                    default:
                        marker.BorderColor = (Brush)newValue;
                        marker.MarkerFillColor = (Brush)newValue;
                        break;
                }

                marker.UpdateMarker();
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Attach event-handler with MouseLeftButtonUp event for selection of DataPoint.
        /// This is used for selection of DataPoint.
        /// </summary>
        internal void AttachEventForSelection()
        {   
            if (Parent != null && Parent.SelectionEnabled)
            {
                IsNotificationEnable = false;
                MouseLeftButtonUp -= dataPoint_selectOrDeselect;
                AttachEventWithHighPriority("MouseLeftButtonUp", new MouseButtonEventHandler(dataPoint_selectOrDeselect));
                IsNotificationEnable = true;
            }
        }
        
        /// <summary>
        /// Detach event-handler with MouseLeftButtonUp event.
        /// This is used for selection of DataPoint.
        /// </summary>
        internal void DetachEventForSelection()
        {
            IsNotificationEnable = false;
            MouseLeftButtonUp -= dataPoint_selectOrDeselect;
            IsNotificationEnable = true;
        }

        /// <summary>
        /// Event handler for DataSeries event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void dataPoint_selectOrDeselect(object sender, MouseButtonEventArgs e)
        {   
            DataPoint dp = sender as DataPoint;
            dp.Selected = (dp.Parent != null && dp.Parent.SelectionEnabled) ? !dp.Selected : false;
        }
        
        /// <summary>
        /// Binds to an Object
        /// </summary>
        internal void BindData(Object dataSource, DataMappingCollection dataMappings)
        {   
            foreach (DataMapping dm in dataMappings)
            {   
                UpdateBindProperty(dm, dataSource);  
            }

            INotifyPropertyChanged iNotifyPropertyChanged = dataSource as INotifyPropertyChanged;

            if (iNotifyPropertyChanged != null)
            {
                // Use a WeakEventListener so that the backwards reference doesn't keep this object alive
                _weakEventListener = new WeakEventListener<DataPoint, object, PropertyChangedEventArgs>(this);

                _weakEventListener.OnEventAction = (instance, source, eventArgs) =>
                {
                    instance.iNotifyPropertyChanged_PropertyChanged(source, eventArgs);
                };

                _weakEventListener.OnDetachAction = (weakEventListener) => iNotifyPropertyChanged.PropertyChanged -= weakEventListener.OnEvent;

                iNotifyPropertyChanged.PropertyChanged += _weakEventListener.OnEvent;
            }
        }

        internal void DetachEventFromWeakEventListner()
        {   
            // Detach the WeakEventListener
            if (null != _weakEventListener)
            {   
                _weakEventListener.Detach();
                _weakEventListener = null;
            }
        }

        /// <summary>
        /// WeakEventListener used to handle INotifyCollectionChanged events.
        /// </summary>
        internal WeakEventListener<DataPoint, object, PropertyChangedEventArgs> _weakEventListener;

        /// <summary>
        /// Update value of the property from DataSource
        /// </summary>
        /// <param name="dm">DataMapping</param>
        /// <param name="dataSource">DataSource</param>
        internal void UpdateBindProperty(DataMapping dm, Object dataSource)
        {   
            switch (dm.MemberName)
            {
                case "Open":
                case "Close":
                case "High":
                case "Low":

                    Double[] newYValues = new Double[4];
                    if (YValues != null)
                        YValues.CopyTo(newYValues, 0);

                    // find the array index of OCHL (open close high low)
                    Int32 enumIndex = (Int32)Enum.Parse(typeof(OCHL), dm.MemberName, true);

                    newYValues[enumIndex] = (Double)dm.GetPropertyValue(dataSource);
                    YValues = newYValues;
                    break;

                default:

                    dm.Map(dataSource, this);
                    break;
            }
        }

        /// <summary>
        /// Reset value of the property that was mapped
        /// </summary>
        /// <param name="dm">DataMapping</param>
        /// <param name="dataSource">DataSource</param>
        internal void ResetBindProperty(DataMapping dm, Object dataSource)
        {   
            Double[] newYValues = new Double[4];

            if (YValues != null)
                YValues.CopyTo(newYValues, 0);

            switch (dm.MemberName)
            {   
                case "Open":
                case "Close":
                case "High":
                case "Low":

                    if (YValues == null)
                    {   
                        Int32 enumIndex = (Int32)Enum.Parse(typeof(OCHL), dm.MemberName, true);
                        YValues[enumIndex] = 0;
                    }

                    break;

                default:
                    this.GetType().GetProperty(dm.MemberName).SetValue(this, null, null);
                    break;
            }
        }

        internal override void ClearInstanceRefs()
        {
            if (Storyboard != null)
            {
                Storyboard.FillBehavior = FillBehavior.Stop;
                Storyboard.Stop();
                Storyboard = null;
            }
        }

        private void iNotifyPropertyChanged_PropertyChanged(object dataSource, PropertyChangedEventArgs e)
        {
            DataMapping dm;

            try
            {
                dm = (from item in Parent.DataMappings where (item.Path == e.PropertyName) select item).Single();
            }
            catch
            {
                return;
            }

            UpdateBindProperty(dm, dataSource);
        }

        /// <summary>
        /// Start interactive animation
        /// </summary>
        internal void InteractiveAnimation(Boolean isFirstTimeAnimation, Boolean allowToChangePropertyValue)
        {   
            // If interactivity animation is not running already and the slice is not exploded
            // then explode the slice
            if (Parent.RenderAs == RenderAs.SectionFunnel || Parent.RenderAs == RenderAs.StreamLineFunnel || Parent.RenderAs == RenderAs.Pyramid)
            {
                if (!Parent.Exploded)
                {
                    if (isFirstTimeAnimation)
                    {
                        if (Exploded == true)
                            ExplodeOrUnexplodeAnimation();
                    }
                    else
                    {
                        if (!Parent.SelectionEnabled)
                        {
                            foreach (DataPoint dp in Parent.InternalDataPoints)
                            {
                                if (dp != this)
                                {
                                    dp.IsNotificationEnable = false;
                                    dp.Exploded = false;
                                    dp._isAlreadyExploded = false;
                                    dp.IsNotificationEnable = true;
                                }
                            }

                            Exploded = !Exploded;
                        }
                    }
                }
            }
            else
            {
                if ((Boolean)(Chart as Chart).AnimatedUpdate)
                {
                    if (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut)
                    {
                        if (!_interativityAnimationState)
                        {
                            if (false == _interactiveExplodeState)
                            {
                                _interativityAnimationState = true;


                                //System.Diagnostics.Debug.WriteLine("Intractivity-- Exploded");

                                if (allowToChangePropertyValue)
                                {
                                    IsNotificationEnable = false;
                                    Exploded = true;
                                    IsNotificationEnable = true;
                                }

                                if (this.ExplodeAnimation != null)
                                {
#if WPF
                                this.ExplodeAnimation.Begin(Chart._rootElement, true);
#else
                                    this.ExplodeAnimation.Begin();
#endif

                                    _isAlreadyExploded = true;
                                }
                            }

                            if (true == _interactiveExplodeState)
                            {
                                _interativityAnimationState = true;

                                if (allowToChangePropertyValue)
                                {
                                    IsNotificationEnable = false;
                                    Exploded = false;
                                    IsNotificationEnable = true;
                                }

                                UnExplodeFunnelSlices();

                                //System.Diagnostics.Debug.WriteLine("Intractivity-- UnExploded");

                                if (this.UnExplodeAnimation != null)
                                {
#if WPF                 
                                this.UnExplodeAnimation.Begin(Chart._rootElement, true);
#else
                                    this.UnExplodeAnimation.Begin();
#endif
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Parent.RenderAs != RenderAs.SectionFunnel && Parent.RenderAs != RenderAs.StreamLineFunnel && Parent.RenderAs != RenderAs.Pyramid)
                    {
                        PieExplodeOrUnExplodeWithoutAnimation();
                    }
                }
            }

        }

        internal void PieDouExplodeWithoutAnimation()
        {
            if(Parent != null && (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut))
            {
                if (!(Chart as Chart).View3D)
                {
                    (Faces.Visual.RenderTransform as TranslateTransform).X = (VisualParams as SectorChartShapeParams).OffsetX;
                    (Faces.Visual.RenderTransform as TranslateTransform).Y = (VisualParams as SectorChartShapeParams).OffsetY;

                    if (LabelVisual != null)
                    {
                        if (LabelStyle == LabelStyles.Inside)
                        {
                            Double left = (Double)(LabelVisual as Canvas).GetValue(Canvas.LeftProperty);
                            if (Chart != null && (Chart as Chart).ChartArea != null)
                            {
                                if (left + (VisualParams as SectorChartShapeParams).OffsetX < 0 || left + (LabelVisual as Canvas).Width + (VisualParams as SectorChartShapeParams).OffsetX > (Chart as Chart).ChartArea.ChartVisualCanvas.Width)
                                    return;
                            }

                            TranslateTransform translateTransform = new TranslateTransform();
                            (LabelVisual as Canvas).RenderTransform = translateTransform;

                            translateTransform.X = (VisualParams as SectorChartShapeParams).OffsetX;
                            translateTransform.Y = (VisualParams as SectorChartShapeParams).OffsetY;
                        }
                        else
                        {
                            (LabelVisual as Canvas).SetValue(Canvas.LeftProperty, (VisualParams as SectorChartShapeParams).ExplodedPoints.LabelPosition.X);
                        }
                    }

                    if (LabelLine != null)
                    {
                        PathFigure figure = (LabelLine.Data as PathGeometry).Figures[0];
                        PathSegmentCollection segments = figure.Segments;
                        (segments[0] as LineSegment).Point = (VisualParams as SectorChartShapeParams).ExplodedPoints.LabelLineMidPoint;
                        (segments[1] as LineSegment).Point = (VisualParams as SectorChartShapeParams).ExplodedPoints.LabelLineEndPoint;
                    }

                    _interactiveExplodeState = true;
                }
                else
                {
                    foreach (Shape path in Faces.VisualComponents)
                    {
                        if (path == null) continue;
                        (path.RenderTransform as TranslateTransform).X = (VisualParams as SectorChartShapeParams).OffsetX;
                        (path.RenderTransform as TranslateTransform).Y = (VisualParams as SectorChartShapeParams).OffsetY;
                    }

                    if (LabelVisual != null)
                    {
                        if (LabelStyle == LabelStyles.Inside)
                        {
                            Double left = (Double)(LabelVisual as Canvas).GetValue(Canvas.LeftProperty);
                            if (Chart != null && (Chart as Chart).ChartArea != null)
                            {
                                if (left + (VisualParams as SectorChartShapeParams).OffsetX < 0 || left + (LabelVisual as Canvas).Width + (VisualParams as SectorChartShapeParams).OffsetX > (Chart as Chart).ChartArea.ChartVisualCanvas.Width)
                                    return;
                            }

                            TranslateTransform translateTransform = new TranslateTransform();
                            (LabelVisual as Canvas).RenderTransform = translateTransform;

                            translateTransform.X = (VisualParams as SectorChartShapeParams).OffsetX;
                            translateTransform.Y = (VisualParams as SectorChartShapeParams).OffsetY;
                        }
                        else
                        {
                            (LabelVisual as Canvas).SetValue(Canvas.LeftProperty, (VisualParams as SectorChartShapeParams).ExplodedPoints.LabelPosition.X);
                        }
                    }

                    if (LabelLine != null)
                    {
                        (LabelLine.RenderTransform as TranslateTransform).X = (VisualParams as SectorChartShapeParams).OffsetX;
                        (LabelLine.RenderTransform as TranslateTransform).Y = (VisualParams as SectorChartShapeParams).OffsetY;

                        PathFigure figure = (LabelLine.Data as PathGeometry).Figures[0];
                        PathSegmentCollection segments = figure.Segments;
                        (segments[0] as LineSegment).Point = (VisualParams as SectorChartShapeParams).ExplodedPoints.LabelLineMidPoint;
                        (segments[1] as LineSegment).Point = (VisualParams as SectorChartShapeParams).ExplodedPoints.LabelLineEndPoint;
                    }

                    _interactiveExplodeState = true;
                }

                _isAlreadyExploded = true;
            }
        }

        internal void PieDouUnExplodeWithoutAnimation()
        {
            if (Parent != null && (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut))
            {
                if (!(Chart as Chart).View3D)
                {
                    (Faces.Visual.RenderTransform as TranslateTransform).X = 0;
                    (Faces.Visual.RenderTransform as TranslateTransform).Y = 0;

                    if (LabelVisual != null)
                    {
                        if (LabelStyle == LabelStyles.Inside)
                        {
                            TranslateTransform translateTransform = new TranslateTransform();
                            (LabelVisual as Canvas).RenderTransform = translateTransform;

                            translateTransform.X = 0;
                            translateTransform.Y = 0;
                        }
                        else
                        {
                            (LabelVisual as Canvas).SetValue(Canvas.LeftProperty, (VisualParams as SectorChartShapeParams).UnExplodedPoints.LabelPosition.X);
                        }
                    }

                    if (LabelLine != null)
                    {
                        PathFigure figure = (LabelLine.Data as PathGeometry).Figures[0];
                        PathSegmentCollection segments = figure.Segments;
                        (segments[0] as LineSegment).Point = (VisualParams as SectorChartShapeParams).UnExplodedPoints.LabelLineMidPoint;
                        (segments[1] as LineSegment).Point = (VisualParams as SectorChartShapeParams).UnExplodedPoints.LabelLineEndPoint;
                    }


                    _interactiveExplodeState = false;
                }
                else
                {
                    foreach (Shape path in Faces.VisualComponents)
                    {
                        if (path == null) continue;
                        (path.RenderTransform as TranslateTransform).X = 0;
                        (path.RenderTransform as TranslateTransform).Y = 0;
                    }

                    if (LabelVisual != null)
                    {
                        if (LabelStyle == LabelStyles.Inside)
                        {
                            TranslateTransform translateTransform = new TranslateTransform();
                            (LabelVisual as Canvas).RenderTransform = translateTransform;

                            translateTransform.X = 0;
                            translateTransform.Y = 0;
                        }
                        else
                        {
                            (LabelVisual as Canvas).SetValue(Canvas.LeftProperty, (VisualParams as SectorChartShapeParams).UnExplodedPoints.LabelPosition.X);
                        }
                    }

                    if (LabelLine != null)
                    {
                        (LabelLine.RenderTransform as TranslateTransform).X = 0;
                        (LabelLine.RenderTransform as TranslateTransform).Y = 0;

                        PathFigure figure = (LabelLine.Data as PathGeometry).Figures[0];
                        PathSegmentCollection segments = figure.Segments;
                        (segments[0] as LineSegment).Point = (VisualParams as SectorChartShapeParams).UnExplodedPoints.LabelLineMidPoint;
                        (segments[1] as LineSegment).Point = (VisualParams as SectorChartShapeParams).UnExplodedPoints.LabelLineEndPoint;
                    }

                    _interactiveExplodeState = false;
                }

                _isAlreadyExploded = false;
            }
        }

        internal void PieExplodeOrUnExplodeWithoutAnimation()
        {   
            IsNotificationEnable = false;
            Exploded = !Exploded;
            IsNotificationEnable = true;

            if (Faces != null && Faces.Visual != null)
            {
                if ((Boolean) Exploded)
                {   
                    PieDouExplodeWithoutAnimation();
                }
                else
                {   
                    PieDouUnExplodeWithoutAnimation();
                }
            }
        }

        /// <summary>
        /// Attach events to each and every individual face of Faces
        /// </summary>
        /// <param name="Object">Object where events to be attached</param>
        internal void AttachEvent2DataPointVisualFaces(ObservableObject Object)
        {
            if (RenderHelper.IsAxisIndependentCType(Parent))
            {   
                if (Faces != null)
                {   
                    if ((Parent.Chart as Chart).View3D)
                    {   
                        foreach (FrameworkElement element in Faces.VisualComponents)
                        {
                            AttachEvents2Visual(Object, this, element);

                            if ((Chart as Chart).ChartArea != null && (Chart as Chart).ChartArea._isDefaultInteractivityAllowed)
                            {
                                element.MouseLeftButtonUp -= new MouseButtonEventHandler(Visual_ExplodeUnExplode);
                                element.MouseLeftButtonUp += new MouseButtonEventHandler(Visual_ExplodeUnExplode);
                            }
                        }
                    }
                    else
                    {
                        if (Faces.Visual != null)
                        {
                            AttachEvents2Visual(Object, this, Faces.Visual);

                            if ((Chart as Chart).ChartArea != null && (Chart as Chart).ChartArea._isDefaultInteractivityAllowed)
                            {
                                Faces.Visual.MouseLeftButtonUp -= new MouseButtonEventHandler(Visual_ExplodeUnExplode);
                                Faces.Visual.MouseLeftButtonUp += new MouseButtonEventHandler(Visual_ExplodeUnExplode);
                            }
                        }
                    }

                    if (this.ExplodeAnimation != null)
                    {
                        this.ExplodeAnimation.Completed -= new EventHandler(ExplodeAnimation_Completed);
                        this.ExplodeAnimation.Completed += new EventHandler(ExplodeAnimation_Completed);
                    }

                    if (this.UnExplodeAnimation != null)
                    {
                        this.UnExplodeAnimation.Completed -= new EventHandler(UnExplodeAnimation_Completed);
                        this.UnExplodeAnimation.Completed += new EventHandler(UnExplodeAnimation_Completed);
                    }
                }

                if (Parent.RenderAs == RenderAs.Pie || Parent.RenderAs == RenderAs.Doughnut)
                {
                    if (LabelVisual != null)
                        AttachEvents2Visual(Object, this, LabelVisual);
                }
            }
            else if (Parent.RenderAs == RenderAs.StackedArea || Parent.RenderAs == RenderAs.StackedArea100 
                || RenderHelper.IsLineCType(Parent))
            {
                if (!RenderHelper.IsLineCType(Parent))
                {   
                    if (Parent.Faces != null)
                    {   
                        if (Object.GetType().Equals(typeof(DataPoint)))
                        {
                            foreach (FrameworkElement face in Parent.Faces.VisualComponents)
                                AttachEvents2AreaVisual(Object, this, face);
                        }
                    }
                }

                if (Marker != null)
                    AttachEvents2Visual(Object, this, Marker.Visual);
            }
            else if (Faces != null)
            {
                if (Parent.RenderAs == RenderAs.Bubble || Parent.RenderAs == RenderAs.Point || Parent.RenderAs == RenderAs.Stock || Parent.RenderAs == RenderAs.CandleStick || Parent.RenderAs == RenderAs.SectionFunnel || Parent.RenderAs == RenderAs.StreamLineFunnel || Parent.RenderAs == RenderAs.Pyramid)
                {   
                    foreach (FrameworkElement face in Faces.VisualComponents)
                    {
                        AttachEvents2Visual(Object, this, face);
                    }

                    if (Parent.RenderAs == RenderAs.Stock || Parent.RenderAs == RenderAs.CandleStick)
                    {
                        if (LabelVisual != null)
                            AttachEvents2Visual(Object, this, LabelVisual);
                    }
                }
                else
                {
                    AttachEvents2Visual(Object, this, Faces.Visual);

                    if (Marker != null)
                        AttachEvents2Visual(Object, this, Marker.Visual);

                    if (LabelVisual != null)
                        AttachEvents2Visual(Object, this, LabelVisual);

                }
            }
        }

        /// <summary>
        /// Set Href property for DataPoint visual faces
        /// </summary>
        internal void SetHref2DataPointVisualFaces()
        {
            if (Faces != null)
                if (Faces.VisualComponents.Count != 0)
                {
                    foreach (FrameworkElement face in Faces.VisualComponents)
                    {
                        AttachHref(Chart, face, Href, (HrefTargets)HrefTarget);
                    }
                }
                else
                    AttachHref(Chart, Faces.Visual, Href, (HrefTargets)HrefTarget);

            //if (this.Parent.Faces != null)
            //    if (this.Parent.Faces.VisualComponents.Count != 0)
            //    {
            //        foreach (FrameworkElement face in this.Parent.Faces.VisualComponents)
            //        {
            //            if (Parent.RenderAs != RenderAs.Area && Parent.RenderAs != RenderAs.StackedArea && Parent.RenderAs != RenderAs.StackedArea100)
            //                AttachHref(Chart, face, Href, (HrefTargets)HrefTarget);
            //        }
            //    }
            //    else
            //        AttachHref(Chart, this.Parent.Faces.Visual, Href, (HrefTargets)HrefTarget);

            if (this.Marker != null)
            {   
                AttachHref(Chart, Marker.Visual, Href, (HrefTargets)HrefTarget);
            }
        }

        /// <summary>
        /// Set Cursor property for DataPoint visual faces
        /// </summary>
        internal void SetCursor2DataPointVisualFaces()
        {   
            if (Faces != null)
                if (Faces.VisualComponents.Count != 0)
                {
                    foreach (FrameworkElement face in Faces.VisualComponents)
                    {
                        face.Cursor = GetCursor();
                    }
                }
                else if(Faces.Visual != null)
                    Faces.Visual.Cursor = GetCursor();

            //if (this.Parent.Faces != null)
            //    if (this.Parent.Faces.VisualComponents.Count != 0)
            //    {
            //        foreach (FrameworkElement face in this.Parent.Faces.VisualComponents)
            //        {
            //            face.Cursor = GetCursor();
            //        }
            //    }
            //    else
            //        this.Parent.Faces.Visual.Cursor = GetCursor();

            if (this.Marker != null && this.Marker.Visual != null)
            {
                Marker.Visual.Cursor = GetCursor();
            }
        }

        ///// <summary>
        ///// Update marker associated with DataPoint
        ///// </summary>
        //internal void UpdateMarker()
        //{
        //    if (Marker != null && Marker.Visual != null && (Boolean)MarkerEnabled)
        //    {
        //        Marker.BorderThickness = ((Thickness)(MarkerBorderThickness as Nullable<Thickness>)).Left;
        //        Marker.BorderColor = MarkerBorderColor;
        //        Marker.MarkerFillColor = MarkerColor;
        //        Marker.UpdateMarker();
        //    }
        //}

        #endregion

        #region Internal Events

        #endregion

        #region Data

        internal Double _oldYValue = 0;
        internal Double[] _oldYValues;

        internal FrameworkElement _oldVisual;
        internal Point _oldMarkerPosition;
        internal Point _oldLabelPosition;

        /// <summary>
        /// Parent of this DataPoint 
        /// </summary>
        private DataSeries _parent;      
       
        /// <summary>
        /// Whether the DataPoint is exploded (used for Pie / Doughnut)
        /// </summary>
        internal Boolean _interactiveExplodeState = false;

        /// <summary>
        /// Whether the animation is going on for the datapoint
        /// </summary>
        private Boolean _interativityAnimationState = false;

        /// <summary>
        /// Internal color holds color from theme
        /// </summary>
        internal Brush _internalColor;

        /// <summary>
        /// Whether name for DataPoint is generated automatically
        /// </summary>
        internal Boolean _isAutoName = true;

        /// <summary>
        /// Distance from mouse pointer along X coordinate
        /// </summary>
        internal Double _x_distance;

        /// <summary>
        /// Distance from mouse pointer along Y coordinate
        /// </summary>
        internal Double _y_distance;

        /// <summary>
        /// Whether the DataPoint is already Exploded
        /// </summary>
        private Boolean _isAlreadyExploded = false;

        private Boolean _isExplodeRunning = false;

        /// <summary>
        /// Whether price is up for financial charts
        /// </summary>
        internal Boolean _isPriceUp;

        /// <summary>
        /// Visual target render point of the visual
        /// </summary>
        internal Point _visualPosition;
        internal Point _visualPositionOld;

        internal Double _targetBubleSize;

        Nullable<Thickness> _borderThickness = null;
        Double _internalOpacity = Double.NaN;

        Size _originalMarkerSize = new Size(Double.NaN, Double.NaN);

#if WPF
        /// <summary>
        /// Whether the default style is applied
        /// </summary>
        private static Boolean _defaultStyleKeyApplied;            
#endif
        #endregion
    }
}
