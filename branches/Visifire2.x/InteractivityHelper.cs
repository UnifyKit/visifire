using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Visifire.Commons;
using Visifire.Charts;

internal class InteractivityHelper
{
    public InteractivityHelper()
	{
        
	}

    /// <summary>
    /// Apply border effect
    /// </summary>
    /// <param name="shape">Shape</param>
    /// <param name="borderStyles">BorderStyles</param>
    /// <param name="borderThickness">Double</param>
    /// <param name="borderColor">Brush</param>
    public static void ApplyBorderEffect(Shape shape, BorderStyles borderStyles, Double borderThickness, Brush borderColor)
    {
        shape.Stroke = borderColor; 
        shape.StrokeThickness = borderThickness;
        shape.StrokeStartLineCap = PenLineCap.Round;
        shape.StrokeEndLineCap = PenLineCap.Round;
        shape.StrokeDashOffset = 0.4;
        shape.StrokeLineJoin = PenLineJoin.Bevel;
        shape.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(borderStyles.ToString());

    }

    /// <summary>
    /// Apply border effect
    /// </summary>
    public static void ApplyBorderEffect(Shape shape, BorderStyles borderStyles, Brush fillColor, Double scaleFactor, Double borderThickness, Brush borderColor)
    {
        ApplyBorderEffect(shape, borderStyles, borderThickness, borderColor);
        shape.Fill = fillColor;
        shape.Height *= scaleFactor;
        shape.Width *= scaleFactor;

        shape.SetValue(Canvas.TopProperty, -shape.Height / 2);
        shape.SetValue(Canvas.LeftProperty, -shape.Width / 2);
    }

    /// <summary>
    /// Remove border effect
    /// </summary>
    public static void RemoveBorderEffect(Shape shape, BorderStyles borderStyle, Double borderThickness, Brush lineColor)
    {   
        shape.Stroke = lineColor;
        shape.StrokeThickness = borderThickness;
        shape.StrokeStartLineCap = PenLineCap.Flat;
        shape.StrokeEndLineCap = PenLineCap.Flat;
        shape.StrokeDashOffset = 0;
        shape.StrokeLineJoin = PenLineJoin.Bevel;
        shape.StrokeDashArray = Graphics.LineStyleToStrokeDashArray(borderStyle.ToString());
    }

    /// <summary>
    /// Remove border effect
    /// </summary>
    public static void RemoveBorderEffect(Shape shape, BorderStyles borderStyle, Double borderThickness, Brush lineColor, Brush fillColor, Double width, Double height)
    {
        RemoveBorderEffect(shape, borderStyle, borderThickness, lineColor);
        shape.Fill = fillColor;
        shape.Height = height;
        shape.Width = width;

        shape.SetValue(Canvas.TopProperty, -shape.Height / 2);
        shape.SetValue(Canvas.LeftProperty, -shape.Width / 2);
    }
    
    /// <summary>
    /// Apply on mouse over opacity interactivity to a framework element
    /// </summary>
    /// <param name="element"></param>
    public static void ApplyOnMouseOverOpacityInteractivity(FrameworkElement element)
    {
        RemoveOnMouseOverOpacityInteractivity(element, Double.NaN);
        element.MouseEnter += new MouseEventHandler(element_MouseEnter);
        element.MouseLeave += new MouseEventHandler(element_MouseLeave);
    }

    /// <summary>
    /// Apply on mouse over opacity interactivity to all associated visual elements
    /// </summary>
    /// <param name="element"></param>
    public static void ApplyOnMouseOverOpacityInteractivity2Visuals(FrameworkElement element)
    {
        element.MouseEnter += new MouseEventHandler(MultipleElements_MouseEnter);
        element.MouseLeave += new MouseEventHandler(MultipleElements_MouseLeave);
    }

    /// <summary>
    /// MouseMove event handler for removing opacity effect on multiple segments
    /// </summary>
    /// <param name="sender">FrameworkElement</param>
    /// <param name="e">MouseEventArgs</param>
    private static void MultipleElements_MouseLeave(object sender, MouseEventArgs e)
    {
        FrameworkElement fe = sender as FrameworkElement;

        RemoveOpacity(sender as FrameworkElement);

        DataPoint dp = (fe.Tag as ElementData).Element as DataPoint;

        foreach (FrameworkElement fe1 in dp.Faces.VisualComponents)
        {
            if (fe1 != sender)
                RemoveOpacity(fe1 as FrameworkElement);
        }
    }

    /// <summary>
    /// MouseEnter event handler for applying opacity effect on multiple segments
    /// </summary>
    /// <param name="sender">FrameworkElement</param>
    /// <param name="e">MouseEventArgs</param>
    private static void MultipleElements_MouseEnter(object sender, MouseEventArgs e)
    {
        FrameworkElement fe = sender as FrameworkElement;

        ApplyOpacity(sender as FrameworkElement);

        DataPoint dp = (fe.Tag as ElementData).Element as DataPoint;

        foreach (FrameworkElement fe1 in dp.Faces.VisualComponents)
        {   
            if (fe1 != sender)
                ApplyOpacity(fe1 as FrameworkElement);
        }
       
    }

#if WPF

    /// <summary>
    /// Detach OpacityProperty of a Frameworkelement from a Storyboard
    /// </summary>
    /// <param name="fe">FrameworkElement</param>
    /// <param name="resetTo">Double</param>
    public static void DetachOpacityPropertyFromAnimation(FrameworkElement fe, Double resetTo)
    {
        fe.BeginAnimation(FrameworkElement.OpacityProperty, null);
        fe.Opacity = resetTo;
    }

#endif

    /// <summary>
    /// Remove OnMouseOver opacity interactivity
    /// </summary>
    /// <param name="element"></param>
    /// <param name="resetOpacity"></param>
    public static void RemoveOnMouseOverOpacityInteractivity(FrameworkElement element, Double resetOpacity)
    {
        if (!Double.IsNaN(resetOpacity))
            element.Opacity = resetOpacity;

        element.MouseEnter -= new MouseEventHandler(element_MouseEnter);
        element.MouseLeave -= new MouseEventHandler(element_MouseLeave);

        element.MouseEnter -= new MouseEventHandler(MultipleElements_MouseEnter);
        element.MouseLeave -= new MouseEventHandler(MultipleElements_MouseLeave);
    }

    /// <summary>
    /// Event handler for MouseLeave event of a FrameworkElement
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">MouseEventArgs</param>
    private static void element_MouseLeave(object sender, MouseEventArgs e)
    {
        RemoveOpacity(sender as FrameworkElement); 
    }
    
    /// <summary>
    /// Event handler for MouseEnter event of a FrameworkElement
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">MouseEventArgs</param>
    private static void element_MouseEnter(object sender, MouseEventArgs e)
    {
        ApplyOpacity(sender as FrameworkElement); 
    }

    /// <summary>
    /// Apply opacity 
    /// </summary>
    /// <param name="obj">FrameworkElement</param>
    private static void ApplyOpacity(FrameworkElement obj)
    {   
        if (obj != null)
        {
            obj.Opacity *= OPACITY_FACTOR;
        }
    }

    /// <summary>
    /// Remove opacity
    /// </summary>
    /// <param name="obj">FrameworkElement</param>
    private static void RemoveOpacity(FrameworkElement obj)
    {
        if (obj != null)
        {
            obj.Opacity /= OPACITY_FACTOR;
        }
    }

    /// <summary>
    /// Opacity effect factor
    /// </summary>
    private static Double OPACITY_FACTOR = 0.6;
}
