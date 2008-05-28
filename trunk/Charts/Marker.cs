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
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Markup;
using Visifire.Commons;

namespace Visifire.Charts
{
    public class Marker : Canvas
    {
        #region Public Methods
        public Marker()
        {
            _path = new Path();
            _shadow = new Path();
            this.Children.Add(_path);
            this.Children.Add(_shadow);
            
            SetDefaults();
        }

        #endregion Public Methods

        #region Marker Properties

        public new String Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                DrawMarker();
            }
        }

        public Double ImageScale
        {
            get
            {
                return _imageScale;
            }
            set
            {
                _imageScale = value;

                ScaleTransform st = new ScaleTransform();
                st.ScaleX = value;
                st.ScaleY = value;

                this.RenderTransform = st;
                _size *= value;
            }
        }

        public Double BorderThickness
        {
            get
            {
                return _path.StrokeThickness; ;
            }
            set
            {
                _path.StrokeThickness = value;
            }
        }

        public Double Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                
                if (_style != null)
                {
                    Style = _style;
                    this.SetValue(WidthProperty, value);
                    this.SetValue(HeightProperty, value);
                }
            }
        }

        public Brush Color
        {
            get
            {
                return _path.Fill;
            }
            set
            {
                _path.Fill = value;
            }

        }

        public Brush BorderColor
        {
            get
            {
                return _path.Stroke;
            }
            set
            {
                _path.Stroke = value;
            }
        }

        public String ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {

                _imagePath = Parser.BuildAbsolutePath( value);
                _style = "Square";

                ImageBrush imageBrush;
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" ImageSource=\"" + _imagePath + "\"/>";
                imageBrush = (ImageBrush)XamlReader.Load(XAMLimage);
                imageBrush.Stretch = ImageStretch;
                Color = imageBrush;                
            }
        }

        public Stretch ImageStretch
        {
            get
            {
                return _imageStretch;
            }
            set
            {
                _imageStretch = value;
                if (_path.Fill.GetType().Name == "ImageBrush")
                {
                    (_path.Fill as ImageBrush).Stretch = value;
                }
            }
        }
        #endregion Marker Properties

        #region Internal Methods
        
        internal void DrawMarker()
        {
            
            _path.SetValue(ZIndexProperty, 2);
            _shadow.SetValue(ZIndexProperty, 1);
            _shadow.SetValue(LeftProperty, 3);
            _shadow.SetValue(TopProperty, 3);
            _shadow.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x7f, 0x7f, 0x7f, 0x7f));

            List<PathGeometryParams> pathGeometryList;

            switch (_style.ToUpper())
            {
                case "CIRCLE":

                    EllipseGeometry eg = new EllipseGeometry();
                    
                    if (!String.IsNullOrEmpty(ImagePath))
                    {
                        eg.Center = new Point(Width / 2, Height / 2);
                        eg.RadiusY = Width / 2;
                        eg.RadiusX = Height / 2;
                        this.Clip = eg;
                    }
                    else
                    {
                        
                        eg.Center = new Point(_size / 2, _size / 2);
                        eg.RadiusY = _size / 2;
                        eg.RadiusX = _size / 2;
                        _path.Data = eg;
                        if (Shadow)
                        {
                            eg = new EllipseGeometry();
                            eg.Center = new Point(_size / 2, _size / 2);
                            eg.RadiusY = _size / 2;
                            eg.RadiusX = _size / 2;
                            _shadow.Data = eg;

                        }
                    }
                    
                    break;

                case "SQUARE":

                    
                    RectangleGeometry rg = new RectangleGeometry();
                    rg.Rect = new Rect(0, 0, _size, _size);
                    _path.Data = rg;
                    if (Shadow)
                    {
                        rg = new RectangleGeometry();
                        rg.Rect = new Rect(0, 0, _size, _size);

                        _shadow.Data = rg;

                    }
                    break;
                case "CROSS":
                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, _size * 0.4)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.9, 0)));

                    pathGeometryList.Add(new ArcSegmentParams(new Size(_size * 0.05, _size * 0.05), 0, false, SweepDirection.Clockwise, new Point(_size, _size * 0.1)));

                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.6, _size * 0.5)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size, _size * 0.9)));

                    pathGeometryList.Add(new ArcSegmentParams(new Size(_size * 0.05, _size * 0.05), 0, false, SweepDirection.Clockwise, new Point(_size * 0.9, _size)));

                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, _size * 0.6)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.1, _size)));

                    pathGeometryList.Add(new ArcSegmentParams(new Size(_size * 0.05, _size * 0.05), 0, false, SweepDirection.Clockwise, new Point(0, _size * 0.9)));

                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.4, _size * 0.5)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(0, _size * 0.1)));

                    pathGeometryList.Add(new ArcSegmentParams(new Size(_size * 0.05, _size * 0.05), 0, false, SweepDirection.Clockwise, new Point(_size * 0.1, 0)));

                    _path.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(_size * 0.1, 0), pathGeometryList);
                    if (Shadow)
                    {

                        _shadow.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(_size * 0.1, 0), pathGeometryList);

                    }
                    break;
                case "CROSS2":

                    pathGeometryList = new List<PathGeometryParams>();
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.1, 0)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, _size * 0.4)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.9, 0)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size, 0)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size, _size * 0.1)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.6, _size * 0.5)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size, _size * 0.9)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size, _size)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.9, _size)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, _size * 0.6)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.1, _size)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(0, _size)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(0, _size * 0.9)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.4, _size * 0.5)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(0, _size * 0.1)));
                    pathGeometryList.Add(new LineSegmentParams(new Point(0, 0)));
                    _path.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(0, 0), pathGeometryList);

                    if (Shadow)
                    {

                        _shadow.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(0, 0), pathGeometryList);

                    }

                    break;

                case "TRIANGLE":
                    
                    
                    if (!String.IsNullOrEmpty(ImagePath))
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(Width * 0.5, 0)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(Width, Height)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(0, Height)));
                        this.Clip = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(0, Height), pathGeometryList);
                    }
                    else
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, 0)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(_size, _size)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(0, _size)));
                        _path.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(0, _size), pathGeometryList);
                        if (Shadow)
                        {
                            _shadow.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(0, _size), pathGeometryList);
                        }
                    }

                    break;
                case "DIAMOND":
                    if (!String.IsNullOrEmpty(ImagePath))
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(Width, Height * 0.5)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(Width * 0.5, Height)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(0, Height * 0.5)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(Width * 0.5, 0)));
                        this.Clip = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(Width * 0.5, 0), pathGeometryList);
                    }
                    else
                    {
                        pathGeometryList = new List<PathGeometryParams>();
                        pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.9, _size * 0.5)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, _size)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(0.1 * _size, _size * 0.5)));
                        pathGeometryList.Add(new LineSegmentParams(new Point(_size * 0.5, 0)));
                        _path.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(_size * 0.5, 0), pathGeometryList);
                        if (Shadow)
                        {
                            _shadow.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(_size * 0.5, 0), pathGeometryList);
                        }
                    }
                    break;
            }
            
        }

        internal void SetTags(String tag)
        {
            SetTag(_path,tag);
            SetTag(_shadow, tag);
        }

        internal Boolean Shadow
        {
            get;
            set;
        }

        #endregion Internal Methods

        #region Private Methods
        private void SetDefaults()
        {

        }

        private void SetTag(FrameworkElement element, String tag)
        {
            if (element != null)
                element.Tag = tag;
        }
        #endregion Private Methods

        #region Data


        private Double _imageScale;
        private Double _size;
        private String _style;
        private String _imagePath;

        private Path _path;
        private Path _shadow;
        private Stretch _imageStretch;
        #endregion Data
    }
}
