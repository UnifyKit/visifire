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
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Markup;
using System.Globalization;
using Visifire.Commons;

namespace Visifire.Charts
{
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
            this.SetValue(LeftProperty, (Double) _parent.PlotArea.GetValue(LeftProperty));


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
                            Background = (_parent.ColorSetReference.GetColor());
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
                            Background = (_parent.ColorSetReference.GetColor());
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
                    Legend legend = (Legend)FindName(Legend);
                    if (legend == null)
                    {
                        throw new Exception("Legend with name '" + Legend + "' does not exist");
                    }
                    if (_parent.DataSeries.Count == 1 )
                    {
                        if(legend.Enabled && legend.EnabledByUser)
                            _showInLegend = true.ToString();
                        else
                            _showInLegend = false.ToString();
                    }
                    else
                    {
                        _showInLegend = true.ToString();
                    }
                }
            }


            CreateReferences();


            if (Background == null)
            {
                Background = (ColorSetReference.GetColor(Index));
            }


            if (_lightingEnabled == "Undefined" && _parent.View3D)
            {
                _lightingEnabled = true.ToString();
            }

            Int32 i = 0;
            Int32 chType = 0;
            switch (RenderAs.ToUpper())
            {
                case "LINE":
                    
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
                        _drawingCanvas = this;
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
                        _drawingCanvas = this;
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
                    if (_parent.View3D)
                    {
                        _drawingCanvas = CreateAndInitialize3DAreaLineCanvas();
                        Create3DAreaElements(_drawingCanvas);
                    }
                    else
                    {
                        Create2DAreaElements();
                    }
                    break;
                case "STACKEDAREA100":
                case "STACKEDAREA":

                    if (_parent.View3D)
                    {
                        if (RenderAs.ToUpper() == "STACKEDAREA")
                        {
                            if (AxisYType == AxisType.Primary)
                                chType = (Int32)Surface3DCharts.StackedAreaPrimary;
                            else
                                chType = (Int32)Surface3DCharts.StackedAreaSecondary;
                        }
                        else
                        {
                            if (AxisYType == AxisType.Primary)
                                chType = (Int32)Surface3DCharts.StackedArea100Primary;
                            else
                                chType = (Int32)Surface3DCharts.StackedArea100Secondary;
                        }

                        _drawingCanvas = CreateAndInitialize3DDrawingCanvas(chType);

                        Create3DAreaElements(_drawingCanvas);
                    }
                    else
                    {
                        Create2DAreaElements();
                    }

                    break;


                case "BAR":
                case "STACKEDBAR":
                case "STACKEDBAR100":
                    break;

                case "COLUMN":
                case "STACKEDCOLUMN":
                case "STACKEDCOLUMN100":
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

        public String LineStyle
        {
            get;
            set;
        }

        public AxisType AxisYType
        {
            get
            {
                return _axisYType;
            }
            set
            {
                _axisYType = value;
            }
        }

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
                _image = Parser.BuildAbsolutePath(value);
                ImageBrush imgBrush = new ImageBrush();
                
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"   ImageSource=\"" + _image + "\"/>";

                imgBrush = (ImageBrush)XamlReader.Load(XAMLimage);
                imgBrush.ImageFailed +=new EventHandler<ExceptionRoutedEventArgs>(imgBrush_ImageFailed);
                _background = imgBrush;
            }
        }

        public override Stretch ImageStretch
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

        public override Boolean ShadowEnabled
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

        public override Boolean LightingEnabled
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
                    return _parent.Label.FontString;
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
                    return (_parent.Label.FontColor);
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
                    return _labelFontWeight;
                else
                    return _parent.Label.FontWeight;
            }
            set
            {
                _labelFontWeight = value;
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
                    return (_parent.Label.Background);
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
                if (!String.IsNullOrEmpty(base.ToolTipText))
                    return base.ToolTipText;
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
                base.ToolTipText = value;
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

        public Stretch MarkerImageStretch
        {
            get;
            set;
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
                _startAngle = ((Int32)(value) % 360) * Math.PI / 180;
            }
        }

        public Int32 DrawingIndex
        {
            get;
            set;
        }

        // Collection of all DataPoints present.
        public System.Collections.Generic.List<DataPoint> DataPoints
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Private Methods

        private Canvas CreateAndInitialize3DDrawingCanvas(Int32 canvasID)
        {
            if (_parent.Surface3D[canvasID] == null)
            {
                _parent.Surface3D[canvasID] = new Canvas();
                _parent.Surface3D[canvasID].Height = _parent.Height;
                _parent.Surface3D[canvasID].Width = _parent.Width;
                _parent.Children.Add(_parent.Surface3D[canvasID]);
                _parent.Surface3D[canvasID].SetValue(ZIndexProperty, this.GetValue(ZIndexProperty));

            }
            else
            {
                if ((Int32)this.GetValue(ZIndexProperty) > (Int32)_parent.Surface3D[canvasID].GetValue(ZIndexProperty))
                    _parent.Surface3D[canvasID].SetValue(ZIndexProperty, this.GetValue(ZIndexProperty));
            }
            return _parent.Surface3D[canvasID];
        }

        private Canvas CreateAndInitialize3DAreaLineCanvas()
        {
            Canvas canvas = new Canvas();
            if (_parent.View3D)
            {
                _parent.AreaLine3D.Add(canvas);
                canvas.Width = _parent.Width;
                canvas.Height = _parent.Height;
                _parent.Children.Add(canvas);
                canvas.SetValue(ZIndexProperty, (Int32)this.GetValue(ZIndexProperty));
            }
            else
            {
                canvas = this;
            }
            return canvas;
        }

        private void Create3DColumnElements(Int32 canvasID)
        {
            _columns = new Path[DataPoints.Count];
            _columnSides = new Path[DataPoints.Count];
            _shadows = new Rectangle[DataPoints.Count];
            _columnTops = new Path[DataPoints.Count];
            _canvas3D = new Canvas[DataPoints.Count];

            for (Int32 i = 0; i < DataPoints.Count; i++)
            {
                _canvas3D[i] = new Canvas();

                _parent.Surface3D[canvasID].Children.Add(_canvas3D[i]);

                _columns[i] = new Path();
                _columnSides[i] = new Path();
                _shadows[i] = new Rectangle();
                _columnTops[i] = new Path();

                SetTag(_columns[i], DataPoints[i].Name);
                SetTag(_columnSides[i], DataPoints[i].Name);
                SetTag(_shadows[i], DataPoints[i].Name);
                SetTag(_columnTops[i], DataPoints[i].Name);

                _canvas3D[i].Children.Add(_columns[i]);
                _canvas3D[i].Children.Add(_columnSides[i]);
                _canvas3D[i].Children.Add(_shadows[i]);
                _canvas3D[i].Children.Add(_columnTops[i]);
            }
        }

        private void Create3DAreaElements(Canvas areaCanvas)
        {
            
            _areas = new Polygon[DataPoints.Count - 1];
            _areaShadows = new Rectangle[DataPoints.Count - 1];
            _areaTops = new Polygon[DataPoints.Count - 1];
            _canvas3D = new Canvas[DataPoints.Count - 1];

            for (Int32 i = 0; i < DataPoints.Count - 1; i++)
            {
                _areas[i] = new Polygon();
                _areaTops[i] = new Polygon();
                _areaShadows[i] = new Rectangle();
                _canvas3D[i] = new Canvas();

                SetTag(_areas[i],DataPoints[i].Name);
                SetTag(_areaTops[i], DataPoints[i].Name);
                SetTag(_areaShadows[i], DataPoints[i].Name);

                areaCanvas.Children.Add(_canvas3D[i]);

                _canvas3D[i].Children.Add(_areas[i]);
                _canvas3D[i].Children.Add(_areaTops[i]);
                _canvas3D[i].Children.Add(_areaShadows[i]);
            }
            
        }

        private void Create2DAreaElements()
        {
            _areas = new Polygon[DataPoints.Count - 1];

            for (Int32 i = 0; i < DataPoints.Count - 1; i++)
            {
                _areas[i] = new Polygon();
                SetTag(_areas[i],DataPoints[i].Name);

                DataPoints[i].Children.Add(_areas[i]);

            }
        }

        protected override void SetDefaults()
        {
            base.SetDefaults();

            DataPoints = new System.Collections.Generic.List<DataPoint>();

            _borderRectangle = new Rectangle();

            RenderAs = "Column";

            _startAngle = 0;

            _axisYType = AxisType.Primary;

            ShadowSize = 6;

            LineStyle = "Solid";

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
            Legend = "";
            
            ColorSet = "";
            ColorSetReference = null;
            // Setting the zindex of DataSeries higher than Axes makes it visible on top of GridLines and other Axes elements
            this.SetValue(ZIndexProperty, 5);
            Opacity = 1.1;
            
        }

        private void PlotPoint()
        {
            Int32 i;
            Double OffsetX=0, OffsetY=0;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count) * (_parent.IndexList[this.Name] - (_parent.Count == 0 ? 0 : 1));
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
                if (Double.IsNaN(DataPoints[i].CorrectedYValue)) continue;

                DataPoints[i].SetValue(LeftProperty, (Double) ( _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) + OffsetX));
                DataPoints[i].SetValue(TopProperty, (Double) ( AxisY.DoubleToPixel(DataPoints[i].CorrectedYValue) + OffsetY));
                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;

                DataPoints[i].MarkerEnabled = "true";
                DataPoints[i].MarkerBackground = (DataPoints[i].Background);
                DataPoints[i].MarkerBorderThickness = DataPoints[i].BorderThickness;
                DataPoints[i].MarkerBorderColor = (DataPoints[i].BorderColor);


                DataPoints[i].PlaceMarker((Int32) GetValue(ZIndexProperty) + 20);

                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i],depth,DataPoints[i].ZIndex);
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

            Int32 i;

            // find true min and max Z
            for (i = 0; i < DataPoints.Count; i++)
            {
                if (Double.IsNaN(DataPoints[i].CorrectedYValue)) continue;
                if (minZ > DataPoints[i].ZValue) minZ = DataPoints[i].ZValue;
                if (maxZ < DataPoints[i].ZValue) maxZ = DataPoints[i].ZValue;
            }

            // Slope to calculate bubble size for datapoint values
            Double slope = (maxSize - minSize) / (maxZ - minZ);
            Double intercept = minSize - minZ * slope;
            Double offsetX=0, offsetY=0;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count) * (_parent.IndexList[this.Name] - (_parent.Count == 0 ? 0 : 1));
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
                if (Double.IsNaN(DataPoints[i].CorrectedYValue)) continue;

                DataPoints[i].SetValue(LeftProperty, (Double) ( _parent.AxisX.DoubleToPixel(DataPoints[i].XValue) - offsetX));
                DataPoints[i].SetValue(TopProperty, (Double) ( AxisY.DoubleToPixel(DataPoints[i].CorrectedYValue) + offsetY));
                DataPoints[i].Width = 1;
                DataPoints[i].Height = 1;

                DataPoints[i].MarkerEnabled = "true";
                if (DataPoints[i].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                {

                    SolidColorBrush tempBrush = (DataPoints[i].Background) as SolidColorBrush;
                    String brush = "-60;";
                    brush += Parser.GetDarkerColor(tempBrush.Color, 0.65) + ",0;";
                    brush += Parser.GetLighterColor(tempBrush.Color, 0.55) + ",1";
                    DataPoints[i].MarkerBackground = Parser.ParseLinearGradient(brush);
                }
                else
                {
                    DataPoints[i].MarkerBackground = (DataPoints[i].Background);

                }
                DataPoints[i].MarkerBorderThickness = DataPoints[i].BorderThickness;
                DataPoints[i].MarkerBorderColor = (DataPoints[i].BorderColor);
                if (Double.IsNaN(DataPoints[i].ZValue) || Double.IsInfinity(DataPoints[i].ZValue))
                {
                    DataPoints[i].MarkerEnabled = "false";
                }
                else
                {
                    DataPoints[i].MarkerSize = (slope * DataPoints[i].ZValue + intercept) * DataPoints[i].MarkerScale;
                }

                DataPoints[i].PlaceMarker((Int32)GetValue(ZIndexProperty) + 20);

                if (DataPoints[i].LabelEnabled.ToLower() == "true")
                    DataPoints[i].AttachLabel(DataPoints[i], 0, DataPoints[i].ZIndex);
            }
        }

        private Int32 ComparePointY(Point a, Point b)
        {
            return a.Y.CompareTo(b.Y);
        }

        private Int32 ComparePointX(Point a, Point b)
        {
            return a.X.CompareTo(b.X);
        }

        private Int32 CompareRectY(Rect a, Rect b)
        {
            return a.Y.CompareTo(b.Y);
        }

        private Int32 CompareRectX(Rect a, Rect b)
        {
            return a.X.CompareTo(b.X);
        }

        private void PositionLabels2D(Point plotRadius, Point startPos, Double sum, Double pieRadius)
        {
            Double startAngle = StartAngle, stopAngle;
            Double angle;
            Double radius = 0;
            Int32 i = 0;

            TextBlock _textBlock = new TextBlock();
            Double offset = 1.1;

            Double centerX, centerY;

            Dictionary<Double, Point> labelYR = new Dictionary<Double, Point>();
            Dictionary<Double, Point> labelYL = new Dictionary<Double, Point>();
            Dictionary<Double, Point> labeltempYR = new Dictionary<Double, Point>();

            Dictionary<Double, Rect> labeltempPosR = new Dictionary<Double, Rect>();
            Dictionary<Double, Rect> labelPosL = new Dictionary<Double, Rect>();
            Dictionary<Double, Rect> labelPosR = new Dictionary<Double, Rect>();

            Dictionary<Double, Point> center = new Dictionary<Double, Point>();

            Double GapL = 2, GapR = 2;
            Double maxGap;

            Int32 lIndex = 0, rIndex = 0, tIndex = 0;
            Double tempY, tempX;

            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].CorrectedYValue / sum;
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
                    if (sum == DataPoints[i].CorrectedYValue)
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
                        DataPoints[i].AttachLabel(tempY, tempX, pieRadius, angle, new Point(centerX, centerY), 1);
                    }
                }
                else
                {
                    offset = 30;

                    radius = pieRadius + offset;

                    if (sum == DataPoints[i].CorrectedYValue)
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
                        if (GapL < _textBlock.ActualHeight) GapL = _textBlock.ActualHeight;
                    }
                    else if (angle >= 0 && angle <= (Math.PI / 2))
                    {
                        labelYR.Add(rIndex, new Point(i, tempY));
                        labelPosR.Add(rIndex, new Rect(tempX, tempY, pieRadius, angle));
                        rIndex++;
                        if (GapR < _textBlock.ActualHeight) GapR = _textBlock.ActualHeight;

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

            Double maxY = _parent.PlotArea.Height;
            Double minY = 0;

            // For placing between angles 90 to 270

            maxGap = ((maxY - minY) - (GapL * (labelYL.Count))) / (labelYL.Count);

            PositionLabels(minY, maxY, GapL, maxGap, lIndex, labelYL, "Left");

            for (i = 0; i < lIndex; i++)
            {
                labelYL.TryGetValue(i, out pt);
                labelYL.Remove(i);



                if (DataPoints[(Int32)pt.X].LabelEnabled.ToLower() == "true")
                {
                    Double X;
                    labelPosL.TryGetValue(i, out rt);

                    Double tempangle = GeometricMath.LineSlope(center[(Int32)pt.X], new Point(rt.X, pt.Y));
                    tempangle = Math.Atan(tempangle);
                    radius = pieRadius + offset;
                    X = center[(Int32)pt.X].X - (radius) * Math.Cos(tempangle);
                    DataPoints[(Int32)pt.X].AttachLabel(pt.Y, X, rt.Width, rt.Height, center[(Int32)pt.X], 1);


                }
            }


            maxGap = ((maxY - minY) - (GapR * (labelYR.Count))) / (labelYR.Count);

            PositionLabels(minY, maxY, GapR, maxGap, rIndex, labelYR, "Right");

            for (i = 0; i < rIndex; i++)
            {
                labelYR.TryGetValue(i, out pt);
                labelYR.Remove(i);

                if (DataPoints[(Int32)pt.X].LabelEnabled.ToLower() == "true")
                {
                    Double X;
                    labelPosR.TryGetValue(i, out rt);

                    Double tempangle = GeometricMath.LineSlope(center[(Int32)pt.X], new Point(rt.X, pt.Y));
                    tempangle = Math.Atan(tempangle);
                    radius = pieRadius + offset;
                    X = center[(Int32)pt.X].X + (radius) * Math.Cos(tempangle);
                    DataPoints[(Int32)pt.X].AttachLabel(pt.Y, X, rt.Width, rt.Height, center[(Int32)pt.X], 1);

                }
            }

        }

        private void PositionLabels3D(Point plotRadius, Point startPos, Point pieRadius, Double sum)
        {
            Double startAngle = StartAngle, stopAngle;
            Double angle;
            Double radius = 0;
            Int32 i = 0;



            Double depth = _parent.Height * 0.075;
            TextBlock _textBlock = new TextBlock();

            Double centerX, centerY;

            Double offset = 0;
            Dictionary<Double, Double> labelOR = new Dictionary<Double, Double>();
            Dictionary<Double, Point> labelYR = new Dictionary<Double, Point>();

            Dictionary<Double, Point> labeltempYR = new Dictionary<Double, Point>();
            Dictionary<Double, Rect> labeltempPosR = new Dictionary<Double, Rect>();


            Dictionary<Double, Double> labelOL = new Dictionary<Double, Double>();
            Dictionary<Double, Point> labelYL = new Dictionary<Double, Point>();

            Dictionary<Double, Rect> labelPosL = new Dictionary<Double, Rect>();
            Dictionary<Double, Rect> labelPosR = new Dictionary<Double, Rect>();

            Dictionary<Double, Point> center = new Dictionary<Double, Point>();

            Double GapL = 2, GapR = 2;
            Double maxGap;

            Int32 lIndex = 0, rIndex = 0, tIndex = 0;
            Double tempY, tempX;
            Double pieradius;
            Double yScalingFactor = pieRadius.Y / pieRadius.X;
            for (i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].CorrectedYValue / sum;
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
                        DataPoints[i].AttachLabel(tempY, tempX, pieradius, angle, new Point(centerX, centerY), yScalingFactor);
                    }
                }
                else
                {
                    offset = 30;

                    centerX = startPos.X + pieradius * DataPoints[i].ExplodeOffset * Math.Cos(angle);
                    centerY = startPos.Y + pieradius * DataPoints[i].ExplodeOffset * Math.Sin(angle) * yScalingFactor;

                    center.Add(i, new Point(centerX, centerY));
                    tempY = startPos.Y + (radius + offset) * Math.Sin(angle) * yScalingFactor;

                    tempX = centerX + (radius + offset) * Math.Cos(angle);

                    if (angle > (Math.PI / 2) && angle < (Math.PI * 3 / 2))
                    {
                        labelYL.Add(lIndex, new Point(i, tempY));
                        labelPosL.Add(lIndex, new Rect(tempX, tempY, pieradius, angle));
                        lIndex++;
                        if (GapL < _textBlock.ActualHeight) GapL = _textBlock.ActualHeight;
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

            Double maxY = _parent.PlotArea.Height;
            Double minY = 0;

            // For placing between angles 90 to 270

            maxGap = ((maxY - minY) - (GapL * (labelYL.Count))) / (labelYL.Count);

            PositionLabels(minY, maxY, GapL, maxGap, lIndex, labelYL, "Left");

            for (i = 0; i < lIndex; i++)
            {
                labelYL.TryGetValue(i, out pt);
                labelYL.Remove(i);

                if (DataPoints[(Int32)pt.X].LabelEnabled.ToLower() == "true")
                {
                    labelPosL.TryGetValue(i, out rt);
                    DataPoints[(Int32)pt.X].AttachLabel(pt.Y, rt.X, rt.Width, rt.Height, center[(Int32)pt.X], yScalingFactor);
                }
            }

            // For placing between angles 270 to 360 and 0 to 90

            maxGap = ((maxY - minY) - (GapR * (labelYR.Count))) / (labelYR.Count);

            PositionLabels(minY, maxY, GapR, maxGap, rIndex, labelYR, "Right");

            for (i = 0; i < rIndex; i++)
            {
                labelYR.TryGetValue(i, out pt);
                labelYR.Remove(i);

                if (DataPoints[(Int32)pt.X].LabelEnabled.ToLower() == "true")
                {
                    labelPosR.TryGetValue(i, out rt);

                    DataPoints[(Int32)pt.X].AttachLabel(pt.Y, rt.X, rt.Width, rt.Height, center[(Int32)pt.X], yScalingFactor);
                }
            }


        }

        private void PositionLabels(Double minY, Double maxY, Double gap, Double maxGap, Double labelCount, Dictionary<Double, Point> labelPositions, String side)
        {
            Boolean isRight = (side == "Right");
            Double limit = (isRight) ? minY : maxY;
            Double sign = (isRight) ? -1 : 1;
            Int32 iterationCount = 0;
            Boolean isOverlap = false;
            Double previousY;
            Double currentY;
            Point point;
            Double offsetFactor = sign * ((gap > maxGap) ? maxGap / 2 : gap / 2);
            do
            {
                previousY = limit;
                isOverlap = false;

                for (Int32 i = 0; i < labelCount; i++)
                {
                    labelPositions.TryGetValue(i, out point);
                    currentY = point.Y;

                    if (Math.Abs(previousY - currentY) < gap && i != 0)
                    {

                        point.Y = previousY - offsetFactor;
                        if (isRight)
                        {
                            if (point.Y > maxY) point.Y = (previousY + maxY - gap) / 2;
                        }
                        else
                        {
                            if (point.Y < minY) point.Y = (minY + previousY) / 2;
                        }
                        currentY = point.Y;

                        labelPositions.Remove(i);
                        labelPositions.Add(i, new Point(point.X, point.Y));

                        labelPositions.TryGetValue(i - 1, out point);
                        point.Y = previousY + offsetFactor;

                        if (isRight)
                        {
                            if (point.Y < minY) point.Y = (minY + previousY) / 2;
                        }
                        else
                        {
                            if (point.Y > maxY) point.Y = (previousY + maxY - gap) / 2;
                        }

                        labelPositions.Remove(i - 1);
                        labelPositions.Add(i - 1, new Point(point.X, point.Y));
                        isOverlap = true;
                        if (isRight)
                        {
                            if (previousY < currentY) isOverlap = true;
                        }
                        else
                        {
                            if (previousY > currentY) isOverlap = true;
                        }
                        break;
                    }

                    previousY = currentY;
                }
                iterationCount++;

            } while (isOverlap && iterationCount < 128);

            if (isOverlap)
            {
                Double stepSize = (maxY - minY) / labelCount;
                for (Int32 i = 0; i < labelCount; i++)
                {
                    labelPositions.TryGetValue(i, out point);
                    if (isRight)
                    {
                        point.Y = stepSize * i;
                    }
                    else
                    {
                        point.Y = maxY - stepSize * (i + 1);
                    }

                    labelPositions.Remove(i);
                    labelPositions.Add(i, new Point(point.X, point.Y));
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
                            if (dp.CorrectedYValue > 0)
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
                            if (dp.CorrectedYValue <= 0)
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
            for (Int32 i = 0; i < Points.Length; i++)
            {
                if (Points[i].X < x1) x1 = Points[i].X;
                if (Points[i].Y < y1) y1 = Points[i].Y;
                if (Points[i].X > x2) x2 = Points[i].X;
                if (Points[i].Y > y2) y2 = Points[i].Y;

            }
            return new Rect(x1, y1, Math.Abs(x2 - x1), Math.Abs(y2 - y1));

        }

        private void PlotPieSingleton(Int32 id,Point startPos ,Double radius)
        {
            List<Object> geometryGroupList;


            geometryGroupList = new List<Object>();
            geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X,startPos.Y),radius,radius));
            _pies[id].Data = Parser.GetGeometryGroupFromList(FillRule.EvenOdd, geometryGroupList);

            DataPoints[id].ApplyStrokeSettings(_pies[id]);

            if (LightingEnabled && DataPoints[id].Background.GetType().Name == "SolidColorBrush")
            {
                String gradient;
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                _pies[id].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity).ToString());

                Ellipse lighting = new Ellipse();
                lighting.Width = radius * 2;
                lighting.Height = radius * 2;
                lighting.SetValue(LeftProperty, (Double) ( startPos.X - radius));
                lighting.SetValue(TopProperty, (Double) ( startPos.Y - radius));
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
                _pies[id].Fill = (DataPoints[id].Background);
            }
            if (Bevel)
            {
                

                String gradient = "-90;";
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                gradient += Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",0;";
                gradient += Parser.GetDarkerColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",1;";

                Path bevel = new Path();

                geometryGroupList = new List<Object>();
                geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius, radius));
                geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius - 4, radius - 4));
                bevel.Data = Parser.GetGeometryGroupFromList(FillRule.EvenOdd, geometryGroupList);

                bevel.Fill = Parser.ParseColor(gradient);
                bevel.IsHitTestVisible = false;
                DataPoints[id].Children.Add(bevel);
                bevel.SetValue(ZIndexProperty, 1);
            }

            DataPoints[id].ApplyEventBasedSettings(_pies[id]);
        }

        private void PlotDoughnutSingleton(Int32 id, Point startPos, Double radius)
        {
            List<Object> geometryGroupList;

            geometryGroupList = new List<Object>();
            geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius, radius));
            geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius / 2, radius / 2));
            _doughnut[id].Data = Parser.GetGeometryGroupFromList(FillRule.EvenOdd, geometryGroupList);

            DataPoints[id].ApplyStrokeSettings(_doughnut[id]);

            if (LightingEnabled && DataPoints[id].Background.GetType().Name == "SolidColorBrush")
            {
                String gradient;
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                _doughnut[id].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity).ToString());

                Ellipse lighting = new Ellipse();
                lighting.Width = radius * 2;
                lighting.Height = radius * 2;
                lighting.SetValue(LeftProperty, (Double) ( startPos.X - radius));
                lighting.SetValue(TopProperty, (Double) ( startPos.Y - radius));
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
                _doughnut[id].Fill = (DataPoints[id].Background);
            }
            if (Bevel)
            {
                String gradient = "-90;";
                Double intensity;
                intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                gradient += Parser.GetLighterColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",0;";
                gradient += Parser.GetDarkerColor((DataPoints[id].Background as SolidColorBrush).Color, intensity) + ",1;";

                Path bevel = new Path();

                geometryGroupList = new List<Object>();
                geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius, radius));
                geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius - 4, radius - 4));
                geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius / 2 + 4, radius / 2 + 4));
                geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), radius / 2, radius / 2));

                bevel.Data = Parser.GetGeometryGroupFromList(FillRule.EvenOdd, geometryGroupList);
                bevel.Fill = Parser.ParseColor(gradient);
                bevel.IsHitTestVisible = false;
                DataPoints[id].Children.Add(bevel);
                bevel.SetValue(ZIndexProperty, 1);
            }

            DataPoints[id].ApplyEventBasedSettings(_doughnut[id]);
        }

        private void PlotPieSingleton3D(Int32 id, Point startPos, Point pieRadius)
        {
            List<Object> geometryGroupList;
            List<PathGeometryParams> pathGeometryList;

            geometryGroupList = new List<Object>();
            geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), pieRadius.X, pieRadius.Y));
            _pies[id].Data = Parser.GetGeometryGroupFromList(FillRule.EvenOdd, geometryGroupList);

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

            pathGeometryList = new List<PathGeometryParams>();
            pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieRadius.X, pieRadius.Y), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
            pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieRadius.X, pieRadius.Y), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
            _pieSides[id].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

            DataPoints[id].ApplyStrokeSettings(_pies[id]);
            DataPoints[id].ApplyStrokeSettings(_pieSides[id]);

            Brush tempBrush = DataPoints[id].Background;
            Brush brushPie, brushSide;
            if (tempBrush.GetType().Name == "LinearGradientBrush")
            {
                LinearGradientBrush brush = DataPoints[id].Background as LinearGradientBrush;
                brushPie = (DataPoints[id].Background);

                brushSide = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""   EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");
                Parser.GenerateDarkerGradientBrush(brush, brushSide as LinearGradientBrush, 0.75);
                RotateTransform rt = new RotateTransform();
                rt.Angle = -45;
                brushSide.RelativeTransform = rt;

            }
            else if (tempBrush.GetType().Name == "RadialGradientBrush")
            {
                RadialGradientBrush brush = DataPoints[id].Background as RadialGradientBrush;
                brushPie = (DataPoints[id].Background);

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
                    brushPie = (brush);
                    brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                }
            }
            else
            {
                brushPie = (DataPoints[id].Background);
                brushSide = (DataPoints[id].Background);
            }
            _pies[id].Fill = brushPie;
            _pieSides[id].Fill = brushSide;

            DataPoints[id].ApplyEventBasedSettings(_pies[id]);
            DataPoints[id].ApplyEventBasedSettings(_pieSides[id]);
        }

        private void PlotDoughnutSingleton3D(Int32 id, Point startPos, Point pieRadius)
        {
            List<Object> geometryGroupList;
            List<PathGeometryParams> pathGeometryList;

            geometryGroupList = new List<Object>();
            geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), pieRadius.X, pieRadius.Y));
            geometryGroupList.Add(new EllipseGeometryParams(new Point(startPos.X, startPos.Y), pieRadius.X / 2, pieRadius.Y / 2));
            _doughnut[id].Data = Parser.GetGeometryGroupFromList(FillRule.EvenOdd, geometryGroupList);


            Double secStartAngle = Math.PI * 0.01;
            Double secStopAngle = Math.PI;
            Point arcStart = new Point();
            Point arcEnd = new Point();
            Double yScalingFactor = pieRadius.Y / pieRadius.X;
            Double radius = pieRadius.X;
            Double depth = _parent.Height * 0.075;
            arcStart.X = startPos.X + radius * Math.Cos(secStartAngle);
            arcStart.Y = startPos.Y + radius * Math.Sin(secStartAngle) * yScalingFactor;

            arcEnd.X = startPos.X + radius * Math.Cos(secStopAngle);
            arcEnd.Y = startPos.Y + radius * Math.Sin(secStopAngle) * yScalingFactor;


            pathGeometryList = new List<PathGeometryParams>();
            pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieRadius.X, pieRadius.Y), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
            pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieRadius.X, pieRadius.Y), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
            _pieSides[id].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

            secStartAngle = Math.PI * 1.01;
            secStopAngle = Math.PI * 2;

            arcStart.X = startPos.X + radius/2 * Math.Cos(secStartAngle);
            arcStart.Y = startPos.Y + radius/2 * Math.Sin(secStartAngle) * yScalingFactor;

            arcEnd.X = startPos.X + radius/2 * Math.Cos(secStopAngle);
            arcEnd.Y = startPos.Y + radius/2 * Math.Sin(secStopAngle) * yScalingFactor;

            pathGeometryList = new List<PathGeometryParams>();
            pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieRadius.X / 2, pieRadius.Y / 2), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
            pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
            pathGeometryList.Add(new ArcSegmentParams(new Size(pieRadius.X / 2, pieRadius.Y / 2), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
            _pies[id].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

            DataPoints[id].ApplyStrokeSettings(_doughnut[id]);
            DataPoints[id].ApplyStrokeSettings(_pies[id]);
            DataPoints[id].ApplyStrokeSettings(_pieSides[id]);

            Brush tempBrush = DataPoints[id].Background;
            Brush brushPie, brushSide;
            if (tempBrush.GetType().Name == "LinearGradientBrush")
            {
                LinearGradientBrush brush = DataPoints[id].Background as LinearGradientBrush;
                brushPie = (DataPoints[id].Background);

                brushSide = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");
                Parser.GenerateDarkerGradientBrush(brush, brushSide as LinearGradientBrush, 0.75);
                RotateTransform rt = new RotateTransform();
                rt.Angle = -45;
                brushSide.RelativeTransform = rt;

            }
            else if (tempBrush.GetType().Name == "RadialGradientBrush")
            {
                RadialGradientBrush brush = DataPoints[id].Background as RadialGradientBrush;
                brushPie = (DataPoints[id].Background);

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
                    brushPie = (brush);
                    brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                }
            }
            else
            {
                brushPie = (DataPoints[id].Background);
                brushSide = (DataPoints[id].Background);
            }
            _doughnut[id].Fill = brushPie;
            _pieSides[id].Fill = brushSide;
            _pies[id].Fill = brushSide;

            DataPoints[id].ApplyEventBasedSettings(_doughnut[id]);
            DataPoints[id].ApplyEventBasedSettings(_pies[id]);
            DataPoints[id].ApplyEventBasedSettings(_pieSides[id]);
        }

        private Storyboard ApplySplineDoubleKeyFrameAnimation(DependencyObject target, String targetProperty, Double from, Double to, Double duration, Double beginTime)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimationUsingKeyFrames doubleAnimation = new DoubleAnimationUsingKeyFrames();

            storyboard.Children.Add(doubleAnimation);

            Storyboard.SetTarget(doubleAnimation, target);

            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(targetProperty));

            SplineDoubleKeyFrame splineKeyframe = new SplineDoubleKeyFrame();

            // From Key frame
            splineKeyframe.Value = from;
            splineKeyframe.KeyTime = TimeSpan.FromMilliseconds((Int32)(1000 * (beginTime)));
            KeySpline Spline = new KeySpline();
            Spline.ControlPoint1 = new Point(0, 0);
            Spline.ControlPoint2 = new Point(0.25, 1);
            splineKeyframe.KeySpline = Spline;
            doubleAnimation.KeyFrames.Add(splineKeyframe);

            // To key frame
            splineKeyframe = new SplineDoubleKeyFrame();
            splineKeyframe.Value = to;
            splineKeyframe.KeyTime = TimeSpan.FromMilliseconds((Int32)(1000 * (beginTime + duration)));
            Spline = new KeySpline();
            Spline.ControlPoint1 = new Point(0, 0);
            Spline.ControlPoint2 = new Point(0.75, 1);
            splineKeyframe.KeySpline = Spline;
            doubleAnimation.KeyFrames.Add(splineKeyframe);

            this.Resources.Add(storyboard.GetHashCode().ToString(), storyboard);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                this.Resources.Remove((sender as Storyboard).GetHashCode().ToString());

                PostInteractiveSteps();
            };

            return storyboard;
        }

        private void PreInteractivitySteps()
        {
            _parent.IsHitTestVisible = false;
        }

        private void PostInteractiveSteps()
        {
            _parent.IsHitTestVisible = true;
        }

        private void AnimatePosition(FrameworkElement element,FrameworkElement index)
        {
            Storyboard leftAnimation = ApplySplineDoubleKeyFrameAnimation(element, "(Canvas.Left)", (Double)element.GetValue(LeftProperty), (Double)element.GetValue(LeftProperty) + _positionOffset[index].X
                , ChartConstants.Interactivity.Duration, ChartConstants.Interactivity.BeginTime);
            Storyboard topAnimation = ApplySplineDoubleKeyFrameAnimation(element, "(Canvas.Top)", (Double)element.GetValue(TopProperty), (Double)element.GetValue(TopProperty) + _positionOffset[index].Y
                , ChartConstants.Interactivity.Duration, ChartConstants.Interactivity.BeginTime);
            leftAnimation.Begin();
            topAnimation.Begin();
        }

        private void AnimateSlice(object sender)
        {
            PreInteractivitySteps();

            FrameworkElement element = (sender as FrameworkElement).Parent as FrameworkElement;

            AnimatePosition(element, element);

            if ((element as DataPoint).Label != null)
            {
                FrameworkElement labelElement = (element as DataPoint).Label as FrameworkElement;
                Storyboard labelAnimation = ApplySplineDoubleKeyFrameAnimation(labelElement, "(Canvas.Left)", (Double)labelElement.GetValue(LeftProperty), (Double)labelElement.GetValue(LeftProperty) + _labelPosOffset[element].X
                    , ChartConstants.Interactivity.Duration, ChartConstants.Interactivity.BeginTime);
                labelAnimation.Begin();
            }
            if ((element as DataPoint).LabelLine != null)
            {

                PointCollection pc = new PointCollection();
                Polyline labelLineElement = (element as DataPoint).LabelLine as Polyline;

                pc.Add(new Point(labelLineElement.Points[0].X + _labelPosOffset[element].X, labelLineElement.Points[0].Y));
                pc.Add(new Point(labelLineElement.Points[1].X + _labelPosOffset[element].X, labelLineElement.Points[1].Y));
                pc.Add(new Point(labelLineElement.Points[2].X + _positionOffset[element].X, labelLineElement.Points[2].Y + _positionOffset[element].Y));

                labelLineElement.Points = pc;
            }
            _positionOffset[element] = new Point(-1 * _positionOffset[element].X, -1 * _positionOffset[element].Y);
            _labelPosOffset[element] = new Point(-1 * _labelPosOffset[element].X, -1 * _labelPosOffset[element].Y);
            
        }

        private void AnimateSlice3D(Int32 i)
        {
            PreInteractivitySteps();

            FrameworkElement element = DataPoints[i] as FrameworkElement;

            if (_pies[i] != null)
            {
                AnimatePosition(_pies[i], element);
            }
            if (_pieSides[i] != null)
            {
                AnimatePosition(_pieSides[i], element);
            }
            if (_pieLeft[i] != null)
            {
                AnimatePosition(_pieLeft[i], element);
            }
            if (_pieRight[i] != null)
            {
                AnimatePosition(_pieRight[i], element);
            }
            if(_doughnut != null)
            {
                AnimatePosition(_doughnut[i], element);
            }
            if (i == auxID1 && auxSide1!=null)
            {
                AnimatePosition(auxSide1, element);
            }
            if (i == auxID2 && auxSide2 != null)
            {
                AnimatePosition(auxSide2, element);
            }
            if (i == auxID4 && auxSide4 != null)
            {
                AnimatePosition(auxSide4, element);
            }
            if (i == auxID5 && auxSide5 != null)
            {
                AnimatePosition(auxSide5, element);
            }

            if ((element as DataPoint).Label != null)
            {
                FrameworkElement labelElement = (element as DataPoint).Label as FrameworkElement;
                Storyboard labelLeftAnimation = ApplySplineDoubleKeyFrameAnimation(labelElement, "(Canvas.Left)", (Double)labelElement.GetValue(LeftProperty), (Double)labelElement.GetValue(LeftProperty) + _labelPosOffset[element].X
                    , ChartConstants.Interactivity.Duration, ChartConstants.Interactivity.BeginTime);
                Storyboard labelTopAnimation = ApplySplineDoubleKeyFrameAnimation(labelElement, "(Canvas.Top)", (Double)labelElement.GetValue(TopProperty), (Double)labelElement.GetValue(TopProperty) + _labelPosOffset[element].Y 
                    , ChartConstants.Interactivity.Duration, ChartConstants.Interactivity.BeginTime);

                labelLeftAnimation.Begin();
                labelTopAnimation.Begin();

                if ((element as DataPoint).LabelLine != null)
                {

                    PointCollection pc = new PointCollection();
                    Polyline labelLineElement = (element as DataPoint).LabelLine as Polyline;

                    pc.Add(new Point(labelLineElement.Points[0].X + _labelPosOffset[element].X, labelLineElement.Points[0].Y + _labelPosOffset[element].Y));
                    pc.Add(new Point(labelLineElement.Points[1].X + _labelPosOffset[element].X, labelLineElement.Points[1].Y + _labelPosOffset[element].Y));
                    pc.Add(new Point(labelLineElement.Points[2].X + _positionOffset[element].X, labelLineElement.Points[2].Y + _positionOffset[element].Y));

                    labelLineElement.Points = pc;
                }
            }
            
            _positionOffset[element] = new Point(-1 * _positionOffset[element].X, -1 * _positionOffset[element].Y);
            _labelPosOffset[element] = new Point(-1 * _labelPosOffset[element].X, -1 * _labelPosOffset[element].Y);
        }

        private Point CheckLabelPosition(Point pos, Double angle,Double width,Double height)
        {
            Double newLeft = pos.X;
            Double newTop = pos.Y;

            if (angle > Math.PI / 2 && angle < Math.PI * 3 / 2)
                newLeft = pos.X - width - 10;
            else
                newLeft = pos.X + 10;

            // this condition places the label such that they do not go outside of the plot area in horizontal direction
            if (angle > (Math.PI / 2) && angle < (Math.PI * 3 / 2))
            {
                if (newLeft < 0)
                    newLeft = 0;
            }
            else
            {

                if ((newLeft + width) > (_parent.PlotArea.Width))
                    newLeft = (_parent.PlotArea.Width - width);
            }

            // this condition places the label such that they do not go outside of the plot area in horizontal direction
            if (angle > Math.PI && angle < Math.PI * 2)
            {
                if (newTop < 0)
                    newTop = 0;
            }
            else
            {
                if ((newTop + height) > (_parent.PlotArea.Height))
                    newTop = _parent.PlotArea.Height - height;

            }
            return new Point(newLeft, newTop);
        }

        private PieDoughnutParams CalculatePlottingParams()
        {
            PieDoughnutParams param = new PieDoughnutParams();

            param._isLabelEnabled = false;
            param._isLabelOutside = false;

            param._dataSeriesTotal = 0;
            param._maxExplodeOffset = 0;
            param._maxLabelHeight = _parent.Padding;
            param._maxLabelWidth = _parent.Padding;
            param._plotDepth = _parent.View3D? Math.Min(_parent.Height,_parent.Width) * 0.075 : 0;

            param._plotHeight = _parent.PlotArea.Height - 2 * _parent.PlotArea.BorderThickness - param._plotDepth * 2;
            param._plotWidth = _parent.PlotArea.Width - 2 * _parent.PlotArea.BorderThickness - param._plotDepth;

            param._plotCenter = new Point();

            // Textblock to calculate the width and height of the label text
            TextBlock _textBlock = new TextBlock();

            foreach (DataPoint dp in DataPoints)
            {

                param._dataSeriesTotal += dp.CorrectedYValue;

                _textBlock.FontSize = dp.LabelFontSize;
                _textBlock.FontFamily = new FontFamily(dp.LabelFontFamily);
                _textBlock.FontWeight = Converter.StringToFontWeight(dp.LabelFontWeight);
                _textBlock.FontStyle = Converter.StringToFontStyle(dp.LabelFontStyle);
                _textBlock.Text = dp.TextParser(dp.LabelText);


                if (dp.LabelEnabled.ToLower() == "true")
                {
                    Math.Max(param._maxLabelHeight, _textBlock.ActualHeight);
                    Math.Max(param._maxLabelWidth, _textBlock.ActualWidth);

                    param._isLabelEnabled = true;

                    if (dp.LabelStyle.ToLower() == "outside")
                        param._isLabelOutside = true;
                }

                if (dp.ExplodeOffset < 0 || dp.ExplodeOffset > 1)
                {
                    Debug.WriteLine("Value of ExplodeOffset must be between 0 and 1");
                    throw (new Exception("Value of ExplodeOffset must be betwee 0 and 1"));
                }

                Math.Max(param._maxExplodeOffset, dp.ExplodeOffset);

            }


            if (param._dataSeriesTotal <= 0)
            {
                //System.Diagnostics.Debug.WriteLine("Pie chart requires at least one entry other than zero");
                //throw (new Exception("Pie chart requires at least one entry other than zero"));
                param._dataSeriesTotal = 1;
            }

            // sum for additional uses like in Tool tip
            _sum = param._dataSeriesTotal;

            // Correction to Explode offset value
            if (param._maxExplodeOffset > 0 && param._maxExplodeOffset + 0.05 <= 1)
                param._maxExplodeOffset += 0.05;

            if (param._maxExplodeOffset <= 0.05) param._maxExplodeOffset = 0.2;

            param._plotWidth -= param._maxLabelWidth;
            param._plotHeight -= param._maxLabelHeight;

            param._plotCenter.X = param._plotWidth / 2 + _parent.PlotArea.BorderThickness + param._maxLabelWidth / 2 + param._plotDepth / 2;
            param._plotCenter.Y = param._plotHeight / 2 + _parent.PlotArea.BorderThickness + param._maxLabelHeight / 2 + param._plotDepth;

            return param;

        }

        private void SetInteractivityParams(Point center,int index, Double angle, Double maxExplodeOffset, Double radius, Double scalingFactor)
        {
            Point positionOffset = new Point();
            Point labelPosOffset = new Point();

            Double centerOffsetFactor = DataPoints[index].ExplodeOffset;
            Double centerOffset = centerOffsetFactor * radius;
            
            if (DataPoints[index].ExplodeOffset == 0)
            {
                centerOffsetFactor = maxExplodeOffset;
                centerOffset = centerOffsetFactor * radius;
                positionOffset.X = centerOffset * Math.Cos(angle);
                positionOffset.Y = centerOffset * Math.Sin(angle) * scalingFactor;
            }
            else
            {
                positionOffset.X = -centerOffset * Math.Cos(angle);
                positionOffset.Y = -centerOffset * Math.Sin(angle) * scalingFactor;
            }
            if (DataPoints[index].Label != null && DataPoints[index].ExplodeOffset == 0)
            {
                labelPosOffset.X = (Double)DataPoints[index].Label.GetValue(LeftProperty) + positionOffset.X;
                labelPosOffset.Y = (Double)DataPoints[index].Label.GetValue(TopProperty) + positionOffset.Y;

                Point tempLabelOffset = CheckLabelPosition(labelPosOffset, angle, DataPoints[index].Label.Width, DataPoints[index].Label.Height);

                labelPosOffset.X = tempLabelOffset.X - (Double)DataPoints[index].Label.GetValue(LeftProperty);
                labelPosOffset.Y = tempLabelOffset.Y - (Double)DataPoints[index].Label.GetValue(TopProperty);
            }
            else if (DataPoints[index].Label != null)
            {
                double offset = 30;
                labelPosOffset.X = center.X + (radius + offset) * Math.Cos(angle);
                labelPosOffset.Y = (Double)DataPoints[index].Label.GetValue(TopProperty) + positionOffset.Y;

                Point tempLabelOffset = CheckLabelPosition(labelPosOffset, angle, DataPoints[index].Label.Width, DataPoints[index].Label.Height);

                labelPosOffset.X = tempLabelOffset.X - (Double)DataPoints[index].Label.GetValue(LeftProperty);
                labelPosOffset.Y = tempLabelOffset.Y - (Double)DataPoints[index].Label.GetValue(TopProperty);
            }
            else
            {
                labelPosOffset.X = 0;
                labelPosOffset.Y = 0;
            }
            _positionOffset.Add(DataPoints[index], positionOffset);
            _labelPosOffset.Add(DataPoints[index], labelPosOffset);
        }

        private void PlotPie()
        {
            Point end1 = new Point();
            Point end2 = new Point();

            PieDoughnutParams plotParams = CalculatePlottingParams();
            
            Double radius;
            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;
            Double minPlotLength = Math.Min(plotParams._plotWidth, plotParams._plotHeight);

            radiusScaling = 1 -  plotParams._maxExplodeOffset;
            centerOffsetFactor = 1 - radiusScaling;

            if (plotParams._isLabelEnabled)
            {
                
                radius = Math.Min(plotParams._plotWidth / 2, plotParams._plotHeight / 2);


                if (plotParams._isLabelOutside && plotParams._maxExplodeOffset <= 0)
                {
                    radius *= 0.7 * radiusScaling;
                    PositionLabels2D(new Point(minPlotLength * radiusScaling * 0.8 / 2, minPlotLength * radiusScaling * 0.8 / 2), plotParams._plotCenter, plotParams._dataSeriesTotal, radius);
                }

                else
                {
                    radius *= radiusScaling;
                    //This is done so that the pie doesn't touch the chart border if padding is not given
                    if (_parent.Padding <= 4) radius -= 5;
                    else radius -= _parent.Padding;

                    PositionLabels2D(new Point(minPlotLength * radiusScaling / 2, minPlotLength * radiusScaling / 2), plotParams._plotCenter, plotParams._dataSeriesTotal, radius);
                }

            }
            else
            {

                minPlotLength /= 2;

                radius = minPlotLength * radiusScaling;
                //This is done so that the pie doesn't touch the chart border if padding is not given
                if (_parent.Padding <= 4) radius -= 5;
                else radius -= _parent.Padding;
            }

            Double centerX;
            Double centerY;
            Double startAngle = StartAngle;
            Double stopAngle;
            Double meanAngle;

            for (Int32 i = 0; i < DataPoints.Count; i++, startAngle = stopAngle)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].CorrectedYValue / plotParams._dataSeriesTotal;

                meanAngle = (startAngle + stopAngle) * 0.5;

                if (DataPoints[i].CorrectedYValue == plotParams._dataSeriesTotal)
                {
                    PlotPieSingleton(i, plotParams._plotCenter, radius);
                    continue;
                }

                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                centerOffset = centerOffsetFactor * radius;

                centerX = plotParams._plotCenter.X + centerOffset * Math.Cos(meanAngle);
                centerY = plotParams._plotCenter.Y + centerOffset * Math.Sin(meanAngle);

                end1.X = centerX + radius * Math.Cos(startAngle);
                end1.Y = centerY + radius * Math.Sin(startAngle);

                end2.X = centerX + radius * Math.Cos(stopAngle);
                end2.Y = centerY + radius * Math.Sin(stopAngle);

                SetInteractivityParams(plotParams._plotCenter, i, meanAngle, plotParams._maxExplodeOffset, radius, 1);

                List<Object> shapeGeometry = new List<Object>();
                shapeGeometry.Add(new EllipseGeometryParams(new Point(centerX, centerY), radius, radius));
                _pies[i].Data = Parser.GetGeometryGroupFromList(FillRule.Nonzero,shapeGeometry);

                List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                clipPathGeometry.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(radius, radius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end1.X, end1.Y)));
                clipPathGeometry.Add(new LineSegmentParams(new Point(centerX, centerY)));

                DataPoints[i].Clip = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY), clipPathGeometry);

                _pies[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice(sender);
                };

                Path borderPath = new Path();
                borderPath.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY), clipPathGeometry);
                DataPoints[i].ApplyStrokeSettings(borderPath);
                DataPoints[i].Children.Add(borderPath);

                //DataPoints[i].ApplyStrokeSettings(_pies[i]);

                DataPoints[i].ApplyEventBasedSettings(_pies[i]);

                #region Lighting
                if (LightingEnabled && DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                {
                    String gradient ;
                    Double intensity;
                    

                    intensity = Math.Floor((0.5 * (1 + 255 / 255)) * 100) / 100;
                    _pies[i].Fill = Parser.ParseColor(Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, intensity).ToString());

                    Ellipse lighting = new Ellipse();
                    lighting.Width = radius * 2;
                    lighting.Height = radius * 2;
                    lighting.SetValue(LeftProperty, (Double) ( centerX - radius));
                    lighting.SetValue(TopProperty, (Double) ( centerY - radius));
                    gradient = "0.5;0.5;";
                    gradient += "#00000000,0;";
                    gradient += "#22000000,0.7;";
                    gradient += "#7F000000,1;";

                    lighting.Fill = Parser.ParseColor(gradient);
                    lighting.IsHitTestVisible = false;
                    DataPoints[i].Children.Add(lighting);
                   
                }
                else
                {
                    _pies[i].Fill = (DataPoints[i].Background);
                }
                #endregion Lighting

                #region Bevel
                if (Bevel && Math.Abs(startAngle - stopAngle) > 0.03)
                {
                    Point bevelCenter = new Point();
                    Point bevelEnd1 = new Point();
                    Point bevelEnd2= new Point();
                    Int32 bevelLength = 4;
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
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", startAngle*180/Math.PI +135, (Int32)_pies[i].GetValue(ZIndexProperty) + 3);
                    }
                    else 
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", -startAngle * 180 / Math.PI, (Int32)_pies[i].GetValue(ZIndexProperty) + 3);
                    }
                    
                    bevelPoints.Clear();
                    bevelPoints.Add(new Point(centerX, centerY));
                    bevelPoints.Add(end2);
                    bevelPoints.Add(bevelEnd2);
                    bevelPoints.Add(bevelCenter);
                    if (stopAngle > Math.PI * 0.5 && stopAngle <= Math.PI * 1.5)
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", stopAngle * 180 / Math.PI + 135, (Int32)_pies[i].GetValue(ZIndexProperty) + 3);
                    }
                    else
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", -stopAngle * 180 / Math.PI, (Int32)_pies[i].GetValue(ZIndexProperty) + 3);
                    }

                    bevelPoints.Clear();

                    Path _bevel = new Path();
                    _bevel.IsHitTestVisible = false;
                    List<PathGeometryParams> bevelGeometry = new List<PathGeometryParams>();
                    bevelGeometry.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                    bevelGeometry.Add(new ArcSegmentParams(new Size(radius, radius),0,(stopAngle - startAngle >= Math.PI || startAngle>stopAngle),SweepDirection.Counterclockwise,new Point(end1.X, end1.Y)));
                    bevelGeometry.Add(new LineSegmentParams(new Point(bevelEnd1.X, bevelEnd1.Y)));
                    bevelGeometry.Add(new ArcSegmentParams(new Size((radius - bevelLength), (radius - bevelLength)),0,(stopAngle - startAngle >= Math.PI || startAngle>stopAngle),SweepDirection.Clockwise,new Point(bevelEnd2.X, bevelEnd2.Y)));
                    _bevel.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(bevelEnd2.X, bevelEnd2.Y), bevelGeometry);

                    String linGrad;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                    {
                        if (tempangle - StartAngle * 180 / Math.PI > 0 && tempangle - StartAngle * 180 / Math.PI < 180)
                        {
                            linGrad = (tempangle + 90).ToString(CultureInfo.InvariantCulture) + ";";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color,0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.85) + ",1";
                        }
                        else
                        {
                            linGrad = (tempangle + 90).ToString(CultureInfo.InvariantCulture) + ";";
                            linGrad += Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.99) + ",1";
                        }
                        _bevel.Fill = Parser.ParseLinearGradient(linGrad);

                    }
                    else 
                    {
                        _bevel.Fill = (DataPoints[i].Background);
                    }
                    
                    _bevel.SetValue(ZIndexProperty, (Int32)_pies[i].GetValue(ZIndexProperty) + 3);

                    DataPoints[i].Children.Add(_bevel);

                }
                #endregion Bevel

                
            }

        }

        private void PlotDoughnut()
        {
            Point end1 = new Point();
            Point end2 = new Point();
            Point end3 = new Point();
            Point end4 = new Point();

            PieDoughnutParams plotParams = CalculatePlottingParams();

            Double radius;
            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;
            Double minPlotLength = Math.Min(plotParams._plotWidth, plotParams._plotHeight);

            radiusScaling = 1 - plotParams._maxExplodeOffset;
            centerOffsetFactor = 1 - radiusScaling;

            if (plotParams._isLabelEnabled)
            {

                radius = Math.Min(plotParams._plotWidth / 2, plotParams._plotHeight / 2);


                if (plotParams._isLabelOutside && plotParams._maxExplodeOffset <= 0)
                {
                    radius *= 0.7 * radiusScaling;
                    PositionLabels2D(new Point(minPlotLength * radiusScaling * 0.8 / 2, minPlotLength * radiusScaling * 0.8 / 2), plotParams._plotCenter, plotParams._dataSeriesTotal, radius);
                }

                else
                {
                    radius *= radiusScaling;
                    //This is done so that the pie doesn't touch the chart border if padding is not given
                    if (_parent.Padding <= 4) radius -= 5;
                    else radius -= _parent.Padding;

                    PositionLabels2D(new Point(minPlotLength * radiusScaling / 2, minPlotLength * radiusScaling / 2), plotParams._plotCenter, plotParams._dataSeriesTotal, radius);
                }

            }
            else
            {

                minPlotLength /= 2;

                radius = minPlotLength * radiusScaling;
                //This is done so that the pie doesn't touch the chart border if padding is not given
                if (_parent.Padding <= 4) radius -= 5;
                else radius -= _parent.Padding;
            }

            Double centerX;
            Double centerY;
            Double startAngle = StartAngle;
            Double stopAngle;
            Double meanAngle;

            for (Int32 i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].CorrectedYValue / plotParams._dataSeriesTotal;

                meanAngle = (startAngle + stopAngle) * 0.5;

                if (DataPoints[i].CorrectedYValue == plotParams._dataSeriesTotal)
                {
                    PlotDoughnutSingleton(i, plotParams._plotCenter, radius);
                    continue;
                }
                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                centerOffset = centerOffsetFactor * radius;

                centerX = plotParams._plotCenter.X + centerOffset * Math.Cos(meanAngle);
                centerY = plotParams._plotCenter.Y + centerOffset * Math.Sin(meanAngle);

                SetInteractivityParams(plotParams._plotCenter, i, meanAngle, plotParams._maxExplodeOffset, radius, 1);

                end1.X = centerX + radius * Math.Cos(startAngle);
                end1.Y = centerY + radius * Math.Sin(startAngle);

                end2.X = centerX + radius * Math.Cos(stopAngle);
                end2.Y = centerY + radius * Math.Sin(stopAngle);

                end3.X = centerX + radius / 2 * Math.Cos(startAngle);
                end3.Y = centerY + radius / 2 * Math.Sin(startAngle);

                end4.X = centerX + radius / 2 * Math.Cos(stopAngle);
                end4.Y = centerY + radius / 2 * Math.Sin(stopAngle);

                List<Object> shapeGeometry = new List<Object>();
                shapeGeometry.Add(new EllipseGeometryParams(new Point(centerX, centerY), radius, radius));
                _doughnut[i].Data = Parser.GetGeometryGroupFromList(FillRule.Nonzero, shapeGeometry);

                List<PathGeometryParams> clipPathGeometry = new List<PathGeometryParams>();
                clipPathGeometry.Add(new LineSegmentParams(new Point(end1.X, end1.Y)));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(radius, radius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Clockwise, new Point(end2.X, end2.Y)));
                clipPathGeometry.Add(new LineSegmentParams(new Point(end4.X, end4.Y)));
                clipPathGeometry.Add(new ArcSegmentParams(new Size(radius / 2, radius / 2), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end3.X, end3.Y)));

                DataPoints[i].Clip = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end3.X, end3.Y), clipPathGeometry);

                Path borderPath = new Path();
                borderPath.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY), clipPathGeometry);
                DataPoints[i].ApplyStrokeSettings(borderPath);
                DataPoints[i].Children.Add(borderPath);

                DataPoints[i].ApplyEventBasedSettings(_doughnut[i]);

                
                _doughnut[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice(sender);
                  //  e.Handled = false;
                };

                


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
                    lighting.SetValue(LeftProperty, (Double) ( centerX - radius));
                    lighting.SetValue(TopProperty, (Double)( centerY - radius));
                    gradient = "0.5;0.5;";
                    gradient += "#00000000,0;";
                    gradient += "#7F000000,0.5;";
                    gradient += "#00000000,0.75;";
                    gradient += "#7F000000,1;";

                    lighting.Fill = Parser.ParseColor(gradient);
                    lighting.IsHitTestVisible = false;
                    DataPoints[i].Children.Add(lighting);
                    
                }
                else
                {
                    _doughnut[i].Fill = (DataPoints[i].Background);
                }

                if (Bevel)
                {
                    #region Bevel
                    Point bevelCenter = new Point();
                    Point bevelEnd1 = new Point();
                    Point bevelEnd2 = new Point();
                    Point bevelEnd3 = new Point();
                    Point bevelEnd4 = new Point();
                    Int32 bevelLength = 4;
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
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", startAngle * 180 / Math.PI + 135, (Int32)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }
                    else
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", -startAngle * 180 / Math.PI, (Int32)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }

                    bevelPoints.Clear();
                    bevelPoints.Add(end4);
                    bevelPoints.Add(end2);
                    bevelPoints.Add(bevelEnd2);
                    bevelPoints.Add(bevelEnd4);
                    if (stopAngle > Math.PI * 0.5 && stopAngle <= Math.PI * 1.5)
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Bright", stopAngle * 180 / Math.PI + 135, (Int32)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }
                    else
                    {
                        DataPoints[i].CreateAndAddBevelPath(bevelPoints, "Dark", -stopAngle * 180 / Math.PI, (Int32)_doughnut[i].GetValue(ZIndexProperty) + 1);
                    }

                    bevelPoints.Clear();

                    
                    Path _bevel = new Path();
                    _bevel.IsHitTestVisible = false;
                    List<PathGeometryParams> bevelGeometry = new List<PathGeometryParams>();
                    bevelGeometry.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                    bevelGeometry.Add(new ArcSegmentParams(new Size(radius, radius), 0, (stopAngle - startAngle >= Math.PI || startAngle > stopAngle), SweepDirection.Counterclockwise, new Point(end1.X, end1.Y)));
                    bevelGeometry.Add(new LineSegmentParams(new Point(bevelEnd1.X, bevelEnd1.Y)));
                    bevelGeometry.Add(new ArcSegmentParams(new Size((radius - bevelLength), (radius - bevelLength)), 0, (stopAngle - startAngle >= Math.PI || startAngle > stopAngle), SweepDirection.Clockwise, new Point(bevelEnd2.X, bevelEnd2.Y)));
                    _bevel.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(bevelEnd2.X, bevelEnd2.Y), bevelGeometry);

                    String linGrad;
                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                    {
                        if (tempangle - StartAngle * 180 / Math.PI > 0 && tempangle - StartAngle * 180 / Math.PI < 180)
                        {
                            linGrad = (tempangle + 90).ToString(CultureInfo.InvariantCulture) + ";";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.85) + ",1";
                        }
                        else
                        {
                            linGrad = (tempangle + 90).ToString(CultureInfo.InvariantCulture) + ";";
                            linGrad += Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.99) + ",1";
                        }
                        _bevel.Fill = Parser.ParseLinearGradient(linGrad);

                    }
                    else
                    {
                        _bevel.Fill = (DataPoints[i].Background);
                    }

                    _bevel.SetValue(ZIndexProperty, (Int32)_doughnut[i].GetValue(ZIndexProperty) + 1);

                    DataPoints[i].Children.Add(_bevel);

                    _bevel = new Path();
                    _bevel.IsHitTestVisible = false;
                    bevelGeometry = new List<PathGeometryParams>();
                    bevelGeometry.Add(new LineSegmentParams(new Point(end4.X, end4.Y)));
                    bevelGeometry.Add(new ArcSegmentParams(new Size(radius / 2, radius / 2), 0, (stopAngle - startAngle >= Math.PI || startAngle > stopAngle), SweepDirection.Counterclockwise, new Point(end3.X, end3.Y)));
                    bevelGeometry.Add(new LineSegmentParams(new Point(bevelEnd3.X, bevelEnd3.Y)));
                    bevelGeometry.Add(new ArcSegmentParams(new Size((radius / 2 + bevelLength), (radius / 2 + bevelLength)), 0, (stopAngle - startAngle >= Math.PI || startAngle > stopAngle), SweepDirection.Clockwise, new Point(bevelEnd4.X, bevelEnd4.Y)));
                    _bevel.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(bevelEnd4.X, bevelEnd4.Y), bevelGeometry);

                    if (DataPoints[i].Background.GetType().Name == "SolidColorBrush")
                    {
                        if (tempangle - StartAngle * 180 / Math.PI > 0 && tempangle - StartAngle * 180 / Math.PI < 180)
                        {

                            linGrad = (tempangle + 90).ToString(CultureInfo.InvariantCulture) + ";";
                            linGrad += Parser.GetLighterColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.99) + ",1";
                        }
                        else
                        {
                            linGrad = (tempangle + 90).ToString(CultureInfo.InvariantCulture) + ";";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.745) + ",0;";
                            linGrad += Parser.GetDarkerColor((DataPoints[i].Background as SolidColorBrush).Color, 0.85) + ",1";
                        }
                        _bevel.Fill = Parser.ParseLinearGradient(linGrad);

                    }
                    else
                    {
                        _bevel.Fill = (DataPoints[i].Background);
                    }

                    _bevel.SetValue(ZIndexProperty, (Int32)_doughnut[i].GetValue(ZIndexProperty) + 1);

                    DataPoints[i].Children.Add(_bevel);
                    #endregion Bevel
                }

                startAngle = stopAngle;
            }

        }

        private void PlotPie3D()
        {
            Point end1 = new Point();
            Point end2 = new Point();

            PieDoughnutParams plotParams = CalculatePlottingParams();

            Double horizontalRadius = plotParams._plotWidth/2;
            Double verticalRadius = plotParams._plotHeight/2;
                    
            // To tilt the pie for appearence as 3D
            if (horizontalRadius > verticalRadius)
            {
                Double tempSide = horizontalRadius * 0.4;
                verticalRadius = verticalRadius > tempSide ? tempSide : verticalRadius;
            }
            else
            {
                verticalRadius = horizontalRadius * 0.4;
            }

            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;
            Double depth = plotParams._plotDepth;
            Double radius;

            radiusScaling = 1 - plotParams._maxExplodeOffset;
            centerOffsetFactor = 1 - radiusScaling;

            Double factor = radiusScaling;

            if (plotParams._isLabelEnabled)
            {

                
                if (plotParams._isLabelOutside && plotParams._maxExplodeOffset > 0)
                {
                    PositionLabels3D(new Point(horizontalRadius * factor, verticalRadius * factor), new Point(plotParams._plotCenter.X, plotParams._plotCenter.Y + depth / 2), new Point(horizontalRadius * factor * 0.7, verticalRadius * factor * 0.7), plotParams._dataSeriesTotal);
                    horizontalRadius *= factor * 0.7;
                    verticalRadius *= factor * 0.7;
                }

                else
                {
                    PositionLabels3D(new Point(horizontalRadius, verticalRadius), new Point(plotParams._plotCenter.X, plotParams._plotCenter.Y + depth / 2), new Point(horizontalRadius * 0.8, verticalRadius * 0.8), plotParams._dataSeriesTotal);
                    horizontalRadius *= 0.8;
                    verticalRadius *= 0.8;
                }

            }
            else
            {
                horizontalRadius *= factor;
                verticalRadius *= factor;
            }

            List<ElementPositionData> elementGroup = new List<ElementPositionData>();
            List<PathGeometryParams> pathGeometryList;

            Double centerX, centerY;
            Double yScalingFactor = verticalRadius / horizontalRadius;
            Double startAngle = StartAngle;
            Double stopAngle;
            Double meanAngle;
            radius = horizontalRadius;
            for (Int32 i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].CorrectedYValue / plotParams._dataSeriesTotal;

                meanAngle = (startAngle + stopAngle) * 0.5;

                if (DataPoints[i].CorrectedYValue == plotParams._dataSeriesTotal)
                {
                    PlotPieSingleton3D(i, plotParams._plotCenter, new Point(horizontalRadius, verticalRadius));
                    continue;
                }
                if (stopAngle == startAngle)
                    continue;

                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                centerOffset = centerOffsetFactor * radius;

                centerX = plotParams._plotCenter.X + centerOffset * Math.Cos(meanAngle);
                centerY = plotParams._plotCenter.Y + centerOffset * Math.Sin(meanAngle) * yScalingFactor;

                SetInteractivityParams(plotParams._plotCenter, i, meanAngle, plotParams._maxExplodeOffset, radius, yScalingFactor);

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
                    brushPie = (DataPoints[i].Background);

                    brushSide = new LinearGradientBrush();
                    (brushSide as LinearGradientBrush).StartPoint = new Point(0, 1);
                    (brushSide as LinearGradientBrush).EndPoint = new Point(1, 0);

                    brushLeft = new LinearGradientBrush();
                    (brushLeft as LinearGradientBrush).StartPoint = new Point(-0.5, 1.5);
                    (brushLeft as LinearGradientBrush).EndPoint = new Point(1.5, -0.5);

                    brushRight = new LinearGradientBrush();
                    (brushRight as LinearGradientBrush).StartPoint = new Point(-0.5, 1.5);
                    (brushRight as LinearGradientBrush).EndPoint = new Point(1.5, -0.5);

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
                    brushPie = (DataPoints[i].Background);

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

                        linbrush = (startAngle * 180 / Math.PI).ToString(CultureInfo.InvariantCulture) + ";";
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



                        linbrush = (((startAngle + stopAngle) / 2) * 180 / Math.PI).ToString(CultureInfo.InvariantCulture) + ";";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                        linbrush += ",1";
                        brushPie = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        brushPie = (brush);

                        brushLeft = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));
                        brushRight = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));

                        brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                    }
                }
                else
                {
                    brushPie = (DataPoints[i].Background);
                    brushLeft = (DataPoints[i].Background);
                    brushRight = (DataPoints[i].Background);
                    brushSide = (DataPoints[i].Background);
                }
                #endregion Color Gradient

                //Create Pie Face
                #region PieFace
                pathGeometryList = new List<PathGeometryParams>();

                pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius),0,(stopAngle - startAngle >= Math.PI),SweepDirection.Counterclockwise,new Point(end1.X, end1.Y)));
                pathGeometryList.Add(new LineSegmentParams(new Point(centerX, centerY)));
                _pies[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY), pathGeometryList);

                #endregion PieFace

                // take floating point mod of the start angle
                while (startAngle < 0) { startAngle += Math.PI * 2; }
                while (stopAngle < 0) { stopAngle += Math.PI * 2; }
                while (startAngle > Math.PI * 2) { startAngle -= Math.PI * 2; }
                while (stopAngle > Math.PI * 2) { stopAngle -= Math.PI * 2; }

                #region ZIndex Setting

                _pies[i].SetValue(ZIndexProperty, 500);

                #endregion ZIndex Setting
                //Create Pie Side
                #region PieSide
                Boolean section1 = false;
                Boolean section2 = false;
                Boolean setbottom = false;

                Double sec1StartAngle = Double.NaN;
                Double sec1StopAngle = Double.NaN;
                Double sec2StartAngle = Double.NaN;
                Double sec2StopAngle = Double.NaN;
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


                    elementGroup.Add(new ElementPositionData(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieRight[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPositionData(DataPoints[i].LabelLine, sec1StartAngle, sec1StopAngle));

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

                    elementGroup.Add(new ElementPositionData(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieRight[i], stopAngle, stopAngle));
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide2, sec2StartAngle, sec2StopAngle));
                    if (DataPoints[i].LabelLine != null)
                        DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);

                    this.Children.Add(auxSide1);
                    this.Children.Add(auxSide2);

                }
                else if ((startAngle >= 0 && startAngle < Math.PI) && (stopAngle > Math.PI && stopAngle <= Math.PI * 2))
                {
                    section1 = true;
                    CreateAuxPath(ref auxSide1, i, brushSide);
                    auxID1 = i;

                    setbottom = true;

                    sec1StartAngle = startAngle;
                    sec1StopAngle = Math.PI;

                    elementGroup.Add(new ElementPositionData(_pieLeft[i], startAngle, startAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPositionData(DataPoints[i].LabelLine, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieRight[i], stopAngle, stopAngle));
                    

                    this.Children.Add(auxSide1);
                }
                else if ((startAngle > Math.PI && startAngle <= Math.PI * 2) && (stopAngle > 0 && stopAngle <= Math.PI))
                {
                    section2 = true;
                    CreateAuxPath(ref auxSide2, i, brushSide);
                    auxID2 = i;

                    setbottom = true;

                    sec2StartAngle = Math.PI * 0.001;
                    sec2StopAngle = stopAngle;

                    elementGroup.Add(new ElementPositionData(_pieLeft[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide2, sec2StartAngle, sec2StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieRight[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPositionData(DataPoints[i].LabelLine, stopAngle, stopAngle));

                    this.Children.Add(auxSide2);
                }
                else
                {
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], startAngle, startAngle));
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        elementGroup.Add(new ElementPositionData(_pieSides[i], startAngle, stopAngle));
                        if(DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, 300);
                    }
                    else
                    {
                        if (DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);
                    }
                    elementGroup.Add(new ElementPositionData(_pieRight[i], stopAngle, stopAngle));

                }

                if (section1)
                {
                    

                    arcStart.X = centerX + radius * Math.Cos(sec1StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec1StartAngle) * yScalingFactor;

                    
                    arcEnd.X = centerX + radius * Math.Cos(sec1StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec1StopAngle) * yScalingFactor;

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
                    auxSide1.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

                    DataPoints[i].ApplyEventBasedSettings(auxSide1);
                    auxSide1.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                    {
                        AnimateSlice3D(auxID1);
                    };
                }

                if (section2)
                {
                    

                    arcStart.X = centerX + radius * Math.Cos(sec2StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec2StartAngle) * yScalingFactor;

                   

                    arcEnd.X = centerX + radius * Math.Cos(sec2StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec2StopAngle) * yScalingFactor;

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
                    auxSide2.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

                    DataPoints[i].ApplyEventBasedSettings(auxSide2);
                    auxSide2.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                    {
                        AnimateSlice3D(auxID2);
                    };
                }
                
                if (setbottom)
                {
                    pathGeometryList = new List<PathGeometryParams>();

                    pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end1.X, end1.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(centerX, centerY + depth)));
                    _pieSides[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY + depth),pathGeometryList);
                    
                    _pieSides[i].Fill = new SolidColorBrush(Colors.Transparent);
                    _pieSides[i].SetValue(ZIndexProperty, -100);

                }
                else
                {
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y + depth)));
                        pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Clockwise, new Point(end2.X, end2.Y + depth)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                        pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end1.X, end1.Y)));
                        _pieSides[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end1.X, end1.Y), pathGeometryList);

                        _pieSides[i].Fill = (brushSide);
                    }

                }
                #endregion PieSide

                #region Pie Left
                pathGeometryList = new List<PathGeometryParams>();
                pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(centerX, centerY + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(centerX, centerY)));
                _pieLeft[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY), pathGeometryList);

                #endregion Pie Left

                #region Pie Right
                pathGeometryList = new List<PathGeometryParams>();
                pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(centerX, centerY + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(centerX, centerY)));
                _pieRight[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(centerX, centerY), pathGeometryList);

                #endregion Pie Right

                _pies[i].Fill = (brushPie);
                _pieLeft[i].Fill = (brushLeft);
                _pieRight[i].Fill = (brushRight);

                _pies[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieSides[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieLeft[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieRight[i].Opacity = Opacity * DataPoints[i].Opacity;

                DataPoints[i].ApplyStrokeSettings(_pies[i]);
                DataPoints[i].ApplyStrokeSettings(_pieSides[i]);
                DataPoints[i].ApplyStrokeSettings(_pieLeft[i]);
                DataPoints[i].ApplyStrokeSettings(_pieRight[i]);

                DataPoints[i].ApplyEventBasedSettings(_pies[i]);
                DataPoints[i].ApplyEventBasedSettings(_pieSides[i]);
                DataPoints[i].ApplyEventBasedSettings(_pieLeft[i]);
                DataPoints[i].ApplyEventBasedSettings(_pieRight[i]);


                _pies[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pies, sender as Path));
                };
                _pieSides[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pieSides, sender as Path));
                };
                _pieLeft[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pieLeft, sender as Path));
                };
                _pieRight[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pieRight, sender as Path));
                };


                startAngle = stopAngle;
            }
            #region ZIndex final
            Int32 zindex1, zindex2;
            elementGroup.Sort(ElementPositionData.CompareAngle);
            zindex1 = 150;
            zindex2 = 100;
            for (Int32 i = 0; i < elementGroup.Count; i++)
            {
                SetZIndex(elementGroup[i].Element, ref zindex1, ref zindex2, elementGroup[i].StartAngle);
            }
            #endregion ZIndex final
        }

        private void SetZIndex(FrameworkElement element, ref Int32 zindex1, ref Int32 zindex2, Double angle)
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
            Point end1 = new Point();
            Point end2 = new Point();
            Point end3 = new Point();
            Point end4 = new Point();

            PieDoughnutParams plotParams = CalculatePlottingParams();

            Double horizontalRadius = plotParams._plotWidth/2;
            Double verticalRadius = plotParams._plotHeight/2;

            // To tilt the pie for appearence as 3D
            if (horizontalRadius > verticalRadius)
            {
                Double tempSide = horizontalRadius * 0.4;
                verticalRadius = verticalRadius > tempSide ? tempSide : verticalRadius;
            }
            else
            {
                verticalRadius = horizontalRadius * 0.4;
            }

            Double centerOffsetFactor, centerOffset;
            Double radiusScaling;
            Double depth = plotParams._plotDepth;
            Double radius;

            radiusScaling = 1 - plotParams._maxExplodeOffset;
            centerOffsetFactor = 1 - radiusScaling;

            Double factor = radiusScaling;

            if (plotParams._isLabelEnabled)
            {


                if (plotParams._isLabelOutside && plotParams._maxExplodeOffset > 0)
                {
                    PositionLabels3D(new Point(horizontalRadius * factor, verticalRadius * factor), new Point(plotParams._plotCenter.X, plotParams._plotCenter.Y + depth / 2), new Point(horizontalRadius * factor * 0.7, verticalRadius * factor * 0.7), plotParams._dataSeriesTotal);
                    horizontalRadius *= factor * 0.7;
                    verticalRadius *= factor * 0.7;
                }

                else
                {
                    PositionLabels3D(new Point(horizontalRadius, verticalRadius), new Point(plotParams._plotCenter.X, plotParams._plotCenter.Y + depth / 2), new Point(horizontalRadius * 0.8, verticalRadius * 0.8), plotParams._dataSeriesTotal);
                    horizontalRadius *= 0.8;
                    verticalRadius *= 0.8;
                }

            }
            else
            {
                horizontalRadius *= factor;
                verticalRadius *= factor;
            }

            List<ElementPositionData> elementGroup = new List<ElementPositionData>();
            List<PathGeometryParams> pathGeometryList;

            Double centerX, centerY;
            Double yScalingFactor = verticalRadius / horizontalRadius;
            Double startAngle = StartAngle;
            Double stopAngle;
            Double meanAngle;
            radius = horizontalRadius;
            for (Int32 i = 0; i < DataPoints.Count; i++)
            {
                stopAngle = startAngle + Math.PI * 2 * DataPoints[i].CorrectedYValue / plotParams._dataSeriesTotal;

                meanAngle = (startAngle + stopAngle) * 0.5;

                if (DataPoints[i].CorrectedYValue == plotParams._dataSeriesTotal)
                {
                    PlotDoughnutSingleton3D(i, plotParams._plotCenter, new Point(horizontalRadius, verticalRadius));
                    continue;
                }
                if (stopAngle == startAngle)
                    continue;

                centerOffsetFactor = DataPoints[i].ExplodeOffset;
                
                centerOffset = centerOffsetFactor * radius;

                centerX = plotParams._plotCenter.X + centerOffset * Math.Cos(meanAngle);
                centerY = plotParams._plotCenter.Y + centerOffset * Math.Sin(meanAngle) * yScalingFactor;

                SetInteractivityParams(plotParams._plotCenter, i, meanAngle, plotParams._maxExplodeOffset, radius, yScalingFactor);
                
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
                    brushPie = (DataPoints[i].Background);

                    brushSide = new LinearGradientBrush();
                    (brushSide as LinearGradientBrush).StartPoint = new Point(0, 1);
                    (brushSide as LinearGradientBrush).EndPoint = new Point(1, 0);

                    brushLeft = new LinearGradientBrush();
                    (brushLeft as LinearGradientBrush).StartPoint = new Point(-0.5, 1.5);
                    (brushLeft as LinearGradientBrush).EndPoint = new Point(1.5, -0.5);

                    brushRight = new LinearGradientBrush();
                    (brushRight as LinearGradientBrush).StartPoint = new Point(-0.5, 1.5);
                    (brushRight as LinearGradientBrush).EndPoint = new Point(1.5, -0.5);

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
                    brushPie = (DataPoints[i].Background);

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

                        linbrush = (startAngle * 180 / Math.PI).ToString(CultureInfo.InvariantCulture) + ";";
                        
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


                        linbrush = (((startAngle + stopAngle) / 2) * 180 / Math.PI).ToString(CultureInfo.InvariantCulture) + ";";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.85);
                        linbrush += ",0;";
                        linbrush += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b);
                        linbrush += ",1";

                        brushPie = Parser.ParseLinearGradient(linbrush);

                    }
                    else
                    {
                        brushPie = (brush);

                        brushLeft = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));
                        brushRight = new SolidColorBrush(Parser.GetDarkerColor(brush.Color, 0.6));

                        brushSide = new SolidColorBrush(Parser.GetLighterColor(brush.Color, 0.6));
                    }
                }
                else
                {
                    brushPie = (DataPoints[i].Background);
                    brushLeft = (DataPoints[i].Background);
                    brushRight = (DataPoints[i].Background);
                    brushSide = (DataPoints[i].Background);
                }
                #endregion Color Gradient

                //Create Doughnut Face
                #region DoughnutFace
                pathGeometryList = new List<PathGeometryParams>();
                pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y)));
                pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Clockwise, new Point(end2.X, end2.Y)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end4.X, end4.Y)));
                pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end3.X, end3.Y)));
                _doughnut[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end3.X, end3.Y), pathGeometryList);
                #endregion DoughnutFace



                // take floating point mod of the start angle
                while (startAngle < 0) { startAngle += Math.PI * 2; }
                while (stopAngle < 0) { stopAngle += Math.PI * 2; }
                while (startAngle > Math.PI * 2) { startAngle -= Math.PI * 2; }
                while (stopAngle > Math.PI * 2) { stopAngle -= Math.PI * 2; }

                #region ZIndex Setting

                _doughnut[i].SetValue(ZIndexProperty, 500);


                if (DataPoints[i].LabelLine != null)
                    DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);
                #endregion ZIndex Setting
                //Create Pie Side
                #region PieSide
                Boolean section1 = false;
                Boolean section2 = false;

                Boolean section4 = false;
                Boolean section5 = false;
                Boolean setbottom = false;

                Double sec1StartAngle = Double.NaN;
                Double sec1StopAngle = Double.NaN;
                Double sec2StartAngle = Double.NaN;
                Double sec2StopAngle = Double.NaN;
                Double sec4StartAngle = Double.NaN;
                Double sec4StopAngle = Double.NaN;
                Double sec5StartAngle = Double.NaN;
                Double sec5StopAngle = Double.NaN;

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

                    elementGroup.Add(new ElementPositionData(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide4, sec4StartAngle, sec4StopAngle));
                    elementGroup.Add(new ElementPositionData(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(auxSide5, sec5StartAngle, sec5StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPositionData(DataPoints[i].LabelLine, sec1StartAngle, sec1StopAngle));


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

                    elementGroup.Add(new ElementPositionData(auxSide4, sec4StartAngle, sec4StopAngle));
                    elementGroup.Add(new ElementPositionData(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], stopAngle, stopAngle));
                    elementGroup.Add(new ElementPositionData(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide2, sec2StartAngle, sec2StopAngle));
                    if (DataPoints[i].LabelLine != null)
                        DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);

                    this.Children.Add(auxSide1);
                    this.Children.Add(auxSide2);

                    this.Children.Add(auxSide4);

                }
                else if ((startAngle >= 0 && startAngle < Math.PI) && (stopAngle > Math.PI && stopAngle <= Math.PI * 2))
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

                    elementGroup.Add(new ElementPositionData(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide1, sec1StartAngle, sec1StopAngle));
                    elementGroup.Add(new ElementPositionData(auxSide4, sec4StartAngle, sec4StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPositionData(DataPoints[i].LabelLine, startAngle, stopAngle));

                    this.Children.Add(auxSide1);

                    this.Children.Add(auxSide4);
                }
                else if ((startAngle > Math.PI && startAngle <= Math.PI * 2) && (stopAngle > 0 && stopAngle <= Math.PI))
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

                    elementGroup.Add(new ElementPositionData(_pieRight[i], startAngle, startAngle));
                    elementGroup.Add(new ElementPositionData(auxSide2, sec2StartAngle, sec2StopAngle));
                    elementGroup.Add(new ElementPositionData(auxSide5, sec5StartAngle, sec5StopAngle));
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], stopAngle, stopAngle));
                    if (DataPoints[i].LabelLine != null)
                        elementGroup.Add(new ElementPositionData(DataPoints[i].LabelLine, stopAngle, stopAngle));

                    this.Children.Add(auxSide2);

                    this.Children.Add(auxSide5);
                }
                else
                {
                    elementGroup.Add(new ElementPositionData(_pieRight[i], startAngle, startAngle));
                    if (startAngle >= Math.PI && startAngle <= Math.PI * 2 && stopAngle >= Math.PI && stopAngle <= Math.PI * 2)
                        elementGroup.Add(new ElementPositionData(_pies[i], startAngle, stopAngle));
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        elementGroup.Add(new ElementPositionData(_pieSides[i], startAngle, stopAngle));
                        if (DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, 300);
                    }
                    else
                    {
                        if (DataPoints[i].LabelLine != null)
                            DataPoints[i].LabelLine.SetValue(ZIndexProperty, -300);
                    }
                    elementGroup.Add(new ElementPositionData(_pieLeft[i], stopAngle, stopAngle));
                }

                if (section1)
                {

                    arcStart.X = centerX + radius * Math.Cos(sec1StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec1StartAngle) * yScalingFactor;

                    arcEnd.X = centerX + radius * Math.Cos(sec1StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec1StopAngle) * yScalingFactor;

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
                    auxSide1.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

                    DataPoints[i].ApplyEventBasedSettings(auxSide1);
                    auxSide1.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                    {
                        AnimateSlice3D(auxID1);
                    };
                }
                if (section2)
                {
                    
                    arcStart.X = centerX + radius * Math.Cos(sec2StartAngle);
                    arcStart.Y = centerY + radius * Math.Sin(sec2StartAngle) * yScalingFactor;

                    arcEnd.X = centerX + radius * Math.Cos(sec2StopAngle);
                    arcEnd.Y = centerY + radius * Math.Sin(sec2StopAngle) * yScalingFactor;

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
                    auxSide2.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

                    DataPoints[i].ApplyEventBasedSettings(auxSide2);
                    auxSide2.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                    {
                        AnimateSlice3D(auxID2);
                    };
                }
                
                if (section4)
                {
                    
                    arcStart.X = centerX + radius/2 * Math.Cos(sec4StartAngle);
                    arcStart.Y = centerY + radius/2 * Math.Sin(sec4StartAngle) * yScalingFactor;

                    

                    arcEnd.X = centerX + radius/2 * Math.Cos(sec4StopAngle);
                    arcEnd.Y = centerY + radius/2 * Math.Sin(sec4StopAngle) * yScalingFactor;

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
                    auxSide4.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

                    DataPoints[i].ApplyEventBasedSettings(auxSide4);
                    auxSide4.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                    {
                        AnimateSlice3D(auxID4);
                    };
                }
                if (section5)
                {
                    

                    arcStart.X = centerX + radius/2 * Math.Cos(sec5StartAngle);
                    arcStart.Y = centerY + radius/2 * Math.Sin(sec5StartAngle) * yScalingFactor;

                    
                    arcEnd.X = centerX + radius/2 * Math.Cos(sec5StopAngle);
                    arcEnd.Y = centerY + radius/2 * Math.Sin(sec5StopAngle) * yScalingFactor;

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcStart.X, arcStart.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, false, SweepDirection.Clockwise, new Point(arcEnd.X, arcEnd.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(arcEnd.X, arcEnd.Y)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, false, SweepDirection.Counterclockwise, new Point(arcStart.X, arcStart.Y)));
                    auxSide5.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(arcStart.X, arcStart.Y), pathGeometryList);

                    DataPoints[i].ApplyEventBasedSettings(auxSide5);
                    auxSide5.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                    {
                        AnimateSlice3D(auxID5);
                    };
                }
                
                if (setbottom)
                {
                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (DataPoints[i].CorrectedYValue / plotParams._dataSeriesTotal >= 0.5), SweepDirection.Clockwise, new Point(end2.X, end2.Y + depth)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(end4.X, end4.Y + depth)));
                    pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, (DataPoints[i].CorrectedYValue / plotParams._dataSeriesTotal >= 0.5), SweepDirection.Counterclockwise, new Point(end3.X, end3.Y + depth)));
                    _pieSides[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end3.X, end3.Y + depth), pathGeometryList);

                    _pieSides[i].Fill = new SolidColorBrush(Colors.Transparent);
                    _pieSides[i].SetValue(ZIndexProperty, -100);

                }
                else
                {
                    if (startAngle >= 0 && stopAngle >= 0 && startAngle <= Math.PI && stopAngle <= Math.PI)
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y + depth)));
                        pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Clockwise, new Point(end2.X, end2.Y + depth)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                        pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius, verticalRadius), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end1.X, end1.Y)));
                        _pieSides[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end1.X, end1.Y), pathGeometryList);

                        _pieSides[i].Fill = (brushSide);
                    }
                    #region InnerFace
                    if (startAngle >= Math.PI && startAngle <= Math.PI * 2 && stopAngle >= Math.PI && stopAngle <= Math.PI * 2)
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(end3.X, end3.Y)));
                        pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Clockwise, new Point(end4.X, end4.Y)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(end4.X, end4.Y + depth)));
                        pathGeometryList.Add(new ArcSegmentParams(new Size(horizontalRadius / 2, verticalRadius / 2), 0, (stopAngle - startAngle >= Math.PI), SweepDirection.Counterclockwise, new Point(end3.X, end3.Y + depth)));
                        _pies[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end3.X, end3.Y + depth), pathGeometryList);
                    }
                    #endregion InnerFace

                }
                #endregion PieSide


                pathGeometryList = new List<PathGeometryParams>();
                pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end2.X, end2.Y + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end4.X, end4.Y + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end4.X, end4.Y)));
                _pieLeft[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end4.X, end4.Y), pathGeometryList);

                pathGeometryList = new List<PathGeometryParams>();
                pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end1.X, end1.Y + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end3.X, end3.Y + depth)));
                pathGeometryList.Add(new LineSegmentParams(new Point(end3.X, end3.Y)));
                _pieRight[i].Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(end3.X, end3.Y), pathGeometryList);
                
                _pies[i].Fill = (brushPie);
                _pieSides[i].Fill = (brushSide);
                _pieLeft[i].Fill = (brushLeft);
                _pieRight[i].Fill = (brushRight);
                _doughnut[i].Fill = (brushPie);

                _pies[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieSides[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieLeft[i].Opacity = Opacity * DataPoints[i].Opacity;
                _pieRight[i].Opacity = Opacity * DataPoints[i].Opacity;
                _doughnut[i].Opacity = Opacity * DataPoints[i].Opacity;

                DataPoints[i].ApplyStrokeSettings(_pies[i]);
                DataPoints[i].ApplyStrokeSettings(_pieSides[i]);
                DataPoints[i].ApplyStrokeSettings(_pieLeft[i]);
                DataPoints[i].ApplyStrokeSettings(_pieRight[i]);
                DataPoints[i].ApplyStrokeSettings(_doughnut[i]);
                                
                DataPoints[i].ApplyEventBasedSettings(_pies[i]);
                DataPoints[i].ApplyEventBasedSettings(_pieSides[i]);
                DataPoints[i].ApplyEventBasedSettings(_pieLeft[i]);
                DataPoints[i].ApplyEventBasedSettings(_pieRight[i]);
                DataPoints[i].ApplyEventBasedSettings(_doughnut[i]);

                _pies[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {

                    AnimateSlice3D(Array.IndexOf(_pies, sender as Path));
                };
                _pieSides[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pieSides, sender as Path));
                };
                _pieLeft[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pieLeft, sender as Path));
                };
                _pieRight[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_pieRight, sender as Path));
                };
                _doughnut[i].MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    AnimateSlice3D(Array.IndexOf(_doughnut, sender as Path));
                };


                startAngle = stopAngle;
            }

            #region ZIndex final
            elementGroup.Sort(ElementPositionData.CompareAngle);
            Int32 zindex1 = 150;
            Int32 zindex2 = 100;
            for (Int32 i = 0; i < elementGroup.Count; i++)
            {
                SetZIndex(elementGroup[i].Element, ref zindex1, ref zindex2, elementGroup[i].StartAngle);
            }
            #endregion ZIndex final

        }

        private void PlotStackedArea()
        {
            Int32 index;
            Double height = 0;

            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
            Point[] points = new Point[4]; // Points for the Plolygon
            Double labeldepth = (depth + initialDepth);
            
            for (index = 0; index < 4; index++) points[index] = new Point();

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<Double, Point>();

            for (index = 0; index < _areas.Length; index++)
            {
                //Ignore current datapoint
                if (Double.IsNaN(DataPoints[index].CorrectedYValue)) continue;
                

                if (_parent.View3D)
                {
                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) - (depth + initialDepth);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[index + 1].XValue) - (depth + initialDepth);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) - (depth + initialDepth);
                }
                else
                {
                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) ;
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[index + 1].XValue);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) ;
                }


                points[0].Y = AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue);
                points[1].Y = AxisY.DoubleToPixel(DataPoints[index + 1].CorrectedYValue);
                points[2].Y = AxisY.DoubleToPixel(AxisY.AxisMinimum > 0 ? AxisY.AxisMinimum : 0);
                points[3].Y = points[2].Y;


                if (Plot.TopBottom.ContainsKey(DataPoints[index].XValue))
                {
                    height = (points[3].Y - points[0].Y);

                    points[3].Y = Plot.TopBottom[DataPoints[index].XValue].Y;
                    points[0].Y = points[3].Y - height;

                    Plot.TopBottom[DataPoints[index].XValue] = new Point(0, points[0].Y);
                }
                else
                {
                    Plot.TopBottom[DataPoints[index].XValue] = new Point(0, points[0].Y);
                }

                // Ignore the next data point, also skip drawing the area
                if (Double.IsNaN(DataPoints[index + 1].CorrectedYValue))
                {
                    index++;
                    continue;
                }


                if (Plot.TopBottom.ContainsKey(DataPoints[index + 1].XValue))
                {
                    height = (points[2].Y - points[1].Y);

                    points[2].Y = Plot.TopBottom[DataPoints[index + 1].XValue].Y;
                    points[1].Y = points[2].Y - height;

                    if (DataPoints[index].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[index + 1].XValue] = new Point(0, points[1].Y);
                }
                else
                {
                    if (DataPoints[index].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[index + 1].XValue] = new Point(0, points[1].Y);
                }


                DataPoints[index].Width = 1;
                DataPoints[index].Height = 1;
                if (!_parent.View3D)
                {

                    DataPoints[index].SetValue(LeftProperty, (Double) points[0].X);
                    DataPoints[index + 1].SetValue(LeftProperty, (Double) points[1].X);

                    DataPoints[index].SetValue(TopProperty, (Double) points[0].Y);
                    DataPoints[index + 1].SetValue(TopProperty, (Double) points[1].Y);

                    _areas[index].Points = Converter.ArrayToCollection(points);

                    Double left = (Double)DataPoints[index].GetValue(LeftProperty);
                    Double top = (Double)DataPoints[index].GetValue(TopProperty);
                    DataPoints[index].points.Add(new Point(points[0].X - left, points[0].Y - top));
                    DataPoints[index].points.Add(new Point(points[1].X - left, points[1].Y - top));
                    DataPoints[index].points.Add(new Point(points[2].X - left, points[2].Y - top));
                    DataPoints[index].points.Add(new Point(points[3].X - left, points[3].Y - top));


                    _areas[index].SetValue(TopProperty, (Double) (-top));
                    _areas[index].SetValue(LeftProperty, (Double) ( -left));

                    if (DataPoints[index].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                    {
                        SolidColorBrush brush = DataPoints[index].Background as SolidColorBrush;
                        String linbrush;
                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        _areas[index].Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else
                    {
                        _areas[index].Fill = (DataPoints[index].Background);
                    }

                    DataPoints[index].ApplyStrokeSettings(_areas[index]);

                    DataPoints[index].ApplyEventBasedSettings(_areas[index]);

                    DataPoints[index].ApplyEffects((Int32)_areas[index].GetValue(ZIndexProperty)+1);
                }
                else
                {
                    

                    DataPoints[index].SetValue(LeftProperty, (Double) points[0].X);
                    DataPoints[index + 1].SetValue(LeftProperty, (Double) points[1].X);

                    DataPoints[index].SetValue(TopProperty, (Double) ( points[0].Y + (depth + initialDepth)));
                    DataPoints[index + 1].SetValue(TopProperty, (Double) ( points[1].Y + (depth + initialDepth)));

                    points[0].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[1].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[2].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[3].X += (Double)_parent.PlotArea.GetValue(LeftProperty);

                    points[0].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[1].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[2].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[3].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);

                    _areas[index].Points = Converter.ArrayToCollection(points);

                    _areas[index].Stroke = (DataPoints[index].BorderColor);
                    _areaShadows[index].Stroke = (DataPoints[index].BorderColor);
                    _areaTops[index].Stroke = (DataPoints[index].BorderColor);


                    _areas[index].StrokeThickness = DataPoints[index].BorderThickness;
                    _areaShadows[index].StrokeThickness = DataPoints[index].BorderThickness;
                    _areaTops[index].StrokeThickness = DataPoints[index].BorderThickness;

                    _areas[index].Opacity = Opacity * DataPoints[index].Opacity;
                    _areaShadows[index].Opacity = Opacity * DataPoints[index].Opacity;
                    _areaTops[index].Opacity = Opacity * DataPoints[index].Opacity;


                    _areaShadows[index].Height = points[2].Y - points[1].Y;
                    _areaShadows[index].Width = depth;
                    _areaShadows[index].SetValue(TopProperty, (Double) points[1].Y);
                    _areaShadows[index].SetValue(LeftProperty, (Double) points[1].X);

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _areaShadows[index].RenderTransform = st1;


                    points[0].X += depth;
                    points[1].X += depth;

                    points[2].Y = points[1].Y;
                    points[3].Y = points[0].Y;
                    points[0].Y -= depth;
                    points[1].Y -= depth;


                    _areaTops[index].Points = Converter.ArrayToCollection(points);

                    //This part of the code generates the 3D gradient
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[index].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[index].Background as LinearGradientBrush;
                        brush2 = (DataPoints[index].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""   EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""   StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[index].Background as RadialGradientBrush;
                        brush2 = (DataPoints[index].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[index].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient String
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
                            brush2 = (DataPoints[index].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[index].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[index].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = (DataPoints[index].Background);
                        brushTop = (DataPoints[index].Background);
                        brushShade = (DataPoints[index].Background);
                    }
                    #endregion Color Gradient

                    _areas[index].Fill = (brush2);
                    _areaShadows[index].Fill = (brushShade);
                    _areaTops[index].Fill = (brushTop);

                    _areas[index].SetValue(ZIndexProperty, 10);
                    _areaShadows[index].SetValue(ZIndexProperty, 5);
                    _areaTops[index].SetValue(ZIndexProperty, 5);

                    DataPoints[index].ApplyStrokeSettings(_areas[index]);
                    DataPoints[index].ApplyStrokeSettings(_areaShadows[index]);
                    DataPoints[index].ApplyStrokeSettings(_areaTops[index]);

                    DataPoints[index].ApplyEventBasedSettings(_areas[index]);
                    DataPoints[index].ApplyEventBasedSettings(_areaShadows[index]);
                    DataPoints[index].ApplyEventBasedSettings(_areaTops[index]);
                }
                
                DataPoints[index].PlaceMarker((Int32)_areas[index].GetValue(ZIndexProperty) + 1);

                if (DataPoints[index].LabelEnabled.ToLower() == "true")
                    DataPoints[index].AttachLabel(DataPoints[index], labeldepth, (Int32)_areas[index].GetValue(ZIndexProperty) + 1);

            }
            index = _areas.Length;

            DataPoints[index].Width = 1;
            DataPoints[index].Height = 1;

            Int32 zIndex = 0;
            //If data series contains only one datapoint no area exists hence draw the marker and label only
            if (DataPoints.Count == 1)
            {
                zIndex = (Int32)DataPoints[index].ZIndex + 1;
            }
            else
            {
                zIndex = (Int32)_areas[index - 1].GetValue(ZIndexProperty) + 1;
            }

            DataPoints[index].PlaceMarker(zIndex);

            if (DataPoints[index].LabelEnabled.ToLower() == "true")
                DataPoints[index].AttachLabel(DataPoints[index], labeldepth, zIndex);

            Int32 chartType = (AxisYType == AxisType.Primary) ? (Int32)Surface3DCharts.StackedAreaPrimary : (Int32)Surface3DCharts.StackedAreaSecondary;

            if (!_parent._plankDrawState[chartType])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Horizontal);
                    _parent._plankDrawState[chartType] = true;
                }
            }
            
        }

        private void PlotStackedArea100()
        {
            Int32 index;
            Double height = 0;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
            Point[] points = new Point[4]; // Points for the Plolygon
            Double labelDepth = (depth + initialDepth);


            for (index = 0; index < 4; index++) points[index] = new Point();

            if (Index == 0)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<Double, Point>();

            for (index = 0; index < _areas.Length; index++)
            {

                
                if (_parent.View3D)
                {

                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) - (depth + initialDepth);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[index + 1].XValue) - (depth + initialDepth);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) - (depth + initialDepth);
                }
                else
                {
                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) ;
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[index + 1].XValue);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) ;
                }

                points[0].Y = AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue / Plot.YValueSum[DataPoints[index].XValue] * 100);
                points[1].Y = AxisY.DoubleToPixel(DataPoints[index + 1].CorrectedYValue / Plot.YValueSum[DataPoints[index + 1].XValue] * 100);
                points[2].Y = AxisY.DoubleToPixel((AxisY.AxisMinimum > 0 ? AxisY.AxisMinimum : 0));
                points[3].Y = points[2].Y;


                if (Plot.TopBottom.ContainsKey(DataPoints[index].XValue))
                {
                    height = (points[3].Y - points[0].Y);

                    points[3].Y = Plot.TopBottom[DataPoints[index].XValue].Y;
                    points[0].Y = points[3].Y - height;

                    Plot.TopBottom[DataPoints[index].XValue] = new Point(0, points[0].Y);
                }
                else
                {
                    Plot.TopBottom[DataPoints[index].XValue] = new Point(0, points[0].Y);
                }

                

                if (Plot.TopBottom.ContainsKey(DataPoints[index + 1].XValue))
                {
                    height = (points[2].Y - points[1].Y);

                    points[2].Y = Plot.TopBottom[DataPoints[index + 1].XValue].Y;
                    points[1].Y = points[2].Y - height;

                    if (DataPoints[index].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[index + 1].XValue] = new Point(0, points[1].Y);
                }
                else
                {
                    if (DataPoints[index].Index == DataPoints.Count - 2)
                        Plot.TopBottom[DataPoints[index + 1].XValue] = new Point(0, points[1].Y);
                }

                DataPoints[index].Width = 1;
                DataPoints[index].Height = 1;
                if (!_parent.View3D)
                {
                    _areas[index].Points = Converter.ArrayToCollection(points);


                    DataPoints[index].SetValue(LeftProperty, (Double) points[0].X);
                    DataPoints[index + 1].SetValue(LeftProperty, (Double) points[1].X);

                    DataPoints[index].SetValue(TopProperty, (Double) points[0].Y);
                    DataPoints[index + 1].SetValue(TopProperty, (Double) points[1].Y);


                    Double left = (Double)DataPoints[index].GetValue(LeftProperty);
                    Double top = (Double)DataPoints[index].GetValue(TopProperty);
                    DataPoints[index].points.Add(new Point(points[0].X - left, points[0].Y - top));
                    DataPoints[index].points.Add(new Point(points[1].X - left, points[1].Y - top));
                    DataPoints[index].points.Add(new Point(points[2].X - left, points[2].Y - top));
                    DataPoints[index].points.Add(new Point(points[3].X - left, points[3].Y - top));


                    _areas[index].SetValue(TopProperty, (Double) (-top));
                    _areas[index].SetValue(LeftProperty, (Double) ( -left));

                    DataPoints[index].ApplyStrokeSettings(_areas[index]);

                    if (DataPoints[index].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                    {
                        SolidColorBrush brush = DataPoints[index].Background as SolidColorBrush;
                        String linbrush;
                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        _areas[index].Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else
                    {
                        _areas[index].Fill = (DataPoints[index].Background);
                    }
                    

                    DataPoints[index].ApplyEventBasedSettings(_areas[index]);

                    DataPoints[index].ApplyEffects((Int32)_areas[index].GetValue(ZIndexProperty) + 1);
                }
                else
                {
                    

                    DataPoints[index].SetValue(LeftProperty, (Double) points[0].X);
                    DataPoints[index + 1].SetValue(LeftProperty, (Double) points[1].X);

                    DataPoints[index].SetValue(TopProperty, (Double) ( points[0].Y + (depth + initialDepth)));
                    DataPoints[index + 1].SetValue(TopProperty, (Double) ( points[1].Y + (depth + initialDepth)));

                    points[0].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[1].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[2].X += (Double)_parent.PlotArea.GetValue(LeftProperty);
                    points[3].X += (Double)_parent.PlotArea.GetValue(LeftProperty);

                    points[0].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[1].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[2].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[3].Y += (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);

                    _areas[index].Points = Converter.ArrayToCollection(points);

                    _areas[index].Stroke = (DataPoints[index].BorderColor);
                    _areaShadows[index].Stroke = (DataPoints[index].BorderColor);
                    _areaTops[index].Stroke = (DataPoints[index].BorderColor);


                    _areas[index].StrokeThickness = DataPoints[index].BorderThickness;
                    _areaShadows[index].StrokeThickness = DataPoints[index].BorderThickness;
                    _areaTops[index].StrokeThickness = DataPoints[index].BorderThickness;

                    _areas[index].Opacity = Opacity * DataPoints[index].Opacity;
                    _areaShadows[index].Opacity = Opacity * DataPoints[index].Opacity;
                    _areaTops[index].Opacity = Opacity * DataPoints[index].Opacity;

                    _areaShadows[index].Height = points[2].Y - points[1].Y;
                    _areaShadows[index].Width = depth;
                    _areaShadows[index].SetValue(TopProperty, (Double) points[1].Y);
                    _areaShadows[index].SetValue(LeftProperty, (Double) points[1].X);

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _areaShadows[index].RenderTransform = st1;


                    points[0].X += depth;
                    points[1].X += depth;

                    points[2].Y = points[1].Y;
                    points[3].Y = points[0].Y;
                    points[0].Y -= depth;
                    points[1].Y -= depth;


                    _areaTops[index].Points = Converter.ArrayToCollection(points);

                    //This part of the code generates the 3D gradient
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[index].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[index].Background as LinearGradientBrush;
                        brush2 = (DataPoints[index].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[index].Background as RadialGradientBrush;
                        brush2 = (DataPoints[index].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[index].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient String
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
                            brush2 = (DataPoints[index].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[index].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[index].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = (DataPoints[index].Background);
                        brushTop = (DataPoints[index].Background);
                        brushShade = (DataPoints[index].Background);
                    }
                    #endregion Color Gradient

                    _areas[index].Fill = (brush2);
                    _areaShadows[index].Fill = (brushShade);
                    _areaTops[index].Fill = (brushTop);

                    _areas[index].SetValue(ZIndexProperty, 10);
                    _areaShadows[index].SetValue(ZIndexProperty, 5);
                    _areaTops[index].SetValue(ZIndexProperty, 5);

                    DataPoints[index].ApplyStrokeSettings(_areas[index]);
                    DataPoints[index].ApplyStrokeSettings(_areaShadows[index]);
                    DataPoints[index].ApplyStrokeSettings(_areaTops[index]);

                    DataPoints[index].ApplyEventBasedSettings(_areas[index]);
                    DataPoints[index].ApplyEventBasedSettings(_areaShadows[index]);
                    DataPoints[index].ApplyEventBasedSettings(_areaTops[index]);
                }
                DataPoints[index].PlaceMarker((Int32)_areas[index].GetValue(ZIndexProperty) + 1);

                if (DataPoints[index].LabelEnabled.ToLower() == "true")
                    DataPoints[index].AttachLabel(DataPoints[index], depth, (Int32)_areas[index].GetValue(ZIndexProperty) + 1);

            }
            index = _areas.Length;

            DataPoints[index].Width = 1;
            DataPoints[index].Height = 1;

            Int32 zIndex = 0;
            //If data series contains only one datapoint no area exists hence draw the marker and label only
            if (DataPoints.Count == 1)
            {
                zIndex = (Int32)DataPoints[index].ZIndex + 1;
            }
            else
            {
                zIndex = (Int32)_areas[index - 1].GetValue(ZIndexProperty) + 1;
            }

            DataPoints[index].PlaceMarker(zIndex);

            if (DataPoints[index].LabelEnabled.ToLower() == "true")
                DataPoints[index].AttachLabel(DataPoints[index], depth, zIndex);

            Int32 chartType = (AxisYType == AxisType.Primary) ? (Int32)Surface3DCharts.StackedArea100Primary : (Int32)Surface3DCharts.StackedArea100Secondary;

            if (!_parent._plankDrawState[chartType])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Horizontal);
                    _parent._plankDrawState[chartType] = true;
                }
            }
        }

        private PathGeometry Get2DColumnPathGeometry(Double width, Double height, Double xRadiusLimit, Double yRadiusLimit)
        {
            //Do not change the order of the lines below
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(0, height);
            pathFigure.Segments = new PathSegmentCollection();

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(0, yRadiusLimit);
            pathFigure.Segments.Add(lineSegment);

            ArcSegment arcSegment = new ArcSegment();
            arcSegment.Point = new Point(xRadiusLimit, 0);
            arcSegment.Size = new Size(xRadiusLimit, yRadiusLimit);
            arcSegment.RotationAngle = 0;
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(arcSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(width - xRadiusLimit, 0);
            pathFigure.Segments.Add(lineSegment);

            arcSegment = new ArcSegment();
            arcSegment.Point = new Point(width, yRadiusLimit);
            arcSegment.Size = new Size(xRadiusLimit, yRadiusLimit);
            arcSegment.RotationAngle = 0;
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(arcSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(width, height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(0, height);
            pathFigure.Segments.Add(lineSegment);

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry Get3DSide(Double left, Double top, Double width, Double height, Double depth)
        {
            //Do not change the order of the lines below
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(left, top);
            pathFigure.Segments = new PathSegmentCollection();

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(left, top + height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + width, top + height-depth);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + width, top - depth);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left, top);
            pathFigure.Segments.Add(lineSegment);

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry Get3DTop(Double left, Double top, Double width, Double height, Double depth)
        {
            //Do not change the order of the lines below
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(left, top);
            pathFigure.Segments = new PathSegmentCollection();

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + depth, top - height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + width + depth, top - height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + width, top);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left, top);
            pathFigure.Segments.Add(lineSegment);

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry Get3DFront(Double left, Double top, Double width, Double height, Double depth)
        {
            //Do not change the order of the lines below
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(left, top);
            pathFigure.Segments = new PathSegmentCollection();

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + width, top);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left + width, top + height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left, top + height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(left, top);
            pathFigure.Segments.Add(lineSegment);

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry Get2DBarPathGeometry(Double width, Double height, Double xRadiusLimit, Double yRadiusLimit)
        {
            //Do not change the order of the lines below
            PathGeometry pathGeometry = new PathGeometry();

            pathGeometry.Figures = new PathFigureCollection();

            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(0, 0);
            pathFigure.Segments = new PathSegmentCollection();

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(width - xRadiusLimit, 0);
            pathFigure.Segments.Add(lineSegment);

            ArcSegment arcSegment = new ArcSegment();
            arcSegment.Point = new Point(width, yRadiusLimit);
            arcSegment.Size = new Size(xRadiusLimit, yRadiusLimit);
            arcSegment.RotationAngle = 0;
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(arcSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(width, height - yRadiusLimit);
            pathFigure.Segments.Add(lineSegment);

            arcSegment = new ArcSegment();
            arcSegment.Point = new Point(width - xRadiusLimit, height);
            arcSegment.Size = new Size(xRadiusLimit, yRadiusLimit);
            arcSegment.RotationAngle = 0;
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(arcSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(0, height);
            pathFigure.Segments.Add(lineSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(0, 0);
            pathFigure.Segments.Add(lineSegment);

            pathGeometry.Figures.Add(pathFigure);
            
            return pathGeometry;
        }

        private Brush Get2DColumnBarColor(Brush baseBrush,Double yValue)
        {
            Brush generatedBrush = null;
            String brush = "";

            if (baseBrush.GetType().Name == "SolidColorBrush" && LightingEnabled && !Bevel)
            {

                brush = "-90;";
                brush += Parser.GetDarkerColor((baseBrush as SolidColorBrush).Color, 0.745);
                brush += ",0;";
                brush += Parser.GetDarkerColor((baseBrush as SolidColorBrush).Color, 0.99);
                brush += ",1";
                generatedBrush = Parser.ParseLinearGradient(brush);
            }
            else if (baseBrush.GetType().Name == "SolidColorBrush" && Bevel)
            {

                if (yValue > 0) brush = "-90;";
                else brush = "90;";
                brush += Parser.GetDarkerColor((baseBrush as SolidColorBrush).Color, 0.80);
                brush += ",0;";
                brush += Parser.GetLighterColor((baseBrush as SolidColorBrush).Color, 0.99);
                brush += ",1";

                generatedBrush = Parser.ParseLinearGradient(brush);

            }
            else
            {
                generatedBrush = (baseBrush);
            }
            return generatedBrush;
        }

        private void Get3DColumnColor(Brush baseBrush, ref Brush front, ref Brush side, ref Brush top)
        {
            
            SoildBrushParams solidParams = new SoildBrushParams("D,0.65,0;L,0.55,1", -90, 1, "N", LightingEnabled);
            LinearGradientParams linearParams = new LinearGradientParams();
            RadialGradientParams radialParams = new RadialGradientParams();
            GradientParams frontParams = new GradientParams(linearParams,radialParams,solidParams);
            front = Parser.GenerateBrush(baseBrush, true, frontParams);

            solidParams = new SoildBrushParams("D,0.35,0;D,0.75,1", -120, 0.85, "D", LightingEnabled);
            linearParams = new LinearGradientParams(new Point(0, 1), new Point(1, 0),0.75,Double.NaN,"D");
            radialParams = new RadialGradientParams(0.75,"D");
            GradientParams sideParams = new GradientParams(linearParams, radialParams, solidParams);
            side = Parser.GenerateBrush(baseBrush, false, sideParams);

            solidParams = new SoildBrushParams("D,0.85,0;L,0.35,1", -45, 0.75, "L", LightingEnabled);
            linearParams = new LinearGradientParams(new Point(-0.5, 1.5), new Point(0.5, 0), 0.85, -45, "L");
            radialParams = new RadialGradientParams(0.85, "L");
            GradientParams topParams = new GradientParams(linearParams, radialParams, solidParams);
            top = Parser.GenerateBrush(baseBrush, false, topParams);


        }

        private void Get3DBarColor(Brush baseBrush, ref Brush front, ref Brush side, ref Brush top)
        {

            SoildBrushParams solidParams = new SoildBrushParams("D,0.65,0;L,0.55,1", 0, 1, "N", LightingEnabled);
            LinearGradientParams linearParams = new LinearGradientParams();
            RadialGradientParams radialParams = new RadialGradientParams();
            GradientParams frontParams = new GradientParams(linearParams, radialParams, solidParams);
            front = Parser.GenerateBrush(baseBrush, true, frontParams);

            solidParams = new SoildBrushParams("D,0.95,0;D,0.85,1", 0, 0.85, "D", LightingEnabled);
            linearParams = new LinearGradientParams(new Point(0, 1), new Point(1, 0), 0.75, Double.NaN, "D");
            radialParams = new RadialGradientParams(0.75, "D");
            GradientParams sideParams = new GradientParams(linearParams, radialParams, solidParams);
            side = Parser.GenerateBrush(baseBrush, false, sideParams);

            solidParams = new SoildBrushParams("D,0.5,0;D,0.75,1", 0, 0.75, "L", LightingEnabled);
            linearParams = new LinearGradientParams(new Point(-0.5, 1.5), new Point(0.5, 0), 0.85, -45, "L");
            radialParams = new RadialGradientParams(0.85, "L");
            GradientParams topParams = new GradientParams(linearParams, radialParams, solidParams);
            top = Parser.GenerateBrush(baseBrush, false, topParams);


        }

        private Boolean IsColumnOutOfPlotRange(Double left, Double width)
        {
            if (left + width <= _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) return true;
            if (left >= _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum)) return true;
            return false;
        }

        private Boolean IsBarOutOfPlotRange(Double top, Double height)
        {
            if (top >= _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) return true;
            if (top + height <= _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum)) return true;
            return false;
        }

        private Double GetColumnWidthDivisionFactor()
        {
            Double factor = 1;
            switch (Plot.ChartType)
            {
                case ChartTypes.Column:

                    factor = TotalSiblings;

                    break;

                case ChartTypes.StackedColumn:
                case ChartTypes.StackedColumn100:

                    //Double tempFactor = _parent.CalculateSiblingCountByChartType(ChartTypes.Column);

                    factor = _parent.GetPlotCountByChartType(Plot.ChartType);

                    //if (tempFactor > factor)
                    //    factor = tempFactor;

                    break;

            }
            return factor;
        }

        private Double CalculateColumnWidth()
        {
            Double width = 10;

            Double divisionFactor = GetColumnWidthDivisionFactor();

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
            {
                width = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / divisionFactor);
            }
            else
            {
                width = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / divisionFactor);

            }

            width -= width * 0.1;

            return width;
        }

        private void PlotStackedColumn()
        {
            Double width;
            Double height;
            Double left;
            Double top;
            Double factor;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            if (_parent.View3D)
            {
                _drawingCanvas = CreateAndInitialize3DDrawingCanvas((Int32)Surface3DCharts.StackedColumn);

                Create3DColumnElements((Int32)Surface3DCharts.StackedColumn);
            }

            width = CalculateColumnWidth();
            factor = GetColumnWidthDivisionFactor();

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<Double, Point>();

            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

            for (Int32 i = 0; i < DataPoints.Count; i++)
            {

                if (Double.IsNaN(DataPoints[i].CorrectedYValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                point.Y = AxisY.DoubleToPixel(DataPoints[i].CorrectedYValue);


                if (AxisY.AxisMinimum > 0)
                    height = AxisY.DoubleToPixel(AxisY.AxisMinimum) - point.Y;
                else
                    height = Math.Abs(AxisY.DoubleToPixel(0) - point.Y);


                //left = (point.X - width / 2);
                left = (point.X + Index * width - (factor * width) / 2);

                if (DataPoints[i].CorrectedYValue >= 0)
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
                    top = AxisY.DoubleToPixel(0);

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
                DataPoints[i].SetValue(LeftProperty, (Double) left);
                DataPoints[i].SetValue(TopProperty, (Double) top);

                if (_parent.View3D)
                {

                    if (IsColumnOutOfPlotRange(left, width)) continue;

                    RenderColumn3D(i, left, top, width, height, depth, initialDepth, ChartTypes.StackedColumn);

                }
                else
                {
                    RenderColumn2D(i, left, top, width, height, ChartTypes.StackedColumn);
                }

            }
            if (!_parent._plankDrawState[(Int32)Surface3DCharts.StackedColumn])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Horizontal);
                    _parent._plankDrawState[(Int32)Surface3DCharts.StackedColumn] = true;
                }
            }
        }

        private void PlotStackedColumn100()
        {
            Double width;
            Double height;
            Double left;
            Double top;
            Double factor;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            
            if (_parent.View3D)
            {
                _drawingCanvas = CreateAndInitialize3DDrawingCanvas((Int32)Surface3DCharts.StackedColumn100);

                Create3DColumnElements((Int32)Surface3DCharts.StackedColumn100);
            }

            width = CalculateColumnWidth();
            factor = GetColumnWidthDivisionFactor();

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<Double, Point>();

            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

            for (Int32 i = 0; i < DataPoints.Count; i++)
            {
                Double sum = Plot.YValueSum[DataPoints[i].XValue];

                if (Double.IsNaN(sum) || sum == 0) sum = 1;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);
                point.Y = AxisY.DoubleToPixel(DataPoints[i].CorrectedYValue / sum * 100);

                if (AxisY.AxisMinimum > 0)
                    height = AxisY.DoubleToPixel(AxisY.AxisMinimum) - point.Y;
                else
                    height = Math.Abs(AxisY.DoubleToPixel(0) - point.Y);


                //left = (point.X - width / 2);
                left = (point.X + Index * width - (factor * width) / 2);

                if (DataPoints[i].CorrectedYValue >= 0)
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
                    top = AxisY.DoubleToPixel(0);

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
                DataPoints[i].SetValue(LeftProperty, (Double) left);
                DataPoints[i].SetValue(TopProperty, (Double) top);

                if (_parent.View3D)
                {
                    if (IsColumnOutOfPlotRange(left, width)) continue;

                    RenderColumn3D(i, left, top, width, height, depth, initialDepth, ChartTypes.StackedColumn100);
                }
                else
                {
                    RenderColumn2D(i, left, top, width, height, ChartTypes.StackedColumn100);
                }

            }
            if (!_parent._plankDrawState[(Int32)Surface3DCharts.StackedColumn100])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Horizontal);
                    _parent._plankDrawState[(Int32)Surface3DCharts.StackedColumn100] = true;
                }
            }
        }

        private void PlotColumn()
        {
            Double width;
            Double height;
            Double left;
            Double top;
            Double factor;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            if (_parent.View3D)
            {
                _drawingCanvas = CreateAndInitialize3DDrawingCanvas((Int32)Surface3DCharts.Column);

                Create3DColumnElements((Int32)Surface3DCharts.Column);
            }

            width = CalculateColumnWidth();
            factor = GetColumnWidthDivisionFactor();

            List<Double> checkDrawPositive = new List<Double>();
            List<Double> checkDrawNegetive = new List<Double>();
            Double finalYValue;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

            for (Int32 i = 0; i < DataPoints.Count; i++)
            {
                if (Double.IsNaN(DataPoints[i].CorrectedYValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[i].XValue);

                finalYValue = DataPoints[i].CorrectedYValue;
                if (_parent.View3D)
                {
                    if (!checkDrawPositive.Contains(DataPoints[i].XValue) && DataPoints[i].CorrectedYValue >= 0)
                    {
                        finalYValue = _maxVals[DataPoints[i].XValue].X;
                        if (DataPoints[i].CorrectedYValue != finalYValue) continue;
                        checkDrawPositive.Add(DataPoints[i].XValue);
                    }
                    else if (!checkDrawNegetive.Contains(DataPoints[i].XValue) && DataPoints[i].CorrectedYValue < 0)
                    {
                        finalYValue = _maxVals[DataPoints[i].XValue].Y;
                        if (DataPoints[i].CorrectedYValue != finalYValue) continue;
                        checkDrawNegetive.Add(DataPoints[i].XValue);
                    }
                    else
                    {
                        continue;
                    }
                }
                point.Y = AxisY.DoubleToPixel(finalYValue);

                if (AxisY.AxisMinimum > 0)
                    height = AxisY.DoubleToPixel(AxisY.AxisMinimum) - point.Y;
                else
                    height = Math.Abs(AxisY.DoubleToPixel(0) - point.Y);


                left = (point.X + Index * width - (factor * width) / 2);


                if (DataPoints[i].CorrectedYValue >= 0)
                    top = point.Y;
                else
                    top = AxisY.DoubleToPixel(0);


                DataPoints[i].SetValue(WidthProperty, width);
                DataPoints[i].SetValue(HeightProperty, height);
                DataPoints[i].SetValue(LeftProperty, (Double) left);
                DataPoints[i].SetValue(TopProperty, (Double)  top);
                
                

                if (_parent.View3D)
                {
                    if (IsColumnOutOfPlotRange(left, width)) continue;

                    RenderColumn3D(i, left, top, width, height, depth, initialDepth, ChartTypes.Column);
                }
                else
                {
                    RenderColumn2D(i, left, top, width, height, ChartTypes.Column);
                }

            }
            //Draw Zero plank
            if (!_parent._plankDrawState[(Int32)Surface3DCharts.Column])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth,depth,0, Orientation.Horizontal);
                    _parent._plankDrawState[(Int32)Surface3DCharts.Column] = true;
                }
            }
        }

        private void RenderColumn3D(Int32 index, Double left, Double top, Double width, Double height, Double depth, Double initialDepth, ChartTypes chartType)
        {
            Boolean ignoreMarkerAndLabel = false;

            Double width3d = width;
            if (left < _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum) - depth)
            {
                width3d = left + width - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum);
                left = _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum);

                ignoreMarkerAndLabel = true;
            }
            else if (left + width > _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum))
            {
                width3d = _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum) - left;

                ignoreMarkerAndLabel = true;
            }


            left -= (depth + initialDepth);
            DataPoints[index].SetValue(LeftProperty, (Double) left);

            _canvas3D[index].SetValue(LeftProperty, (Double) ( left + (Double)_parent.PlotArea.GetValue(LeftProperty)));
            _canvas3D[index].SetValue(TopProperty, (Double) ( top + (Double)_parent.PlotArea.GetValue(TopProperty) + depth + initialDepth));

            _columns[index].Data = Get3DFront(0, 0, width3d, height, depth);

            // Create side face with rendering correction
            _columnSides[index].Data = Get3DSide(width3d - 0.5, 0, depth, height, depth);

            _columnTops[index].Data = Get3DTop(0, 0, width3d, depth, depth);

            _shadows[index].SetValue(TopProperty, (Double) ( -depth - ShadowSize));
            _shadows[index].SetValue(LeftProperty, (Double) ( depth + ShadowSize));

            if ((Double)_shadows[index].GetValue(LeftProperty) + width3d > _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum))
                _shadows[index].Width = Math.Abs( width3d - ShadowSize);
            else
                _shadows[index].Width = width3d;
            
            switch (chartType)
            {
                case ChartTypes.Column:
                    _shadows[index].Height = height + ShadowSize;
                    break;

                case ChartTypes.StackedColumn:
                case ChartTypes.StackedColumn100:
                    if (-depth - ShadowSize + height > AxisY.DoubleToPixel(AxisY.AxisMinimum))
                    {
                        _shadows[index].Height = Math.Abs( height - ShadowSize);
                    }
                    else
                    {
                        _shadows[index].Height = height;
                    }
                    break;
            }

            _canvas3D[index].Opacity = this.Opacity * DataPoints[index].Opacity;
            _shadows[index].Opacity = ShadowEnabled ? 0.8 : 0;

            switch (chartType)
            {
                case ChartTypes.Column:
                    _canvas3D[index].SetValue(ZIndexProperty, GetColumnZIndex(left, top, DataPoints[index].CorrectedYValue > 0));
                    break;
                case ChartTypes.StackedColumn:
                    _canvas3D[index].SetValue(ZIndexProperty, GetStackedColumnZIndex(left, top, DataPoints[index].CorrectedYValue > 0));
                    break;
                case ChartTypes.StackedColumn100:
                    _canvas3D[index].SetValue(ZIndexProperty, GetStackedColumn100ZIndex(left, top, DataPoints[index].CorrectedYValue > 0));
                    break;
            }

            _columns[index].SetValue(ZIndexProperty, 1);
            _columnSides[index].SetValue(ZIndexProperty, 1);
            _columnTops[index].SetValue(ZIndexProperty, 1);
            _shadows[index].SetValue(ZIndexProperty, 0);

            Brush frontBrush = null;
            Brush sideBrush = null;
            Brush topBrush = null;
            Get3DColumnColor(DataPoints[index].Background, ref frontBrush, ref sideBrush, ref topBrush);

            _columns[index].Fill = (frontBrush);
            _columnSides[index].Fill = (sideBrush);
            _columnTops[index].Fill = (topBrush);
            _shadows[index].Fill = Parser.ParseSolidColor("#66000000");

            DataPoints[index].ApplyStrokeSettings(_columns[index]);
            DataPoints[index].ApplyStrokeSettings(_columnSides[index]);
            DataPoints[index].ApplyStrokeSettings(_columnTops[index]);


            DataPoints[index].ApplyEventBasedSettings(_canvas3D[index]);

            DataPoints[index].SetValue(TopProperty, (Double) ( (Double)DataPoints[index].GetValue(TopProperty) + depth + initialDepth));

            if (!ignoreMarkerAndLabel)
            {
                DataPoints[index].PlaceMarker((Int32)_columns[index].GetValue(ZIndexProperty) + 1);

                if (DataPoints[index].LabelEnabled.ToLower() == "true")
                    DataPoints[index].AttachLabel(DataPoints[index], depth,(Int32)_canvas3D[index].GetValue(ZIndexProperty));
            }

        }

        private void RenderColumn2D(int index, Double left, Double top, Double width, Double height, ChartTypes chartType)
        {
            // create required shape objects
            Path shadow = new Path();
            Path column = new Path();
            String maxDP = "", maxDS = "";

            // Add shape objects to the visual list
            DataPoints[index].Children.Add(column);
            DataPoints[index].Children.Add(shadow);

            // initialize parameters for calculation
            Double xRadiusLimit = 0;
            Double yRadiusLimit = 0;

            // check if rounded edge has to be applied to this part of the column or not
            if (chartType == ChartTypes.Column)
            {
                xRadiusLimit = DataPoints[index].RadiusX;
                yRadiusLimit = DataPoints[index].RadiusY;
            }
            else
            {

                if (DataPoints[index].CorrectedYValue > 0)
                    GetPositiveMax(DataPoints[index].XValue, ref maxDP, ref maxDS);
                else
                    GetNegativeMax(DataPoints[index].XValue, ref maxDP, ref maxDS);

                if (maxDS == this.Name && maxDP == DataPoints[index].Name)
                {
                    xRadiusLimit = DataPoints[index].RadiusX;
                    yRadiusLimit = DataPoints[index].RadiusY;

                    if (DataPoints[index].CorrectedYValue >= 0)
                        shadow.Height = Math.Abs(height - ShadowSize);
                    else
                        shadow.Height = height + ShadowSize;

                    shadow.SetValue(TopProperty, (Double) ShadowSize);
                }
                else
                {
                    shadow.Height = height;
                    shadow.SetValue(TopProperty, (Double) 0);
                }
            }

            // These limits revent the radius of curvature from crossing the size of the column
            if (xRadiusLimit > width / 2) xRadiusLimit = width / 2;
            if (yRadiusLimit > height) yRadiusLimit = height;

            // creates the shape of the column
            column.Data = Get2DColumnPathGeometry(width, height, xRadiusLimit, yRadiusLimit);

            // set the basic size settings for the shape object
            column.Width = width;
            column.Height = height;
            column.SetValue(TopProperty, (Double) 0);
            column.SetValue(LeftProperty, (Double) 0);

            // create the shape of the column shadow
            shadow.Data = Get2DColumnPathGeometry(width, height, xRadiusLimit, yRadiusLimit);

            // set the basic size settings for the shape object
            shadow.Width = width;
            shadow.Height = Math.Abs( height - ShadowSize);
            shadow.SetValue(TopProperty, (Double) ShadowSize);
            shadow.SetValue(LeftProperty, (Double) ShadowSize);

            // in case the column has to be drawn for the negaive side then use transform to flip the column
            if (DataPoints[index].CorrectedYValue < 0)
            {
                // To flip the column
                ScaleTransform st = new ScaleTransform();
                st.ScaleX = 1;
                st.ScaleY = -1;
                column.RenderTransformOrigin = new Point(0.5, 0.5);
                column.RenderTransform = st;

                // To flip the column shadow
                st = new ScaleTransform();
                st.ScaleX = 1;
                st.ScaleY = -1;
                shadow.RenderTransformOrigin = new Point(0.5, 0.5);
                shadow.RenderTransform = st;
                if (chartType == ChartTypes.Column)
                {
                    shadow.SetValue(TopProperty, (Double) ShadowSize);
                }
                else
                {
                    if (maxDS == this.Name && maxDP == DataPoints[index].Name)
                        shadow.SetValue(TopProperty, (Double) (-ShadowSize));
                    else
                        shadow.SetValue(TopProperty, (Double) 0);
                }
            }

            // Apply color
            column.Fill = Get2DColumnBarColor(DataPoints[index].Background, DataPoints[index].CorrectedYValue);
            shadow.Fill = Parser.ParseSolidColor("#66000000");

            // Set Opacity
            column.Opacity = 1;
            shadow.Opacity = ShadowEnabled ? 0.8 : 0;
            // Set Zindex
            shadow.SetValue(ZIndexProperty, 1);
            column.SetValue(ZIndexProperty, 5);

            // Set the border line thickness, color and style
            DataPoints[index].ApplyStrokeSettings(column);

            // Set events to this object
            DataPoints[index].ApplyEventBasedSettings(column);

            // Create and Position marker for this element
            DataPoints[index].PlaceMarker((Int32)column.GetValue(ZIndexProperty) + 1);

            // Create and Position label
            if (DataPoints[index].LabelEnabled.ToLower() == "true")
                DataPoints[index].AttachLabel(DataPoints[index], 0, DataPoints[index].ZIndex);

            // Add Bevel,... effects
            DataPoints[index].ApplyEffects((Int32)column.GetValue(ZIndexProperty) + 1);

        }

        private Int32 GetColumnZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 Zi = (Int32)((Double) _parent.PlotArea.GetValue(LeftProperty));
            if (isPositive)
            {
                Zi = Zi + (Int32)(left );
            }
            else
            {
                Zi = Zi + Int32.MinValue + (Int32)(left);
                
            }
            return Zi;
        }

        private Int32 GetStackedColumnZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 ioffset = (Int32)((Double)_parent.PlotArea.GetValue(LeftProperty) + left);
            Int32 topOffset = (Int32)(DrawingIndex);
            Int32 zindex = 0;
            if (isPositive)
            {
                zindex = (Int32)(ioffset + topOffset);
            }
            else
            {
                zindex = Int32.MinValue + (Int32)(ioffset - topOffset);
            }
            return zindex;
        }

        private Int32 GetStackedColumn100ZIndex(Double left, Double top, Boolean isPositive)
        {
            return GetStackedColumnZIndex(left, top, isPositive);
        }

        private void ApplyPlankSettings(Rectangle plank)
        {
            plank.Tag = this.Name;
            plank.StrokeThickness = 0;
            plank.StrokeMiterLimit = 1;
            plank.Opacity = 0.9;
        }

        private void DrawZeroPlank(Double offset,Double depth, Int32 zIndex,Orientation orientation)
        {
            Rectangle zeroPlank = new Rectangle();
            Rectangle zeroPlankFront = new Rectangle();
            Rectangle zeroPlankSide = new Rectangle();

            zeroPlank.SetValue(ZIndexProperty, zIndex);
            zeroPlankFront.SetValue(ZIndexProperty, zIndex + 1);
            zeroPlankSide.SetValue(ZIndexProperty, zIndex + 1);

            ApplyPlankSettings(zeroPlank);
            ApplyPlankSettings(zeroPlankFront);
            ApplyPlankSettings(zeroPlankSide);

            _drawingCanvas.Children.Add(zeroPlank);
            _drawingCanvas.Children.Add(zeroPlankFront);
            _drawingCanvas.Children.Add(zeroPlankSide);

            if (orientation == Orientation.Horizontal)
            {
                // Plank
                zeroPlank.Width = (Double)_parent.PlotArea.Width;
                zeroPlank.Height = depth;
                zeroPlank.SetValue(TopProperty, (Double) ( AxisY.DoubleToPixel(0) + (Double)_parent.PlotArea.GetValue(TopProperty) + offset));
                zeroPlank.SetValue(LeftProperty, (Double) ( (Double)_parent.PlotArea.GetValue(LeftProperty) - offset));

                SkewTransform transform = new SkewTransform();
                transform.AngleX = -45;
                zeroPlank.RenderTransform = transform;
                //zeroPlank.Fill = Parser.ParseLinearGradient("#44FFFFFF,0;#BBB6B6B6,1", new Point(0.5, 0), new Point(0.5, 1));
                zeroPlank.Fill = Parser.ParseLinearGradient("#CCFFFFFF,0;#AADDDDDD,1", new Point(0.5, 0), new Point(0.5, 1));

                // Front border
                zeroPlankFront.Width = _parent.PlotArea.Width;
                zeroPlankFront.Height = 2;
                zeroPlankFront.SetValue(TopProperty, (Double) ( AxisY.DoubleToPixel(0) + (Double)_parent.PlotArea.GetValue(TopProperty) + offset + depth));
                zeroPlankFront.SetValue(LeftProperty, (Double) ( (Double)_parent.PlotArea.GetValue(LeftProperty) - offset - depth));
                zeroPlankFront.Fill = Parser.ParseColor("#B6B6B6");

                // Side border
                zeroPlankSide.Width = _parent.AxisX.MajorTicks.TickLength;
                zeroPlankSide.Height = 2;
                zeroPlankSide.SetValue(TopProperty, (Double) ( AxisY.DoubleToPixel(0) + (Double)_parent.PlotArea.GetValue(TopProperty) + _parent.AxisX.MajorTicks.TickLength));
                zeroPlankSide.SetValue(LeftProperty, (Double) ( (Double)_parent.PlotArea.GetValue(LeftProperty) + (Double)_parent.PlotArea.Width - _parent.AxisX.MajorTicks.TickLength));

                transform = new SkewTransform();
                transform.AngleY = -45;
                zeroPlankSide.RenderTransform = transform;

                zeroPlankSide.Fill = Parser.ParseColor("#B6B6B6");
            }
            else
            {
                zeroPlank.Width = depth;
                zeroPlank.Height = (Double)_parent.PlotArea.Height;
                zeroPlank.SetValue(TopProperty, (Double) ( (Double)_parent.PlotArea.GetValue(TopProperty) + offset + depth));
                zeroPlank.SetValue(LeftProperty, (Double) ( (Double)_parent.PlotArea.GetValue(LeftProperty) + AxisY.DoubleToPixel(0) - offset - depth));

                SkewTransform transform = new SkewTransform();
                transform.AngleY = -45;
                zeroPlank.RenderTransform = transform;

                //zeroPlank.Fill = Parser.ParseLinearGradient("#44FFFFFF,0;#BBB6B6B6,1", new Point(1, 0.5), new Point(0, 0.5));
                zeroPlank.Fill = Parser.ParseLinearGradient("#CCFFFFFF,0;#AADDDDDD,1", new Point(1, 0.5), new Point(0, 0.5));

                // Front border
                zeroPlankFront.Width = 2;
                zeroPlankFront.Height = _parent.PlotArea.Height;
                zeroPlankFront.SetValue(TopProperty, (Double) ( (Double)_parent.PlotArea.GetValue(TopProperty) + _parent.AxisX.MajorTicks.TickLength));
                zeroPlankFront.SetValue(LeftProperty, (Double) ( AxisY.DoubleToPixel(0) + (Double)_parent.PlotArea.GetValue(LeftProperty) - _parent.AxisX.MajorTicks.TickLength));

                zeroPlankFront.Fill = Parser.ParseColor("#B6B6B6");

                // Side border
                zeroPlankSide.Width = 2;
                zeroPlankSide.Height = _parent.AxisX.MajorTicks.TickLength;
                zeroPlankSide.SetValue(TopProperty, (Double) _parent.PlotArea.GetValue(TopProperty));
                zeroPlankSide.SetValue(LeftProperty, (Double) ( AxisY.DoubleToPixel(0) + (Double)_parent.PlotArea.GetValue(LeftProperty)));

                transform = new SkewTransform();
                transform.AngleX = -45;
                zeroPlankSide.RenderTransform = transform;

                zeroPlankSide.Fill = Parser.ParseColor("#B6B6B6");
            }
        }

        private void RenderBar3D(int index,Double left,Double top,Double width,Double height,Double depth,Double initialDepth,ChartTypes chartType)
        {
            Boolean ignoreMarkerAndLabel = false;
            Double height3d = height;

            if (top < _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum))
            {
                height3d = top + height - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum);
                top = _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMaximum);

                ignoreMarkerAndLabel = true;
            }
            else if (top + height > _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum))
            {
                height3d = _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum) - top;

                ignoreMarkerAndLabel = true;
            }

            top += (depth + initialDepth);
            DataPoints[index].SetValue(TopProperty, (Double) top);

            _canvas3D[index].SetValue(LeftProperty, (Double) ( left + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth)));
            _canvas3D[index].SetValue(TopProperty, (Double) ( top + (Double)_parent.PlotArea.GetValue(TopProperty)));

            _columns[index].Data = Get3DFront(0, 0, width, height3d, depth);

            // Create side face with rendering correction
            _columnSides[index].Data = Get3DSide(width - 0.1, 0, depth, height3d, depth);

            _columnTops[index].Data = Get3DTop(0, 0, width, depth, depth);

            _shadows[index].SetValue(TopProperty, (Double) (  -depth - height3d * 0.1));
            _shadows[index].SetValue(LeftProperty, (Double) depth);
            
            _shadows[index].Height = height3d;
            switch (chartType)
            {
                case ChartTypes.Bar:
                    _shadows[index].Width = width + ShadowSize;
                    break;

                case ChartTypes.StackedBar:
                case ChartTypes.StackedBar100:
                    if (left < AxisY.DoubleToPixel(AxisY.AxisMinimum))
                    {
                        _shadows[index].Width = width + ShadowSize;
                    }
                    else
                    {
                        _shadows[index].Width = width;
                    }
                    break;

                default:
                    break;
            }
            _canvas3D[index].Opacity = this.Opacity * DataPoints[index].Opacity;
            _shadows[index].Opacity = ShadowEnabled ? 0.8 : 0;

            //This part of the code generates the 3d gradients
            Brush frontBrush = null;
            Brush sideBrush = null;
            Brush topBrush = null;
            Get3DBarColor(DataPoints[index].Background, ref frontBrush, ref sideBrush, ref topBrush);


            _columns[index].Fill = (frontBrush);
            _columnSides[index].Fill = (sideBrush);
            _columnTops[index].Fill = (topBrush);
            _shadows[index].Fill = Parser.ParseSolidColor("#66000000");

            switch (chartType)
            {
                case ChartTypes.Bar:
                    _canvas3D[index].SetValue(ZIndexProperty, GetBarZIndex(left, top, DataPoints[index].CorrectedYValue >= 0));
                    break;

                case ChartTypes.StackedBar:
                    _canvas3D[index].SetValue(ZIndexProperty, GetStackedBarZIndex(left, top, DataPoints[index].CorrectedYValue >= 0));
                    break;

                case ChartTypes.StackedBar100:
                    _canvas3D[index].SetValue(ZIndexProperty, GetStackedBar100ZIndex(left, top, DataPoints[index].CorrectedYValue >= 0));
                    break;

                default:
                    break;
            }

            _columns[index].SetValue(ZIndexProperty, 1);
            _columnSides[index].SetValue(ZIndexProperty, 1);
            _columnTops[index].SetValue(ZIndexProperty, 1);
            _shadows[index].SetValue(ZIndexProperty, 0);


            DataPoints[index].SetValue(LeftProperty, (Double) ( (Double)DataPoints[index].GetValue(LeftProperty) - (depth + initialDepth)));

            DataPoints[index].ApplyStrokeSettings(_columns[index]);
            DataPoints[index].ApplyStrokeSettings(_columnSides[index]);
            DataPoints[index].ApplyStrokeSettings(_columnTops[index]);

            DataPoints[index].ApplyEventBasedSettings(_canvas3D[index]);

            if (!ignoreMarkerAndLabel)
            {
                DataPoints[index].PlaceMarker((Int32)_columns[index].GetValue(ZIndexProperty) + 1);

                if (DataPoints[index].LabelEnabled.ToLower() == "true")
                    DataPoints[index].AttachLabel(DataPoints[index], depth,(Int32)_canvas3D[index].GetValue(ZIndexProperty));
            }
        }

        private void RenderBar2D(int index, Double left, Double top, Double width, Double height, ChartTypes chartType)
        {
            Path bar = new Path();
            Path shadow = new Path();

            Double xRadiusLimit = 0;
            Double yRadiusLimit = 0;

            if (chartType == ChartTypes.Bar)
            {
                xRadiusLimit = DataPoints[index].RadiusX;
                yRadiusLimit = DataPoints[index].RadiusY;
            }
            else
            {
                String maxDP = "", maxDS = "";
                if (DataPoints[index].CorrectedYValue > 0)
                    GetPositiveMax(DataPoints[index].XValue, ref maxDP, ref maxDS);
                else
                    GetNegativeMax(DataPoints[index].XValue, ref maxDP, ref maxDS);

                if (maxDS == this.Name && maxDP == DataPoints[index].Name)
                {
                    xRadiusLimit = DataPoints[index].RadiusX;
                    yRadiusLimit = DataPoints[index].RadiusY;
                }

            }

            if (xRadiusLimit > width) xRadiusLimit = width;
            if (yRadiusLimit > height / 2) yRadiusLimit = height / 2;

            DataPoints[index].Children.Add(bar);
            DataPoints[index].Children.Add(shadow);

            bar.Width = width;
            bar.Height = height;
            bar.SetValue(TopProperty, (Double) 0);
            bar.SetValue(LeftProperty, (Double) 0);

            bar.Data = Get2DBarPathGeometry(width, height, xRadiusLimit, yRadiusLimit);

            shadow.Width = width;
            shadow.Height = height;
            shadow.SetValue(TopProperty, (Double) ShadowSize);
            shadow.SetValue(LeftProperty, (Double) 0);

            shadow.Data = Get2DBarPathGeometry(width, height, xRadiusLimit, yRadiusLimit);

            bar.Fill = Get2DColumnBarColor(DataPoints[index].Background, DataPoints[index].CorrectedYValue);

            bar.SetValue(ZIndexProperty, 5);

            shadow.Opacity = ShadowEnabled ? 0.8 : 0;

            shadow.Fill = Parser.ParseSolidColor("#66000000");

            shadow.SetValue(ZIndexProperty, 3);

            if (DataPoints[index].CorrectedYValue < 0)
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

            DataPoints[index].ApplyStrokeSettings(bar);

            DataPoints[index].ApplyEventBasedSettings(bar);

            DataPoints[index].PlaceMarker((Int32)bar.GetValue(ZIndexProperty) + 1);

            if (DataPoints[index].LabelEnabled.ToLower() == "true")
                DataPoints[index].AttachLabel(DataPoints[index], 0,DataPoints[index].ZIndex);


            if (width > 10 && height > 10)
                DataPoints[index].ApplyEffects((Int32)bar.GetValue(ZIndexProperty) + 1);
        }

        private Double GetBarHeightDivisionFactor()
        {
            Double factor = 1;
            switch (Plot.ChartType)
            {
                case ChartTypes.Bar:

                    factor = TotalSiblings;

                    break;

                case ChartTypes.StackedBar:
                case ChartTypes.StackedBar100:

                    //Double tempFactor = _parent.CalculateSiblingCountByChartType(ChartTypes.Bar);

                    factor = _parent.GetPlotCountByChartType(Plot.ChartType);

                    //if (tempFactor > factor)
                    //    factor = tempFactor;

                    break;

            }
            return factor;
        }

        private Double CalculateBarHeight()
        {
            Double height = 10;

            Double divisionFactor = GetBarHeightDivisionFactor();

            if (Double.IsNaN(MinDifference) || MinDifference == 0)
                height = Math.Abs((_parent.AxisX.DoubleToPixel(_parent.AxisX.Interval + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / divisionFactor);
            else
                height = Math.Abs((_parent.AxisX.DoubleToPixel(((MinDifference < _parent.AxisX.Interval) ? MinDifference : _parent.AxisX.Interval) + _parent.AxisX.AxisMinimum) - _parent.AxisX.DoubleToPixel(_parent.AxisX.AxisMinimum)) / divisionFactor);

            height -= height * 0.3;

            return height;
        }

        private void PlotStackedBar100()
        {
            Double height;
            Double width;
            Double top;
            Double left;
            Double factor;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            if (_parent.View3D)
            {
                _drawingCanvas = CreateAndInitialize3DDrawingCanvas((Int32)Surface3DCharts.StackedBar100);

                Create3DColumnElements((Int32)Surface3DCharts.StackedBar100);
            }

            height = CalculateBarHeight();
            factor = GetBarHeightDivisionFactor();

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<Double, Point>();

            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

            for (Int32 index = 0; index < DataPoints.Count; index++)
            {
                Double sum = Plot.YValueSum[DataPoints[index].XValue];

                if (Double.IsNaN(sum) || sum == 0) sum = 1;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue);
                point.Y = AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue / sum * 100);

                if (AxisY.AxisMinimum > 0)
                    width = AxisY.DoubleToPixel(AxisY.AxisMinimum) - point.Y;
                else
                    width = Math.Abs(AxisY.DoubleToPixel(0) - point.Y);


                //top = (point.X - height / 2);
                top = point.X + Index * height - (factor * height) / 2;


                if (DataPoints[index].CorrectedYValue >= 0)
                {

                    left = AxisY.DoubleToPixel(0);

                    if (Plot.TopBottom.ContainsKey(DataPoints[index].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[index].XValue].X;

                        Plot.TopBottom[DataPoints[index].XValue] = new Point(left + width, Plot.TopBottom[DataPoints[index].XValue].Y);
                    }
                    else
                        Plot.TopBottom[DataPoints[index].XValue] = new Point(left + width, left);

                }
                else
                {

                    left = point.Y;

                    if (Plot.TopBottom.ContainsKey(DataPoints[index].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[index].XValue].Y - width;

                        Plot.TopBottom[DataPoints[index].XValue] = new Point(Plot.TopBottom[DataPoints[index].XValue].X, left);
                    }
                    else
                        Plot.TopBottom[DataPoints[index].XValue] = new Point(left + width, left);
                }

                DataPoints[index].SetValue(WidthProperty, width);
                DataPoints[index].SetValue(HeightProperty, height);
                DataPoints[index].SetValue(LeftProperty, (Double) left);
                DataPoints[index].SetValue(TopProperty, (Double) top);

                if (_parent.View3D)
                {
                    if (IsBarOutOfPlotRange(top, height)) continue;

                    RenderBar3D(index, left, top, width, height, depth, initialDepth, ChartTypes.StackedBar100);

                }
                else
                {
                    RenderBar2D(index, left, top, width, height, ChartTypes.StackedBar100);
                }

            }
            //Draw Zero plank
            if (!_parent._plankDrawState[(Int32)Surface3DCharts.StackedBar100])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Vertical);
                    _parent._plankDrawState[(Int32)Surface3DCharts.StackedBar100] = true;
                }
            }
        }

        private void PlotStackedBar()
        {
            Double height;
            Double width;
            Double top;
            Double left;
            Double factor;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            if (_parent.View3D)
            {
                _drawingCanvas = CreateAndInitialize3DDrawingCanvas((Int32)Surface3DCharts.StackedBar);

                Create3DColumnElements((Int32)Surface3DCharts.StackedBar);
            }

            height = CalculateBarHeight();
            factor = GetBarHeightDivisionFactor();

            if (Plot.TopBottom == null)
                Plot.TopBottom = new System.Collections.Generic.Dictionary<Double, Point>();

            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

            for (Int32 index = 0; index < DataPoints.Count; index++)
            {
                if (Double.IsNaN(DataPoints[index].CorrectedYValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue);
                point.Y = AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue);


                if (AxisY.AxisMinimum > 0)
                    width = AxisY.DoubleToPixel(AxisY.AxisMinimum) - point.Y;
                else
                    width = Math.Abs(AxisY.DoubleToPixel(0) - point.Y);


                //top = (point.X - height / 2);
                top = point.X + Index * height - (factor * height) / 2;

                if (DataPoints[index].CorrectedYValue >= 0)
                {

                    left = AxisY.DoubleToPixel(0);

                    if (Plot.TopBottom.ContainsKey(DataPoints[index].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[index].XValue].X;

                        Plot.TopBottom[DataPoints[index].XValue] = new Point(left + width, Plot.TopBottom[DataPoints[index].XValue].Y);
                    }
                    else
                        Plot.TopBottom[DataPoints[index].XValue] = new Point(left + width, left);

                }
                else
                {
                    left = point.Y;

                    if (Plot.TopBottom.ContainsKey(DataPoints[index].XValue))
                    {
                        left = Plot.TopBottom[DataPoints[index].XValue].Y - width;

                        Plot.TopBottom[DataPoints[index].XValue] = new Point(Plot.TopBottom[DataPoints[index].XValue].X, left);
                    }
                    else
                        Plot.TopBottom[DataPoints[index].XValue] = new Point(left + width, left);
                }

                DataPoints[index].SetValue(WidthProperty, width);
                DataPoints[index].SetValue(HeightProperty, height);
                DataPoints[index].SetValue(LeftProperty, (Double) left);
                DataPoints[index].SetValue(TopProperty, (Double) top);

                if (_parent.View3D)
                {
                    if (IsBarOutOfPlotRange(top, height)) continue;

                    RenderBar3D(index, left, top, width, height, depth, initialDepth, ChartTypes.StackedBar);
                }
                else
                {
                    RenderBar2D(index, left, top, width, height, ChartTypes.StackedBar);
                }

            }
            //Draw Zero plank
            if (!_parent._plankDrawState[(Int32)Surface3DCharts.StackedBar])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Vertical);
                    _parent._plankDrawState[(Int32)Surface3DCharts.StackedBar] = true;
                }
            }
        }

        private void PlotBar()
        {
            Double width;
            Double height;
            Double left;
            Double top;
            Double factor;
            Point point = new Point(); // This is the X and Y co-ordinate of the XValue and Yvalue combined.

            if (_parent.View3D)
            {
                _drawingCanvas = CreateAndInitialize3DDrawingCanvas((Int32)Surface3DCharts.Bar);

                Create3DColumnElements((Int32)Surface3DCharts.Bar);
            }

            height = CalculateBarHeight();
            factor = GetBarHeightDivisionFactor();

            List<Double> checkDrawPositive = new List<Double>();
            List<Double> checkDrawNegetive = new List<Double>();
            Double finalYValue;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;

            for (Int32 index = 0; index < DataPoints.Count; index++)
            {
                if (Double.IsNaN(DataPoints[index].CorrectedYValue)) continue;

                point.X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue);

                if (!checkDrawPositive.Contains(DataPoints[index].XValue) && DataPoints[index].CorrectedYValue >= 0)
                {
                    finalYValue = _maxVals[DataPoints[index].XValue].X;
                    if (DataPoints[index].CorrectedYValue != finalYValue) continue;
                    checkDrawPositive.Add(DataPoints[index].XValue);
                }
                else if (!checkDrawNegetive.Contains(DataPoints[index].XValue) && DataPoints[index].CorrectedYValue < 0)
                {
                    finalYValue = _maxVals[DataPoints[index].XValue].Y;
                    if (DataPoints[index].CorrectedYValue != finalYValue) continue;
                    checkDrawNegetive.Add(DataPoints[index].XValue);
                }
                else
                {
                    continue;
                }
                point.Y = AxisY.DoubleToPixel(finalYValue);

                top = point.X + Index * height - (factor * height) / 2;

                if (AxisY.AxisMinimum > 0)
                    width = Math.Abs(point.Y - AxisY.DoubleToPixel(AxisY.AxisMinimum));
                else
                    width = Math.Abs(point.Y - AxisY.DoubleToPixel(0));


                if (AxisY.AxisMinimum > 0)
                {
                    left = AxisY.DoubleToPixel(AxisY.AxisMinimum);
                }
                else
                {
                    if (DataPoints[index].CorrectedYValue > 0)
                        left = AxisY.DoubleToPixel(0);
                    else
                        left = AxisY.DoubleToPixel(0) - width;
                }

                DataPoints[index].SetValue(WidthProperty, width);
                DataPoints[index].SetValue(HeightProperty, height);
                DataPoints[index].SetValue(LeftProperty, (Double) left);
                DataPoints[index].SetValue(TopProperty, (Double)top);

                if (_parent.View3D)
                {
                    if (IsBarOutOfPlotRange(top, height)) continue;

                    RenderBar3D(index,left,top,width,height,depth,initialDepth,ChartTypes.Bar);
                    
                }
                else
                {

                    RenderBar2D(index,left,top,width,height,ChartTypes.Bar);
                }

            }
            //Draw Zero plank
            if (!_parent._plankDrawState[(Int32)Surface3DCharts.Bar])
            {
                if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
                {
                    DrawZeroPlank(initialDepth, depth, 0, Orientation.Vertical);
                    _parent._plankDrawState[(Int32)Surface3DCharts.Bar] = true;
                }
            }
        }

        private Int32 GetBarZIndex(Double left, Double top, Boolean isPositive)
        {
            Int32 yOffset = (Int32)((Double)_parent.PlotArea.GetValue(TopProperty) + _parent.PlotArea.Height - top);
            Int32 zindex = (Int32)(Math.Sqrt(Math.Pow(left / 2, 2) + Math.Pow(yOffset, 2)));
            if (isPositive)
                return zindex;
            else
                return Int32.MinValue + zindex;
        }

        private Int32 GetStackedBarZIndex(Double left, Double top, Boolean isPositive)
        {
            Double zOffset = Math.Pow(10, (Int32)(Math.Log10(_parent.PlotArea.Width) - 1));
            Int32 iOffset = (Int32)(left / (zOffset < 1 ? 1 : zOffset));
            Int32 zindex = (Int32)((_parent.PlotArea.Height - top)*zOffset) + iOffset;
            if (isPositive)
                return zindex;
            else
                return Int32.MinValue + zindex;

        }

        private Int32 GetStackedBar100ZIndex(Double left, Double top, Boolean isPositive)
        {
            return GetStackedBarZIndex(left, top, isPositive);
        }

        private void PlotLine()
        {
            _drawingCanvas = CreateAndInitialize3DAreaLineCanvas();

            _line = new Polyline();
            _lineShadow = new Polyline();

            SetTag(_line,this.Name);
            SetTag(_lineShadow, this.Name);

            if (_parent.View3D)
            {
                _drawingCanvas.Children.Add(_line);
                _drawingCanvas.Children.Add(_lineShadow);
            }
            else
            {
                this.Children.Add(_line);
                this.Children.Add(_lineShadow);
            }

            Int32 index;
            Double strokeThickness = ((_parent.Width * _parent.Height) + 25000) / 35000;
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count) * (_parent.IndexList[this.Name] - (_parent.Count == 0 ? 0 : 1));
            Double depth = _parent.AxisX.MajorTicks.TickLength / (_parent.Count == 0 ? 1 : _parent.Count);

            if (Double.IsNaN(initialDepth)) initialDepth = 0;
            if (Double.IsNaN(depth)) depth = _parent.AxisX.MajorTicks.TickLength;

            _line.Stroke = (Background);
            _lineShadow.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(127, 127, 127, 127));

            _line.StrokeDashArray = Parser.GetStrokeDashArray(LineStyle);
            _lineShadow.StrokeDashArray = Parser.GetStrokeDashArray(LineStyle);

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

            _line.SetValue(ZIndexProperty, (Int32)GetValue(ZIndexProperty) + 20);
            _lineShadow.SetValue(ZIndexProperty, (Int32)_line.GetValue(ZIndexProperty) - 1);



            List<Point> points = new List<Point>();
            List<Point> points2 = new List<Point>();

            Double offsetX = 0, offsetY = 0;
            if (_parent.View3D)
            {
                offsetX = (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);
                offsetY = (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
            }

            for (index = 0; index < DataPoints.Count; index++)
            {
                if (Double.IsNaN(DataPoints[index].CorrectedYValue)) continue;
                points.Add(new Point(_parent.AxisX.DoubleToPixel(DataPoints[index].XValue) + offsetX, AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue) + offsetY));
                points2.Add(new Point(_parent.AxisX.DoubleToPixel(DataPoints[index].XValue) + offsetX + strokeThickness / 2, AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue) + offsetY + strokeThickness / 2));

                DataPoints[index].SetValue(LeftProperty, (Double) ( _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) - (_parent.View3D ? (depth + initialDepth) : 0)));
                DataPoints[index].SetValue(TopProperty, (Double) ( AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue) + (_parent.View3D ? (depth + initialDepth) : 0)));
                DataPoints[index].Width = 1;
                DataPoints[index].Height = 1;

                DataPoints[index].PlaceMarker((Int32)_line.GetValue(ZIndexProperty) + 1);

                if (DataPoints[index].LabelEnabled.ToLower() == "true")
                    DataPoints[index].AttachLabel(DataPoints[index], depth, DataPoints[index].ZIndex);
            }
            _line.Points = Converter.ArrayToCollection(points.ToArray());
            _lineShadow.Points = Converter.ArrayToCollection(points2.ToArray());
            if (!ShadowEnabled)
            {
                _lineShadow.Opacity = 0;
            }

        }

        private void PlotArea()
        {
            Int32 index;
            Point[] points = new Point[4];
            Double initialDepth = _parent.AxisX.MajorTicks.TickLength / _parent.Count * _parent.IndexList[this.Name];
            Double depth = _parent.AxisX.MajorTicks.TickLength / _parent.Count;
            Double labeldepth = _parent.View3D ? 0 : depth;
            for (index = 0; index < 4; index++) points[index] = new Point();

            foreach (DataPoint datapoint in DataPoints)
            {
                if (_parent.View3D)
                {
                    datapoint.SetValue(LeftProperty, (Double) ( (Double)_parent.AxisX.DoubleToPixel(datapoint.XValue) + (Double)_parent.GetValue(LeftProperty) - (depth + initialDepth)));
                    datapoint.SetValue(TopProperty, (Double) ( (Double)AxisY.DoubleToPixel(datapoint.CorrectedYValue) + (Double)_parent.GetValue(TopProperty) + (depth + initialDepth)));
                }
                else
                {
                    datapoint.SetValue(LeftProperty, (Double) _parent.AxisX.DoubleToPixel(datapoint.XValue));
                    datapoint.SetValue(TopProperty, (Double) AxisY.DoubleToPixel(datapoint.CorrectedYValue));
                }
                datapoint.Width = 1;
                datapoint.Height = 1;
            }

            for (index = 0; index < _areas.Length; index++)
            {
                if (Double.IsNaN(DataPoints[index].CorrectedYValue)) continue;
                if (Double.IsNaN(DataPoints[index + 1].CorrectedYValue))
                {
                    index++;
                    continue;
                }

                if (!_parent.View3D)
                {

                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[index + 1].XValue);
                    points[2].X = points[1].X;
                    points[3].X = points[0].X;

                    points[0].Y = AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue);
                    points[1].Y = AxisY.DoubleToPixel(DataPoints[index + 1].CorrectedYValue);
                    points[2].Y = AxisY.DoubleToPixel(AxisY.AxisMinimum > 0 ? AxisY.AxisMinimum : 0);
                    points[3].Y = points[2].Y;

                    _areas[index].Points = Converter.ArrayToCollection(points);

                    Double left = (Double)DataPoints[index].GetValue(LeftProperty);
                    Double top = (Double)DataPoints[index].GetValue(TopProperty);
                    DataPoints[index].points.Add(new Point(points[0].X - left, points[0].Y - top));
                    DataPoints[index].points.Add(new Point(points[1].X - left, points[1].Y - top));
                    DataPoints[index].points.Add(new Point(points[2].X - left, points[2].Y - top));
                    DataPoints[index].points.Add(new Point(points[3].X - left, points[3].Y - top));


                    _areas[index].SetValue(TopProperty, (Double) (-top));
                    _areas[index].SetValue(LeftProperty, (Double) ( -left));

                    if (DataPoints[index].Background.GetType().Name == "SolidColorBrush" && LightingEnabled)
                    {
                        SolidColorBrush brush = DataPoints[index].Background as SolidColorBrush;
                        String linbrush;
                        linbrush = "-90;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.745);
                        linbrush += ",0;";
                        linbrush += Parser.GetDarkerColor(brush.Color, 0.99);
                        linbrush += ",1";
                        _areas[index].Fill = Parser.ParseLinearGradient(linbrush);
                    }
                    else
                    {
                        _areas[index].Fill = (DataPoints[index].Background);
                    }


                    DataPoints[index].ApplyStrokeSettings(_areas[index]);

                    DataPoints[index].ApplyEventBasedSettings(_areas[index]);

                    DataPoints[index].ApplyEffects((Int32)_areas[index].GetValue(ZIndexProperty) + 1);

                }
                else
                {

                    points[0].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);
                    points[1].X = _parent.AxisX.DoubleToPixel(DataPoints[index + 1].XValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);
                    points[2].X = points[1].X;
                    points[3].X = _parent.AxisX.DoubleToPixel(DataPoints[index].XValue) + (Double)_parent.PlotArea.GetValue(LeftProperty) - (depth + initialDepth);

                    points[0].Y = AxisY.DoubleToPixel(DataPoints[index].CorrectedYValue) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[1].Y = AxisY.DoubleToPixel(DataPoints[index + 1].CorrectedYValue) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[2].Y = AxisY.DoubleToPixel(AxisY.AxisMinimum > 0 ? AxisY.AxisMinimum : 0) + (Double)_parent.PlotArea.GetValue(TopProperty) + (depth + initialDepth);
                    points[3].Y = points[2].Y;

                    _areas[index].Points = Converter.ArrayToCollection(points);

                    _areas[index].Stroke = (DataPoints[index].BorderColor);
                    _areaShadows[index].Stroke = (DataPoints[index].BorderColor);
                    _areaTops[index].Stroke = (DataPoints[index].BorderColor);


                    _areas[index].StrokeThickness = DataPoints[index].BorderThickness;
                    _areaShadows[index].StrokeThickness = DataPoints[index].BorderThickness;
                    _areaTops[index].StrokeThickness = DataPoints[index].BorderThickness;

                    _canvas3D[index].Opacity = this.Opacity * DataPoints[index].Opacity;


                    _areaShadows[index].Height = Math.Abs(points[2].Y - points[1].Y);
                    _areaShadows[index].Width = depth;
                    if (DataPoints[index + 1].CorrectedYValue >= 0)
                        _areaShadows[index].SetValue(TopProperty, (Double) points[1].Y);
                    else
                        _areaShadows[index].SetValue(TopProperty, (Double) points[2].Y);
                    _areaShadows[index].SetValue(LeftProperty, (Double) points[1].X);

                    SkewTransform st1 = new SkewTransform();
                    st1.AngleY = -45;
                    st1.CenterX = 0;
                    st1.CenterY = 0;
                    st1.AngleX = 0;
                    _areaShadows[index].RenderTransform = st1;


                    points[0].X += depth;
                    points[1].X += depth;

                    if (DataPoints[index + 1].CorrectedYValue < 0 && DataPoints[index].CorrectedYValue < 0)
                    {
                        points[2].Y = points[3].Y;
                        points[3].Y = points[2].Y;
                        points[0].Y = points[2].Y - depth;
                        points[1].Y = points[3].Y - depth;
                    }
                    else
                    {
                        points[2].Y = points[1].Y;
                        points[3].Y = points[0].Y;
                        points[0].Y -= depth;
                        points[1].Y -= depth;
                    }
                    _areaTops[index].Points = Converter.ArrayToCollection(points);

                    //This part of the code generates the 3D gradient
                    #region Color Gradient
                    Brush brush2 = null;
                    Brush brushShade = null;
                    Brush brushTop = null;
                    Brush tempBrush = DataPoints[index].Background;
                    if (tempBrush.GetType().Name == "LinearGradientBrush")
                    {
                        LinearGradientBrush brush = DataPoints[index].Background as LinearGradientBrush;
                        brush2 = (DataPoints[index].Background);

                        brushShade = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""   EndPoint=""1,0"" StartPoint=""0,1""></LinearGradientBrush>");

                        brushTop = (LinearGradientBrush)XamlReader.Load(@"<LinearGradientBrush xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""   StartPoint=""-0.5,1.5"" EndPoint=""0.5,0"" ></LinearGradientBrush>");

                        Parser.GenerateDarkerGradientBrush(brush, brushShade as LinearGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as LinearGradientBrush, 0.85);

                        RotateTransform rt = new RotateTransform();
                        rt.Angle = -45;
                        brushTop.RelativeTransform = rt;

                    }
                    else if (tempBrush.GetType().Name == "RadialGradientBrush")
                    {
                        RadialGradientBrush brush = DataPoints[index].Background as RadialGradientBrush;
                        brush2 = (DataPoints[index].Background);

                        brushShade = new RadialGradientBrush();
                        brushTop = new RadialGradientBrush();
                        (brushShade as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        (brushTop as RadialGradientBrush).GradientOrigin = brush.GradientOrigin;
                        Parser.GenerateDarkerGradientBrush(brush, brushShade as RadialGradientBrush, 0.75);
                        Parser.GenerateLighterGradientBrush(brush, brushTop as RadialGradientBrush, 0.85);
                    }
                    else if (tempBrush.GetType().Name == "SolidColorBrush")
                    {
                        SolidColorBrush brush = DataPoints[index].Background as SolidColorBrush;
                        if (LightingEnabled)
                        {
                            String linbrush;
                            //Generate a gradient String
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
                            brush2 = (DataPoints[index].Background);
                            brushTop = new SolidColorBrush(Parser.GetLighterColor(((SolidColorBrush)DataPoints[index].Background).Color, 0.75));
                            brushShade = new SolidColorBrush(Parser.GetDarkerColor(((SolidColorBrush)DataPoints[index].Background).Color, 0.85));
                        }
                    }
                    else
                    {
                        brush2 = (DataPoints[index].Background);
                        brushTop = (DataPoints[index].Background);
                        brushShade = (DataPoints[index].Background);
                    }
                    #endregion Color Gradient

                    _areas[index].Fill = (brush2);
                    _areaShadows[index].Fill = (brushShade);
                    _areaTops[index].Fill = (brushTop);

                    Int32 zindex = (Int32)(points[0].X + _parent.AxisX.MajorTicks.TickLength);

                    _areas[index].SetValue(ZIndexProperty, 5);
                    _areaShadows[index].SetValue(ZIndexProperty, 0);
                    _areaTops[index].SetValue(ZIndexProperty, 0);
                    _canvas3D[index].SetValue(ZIndexProperty, zindex);

                    DataPoints[index].ApplyStrokeSettings(_areas[index]);
                    DataPoints[index].ApplyStrokeSettings(_areaShadows[index]);
                    DataPoints[index].ApplyStrokeSettings(_areaTops[index]);

                    DataPoints[index].ApplyEventBasedSettings(_canvas3D[index]);

                }

                DataPoints[index].PlaceMarker((Int32)_areas[index].GetValue(ZIndexProperty) + 1);

                if (DataPoints[index].LabelEnabled.ToLower() == "true")
                    DataPoints[index].AttachLabel(DataPoints[index], labeldepth, (Int32)_areas[index].GetValue(ZIndexProperty) + 1);

            }
            index = _areas.Length;
            
            Int32 zIndex=0;
            //If data series contains only one datapoint no area exists hence draw the marker and label only
            if (DataPoints.Count == 1)
            {
                zIndex = (Int32)DataPoints[index].ZIndex + 1;
            }
            else
            {
                zIndex =(Int32)_areas[index - 1].GetValue(ZIndexProperty) + 1;
            }

            DataPoints[index].PlaceMarker(zIndex);

            if (DataPoints[index].LabelEnabled.ToLower() == "true")
                DataPoints[index].AttachLabel(DataPoints[index], labeldepth, zIndex);

            if (AxisY.AxisMaximum > 0 && AxisY.AxisMinimum < 0 && _parent.View3D)
            {
                DrawZeroPlank(initialDepth, depth, 0, Orientation.Horizontal);
            }
            
        }

        private void CreateAuxPath(ref Path path, Int32 index, Brush brush)
        {
            path = new Path();
            path.Fill = (brush);
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

                    _parent.CollectStackContent(dataPoint.XValue, dataPoint.CorrectedYValue);

                    if (!Double.IsNaN(dataPoint.XValue) && !_parent.PlotDetails.AxisLabels.ContainsKey(dataPoint.XValue))
                    {
                        if (!String.IsNullOrEmpty(dataPoint.AxisLabel))
                        {
                            _parent.PlotDetails.AxisLabels.Add(dataPoint.XValue, dataPoint.AxisLabel);
                        }
                    }
                    if (_maxVals.ContainsKey(dataPoint.XValue))
                    {
                        if (dataPoint.CorrectedYValue > 0)
                            _maxVals[dataPoint.XValue] = new Point(Math.Max(_maxVals[dataPoint.XValue].X, dataPoint.CorrectedYValue), _maxVals[dataPoint.XValue].Y);
                        else
                            _maxVals[dataPoint.XValue] = new Point(_maxVals[dataPoint.XValue].X, Math.Min(_maxVals[dataPoint.XValue].Y, dataPoint.CorrectedYValue));
                    }
                    else
                    {
                        if (dataPoint.CorrectedYValue > 0)
                            _maxVals.Add(dataPoint.XValue, new Point(dataPoint.CorrectedYValue, 0));
                        else
                            _maxVals.Add(dataPoint.XValue, new Point(0, dataPoint.CorrectedYValue));
                    }

                }
            }
            DataPoints.Sort();

            _parent.PlotDetails.MaxDataPoints = Math.Max(_parent.PlotDetails.MaxDataPoints, DataPoints.Count);

        }

        private void SetTag(FrameworkElement element,String tag)
        {
            if(element!=null)
                element.Tag = tag;
        }

        #endregion Private Methods

        #region Internal Methods

        internal void DrawPlotBorder()
        {
            _borderRectangle.Width = this.Width;
            _borderRectangle.Height = Math.Abs(this.Height);

            _borderRectangle.Stroke = (_parent.PlotArea.BorderColor);
            _borderRectangle.StrokeThickness = _parent.PlotArea.BorderThickness;
            _borderRectangle.RadiusX = _parent.PlotArea.RadiusX;
            _borderRectangle.RadiusY = _parent.PlotArea.RadiusY;
            _borderRectangle.StrokeDashArray = Parser.GetStrokeDashArray(_parent.PlotArea.BorderStyle);

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

            if (_parent.View3D)
                _drawingCanvas.SetValue(ZIndexProperty, this.ZIndex);

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

        internal void PlaceDataSeries()
        {
            SetLeft();
            SetTop();
            SetWidth();
            SetHeight();
        }

        #endregion Internal Methods

        #region Internal Properties

        internal Double ShadowSize
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

        internal AxisY AxisY
        {
            get
            {
                return Plot.AxisY;
            }
        }

        #endregion Internal Properties

        #region Data

        private Int32 _index;
        private Brush _background;
        private String _lightingEnabled;
        private String _shadowEnabled;
        private String _color;

        private AxisType _axisYType;

        internal Canvas _drawingCanvas;

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
        private String _labelFontWeight;
        private String _labelFontFamily;
        private String _labelStyle;
        private Brush _labelLineColor;
        private Double _labelLineThickness;
        private String _labelLineStyle;
        private String _labelLineEnabled;

        private String _labelFontStyle;

        internal Chart _parent;

        internal Polyline _line, _lineShadow;

        internal Polygon[] _areas;
        internal Path[] _columns;
        internal Path[] _columnSides;
        internal Path[] _pies;

        //Charting objects for 3d facing side
        internal Path[] _columnTops;
        internal Rectangle[] _areaShadows;
        internal Polygon[] _areaTops;
        internal Path[] _pieSides, _pieRight, _pieLeft;
        internal Path[] _doughnut;
        internal Path auxSide1, auxSide2,  auxSide4,auxSide5;
        internal Int32 auxID1, auxID2, auxID4, auxID5;
        internal Canvas[] _canvas3D;
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
        private Dictionary<FrameworkElement, Point> _positionOffset = new Dictionary<FrameworkElement, Point>();
        private Dictionary<FrameworkElement, Point> _labelPosOffset = new Dictionary<FrameworkElement, Point>();
        

        private enum Surface3DCharts
        {
            Bar = 0, Column = 1, Line = 2, Area = 3,
            StackedColumn = 4, StackedColumn100 = 5,
            StackedAreaPrimary = 6, StackedAreaSecondary = 7, 
            StackedArea100Primary = 8, StackedArea100Secondary=9, 
            StackedBar = 10, StackedBar100 = 11
        }

        private struct PieDoughnutParams
        {
            public Double _maxLabelWidth;
            public Double _maxLabelHeight;
            public Double _maxExplodeOffset;
            public Double _dataSeriesTotal;
            public Double _plotHeight;
            public Double _plotWidth;
            public Double _plotDepth;

            public Boolean _isLabelEnabled;
            public Boolean _isLabelOutside;

            public Point _plotCenter;
        }

        
        #endregion Data

    }
}

