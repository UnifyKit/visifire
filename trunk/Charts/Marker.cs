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
using System.Net;
using System.Windows.Markup;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
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
            _image = new System.Windows.Controls.Image();
            _image.Opacity = 0;

            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = new TimeSpan(0,0,0,0,100);
            _timer.Tick += new EventHandler(_timer_Tick);

            

            this.Children.Add(_image);

            SetDefaults();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (_image.DownloadProgress == 100)
            {
                _timer.Stop();

                this.SetValue(WidthProperty, (_image.Width * ImageScale));
                this.SetValue(HeightProperty, (_image.Height * ImageScale));

                this.SetValue(LeftProperty, (Double)this.GetValue(LeftProperty) - (_image.Width * ImageScale) / 2);
                this.SetValue(TopProperty, (Double)this.GetValue(TopProperty) - (_image.Height * ImageScale) / 2);
                

                _downloaded = true;
                DrawMarker();
                _image.Opacity = 1;
            }
        }

        #endregion Public Methods

        #region Marker Properties

        public String Style
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
        internal Boolean Shadow
        {
            get;
            set;
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

                _imagePath = value;
                _style = "Square";

                
                ImageBrush ib;
                Uri ur = new Uri(_imagePath, UriKind.RelativeOrAbsolute);
                if (ur.IsAbsoluteUri)
                {
                    _imagePath = ur.AbsoluteUri;
                }
                else
                {
                    UriBuilder ub = new UriBuilder(Application.Current.Host.Source);
                    String sourcePath = ub.Path.Substring(0, ub.Path.LastIndexOf('/') + 1);
                    UriBuilder ub2 = new UriBuilder(ub.Scheme, ub.Host, ub.Port, sourcePath + value);
                    _imagePath = ub2.ToString();
                }
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" ImageSource=\"" + _imagePath + "\"/>";
                ib = (ImageBrush)XamlReader.Load(XAMLimage);
                Color = ib;                
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

            String pathCross;
            switch (_style.ToUpper())
            {
                case "CIRCLE":

                    EllipseGeometry eg = new EllipseGeometry();
                    
                    if (!String.IsNullOrEmpty(ImagePath) && _downloaded)
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

                    pathCross = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathCross += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", _size * 0.1, 0);

                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, _size * 0.4);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.9, 0);

                    pathCross += String.Format(@"<ArcSegment Point=""{0},{1}""  ", _size, _size * 0.1);
                    pathCross += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", _size * 0.05, _size * 0.05);
                    pathCross += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.6, _size * 0.5);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size, _size * 0.9);

                    pathCross += String.Format(@"<ArcSegment Point=""{0},{1}""  ", _size * 0.9, _size);
                    pathCross += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", _size * 0.05, _size * 0.05);
                    pathCross += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, _size * 0.6);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.1, _size);

                    pathCross += String.Format(@"<ArcSegment Point=""{0},{1}""  ", 0, _size * 0.9);
                    pathCross += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", _size * 0.05, _size * 0.05);
                    pathCross += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.4, _size * 0.5);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, _size * 0.1);

                    pathCross += String.Format(@"<ArcSegment Point=""{0},{1}""  ", _size * 0.1, 0);
                    pathCross += String.Format(@"Size=""{0},{1}"" RotationAngle=""0"" ", _size * 0.05, _size * 0.05);
                    pathCross += String.Format(@"SweepDirection=""Clockwise"" />");

                    pathCross += String.Format("</PathFigure.Segments></PathFigure>");
                    pathCross += String.Format("</PathGeometry.Figures></PathGeometry>");


                    _path.Data = (PathGeometry)XamlReader.Load(pathCross);
                    if (Shadow)
                    {

                        _shadow.Data = (PathGeometry)XamlReader.Load(pathCross);

                    }
                    break;
                case "CROSS2":
                    

                    pathCross = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                    pathCross += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, 0);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.1, 0);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, _size * 0.4);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.9, 0);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size, 0);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size, _size * 0.1);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.6, _size * 0.5);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size, _size * 0.9);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size, _size);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.9, _size);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, _size * 0.6);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.1, _size);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, _size);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, _size * 0.9);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.4, _size * 0.5);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, _size * 0.1);
                    pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, 0);
                    pathCross += String.Format("</PathFigure.Segments></PathFigure>");
                    pathCross += String.Format("</PathGeometry.Figures></PathGeometry>");


                    _path.Data = (PathGeometry)XamlReader.Load(pathCross);
                    if (Shadow)
                    {

                        _shadow.Data = (PathGeometry)XamlReader.Load(pathCross);

                    }

                    break;

                case "TRIANGLE":
                    
                    
                    if (!String.IsNullOrEmpty(ImagePath) && _downloaded)
                    {
                        pathCross = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathCross += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, Height);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", Width * 0.5, 0);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", Width, Height);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, Height);
                        pathCross += String.Format("</PathFigure.Segments></PathFigure>");
                        pathCross += String.Format("</PathGeometry.Figures></PathGeometry>");
                        this.Clip = (PathGeometry)XamlReader.Load(pathCross);
                    }
                    else
                    {
                        pathCross = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathCross += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", 0, _size);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, 0);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size, _size);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, _size);
                        pathCross += String.Format("</PathFigure.Segments></PathFigure>");
                        pathCross += String.Format("</PathGeometry.Figures></PathGeometry>");
                        _path.Data = (PathGeometry)XamlReader.Load(pathCross);
                        if (Shadow)
                        {
                            _shadow.Data = (PathGeometry)XamlReader.Load(pathCross);
                        }
                    }

                    break;
                case "DIAMOND":
                    if (!String.IsNullOrEmpty(ImagePath) && _downloaded)
                    {
                        pathCross = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathCross += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", Width*0.5,0 );
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", Width, Height*0.5);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", Width*0.5, Height);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0, Height*0.5);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", Width * 0.5, 0);
                        pathCross += String.Format("</PathFigure.Segments></PathFigure>");
                        pathCross += String.Format("</PathGeometry.Figures></PathGeometry>");
                        this.Clip = (PathGeometry)XamlReader.Load(pathCross);
                    }
                    else
                    {
                        pathCross = @"<PathGeometry xmlns=""http://schemas.microsoft.com/client/2007""><PathGeometry.Figures>";
                        pathCross += String.Format(@"<PathFigure StartPoint=""{0},{1}""><PathFigure.Segments>", _size * 0.5, 0);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size*0.9, _size * 0.5);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, _size);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", 0.1*_size, _size * 0.5);
                        pathCross += String.Format(@"<LineSegment Point=""{0},{1}""/>", _size * 0.5, 0);
                        pathCross += String.Format("</PathFigure.Segments></PathFigure>");
                        pathCross += String.Format("</PathGeometry.Figures></PathGeometry>");
                        _path.Data = (PathGeometry)XamlReader.Load(pathCross);
                        if (Shadow)
                        {
                            _shadow.Data = (PathGeometry)XamlReader.Load(pathCross);
                        }
                    }
                    break;
            }
            
        }
        #endregion Internal Methods

        #region Private Methods
        private void SetDefaults()
        {

            _downloaded = false;
        }
        #endregion Private Methods

        #region Data


        private Double _imageScale;
        private Double _size;
        private String _style;
        private String _imagePath;

        private System.Windows.Threading.DispatcherTimer  _timer;

        private Path _path;
        private Path _shadow;
        private System.Windows.Controls.Image _image;

        
        WebClient _webClient = new WebClient();

        Boolean _downloaded;
        
        #endregion Data
    }
}
