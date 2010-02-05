using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.ComponentModel;
using Visifire.Charts;
using Visifire.Commons;
using System.Windows.Media.Animation;
using System.IO;
using System.Xml;
using System.Windows.Threading;

namespace WPFVisifireChartsApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            //button1.Click += new RoutedEventHandler(button1_Click);

            //MyChart.Rendered += new EventHandler(MyChart_Rendered);

            int width = 300;
            int height = 300;

            var chart = new Visifire.Charts.Chart
            {
                Width = width,
                Height = height,
                AnimationEnabled = false,
                Theme = "Theme1"
            };

            // create series
            var series = new DataSeries
            {
                RenderAs = RenderAs.Pie,
                LabelEnabled = true,
                LabelText = "#AxisXLabel, #YValue",
                LabelLineThickness = 0.5,
                Bevel = false,
                ShowInLegend = false
            };

            // Add datapoints
            var dp = new DataPoint { YValue = 60, AxisXLabel = "Wedge 1", Exploded = true };
            series.DataPoints.Add(dp);
            dp = new DataPoint { YValue = 40, AxisXLabel = "Wedge 2", Exploded = true };
            series.DataPoints.Add(dp);

            chart.Series.Add(series);

            chart.BeginInit();
            chart.EndInit();
            chart.Measure(new Size(width, height));
            chart.Arrange(new Rect(0, 0, width, height));
            chart.UpdateLayout();

            chart.Rendered += new EventHandler(chart_Rendered);

            // Render as bitmap 
            var bmp = new RenderTargetBitmap(width * 300 / 72, height * 300 / 72, 300, 300, PixelFormats.Pbgra32);
            bmp.Render(chart);
            BitmapEncoder encoder;
            encoder = new PngBitmapEncoder();
            var bf = BitmapFrame.Create(bmp);
            encoder.Frames.Add(bf);
            Stream strm = new FileStream("Output.png", FileMode.OpenOrCreate);
            encoder.Save(strm);
            
        }

        void chart_Rendered(object sender, EventArgs e)
        {
            MessageBox.Show("Hi");
        }

        void button1_Click(object sender, RoutedEventArgs e)
        {
            //button1.Visibility = Visibility.Collapsed;
            //ExportToPng(new Uri("d:\\button1.png"), button1);
        }

        public void ExportToPng(Uri path, Button surface)
        {
            if (path == null) return;

            // Save current canvas transform
            Transform transform = surface.LayoutTransform;
            // reset current transform (in case it is scaled or rotated)
            surface.LayoutTransform = null;

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                    (int)surface.Width,
                    (int)surface.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }

            // Restore previously saved layout
            surface.LayoutTransform = transform;
        }

        void MyChart_Rendered(object sender, EventArgs e)
        {
            //MyChart.Visibility = Visibility.Collapsed;
        }
    }
}