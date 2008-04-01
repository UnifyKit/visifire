/*
      Copyright (C) 2008 Webyog Softworks Private Limited

     This file is part of VisifireCharts.
 
     VisifireCharts is a free software: you can redistribute it and/or modify
     it under the terms of the GNU General Public License as published by
     the Free Software Foundation, either version 3 of the License, or
     (at your option) any later version.
 
     VisifireCharts is distributed in the hope that it will be useful,
     but WITHOUT ANY WARRANTY; without even the implied warranty of
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     GNU General Public License for more details.
 
     You should have received a copy of the GNU General Public License
     along with VisifireCharts.  If not, see <http://www.gnu.org/licenses/>.
 
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
using Visifire.Commons;

namespace Visifire.Charts
{
    public class AxisLabels : VisualObject
    {
        #region Public Methods

        public AxisLabels()
        {
        }

        public override void Render()
        {
        }

        public override void Init()
        {
            base.Init();
            ValidateParent();

            SetName();
            MaxLabelWidth = (_parent.Parent as Chart).Width * (TextWrap <= 0 ? 0.3 : TextWrap);
        }

        public override void SetWidth()
        {
            Double maxWidth = 0;

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                Double padding = (_parent.Parent as Chart).Padding;
                foreach (AxisLabel lbl in this.Children)
                {
                    if (maxWidth < ((Double)lbl.ActualWidth + lbl.Left))
                        maxWidth = (Double)lbl.ActualWidth + lbl.Left;
                }

                maxWidth += padding;

            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                maxWidth = (Double)_parent.GetValue(WidthProperty);

            }
            this.SetValue(WidthProperty, maxWidth);
        }

        public override void SetHeight()
        {
            Double maxHeight = 0;

            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                maxHeight = (Double)_parent.GetValue(HeightProperty);


            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {


                maxHeight = _rowHeight * _numberOfCalculatedRows + (_parent.Parent as Chart).Padding;

            }
            this.SetValue(HeightProperty, maxHeight);
        }

        public override void SetLeft()
        {
            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                if ((_parent.Parent as Chart).View3D)
                {
                    if (_parent.GetType().Name == "AxisX")
                    {
                        if (!String.IsNullOrEmpty(_parent.Title))
                            this.SetValue(LeftProperty, _parent._titleTextBlock.ActualHeight);
                        else
                            this.SetValue(LeftProperty, (_parent.Parent as Chart).Padding);
                    }
                    else
                    {

                        if (!String.IsNullOrEmpty(_parent.Title))
                            this.SetValue(LeftProperty, _parent._titleTextBlock.ActualHeight);
                        else
                            this.SetValue(LeftProperty, (_parent.Parent as Chart).Padding);
                    }
                }
                else
                {

                    if (!String.IsNullOrEmpty(_parent.Title))
                        this.SetValue(LeftProperty, _parent._titleTextBlock.ActualHeight);
                    else
                        this.SetValue(LeftProperty, (_parent.Parent as Chart).Padding);
                }

            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                if ((_parent.Parent as Chart).View3D)
                {
                    this.SetValue(LeftProperty, -_parent.MajorTicks.TickLength);
                }
                else
                {
                    this.SetValue(LeftProperty, 0);
                }
            }
        }

        public override void SetTop()
        {
            if (_parent.AxisOrientation == AxisOrientation.Bar)
            {
                if ((_parent.Parent as Chart).View3D)
                {
                    this.SetValue(TopProperty, _parent.MajorTicks.TickLength);
                }
                else
                {
                    this.SetValue(TopProperty, 0);
                }
            }
            else if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                if ((_parent.Parent as Chart).View3D)
                {
                    if (_parent.GetType().Name == "AxisX")
                        this.SetValue(TopProperty, (Double)_parent.MajorTicks.Height);
                    else
                        this.SetValue(TopProperty, (Double)_parent.MajorTicks.Height + (_parent.Parent as Chart).AxisX.MajorTicks.TickLength);
                }
                else
                {
                    this.SetValue(TopProperty, (Double)_parent.MajorTicks.GetValue(HeightProperty));
                }
            }
        }

        #endregion Public Methods

        #region Public Properties

        #region Font Properties

        public String FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
            }
        }

        public Double FontSize
        {
            get
            {
                if (!Double.IsNaN(_fontSize))
                    return _fontSize;
                else
                    return CalculateFontSize();
            }
            set
            {
                _fontSize = value;
            }
        }

        public Brush FontColor
        {
            get
            {
                return _fontColor;
            }

            set
            {
                _fontColor = value;
            }
        }

        public String FontStyle
        {
            get
            {
                return _fontStyle.ToString();
            }

            set
            {
                _fontStyle = Converter.StringToFontStyle(value);
            }

        }
        public String FontWeight
        {
            get
            {
                return _fontWeight.ToString();
            }

            set
            {
                _fontWeight = Converter.StringToFontWeight(value);
            }

        }
        #endregion
        
        public Double TextWrap
        {
            get;
            set;
        }

        public Int32 Rows
        {
            get;
            set;
        }

        public Double LabelAngle
        {
            get
            {
                if (!Double.IsNaN(_labelAngle))
                    return _labelAngle;
                else
                    return _parent.LabelAngle;
            }
            set
            {
                _labelAngle = value;
            }
        }

        public Double Interval
        {
            get
            {
                if (Double.IsNaN(_interval))
                    return _parent.Interval;
                else
                    return _interval;
            }
            set
            {
                _interval = value;
            }
        }
        
        #endregion Public Properties

        #region Internal Properties

        internal Double MaxLabelWidth
        {
            get;
            set;
        }

        internal System.Collections.Generic.Dictionary<Double, AxisLabel> LabelDictionary
        {
            get
            {
                return _labelDictionary;
            }
        }

        #endregion Internal properties

        #region Internal Methods
        internal void CreateLabels()
        {
            if (!Enabled || !_parent.Enabled)
                return;

            Double calculatedFontSize = CalculateFontSize();

            Double position = 0;
            PlotDetails plotDetails = (_parent.Parent as Chart).PlotDetails;

            Double noOfIntervals = _parent.AxisManager.GetNoOfIntervals();

            Double interval = Interval;
            AxisLabel lbl = new AxisLabel();

            this.LabelDictionary.Clear();

            Double i = _parent.AxisMinimum;


            if (_parent.GetType().Name == "AxisX")
            {
                if ((Double)_parent.AxisManager.GetMinimumDataValue() - interval < _parent.AxisMinimum && _parent.GetType().Name == "AxisX")
                    i = (Double)_parent.AxisManager.GetMinimumDataValue();
                else
                    i = _parent.AxisMinimum;

                if (plotDetails.AllAxisLabels && plotDetails.AxisLabels.Count > 0)
                {

                    System.Collections.Generic.Dictionary<Double, String>.Enumerator enumerator = plotDetails.AxisLabels.GetEnumerator();
                    enumerator.MoveNext();
                    int j = 0;
                    for (i = enumerator.Current.Key; j < plotDetails.AxisLabels.Count - 1; j++)
                    {
                        enumerator.MoveNext();
                        if (i > enumerator.Current.Key)
                            i = enumerator.Current.Key;
                    }
                    enumerator.Dispose();
                }
            }

            int countIntervals = 0;
            double minValue = i;
            for (; i <= _parent.AxisMaximum; i = minValue + (++countIntervals) * interval)
            {
                if (_parent.GetType().Name == "AxisX")
                    if ((plotDetails.AllAxisLabels && plotDetails.AxisLabels.Count > 0) && i > plotDetails.MaxAxisXValue)
                        continue;

                lbl = new AxisLabel();
                lbl.FontStyle = FontStyle;
                lbl.FontWeight = FontWeight;
                position = i;

                // Here the axis label text is genereted, The condition below forces the AxisY to 
                // be initialized with only formated numeric value
                String text;
                if (!plotDetails.AxisLabels.ContainsKey(i) || (_parent.GetType().Name == "AxisY"))
                {
                    if (_parent.ValueFormatString != null)
                    {
                        text = _parent.GetFormattedText(position);
                    }
                    else
                    {
                        text = _parent.Prefix + position.ToString() + _parent.Suffix;
                    }
                }
                else
                {
                    // Here the axis labels given by the user is selected as text. only for Axis X
                    text = plotDetails.AxisLabels[i];
                }

                lbl.Text = text;
                this.Children.Add(lbl);

                lbl.Init();
                lbl.Angle = LabelAngle;

                lbl.Position = position;
                if (Double.IsNaN(_fontSize))
                {

                    lbl.FontSize = calculatedFontSize;
                }


            }

        }

        internal void PositionLabels()
        {
            if (!Enabled || !_parent.Enabled)
                return;

            Double rowHeight = 0;
            Double totalLabelWidth = 0;
            Double axisWidth = (_parent.DoubleToPixel(_parent.AxisMaximum) - _parent.DoubleToPixel(_parent.AxisMinimum));
            Double labelHeight = 0, tempHeight;
            Double textSize;
            Int32 numberOfRows = 1;
            Double prevLabelRight = 0, curLabelLeft;
            Boolean overlapOccured = false;

            Double gapBetweenText = 0;
            TextBlock tb = new TextBlock();
            tb.Text = "AB";

            if (_parent.AxisOrientation == AxisOrientation.Column)
            {
                // To check if multiple rows of titles is necessary or not
                Double textMinimum = 8;
                if (Double.IsNaN(_fontSize))
                {
                    textSize = CalculateFontSize();
                }
                else
                {
                    textSize = _fontSize;
                    textMinimum = _fontSize;
                }

                for (; textSize >= textMinimum; textSize -= 2)
                {
                    totalLabelWidth = 0;
                    labelHeight = 0;
                    overlapOccured = false;
                    tb.FontSize = textSize;
                    gapBetweenText = tb.ActualWidth;

                    foreach (AxisLabel lbl in this.Children)
                    {
                        lbl.FontSize = textSize;
                        curLabelLeft = (Double)this._parent.DoubleToPixel(lbl.Position) - lbl.Width / 2;

                        // if there is a overlap then add the overlap region to the label list

                        if (((prevLabelRight - curLabelLeft) > 0) && totalLabelWidth > 0)
                            overlapOccured = true;

                        totalLabelWidth += (lbl.ActualWidth + gapBetweenText);
                        tempHeight = Math.Abs((Math.Sqrt(Math.Pow(lbl.ActualWidth, 2) + Math.Pow(lbl.ActualHeight, 2))) * Math.Sin(lbl.Angle * Math.PI / 180));
                        if (tempHeight == 0) tempHeight = lbl.ActualHeight;
                        labelHeight = Math.Max(labelHeight, tempHeight);
                        prevLabelRight = (Double)this._parent.DoubleToPixel(lbl.Position) + lbl.Width / 2;
                    }

                    // This is to number of rows required to display the labels
                    if (overlapOccured == false)
                    {
                        numberOfRows = (int)Math.Ceiling(totalLabelWidth / axisWidth);
                        if (numberOfRows == 1) break;
                    }
                }


                //if overlap occured then fore change of angle
                if (overlapOccured)
                {
                    numberOfRows = (int)Math.Ceiling(totalLabelWidth / axisWidth);
                    Double[] prevLR = new Double[numberOfRows];
                    Double[] totalLW = new Double[numberOfRows];
                    Double curLL;
                    int k, j = 0;
                    prevLR.Initialize();
                    totalLW.Initialize();

                    overlapOccured = false;
                    foreach (AxisLabel lbl in this.Children)
                    {
                        k = j++ % numberOfRows;
                        curLL = (Double)this._parent.DoubleToPixel(lbl.Position) - lbl.Width / 2;

                        // if there is a overlap then add the overlap region to the label list

                        if (((prevLR[k] - curLL) > 0) && totalLW[k] > 0)
                            overlapOccured = true;

                        totalLW[k] += (lbl.ActualWidth + 10);
                        prevLR[k] = (Double)this._parent.DoubleToPixel(lbl.Position) + lbl.Width / 2;
                    }
                }

                // If Number of rows is given by the user then overide the previous setting
                if (Rows > 0)
                {
                    numberOfRows = Rows;
                }
                else if ((numberOfRows > 2 || overlapOccured) && Double.IsNaN(_labelAngle) && Double.IsNaN(_parent._labelAngle))
                {
                    labelHeight = 0;
                    totalLabelWidth = 0;
                    numberOfRows = 1;
                    foreach (AxisLabel lbl in this.Children)
                    {
                        lbl.Angle = -45;
                        totalLabelWidth += lbl.ActualWidth;
                        tempHeight = Math.Abs(Math.Sqrt(Math.Pow(lbl.ActualWidth, 2) + Math.Pow(lbl.ActualHeight, 2)) * Math.Sin(lbl.Angle * Math.PI / 180));
                        if (tempHeight == 0) tempHeight = lbl.ActualHeight;
                        labelHeight = Math.Max(labelHeight, tempHeight);
                    }
                }
                // this is the height of each row
                rowHeight = labelHeight;

                //These variables will be used to update the height of the row
                _rowHeight = rowHeight;
                _numberOfCalculatedRows = numberOfRows;
            }
            else
            {
                if (Double.IsNaN(_fontSize))
                {
                    textSize = CalculateFontSize();
                    foreach (AxisLabel lbl in this.Children)
                    {
                        lbl.FontSize = textSize;
                    }

                }
            }
            // label index decides which label goes into which row
            int index = 0;

            foreach (AxisLabel lbl in this.Children)
            {
                Double height1 = Math.Abs(lbl.DiagonalLength * Math.Sin((lbl.DiagonalAngle) * Math.PI / 180));
                Double height2 = Math.Abs(lbl.DiagonalLength * Math.Sin((360 - lbl.DiagonalAngle + lbl.Angle * 2) * Math.PI / 180));
                Double width1 = Math.Abs(lbl.DiagonalLength * Math.Cos(lbl.DiagonalAngle * Math.PI / 180));
                Double width2 = Math.Abs(lbl.DiagonalLength * Math.Cos((360 - lbl.DiagonalAngle + lbl.Angle * 2) * Math.PI / 180));


                if (_parent.AxisOrientation == AxisOrientation.Bar)
                {
                    if ((lbl.Angle >= 0 && lbl.Angle <= 90))
                        lbl.Left = (Double)this.GetValue(WidthProperty) - lbl.Width * Math.Cos((lbl.Angle) * Math.PI / 180);
                    else
                        lbl.Left = (Double)this.GetValue(WidthProperty) - lbl.ActualWidth;


                    lbl.Top = (Double)this._parent.DoubleToPixel(lbl.Position);

                    if (lbl.Angle >= 0 && lbl.Angle < 90)
                    {
                        lbl.Top += -lbl.ActualHeight + ((Double)lbl.GetValue(HeightProperty) / 2 * Math.Cos((lbl.Angle) * Math.PI / 180));
                    }
                    else if (lbl.Angle > -90 && lbl.Angle < 0)
                    {

                        lbl.Top += lbl.ActualHeight - ((Double)lbl.GetValue(HeightProperty) / 2 * Math.Cos((lbl.Angle) * Math.PI / 180) * 3);
                    }
                    else if (lbl.Angle == -90)
                        lbl.Top += (Double)lbl.GetValue(WidthProperty) / 2;
                    else
                        lbl.Top -= (Double)lbl.GetValue(WidthProperty) / 2;



                }
                else if (_parent.AxisOrientation == AxisOrientation.Column)
                {
                    lbl.Left = (Double)this._parent.DoubleToPixel(lbl.Position);

                    if (lbl.Angle > 0 && lbl.Angle <= 90)
                    {
                        lbl.Left += ((Double)lbl.GetValue(HeightProperty) / 2 * Math.Cos((90 - lbl.Angle) * Math.PI / 180));
                    }
                    else if (lbl.Angle >= -90 && lbl.Angle < 0)
                    {
                        lbl.Left += -lbl.ActualWidth + ((Double)lbl.GetValue(HeightProperty) / 2 * Math.Cos((90 + lbl.Angle) * Math.PI / 180));
                    }
                    else
                        lbl.Left -= lbl.Width / 2;

                    if (lbl.Angle >= 0 && lbl.Angle <= 90)
                    {

                        // If multiplerows then move consecutive labels into different rows
                        lbl.Top = rowHeight * (index % numberOfRows);
                        index++;


                    }
                    else if (lbl.Angle >= -90 && lbl.Angle < 0)
                    {


                        lbl.Top = ((Double)lbl.GetValue(WidthProperty) * Math.Sin((-lbl.Angle) * Math.PI / 180));

                        // If multiplerows then move consecutive labels into different rows
                        lbl.Top += rowHeight * (index % numberOfRows);
                        index++;



                    }



                }


                lbl.SetValue(LeftProperty, lbl.Left);
                lbl.SetValue(TopProperty, lbl.Top);
            }
            CheckOutOfBounds();
        }

        #endregion Internal Method

        #region Private Methods

        /// <summary>
        /// Validate Parent element and assign it to _parent.
        /// Parent element should be a Chart element. Else throw an exception.
        /// </summary>
        private void ValidateParent()
        {
            if (this.Parent is Axes)
                _parent = this.Parent as Axes;
            else
                throw new Exception(this + "Parent should be an Axis");
        }

        /// <summary>
        /// Set a default Name. This is usefull if user has not specified this object in data XML and it has been 
        /// created by default.
        /// </summary>
        private void SetName()
        {
            if (this.Name.Length == 0)
            {
                int i = 0;

                String type = this.GetType().Name;
                String name = type;

                // Check for an available name
                while (FindName(name + i.ToString()) != null)
                {
                    i++;
                }
                name += i.ToString();

                this.SetValue(NameProperty, name);
            }
        }

        protected override void SetDefaults()
        {
            base.SetDefaults();
            MaxLabelWidth = 0;
            _labelDictionary = new System.Collections.Generic.Dictionary<double, AxisLabel>();
            _fontSize = Double.NaN;
            _interval = Double.NaN;
            Enabled = true;
            Rows = 0;
            
            _fontColor = null;
           
            this.FontFamily = "Verdana";
            _labelAngle = Double.NaN;
            TextWrap = 0.3;
            
        }

        private int CalculateFontSize()
        {
            int[] fontSizes = { 6, 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };
            Double _parentSize = (_parent.Parent as Chart).Width * (_parent.Parent as Chart).Height;
            int i = (int)(Math.Ceiling(((_parentSize + 10000) / 115000)));
            i = (i >= fontSizes.Length ? fontSizes.Length - 1 : i);
            return fontSizes[i];
        }

        private void CheckOutOfBounds()
        {
            foreach (AxisLabel lbl in this.Children)
            {
                if (_parent.AxisOrientation == AxisOrientation.Bar)
                {
                    if (lbl.ActualTop < 0)
                    {
                        if (Math.Abs(lbl.ActualTop) > (_parent.Parent as Chart).LabelPaddingTop)
                            (_parent.Parent as Chart).LabelPaddingTop = Math.Abs(lbl.ActualTop);
                    }
                    else if (((lbl.ActualTop + lbl.ActualHeight) > (Double)this.GetValue(HeightProperty)))
                    {
                        Double overflow = ((lbl.ActualTop + lbl.ActualHeight) - (Double)this.GetValue(HeightProperty));



                        if (overflow > (Double)((_parent.Parent as Chart).LabelPaddingBottom))
                            ((_parent.Parent as Chart).LabelPaddingBottom) = overflow;
                    }
                }
                else if (_parent.AxisOrientation == AxisOrientation.Column)
                {
                    if ((lbl.ActualLeft + lbl.ActualWidth) > (Double)this.GetValue(WidthProperty))
                    {
                        Double overflow = ((lbl.ActualLeft + lbl.ActualWidth) - (Double)this.GetValue(WidthProperty));



                        overflow = overflow / lbl.Position * (_parent.AxisMaximum - _parent.AxisMinimum);



                        if ((_parent.Parent as Chart).LabelPaddingRight < overflow)
                            (_parent.Parent as Chart).LabelPaddingRight = overflow;


                    }
                    else if (lbl.ActualLeft < 0)
                    {
                        Double overflow = Math.Abs(lbl.ActualLeft);



                        overflow = overflow / (_parent.AxisMaximum - lbl.Position) * (_parent.AxisMaximum - _parent.AxisMinimum);



                        if (overflow > (_parent.Parent as Chart).LabelPaddingLeft)
                            (_parent.Parent as Chart).LabelPaddingLeft = overflow;


                    }
                }


            }
        }
        #endregion Private Methods

        #region Data
        
        private Double _labelAngle;
        private Double _interval;
        private System.Collections.Generic.Dictionary<Double, AxisLabel> _labelDictionary;

        private String _fontFamily;
        private Double _fontSize;
        internal Brush _fontColor;
        private FontStyle _fontStyle;
        private FontWeight _fontWeight;
        
        private Axes _parent;
        private Double _rowHeight;
        private int _numberOfCalculatedRows;
        #endregion Data
    }
}
