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


            // Application.Current.Host.Settings.MaxFrameRate = 60;

            //MyChart.Series[0].RenderAs = RenderAs.StackedBar100;
            //MyChart.Series[1].RenderAs = RenderAs.StackedBar100;
                
            // MyChart.AnimationEnabled = false;
            // MyChart.ScrollingEnabled = false;
            // MyChart.Series[0].ShadowEnabled = false;

            MyChart.View3D = false;

            // for (int j = 0; j < MyChart.Series.Count; j++)
            //    for (int i = 0; i < 10; i++)
            //    {   
            //        DataPoint dp = new DataPoint() { YValue = rand.Next(-150, 150) };
            //        MyChart.Series[j].DataPoints.Add(dp);
            //    }

            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += new EventHandler(timer_Tick);
            
            // MyChart.Series[0].LabelEnabled = true;
            // MyChart.Series[0].LabelEnabled = true;
            // MyChart.Series[0].MarkerEnabled = true;
            // MyChart.Series[1].MarkerEnabled = true;

            button.Click += new RoutedEventHandler(button_Click);
            button1.Click += new RoutedEventHandler(button1_Click);

            // MyChart.Rendered += new EventHandler(MyChart_Rendered);
        }

        void button1_Click(object sender, RoutedEventArgs e)
        {
            //DataSeries ds = MyChart.Series[0];
            //DataPoint dp = MyChart.Series[0].DataPoints[6];

            //dp.YValue =  -dp.YValue;
            timer.Start();
        }

        void MyChart_Rendered(object sender, EventArgs e)
        {
            // Visifire.Profiler.Profiler.End("Test1");
            // Visifire.Profiler.Profiler.Report("Test1", true);
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            DataSeries ds = MyChart.Series[0];
            DataPoint dp = MyChart.Series[0].DataPoints[2];

            dp.YValues = new Double[]{ dp.YValues[0] + 3,  dp.YValues[1] +3,  dp.YValues[2] + 2,  dp.YValues[3] + 4 };
            // ds.Enabled = !ds.Enabled;
            // dp.Enabled = !dp.Enabled;
           // dp.Color = Graphics.GetRandonColor();
            //dp.BorderThickness = new Thickness(3);
            //dp.BorderColor = new SolidColorBrush(Colors.Red);
            // timer.Start();
            // UpdateDataPoint1();
            // UpdateDataPoint();
            
            // AddDataPoints();
            // timer.Start();
        }

        void UpdateDataPoint1()
        {
            Double i = 0.1;
            val = -val;

            foreach (DataPoint dp in MyChart.Series[0].DataPoints)
            {
                dp.YValue = val * Math.Sin(i);
                i += 0.6;
            }
        }

        Double val = 100;

        void timer_Tick(object sender, EventArgs e)
        {
            DataSeries ds = MyChart.Series[0];
            //DataPoint dp = MyChart.Series[0].DataPoints[6];

           // dp.YValue = -dp.YValue;

            //foreach (DataPoint dp in MyChart.Series[0].DataPoints)
            //{
            //    dp.YValue = rand.Next(10, 100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp));
            //}

            //UpdateDataPoint1();

            // MyChart.Series[0].LabelBackground = new SolidColorBrush(Color.FromArgb(255, (byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255)));
            // ReRender();
            // AddDataPoints();

            // UpdateDataPoint();
        }

        void AddDataPoints()
        {   
            MyChart.Series[0].DataPoints.Add(new DataPoint() { YValue = rand.Next(-150, 150) });
            // if (MyChart.AxesX.Count > 1)
            //  MyChart.AxesX[0].ScrollBarOffset = 1;
            // MyChart.Series[0].DataPoints.Add(new DataPoint() { XValue = MyChart.Series[0].DataPoints.Count, YValue = MyChart.Series[0].DataPoints.Count + rand.Next(-20, 20) });
            // MyChart.Series[1].DataPoints.Add(new DataPoint() { XValue = MyChart.Series[1].DataPoints.Count, YValue = MyChart.Series[1].DataPoints.Count + rand.Next(-20, 20) });
        }

        void UpdateDataPoint()
        {
            //MyChart.PlotArea.MouseMove += new EventHandler<PlotAreaMouseEventArgs>(PlotArea_MouseMove);
            // AddDataPoints();
            // MyChart.Series[0].DataPoints[2].YValue = MyChart.Series[0].DataPoints.Count + rand.Next(-100, 100);

            /*
                        //foreach (DataSeries ds in MyChart.Series)
                        DataSeries ds = MyChart.Series[0];
                            foreach (DataPoint dp in ds.DataPoints)
                            {
                                dp.YValue = rand.Next(-100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp), 100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp));
                                //dp.YValue = dp.YValue++;
                                //dp.YValue = rand.Next(-50 + rand.Next(-10, 10), 30 + rand.Next());
                            }

                        dp.YValue += 10;
                        * 
                        */

             DataSeries ds = MyChart.Series[0];
             DataPoint dp = MyChart.Series[0].DataPoints[0];

             // dp.YValue = rand.Next(-100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp), 100 * (MyChart.Series.IndexOf(ds) + 1) + ds.DataPoints.IndexOf(dp));
             dp.YValue += 10;
            
            // MyChart.Series[0].Bevel = !MyChart.Series[0].Bevel;
            // MyChart.Series[0].LightingEnabled = !MyChart.Series[0].LightingEnabled;
            // MyChart.Series[0].ShadowEnabled = !MyChart.Series[0].ShadowEnabled;
            // MyChart.Series[0].Color = new SolidColorBrush(Colors.Orange);
            // MyChart.Series[0].Cursor = Cursors.Hand;
               
            // MyChart.Series[0].Href = "http://www.yahoo.com";
            
          //  MyChart.Series[0].LabelEnabled = !MyChart.Series[0].LabelEnabled;
           // MyChart.Series[0].MarkerEnabled = !MyChart.Series[0].MarkerEnabled;
           // MyChart.Series[0].LabelBackground = new SolidColorBrush(Colors.Red);
           // MyChart.Series[0].LabelText = "Somnatgh";

            // MyChart.Series[0].DataPoints[MyChart.Series[0].DataPoints.Count - 1].MarkerColor = new SolidColorBrush(Colors.Red);
            // MyChart.Series[0].DataPoints[MyChart.Series[0].DataPoints.Count - 1].Cursor = Cursors.Hand;
        }

        void PlotArea_MouseMove(object sender, PlotAreaMouseEventArgs e)
        {
            throw new NotImplementedException();
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