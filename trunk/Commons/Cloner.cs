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
using System.Windows.Media.Imaging;

namespace Visifire.Commons
{
    /// <summary>
    /// This Class contains methods to clone various objects.
    /// </summary>
    public class Cloner
    {
        #region Static Methods
        /// <summary>
        /// This method creates a clone of given Brush and returns it.
        /// </summary>
        /// <param name="brush"></param>
        /// <returns></returns>
        
        public static Brush CloneBrush(Brush brush)
        {
            
            Brush brushClone = brush;

            if (brush != null)
            {
                switch (brush.GetType().Name)
                {
                    case "LinearGradientBrush":
                        LinearGradientBrush lgb = (LinearGradientBrush)brush;
                        LinearGradientBrush lgb2 = new LinearGradientBrush();
                        
                        lgb2.StartPoint = lgb.StartPoint;
                        lgb2.EndPoint = lgb.EndPoint;
                        GradientStopCollection gsc = new GradientStopCollection();
                        foreach (GradientStop item in lgb.GradientStops)
                        {
                            GradientStop gs = new GradientStop();
                            gs.Color = item.Color;
                            gs.Offset = item.Offset;
                            gsc.Add(gs);
                        }
                        lgb2.GradientStops = gsc;
                        
                        lgb2.Transform = CloneTransform(lgb.Transform);
                        lgb2.RelativeTransform = CloneTransform(lgb.RelativeTransform);
                        brushClone = lgb2;
                        break;

                    case "RadialGradientBrush":
                        RadialGradientBrush rgb = (RadialGradientBrush)brush;
                        RadialGradientBrush rgb2 = new RadialGradientBrush();

                        rgb2.GradientOrigin = rgb.GradientOrigin;
                        rgb2.Center = rgb.Center;
                        rgb2.ColorInterpolationMode = rgb.ColorInterpolationMode;
                        rgb2.MappingMode = rgb.MappingMode;
                        rgb2.SpreadMethod = rgb.SpreadMethod;

                        GradientStopCollection gradientStops = new GradientStopCollection();
                        foreach (GradientStop item in rgb.GradientStops)
                        {
                            GradientStop gs = new GradientStop();
                            gs.Color = item.Color;
                            gs.Offset = item.Offset;
                            gradientStops.Add(gs);
                        }
                        rgb2.GradientStops = gradientStops;

                        rgb2.Transform = CloneTransform(rgb.Transform);
                        rgb2.RelativeTransform = CloneTransform(rgb.RelativeTransform);
                        brushClone = rgb2;
                        break;

                    case "SolidColorBrush":
                        SolidColorBrush scb = new SolidColorBrush();
                        scb.Color = ((SolidColorBrush)brush).Color;
                        scb.Transform = CloneTransform(((SolidColorBrush)brush).Transform);
                        scb.RelativeTransform = CloneTransform(((SolidColorBrush)brush).RelativeTransform);
                        brushClone = scb;
                        break;
                    case "ImageBrush":
                        ImageBrush ib = new ImageBrush();
                        ImageBrush ib2 = (ImageBrush)brush;
                        System.Windows.Controls.Image img = new Image();

                        
                        ib.ImageSource = new BitmapImage();
                        ib.ImageSource.SetValue(BitmapImage.UriSourceProperty, ib2.ImageSource.GetValue(BitmapImage.UriSourceProperty));

                        ib.AlignmentX = ib2.AlignmentX;
                        ib.AlignmentY = ib2.AlignmentY;
                        ib.Opacity = ib2.Opacity;
                        ib.Stretch = ib2.Stretch;
                        ib.Transform = CloneTransform(ib2.Transform); 
                        ib.Transform = CloneTransform(ib2.RelativeTransform);
                        brushClone=ib;
                        break;
                    case "VideoBrush":
                        VideoBrush vb1 = new VideoBrush();
                        VideoBrush vb2 = (VideoBrush)brush;
                        vb1.SourceName = vb2.SourceName;
                        vb1.AlignmentX = vb2.AlignmentX;
                        vb1.AlignmentY = vb2.AlignmentY;
                        vb1.Opacity = vb2.Opacity;
                        vb1.Stretch = vb2.Stretch;
                        vb1.Transform = CloneTransform(vb2.Transform);
                        vb1.RelativeTransform = CloneTransform(vb2.RelativeTransform);
                        brushClone=vb1;
                        break;
                }
            }
            

            return brushClone;
        }
        
        /// <summary>
        /// This method creates a clone of any transform object and returns it.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Transform CloneTransform(Transform source)
        {
            Transform transform;
            if(source == null) return null;
            switch(source.GetType().Name)
            {
                // This section creates a clone of Rotate transform
                case "RotateTransform":
                    RotateTransform rt = new RotateTransform();    // new Transform object
                    RotateTransform rt2 = (RotateTransform)source; // Source transfrom object is typecasted to Rotate transform object 
                    rt.Angle = rt2.Angle;
                    rt.CenterX = rt2.CenterX;
                    rt.CenterY = rt2.CenterY;
                    transform = rt;
                    break;
                case "ScaleTransform":
                    ScaleTransform st = new ScaleTransform();
                    ScaleTransform st2 = (ScaleTransform)source;
                    st.CenterX = st2.CenterX;
                    st.CenterY = st2.CenterY;
                    st.ScaleX = st2.ScaleX;
                    st.ScaleY = st2.ScaleY;
                    transform = st;
                    break;
                case "SkewTransform":
                    SkewTransform sk = new SkewTransform();
                    SkewTransform sk2 = (SkewTransform)source;
                    sk.AngleX = sk2.AngleX;
                    sk.AngleY = sk2.AngleY;
                    sk.CenterX = sk2.CenterX;
                    sk.CenterY = sk2.CenterY;
                    transform = sk;
                    break;
                case "TranslateTransform":
                    TranslateTransform tt = new TranslateTransform();
                    TranslateTransform tt2 = (TranslateTransform)source;
                    tt.X = tt2.X;
                    tt.Y = tt2.Y;
                    transform = tt;
                    break;
                case "MatrixTransform":
                   MatrixTransform mt = new MatrixTransform();
                   
                   MatrixTransform mt2 = (MatrixTransform)source;
                   Matrix m = new Matrix(mt2.Matrix.M11,mt2.Matrix.M12,mt2.Matrix.M21,mt2.Matrix.M22,mt2.Matrix.OffsetX,mt2.Matrix.OffsetY);
                   mt.Matrix = m;
                   transform = mt;
                   break;
                case "TransformGroup":
                    TransformGroup tg = new TransformGroup();
                    TransformGroup tg2 = (TransformGroup)source;
                    Transform tr;
                    foreach(Transform tr2 in tg2.Children)
                    {
                        tr=CloneTransform(tr2);
                        tg.Children.Add(tr);
                    }
                    transform = tg;
                    break;
                default:
                    transform = null;
                    break;
            }
            return transform;

        }

        #endregion Static Methods
    }
}
