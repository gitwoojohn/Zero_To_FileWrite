using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shell;
using Microsoft.Win32;
using WPF_FileContentDelete.ViewModels;

namespace WPF_FileContentDelete
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Stack<string> DeleteSubDirs = new Stack<string>();
        OpenFileDialog openFileDialog = null;
        ListViewDataSource listView;

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public MainWindow()
        {
            // 실행 성능 향상.
            //ProfileOptimization.SetProfileRoot( @"..\..\" );
            //ProfileOptimization.StartProfile( "profile" );

            InitializeComponent();
            InitializeOpenFileDialog();

            // 초기화 안해주면 진행바 먹통....
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        }

        private void InitializeOpenFileDialog()
        {
            openFileDialog = new OpenFileDialog();

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;

            // Set the file dialog to filter for All files.
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"G:\Baidu";
            openFileDialog.Title = "파일 선택";
            openFileDialog.FileName = string.Empty;

            button_Delete.IsEnabled = false;
        }

        private void button_Select_Click( object sender, RoutedEventArgs e )
        {
            if( openFileDialog.ShowDialog() == true )
            {
                ListView_AddFile( openFileDialog.FileNames );
            }
        }

        private async void button_ZeroFill_Click( object sender, RoutedEventArgs e )
        {
            var progressIndicator = new Progress<int>( ReportProgress );
            await DeleteFileAsync( lstView.Items.Count, progressIndicator );

            button_Delete.IsEnabled = true;
        }

        private void button_Delete_Click( object sender, RoutedEventArgs e )
        {
            string NewFileName;
            string OldFileName;

            for( int i = 0; i < lstView.Items.Count; i++ )
            {
                NewFileName = Path.GetDirectoryName( lstView.Items[ i ].ToString() ) + @"\1.tmp";
                OldFileName = lstView.Items[ i ].ToString();

                // 파일 이름 변경
                File.Move( OldFileName, NewFileName );
                File.Delete( NewFileName );
            }

            DeleteSubDirectory( DeleteSubDirs );

            listView.Clear();
            button_Delete.IsEnabled = false;
        }

        private async Task DeleteFileAsync( int totalCount, IProgress<int> progress )
        {
            int tempCount = 1;
            try
            {
                //await the processing and delete file logic here
                for( int i = 0; i < totalCount; i++ )
                {
                    await StreamFileWrite( lstView.Items[ i ].ToString() );
                    if( progress != null )
                    {
                        progress.Report( ( tempCount * 100 / totalCount ) );
                    }
                    tempCount++;
                }
            }
            catch( Exception e )
            {
                Debug.Write( e.Message );
            }

            //int tempCount = 1;
            //const int blockSize = 1024 * 64; // 8; // 16; 
            //byte[] data = new byte[ blockSize ];
            //List<Task> tasks = new List<Task>();
            //List<FileStream> sourceStreams = new List<FileStream>();

            //try
            //{
            //    //await the processing and delete file logic here
            //    for( int i = 0; i < totalCount; i++ )
            //    {
            //        //await StreamFileWrite( lstView.Items[ i ].ToString() );

            //        FileStream sourceStream = 
            //            new FileStream( lstView.Items[ i ].ToString(), 
            //                            FileMode.Open, 
            //                            FileAccess.Write, 
            //                            FileShare.None, 
            //                            bufferSize:4096, 
            //                            useAsync: true );

            //        if( progress != null )
            //        {
            //            progress.Report( ( tempCount * 100 / totalCount ) );
            //        }

            //        Task theTask = sourceStream.WriteAsync( data, 0, data.Length );
            //        sourceStreams.Add( sourceStream );

            //        tasks.Add( theTask );
            //        tempCount++;
            //    }
            //    await Task.WhenAll( tasks );
            //}

            //catch( Exception e )
            //{
            //    Debug.Write( e.Message );
            //}
            //finally
            //{
            //    foreach(FileStream sourceStream in sourceStreams)
            //    {
            //        sourceStream.Close();
            //    }
            //}
        }

        //public static bool IsAdministrator()
        //{
        //    WindowsIdentity identity = WindowsIdentity.GetCurrent();

        //    if( null != identity )
        //    {
        //        WindowsPrincipal principal = new WindowsPrincipal( identity );
        //        return principal.IsInRole( WindowsBuiltInRole.Administrator );
        //    }

        //    return false;
        //}

        private async Task StreamFileWrite( string filePath )
        {
            const int blockSize = 1024 * 64; // 8; // 16; 
            byte[] data = new byte[ blockSize ];
            long count = 0;

            //if( IsAdministrator() == false )
            //{
            //    try
            //    {
            //        ProcessStartInfo procInfo = new ProcessStartInfo();
            //        procInfo.UseShellExecute = true;
            //        procInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            //        procInfo.WorkingDirectory = Environment.CurrentDirectory;
            //        procInfo.Verb = "runas";
            //        Process.Start( procInfo );
            //    }
            //    catch( Exception ex )
            //    {
            //        MessageBox.Show( ex.Message.ToString() );
            //    }
            //}
            //// 권한 상승
            //DirectoryInfo dInfo = new DirectoryInfo( filePath );
            //DirectorySecurity dSecurity = dInfo.GetAccessControl();
            //dSecurity.AddAccessRule( new FileSystemAccessRule( 
            //                            new SecurityIdentifier( 
            //                                WellKnownSidType.WorldSid, null ), 
            //                                FileSystemRights.FullControl, 
            //                                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, 
            //                                PropagationFlags.NoPropagateInherit, AccessControlType.Allow ) );
            //dInfo.SetAccessControl( dSecurity );


            using( FileStream streamWrite = File.OpenWrite( filePath ) )
            {
                try
                {
                    count = streamWrite.Length / blockSize;

                    for( int i = 0; i < count; i++ )
                    {
                        await streamWrite.WriteAsync( data, 0, data.Length );
                    }

                    if( streamWrite.Length > count * blockSize )
                    {
                        byte[] Last_Data = new byte[ streamWrite.Length - ( count * blockSize ) ];
                        await streamWrite.WriteAsync( Last_Data, 0, Last_Data.Length );
                    }
                }
                catch( Exception e )
                {
                    MessageBox.Show( e.ToString() );
                }
            }
        }

        // 일반적으로 재귀 방식을 쓰지만 복잡하거나 중첩 규모가 크면 스택 오버 플로우 발생 가능성
        private void TraverseTree( string[] SourceDirs )
        {
            Stack<string> dirs = new Stack<string>( 30 );

            // 스택에 소스 경로 넣기( Push )
            foreach( var SourceDir in SourceDirs )
            {
                dirs.Push( SourceDir );
            }

            while( dirs.Count > 0 )
            {
                string CurrentDir = dirs.Pop();
                string[] SubDirs = null;
                string[] files = null;

                try
                {
                    SubDirs = Directory.GetDirectories( CurrentDir );
                    foreach( var SubDir in SubDirs )
                    {
                        // 폴더 일반 속성으로 변경
                        File.SetAttributes( SubDir, FileAttributes.Normal );

                        // 스택에 폴더 넣기( 후입선출 - LIFO )
                        DeleteSubDirs.Push( SubDir.ToString() );
                    }
                    files = Directory.GetFiles( CurrentDir );
                    ListView_AddFile( files );
                }
                catch( UnauthorizedAccessException e )
                {
                    Debug.WriteLine( e.Message );
                }

                foreach( string SubDir in SubDirs )
                {
                    dirs.Push( SubDir );
                }
            }
        }

        private void DeleteSubDirectory( Stack<string> SubDirs )
        {
            int Count = SubDirs.Count;
            string NewFolderName = null;
            string WorkDirectory = null;
            string[] SplitDirectory = null;

            for( int i = 0; i < Count; i++ )
            {
                WorkDirectory = SubDirs.Pop();
                SplitDirectory = WorkDirectory.Split( '\\' );

                // 마지막 폴더 이름을 항상 tmp로 변경
                SplitDirectory[ SplitDirectory.Length - 1 ] = "tmp";

                // 배열 문자 합치기
                NewFolderName = string.Join( "\\", SplitDirectory );

                Directory.Move( WorkDirectory, NewFolderName );
                Directory.Delete( NewFolderName );
            }
        }

        private void FileSizeSorting_Click( object sender, RoutedEventArgs e )
        {

        }

        // 파일 드래그 앤 드랍 
        private void lstView_DragDrop( object sender, DragEventArgs e )
        {
            //if( e.Data.GetDataPresent( DataFormats.FileDrop, true ) )
            //{
            //    e.Effects = DragDropEffects.Copy;
            //}

            //lstView.Items.Clear();


            if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                string[] DragDropItems = e.Data.GetData( DataFormats.FileDrop, true ) as string[];

                List<string> DropFolder = new List<string>();
                List<string> DropFile = new List<string>();

                // 폴더와 파일을 동시에 드래그 했을 때
                // 폴더와 파일을 각 각의 리스트에 담기.
                foreach( var item in DragDropItems )
                {
                    if( Directory.Exists( item.ToString() ) )
                    {
                        DropFolder.Add( item.ToString() );
                    }
                    else
                    {
                        DropFile.Add( item.ToString() );
                    }
                }

                if( DropFolder != null )
                {
                    TraverseTree( DropFolder.ToArray() );
                }

                if( DropFile != null )
                {
                    ListView_AddFile( DropFile.ToArray() );
                }
            }

        }

        private void ListView_AddFile( string[] files )
        {
            foreach( string file in files )
            {
                // 리스트뷰 아이템 추가
                long fileSize = new FileInfo( file ).Length;

                //ListViewDataSource listView = Resources[ "listViewDataSource" ] as ListViewDataSource;
                listView = Resources[ "listViewDataSource" ] as ListViewDataSource;

                listView.Add( new SelectionFileData( file, ( fileSize / 1024 + 1 ) ) ); //.ToString( "#,#" ) ) );                
            }
        }

        // 진행바 갱신
        void ReportProgress( int value )
        {
            //Update the UI to reflect the progress value that is passed back.
            progressBar.Value = value;
            TaskbarItemInfo.ProgressValue = ( double )value / 100;
        }

        // 파일 크기별로 정렬
        private void lvUsersColumnHeader_Click( object sender, RoutedEventArgs e )
        {
            GridViewColumnHeader column = ( sender as GridViewColumnHeader );
            string sortBy = column.Tag.ToString();
            if( listViewSortCol != null )
            {
                AdornerLayer.GetAdornerLayer( listViewSortCol ).Remove( listViewSortAdorner );
                lstView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if( listViewSortCol == column && listViewSortAdorner.Direction == newDir )
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner( listViewSortCol, newDir );
            AdornerLayer.GetAdornerLayer( listViewSortCol ).Add( listViewSortAdorner );
            lstView.Items.SortDescriptions.Add( new SortDescription( sortBy, newDir ) );
        }

    }



    // 컬럼을 정렬하기 위해서 사용하는 클래스
    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
                Geometry.Parse( "M 0 4 L 3.5 0 L 7 4 Z" );

        private static Geometry descGeometry =
                Geometry.Parse( "M 0 0 L 3.5 4 L 7 0 Z" );

        public ListSortDirection Direction { get; private set; }

        public SortAdorner( UIElement element, ListSortDirection dir )
                : base( element )
        {
            this.Direction = dir;
        }

        protected override void OnRender( DrawingContext drawingContext )
        {
            base.OnRender( drawingContext );

            if( AdornedElement.RenderSize.Width < 20 )
                return;

            TranslateTransform transform = new TranslateTransform
                    (
                            AdornedElement.RenderSize.Width - 15,
                            ( AdornedElement.RenderSize.Height - 5 ) / 2
                    );
            drawingContext.PushTransform( transform );

            Geometry geometry = ascGeometry;
            if( Direction == ListSortDirection.Descending )
                geometry = descGeometry;
            drawingContext.DrawGeometry( Brushes.Black, null, geometry );

            drawingContext.Pop();
        }
    }
}

