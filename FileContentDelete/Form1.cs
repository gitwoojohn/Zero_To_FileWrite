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

        private void btnErase_Click( object sender, EventArgs e )
        {
            // 선택한 파일 삭제
            for( int i = 0; i < listView.Items.Count; i++ )
            {
                ByteArrayToFile( listView.Items[ i ].Text );
            }
        }

        private void BinaryFileWrite( string filePath )
        {
            const int blockSize = 1024 * 8;
            byte[] data = new byte[ blockSize ];
            long count = 0;

            using( FileStream streamWrite = File.OpenWrite( filePath ) )
            {
                count = streamWrite.Length / blockSize;

                for( int i = 0; i < count; i++ )
                {
                    streamWrite.Write( data, 0, data.Length );
                }

                if( streamWrite.Length > count * 8192 )
                {
                    byte[] Last_Data = new byte[ streamWrite.Length - ( count * 8192 ) ];
                    streamWrite.Write( Last_Data, 0, Last_Data.Length );
                }
            }
        }

        private void ByteArrayToFile( string _FileName )
        {
            using( FileStream _FileStream = new FileStream( _FileName, FileMode.Open, FileAccess.Write ) )
            {
                try
                {
                    byte[] _ByteArray = new byte[ _FileStream.Length ];

                    //int i = 0;

                    if( _FileStream.Length > 50000000 ) /*( _FileStream.Length >= 500000000 )*/
                    {
                        _FileStream.Close();
                        BinaryFileWrite( _FileName );
                        //double c = _FileStream.Length / 500000000;
                        //double quotient = Math.Truncate( c );


                        //for( ; i < quotient; i++ )
                        //{
                        //    _FileStream.Write( _ByteArray, i * 500000000, 500000000 );
                        //    Thread.Sleep( 10 );
                        //}

                        //int Last_ByteArray = Convert.ToInt32( _FileStream.Length % 500000000 );
                        //_FileStream.Write( _ByteArray, ( ( i - 1 ) * 500000000 ), Last_ByteArray );
                    }
                    else
                    {
                        // 바이트 블록 단위로 파일 쓰기( 바이트 블록, 시작 위치, Write Size )
                        _FileStream.Write( _ByteArray, 0, _ByteArray.Length );
                    }
                }
                catch( Exception e )
                {
                    MessageBox.Show( e.ToString() );

                    // 방법 1 파일 길이에 따른 처리 시간 너무 길어짐
                    //Stopwatch sw = new Stopwatch();
                    //sw.Start();
                    //for( int i = 0; i < _FileStream.Length; i++ )
                    //{
                    //    Thread.Sleep( 10 );
                    //    _FileStream.WriteByte( 0x00 );
                    //}
                    //sw.Stop();
                    //string result = ( sw.ElapsedMilliseconds / 1000 ).ToString();
                    //MessageBox.Show( result );

                    //_FileStream.Close();
                    //_FileStream = new FileStream( _FileName, FileMode.Create );

                    //MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile( _FileName );
                    //MemoryMappedViewStream mms = mmf.CreateViewStream();
                    //using( BinaryReader b = new BinaryReader( mms ) )
                    //{

                    //}


                    // MemoryMappingFile 방식 
                    //byte[] _ByteArray = new byte[ _FileStream.Length ];
                    //int i = 0;

                    //double c = _FileStream.Length / 500000000;
                    //double quotient = Math.Truncate( c );


                    //for( ; i < quotient; i++ )
                    //{
                    //    //_FileStream.Write( _ByteArray, i * 500000000, 500000000 );

                    //    MemoryMappingFile( _FileName, 1, 1 );
                    //    Thread.Sleep( 10 );
                    //}

                    //long Last_Position = Convert.ToInt32( _FileStream.Length % 500000000 );
                    ////_FileStream.Write( _ByteArray, ( ( i - 1 ) * 500000000 ), Last_ByteArray );

                    //MemoryMappingFile( _FileName, ( i - 1 ) * 500000000, Last_Position );


                    //BinaryFileWrite( _FileName );
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

                // 파일의 크기
                using( FileStream fs = new FileStream( file, FileMode.Open ) )
                {
                    string fileSize = ( ( fs.Length / 1024 ) + 1 ).ToString( "#,#" );
                    item.SubItems.Add( fileSize );
                };

                // 리스트 뷰에 아이템 입력
                listView.Items.Add( item );
            }
        }

        private void MemoryMappingFile( string filePath, long startPosition, long endPosition )
        {
            long offset = 0; //0x00000000 * startPosition; // offset 0
            long length = 1024; //0x20000000 * endPosition; // 512 megabytes

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
    }
}
