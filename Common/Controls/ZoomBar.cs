using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Automation.Peers;

namespace Visifire.Commons.Controls
{
    [TemplatePart(Name = "HorizontalGripLeftElement", Type = typeof(Thumb))] 
    [TemplatePart(Name = "HorizontalLargeIncrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "HorizontalSmallIncrease", Type = typeof(RepeatButton))] 

    [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")] 
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]

    [TemplatePart(Name = "VerticalSmallIncrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "HorizontalRoot", Type = typeof(FrameworkElement))] 
    [TemplatePart(Name = "HorizontalLargeDecrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "HorizontalSmallDecrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "HorizontalThumb", Type = typeof(Thumb))] 
    [TemplatePart(Name = "VerticalRoot", Type = typeof(FrameworkElement))] 
    [TemplatePart(Name = "VerticalLargeIncrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "VerticalLargeDecrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "VerticalSmallDecrease", Type = typeof(RepeatButton))] 
    [TemplatePart(Name = "VerticalThumb", Type = typeof(Thumb))] 

    public class ZoomBar : RangeBase
    {
        #region Public Methods

        // Methods
        public ZoomBar()
        {
            ScrollEventFireEnabled = true;

            base.SizeChanged += delegate
            {
                this.UpdateTrackLayout(this.GetTrackLength());

                if (!_isGripDragged && !_isZoomedUsingZoomRect)
                    _currentThumbSize = Double.NaN;
            };

            base.DefaultStyleKey = typeof(ZoomBar);
        }

        /// <summary>
        /// On apply template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get reference of Child present in Template
            this._elementHorizontalTemplate = base.GetTemplateChild("HorizontalRoot") as FrameworkElement;
            this._elementHorizontalLargeIncrease = base.GetTemplateChild("HorizontalLargeIncrease") as RepeatButton;
            this._elementHorizontalLargeDecrease = base.GetTemplateChild("HorizontalLargeDecrease") as RepeatButton;
            this._elementHorizontalSmallIncrease = base.GetTemplateChild("HorizontalSmallIncrease") as RepeatButton;
            this._elementHorizontalSmallDecrease = base.GetTemplateChild("HorizontalSmallDecrease") as RepeatButton;
            this._elementHorizontalThumb = base.GetTemplateChild("HorizontalThumb") as Thumb;
            this._elementVerticalTemplate = base.GetTemplateChild("VerticalRoot") as FrameworkElement;
            this._elementVerticalLargeIncrease = base.GetTemplateChild("VerticalLargeIncrease") as RepeatButton;
            this._elementVerticalLargeDecrease = base.GetTemplateChild("VerticalLargeDecrease") as RepeatButton;
            this._elementVerticalSmallIncrease = base.GetTemplateChild("VerticalSmallIncrease") as RepeatButton;
            this._elementVerticalSmallDecrease = base.GetTemplateChild("VerticalSmallDecrease") as RepeatButton;
            this._elementVerticalThumb = base.GetTemplateChild("VerticalThumb") as Thumb;

            // Attch events
            this._elementHorizontalThumb.LayoutUpdated += new EventHandler(ElementLeftGrip_LayoutUpdated);

            if (this._elementHorizontalThumb != null)
            {
                this._elementHorizontalThumb.DragStarted += delegate(object sender, DragStartedEventArgs e)
                {
                    this.OnThumbDragStarted();
                };

                this._elementHorizontalThumb.DragDelta += delegate(object sender, DragDeltaEventArgs e)
                {
                    this.OnThumbDragDelta(e);
                };

                this._elementHorizontalThumb.DragCompleted += delegate(object sender, DragCompletedEventArgs e)
                {
                    this.OnThumbDragCompleted();
                };
            }

            if (this._elementHorizontalLargeDecrease != null)
            {
                this._elementHorizontalLargeDecrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.LargeDecrement();
                };
            }

            if (this._elementHorizontalLargeIncrease != null)
            {
                this._elementHorizontalLargeIncrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.LargeIncrement();
                };
            }

            if (this._elementHorizontalSmallDecrease != null)
            {
                this._elementHorizontalSmallDecrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.SmallDecrement();
                };
            }

            if (this._elementHorizontalSmallIncrease != null)
            {
                this._elementHorizontalSmallIncrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.SmallIncrement();
                };
            }

            if (this._elementVerticalThumb != null)
            {
                this._elementVerticalThumb.DragStarted += delegate(object sender, DragStartedEventArgs e)
                {
                    this.OnThumbDragStarted();
                };

                this._elementVerticalThumb.DragDelta += delegate(object sender, DragDeltaEventArgs e)
                {
                    this.OnThumbDragDelta(e);
                };

                this._elementVerticalThumb.DragCompleted += delegate(object sender, DragCompletedEventArgs e)
                {
                    this.OnThumbDragCompleted();
                };
            }

            if (this._elementVerticalLargeDecrease != null)
            {
                this._elementVerticalLargeDecrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.LargeDecrement();
                };

            }
            if (this._elementVerticalLargeIncrease != null)
            {
                this._elementVerticalLargeIncrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.LargeIncrement();
                };
            }

            if (this._elementVerticalSmallDecrease != null)
            {
                this._elementVerticalSmallDecrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.SmallDecrement();
                };

            }

            if (this._elementVerticalSmallIncrease != null)
            {
                this._elementVerticalSmallIncrease.Click += delegate(object sender, RoutedEventArgs e)
                {
                    this.SmallIncrement();
                };
            }

            this.OnOrientationChanged();
            this.UpdateVisualState();
        }

        public static IEnumerable<DependencyObject> GetVisuals(DependencyObject root)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);

                yield return child;

                foreach (var descendants in GetVisuals(child))
                {
                    yield return descendants;
                }
            }
        }

        #endregion

        #region Public Properties

 
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(ZoomBar),
            new PropertyMetadata(Orientation.Vertical,
            new PropertyChangedCallback(ZoomBar.OnOrientationPropertyChanged)));

        public static readonly DependencyProperty ViewportSizeProperty = DependencyProperty.Register("ViewportSize",
            typeof(double),
            typeof(ZoomBar),
            new PropertyMetadata(0.0, new PropertyChangedCallback(ZoomBar.OnViewportSizeChanged)));

        public static readonly DependencyProperty ZoomingEnabledProperty = DependencyProperty.Register("ZoomingEnabled",
            typeof(Boolean),
            typeof(ZoomBar),
            new PropertyMetadata(false, new PropertyChangedCallback(ZoomBar.OnZoomingEnabledChanged)));

        public static readonly DependencyProperty ScrollEventFireEnabledProperty = DependencyProperty.Register("ScrollEventFireEnabled",
           typeof(Boolean),
           typeof(ZoomBar),
           new PropertyMetadata(true, new PropertyChangedCallback(ZoomBar.OnScrollEventFireEnabledPropertyChanged)));

        /// <summary>
        /// Orientation of ZoomBar
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return (Orientation)base.GetValue(OrientationProperty);
            }
            set
            {
                base.SetValue(OrientationProperty, (Enum)value);
            }
        }

        /// <summary>
        /// ViewportSize 
        /// Note: Applicable while ZoomingEnabled is set to false
        /// </summary>
        public double ViewportSize
        {
            get
            {
                return (double)base.GetValue(ViewportSizeProperty);
            }
            set
            {
                base.SetValue(ViewportSizeProperty, value);
            }
        }

        /// <summary>
        /// Whether the scrollbar thumb is stretchable
        /// </summary>
        public Boolean IsStretchable
        {
            get 
            { 
                return (Boolean)GetValue(ZoomingEnabledProperty); 
            }
            set 
            { 
                SetValue(ZoomingEnabledProperty, value); 
            }
        }

        //Change it
        /// <summary>
        /// Zooming Scale
        /// </summary>
        internal Double Scale
        {   
            get; 
            set; 
        }

        internal Double ZoomBarStartPosition
        {
            get;
            private set;
        }

        internal Double ZoomBarEndPosition
        {   
            get;
            private set;
        }

        public Boolean ScrollEventFireEnabled
        {
            get
            {
                return (Boolean)base.GetValue(ScrollEventFireEnabledProperty);
            }
            set
            {
                base.SetValue(ScrollEventFireEnabledProperty, value);
            }
        }

        #endregion

        #region Public Events And Delegates

        // Events
        public event ScrollEventHandler Scroll;
        public event EventHandler ScaleChanged;
        public event EventHandler DragStarted;
        public event EventHandler DragCompleted;
        public event EventHandler Drag;

        #endregion

        #region Protected Methods

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            this.UpdateVisualState();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            if (!IsStretching)
            {
                System.Diagnostics.Debug.WriteLine("ValueChanged");
                double trackLength = this.GetTrackLength();
                base.OnMaximumChanged(oldMaximum, newMaximum);
                this.UpdateTrackLayout(trackLength);
            }
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            if (!IsStretching)
            {
                System.Diagnostics.Debug.WriteLine("ValueChanged");
                double trackLength = this.GetTrackLength();
                base.OnMinimumChanged(oldMinimum, newMinimum);
                this.UpdateTrackLayout(trackLength);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
#if SL
            this.IsMouseOver = true;
#endif
            if ((((this.Orientation == Orientation.Horizontal) && (this._elementHorizontalThumb != null)) && !this._elementHorizontalThumb.IsDragging) || (((this.Orientation == Orientation.Vertical) && (this._elementVerticalThumb != null)) && !this._elementVerticalThumb.IsDragging))
            {
                this.UpdateVisualState();
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
#if SL
            this.IsMouseOver = false;
#endif
            if ((((this.Orientation == Orientation.Horizontal) && (this._elementHorizontalThumb != null)) && !this._elementHorizontalThumb.IsDragging) || (((this.Orientation == Orientation.Vertical) && (this._elementVerticalThumb != null)) && !this._elementVerticalThumb.IsDragging))
            {
                this.UpdateVisualState();
            }
        }
        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                e.Handled = true;
                base.CaptureMouse();
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (!e.Handled)
            {
                e.Handled = true;
            }
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            if (_isNotificationEnabled)
            {
                System.Diagnostics.Debug.WriteLine("Max=" + Maximum.ToString() + " Min=" + Minimum.ToString() + " Value=" + newValue.ToString());

                double trackLength = this.GetTrackLength();

                if (trackLength >= 0)
                {
                    base.OnValueChanged(oldValue, newValue);
                    this.UpdateTrackLayout(trackLength);
                }
            }

            // TextBox.Text = _isNotificationEnabled.ToString() + ">, OldValue = " + oldValue.ToString() + " NewValue =" + newValue.ToString();
        }

        #endregion

        #region Internal Properties

        internal Boolean IsStretching
        {
            get;
            private set;
        }

        /// <summary>
        /// Current ThumbSize
        /// </summary>
        internal Double ThumbSize
        {
            get { return _currentThumbSize; }
        }



        /// <summary>
        /// Whether the Thumb is dragged
        /// </summary>
        internal bool IsDragging
        {
            get
            {
                if ((this.Orientation == Orientation.Horizontal) && (this._elementHorizontalThumb != null))
                {
                    return this._elementHorizontalThumb.IsDragging;
                }
                return (((this.Orientation == Orientation.Vertical) && (this._elementVerticalThumb != null)) && this._elementVerticalThumb.IsDragging);
                
            }
        }

#if SL
        /// <summary>
        /// Whether the mouse is over the Scroll Bar
        /// </summary>
        internal bool IsMouseOver
        {
            get;
            set;
        }

#endif
        #endregion

        #region Private Properties

        #endregion

        #region Private Delegates

        #endregion

        #region Private Methods

        private double ConvertViewportSizeToDisplayUnits(double trackLength)
        {   
            if (IsStretchable)
            {   
                if (Double.IsNaN(_currentThumbSize))
                {
                    //double num = base.Maximum - base.Minimum;
                    //return ((trackLength * this.ViewportSize) / (this.ViewportSize + num));
                    _currentThumbSize = trackLength;

                    _oldTrackLength = trackLength;

                    return _currentThumbSize;
                }
                else if (_oldTrackLength != trackLength)
                {
                    if (trackLength < _oldTrackLength)
                        _currentThumbSize = _currentThumbSize - Math.Abs(trackLength - _oldTrackLength);
                    else
                        _currentThumbSize = _currentThumbSize + Math.Abs(trackLength - _oldTrackLength);

                    _oldTrackLength = trackLength;

                    return _currentThumbSize;
                }
                else
                {
                    //_oldTrackLength = trackLength;

                    return _currentThumbSize;
                }
            }
            else
            {
                double num = base.Maximum - base.Minimum;

                //_oldTrackLength = trackLength;

                return ((trackLength * this.ViewportSize) / (this.ViewportSize + num));
            }
        }

        private void LargeDecrement()
        {
            if (!IsStretching)
            {
                double num = Math.Max(base.Value - base.LargeChange, base.Minimum);
                if (base.Value != num)
                {
                    base.Value = num;
                    this.RaiseScrollEvent(ScrollEventType.LargeDecrement);
                }
            }
        }

        private void LargeIncrement()
        {
            if (!IsStretching)
            {
                double num = Math.Min(base.Value + base.LargeChange, base.Maximum);
                if (base.Value != num)
                {
                    base.Value = num;
                    this.RaiseScrollEvent(ScrollEventType.LargeIncrement);
                }
            }
        }

        private void ElementLeftGrip_LayoutUpdated(object sender, EventArgs e)
        {
            SetReferenceOfGrips();
        }

        private void OnOrientationChanged()
        {
            double trackLength = this.GetTrackLength();
            if (this._elementHorizontalTemplate != null)
            {
                this._elementHorizontalTemplate.Visibility = (this.Orientation == Orientation.Horizontal) ? Visibility.Visible : Visibility.Collapsed;
            }
            if (this._elementVerticalTemplate != null)
            {
                this._elementVerticalTemplate.Visibility = (this.Orientation == Orientation.Horizontal) ? Visibility.Collapsed : Visibility.Visible;
            }
            this.UpdateTrackLayout(trackLength);
        }

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ZoomBar).OnOrientationChanged();
        }

        private void OnThumbDragCompleted()
        {
            if(!IsStretchable)
                this.RaiseScrollEvent(ScrollEventType.EndScroll);
        }

        private void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            if (!IsStretching)
            {
                System.Diagnostics.Debug.WriteLine("--Value change--");
                double d = 0.0;
                if ((this.Orientation == Orientation.Horizontal) && (this._elementHorizontalThumb != null))
                {
                    d = (e.HorizontalChange / (this.GetTrackLength() - this._elementHorizontalThumb.ActualWidth)) * (base.Maximum - base.Minimum);
                }
                else if ((this.Orientation == Orientation.Vertical) && (this._elementVerticalThumb != null))
                {   
                    d = (e.VerticalChange / (this.GetTrackLength() - this._elementVerticalThumb.ActualHeight)) * (base.Maximum - base.Minimum);
                }
                if (!double.IsNaN(d) && !double.IsInfinity(d))
                {   
                    this._dragValue += d;
                    double num2 = Math.Min(base.Maximum, Math.Max(base.Minimum, this._dragValue));
                    if (num2 != base.Value)
                    {
                        System.Diagnostics.Debug.WriteLine("--Value change--XXXXXXXXXXXX");
                        base.Value = num2;
                        this.RaiseScrollEvent(ScrollEventType.ThumbTrack);
                    }
                }
            }
        }

        private void OnThumbDragStarted()
        {
            this._dragValue = base.Value;
        }

        private static void OnViewportSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomBar bar = d as ZoomBar;

            if(!bar.IsStretchable)
                bar.UpdateTrackLayout(bar.GetTrackLength());
        }

        private static void OnScrollEventFireEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ZoomBar ZoomBar = d as ZoomBar;

            if (!(Boolean)e.OldValue && (Boolean)e.NewValue)
                ZoomBar.RaiseScrollEvent(ScrollEventType.ThumbPosition); 
        }

        private static void OnZoomingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {   
            Visibility visiblity = ((Boolean)e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
            ZoomBar bar = d as ZoomBar;

            if (bar._elementLeftGrip != null) bar._elementLeftGrip.Visibility = visiblity;
            if (bar._elementRightGrip != null) bar._elementRightGrip.Visibility = visiblity;
            if (bar._elementTopGrip != null) bar._elementTopGrip.Visibility = visiblity;
            if (bar._elementBottomGrip != null) bar._elementBottomGrip.Visibility = visiblity;
        }

        private void SmallDecrement()
        {
            if (!IsStretching)
            {   
                double num = Math.Max(base.Value - base.SmallChange, base.Minimum);
                if (base.Value != num)
                {
                    base.Value = num;
                    this.RaiseScrollEvent(ScrollEventType.SmallDecrement);
                }
            }
        }

        private void SmallIncrement()
        {   
            if (!IsStretching)
            {
                double num = Math.Min(base.Value + base.SmallChange, base.Maximum);
                if (base.Value != num)
                {
                    base.Value = num;
                    this.RaiseScrollEvent(ScrollEventType.SmallIncrement);
                }
            }
        }

        private void CalculateZoomStartPosition()
        {
            if (Orientation == Orientation.Horizontal)
                ZoomBarStartPosition = _elementHorizontalLargeDecrease.Width + _elementHorizontalSmallDecrease.ActualWidth;
            else
                ZoomBarStartPosition = _elementVerticalLargeDecrease.Height + _elementVerticalSmallDecrease.ActualHeight;
        }

        private void CalculateZoomEndPosition()
        {
            if (Orientation == Orientation.Horizontal)
                ZoomBarEndPosition = base.ActualWidth - (_elementHorizontalLargeDecrease.Width + _elementHorizontalSmallDecrease.ActualWidth + _currentThumbSize);
            else
                ZoomBarEndPosition = base.ActualHeight - (_elementVerticalLargeDecrease.Height + _elementVerticalSmallDecrease.ActualHeight + _currentThumbSize);
        }

        private Border ElementCenterBorder;

        /// <summary>
        ///  Finding Reference of GripControl inside Thumb Control
        /// </summary>
        private void SetReferenceOfGrips()
        {
            DragStartedEventHandler leftGripDragStartHandler = delegate(object sender, DragStartedEventArgs e)
            {
                IsStretching = true;

                CalculateZoomStartPosition();
                CalculateZoomEndPosition();
                
                if (DragStarted != null)
                    DragStarted(this, null);

             };

            DragStartedEventHandler rightGripDragStartHandler = delegate(object sender, DragStartedEventArgs e)
            {
                IsStretching = true;

                CalculateZoomStartPosition();
                CalculateZoomEndPosition();

                if (DragStarted != null)
                    DragStarted(this, null);
            };

            DragCompletedEventHandler handler3 = delegate(object sender1, DragCompletedEventArgs e1)
            {
                IsStretching = false;

                if (DragCompleted != null)
                    DragCompleted(this, null);
            };

            if (this.Orientation == Orientation.Horizontal)
            {
                Func<Thumb, Boolean> matchName = delegate(Thumb thumb) { return thumb.Name == "ElementLeftGrip"; };
                var grips = GetVisuals(_elementHorizontalThumb).OfType<Thumb>();
                var borders = GetVisuals(_elementHorizontalThumb).OfType<Border>();

                if (grips.Count() >= 2)
                {
                    // Find Control
                    _elementLeftGrip = grips.Where(matchName).First();

                    // Attach event handler
                    _elementLeftGrip.DragDelta += new DragDeltaEventHandler(ElementLeftGrip_DragDelta);
                    _elementLeftGrip.DragStarted += leftGripDragStartHandler;
                    _elementLeftGrip.DragCompleted += handler3;

                    // Find Control
                    matchName = delegate(Thumb thumb) { return thumb.Name == "ElementRightGrip"; };
                    _elementRightGrip = grips.Where(matchName).First();

                    // Attach event handler
                    _elementRightGrip.DragDelta += new DragDeltaEventHandler(ElementRightGrip_DragDelta);
                    _elementRightGrip.DragStarted += rightGripDragStartHandler;
                    _elementRightGrip.DragCompleted += handler3;

                    if (!IsStretchable)
                    {
                        _elementLeftGrip.Visibility = Visibility.Collapsed;
                        _elementRightGrip.Visibility = Visibility.Collapsed;
                    }

                    _elementHorizontalThumb.LayoutUpdated -= ElementLeftGrip_LayoutUpdated;
                }

                if (borders.Count() >= 1)
                {
                    Func<Border, Boolean> matchBorderName = delegate(Border border) { return border.Name == "ElementCenterBorder"; };
                    ElementCenterBorder = borders.Where(matchBorderName).First();
                }

            }
            else
            {
                // Find Control
                Func<Thumb, Boolean> matchName = delegate(Thumb thumb) { return thumb.Name == "ElementTopGrip"; };
                var grips = GetVisuals(_elementVerticalThumb).OfType<Thumb>();
                var borders = GetVisuals(_elementVerticalThumb).OfType<Border>();

                if (grips.Count() >= 2)
                {   
                    // Find Control
                    _elementTopGrip = grips.Where(matchName).First();
                    
                    // Attach event handler
                    _elementTopGrip.DragDelta += new DragDeltaEventHandler(ElementLeftGrip_DragDelta);
                    _elementTopGrip.DragStarted += leftGripDragStartHandler;
                    _elementTopGrip.DragCompleted += handler3;

                    // Find Control
                    matchName = delegate(Thumb thumb) { return thumb.Name == "ElementBottomGrip"; };
                    _elementBottomGrip = grips.Where(matchName).First();

                    // Attach event handler
                    _elementBottomGrip.DragDelta += new DragDeltaEventHandler(ElementRightGrip_DragDelta);
                    _elementBottomGrip.DragStarted += leftGripDragStartHandler;
                    _elementBottomGrip.DragCompleted += handler3;

                    if (!IsStretchable)
                    {
                        _elementTopGrip.Visibility = Visibility.Collapsed;
                        _elementBottomGrip.Visibility = Visibility.Collapsed;
                    }

                    _elementHorizontalThumb.LayoutUpdated -= ElementLeftGrip_LayoutUpdated;
                }

                if (borders.Count() >= 1)
                {
                    Func<Border, Boolean> matchBorderName = delegate(Border border) { return border.Name == "ElementCenterBorder"; };
                    ElementCenterBorder = borders.Where(matchBorderName).First();
                }
            }

            if (ElementCenterBorder != null)
            {
                ElementCenterBorder.Visibility = IsStretchable ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        
        /// <summary>
        /// Calculate and update Values
        /// </summary>
        /// <param name="trackLength">Length of the track</param>
        private void CalculateAndUpdateValues4LeftGrip(Double trackLength)
        {
            Double movableTrackLength = trackLength - _currentThumbSize;
            Double value = 0;
            
            if (movableTrackLength <= 0)
            {
                if (Orientation == Orientation.Horizontal)
                    _elementHorizontalLargeDecrease.Width = 0;
                else
                    _elementVerticalLargeDecrease.Height = 0;

                value = (Maximum - Minimum) / 2;
            }
            else
            {   
                Double leftGap = (Orientation == Orientation.Horizontal)? _elementHorizontalLargeDecrease.Width :_elementVerticalLargeDecrease.Height;
                Double rightGap = trackLength - (leftGap + _currentThumbSize);
                
                Double maxThumbIndicatorPosition = trackLength - _currentThumbSize / 2;
                Double currentThumbIndicatiorPosition = leftGap + _currentThumbSize / 2;
                Double minThumbIndicatorPosition = _currentThumbSize/2;
                
                Double pixPosPercent = (currentThumbIndicatiorPosition - minThumbIndicatorPosition) / (maxThumbIndicatorPosition - minThumbIndicatorPosition);

                //if(Orientation == Orientation.Horizontal)
                    value = (Maximum - Minimum) * pixPosPercent;
                //else
                //    value = Maximum - (Maximum - Minimum) * pixPosPercent;
                
                System.Diagnostics.Debug.WriteLine("Left - Value=" + Value.ToString());
            }

            if (Value != value)
            {   
                _isNotificationEnabled = false;
                Value = value;
                _isNotificationEnabled = true;
            }

            Scale = _currentThumbSize / trackLength;

            if (ScaleChanged != null)
                ScaleChanged(this, new EventArgs());

            this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
        }
        
        /// <summary>
        /// Calculate and update Values
        /// </summary>
        /// <param name="trackLength">Length of the track</param>
        private void CalculateAndUpdateValues4RightGrip(Double trackLength)
        {   
            System.Diagnostics.Debug.WriteLine("ValueChanged-X");

            //if (trackLength < 0)
            //    return;

            Double movableTrackLength = trackLength - _currentThumbSize;

            if (movableTrackLength <= 0)
            {
                if (Orientation == Orientation.Horizontal)
                    _elementHorizontalLargeDecrease.Width = 0;
                else
                    _elementVerticalLargeDecrease.Height = 0;

                //if (Value != 0)
                {
                    _isNotificationEnabled = false;
                    Value = 0;

                    System.Diagnostics.Debug.WriteLine("Me--1");
                    if (Value == Maximum)
                        System.Diagnostics.Debug.WriteLine("Value=1");

                    Scale = _currentThumbSize / trackLength;

                    if (ScaleChanged != null)
                        ScaleChanged(this, new EventArgs());

                    _isNotificationEnabled = true;
                    this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
                }

                return;
            }

            _isNotificationEnabled = false;

            if (Orientation == Orientation.Horizontal)
            {   
                double value = ((_elementHorizontalLargeDecrease.ActualWidth / (trackLength - _currentThumbSize)) * (Maximum - Minimum)) - Minimum;
                
                if (!Double.IsNaN(value) && Value != value)
                {   
                    System.Diagnostics.Debug.WriteLine("Me--2");
                    Value = value;
                    if (Value == Maximum)
                        System.Diagnostics.Debug.WriteLine("Value=1");
                    // this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
                }
            }
            else
            {   
                double value = ((_elementVerticalLargeDecrease.Height / (trackLength - _currentThumbSize)) * (Maximum - Minimum)) - Minimum;
                if (!Double.IsNaN(value) && Value != value)
                {
                    System.Diagnostics.Debug.WriteLine("Me--2");
                    Value = value;
                    if (Value == Maximum)
                        System.Diagnostics.Debug.WriteLine("Value=1");
                    //this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
                }
            }

            _isNotificationEnabled = true;

            Scale = _currentThumbSize / trackLength;

            if (ScaleChanged != null)
                ScaleChanged(this, new EventArgs());

            this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
            System.Diagnostics.Debug.WriteLine("Right - Value=" + Value.ToString());
            //TextBox.Text += ">Scale=" + Scale.ToString();
        }

        /// <summary>
        /// ElementRightGrip_DragDelta
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DragDeltaEventArgs</param>
        private void ElementRightGrip_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _isGripDragged = true;
            Double oldThumbSize = _currentThumbSize;
            Double trackLength = GetTrackLength();

            if (Orientation == Orientation.Horizontal)
            {   
                _currentThumbSize += e.HorizontalChange;

                if (_currentThumbSize > (trackLength - _elementHorizontalLargeDecrease.ActualWidth))
                    _currentThumbSize = trackLength - _elementHorizontalLargeDecrease.ActualWidth;
            }
            else
            {   
                _currentThumbSize += e.VerticalChange;

                Double verticalLargeDec = trackLength - _currentThumbSize;

                if (_currentThumbSize > (trackLength - _elementVerticalLargeDecrease.ActualHeight))
                    _currentThumbSize = trackLength - _elementVerticalLargeDecrease.ActualHeight;
            }

            UpdateThumbSize(trackLength);
            CalculateAndUpdateValues4RightGrip(trackLength);

            CalculateZoomEndPosition();

            if (Drag != null)
                Drag(this, null);
        }


        internal void UpdateScale(Double scale)
        {
            Double oldThumbSize = _currentThumbSize;
            Double trackLength = GetTrackLength();
            Scale = scale;
            _currentThumbSize = scale * trackLength;
            //UpdateThumbSize(trackLength);
            UpdateTrackLayout(trackLength);
            CalculateZoomEndPosition();
        }

        /// <summary>
        /// ElementLeftGrip_DragDelta
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">DragDeltaEventArgs</param>
        private void ElementLeftGrip_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _isGripDragged = true;
            Double oldThumbSize = _currentThumbSize;
            Double trackLength = GetTrackLength();
            Double deltaChange = (Orientation == Orientation.Horizontal) ? e.HorizontalChange : e.VerticalChange;

            if (deltaChange < 0)
            {
                
                Double tempNewThumbSize = _currentThumbSize;
                tempNewThumbSize -= deltaChange;

                if (Orientation == Orientation.Horizontal)
                {
                    Double maxThumbSize = trackLength;
                    tempNewThumbSize = Math.Max(_elementHorizontalThumb.MinWidth, Math.Min(maxThumbSize, tempNewThumbSize));

                    Double largeDec = _elementHorizontalLargeDecrease.Width - Math.Abs(oldThumbSize - tempNewThumbSize);

                    if (largeDec < 0)
                    {
                        largeDec = 0;
                        _currentThumbSize += _elementHorizontalLargeDecrease.Width;
                    }
                    else
                        _currentThumbSize = tempNewThumbSize;

                    _elementHorizontalLargeDecrease.Width = largeDec;
                    
                }
                else
                {
                    Double maxThumbSize = trackLength;
                    tempNewThumbSize = Math.Max(_elementVerticalThumb.MinHeight, Math.Min(maxThumbSize, tempNewThumbSize));

                    Double largeDec = _elementVerticalLargeDecrease.Height - Math.Abs(oldThumbSize - tempNewThumbSize);

                    if (largeDec < 0)
                    {
                        largeDec = 0;
                        _currentThumbSize += _elementVerticalLargeDecrease.Height;
                    }
                    else
                        _currentThumbSize = tempNewThumbSize;

                    _elementVerticalLargeDecrease.Height = largeDec;
                }

            }
            else if (deltaChange > 0)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    if (!(_elementHorizontalLargeDecrease.Width == 0 && _currentThumbSize == _elementHorizontalThumb.MinWidth))
                    {
                        if ((_currentThumbSize - deltaChange) > _elementHorizontalThumb.MinWidth)
                        {
                            _currentThumbSize -= deltaChange;

                            Double largeDec = _elementHorizontalLargeDecrease.Width + Math.Abs(oldThumbSize - _currentThumbSize);
                            _elementHorizontalLargeDecrease.Width = largeDec;
                        }
                    }
                }
                else
                {
                    if (!(_elementVerticalLargeDecrease.Height == 0 && _currentThumbSize == _elementVerticalThumb.MinHeight))
                    {   
                        if ((_currentThumbSize - deltaChange) > _elementVerticalThumb.MinHeight)
                        {
                            _currentThumbSize -= deltaChange;

                            Double largeDec = _elementVerticalLargeDecrease.Height + Math.Abs(oldThumbSize - _currentThumbSize);
                            _elementVerticalLargeDecrease.Height = largeDec;
                        }
                    }
                }
            }

            UpdateThumbSize(trackLength);
            
            CalculateAndUpdateValues4LeftGrip(trackLength);

            CalculateZoomStartPosition();

            if (Drag != null)
                Drag(this, null);
        }

        /// <summary>
        /// Update laypout of the trackTrackLayout
        /// </summary>
        /// <param name="trackLength">Length of the track</param>
        private void UpdateTrackLayout(double trackLength)
        {
            double maximum = base.Maximum;
            double minimum = base.Minimum;
            double num4 = (base.Value - minimum) / (maximum - minimum);
            double num5 = this.UpdateThumbSize(trackLength);

            if (((this.Orientation == Orientation.Horizontal) && (this._elementHorizontalLargeDecrease != null)) && (this._elementHorizontalThumb != null))
            {
                this._elementHorizontalLargeDecrease.Width = Math.Max((double)0.0, (double)(num4 * (trackLength - num5)));
            }
            else if (((this.Orientation == Orientation.Vertical) && (this._elementVerticalLargeDecrease != null)) && (this._elementVerticalThumb != null))
            {
                this._elementVerticalLargeDecrease.Height = Math.Max((double)0.0, (double)(num4 * (trackLength - num5)));
            }

            if(ElementCenterBorder != null)
            {
                ElementCenterBorder.Visibility = IsStretchable ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Update layout of the ZoomBar
        /// </summary>
        internal void UpdateTrackLayout()
        {
            UpdateTrackLayout(GetTrackLength());
        }

        /// <summary>
        /// Reset ResetThumSize to initial size
        /// </summary>
        internal void ResetThumSize()
        {
            _currentThumbSize = Double.NaN;
        }

        /// <summary>
        /// Get length of the zoom bar track
        /// </summary>
        /// <returns>Track length</returns>
        private double GetTrackLength()
        {
            double trackLength = double.NaN;

            if (this.Orientation == Orientation.Horizontal)
            {   
                trackLength = base.ActualWidth;

                if (this._elementHorizontalSmallDecrease != null)
                {
                    trackLength -= (this._elementHorizontalSmallDecrease.ActualWidth + this._elementHorizontalSmallDecrease.Margin.Left) + this._elementHorizontalSmallDecrease.Margin.Right;
                }
                if (this._elementHorizontalSmallIncrease != null)
                {
                    trackLength -= (this._elementHorizontalSmallIncrease.ActualWidth + this._elementHorizontalSmallIncrease.Margin.Left) + this._elementHorizontalSmallIncrease.Margin.Right;
                }
                return trackLength;
            }
            trackLength = base.ActualHeight;
            if (this._elementVerticalSmallDecrease != null)
            {
                trackLength -= (this._elementVerticalSmallDecrease.ActualHeight + this._elementVerticalSmallDecrease.Margin.Top) + this._elementVerticalSmallDecrease.Margin.Bottom;
            }
            if (this._elementVerticalSmallIncrease != null)
            {
                trackLength -= (this._elementVerticalSmallIncrease.ActualHeight + this._elementVerticalSmallIncrease.Margin.Top) + this._elementVerticalSmallIncrease.Margin.Bottom;
            }

            return trackLength;
        }

        /// <summary>
        /// Update the size of the Thumb
        /// </summary>
        /// <param name="trackLength">Length of the track</param>
        /// <returns>ThumbSize as Double</returns>
        internal double UpdateThumbSize(double trackLength)
        {
            double thumbSize = double.NaN;
            bool flag = trackLength <= 0.0;
            if (trackLength > 0.0)
            {
                if ((this.Orientation == Orientation.Horizontal) && (this._elementHorizontalThumb != null))
                {   
                    if ((base.Maximum - base.Minimum) != 0.0)
                    {
                        thumbSize = Math.Max(this._elementHorizontalThumb.MinWidth, this.ConvertViewportSizeToDisplayUnits(trackLength));
                    }

                    if ((((base.Maximum - base.Minimum) == 0.0) || (thumbSize > base.ActualWidth)) || (trackLength <= this._elementHorizontalThumb.MinWidth))
                    {
                        flag = true;
                    }
                    else
                    {
                        this._elementHorizontalThumb.Visibility = Visibility.Visible;
                        this._elementHorizontalThumb.Width = thumbSize;
                    }
                }
                else if ((this.Orientation == Orientation.Vertical) && (this._elementVerticalThumb != null))
                {
                    if ((base.Maximum - base.Minimum) != 0.0)
                    {
                        thumbSize = Math.Max(this._elementVerticalThumb.MinHeight, this.ConvertViewportSizeToDisplayUnits(trackLength));
                    }
                    if ((((base.Maximum - base.Minimum) == 0.0) || (thumbSize > base.ActualHeight)) || (trackLength <= this._elementVerticalThumb.MinHeight))
                    {
                        flag = true;
                    }
                    else
                    {
                        this._elementVerticalThumb.Visibility = Visibility.Visible;
                        this._elementVerticalThumb.Height = thumbSize;
                    }
                }
            }
            if (flag && !IsStretchable)
            {   
                if (this._elementHorizontalThumb != null)
                {
                    this._elementHorizontalThumb.Visibility = Visibility.Collapsed;
                }
                if (this._elementVerticalThumb != null)
                {
                    this._elementVerticalThumb.Visibility = Visibility.Collapsed;
                }

                System.Diagnostics.Debug.WriteLine("----------3");
            }

            _currentThumbSize = thumbSize;

            Scale = _currentThumbSize / trackLength;

            return thumbSize;
        }

        /// <summary>
        ///  Update Visual State
        /// </summary>
        internal void UpdateVisualState()
        {
            // Not implemented, Currently all Visual states are  defined in generic XAML
        }

        /// <summary>
        /// Raise Scroll Event
        /// </summary>
        /// <param name="scrollEventType">ScrollEventType</param>
        internal void RaiseScrollEvent(ScrollEventType scrollEventType)
        {
            ScrollEventArgs e = new ScrollEventArgs(scrollEventType, base.Value);
            
            if (this.Scroll != null && ScrollEventFireEnabled)
            {
                this.Scroll(this, e);
            }
        }

        #endregion

        #region Internal Events And Delegates

        #endregion

        #region Data

        // Fields
        //internal TextBox TextBox;

        // Horizontal element references
        internal RepeatButton _elementHorizontalLargeDecrease;
        internal RepeatButton _elementHorizontalLargeIncrease;

        internal RepeatButton _elementHorizontalSmallDecrease;
        internal RepeatButton _elementHorizontalSmallIncrease;
        internal FrameworkElement _elementHorizontalTemplate;
        internal Thumb _elementHorizontalThumb;
        internal Thumb _elementLeftGrip;
        internal Thumb _elementRightGrip;

        // Vertical element references
        internal RepeatButton _elementVerticalLargeDecrease;
        internal RepeatButton _elementVerticalLargeIncrease;
        internal RepeatButton _elementVerticalSmallDecrease;
        internal RepeatButton _elementVerticalSmallIncrease;
        internal FrameworkElement _elementVerticalTemplate;
        internal Thumb _elementVerticalThumb;
        internal Thumb _elementTopGrip;
        internal Thumb _elementBottomGrip;

        // Element names
        internal const string ElementHorizontalLargeDecreaseName = "HorizontalLargeDecrease";
        internal const string ElementHorizontalLargeIncreaseName = "HorizontalLargeIncrease";
        internal const string ElementHorizontalSmallDecreaseName = "HorizontalSmallDecrease";
        internal const string ElementHorizontalSmallIncreaseName = "HorizontalSmallIncrease";
        internal const string ElementHorizontalTemplateName = "HorizontalRoot";
        internal const string ElementHorizontalThumbName = "HorizontalThumb";
        internal const string ElementLeftGripName = "ElementLeftGrip";
        internal const string ElementRightGripName = "ElementRightGrip";
        internal const string ElementVerticalLargeDecreaseName = "VerticalLargeDecrease";
        internal const string ElementVerticalLargeIncreaseName = "VerticalLargeIncrease";
        internal const string ElementVerticalSmallDecreaseName = "VerticalSmallDecrease";
        internal const string ElementVerticalSmallIncreaseName = "VerticalSmallIncrease";
        internal const string ElementVerticalTemplateName = "VerticalRoot";
        internal const string ElementVerticalThumbName = "VerticalThumb";
        internal const string ElementTopGripName = "ElementTopGrip";
        internal const string ElementBottomGripName = "ElementBottomGrip";
        internal const string GroupCommon = "CommonStates";
        internal const string StateDisabled = "Disabled";
        internal const string StateMouseOver = "MouseOver";
        internal const string StateNormal = "Normal";

        // Drag value
        private double _dragValue;

        private Boolean _isGripDragged = false;

        internal Boolean _isZoomedUsingZoomRect = false;

        // Current size of the thumb
        private Double _currentThumbSize = Double.NaN;

        private Double _oldTrackLength = Double.NaN;

        // Whether notication is enabled for the Value changed event
        internal Boolean _isNotificationEnabled = true;

        internal Double _internalValue;

        #endregion
    }
}
