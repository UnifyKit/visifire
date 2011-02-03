using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Commons.CircularLabel class
    /// </summary>
    internal class CircularAxisLabel
    {
        /// <summary>
        /// Initializes new instance for CircularAxisLabel class
        /// </summary>
        /// <param name="position"></param>
        /// <param name="center"></param>
        /// <param name="xRadius"></param>
        /// <param name="yRadius"></param>
        /// <param name="label"></param>
        /// <param name="index"></param>
        internal CircularAxisLabel(Point position, Point center, Double xRadius, Double yRadius, AxisLabel label, Double index)
        {
            Center = center;
            XRadius = xRadius;
            YRadius = yRadius;
            Position = position;
            AxisLabel = label;
            Index = index;
            Angle = Visifire.Commons.CircularLabel.ResetMeanAngle(CalculateAngleByCoordinate(Position));
        }

        /// <summary>
        /// Calculate angle by Coordinate
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Double CalculateAngleByCoordinate(Point position)
        {
            return Math.Atan2((position.Y - Center.Y), (position.X - Center.X));
        }

        internal Point Center
        {
            get;
            set;
        }

        internal Double XRadius
        {
            get;
            set;
        }

        internal Double YRadius
        {
            get;
            set;
        }

        internal AxisLabel AxisLabel
        {
            get;
            set;
        }

        internal Double Angle
        {
            get;
            set;
        }

        internal Double Index
        {
            get;
            set;
        }

        internal Point Position
        {
            get;
            set;
        }

        internal Point NextPosition
        {
            get;
            set;
        }
    }
}
