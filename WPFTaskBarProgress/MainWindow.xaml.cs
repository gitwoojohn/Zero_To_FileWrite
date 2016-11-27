using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTaskBarProgress
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 상태 초기화 안해주면 진행바 먹통....
            TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
        }

        void MainWindow_Loaded( object sender, RoutedEventArgs e )
        {
            TaskbarItemInfo.ProgressValue = 0.5;
        }

        private async Task TaskProgressBar( IProgress<int> progress )
        {
            int tempCount = 1;
            if( progress != null )
            {
                for( int i = 0; i < 100; i++ )
                {
                    await Task.Delay( 100 );
                    progress.Report( ( tempCount * 100 ) / 100 );
                    tempCount++;
                }
            }
        }

        private void ReportProgress( int value )
        {
            ProgressBar1.Value = value;
            TaskbarItemInfo.ProgressValue = ( double )value / 100;
        }

        private async void button_Click( object sender, RoutedEventArgs e )
        {
            var progressIndicator = new Progress<int>( ReportProgress );
            await TaskProgressBar( progressIndicator );
        }
    }
}
