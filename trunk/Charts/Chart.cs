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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Visifire.Commons;
using System.Windows.Markup;
using System.Globalization;

namespace Visifire.Charts
{

    public class Chart : Container
    {
        #region Public Methods

        public Chart()
        {

            this.Loaded += new RoutedEventHandler(OnLoaded);
                        
        }

        public void OnLoaded(Object sender, EventArgs e)
        {
            Init();

            Render();

            SetTags();

            // Svae hit test for the chart
            _chartHitTestState = this.IsHitTestVisible;

            if (AnimationEnabled)
            {
                GenerateAnimationData();

                this.IsHitTestVisible = false;

                foreach (Storyboard sb in animation)
                {
                    sb.Begin();

                }
            }
            else
            {
                ApplyPostAnimationSettings();
            }
            
        }

        public override System.Collections.Generic.List<Point> GetBoundingPoints()
        {
            System.Collections.Generic.List<Point> points = new System.Collections.Generic.List<Point>();
            points.Add(new Point(0, 0));
            points.Add(new Point(this.Width, 0));
            points.Add(new Point(this.Width, this.Height));
            points.Add(new Point(0, this.Height));
            return points;
        }

        public override void Init()
        {

            InitialCommonInitSteps();
             
            if (PlotDetails.AxisOrientation == AxisOrientation.Column )
            {
                if (_innerBounds.X < AxisX.MajorTicks.TickLength && !AxisYPrimary.Enabled)
                    _innerBounds.X = AxisX.MajorTicks.TickLength;

                PlotArea.SetTop();
                AxisYPrimary.SetTop();
                if (AxisYSecondary != null) AxisYSecondary.SetTop();

                AxisX.AxisLabels.SetHeight();
                AxisX.AxisLabels.SetLeft();
                AxisX.SetHeight();
                AxisX.SetTop();

                AxisYPrimary.AxisLabels.CreateLabels();
                AxisYPrimary.AxisLabels.SetWidth();
                AxisYPrimary.SetWidth();
                AxisYPrimary.AxisLabels.SetLeft();
                AxisYPrimary.SetLeft();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.AxisLabels.CreateLabels();
                    AxisYSecondary.AxisLabels.SetWidth();
                    AxisYSecondary.SetWidth();
                    AxisYSecondary.AxisLabels.SetLeft();
                    AxisYSecondary.SetLeft();
                }

                PlotArea.SetLeft();
                PlotArea.SetHeight();
                PlotArea.SetWidth();

                AxisYPrimary.SetHeight();
                AxisYPrimary.AxisLabels.SetHeight();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetHeight();
                    AxisYSecondary.AxisLabels.SetHeight();
                }

                AxisX.SetLeft();
                AxisX.SetWidth();

                AxisX.SetAxisLimits();

                //Create axis labels must be done after Axis limit are set
                AxisX.AxisLabels.CreateLabels();
                AxisX.AxisLabels.SetWidth();
                AxisX.AxisLabels.PositionLabels();
                AxisX.AxisLabels.SetHeight();
                AxisX.SetHeight();
                AxisYPrimary.SetLeft();
                AxisYPrimary.SetWidth();
                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetLeft();
                    AxisYSecondary.SetWidth();
                }

                PlotArea.SetLeft();
                PlotArea.SetWidth();

                AxisYPrimary.AxisLabels.PositionLabels();
                if (AxisYSecondary != null)  AxisYSecondary.AxisLabels.PositionLabels();
                AxisX.SetTop();
                PlotArea.SetTop();
                PlotArea.SetHeight();
                AxisYPrimary.SetTop();
                AxisYPrimary.SetHeight();
                AxisYPrimary.AxisLabels.SetHeight();
                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetTop();
                    AxisYSecondary.SetHeight();
                    AxisYSecondary.AxisLabels.SetHeight();
                }
                AxisX.SetLeft();
                AxisX.SetWidth();
                AxisX.AxisLabels.SetWidth();


                AxisX.AxisLabels.PositionLabels();
                AxisX.AxisLabels.SetHeight();
                AxisX.SetHeight();
                AxisX.SetTop();
                PlotArea.SetHeight();
                AxisYPrimary.SetHeight();
                AxisYPrimary.AxisLabels.PositionLabels();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetHeight();
                    AxisYSecondary.AxisLabels.PositionLabels();
                }

                AxisX.MajorGrids.SetWidth();
                AxisYPrimary.MajorGrids.SetHeight();

                AxisX.MajorGrids.SetLeft();
                AxisYPrimary.MajorGrids.SetLeft();
                if (AxisYSecondary != null) AxisYSecondary.MajorGrids.SetLeft();

                AxisX.MajorGrids.DrawGrids();
                AxisYPrimary.MajorGrids.DrawGrids();
                if (AxisYSecondary != null)  AxisYSecondary.MajorGrids.DrawGrids();

                AxisX.MajorTicks.SetLeft();
                AxisX.MajorTicks.SetWidth();
                AxisX.MajorTicks.SetHeight();
                AxisX.MajorTicks.SetTop();
                AxisX.MajorTicks.DrawTicks();
                AxisX.DrawAxisLine();
                AxisX.AxisLabels.SetTop();

                AxisYPrimary.MajorTicks.SetLeft();
                AxisYPrimary.MajorTicks.SetHeight();
                AxisYPrimary.MajorTicks.SetWidth();
                AxisYPrimary.MajorTicks.SetTop();
                AxisYPrimary.MajorTicks.DrawTicks();
                AxisYPrimary.DrawAxisLine();
                AxisYPrimary.AxisLabels.SetLeft();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.MajorTicks.SetLeft();
                    AxisYSecondary.MajorTicks.SetHeight();
                    AxisYSecondary.MajorTicks.SetWidth();
                    AxisYSecondary.MajorTicks.SetTop();
                    AxisYSecondary.MajorTicks.DrawTicks();
                    AxisYSecondary.DrawAxisLine();
                    AxisYSecondary.AxisLabels.SetLeft();
                }

                // Positions titles that have to be placed outside PlotArea
                _titles.ForEach(delegate(Title child) { child.PlaceInsidePlotArea(); });


                // Positions legends that have to be placed outside PlotArea
                _legends.ForEach(delegate(Legend child) 
                { 
                    child.PlaceInsidePlotArea(new Rect((Double)PlotArea.GetValue(LeftProperty), (Double)PlotArea.GetValue(TopProperty), PlotArea.Width, PlotArea.Height), Padding); 
                });

                //Removes unused legends
                RemoveUnMarkedLegends();

                AxisX.PlaceTitle();
                AxisYPrimary.PlaceTitle();
                if (AxisYSecondary != null)  AxisYSecondary.PlaceTitle();

                PlotArea.ApplyBorder();

                // Position trend lines
                _trendLines.ForEach(delegate(TrendLine trendLine) { trendLine.InitAndDraw(); });
                
            }

            else if (PlotDetails.AxisOrientation == AxisOrientation.Bar)
            {
                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetTop();
                    AxisYSecondary.AxisLabels.CreateLabels();
                    AxisYSecondary.AxisLabels.SetHeight();
                    AxisYSecondary.AxisLabels.SetTop();
                    AxisYSecondary.SetHeight();
                    AxisYSecondary.SetTop();
                }

                PlotArea.SetTop();
                AxisX.SetTop();

                AxisYPrimary.AxisLabels.CreateLabels();
                AxisYPrimary.AxisLabels.SetHeight();
                AxisYPrimary.AxisLabels.SetTop();
                AxisYPrimary.SetHeight();
                AxisYPrimary.SetTop();

                AxisX.AxisLabels.SetWidth();
                AxisX.SetWidth();
                AxisX.AxisLabels.SetLeft();
                AxisX.SetLeft();

                PlotArea.SetLeft();
                PlotArea.SetHeight();
                PlotArea.SetWidth();

                AxisX.SetHeight();
                AxisX.AxisLabels.SetHeight();
                AxisYPrimary.SetLeft();
                AxisYPrimary.SetWidth();
                
                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetLeft();
                    AxisYSecondary.SetWidth();
                }

                AxisX.SetAxisLimits();

                //Create labels must appear after axis min and max are set
                AxisX.AxisLabels.CreateLabels();
                AxisX.AxisLabels.SetWidth();
                AxisX.SetWidth();

                AxisYPrimary.AxisLabels.SetWidth();
                AxisYPrimary.AxisLabels.PositionLabels();
                AxisYPrimary.AxisLabels.SetHeight();
                AxisYPrimary.SetHeight();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.AxisLabels.SetWidth();
                    AxisYSecondary.AxisLabels.PositionLabels();
                    AxisYSecondary.AxisLabels.SetHeight();
                    AxisYSecondary.SetHeight();
                    AxisYSecondary.SetTop();
                }

                AxisX.SetLeft();

                PlotArea.SetLeft();
                PlotArea.SetWidth();

                AxisX.AxisLabels.PositionLabels();
                AxisYPrimary.SetTop();
                PlotArea.SetTop();
                PlotArea.SetHeight();
                AxisX.SetTop();
                AxisX.SetHeight();
                AxisX.AxisLabels.SetHeight();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.SetLeft();
                    AxisYSecondary.SetWidth();
                    AxisYSecondary.AxisLabels.SetWidth();

                    //test
                    AxisYSecondary.AxisLabels.PositionLabels();
                    AxisYSecondary.AxisLabels.SetHeight();
                    AxisYSecondary.SetHeight();
                    AxisYSecondary.SetTop();
                }

                AxisYPrimary.SetLeft();
                AxisYPrimary.SetWidth();
                AxisYPrimary.AxisLabels.SetWidth();

                //test
                AxisYPrimary.AxisLabels.PositionLabels();
                AxisYPrimary.AxisLabels.SetHeight();
                AxisYPrimary.SetHeight();
                AxisYPrimary.SetTop();
                PlotArea.SetTop();
                PlotArea.SetHeight();
                AxisX.SetHeight();
                AxisX.AxisLabels.PositionLabels();
                AxisX.AxisLabels.SetTop();

                

                AxisX.MajorGrids.SetHeight();
                AxisX.MajorGrids.SetLeft();
                AxisX.MajorGrids.DrawGrids();

                AxisX.MajorTicks.SetLeft();
                AxisX.MajorTicks.SetHeight();
                AxisX.MajorTicks.SetWidth();
                AxisX.MajorTicks.SetTop();

                AxisX.MajorTicks.DrawTicks();
                
                AxisX.DrawAxisLine();

                AxisX.AxisLabels.SetLeft();


                AxisYPrimary.MajorGrids.SetWidth();
                AxisYPrimary.MajorGrids.SetLeft();
                AxisYPrimary.MajorGrids.DrawGrids();

                AxisYPrimary.MajorTicks.SetLeft();
                AxisYPrimary.MajorTicks.SetWidth();
                AxisYPrimary.MajorTicks.SetHeight();
                AxisYPrimary.MajorTicks.SetTop();

                AxisYPrimary.MajorTicks.DrawTicks();
                AxisYPrimary.DrawAxisLine();

                AxisYPrimary.AxisLabels.SetTop();

                if (AxisYSecondary != null)
                {
                    AxisYSecondary.MajorGrids.SetWidth();
                    AxisYSecondary.MajorGrids.SetLeft();
                    AxisYSecondary.MajorGrids.DrawGrids();

                    AxisYSecondary.MajorTicks.SetLeft();
                    AxisYSecondary.MajorTicks.SetWidth();
                    AxisYSecondary.MajorTicks.SetHeight();
                    AxisYSecondary.MajorTicks.SetTop();

                    AxisYSecondary.MajorTicks.DrawTicks();
                    AxisYSecondary.DrawAxisLine();

                    AxisYSecondary.AxisLabels.SetTop();
                }

                // Positions titles that have to be placed outside PlotArea
                _titles.ForEach(delegate(Title child) { child.PlaceInsidePlotArea(); });


                // Positions legends that have to be placed outside PlotArea
                _legends.ForEach(delegate(Legend child)
                {
                    child.PlaceInsidePlotArea(new Rect((Double)PlotArea.GetValue(LeftProperty), (Double)PlotArea.GetValue(TopProperty), PlotArea.Width, PlotArea.Height), Padding);
                });

                //Removes unused legends
                RemoveUnMarkedLegends();

                AxisX.PlaceTitle();
                AxisYPrimary.PlaceTitle();
                if (AxisYSecondary != null)  AxisYSecondary.PlaceTitle();

                PlotArea.ApplyBorder();

                // Position trend lines
                _trendLines.ForEach(delegate(TrendLine trendLine) { trendLine.InitAndDraw(); });

                
            }
            else if (PlotDetails.AxisOrientation == AxisOrientation.Pie)
            {
                PlotArea.SetTop();
                PlotArea.SetHeight();
                PlotArea.SetLeft();
                PlotArea.SetWidth();

                AxisX.SetValue(VisibilityProperty, Visibility.Collapsed);
                AxisYPrimary.SetValue(VisibilityProperty, Visibility.Collapsed);

                // Positions titles that have to be placed outside PlotArea
                _titles.ForEach(delegate(Title child) { child.PlaceInsidePlotArea(); });


                // Positions legends that have to be placed outside PlotArea
                _legends.ForEach(delegate(Legend child)
                {
                    child.PlaceInsidePlotArea(new Rect((Double)PlotArea.GetValue(LeftProperty), (Double)PlotArea.GetValue(TopProperty), PlotArea.Width, PlotArea.Height), Padding);
                });

                //Removes unused legends
                RemoveUnMarkedLegends();

            }


            //Apply lighting bevel and other such effects to plotarea
            PlotArea.ApplyEffects();

            _dataSeries.ForEach(delegate(Visifire.Charts.DataSeries child) { child.PlaceDataSeries(); });


        }

        public override void Render()
        {

            if (_logo != null)
                _logo.Render();

            if (_titles.Count != 0)
            {
                foreach (Title child in _titles)
                {
                    child.Render();
                }
            }

            if (_legends.Count != 0)
            {
                foreach (Legend child in _legends)
                {
                    child.Render();
                }

            }


            PlotArea.Render();
            if (PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {
                AxisX.Render();
                AxisYPrimary.Render();

                if (AxisYSecondary != null) AxisYSecondary.Render();
            }


            foreach (Plot plot in PlotDetails.Plots)
            {
                foreach (DataSeries ds in plot.DataSeries)
                {
                    ds.PlotData();
                }
            }
            
            if (OnLoadedEvent != null)
            {
                OnLoadedEvent(this, null);
            }

            AttachHref();
            AttachToolTip();

            DisplayWatermark();
        }

        public override void SetLeft()
        {
            throw new NotImplementedException();
        }

        public override void SetTop()
        {
            throw new NotImplementedException();
        }

        public override void SetWidth()
        {
            this.SetValue(WidthProperty, _parent.Width);
        }

        public override void SetHeight()
        {
            this.SetValue(HeightProperty, _parent.Height);
        }

        public void DisplayWatermark()
        {
            if (!Watermark) return;
            _watermark = new TextBlock();
            _watermark.SetValue(ZIndexProperty, 1000);
            if (Parser.GetBrushIntensity(this.Background) > 0.5)
                _watermark.Foreground = new SolidColorBrush(Colors.Black);
            else
                _watermark.Foreground = new SolidColorBrush(Colors.White);

            _watermark.Text = "Powered by Visifire";
            _watermark.FontSize = 10;
            _watermark.Opacity = 0.3;
            _watermark.SetValue(LeftProperty, (Double) ( this.Width - _watermark.ActualWidth - 6));
            _watermark.SetValue(TopProperty, (Double) 3);

            _watermark.MouseLeftButtonUp += delegate(Object sender, MouseButtonEventArgs e)
            {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://www.visifire.com"));
            };
            _watermark.MouseEnter += delegate(Object sender, MouseEventArgs e)
            {
                this.Cursor = Cursors.Hand;
            };
            _watermark.MouseLeave += delegate(Object sender, MouseEventArgs e)
            {
                this.Cursor = Cursors.Arrow;
            };
            this.Children.Add(_watermark);
        }

        #endregion Public Methods

        #region Public Properties

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

        public Boolean UniqueColors
        {
            get
            {
                if (String.IsNullOrEmpty(_uniqueColors) || _uniqueColors == "Undefined")
                {
                    if (GetFromTheme("UniqueColors") != null)
                        return Convert.ToBoolean(GetFromTheme("UniqueColors"));
                    else
                        return true;
                }
                else
                    return Boolean.Parse(_uniqueColors);

            }
            set
            {
                _uniqueColors = value.ToString();
            }
        }

        public Boolean View3D
        {
            get
            {
                if (_view3D == "Undefined" || String.IsNullOrEmpty(_view3D))
                {
                    if (GetFromTheme("View3D") == null)
                        return false;
                    else
                        return Convert.ToBoolean(GetFromTheme("View3D"));
                }
                else
                    return Boolean.Parse(_view3D);
            }
            set
            {
                _view3D = value.ToString();
            }
        }

        public Double AnimationDuration
        {
            get
            {
                if (!Double.IsNaN(_animationDuration))
                    return _animationDuration;
                else if (GetFromTheme("AnimationDuration") != null)
                    return Convert.ToDouble(GetFromTheme("AnimationDuration"));
                else
                    return 1;
            }
            set
            {
                if (value < 0)
                {
                    throw (new Exception("Animation duration cannot be negative"));
                }
                _animationDuration = value;
            }
        }

        public Boolean AnimationEnabled
        {
            get
            {
                if (_animationEnabled == "Undefined" || String.IsNullOrEmpty(_animationEnabled))
                {
                    if (GetFromTheme("AnimationEnabled") == null)
                        return false;
                    else
                        return Convert.ToBoolean(GetFromTheme("AnimationEnabled"));
                }
                else
                    return Boolean.Parse(_animationEnabled);
            }
            set
            {
                _animationEnabled = value.ToString();
            }
        }

        public String AnimationType
        {
            get
            {
                if (_animationType == "Undefined" || String.IsNullOrEmpty(_animationType))
                    return Convert.ToString(GetFromTheme("AnimationType"));
                else
                    return _animationType;
            }
            set
            {
                _animationType = value;
                if (_animationEnabled == "Undefined" || String.IsNullOrEmpty(_animationEnabled))
                    _animationEnabled = true.ToString();
            }
        }

        public Boolean Watermark
        {
            get;
            set;
        }

        public override LabelBase ToolTip
        {
            get
            {
                return _toolTip;
            }
        }

        
        #endregion Public Properties

        #region Public Events

        public EventHandler OnLoadedEvent;

        #endregion Public Events

        #region Protected Methods
        protected override void SetDefaults()
        {
            base.SetDefaults();

            _dataSeries = new System.Collections.Generic.List<DataSeries>();
            _trendLines = new System.Collections.Generic.List<TrendLine>();
            _titles = new System.Collections.Generic.List<Title>();
            _legends = new System.Collections.Generic.List<Legend>();
            _colorSets = new System.Collections.Generic.List<ColorSet>();

            _plotDetails = new PlotDetails();
            _view3D = "Undefined";
            BorderThickness = Double.NaN;
            _animationDuration = Double.NaN;
            AnimationEnabled = true;
            if (_label != null) _label.FontSize = Double.NaN;
            ColorSet = "";
            ColorSetReference = null;
            this.SetValue(ZIndexProperty, 1);
            _animationEnabled = "Undefined";
            _uniqueColors = "Undefined";
            Theme = "Theme1";

            Watermark = true;
        }
        #endregion Protected Methods

        #region Internal Properties

        internal Int32 TotalSiblings
        {
            get;
            set;
        }

        internal Boolean BorderDrawn
        {
            get;
            set;
        }

        public System.Collections.Generic.List<DataSeries> DataSeries
        {
            get
            {
                return _dataSeries;
            }
        }

        public System.Collections.Generic.List<Title> Titles
        {
            get
            {
                return _titles;
            }
        }

        public System.Collections.Generic.List<Legend> Legends
        {
            get
            {
                return _legends;
            }
        }

        internal System.Collections.Generic.List<ColorSet> ColorSets
        {
            get
            {
                return _colorSets;
            }
            set
            {
                _colorSets = value;
            }
        }

        internal Double Count
        {
            get;
            set;
        }

        internal PlotArea PlotArea
        {
            get
            {
                return _plotArea;
            }
        }

        internal PlotDetails PlotDetails
        {
            get
            {
                return _plotDetails;
            }
        }

        public AxisX AxisX
        {
            get
            {
                return _axisX;
            }
        }

        public AxisY AxisYPrimary
        {
            get
            {
                return _axisYPrimary;
            }
        }

        public AxisY AxisYSecondary
        {
            get
            {
                return _axisYSecondary;
            }
        }

        internal Double LabelPaddingTop
        {
            get
            {
                return _labelPaddingTop;
            }
            set
            {
                _labelPaddingTop = value;
                
            }
        }

        internal Double LabelPaddingBottom
        {
            get
            {
                return _labelPaddingBottom;
            }
            set
            {
                _labelPaddingBottom = value;
                
            }
        }

        internal Double LabelPaddingRight
        {
            get
            {
                return _labelPaddingRight;
            }
            set
            {
                _labelPaddingRight = value;
                
            }
        }

        internal Double LabelPaddingLeft
        {
            get
            {
                return _labelPaddingLeft;
            }
            set
            {
                _labelPaddingLeft = value;
                
            }
        }

        internal ColorSet ColorSetReference
        {
            get;
            set;
        }

        internal Label Label
        {
            get
            {
                return _label;
            }
        }

        internal Rect InnerBounds
        {
            get
            {
                return _innerBounds;
            }
            set
            {
                _innerBounds = value;
            }

        }

        #endregion

        #region Private Methods

        private void ApplyDoubleAnimation(DependencyObject target, String targetProperty, Double from, Double to, Double duration, Double beginTime)
        {
            TimeSpan durationTimeSpan = new TimeSpan(0, 0, 0, 0, (Int32)(1000 * duration));
            TimeSpan beginTimeSpan = new TimeSpan(0, 0, 0, 0, (Int32)(1000 * beginTime));
            
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();

            doubleAnimation.Duration = durationTimeSpan;

            doubleAnimation.BeginTime = beginTimeSpan;

            storyboard.Children.Add(doubleAnimation);

            Storyboard.SetTarget(doubleAnimation, target);


            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(targetProperty));

            doubleAnimation.From = from;

            doubleAnimation.To = to;
            
            this.Resources.Add(storyboard.GetHashCode().ToString(), storyboard);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                this.Resources.Remove((sender as Storyboard).GetHashCode().ToString());
                _storyboardEndCounter++;
                ApplyPostAnimationSettings();
            };

            animation.Add(storyboard);

        }

        private void ApplySplineDoubleKeyFrameAnimation(DependencyObject target, String targetProperty, Double duration, Double beginTime, Double[] valueSet, Double[] timeSet)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimationUsingKeyFrames doubleAnimation = new DoubleAnimationUsingKeyFrames();

            storyboard.Children.Add(doubleAnimation);

            Storyboard.SetTarget(doubleAnimation, target);

            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(targetProperty));

            for (Int32 i = 0; i < valueSet.Length; i++)
            {
                SplineDoubleKeyFrame splineKeyframe = new SplineDoubleKeyFrame();

                splineKeyframe.Value = valueSet[i];

                splineKeyframe.KeyTime = TimeSpan.FromMilliseconds((Int32)(1000 * (timeSet[i] * duration + beginTime)));

                KeySpline Spline = new KeySpline();

                if (i % 2 == 0)
                {
                    Spline.ControlPoint1 = new Point(0, 0);
                    Spline.ControlPoint2 = new Point(0.25, 1);
                }
                else
                {
                    Spline.ControlPoint1 = new Point(0.75, 0);
                    Spline.ControlPoint2 = new Point(1, 1);
                }
                splineKeyframe.KeySpline = Spline;

                doubleAnimation.KeyFrames.Add(splineKeyframe);
            }
            this.Resources.Add(storyboard.GetHashCode().ToString(), storyboard);

            storyboard.Completed += delegate(object sender, EventArgs e)
            {
                this.Resources.Remove((sender as Storyboard).GetHashCode().ToString());
                _storyboardEndCounter++;
                ApplyPostAnimationSettings();
            };

            animation.Add(storyboard);
        }

        /// <summary>
        /// Applies visual effects like lighting and shadow and bevel
        /// </summary>
        private void ApplyEffects()
        {
            if (ShadowEnabled)
            {
                this.Width -= 4;
                this.Height -= 4;
                ApplyShadow();
            }
            if (LightingEnabled)
            {
                ApplyLighting();
            }
            if (Bevel)
            {

                String[] type = { "Bright", "Medium", "Dark", "Medium" };
                Double[] length = { 10, 10, 10, 10 };
                Double[] Angle = { 90, 180, -90, 0 };
                ApplyBevel(type, length, Angle, 0);

            }
        }

        private void PostAnimationSettings()
        {
            if (_plotAreaBorder != null) _plotAreaBorder.Opacity = _plotArea.Opacity;

            //Restore Hit test state
            this.IsHitTestVisible = _chartHitTestState;

        }

        private void ApplyPostAnimationSettings()
        {

            if (AnimationEnabled)
            {
                if (_storyboardEndCounter == animation.Count)
                {
                    PostAnimationSettings();
                }
            }
            else
            {
                PostAnimationSettings();
            }
   
        }

        private Storyboard CreateStoryboard(String storyboard)
        {
            Storyboard sb;
            sb = (Storyboard)XamlReader.Load(storyboard);
            
            this.Resources.Add(sb.GetHashCode().ToString(), sb);

            sb.Completed += delegate(object sender1, EventArgs e1)
            {
                this.Resources.Remove((sender1 as Storyboard).GetHashCode().ToString());
                _storyboardEndCounter++;
                ApplyPostAnimationSettings();
            };
            return sb;
        }

        private void AnimationType1(Double initialTime)
        {
            ApplyDoubleAnimation(this, "Opacity", 0, 1, AnimationDuration, initialTime);
            this.Opacity = 0;
        }

        private void AnimationType2(Double initialTime,Double initialShootup)
        {
            System.Collections.Generic.List<Double> framesetTime = new System.Collections.Generic.List<Double>();
            System.Collections.Generic.List<Double>  framesetScaleY = new System.Collections.Generic.List<Double>();

            framesetTime.Add(0);
            framesetTime.Add(initialTime);
            framesetTime.Add(initialTime + 0.2);
            framesetTime.Add(initialTime + 0.36);
            framesetTime.Add(initialTime + 0.52);
            framesetTime.Add(initialTime + 0.68);
            framesetTime.Add(initialTime + 0.84);
            framesetTime.Add(initialTime + 1);

            Double skipSize = initialShootup - 1;
            framesetScaleY.Add(0);
            framesetScaleY.Add(0);
            framesetScaleY.Add(initialShootup);
            framesetScaleY.Add(1 - skipSize / 2);
            framesetScaleY.Add(1 + skipSize / 4);
            framesetScaleY.Add(1 - skipSize / 8);
            framesetScaleY.Add(1 + skipSize / 16);
            framesetScaleY.Add(1);


            if (PlotDetails.AxisOrientation == AxisOrientation.Bar || PlotDetails.AxisOrientation == AxisOrientation.Column)
            {
                ApplyDoubleAnimation(AxisX, "Opacity", 0, AxisX.Opacity, AnimationDuration, 0);
                ApplyDoubleAnimation(AxisYPrimary, "Opacity", 0, AxisYPrimary.Opacity, AnimationDuration, 0);
                ApplyDoubleAnimation(AxisYPrimary.MajorGrids, "Opacity", 0, AxisYPrimary.MajorGrids.Opacity, AnimationDuration, 0);
                if (AxisYSecondary != null)
                {
                    ApplyDoubleAnimation(AxisYSecondary, "Opacity", 0, AxisYSecondary.Opacity, AnimationDuration, 0);
                    ApplyDoubleAnimation(AxisYSecondary.MajorGrids, "Opacity", 0, AxisYSecondary.MajorGrids.Opacity, AnimationDuration, 0);
                }
                ApplyDoubleAnimation(PlotArea, "Opacity", 0, PlotArea.Opacity, AnimationDuration, 0);
            }

            // Layer containing the label and marker for area charts
            if (_areaLabelMarker != null)
            {
                ApplyDoubleAnimation(_areaLabelMarker, "Opacity", 0, 1, 0.1, initialTime + AnimationDuration);
                _areaLabelMarker.Opacity = 0;
            }

            ScaleTransform st = new ScaleTransform();

            if (View3D && PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {

                for (Int32 i = 0; i < Surface3D.Length; i++)
                {
                    if (Surface3D[i] == null) continue;
                    st = new ScaleTransform();
                    st.CenterX = Surface3D[i].Width / 2;
                    st.CenterY = Surface3D[i].Height / 2;

                    Surface3D[i].RenderTransform = st;
                    Surface3D[i].SetValue(NameProperty, GetNewObjectName(Surface3D[i]));
                    st.SetValue(NameProperty, st.GetType().Name + Surface3D[i].Name);

                    ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());
                    ApplyDoubleAnimation(Surface3D[i], "Opacity", 0, Surface3D[i].Opacity, AnimationDuration, 0);

                    Surface3D[i].Opacity = 0;
                }
                for (Int32 i = 0; i < AreaLine3D.Count; i++)
                {
                    if (AreaLine3D[i] == null) continue;
                    st = new ScaleTransform();
                    st.CenterX = AreaLine3D[i].Width / 2;
                    st.CenterY = AreaLine3D[i].Height / 2;

                    AreaLine3D[i].RenderTransform = st;
                    AreaLine3D[i].SetValue(NameProperty, GetNewObjectName(AreaLine3D[i]));
                    st.SetValue(NameProperty, st.GetType().Name + AreaLine3D[i].Name);

                    ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());
                    ApplyDoubleAnimation(AreaLine3D[i], "Opacity", 0, AreaLine3D[i].Opacity, AnimationDuration, 0);
                    AreaLine3D[i].Opacity = 0;
                }
                
                foreach (DataSeries child in DataSeries)
                {
                    if (child.RenderAs.ToLower() == "point" || child.RenderAs.ToLower() == "bubble")
                    {
                        st = new ScaleTransform();
                        st.CenterX = child.Width / 2;
                        st.CenterY = child.Height / 2;
                        child.RenderTransform = st;
                        st.SetValue(NameProperty, st.GetType().Name + child.Name);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());
                        ApplyDoubleAnimation(child, "Opacity", 0, child.Opacity, AnimationDuration, 0);
                        child.Opacity = 0;
                    }
                }
            }
            else
            {
                Int32 i = 0;
                foreach (DataSeries child in _dataSeries)
                {

                    st.CenterX = child.Width / 2;
                    st.CenterY = child.Height / 2;
                    child.RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + child.Name);


                    ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", (Double)AnimationDuration / (Double)_dataSeries.Count, (Double)i / (Double)_dataSeries.Count + initialTime, framesetScaleY.ToArray(), framesetTime.ToArray());
                    ApplyDoubleAnimation(child, "Opacity", 0, child.Opacity, 0, (Double)i / (Double)_dataSeries.Count + initialTime);
                    child.Opacity = 0;
                    st = new ScaleTransform();
                    i++;
                }
            }

        }

        private void AnimationType3(Double initialTime, Double initialShootup)
        {
            System.Collections.Generic.List<Double> framesetTime = new System.Collections.Generic.List<Double>();
            System.Collections.Generic.List<Double> framesetScaleY = new System.Collections.Generic.List<Double>();

            framesetTime.Add(0);
            framesetTime.Add(initialTime);
            framesetTime.Add(initialTime + 0.2);
            framesetTime.Add(initialTime + 0.36);
            framesetTime.Add(initialTime + 0.52);
            framesetTime.Add(initialTime + 0.68);
            framesetTime.Add(initialTime + 0.84);
            framesetTime.Add(initialTime + 1);

            Double skipSize = initialShootup - 1;
            framesetScaleY.Add(0);
            framesetScaleY.Add(0);
            framesetScaleY.Add(initialShootup);
            framesetScaleY.Add(1 - skipSize / 2);
            framesetScaleY.Add(1 + skipSize / 4);
            framesetScaleY.Add(1 - skipSize / 8);
            framesetScaleY.Add(1 + skipSize / 16);
            framesetScaleY.Add(1);

            if (PlotDetails.AxisOrientation == AxisOrientation.Bar || PlotDetails.AxisOrientation == AxisOrientation.Column)
            {

                ApplyDoubleAnimation(AxisX, "Opacity", 0, AxisX.Opacity, AnimationDuration, 0);
                ApplyDoubleAnimation(AxisYPrimary, "Opacity", 0, AxisYPrimary.Opacity, AnimationDuration, 0);
                ApplyDoubleAnimation(AxisYPrimary.MajorGrids, "Opacity", 0, AxisYPrimary.MajorGrids.Opacity, AnimationDuration, 0);

                if (AxisYSecondary != null)
                {
                    ApplyDoubleAnimation(AxisYSecondary, "Opacity", 0, AxisYSecondary.Opacity, AnimationDuration, 0);
                    ApplyDoubleAnimation(AxisYSecondary.MajorGrids, "Opacity", 0, AxisYSecondary.MajorGrids.Opacity, AnimationDuration, 0);
                }

                ApplyDoubleAnimation(PlotArea, "Opacity", 0, PlotArea.Opacity, AnimationDuration, 0);
            }

            // Layer containing the label and marker for area charts
            if (_areaLabelMarker != null)
            {
                ApplyDoubleAnimation(_areaLabelMarker, "Opacity", 0, 1, 0.1, initialTime + AnimationDuration);
                _areaLabelMarker.Opacity = 0;
            }

            ScaleTransform st = new ScaleTransform();

            if (View3D && PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {
                for (Int32 i = 0; i < Surface3D.Length; i++)
                {

                    if (Surface3D[i] == null) continue;
                    Surface3D[i].SetValue(NameProperty, GetNewObjectName(Surface3D[i]));
                    st = new ScaleTransform();


                    Surface3D[i].RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + Surface3D[i].Name);

                    if (PlotDetails.AxisOrientation == AxisOrientation.Column)
                    {

                        Surface3D[i].Width = PlotArea.Width + (Double)PlotArea.GetValue(LeftProperty);
                        Surface3D[i].Height = (Double)PlotArea.GetValue(TopProperty) + PlotArea.Height + AxisX.MajorTicks.TickLength;

                        Surface3D[i].RenderTransformOrigin = new Point(0.5, 1);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());

                    }
                    else
                    {

                        Double scaleX = ((Double)PlotArea.GetValue(LeftProperty) - AxisX.MajorTicks.TickLength) / Surface3D[i].Width;
                        Surface3D[i].RenderTransformOrigin = new Point(scaleX, 0.5);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleX", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());

                    }
                }
                for (Int32 i = 0; i < AreaLine3D.Count; i++)
                {

                    if (AreaLine3D[i] == null) continue;
                    AreaLine3D[i].SetValue(NameProperty, GetNewObjectName(AreaLine3D[i]));
                    st = new ScaleTransform();

                    AreaLine3D[i].Width = PlotArea.Width + (Double)PlotArea.GetValue(LeftProperty);
                    AreaLine3D[i].Height = (Double)PlotArea.GetValue(TopProperty) + PlotArea.Height + AxisX.MajorTicks.TickLength;

                    AreaLine3D[i].RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + AreaLine3D[i].Name);

                    AreaLine3D[i].RenderTransformOrigin = new Point(0.5, 1);

                    ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());



                }
                foreach (DataSeries child in DataSeries)
                {
                    if (child.RenderAs.ToLower() == "point" || child.RenderAs.ToLower() == "bubble")
                    {
                        st = new ScaleTransform();
                        st.CenterX = child.Width / 2;
                        st.CenterY = child.Height / 2;
                        child.RenderTransform = st;


                        child.RenderTransformOrigin = new Point(0.5, 1);
                        st.SetValue(NameProperty, st.GetType().Name + child.Name);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());


                    }
                }

            }
            else
            {
                Int32 i = 0;
                foreach (DataSeries child in _dataSeries)
                {

                    child.RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + child.Name);


                    if (PlotDetails.AxisOrientation == AxisOrientation.Column || PlotDetails.AxisOrientation == AxisOrientation.Pie)
                    {

                        child.RenderTransformOrigin = new Point(0.5, 1);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());


                    }
                    else
                    {

                        child.RenderTransformOrigin = new Point(0, 0.5);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleX", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());


                    }
                    st = new ScaleTransform();
                    i++;
                }

            }
            
        }

        private void AnimationType4(Double initialTime)
        {
            Int32 i = 0;
            if (PlotDetails.AxisOrientation == AxisOrientation.Bar || PlotDetails.AxisOrientation == AxisOrientation.Column)
            {

                ApplyDoubleAnimation(AxisX, "Opacity", 0, AxisX.Opacity, AnimationDuration, initialTime);
                ApplyDoubleAnimation(AxisYPrimary, "Opacity", 0, AxisYPrimary.Opacity, AnimationDuration, initialTime);
                ApplyDoubleAnimation(AxisYPrimary.MajorGrids, "Opacity", 0, AxisYPrimary.MajorGrids.Opacity, AnimationDuration, initialTime);

                if (AxisYSecondary != null)
                {
                    ApplyDoubleAnimation(AxisYSecondary, "Opacity", 0, AxisYSecondary.Opacity, AnimationDuration, initialTime);
                    ApplyDoubleAnimation(AxisYSecondary.MajorGrids, "Opacity", 0, AxisYSecondary.MajorGrids.Opacity, AnimationDuration, initialTime);
                    AxisYSecondary.Opacity = 0;
                    AxisYSecondary.MajorGrids.Opacity = 0;

                }

                ApplyDoubleAnimation(PlotArea, "Opacity", 0, PlotArea.Opacity, AnimationDuration, initialTime);
                AxisX.Opacity = 0;
                AxisYPrimary.Opacity = 0;
                AxisYPrimary.MajorGrids.Opacity = 0;
                PlotArea.Opacity = 0;
            }

            // Layer containing the label and marker for area charts
            if (_areaLabelMarker != null)
            {
                ApplyDoubleAnimation(_areaLabelMarker, "Opacity", 0, 1, 0.1, initialTime);
                _areaLabelMarker.Opacity = 0;
            }

            System.Collections.Generic.List<Double> rx = new System.Collections.Generic.List<Double>();
            System.Collections.Generic.List<Double> ry = new System.Collections.Generic.List<Double>();
            Random rand = new Random();


            foreach (DataSeries child in _dataSeries)
            {
                rx.Clear();
                ry.Clear();
                Double angle = Math.PI / Math.Abs(child.DataPoints.Count - 1);
                for (i = 0; i < child.DataPoints.Count; i++)
                {

                    rx.Add(child.Width / 2 * Math.Cos(angle * (i + 1)));
                    ry.Add(child.Height / 2 * Math.Sin(angle * (i + 1)));
                }
                if (View3D && PlotDetails.AxisOrientation == AxisOrientation.Pie)
                {
                    #region PieDoughnut3D animation

                    foreach (DataPoint dp in child.DataPoints)
                    {
                        if (dp.LabelLine != null)
                        {
                            dp.LabelLine.SetValue(NameProperty, GetNewObjectName(dp.LabelLine));


                            ApplyDoubleAnimation(dp.LabelLine, "Opacity", 0, dp.LabelLine.Opacity, initialTime, AnimationDuration);
                            dp.LabelLine.Opacity = 0;
                        }
                        if (dp.Label != null)
                        {
                            dp.Label.SetValue(NameProperty, GetNewObjectName(dp.Label));


                            ApplyDoubleAnimation(dp.Label, "Opacity", 0, dp.Label.Opacity, initialTime, AnimationDuration);
                        }
                    }
                    i = 0;
                    foreach (Path path in child._pies)
                    {
                        if (path == null) continue;
                        path.SetValue(NameProperty, GetNewObjectName(path));

                        ApplyDoubleAnimation(path, "Opacity", 0, path.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Left)", (Double)path.GetValue(LeftProperty) + rx[i], (Double)path.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Top)", (Double)path.GetValue(TopProperty) + ry[i], (Double)path.GetValue(TopProperty), AnimationDuration, initialTime);
                        path.Opacity = 0;
                        i++;
                    }
                    i = 0;
                    foreach (Path path in child._pieSides)
                    {
                        if (path == null) continue;
                        path.SetValue(NameProperty, GetNewObjectName(path));

                        ApplyDoubleAnimation(path, "Opacity", 0, path.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Left)", (Double)path.GetValue(LeftProperty) + rx[i], (Double)path.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Top)", (Double)path.GetValue(TopProperty) + ry[i], (Double)path.GetValue(TopProperty), AnimationDuration, initialTime);
                        path.Opacity = 0;
                        i++;
                    }
                    i = 0;
                    foreach (Path path in child._pieRight)
                    {
                        if (path == null) continue;
                        path.SetValue(NameProperty, GetNewObjectName(path));

                        ApplyDoubleAnimation(path, "Opacity", 0, path.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Left)", (Double)path.GetValue(LeftProperty) + rx[i], (Double)path.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Top)", (Double)path.GetValue(TopProperty) + ry[i], (Double)path.GetValue(TopProperty), AnimationDuration, initialTime);
                        path.Opacity = 0;
                        i++;
                    }
                    i = 0;
                    foreach (Path path in child._pieLeft)
                    {
                        if (path == null) continue;
                        path.SetValue(NameProperty, GetNewObjectName(path));

                        ApplyDoubleAnimation(path, "Opacity", 0, path.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Left)", (Double)path.GetValue(LeftProperty) + rx[i], (Double)path.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(path, "(Canvas.Top)", (Double)path.GetValue(TopProperty) + ry[i], (Double)path.GetValue(TopProperty), AnimationDuration, initialTime);
                        path.Opacity = 0;
                        i++;
                    }
                    i = 0;
                    if (child._doughnut != null)
                        foreach (Path path in child._doughnut)
                        {
                            if (path == null) continue;
                            path.SetValue(NameProperty, GetNewObjectName(path));

                            ApplyDoubleAnimation(path, "Opacity", 0, path.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(path, "(Canvas.Left)", (Double)path.GetValue(LeftProperty) + rx[i], (Double)path.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(path, "(Canvas.Top)", (Double)path.GetValue(TopProperty) + ry[i], (Double)path.GetValue(TopProperty), AnimationDuration, initialTime);
                            path.Opacity = 0;
                            i++;
                        }
                    if (child.auxSide1 != null)
                    {

                        child.auxSide1.SetValue(NameProperty, GetNewObjectName(child.auxSide1));

                        ApplyDoubleAnimation(child.auxSide1, "Opacity", 0, child.auxSide1.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide1, "(Canvas.Left)", (Double)child.auxSide1.GetValue(LeftProperty) + rx[child.auxID1], (Double)child.auxSide1.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide1, "(Canvas.Top)", (Double)child.auxSide1.GetValue(TopProperty) + ry[child.auxID1], (Double)child.auxSide1.GetValue(TopProperty), AnimationDuration, initialTime);
                        child.auxSide1.Opacity = 0;
                    }
                    if (child.auxSide2 != null)
                    {

                        child.auxSide2.SetValue(NameProperty, GetNewObjectName(child.auxSide2));

                        ApplyDoubleAnimation(child.auxSide2, "Opacity", 0, child.auxSide2.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide2, "(Canvas.Left)", (Double)child.auxSide2.GetValue(LeftProperty) + rx[child.auxID2], (Double)child.auxSide2.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide2, "(Canvas.Top)", (Double)child.auxSide2.GetValue(TopProperty) + ry[child.auxID2], (Double)child.auxSide2.GetValue(TopProperty), AnimationDuration, initialTime);

                        child.auxSide2.Opacity = 0;
                    }

                    if (child.auxSide4 != null)
                    {

                        child.auxSide4.SetValue(NameProperty, GetNewObjectName(child.auxSide4));

                        ApplyDoubleAnimation(child.auxSide4, "Opacity", 0, child.auxSide4.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide4, "(Canvas.Left)", (Double)child.auxSide4.GetValue(LeftProperty) + rx[child.auxID4], (Double)child.auxSide4.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide4, "(Canvas.Top)", (Double)child.auxSide4.GetValue(TopProperty) + ry[child.auxID4], (Double)child.auxSide4.GetValue(TopProperty), AnimationDuration, initialTime);

                        child.auxSide4.Opacity = 0;
                    }
                    if (child.auxSide5 != null)
                    {

                        child.auxSide5.SetValue(NameProperty, GetNewObjectName(child.auxSide5));

                        ApplyDoubleAnimation(child.auxSide5, "Opacity", 0, child.auxSide5.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide5, "(Canvas.Left)", (Double)child.auxSide5.GetValue(LeftProperty) + rx[child.auxID5], (Double)child.auxSide5.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(child.auxSide5, "(Canvas.Top)", (Double)child.auxSide5.GetValue(TopProperty) + ry[child.auxID5], (Double)child.auxSide5.GetValue(TopProperty), AnimationDuration, initialTime);

                        child.auxSide5.Opacity = 0;
                    }

                    #endregion PieDoughnut3D animation
                }
                if (View3D && (child.RenderAs.ToLower() != "bubble" && child.RenderAs.ToLower() != "point" && child.RenderAs.ToLower() != "line"))
                {
                    foreach (DataPoint dp in child.DataPoints)
                    {
                        if (dp.Marker != null)
                        {
                            dp.Marker.SetValue(NameProperty, GetNewObjectName(dp.Marker));


                            ApplyDoubleAnimation(dp.Marker, "Opacity", 0, dp.Marker.Opacity, 0, AnimationDuration + initialTime);
                            dp.Marker.Opacity = 0;
                        }
                        if (dp.Label != null)
                        {
                            dp.Label.SetValue(NameProperty, GetNewObjectName(dp.Label));


                            ApplyDoubleAnimation(dp.Label, "Opacity", 0, dp.Label.Opacity, 0, AnimationDuration + initialTime);
                            dp.Label.Opacity = 0;
                        }
                    }
                    if (child._areas != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._areas)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }
                    if (child._areaShadows != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._areaShadows)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }
                    if (child._areaTops != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._areaTops)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }
                    if (child._columns != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._columns)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }
                    if (child._columnSides != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._columnSides)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }
                    if (child._columnTops != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._columnTops)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }
                    if (child._shadows != null)
                    {
                        i = 0;
                        foreach (Shape shape in child._shadows)
                        {
                            shape.SetValue(NameProperty, GetNewObjectName(shape));

                            ApplyDoubleAnimation(shape, "Opacity", 0, shape.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Left)", (Double)shape.GetValue(LeftProperty) + rx[i], (Double)shape.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(shape, "(Canvas.Top)", (Double)shape.GetValue(TopProperty) + ry[i], (Double)shape.GetValue(TopProperty), AnimationDuration, initialTime);
                            shape.Opacity = 0;
                            i++;
                        }
                    }


                }
                else if (child.RenderAs.ToLower() == "bubble" || child.RenderAs.ToLower() == "point")
                {
                    i = 0;
                    foreach (DataPoint dp in child.DataPoints)
                    {
                        if (dp.Marker != null)
                        {
                            dp.Marker.SetValue(NameProperty, GetNewObjectName(dp.Marker));



                            ApplyDoubleAnimation(dp.Marker, "Opacity", 0, dp.Marker.Opacity, AnimationDuration, initialTime);
                            ApplyDoubleAnimation(dp.Marker, "(Canvas.Left)", (Double)dp.Marker.GetValue(LeftProperty) + rx[i], (Double)dp.Marker.GetValue(LeftProperty), AnimationDuration, initialTime);
                            ApplyDoubleAnimation(dp.Marker, "(Canvas.Top)", (Double)dp.Marker.GetValue(TopProperty) + ry[i], (Double)dp.Marker.GetValue(TopProperty), AnimationDuration, initialTime);
                            dp.Marker.Opacity = 0;
                            i++;

                        }
                        if (dp.Label != null)
                        {
                            dp.Label.SetValue(NameProperty, GetNewObjectName(dp.Label));


                            ApplyDoubleAnimation(dp.Label, "Opacity", 0, dp.Label.Opacity, 0, AnimationDuration);
                            dp.Label.Opacity = 0;
                        }
                    }
                }
                else if (child.RenderAs.ToLower() == "line")
                {
                    foreach (DataPoint dp in child.DataPoints)
                    {
                        if (dp.Marker != null)
                        {
                            dp.Marker.SetValue(NameProperty, GetNewObjectName(dp.Marker));


                            ApplyDoubleAnimation(dp.Marker, "Opacity", 0, dp.Marker.Opacity, 0, AnimationDuration + initialTime);
                            dp.Marker.Opacity = 0;
                        }
                        if (dp.Label != null)
                        {
                            dp.Label.SetValue(NameProperty, GetNewObjectName(dp.Label));


                            ApplyDoubleAnimation(dp.Label, "Opacity", 0, dp.Label.Opacity, 0, AnimationDuration + initialTime);
                            dp.Label.Opacity = 0;
                        }
                    }
                    if (child._line != null)
                    {
                        child._line.SetValue(NameProperty, GetNewObjectName(child._line));


                        ApplyDoubleAnimation(child._line, "Opacity", 0, child._line.Opacity, AnimationDuration, initialTime);
                        child._line.Opacity = 0;
                        ScaleTransform st = new ScaleTransform();
                        st.CenterX = child.Width / 2;
                        st.CenterY = child.Height / 2;
                        child._line.RenderTransform = st;
                        st.SetValue(NameProperty, GetNewObjectName(st));

                        ApplyDoubleAnimation(st, "ScaleX", 0, 1, AnimationDuration / 2, initialTime);
                        ApplyDoubleAnimation(st, "ScaleY", 0, 1, AnimationDuration, initialTime);


                    }
                    if (child._lineShadow != null)
                    {
                        child._lineShadow.SetValue(NameProperty, GetNewObjectName(child._lineShadow));


                        ApplyDoubleAnimation(child._lineShadow, "Opacity", 0, child._lineShadow.Opacity, AnimationDuration, initialTime);
                        ScaleTransform st = new ScaleTransform();
                        st.CenterX = child.Width / 2;
                        st.CenterY = child.Height / 2;
                        child._lineShadow.RenderTransform = st;
                        st.SetValue(NameProperty, GetNewObjectName(st));

                        ApplyDoubleAnimation(st, "ScaleX", 0, 1, AnimationDuration / 2, initialTime);
                        ApplyDoubleAnimation(st, "ScaleY", 0, 1, AnimationDuration, initialTime);
                    }
                }
                else
                {
                    i = 0;
                    foreach (DataPoint dp in child.DataPoints)
                    {
                        if (PlotDetails.AxisOrientation == AxisOrientation.Pie)
                        {

                            if (dp.LabelLine != null)
                            {
                                dp.LabelLine.SetValue(NameProperty, GetNewObjectName(dp.LabelLine));


                                ApplyDoubleAnimation(dp.LabelLine, "Opacity", 0, dp.LabelLine.Opacity, 0, AnimationDuration + initialTime);
                                dp.LabelLine.Opacity = 0;
                            }
                        }
                        if (dp.Label != null)
                        {
                            dp.Label.SetValue(NameProperty, GetNewObjectName(dp.Label));


                            ApplyDoubleAnimation(dp.Label, "Opacity", 0, dp.Label.Opacity, 0, AnimationDuration + initialTime);
                            dp.Label.Opacity = 0;
                        }
                        ApplyDoubleAnimation(dp, "Opacity", 0, dp.Opacity, AnimationDuration, initialTime);
                        ApplyDoubleAnimation(dp, "(Canvas.Left)", (Double)dp.GetValue(LeftProperty) + rx[i], (Double)dp.GetValue(LeftProperty), AnimationDuration, initialTime);
                        ApplyDoubleAnimation(dp, "(Canvas.Top)", (Double)dp.GetValue(TopProperty) + ry[i], (Double)dp.GetValue(TopProperty), AnimationDuration, initialTime);
                        dp.Opacity = 0;
                        i++;
                    }
                }
            }
        }

        private void AnimationType5(Double initialTime)
        {
            System.Collections.Generic.List<Double> framesetTime = new System.Collections.Generic.List<Double>();
            System.Collections.Generic.List<Double> framesetScaleY = new System.Collections.Generic.List<Double>();

            framesetTime.Clear();
            framesetTime.Add(0);
            framesetTime.Add(initialTime);
            framesetTime.Add(0.99);
            framesetTime.Add(1);

            framesetScaleY.Clear();
            framesetScaleY.Add(0);
            framesetScaleY.Add(0);
            framesetScaleY.Add(0.99);
            framesetScaleY.Add(1);

            AnimationDuration -= initialTime;

            if (PlotDetails.AxisOrientation == AxisOrientation.Bar || PlotDetails.AxisOrientation == AxisOrientation.Column)
            {

                ApplyDoubleAnimation(AxisX, "Opacity", 0, AxisX.Opacity, AnimationDuration, 0);
                ApplyDoubleAnimation(AxisYPrimary, "Opacity", 0, AxisYPrimary.Opacity, AnimationDuration, 0);
                ApplyDoubleAnimation(AxisYPrimary.MajorGrids, "Opacity", 0, AxisYPrimary.MajorGrids.Opacity, AnimationDuration, 0);
                if (AxisYSecondary != null)
                {
                    ApplyDoubleAnimation(AxisYSecondary, "Opacity", 0, AxisYSecondary.Opacity, AnimationDuration, 0);
                    ApplyDoubleAnimation(AxisYSecondary.MajorGrids, "Opacity", 0, AxisYSecondary.MajorGrids.Opacity, AnimationDuration, 0);

                }
                ApplyDoubleAnimation(PlotArea, "Opacity", 0, PlotArea.Opacity, AnimationDuration, 0);
            }

            // Layer containing the label and marker for area charts
            if (_areaLabelMarker != null)
            {
                ApplyDoubleAnimation(_areaLabelMarker, "Opacity", 0, 1, 0.1, initialTime + AnimationDuration);
                _areaLabelMarker.Opacity = 0;
            }

            ScaleTransform st = new ScaleTransform();

            if (View3D && PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {
                for (Int32 i = 0; i < Surface3D.Length; i++)
                {

                    if (Surface3D[i] == null) continue;
                    Surface3D[i].SetValue(NameProperty, GetNewObjectName(Surface3D[i]));
                    st = new ScaleTransform();


                    Surface3D[i].RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + Surface3D[i].Name);

                    if (PlotDetails.AxisOrientation == AxisOrientation.Column)
                    {

                        Surface3D[i].Width = PlotArea.Width + (Double)PlotArea.GetValue(LeftProperty);
                        Surface3D[i].Height = (Double)PlotArea.GetValue(TopProperty) + PlotArea.Height + AxisX.MajorTicks.TickLength;

                        Surface3D[i].RenderTransformOrigin = new Point(0.5, 1);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());

                    }
                    else
                    {

                        Double scaleX = ((Double)PlotArea.GetValue(LeftProperty) - AxisX.MajorTicks.TickLength) / Surface3D[i].Width;
                        Surface3D[i].RenderTransformOrigin = new Point(scaleX, 0.5);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleX", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());

                    }
                }
                for (Int32 i = 0; i < AreaLine3D.Count; i++)
                {

                    if (AreaLine3D[i] == null) continue;
                    AreaLine3D[i].SetValue(NameProperty, GetNewObjectName(AreaLine3D[i]));
                    st = new ScaleTransform();

                    AreaLine3D[i].Width = PlotArea.Width + (Double)PlotArea.GetValue(LeftProperty);
                    AreaLine3D[i].Height = (Double)PlotArea.GetValue(TopProperty) + PlotArea.Height + AxisX.MajorTicks.TickLength;

                    AreaLine3D[i].RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + AreaLine3D[i].Name);

                    AreaLine3D[i].RenderTransformOrigin = new Point(0.5, 1);

                    ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());



                }
                foreach (DataSeries child in DataSeries)
                {
                    if (child.RenderAs.ToLower() == "point" || child.RenderAs.ToLower() == "bubble")
                    {
                        st = new ScaleTransform();
                        st.CenterX = child.Width / 2;
                        st.CenterY = child.Height / 2;
                        child.RenderTransform = st;


                        child.RenderTransformOrigin = new Point(0.5, 1);
                        st.SetValue(NameProperty, st.GetType().Name + child.Name);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());


                    }
                }

            }
            else
            {
                Int32 i = 0;
                foreach (DataSeries child in _dataSeries)
                {

                    child.RenderTransform = st;
                    st.SetValue(NameProperty, st.GetType().Name + child.Name);


                    if (PlotDetails.AxisOrientation == AxisOrientation.Column || PlotDetails.AxisOrientation == AxisOrientation.Pie)
                    {

                        child.RenderTransformOrigin = new Point(0.5, 1);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleY", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());


                    }
                    else
                    {

                        child.RenderTransformOrigin = new Point(0, 0.5);

                        ApplySplineDoubleKeyFrameAnimation(st, "ScaleX", AnimationDuration, 0, framesetScaleY.ToArray(), framesetTime.ToArray());


                    }
                    st = new ScaleTransform();
                    i++;
                }

            }
        }

        private void GenerateAnimationData()
        {
   
            String animationTypeSelected = Constants.ThemeAndAnimation.DefaultAnimation;

            if (AnimationType != Constants.General.SrtingUndefined && !String.IsNullOrEmpty(AnimationType))
            {
                animationTypeSelected = AnimationType;
            }


            if (AnimationDuration > 0 && AnimationDuration < 0.6) AnimationDuration = 0.6;

            if (AnimationDuration <= 0) return;

            Double initialTime = 600/(AnimationDuration * 1000);
            Double initialShootup = ((Double)PlotArea.GetValue(TopProperty)+PlotArea.Height) / PlotArea.Height;
            
            AnimationDuration += initialTime;


            switch (animationTypeSelected.ToLower())
            {
                case "type1":
                    AnimationType1(initialTime);
                    break;
                case "type2":
                    AnimationType2(initialTime,initialShootup);
                    break;
                case "type3":
                    AnimationType3(initialTime, initialShootup);
                    break;
                case "type4":
                    AnimationType4(initialTime);
                    break;
                case "type5":
                    AnimationType5(initialTime);
                    break;
            }
            
            
        }

        /// <summary>
        /// Validate Parent element and assign it to _parent.
        /// Parent element should be a Canvas element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            if (this.Parent is Canvas)
                _parent = this.Parent as Canvas;
            else
                throw new Exception(this.GetType().Name + " Parent should be a Canvas");
        }

        /// <summary>
        /// Create reference to all child elements. If the user has not sepcified any required element, then 
        /// create them and assign a reference.
        /// </summary>
        private void CreateReferences()
        {
            // This function is required to prepare the color palette before and object is initialized
            // This is done so that any object color in future can be initialized using this palette
            GenerateDefaultColorSets();

            Int32 dataSeriesIndex = 0;
            Int32 legendIndex = 0;
            Int32 titleIndex = 0;
            Boolean PrimaryYAxis = false;
            Boolean SecondaryYAxis = false;
            foreach (FrameworkElement child in Children)
            {
                switch (child.GetType().Name)
                {
                    case "PlotArea":
                        if (_plotArea != null)
                            throw new Exception("There can be only one PlotArea");

                        _plotArea = child as PlotArea;
                        break;

                    case "AxisX":
                        if (_axisX != null)
                            throw new Exception("There can be only one AxisX");

                        _axisX = child as AxisX;
                        break;

                    case "AxisY":
                        if ((child as AxisY).AxisType == AxisType.Primary)
                        {
                            if(_axisYPrimary != null)
                                throw new Exception("There can be only one Primary AxisY");

                            _axisYPrimary = child as AxisY;
                        }
                        else if ((child as AxisY).AxisType == AxisType.Secondary)
                        {
                            if(_axisYSecondary != null)
                                throw new Exception("There can be only one Secondary AxisY");

                            _axisYSecondary = child as AxisY;
                        }
                        break;

                    case "Title":
                        if ((child as Title).Enabled)
                        {
                            _titles.Add(child as Title);
                            (child as Title).SetValue(ZIndexProperty, (Int32)(child as Title).GetValue(ZIndexProperty) + 6);
                            (child as Title).Index = titleIndex++;
                        }
                        break;

                    case "Legend":
                        _legends.Add(child as Legend);
                        (child as Legend).SetValue(ZIndexProperty, (Int32)(child as Legend).GetValue(ZIndexProperty) + 6);
                        (child as Legend).Index = legendIndex++;
                        break;

                    case "DataSeries":
                        if (!(child as DataSeries).Enabled) continue;
                        _dataSeries.Add(child as DataSeries);
                        child.SetValue(ZIndexProperty, dataSeriesIndex + (Int32)child.GetValue(ZIndexProperty));
                        (child as DataSeries).DrawingIndex = dataSeriesIndex++;
                        if ((child as DataSeries).AxisYType == AxisType.Primary) PrimaryYAxis = true;
                        else SecondaryYAxis = true;
                        break;

                    case "TrendLine":
                        if (!(child as TrendLine).Enabled) continue;
                        _trendLines.Add(child as TrendLine);
                        break;

                    case "ColorSet":
                        _colorSets.Add(child as ColorSet);
                        (child as ColorSet).Init();
                        break;

                    case "ToolTip":
                        _toolTip = child as ToolTip;
                        _toolTip.FixToolTipSize();
                        if (!_toolTip.Enabled)
                            _toolTip.Opacity = 0;
                        break;

                    case "Image":
                        if (!(child as Image).Enabled) continue;
                        _logo = child as Image;
                        break;

                    case "Label":
                        _label = child as Label;
                        _label.Init();
                        _label.Visibility = Visibility.Collapsed;
                        break;
                }
            }

            // If user has not specified AxisX, then create one and add to the children collection
            if (_axisX == null)
            {
                _axisX = new AxisX();
                this.Children.Add(_axisX);
            }

            // If user has not specified AxisY, then create one and add to the children collection
            if (_axisYPrimary == null)
            {
                _axisYPrimary = new AxisY();
                if (!PrimaryYAxis)
                    _axisYPrimary.Enabled = false;
                this.Children.Add(_axisYPrimary);
            }

            if (_axisYSecondary == null && SecondaryYAxis)
            {
                _axisYSecondary = new AxisY();
                _axisYSecondary.AxisType = AxisType.Secondary;
                this.Children.Add(_axisYSecondary);
            }

            // If user has not specified AxisY, then create one and add to the children collection
            if (_legends.Count == 0)
            {
                Legend _legend = new Legend();
                this.Children.Add(_legend);
                _legends.Add(_legend);
                _legend.Index = legendIndex++;
            }

            // If there is no PlotArea specified, create one and add to the children collection
            if (_plotArea == null)
            {
                _plotArea = new PlotArea();
                this.Children.Add(_plotArea);
            }

            if (_toolTip == null)
            {
                _toolTip = new ToolTip();
                this.Children.Add(_toolTip);
                _toolTip.FixToolTipSize();
            }

            if (_label == null)
            {
                _label = new Label();
                this.Children.Add(_label);
                _label.Init();
                _label.Visibility = Visibility.Collapsed;
            }

            //Get the ColorSet reference
            // Find if the pallete is available in the pallets collection
            foreach (ColorSet child in ColorSets)
            {
                if (child.Name.ToLower() == ColorSet.ToLower())
                {
                    ColorSetReference = child;
                    break;
                }
            }
            
        }

        private Plot FindPlot(ChartTypes chartType, AxisType axisYType)
        {
            foreach (Plot plot in PlotDetails.Plots)
            {
                if (plot.ChartType == chartType && plot.AxisY.AxisType == axisYType)
                {
                    return plot;
                }
            }
            return null;
        }

        private Plot CreatePlot(ChartTypes chartType, AxisType axisYType)
        {
            Plot plot = new Plot();
            plot.ChartType = chartType;
            plot.AxisY = (axisYType == AxisType.Primary) ? AxisYPrimary : AxisYSecondary;
            return plot;
        }

        private Int32 FindDataSeriesCountByChartType(ChartTypes chartType)
        {
            Int32 count = 0;
            foreach (Plot plot in PlotDetails.Plots)
            {
                if (plot.ChartType == chartType)
                {
                    count += plot.DataSeries.Count;
                }
            }
            return count;
        }

        private void GeneratePlotDetailsForDatsSeries(ChartTypes chartType,DataSeries dataSeries)
        {
            Plot plot = null;

            plot = FindPlot(chartType, dataSeries.AxisYType);

            if (plot == null)
            {
                plot = CreatePlot(chartType, dataSeries.AxisYType);
                PlotDetails.Plots.Add(plot);
            }

            plot.DataSeries.Add(dataSeries);

            dataSeries.Index = FindDataSeriesCountByChartType(chartType) - 1;
            dataSeries.Plot = plot;
            
        }

        private Int32 GetAxisCount(ChartTypes chartType)
        {
            Int32 count = 0;
            Boolean primaryAxisUsed = false;
            Boolean secondaryAxisUsed = false;

            foreach (DataSeries dataSeries in DataSeries)
            {
                if (dataSeries.RenderAs.ToLower() == chartType.ToString().ToLower())
                {
                    if (dataSeries.AxisYType == AxisType.Primary)
                        primaryAxisUsed = true;
                    else if (dataSeries.AxisYType == AxisType.Secondary)
                        secondaryAxisUsed = true;
                }
            }

            if (primaryAxisUsed) count++;
            if (secondaryAxisUsed) count++;

            return count;
        }

        /// <summary>
        /// Creates summary out of all DataSeries present.
        /// </summary>
        private void GeneratePlotDetails()
        {
            PlotDetails.TotalNoOfSeries = DataSeries.Count;

            foreach (DataSeries dataSeries in DataSeries)
            {

                switch (dataSeries.RenderAs.ToUpper())
                {
                    case "BAR":

                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Bar)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Bar, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Bar;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Bar chart cannot be combined with Column,Pie or Doughnut");
                        break;

                    case "COLUMN":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Column, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Column chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    case "POINT":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Point, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Point chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    case "BUBBLE":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Bubble, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Bubble chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    case "STACKEDCOLUMN":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.StackedColumn, dataSeries);

                            if (GetAxisCount(ChartTypes.StackedColumn) > 1)
                                dataSeries.Index = (dataSeries.Plot.AxisY.AxisType == AxisType.Primary) ? 0 : 1;
                            else
                                dataSeries.Index = 0;

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Stacked column chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;
                    case "STACKEDBAR":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Bar)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.StackedBar, dataSeries);

                            if (GetAxisCount(ChartTypes.StackedBar) > 1)
                                dataSeries.Index = (dataSeries.Plot.AxisY.AxisType == AxisType.Primary) ? 1 : 0;
                            else
                                dataSeries.Index = 0;

                            PlotDetails.AxisOrientation = AxisOrientation.Bar;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Stacked bar chart cannot be combined with Column chart, Pie or Doughnut chart");
                        break;

                    case "STACKEDBAR100":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Bar)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.StackedBar100, dataSeries);

                            if (GetAxisCount(ChartTypes.StackedBar100) > 1)
                                dataSeries.Index = (dataSeries.Plot.AxisY.AxisType == AxisType.Primary) ? 1 : 0;
                            else
                                dataSeries.Index = 0;

                            PlotDetails.AxisOrientation = AxisOrientation.Bar;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. 100% Stacked bar chart cannot be combined with Column chart, Pie or Doughnut chart");
                        break;

                    case "STACKEDCOLUMN100":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.StackedColumn100, dataSeries);

                            if (GetAxisCount(ChartTypes.StackedColumn100) > 1)
                                dataSeries.Index = (dataSeries.Plot.AxisY.AxisType == AxisType.Primary) ? 0 : 1;
                            else
                                dataSeries.Index = 0;

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. 100% Stacked column chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    case "PIE":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Pie, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Pie;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Pie chart cannot be combined with Bar chart, Column or Doughnut chart");
                        break;

                    case "DOUGHNUT":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Doughnut, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Pie;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Doughnut chart cannot be combined with any other chart type");
                        break;

                    case "LINE":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Line, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Line chart cannot be combined with any other chart type");
                        break;

                    case "AREA":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.Area, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Area chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    case "STACKEDAREA":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.StackedArea, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. Stacked area chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    case "STACKEDAREA100":
                        if (PlotDetails.AxisOrientation == AxisOrientation.Undefined || PlotDetails.AxisOrientation == AxisOrientation.Column)
                        {
                            GeneratePlotDetailsForDatsSeries(ChartTypes.StackedArea100, dataSeries);

                            PlotDetails.AxisOrientation = AxisOrientation.Column;
                        }
                        else
                            throw new Exception("Wrong Chart Combination. 100% Stacked area chart cannot be combined with Bar chart, Pie or Doughnut chart");
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine("Unknown Chart Type");
                        throw new Exception("Unknown Chart Type");
                }
            }
            
            foreach (Plot plot in PlotDetails.Plots)
            {
                Int32 siblingCount = CalculateSiblingCountByChartType(plot.ChartType);

                SetSiblingCountByChartType(plot.ChartType, siblingCount);

                foreach (DataSeries dataSeries in plot.DataSeries)
                {
                    dataSeries.MinDifference = plot.MinDifference;
                }
            }


            if (PlotDetails.AxisOrientation == AxisOrientation.Bar)
                TotalSiblings = Math.Max(CalculateSiblingCountByChartType(ChartTypes.Bar),Math.Max(GetPlotCountByChartType(ChartTypes.StackedBar),GetPlotCountByChartType(ChartTypes.StackedBar100)));
            else if (PlotDetails.AxisOrientation == AxisOrientation.Column)
                TotalSiblings = Math.Max(CalculateSiblingCountByChartType(ChartTypes.Column), Math.Max(GetPlotCountByChartType(ChartTypes.StackedColumn), GetPlotCountByChartType(ChartTypes.StackedColumn100)));
            else
                TotalSiblings = 0;
            
        }

        internal Int32 CalculateSiblingCountByChartType(ChartTypes chartType)
        {
            Int32 count = 0;
            foreach (Plot plot in PlotDetails.Plots)
            {
                if (plot.ChartType == chartType)
                    count += plot.DataSeries.Count;
            }

            return count;
        }

        private void SetSiblingCountByChartType(ChartTypes chartType, Int32 count)
        {
            foreach (Plot plot in PlotDetails.Plots)
            {
                if (plot.ChartType == chartType)
                {
                    foreach (DataSeries dataSeries in plot.DataSeries)
                    {
                        dataSeries.TotalSiblings = count;
                    }
                }
            }
        }

        /// <summary>
        /// Since each plot contains dataseries of same chart type and same axis
        /// we get  more than one plot for a chart type when a dataseries points to more than one axis
        /// </summary>
        /// <returns></returns>
        internal Int32 GetPlotCountByChartType(ChartTypes chartType)
        {
            Int32 count = 0;

            foreach (Plot plot in PlotDetails.Plots)
            {
                if (plot.ChartType == chartType)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Generates default Colors sets
        /// </summary>
        /// <returns>number of color sets created</returns>
        private Int32 GenerateDefaultColorSets()
        {
            Dictionary<String, List<Brush>> defaultColorSetList = ColorSetDefaultList.GenerateColorSetDefaultList();
            Dictionary<String, List<Brush>>.Enumerator enumerator = defaultColorSetList.GetEnumerator();

            for (Int32 i = 0; i < defaultColorSetList.Count; i++)
            {
                enumerator.MoveNext();
                ColorSet cl = new ColorSet();
                cl.SetValue(NameProperty, enumerator.Current.Key);
                foreach (Brush brush in enumerator.Current.Value)
                {
                    cl.Children.Add(new Color(brush));
                }
                _colorSets.Add(cl);
            }
            enumerator.Dispose();
            return _colorSets.Count;

        }

        

        private Boolean IsChartTypePresent(ChartTypes chartType)
        {
            foreach (Plot plot in PlotDetails.Plots)
            {
                if (plot.ChartType == chartType)
                    return true;
            }

            return false;
        }

        private Int32 CompareIndex(KeyValuePair<DataSeries, Int32> entry1, KeyValuePair<DataSeries, Int32> entry2)
        {
            return entry1.Value.CompareTo(entry2.Value);
        }

        private Int32 GetHighestIndexForChartType(ChartTypes chartType, List<KeyValuePair<DataSeries, Int32>> seriesIndex)
        {
            Int32 index = Int32.MinValue;
            seriesIndex.ForEach(delegate(KeyValuePair<DataSeries, Int32> entry)
            {
                if (entry.Key.Plot.ChartType == chartType)
                    index = Math.Max(index, entry.Value);
            });
            return index;
        }

        private Int32 GetHighestIndexForChartType(ChartTypes chartType,AxisType axisType, List<KeyValuePair<DataSeries, Int32>> seriesIndex)
        {
            Int32 index = Int32.MinValue;
            seriesIndex.ForEach(delegate(KeyValuePair<DataSeries, Int32> entry)
            {
                if (entry.Key.Plot.ChartType == chartType && entry.Key.AxisYType == axisType)
                    index = Math.Max(index, entry.Value);
            });
            return index;
        }

        private List<KeyValuePair<DataSeries, Int32>> SetIndexForChartType(ChartTypes chartType, List<KeyValuePair<DataSeries, Int32>> seriesIndex, Int32 index)
        {
            for (Int32 i = 0; i < seriesIndex.Count; i++)
            {
                if (seriesIndex[i].Key.Plot.ChartType == chartType)
                {
                    seriesIndex[i] = new KeyValuePair<DataSeries, int>(seriesIndex[i].Key, index);
                }
            }
            return seriesIndex;
        }

        private List<KeyValuePair<DataSeries, Int32>> SetIndexForChartType(ChartTypes chartType,AxisType axisType, List<KeyValuePair<DataSeries, Int32>> seriesIndex, Int32 index)
        {
            for (Int32 i = 0; i < seriesIndex.Count; i++)
            {
                if (seriesIndex[i].Key.Plot.ChartType == chartType && seriesIndex[i].Key.AxisYType == axisType)
                {
                    seriesIndex[i] = new KeyValuePair<DataSeries, int>(seriesIndex[i].Key, index);
                }
            }
            return seriesIndex;
        }

        private Boolean SetDrawingIndexByChartType(ChartTypes chartType, ref List<KeyValuePair<DataSeries, Int32>> seriesIndex, Int32 index)
        {
            Boolean found = false;
            for (Int32 i = 0; i < seriesIndex.Count; i++)
            {
                if (seriesIndex[i].Key.Plot.ChartType == chartType)
                {
                    seriesIndex[i] = new KeyValuePair<DataSeries, int>(seriesIndex[i].Key, index);
                    found = true;
                }
            }
            return found;
        }

        private Boolean SetDrawingIndexByChartType(ChartTypes chartType, AxisType axisType, ref List<KeyValuePair<DataSeries, Int32>> seriesIndex, Int32 index)
        {
            Boolean found = false;
            for (Int32 i = 0; i < seriesIndex.Count; i++)
            {
                if (seriesIndex[i].Key.Plot.ChartType == chartType && seriesIndex[i].Key.AxisYType == axisType)
                {
                    seriesIndex[i] = new KeyValuePair<DataSeries, int>(seriesIndex[i].Key, index);
                    found = true;
                }
            }
            return found;
        }

        private List<KeyValuePair<DataSeries, Int32>> SetHighestIndexToAllSeriesByChartType(ChartTypes chartType, List<KeyValuePair<DataSeries, Int32>> seriesIndex)
        {
            Int32 index = GetHighestIndexForChartType(chartType,seriesIndex);
            seriesIndex = SetIndexForChartType(chartType,seriesIndex,index);
            return seriesIndex;
        }

        private List<KeyValuePair<DataSeries, Int32>> SetHighestIndexToAllSeriesByChartType(ChartTypes chartType,AxisType axisType, List<KeyValuePair<DataSeries, Int32>> seriesIndex)
        {
            Int32 index = GetHighestIndexForChartType(chartType,axisType, seriesIndex);
            seriesIndex = SetIndexForChartType(chartType,axisType, seriesIndex, index);
            return seriesIndex;
        }

        private Int32 GetLowestIndexFromList(List<KeyValuePair<DataSeries, Int32>> seriesIndex)
        {
            Int32 index = Int32.MaxValue;
            foreach (KeyValuePair<DataSeries, Int32> entry in seriesIndex)
            {
                index = Math.Min(index, entry.Value);
            }
            return index;
        }

        private void GenerateIndexTable()
        {
            Int32 depthCount = 0;

            depthCount += IsChartTypePresent(ChartTypes.Column) ? 1 : 0;
            depthCount += IsChartTypePresent(ChartTypes.StackedColumn) ? 1 : 0;
            depthCount += IsChartTypePresent(ChartTypes.StackedColumn100) ? 1 : 0;
            depthCount += IsChartTypePresent(ChartTypes.Bar) ? 1 : 0;
            depthCount += IsChartTypePresent(ChartTypes.StackedBar) ? 1 : 0;
            depthCount += IsChartTypePresent(ChartTypes.StackedBar100) ? 1 : 0;

            depthCount += CalculateSiblingCountByChartType(ChartTypes.Area);
            depthCount += GetPlotCountByChartType(ChartTypes.StackedArea);
            depthCount += GetPlotCountByChartType(ChartTypes.StackedArea100);

            Count = depthCount;

            List<KeyValuePair<DataSeries, Int32>> seriesIndex = new List<KeyValuePair<DataSeries, Int32>>();

            foreach (DataSeries dataSeries in DataSeries)
            {
                seriesIndex.Add(new KeyValuePair<DataSeries, Int32>(dataSeries, dataSeries.ZIndex));
            }

            seriesIndex.Sort(CompareIndex);

            
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.Column, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedColumn, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedColumn100, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.Bar, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedBar, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedBar100, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedArea,AxisType.Primary, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedArea,AxisType.Secondary, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedArea100,AxisType.Primary, seriesIndex);
            seriesIndex = SetHighestIndexToAllSeriesByChartType(ChartTypes.StackedArea100,AxisType.Secondary, seriesIndex);

            seriesIndex.Sort(CompareIndex);

            //seriesIndex = SetIndexForChartType(ChartTypes.Line, seriesIndex, 0);
            //seriesIndex = SetIndexForChartType(ChartTypes.Bubble, seriesIndex, 0);
            //seriesIndex = SetIndexForChartType(ChartTypes.Point, seriesIndex, 0);

            Int32 depth = 0;
            List<KeyValuePair<DataSeries, Int32>> seriesIndex2 = new List<KeyValuePair<DataSeries, Int32>>();
            List<KeyValuePair<DataSeries, Int32>> seriesIndexFinal = new List<KeyValuePair<DataSeries, Int32>>();
            Int32 lowestIndex;
            Boolean ignoreIncrement = false;
            while (seriesIndex.Count > 0)
            {
                ignoreIncrement = false;
                lowestIndex = GetLowestIndexFromList(seriesIndex);

                for (Int32 i = 0; i < seriesIndex.Count; i++)
                {
                    if (seriesIndex[i].Value == lowestIndex)
                    {
                        seriesIndexFinal.Add(new KeyValuePair<DataSeries, Int32>(seriesIndex[i].Key, depth));
                        if (seriesIndex[i].Key.Plot.ChartType == ChartTypes.Line || seriesIndex[i].Key.Plot.ChartType == ChartTypes.Point
                            || seriesIndex[i].Key.Plot.ChartType == ChartTypes.Bubble)
                            ignoreIncrement = true;
                    }
                    else
                        seriesIndex2.Add(new KeyValuePair<DataSeries, Int32>(seriesIndex[i].Key, seriesIndex[i].Value));
                }

                seriesIndex.Clear();
                seriesIndex = seriesIndex2;

                seriesIndex2 = new List<KeyValuePair<DataSeries, Int32>>();
                
                if(!ignoreIncrement) depth++;
            }

            foreach (KeyValuePair<DataSeries, Int32> entry in seriesIndexFinal)
            {
                IndexList.Add(entry.Key.Name, entry.Value);
            }

        }

        private void SetTag(FrameworkElement element)
        {
            if(element != null)
                element.Tag = this.Name;
        }

        private void SetTag(FrameworkElement[] elements)
        {
            foreach(FrameworkElement element in elements)
            {
                SetTag(element);
            }
        }

        private void SetTags()
        {
            SetTag(_plotAreaBorder);

            SetTag(_watermark);

            SetTag(Surface3D);
            SetTag(_areaLabelMarker);
        }

        private void InitialCommonInitSteps()
        {
            SetName();

            // This is done to apply background from themes
            if (GetFromTheme("Background") != null && GetCurrentBackground() == null)
                Background = GetFromTheme("Background") as Brush;

            if (Double.IsNaN(this.Width))
            {
                if (Double.IsNaN(((FrameworkElement)Parent).Width))
                    throw new Exception("Specify the width of the Chart element.");
                else
                    this.Width = ((FrameworkElement)Parent).Width;

            }

            if (Double.IsNaN(this.Height))
            {
                if (Double.IsNaN(((FrameworkElement)Parent).Height))
                    throw new Exception("Specify the height of the Chart element.");
                else
                    this.Height = ((FrameworkElement)Parent).Height;
            }

            ApplyEffects();

            _plankDrawState.Initialize();

            if (BorderColor == null)
            {
                BorderColor = Parser.GetDefaultBorderColor(Parser.GetBrushIntensity(Background));
            }

            this.ApplyBorder();

            CreateReferences();

            if (DataSeries.Count == 0)
            {
                DataSeries ds = new DataSeries();
                this.DataSeries.Add(ds);
                this.Children.Add(ds);
            }

            ToolTip.Init();

            // Initialize inner bounds rectangle
            _innerTitleBounds = new Rect(Padding, Padding, Width - 2 * Padding, Height - 2 * Padding);

            // Initializes all titles
            _titles.ForEach(delegate(Title child) { child.Init(); });

            // Initializes all legends
            _legends.ForEach(delegate(Legend child) { child.Init(); });

            // Initalizes DataSeries and sets the default legend
            foreach (DataSeries child in _dataSeries)
            {
                if (String.IsNullOrEmpty(child.Legend) && _legends.Count != 0)
                    child.Legend = _legends[0].Name;
                child.Init();
            }

            GeneratePlotDetails();

            GenerateIndexTable();


            foreach (Legend legend in _legends)
            {
                foreach (DataSeries ds in _dataSeries)
                {
                    if (legend.Name == ds.Legend)
                        legend.SeriesCount += 1;
                }

                // if no DataSeries Points to the Legend then Disable the legend
                if (legend.SeriesCount == 0)
                    legend.Enabled = false;
            }


            // Remove all disabled Legends
            List<Legend> newLegendSet = new List<Legend>();
            _legends.ForEach(delegate(Legend child)
            {
                if (child.Enabled)
                    newLegendSet.Add(child);
            });
            _legends.Clear();
            _legends = newLegendSet;


            PlotArea.Init();

            if (_logo != null)
                _logo.Init();

            if (PlotDetails.AxisOrientation != AxisOrientation.Pie)
            {

                AxisX.Init();

                AxisYPrimary.Init();

                if (AxisYSecondary != null)
                    AxisYSecondary.Init();
            }

            // Positions titles that have to be placed outside PlotArea
            _titles.ForEach(delegate(Title child) { child.PlaceOutsidePlotArea(); });

            // Copy titles inner bounds to inner bounds to be used by legend
            if (Padding <= 4 && !_paddingOverride)
            {
                _innerBounds = new Rect(_innerTitleBounds.X + 5, _innerTitleBounds.Y + 10, _innerTitleBounds.Width - 10, _innerTitleBounds.Height - 10);
                Padding = 5;
             }
            else
            {
                _innerBounds = new Rect(_innerTitleBounds.X, _innerTitleBounds.Y, _innerTitleBounds.Width, _innerTitleBounds.Height);
            }


            Rect previousBounds = new Rect();
            foreach (Legend legend in _legends)
            {
                previousBounds = _innerBounds;
                legend.PlaceOutsidePlotArea(_innerTitleBounds, Padding);
                if (!legend.EntriesPresent)
                    _innerBounds = previousBounds;
            }

        }

        private void RemoveUnMarkedLegends()
        {
            // Remove all unmarked legends
            List<Legend> newLegendSet = new List<Legend>();
            newLegendSet = new List<Legend>();
            _legends.ForEach(delegate(Legend child)
            {

                if (child.EntriesPresent)
                    newLegendSet.Add(child);
                else
                    Children.Remove(child);

            });
            _legends.Clear();
            _legends = newLegendSet;
        }

        #endregion Private Methods

        #region Internal Methods

        internal void CollectStackContent(Double xValue, Double yValue)
        {
            // here point.X contains Sum
            // here poin.Y contains Absolute sum
            if (_stackTotals.ContainsKey(xValue))
                _stackTotals[xValue] = new Point(_stackTotals[xValue].X + yValue, _stackTotals[xValue].Y + Math.Abs(yValue));
            else
                _stackTotals.Add(xValue, new Point(yValue, Math.Abs(yValue)));

        }

        internal String GetNewObjectName(Object o)
        {
            Int32 i = 0;
            if (o == null) return "";
            String type = o.GetType().Name;
            String name = type;

            // Check for an available name
            while (FindName(name + i.ToString()) != null)
            {
                i++;
            }

            name += i.ToString();

            return name;
        
        }

        #endregion Internal Methods

        #region Data
        private Int32 _storyboardEndCounter = 0;

        private Double _labelPaddingTop;
        private Double _labelPaddingBottom;
        private Double _labelPaddingRight;
        private Double _labelPaddingLeft;

        private Double _animationDuration;

        internal Dictionary<String, Int32> ChartCounts = new Dictionary<String, Int32>();
        internal Dictionary<String, Int32> IndexList = new Dictionary<String, Int32>();
        internal Dictionary<Double, Point> _stackTotals = new Dictionary<Double, Point>();
        
        internal List<Canvas> AreaLine3D = new List<Canvas>();
        private List<Storyboard> animation = new List<Storyboard>();
        private List<DataSeries> _dataSeries;
        private List<Title> _titles;
        private List<Legend> _legends;
        private List<TrendLine> _trendLines;
        private List<ColorSet> _colorSets;

        internal Boolean[] _plankDrawState = new Boolean[12];

        private Canvas _parent;
        internal Canvas[] Surface3D = new Canvas[12];
        internal Canvas _areaLabelMarker = null;

        private PlotDetails _plotDetails;
        private PlotArea _plotArea;
        private AxisX _axisX;
        private AxisY _axisYPrimary;
        private AxisY _axisYSecondary;
        private ToolTip _toolTip;
        private Label _label;
        private Image _logo;

        internal Rect _innerBounds;
        internal Rect _innerTitleBounds;

        private String _animationType;
        private String _colorSet;
        private String _uniqueColors;
        private String _view3D;
        private String _animationEnabled;

        internal Rectangle _plotAreaBorder;

        private TextBlock _watermark;
        private Boolean _chartHitTestState;

        #endregion Data
    }
}
