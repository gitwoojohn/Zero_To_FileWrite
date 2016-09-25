using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemEventArgs_Demo
{
    class Program
    {
        // 특정 하드 드라이브의 파일 생성, 변경, 삭제 이벤트 처리
        static void Main( string[] args )
        {
            //  Create a FileSystemWatcher to monitor all files on drive C.
            FileSystemWatcher fsw = new FileSystemWatcher( @"C:\Temp" );

            //  Watch for changes in LastAccess and LastWrite times, and
            //  the renaming of files or directories. 
            fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // 필터 적용 가능
            //fsw.Filter = "*.txt";

            //  Register a handler that gets called when a 
            //  file is created, changed, or deleted.
            //fsw.Changed += new FileSystemEventHandler( OnChanged );
            //fsw.Created += new FileSystemEventHandler( OnCreated );
            //fsw.Deleted += new FileSystemEventHandler( OnDeleted );

            fsw.Changed += new FileSystemEventHandler( ( o, e ) => { UpdateConsolWrite( e.Name + " : is Changed"); } );
            fsw.Created += new FileSystemEventHandler( ( o, e ) => { UpdateConsolWrite( e.Name + " : is 생성" ); } );
            fsw.Deleted += new FileSystemEventHandler( ( o, e ) => { UpdateConsolWrite( e.Name + " : is 삭제" ); } );

            // Register a handler that gets called when a file is renamed.
            fsw.Renamed += new RenamedEventHandler( OnRenamed );

            // Register a handler that gets called if the 
            // FileSystemWatcher needs to report an error.
            fsw.Error += new ErrorEventHandler( OnError );

            // Begin watching.
            // 파일 감시 시작
            fsw.EnableRaisingEvents = true;

            // 내부 버퍼 오류시 버퍼 크기 증가( 기본 8192(8KB) )
            //fsw.InternalBufferSize = 8192;

            Console.WriteLine( "Press \'q\' to quit the sample." );
            while( Console.Read() != 'q' ) ;

        }
        private static void UpdateConsolWrite(string text)
        {
            //MethodInvoker mi = new MethodInvoker( () => { text; } );
            //this.Invoke( mi );
            Console.WriteLine( text );
        }
        //  This method is called when a file is created, changed, or deleted.
        private static void OnChanged( object source, FileSystemEventArgs e )
        {
            // Show that a file has been changed.
            // 파일 생성, 내용 변경, 삭제시
            //WatcherChangeTypes wct = e.ChangeType;
            //Console.WriteLine( "File {0} {1}", e.FullPath, wct.ToString() );
        }
        private static void OnCreated( object source, FileSystemEventArgs e )
        {
            // Show that a file has been created.
            // 파일 생성시
            //WatcherChangeTypes wct = e.ChangeType;
            //Console.WriteLine( "File {0} {1}", e.FullPath, wct.ToString() );
            FileInfo file = new FileInfo( e.FullPath );
            Console.WriteLine( file.Name + "\t*** 생성 또는 복사" ); // this is what you're looking for.
        }
        private static void OnDeleted( object source, FileSystemEventArgs e )
        {
            // Show that a file has been deleted.
            // 파일 생성시
            //WatcherChangeTypes wct = e.ChangeType;
            //Console.WriteLine( "File {0} {1}", e.FullPath, wct.ToString() );
            FileInfo file = new FileInfo( e.FullPath );
            Console.WriteLine( file.Name + "\t*** 파일 삭제"); // this is what you're looking for.
        }
        // This method is called when a file is renamed.
        // 파일 이름 변경시
        private static void OnRenamed( object source, RenamedEventArgs e )
        {
            //  Show that a file has been renamed.
            WatcherChangeTypes wct = e.ChangeType;
            Console.WriteLine( "File {0}을 {1}로 {2} 했습니다.", e.OldFullPath, e.FullPath, wct.ToString() );
        }

        //  This method is called when the FileSystemWatcher detects an error.
        private static void OnError( object source, ErrorEventArgs e )
        {
            //  Show that an error has been detected.
            Console.WriteLine( "The FileSystemWatcher has detected an error" );

            //  Give more information if the error is due to an internal buffer overflow.
            if( e.GetException().GetType() == typeof( InternalBufferOverflowException ) )
            {
                // 내부 버퍼 오버플로우시 발생 ( 기본 버퍼 크기 8192(8KB))
                // InternalBufferSize { get; set; }
                //  This can happen if Windows is reporting many file system events quickly 
                //  and internal buffer of the  FileSystemWatcher is not large enough to handle this
                //  rate of events. The InternalBufferOverflowException error informs the application
                //  that some of the file system events are being lost.
                Console.WriteLine( ( "The file system watcher experienced an internal buffer overflow: " + e.GetException().Message ) );
            }
        }
    }
}
