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
using System.Collections.ObjectModel;

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

            //DataSeries ds = new DataSeries();
            //ds.RenderAs = RenderAs.Line;
            //ds.MarkerEnabled = false;
            //ds.MovingMarkerEnabled = true;
            //ds.LightingEnabled = false;
            //ds.LineThickness = 1.5;
            ////ds.XValueType = ChartValueTypes.DateTime;
            //ds.LegendText = "Example Legend Text";
            ////ds.Color = new SolidColorBrush(Colors.Red);

            //for (Int32 i = 0; i < 5; i++)
            //{
            //    ds.DataPoints.Add(new DataPoint
            //    {
            //        //XValue = xDateTime, // a DateTime value
            //        ToolTipText = "Example Tool Tip Text",
            //        YValue = 12, // a double value
            //        Color = new SolidColorBrush(Colors.Red)
            //    });
            //}
            //MyChart.Series.Add(ds);

            MyChart.MouseLeftButtonUp += new MouseButtonEventHandler(MyChart_MouseLeftButtonUp);
        }

        void MyChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (DataPoint dp in MyChart.Series[0].DataPoints)
                dp.Color = new SolidColorBrush(Colors.Red);
        }
    }
}