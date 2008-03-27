/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
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
using System.Collections.Generic;
using System.Diagnostics;
using Visifire.Commons;
using System.Windows.Markup;

namespace Visifire.Charts
{
    internal enum Surface3DCharts
    {
        Bar=0, Column=1, Line=2, Area=3, 
        StackedColumn=4, StackedColumn100=5,
        StackedArea=6, StackedArea100=7, StackedBar=8,
        StackedBar100= 9
    }
    internal class ElementPosition
    {
        public static int CompareAngle(ElementPosition a, ElementPosition b)
        {
            double angle1 = (a._angle1 + a._angle2) / 2;
            double angle2 = (b._angle1 + b._angle2) / 2;
            return angle1.CompareTo(angle2);
        }
        public ElementPosition()
        {
        }
        public ElementPosition(FrameworkElement element, Double angle1, Double angle2)
        {
            _element = element;
            _angle1 = angle1;
            _angle2 = angle2;
        }
        public ElementPosition(ElementPosition m)
        {
            _element = m._element;
            _angle1 = m._angle1;
            _angle2 = m._angle2;
        }
        public FrameworkElement _element;
        public Double _angle1;
        public Double _angle2;
    }

    public class DataSeries : VisualObject
    {
        #region Public Methods
        public DataSeries()
        {

          
        }

        public override void SetWidth()
        {
            this.SetValue(WidthProperty, (Double)_parent.PlotArea.GetValue(WidthProperty));

            Double x = (Double)_parent.PlotArea.GetValue(WidthProperty);

            _borderRectangle.SetValue(WidthProperty, this.GetValue(WidthProperty));

        }

        public override void SetHeight()
        {
            this.SetValue(HeightProperty, (Double)_parent.PlotArea.GetValue(HeightProperty));

            _borderRectangle.SetValue(HeightProperty, this.GetValue(HeightProperty));

        }

        public override void SetTop()
        {
            this.SetValue(TopProperty, (Double)_parent.PlotArea.GetValue(TopProperty));


        }

        public override void SetLeft()
        {
            this.SetValue(LeftProperty, (Double)_parent.PlotArea.GetValue(LeftProperty));


        }

        public override void Render()
        {
        }

        public override void Init()
        {
            ValidateParent();

            if (base.Background != null)
            {
                Background = base.Background;
                base.Background = null;
            }
            if (String.IsNullOrEmpty(ColorSet))
            {
                if (_parent.ColorSetReference != null)
                {
                    if (_parent.UniqueColors)
                    {
                        if (Background == null && _parent.DataSeries.Count > 1)
                        {
                            Background = Cloner.CloneBrush(_parent.ColorSetReference.GetColor());
                        }
                        else if (Background == null)
                        {
                            ColorSetReference = _parent.ColorSetReference;
                        }
                    }
                    else
                    {
                        if (Background == null)
                        {
                            Background = Cloner.CloneBrush(_parent.ColorSetReference.GetColor());
                        }
                    }
                }
            }
            else
            {
                // Find if the pallete is available in the pallets collection
                foreach (ColorSet child in _parent.ColorSets)
                {
                    if (child.Name.ToLower() == ColorSet.ToLower())
                    {
                        ColorSetReference = child;
                    }
                }
            }
            SetName();


            if (Opacity > 1 && GetFromTheme("Opacity") != null)
                Opacity = Convert.ToDouble(GetFromTheme("Opacity"));
            else if (Opacity > 1) Opacity = 1;

            if (ColorSetReference == null && _parent.ColorSetReference == null)
            {
                foreach (ColorSet child in _parent.ColorSets)
                {
                    if (child.Name.ToLower() == "visifire1")
                        ColorSetReference = child;
                }
            }
            // If there is only one Dataseries and if ShowInLegend is not defined then make it false
            if (RenderAs.ToLower() != "pie" && RenderAs.ToLower() != "doughnut")
            {
                if (_showInLegend == "Undefined")
                {
                    if (_parent.DataSeries.Count == 1)
                    {
                        _showInLegend = false.ToString();
                    }
                    else
                    {
                        _showInLegend = true.ToString();
                    }
                }
            }


            CreateReferences();


            if (DataPoints.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine(RenderAs + " : To draw chart atleast one DataPoint is required");
                throw (new Exception(RenderAs + " : To draw chart atleast one DataPoint is required"));

            }


            if (Background == null)
            {
                Background = Cloner.CloneBrush(ColorSetReference.GetColor(Index));
            }


            if (_lightingEnabled == "Undefined" && _parent.View3D)
            {
                _lightingEnabled = true.ToString();
            }

            int i = 0;
            int chType = 0;
            switch (RenderAs.ToUpper())
            {
                case "LINE":
                    Canvas _lineCanvas = new Canvas();
                    if (_parent.View3D)
                    {
                        _parent.AreaLine3D.Add(_lineCanvas);
                        _lineCanvas.Width = _parent.Width;
                        _lineCanvas.Height = _parent.Height;
                        _parent.Children.Add(_lineCanvas);
                        _lineCanvas.SetValue(ZIndexProperty, (int)this.GetValue(ZIndexProperty));
                        _drawingCanvas = _lineCanvas;

                    }
                    _line = new Polyline();
                    _lineShadow = new Polyline();
                    if (_parent.View3D)
                    {
                        _lineCanvas.Children.Add(_line);
                        _lineCanvas.Children.Add(_lineShadow);
                    }
                    else
                    {
                        this.Children.Add(_line);
                        this.Children.Add(_lineShadow);
                    }
                    break;

                case "PIE":
                    if (_parent.View3D)
                    {
                        _pies = new Path[DataPoints.Count];
                        _pieLeft = new Path[DataPoints.Count];
                        _pieRight = new Path[DataPoints.Count];
                        _pieSides = new Path[DataPoints.Count];
                        for (i = 0; i < DataPoints.Count; i++)
                        {
                            _pies[i] = new Path();
                            _pieLeft[i] = new Path();
                            _pieRight[i] = new Path();
                            _pieSides[i] = new Path();

                            this.Children.Add(_pies[i]);
                            this.Children.Add(_pieLeft[i]);
                            this.Children.Add(_pieRight[i]);
                            this.Children.Add(_pieSides[i]);
                        }
                    }
                    else
                    {
                        _pies = new Path[DataPoints.Count];
                        for (i = 0; i < DataPoints.Count; i++)
                        {
                            _pies[i] = new Path();
                            DataPoints[i].Children.Add(_pies[i]);
                        }
                    }
                    break;
                case "DOUGHNUT":
                    if (_parent.View3D)
                    {
                        _pies = new Path[DataPoints.Count];
                        _pieLeft = new Path[DataPoints.Count];
                        _pieRight = new Path[DataPoints.Count];
                        _pieSides = new Path[DataPoints.Count];
                        _doughnut = new Path[DataPoints.Count];
                        for (i = 0; i < DataPoints.Count; i++)
                        {
                            _pies[i] = new Path();
                            _pieLeft[i] = new Path();
                            _pieRight[i] = new Path();
                            _pieSides[i] = new Path();
                            _doughnut[i] = new Path();
                            this.Children.Add(_pies[i]);
                            this.Children.Add(_pieLeft[i]);
                            this.Children.Add(_pieRight[i]);
                            this.Children.Add(_pieSides[i]);
                            this.Children.Add(_doughnut[i]);
                        }
                    }
                    else
                    {
                        _doughnut = new Path[DataPoints.Count];
                        for (i = 0; i < DataPoints.Count; i++)
                        {
                            _doughnut[i] = new Path();
                            DataPoints[i].Children.Add(_doughnut[i]);
                        }
                    }
                    break;
                case "AREA":
                    Canvas _areaCanvas = new Canvas();
                    if (_parent.View3D)
                    {
                        _parent.AreaLine3D.Add(_areaCanvas);
                        _areaCanvas.Width = _parent.Width;
                        _areaCanvas.Height = _parent.Height;
                        _parent.Children.Add(_areaCanvas);
                        _areaCanvas.SetValue(ZIndexProperty, (int)this.GetValue(ZIndexProperty));
                        _drawingCanvas = _areaCanvas;
                    }
                    _areas = new Polygon[DataPoints.Count - 1];


                    for (i = 0; i < DataPoints.Count - 1; i++)
                    {
                        _areas[i] = new Polygon();
                        if (_parent.View3D)
                        {
                            _areaCanvas.Children.Add(_areas[i]);
                        }
                        else
                        {
                            DataPoints[i].Children.Add(_areas[i]);
                        }
                    }
                    if (_parent.View3D)
                    {
                        _areaShadows = new Rectangle[DataPoints.Count - 1];
                        _areaTops = new Polygon[DataPoints.Count - 1];
                        for (i = 0; i < DataPoints.Count - 1; i++)
                        {
                            _areaTops[i] = new Polygon();
                            _areaShadows[i] = new Rectangle();
                            _areaCanvas.Children.Add(_areaTops[i]);
                            _areaCanvas.Children.Add(_areaShadows[i]);
                        }
                    }
                    break;
                case "STACKEDAREA100":
                case "STACKEDAREA":
                    if (_parent.View3D)
                    {
                        if (RenderAs.ToUpper() == "STACKEDAREA")
                            chType = (int)Surface3DCharts.StackedArea;
                        else
                            chType = (int)Surface3DCharts.StackedArea100;

                        if (_parent.Surface3D[chType] == null)
                        {
                            _parent.Surface3D[chType] = new Canvas();
                            _parent.Surface3D[chType].Height = _parent.Height;
                            _parent.Surface3D[chType].Width = _parent.Width;
                            _parent.Children.Add(_parent.Surface3D[chType]);
                            _parent.Surface3D[chType].SetValue(ZIndexProperty, this.GetValue(ZIndexProperty));

                        }
                        else
                        {
                            if ((int)this.GetValue(ZIndexProperty) > (int)_parent.Surface3D[chType].GetValue(ZIndexProperty))
                                _parent.Surface3D[chType].SetValue(ZIndexProperty, this.GetValue(ZIndexProperty));
                        }
                        _drawingCanvas = _parent.Surface3D[chType];
                    }
                    _areas = new Polygon[DataPoints.Count - 1];
                    for (i = 0; i < DataPoints.Count - 1; i++)
                    {
                        _areas[i] = new Polygon();
                        if (_parent.View3D)
                        {
                            _parent.Surface3D[chType].Children.Add(_areas[i]);
                        }
                        else
                        {
                            DataPoints[i].Children.Add(_areas[i]);
                        }
                    }
                    if (_parent.View3D)
                    {
                        _areaShadows = new Rectangle[DataPoints.Count - 1];
                        _areaTops = new Polygon[DataPoints.Count - 1];
                        for (i = 0; i < DataPoints.Count - 1; i++)
                        {
                            _areaTops[i] = new Polygon();
                            _areaShadows[i] = new Rectangle();
                            _parent.Surface3D[chType].Children.Add(_areaTops[i]);
                            _parent.Surface3D[chType].Children.Add(_areaShadows[i]);
                        }
                    }

                    break;

                case "COLUMN":
                case "STACKEDCOLUMN":
                case "BAR":
                case "STACKEDBAR":
                case "STACKEDBAR100":
                case "STACKEDCOLUMN100":
                    if (_parent.View3D)
                    {
                        if (RenderAs.ToUpper() == "COLUMN")
                            chType = (int)Surface3DCharts.Column;
                        else if (RenderAs.ToUpper() == "STACKEDCOLUMN")
                            chType = (int)Surface3DCharts.StackedColumn;
                        else if (RenderAs.ToUpper() == "BAR")
                            chType = (int)Surface3DCharts.Bar;
                        else if (RenderAs.ToUpper() == "STACKEDBAR")
                            chType = (int)Surface3DCharts.StackedBar;
                        else if (RenderAs.ToUpper() == "STACKEDBAR100")
                            chType = (int)Surface3DCharts.StackedBar100;
                        else
                            chType = (int)Surface3DCharts.StackedColumn100;

                        if (_parent.Surface3D[chType] == null)
                        {
                            _parent.Surface3D[chType] = new Canvas();
                            _parent.Surface3D[chType].Height = _parent.Height;
                            _parent.Surface3D[chType].Width = _parent.Width;
                            _parent.Children.Add(_parent.Surface3D[chType]);
                            _parent.Surface3D[chType].SetValue(ZIndexProperty, this.GetValue(ZIndexProperty));

                        }
                        else
                        {
                            if ((int)this.GetValue(ZIndexProperty) > (int)_parent.Surface3D[chType].GetValue(ZIndexProperty))
                                _parent.Surface3D[chType].SetValue(ZIndexProperty, this.GetValue(ZIndexProperty));
                        }
                        _drawingCanvas = _parent.Surface3D[chType];
                    }
                    _columns = new Rectangle[DataPoints.Count];
                    _columnShadows = new Rectangle[DataPoints.Count];
                    _shadows = new Rectangle[DataPoints.Count];
                    for (i = 0; i < DataPoints.Count; i++)
                    {
                        _columns[i] = new Rectangle();
                        _columnShadows[i] = new Rectangle();
                        _shadows[i] = new Rectangle();
                        if (_parent.View3D)
                        {
                            _parent.Surface3D[chType].Children.Add(_columns[i]);
                            _parent.Surface3D[chType].Children.Add(_columnShadows[i]);
                            _parent.Surface3D[chType].Children.Add(_shadows[i]);
                        }
                        else
                        {
                            DataPoints[i].Children.Add(_columns[i]);
                            DataPoints[i].Children.Add(_columnShadows[i]);
                            DataPoints[i].Children.Add(_shadows[i]);
                        }
                    }
                    if (_parent.View3D)
                    {
                        _columnTops = new Rectangle[DataPoints.Count];


                        for (i = 0; i < DataPoints.Count; i++)
                        {
                            _columnTops[i] = new Rectangle();


                            _parent.Surface3D[chType].Children.Add(_columnTops[i]);

                        }
                    }
                    break;
                default:
                    if (_parent.View3D)
                        _drawingCanvas = _parent;
                    else
                        _drawingCanvas = this;
                    break;
            }
        }

        #endregion Public Methods

        #region Public Properties

        public String XValueFormatString
        {
            get
            {
                return _xValueFormatString;
            }
            set
            {
                _xValueFormatString = value;
            }
        }
        public String YValueFormatString
        {
            get
            {
                return _yValueFormatString;
            }
            set
            {
                _yValueFormatString = value;
            }
        }
        public String ZValueFormatString
        {
            get
            {
                if (String.IsNullOrEmpty(_zValueFormatString))
                    return "###,##0.##";
                else
                    return _zValueFormatString;

            }
            set
            {
                _zValueFormatString = value;
            }
        }

        public String RenderAs
        {
            get;
            set;
        }

        public String ColorSet
        {
            get
            {
                if (String.IsNullOrEmpty(_colorSet) || _colorSet == "Undefined")
                    return Convert.ToString(GetFromTheme("ColorSet"));
                else
                    return _colorSet;
                
            }
            set
            {
                _colorSet = value;
            }
        }

        
        public override Brush Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
            }
        }

        
        public override String Color
        {
            get
            {
                return _color;
            }
            set
            {
                _background = Parser.ParseColor(value);
                _color = value;
            }
        }
        public override String Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                ImageBrush imgBrush = new ImageBrush();
                Uri ur = new Uri(_image, UriKind.RelativeOrAbsolute);
                if (ur.IsAbsoluteUri)
                {
                    _image = ur.AbsoluteUri;
                }
                else
                {
                    UriBuilder ub = new UriBuilder(Application.Current.Host.Source);
                    String sourcePath = ub.Path.Substring(0, ub.Path.LastIndexOf('/') + 1);
                    UriBuilder ub2 = new UriBuilder(ub.Scheme, ub.Host, ub.Port, sourcePath + value);
                    _image = ub2.ToString();
                }
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" ImageSource=\"" + _image + "\"/>";

                imgBrush = (ImageBrush)XamlReader.Load(XAMLimage);
                imgBrush.ImageFailed += new ExceptionRoutedEventHandler(imgBrush_ImageFailed);
                _background = imgBrush;
            }
        }
        public virtual Stretch ImageStretch
        {
            get
            {
                return _imageStretch;
            }
            set
            {
                _imageStretch = value;
                if (_background != null)
                {
                    if (_background.GetType().Name == "ImageBrush")
                    {
                        (_background as ImageBrush).Stretch = value;
                    }
                }
            }
        }

        public LabelBase ToolTip
        {
            get
            {
                return _parent.ToolTip;
            }
        }

        public Boolean ShowInLegend
        {
            get
            {
                if (_showInLegend != "Undefined" && !String.IsNullOrEmpty(_showInLegend))
                    return Boolean.Parse(_showInLegend);
                else
                    return false;
            }
            set
            {
                _showInLegend = value.ToString();
            }
        }

        public String LegendText
        {
            get;
            set;
        }

        public String Legend
        {
            get;

            set;

        }

        public  Boolean ShadowEnabled
        {
            get
            {
                if (_shadowEnabled == "Undefined" || String.IsNullOrEmpty(_shadowEnabled))
                {
                    if (GetFromTheme("ShadowEnabled") == null)
                        return false;
                    else
                        return Convert.ToBoolean(GetFromTheme("ShadowEnabled"));
                }
                else
                    return Boolean.Parse(_shadowEnabled);
            }
            set
            {
                _shadowEnabled = value.ToString();
            }
        }

        public  Boolean LightingEnabled
        {
            get
            {
                if (_lightingEnabled == "Undefined" || String.IsNullOrEmpty(_lightingEnabled))
                {
                    if (GetFromTheme("LightingEnabled") == null)
                        return false;
                    else
                        return Convert.ToBoolean(GetFromTheme("LightingEnabled"));
                }
                else
                    return Boolean.Parse(_lightingEnabled);
            }
            set
            {
                _lightingEnabled = value.ToString();
            }
        }

        #region Label Properties

        public String LabelEnabled
        {
            get
            {
                if (_labelEnabled == "Undefined" && (this.RenderAs.ToLower() == "pie" || this.RenderAs.ToLower() == "doughnut"))
                {
                    return "True";
                }
                else
                    return _labelEnabled;
            }
            set
            {
                _labelEnabled = value;

            }
        }

        public String LabelText
        {
            get
            {
                if (!String.IsNullOrEmpty(_labelText))
                    return _labelText;
                else
                    return _parent.Label.Text;
            }
            set
            {
                _labelText = value; ;
            }
        }

        public String LabelFontFamily
        {
            get
            {
                if (_labelFontFamily != null)
                    return _labelFontFamily;
                else
                    return _parent.Label.FontFamily;
            }
            set
            {
                _labelFontFamily = value;
            }
        }

        public Double LabelFontSize
        {
            get
            {
                if (!Double.IsNaN(_labelFontSize))
                    return _labelFontSize;
                else
                    return _parent.Label.FontSize;
            }
            set
            {
                _labelFontSize = value;
            }
        }

        public Brush LabelFontColor
        {
            get
            {
                if (_labelFontColor != null)
                    return _labelFontColor;
                else
                    return Cloner.CloneBrush(_parent.Label.FontColor);
            }
            set
            {
                _labelFontColor = value;
            }
        }

        public String LabelFontWeight
        {
            get
            {
                if (_labelFontWeight != null)
                    return _labelFontWeight.ToString();
                else
                    return _parent.Label.FontWeight;
            }
            set
            {
                _labelFontWeight = Converter.StringToFontWeight(value);
            }
        }

        public String LabelFontStyle
        {
            get
            {
                if (_labelFontStyle != null)
                    return _labelFontStyle;
                else
                    return _parent.Label.FontStyle;
            }
            set
            {
                _labelFontStyle = value;
            }
        }

        public Brush LabelBackground
        {
            get
            {
                if (_labelBackground != null)
                    return _labelBackground;
                else if (_parent.Label.Background != null)
                    return Cloner.CloneBrush(_parent.Label.Background);
                else
                    return null;
            }
            set
            {
                _labelBackground = value;
            }
        }

        public String LabelStyle
        {
            get
            {
                if (!String.IsNullOrEmpty(_labelStyle))
                    return _labelStyle;
                else
                {
                    switch(RenderAs.ToLower())
                    {
                        case "stackedcolumn":
                        case "stackedbar":
                        case "stackedarea":
                        case "stackedcolumn100":
                        case "stackedbar100":
                        case "stackedarea100":
                            return "Inside";
                        default:
                            return "Outside";
                    }
                }
            }
            set
            {
                _labelStyle = value;
            }
        }

        public String LabelLineEnabled
        {
            get
            {
                if (LabelEnabled.ToLower() == "true")
                {
                    if (_labelLineEnabled.ToLower() == "undefined" )
                    {
                        if(this.RenderAs.ToLower() == "pie" || this.RenderAs.ToLower() == "doughnut")
                            return "True";
                        else
                            return "undefined";
                    }
                    else return _labelLineEnabled;
                }
                return "false";
            }
            set
            {
                _labelLineEnabled = value;
            }
        }

        public Brush LabelLineColor
        {
            get
            {
                if (_labelLineColor != null)
                    return _labelLineColor;
                else if(GetFromTheme("LabelLineColor") != null)
                    return (GetFromTheme("LabelLineColor") as Brush);
                else
                    return null;
            }
            set
            {
                _labelLineColor = value;
            }
        }

        public Double LabelLineThickness
        {
            get
            {
                if (!Double.IsNaN(_labelLineThickness))
                    return _labelLineThickness;
                else if (GetFromTheme("LabelLineThickness") != null)
                    return Convert.ToDouble(GetFromTheme("LabelLineThickness"));
                else
                    return 0.5;
            }
            set
            {
                _labelLineThickness = value;
            }

        }

        public String LabelLineStyle
        {
            get
            {
                if (!String.IsNullOrEmpty(_labelLineStyle))
                    return _labelLineStyle;
                else
                    return "solid";
            }
            set
            {
                _labelLineStyle = value;
            }
        }

        #endregion Label Properties

        public override String ToolTipText
        {
            get
            {
                if (!String.IsNullOrEmpty(_toolTipText))
                    return _toolTipText;
                else
                {
                    switch(RenderAs.ToLower())
                    {
                        case "stackedcolumn100":
                        case "stackedbar100":
                        case "stackedarea100":
                        case "pie":
                        case "doughnut":
                            return "#AxisLabel, #YValue(#Percentage%)";
                           
                        default:
                            return "#AxisLabel, #YValue";
                            
                    }
                }
            }
            set
            {
                _toolTipText = value;
            }
        }
       
        public override String Href
        {
            get
            {
                if (!String.IsNullOrEmpty(_href))
                    return _href;
                else
                    return "";
            }
            set
            {
                _href = value;
            }
        }

        public Double LineThickness
        {
            get;
            set;
        }

        #region Marker Properties
        public String MarkerEnabled
        {
            get
            {
                return _markerEnabled;
            }
            set
            {
                _markerEnabled = value;
            }
        }
        public String MarkerStyle
        {
            get
            {
                if (_markerStyle != null)
                    return _markerStyle;
                else if (GetFromTheme("MarkerStyle") == null)
                    return "Circle";
                else
                    return Convert.ToString(GetFromTheme("MarkerStyle"));
            }
            set
            {
                _markerStyle = value;
            }
        }

        public Double MarkerBorderThickness
        {
            get
            {
                if (!Double.IsNaN(_markerBorderThickness))
                    return _markerBorderThickness;
                else
                    return Convert.ToDouble(GetFromTheme("MarkerBorderThickness"));
            }
            set
            {
                _markerBorderThickness = value;
            }
        }

        public Double MarkerSize
        {
            get
            {
                return _markerSize;
            }
            set
            {
                _markerSize = value;
            }
        }
        internal Brush MarkerBackground
        {
            get
            {
                if (_markerBackground != null)
                    return _markerBackground;
                else
                    return GetFromTheme("MarkerBackground") as Brush;
            }
            set
            {
                _markerBackground = value;
            }

        }
        public String MarkerColor
        {
            set
            {
                MarkerBackground = Parser.ParseColor(value);
            }
        }
        
        public Brush MarkerBorderColor
        {
            get
            {
                if (_markerBorderColor != null)
                    return _markerBorderColor;
                else
                    return GetFromTheme("MarkerBorderColor") as Brush;
            }
            set
            {
                _markerBorderColor = value;
            }
        }

        public String MarkerImage
        {
            get
            {
                return _markerImage;
            }
            set
            {
                _markerImage = value;
            }
        }

        public Double MarkerScale
        {
            get
            {
                return _markerScale;
            }
            set
            {
                _markerScale = value;
            }
        }


        #endregion Marker Properties

        public Double StartAngle
        {
            get
            {
                return _startAngle;
            }
            set
            {
                _startAngle = ((int)(value) % 360) * Math.PI / 180;
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void SetName()
        {
            if (this.Name.Length == 0)
            {
                int i = 0;

                String type = this.GetType().Name;
                String name = type;

                // Check for an available name
                while (FindName(name + i.ToString()) != null)
                {
                    i++;
                }

                name += i.ToString();

                this.SetValue(NameProperty, name);
            }
        }

        protected override void SetDefaults()
        {
            base.SetDefaults();

            DataPoints = new System.Collections.Generic.List<DataPoint>();

            _borderRectangle = new Rectangle();

            RenderAs = "Column";

            _startAngle = 0;

            
            ShadowSize = 6;
            
            _borderRectangle.Stroke = new SolidColorBrush(Colors.Black);
            _borderRectangle.StrokeThickness = .4;

            _markerBorderThickness = Double.NaN;
            _markerSize = Double.NaN;
            _markerScale = Double.NaN;
            Enabled = true;
            _labelEnabled = "Undefined";
            _labelLineEnabled = "Undefined";
            _labelLineThickness = Double.NaN;
            _markerEnabled = "Undefined";
            _lightingEnabled = "Undefined";
            _shadowEnabled = "Undefined";
            LineThickness = Double.NaN;
            _labelFontSize = Double.NaN;
            _labelFontColor = null;
            
            _showInLegend = "Undefined";
            Legend = "Legend0";
            
            ColorSet = "";
            ColorSetReference = null;
            // Setting the zindex of DataSeries higher than Axes makes it visible on top of GridLines and other Axes elements
            this.SetValue(ZIndexProperty, 5);
            Opacity = 1.1;
            
        }

     
        private void PlotPoint()
        {
            int i;
            Double OffsetX=0, OffsetY=0;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count) * _parent.IndexList["point" + DrawingIndex.ToString()];
            Double depth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count);

            if (Double.IsNaN(initialDepth)) initialDepth = 0;
            if (Double.IsNaN(depth)) depth = _parent.AxisX.MajorTicks.TickLength;

            if (_parent.View3D)
            {
                OffsetX = -(depth + initialDepth);
                OffsetY = (depth + initialDepth);
            }
            for (i = 0; i < DataPoints.Count; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;

                DataPoints[i].SetValue(LeftProperty, _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) + OffsetX);
                DataPoints[i].SetValue(TopProperty, _parent.AxisY.DoubleToPixel(DataPoints[i].YValue) + OffsetY);
                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;

                DataPoints[i].MarkerEnabled = "true";
                DataPoints[i].MarkerBackground = Cloner.CloneBrush(DataPoints[i].Background);
                DataPoints[i].MarkerBorderThickness = DataPoints[i].BorderThickness;
                DataPoints[i].MarkerBorderColor = Cloner.CloneBrush(DataPoints[i].BorderColor);


                Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                if (marker != null)
                {
                    marker.SetValue(ZIndexProperty,(int) GetValue(ZIndexProperty) + 20);
                    DataPoints[i].AttachToolTip(marker);
                    marker.Opacity = Opacity * DataPoints[i].Opacity;
                }

                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i],depth);
            }

            
        }

        private void PlotBubble()
        {

            // Initial values for minimum size of the bubble
            Double minSize = (_parent.PlotArea.Width > _parent.PlotArea.Height ? _parent.PlotArea.Height : _parent.PlotArea.Width) * 0.05;
            Double minZ = Double.PositiveInfinity;

            // Initial values of the maximum size of the bubble
            Double maxSize = (_parent.PlotArea.Width > _parent.PlotArea.Height ? _parent.PlotArea.Width : _parent.PlotArea.Height) * 1 / 5;
            Double maxZ = 0;

            int i;

            // find true min and max Z
            for (i = 0; i < DataPoints.Count; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;
                if (minZ > DataPoints[i].ZValue) minZ = DataPoints[i].ZValue;
                if (maxZ < DataPoints[i].ZValue) maxZ = DataPoints[i].ZValue;
            }

            // Slope to calculate bubble size for datapoint values
            Double slope = (maxSize - minSize) / (maxZ - minZ);
            Double intercept = minSize - minZ * slope;
            Double offsetX=0, offsetY=0;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count==0?1:_parent.Count) * _parent.IndexList["bubble" + DrawingIndex.ToString()];
            Double depth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count);

            if (Double.IsNaN(initialDepth)) initialDepth = 0;
            if (Double.IsNaN(depth)) depth = _parent.AxisX.MajorTicks.TickLength;
            if (_parent.View3D)
            {
                offsetX = (depth + initialDepth);
                offsetY = (depth + initialDepth);
            }
            

            for (i = 0; i < DataPoints.Count; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;

                DataPoints[i].SetValue(LeftProperty, _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - offsetX);
                DataPoints[i].SetValue(TopProperty, _parent.AxisY.DoubleToPixel(DataPoints[i].YValue) + offsetY);
                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;

                DataPoints[i].MarkerEnabled = "true";
                if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                {

                    SolidColorBrush tempBrush = Cloner.CloneBrush(DataPoints[i].Background) as SolidColorBrush;
                    String brush = "-60;";
                    brush += Parser.GetDarkerColor(tempBrush.Color, 0.65) + ",0;";
                    brush += Parser.GetLighterColor(tempBrush.Color, 0.55) + ",1";
                    DataPoints[i].MarkerBackground = Parser.ParseLinearGradient(brush);
                }
                else
                {
                    DataPoints[i].MarkerBackground = Cloner.CloneBrush(DataPoints[i].Background);

                }
                DataPoints[i].MarkerBorderThickness = DataPoints[i].BorderThickness;
                DataPoints[i].MarkerBorderColor = Cloner.CloneBrush(DataPoints[i].BorderColor);
                if (Double.IsNaN(DataPoints[i].ZValue) || Double.IsInfinity(DataPoints[i].ZValue))
                {
                    DataPoints[i].MarkerEnabled = "false";
                }
                else
                {
                    DataPoints[i].MarkerSize = (slope * DataPoints[i].ZValue + intercept) * DataPoints[i].MarkerScale;
                }

                Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                if (marker != null)
                {

                    DataPoints[i].AttachToolTip(marker);
                    marker.Opacity = DataPoints[i].Opacity * Opacity;
                }

                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i],0);
            }
        }

        private void PlotStackedBar100()
        {
            Double height = 10;
            Double width;
            Double top;
            Double left;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.
            Visifire.Charts.Marker marker;

            int i;

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
                height = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);
            else
                height = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);

            Double temp = Math.Abs(_parent.AxisX.DoubleToPixel(Plot.MinAxisXValue) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum));
            temp *= 2;
            if (height > temp)
                height = temp;

            height -= height * 0.3;

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<double, Point>();

            for (i = 0; i < _columns.Length; i++)
            {
                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                point.Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue / Plot.YValueSum[DataPoints[i].XValue] * 100);

                if (_parent.AxisY.AxisMinimum > 0)
                    width = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum) - point.Y;
                else
                    width = Math.Abs(_parent.AxisY.DoubleToPixel(0) - point.Y);

                
                top = (point.X - height / 2);

                if (DataPoints[i].YValue >= 0)
                {

                    left = _parent.AxisY.DoubleToPixel(0);

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[i].XValue].X;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(left + width, Plot.TopBottom[DataPoints[i].XValue].Y);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(left + width, left);

                }
                else
                {

                    left = point.Y;

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[i].XValue].Y - width;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(Plot.TopBottom[DataPoints[i].XValue].X, left);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(left + width, left);
                }

                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, left);
                DataPoints[i].SetValue(TopProperty, top);

                if (_parent.View3D)
                {

                    //To plot in 3D
                    #region Bar 3D Plotting
                    //relative width is set here
                    Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["stackedbar100"];
                    Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
                    
                    top += (depth+ initialDepth);
                    DataPoints[i].SetValue(TopProperty, top);
                    
                    _columnShadows[i].Width = depth;
                    _columnShadows[i].Height = height;
                    _columnShadows[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columnShadows[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + width + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth));

                    _columnShadows[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    _columnShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);



                    _columnShadows[i].StrokeThickness = DataPoints[i].BorderThickness;

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _columnShadows[i].RenderTransform = st1;

                    _columnTops[i].Width = width;
                    //relative height is set here
                    _columnTops[i].Height = depth;

                    _columnTops[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) - _columnTops[i].Height + (Double)_parent.PlotArea.GetValue(TopProperty));

                    //use4 the same relative width here
                    _columnTops[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + (Double)_parent.PlotArea.GetValue(LeftProperty) - initialDepth);


                    _columnTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columnTops[i].StrokeThickness = DataPoints[i].BorderThickness;


                    _columnTops[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    SkewTransform st2 = new SkewTransform();
                    st2.AngleY = 0;
                    st2.CenterX = 0;
                    st2.CenterY = 0;
                    st2.AngleX = -45;
                    _columnTops[i].RenderTransform = st2;

                    _columns[i].Width = width;
                    _columns[i].Height = height;
                    _columns[i].SetValue(LeftProperty, left + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth));
                    _columns[i].SetValue(TopProperty, top + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columns[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columns[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _columns[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    _columns[i].SetValue(ZIndexProperty, (int)(_parent.PlotArea.Height - top) + 10);
                    _columnShadows[i].SetValue(ZIndexProperty, (int)(_parent.PlotArea.Height - top) + 5);
                    _columnTops[i].SetValue(ZIndexProperty, (int)(_parent.PlotArea.Height - top) + 6);

                    //This part of the code generates the 3d gradients
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.5);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";
                            brushTop = Parser.ParseLinearGradient(linbrush);


                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.95);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",1";
                            brushShade = Parser.ParseLinearGradient(linbrush);
                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _columnShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _columns[i].Fill = Cloner.CloneBrush(brush2);

                    //Apply transform to the brushTop

                    _columnTops[i].Fill = Cloner.CloneBrush(brushTop);


                    if (left < _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum))
                    {
                        _shadows[i].Width = width + ShadowSize;
                    }
                    else
                    {
                        _shadows[i].Width = width;
                    }
                    _shadows[i].SetValue(TopProperty, (Double)_columnTops[i].GetValue(TopProperty) - _columnShadows[i].Height * 0.3);
                    _shadows[i].SetValue(LeftProperty, (Double)_columnTops[i].GetValue(LeftProperty));
                    _shadows[i].Height = _columnShadows[i].Height;
                    _shadows[i].Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    _shadows[i].Fill = Parser.ParseSolidColor("#66000000");
                    _shadows[i].SetValue(ZIndexProperty, 3);
                    DataPoints[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) - (depth + initialDepth));
                    if (!ShadowEnabled)
                    {
                        _shadows[i].Opacity = 0;
                    }
                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    #endregion Bar 3D Plotting

                    DataPoints[i].AttachToolTip(_columnShadows[i]);
                    DataPoints[i].AttachToolTip(_columnTops[i]);

                    DataPoints[i].AttachToolTip(_columns[i]);
                    marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)_columns[i].GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],depth);
                }
                else
                {

                    #region Bar2D
                    Path bar = new Path();
                    Path shadow = new Path();
                    String pathXAML = @"";
                    String maxDP = "", maxDS = "";
                    Double xRadiusLimit = 0;
                    Double yRadiusLimit = 0;
                    if (DataPoints[i].YValue > 0)
                    {
                        GetPositiveMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }
                    else
                    {
                        GetNegativeMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }


                    if (maxDS == this.Name && maxDP == DataPoints[i].Name)
                    {
                        xRadiusLimit = DataPoints[i].RadiusX;
                        yRadiusLimit = DataPoints[i].RadiusY;
                    }

                    if (xRadiusLimit > width) xRadiusLimit = width;
                    if (yRadiusLimit > height / 2) yRadiusLimit = height / 2;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height - yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width - xRadiusLimit, height);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    DataPoints[i].Children.Add(bar);
                    DataPoints[i].Children.Add(shadow);

                    bar.Width = width;
                    bar.Height = height;
                    
                    bar.SetValue(TopProperty, 0);
                    bar.SetValue(LeftProperty, 0);
                    bar.Data = (PathGeometry)XamlReader.Load(pathXAML);



                    shadow.Width = width;
                    shadow.Height = ShadowSize;
                    
                    shadow.SetValue(TopProperty, height);
                    shadow.SetValue(LeftProperty, 0);

                    
                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", shadow.Width - xRadiusLimit, 0);



                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", shadow.Width, -yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", shadow.Width, shadow.Height - yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", shadow.Width - xRadiusLimit, shadow.Height);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, shadow.Height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    shadow.Data = (PathGeometry)XamlReader.Load(pathXAML);


                    bar.Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    bar.StrokeThickness = DataPoints[i].BorderThickness;
                    String linbrush;
                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
                    {


                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        bar.Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && Bevel)
                    {
                        
                        if (DataPoints[i].YValue > 0) linbrush = "-90;";
                        else linbrush = "90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor(brush.Color, 0.99);
                        linbrush += ",1";
                        bar.Fill = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        bar.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    bar.SetValue(ZIndexProperty, 5);

                    if (DataPoints[i].YValue < 0)
                    {
                        ScaleTransform st = new ScaleTransform();
                        st.ScaleX = -1;
                        st.ScaleY = 1;
                        bar.RenderTransformOrigin = new Point(0.5, 0.5);
                        bar.RenderTransform = st;

                        st = new ScaleTransform();
                        st.ScaleX = -1;
                        st.ScaleY = 1;
                        shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                        shadow.RenderTransform = st;
                    }
                    shadow.Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    shadow.Fill = Parser.ParseSolidColor("#66000000");
                    shadow.SetValue(ZIndexProperty, 3);
                    if (!ShadowEnabled)
                    {
                        shadow.Opacity = 0;
                    }
                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            bar.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            bar.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }

                    DataPoints[i].AttachToolTip(bar);
                    marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)bar.GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],0);

                    DataPoints[i].AttachHref(bar);
                    if (width > 10 && height > 10)
                        DataPoints[i].ApplyEffects((int)bar.GetValue(ZIndexProperty) + 1);
                    #endregion Bar2D
                }

            }

        }

        private void PlotStackedBar()
        {
            Double height = 10;
            Double width;
            Double top;
            Double left;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.
            Visifire.Charts.Marker marker;
            int i;

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
                height = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);
            else
                height = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);

            Double temp = Math.Abs(_parent.AxisX.DoubleToPixel(Plot.MinAxisXValue) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum));
            temp *= 2;
            if (height > temp)
                height = temp;

            height -= height * 0.3;

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<double, Point>();

            for (i = 0; i < _columns.Length; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                point.Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue);


                if (_parent.AxisY.AxisMinimum > 0)
                    width = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum) - point.Y;
                else
                    width = Math.Abs(_parent.AxisY.DoubleToPixel(0) - point.Y);

                
                top = (point.X - height / 2);

                if (DataPoints[i].YValue >= 0)
                {

                    left = _parent.AxisY.DoubleToPixel(0);

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[i].XValue].X;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(left + width, Plot.TopBottom[DataPoints[i].XValue].Y);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(left + width, left);



                }
                else
                {
                    left = point.Y;

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[i].XValue].Y - width;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(Plot.TopBottom[DataPoints[i].XValue].X, left);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(left + width, left);
                }

                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, left);
                DataPoints[i].SetValue(TopProperty, top);

                if (_parent.View3D)
                {
                    
                    //To plot in 3D
                    #region Bar 3D Plotting
                    //relative width is set here
                    Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["stackedbar"];
                    Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
                    
                    top += (depth + initialDepth);
                    DataPoints[i].SetValue(TopProperty, top);
                    
                    _columnShadows[i].Width = depth;
                    _columnShadows[i].Height = height;
                    _columnShadows[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columnShadows[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + width + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth));

                    _columnShadows[i].Opacity = this.Opacity * DataPoints[i].Opacity;
                    
                    _columnShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);



                    _columnShadows[i].StrokeThickness = DataPoints[i].BorderThickness;

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _columnShadows[i].RenderTransform = st1;

                    _columnTops[i].Width = width;
                    //relative height is set here
                    _columnTops[i].Height = depth;

                    _columnTops[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) - _columnTops[i].Height + (Double)_parent.PlotArea.GetValue(TopProperty));

                    //use4 the same relative width here
                    _columnTops[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + (Double)_parent.PlotArea.GetValue(LeftProperty) - initialDepth);


                    _columnTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columnTops[i].StrokeThickness = DataPoints[i].BorderThickness;


                    _columnTops[i].Opacity = this.Opacity * DataPoints[i].Opacity;
                    
                    SkewTransform st2 = new SkewTransform();
                    st2.AngleY = 0;
                    st2.CenterX = 0;
                    st2.CenterY = 0;
                    st2.AngleX = -45;
                    _columnTops[i].RenderTransform = st2;

                    _columns[i].Width = width;
                    _columns[i].Height = height;
                    _columns[i].SetValue(LeftProperty, left + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth));
                    _columns[i].SetValue(TopProperty, top + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columns[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columns[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _columns[i].Opacity = this.Opacity * DataPoints[i].Opacity;


                    _columns[i].SetValue(ZIndexProperty, (int)(_parent.PlotArea.Height - top) + 10);
                    _columnShadows[i].SetValue(ZIndexProperty, (int)(_parent.PlotArea.Height - top) + 5);
                    _columnTops[i].SetValue(ZIndexProperty, (int)(_parent.PlotArea.Height - top) + 6);

                    //This part of the code generates the 3d gradients
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.5);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";
                            brushTop = Parser.ParseLinearGradient(linbrush);
                            

                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.95);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",1";
                            brushShade = Parser.ParseLinearGradient(linbrush);
                            

                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _columnShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _columns[i].Fill = Cloner.CloneBrush(brush2);

                    //Apply transform to the brushTop

                    _columnTops[i].Fill = Cloner.CloneBrush(brushTop);


                    if (left < _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum))
                    {
                        _shadows[i].Width = width + ShadowSize;
                    }
                    else
                    {
                        _shadows[i].Width = width;
                    }
                    _shadows[i].SetValue(TopProperty, (Double)_columnTops[i].GetValue(TopProperty) - _columnShadows[i].Height * 0.3);
                    _shadows[i].SetValue(LeftProperty, (Double)_columnTops[i].GetValue(LeftProperty));
                    _shadows[i].Height = _columnShadows[i].Height;
                    _shadows[i].Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    _shadows[i].Fill = Parser.ParseSolidColor("#66000000");
                    _shadows[i].SetValue(ZIndexProperty, 3);
                    DataPoints[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) - (depth + initialDepth));
                    if (!ShadowEnabled)
                    {
                        _shadows[i].Opacity = 0;
                    }

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    #endregion Bar 3D Plotting

                    DataPoints[i].AttachToolTip(_columnShadows[i]);
                    DataPoints[i].AttachToolTip(_columns[i]);
                    DataPoints[i].AttachToolTip(_columnTops[i]);
                    DataPoints[i].AttachHref(_columnShadows[i]);
                    DataPoints[i].AttachHref(_columns[i]);

                    DataPoints[i].AttachHref(_columnTops[i]);

                    marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)_columns[i].GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],depth);
                }
                else
                {
                    #region Bar2D
                    Path bar = new Path();
                    Path shadow = new Path();
                    String pathXAML = @"";
                    String maxDP = "", maxDS = "";
                    Double xRadiusLimit = 0;
                    Double yRadiusLimit = 0;
                    if (DataPoints[i].YValue > 0)
                    {
                        GetPositiveMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }
                    else
                    {
                        GetNegativeMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }


                    if (maxDS == this.Name && maxDP == DataPoints[i].Name)
                    {
                        xRadiusLimit = DataPoints[i].RadiusX;
                        yRadiusLimit = DataPoints[i].RadiusY;
                    }

                    if (xRadiusLimit > width) xRadiusLimit = width;
                    if (yRadiusLimit > height / 2) yRadiusLimit = height / 2;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height - yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width - xRadiusLimit, height);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    DataPoints[i].Children.Add(bar);
                    DataPoints[i].Children.Add(shadow);

                    bar.Width = width;
                    bar.Height = height;
                    
                    bar.SetValue(TopProperty, 0);
                    bar.SetValue(LeftProperty, 0);
                    bar.Data = (PathGeometry)XamlReader.Load(pathXAML);



                    shadow.Width = width;
                    shadow.Height = ShadowSize;
                    
                    shadow.SetValue(TopProperty, height);
                    shadow.SetValue(LeftProperty, 0);

                    

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", shadow.Width - xRadiusLimit, 0);



                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", shadow.Width, -yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", shadow.Width, shadow.Height - yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", shadow.Width - xRadiusLimit, shadow.Height);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, shadow.Height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    shadow.Data = (PathGeometry)XamlReader.Load(pathXAML);


                    bar.Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    bar.StrokeThickness = DataPoints[i].BorderThickness;
                    String linbrush;
                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
                    {


                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        bar.Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && Bevel)
                    {
                        if (DataPoints[i].YValue > 0) linbrush = "-90;";
                        else linbrush = "90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor(brush.Color, 0.99);
                        linbrush += ",1";
                        bar.Fill = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        bar.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    bar.SetValue(ZIndexProperty, 5);

                    if (DataPoints[i].YValue < 0)
                    {
                        ScaleTransform st = new ScaleTransform();
                        st.ScaleX = -1;
                        st.ScaleY = 1;
                        bar.RenderTransformOrigin = new Point(0.5, 0.5);
                        bar.RenderTransform = st;

                        st = new ScaleTransform();
                        st.ScaleX = -1;
                        st.ScaleY = 1;
                        shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                        shadow.RenderTransform = st;
                    }
                    shadow.Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    shadow.Fill = Parser.ParseSolidColor("#66000000");
                    shadow.SetValue(ZIndexProperty, 3);
                    if (!ShadowEnabled)
                    {
                        shadow.Opacity = 0;
                    }
                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            bar.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            bar.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }

                    DataPoints[i].AttachToolTip(bar);
                    marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)bar.GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],0);

                    DataPoints[i].AttachHref(bar);
                    if (width > 10 && height > 10)
                        DataPoints[i].ApplyEffects((int)bar.GetValue(ZIndexProperty) + 1);
                    #endregion Bar2D
                }
                
            }

        }

        private Double Minimum(Dictionary<Double, Double> data, int start, int limit)
        {
            Double min = Double.PositiveInfinity;
            for (int i = start; i < limit; i++)
            {
                if (min > data[(Double)i]) min = data[(Double)i];
            }
            return min;
        }
        private Double Maximum(Dictionary<Double, Double> data, int start, int limit)
        {
            Double max = Double.NegativeInfinity;
            for (int i = start; i < limit; i++)
            {
                if (max < data[(Double)i]) max = data[(Double)i];
            }
            return max;
        }

        private int ComparePointY(Point a, Point b)
        {
            return a.Y.CompareTo(b.Y);
        }
        private int ComparePointX(Point a, Point b)
        {
            return a.X.CompareTo(b.X);
        }
        private int CompareRectY(Rect a, Rect b)
        {
            return a.Y.CompareTo(b.Y);
        }
        private int CompareRectX(Rect a, Rect b)
        {
            return a.X.CompareTo(b.X);
        }
        private Double Maximum(Double a, Double b)
        {
            return (a > b ? a : b);
        }
        private Double Minimum(Double a, Double b)
        {
            return (a < b ? a : b);
        }

        private void PositionLabels2(Point plotRadius, Point startPos, Double sum, Double pieRadius)
        {
            Double startAngle = StartAngle, stopAngle;
            Double angle;
            Double radius = 0;
            int i = 0;

            Double relH, relW; //relative height and width of ellipse


            TextBlock _textBlock = new TextBlock();
            Double offset = 1.1;

            Double centerX, centerY;

            Dictionary<Double, Double> labelOR = new Dictionary<Double, Double>();
            Dictionary<Double, Point> labelYR = new Dictionary<Double, Point>();

            Dictionary<Double, Point> labeltempYR = new Dictionary<Double, Point>();
            Dictionary<Double, Rect> labeltempPosR = new Dictionary<Double, Rect>();


            Dictionary<Double, Double> labelOL = new Dictionary<Double, Double>();
            Dictionary<Double, Point> labelYL = new Dictionary<Double, Point>();

            Dictionary<Double, Rect> labelPosL = new Dictionary<Double, Rect>();
            Dictionary<Double, Rect> labelPosR = new Dictionary<Double, Rect>();

            Dictionary<Double, Point> center = new Dictionary<double, Point>();

            double GapL = 2, GapR = 2;
            double maxGap;

            int lIndex = 0, rIndex = 0, tIndex = 0;
            Double tempY, tempX;

            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].YValue / sum;
                angle = (startAngle + stopAngle) / 2;

                if (angle > Math.PI * 2) angle -= Math.PI * 2;

                _textBlock.FontSize = DataPoints[i].LabelFontSize;
                _textBlock.FontFamily = new FontFamily(DataPoints[i].LabelFontFamily);

                _textBlock.FontWeight = Converter.StringToFontWeight(DataPoints[i].LabelFontWeight);
                _textBlock.FontStyle = Converter.StringToFontStyle(DataPoints[i].LabelFontStyle);

                _textBlock.Text = DataPoints[i].TextParser(DataPoints[i].LabelText);


                if (DataPoints[i].LabelStyle.ToLower() == "inside")
                {
                    offset = 0.7;
                    if (sum == DataPoints[i].YValue)
                    {
                        centerX = startPos.X;
                        centerY = startPos.Y;
                    }
                    else
                    {
                        centerX = startPos.X + pieRadius * DataPoints[i].ExplodeOffset * Math.Cos((startAngle + stopAngle) * 0.5);
                        centerY = startPos.Y + pieRadius * DataPoints[i].ExplodeOffset * Math.Sin((startAngle + stopAngle) * 0.5);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    {
                        tempX = centerX + pieRadius * offset * Math.Cos(angle) - _textBlock.ActualWidth / 2;
                        tempY = centerY + pieRadius * offset * Math.Sin(angle) - _textBlock.ActualHeight / 2;
                        DataPoints[i].AttachLabel(tempY, tempX, pieRadius, angle, new Point(centerX, centerY),1);
                    }
                }
                else
                {
                    offset = 30;
                    
                    relH = Math.Pow((pieRadius + offset) * Math.Cos(angle), 2);
                    relW = Math.Pow((pieRadius + offset) * Math.Sin(angle), 2);
                    radius = (pieRadius + offset) * (pieRadius + offset) / Math.Sqrt(relH + relW);
                    if (sum == DataPoints[i].YValue)
                    {
                        centerX = startPos.X;
                        centerY = startPos.Y;
                    }
                    else
                    {
                        centerX = startPos.X + pieRadius * DataPoints[i].ExplodeOffset * Math.Cos((startAngle + stopAngle) * 0.5);
                        centerY = startPos.Y + pieRadius * DataPoints[i].ExplodeOffset * Math.Sin((startAngle + stopAngle) * 0.5);
                    }

                    tempY = startPos.Y + radius * Math.Sin(angle);
                    if (DataPoints[i].ExplodeOffset > 0)
                    {
                        Double maxAngle = (Math.Abs(Math.Sin(angle)) > Math.Abs(Math.Cos(angle))) ? Math.Sin(angle) : Math.Cos(angle);

                        tempX = centerX + radius * maxAngle * (Math.Sign(Math.Cos(angle)));
                    }
                    else
                    {
                        tempX = centerX + radius * Math.Cos(angle);
                    }
                    center.Add(i, new Point(centerX, centerY));

                    if (angle > (Math.PI / 2) && angle < (Math.PI * 3 / 2))
                    {
                        labelYL.Add(lIndex, new Point(i, tempY));
                        labelPosL.Add(lIndex, new Rect(tempX, tempY, pieRadius, angle));
                        lIndex++;
                        if (GapL < _textBlock.ActualHeight) GapL = _textBlock.ActualHeight ;
                    }
                    else if (angle >= 0 && angle <= (Math.PI / 2))
                    {
                        labelYR.Add(rIndex, new Point(i, tempY));
                        labelPosR.Add(rIndex, new Rect(tempX, tempY, pieRadius, angle));
                        rIndex++;
                        if (GapR < _textBlock.ActualHeight) GapR = _textBlock.ActualHeight ;

                    }
                    else
                    {

                        labeltempYR.Add(tIndex, new Point(i, tempY));
                        
                        labeltempPosR.Add(tIndex, new Rect(tempX, tempY, pieRadius, angle));
                        tIndex++;
                        if (GapR < _textBlock.ActualHeight) GapR = _textBlock.ActualHeight;
                    }
                }

                startAngle = stopAngle;
            }
            // Regroup split dictionary
            Point pt;
            Rect rt;

            for (i = 0; i < rIndex; i++)
            {
                labelYR.TryGetValue(i, out pt);
                labelYR.Remove(i);

                labelPosR.TryGetValue(i, out rt);
                labelPosR.Remove(i);

                labeltempYR.Add(tIndex, new Point(pt.X, pt.Y));
                labeltempPosR.Add(tIndex, new Rect(rt.X, rt.Y, rt.Width, rt.Height));
                tIndex++;
            }

            List<Point> tempList1 = new List<Point>();
            tempList1.InsertRange(0, labeltempYR.Values);
            tempList1.Sort(ComparePointY);

            List<Rect> tempList2 = new List<Rect>();
            tempList2.InsertRange(0, labeltempPosR.Values);
            tempList2.Sort(CompareRectY);

           
            for (i = 0, rIndex = 0; i < tIndex; i++)
            {
                labelYR.Add(rIndex, new Point(tempList1[i].X, tempList1[i].Y));
                labelPosR.Add(rIndex, new Rect(tempList2[i].X, tempList2[i].Y, tempList2[i].Width, tempList2[i].Height));
                rIndex++;
            }

            Double maxY, minY;


            maxY = _parent.PlotArea.Height;
            minY = 0;

            // For placing between angles 90 to 270

            maxGap = ((maxY-minY) - (GapL* (labelYL.Count)))/(labelYL.Count);
            Double prevY = maxY, curY;

            int countIterations = 0;
            Boolean isoverlap=false;
            do
            {
                isoverlap = false;
                prevY = maxY;
                for (i = 0; i < lIndex; i++)
                {
                    labelYL.TryGetValue(i, out pt);
                    curY = pt.Y;

                    if (Math.Abs(prevY - curY) < GapL && i!=0)
                    {
                        pt.Y = prevY - ((GapL>maxGap)?maxGap/2:GapL/2);
                        if (pt.Y < minY) pt.Y = (minY + prevY) / 2;
                        curY=pt.Y;
                        labelYL.Remove(i);
                        labelYL.Add(i, new Point(pt.X, pt.Y));

                        labelYL.TryGetValue(i - 1, out pt);
                        pt.Y = prevY + ((GapL > maxGap) ? maxGap / 2 : GapL / 2);
                        if (pt.Y > maxY) pt.Y = (prevY + maxY - GapL)/2;
                        labelYL.Remove(i - 1);
                        labelYL.Add(i - 1, new Point(pt.X, pt.Y));

                        isoverlap = true;
                        break;
                    }
                    
                    prevY = curY;
                }
                countIterations++;

            } while (isoverlap && countIterations<1024);

            if (isoverlap)
            {
                Double stepSize = (maxY - minY) / lIndex;
                for (i = 0; i < lIndex; i++)
                {
                    labelYL.TryGetValue(i, out pt);
                    pt.Y = maxY - stepSize*(i + 1);
                    labelYL.Remove(i);
                    labelYL.Add(i, new Point(pt.X, pt.Y));
                }
            }

            for (i = 0; i < lIndex; i++)
            {
                labelYL.TryGetValue(i, out pt);
                labelYL.Remove(i);
                
                
                
                if (DataPoints[(int)pt.X].LabelEnabled.ToLower() == "true")
                {
                    Double X;
                    labelPosL.TryGetValue(i, out rt);
                    
                    Double tempangle = PointMath.LineSlope(center[(int)pt.X], new Point(rt.X, pt.Y));
                    tempangle = Math.Atan(tempangle);
                    relH = Math.Pow((pieRadius + offset) * Math.Cos(tempangle), 2);
                    relW = Math.Pow((pieRadius + offset) * Math.Sin(tempangle), 2);
                    radius = (pieRadius + offset) * (pieRadius + offset) / Math.Sqrt(relH + relW);
                    X = center[(int)pt.X].X - (radius) * Math.Cos(tempangle);
                    DataPoints[(int)pt.X].AttachLabel(pt.Y, X, rt.Width, rt.Height, center[(int)pt.X], 1);
                    
                    
                }
            }


            maxGap = ((maxY - minY) - (GapR * (labelYR.Count))) / (labelYR.Count);
            prevY = minY;
            countIterations = 0;
            do
            {
                prevY = minY;
                isoverlap = false;

                for (i = 0; i < rIndex; i++)
                {
                    labelYR.TryGetValue(i, out pt);
                    curY = pt.Y;
                    
                    if (Math.Abs(prevY - curY) < GapR && i!=0)
                    {
                        pt.Y = prevY + ((GapR > maxGap) ? maxGap/2 : GapR/2);
                        if (pt.Y > maxY) pt.Y = (prevY + maxY - GapR)/2;
                        curY=pt.Y;
                        labelYR.Remove(i);
                        labelYR.Add(i, new Point(pt.X, pt.Y));

                        labelYR.TryGetValue(i-1, out pt);
                        pt.Y = prevY - ((GapR > maxGap) ? maxGap / 2 : GapR / 2);
                        if (pt.Y < minY) pt.Y = (minY + prevY)/2;
                        labelYR.Remove(i-1);
                        labelYR.Add(i-1, new Point(pt.X, pt.Y));
                        isoverlap = true;
                        break;
                    }
                    
                    prevY = curY;
                }
                countIterations++;

            } while (isoverlap && countIterations < 1024);

            if (isoverlap)
            {
                Double stepSize = (maxY - minY) / rIndex;
                for (i = 0; i < rIndex; i++)
                {
                    labelYR.TryGetValue(i, out pt);
                    pt.Y = stepSize * i;
                    labelYR.Remove(i);
                    labelYR.Add(i, new Point(pt.X, pt.Y));
                }
            }

            for (i = 0; i < rIndex; i++)
            {
                labelYR.TryGetValue(i, out pt);
                labelYR.Remove(i);

                if (DataPoints[(int)pt.X].LabelEnabled.ToLower() == "true")
                {
                    Double X;
                    labelPosR.TryGetValue(i, out rt);
                    
                        Double tempangle = PointMath.LineSlope(center[(int)pt.X], new Point(rt.X, pt.Y));
                        tempangle = Math.Atan(tempangle);
                        relH = Math.Pow((pieRadius + offset) * Math.Cos(tempangle), 2);
                        relW = Math.Pow((pieRadius + offset) * Math.Sin(tempangle), 2);
                        radius = (pieRadius + offset) * (pieRadius + offset) / Math.Sqrt(relH + relW);
                        X = center[(int)pt.X].X + (radius) * Math.Cos(tempangle);
                        DataPoints[(int)pt.X].AttachLabel(pt.Y, X, rt.Width, rt.Height, center[(int)pt.X], 1);
                    
                }
            }
  
        }

        private void PositionLabels3D(Point plotRadius, Point startPos, Point pieRadius, Double sum)
        {
            Double startAngle = StartAngle, stopAngle;
            Double angle;
            Double radius = 0;
            int i = 0;

            

            Double depth = _parent.Height * 0.075;
            TextBlock _textBlock = new TextBlock();

            Double centerX, centerY;

            Double offset=0;
            Dictionary<Double, Double> labelOR = new Dictionary<Double, Double>();
            Dictionary<Double, Point> labelYR = new Dictionary<Double, Point>();

            Dictionary<Double, Point> labeltempYR = new Dictionary<Double, Point>();
            Dictionary<Double, Rect> labeltempPosR = new Dictionary<Double, Rect>();


            Dictionary<Double, Double> labelOL = new Dictionary<Double, Double>();
            Dictionary<Double, Point> labelYL = new Dictionary<Double, Point>();

            Dictionary<Double, Rect> labelPosL = new Dictionary<Double, Rect>();
            Dictionary<Double, Rect> labelPosR = new Dictionary<Double, Rect>();

            Dictionary<Double, Point> center = new Dictionary<double, Point>();

            double GapL = 2, GapR = 2;
            double maxGap;

            int lIndex = 0, rIndex = 0, tIndex = 0;
            Double tempY, tempX;
            Double pieradius;
            Double yScalingFactor = pieRadius.Y / pieRadius.X;
            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].YValue / sum;
                angle = (startAngle + stopAngle) / 2;

                if (angle > Math.PI * 2) angle -= Math.PI * 2;

                _textBlock.FontSize = DataPoints[i].LabelFontSize;
                _textBlock.FontFamily = new FontFamily(DataPoints[i].LabelFontFamily);

                _textBlock.FontWeight = Converter.StringToFontWeight(DataPoints[i].LabelFontWeight);
                _textBlock.FontStyle = Converter.StringToFontStyle(DataPoints[i].LabelFontStyle);
                _textBlock.Text = DataPoints[i].TextParser(DataPoints[i].LabelText);

                
                pieradius = pieRadius.X;
                radius = plotRadius.X;
                if (DataPoints[i].LabelStyle.ToLower() == "inside")
                {

                    

                    centerX = startPos.X + pieradius * DataPoints[i].ExplodeOffset * Math.Cos(angle);
                    centerY = startPos.Y + pieradius * DataPoints[i].ExplodeOffset * Math.Sin(angle) * yScalingFactor;


                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    {
                        tempX = centerX + pieradius * 0.7 * Math.Cos(angle) - _textBlock.ActualWidth / 2;
                        tempY = (centerY - depth / 2) + pieradius * 0.7 * Math.Sin(angle) * yScalingFactor - _textBlock.ActualHeight / 2;
                        DataPoints[i].AttachLabel(tempY, tempX, pieradius, angle, new Point(centerX, centerY),yScalingFactor);
                    }
                }
                else
                {
                    offset = 30;
                    
                   


                    centerX = startPos.X + pieradius * DataPoints[i].ExplodeOffset * Math.Cos(angle);
                    centerY = startPos.Y + pieradius * DataPoints[i].ExplodeOffset * Math.Sin(angle)*yScalingFactor;

                    center.Add(i, new Point(centerX, centerY));
                    tempY = startPos.Y + (radius + offset) * Math.Sin(angle)*yScalingFactor;
                    
                    tempX = centerX + (radius + offset) * Math.Cos(angle);
                    
                    if (angle > (Math.PI / 2) && angle < (Math.PI * 3 / 2))
                    {
                        labelYL.Add(lIndex, new Point(i, tempY));
                        labelPosL.Add(lIndex, new Rect(tempX, tempY, pieradius, angle));
                        lIndex++;
                        if (GapL < _textBlock.ActualHeight) GapL = _textBlock.ActualHeight ;
                    }
                    else if (angle >= 0 && angle <= (Math.PI / 2))
                    {
                        labelYR.Add(rIndex, new Point(i, tempY));
                        labelPosR.Add(rIndex, new Rect(tempX, tempY, pieradius, angle));
                        rIndex++;
                        if (GapR < _textBlock.ActualHeight) GapR = _textBlock.ActualHeight;

                    }
                    else
                    {
                        labeltempYR.Add(tIndex, new Point(i, tempY));
                        labeltempPosR.Add(tIndex, new Rect(tempX, tempY, pieradius, angle));
                        tIndex++;
                        if (GapR < _textBlock.ActualHeight) GapR = _textBlock.ActualHeight ;
                    }
                }

                startAngle = stopAngle;
            }
            // Regroup split dictionary
            Point pt;
            Rect rt;

            for (i = 0; i < rIndex; i++)
            {
                labelYR.TryGetValue(i, out pt);
                labelYR.Remove(i);

                labelPosR.TryGetValue(i, out rt);
                labelPosR.Remove(i);

                labeltempYR.Add(tIndex, new Point(pt.X, pt.Y));
                labeltempPosR.Add(tIndex, new Rect(rt.X, rt.Y, rt.Width, rt.Height));
                tIndex++;
            }

            List<Point> tempList1 = new List<Point>();
            tempList1.InsertRange(0, labeltempYR.Values);
            tempList1.Sort(ComparePointY);

            List<Rect> tempList2 = new List<Rect>();
            tempList2.InsertRange(0, labeltempPosR.Values);
            tempList2.Sort(CompareRectY);

            for (i = 0, rIndex = 0; i < tIndex; i++)
            {
                labelYR.Add(rIndex, new Point(tempList1[i].X, tempList1[i].Y));
                labelPosR.Add(rIndex, new Rect(tempList2[i].X, tempList2[i].Y, tempList2[i].Width, tempList2[i].Height));
                rIndex++;
            }

            Double maxY, minY;


            maxY = (Double)_parent.PlotArea.GetValue(TopProperty) + _parent.PlotArea.Height;
            minY = (Double)_parent.PlotArea.GetValue(TopProperty);

            // For placing between angles 90 to 270

            maxGap = ((maxY - minY) - (GapL * (labelYL.Count))) / (labelYL.Count);
            Double prevY = maxY, curY;

            int countIterations = 0;
            Boolean isoverlap = false;
            do
            {
                isoverlap = false;
                prevY = maxY;
                for (i = 0; i < lIndex; i++)
                {
                    labelYL.TryGetValue(i, out pt);
                    curY = pt.Y;

                    if (Math.Abs(prevY - curY) < GapL && i != 0)
                    {
                        pt.Y = prevY - ((GapL > maxGap) ? maxGap / 2 : GapL / 2);
                        curY = pt.Y;
                        labelYL.Remove(i);
                        labelYL.Add(i, new Point(pt.X, pt.Y));

                        labelYL.TryGetValue(i - 1, out pt);
                        pt.Y = prevY + ((GapL > maxGap) ? maxGap / 2 : GapL / 2);
                        if (pt.Y > maxY) pt.Y = maxY - GapL;
                        labelYL.Remove(i - 1);
                        labelYL.Add(i - 1, new Point(pt.X, pt.Y));

                        isoverlap = true;
                        break;
                    }

                    prevY = curY;
                }
                countIterations++;

            } while (isoverlap && countIterations < 128);


            for (i = 0; i < lIndex; i++)
            {
                labelYL.TryGetValue(i, out pt);
                labelYL.Remove(i);
                


                if (DataPoints[(int)pt.X].LabelEnabled.ToLower() == "true")
                {
                    labelPosL.TryGetValue(i, out rt);
                    DataPoints[(int)pt.X].AttachLabel(pt.Y, rt.X, rt.Width, rt.Height, center[(int)pt.X],yScalingFactor);
                }
            }


            maxGap = ((maxY - minY) - (GapR * (labelYR.Count))) / (labelYR.Count);
            prevY = minY;
            countIterations = 0;
            do
            {
                prevY = minY;
                isoverlap = false;

                for (i = 0; i < rIndex; i++)
                {
                    labelYR.TryGetValue(i, out pt);
                    curY = pt.Y;

                    if (Math.Abs(prevY - curY) < GapR && i != 0)
                    {
                        pt.Y = prevY + ((GapR > maxGap) ? maxGap / 2 : GapR / 2);
                        if (pt.Y > maxY) pt.Y = maxY - GapR;
                        curY = pt.Y;
                        labelYR.Remove(i);
                        labelYR.Add(i, new Point(pt.X, pt.Y));

                        labelYR.TryGetValue(i - 1, out pt);
                        pt.Y = prevY - ((GapR > maxGap) ? maxGap / 2 : GapR / 2);
                        labelYR.Remove(i - 1);
                        labelYR.Add(i - 1, new Point(pt.X, pt.Y));
                        isoverlap = true;
                        break;
                    }

                    prevY = curY;
                }
                countIterations++;

            } while (isoverlap && countIterations < 128);


            for (i = 0; i < rIndex; i++)
            {
                labelYR.TryGetValue(i, out pt);
                labelYR.Remove(i);
                
                if (DataPoints[(int)pt.X].LabelEnabled.ToLower() == "true")
                {
                    labelPosR.TryGetValue(i, out rt);
                    
                    DataPoints[(int)pt.X].AttachLabel(pt.Y, rt.X, rt.Width, rt.Height, center[(int)pt.X],yScalingFactor);
                }
            }
  

        }


        private void GetPositiveMax(Double xValue,ref String maxDP,ref String maxDS)
        {
            maxDP = "";
            maxDS = "";
            foreach (DataSeries ds in _parent.DataSeries)
            {
                if (ds.RenderAs == this.RenderAs)
                {
                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        if (xValue == dp.XValue)
                        {
                            if (dp.YValue > 0)
                            {
                                maxDS = ds.Name;
                                maxDP = dp.Name;
                            }
                        }
                    }
                }
            }
            
        }

        private void GetNegativeMax(Double xValue, ref String maxDP, ref String maxDS)
        {
            maxDP = "";
            maxDS = "";
            foreach (DataSeries ds in _parent.DataSeries)
            {
                if (ds.RenderAs == this.RenderAs)
                {
                    foreach (DataPoint dp in ds.DataPoints)
                    {
                        if (xValue == dp.XValue)
                        {
                            if (dp.YValue <= 0)
                            {
                                maxDS = ds.Name;
                                maxDP = dp.Name;
                            }
                        }
                    }
                }
            }

        }

        private Rect GetBoundingBox(params Point[] Points)
        {
            Double x1=Double.MaxValue, y1=Double.MaxValue;
            Double x2=Double.MinValue, y2=Double.MinValue;
            for (int i = 0; i < Points.Length; i++)
            {
                if (Points[i].X < x1) x1 = Points[i].X;
                if (Points[i].Y < y1) y1 = Points[i].Y;
                if (Points[i].X > x2) x2 = Points[i].X;
                if (Points[i].Y > y2) y2 = Points[i].Y;

            }
            return new Rect(x1, y1, Math.Abs(x2 - x1), Math.Abs(y2 - y1));

        }

        private void PlotPieSingleton(int id,Point startPos ,Double radius)
        {
            String pathXAML;
            
            pathXAML = @"<GeometryGroup FillRule=""EvenOdd"" xmlns=""http://schemas.microsoft.com/client/2007"">";
            pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos,radius,radius);
            pathXAML += "</GeometryGroup>";

            _pies[id].Data = (GeometryGroup)XamlReader.Load(pathXAML);


            _pies[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            
            switch (DataPoints[id].BorderStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    
                    break;

                case "Dotted":
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    
                    break;
            }

            if (LightingEnabled && DataPoints[id].Background.GetType().Name == "SolidColorBrush")
            {
                String gradient;
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                _pies[id].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity).ToString());

                Ellipse lighting = new Ellipse();
                lighting.Width = radius * 2;
                lighting.Height = radius * 2;
                lighting.SetValue(LeftProperty, startPos.X - radius);
                lighting.SetValue(TopProperty, startPos.Y - radius);
                gradient = "0.5;0.5;";
                gradient += "#00000000,0;";
                gradient += "#22000000,0.7;";
                gradient += "#7F000000,1;";

                lighting.Fill = Parser.ParseColor(gradient);
                lighting.IsHitTestVisible = false;
                DataPoints[id].Children.Add(lighting);
            }
            else
            {
                _pies[id].Fill = Cloner.CloneBrush(DataPoints[id].Background);
            }
            if (Bevel)
            {
                pathXAML = @"<GeometryGroup FillRule=""EvenOdd"" xmlns=""http://schemas.microsoft.com/client/2007"">";
                pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius, radius);
                pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius-4, radius-4);
                pathXAML += "</GeometryGroup>";

                String gradient = "-90;";
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                gradient += Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",0;";
                gradient += Parser.GetDarkerColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",1;";

                Path bevel = new Path();
                bevel.Data = (GeometryGroup)XamlReader.Load(pathXAML);
                bevel.Fill = Parser.ParseColor(gradient);
                bevel.IsHitTestVisible = false;
                DataPoints[id].Children.Add(bevel);
                bevel.SetValue(ZIndexProperty, 1);
            }
            DataPoints[id].AttachToolTip(_pies[id]);
            
            DataPoints[id].AttachHref(_pies[id]);
        }
        private void PlotDoughnutSingleton(int id, Point startPos, Double radius)
        {
            String pathXAML;

            pathXAML = @"<GeometryGroup FillRule=""EvenOdd"" xmlns=""http://schemas.microsoft.com/client/2007"">";
            pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius, radius);
            pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius/2, radius/2);
            pathXAML += "</GeometryGroup>";

            _doughnut[id].Data = (GeometryGroup)XamlReader.Load(pathXAML);
            _doughnut[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            switch (DataPoints[id].BorderStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    break;

                case "Dotted":
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    break;
            }

            if (LightingEnabled && DataPoints[id].Background.GetType().Name == "SolidColorBrush")
            {
                String gradient;
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                _doughnut[id].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity).ToString());

                Ellipse lighting = new Ellipse();
                lighting.Width = radius * 2;
                lighting.Height = radius * 2;
                lighting.SetValue(LeftProperty, startPos.X - radius);
                lighting.SetValue(TopProperty, startPos.Y - radius);
                gradient = "0.5;0.5;";
                gradient += "#00000000,0;";
                gradient += "#22000000,0.7;";
                gradient += "#7F000000,1;";

                lighting.Fill = Parser.ParseColor(gradient);
                lighting.IsHitTestVisible = false;
                DataPoints[id].Children.Add(lighting);
            }
            else
            {
                _doughnut[id].Fill = Cloner.CloneBrush(DataPoints[id].Background);
            }
            if (Bevel)
            {
                pathXAML = @"<GeometryGroup FillRule=""EvenOdd"" xmlns=""http://schemas.microsoft.com/client/2007"">";
                pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius, radius);
                pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius - 4, radius - 4);
                pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius/2 + 4, radius/2 + 4);
                pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, radius/2, radius/2);
                pathXAML += "</GeometryGroup>";

                String gradient = "-90;";
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                gradient += Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",0;";
                gradient += Parser.GetDarkerColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",1;";

                Path bevel = new Path();
                bevel.Data = (GeometryGroup)XamlReader.Load(pathXAML);
                bevel.Fill = Parser.ParseColor(gradient);
                bevel.IsHitTestVisible = false;
                DataPoints[id].Children.Add(bevel);
                bevel.SetValue(ZIndexProperty, 1);
            }
            DataPoints[id].AttachToolTip(_doughnut[id]);
            
            DataPoints[id].AttachHref(_doughnut[id]);
        }
        private void PlotPieSingleton3D(int id, Point startPos, Point pieRadius)
        {
            String pathXAML;

            pathXAML = @"<GeometryGroup FillRule=""EvenOdd"" xmlns=""http://schemas.microsoft.com/client/2007"">";
            pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", startPos, pieRadius.X, pieRadius.Y);
            pathXAML += "</GeometryGroup>";

            _pies[id].Data = (GeometryGroup)XamlReader.Load(pathXAML);

            Double sec1StartAngle = Math.PI * 0.01;
            Double sec1StopAngle = Math.PI;
            Point arcStart = new Point();
            Point arcEnd = new Point();
            Double yScalingFactor = pieRadius.Y / pieRadius.X;
            Double radius = pieRadius.X;
            Double depth = _parent.Height * 0.075;
            arcStart.X = startPos.X + radius * Math.Cos(sec1StartAngle);
            arcStart.Y = startPos.Y + radius * Math.Sin(sec1StartAngle) * yScalingFactor;

            arcEnd.X = startPos.X + radius * Math.Cos(sec1StopAngle);
            arcEnd.Y = startPos.Y + radius * Math.Sin(sec1StopAngle) * yScalingFactor;

            pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
            pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
            pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
            pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
            pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", pieRadius.X,pieRadius.Y );
            pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
            pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
            pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
            pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", pieRadius.X, pieRadius.Y);
            pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
            pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
            pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
            _pieSides[id].Data = (PathGeometry)XamlReader.Load(pathXAML);

            _pies[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            _pieSides[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            switch (DataPoints[id].BorderStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    _pieSides[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    break;

                case "Dotted":
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    _pieSides[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    break;
            }
            DataPoints[id].AttachToolTip(_pies[id]);
            
            DataPoints[id].AttachHref(_pies[id]);
            DataPoints[id].AttachToolTip(_pieSides[id]);
            
            DataPoints[id].AttachHref(_pieSides[id]);

            Brush tempBrush = DataPoints[id].Background;
            Brush brushPie, brushSide;
            if (tempBrush.GetType().Name == "LinearGradientBrush")
            {
                LinearGradientBrush brush = DataPoints[id].Background as LinearGradientBrush;
                brushPie = Cloner.CloneBrush(DataPoints[id].Background);

                brushSide = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");
                Parser.GenerateDarkerGradientBrush(brush, brushSide as LinearGradientBrush, 0.75);
                RotateTransform rt = new RotateTransform();
                rt.Angle = -45;
                brushSide.RelativeTransform = rt;

            }
            else if (tempBrush.GetType().Name == "RadialGradientBrush")
            {
                RadialGradientBrush brush = DataPoints[id].Background as RadialGradientBrush;
                brushPie = Cloner.CloneBrush(DataPoints[id].Background);

                brushSide = new RadialGradientBrush();

                (brushSide as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                Parser.GenerateDarkerGradientBrush(brush, brushSide as RadialGradientBrush, 0.75);
            }
            else if (tempBrush.GetType().Name == "SolidColorBrush")
            {

                SolidColorBrush brush = DataPoints[id].Background as SolidColorBrush;
                if (LightingEnabled)
                {
                    String linbrush;
                    Double r, g, b;

                    linbrush = "0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                    linbrush += ",0;";
                    r = ((Double)(brush as SolidColorBrush).Color.R / 255) * 0.6;
                    g = ((Double)(brush as SolidColorBrush).Color.G / 255) * 0.6;
                    b = ((Double)(brush as SolidColorBrush).Color.B / 255) * 0.6;
                    linbrush += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                    linbrush += ",1";

                    brushSide = Parser.ParseLinearGradient(linbrush);

                    linbrush = "0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                    linbrush += ",0;";
                    linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                    linbrush += ",1";
                    brushPie = Parser.ParseLinearGradient(linbrush);

                }
                else
                {
                    brushPie = Cloner.CloneBrush(brush);
                    brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                }
            }
            else
            {
                brushPie = Cloner.CloneBrush(DataPoints[id].Background);
                brushSide = Cloner.CloneBrush(DataPoints[id].Background);
            }
            _pies[id].Fill = brushPie;
            _pieSides[id].Fill = brushSide;


        }
        private void PlotDoughnutSingleton3D(int id, Point StartPos, Point pieRadius)
        {
            String pathXAML;

            pathXAML = @"<GeometryGroup FillRule=""EvenOdd"" xmlns=""http://schemas.microsoft.com/client/2007"">";
            pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", StartPos, pieRadius.X, pieRadius.Y);
            pathXAML += String.Format(@"<EllipseGeometry Center=""{0}"" RadiusX=""{1}"" RadiusY=""{2}"" />", StartPos, pieRadius.X/2, pieRadius.Y/2);
            pathXAML += "</GeometryGroup>";

            _doughnut[id].Data = (GeometryGroup)XamlReader.Load(pathXAML);

            Double secStartAngle = Math.PI * 0.01;
            Double secStopAngle = Math.PI;
            Point arcStart = new Point();
            Point arcEnd = new Point();
            Double yScalingFactor = pieRadius.Y / pieRadius.X;
            Double radius = pieRadius.X;
            Double depth = _parent.Height * 0.075;
            arcStart.X = StartPos.X + radius * Math.Cos(secStartAngle);
            arcStart.Y = StartPos.Y + radius * Math.Sin(secStartAngle) * yScalingFactor;

            arcEnd.X = StartPos.X + radius * Math.Cos(secStopAngle);
            arcEnd.Y = StartPos.Y + radius * Math.Sin(secStopAngle) * yScalingFactor;

            pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
            pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
            pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
            pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
            pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", pieRadius.X, pieRadius.Y);
            pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
            pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
            pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
            pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", pieRadius.X, pieRadius.Y);
            pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
            pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
            pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
            _pieSides[id].Data = (PathGeometry)XamlReader.Load(pathXAML);

            secStartAngle = Math.PI * 1.01;
            secStopAngle = Math.PI * 2;

            arcStart.X = StartPos.X + radius/2 * Math.Cos(secStartAngle);
            arcStart.Y = StartPos.Y + radius/2 * Math.Sin(secStartAngle) * yScalingFactor;

            arcEnd.X = StartPos.X + radius/2 * Math.Cos(secStopAngle);
            arcEnd.Y = StartPos.Y + radius/2 * Math.Sin(secStopAngle) * yScalingFactor;

            pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
            pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
            pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
            pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
            pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", pieRadius.X/2, pieRadius.Y/2);
            pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
            pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
            pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
            pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", pieRadius.X/2, pieRadius.Y/2);
            pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
            pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
            pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
            _pies[id].Data = (PathGeometry)XamlReader.Load(pathXAML);

            _doughnut[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            _pieSides[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            _pies[id].Stroke = Cloner.CloneBrush(DataPoints[id].BorderColor);
            switch (DataPoints[id].BorderStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    _doughnut[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    _pieSides[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    break;

                case "Dotted":
                    _doughnut[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    _pieSides[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    _pies[id].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    break;
            }
            DataPoints[id].AttachToolTip(_doughnut[id]);
            
            DataPoints[id].AttachHref(_doughnut[id]);
            DataPoints[id].AttachToolTip(_pieSides[id]);
            
            DataPoints[id].AttachHref(_pieSides[id]);
            DataPoints[id].AttachToolTip(_pies[id]);
            
            DataPoints[id].AttachHref(_pies[id]);

            Brush tempBrush = DataPoints[id].Background;
            Brush brushPie, brushSide;
            if (tempBrush.GetType().Name == "LinearGradientBrush")
            {
                LinearGradientBrush brush = DataPoints[id].Background as LinearGradientBrush;
                brushPie = Cloner.CloneBrush(DataPoints[id].Background);

                brushSide = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");
                Parser.GenerateDarkerGradientBrush(brush, brushSide as LinearGradientBrush, 0.75);
                RotateTransform rt = new RotateTransform();
                rt.Angle = -45;
                brushSide.RelativeTransform = rt;

            }
            else if (tempBrush.GetType().Name == "RadialGradientBrush")
            {
                RadialGradientBrush brush = DataPoints[id].Background as RadialGradientBrush;
                brushPie = Cloner.CloneBrush(DataPoints[id].Background);

                brushSide = new RadialGradientBrush();

                (brushSide as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                Parser.GenerateDarkerGradientBrush(brush, brushSide as RadialGradientBrush, 0.75);
            }
            else if (tempBrush.GetType().Name == "SolidColorBrush")
            {

                SolidColorBrush brush = DataPoints[id].Background as SolidColorBrush;
                if (LightingEnabled)
                {
                    String linbrush;
                    Double r, g, b;

                    linbrush = "0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                    linbrush += ",0;";
                    r = ((Double)(brush as SolidColorBrush).Color.R / 255) * 0.6;
                    g = ((Double)(brush as SolidColorBrush).Color.G / 255) * 0.6;
                    b = ((Double)(brush as SolidColorBrush).Color.B / 255) * 0.6;
                    linbrush += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                    linbrush += ",1";

                    brushSide = Parser.ParseLinearGradient(linbrush);

                    linbrush = "0;";
                    linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                    linbrush += ",0;";
                    linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                    linbrush += ",1";
                    brushPie = Parser.ParseLinearGradient(linbrush);

                }
                else
                {
                    brushPie = Cloner.CloneBrush(brush);
                    brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                }
            }
            else
            {
                brushPie = Cloner.CloneBrush(DataPoints[id].Background);
                brushSide = Cloner.CloneBrush(DataPoints[id].Background);
            }
            _doughnut[id].Fill = brushPie;
            _pieSides[id].Fill = brushSide;
            _pies[id].Fill = brushSide;

        }

        private void PlotPie()
        {
            Double sum = 0;
            Double plotSide1, plotSide2;

            Point startPos = new Point();

            Point end1 = new Point();

            Point end2 = new Point();

            Double startAngle, stopAngle;
            Double radius;

            String pathXAML;
            int i;
            Boolean labelFlag = false, labelOutsideFlag = false;
            Double maxExplodeValue = 0;

            Double MaxLabelWidth = 0;
            Double MaxLabelHeight = 0;
            TextBlock _textBlock = new TextBlock();
            foreach (DataPoint dp in DataPoints)
            {
                if (dp.YValue == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Zero valued DataPoints area not supported");
                    throw (new Exception("Zero valued DataPoints area not supported"));
                    
                }
                sum += dp.YValue;
                
                _textBlock.FontSize = dp.LabelFontSize;
                _textBlock.FontFamily = new FontFamily(dp.LabelFontFamily);
                _textBlock.FontWeight = Converter.StringToFontWeight(dp.LabelFontWeight);
                _textBlock.Text = dp.TextParser(dp.LabelText);

                if (MaxLabelHeight < _textBlock.ActualHeight) MaxLabelHeight = _textBlock.ActualHeight;
                if (MaxLabelWidth < _textBlock.ActualWidth) MaxLabelWidth = _textBlock.ActualWidth;

                if (dp.LabelEnabled.ToLower() == "true")
                {
                    labelFlag = true;
                    if (dp.LabelStyle.ToLower() == "outside")
                        labelOutsideFlag = true;
                }
                if (dp.ExplodeOffset < 0 || dp.ExplodeOffset > 1)
                {
                    Debug.WriteLine("Value of ExplodeOffset must be between 0 and 1");
                    throw (new Exception("Value of ExplodeOffset must be betwee 0 and 1"));
                }
                if (maxExplodeValue < dp.ExplodeOffset) maxExplodeValue = dp.ExplodeOffset;

            }

            // sum for additional uses like in Tool tip
            _sum = sum;
            if (_sum <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Pie chart requires at least one entry other than zero");
                throw (new Exception("Pie chart requires at least one entry other than zero"));
                
            }
            plotSide1 = _parent.PlotArea.Width - 2 * _parent.PlotArea.BorderThickness;
            plotSide2 = _parent.PlotArea.Height - 2 * _parent.PlotArea.BorderThickness;

            plotSide1 -= MaxLabelWidth;
            plotSide2 -= MaxLabelHeight;

            startPos.X = plotSide1 / 2 + _parent.PlotArea.BorderThickness + MaxLabelWidth/2;
            startPos.Y = plotSide2 / 2 + _parent.PlotArea.BorderThickness + MaxLabelHeight/2;


            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;

            if (maxExplodeValue > 0 && maxExplodeValue + 0.05 <= 1)
                maxExplodeValue += 0.05;

            radiusScaling = 1 - 0.5 * maxExplodeValue;
            centerOffsetFactor = 1 - radiusScaling;

            if (labelFlag)
            {
                
                radius = Math.Min(plotSide1 / 2, plotSide2 / 2);


                if (labelOutsideFlag && maxExplodeValue <= 0)
                {
                    radius *= 0.7 * radiusScaling;
                    PositionLabels2(new Point(plotSide1 * radiusScaling*0.8/2 , plotSide1 * radiusScaling*0.8/2 ), startPos, sum, radius);
                }

                else
                {
                    radius *= radiusScaling;
                    //This is done so that the pie doesn't touch the chart border if padding is not given
                    if (_parent.Padding <= 4) radius -= 5;
                    else radius -= _parent.Padding;

                    PositionLabels2(new Point(plotSide1 * radiusScaling/2, plotSide1 * radiusScaling/2), startPos, sum, radius);
                }

            }
            else
            {
                if (plotSide1 > plotSide2)
                    plotSide1 = plotSide2;

                plotSide1 /= 2;

                radius = plotSide1 * radiusScaling;
                //This is done so that the pie doesn't touch the chart border if padding is not given
                if (_parent.Padding <= 4) radius -= 5;
                else radius -= _parent.Padding;
            }
            startAngle = StartAngle;


            Double centerX, centerY;
            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].YValue / sum;

                if (DataPoints[i].YValue == sum)
                {
                    PlotPieSingleton(i,startPos,radius);
                    continue;
                }

                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                centerOffset = centerOffsetFactor * radius;

                centerX = startPos.X + centerOffset * Math.Cos((startAngle + stopAngle) * 0.5);
                centerY = startPos.Y + centerOffset * Math.Sin((startAngle + stopAngle) * 0.5);

                end1.X = centerX + radius * Math.Cos(startAngle);
                end1.Y = centerY + radius * Math.Sin(startAngle);

                end2.X = centerX + radius * Math.Cos(stopAngle);
                end2.Y = centerY + radius * Math.Sin(stopAngle);


                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", centerX, centerY);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y);

                if (stopAngle - startAngle >= Math.PI)
                    pathXAML += String.Format(@"IsLargeArc=""true"" ");
                pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", radius, radius);
                pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY);
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _pies[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                _pies[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);

                _pies[i].StrokeThickness = DataPoints[i].BorderThickness;

                DataPoints[i].AttachToolTip(_pies[i]);
                DataPoints[i].AttachHref(_pies[i]);

                switch (DataPoints[i].BorderStyle)
                {
                    case "Solid":
                        break;

                    case "Dashed":
                        _pies[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        break;

                    case "Dotted":
                        _pies[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        break;
                }
                if (LightingEnabled && DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                {
                    String gradient ;
                    Double intensity;
                    

                    intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                    _pies[i].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, intensity).ToString());

                    Ellipse lighting = new Ellipse();
                    lighting.Width = radius * 2;
                    lighting.Height = radius * 2;
                    lighting.SetValue(LeftProperty, centerX - radius);
                    lighting.SetValue(TopProperty, centerY - radius);
                    gradient = "0.5;0.5;";
                    gradient += "#00000000,0;";
                    gradient += "#22000000,0.7;";
                    gradient += "#7F000000,1;";

                    lighting.Fill = Parser.ParseColor(gradient);
                    lighting.IsHitTestVisible = false;
                    DataPoints[i].Children.Add(lighting);
                    DataPoints[i].Clip = (PathGeometry)XamlReader.Load(pathXAML);
                }
                else
                {
                    _pies[i].Fill = Cloner.CloneBrush(DataPoints[i].Background);
                }

                

                if (Bevel)
                {
                    Point bevelCenter = new Point();
                    Point bevelEnd1 = new Point();
                    Point bevelEnd2= new Point();
                    int bevelLength = 4;
                    Double bevelRadius = (radius - 2 * bevelLength);
                    bevelCenter.X = centerX + bevelLength * Math.Cos((startAngle + stopAngle) * 0.5);
                    bevelCenter.Y = centerY + bevelLength * Math.Sin((startAngle + stopAngle) * 0.5);

                    bevelEnd1.X = centerX + (radius -  bevelLength) * Math.Cos(startAngle + 0.03);
                    bevelEnd1.Y = centerY + (radius -  bevelLength) * Math.Sin(startAngle + 0.03);

                    bevelEnd2.X = centerX + (radius -  bevelLength) * Math.Cos(stopAngle - 0.03);
                    bevelEnd2.Y = centerY + (radius -  bevelLength) * Math.Sin(stopAngle - 0.03);

                    Double tempangle = (startAngle + stopAngle) * 0.5 * 180/Math.PI;
                    
                    List<Point> bevelPoints = new List<Point>();
                    bevelPoints.Add(new Point(centerX, centerY));
                    bevelPoints.Add(end1);
                    bevelPoints.Add(bevelEnd1);
                    bevelPoints.Add(bevelCenter);
                    if (startAngle > Math.PI *0.5 && startAngle <= Math.PI *1.5)
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", startAngle*180/Math.PI +135, (int)_pies[i].GetValue(ZIndexProperty) + 3);
                    }
                    else 
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", -startAngle * 180 / Math.PI, (int)_pies[i].GetValue(ZIndexProperty) + 3);
                    }
                    
                    bevelPoints.Clear();
                    bevelPoints.Add(new Point(centerX, centerY));
                    bevelPoints.Add(end2);
                    bevelPoints.Add(bevelEnd2);
                    bevelPoints.Add(bevelCenter);
                    if (stopAngle > Math.PI * 0.5 && stopAngle <= Math.PI * 1.5)
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", stopAngle * 180 / Math.PI + 135, (int)_pies[i].GetValue(ZIndexProperty) + 3);
                    }
                    else
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", -stopAngle * 180 / Math.PI, (int)_pies[i].GetValue(ZIndexProperty) + 3);
                    }

                    bevelPoints.Clear();

                    Path _bevel = new Path();
                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", bevelEnd2.X, bevelEnd2.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y);
                    if (stopAngle - startAngle >= Math.PI || startAngle>stopAngle)
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", radius, radius);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", bevelEnd1.X, bevelEnd1.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", bevelEnd2.X, bevelEnd2.Y);
                    if (stopAngle - startAngle >= Math.PI || startAngle > stopAngle)
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", (radius - bevelLength), (radius - bevelLength));
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    _bevel.Data = (PathGeometry)XamlReader.Load(pathXAML);

                    String linGrad;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                    {
                        if (tempangle - StartAngle * 180 / Math.PI > 0 && tempangle - StartAngle * 180 / Math.PI < 180)
                        {
                            linGrad = (tempangle + 90) + ";";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color,0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.85) + ",1";
                        }
                        else
                        {
                            linGrad = (tempangle + 90) + ";";
                            linGrad += Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.99) + ",1";
                        }
                        _bevel.Fill = Parser.ParseLinearGradient(linGrad);

                    }
                    else 
                    {
                        _bevel.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    
                    _bevel.SetValue(ZIndexProperty, (int)_pies[i].GetValue(ZIndexProperty) + 3);

                    DataPoints[i].Children.Add(_bevel);

                }
                
                startAngle = stopAngle;
            }

        }

        private void PlotDoughnut()
        {
            Double sum = 0;
            Double plotSide1, plotSide2;

            Point startPos = new Point();

            Point end1 = new Point();

            Point end2 = new Point();

            Point end3 = new Point();

            Point end4 = new Point();

            Double startAngle, stopAngle;
            Double radius;

            String pathXAML;
            int i;
            Boolean labelFlag = false, labelOutsideFlag = false;
            Double maxExplodeValue = 0;

            Double MaxLabelWidth = 0;
            Double MaxLabelHeight = 0;
            TextBlock _textBlock = new TextBlock();
            foreach (DataPoint dp in DataPoints)
            {
                if (dp.YValue == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Zero valued DataPoints area not supported");
                    throw (new Exception("Zero valued DataPoints area not supported"));
                    
                }
                sum += dp.YValue;

                _textBlock.FontSize = dp.LabelFontSize;
                _textBlock.FontFamily = new FontFamily(dp.LabelFontFamily);
                _textBlock.FontWeight = Converter.StringToFontWeight(dp.LabelFontWeight);
                _textBlock.Text = dp.TextParser(dp.LabelText);

                if (MaxLabelHeight < _textBlock.ActualHeight) MaxLabelHeight = _textBlock.ActualHeight;
                if (MaxLabelWidth < _textBlock.ActualWidth) MaxLabelWidth = _textBlock.ActualWidth;

                if (dp.LabelEnabled.ToLower() == "true")
                {
                    labelFlag = true;
                    if (dp.LabelStyle.ToLower() == "outside")
                        labelOutsideFlag = true;
                }
                if (dp.ExplodeOffset < 0 || dp.ExplodeOffset > 1)
                {
                    Debug.WriteLine("Value of ExplodeOffset must be between 0 and 1");
                    throw (new Exception("Value of ExplodeOffset must be betwee 0 and 1"));
                }
                if (maxExplodeValue < dp.ExplodeOffset) maxExplodeValue = dp.ExplodeOffset;
            }

            // sum for additional uses like in Tool tip
            _sum = sum;
            if (_sum <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Pie chart requires at least one entry other than zero");
                throw (new Exception("Pie chart requires at least one entry other than zero"));
            }
            plotSide1 = _parent.PlotArea.Width - 2 * _parent.PlotArea.BorderThickness;
            plotSide2 = _parent.PlotArea.Height - 2 * _parent.PlotArea.BorderThickness;

            plotSide1 -= MaxLabelWidth;
            plotSide2 -= MaxLabelHeight;

            startPos.X = plotSide1 / 2 + _parent.PlotArea.BorderThickness/2 + MaxLabelWidth/2;
            startPos.Y = plotSide2 / 2 + _parent.PlotArea.BorderThickness/2 + MaxLabelHeight/2;

            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;

            if (maxExplodeValue > 0 && maxExplodeValue + 0.05 <= 1)
                maxExplodeValue += 0.05;

            
            radiusScaling = 1 - 0.5*maxExplodeValue;
            centerOffsetFactor = 1 - radiusScaling;

            if (labelFlag)
            {
                
                radius = Math.Min(plotSide1 / 2, plotSide2 / 2);

                

                if (labelOutsideFlag && maxExplodeValue <= 0)
                {
                    radius *= 0.7 * radiusScaling;
                    PositionLabels2(new Point(plotSide1 * radiusScaling * 0.8, plotSide1 * radiusScaling * 0.8), startPos, sum, radius);
                }

                else
                {
                    radius *= radiusScaling;
                    //This is done so that the pie doesn't touch the chart border if padding is not given
                    if (_parent.Padding <= 4) radius -= 5;
                    else radius -= _parent.Padding;

                    PositionLabels2(new Point(plotSide1 * radiusScaling, plotSide1 * radiusScaling), startPos, sum, radius);
                }

            }
            else
            {
                if (plotSide1 > plotSide2)
                    plotSide1 = plotSide2;

                plotSide1 /= 2;
                radius = plotSide1 * radiusScaling;

                //This is done so that the pie doesn't touch the chart border if padding is not given
                if (_parent.Padding <= 4) radius -= 5;
                else radius -= _parent.Padding;


            }
            startAngle = StartAngle;


            Double centerX, centerY;
            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].YValue / sum;
                if (DataPoints[i].YValue == sum)
                {
                    PlotDoughnutSingleton(i, startPos, radius);
                    continue;
                }
                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                centerOffset = centerOffsetFactor * radius;

                centerX = startPos.X + centerOffset * Math.Cos((startAngle + stopAngle) * 0.5);
                centerY = startPos.Y + centerOffset * Math.Sin((startAngle + stopAngle) * 0.5);


                end1.X = centerX + radius * Math.Cos(startAngle);
                end1.Y = centerY + radius * Math.Sin(startAngle);

                end2.X = centerX + radius * Math.Cos(stopAngle);
                end2.Y = centerY + radius * Math.Sin(stopAngle);

                end3.X = centerX + radius / 2 * Math.Cos(startAngle);
                end3.Y = centerY + radius / 2 * Math.Sin(startAngle);

                end4.X = centerX + radius / 2 * Math.Cos(stopAngle);
                end4.Y = centerY + radius / 2 * Math.Sin(stopAngle);

                
                

                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end3.X, end3.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y);
                pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end2.X, end2.Y);

                if (stopAngle - startAngle >= Math.PI)
                    pathXAML += String.Format(@"IsLargeArc=""true"" ");

                pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", radius, radius);
                pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y);
                pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end3.X, end3.Y);

                if (stopAngle - startAngle >= Math.PI)
                    pathXAML += String.Format(@"IsLargeArc=""true"" ");

                pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", radius / 2, radius / 2);
                pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                _doughnut[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);

                _doughnut[i].StrokeThickness = DataPoints[i].BorderThickness;

                DataPoints[i].AttachToolTip(_doughnut[i]);
                DataPoints[i].AttachHref(_doughnut[i]);

                _doughnut[i].Data = (PathGeometry)XamlReader.Load(pathXAML);

                

                switch (DataPoints[i].BorderStyle)
                {
                    case "Solid":
                        break;

                    case "Dashed":
                        _doughnut[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        break;

                    case "Dotted":
                        _doughnut[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        break;
                }

                if (LightingEnabled && DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                {
                    String gradient;
                    SolidColorBrush brush = (DataPoints[i].Background as SolidColorBrush);
                    Double intensity = Math.Floor((0.85 * (1 - 0.35 * 255 / 255)) * 100) / 100;
                    
                    intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                    _doughnut[i].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, intensity).ToString());

                    Ellipse lighting = new Ellipse();
                    lighting.Width = radius * 2;
                    lighting.Height = radius * 2;
                    lighting.SetValue(LeftProperty, centerX - radius);
                    lighting.SetValue(TopProperty, centerY - radius);
                    gradient = "0.5;0.5;";
                    gradient += "#00000000,0;";
                    gradient += "#7F000000,0.5;";
                    gradient += "#00000000,0.75;";
                    gradient += "#7F000000,1;";

                    lighting.Fill = Parser.ParseColor(gradient);
                    lighting.IsHitTestVisible = false;
                    DataPoints[i].Children.Add(lighting);
                    DataPoints[i].Clip = (PathGeometry)XamlReader.Load(pathXAML);
                }
                else
                {
                    _doughnut[i].Fill = Cloner.CloneBrush(DataPoints[i].Background);
                }


                

                if (Bevel)
                {
                    #region Bevel
                    Point bevelCenter = new Point();
                    Point bevelEnd1 = new Point();
                    Point bevelEnd2 = new Point();
                    Point bevelEnd3 = new Point();
                    Point bevelEnd4 = new Point();
                    int bevelLength = 4;
                    Double bevelRadius = (radius - 2 * bevelLength);
                    bevelCenter.X = centerX;
                    bevelCenter.Y = centerY;
                    
                    bevelEnd1.X = centerX + (radius - bevelLength) * Math.Cos(startAngle + 0.03);
                    bevelEnd1.Y = centerY + (radius - bevelLength) * Math.Sin(startAngle + 0.03);

                    bevelEnd2.X = centerX + (radius - bevelLength) * Math.Cos(stopAngle - 0.03);
                    bevelEnd2.Y = centerY + (radius - bevelLength) * Math.Sin(stopAngle - 0.03);

                    bevelEnd3.X = centerX + (radius/2 + bevelLength) * Math.Cos(startAngle + 0.03);
                    bevelEnd3.Y = centerY + (radius/2 + bevelLength) * Math.Sin(startAngle + 0.03);

                    bevelEnd4.X = centerX + (radius/2 + bevelLength) * Math.Cos(stopAngle - 0.03);
                    bevelEnd4.Y = centerY + (radius/2 + bevelLength) * Math.Sin(stopAngle - 0.03);


                    Double tempangle = (startAngle + stopAngle) * 0.5 * 180 / Math.PI;

                    List<Point> bevelPoints = new List<Point>();
                    bevelPoints.Add(end3);
                    bevelPoints.Add(end1);
                    bevelPoints.Add(bevelEnd1);
                    bevelPoints.Add(bevelEnd3);
                    if (startAngle > Math.PI * 0.5 && startAngle <= Math.PI * 1.5)
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", startAngle * 180 / Math.PI + 135, (int)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }
                    else
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", -startAngle * 180 / Math.PI, (int)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }

                    bevelPoints.Clear();
                    bevelPoints.Add(end4);
                    bevelPoints.Add(end2);
                    bevelPoints.Add(bevelEnd2);
                    bevelPoints.Add(bevelEnd4);
                    if (stopAngle > Math.PI * 0.5 && stopAngle <= Math.PI * 1.5)
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", stopAngle * 180 / Math.PI + 135, (int)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }
                    else
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", -stopAngle * 180 / Math.PI, (int)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }

                    bevelPoints.Clear();

                    Path _bevel = new Path();
                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", bevelEnd2.X, bevelEnd2.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y);
                    if (stopAngle - startAngle >= Math.PI || startAngle > stopAngle )
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", radius, radius);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", bevelEnd1.X, bevelEnd1.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", bevelEnd2.X, bevelEnd2.Y);
                    if (stopAngle - startAngle >= Math.PI || startAngle > stopAngle )
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", (radius - bevelLength), (radius - bevelLength));
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    _bevel.Data = (PathGeometry)XamlReader.Load(pathXAML);

                    String linGrad;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                    {
                        if (tempangle - StartAngle * 180 / Math.PI > 0 && tempangle - StartAngle * 180 / Math.PI < 180)
                        {
                            linGrad = (tempangle + 90) + ";";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.85) + ",1";
                        }
                        else
                        {
                            linGrad = (tempangle + 90) + ";";
                            linGrad += Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.99) + ",1";
                        }
                        _bevel.Fill = Parser.ParseLinearGradient(linGrad);

                    }
                    else
                    {
                        _bevel.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }

                    _bevel.SetValue(ZIndexProperty, (int)_doughnut[i].GetValue(ZIndexProperty) + 1);

                    DataPoints[i].Children.Add(_bevel);
                    _bevel = new Path();
                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", bevelEnd4.X, bevelEnd4.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end3.X, end3.Y);
                    if (stopAngle - startAngle >= Math.PI || startAngle > stopAngle )
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", radius / 2, radius / 2);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", bevelEnd3.X, bevelEnd3.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", bevelEnd4.X, bevelEnd4.Y);
                    if (stopAngle - startAngle >= Math.PI || startAngle > stopAngle)
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", (radius / 2 + bevelLength), (radius / 2 + bevelLength));
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    _bevel.Data = (PathGeometry)XamlReader.Load(pathXAML);

                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                    {
                        if (tempangle - StartAngle * 180 / Math.PI > 0 && tempangle - StartAngle * 180 / Math.PI < 180)
                        {

                            linGrad = (tempangle + 90) + ";";
                            linGrad += Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.99) + ",1";
                        }
                        else
                        {
                            linGrad = (tempangle + 90) + ";";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.85) + ",1";
                        }
                        _bevel.Fill = Parser.ParseLinearGradient(linGrad);

                    }
                    else
                    {
                        _bevel.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }

                    _bevel.SetValue(ZIndexProperty, (int)_doughnut[i].GetValue(ZIndexProperty) + 1);

                    DataPoints[i].Children.Add(_bevel);
                    #endregion Bevel
                }

                startAngle = stopAngle;
            }

        }

        private void PlotPie3D()
        {
            Double sum = 0;
            Double plotHeight;
            Double plotWidth;
            Double startAngle, stopAngle;
            Double radius;
            int i;
           
            Double tempSide;

            Point startPos = new Point();
            Point end1 = new Point();
            Point end2 = new Point();

            String pathXAML;
            Boolean labelFlag = false, labelOutsideFlag = false;
            Double maxExplodeValue = 0;

            Double depth = _parent.Height * 0.075;
            Double MaxLabelWidth = 0;
            Double MaxLabelHeight = 0;
            TextBlock _textBlock = new TextBlock();

            foreach (DataPoint dp in DataPoints)
            {
                if (dp.YValue == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Zero valued DataPoints area not supported");
                    throw (new Exception("Zero valued DataPoints area not supported"));
                }
                sum += dp.YValue;
                _textBlock.FontSize = dp.LabelFontSize;
                _textBlock.FontFamily = new FontFamily(dp.LabelFontFamily);
                _textBlock.FontWeight = Converter.StringToFontWeight(dp.LabelFontWeight);
                _textBlock.Text = dp.TextParser(dp.LabelText);

                if (MaxLabelHeight < _textBlock.ActualHeight) MaxLabelHeight = _textBlock.ActualHeight;
                if (MaxLabelWidth < _textBlock.ActualWidth) MaxLabelWidth = _textBlock.ActualWidth;

                if (dp.LabelEnabled.ToLower() == "true")
                {
                    labelFlag = true;
                    if (dp.LabelStyle.ToLower() == "outside")
                        labelOutsideFlag = true;
                }
                if (dp.ExplodeOffset < 0 || dp.ExplodeOffset > 1)
                {
                    Debug.WriteLine("Value of ExplodeOffset must be between 0 and 1");
                    throw (new Exception("Value of ExplodeOffset must be betwee 0 and 1"));
                }
                if (maxExplodeValue < dp.ExplodeOffset) maxExplodeValue = dp.ExplodeOffset;
            }
            // sum for additional uses like in Tool tip
            _sum = sum;
            if (_sum <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Pie chart requires at least one entry other than zero");
                throw (new Exception("Pie chart requires at least one entry other than zero"));
            }

            plotWidth = _parent.PlotArea.Width - 2 * _parent.PlotArea.BorderThickness - depth;
            plotHeight = _parent.PlotArea.Height - 2 * _parent.PlotArea.BorderThickness - depth * 2;

            plotWidth -= MaxLabelWidth;
            plotHeight -= MaxLabelHeight;

            startPos.X = plotWidth / 2 + _parent.PlotArea.BorderThickness + depth / 2 + MaxLabelWidth / 2;
            startPos.Y = plotHeight / 2 + _parent.PlotArea.BorderThickness + depth + MaxLabelHeight / 2;



            plotWidth /= 2;
            plotHeight /= 2;

            if (plotWidth > plotHeight)
            {
                tempSide = plotWidth * 0.4;
                plotHeight = plotHeight > tempSide ? tempSide : plotHeight;
            }
            else
            {
                plotHeight = plotWidth * 0.4;
            }

            Double centerOffsetFactor, centerOffset;
            Double radiusScaling, angle;
            
            radiusScaling = 1 - 0.5 * maxExplodeValue;
            centerOffsetFactor = 1 - radiusScaling;

            Double factor = radiusScaling;

            if (labelFlag)
            {

                
                if (labelOutsideFlag && maxExplodeValue > 0)
                {
                    PositionLabels3D(new Point(plotWidth * factor, plotHeight * factor), new Point(startPos.X, startPos.Y + depth / 2), new Point(plotWidth * factor * 0.7, plotHeight * factor * 0.7), sum);
                    plotWidth *= factor * 0.7;
                    plotHeight *= factor * 0.7;
                }

                else
                {
                    PositionLabels3D(new Point(plotWidth, plotHeight), new Point(startPos.X, startPos.Y + depth / 2), new Point(plotWidth * 0.8, plotHeight * 0.8), sum);
                    plotWidth *= 0.8;
                    plotHeight *= 0.8;
                }

            }
            else
            {
                plotWidth *= factor;
                plotHeight *= factor;
            }

            startAngle = StartAngle;

            List<ElementPosition> elementGroup = new List<ElementPosition>();
            Double centerX, centerY;
            Double yScalingFactor = plotHeight / plotWidth;
            radius = plotWidth;
            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].YValue / sum;
                if (DataPoints[i].YValue == sum)
                {
                    PlotPieSingleton3D(i, startPos, new Point(plotWidth, plotHeight));
                    continue;
                }
                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                
                centerOffset = centerOffsetFactor * radius;

                centerX = startPos.X + centerOffset * Math.Cos((startAngle + stopAngle) * 0.5);
                centerY = startPos.Y + centerOffset * Math.Sin((startAngle + stopAngle) * 0.5) * yScalingFactor;

                

                end1.X = centerX + radius * Math.Cos(startAngle);
                end1.Y = centerY + radius * Math.Sin(startAngle) * yScalingFactor;


                

                end2.X = centerX + radius * Math.Cos(stopAngle);
                end2.Y = centerY + radius * Math.Sin(stopAngle) * yScalingFactor;

                //This part of the code generates the 3d gradients
                #region Color Gradient
                Brush brushPie = null;
                Brush brushSide = null;
                Brush brushLeft = null;
                Brush brushRight = null;
                Brush tempBrush = DataPoints[i].Background;
                if (tempBrush.GetType().Name == "LinearGradientBrush")
                {
                    LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                    brushPie = Cloner.CloneBrush(DataPoints[i].Background);

                    brushSide = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");
                    brushLeft = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1.5,-0.5"" StartPoint=""-0.5,1.5""></LinearGradientBrush>");
                    brushRight = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1.5,-0.5"" StartPoint=""-0.5,1.5""></LinearGradientBrush>");


                    Parser.GenerateDarkerGradientBrush(brush, brushSide as LinearGradientBrush, 0.75);
                    Parser.GenerateLighterGradientBrush(brush, brushLeft as LinearGradientBrush, 0.85);
                    Parser.GenerateLighterGradientBrush(brush, brushRight as LinearGradientBrush, 0.85);

                    RotateTransform rt = new RotateTransform();
                    rt.Angle = -45;
                    brushSide.RelativeTransform = rt;

                }
                else if (tempBrush.GetType().Name == "RadialGradientBrush")
                {
                    RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                    brushPie = Cloner.CloneBrush(DataPoints[i].Background);

                    brushSide = new RadialGradientBrush();
                    brushLeft = new RadialGradientBrush();
                    brushRight = new RadialGradientBrush();

                    (brushSide as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                    (brushLeft as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                    (brushRight as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;

                    Parser.GenerateDarkerGradientBrush(brush, brushSide as RadialGradientBrush, 0.75);
                    Parser.GenerateLighterGradientBrush(brush, brushLeft as RadialGradientBrush, 0.85);
                    Parser.GenerateLighterGradientBrush(brush, brushRight as RadialGradientBrush, 0.85);

                }
                else if (tempBrush.GetType().Name == "SolidColorBrush")
                {

                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (LightingEnabled)
                    {
                        String linbrush;
                        Double r, g, b;

                        linbrush = (startAngle * 180 / Math.PI).ToString() + ";";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                        linbrush += ",0;";
                        r = ((Double)(brush as SolidColorBrush).Color.R / 255) * 0.6;
                        g = ((Double)(brush as SolidColorBrush).Color.G / 255) * 0.6;
                        b = ((Double)(brush as SolidColorBrush).Color.B / 255) * 0.6;
                        linbrush += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                        linbrush += ",1";

                        brushSide = Parser.ParseLinearGradient(linbrush);

                        brushLeft = Parser.ParseLinearGradient(linbrush);
                        brushRight = Parser.ParseLinearGradient(linbrush);


                        
                        linbrush = (((startAngle + stopAngle) / 2) * 180 / Math.PI).ToString() + ";";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                        linbrush += ",1";
                        brushPie = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        brushPie = Cloner.CloneBrush(brush);

                        brushLeft = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));
                        brushRight = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));

                        brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                    }
                }
                else
                {
                    brushPie = Cloner.CloneBrush(DataPoints[i].Background);
                    brushLeft = Cloner.CloneBrush(DataPoints[i].Background);
                    brushRight = Cloner.CloneBrush(DataPoints[i].Background);
                    brushSide = Cloner.CloneBrush(DataPoints[i].Background);
                }
                #endregion Color Gradient

                //Create Pie Face
                #region PieFace

                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", centerX, centerY);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y);

                if (stopAngle - startAngle >= Math.PI)
                    pathXAML += String.Format(@"IsLargeArc=""true"" ");

                pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY);
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _pies[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                #endregion PieFace

                // take floating point mod of the start angle
                while (startAngle < 0) { startAngle += Math.PI * 2; }
                while (stopAngle < 0) { stopAngle += Math.PI * 2; }
                while (startAngle > Math.PI * 2) { startAngle -= Math.PI * 2; }
                while (stopAngle > Math.PI * 2) { stopAngle -= Math.PI * 2; }

                angle = (startAngle + stopAngle) / 2;

                #region ZIndex Setting

                _pies[i].SetValue(ZIndexProperty, 500);

                #endregion ZIndex Setting
                //Create Pie Side
                #region PieSide
                Boolean section1 = false;
                Boolean section2 = false;
                Boolean section3 = false;
                Boolean setbottom = false;

                Double sec1StartAngle = Double.NaN;
                Double sec1StopAngle = Double.NaN;
                Double sec2StartAngle = Double.NaN;
                Double sec2StopAngle = Double.NaN;
                Double sec3StartAngle = Double.NaN;
                Double sec3StopAngle = Double.NaN;
                Point arcStart = new Point();
                Point arcEnd = new Point();



                if ((startAngle >= Math.PI && startAngle <= Math.PI * 2 && stopAngle >= Math.PI && stopAngle <= Math.PI * 2) && (startAngle > stopAngle))
                {
                    // Outer face
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    setbottom = true;


                    sec1StartAngle = Math.PI * 0.01;
                    sec1StopAngle = Math.PI;


                    elementGroup.Add(new ElementPosition(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPosition(_pieRight[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPosition(DataPoints[i].LabelLine, sec1StartAngle, sec1StopAngle));

                    this.Children.Add(auxSide1);
                }
                else if ((startAngle >= 0 && startAngle <= Math.PI && stopAngle >= 0 && stopAngle <= Math.PI) && (startAngle > stopAngle))
                {
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    section2 = true;
                    CreateAuxPath(ref auxSide2, i, brushSide);
                    auxID2 = i;

                    setbottom = true;

                    sec1StartAngle = startAngle;
                    sec1StopAngle = Math.PI;

                    sec2StartAngle = Math.PI * 0.01;
                    sec2StopAngle = stopAngle;

                    elementGroup.Add(new ElementPosition(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPosition(_pieRight[i], stopAngle, stopAngle));
                    elementGroup.Add(new ElementPosition(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide2, sec2StartAngle, sec2StopAngle));
                    if (DataPoints[i].LabelLine != null)
                        DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);

                    this.Children.Add(auxSide1);
                    this.Children.Add(auxSide2);

                }
                else if ((startAngle >= 0 && startAngle <= Math.PI) && (stopAngle >= Math.PI && stopAngle <= Math.PI * 2))
                {
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    setbottom = true;

                    sec1StartAngle = startAngle;
                    sec1StopAngle = Math.PI;

                    elementGroup.Add(new ElementPosition(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPosition(_pieRight[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPosition(DataPoints[i].LabelLine, startAngle, stopAngle));

                    this.Children.Add(auxSide1);
                }
                else if ((startAngle >= Math.PI && startAngle <= Math.PI * 2) && (stopAngle >= 0 && stopAngle <= Math.PI))
                {
                    section2 = true;
                    CreateAuxPath(ref auxSide2, i, brushSide);
                    auxID2 = i;

                    setbottom = true;

                    sec2StartAngle = Math.PI * 0.001;
                    sec2StopAngle = stopAngle;

                    elementGroup.Add(new ElementPosition(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide2, sec2StartAngle, sec2StopAngle));
                    elementGroup.Add(new ElementPosition(_pieRight[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPosition(DataPoints[i].LabelLine, stopAngle, stopAngle));

                    this.Children.Add(auxSide2);
                }
                else
                {
                    elementGroup.Add(new ElementPosition(_pieLeft[i], startAngle, startAngle));
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        elementGroup.Add(new ElementPosition(_pieSides[i], startAngle, stopAngle));
                        if(DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, 300);
                    }
                    else
                    {
                        if (DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);
                    }
                    elementGroup.Add(new ElementPosition(_pieRight[i], stopAngle, stopAngle));

                }

                if (section1)
                {
                    

                    arcStart.X = centerX + radius * Math.Cos(sec1StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec1StartAngle) * yScalingFactor;

                    
                    arcEnd.X = centerX + radius * Math.Cos(sec1StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec1StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide1.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section2)
                {
                    

                    arcStart.X = centerX + radius * Math.Cos(sec2StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec2StartAngle) * yScalingFactor;

                   

                    arcEnd.X = centerX + radius * Math.Cos(sec2StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec2StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide2.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section3)
                {
                    

                    arcStart.X = centerX + radius * Math.Cos(sec3StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec3StartAngle) * yScalingFactor;

                   

                    arcEnd.X = centerX + radius * Math.Cos(sec3StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec3StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide3.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (setbottom)
                {
                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""> <PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", centerX, centerY + depth);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y + depth);

                    if (stopAngle - startAngle >= Math.PI)
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");

                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY + depth);
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    _pieSides[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                    _pieSides[i].Fill = new SolidColorBrush(Colors.Transparent);
                    _pieSides[i].SetValue(ZIndexProperty, -100);

                }
                else
                {
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end1.X, end1.Y);
                        pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y + depth);
                        pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end2.X, end2.Y + depth);

                        if (stopAngle - startAngle >= Math.PI)
                            pathXAML += String.Format(@"IsLargeArc=""true"" ");

                        pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                        pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                        pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                        pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y);

                        if (stopAngle - startAngle >= Math.PI)
                            pathXAML += String.Format(@"IsLargeArc=""true"" ");

                        pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                        pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                        pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                        pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                        _pieSides[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                        _pieSides[i].Fill = Cloner.CloneBrush(brushSide);
                    }

                }
                #endregion PieSide

                #region Pie Left
                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", centerX, centerY);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY);
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _pieLeft[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                #endregion Pie Left

                #region Pie Right
                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", centerX, centerY);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", centerX, centerY);
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _pieRight[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                #endregion Pie Right



                _pies[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _pieSides[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _pieLeft[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _pieRight[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);


                _pies[i].Fill = Cloner.CloneBrush(brushPie);

                _pieLeft[i].Fill = Cloner.CloneBrush(brushLeft);
                _pieRight[i].Fill = Cloner.CloneBrush(brushRight);

                _pies[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieSides[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieLeft[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieRight[i].Opacity = Opacity * DataPoints[i].Opacity;

                _pies[i].StrokeThickness = DataPoints[i].BorderThickness;
                _pieSides[i].StrokeThickness = DataPoints[i].BorderThickness;
                _pieLeft[i].StrokeThickness = DataPoints[i].BorderThickness;
                _pieRight[i].StrokeThickness = DataPoints[i].BorderThickness;

                DataPoints[i].AttachToolTip(_pies[i]);
                DataPoints[i].AttachToolTip(_pieSides[i]);
                DataPoints[i].AttachToolTip(_pieLeft[i]);
                DataPoints[i].AttachToolTip(_pieRight[i]);

                DataPoints[i].AttachHref(_pies[i]);
                DataPoints[i].AttachHref(_pieSides[i]);
                DataPoints[i].AttachHref(_pieLeft[i]);
                DataPoints[i].AttachHref(_pieRight[i]);

                switch (DataPoints[i].BorderStyle)
                {
                    case "Solid":
                        break;

                    case "Dashed":
                        _pies[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        break;

                    case "Dotted":
                        _pies[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        break;
                }

                

                startAngle = stopAngle;
            }
            #region ZIndex final
            int zindex1, zindex2;
            elementGroup.Sort(ElementPosition.CompareAngle);
            zindex1 = 150;
            zindex2 = 100;
            for (i = 0; i < elementGroup.Count; i++)
            {
                SetZIndex(elementGroup[i]._element, ref zindex1, ref zindex2, elementGroup[i]._angle1);
            }
            #endregion ZIndex final
        }

        private void SetZIndex(FrameworkElement element, ref int zindex1, ref int zindex2, Double angle)
        {
            if (angle >= 0 && angle <= Math.PI / 2)
            {
                element.SetValue(ZIndexProperty, ++zindex1);
            }
            else if (angle > Math.PI / 2 && angle <= Math.PI)
            {
                element.SetValue(ZIndexProperty, --zindex1);
            }
            else if (angle > Math.PI && angle <= Math.PI * 3 / 2)
            {
                element.SetValue(ZIndexProperty, --zindex2);
            }
            else
            {
                element.SetValue(ZIndexProperty, ++zindex2);
            }
        }

        private void PlotDoughnut3D()
        {
            Double sum = 0;
            Double plotHeight;
            Double plotWidth;
            Double startAngle = StartAngle, stopAngle;
            Double radius;
            int i;
            

            Double tempSide;

            Point startPos = new Point();
            Point end1 = new Point();
            Point end2 = new Point();
            Point end3 = new Point();
            Point end4 = new Point();

            String pathXAML;

            Double depth = _parent.Height * 0.075;
            Boolean labelFlag = false, labelOutsideFlag = false;
            Double maxExplodeValue = 0;

            Double MaxLabelWidth = 0;
            Double MaxLabelHeight = 0;
            TextBlock _textBlock = new TextBlock();

            foreach (DataPoint dp in DataPoints)
            {
                if (dp.YValue == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Zero valued DataPoints area not supported");
                    throw (new Exception("Zero valued DataPoints area not supported"));
                    
                }

                sum += dp.YValue;
                _textBlock.FontSize = dp.LabelFontSize;
                _textBlock.FontFamily = new FontFamily(dp.LabelFontFamily);
                _textBlock.FontWeight = Converter.StringToFontWeight(dp.LabelFontWeight);
                _textBlock.Text = dp.TextParser(dp.LabelText);

                if (MaxLabelHeight < _textBlock.ActualHeight) MaxLabelHeight = _textBlock.ActualHeight;
                if (MaxLabelWidth < _textBlock.ActualWidth) MaxLabelWidth = _textBlock.ActualWidth;

                if (dp.LabelEnabled.ToLower() == "true")
                {
                    labelFlag = true;
                    if (dp.LabelStyle.ToLower() == "outside")
                        labelOutsideFlag = true;
                }
                if (dp.ExplodeOffset < 0 || dp.ExplodeOffset > 1)
                {
                    Debug.WriteLine("Value of ExplodeOffset must be between 0 and 1");
                    throw (new Exception("Value of ExplodeOffset must be betwee 0 and 1"));
                }
                if (maxExplodeValue < dp.ExplodeOffset) maxExplodeValue = dp.ExplodeOffset;
            }

            // sum for additional uses like in Tool tip
            _sum = sum;
            if (_sum <= 0)
            {
                System.Diagnostics.Debug.WriteLine("Pie chart requires at least one entry other than zero");
                throw (new Exception("Pie chart requires at least one entry other than zero"));
                
            }

            plotWidth = _parent.PlotArea.Width - 2 * _parent.PlotArea.BorderThickness - depth;
            plotHeight = _parent.PlotArea.Height - 2 * _parent.PlotArea.BorderThickness - depth * 2;

            plotWidth -= MaxLabelWidth;
            plotHeight -= MaxLabelHeight;

            plotWidth /= 2;
            plotHeight /= 2;

            startPos.X = plotWidth + _parent.PlotArea.BorderThickness + depth / 2 + MaxLabelWidth / 2;
            startPos.Y = plotHeight + _parent.PlotArea.BorderThickness + depth / 2 + MaxLabelHeight / 2;

            for (i = 0; i < DataPoints.Count; i++)
            {
                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    labelFlag = true;
            }

            if (plotWidth > plotHeight)
            {
                tempSide = plotWidth * 0.4;
                plotHeight = plotHeight > tempSide ? tempSide : plotHeight;
            }
            else
            {
                plotHeight = plotWidth * 0.4;
            }

            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;

            radiusScaling = 1 - 0.5 * maxExplodeValue;
            centerOffsetFactor = 1 - radiusScaling;
            int zindex1 = 150, zindex2 = 100;
            

            Double factor = radiusScaling;

            if (labelFlag)
            {

                

                if (labelOutsideFlag && maxExplodeValue > 0)
                {
                    PositionLabels3D(new Point(plotWidth, plotHeight), new Point(startPos.X, startPos.Y + depth / 2), new Point(plotWidth * factor * 0.7, plotHeight * factor * 0.7), sum);
                    plotWidth *= factor * 0.7;
                    plotHeight *= factor * 0.7;
                }

                else
                {
                    PositionLabels3D(new Point(plotWidth, plotHeight), new Point(startPos.X, startPos.Y + depth / 2), new Point(plotWidth * 0.8, plotHeight * 0.8), sum);
                    plotWidth *= 0.8;
                    plotHeight *= 0.8;
                }

            }
            else
            {
                plotWidth *= factor;
                plotHeight *= factor;
            }
            List<ElementPosition> elementGroup = new List<ElementPosition>();
            Double centerX, centerY, angle;
            Double yScalingFactor = plotHeight / plotWidth;
            radius = plotWidth;
            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].YValue / sum;
                if (DataPoints[i].YValue == sum)
                {
                    PlotDoughnutSingleton3D(i, startPos, new Point(plotWidth, plotHeight));
                    continue;
                }
                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                
                centerOffset = centerOffsetFactor * radius;

                centerX = startPos.X + centerOffset * Math.Cos((startAngle + stopAngle) * 0.5);
                centerY = startPos.Y + centerOffset * Math.Sin((startAngle + stopAngle) * 0.5) * yScalingFactor;


                
                end1.X = centerX + radius * Math.Cos(startAngle);
                end1.Y = centerY + radius * Math.Sin(startAngle) * yScalingFactor;
                end3.X = centerX + radius / 2 * Math.Cos(startAngle);
                end3.Y = centerY + radius / 2 * Math.Sin(startAngle) * yScalingFactor;


                

                end2.X = centerX + radius * Math.Cos(stopAngle);
                end2.Y = centerY + radius * Math.Sin(stopAngle) * yScalingFactor;
                end4.X = centerX + radius / 2 * Math.Cos(stopAngle);
                end4.Y = centerY + radius / 2 * Math.Sin(stopAngle) * yScalingFactor;

                //This part of the code generates the 3d gradients
                #region Color Gradient
                Brush brushPie = null;
                Brush brushSide = null;
                Brush brushLeft = null;
                Brush brushRight = null;
                Brush tempBrush = DataPoints[i].Background;
                if (tempBrush.GetType().Name == "LinearGradientBrush")
                {
                    LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                    brushPie = Cloner.CloneBrush(DataPoints[i].Background);

                    brushSide = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");
                    brushLeft = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1.5,-0.5"" StartPoint=""-0.5,1.5""></LinearGradientBrush>");
                    brushRight = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush EndPoint=""1.5,-0.5"" StartPoint=""-0.5,1.5""></LinearGradientBrush>");


                    Parser.GenerateDarkerGradientBrush(brush, brushSide as LinearGradientBrush, 0.75);
                    Parser.GenerateLighterGradientBrush(brush, brushLeft as LinearGradientBrush, 0.85);
                    Parser.GenerateLighterGradientBrush(brush, brushRight as LinearGradientBrush, 0.85);

                    RotateTransform rt = new RotateTransform();
                    rt.Angle = -45;
                    brushSide.RelativeTransform = rt;

                }
                else if (tempBrush.GetType().Name == "RadialGradientBrush")
                {
                    RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                    brushPie = Cloner.CloneBrush(DataPoints[i].Background);

                    brushSide = new RadialGradientBrush();
                    brushLeft = new RadialGradientBrush();
                    brushRight = new RadialGradientBrush();

                    (brushSide as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                    (brushLeft as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                    (brushRight as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;

                    Parser.GenerateDarkerGradientBrush(brush, brushSide as RadialGradientBrush, 0.75);
                    Parser.GenerateLighterGradientBrush(brush, brushLeft as RadialGradientBrush, 0.85);
                    Parser.GenerateLighterGradientBrush(brush, brushRight as RadialGradientBrush, 0.85);

                }
                else if (tempBrush.GetType().Name == "SolidColorBrush")
                {

                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (LightingEnabled)
                    {
                        String linbrush;

                        Double r, g, b;

                        linbrush = (startAngle * 180 / Math.PI).ToString() + ";";
                        
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.55);
                        linbrush += ",0;";
                        r = ((Double)(brush as SolidColorBrush).Color.R / 255) * 0.6;
                        g = ((Double)(brush as SolidColorBrush).Color.G / 255) * 0.6;
                        b = ((Double)(brush as SolidColorBrush).Color.B / 255) * 0.6;
                        linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                        linbrush += ",1";

                        brushSide = Parser.ParseLinearGradient(linbrush);

                        brushLeft = Parser.ParseLinearGradient(linbrush);
                        brushRight = Parser.ParseLinearGradient(linbrush);


                        linbrush = (((startAngle + stopAngle) / 2) * 180 / Math.PI).ToString() + ";";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                        linbrush += ",1";

                        brushPie = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        brushPie = Cloner.CloneBrush(brush);

                        brushLeft = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));
                        brushRight = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));

                        brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                    }
                }
                else
                {
                    brushPie = Cloner.CloneBrush(DataPoints[i].Background);
                    brushLeft = Cloner.CloneBrush(DataPoints[i].Background);
                    brushRight = Cloner.CloneBrush(DataPoints[i].Background);
                    brushSide = Cloner.CloneBrush(DataPoints[i].Background);
                }
                #endregion Color Gradient

                //Create Doughnut Face
                #region DoughnutFace

                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end3.X, end3.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y);
                pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end2.X, end2.Y);

                if (stopAngle - startAngle >= Math.PI)
                    pathXAML += String.Format(@"IsLargeArc=""true"" ");

                pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y);
                pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end3.X, end3.Y);

                if (stopAngle - startAngle >= Math.PI)
                    pathXAML += String.Format(@"IsLargeArc=""true"" ");

                pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _doughnut[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                #endregion DoughnutFace



                // take floating point mod of the start angle
                while (startAngle < 0) { startAngle += Math.PI * 2; }
                while (stopAngle < 0) { stopAngle += Math.PI * 2; }
                while (startAngle > Math.PI * 2) { startAngle -= Math.PI * 2; }
                while (stopAngle > Math.PI * 2) { stopAngle -= Math.PI * 2; }

                angle = (startAngle + stopAngle) / 2;

                #region ZIndex Setting

                _doughnut[i].SetValue(ZIndexProperty, 500);


                if (DataPoints[i].LabelLine != null)
                    DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);
                #endregion ZIndex Setting
                //Create Pie Side
                #region PieSide
                Boolean section1 = false;
                Boolean section2 = false;
                Boolean section3 = false;
                Boolean section4 = false;
                Boolean section5 = false;
                Boolean section6 = false;
                Boolean setbottom = false;

                Double sec1StartAngle = Double.NaN;
                Double sec1StopAngle = Double.NaN;
                Double sec2StartAngle = Double.NaN;
                Double sec2StopAngle = Double.NaN;
                Double sec3StartAngle = Double.NaN;
                Double sec3StopAngle = Double.NaN;
                Double sec4StartAngle = Double.NaN;
                Double sec4StopAngle = Double.NaN;
                Double sec5StartAngle = Double.NaN;
                Double sec5StopAngle = Double.NaN;
                Double sec6StartAngle = Double.NaN;
                Double sec6StopAngle = Double.NaN;
                Point arcStart = new Point();
                Point arcEnd = new Point();


                if ((startAngle >= Math.PI && startAngle <= Math.PI * 2 && stopAngle >= Math.PI && stopAngle <= Math.PI * 2) && (startAngle > stopAngle))
                {
                    // Outer face
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    // Inner face
                    section4 = true;
                    CreateAuxPath(ref auxSide4, i, brushSide);
                    auxID4 = i;

                    section5 = true;
                    CreateAuxPath(ref auxSide5, i, brushSide);
                    auxID5 = i;

                    setbottom = true;


                    sec1StartAngle = Math.PI * 0.01;
                    sec1StopAngle = Math.PI;

                    sec4StartAngle = startAngle;
                    sec4StopAngle = Math.PI * 2;

                    sec5StartAngle = Math.PI * 1.01;
                    sec5StopAngle = stopAngle;

                    elementGroup.Add(new ElementPosition(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide4, sec4StartAngle, sec4StopAngle));
                    elementGroup.Add(new ElementPosition(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPosition(auxSide5, sec5StartAngle, sec5StopAngle));
                    elementGroup.Add(new ElementPosition(_pieLeft[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPosition(DataPoints[i].LabelLine, sec1StartAngle, sec1StopAngle));


                    this.Children.Add(auxSide1);

                    this.Children.Add(auxSide4);
                    this.Children.Add(auxSide5);

                }
                else if ((startAngle >= 0 && startAngle <= Math.PI && stopAngle >= 0 && stopAngle <= Math.PI) && (startAngle > stopAngle))
                {
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    section2 = true;
                    CreateAuxPath(ref auxSide2, i, brushSide);
                    auxID2 = i;

                    section4 = true;
                    CreateAuxPath(ref auxSide4, i, brushSide);
                    auxID4 = i;

                    setbottom = true;

                    sec1StartAngle = startAngle;
                    sec1StopAngle = Math.PI;

                    sec2StartAngle = Math.PI * 0.01;
                    sec2StopAngle = stopAngle;

                    sec4StartAngle = Math.PI * 1.01;
                    sec4StopAngle = Math.PI * 2;

                    elementGroup.Add(new ElementPosition(auxSide4, sec4StartAngle, sec4StopAngle));
                    elementGroup.Add(new ElementPosition(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPosition(_pieLeft[i], stopAngle, stopAngle));
                    elementGroup.Add(new ElementPosition(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide2, sec2StartAngle, sec2StopAngle));
                    if (DataPoints[i].LabelLine != null)
                        DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);

                    this.Children.Add(auxSide1);
                    this.Children.Add(auxSide2);

                    this.Children.Add(auxSide4);

                }
                else if ((startAngle >= 0 && startAngle <= Math.PI) && (stopAngle >= Math.PI && stopAngle <= Math.PI * 2))
                {
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    section4 = true;
                    CreateAuxPath(ref auxSide4, i, brushSide);
                    auxID4 = i;

                    setbottom = true;

                    sec1StartAngle = startAngle;
                    sec1StopAngle = Math.PI;

                    sec4StartAngle = Math.PI * 1.01;
                    sec4StopAngle = stopAngle;

                    elementGroup.Add(new ElementPosition(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPosition(auxSide4, sec4StartAngle, sec4StopAngle));
                    elementGroup.Add(new ElementPosition(_pieLeft[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPosition(DataPoints[i].LabelLine, startAngle, stopAngle));

                    this.Children.Add(auxSide1);

                    this.Children.Add(auxSide4);
                }
                else if ((startAngle >= Math.PI && startAngle <= Math.PI * 2) && (stopAngle >= 0 && stopAngle <= Math.PI))
                {
                    section2 = true;
                    CreateAuxPath(ref auxSide2, i, brushSide);
                    auxID2 = i;

                    section5 = true;
                    CreateAuxPath(ref auxSide5, i, brushSide);
                    auxID5 = i;

                    setbottom = true;

                    sec2StartAngle = Math.PI * 0.001;
                    sec2StopAngle = stopAngle;

                    sec5StartAngle = startAngle;
                    sec5StopAngle = 0;

                    elementGroup.Add(new ElementPosition(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPosition(auxSide2, sec2StartAngle, sec2StopAngle));
                    elementGroup.Add(new ElementPosition(auxSide5, sec5StartAngle, sec5StopAngle));
                    elementGroup.Add(new ElementPosition(_pieLeft[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPosition(DataPoints[i].LabelLine, stopAngle, stopAngle));

                    this.Children.Add(auxSide2);

                    this.Children.Add(auxSide5);
                }
                else
                {
                    elementGroup.Add(new ElementPosition(_pieRight[i], startAngle, startAngle));
                    if (startAngle >= Math.PI && startAngle <= Math.PI * 2 && stopAngle >= Math.PI && stopAngle <= Math.PI * 2)
                        elementGroup.Add(new ElementPosition(_pies[i], startAngle, stopAngle));
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        elementGroup.Add(new ElementPosition(_pieSides[i], startAngle, stopAngle));
                        if (DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, 300);
                    }
                    else
                    {
                        if (DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);
                    }
                    elementGroup.Add(new ElementPosition(_pieLeft[i], stopAngle, stopAngle));
                }

                if (section1)
                {

                    

                    arcStart.X = centerX + radius * Math.Cos(sec1StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec1StartAngle) * yScalingFactor;

                    

                    arcEnd.X = centerX + radius * Math.Cos(sec1StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec1StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide1.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section2)
                {
                    
                    arcStart.X = centerX + radius * Math.Cos(sec2StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec2StartAngle) * yScalingFactor;

                    

                    arcEnd.X = centerX + radius * Math.Cos(sec2StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec2StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide2.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section3)
                {
                    
                    arcStart.X = centerX + radius * Math.Cos(sec3StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec3StartAngle) * yScalingFactor;

                   

                    arcEnd.X = centerX + radius * Math.Cos(sec3StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec3StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide3.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section4)
                {
                    
                    arcStart.X = centerX + radius/2 * Math.Cos(sec4StartAngle);
                    arcStart.Y = centerY + radius/2 * Math.Sin(sec4StartAngle) * yScalingFactor;

                    

                    arcEnd.X = centerX + radius/2 * Math.Cos(sec4StopAngle);
                    arcEnd.Y = centerY + radius/2 * Math.Sin(sec4StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide4.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section5)
                {
                    

                    arcStart.X = centerX + radius/2 * Math.Cos(sec5StartAngle);
                    arcStart.Y = centerY + radius/2 * Math.Sin(sec5StartAngle) * yScalingFactor;

                    
                    arcEnd.X = centerX + radius/2 * Math.Cos(sec5StopAngle);
                    arcEnd.Y = centerY + radius/2 * Math.Sin(sec5StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide5.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (section6)
                {
                    

                    arcStart.X = centerX + radius/2 * Math.Cos(sec6StartAngle);
                    arcStart.Y = centerY + radius/2 * Math.Sin(sec6StartAngle) * yScalingFactor;

                    

                    arcEnd.X = centerX + radius/2 * Math.Cos(sec6StopAngle);
                    arcEnd.Y = centerY + radius/2 * Math.Sin(sec6StopAngle) * yScalingFactor;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcStart.X, arcStart.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcEnd.X, arcEnd.Y + depth);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", arcEnd.X, arcEnd.Y);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", arcStart.X, arcStart.Y);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    auxSide6.Data = (PathGeometry)XamlReader.Load(pathXAML);
                }
                if (setbottom)
                {
                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end3.X, end3.Y + depth);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end2.X, end2.Y + depth);
                    if (DataPoints[i].YValue / sum >= 0.5)
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y + depth);
                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end3.X, end3.Y + depth);
                    if (DataPoints[i].YValue / sum >= 0.5)
                        pathXAML += String.Format(@"IsLargeArc=""true"" ");
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                    _pieSides[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                    _pieSides[i].Fill = new SolidColorBrush(Colors.Transparent);
                    _pieSides[i].SetValue(ZIndexProperty, -100);

                }
                else
                {
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end1.X, end1.Y);
                        pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y + depth);
                        pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end2.X, end2.Y + depth);

                        if (stopAngle - startAngle >= Math.PI)
                            pathXAML += String.Format(@"IsLargeArc=""true"" ");

                        pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                        pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                        pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                        pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end1.X, end1.Y);

                        if (stopAngle - startAngle >= Math.PI)
                            pathXAML += String.Format(@"IsLargeArc=""true"" ");

                        pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth, plotHeight);
                        pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                        pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                        pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                        _pieSides[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                        _pieSides[i].Fill = Cloner.CloneBrush(brushSide);
                    }
                    #region InnerFace
                    if (startAngle >= Math.PI && startAngle <= Math.PI * 2 && stopAngle >= Math.PI && stopAngle <= Math.PI * 2)
                    {
                        pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end3.X, end3.Y + depth);
                        pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end3.X, end3.Y);
                        pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end4.X, end4.Y);

                        if (stopAngle - startAngle >= Math.PI)
                            pathXAML += String.Format(@"IsLargeArc=""true"" ");

                        pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                        pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");
                        pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y + depth);
                        pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", end3.X, end3.Y + depth);

                        if (stopAngle - startAngle >= Math.PI)
                            pathXAML += String.Format(@"IsLargeArc=""true"" ");

                        pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", plotWidth / 2, plotHeight / 2);
                        pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");
                        pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                        pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                        _pies[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                    }
                    #endregion InnerFace

                }
                #endregion PieSide


                
                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end4.X, end4.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end2.X, end2.Y + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end4.X, end4.Y);
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _pieLeft[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                

                
                pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", end3.X, end3.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end1.X, end1.Y + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end3.X, end3.Y + depth);
                pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", end3.X, end3.Y);
                pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");
                _pieRight[i].Data = (PathGeometry)XamlReader.Load(pathXAML);
                



                _pies[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _pieSides[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _pieLeft[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _pieRight[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                _doughnut[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);

                _pies[i].Fill = Cloner.CloneBrush(brushPie);
                _pieSides[i].Fill = Cloner.CloneBrush(brushSide);
                _pieLeft[i].Fill = Cloner.CloneBrush(brushLeft);
                _pieRight[i].Fill = Cloner.CloneBrush(brushRight);
                _doughnut[i].Fill = Cloner.CloneBrush(brushPie);

                _pies[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieSides[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieLeft[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieRight[i].Opacity = Opacity * DataPoints[i].Opacity;
                _doughnut[i].Opacity = Opacity * DataPoints[i].Opacity;


                _pies[i].StrokeThickness = DataPoints[i].BorderThickness;
                _pieSides[i].StrokeThickness = DataPoints[i].BorderThickness;
                _pieLeft[i].StrokeThickness = DataPoints[i].BorderThickness;
                _pieRight[i].StrokeThickness = DataPoints[i].BorderThickness;
                _doughnut[i].StrokeThickness = DataPoints[i].BorderThickness;


                DataPoints[i].AttachToolTip(_pies[i]);
                DataPoints[i].AttachToolTip(_pieSides[i]);
                DataPoints[i].AttachToolTip(_pieLeft[i]);
                DataPoints[i].AttachToolTip(_pieRight[i]);
                DataPoints[i].AttachToolTip(_doughnut[i]);

                DataPoints[i].AttachHref(_pies[i]);
                DataPoints[i].AttachHref(_pieSides[i]);
                DataPoints[i].AttachHref(_pieLeft[i]);
                DataPoints[i].AttachHref(_pieRight[i]);
                DataPoints[i].AttachHref(_doughnut[i]);


                switch (DataPoints[i].BorderStyle)
                {
                    case "Solid":
                        break;

                    case "Dashed":
                        _pies[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        _pieSides[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        _pieLeft[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        _pieRight[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        _doughnut[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                        break;

                    case "Dotted":
                        _pies[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        _pieSides[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        _pieLeft[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        _pieRight[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        _doughnut[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                        break;
                }

                

                startAngle = stopAngle;
            }

            #region ZIndex final
            elementGroup.Sort(ElementPosition.CompareAngle);
            zindex1 = 150;
            zindex2 = 100;
            for (i = 0; i < elementGroup.Count; i++)
            {
                SetZIndex(elementGroup[i]._element, ref zindex1, ref zindex2, elementGroup[i]._angle1);
            }
            #endregion ZIndex final

        }


        private void PlotStackedColumn()
        {
            Double width = 10;
            Double height;
            Double left;
            Double top;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            int i;

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
                width = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);
            else
                width = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);

            Double temp = Math.Abs(_parent.AxisX.DoubleToPixel(Plot.MinAxisXValue) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum));
            temp *= 2;
            if (width > temp)
                width = temp;

            width -= width * 0.1;

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<double, Point>();

            for (i = 0; i < _columns.Length; i++)
            {

                if (Double.IsNaN(DataPoints[i].YValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                point.Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue);


                if (_parent.AxisY.AxisMinimum > 0)
                    height = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum) - point.Y;
                else
                    height = Math.Abs(_parent.AxisY.DoubleToPixel(0) - point.Y);

                
                left = (point.X - width / 2);

                if (DataPoints[i].YValue >= 0)
                {

                    top = point.Y;

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        top = Plot.TopBottom[DataPoints[i].XValue].Y - height;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(Plot.TopBottom[DataPoints[i].XValue].X, top);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(top + height, top);

                }
                else
                {
                    top = _parent.AxisY.DoubleToPixel(0);

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        top = Plot.TopBottom[DataPoints[i].XValue].X;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(top + height, Plot.TopBottom[DataPoints[i].XValue].Y);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(top + height, top);
                }

                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, left);
                DataPoints[i].SetValue(TopProperty, top);

                if (_parent.View3D)
                {
                    
                    //relative width is set here
                    
                    Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["stackedcolumn"];
                    Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
                    

                    left -= (depth + initialDepth);
                    DataPoints[i].SetValue(LeftProperty, left);

                    
                    _columnShadows[i].Width = depth;
                    _columnShadows[i].Height = height;
                    _columnShadows[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));
                    _columnShadows[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + width + (Double)_parent.PlotArea.GetValue(LeftProperty));

                    _columnShadows[i].Opacity = this.Opacity * DataPoints[i].Opacity;
                    
                    _columnShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);



                    _columnShadows[i].StrokeThickness = DataPoints[i].BorderThickness;

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _columnShadows[i].RenderTransform = st1;

                    _columnTops[i].Width = width;
                    //relative height is set here
                    _columnTops[i].Height = depth;

                    _columnTops[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) - _columnTops[i].Height + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));

                    //use4 the same relative width here
                    _columnTops[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + depth + (Double)_parent.PlotArea.GetValue(LeftProperty));


                    _columnTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columnTops[i].StrokeThickness = DataPoints[i].BorderThickness;


                    _columnTops[i].Opacity = this.Opacity * DataPoints[i].Opacity;
                    
                    SkewTransform st2 = new SkewTransform();
                    st2.AngleY = 0;
                    st2.CenterX = 0;
                    st2.CenterY = 0;
                    st2.AngleX = -45;
                    _columnTops[i].RenderTransform = st2;



                    _shadows[i].SetValue(TopProperty, (Double)_columnTops[i].GetValue(TopProperty) - ShadowSize);
                    _shadows[i].SetValue(LeftProperty, (Double)_columnTops[i].GetValue(LeftProperty) + ShadowSize);

                    if ((Double)_shadows[i].GetValue(LeftProperty) + width > _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum))
                        _shadows[i].Width = width - ShadowSize;
                    else
                        _shadows[i].Width = width;

                    if ((Double)_columnTops[i].GetValue(TopProperty) - ShadowSize + height > _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum))
                    {
                        _shadows[i].Height = height - ShadowSize;
                    }
                    else
                    {
                        _shadows[i].Height = height;
                    }

                    _shadows[i].Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    _shadows[i].Fill = Parser.ParseSolidColor("#66000000");
                    

                    _shadows[i].SetValue(ZIndexProperty, 3);
                    if (!ShadowEnabled)
                    {
                        _shadows[i].Opacity = 0;
                    }

                    _columns[i].Width = width;
                    _columns[i].Height = height;
                    _columns[i].SetValue(LeftProperty, left + (Double)_parent.PlotArea.GetValue(LeftProperty));
                    _columns[i].SetValue(TopProperty, top + (depth + initialDepth) + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columns[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columns[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _columns[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    
                    int ioffset = (int)((left < 0)?Math.Abs(left)+10:0);
                    int zindex = (int)(left + ioffset + Index + DataPoints[i].Index);
                    _columns[i].SetValue(ZIndexProperty, zindex);
                    _columnShadows[i].SetValue(ZIndexProperty, zindex -1);
                    _columnTops[i].SetValue(ZIndexProperty, zindex - 2);

                    //This part of the code generates the 3d gradients
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "-90;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-45;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                            linbrush += ",1";

                            brushTop = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-120;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";

                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _columnShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    
                    _columns[i].Fill = Cloner.CloneBrush(brush2);
                    

                    //Apply transform to the brushTop

                    _columnTops[i].Fill = Cloner.CloneBrush(brushTop);



                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_columnShadows[i]);
                    DataPoints[i].AttachToolTip(_columns[i]);
                    DataPoints[i].AttachToolTip(_columnTops[i]);
                    DataPoints[i].AttachHref(_columnShadows[i]);
                    DataPoints[i].AttachHref(_columns[i]);

                    DataPoints[i].AttachHref(_columnTops[i]);

                    DataPoints[i].SetValue(TopProperty, top + (depth + initialDepth));

                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)_columns[i].GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],depth);
                }
                else
                {
                    //To plot in 2D
                    #region Stacked Column 2d Plotting

                    Path column = new Path();
                    Path shadow = new Path();
                    String pathXAML = @"";
                    String maxDP="", maxDS="";
                    Double xRadiusLimit = 0;
                    Double yRadiusLimit = 0;
                    if (DataPoints[i].YValue > 0)
                    {
                        GetPositiveMax(DataPoints[i].XValue,ref maxDP,ref maxDS);
                    }
                    else
                    {
                        GetNegativeMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }


                    if (maxDS == this.Name && maxDP == DataPoints[i].Name)
                    {
                        xRadiusLimit = DataPoints[i].RadiusX;
                        yRadiusLimit = DataPoints[i].RadiusY;

                        if (DataPoints[i].YValue >= 0)
                            shadow.Height = height - ShadowSize;
                        else
                            shadow.Height = height + ShadowSize;
                        shadow.SetValue(TopProperty, ShadowSize);
                    }
                    else
                    {
                        shadow.Height = height;
                        shadow.SetValue(TopProperty, 0);
                    }

                    if (xRadiusLimit > width / 2) xRadiusLimit = width / 2;
                    if (yRadiusLimit > height) yRadiusLimit = height;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", xRadiusLimit, 0);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    DataPoints[i].Children.Add(column);
                    DataPoints[i].Children.Add(shadow);

                    column.Data = (PathGeometry)XamlReader.Load(pathXAML);
                    column.Width = width;
                    column.Height = height;
                    column.SetValue(TopProperty, 0);
                    column.SetValue(LeftProperty, 0);




                    shadow.Width = width;
                    shadow.SetValue(LeftProperty, ShadowSize);


                    if (xRadiusLimit > shadow.Width / 2) xRadiusLimit = shadow.Width / 2;
                    if (yRadiusLimit > height) yRadiusLimit = height;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", xRadiusLimit, 0);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    shadow.Data = (PathGeometry)XamlReader.Load(pathXAML);

                    if (DataPoints[i].YValue < 0)
                    {
                        ScaleTransform st = new ScaleTransform();
                        st.ScaleX = 1;
                        st.ScaleY = -1;
                        column.RenderTransformOrigin = new Point(0.5, 0.5);
                        column.RenderTransform = st;

                        st = new ScaleTransform();
                        st.ScaleX = 1;
                        st.ScaleY = -1;
                        shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                        shadow.RenderTransform = st;
                        if (maxDS == this.Name && maxDP == DataPoints[i].Name)
                        {
                            shadow.SetValue(TopProperty, -ShadowSize);
                        }
                        else
                        {
                            shadow.SetValue(TopProperty, 0);
                        }
                    }

                    column.Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    column.StrokeThickness = DataPoints[i].BorderThickness;

                    String linbrush;
                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
                    {


                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        column.Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && Bevel)
                    {
                        
                        if (DataPoints[i].YValue > 0) linbrush = "-90;";
                        else linbrush = "90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor(brush.Color, 0.99);
                        linbrush += ",1";

                        column.Fill = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        column.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }


                    column.Opacity = this.Opacity * DataPoints[i].Opacity;
                    shadow.Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;

                    shadow.Fill = Parser.ParseSolidColor("#66000000");


                    shadow.SetValue(ZIndexProperty, 1);
                    column.SetValue(ZIndexProperty, 5);
                    if (!ShadowEnabled)
                    {
                        shadow.Opacity = 0;
                    }

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            
                            column.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            
                            column.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(column);
                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)column.GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],0);

                    DataPoints[i].AttachHref(column);
                    DataPoints[i].ApplyEffects((int)column.GetValue(ZIndexProperty) + 1);
                    #endregion Stacked Column 2d Plotting
                }

            }
        }

        private void PlotStackedColumn100()
        {
            Double width = 10;
            Double height;
            Double left;
            Double top;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            int i;

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
                width = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);
            else
                width = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);

            Double temp = Math.Abs(_parent.AxisX.DoubleToPixel(Plot.MinAxisXValue) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum));
            temp *= 2;
            if (width > temp)
                width = temp;

            width -= width * 0.1;

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<double, Point>();

            for (i = 0; i < _columns.Length; i++)
            {
                

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                point.Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue / Plot.YValueSum[DataPoints[i].XValue] * 100);

                if (_parent.AxisY.AxisMinimum > 0)
                    height = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum) - point.Y;
                else
                    height = Math.Abs(_parent.AxisY.DoubleToPixel(0) - point.Y);

                
                left = (point.X - width / 2);

                if (DataPoints[i].YValue >= 0)
                {

                    top = point.Y;

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        top = Plot.TopBottom[DataPoints[i].XValue].Y - height;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(Plot.TopBottom[DataPoints[i].XValue].X, top);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(top + height, top);

                }
                else
                {
                    top = _parent.AxisY.DoubleToPixel(0);

                    if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                    {
                        top = Plot.TopBottom[DataPoints[i].XValue].X;

                        Plot.TopBottom[DataPoints[i].XValue] = new Point(top + height, Plot.TopBottom[DataPoints[i].XValue].Y);
                    }
                    else
                        Plot.TopBottom[DataPoints[i].XValue] = new Point(top + height, top);
                }

                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, left);
                DataPoints[i].SetValue(TopProperty, top);

                if (_parent.View3D)
                {

                    //To plot in 3D
                    #region Column100 3D Plotting
                    //relative width is set here
                    Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["stackedcolumn100"];
                    Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
                    
                    left -= (depth + initialDepth);
                    DataPoints[i].SetValue(LeftProperty, left);
                    
                    _columnShadows[i].Width = depth;
                    _columnShadows[i].Height = height;
                    _columnShadows[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));
                    _columnShadows[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + width + (Double)_parent.PlotArea.GetValue(LeftProperty));

                    _columnShadows[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    _columnShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);



                    _columnShadows[i].StrokeThickness = DataPoints[i].BorderThickness;

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _columnShadows[i].RenderTransform = st1;

                    _columnTops[i].Width = width;
                    //relative height is set here
                    _columnTops[i].Height = depth;

                    _columnTops[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) - _columnTops[i].Height + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));

                    //use4 the same relative width here
                    _columnTops[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + depth + (Double)_parent.PlotArea.GetValue(LeftProperty));


                    _columnTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columnTops[i].StrokeThickness = DataPoints[i].BorderThickness;


                    _columnTops[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    SkewTransform st2 = new SkewTransform();
                    st2.AngleY = 0;
                    st2.CenterX = 0;
                    st2.CenterY = 0;
                    st2.AngleX = -45;
                    _columnTops[i].RenderTransform = st2;

                    _columns[i].Width = width;
                    _columns[i].Height = height;
                    _columns[i].SetValue(LeftProperty, left + (Double)_parent.PlotArea.GetValue(LeftProperty));
                    _columns[i].SetValue(TopProperty, top + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));
                    _columns[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columns[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _columns[i].Opacity = this.Opacity * DataPoints[i].Opacity;


                    int zindex = (int)Math.Round(left + depth);

                    _columns[i].SetValue(ZIndexProperty, zindex + 2);
                    _columnShadows[i].SetValue(ZIndexProperty, zindex + 1);
                    _columnTops[i].SetValue(ZIndexProperty, zindex);

                    _shadows[i].SetValue(TopProperty, (Double)_columnTops[i].GetValue(TopProperty) - ShadowSize);
                    _shadows[i].SetValue(LeftProperty, (Double)_columnTops[i].GetValue(LeftProperty) + ShadowSize);
                    if ((Double)_shadows[i].GetValue(LeftProperty) + width > _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum))
                        _shadows[i].Width = width - ShadowSize;
                    else
                        _shadows[i].Width = width;

                    if ((Double)_columnTops[i].GetValue(TopProperty) + ShadowSize + height > _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum))
                    {
                        _shadows[i].Height = height - ShadowSize;
                    }
                    else
                    {
                        _shadows[i].Height = height;
                    }

                    _shadows[i].Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    _shadows[i].Fill = Parser.ParseSolidColor("#66000000");
                    _shadows[i].SetValue(ZIndexProperty, 3);
                    if (!ShadowEnabled)
                    {
                        _shadows[i].Opacity = 0;
                    }

                    //This part of the code generates the 3d gradients
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "-90;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-45;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                            linbrush += ",1";

                            brushTop = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-120;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";

                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _columnShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _columns[i].Fill = Cloner.CloneBrush(brush2);

                    //Apply transform to the brushTop

                    _columnTops[i].Fill = Cloner.CloneBrush(brushTop);

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    #endregion Column100 3D Plotting

                    DataPoints[i].AttachToolTip(_columnShadows[i]);
                    DataPoints[i].AttachToolTip(_columnTops[i]);

                    DataPoints[i].AttachToolTip(_columns[i]);
                    DataPoints[i].SetValue(TopProperty, top + (depth + initialDepth));
                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)_columns[i].GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],depth);

                    DataPoints[i].AttachHref(_columns[i]);
                    
                }
                else
                {

                    Path column = new Path();
                    Path shadow = new Path();
                    String pathXAML = @"";
                    String maxDP = "", maxDS = "";
                    Double xRadiusLimit = 0;
                    Double yRadiusLimit = 0;
                    if (DataPoints[i].YValue > 0)
                    {
                        GetPositiveMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }
                    else
                    {
                        GetNegativeMax(DataPoints[i].XValue, ref maxDP, ref maxDS);
                    }


                    if (maxDS == this.Name && maxDP == DataPoints[i].Name)
                    {
                        xRadiusLimit = DataPoints[i].RadiusX;
                        yRadiusLimit = DataPoints[i].RadiusY;

                        if (DataPoints[i].YValue >= 0)
                            shadow.Height = height - ShadowSize;
                        else
                            shadow.Height = height + ShadowSize;
                        shadow.SetValue(TopProperty, ShadowSize);
                    }
                    else
                    {
                        shadow.Height = height;
                        shadow.SetValue(TopProperty, 0);
                    }


                    if (xRadiusLimit > width / 2) xRadiusLimit = width / 2;
                    if (yRadiusLimit > height) yRadiusLimit = height;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", xRadiusLimit, 0);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    DataPoints[i].Children.Add(column);
                    DataPoints[i].Children.Add(shadow);

                    column.Data = (PathGeometry)XamlReader.Load(pathXAML);
                    column.Width = width;
                    column.Height = height;
                    column.SetValue(TopProperty, 0);
                    column.SetValue(LeftProperty, 0);




                    shadow.Width = width;
                    shadow.SetValue(LeftProperty, ShadowSize);


                    if (xRadiusLimit > shadow.Width / 2) xRadiusLimit = shadow.Width / 2;
                    if (yRadiusLimit > height) yRadiusLimit = height;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", xRadiusLimit, 0);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    shadow.Data = (PathGeometry)XamlReader.Load(pathXAML);

                    if (DataPoints[i].YValue < 0)
                    {
                        ScaleTransform st = new ScaleTransform();
                        st.ScaleX = 1;
                        st.ScaleY = -1;
                        column.RenderTransformOrigin = new Point(0.5, 0.5);
                        column.RenderTransform = st;

                        st = new ScaleTransform();
                        st.ScaleX = 1;
                        st.ScaleY = -1;
                        shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                        shadow.RenderTransform = st;
                        if (maxDS == this.Name && maxDP == DataPoints[i].Name)
                        {
                            shadow.SetValue(TopProperty, -ShadowSize);
                        }
                        else
                        {
                            shadow.SetValue(TopProperty, 0);
                        }
                    }

                    column.Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    column.StrokeThickness = DataPoints[i].BorderThickness;

                    String linbrush;
                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
                    {


                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        column.Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && Bevel)
                    {
                        
                        if (DataPoints[i].YValue > 0) linbrush = "-90;";
                        else linbrush = "90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor(brush.Color, 0.99);
                        linbrush += ",1";

                        column.Fill = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        column.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }


                    column.Opacity = this.Opacity * DataPoints[i].Opacity;
                    shadow.Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;

                    shadow.Fill = Parser.ParseSolidColor("#66000000");


                    shadow.SetValue(ZIndexProperty, 1);
                    column.SetValue(ZIndexProperty, 5);
                    if (!ShadowEnabled)
                    {
                        shadow.Opacity = 0;
                    }

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            
                            column.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            
                            column.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(column);
                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)column.GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i],0);

                    DataPoints[i].AttachHref(column);
                    DataPoints[i].ApplyEffects((int)column.GetValue(ZIndexProperty) + 1);
                }
                
            }
        }

        private void PlotStackedArea()
        {
            Int32 i;
            Double height = 0;
            Visifire.Charts.Marker marker;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["stackedarea"];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
            Point[] points = new Point[4]; // Points for the Plolygon
            Double labeldepth = (depth + initialDepth);
            
            for (i = 0; i < 4; i++) points[i] = new Point();

            if (Index == 0)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<double, Point>();

            for (i = 0; i < _areas.Length; i++)
            {
                //Ignore current datapoint
                if (Double.IsNaN(DataPoints[i].YValue)) continue;
                

                if (_parent.View3D)
                {
                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - (depth + initialDepth);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[i + 1].XValue) - (depth + initialDepth);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - (depth + initialDepth);
                }
                else
                {
                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) ;
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[i + 1].XValue);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) ;
                }


                points[0].Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue);
                points[1].Y = _parent.AxisY.DoubleToPixel(DataPoints[i + 1].YValue);
                points[2].Y = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum > 0 ? _parent.AxisY.AxisMinimum : 0);
                points[3].Y = points[2].Y;


                if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                {
                    height = (points[3].Y - points[0].Y);

                    points[3].Y = Plot.TopBottom[DataPoints[i].XValue].Y;
                    points[0].Y = points[3].Y - height;

                    Plot.TopBottom[DataPoints[i].XValue] = new Point(0, points[0].Y);
                }
                else
                {
                    Plot.TopBottom[DataPoints[i].XValue] = new Point(0, points[0].Y);
                }

                // Ignore the next data point, also skip drawing the area
                if (Double.IsNaN(DataPoints[i + 1].YValue))
                {
                    i++;
                    continue;
                }


                if (Plot.TopBottom.ContainsKey(DataPoints[i + 1].XValue))
                {
                    height = (points[2].Y - points[1].Y);

                    points[2].Y = Plot.TopBottom[DataPoints[i + 1].XValue].Y;
                    points[1].Y = points[2].Y - height;

                    if (DataPoints[i].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[i + 1].XValue] = new Point(0, points[1].Y);
                }
                else
                {
                    if (DataPoints[i].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[i + 1].XValue] = new Point(0, points[1].Y);
                }


                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;
                if (!_parent.View3D)
                {

                    DataPoints[i].SetValue(LeftProperty, points[0].X);
                    DataPoints[i + 1].SetValue(LeftProperty, points[1].X);

                    DataPoints[i].SetValue(TopProperty, points[0].Y);
                    DataPoints[i + 1].SetValue(TopProperty, points[1].Y);

                    _areas[i].Points = Converter.ArrayToCollection(points);

                    Double left = (Double)DataPoints[i].GetValue(LeftProperty);
                    Double top = (Double)DataPoints[i].GetValue(TopProperty);
                    DataPoints[i].points.Add(new Point(points[0].X - left, points[0].Y - top));
                    DataPoints[i].points.Add(new Point(points[1].X - left, points[1].Y - top));
                    DataPoints[i].points.Add(new Point(points[2].X - left, points[2].Y - top));
                    DataPoints[i].points.Add(new Point(points[3].X - left, points[3].Y - top));


                    _areas[i].SetValue(TopProperty, -top);
                    _areas[i].SetValue(LeftProperty, -left);

                    _areas[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areas[i].StrokeThickness = DataPoints[i].BorderThickness;

                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        String linbrush;
                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        _areas[i].Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else
                    {
                        _areas[i].Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_areas[i]);
                    DataPoints[i].ApplyEffects((int)_areas[i].GetValue(ZIndexProperty)+1);
                }
                else
                {
                    

                    DataPoints[i].SetValue(LeftProperty, points[0].X);
                    DataPoints[i + 1].SetValue(LeftProperty, points[1].X);

                    DataPoints[i].SetValue(TopProperty, points[0].Y + (depth + initialDepth));
                    DataPoints[i + 1].SetValue(TopProperty, points[1].Y + (depth + initialDepth));

                    points[0].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[1].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[2].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[3].X += (Double)_parent.PlotArea.GetValue(LeftProperty);

                    points[0].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[1].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[2].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[3].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);

                    _areas[i].Points = Converter.ArrayToCollection(points);

                    _areas[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areaShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areaTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);


                    _areas[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _areaShadows[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _areaTops[i].StrokeThickness = DataPoints[i].BorderThickness;

                    _areas[i].Opacity = Opacity * DataPoints[i].Opacity;
                    _areaShadows[i].Opacity = Opacity * DataPoints[i].Opacity;
                    _areaTops[i].Opacity = Opacity * DataPoints[i].Opacity;


                    _areaShadows[i].Height = points[2].Y - points[1].Y;
                    _areaShadows[i].Width = depth;
                    _areaShadows[i].SetValue(TopProperty, points[1].Y);
                    _areaShadows[i].SetValue(LeftProperty, points[1].X);

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _areaShadows[i].RenderTransform = st1;


                    points[0].X += depth;
                    points[1].X += depth;

                    points[2].Y = points[1].Y;
                    points[3].Y = points[0].Y;
                    points[0].Y -= depth;
                    points[1].Y -= depth;


                    _areaTops[i].Points = Converter.ArrayToCollection(points);

                    //This part of the code generates the 3D gradient
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "-90;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-45;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                            linbrush += ",1";

                            brushTop = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-120;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";

                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _areas[i].Fill = Cloner.CloneBrush(brush2);
                    _areaShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _areaTops[i].Fill = Cloner.CloneBrush(brushTop);

                    _areas[i].SetValue(ZIndexProperty, 10);
                    _areaShadows[i].SetValue(ZIndexProperty, 5);
                    _areaTops[i].SetValue(ZIndexProperty, 5);

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _areaShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _areaShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_areas[i]);
                    DataPoints[i].AttachToolTip(_areaShadows[i]);
                    DataPoints[i].AttachToolTip(_areaTops[i]);
                }
                
                marker = DataPoints[i].PlaceMarker();
                if (marker != null)
                {
                    marker.SetValue(ZIndexProperty, (int)_areas[i].GetValue(ZIndexProperty) + 1);
                    DataPoints[i].AttachToolTip(marker);
                }

                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i],labeldepth);

            }
            i = _areas.Length;
            marker = DataPoints[i].PlaceMarker();
            if (marker != null)
            {
                marker.SetValue(ZIndexProperty, (int)_areas[i - 1].GetValue(ZIndexProperty) + 1);
                DataPoints[i].AttachToolTip(marker);
            }
            DataPoints[i].Width = 1;
            DataPoints[i].Height = 1;
            if (DataPoints[i].LabelEnabled.ToLower() == "true")
                DataPoints[i].AttachLabel(DataPoints[i],labeldepth);
        }

        private void PlotStackedArea100()
        {
            Int32 i;
            Double height = 0;
            Visifire.Charts.Marker marker;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["stackedarea100"];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
            Point[] points = new Point[4]; // Points for the Plolygon
            Double labelDepth = (depth + initialDepth);


            for (i = 0; i < 4; i++) points[i] = new Point();

            if (Index == 0)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<double, Point>();

            for (i = 0; i < _areas.Length; i++)
            {

                
                if (_parent.View3D)
                {

                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - (depth + initialDepth);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[i + 1].XValue) - (depth + initialDepth);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - (depth + initialDepth);
                }
                else
                {
                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) ;
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[i + 1].XValue);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) ;
                }

                points[0].Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue / Plot.YValueSum[DataPoints[i].XValue] * 100);
                points[1].Y = _parent.AxisY.DoubleToPixel(DataPoints[i + 1].YValue / Plot.YValueSum[DataPoints[i+1].XValue] * 100);
                points[2].Y = _parent.AxisY.DoubleToPixel((_parent.AxisY.AxisMinimum > 0 ? _parent.AxisY.AxisMinimum : 0));
                points[3].Y = points[2].Y;


                if (Plot.TopBottom.ContainsKey(DataPoints[i].XValue))
                {
                    height = (points[3].Y - points[0].Y);

                    points[3].Y = Plot.TopBottom[DataPoints[i].XValue].Y;
                    points[0].Y = points[3].Y - height;

                    Plot.TopBottom[DataPoints[i].XValue] = new Point(0, points[0].Y);
                }
                else
                {
                    Plot.TopBottom[DataPoints[i].XValue] = new Point(0, points[0].Y);
                }

                

                if (Plot.TopBottom.ContainsKey(DataPoints[i + 1].XValue))
                {
                    height = (points[2].Y - points[1].Y);

                    points[2].Y = Plot.TopBottom[DataPoints[i + 1].XValue].Y;
                    points[1].Y = points[2].Y - height;

                    if (DataPoints[i].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[i + 1].XValue] = new Point(0, points[1].Y);
                }
                else
                {
                    if (DataPoints[i].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[i + 1].XValue] = new Point(0, points[1].Y);
                }

                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;
                if (!_parent.View3D)
                {
                    _areas[i].Points = Converter.ArrayToCollection(points);


                    DataPoints[i].SetValue(LeftProperty, points[0].X);
                    DataPoints[i + 1].SetValue(LeftProperty, points[1].X);

                    DataPoints[i].SetValue(TopProperty, points[0].Y);
                    DataPoints[i + 1].SetValue(TopProperty, points[1].Y);


                    Double left = (Double)DataPoints[i].GetValue(LeftProperty);
                    Double top = (Double)DataPoints[i].GetValue(TopProperty);
                    DataPoints[i].points.Add(new Point(points[0].X - left, points[0].Y - top));
                    DataPoints[i].points.Add(new Point(points[1].X - left, points[1].Y - top));
                    DataPoints[i].points.Add(new Point(points[2].X - left, points[2].Y - top));
                    DataPoints[i].points.Add(new Point(points[3].X - left, points[3].Y - top));


                    _areas[i].SetValue(TopProperty, -top);
                    _areas[i].SetValue(LeftProperty, -left);

                    _areas[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areas[i].StrokeThickness = DataPoints[i].BorderThickness;


                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        String linbrush;
                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        _areas[i].Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else
                    {
                        _areas[i].Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_areas[i]);
                    DataPoints[i].ApplyEffects((int)_areas[i].GetValue(ZIndexProperty) + 1);
                }
                else
                {
                    

                    DataPoints[i].SetValue(LeftProperty, points[0].X);
                    DataPoints[i + 1].SetValue(LeftProperty, points[1].X);

                    DataPoints[i].SetValue(TopProperty, points[0].Y + (depth + initialDepth));
                    DataPoints[i + 1].SetValue(TopProperty, points[1].Y + (depth + initialDepth));

                    points[0].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[1].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[2].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[3].X += (Double)_parent.PlotArea.GetValue(LeftProperty);

                    points[0].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[1].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[2].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[3].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);

                    _areas[i].Points = Converter.ArrayToCollection(points);

                    _areas[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areaShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areaTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);


                    _areas[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _areaShadows[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _areaTops[i].StrokeThickness = DataPoints[i].BorderThickness;

                    _areas[i].Opacity = Opacity * DataPoints[i].Opacity;
                    _areaShadows[i].Opacity = Opacity * DataPoints[i].Opacity;
                    _areaTops[i].Opacity = Opacity * DataPoints[i].Opacity;

                    _areaShadows[i].Height = points[2].Y - points[1].Y;
                    _areaShadows[i].Width = depth;
                    _areaShadows[i].SetValue(TopProperty, points[1].Y);
                    _areaShadows[i].SetValue(LeftProperty, points[1].X);

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _areaShadows[i].RenderTransform = st1;


                    points[0].X += depth;
                    points[1].X += depth;

                    points[2].Y = points[1].Y;
                    points[3].Y = points[0].Y;
                    points[0].Y -= depth;
                    points[1].Y -= depth;


                    _areaTops[i].Points = Converter.ArrayToCollection(points);

                    //This part of the code generates the 3D gradient
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "-90;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-45;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                            linbrush += ",1";

                            brushTop = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-120;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";

                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _areas[i].Fill = Cloner.CloneBrush(brush2);
                    _areaShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _areaTops[i].Fill = Cloner.CloneBrush(brushTop);

                    _areas[i].SetValue(ZIndexProperty, 10);
                    _areaShadows[i].SetValue(ZIndexProperty, 5);
                    _areaTops[i].SetValue(ZIndexProperty, 5);

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _areaShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _areaShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_areas[i]);
                    DataPoints[i].AttachToolTip(_areaShadows[i]);
                    DataPoints[i].AttachToolTip(_areaTops[i]);
                }
                marker = DataPoints[i].PlaceMarker();
                if (marker != null)
                {
                    marker.SetValue(ZIndexProperty, (int)_areas[i].GetValue(ZIndexProperty) + 1);
                    DataPoints[i].AttachToolTip(marker);
                }
                
                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i],depth);

            }
            i = _areas.Length;
            marker = DataPoints[i].PlaceMarker();
            if (marker != null)
            {
                marker.SetValue(ZIndexProperty, (int)_areas[i - 1].GetValue(ZIndexProperty) + 1);
                DataPoints[i].AttachToolTip(marker);
            }
            DataPoints[i].Width = 1;
            DataPoints[i].Height = 1;
            if (DataPoints[i].LabelEnabled.ToLower() == "true")
                DataPoints[i].AttachLabel(DataPoints[i],depth);
        }

        

        private void CreateAuxPath(ref Path path, int index, Brush brush)
        {
            path = new Path();
            path.Fill = Cloner.CloneBrush(brush);
            path.Opacity = Opacity * DataPoints[index].Opacity;
            DataPoints[index].AttachHref(path);
            DataPoints[index].AttachToolTip(path);
        }

        

        /// <summary>
        /// Validate Parent element and assign it to _parent.
        /// Parent element should be a Canvas element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            if (this.Parent.GetType().Name == "Chart")
                _parent = this.Parent as Chart;
            else
                throw new Exception(this + "Parent should be a Chart");
        }

        /// <summary>
        /// Create reference to all DataPoints.
        /// </summary>
        private void CreateReferences()
        {
            DataPoint dataPoint;
            Int32 dataPointIndex;

            dataPointIndex = 0;
            foreach (FrameworkElement child in this.Children)
            {
                if (child.GetType().Name == "DataPoint")
                {

                    dataPoint = (child as DataPoint);
                    if (!dataPoint.Enabled) continue;
                    dataPoint.Index = dataPointIndex;
                    dataPointIndex++;
                    dataPoint.Init();
                    DataPoints.Add(dataPoint);
                    _parent.CollectStackContent(dataPoint.XValue, dataPoint.YValue);
                    if (_maxVals.ContainsKey(dataPoint.XValue))
                    {
                        if (dataPoint.YValue > 0)
                            _maxVals[dataPoint.XValue] = new Point(Math.Max(_maxVals[dataPoint.XValue].X, dataPoint.YValue), _maxVals[dataPoint.XValue].Y);
                        else
                            _maxVals[dataPoint.XValue] = new Point(_maxVals[dataPoint.XValue].X, Math.Min(_maxVals[dataPoint.XValue].Y, dataPoint.YValue));
                    }
                    else
                    {
                        if (dataPoint.YValue > 0)
                            _maxVals.Add(dataPoint.XValue,new Point(dataPoint.YValue,0));
                        else
                            _maxVals.Add(dataPoint.XValue, new Point(0,dataPoint.YValue));
                    }
                }
            }
            DataPoints.Sort();
        }

        #endregion Private Methods

        #region Internal Methods

        internal void DrawPlotBorder()
        {
            _borderRectangle.Width = this.Width;
            _borderRectangle.Height = Math.Abs(this.Height);

            _borderRectangle.Stroke = Cloner.CloneBrush(_parent.PlotArea.BorderColor);
            _borderRectangle.StrokeThickness = _parent.PlotArea.BorderThickness;
            _borderRectangle.RadiusX = _parent.PlotArea.RadiusX;
            _borderRectangle.RadiusY = _parent.PlotArea.RadiusY;

            switch (_parent.PlotArea.BorderStyle)
            {
                case "Solid":
                    break;

                case "Dashed":
                    _borderRectangle.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                    break;

                case "Dotted":
                    _borderRectangle.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                    break;
            }
            Children.Add(_borderRectangle);

            _parent._plotAreaBorder = _borderRectangle;
            _parent._plotAreaBorder.SetValue(NameProperty, _parent.GetNewObjectName(_parent._plotAreaBorder));
            _parent._plotAreaBorder.Opacity = 0;
        }

        internal void PlotData()
        {


            switch (RenderAs.ToUpper())
            {
                case "LINE":
                    PlotLine();
                    break;

                case "AREA":
                    PlotArea();
                    break;

                case "PIE":
                    if (_parent.View3D) PlotPie3D();
                    else PlotPie();
                    break;

                case "DOUGHNUT":
                    if (_parent.View3D) PlotDoughnut3D();
                    else PlotDoughnut();
                    break;

                case "STACKEDAREA":
                    PlotStackedArea();
                    break;

                case "STACKEDAREA100":
                    PlotStackedArea100();
                    break;

                case "COLUMN":
                    PlotColumn();
                    break;

                case "STACKEDCOLUMN":
                    PlotStackedColumn();
                    break;

                case "STACKEDCOLUMN100":
                    PlotStackedColumn100();
                    break;

                case "BAR":
                    PlotBar();
                    break;

                case "BUBBLE":
                    PlotBubble();
                    break;

                case "STACKEDBAR":
                    PlotStackedBar();
                    break;

                case "STACKEDBAR100":
                    PlotStackedBar100();
                    break;

                case "POINT":
                    PlotPoint();
                    break;

                default:

                    break;
            }
            if (_parent.PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {
                if (!_parent.View3D)
                {
                    if (!_parent.BorderDrawn)
                        DrawPlotBorder();
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(0, 0, _borderRectangle.Width, _borderRectangle.Height);
                    rg.RadiusX = RadiusX + BorderThickness / 2;
                    rg.RadiusY = RadiusY + BorderThickness / 2;
                    this.Clip = rg;
                }
                if (_parent.View3D && RenderAs.ToLower() == "bubble")
                {
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(0, 0, _borderRectangle.Width, _borderRectangle.Height);
                    rg.RadiusX = RadiusX + BorderThickness / 2;
                    rg.RadiusY = RadiusY + BorderThickness / 2;
                    this.Clip = rg;
                }
            }


        }

        internal void PlotColumn()
        {
            Double width = 10;
            Double height;
            Double left;
            Double top;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            int i;

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
            {
                width = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);
            }
            else
            {
                width = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);

            }

            Double temp = Math.Abs(_parent.AxisX.DoubleToPixel(Plot.MinAxisXValue) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum));
            temp *= 2;
            if (width > temp)
                width = temp;

            width -= width * 0.1;

            List<Double> checkDrawPositive = new List<double>();
            List<Double> checkDrawNegetive = new List<double>();
            Double finalYValue;
            for (i = 0; i < _columns.Length; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);

                if (!checkDrawPositive.Contains(DataPoints[i].XValue) && DataPoints[i].YValue >= 0)
                {
                    finalYValue = _maxVals[DataPoints[i].XValue].X;
                    if (DataPoints[i].YValue != finalYValue) continue;
                    checkDrawPositive.Add(DataPoints[i].XValue);
                }
                else if (!checkDrawNegetive.Contains(DataPoints[i].XValue) && DataPoints[i].YValue < 0)
                {
                    finalYValue = _maxVals[DataPoints[i].XValue].Y;
                    if (DataPoints[i].YValue != finalYValue) continue;
                    checkDrawNegetive.Add(DataPoints[i].XValue);
                }
                else
                {
                    continue;
                }
                point.Y = _parent.AxisY.DoubleToPixel(finalYValue);

                if (_parent.AxisY.AxisMinimum > 0)
                    height = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum) - point.Y;
                else
                    height = Math.Abs(_parent.AxisY.DoubleToPixel(0) - point.Y);


                left = (point.X + Index * width - (TotalSiblings * width) / 2);



                if (DataPoints[i].YValue >= 0)
                    top = point.Y;
                else
                    top = _parent.AxisY.DoubleToPixel(0);


                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, left);
                DataPoints[i].SetValue(TopProperty, top);
                Path column = new Path();

                if (_parent.View3D)
                {


                    //To plot in 3D
                    #region Column 3D Plotting
                    //relative width is set here

                    Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["column"];
                    Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;


                    left -= (depth + initialDepth);
                    DataPoints[i].SetValue(LeftProperty, left);


                    _columnShadows[i].Width = depth;
                    _columnShadows[i].Height = height;
                    _columnShadows[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));
                    _columnShadows[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + width + (Double)_parent.PlotArea.GetValue(LeftProperty));

                    _columnShadows[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    _columnShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);

                    _columnShadows[i].StrokeThickness = DataPoints[i].BorderThickness;

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _columnShadows[i].RenderTransform = st1;

                    _columnTops[i].Width = width;
                    //relative height is set here
                    _columnTops[i].Height = depth;

                    _columnTops[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) - _columnTops[i].Height + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth));

                    //use4 the same relative width here
                    _columnTops[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + depth + (Double)_parent.PlotArea.GetValue(LeftProperty));


                    _columnTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columnTops[i].StrokeThickness = DataPoints[i].BorderThickness;


                    _columnTops[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    SkewTransform st2 = new SkewTransform();
                    st2.AngleY = 0;
                    st2.CenterX = 0;
                    st2.CenterY = 0;
                    st2.AngleX = -45;
                    _columnTops[i].RenderTransform = st2;

                    _shadows[i].SetValue(TopProperty, (Double)_columnTops[i].GetValue(TopProperty) - ShadowSize);
                    _shadows[i].SetValue(LeftProperty, (Double)_columnTops[i].GetValue(LeftProperty) + ShadowSize);
                    if ((Double)_shadows[i].GetValue(LeftProperty) + width > _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum))
                        _shadows[i].Width = width - ShadowSize;
                    else
                        _shadows[i].Width = width;
                    _shadows[i].Height = _columnShadows[i].Height + ShadowSize;
                    _shadows[i].Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    _shadows[i].Fill = Parser.ParseSolidColor("#66000000");
                    //_shadows[i].OpacityMask = Parser.ParseLinearGradient("0;#FF000000,0;#66666666,1");

                    if (!ShadowEnabled)
                    {
                        _shadows[i].Opacity = 0;
                    }

                    _columns[i].Width = width;
                    _columns[i].Height = height;
                    _columns[i].SetValue(LeftProperty, left + (Double)_parent.PlotArea.GetValue(LeftProperty));
                    _columns[i].SetValue(TopProperty, top + (Double)_parent.PlotArea.GetValue(TopProperty) + depth + initialDepth);
                    _columns[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columns[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _columns[i].Opacity = this.Opacity * DataPoints[i].Opacity;





                    _shadows[i].SetValue(ZIndexProperty, 2);
                    int Zi = (int)(left + Math.Abs(left) + depth);

                    if (DataPoints[i].YValue > 0)
                    {
                        _columnTops[i].SetValue(ZIndexProperty, Zi - 1);
                        _columnShadows[i].SetValue(ZIndexProperty, Zi - 2);
                        _columns[i].SetValue(ZIndexProperty, Zi);
                    }
                    else
                    {
                        _columnTops[i].SetValue(ZIndexProperty, Zi - 11);
                        _columnShadows[i].SetValue(ZIndexProperty, Zi - 12);
                        _columns[i].SetValue(ZIndexProperty, Zi - 10);
                    }
                    //This part of the code generates the 3d gradients
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "-90;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-45;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                            linbrush += ",1";

                            brushTop = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-120;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";

                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _columnShadows[i].Fill = Cloner.CloneBrush(brushShade);

                    _columns[i].Fill = Cloner.CloneBrush(brush2);


                    //Apply transform to the brushTop

                    _columnTops[i].Fill = Cloner.CloneBrush(brushTop);

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    #endregion Column 3D Plotting

                    DataPoints[i].AttachToolTip(_columnShadows[i]);
                    DataPoints[i].AttachToolTip(_columnTops[i]);
                    DataPoints[i].AttachHref(_columnShadows[i]);
                    DataPoints[i].AttachHref(_columnTops[i]);
                    DataPoints[i].AttachToolTip(_columns[i]);
                    DataPoints[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + depth + initialDepth);
                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)_columns[i].GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i], depth);

                    DataPoints[i].AttachHref(_columns[i]);

                }
                else
                {
                    //To plot in 2d use this
                    #region Column 2D Plotting


                    Path shadow = new Path();
                    String pathXAML = @"";
                    Double xRadiusLimit = DataPoints[i].RadiusX;
                    Double yRadiusLimit = DataPoints[i].RadiusY;

                    if (xRadiusLimit > width / 2) xRadiusLimit = width / 2;
                    if (yRadiusLimit > height) yRadiusLimit = height;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", xRadiusLimit, 0);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    DataPoints[i].Children.Add(column);
                    DataPoints[i].Children.Add(shadow);

                    column.Data = (PathGeometry)XamlReader.Load(pathXAML);
                    column.Width = width;
                    column.Height = height;
                    column.SetValue(TopProperty, 0);
                    column.SetValue(LeftProperty, 0);




                    shadow.Width = width;
                    shadow.Height = height - ShadowSize;
                    shadow.SetValue(TopProperty, ShadowSize);
                    shadow.SetValue(LeftProperty, ShadowSize);


                    if (xRadiusLimit > shadow.Width / 2) xRadiusLimit = shadow.Width / 2;
                    if (yRadiusLimit > height) yRadiusLimit = height;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", xRadiusLimit, 0);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    shadow.Data = (PathGeometry)XamlReader.Load(pathXAML);

                    if (DataPoints[i].YValue < 0)
                    {
                        ScaleTransform st = new ScaleTransform();
                        st.ScaleX = 1;
                        st.ScaleY = -1;
                        column.RenderTransformOrigin = new Point(0.5, 0.5);
                        column.RenderTransform = st;

                        st = new ScaleTransform();
                        st.ScaleX = 1;
                        st.ScaleY = -1;
                        shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                        shadow.RenderTransform = st;
                        shadow.SetValue(TopProperty, ShadowSize);
                    }

                    column.Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    column.StrokeThickness = DataPoints[i].BorderThickness;

                    String linbrush;
                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
                    {


                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        column.Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && Bevel)
                    {

                        if (DataPoints[i].YValue > 0) linbrush = "-90;";
                        else linbrush = "90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor(brush.Color, 0.99);
                        linbrush += ",1";

                        column.Fill = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        column.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }


                    column.Opacity = this.Opacity * DataPoints[i].Opacity;
                    shadow.Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;

                    shadow.Fill = Parser.ParseSolidColor("#66000000");


                    shadow.SetValue(ZIndexProperty, 1);
                    column.SetValue(ZIndexProperty, 5);
                    if (!ShadowEnabled)
                    {
                        shadow.Opacity = 0;
                    }

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":

                            column.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":

                            column.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(column);
                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)column.GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i], 0);

                    DataPoints[i].AttachHref(column);
                    DataPoints[i].ApplyEffects((int)column.GetValue(ZIndexProperty) + 1);

                    #endregion Column 2D Plotting


                }

            }

        }

        internal void PlotBar()
        {
            Double width = 10;
            Double height;
            Double left;
            Double top;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            int i;

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
                height = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);
            else
                height = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / TotalSiblings);

            Double temp = Math.Abs(_parent.AxisX.DoubleToPixel(Plot.MinAxisXValue) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum));
            temp *= 2;
            if (height > temp)
                height = temp;

            height -= height * 0.3;

            List<Double> checkDrawPositive = new List<double>();
            List<Double> checkDrawNegetive = new List<double>();
            Double finalYValue;
            for (i = 0; i < _columns.Length; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);

                if (!checkDrawPositive.Contains(DataPoints[i].XValue) && DataPoints[i].YValue >= 0)
                {
                    finalYValue = _maxVals[DataPoints[i].XValue].X;
                    if (DataPoints[i].YValue != finalYValue) continue;
                    checkDrawPositive.Add(DataPoints[i].XValue);
                }
                else if (!checkDrawNegetive.Contains(DataPoints[i].XValue) && DataPoints[i].YValue < 0)
                {
                    finalYValue = _maxVals[DataPoints[i].XValue].Y;
                    if (DataPoints[i].YValue != finalYValue) continue;
                    checkDrawNegetive.Add(DataPoints[i].XValue);
                }
                else
                {
                    continue;
                }
                point.Y = _parent.AxisY.DoubleToPixel(finalYValue);

                top = point.X + Index * height - (TotalSiblings * height) / 2;

                if (_parent.AxisY.AxisMinimum > 0)
                    width = Math.Abs(point.Y - _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum));
                else
                    width = Math.Abs(point.Y - _parent.AxisY.DoubleToPixel(0));


                if (_parent.AxisY.AxisMinimum > 0)
                {
                    left = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum);
                }
                else
                {
                    if (DataPoints[i].YValue > 0)
                        left = _parent.AxisY.DoubleToPixel(0);
                    else
                        left = _parent.AxisY.DoubleToPixel(0) - width;
                }

                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, left);
                DataPoints[i].SetValue(TopProperty, top);

                Path bar = new Path();

                if (_parent.View3D)
                {

                    // Add zero plane if required


                    //To plot in 3D
                    #region Bar 3D Plotting
                    //relative width is set here
                    Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["bar"];
                    Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

                    top += (depth + initialDepth);
                    DataPoints[i].SetValue(TopProperty, top);

                    _columnShadows[i].Width = depth;
                    _columnShadows[i].Height = height;
                    _columnShadows[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columnShadows[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + width + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth));

                    _columnShadows[i].Opacity = this.Opacity * DataPoints[i].Opacity;
                    _columnShadows[i].SetValue(ZIndexProperty, 7);
                    _columnShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);



                    _columnShadows[i].StrokeThickness = DataPoints[i].BorderThickness;

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _columnShadows[i].RenderTransform = st1;

                    _columnTops[i].Width = width;
                    //relative height is set here
                    _columnTops[i].Height = depth;

                    _columnTops[i].SetValue(TopProperty, (Double)DataPoints[i].GetValue(TopProperty) - _columnTops[i].Height + (Double)_parent.PlotArea.GetValue(TopProperty));

                    //use4 the same relative width here
                    _columnTops[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) + (Double)_parent.PlotArea.GetValue(LeftProperty) - initialDepth);


                    _columnTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columnTops[i].StrokeThickness = DataPoints[i].BorderThickness;


                    _columnTops[i].Opacity = this.Opacity * DataPoints[i].Opacity;

                    SkewTransform st2 = new SkewTransform();
                    st2.AngleY = 0;
                    st2.CenterX = 0;
                    st2.CenterY = 0;
                    st2.AngleX = -45;
                    _columnTops[i].RenderTransform = st2;

                    _columns[i].Width = width;
                    _columns[i].Height = height;
                    _columns[i].SetValue(LeftProperty, left + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth));
                    _columns[i].SetValue(TopProperty, top + (Double)_parent.PlotArea.GetValue(TopProperty));
                    _columns[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _columns[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _columns[i].Opacity = this.Opacity * DataPoints[i].Opacity;


                    //This part of the code generates the 3d gradients
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.5);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";
                            brushTop = Parser.ParseLinearGradient(linbrush);


                            linbrush = "0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.95);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",1";
                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _columnShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _columns[i].Fill = Cloner.CloneBrush(brush2);

                    //Apply transform to the brushTop

                    _columnTops[i].Fill = Cloner.CloneBrush(brushTop);

                    _shadows[i].SetValue(TopProperty, (Double)_columnTops[i].GetValue(TopProperty) - _columnShadows[i].Height * 0.1);
                    _shadows[i].SetValue(LeftProperty, (Double)_columnTops[i].GetValue(LeftProperty));
                    _shadows[i].Width = width + ShadowSize;
                    _shadows[i].Height = _columnShadows[i].Height;
                    _shadows[i].Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    _shadows[i].Fill = Parser.ParseSolidColor("#66000000");

                    _shadows[i].SetValue(ZIndexProperty, 2);

                    int zindex = (int)((Double)_parent.PlotArea.GetValue(TopProperty) + _parent.PlotArea.Height - top);
                    zindex = (int)(Math.Sqrt(Math.Pow(left, 2) + Math.Pow(zindex, 2)));
                    _columnTops[i].SetValue(ZIndexProperty, zindex - 2);
                    _columnShadows[i].SetValue(ZIndexProperty, zindex - 1);
                    _columns[i].SetValue(ZIndexProperty, zindex);

                    if (!ShadowEnabled)
                    {
                        _shadows[i].Opacity = 0;
                    }

                    DataPoints[i].SetValue(LeftProperty, (Double)DataPoints[i].GetValue(LeftProperty) - (depth + initialDepth));

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _columns[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _columnTops[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    #endregion Bar 3D Plotting

                    DataPoints[i].AttachToolTip(_columnShadows[i]);
                    DataPoints[i].AttachToolTip(_columnTops[i]);
                    DataPoints[i].AttachToolTip(_columns[i]);



                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)_columns[i].GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i], depth);

                    DataPoints[i].AttachHref(_columns[i]);
                    DataPoints[i].AttachHref(_columnShadows[i]);
                    DataPoints[i].AttachHref(_columnTops[i]);
                }
                else
                {

                    #region Bar2D
                    Path shadow = new Path();
                    String pathXAML = @"";
                    Double xRadiusLimit = DataPoints[i].RadiusX;
                    Double yRadiusLimit = DataPoints[i].RadiusY;

                    if (xRadiusLimit > width) xRadiusLimit = width;
                    if (yRadiusLimit > height / 2) yRadiusLimit = height / 2;

                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width - xRadiusLimit, 0);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width, yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", width, height - yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", width - xRadiusLimit, height);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    DataPoints[i].Children.Add(bar);
                    DataPoints[i].Children.Add(shadow);

                    bar.Width = width;
                    bar.Height = height;

                    bar.SetValue(TopProperty, 0);
                    bar.SetValue(LeftProperty, 0);
                    bar.Data = (PathGeometry)XamlReader.Load(pathXAML);



                    shadow.Width = width;
                    shadow.Height = ShadowSize;

                    shadow.SetValue(TopProperty, height);
                    shadow.SetValue(LeftProperty, 0);



                    pathXAML = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathXAML += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", shadow.Width - xRadiusLimit, 0);



                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", shadow.Width, -yRadiusLimit);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Counterclockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", shadow.Width, shadow.Height - yRadiusLimit);

                    pathXAML += String.Format(@"<ArcSegment Point=""{0},{1}"" ", shadow.Width - xRadiusLimit, shadow.Height);
                    pathXAML += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", xRadiusLimit, yRadiusLimit);
                    pathXAML += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, shadow.Height);
                    pathXAML += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);

                    pathXAML += String.Format("</PathFigure.Segments></PathFigure>");
                    pathXAML += String.Format("</PathGeometry.Figures></PathGeometry>");

                    shadow.Data = (PathGeometry)XamlReader.Load(pathXAML);


                    bar.Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    bar.StrokeThickness = DataPoints[i].BorderThickness;
                    String linbrush;
                    SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
                    {


                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        bar.Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && Bevel)
                    {

                        if (DataPoints[i].YValue > 0) linbrush = "-90;";
                        else linbrush = "90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.80);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor(brush.Color, 0.99);
                        linbrush += ",1";
                        bar.Fill = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        bar.Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    bar.SetValue(ZIndexProperty, 5);

                    if (DataPoints[i].YValue < 0)
                    {
                        ScaleTransform st = new ScaleTransform();
                        st.ScaleX = -1;
                        st.ScaleY = 1;
                        bar.RenderTransformOrigin = new Point(0.5, 0.5);
                        bar.RenderTransform = st;

                        st = new ScaleTransform();
                        st.ScaleX = -1;
                        st.ScaleY = 1;
                        shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                        shadow.RenderTransform = st;
                    }
                    shadow.Opacity = this.Opacity * DataPoints[i].Opacity * 0.8;
                    shadow.Fill = Parser.ParseSolidColor("#66000000");
                    shadow.SetValue(ZIndexProperty, 3);
                    if (!ShadowEnabled)
                    {
                        shadow.Opacity = 0;
                    }
                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            bar.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            bar.StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }

                    DataPoints[i].AttachToolTip(bar);
                    Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                    if (marker != null)
                    {
                        marker.SetValue(ZIndexProperty, (int)bar.GetValue(ZIndexProperty) + 1);
                        DataPoints[i].AttachToolTip(marker);
                    }
                    if (DataPoints[i].LabelEnabled.ToLower() == "true")
                        DataPoints[i].AttachLabel(DataPoints[i], 0);

                    DataPoints[i].AttachHref(bar);
                    if (width > 10 && height > 10)
                        DataPoints[i].ApplyEffects((int)bar.GetValue(ZIndexProperty) + 1);
                    #endregion Bar2D
                }

            }

        }

        internal void PlotLine()
        {
            int i;
            Double strokeThickness = ((_parent.Width * _parent.Height) + 25000) / 35000;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count) * _parent.IndexList["line" + DrawingIndex.ToString()];
            Double depth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count);

            if (Double.IsNaN(initialDepth)) initialDepth = 0;
            if (Double.IsNaN(depth)) depth = _parent.AxisX.MajorTicks.TickLength;

            _line.Stroke = Cloner.CloneBrush(Background);
            _lineShadow.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(127, 127, 127, 127));

            if (!Double.IsNaN(LineThickness) && LineThickness <= 0)
            {
                Debug.WriteLine("LineThichness for line Cahrt cannot be 0 or negative");
                throw (new Exception("Invalid Line Thickness value"));
            }
            else if (!Double.IsNaN(LineThickness))
            {
                strokeThickness = LineThickness;
            }

            _line.StrokeThickness = strokeThickness;
            _lineShadow.StrokeThickness = strokeThickness;

            _line.StrokeLineJoin = PenLineJoin.Round;
            _lineShadow.StrokeLineJoin = PenLineJoin.Round;

            _line.StrokeEndLineCap = PenLineCap.Round;
            _lineShadow.StrokeEndLineCap = PenLineCap.Round;

            _line.StrokeStartLineCap = PenLineCap.Round;
            _lineShadow.StrokeEndLineCap = PenLineCap.Round;

            _line.SetValue(ZIndexProperty, (int)GetValue(ZIndexProperty) + 20);
            _lineShadow.SetValue(ZIndexProperty, (int)_line.GetValue(ZIndexProperty) - 1);



            List<Point> points = new List<Point>();
            List<Point> points2 = new List<Point>();

            Double offsetX = 0, offsetY = 0;
            if (_parent.View3D)
            {
                offsetX = (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);
                offsetY = (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
            }

            for (i = 0; i < DataPoints.Count; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;
                points.Add(new Point(_parent.AxisX.DoubleToPixel(DataPoints[i].XValue) + offsetX, _parent.AxisY.DoubleToPixel(DataPoints[i].YValue) + offsetY));
                points2.Add(new Point(_parent.AxisX.DoubleToPixel(DataPoints[i].XValue) + offsetX + strokeThickness / 2, _parent.AxisY.DoubleToPixel(DataPoints[i].YValue) + offsetY + strokeThickness / 2));

                DataPoints[i].SetValue(LeftProperty, _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - (_parent.View3D ? (depth + initialDepth) : 0));
                DataPoints[i].SetValue(TopProperty, _parent.AxisY.DoubleToPixel(DataPoints[i].YValue) + (_parent.View3D ? (depth + initialDepth) : 0));
                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;


                Visifire.Charts.Marker marker = DataPoints[i].PlaceMarker();
                if (marker != null)
                {
                    marker.SetValue(ZIndexProperty, (int)_line.GetValue(ZIndexProperty) + 1);
                    DataPoints[i].AttachToolTip(marker);
                }
                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i], depth);
            }
            _line.Points = Converter.ArrayToCollection(points.ToArray());
            _lineShadow.Points = Converter.ArrayToCollection(points2.ToArray());
            if (!ShadowEnabled)
            {
                _lineShadow.Opacity = 0;
            }

        }

        internal void PlotArea()
        {
            int i;
            Point[] points = new Point[4];
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList["area" + DrawingIndex.ToString()];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
            Visifire.Charts.Marker marker;
            Double labeldepth = _parent.View3D ? 0 : depth;
            for (i = 0; i < 4; i++) points[i] = new Point();

            foreach (DataPoint datapoint in DataPoints)
            {
                if (_parent.View3D)
                {
                    datapoint.SetValue(LeftProperty, (Double)_parent.AxisX.DoubleToPixel(datapoint.XValue) + (Double)_parent.GetValue(LeftProperty) - (depth + initialDepth));
                    datapoint.SetValue(TopProperty, (Double)_parent.AxisY.DoubleToPixel(datapoint.YValue) + (Double)_parent.GetValue(TopProperty) + (depth + initialDepth));
                }
                else
                {
                    datapoint.SetValue(LeftProperty, _parent.AxisX.DoubleToPixel(datapoint.XValue));
                    datapoint.SetValue(TopProperty, _parent.AxisY.DoubleToPixel(datapoint.YValue));
                }
                datapoint.Width = 1;
                datapoint.Height = 1;
            }

            for (i = 0; i < _areas.Length; i++)
            {
                if (Double.IsNaN(DataPoints[i].YValue)) continue;
                if (Double.IsNaN(DataPoints[i + 1].YValue))
                {
                    i++;
                    continue;
                }

                if (!_parent.View3D)
                {


                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[i + 1].XValue);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);

                    points[0].Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue);
                    points[1].Y = _parent.AxisY.DoubleToPixel(DataPoints[i + 1].YValue);
                    points[2].Y = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum > 0 ? _parent.AxisY.AxisMinimum : 0);
                    points[3].Y = points[2].Y;

                    _areas[i].Points = Converter.ArrayToCollection(points);

                    Double left = (Double)DataPoints[i].GetValue(LeftProperty);
                    Double top = (Double)DataPoints[i].GetValue(TopProperty);
                    DataPoints[i].points.Add(new Point(points[0].X - left, points[0].Y - top));
                    DataPoints[i].points.Add(new Point(points[1].X - left, points[1].Y - top));
                    DataPoints[i].points.Add(new Point(points[2].X - left, points[2].Y - top));
                    DataPoints[i].points.Add(new Point(points[3].X - left, points[3].Y - top));


                    _areas[i].SetValue(TopProperty, -top);
                    _areas[i].SetValue(LeftProperty, -left);

                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        String linbrush;
                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        _areas[i].Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else
                    {
                        _areas[i].Fill = Cloner.CloneBrush(DataPoints[i].Background);
                    }



                    _areas[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areas[i].StrokeThickness = DataPoints[i].BorderThickness;


                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_areas[i]);
                    DataPoints[i].ApplyEffects((int)_areas[i].GetValue(ZIndexProperty) + 1);

                }
                else
                {


                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[i + 1].XValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);

                    points[0].Y = _parent.AxisY.DoubleToPixel(DataPoints[i].YValue) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[1].Y = _parent.AxisY.DoubleToPixel(DataPoints[i + 1].YValue) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[2].Y = _parent.AxisY.DoubleToPixel(_parent.AxisY.AxisMinimum > 0 ? _parent.AxisY.AxisMinimum : 0) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[3].Y = points[2].Y;

                    _areas[i].Points = Converter.ArrayToCollection(points);

                    _areas[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areaShadows[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);
                    _areaTops[i].Stroke = Cloner.CloneBrush(DataPoints[i].BorderColor);


                    _areas[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _areaShadows[i].StrokeThickness = DataPoints[i].BorderThickness;
                    _areaTops[i].StrokeThickness = DataPoints[i].BorderThickness;

                    _areas[i].Opacity = Opacity * DataPoints[i].Opacity;
                    _areaShadows[i].Opacity = Opacity * DataPoints[i].Opacity;
                    _areaTops[i].Opacity = Opacity * DataPoints[i].Opacity;


                    _areaShadows[i].Height = points[2].Y - points[1].Y;
                    _areaShadows[i].Width = depth;
                    _areaShadows[i].SetValue(TopProperty, points[1].Y);
                    _areaShadows[i].SetValue(LeftProperty, points[1].X);

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _areaShadows[i].RenderTransform = st1;


                    points[0].X += depth;
                    points[1].X += depth;

                    points[2].Y = points[1].Y;
                    points[3].Y = points[0].Y;
                    points[0].Y -= depth;
                    points[1].Y -= depth;


                    _areaTops[i].Points = Converter.ArrayToCollection(points);

                    //This part of the code generates the 3D gradient
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[i].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[i].Background as LinearGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[i].Background as RadialGradientBrush;
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[i].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient string
                            linbrush = "-90;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.65);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.55);
                            linbrush += ",1";

                            //Get the gradient shade
                            brush2 = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-45;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                            linbrush += ",0;";
                            linbrush += Parser.GetLighterColor(brush.Color, 0.35);
                            linbrush += ",1";

                            brushTop = Parser.ParseLinearGradient(linbrush);

                            linbrush = "-120;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.35);
                            linbrush += ",0;";
                            linbrush += Parser.GetDarkerColor(brush.Color, 0.75);
                            linbrush += ",1";

                            brushShade = Parser.ParseLinearGradient(linbrush);


                        }
                        else
                        {
                            brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[i].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = Cloner.CloneBrush(DataPoints[i].Background);
                        brushTop = Cloner.CloneBrush(DataPoints[i].Background);
                        brushShade = Cloner.CloneBrush(DataPoints[i].Background);
                    }
                    #endregion Color Gradient

                    _areas[i].Fill = Cloner.CloneBrush(brush2);
                    _areaShadows[i].Fill = Cloner.CloneBrush(brushShade);
                    _areaTops[i].Fill = Cloner.CloneBrush(brushTop);

                    int zindex = 10;

                    _areas[i].SetValue(ZIndexProperty, zindex);
                    _areaShadows[i].SetValue(ZIndexProperty, zindex - 5);
                    _areaTops[i].SetValue(ZIndexProperty, zindex - 5);

                    switch (DataPoints[i].BorderStyle)
                    {
                        case "Solid":
                            break;

                        case "Dashed":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            _areaShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 4, 4, 4, 4 });
                            break;

                        case "Dotted":
                            _areas[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            _areaShadows[i].StrokeDashArray = Converter.ArrayToCollection(new Double[] { 1, 2, 1, 2 });
                            break;
                    }
                    DataPoints[i].AttachToolTip(_areas[i]);
                    DataPoints[i].AttachToolTip(_areaShadows[i]);
                    DataPoints[i].AttachToolTip(_areaTops[i]);
                }

                marker = DataPoints[i].PlaceMarker();
                if (marker != null)
                {
                    marker.SetValue(ZIndexProperty, (int)_areas[i].GetValue(ZIndexProperty) + 1);
                    DataPoints[i].AttachToolTip(marker);
                }
                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i], labeldepth);

            }
            i = _areas.Length;
            marker = DataPoints[i].PlaceMarker();
            if (marker != null)
            {
                marker.SetValue(ZIndexProperty, (int)_areas[i - 1].GetValue(ZIndexProperty) + 1);
                DataPoints[i].AttachToolTip(marker);
            }

            if (DataPoints[i].LabelEnabled.ToLower() == "true")
                DataPoints[i].AttachLabel(DataPoints[i], labeldepth);
        }

        #endregion Internal Methods

        #region Internal Properties

        internal Double ShadowSize
        {
            get;
            set;
        }

        // Collection of all DataPoints present.
        internal System.Collections.Generic.List<DataPoint> DataPoints
        {
            get;
            set;
        }

        internal Int32 Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }
        internal Int32 DrawingIndex
        {
            get;
            set;
        }
        internal Int32 TotalSiblings
        {
            get;
            set;
        }

        internal Double MinDifference
        {
            get;
            set;
        }

        internal Plot Plot
        {
            get;
            set;
        }

        internal ColorSet ColorSetReference
        {
            get;
            set;
        }
        #endregion Internal Properties

        #region Data

        private Int32 _index;
        private Brush _background;
        private String _lightingEnabled;
        private String _shadowEnabled;
        private String _color;

        internal Canvas _drawingCanvas;
        // This is temporary and used for test purpose. Should be removed at the end.
        private Rectangle _borderRectangle;

        //Starting angle only for pie and Doughnut types
        private Double _startAngle;


        private Double _markerBorderThickness;
        private Double _markerSize;
        private Double _markerScale;
        private Brush _markerBackground;
        private Brush _markerBorderColor;
        private String _markerImage;
        private String _markerStyle;
        internal String _markerEnabled;
        

        //private TextBlock _toolTip;


        private String _labelEnabled;
        private String _labelText;
        private Double _labelFontSize;
        private Brush _labelFontColor;
        private Brush _labelBackground;
        private FontWeight _labelFontWeight;
        private String _labelFontFamily;
        private String _labelStyle;
        private Brush _labelLineColor;
        private Double _labelLineThickness;
        private String _labelLineStyle;
        private String _labelLineEnabled;

        private String _labelFontStyle;

        internal Chart _parent;

        //Charting objects
        //private Line[] _lines, _lineShadows;
        internal Polyline _line, _lineShadow;

        internal Polygon[] _areas;
        internal Rectangle[] _columns, _columnShadows;
        internal Path[] _pies;

        //Charting objects for 3d facing side
        internal Rectangle[] _columnTops, _areaShadows;
        internal Polygon[] _areaTops;
        internal Path[] _pieSides, _pieRight, _pieLeft;
        internal Path[] _doughnut;
        internal Path auxSide1, auxSide2, auxSide3, auxSide4,auxSide5,auxSide6;
        internal int auxID1, auxID2, auxID3, auxID4, auxID5, auxID6;

        //3D shadows
        internal Rectangle[] _shadows;


        internal String _showInLegend;

        private String _toolTipText;
        private String _href;

        private String _colorSet;

        internal Double _sum = 0;
        
        private String _image;
        private Stretch _imageStretch;
        private String _xValueFormatString;
        private String _yValueFormatString;
        private String _zValueFormatString;
        private Dictionary<Double, Point> _maxVals = new Dictionary<Double, Point>();
        #endregion Data

    }
}