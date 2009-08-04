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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
#endif

using Visifire.Commons;
using System.Windows.Shapes;

namespace Visifire.Charts
{
    /// <summary>
    /// Visifire.Charts.StockChart class
    /// </summary>
    internal class StockChart
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

        /// <summary>
        /// Current working DataSeries
        /// </summary>
        private static DataSeries CurrentDataSeries
        {
            get;
            set;
        }

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        /// <summary>
        /// Apply animation for point chart
        /// </summary>
        /// <param name="pointGrid">Point chart grid</param>
        /// <param name="storyboard">Stroyboard</param>
        /// <param name="width">Width of the chart canvas</param>
        /// <param name="height">Height of the chart canvas</param>
        /// <returns>Storyboard</returns>
        private static Storyboard ApplyStockChartAnimation(Panel pointGrid, Storyboard storyboard, Double width, Double height)
        {
#if WPF
            if (storyboard != null && storyboard.GetValue(System.Windows.Media.Animation.Storyboard.TargetProperty) != null)
                storyboard.Stop();
#else
            if (storyboard != null)
                storyboard.Stop();
#endif

            TransformGroup group = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 0, CenterX = 0.5, CenterY = 0.5 };
            TranslateTransform translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);

            pointGrid.RenderTransform = group;

            Random rand = new Random((Int32)DateTime.Now.Ticks);
            double begin = rand.NextDouble();

            pointGrid.Measure(new Size(Double.MaxValue, Double.MaxValue));

            DoubleCollection times = Graphics.GenerateDoubleCollection(0, 0.5, 0.75, 1);
            DoubleCollection scaleValues = Graphics.GenerateDoubleCollection(0, 1, 0.5, 1);
            DoubleCollection translateXValues = Graphics.GenerateDoubleCollection(pointGrid.DesiredSize.Width / 2, 0, pointGrid.DesiredSize.Width / 4, 0);
            DoubleCollection translateYValues = Graphics.GenerateDoubleCollection(pointGrid.DesiredSize.Height / 2, 0, pointGrid.DesiredSize.Height / 4, 0);
            List<KeySpline> splines1 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines2 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines3 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));
            List<KeySpline> splines4 = AnimationHelper.GenerateKeySplineList(new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1), new Point(0, 0.5), new Point(0.5, 1));

            DoubleAnimationUsingKeyFrames xScaleAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleX)", begin + 0.5, times, scaleValues, splines1);
            DoubleAnimationUsingKeyFrames yScaleAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, scaleTransform, "(ScaleTransform.ScaleY)", begin + 0.5, times, scaleValues, splines2);
            DoubleAnimationUsingKeyFrames xTranslateAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, translateTransform, "(TranslateTransform.X)", begin + 0.5, times, translateXValues, splines3);
            DoubleAnimationUsingKeyFrames yTranslateAnimation = AnimationHelper.CreateDoubleAnimation(CurrentDataSeries, translateTransform, "(TranslateTransform.Y)", begin + 0.5, times, translateYValues, splines4);

            storyboard.Children.Add(xScaleAnimation);
            storyboard.Children.Add(yScaleAnimation);
            storyboard.Children.Add(xTranslateAnimation);
            storyboard.Children.Add(yTranslateAnimation);

            return storyboard;
        }

        #endregion

        #region Internal Methods

         /// <summary>
        /// Calculate DataPoint width
        /// </summary>
        /// <param name="width">PlotCanvas width</param>
        /// <param name="height">PlotCanvas height</param>
        /// <param name="chart">Chart reference</param>
        /// <returns>DataPointWidth as Double</returns>
        //private static Double CalculateDataPointWidth(Double width, Double height, Chart chart)
        //{
        //    Double dataPointWidth;

        //    Double minDiffValue = chart.PlotDetails.GetMinOfMinDifferencesForXValue(RenderAs.Column, RenderAs.StackedColumn, RenderAs.StackedColumn100, RenderAs.Stock, RenderAs.CandleStick);
            
        //    if (double.IsPositiveInfinity(minDiffValue))
        //        minDiffValue = 0;

        //    if (Double.IsNaN(chart.DataPointWidth))
        //    {
        //        if (minDiffValue != 0)
        //        {
        //            dataPointWidth = Graphics.ValueToPixelPosition(0, width, (Double)chart.AxesX[0].InternalAxisMinimum, (Double)chart.AxesX[0].InternalAxisMaximum, minDiffValue + (Double)chart.AxesX[0].InternalAxisMinimum);
        //            dataPointWidth *= .9;

        //            if (dataPointWidth < 5)
        //                dataPointWidth = 5;
        //        }
        //        else
        //        {
        //            dataPointWidth = width * .3;
        //        }
        //    }
        //    else
        //    {
        //        dataPointWidth = chart.PlotArea.Width / 100 * chart.DataPointWidth;
        //    }

        //    return dataPointWidth;
        //}


        /// <summary>
        /// Recalculate and apply new brush
        /// </summary>
        /// <param name="shape">Shape reference</param>
        /// <param name="newBrush">New Brush</param>
        /// <param name="isLightingEnabled">Whether lighting is enabled</param>
        /// <returns>New Calculated Brush</returns>
        internal static Brush ReCalculateAndApplyTheNewBrush(Shape shape, Brush newBrush, Boolean isLightingEnabled)
        {
            shape.Stroke = ((Boolean)isLightingEnabled) ? Graphics.GetLightingEnabledBrush(newBrush, "Linear", null) : newBrush;

            return shape.Stroke;
        }

        /// <summary>
        /// Get visual object for point chart
        /// </summary>
        /// <param name="width">Width of the charat</param>
        /// <param name="height">Height of the charat</param>
        /// <param name="plotDetails">plotDetails</param>
        /// <param name="seriesList">List of DataSeries</param>
        /// <param name="chart">Chart</param>
        /// <param name="plankDepth">Plank depth</param>
        /// <param name="animationEnabled">Whether animation is enabled</param>
        /// <returns>Point chart canvas</returns>
        internal static Canvas GetVisualObjectForStockChart(Double width, Double height, PlotDetails plotDetails, List<DataSeries> seriesList, Chart chart, Double plankDepth, bool animationEnabled)
        {
            if (Double.IsNaN(width) || Double.IsNaN(height) || width <= 0 || height <= 0) return null;

            Canvas visual = new Canvas() { Width = width, Height = height };
            Canvas labelCanvas = new Canvas() { Width = width, Height = height };

            Double depth3d = plankDepth / (plotDetails.Layer3DCount == 0 ? 1 : plotDetails.Layer3DCount) * (chart.View3D ? 1 : 0);
            Double visualOffset = depth3d * (plotDetails.SeriesDrawingIndex[seriesList[0]] + 1 - (plotDetails.Layer3DCount == 0 ? 0 : 1));
            visual.SetValue(Canvas.TopProperty, visualOffset);
            visual.SetValue(Canvas.LeftProperty, -visualOffset);

            Double highY = 0, lowY = 0, openY = 0, closeY = 0;
            Double animationBeginTime = 0;
            DataSeries _tempDataSeries = null;

            // Calculate width of a DataPoint 
            _dataPointWidth = CandleStick.CalculateDataPointWidth(width, height, chart);
            
            foreach (DataSeries series in seriesList)
            {
                if (series.Enabled == false)
                    continue;

                Canvas seriesCanvas = new Canvas() { Width = width, Height = height };

                _tempDataSeries = series;

                PlotGroup plotGroup = series.PlotGroup;

                foreach (DataPoint dataPoint in series.InternalDataPoints)
                {
                    if (dataPoint.YValues == null || (dataPoint.Enabled == false))
                        continue;

                    // Creating ElementData for Tag
                    ElementData tagElement = new ElementData() { Element = dataPoint };

                    CandleStick.SetDataPointValues(dataPoint, ref highY, ref lowY, ref openY, ref closeY);

                    // Calculate required pixel positions
                    Double xPositionOfDataPoint = Graphics.ValueToPixelPosition(0, width, (Double)plotGroup.AxisX.InternalAxisMinimum, (Double)plotGroup.AxisX.InternalAxisMaximum, dataPoint.InternalXValue);
                    openY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, openY);
                    closeY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, closeY);
                    highY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, highY);
                    lowY = Graphics.ValueToPixelPosition(height, 0, (Double)plotGroup.AxisY.InternalAxisMinimum, (Double)plotGroup.AxisY.InternalAxisMaximum, lowY);

                    Double dataPointTop = (lowY < highY) ? lowY : highY;
                    openY = openY - dataPointTop;
                    closeY = closeY - dataPointTop;

                    // Create DataPoint Visual
                    Canvas dataPointVisual = new Canvas() 
                    {
                        Width = _dataPointWidth, 
                        Height = Math.Abs(lowY - highY) 
                    };

                    // Set DataPoint Visual position
                    dataPointVisual.SetValue(Canvas.TopProperty, dataPointTop);
                    dataPointVisual.SetValue(Canvas.LeftProperty, xPositionOfDataPoint - _dataPointWidth / 2);

                    // Create High and Low Line
                    Line highLow = new Line()
                    {   
                        Tag = tagElement,
                        X1 = dataPointVisual.Width / 2,
                        X2 = dataPointVisual.Width / 2,
                        Y1 = 0,
                        Y2 = dataPointVisual.Height,
                        StrokeThickness = (Double) dataPoint.BorderThickness.Left,
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                    };

                    ReCalculateAndApplyTheNewBrush(highLow, dataPoint.Color, (Boolean) dataPoint.LightingEnabled);

                    // Create Open Line
                    Line open = new Line()
                    {
                        Tag = tagElement,
                        X1 = 0,
                        X2 = dataPointVisual.Width / 2,
                        Y1 = openY,
                        Y2 = openY,
                        Stroke = highLow.Stroke,
                        StrokeThickness = (Double)dataPoint.BorderThickness.Left,
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                    };

                    // Create Close Line
                    Line close = new Line()
                    {
                        Tag = tagElement,
                        X1 = dataPointVisual.Width / 2,
                        X2 = dataPointVisual.Width,
                        Y1 = closeY,
                        Y2 = closeY,
                        Stroke = highLow.Stroke,
                        StrokeThickness = (Double)dataPoint.BorderThickness.Left,
                        StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                    };

                    #region Apply Shadow

                    if (dataPoint.ShadowEnabled == true)
                    {
                        // Create High and Low Line
                        Line highLowShadow = new Line()
                        {   
                            IsHitTestVisible = false,
                            X1 = dataPointVisual.Width / 2 + CandleStick._shadowDepth,
                            X2 = dataPointVisual.Width / 2 + CandleStick._shadowDepth,
                            Y1 = 0 + CandleStick._shadowDepth,
                            Y2 = dataPointVisual.Height + CandleStick._shadowDepth,
                            Stroke = CandleStick._shadowColor,
                            StrokeThickness = (Double)dataPoint.BorderThickness.Left,
                            StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                        };
                        
                        // Create Open Line
                        Line openShadowLine = new Line()
                        {   
                            IsHitTestVisible = false,
                            X1 = 0 + CandleStick._shadowDepth,
                            X2 = dataPointVisual.Width / 2 + CandleStick._shadowDepth,
                            Y1 = openY + CandleStick._shadowDepth,
                            Y2 = openY + CandleStick._shadowDepth,
                            Stroke = CandleStick._shadowColor,
                            StrokeThickness = (Double)dataPoint.BorderThickness.Left,
                            StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                        };

                        // Create Close Line
                        Line closeShadowLine = new Line()
                        {   
                            IsHitTestVisible = false,
                            X1 = dataPointVisual.Width / 2 + CandleStick._shadowDepth,
                            X2 = dataPointVisual.Width + CandleStick._shadowDepth,
                            Y1 = closeY + CandleStick._shadowDepth,
                            Y2 = closeY + CandleStick._shadowDepth,
                            Stroke = CandleStick._shadowColor,
                            StrokeThickness = (Double)dataPoint.BorderThickness.Left,
                            StrokeDashArray = Graphics.LineStyleToStrokeDashArray(dataPoint.BorderStyle.ToString())
                        };

                        // Add shadows
                        dataPointVisual.Children.Add(highLowShadow);
                        dataPointVisual.Children.Add(openShadowLine);
                        dataPointVisual.Children.Add(closeShadowLine);
                    }

                    #endregion

                    if (highLow.StrokeThickness > _dataPointWidth / 2)
                        highLow.StrokeThickness = _dataPointWidth / 2;
                    else if (highLow.StrokeThickness > _dataPointWidth)
                        highLow.StrokeThickness = _dataPointWidth;

                    // Compose DataPoint faces, visual components and visual parts
                    dataPoint.Faces = new Faces();
                    dataPoint.Faces.Parts = new List<DependencyObject>();

                    // Add parts
                    dataPoint.Faces.Parts.Add(highLow);
                    dataPoint.Faces.Parts.Add(open);
                    dataPoint.Faces.Parts.Add(close);

                    // Add VisualComponents
                    dataPoint.Faces.VisualComponents.Add(highLow);
                    dataPoint.Faces.VisualComponents.Add(open);
                    dataPoint.Faces.VisualComponents.Add(close);

                    // Add Border elements
                    dataPoint.Faces.BorderElements.Add(highLow);
                    dataPoint.Faces.BorderElements.Add(open);
                    dataPoint.Faces.BorderElements.Add(close);

                    // Add VisualComponents to visual
                    dataPointVisual.Children.Add(highLow);
                    dataPointVisual.Children.Add(open);
                    dataPointVisual.Children.Add(close);

                    dataPoint.Faces.Visual = dataPointVisual;
                    seriesCanvas.Children.Add(dataPointVisual);

                    // Place label for the DataPoint
                    CandleStick.PlaceLabel(visual, labelCanvas, dataPoint);
                }

                // Apply animation to series
                if (animationEnabled)
                {
                    if (_tempDataSeries.Storyboard == null)
                        _tempDataSeries.Storyboard = new Storyboard();

                    _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(seriesCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 0, 1);
                    animationBeginTime += 0.5;
                }
                
                visual.Children.Add(seriesCanvas);
            }

            // Label animation
            if (animationEnabled && _tempDataSeries != null)
                _tempDataSeries.Storyboard = AnimationHelper.ApplyOpacityAnimation(labelCanvas, _tempDataSeries, _tempDataSeries.Storyboard, animationBeginTime, 1, 0, 1);
            
            visual.Children.Add(labelCanvas);

            return visual;
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data


         private static Double _dataPointWidth;

        #endregion
    }
}