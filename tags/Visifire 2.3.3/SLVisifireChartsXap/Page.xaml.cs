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

namespace SLVisifireChartsXap
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            //Button1.Click += new RoutedEventHandler(Button1_Click);

            c.Height = 300;
            c.Width = 500;
            c.Background = new SolidColorBrush(Colors.Green);

            // remove from here
            Random rand = new Random();
            for (Int32 i = 0; i < 2; i++)
            {
                DataSeries ds = new DataSeries();
                for (Int32 j = 0; j < 10; j++)
                {
                    DataPoint dp = new DataPoint();
                    dp.XValue = j + 1;
                    dp.YValue = rand.Next(-1000, 0);
                    dp.ZValue = rand.Next(0, 100);
                    dp.AxisXLabel = "XValue Axis Labels " + dp.XValue.ToString();
                    ds.DataPoints.Add(dp);
                }
                ds.RenderAs = RenderAs.Column;
                ds.AxisXType = AxisTypes.Primary;
                ds.AxisYType = AxisTypes.Primary;

                ds.ZIndex = rand.Next(0, 50);
                c.Series.Add(ds);
            }

            // remove till here

            Title t = new Title("Title1");
            t.FontColor = new SolidColorBrush(Colors.Red);
            t.VerticalAlignment = VerticalAlignment.Center;
            t.HorizontalAlignment = HorizontalAlignment.Left;
            c.Titles.Add(t);
            t.FontColor = new SolidColorBrush(Colors.Yellow);
            this.LayoutRoot.Children.Add(c);
            LayoutRoot.MouseMove += new MouseEventHandler(LayoutRoot_MouseMove);
            LayoutRoot.Children.Add(tx);
        }

        void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            //tx.Text = e.GetPosition(c).ToString();
        }

        void Button1_Click(object sender, RoutedEventArgs e)
        {
            Title t = new Title("Title1");
            t.FontColor = new SolidColorBrush(Colors.Red);
            t.VerticalAlignment = VerticalAlignment.Center;
            t.HorizontalAlignment = HorizontalAlignment.Left;
            c.Titles.Add(t);
            t.FontColor = new SolidColorBrush(Colors.Yellow);
        }

        Chart c = new Chart();
        TextBlock tx = new TextBlock() { Foreground = new SolidColorBrush(Colors.Black)};
    }
}
