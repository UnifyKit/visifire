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

        /// <summary>
        /// Apply point animation
        /// </summary>
        /// <param name="existingStoryBoard">StoryBoard</param>
        /// <param name="target">Target object</param>
        /// <param name="targetName">Name of the Target object </param>
        /// <param name="propertyName">Property to animate</param>
        /// <param name="oldPosition">Old position</param>
        /// <param name="newPosition">New position</param>
        /// <param name="animationTime">Animation Duration</param>
        /// <param name="beginTime">Animations begin time</param>
        public static void ApplyPointAnimation(Storyboard existingStoryBoard, DependencyObject target,
            String targetName, String propertyName, Point oldPosition, Point newPosition, 
            Double animationTime, Double beginTime)
        {   
            PointAnimation pointAnimation = new PointAnimation();

            pointAnimation.From = oldPosition;
            pointAnimation.To = newPosition;
            pointAnimation.SpeedRatio = 2;
            pointAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, (int) animationTime * 1000));

            target.SetValue(FrameworkElement.NameProperty, targetName);

            Storyboard.SetTarget(pointAnimation, target);
            Storyboard.SetTargetProperty(pointAnimation, new PropertyPath(propertyName));
            Storyboard.SetTargetName(pointAnimation, (String)target.GetValue(FrameworkElement.NameProperty));
            
            existingStoryBoard.Children.Add(pointAnimation);
        }

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
        /// Create PointAnimation
        /// </summary>
        /// <param name="parentObj">Storyboard parent object</param>
        /// <param name="target">Animation target object</param>
        /// <param name="property">Property path to animate</param>
        /// <param name="beginTime">Animation begin time</param>
        /// <param name="frameTime">Frame time collection</param>
        /// <param name="values">Target value collection</param>
        /// <param name="splines">List of KeySpline</param>
        /// <returns>PointAnimationUsingKeyFrames</returns>
        internal static PointAnimationUsingKeyFrames CreatePointAnimation(FrameworkElement parentObj, DependencyObject target, String property, Double beginTime, DoubleCollection frameTime, PointCollection values, List<KeySpline> splines)
        {
            PointAnimationUsingKeyFrames da = new PointAnimationUsingKeyFrames();
            
#if WPF
            target.SetValue(FrameworkElement.NameProperty, target.GetType().Name + Guid.NewGuid().ToString().Replace('-', '_'));
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            (parentObj as ObservableObject).Chart._rootElement.RegisterName((string)target.GetValue(FrameworkElement.NameProperty), target);
#else


            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));

            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < values.Count; index++)
            {
                SplinePointKeyFrame keyFrame = new SplinePointKeyFrame();
                
                if (splines != null)
                    keyFrame.KeySpline = splines[index];

                if (Double.IsNaN(frameTime[index]))
                    continue;

                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTime[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

#if WPF
            da.Completed += delegate(object sender, EventArgs e)
            {
                Object element = (parentObj as ObservableObject).Chart._rootElement.FindName((string)target.GetValue(FrameworkElement.NameProperty));
                if (element != null)
                    (parentObj as ObservableObject).Chart._rootElement.UnregisterName((string)target.GetValue(FrameworkElement.NameProperty));
            };
#endif


            return da;
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
            String name = target.GetType().Name + Guid.NewGuid().ToString().Replace('-', '_');
            target.SetValue(FrameworkElement.NameProperty, name);
            Storyboard.SetTargetName(da, target.GetValue(FrameworkElement.NameProperty).ToString());

            if (parentObj.GetType().Equals(typeof(DataPoint)))
                parentObj = (parentObj as DataPoint).Parent;
            
            (parentObj as ObservableObject).Chart._rootElement.RegisterName(name, target);
#else
            Storyboard.SetTarget(da, target);
#endif
            Storyboard.SetTargetProperty(da, new PropertyPath(property));
            
            da.BeginTime = TimeSpan.FromSeconds(beginTime);

            for (Int32 index = 0; index < values.Count; index++)
            {
                SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
                
                if (splines != null)
                    keyFrame.KeySpline = splines[index];

                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(frameTime[index]));
                keyFrame.Value = values[index];
                da.KeyFrames.Add(keyFrame);
            }

#if WPF
            da.Completed += delegate(object sender, EventArgs e)
            {
                Object element = (parentObj as ObservableObject).Chart._rootElement.FindName(name);
                if (element != null)
                    (parentObj as ObservableObject).Chart._rootElement.UnregisterName(name);

                
                da.KeyFrames.Clear();
            };
#endif

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
                return ApplyOpacityAnimation(marker.Visual, parentObj, storyboard, beginTime, 0.75, 0,targetValue);
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
        internal static Storyboard ApplyOpacityAnimation(FrameworkElement objectToAnimate, FrameworkElement parentObj, Storyboard storyboard, Double beginTime, Double duration, Double fromValue, Double targetValue)
        {
            if (objectToAnimate != null && parentObj != null)
            {
                DoubleCollection values = Graphics.GenerateDoubleCollection(fromValue, targetValue);
                DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(0, duration);
                List<KeySpline> splines = GenerateKeySplineList
                    (
                    new Point(0, 0), new Point(1, 1),
                    new Point(0, 0), new Point(0.5, 1)
                    );

                objectToAnimate.Opacity = fromValue;

                DoubleAnimationUsingKeyFrames opacityAnimation = AnimationHelper.CreateDoubleAnimation(parentObj, objectToAnimate, "(UIElement.Opacity)", beginTime + 0.5, frameTimes, values, splines);
                storyboard.Children.Add(opacityAnimation);
            }

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
        internal static Storyboard ApplyPropertyAnimation(DependencyObject objectToAnimate, String property, FrameworkElement parentObj, Storyboard storyboard, Double beginTime, Double[] timeCollection, Double[] valueCollection, List<KeySpline> splain)
        {
            if (objectToAnimate != null && parentObj != null)
            {
                DoubleCollection values = Graphics.GenerateDoubleCollection(valueCollection);
                DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(timeCollection);
        
                DoubleAnimationUsingKeyFrames animation = AnimationHelper.CreateDoubleAnimation(parentObj, objectToAnimate, property, beginTime, frameTimes, values, splain);

                storyboard.Children.Add(animation);
            }

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
        internal static Storyboard ApplyPointAnimation(DependencyObject objectToAnimate, String property, FrameworkElement parentObj, Storyboard storyboard, Double beginTime, Double[] timeCollection, Point[] valueCollection, List<KeySpline> splain, Double speedRatio)
        {
            if (objectToAnimate != null && parentObj != null)
            {
                PointCollection values = Graphics.GeneratePointCollection(valueCollection);
                DoubleCollection frameTimes = Graphics.GenerateDoubleCollection(timeCollection);

                PointAnimationUsingKeyFrames animation = AnimationHelper.CreatePointAnimation(parentObj, objectToAnimate, property, beginTime, frameTimes, values, splain);

                if (!Double.IsNaN(speedRatio))
                    animation.SpeedRatio = speedRatio;

                storyboard.Children.Add(animation);
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