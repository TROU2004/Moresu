using MaterialDesignThemes.Wpf;
using Moresu.Component.Profile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Moresu
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            Host.Home = this;
            Host.StartupPrepare();
        }

        private void Button_AddProfile_Click(object sender, RoutedEventArgs e)
        {
            dialogHost_Root.ShowDialog(new ProfileCreate());
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
