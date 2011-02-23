using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Visifire.Charts
{
    public static class HeatMapHelper
    {
        public static readonly Matrix TransformToDevice;
        public static readonly double DpiX;
        public static readonly double DpiY;

        static HeatMapHelper()
        {
            var source = PresentationSource.FromVisual(Application.Current.MainWindow);
            if (source != null)
                TransformToDevice = source.CompositionTarget.TransformToDevice;
            else
                using (var sources = new HwndSource(new HwndSourceParameters("")))
                {
                    TransformToDevice = sources.CompositionTarget.TransformToDevice;
                }
            int defaultDPI = 96;
            DpiX = TransformToDevice.M11 * defaultDPI;
            DpiY = TransformToDevice.M22 * defaultDPI;
        }

        public static Size MeasureUIElementPixelSize(UIElement element)
        {
            if (element.DesiredSize == new Size())
                element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return (Size)TransformToDevice.Transform((Vector)element.DesiredSize);
        }
    }

    public class HeatMapPalette
    {
        public HeatMapPalette()
        {
            SetupTable(HeatMapPalettes.Ahsb);
        }

        public HeatMapPalette(HeatMapPalettes palette)
        {
            SetupTable(palette);
        }

        public UInt32[] ColorLookup = null;
        public int ColorCount = 512;

        private void SetupTable(HeatMapPalettes palette)
        {
            switch (palette)
            {
                case HeatMapPalettes.Ahsb:
                    ColorCount = 512;
                    ColorLookup = new UInt32[ColorCount];
                    for (int i = 0; i < ColorCount; i++)
                    {
                        double cV = ((double)i) / ColorLookup.Length;
                        Color c = ColorFromAhsb(255, (float)((cV * 0.9 + 0.09) * 360.0), 1.0f, 0.5f);
                        ColorLookup[i] = BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, c.A }, 0);
                    }
                    break;
                case HeatMapPalettes.Gray256:
                    ColorCount = 256;
                    ColorLookup = new UInt32[ColorCount];
                    for (int i = 0; i < ColorCount; i++)
                    {
                        Color c = ColorFromGray256((byte)i);
                        ColorLookup[(ColorCount - 1) - i] = BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, c.A }, 0);
                    }
                    break;
                case HeatMapPalettes.Ahsb200To60:
                    ColorCount = 512;
                    ColorLookup = new UInt32[ColorCount];
                    for (int i = 0; i < ColorCount; i++)
                    {
                        double cV = ((double)i) / ColorLookup.Length;
                        double incre = (cV * 220.0);
                        double hue = (200 + incre) % 360;
                        Color c = ColorFromAhsb(255, (float)hue, 1.0f, 0.5f);
                        ColorLookup[i] = BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, c.A }, 0);
                    }
                    break;
                case HeatMapPalettes.Ahsb120To360:
                    ColorCount = 512;
                    ColorLookup = new UInt32[ColorCount];
                    for (int i = 0; i < ColorCount; i++)
                    {
                        double cV = ((double)i) / ColorLookup.Length;
                        double incre = (cV * 240.0);
                        double hue = (120 + incre) % 360;
                        Color c = ColorFromAhsb(255, (float)hue, 1.0f, 0.5f);
                        ColorLookup[i] = BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, c.A }, 0);
                    }
                    break;
                case HeatMapPalettes.WhiteBlueRed:
                    ColorCount = 512;
                    ColorLookup = new UInt32[ColorCount];
                    {
                        int i = 0;
                        //White -->  Blue
                        for (i = 0; i < ColorCount; i++)
                        {
                            var s = (float) (i/((double) ColorCount));
                            Color c = ColorFromAhsb(255, (float)240, s, 1.0f - s*0.5f);
                            ColorLookup[i] = BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, c.A }, 0);
                        }
                        //Blue --> Red
/*                        for (; i < ColorCount; i++)
                        {
                            double cV = ((double)i) / ColorLookup.Length;
                            double incre = (cV * 120.0);
                            double hue = (240 + incre) % 360;
                            Color c = ColorFromAhsb(255, (float)hue, 1.0f, 0.5f);
                            ColorLookup[i] = BitConverter.ToUInt32(new byte[] { c.B, c.G, c.R, c.A }, 0);
                        }*/
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public UInt32 GetColor(int index)
        {
            return ColorLookup[index];
        }

        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {
            byte alpha = Convert.ToByte(a);
            if (0 == s)
            {
                return Color.FromArgb(alpha, Convert.ToByte(b * 255),
                  Convert.ToByte(b * 255), Convert.ToByte(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant;
            byte iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToByte(fMax * 255);
            iMid = Convert.ToByte(fMid * 255);
            iMin = Convert.ToByte(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }

        public static Color ColorFromGray256(byte index)
        {


            return Color.FromArgb(255, index, index, index);
        }
    }

    public enum HeatMapPalettes
    {
        Gray256,
        Ahsb,
        Ahsb200To60,
        Ahsb120To360,
        WhiteBlueRed
    }

    class Bitmap : UIElement
    {
        public Bitmap()
        {
            _sourceDownloaded = new EventHandler(OnSourceDownloaded);
            _sourceFailed = new EventHandler<ExceptionEventArgs>(OnSourceFailed);

            LayoutUpdated += new EventHandler(OnLayoutUpdated);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(BitmapSource),
            typeof(Bitmap),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(Bitmap.OnSourceChanged)));

        public BitmapSource Source
        {
            get
            {
                return (BitmapSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public event EventHandler<ExceptionEventArgs> BitmapFailed;

        // Return our measure size to be the size needed to display the bitmap pixels.
        protected override Size MeasureCore(Size availableSize)
        {
            Size measureSize = new Size();

            BitmapSource bitmapSource = Source;
            if (bitmapSource != null)
            {
                PresentationSource ps = PresentationSource.FromVisual(this);
                if (ps != null)
                {
                    Matrix fromDevice = ps.CompositionTarget.TransformFromDevice;

                    Vector pixelSize = new Vector(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
                    Vector measureSizeV = fromDevice.Transform(pixelSize);
                    measureSize = new Size(measureSizeV.X, measureSizeV.Y);
                }
            }

            return measureSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            BitmapSource bitmapSource = this.Source;
            if (bitmapSource != null)
            {
                _pixelOffset = GetPixelOffset();

                // Render the bitmap offset by the needed amount to align to pixels.
                dc.DrawImage(bitmapSource, new Rect(_pixelOffset, DesiredSize));
            }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Bitmap bitmap = (Bitmap)d;

            BitmapSource oldValue = (BitmapSource)e.OldValue;
            BitmapSource newValue = (BitmapSource)e.NewValue;

            if (((oldValue != null) && (bitmap._sourceDownloaded != null)) && (!oldValue.IsFrozen && (oldValue is BitmapSource)))
            {
                ((BitmapSource)oldValue).DownloadCompleted -= bitmap._sourceDownloaded;
                ((BitmapSource)oldValue).DownloadFailed -= bitmap._sourceFailed;
                // ((BitmapSource)newValue).DecodeFailed -= bitmap._sourceFailed; // 3.5
            }
            if (((newValue != null) && (newValue is BitmapSource)) && !newValue.IsFrozen)
            {
                ((BitmapSource)newValue).DownloadCompleted += bitmap._sourceDownloaded;
                ((BitmapSource)newValue).DownloadFailed += bitmap._sourceFailed;
                // ((BitmapSource)newValue).DecodeFailed += bitmap._sourceFailed; // 3.5
            }
        }

        private void OnSourceDownloaded(object sender, EventArgs e)
        {
            InvalidateMeasure();
            InvalidateVisual();
        }

        private void OnSourceFailed(object sender, ExceptionEventArgs e)
        {
            Source = null; // setting a local value seems scetchy...

            BitmapFailed(this, e);
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            // This event just means that layout happened somewhere.  However, this is
            // what we need since layout anywhere could affect our pixel positioning.
            Point pixelOffset = GetPixelOffset();
            if (!AreClose(pixelOffset, _pixelOffset))
            {
                InvalidateVisual();
            }
        }

        // Gets the matrix that will convert a point from "above" the
        // coordinate space of a visual into the the coordinate space
        // "below" the visual.
        private Matrix GetVisualTransform(Visual v)
        {
            if (v != null)
            {
                Matrix m = Matrix.Identity;

                Transform transform = VisualTreeHelper.GetTransform(v);
                if (transform != null)
                {
                    Matrix cm = transform.Value;
                    m = Matrix.Multiply(m, cm);
                }

                Vector offset = VisualTreeHelper.GetOffset(v);
                m.Translate(offset.X, offset.Y);

                return m;
            }

            return Matrix.Identity;
        }

        private Point TryApplyVisualTransform(Point point, Visual v, bool inverse, bool throwOnError, out bool success)
        {
            success = true;
            if (v != null)
            {
                Matrix visualTransform = GetVisualTransform(v);
                if (inverse)
                {
                    if (!throwOnError && !visualTransform.HasInverse)
                    {
                        success = false;
                        return new Point(0, 0);
                    }
                    visualTransform.Invert();
                }
                point = visualTransform.Transform(point);
            }
            return point;
        }

        private Point ApplyVisualTransform(Point point, Visual v, bool inverse)
        {
            bool success = true;
            return TryApplyVisualTransform(point, v, inverse, true, out success);
        }

        public Point GetPixelOffset()
        {
            Point pixelOffset = new Point();

            PresentationSource ps = PresentationSource.FromVisual(this);
            if (ps != null)
            {
                Visual rootVisual = ps.RootVisual;

                // Transform (0,0) from this element up to pixels.
                pixelOffset = this.TransformToAncestor(rootVisual).Transform(pixelOffset);
                pixelOffset = ApplyVisualTransform(pixelOffset, rootVisual, false);
                pixelOffset = ps.CompositionTarget.TransformToDevice.Transform(pixelOffset);

                // Round the origin to the nearest whole pixel.
                pixelOffset.X = Math.Round(pixelOffset.X);
                pixelOffset.Y = Math.Round(pixelOffset.Y);

                // Transform the whole-pixel back to this element.
                pixelOffset = ps.CompositionTarget.TransformFromDevice.Transform(pixelOffset);
                pixelOffset = ApplyVisualTransform(pixelOffset, rootVisual, true);
                pixelOffset = rootVisual.TransformToDescendant(this).Transform(pixelOffset);
            }

            return pixelOffset;
        }

        private bool AreClose(Point point1, Point point2)
        {
            return AreClose(point1.X, point2.X) && AreClose(point1.Y, point2.Y);
        }

        private bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            double delta = value1 - value2;
            return ((delta < 1.53E-06) && (delta > -1.53E-06));
        }

        private EventHandler _sourceDownloaded;
        private EventHandler<ExceptionEventArgs> _sourceFailed;
        private Point _pixelOffset;
    }
}
