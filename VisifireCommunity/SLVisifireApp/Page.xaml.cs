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
using System.ComponentModel;
using System.Windows.Threading;
using Visifire.Charts;
using Visifire.Commons;

namespace SLVisifireApp
{

    public partial class Page : UserControl
    {

        public Page()
        {
            InitializeComponent();

            //CreateChart();
        }


        private void CreateChart()
        {
            chart = new Chart();

            chart.Width = 500;
            chart.Height = 300;

            chart.AnimatedUpdate = true;

            for (Int16 j = 0; j < 3; j++)
            {
                DataSeries ds = new DataSeries();
                ds.RenderAs = RenderAs.Column;

                for (int i = 0; i < 50; i++)
                {
                    DataPoint dp = new DataPoint();
                    dp.YValue = rand.Next(10, 100);
                    ds.DataPoints.Add(dp);
                }

                // Add DataSeries to Chart
                chart.Series.Add(ds);
            }

            // Add chart to the LayoutRoot for display
            LayoutRoot.Children.Add(chart);

            chart.MouseLeftButtonUp += new MouseButtonEventHandler(chart_MouseLeftButtonUp);
        }

        void chart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            timer.Start();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            chart.Series[0].DataPoints.Clear();
            chart.Series[1].DataPoints.Clear();
            chart.Series[2].DataPoints.Clear();

            for (Int16 j = 0; j < 3; j++)
            {
                //DataSeries ds = new DataSeries();
                //ds.RenderAs = RenderAs.Column;

                for (int i = 0; i < 50; i++)
                {
                    DataPoint dp = new DataPoint();
                    chart.Series[j].DataPoints.Add(dp);
                    chart.Series[j].DataPoints[chart.Series[j].DataPoints.Count - 1].YValue = rand.Next(10, 100);
                }

                // Add DataSeries to Chart
                //chart.Series.Add(ds);
            }
        }

        Chart chart;
        Random rand = new Random();
        System.Windows.Threading.DispatcherTimer timer = new DispatcherTimer();
    }
}