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

#if WPF

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
#endif

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.AxisLabel class
    /// </summary>
    internal class AxisLabel
    {
        #region Public Methods
            public AxisLabel()
            {
                MaxWidth = Double.NaN;
            }

        #endregion

        #region Public Properties

            public Double MaxWidth { get; set; }

        #endregion

        #region Public Events

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties
        /// <summary>
        /// Angle of the label has to be between -90 to 90
        /// </summary>
        internal Double Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                if (value >= -90 && value <= 90)
                    _angle = value;
                else if (double.IsNaN(value))
                    _angle = 0;
                else
                {
                    Debug.Assert(false, "Label angle must be between -90 and 90");
                }
            }
        }

        /// <summary>
        /// The point w.r.t which the label will be drawn
        /// </summary>
        internal Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Text content for the label
        /// </summary>
        internal String Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        /// <summary>
        /// Text content for the label
        /// </summary>
        internal Double From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        /// <summary>
        /// Text content for the label
        /// </summary>
        internal Double To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
        }

        /// <summary>
        /// Determines how the label should be placed around the point given in Position property
        /// </summary>
        /// </summary>
        internal PlacementTypes Placement
        {
            get;
            set;
        }

        /// <summary>
        /// Visual label element
        /// </summary>
        internal TextBlock Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the top most point of the axis label canvas
        /// </summary>
        internal Double ActualTop
        {
            get
            {
                return _actualTop;
            }
            private set
            {
                _actualTop = value;
            }
        }

        /// <summary>
        /// Returns the left most point of the axis label canvas
        /// </summary>
        internal Double ActualLeft
        {
            get
            {
                return _actualLeft;
            }
            private set
            {
                _actualLeft = value;
            }
        }

        /// <summary>
        /// Returns the height of the axis label canvas
        /// </summary>
        internal Double ActualHeight
        {
            get
            {
                return _actualHeight;
            }
            set
            {
                _actualHeight = value;
            }
        }

        /// <summary>
        /// Returns the width of the axis label canvas
        /// </summary>
        internal Double ActualWidth
        {
            get
            {
                return _actualWidth;
            }
            set
            {
                _actualWidth = value;
            }
        }

        /// <summary>
        /// Width of the text element
        /// </summary>
        internal Double ActualTextWidth
        {
            get
            {
                return _actualTextWidth;
            }
            private set
            {
                _actualTextWidth = value;
            }
        }

        /// <summary>
        /// Height of the text element
        /// </summary>
        internal Double ActualTextHeight
        {
            get
            {
                return _actualTextHeight;
            }
            private set
            {
                _actualTextHeight = value;
            }
        }

        #region Font Properties

        /// <summary>
        /// TextAlignment of the axis labels
        /// </summary>
        internal TextAlignment TextAlignment
        {
            get;
            set;
        }

        /// <summary>
        /// Font size of the axis labels
        /// </summary>
        internal Double FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Font family of the axis labels
        /// </summary>
        internal FontFamily FontFamily
        {
            get;
            set;
        }

        /// <summary>
        /// Font Color of the axis labels
        /// </summary>
        internal Brush FontColor
        {
            get;
            set;
        }

        /// <summary>
        /// Font Style of the axis labels
        /// </summary>
        internal FontStyle FontStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Font Weight of the axis labels
        /// </summary>
        internal FontWeight FontWeight
        {
            get;
            set;
        }

        #endregion
        #endregion

        #region Private Delegates

        #endregion

        #region Private Properties

        /// <summary>
        /// Visual text element for the label
        /// </summary>
        internal TextBlock TextElement
        {
            get;
            set;
        }

        /// <summary>
        /// To apply rotation to the text element
        /// </summary>
        private RotateTransform Rotation
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set the position of the label based on the angle and the Position Property
        /// </summary>
        internal void SetPosition()
        {
            // Depending on the placement type call the method for positioning of he labels
            switch (Placement)
            {
                case PlacementTypes.Top:
                    // set the position for axis that will be placed to the top of plot area
                    SetPositionTop(Position);
                    break;
                case PlacementTypes.Bottom:
                    // set the position for axis that will be placed to the bottom of plot area
                    SetPositionBottom(Position);
                    break;
                case PlacementTypes.Left:
                    // set the position for axis that will be placed to the left of plot area
                    SetPositionLeft(Position);
                    break;
                case PlacementTypes.Right:
                    // set the position for axis that will be placed to the right of plot area
                    SetPositionRight(Position);
                    break;
            }
        }

        /// <summary>
        /// Set the position for axis that will be placed to the right of plot area
        /// </summary>
        /// <param name="newPos"></param>
        private void SetPositionRight(Point newPos)
        {
            // The angle made by the diagonal (passing from the top left corner to center right) with the horizontal
            Double relativeAngle = Math.Atan(ActualTextHeight / (2 * ActualTextWidth));

            // length of the diagonal (passing from the top left corner to center right)
            Double length = Math.Sqrt(Math.Pow(ActualTextHeight / 2, 2) + Math.Pow(ActualTextWidth, 2));

            // Gets the positive value of the angle
            Double angle = GetAngle(Angle);

            // Set the transform angle
            Rotation.Angle = angle;

            // Set the transform position
            Rotation.CenterX = 0;
            Rotation.CenterY = 0.5;

            // calculate the top and left for the TextBlock 
            // Stores the top and left of the textblock
            Double top = (ActualTextHeight / 2) * Math.Cos(GetRadians(angle));
            Double left = -(ActualTextHeight / 2) * Math.Sin(GetRadians(angle));

            // set the top and left for the AxisLabel element
            Visual.SetValue(Canvas.LeftProperty, newPos.X - left);
            Visual.SetValue(Canvas.TopProperty, newPos.Y - top);
            
            if (angle > 90)
            {
                // If the angle is > 90 this means the title will be slanting downward towards right of the point specified in position
                // Hence the actual left of the axis label element does not change
                ActualTop = (Double)Visual.GetValue(Canvas.TopProperty) - top + length * Math.Sin(GetRadians(angle) + relativeAngle);
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty);
            }
            else
            {
                // If the angle is <= 90 this means the title will be slanting upwards towards right of the point specified in position
                // Hence the actual top of the axis label element does not change
                ActualTop = (Double)Visual.GetValue(Canvas.TopProperty);
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty) + left - (ActualTextHeight / 2) * Math.Cos(GetRadians(90 - angle));
            }
        }

        /// <summary>
        /// Set the position for axis that will be placed to the left of plot area
        /// </summary>
        /// <param name="newPos"></param>
        private void SetPositionLeft(Point newPos)
        {
            // Stores the top and left of the textblock
            Double top;
            Double left;

            // Get the angle in radians
            Double radians = GetRadians(Angle);

            // The angle made by the diagonal (passing from the top left corner to center right) with the horizontal
            Double relativeAngle = Math.Atan(ActualTextHeight / (2 * ActualTextWidth));

            // length of the diagonal (passing from the top left corner to center right)
            Double length = Math.Sqrt(Math.Pow(ActualTextHeight / 2, 2) + Math.Pow(ActualTextWidth, 2));

            // Gets the positive value of the angle
            Double angle = GetAngle(Angle);

            // Set the transform angle
            Rotation.Angle = angle;

            // Set the transform position
            Rotation.CenterX = 0;
            Rotation.CenterY = 0.5;

            // calculate the top and left for the TextBlock
            left = length * Math.Cos(radians + relativeAngle);
            top = length * Math.Sin(radians + relativeAngle);

            // set the top and left for the AxisLabel element
            Visual.SetValue(Canvas.LeftProperty, newPos.X - left);
            Visual.SetValue(Canvas.TopProperty, newPos.Y - top);

            if (angle > 90)
            {
                // If the angle is > 90 this means the title will be slanting upwards towards left of the point specified in position
                // Hence the actual left of the axis label element does not change
                ActualTop = (Double)Visual.GetValue(Canvas.TopProperty) + top - ((ActualTextHeight / 2) * Math.Sin(GetRadians(90 - angle)));
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty);
            }
            else
            {
                // If the angle is <= 90 this means the title will be slanting downwards towards left of the point specified in position
                // Hence the actual top of the axis label element does not change
                ActualTop = (Double)Visual.GetValue(Canvas.TopProperty);
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty) + left - ((ActualTextHeight / 2) * Math.Cos(GetRadians(90 - Angle)));
            }
        }
        /// <summary>
        /// Set the position for axis that will be placed to the bottom of plot area
        /// </summary>
        /// <param name="newPos"></param>
        private void SetPositionBottom(Point newPos)
        {
            // Stores the top and left of the textblock
            Double top;
            Double left;

            // Gets the positive value of the angle
            Double angle = GetAngle(Angle);

            // if the angle is zero then do not apply rotation logic just get the horizontal center and place it
            // using absolute value
            if (angle == 0)
            {
                // Get the horizontal center
                left = ActualTextWidth / 2;

                // set the top to zero
                top = 0;

                // set rotation to zero
                Rotation.Angle = 0;

                // set the top and left for the AxisLabel element
                Visual.SetValue(Canvas.LeftProperty, newPos.X - left);
                Visual.SetValue(Canvas.TopProperty, newPos.Y + top);

                // set the actual Top, left same as the visual top and left
                ActualTop = (Double)Visual.GetValue(Canvas.TopProperty);
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty);
            }
            else if (angle > 90)
            {
                // this is same as placing the axis label to the left
                SetPositionLeft(newPos);
            }
            else
            {
                // this is same as placing the axis label to the right
                SetPositionRight(newPos);
            }

        }

        /// <summary>
        /// Set the position for axis that will be placed to the top of plot area
        /// </summary>
        /// <param name="newPos"></param>
        private void SetPositionTop(Point newPos)
        {
            // Stores the top and left of the textblock
            Double top;
            Double left;

            // Gets the positive value of the angle
            Double angle = GetAngle(Angle);

            // if the angle is zero the do not apply rotation logic just get the horizontal center and place it
            // using absolute value
            if (angle == 0)
            {
                // Get the horizontal center
                left = ActualTextWidth / 2;

                // set the top same as the text height
                top = ActualTextHeight;

                // set rotation to zero
                Rotation.Angle = 0;

                // set the top and left for the AxisLabel element
                Visual.SetValue(Canvas.LeftProperty, newPos.X - left);
                Visual.SetValue(Canvas.TopProperty, newPos.Y - top);

                // set the actual Top, left same as the visual top and left
                ActualTop = (Double)Visual.GetValue(Canvas.TopProperty);
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty);
            }
            else if (angle > 90)
            {
                // this is same as placing the axis label to the right
                SetPositionRight(newPos);
            }
            else
            {
                // this is same as placing the axis label to the left
                SetPositionLeft(newPos);
            }

        }

        /// <summary>
        /// Calculates the ActualHeight and ActialWidth of the AxisLabel
        /// </summary>
        /// <param name="radianAngle"></param>
        private void CalculateSize(Double radianAngle)
        {
            // length of the diagonal from top left to bottom right
            Double length = Math.Sqrt(Math.Pow(ActualTextHeight, 2) + Math.Pow(ActualTextWidth, 2));

            // angle made by the diagonal with respect to the horizontal
            Double beta = Math.Atan(ActualTextHeight / ActualTextWidth);

            // calculate the two possible height and width values using the diagonal length and angle
            Double height1 = length * Math.Sin(radianAngle + beta);
            Double height2 = length * Math.Sin(radianAngle - beta);
            Double width1 = length * Math.Cos(radianAngle + beta);
            Double width2 = length * Math.Cos(radianAngle - beta);

            // Actual height will be the maximum of the two calculated heights
            ActualHeight = Math.Max(Math.Abs(height1), Math.Abs(height2));

            // Actual width will be the maximum of the two calculated widths
            ActualWidth = Math.Max(Math.Abs(width1), Math.Abs(width2));
        }

        /// <summary>
        /// Calculate the visual size of the text block
        /// </summary>
        private void CalculateTextElementSize()
        {
            // Get the visual size of the text block
#if WPF
            if(!TextElement.IsMeasureValid)
                TextElement.Measure(new Size(Double.MaxValue, Double.MaxValue));

            ActualTextHeight = TextElement.DesiredSize.Height;
            ActualTextWidth = TextElement.DesiredSize.Width;
#else       
            ActualTextHeight = TextElement.ActualHeight;
            ActualTextWidth = TextElement.ActualWidth;
#endif
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Applies properties to the TextBlock
        /// </summary>
        /// <param name="axisLabel"></param>
        internal void ApplyProperties(AxisLabel axisLabel)
        {
#if WPF
            TextElement.FlowDirection = FlowDirection.LeftToRight;
#endif
            TextElement.Text = axisLabel.Text;
            TextElement.Foreground = FontColor;
            TextElement.FontSize = FontSize;
            TextElement.LineHeight = FontSize;
            TextElement.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
            
            if(FontFamily != null)
                TextElement.FontFamily = FontFamily;
            TextElement.FontStyle = FontStyle;
            TextElement.FontWeight = FontWeight;
            TextElement.TextAlignment = TextAlignment;

            if (!Double.IsNaN(MaxWidth))
            {
                TextElement.TextWrapping = TextWrapping.Wrap;
#if WPF
                TextElement.MaxWidth = MaxWidth;
#else
                TextElement.Width = MaxWidth;
#endif

            }
        }

        /// <summary>
        /// Create visual for AxisLabel
        /// </summary>
        internal void CreateVisualObject(ElementData tag)
        {
            TextElement = new TextBlock() { Tag = tag };
            Rotation = new RotateTransform();
            TextElement.RenderTransform = Rotation;
            Visual = TextElement;

            ApplyProperties(this);

            CalculateTextElementSize();

            //if (positioningAllowed)
            //    SetPosition();

            // calculate the actual size of the AxisLabel element
            CalculateSize(GetRadians(Angle));
        }

        #endregion

        #region Internal Events

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a positive angle. 
        /// The angle must always be between -90 and 90 
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <returns>Angle as Double</returns>
        internal static Double GetAngle(Double angle)
        {
            return (angle >= 0 ? angle : 360 + angle);
        }

        /// <summary>
        /// Returns the radian for the angle. 
        /// Internal calls angle to get positive angle value
        /// </summary>
        /// <param name="angle">Angle</param>
        /// <returns>Angle as Double</returns>
        internal static Double GetRadians(Double angle)
        {
            return Math.PI / 180 * GetAngle(angle);
        }
        #endregion

        #region Data

        /// <summary>
        /// Identifier for Angle property
        /// </summary>
        private Double _angle;

        /// <summary>
        /// Identifier for Position property
        /// </summary>
        private Point _position;

        /// <summary>
        /// Identifier for Text property
        /// </summary>
        private String _text;

        private Double _from;

        private Double _to;

        /// <summary>
        /// Identifier for ActualLeft property
        /// </summary>
        private Double _actualLeft;

        /// <summary>
        /// Identifier for ActualTop property
        /// </summary>
        private Double _actualTop;

        /// <summary>
        /// Identifier for ActualWidth property
        /// </summary>
        private Double _actualWidth;

        /// <summary>
        /// Identifier for ActualHeight property
        /// </summary>
        private Double _actualHeight;

        /// <summary>
        /// Identifier for ActualTextHeight property
        /// </summary>
        private Double _actualTextHeight;

        /// <summary>
        /// Identifier for ActualTextWidth property
        /// </summary>
        private Double _actualTextWidth;

        #endregion
    }
}
