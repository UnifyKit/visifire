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

namespace SLVisifireApp
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();

            MyChart.Titles[0].MouseLeftButtonUp += new MouseButtonEventHandler(Page_MouseLeftButtonUp);
        }

        void Page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Random rand = new Random();
            for (Int32 i = 0; i < MyChart.Series[0].DataPoints.Count;i++ )
            {
                MyChart.Series[0].DataPoints[i].YValue = rand.Next(10, 100);
            }
        }
    }
}