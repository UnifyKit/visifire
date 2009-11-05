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

            foreach (DataSeries ds in MyChart.Series)
            {
                ds.MouseLeftButtonDown += new MouseButtonEventHandler(ds_MouseLeftButtonDown);
                ds.LabelEnabled = true;
            }

            Double i = 1;

            foreach (DataPoint dp in MyChart.Series[0].DataPoints)
            {
                dp.XValue = i++;
            }

            MyChart.MouseLeftButtonUp += new MouseButtonEventHandler(MyChart_MouseLeftButtonUp);
        }

        void MyChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
             // MyChart.Series[0].Enabled = !MyChart.Series[0].Enabled;
             // MyChart.Series[1].Enabled = !MyChart.Series[0].Enabled;

             MyChart.Series[0].Enabled = !MyChart.Series[0].Enabled;
             
             // MyChart.Series[0].Color = Graphics.GetRandomColor();
             //MyChart.Series[0].DataPoints[0].Enabled = !MyChart.Series[0].DataPoints[0].Enabled;
             //MyChart.Series[1].DataPoints[0].Enabled = !MyChart.Series[1].DataPoints[0].Enabled;

             // MyChart.Series[0].DataPoints[0].Enabled = !MyChart.Series[0].DataPoints[0].Enabled;
             //MyChart.Series[0].DataPoints[0].Opacity = 0.5;
             // MyChart.Series[0].LightingEnabled = !MyChart.Series[0].LightingEnabled;

             //MyChart.Series[0].DataPoints[0].LabelFontFamily = new FontFamily("Verdana");

             //MyChart.Series[0].BorderThickness = new Thickness(3);

            //MyChart.Series[0].YValueFormatString = "000.##";

             //foreach (DataPoint dp in MyChart.Series[0].DataPoints)
             //{
             //    //dp.Color = Graphics.GetRandomColor();
             //    dp.YValue = rand.Next(10, 100);
             //    //dp.XValue = rand.Next(-100, 100);
             //    //dp.ZValue = rand.Next(10, 100);
             //    //dp.XValue = rand.Next(1, 9);
             //}
        }

        void ds_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {   
            DataPoint dp = sender as DataPoint;
            //dp.Selected = !dp.Selected;
            //dp.LabelEnabled = !dp.LabelEnabled;
            // dp.Parent.LabelStyle = LabelStyles.Inside;
            //dp.Parent.ShowInLegend = true;
            //dp.Parent.LegendText = "Hi";
            //dp.Parent.Cursor
            //dp.Parent.Href = "http://www.yahoo.com";
            //dp.YValue = -dp.YValue;
        }

        Random rand = new Random(DateTime.Now.Millisecond);
        DispatcherTimer timer = new DispatcherTimer();
    }
}