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


namespace Visifire.Commons
{
    public struct GradientParams
    {
        #region Public Methods
        public GradientParams(LinearGradientParams linearGradientParams, RadialGradientParams radialGradientParams, SoildBrushParams solidBrushParams)
        {
            _linearGradientParams = linearGradientParams;
            _radialGradientParams = radialGradientParams;
            _solidBrushParams = solidBrushParams;
        }
        #endregion Public Methods

        #region Data
        public LinearGradientParams _linearGradientParams;
        public RadialGradientParams _radialGradientParams;
        public SoildBrushParams _solidBrushParams;
        #endregion Data
    }

    public struct LinearGradientParams
    {
        #region Public Methods
        public LinearGradientParams(Point startPoint, Point endPoint, Double intensity, Double angle, String shadeType)
        {
            _startPoint = new Point(startPoint.X, startPoint.Y);
            _endPoint = new Point(endPoint.X, endPoint.Y);
            _intensity = intensity;
            _angle = angle;
            _shadeType = shadeType;
        }
        #endregion Public Methods

        #region Data
        public Point _startPoint;
        public Point _endPoint;
        public Double _intensity;
        public Double _angle;
        public String _shadeType;
        #endregion Data
    }

    public struct RadialGradientParams
    {
        #region Public Methods
        public RadialGradientParams(Double intensity, String shadeType)
        {
            _intensity = intensity;
            _shadeType = shadeType;
        }
        #endregion Public Methods

        #region Data
        public Double _intensity;
        public String _shadeType;
        #endregion Data
    }

    public struct SoildBrushParams
    {
        #region Public Methods
        public SoildBrushParams(String gradString, Double angle, Double intensity, String shadeType, Boolean isGrad)
        {
            _gradString = gradString;
            _angle = angle;
            _intensity = intensity;
            _shadeType = shadeType;
            _isGrad = isGrad;
        }
        #endregion Public Methods

        #region Data
        public String _gradString;
        public Double _angle;
        public Double _intensity;
        public String _shadeType;
        public Boolean _isGrad;
        #endregion Data
    }
}
