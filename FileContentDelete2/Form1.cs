using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileContentDelete
{
    public partial class Form : System.Windows.Forms.Form
    {
        Stack<string> DeleteSubDirs = new Stack<string>();

        public Form()
        {
            InitializeComponent();
            InitializeOpenFileDialog();
        }

        private void InitializeOpenFileDialog()
        {
            // Set the file dialog to filter for graphics files.
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = @"C:\Temp";

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "파일 선택";

            openFileDialog.FileName = string.Empty;
            btnListBoxClear.Enabled = false;
        }

        private void btnFileSelect_Click( object sender, EventArgs e )
        {
            FileSelect();
        }

        private void FileSelect()
        {
            DialogResult dr = openFileDialog.ShowDialog();

            if( dr == DialogResult.OK )
            {
                ListView_AddFile( openFileDialog.FileNames );
            }
        }

        void ReportProgress( int value )
        {
            //Update the UI to reflect the progress value that is passed back.
            progressBar.Value = value;
        }

        public async Task<int> DeleteFileAsync( int totalCount, IProgress<int> progress )
        {
            int tempCount = 1;
            try
            {
                //processCount = await Task.Run( async () =>
                //{
                //    int tempCount = 0;
                //    //await the processing and delete file logic here
                //    for( int i = 0; i < totalCount; i++ )
                //    {
                //        //int processed = await DeleteFileProcess( tempCount );
                //        await StreamFileWrite( listView.Items[ i ].Text );
                //        if( progress != null )
                //        {
                //            progress.Report( ( tempCount * 100 / totalCount ) );
                //        }
                //        tempCount++;
                //    }
                //    return tempCount;
                //} );

                //await the processing and delete file logic here
                for( int i = 0; i < totalCount; i++ )
                {
                    await StreamFileWrite( listView.Items[ i ].Text );
                    //await ByteArrayWrite( listView.Items[ i ].Text );
                    if( progress != null )
                    {
                        progress.Report( ( tempCount * 100 / totalCount ) );
                    }
                    tempCount++;
                }
                return tempCount;
            }

            catch( Exception e )
            {
                Debug.Write( e.Message );
            }
            return tempCount;
        }

        private async Task<int> DeleteFileProcess( int i )
        {
            long length = 0;

            if( DeleteSubDirs.Count != 0 || listView.Items.Count != 0 )
            {
                length = new FileInfo( listView.Items[ i ].Text ).Length;

                if( length <= 50000000 )
                {
                    // 500메가 이하
                    await ByteArrayWrite( listView.Items[ i ].Text );
                }
                else
                {
                    // 501 메가 이상 
                    await StreamFileWrite( listView.Items[ i ].Text );
                }
                btnListBoxClear.Enabled = true;
            }
            return i;
        }

        //
        private async void btnErase_Click( object sender, EventArgs e )
        {
            var progressIndicator = new Progress<int>( ReportProgress );
            int progressDelete = await DeleteFileAsync( listView.Items.Count, progressIndicator );

            btnListBoxClear.Enabled = true;

            //int progressDelete = await DeleteFileAsync( listView, new Progress<int>( percent => progressBar1.Value = percent ) );

            //long length = 0;
            //int totalCount = listView.Items.Count;

            //if( DeleteSubDirs.Count != 0 || listView.Items.Count != 0 )
            //{
            //    // 선택한 파일 삭제
            //    for( int i = 0; i < listView.Items.Count; i++ )
            //    {
            //        this.Text =  --totalCount + " 개 남음";

            //        length = new FileInfo( listView.Items[ i ].Text ).Length;

            //        if( length <= 50000000 )
            //        {
            //            // 500메가 이하
            //            await ByteArrayWrite( listView.Items[ i ].Text );
            //        }
            //        else
            //        {
            //            // 501 메가 이상 
            //            await StreamFileWrite( listView.Items[ i ].Text );
            //        }
            //    }
            //    btnListBoxClear.Enabled = true;
            //}
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
                    count = ( streamWrite.Length / blockSize ) + 1;

                    for( int i = 0; i < count; i++ )
                    {
                        await streamWrite.WriteAsync( data, 0, data.Length );
                    }

                    //if( streamWrite.Length > count * blockSize )
                    //{
                    //    byte[] Last_Data = new byte[ streamWrite.Length - ( count * blockSize ) ];
                    //    await streamWrite.WriteAsync( Last_Data, 0, Last_Data.Length );
                    //}
                }
                catch( Exception e )
                {
                    MessageBox.Show( e.ToString() );
                }
            }
        }

        private async Task ByteArrayWrite( string filename )
        {
            using( FileStream streamWrite = new FileStream( filename, FileMode.Open, FileAccess.Write ) )
            {
                try
                {
                    byte[] _ByteArray = new byte[ streamWrite.Length ];

                    // 바이트 블록 단위로 파일 쓰기( 바이트 블록, 시작 위치, Write Size )
                    await streamWrite.WriteAsync( _ByteArray, 0, _ByteArray.Length );

                }
                catch( Exception e )
                {
                    MessageBox.Show( e.ToString() );
                }
            };
        }

        // 리스트 박스 목록 지우기
        private void btnListBoxClear_Click( object sender, EventArgs e )
        {
            string NewFileName;
            string OldFileName;

            for( int i = 0; i < listView.Items.Count; i++ )
            {
                NewFileName = Path.GetDirectoryName( listView.Items[ i ].Text ) + @"\1.tmp";
                OldFileName = listView.Items[ i ].Text;

                // 파일 이름 변경
                File.Move( OldFileName, NewFileName );
                File.Delete( NewFileName );
            }

            DeleteSubDirectory( DeleteSubDirs );

            listView.Items.Clear();
            btnListBoxClear.Enabled = false;
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

            //if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
            //{
            //    var DragDropItem = ( ( string[] )e.Data.GetData( DataFormats.FileDrop ) )[ 0 ];
            //    if( Directory.Exists( DragDropItem ) )
            //    {
            //        TraverseTree( DragDropItems );
            //    }
            //    else
            //    {
            //        ListView_AddFile( DragDropItems );
            //    }
            //}
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

        private void MemoryMappingFile( string filePath, long startPosition, long endPosition )
        {
            //MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile( _FileName );
            //MemoryMappedViewStream mms = mmf.CreateViewStream();
            //using( BinaryReader b = new BinaryReader( mms ) )
            //{

            //}

            long offset = 0;
            long length = 1024;

            // Create the memory-mapped file.
            using( var mmf = MemoryMappedFile.CreateFromFile( filePath, FileMode.Open, "ImgA" ) )
            {
                // Create a random access view, from the 256th megabyte (the offset)
                // to the 768th megabyte (the offset plus length).
                using( var accessor = mmf.CreateViewAccessor( offset, length ) )
                {
                    int colorSize = Marshal.SizeOf( typeof( MyColor ) );
                    MyColor color;

                    // Make changes to the view.
                    for( long i = 0; i < length; i += colorSize )
                    {
                        accessor.Read( i, out color );
                        color.Brighten( 10 );
                        accessor.Write( i, ref color );
                    }
                }
            }
        }
        public struct MyColor
        {
            public short Red;
            public short Green;
            public short Blue;
            public short Alpha;

            // Make the view brighter.
            public void Brighten( short value )
            {
                Red = ( short )Math.Min( short.MaxValue, Red + value );
                Green = ( short )Math.Min( short.MaxValue, Green + value );
                Blue = ( short )Math.Min( short.MaxValue, Blue + value );
                Alpha = ( short )Math.Min( short.MaxValue, Alpha + value );
            }
        }

        private void listView_MouseClick( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Right )
            {
                //ContextMenuStrip.Show( Cursor.Position );
                if( listView.FocusedItem.Bounds.Contains( e.Location ) == true )
                {
                    ContextMenuStrip.Show( Cursor.Position );
                }
            }

            //if( e.Button == MouseButtons.Right )
            //{
            //    EventHandler ConTextMenuEvent = new EventHandler( ConTextMenu );
            //    MenuItem[] ConTextMenuItem =
            //    {
            //        new MenuItem( "목록에서 삭제", ConTextMenuEvent ),
            //        new MenuItem( "-", ConTextMenuEvent ),
            //        new MenuItem( "파일 속성 변경", ConTextMenuEvent )
            //    };
            //    ContextMenu = new ContextMenu( ConTextMenuItem );

            //}
        }

        // MenuItem를 사용한 방법 1
        //private void ConTextMenu( object sender, EventArgs e )
        //{
        //    MenuItem ContextMenuItem = ( MenuItem )sender;
        //    string MenuString = ContextMenuItem.Text;

        //    switch( MenuString )
        //    {
        //        case "목록에서 삭제":
        //            ListItemDelete();
        //            break;
        //        case "파일 속성 변경":
        //            ListItemChangeAttribute();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private void ListItemDelete()
        {
            foreach( ListViewItem item in listView.SelectedItems )
            {
                item.Remove();
            }
        }

        private void ListItemChangeAttribute()
        {
            foreach( ListViewItem item in listView.SelectedItems )
            {
                File.SetAttributes( item.Text, FileAttributes.Normal );
            }
        }

        void ContextStripMenu_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
        {
            ToolStripItem item = e.ClickedItem;

            switch( item.Text )
            {
                case "선택 파일 삭제":
                    ListItemDelete();
                    break;
                case "파일 속성 변경":
                    ListItemChangeAttribute();
                    break;
                default:
                    break;
            }
        }
    }
}
