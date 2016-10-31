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
    public partial class Form1 : Form
    {
        public Form1()
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

        private async void btnErase_Click( object sender, EventArgs e )
        {
            long length = 0;

            // 선택한 파일 삭제
            for( int i = 0; i < listView.Items.Count; i++ )
            {
                length = new FileInfo( listView.Items[ i ].Text ).Length;


                if( length <= 50000000 )
                {
                    // 500메가 이하
                    await ByteArrayToFile( listView.Items[ i ].Text );
                }
                else
                {
                    // 501 메가 이상 
                    await BinaryFileWrite( listView.Items[ i ].Text );
                }
            }
            btnListBoxClear.Enabled = true;
        }

        private async Task BinaryFileWrite( string filePath )
        {
            const int blockSize = 1024 * 16; //8;
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

        private async Task ByteArrayToFile( string _FileName )
        {
            using( FileStream _FileStream = new FileStream( _FileName, FileMode.Open, FileAccess.Write ) )
            {
                try
                {
                    byte[] _ByteArray = new byte[ _FileStream.Length ];

                    // 바이트 블록 단위로 파일 쓰기( 바이트 블록, 시작 위치, Write Size )
                    await _FileStream.WriteAsync( _ByteArray, 0, _ByteArray.Length );

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
            string[] files = ( string[] )e.Data.GetData( DataFormats.FileDrop );

            ListView_AddFile( files );
        }

        private void ListView_AddFile( string[] files )
        {
            foreach( string file in files )
            {
                // 리스트 뷰의 각 아이템
                ListViewItem item = new ListViewItem( file );

                long fileSize = new FileInfo( file ).Length;

                item.SubItems.Add( ( fileSize / 1024 + 1 ).ToString( "#,#" ) ); //.ToString( "#,#" ) );

                // 리스트 뷰에 아이템 입력
                listView.Items.Add( item );
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
                EventHandler ConTextMenuEvent = new EventHandler( ConTextMenu );
                MenuItem[] ConTextMenuItem = {
                    new MenuItem( "목록에서 삭제", ConTextMenuEvent ),
                    new MenuItem( "-", ConTextMenuEvent ),
                    new MenuItem( "파일 속성 변경", ConTextMenuEvent )
                };
                ContextMenu = new ContextMenu( ConTextMenuItem );
            }
        }
        private void ConTextMenu( object sender, EventArgs e )
        {
            MenuItem ContextMenuItem = ( MenuItem )sender;
            String MenuString = ContextMenuItem.Text;

            switch( MenuString )
            {
                case "목록에서 삭제":
                    ListItemDelete();
                    break;
                case "파일 속성 변경":
                    ListItemChangeAttribute();
                    break;
                default:
                    Console.WriteLine( "Default case" );
                    break;
            }
        }

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
    }
}
