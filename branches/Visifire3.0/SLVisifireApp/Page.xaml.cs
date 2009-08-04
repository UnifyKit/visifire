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
using Visifire.Charts;
using Visifire.Commons;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Threading;

namespace SLVisifireApp
{
    public partial class Page : UserControl
    {   
        public Page()
        {   
            InitializeComponent();

            Application.Current.Host.Settings.MaxFrameRate = 60;

            MyChart.Series[0].RenderAs = RenderAs.Column;
            MyChart.Series[0].MarkerEnabled = false;

            // MyChart.Series[0].MarkerSize = 10;
            
            //MyChart.Series[0].LightingEnabled = true;

            // MyChart.AnimationEnabled = false;
            // MyChart.ScrollingEnabled = false;

            MyChart.Series[0].Bevel = false;
            MyChart.Series[0].ShadowEnabled = false;
            MyChart.View3D = false;

            // MyChart.Series[0].DataPoints.Add(new DataPoint() { YValue = 500 });
            // MyChart.Series[0].DataPoints.Add(new DataPoint() { YValue = -500 });

            for (int j = 0; j < MyChart.Series.Count; j++)
            for (int i = 0; i < 50; i++)
            {   
                DataPoint dp = new DataPoint() { YValue =  rand.Next(-150, 150) };

                //  if (i % 3 == 0)
                //      dp.YValue = Double.NaN;

                MyChart.Series[j].DataPoints.Add(dp);

                // DataPoint dp1 = new DataPoint() { YValue = MyChart.Series[0].DataPoints.Count + rand.Next(-150, 150) };
                // MyChart.Series[1].DataPoints.Add(dp1);

                // DataPoint dp2 = new DataPoint() { YValue = MyChart.Series[0].DataPoints.Count + rand.Next(-150, 150) };
                // MyChart.Series[2].DataPoints.Add(dp2);
            }

            timer.Interval = new TimeSpan(0, 0,0,0,1000);
            timer.Tick += new EventHandler(timer_Tick);

            timer.Start();

            button.Click += new RoutedEventHandler(button_Click);
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            // UpdateDataPoint();

            // AddDataPoints();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            // ReRender();
            // AddDataPoints();
            
            UpdateDataPoint();
        }

        void AddDataPoints()
        {
            MyChart.Series[0].DataPoints.Add(new DataPoint() {  YValue = rand.Next(-150, 150) });

            // MyChart.Series[0].DataPoints.Add(new DataPoint() { XValue = MyChart.Series[0].DataPoints.Count, YValue = MyChart.Series[0].DataPoints.Count + rand.Next(-20, 20) });
            // MyChart.Series[1].DataPoints.Add(new DataPoint() { XValue = MyChart.Series[1].DataPoints.Count, YValue = MyChart.Series[1].DataPoints.Count + rand.Next(-20, 20) });
        }

        void UpdateDataPoint()
        {   
            // MyChart.Series[0].DataPoints[2].YValue = MyChart.Series[0].DataPoints.Count + rand.Next(-100, 100);

            
          foreach (DataSeries ds in MyChart.Series)
            foreach (DataPoint dp in ds.DataPoints)
            {   
                dp.YValue = rand.Next(-100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp), 100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp));
                //dp.YValue = dp.YValue;
                //dp.YValue = rand.Next(-50 + rand.Next(-10, 10), 30 + rand.Next());
            }
            

            // DataSeries ds = MyChart.Series[0];
            // DataPoint dp = MyChart.Series[0].DataPoints[2];

            // dp.YValue = rand.Next(-100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp), 100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp));

            // MyChart.Series[0].Bevel = false;// !MyChart.Series[0].Bevel;
            // MyChart.Series[0].LightingEnabled = !MyChart.Series[0].LightingEnabled;
            // MyChart.Series[0].ShadowEnabled = !MyChart.Series[0].ShadowEnabled;
            // MyChart.Series[0].Color = new SolidColorBrush(Colors.Orange);
            //MyChart.Series[0].Cursor = Cursors.Hand;

            // MyChart.Series[0].Href = "http://www.yahoo.com";
           // MyChart.Series[0].LabelEnabled = !MyChart.Series[0].LabelEnabled;
           // MyChart.Series[0].LabelBackground = new SolidColorBrush(Colors.Red);
            //MyChart.Series[0].LabelText = "Somnatgh";

            // MyChart.Series[0].DataPoints[MyChart.Series[0].DataPoints.Count - 1].MarkerColor = new SolidColorBrush(Colors.Red);
            // MyChart.Series[0].DataPoints[MyChart.Series[0].DataPoints.Count - 1].Cursor = Cursors.Hand;
        }

        void Page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataPoint dp = sender as DataPoint;

            dp.Color = new SolidColorBrush(Colors.Blue);
        }

        Random rand = new Random();

        DispatcherTimer timer = new DispatcherTimer();
    }
}