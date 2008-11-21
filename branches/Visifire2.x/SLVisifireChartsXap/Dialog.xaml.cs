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

namespace SLVisifireChartsXap
{
    public partial class Dialog : UserControl
    {
        public Dialog()
        {
            InitializeComponent();
            this.DialogOutStoryBoard.Completed += new EventHandler(DialogOutStoryBoard_Completed);
            CloseButton.Click += new RoutedEventHandler(CloseButton_Click);
            this.SetValue(Canvas.ZIndexProperty, 1000000);
        }

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogOutStoryBoard.Begin();
        }

        void DialogOutStoryBoard_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public String VersionInfo
        {
            set
            {
                Info.Text = value;
            }
            get
            {
                return Info.Text;
            }
        }

        public Boolean Visible
        {
            get
            {
                return (Boolean) GetValue(VisibleProperty);
            }
            set
            {
                SetValue(VisibleProperty, value);
            }
        }

        public readonly static DependencyProperty VisibleProperty  = DependencyProperty.Register(
        "Visible",
        typeof(Boolean),
        typeof(Dialog), new PropertyMetadata(OnVisiblePropertyChanged));

        private static void OnVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Dialog d = sender as Dialog;

            if((Boolean)e.NewValue  == true)
            {
                d.Visibility = Visibility.Visible;
            }
            else
            {
                d.Visibility = Visibility.Collapsed;
            }
        }
    }
}
