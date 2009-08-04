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

            // Create a Chart
            chart = new Chart();

            // Set Chart size
            chart.Width = 500;
            chart.Height = 300;

            // Initialize Random class
            Random rand = new Random();

            // Create DataSeries
            DataSeries dataSeries = new DataSeries();

            for (Int32 i = 0; i < 5; i++)
            {
                // Create DataPoint
                DataPoint dataPoint = new DataPoint();
                // Set DataPoint property
                dataPoint.YValue = rand.Next(10, 100);

                // Add DataPoint to DataPoints collection of DataSeries
                dataSeries.DataPoints.Add(dataPoint);
            }

            // Add DataSeries to Series collection of Chart
            chart.Series.Add(dataSeries);

            // Add Chart to LayoutRoot
            //LayoutRoot.Children.Add(chart);

            // Attach event to Chart
            chart.MouseLeftButtonDown += new MouseButtonEventHandler(chart_MouseLeftButtonDown);
        }

        Chart chart;

        void chart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExportToPng(new Uri("d:/visifire.png"), chart);
        }

        public void ExportToPng(Uri path, Visifire.Charts.Chart surface)
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
    }
}