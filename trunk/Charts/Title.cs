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
using System.Windows.Browser;
using Visifire.Commons;
using System.Collections.Generic;

namespace Visifire.Charts
{
    public class Title : TitleBase
    {
        #region Public Methods

        public Title()
        {
            
        }

        public override void SetWidth()
        {
            // Width of the Title will be dependent on the _textBlock's height if _textBlock is rotated by 90 degree.
            // _textBlock is rotated by 90 Degree if its placed to the left or right of the PlotArea.
            if (FullSize)
            {
                if (!DockInsidePlotArea && AlignmentY == AlignmentY.Center && (AlignmentX == AlignmentX.Left || AlignmentX == AlignmentX.Right))
                {
                    this.Width = _textBlock.ActualHeight + Padding * 2;
                }
                else if (!DockInsidePlotArea && AlignmentY != AlignmentY.Center)
                {
                    this.Width = _parent.Width - _parent.BorderThickness * 2;
                }
                else if (DockInsidePlotArea)
                {
                    this.Width = _parent.PlotArea.Width - _parent.PlotArea.BorderThickness * 2;
                }

                if (AlignmentY != AlignmentY.Center)
                {
                    switch (AlignmentX)
                    {
                        case AlignmentX.Right:
                            _textBlock.SetValue(LeftProperty, (Double) ( Width - _textBlock.ActualWidth - Padding));
                            break;
                        case AlignmentX.Left:
                            _textBlock.SetValue(LeftProperty, (Double) Padding);
                            break;
                        case AlignmentX.Center:
                            _textBlock.SetValue(LeftProperty, (Double) ( Width / 2 - _textBlock.ActualWidth / 2 + Padding));
                            break;
                    }
                }

            }
            else
            {
                if (!DockInsidePlotArea && (AlignmentX == AlignmentX.Left || AlignmentX == AlignmentX.Right) && AlignmentY == AlignmentY.Center)
                {

                    this.SetValue(WidthProperty, _textBlock.ActualHeight + Padding * 2);

                }
                else
                {
                    this.SetValue(WidthProperty, _textBlock.ActualWidth + Padding * 2);
                }
            }
        }

        public override void SetHeight()
        {
            // Height of the Title will be dependent on the _textBlock's width if _textBlock is rotated by 90 degree.
            // _textBlock is rotated by 90 Degree if its placed to the left or right of the PlotArea.
            if (FullSize)
            {
                if (!DockInsidePlotArea && AlignmentY == AlignmentY.Center && (AlignmentX == AlignmentX.Left || AlignmentX == AlignmentX.Right))
                {
                    this.Height = _parent.Height - _parent.BorderThickness * 2;
                }
                else if (!DockInsidePlotArea && AlignmentY != AlignmentY.Center)
                {
                    this.Height = _textBlock.ActualHeight + Padding * 2;
                }
                else if (DockInsidePlotArea)
                {
                    this.Height = _textBlock.ActualHeight + Padding * 2;
                }

                if (AlignmentY == AlignmentY.Center && AlignmentX == AlignmentX.Left)
                {
                    _textBlock.SetValue(TopProperty, (Double) ( Height / 2 + _textBlock.ActualWidth / 2));
                }
                else if (AlignmentY == AlignmentY.Center && AlignmentX == AlignmentX.Right)
                {
                    _textBlock.SetValue(TopProperty, (Double) ( Height / 2 - _textBlock.ActualWidth / 2));
                }
            }
            else
            {

                if (!DockInsidePlotArea && (AlignmentX == AlignmentX.Left || AlignmentX == AlignmentX.Right) && AlignmentY == AlignmentY.Center)
                {
                    this.SetValue(HeightProperty, _textBlock.ActualWidth + Padding * 2);
                }
                else
                {
                    this.SetValue(HeightProperty, _textBlock.ActualHeight + Padding * 2);
                }
            }
        }

        public Brush GetDefaultFontColor()
        {
            Brush tempBrush = null;

            if (Background == null)
            {
                if (_parent.Background != null && !DockInsidePlotArea) tempBrush = _parent.Background;
                else if (_parent.PlotArea.Background != null && DockInsidePlotArea) tempBrush = _parent.PlotArea.Background;
                else tempBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                tempBrush = Background;
            }

            Double intensity = Parser.GetBrushIntensity(tempBrush);

            return Parser.GetDefaultFontColor(intensity);
        }

        public override void Init()
        {
            // Validate the parent
            ValidateParent();

            if (DockInsidePlotArea) WrapSize = _parent.PlotArea.Width;
            else
            {
                if (AlignmentX != AlignmentX.Center && AlignmentY == AlignmentY.Center)
                    WrapSize = _parent.Height;
                else
                    WrapSize = _parent.Width;
            }

            // Set a default name for the Title
            SetName();

            base.Init();

            AttachToolTip();
            AttachHref();

            // if font size is not given Calculate font size and put the text
            if (Double.IsNaN(_fontSize))
            {
                _textBlock.FontSize = CalculateFontSize();
                SetWidth();
                SetHeight();
            }

            // if full size is selected the set background color as parent border color
            if (FullSize)
            {
                if (this.Background == null)
                {
                    if (!DockInsidePlotArea)
                    {
                        this.Background = (_parent.BorderColor);
                    }
                    else
                    {
                        this.Background = (_parent.PlotArea.BorderColor);
                    }
                }
            }

            // if font color is not set then set font color
            if (_fontColor == null)
            {
                _textBlock.Foreground = GetDefaultFontColor();

            }
            else
            {
                _textBlock.Foreground = (_fontColor);
            }

            if (BorderColor == null)
            {
                if (DockInsidePlotArea && _parent.PlotArea.Background == null)
                {
                    if (_parent.Background == null)
                        BorderColor = new SolidColorBrush(Colors.Black);
                    else
                    {
                        if (Parser.GetBrushIntensity(_parent.Background) > 0.5)
                        {
                            BorderColor = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            BorderColor = new SolidColorBrush(Colors.LightGray);
                        }
                    }

                }
                else if (DockInsidePlotArea)
                {
                    if (Parser.GetBrushIntensity(_parent.PlotArea.Background) > 0.5)
                    {
                        BorderColor = new SolidColorBrush(Colors.Black);
                    }
                    else
                    {
                        BorderColor = new SolidColorBrush(Colors.LightGray);
                    }
                }
                else
                {
                    if (_parent.Background == null)
                        BorderColor = new SolidColorBrush(Colors.Black);
                    else
                    {
                        if (Parser.GetBrushIntensity(_parent.Background) > 0.5)
                        {
                            BorderColor = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            BorderColor = new SolidColorBrush(Colors.LightGray);
                        }
                    }
                }
            }
        }

        public override void Render()
        {
            base.Render();
        }

        /// <summary>
        /// Positions Title by checking whether other elements are present to its left.
        /// </summary>
        public override void SetLeft()
        {

            Double left = 0;
            Boolean flag = false;

            // As there can be multiple Title elements, place Title by looking at other Title elements.
            // Other title elements to be considered only if DockInsidePlotArea is false. This is because,
            // Titles are placed Vertically only when AlignmentY is Center. Otherwise Left property is
            // independent of each other.
            if (!DockInsidePlotArea)
            {
                if (AlignmentX == AlignmentX.Right && AlignmentY == AlignmentY.Center)
                {
                    left = _parent.Width - _parent.BorderThickness;
                    foreach (Title child in _parent.Titles)
                    {
                        if (child == this) break;
                        if (AlignmentX == child.AlignmentX && AlignmentY == child.AlignmentY && !child.DockInsidePlotArea)
                        {
                            if (left > (Double)child.GetValue(LeftProperty))
                                left = (Double)child.GetValue(LeftProperty);
                        }
                    }
                    left -= this.Width + _parent.Padding;
                    if (!DockInsidePlotArea && (left < _parent._innerTitleBounds.Right)) _parent._innerTitleBounds.Width = left - _parent._innerTitleBounds.Left;

                }
                else if (AlignmentX == AlignmentX.Left && AlignmentY == AlignmentY.Center)
                {
                    left = _parent.BorderThickness;
                    foreach (Title child in _parent.Titles)
                    {
                        if (child == this) break;
                        if (AlignmentX == child.AlignmentX && AlignmentY == child.AlignmentY && !child.DockInsidePlotArea)
                        {
                            flag = true;
                            if (left < (Double)child.GetValue(LeftProperty) + (Double)child.Width)
                                left = (Double)child.GetValue(LeftProperty) + (Double)child.Width;
                        }
                    }
                    if (!flag) left += _parent.Padding;
                    if (!DockInsidePlotArea && (left + Width > _parent._innerTitleBounds.Left))
                    {
                        _parent._innerTitleBounds.X = left + Width;
                        _parent._innerTitleBounds.Width -= Width;
                    }
                }
                else
                    left = _parent.Padding + _parent.BorderThickness + (_parent.Width - this.Width - 2 * _parent.Padding - 2 * _parent.BorderThickness) * GetRelativePosX(AlignmentX);
            }
            else
            {
                left = (Double)_parent.PlotArea.GetValue(LeftProperty) + _parent.Padding + (_parent.PlotArea.Width - this.Width - 2 * _parent.Padding) * GetRelativePosX(AlignmentX);
            }

            if (!FullSize)
            {
                if (!DockInsidePlotArea && AlignmentX == AlignmentX.Left && AlignmentY == AlignmentY.Center)
                {
                    _textBlock.SetValue(TopProperty, (Double) _textBlock.ActualWidth);
                    _textBlock.SetValue(LeftProperty, (Double)  0);
                }
                else if (!DockInsidePlotArea && AlignmentX == AlignmentX.Right && AlignmentY == AlignmentY.Center)
                {
                    _textBlock.SetValue(TopProperty, (Double)0);
                    _textBlock.SetValue(LeftProperty, (Double) _textBlock.ActualHeight);
                }
            }
            this.SetValue(Canvas.LeftProperty, left);
        }

        /// <summary>
        /// Position Title depending on the position of other Title Elements.
        /// </summary>
        public override void SetTop()
        {
            Double top = 0;
            Double tempTop = _parent.Padding;

            Double childHeight = 0;

            if (!DockInsidePlotArea)
            {
                top = _parent.Padding + _parent.BorderThickness + (_parent.Height - this.Height - _parent.Padding - _parent.BorderThickness) * GetRelativePosY(AlignmentY);
            }
            else
            {
                top = (Double)_parent.PlotArea.GetValue(TopProperty) + _parent.Padding + (_parent.PlotArea.Height - this.Height - _parent.Padding) * GetRelativePosY(AlignmentY);
            }
            switch (AlignmentY)
            {   
                case AlignmentY.Top:

                    foreach (Title child in _parent.Titles)
                    {
                        if (child == this) break;
                        if (child.AlignmentY == this.AlignmentY && this.DockInsidePlotArea == child.DockInsidePlotArea)
                        {

                            if (CheckHorizontalOverlap(this, child))
                            {
                                if (tempTop <= (Double)child.GetValue(TopProperty))
                                {
                                    tempTop = (Double)child.GetValue(TopProperty);

                                    childHeight = child.Height;
                                }
                            }
                        }
                    }

                    tempTop += childHeight;

                    if (tempTop > top) top = tempTop;
                    this.SetValue(Canvas.TopProperty, top);
                    // Set the inner bounds
                    if (!DockInsidePlotArea && (top + Height > _parent._innerTitleBounds.Top))
                    {
                        _parent._innerTitleBounds.Y = top + Height;
                        _parent._innerTitleBounds.Height -= Height;
                    }

                    break;
                case AlignmentY.Center:
                    if ((AlignmentX == AlignmentX.Right || AlignmentX == AlignmentX.Left) && !DockInsidePlotArea)
                    {
                        top = (_parent.Height - this.Height) / 2;
                    }
                    else
                    {
                        foreach (Title child in _parent.Titles)
                        {
                            if (child == this) break;
                            if (child.AlignmentY == this.AlignmentY)
                            {

                                if (CheckHorizontalOverlap(this, child))
                                {
                                    if (tempTop < (Double)child.GetValue(TopProperty))
                                    {
                                        tempTop = (Double)child.GetValue(TopProperty);
                                        childHeight = child.Height;
                                    }
                                }
                            }
                        }

                        tempTop += childHeight;
                        if (tempTop > top) top = tempTop;

                    }
                    this.SetValue(TopProperty, (Double) top);
                    break;
                case AlignmentY.Bottom:
                    if (DockInsidePlotArea)
                        tempTop = (Double)_parent.PlotArea.GetValue(TopProperty) + (Double)_parent.PlotArea.GetValue(HeightProperty) - _parent.Padding;
                    else
                        tempTop = _parent.Height - _parent.Padding - _parent.BorderThickness;
                    childHeight = 0;
                    foreach (Title child in _parent.Titles)
                    {
                        if (child == this) break;
                        if (child.AlignmentY == this.AlignmentY && this.DockInsidePlotArea == child.DockInsidePlotArea)
                        {

                            if (CheckHorizontalOverlap(this, child))
                            {
                                if (tempTop >= (Double)child.GetValue(TopProperty))
                                {
                                    tempTop = (Double)child.GetValue(TopProperty);

                                    childHeight = child.Height;
                                }
                            }
                        }
                    }
                    tempTop -= this.Height;
                    top = tempTop;
                    this.SetValue(Canvas.TopProperty, top);
                    if (!DockInsidePlotArea && (top < _parent._innerTitleBounds.Bottom)) _parent._innerTitleBounds.Height = top - _parent._innerTitleBounds.Top;
                    break;

            }

        }
        #endregion Public Methods

        #region Static Methods
        /// <summary>
        /// Calculates the inner region using the size data for only those title which have 
        /// DockInsidePlotArea = false
        /// </summary>

        public static Rect GetTitleRegion(List<Title> titles)
        {
            Rect rect = new Rect(); // Variable to store the inner region

            // Initialize the variables
            rect.X = 0;
            rect.Y = 0;
            rect.Width = Double.PositiveInfinity;
            rect.Height = Double.PositiveInfinity;

            // Temporary variable to calculate region
            Double temp;

            // Variable that will be used to calculate the bottom most point
            Double bottom = Double.PositiveInfinity;

            // Variable that will be used to calculate the right most point
            Double right = Double.PositiveInfinity;

            // Get each title from the titles collection
            foreach (Title title in titles)
            {   
                // check for docking state
                if (!title.DockInsidePlotArea)
                {
                    if (title.AlignmentY == AlignmentY.Top)
                    {
                        // calculate the top most point for region. This is given by sum of the title top and title height
                        temp = (Double)title.GetValue(TopProperty) + (Double)title.GetValue(HeightProperty);

                        // rect.Y is the top most point for the region and is given by the largest value of temp
                        if (temp > rect.Y)
                            rect.Y = temp;
                    }
                    else if (title.AlignmentY == AlignmentY.Bottom)
                    {
                        // calculate the bottom most point for region. This is given the title top only
                        temp = (Double)title.GetValue(TopProperty);

                        // bottom is the smallest value obtained in the above calculation  
                        if (temp < bottom)
                            bottom = temp;
                    }
                    else
                    {
                        if (title.AlignmentX == AlignmentX.Left)
                        {
                            // calculate the left most point for region. This is given by sum of the title left and title width
                            temp = (Double)title.GetValue(LeftProperty) + (Double)title.GetValue(WidthProperty);

                            // rect.X is the left most point for the region and is given by the largest value of temp
                            if (temp > rect.X)
                                rect.X = temp;
                        }
                        else if (title.AlignmentX == AlignmentX.Right)
                        {
                            // calculate the right most point for region. This is given by the title left only
                            temp = (Double)title.GetValue(LeftProperty);

                            // right is the smallest value obtained in the above calculation 
                            if (temp < right)
                                right = temp;
                        }
                        else
                        {
                            // this is for center and is not reqired
                        }
                    }
                }
            }
            rect.Width = right - rect.X;
            rect.Height = bottom - rect.Y;
            return rect;
        }

        #endregion Static Methods

        #region Public Properties

        #endregion Public Properties

        #region Private Methods
        
        /// <summary>
        /// Gives the horizontal position with respect to its parent on a Scale of 0 - 1 Depending on the AlignmentX property.
        /// </summary>
        /// <param name="alignX">Horizontal Alignment</param>
        /// <returns></returns>
        private Double GetRelativePosX(AlignmentX alignX)
        {
            switch (alignX)
            {
                case AlignmentX.Left: return 0.0;
                case AlignmentX.Right: return 1.0;
                case AlignmentX.Center: return 0.5;
            }
            return 0.0;
        }

        /// <summary>
        /// Gives the vertical position with respect to its parent on a Scale of 0 - 1 Depending on the AlignmentY property.
        /// </summary>
        /// <param name="alignY">Vertical Alignment</param>
        /// <returns></returns>
        private Double GetRelativePosY(AlignmentY alignY)
        {
            switch (alignY)
            {
                case AlignmentY.Bottom: return 1.0;
                case AlignmentY.Top: return 0.0;
                case AlignmentY.Center: return 0.5;
            }
            return 0.0;
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
        /// Checks if the two tiles overalp horizontlly
        /// </summary>
        /// <param name="title1"></param>
        /// <param name="title2"></param>
        /// <returns></returns>
        private Boolean CheckHorizontalOverlap(Title title1, Title title2)
        {
            Rect rect1 = new Rect((Double)title1.GetValue(LeftProperty), (Double)title1.GetValue(TopProperty), title1.Width, title1.Height);
            Rect rect2 = new Rect((Double)title2.GetValue(LeftProperty), (Double)title2.GetValue(TopProperty), title2.Width, title2.Height);

            if (rect1.Right >= rect2.Left && rect1.Right <= rect2.Right) return true;
            if (rect1.Left >= rect2.Left && rect1.Left <= rect2.Right) return true;
            if (rect1.Left >= rect2.Left && rect1.Right <= rect2.Right) return true;
            if (rect1.Left <= rect2.Left && rect1.Right >= rect2.Right) return true;
            return false;
        }

        #endregion Private Methods

        #region Internal Methods

        internal void PlaceOutsidePlotArea()
        {
            if (!DockInsidePlotArea)
                PlaceTitle();
        }

        internal void PlaceInsidePlotArea()
        {
            if (DockInsidePlotArea)
                PlaceTitle();
        }

        internal void PlaceTitle()
        {
            SetWidth();
            SetHeight();
            SetLeft();
            SetTop();
        }

        #endregion Internal Methods

        #region Data

        // Parent should always be a Chart
        private Chart _parent;                                  
        
        #endregion Data

    }
}
