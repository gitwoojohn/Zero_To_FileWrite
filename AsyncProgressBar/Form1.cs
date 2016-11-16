using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncProgressBar
{
    public partial class Form1 : Form
    {
        Stack<string> DeleteSubDirs = new Stack<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnStart_Click( object sender, EventArgs e )
        {
            var progressIndicator = new Progress<int>( ReportProgress );
            int progressDelete = await DeleteFileAsync( listView.Items.Count, progressIndicator );
        }

        private async Task<int> DeleteFileAsync( int totalCount, IProgress<int> progress )
        {
            int processCount = 0;
            try
            {
                //processCount = await Task.Run( async () =>
                //{
                //    int tempCount = 0;

                //    //await the processing and delete file logic here
                //    for( int i = 0; i <= totalCount; i++ )
                //    {
                //        //await DeleteFileProcess( tempCount );
                //        await StreamFileWrite( listView.Items[ i ].Text );
                //        if( progress != null )
                //        {
                //            progress.Report( ( tempCount * 100 / totalCount ) );
                //        }
                //        tempCount++;
                //    }
                //    return tempCount;
                //} );
                int tempCount = 1;

                //await the processing and delete file logic here
                for( int i = 0; i < totalCount; i++ )
                {
                    //await DeleteFileProcess( tempCount );
                    await StreamFileWrite( listView.Items[ i ].Text );
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
            return processCount;
        }

        private async Task DeleteFileProcess( int tempCount )
        {
            await Task.Delay( 500 );
        }

        private void ReportProgress( int value )
        {
            Debug.Write( value );
            progressBar1.Value = value;
        }

        private async Task StreamFileWrite( string filePath )
        {
            const int blockSize = 1024 * 8; //16; 
            byte[] data = new byte[ blockSize ];
            long count = 0;

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

        // 파일 드래그 앤 드랍 
        private void listView_DragDrop( object sender, DragEventArgs e )
        {
            if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void listView_DragEnter( object sender, DragEventArgs e )
        {
            string[] DragDropItems = ( string[] )e.Data.GetData( DataFormats.FileDrop );

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

        private void ListView_AddFile( string[] files )
        {
            foreach( string file in files )
            {
                // 리스트 뷰의 각 아이템
                ListViewItem item = new ListViewItem( file );

                long fileSize = new FileInfo( file ).Length;

                item.SubItems.Add( ( fileSize / 1024 + 1 ).ToString( "#,#" ) ); //.ToString( "#,#" ) );

                // 각 파일의 속성을 일반 속성으로 변경
                File.SetAttributes( file, FileAttributes.Normal );

                // 리스트 뷰에 아이템 입력
                listView.Items.Add( item );
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

                        // 스택에 폴더 넣기( 선입후출 - FILO )
                        DeleteSubDirs.Push( SubDir.ToString() );
                    }
                    files = Directory.GetFiles( CurrentDir );
                    ListView_AddFile( files );
                }
                catch( UnauthorizedAccessException e )
                {
                    Debug.WriteLine( e.Message );
                }


                //try
                //{
                //    files = Directory.GetFiles( CurrentDir );
                //    ListView_AddFile( files );
                //}
                //catch( UnauthorizedAccessException e )
                //{
                //    Debug.WriteLine( e.Message );
                //}

                //foreach( string file in files )
                //{
                //    try
                //    {
                //FileInfo fi = new FileInfo( file );
                //Console.WriteLine( "{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime );
                //    }
                //    catch( FileNotFoundException e )
                //    {
                //        Debug.WriteLine( e.Message );
                //    }
                //}
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
    }
}
