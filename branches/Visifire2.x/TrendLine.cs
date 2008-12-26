#if WPF

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


#else
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


#endif
using Visifire.Commons;

namespace Visifire.Charts
{

    public class TrendLine : ObservableObject
    {
        public TrendLine()
        {
        }

        #region Internal Properties
        internal Axis ReferingAxis
        {
            get;
            set;
        }
        #endregion Internal Properties

        #region Internal Methods

        internal override void UpdateVisual(string PropertyName, object Value)
        {
            if (Line == null || Shadow == null)
                FirePropertyChanged(PropertyName);
            else
                ApplyProperties();
        }

        #endregion

        #region Public Properties

        [System.ComponentModel.TypeConverter(typeof(NullableBoolConverter))]
        public Nullable<Boolean> Enabled
        {
            get
            {
                if ((Nullable<Boolean>)GetValue(EnabledProperty) == null)
                    return true;
                else
                    return (Nullable<Boolean>)GetValue(EnabledProperty);
            }
            set
            {
                SetValue(EnabledProperty, value);
            }
        }

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register
            ("Enabled",
            typeof(Nullable<Boolean>),
            typeof(TrendLine),
            new PropertyMetadata(OnEnabledPropertyChanged));

        private static void OnEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Enabled");
        }

        #region LineColor

        public Brush LineColor
        {
            get { return (Brush)GetValue(LineColorProperty) == null ? new SolidColorBrush(Colors.Red) : (Brush)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(
            "LineColor",
            typeof(Brush),
            typeof(TrendLine),
            new PropertyMetadata(OnLineColorPropertyChanged));

        private static void OnLineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            //source.FirePropertyChanged("LineColor");
            source.UpdateVisual("LineColor", e.NewValue);
        }

        #endregion  LineColor

        #region LineThickness

        public Double LineThickness
        {
            get { return (Double)GetValue(LineThicknessProperty) == 0 ? 2 : (Double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register(
            "LineThickness",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(OnLineThicknessPropertyChanged));

        private static void OnLineThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            //source.FirePropertyChanged("LineThickness");
            source.UpdateVisual("LineThickness", e.NewValue);
        }

        #endregion  LineThickness

        #region LineStyle

        public LineStyles LineStyle
        {
            get { return (LineStyles)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register(
            "LineStyle",
            typeof(LineStyles),
            typeof(TrendLine),
            new PropertyMetadata(OnLineStylePropertyChanged));

        private static void OnLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            //source.FirePropertyChanged("LineStyle");
            source.UpdateVisual("LineStyle", e.NewValue);
        }

        #endregion  LineStyle

        #region ShadowEnabled

        public bool ShadowEnabled
        {
            get { return (bool)GetValue(ShadowEnabledProperty); }
            set { SetValue(ShadowEnabledProperty, value); }
        }

        public static readonly DependencyProperty ShadowEnabledProperty =
            DependencyProperty.Register(
            "ShadowEnabled",
            typeof(bool),
            typeof(TrendLine),
            new PropertyMetadata(OnShadowEnabledPropertyChanged));

        private static void OnShadowEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            //source.FirePropertyChanged("ShadowEnabled");
            source.UpdateVisual("ShadowEnabled", e.NewValue);
        }

        #endregion  ShadowEnabled

        #region Value

        public Double Value
        {
            get { return (Double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
            "Value",
            typeof(Double),
            typeof(TrendLine),
            new PropertyMetadata(OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Value");
        }

        #endregion  Value

        #region AxisType

        public AxisTypes AxisType
        {
            get { return (AxisTypes)GetValue(AxisTypeProperty); }
            set { SetValue(AxisTypeProperty, value); }
        }
        public static readonly DependencyProperty AxisTypeProperty =
            DependencyProperty.Register(
            "AxisType",
            typeof(AxisTypes),
            typeof(TrendLine),
            new PropertyMetadata(OnAxisTypePropertyChanged));

        private static void OnAxisTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("AxisType");
        }

        #endregion  AxisType

        #region Orientation
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(TrendLine),
            new PropertyMetadata(OnOrientationPropertyChanged));

        private static void OnOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrendLine source = d as TrendLine;
            source.FirePropertyChanged("Orientation");
        }

        #endregion  Orientation

        public Canvas Visual
        {
            get;
            set;
        }

        /// <summary>
        /// Href text property for the TrendLine
        /// </summary>
        public String Href
        {
            get;
            set;
        }

        public HrefTargets HrefTarget
        {
            get;
            set;
        }

        #endregion Public Properties

        #region Private Properties
        private Line Line
        {
            get;
            set;
        }
        private Line Shadow
        {
            get;
            set;
        }
        
        #endregion Private Properties
        
        #region Public Methods

        private void ApplyProperties()
        {         
            Line.Stroke = LineColor;
            Line.StrokeThickness = LineThickness;
            Line.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            Shadow.StrokeThickness = LineThickness + 2;
            Shadow.StrokeDashArray = ExtendedGraphics.GetDashArray(LineStyle);

            Shadow.Stroke = new SolidColorBrush(Colors.LightGray);
            Shadow.Opacity = 0.7;

            Shadow.StrokeDashCap = PenLineCap.Round;
            Shadow.StrokeLineJoin = PenLineJoin.Round;
            Shadow.Visibility = ShadowEnabled ? Visibility.Visible : Visibility.Collapsed;
        }
        
        public void CreateVisualObject(Double width, Double height)
        {
            if (ReferingAxis == null || !(Boolean)Enabled)
            {
                Visual = null;
                return;
            }

            Visual = new Canvas();
            Visual.Opacity = this.Opacity;
            Double shadowThickness = LineThickness + 2;
            Line = new Line();
            Shadow = new Line();

            ApplyProperties();

            switch (Orientation)
            {
                case Orientation.Vertical:
                    Line.Y1 = 0;
                    Line.Y2 = height;
                    Line.X1 = Graphics.ValueToPixelPosition(0,
                                        width,
                                        (Double) ReferingAxis.InternalAxisMinimum,
                                        (Double) ReferingAxis.InternalAxisMaximum,
                                        Value);
                    Line.X2 = Line.X1;
                    Visual.Height = height;
                    Visual.Width = shadowThickness;
                    break;
                case Orientation.Horizontal:
                    Line.X1 = 0;
                    Line.X2 = width;
                    Line.Y1 = Graphics.ValueToPixelPosition(height,
                                        0,
                                        (Double)ReferingAxis.InternalAxisMinimum,
                                        (Double)ReferingAxis.InternalAxisMaximum,
                                        Value);
                    Line.Y2 = Line.Y1;
                    Visual.Height = shadowThickness;
                    Visual.Width = width;
                    break;

            }

            if (Orientation == Orientation.Horizontal)
            {
                Shadow.X1 = Line.X1;
                Shadow.X2 = Line.X2 + 3;
                Shadow.Y1 = Line.Y1 + 3;
                Shadow.Y2 = Line.Y2 + 3;
            }
            else
            {
                Shadow.X1 = Line.X1 + 3;
                Shadow.X2 = Line.X2 + 3;
                Shadow.Y1 = Line.Y1 + 3;
                Shadow.Y2 = Line.Y2;
            }

            Visual.Children.Add(Shadow);
            Visual.Children.Add(Line);

            ObservableObject.AttachToolTip(ReferingAxis.Chart, Line, ToolTipText);
            ObservableObject.AttachHref(ReferingAxis.Chart, Line, Href, HrefTarget);
        }

        #endregion Public Methods

        #region Data

        #endregion Data
    }
}
