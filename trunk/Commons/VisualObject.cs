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
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Windows.Markup;
using System.Globalization;
using System.Linq;

namespace Visifire.Commons
{
    public abstract class VisualObject:Canvas
    {
        #region Public Methods
            
        /// <summary>
        /// Constructor for visual object class. Internall calls SetDefaults()
        /// </summary>
        public VisualObject()
        {
            SetDefaults();
        }

        public Brush GetCurrentBackground()
        {
            return base.Background;
        }
        
        /// <summary>
        /// This function applies the border to the canvas depending on the border properties
        /// Also applies clipping region
        /// </summary>
        public virtual void ApplyBorder()
        {
            if (_borderRectangle == null)
            {
                _borderRectangle = new Rectangle();
                this.Children.Add(_borderRectangle);
            }
            
            _borderRectangle.Width = (Double)this.GetValue(WidthProperty);
            _borderRectangle.Height = Math.Abs((Double)this.GetValue(HeightProperty));

            _borderRectangle.Stroke = Cloner.CloneBrush(BorderColor);
            _borderRectangle.StrokeThickness = BorderThickness;
            _borderRectangle.RadiusX = RadiusX;
            _borderRectangle.RadiusY = RadiusY;
            _borderRectangle.StrokeDashArray = Parser.GetStrokeDashArray(this._borderStyle);
            
            if (ApplyClipRegion)
            {
                RectangleGeometry rg = new RectangleGeometry();
                rg.Rect = new Rect(0, 0, _borderRectangle.Width, _borderRectangle.Height);
                rg.RadiusX = RadiusX + BorderThickness / 2;
                rg.RadiusY = RadiusY + BorderThickness / 2;
                this.Clip = rg;
            }

            
            
        }
        
        /// <summary>
        /// This function applies href to a canvas
        /// </summary>
        public void AttachHref()
        {
            if (!String.IsNullOrEmpty(Href))
            {
                String link = TextParser(Href);
                FrameworkElement parent=this.Parent as FrameworkElement;
                
                this.MouseEnter += delegate(object sender, MouseEventArgs e)
                {
                    parent.Cursor = Cursors.Hand;
                };
                this.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    parent.Cursor = Cursors.Arrow;
                };

                this.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    HtmlPage.Window.Navigate(new Uri(link));
                };
            }
        }

        public void AttachHref(FrameworkElement element)
        {
            if (element == null) return;
            if (!String.IsNullOrEmpty(Href))
            {
                String link = TextParser(Href);
                FrameworkElement parent = element.Parent as FrameworkElement;
                element.MouseEnter += delegate(object sender, MouseEventArgs e)
                {

                    parent.Cursor = Cursors.Hand;
                };
                element.MouseLeave += delegate(object sender, MouseEventArgs e)
                {
                    //parent.Cursor = Cursors.Arrow;
                };

                element.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
                {
                    HtmlPage.Window.Navigate(new Uri(link));
                };
            }
        }

        /// <summary>
        /// This function attaches the Tool tip
        /// </summary>
        public void AttachToolTip()
        {
            FrameworkElement obj = this;
            while (!(obj is Container))
                obj = (FrameworkElement)obj.Parent;

            Container container = obj as Container;

            String str;
            if (String.IsNullOrEmpty(ToolTipText)) return;        
            str = TextParser(ToolTipText);

            this.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                container.ToolTip.Text = str;
                container.ToolTip.Visibility = Visibility.Visible;
                container.ToolTip.SetTop(e.GetPosition(container).Y - (Double)container.ToolTip.GetValue(HeightProperty) * 1.5);
                container.ToolTip.SetLeft(e.GetPosition(container).X - (Double)container.ToolTip.GetValue(WidthProperty) / 2);

            };

            this.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                container.ToolTip.Visibility = Visibility.Collapsed;

            };
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public virtual void AttachToolTip(FrameworkElement element)
        {
            if (element == null) return;
            FrameworkElement obj = this;
            while (!(obj is Container))
                obj = (FrameworkElement)obj.Parent;

            Container container = obj as Container;

            String str;
            if (String.IsNullOrEmpty(ToolTipText)) return;
            str = TextParser(ToolTipText);

            element.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                container.ToolTip.Text = str;
                container.ToolTip.Visibility = Visibility.Visible;
                container.ToolTip.SetTop(e.GetPosition(container).Y - (Double)container.ToolTip.GetValue(HeightProperty) * 1.5);
                container.ToolTip.SetLeft(e.GetPosition(container).X - (Double)container.ToolTip.GetValue(WidthProperty) / 2);
            };

            element.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                container.ToolTip.Visibility = Visibility.Collapsed;

            };
        }

        public virtual String TextParser(String str)
        {
            return str;
        }

        public virtual void Init()
        {

        }

        /// <summary>
        /// Rendering function internally calls AplyBorder
        /// </summary>
        public virtual void Render()
        {
            ApplyBorder();
        }

        public virtual void ApplyLighting()
        {
            if (Background.GetType().Name == "SolidColorBrush")
            {
                String linearGrad = "0;";
                linearGrad += Parser.GetDarkerColor((Background as SolidColorBrush).Color, 0.90);//Lighter 0.65
                linearGrad += ",0;";
                linearGrad += Parser.GetLighterColor((Background as SolidColorBrush).Color, 0.25);//Lighter 0.55
                linearGrad += ",1";
                Background = Parser.ParseLinearGradient(linearGrad);
            }
        }

        public virtual void ApplyShadow()
        {
            Rectangle rect = new Rectangle();
            rect.SetValue(LeftProperty, (Double)this.GetValue(LeftProperty) + 4);
            rect.SetValue(TopProperty, (Double)this.GetValue(TopProperty) + 4);
            rect.Width = this.Width;
            rect.Height = this.Height;
            rect.RadiusX = RadiusX;
            rect.RadiusY = RadiusY;
            rect.Opacity = Opacity * 0.8;
            rect.Fill = Parser.ParseLinearGradient("-90;#FF000000,0;#7f000000,1");
            rect.OpacityMask = Parser.ParseLinearGradient("0;#FF000000,0;#7f000000,1");
            rect.SetValue(ZIndexProperty, (Int32)this.GetValue(ZIndexProperty) - 1);
            (this.Parent as Canvas).Children.Add(rect);
        }

        public void SetName()
        {
            Generic.SetNameAndTag(this);
        }

        /// <summary>
        /// Finds list of objects of specified type.
        /// </summary>
        public static void FindByType(ref List<UIElement> tResult, Panel parent, Type objType)
        {
            if (parent != null)
            {
                var objs = from child in parent.Children where objType.Equals(child.GetType()) select child;

                foreach (UIElement uiElement in objs)
                {
                    tResult.Add(uiElement);
                    FindByType(ref tResult, uiElement as Panel, objType);
                }

                var panels = from child in parent.Children where (!objType.Equals(child.GetType()) && ((child as Panel) != null)) select child;

                foreach (Panel f in panels)
                    FindByType(ref tResult, f, objType);
            }
        }

        public virtual System.Collections.Generic.List<Point> GetBoundingPoints()
        {
            System.Collections.Generic.List<Point> points = new System.Collections.Generic.List<Point>();

            return points;
        }

        public void CreateAndAddBevelPath(System.Collections.Generic.List<Point> points, String type, Double Angle, Int32 Zindex)
        {
            Path path = new Path();

            path.IsHitTestVisible = false;

            List<PathGeometryParams> pathGeometryList = new List<PathGeometryParams>();

            for (Int32 i = 1; i <= points.Count; i++)
                pathGeometryList.Add(new LineSegmentParams(new Point(points[i % points.Count].X, points[i % points.Count].Y)));

            path.Data = Parser.GetPathGeometryFromList(FillRule.Nonzero, new Point(points[0].X, points[0].Y), pathGeometryList);

            String fillString = Angle.ToString(CultureInfo.InvariantCulture) + ";";
            Brush brush = Background;
            if (brush == null)
                brush = new SolidColorBrush(Colors.Transparent);

            if (brush.GetType().Name == "SolidColorBrush")
            {
                System.Windows.Media.Color C = (brush as SolidColorBrush).Color;
                Double intensity = (Double)(C.R + C.G + C.B) / (3 * 255);
                Double r, g, b;
                switch (type)
                {
                    case "Bright":
                        fillString += Parser.GetLighterColor((brush as SolidColorBrush).Color, 0.99) + ",0;";
                        r = ((Double)(brush as SolidColorBrush).Color.R / 255) * 0.9999;
                        g = ((Double)(brush as SolidColorBrush).Color.G / 255) * 0.9999;
                        b = ((Double)(brush as SolidColorBrush).Color.B / 255) * 0.9999;
                        fillString += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b) + ",0.2;";
                        fillString += Parser.GetLighterColor((brush as SolidColorBrush).Color, 1 - r, 1 - g, 1 - b) + ",0.6;";
                        fillString += Parser.GetLighterColor((brush as SolidColorBrush).Color, 0.99) + ",1";

                        break;
                    case "Medium":

                        fillString += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 0.75) + ",0;";
                        fillString += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 0.97) + ",1";

                        break;
                    case "Dark":
                        fillString += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 0.35) + ",0;";
                        fillString += Parser.GetDarkerColor((brush as SolidColorBrush).Color, 0.65) + ",1";
                        break;
                }
                path.Fill = Parser.ParseLinearGradient(fillString);
            }
            else if (brush.GetType().Name == "LinearGradientBrush" || brush.GetType().Name == "RadialGradientBrush")
            {
                if (brush.GetType().Name == "LinearGradientBrush")
                    path.Fill = new LinearGradientBrush();
                else
                    path.Fill = new RadialGradientBrush();
                switch (type)
                {

                    case "Bright":
                        Parser.GenerateLighterGradientBrush(brush as GradientBrush, path.Fill as GradientBrush, 0.6);
                        break;
                    case "Medium":
                        Parser.GenerateDarkerGradientBrush(brush as GradientBrush, path.Fill as GradientBrush, 0.745);
                        break;
                    case "Dark":
                        Parser.GenerateDarkerGradientBrush(brush as GradientBrush, path.Fill as GradientBrush, 0.55);
                        break;
                }
                RotateTransform rt = new RotateTransform();
                rt.Angle = Angle;

                path.Fill.RelativeTransform = rt;
            }

            path.Opacity = 1;

            path.SetValue(ZIndexProperty, Zindex + 1);
            this.Children.Add(path);
        }

        #endregion Public Methods

        #region Public Properties
        
        #region Background Properties
        public Boolean Bevel
        {
            get
            {
                if (String.IsNullOrEmpty(_bevel) || _bevel == "Undefined")
                {
                    if (GetFromTheme("Bevel") == null)
                        return false;
                    else
                        return Convert.ToBoolean(GetFromTheme("Bevel"));
                }
                else
                    return Boolean.Parse(_bevel);
            }
            set
            {
                _bevel = value.ToString();
            }
        }

        public virtual Boolean LightingEnabled
        {
            get
            {
                if (String.IsNullOrEmpty(_lightingEnabled) || _lightingEnabled == "Undefined")
                {
                    if (GetFromTheme("LightingEnabled") != null)
                        return Convert.ToBoolean(GetFromTheme("LightingEnabled"));
                    else
                        return false;
                }
                else
                    return Boolean.Parse(_lightingEnabled);
            }
            set
            {
                _lightingEnabled = value.ToString();
            }
        }

        public virtual Boolean ShadowEnabled
        {
            get
            {
                if (String.IsNullOrEmpty(_shadowEnabled) || _shadowEnabled == "Undefined")
                {
                    if (GetFromTheme("ShadowEnabled") != null)
                        return Convert.ToBoolean(GetFromTheme("ShadowEnabled"));
                    else
                        return false;
                }
                else
                    return Boolean.Parse(_shadowEnabled);
            }
            set
            {
                _shadowEnabled = value.ToString();
            }
        }

        public virtual Brush Background
        {
            get
            {
                if(base.Background != null)
                    return base.Background;
                else
                    return GetFromTheme("Background") as Brush;
            }
            set
            {
                base.Background = value;
            }
        }

        public virtual Int32 ZIndex
        {
            set
            {
                this.SetValue(ZIndexProperty, value);
            }
            get
            {
                return (Int32)this.GetValue(ZIndexProperty);
            }
        }

        public virtual String Color
        {
            get
            {
                if (String.IsNullOrEmpty(_color))
                {
                    //replace this with the default background from sytle
                    return "Transparent";
                }
                else
                {
                    return _color;
                }
            }
            set
            {
                base.Background = Parser.ParseColor(value);
                _color = value;
            }
        }

        public virtual String Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = Parser.BuildAbsolutePath(value);
                ImageBrush imgBrush;
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" ImageSource=\"" + _image + "\"/>";

                imgBrush = (ImageBrush)XamlReader.Load(XAMLimage);
                imgBrush.ImageFailed += new ExceptionRoutedEventHandler(imgBrush_ImageFailed);

                imgBrush.Stretch = ImageStretch;
                base.Background = imgBrush;
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
                if (base.Background != null)
                {
                    if (base.Background.GetType().Name == "ImageBrush")
                    {
                        (base.Background as ImageBrush).Stretch = value;
                    }
                }
            }
        }

        #endregion Background Properties

        #region Border Properties

        public virtual Double BorderThickness
        {
            get
            {
                if(!Double.IsNaN(_borderThickness))
                    return _borderThickness;
                else
                    return Convert.ToDouble(GetFromTheme("BorderThickness"));
            }
            set
            {
                _borderThickness = value;
            }
        }

        public virtual Brush BorderColor
        {
            get
            {
                if (_borderColor != null)
                    return _borderColor;
                else if (GetFromTheme("BorderColor") != null)
                    return GetFromTheme("BorderColor") as Brush;
                else 
                    return null;
            }
            set
            {
                _borderColor = value;
            }
        }

        public virtual String BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                _borderStyle = value;
            }
        }

        public virtual Double RadiusX
        {
            get
            {
                if (!Double.IsNaN(_radiusX))
                    return _radiusX;
                else
                {
                    return Convert.ToDouble(GetFromTheme("RadiusX"));
                }
            }
            set
            {
                _radiusX = value;
            }
        }

        public virtual Double RadiusY
        {
            get
            {
                if (!Double.IsNaN(_radiusY))
                    return _radiusY;
                else
                {
                    return Convert.ToDouble(GetFromTheme("RadiusY"));
                }
            }
            set
            {
                _radiusY = value;
            }
        }

        #endregion Border Properties
        
        public virtual String Href
        {
            get
            {
                return _href;
            }
            set
            {
                _href = Parser.BuildAbsolutePath(value);
            }
        }
        
        public virtual String ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set
            {
                _toolTipText = value;
                
            }
        }
        
        public virtual Boolean Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }
        
        public Container Root
        {
            get
            {
                FrameworkElement parent;

                if (_root == null)
                {
                    parent = this;

                    while (!(parent is Container))
                        parent = (FrameworkElement)parent.Parent;

                    _root = (Container)parent;
                }
                return _root;
            }
        }
                    
        #endregion Public Properties

        #region Protected Methods

        protected Boolean ApplyClipRegion
        {
            get;
            set;
        }

        protected virtual void SetDefaults()
        {
            
            _borderThickness = Double.NaN;
            _borderStyle = "Solid";
            _borderColor = null;
            _radiusX = Double.NaN;
            _radiusY = Double.NaN; ;
            _href = null;
            _toolTipText = null;
            ApplyClipRegion = true;
            _shadowEnabled = "Undefined";
            _lightingEnabled = "Undefined";
            
        }

        protected Object GetFromTheme(String propertyName)
        {
            Object obj = null;



            if (Root.AppliedTheme.ContainsKey(this.GetType().Name))
            {
                System.Collections.Generic.Dictionary<String, Object> properties = Root.AppliedTheme[this.GetType().Name];

                if (properties.ContainsKey(propertyName))
                    obj = (properties[propertyName]);
            }

            return obj;
        }

        /// <summary>
        /// While image loading is failed
        /// </summary>
        protected void imgBrush_ImageFailed(Object o, ExceptionRoutedEventArgs e)
        {
            throw new Exception("Description:" + Image.ToString() + ", Download failed");
        }

        /// <summary>
        /// Generates points for bevel
        /// </summary>
        protected void ApplyBevel(String[] type, Double[] length, Double[] Angle)
        {
            System.Collections.Generic.List<Point> points = GetBoundingPoints();
            Point centroid = GeometricMath.Centroid(points);
            System.Collections.Generic.List<Point> bevelArea = new System.Collections.Generic.List<Point>();
            Int32 i, j;
            for (i = 0; i < points.Count; i++)
            {
                j = (i + 1) % points.Count;
                if (length[i] == 0) continue;
                bevelArea.Add(points[i]);
                bevelArea.Add(points[j]);
                bevelArea.Add(GeometricMath.PointFromLength(length[i], centroid, points[j]));
                bevelArea.Add(GeometricMath.PointFromLength(length[i], centroid, points[i]));
                CreateAndAddBevelPath(bevelArea, type[i], Angle[i], (Int32)this.GetValue(ZIndexProperty) + 1);
                bevelArea.Clear();
            }

        }

        /// <summary>
        /// This function creates bevel effect same as the previous one but this function accepts an extra zindex parameter
        /// </summary>
        protected void ApplyBevel(String[] type, Double[] length, Double[] Angle, Int32 Zindex)
        {
            System.Collections.Generic.List<Point> points = GetBoundingPoints();
            Point centroid = GeometricMath.Centroid(points);
            System.Collections.Generic.List<Point> bevelArea = new System.Collections.Generic.List<Point>();

            if (Double.IsNaN(centroid.X) || Double.IsNaN(centroid.Y)) return;

            for (Int32 i = 0,j; i < points.Count; i++)
            {
                j = (i + 1) % points.Count;
                if (length[i] == 0) continue;
                bevelArea.Add(points[i]);
                bevelArea.Add(points[j]);
                bevelArea.Add(GeometricMath.PointFromLength(length[i], centroid, points[j]));
                bevelArea.Add(GeometricMath.PointFromLength(length[i], centroid, points[i]));
                CreateAndAddBevelPath(bevelArea, type[i], Angle[i], Zindex);
                bevelArea.Clear();
            }

        }   

        #endregion Protected Methods

        #region Public Abstract Functions

        public abstract void SetWidth();

        public abstract void SetHeight();

        public abstract void SetLeft();

        public abstract void SetTop();
                    
        #endregion Public Abstract Functions

        #region Data
        //private Brush _background;
        
        private String _color;

        private Double _borderThickness;
        private Brush _borderColor;
        private String _borderStyle;
        
        private Double _radiusX;
        private Double _radiusY;
        
        private String _href;
        
        private String _toolTipText;

        private Rectangle _borderRectangle;

        private Boolean _enabled = true;
                
        private Container _root = null;
        private String _bevel;
        private String _shadowEnabled;
        private String _lightingEnabled;
        private String _image;
        private Stretch _imageStretch = Stretch.Fill;

        #endregion Data

    }
    
}
