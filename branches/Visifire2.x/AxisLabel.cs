﻿#if WPF

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;

#else
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.Diagnostics;
#endif
using Visifire.Commons;

namespace Visifire.Charts
{
    internal class AxisLabel
    {
        #region Public Methods
        public AxisLabel()
        {
        }
        #endregion

        #region Public Properties

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
        /// Determines how the label should be placed around the point given in Position property
        /// </summary>
        /// </summary>
        internal PlacementTypes Placement
        {
            get;
            set;
        }

        /// <summary>
        /// Visual lable element
        /// </summary>
        internal Canvas Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the top most point of the Axis label canvas
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
            private set
            {
                _actualHeight = value;
            }
        }

        /// <summary>
        /// Returns the width of the axis labels canvas
        /// </summary>
        internal Double ActualWidth
        {
            get
            {
                return _actualWidth;
            }
            private set
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
        /// Font size of the Axis Labels
        /// </summary>
        internal Double FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Font family of the AxisLabels
        /// </summary>
        internal FontFamily FontFamily
        {
            get;
            set;
        }

        /// <summary>
        /// Font Color of the AxisLabels
        /// </summary>
        internal Brush FontColor
        {
            get;
            set;
        }

        /// <summary>
        /// Font Style of the AxisLabels
        /// </summary>
        internal FontStyle FontStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Font Weight of the AxisLabels
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
        /// To apply rotation to the Text element
        /// </summary>
        private RotateTransform Rotation
        {
            get;
            set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Applies properties to the TextBlock
        /// </summary>
        /// <param name="axisLabel"></param>
        private void ApplyProperties(AxisLabel axisLabel)
        {
            TextElement.Text = axisLabel.Text;
            TextElement.Foreground = FontColor;
            TextElement.FontSize = FontSize;
            TextElement.FontFamily = FontFamily;
            TextElement.FontStyle = FontStyle;
            TextElement.FontWeight = FontWeight;
        }

        /// <summary>
        /// Sets the position of the label based on the angle and the Position Property
        /// </summary>
        private void SetPosition()
        {
            // Get the visual size of the text block
#if WPF
            TextElement.Measure(new Size(Double.MaxValue, Double.MaxValue));
            ActualTextHeight = TextElement.DesiredSize.Height;
            ActualTextWidth = TextElement.DesiredSize.Width;
#else
            ActualTextHeight = TextElement.ActualHeight;
            ActualTextWidth = TextElement.ActualWidth;
#endif

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

            // calculate the actual size of the AxisLabel element
            CalculateSize(GetRadians(Angle));
        }

        /// <summary>
        /// set the position for axis that will be placed to the right of plot area
        /// </summary>
        /// <param name="newPos"></param>
        private void SetPositionRight(Point newPos)
        {
            // Stores the top and left of the textblock
            Double top;
            Double left;

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
            top = (ActualTextHeight / 2) * Math.Cos(GetRadians(angle));
            left = -(ActualTextHeight / 2) * Math.Sin(GetRadians(angle));

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
        /// set the position for axis that will be placed to the left of plot area
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
                ActualLeft = (Double)Visual.GetValue(Canvas.LeftProperty) + left - ((ActualTextHeight / 2) * Math.Cos(GetRadians(90 - Angle))); ;
            }
        }
        /// <summary>
        /// set the position for axis that will be placed to the bottom of plot area
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
        /// set the position for axis that will be placed to the top of plot area
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

        #endregion

        #region Internal Methods
        internal void CreateVisualObject()
        {
            Visual = new Canvas();
            TextElement = new TextBlock();
            Rotation = new RotateTransform();
            TextElement.RenderTransform = Rotation;
            Visual.Children.Add(TextElement);

            ApplyProperties(this);

            SetPosition();

        }
        #endregion

        #region Internal Events

        #endregion

        #region Static Methods
        /// <summary>
        /// Returns a positive angle
        /// The angle must always be between -90 and 90
        /// </summary>
        internal static Double GetAngle(Double angle)
        {
            return (angle >= 0 ? angle : 360 + angle);
        }

        /// <summary>
        /// Returns the radian for of the angle.
        /// Internall calls Angle to get positive angle value
        /// </summary>
        internal static Double GetRadians(Double angle)
        {
            return Math.PI / 180 * GetAngle(angle);
        }
        #endregion

        #region Data
        private Double _angle;                      
        private Point _position;
        private String _text;

        private Double _actualLeft;
        private Double _actualTop;
        private Double _actualWidth;
        private Double _actualHeight;

        private Double _actualTextHeight;
        private Double _actualTextWidth;
        #endregion
    }
}