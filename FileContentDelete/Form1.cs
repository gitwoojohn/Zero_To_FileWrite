using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
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
                //foreach( string itemFile in openFileDialog.FileNames )
                //{
                //    // 리스트 뷰의 각 아이템
                //    ListViewItem item = new ListViewItem( itemFile );

                //    // 파일의 크기
                //    using( FileStream fs = new FileStream( itemFile, FileMode.Open ) )
                //    {
                //        string fileSize = ( fs.Length / 1024 ).ToString( "#,#" );
                //        item.SubItems.Add( fileSize );
                //    };

                //    // 리스트 뷰에 아이템 입력
                //    listView.Items.Add( item );
                //}
            }
        }

        // 비동기 TAP(Task Asynchronous Pattern) 방식으로 파일 처리
        private void btnErase_Click( object sender, EventArgs e )
        {
            // 선택한 파일 삭제
            for( int i = 0; i < listView.Items.Count; i++ )
            {
                ByteArrayToFile( listView.Items[ i ].Text );
            }
        }

        private void ByteArrayToFile( string _FileName )
        {
            using( FileStream _FileStream = new FileStream( _FileName, FileMode.Open, FileAccess.Write ) )
            {
                try
                {
                    byte[] _ByteArray = new byte[ _FileStream.Length ];

                    int i = 0;

                    if( _FileStream.Length >= 500000000 )
                    {
                        double c = _FileStream.Length / 500000000;
                        double quotient = Math.Truncate( c );


                        for( ; i < quotient; i++ )
                        {
                            _FileStream.Write( _ByteArray, i * 500000000, 500000000 );
                            Thread.Sleep( 10 );
                        }

                        int Last_ByteArray = Convert.ToInt32( _FileStream.Length % 500000000 );
                        _FileStream.Write( _ByteArray, ( ( i - 1 ) * 500000000 ), Last_ByteArray );
                    }
                    else
                    {
                        // 바이트 블록 단위로 파일 쓰기( 바이트 블록, 시작 위치, Write Size )
                        _FileStream.Write( _ByteArray, 0, _ByteArray.Length );
                    }
                }
                catch( Exception )
                {
                    //MessageBox.Show( e.ToString() );

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

                    MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile( _FileName );
                    MemoryMappedViewStream mms = mmf.CreateViewStream();
                    using( BinaryReader b = new BinaryReader( mms ) )
                    {

                    }

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

            //foreach( string file in files )
            //{

            //    // 리스트 뷰의 각 아이템
            //    ListViewItem item = new ListViewItem( file );

            //    // 파일의 크기
            //    using( FileStream fs = new FileStream( file, FileMode.Open ) )
            //    {
            //        string fileSize = ( fs.Length / 1024 ).ToString( "#,#" );
            //        item.SubItems.Add( fileSize );
            //    };

            //    // 리스트 뷰에 아이템 입력
            //    listView.Items.Add( item );
            //}
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
    }
}
