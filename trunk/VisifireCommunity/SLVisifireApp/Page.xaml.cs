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

namespace SLVisifireApp
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            //MyChart.Visibility = Visibility.Collapsed;

            //MyChart.Rendered += new EventHandler(MyChart_Rendered);
            //MyChart.ToolBarEnabled = true;
            MyChart.MouseLeftButtonUp += new MouseButtonEventHandler(MyChart_MouseLeftButtonUp);

        }

        void MyChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MyChart.AxesX[0].ScrollBarScale = 0.2;
            //MyChart.DataPointWidth = 2;

            //MyChart.Series.RemoveAt(1);
            MyChart.Series[0].DataPoints.RemoveAt(14);

            DataPoint dp = new DataPoint();
            dp.YValue = 20;
            dp.XValue = new DateTime(2009, 1, 15);
            MyChart.Series[0].DataPoints.Add(dp);
            //MyChart.AxesX[0].ScrollBarOffset = 1;

            //DataSeries ds = new DataSeries();
            //ds.RenderAs = RenderAs.Line;
            //for (Int32 i = 0; i < 10; i++)
            //{
            //    DataPoint dp = new DataPoint();
            //    dp.YValue = new Random().Next(10, 100);
            //    ds.DataPoints.Add(dp);
            //}
            //MyChart.Series.Add(ds);
            //Double chartSize = MyChart.AxesX[0].Width / MyChart.AxesX[0].ScrollBarScale;

            //MyChart.AxesX[0].ScrollBarScale = MyChart.AxesX[0].Width / chartSize;

        }

        void MyChart_Rendered(object sender, EventArgs e)
        {
            //MyChart.Export();
        }
    }
}