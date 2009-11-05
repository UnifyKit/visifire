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

            for (Int32 i = 0; i < 3; i++)
            {
                DataSeries ds = new DataSeries();
                if (i == 0 || i == 1)
                    ds.RenderAs = RenderAs.Line;
                else
                    ds.RenderAs = RenderAs.CandleStick;

                Int32 m = 1;
                Int32 d = 1;
                Int32 y = 2009;
                for (Int32 j = 0; j < 2500; j++)
                {
                    DataPoint dp = new DataPoint();
                    if (d == 28)
                    {
                        d = 1;
                        m++;
                    }
                    if (m > 12)
                    {
                        m = 1;
                        d = 1;
                        y++;
                    }
                    dp.XValue = new DateTime(y, m, d++);
                    dp.YValue = rand.Next(10, 100);
                    ds.DataPoints.Add(dp);
                }

                MyChart1.Series.Add(ds);
            }

            for (Int32 i = 0; i < 3; i++)
            {
                DataSeries ds = new DataSeries();
                if (i == 0 || i == 1)
                    ds.RenderAs = RenderAs.Line;
                else
                    ds.RenderAs = RenderAs.Column;

                Int32 m = 1;
                Int32 d = 1;
                Int32 y = 2009;
                for (Int32 j = 0; j < 2500; j++)
                {
                    DataPoint dp = new DataPoint();
                    if (d == 28)
                    {
                        d = 1;
                        m++;
                    }
                    if (m > 12)
                    {
                        m = 1;
                        d = 1;
                        y++;
                    }
                    dp.XValue = new DateTime(y, m, d++);
                    dp.YValue = rand.Next(10, 100);
                    ds.DataPoints.Add(dp);
                }


                MyChart2.Series.Add(ds);
            }
        }

        Random rand = new Random();
    }
}