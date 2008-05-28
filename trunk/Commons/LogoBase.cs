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

namespace Visifire.Commons
{
    public abstract class LogoBase:VisualObject
    {
    
        #region Public Methods

        public LogoBase()
        {
            
            
        }

        public override String TextParser(String unParsed)
        {
            if (String.IsNullOrEmpty(unParsed)) return "";
            String str = new String(unParsed.ToCharArray());
            if (str.Contains("##Source"))
                str = str.Replace("##Source", "#Source");
            else
                str = str.Replace("#Source", Source);

            return str;
        }

        public override void Render()
        {
            base.Render();
        }

        #endregion Public Methods

        #region Public Properties

        public Double Left
        {
            get
            {
                return _left;
            }
            set
            { _left = value; }
        }

        public Double Top
        {
            get { return _top; }
            set { _top = value; }
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

                _image.RenderTransform = st;
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
                _timer.Start();
                String URL = System.Windows.Browser.HtmlPage.Document.DocumentUri.OriginalString;
                String[] parts = URL.Split('/');
                String newpath = URL.Substring(0, URL.IndexOf(parts[parts.Length - 1])) + _source;
                

                System.Windows.Controls.Image img = new Image();
                img.SetValue(System.Windows.Controls.Image.SourceProperty, newpath);
                
                _image.Source = img.Source;

                
            }
        }

        public Boolean OnTop
        {
            get
            {
                if ((Int32)GetValue(ZIndexProperty) > 1) return true;
                return false;
            }
            set
            {
                if (value)
                    SetValue(ZIndexProperty, 50);
                else
                    SetValue(ZIndexProperty, 1);
            }
        }

        #endregion Public Properties

        #region Private Methods

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (_image.DownloadProgress == 1)
            {
                _timer.Stop();
                SetHeight();
                SetWidth();
                SetLeft();
                SetTop();
                Render();
                _image.Opacity = 1;
            }
        }

        protected override void SetDefaults()
        {
            base.SetDefaults();

            _image = new Image();
            _image.SetValue(ZIndexProperty, 1);

            _timer = new System.Windows.Threading.DispatcherTimer();
            _timer.Interval = new TimeSpan(0,0,1);
            _timer.Tick += new EventHandler(_timer_Tick);
            
            this.Children.Add(_image);

            Left = Double.NaN;
            Top = Double.NaN;
            Scale = 1.0;
            AlignmentX = AlignmentX.Center;
            AlignmentY = AlignmentY.Top;
            _image.Opacity = 0;
            _source = "";
        }

        #endregion Private Methods
        
        #region Data
        public Image _image;
        private String _source;
        private Double _scale;
        private Double _left;
        private Double _top;
        private AlignmentX _alignmentX;
        private AlignmentY _alignmentY;
        
                
        
        private System.Windows.Threading.DispatcherTimer _timer;

        
        #endregion Data
    }
}
