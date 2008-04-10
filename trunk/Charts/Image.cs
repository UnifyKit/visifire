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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Net;
using Visifire.Commons;
namespace Visifire.Charts
{
    public class Image:Canvas
    {
        #region Public Methods
        public Image()
        {
            SetDefaults();

        }
        
        
        public void Render()
        {
            _parent = (Parent as Chart);
            if(_parent.ToolTip.Enabled==true && !String.IsNullOrEmpty(ToolTipText)) AttachToolTip();
            if (!String.IsNullOrEmpty(Href)) AttachHref();
            
            SetWidth();
            SetHeight();
            SetLeft();
            SetTop();
        }
        
        public void AttachHref()
        {
            if (!String.IsNullOrEmpty(Href))
            {
                String link = TextParser(Href);
                FrameworkElement parent = this.Parent as FrameworkElement;

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
                    System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(link));
                };
            }
        }

        public String TextParser(String unParsed)
        {
            if (String.IsNullOrEmpty(unParsed))
                return "";
            String str = new String(unParsed.ToCharArray());
            if (str.Contains("##Source"))
                str = str.Replace("##Source", "#Source");
            else
                str = str.Replace("#Source", Source);

            return str;
        }
        
        private void SetName()
        {
            if (this.Parent.GetType().Name == "Chart")
                _parent = this.Parent as Chart;
            else
                throw new Exception(this + "Parent should be a Chart");
        }

        internal void Init()
        {

            if ((_imageHeightSet && !_imageWidthSet) || (!_imageHeightSet && _imageWidthSet))
            {
                throw new Exception("ImageWidth and ImageHeight; both must be set.");
            }
            else if (_imageWidthSet && _imageHeightSet)
            {
                imgBrush.Stretch = Stretch.Fill;
            }
            else
            {
                imgBrush.Stretch = Stretch.Uniform;
            }
        }

        private void SetObjectSize(Object o,RoutedEventArgs e)
        {
            SetWidth();
            SetHeight();
            SetLeft();
            SetTop();

        }

        public void SetLeft()
        {
            Double logoLeft = ImageWidth;

            if (!Double.IsNaN(ImageWidth))
            {
                switch (AlignmentX)
                {
                    case AlignmentX.Left:
                        logoLeft = _parent.Padding;
                        break;

                    case AlignmentX.Center:
                        logoLeft = (_parent.Width - this.Width) / 2;
                        break;

                    case AlignmentX.Right:
                        logoLeft = (_parent.Width - this.Width - _parent.Padding);
                        break;
                }
            }

            SetValue(LeftProperty, logoLeft);
        }

        public void SetTop()
        {
            Double logoTop = ImageHeight;
            if (!Double.IsNaN(ImageHeight))
            {
                switch (AlignmentY)
                {
                    case AlignmentY.Top:
                        logoTop = _parent.Padding;
                        break;

                    case AlignmentY.Center:
                        logoTop = (_parent.Height - this.Height) / 2;
                        break;

                    case AlignmentY.Bottom:
                        logoTop = (_parent.Height - this.Height - _parent.Padding);
                        break;
                }
            }

            SetValue(TopProperty, logoTop);
        }

        public void SetHeight()
        {

            if (ImageHeight <= 0 || Double.IsNaN(ImageHeight))
                this.Height = _parent.Height * 0.2 * Scale;
            else
                this.Height = ImageHeight * Scale;
        }

        public void SetWidth()
        {

            if (ImageWidth <= 0 || Double.IsNaN(ImageWidth))
                this.Width = _parent.Width * 0.2 * Scale;
            else
                this.Width = ImageWidth * Scale;

        }
        #endregion Public Methods
        
        #region Public Properties

        public bool Enabled
        {
            get;
            set;
        }
        public String ToolTipText
        {
            get;
            set;
        }
        public  String Href
        {
            get
            {
                return _href;
            }
            set
            {
                _href = value;
            }
        }
        public Double ImageWidth
        {
            get
            {
                return _imageWidth;
            }
            set
            { 
                _imageWidth = value;
                _imageWidthSet = true;
            }
        }

        public Double ImageHeight
        {
            get 
            { 
                return _imageHeight; 
            }
            set 
            { 
                _imageHeight = value;
                _imageHeightSet = true;
            }
        }

        public AlignmentX AlignmentX
        {
            get { return _alignmentX; }
            set { _alignmentX = value; }
        }

        public AlignmentY AlignmentY
        {
            get { return _alignmentY; }
            set { _alignmentY = value; }
        }

        public Double Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                ScaleTransform st = new ScaleTransform();
                _scale = value;
                st.ScaleX = value;
                st.ScaleY = value;

                this.RenderTransform = st;
            }
        }

        public String Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                Uri ur = new Uri(_source,UriKind.RelativeOrAbsolute);
                if (ur.IsAbsoluteUri)
                {
                    _source = ur.AbsoluteUri;
                }
                else
                {
                    UriBuilder ub = new UriBuilder(Application.Current.Host.Source);
                    String sourcePath = ub.Path.Substring(0, ub.Path.LastIndexOf('/') + 1);
                    UriBuilder ub2 = new UriBuilder(ub.Scheme, ub.Host, ub.Port, sourcePath + value);
                    _source = ub2.ToString();
                }
                
                String XAMLimage = "<ImageBrush xmlns=\"http://schemas.microsoft.com/client/2007\" ImageSource=\"" + _source + "\"/>";
                imgBrush = (ImageBrush)XamlReader.Load(XAMLimage);
                
                imgBrush.ImageFailed += delegate(Object o, ExceptionRoutedEventArgs e)
                {
                    throw new Exception("Image Failed in Logo");
                };
                this.Background = imgBrush;
            }
        }

        public Boolean ShowOnTop
        {
            set
            {
                if (value)
                    this.SetValue(ZIndexProperty, 1000);
                else
                    this.SetValue(ZIndexProperty, 1);
            }
        }
        #endregion Public Properties

        #region Private Methods

        private void SetDefaults()
        {
            Scale = 1.0;
            AlignmentX = AlignmentX.Center;
            AlignmentY = AlignmentY.Top;
            Enabled = true;
        }

        private void AttachToolTip()
        {
            String str;

            str = TextParser(ToolTipText);

            this.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                _parent.ToolTip.Text = str;
                _parent.ToolTip.Visibility = Visibility.Visible;
                _parent.ToolTip.SetTop(e.GetPosition(_parent.ToolTip.Parent as UIElement).Y - (Double)_parent.ToolTip.GetValue(HeightProperty) * 1.5);
                _parent.ToolTip.SetLeft(e.GetPosition(_parent.ToolTip.Parent as UIElement).X - (Double)_parent.ToolTip.GetValue(WidthProperty) / 2);

            };

            this.MouseLeave += delegate(object sender, MouseEventArgs e)
            {
                _parent.ToolTip.Visibility = Visibility.Collapsed;

            };
        }

        #endregion Private Methods

        #region Data


        private Chart _parent;
        private String _source;
        private Double _scale;
        private AlignmentX _alignmentX;
        private AlignmentY _alignmentY;
        private Double _imageHeight;
        private Double _imageWidth;
        private Boolean _imageHeightSet = false;
        private Boolean _imageWidthSet = false;
        private Canvas _image;
        private String _href;
        private ImageBrush imgBrush;
        #endregion Data
    }
}
