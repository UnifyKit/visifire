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
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using Visifire.Charts;

namespace Visifire.Commons
{
    /// <summary>
    /// Visifire.Commons.AnimationHelper class
    /// </summary>
    internal class AnimationHelper
    {
        #region Public Methods

        #endregion

        #region Public Properties

        #endregion

        #region Public Events And Delegates

        #endregion

        #region Protected Methods

        #endregion

        #region Internal Properties

        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns list of KeySpline
        /// </summary>
        /// <param name="count">Required number of KeySplines</param>
        /// <returns>List of KeySpline</returns>
        public static List<KeySpline> GenerateKeySplineList(int count)
        {
            List<KeySpline> splines = new List<KeySpline>();

            for (int i = 0; i < count; i++)
                splines.Add(GetKeySpline(new Point(0, 0), new Point(1, 1)));

            return splines;
        }

        /// <summary>
        /// Returns a new KeySpline
        /// </summary>
        /// <param name="controlPoint1">First control point</param>
        /// <param name="controlPoint2">Second control point</param>
        /// <returns>KeySpline</returns>
        internal static KeySpline GetKeySpline(Point controlPoint1, Point controlPoint2)
        {
            return new KeySpline() { ControlPoint1 = controlPoint1, ControlPoint2 = controlPoint2 };
        }

        /// <summary>
        /// Create DoubleAnimation
        /// </summary>
        /// <param name="parentObj">Storyboard parent object</param>
        /// <param name="target">Animation target object</param>
        /// <param name="property">Property path to animate</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="frameTime">Frame time collection</param>
        /// <param name="values">Target value collection</param>
        /// <param name="splines">List of KeySpline</param>
        /// <returns>DoubleAnimationUsingKeyFrames</returns>
        internal static DoubleAnimationUsingKeyFrames CreateDoubleAnimation(FrameworkElement parentObj, DependencyObject target, String property, Double beginTime, DoubleCollection frameTime, DoubleCollection values, List<KeySpline> splines)
        {
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();

#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name + Guid.NewGuid().ToString().Replace('-', '_'));
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            (parentObj as DataSeries).Chart._rootElement.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));
            
            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < splines.Count; index++)
            {
                SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
                keyFrame.KeySpline = splines[index];
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTime[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

            return da;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Apply Opacity animation to a Marker
        /// </summary>
        /// <param name="marker">Marker to animate</param>
        /// <param name="parentObj">Storyboard parent Object</param>
        /// <param name="storyboard">Storyboard reference</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="targetValue">Target opacity value</param>
        /// <returns>Storyboard</returns>
        internal static Storyboard ApplyOpacityAnimation(Marker marker, FrameworkElement parentObj, Storyboard storyboard, Double beginTime, Double targetValue)
        {   
            if (marker != null && parentObj != null)
                return ApplyOpacityAnimation(marker.Visual, parentObj, storyboard, beginTime, 0.75, targetValue);
            else
                return storyboard;
        }

        /// <summary>
        /// Apply Opacity animation to an object
        /// </summary>
        /// <param name="objectToAnimate">Object to animate</param>
        /// <param name="parentObj">Storyboard parent Object</param>
        /// <param name="storyboard">Storyboard reference</param>
        /// <param name="beginTime">Begin time</param>
        /// <param name="duration">Animation duration</param>
        /// <param name="targetValue">Target opacity value</param>
        /// <returns>Storyboard</returns>
        internal static Storyboard ApplyOpacityAnimation(FrameworkElement objectToAnimate, FrameworkElement parentObj, Storyboard storyboard, Double beginTime, Double duration, Double targetValue)
        {
            if (objectToAnimate != null && parentObj != null)
            {
                DoubleCollection values = Graphics.GenerateDoubleCollection(0, targetValue);
                DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, duration);
                List<KeySpline> splines = GenerateKeySplineList
                    (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0.5, 1)
                    );

                objectToAnimate.Opacity = 0;

                DoubleAnimationUsingKeyFrames opacityAnimation = AnimationHelper.CreateDoubleAnimation(parentObj, objectToAnimate, "(UIElement.Opacity)", beginTime + 0.5, frameTimes, values, splines);
                storyboard.Children.Add(opacityAnimation);
            }

            return storyboard;
        }

        /// <summary>
        /// Returns list of KeySpline from point array
        /// </summary>
        /// <param name="values">List of points</param>
        /// <returns>List of KeySpline</returns>
        internal static List<KeySpline> GenerateKeySplineList(params Point[] values)
        {
            List<KeySpline> splines = new List<KeySpline>();
            for (Int32 i = 0; i < values.Length; i += 2)
                splines.Add(GetKeySpline(values[i], values[i + 1]));

            return splines;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        #endregion
    }

}