using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoldersFilesDeepScan
{
    class Program
    {
        // How much deep to scan. (of course you can also pass it to the method)
        const int HowDeepToScan = 10000;
        static string[] fileEntries = null;
        static string[] SubDirectoryEntries = null;

        static Stack<string> DeleteSubDirs = new Stack<string>();

        static void Main( string[] args )
        {
            // 재귀 방식 1 - 재귀 호출 제한
            //ProcessDir( @"G:\Baidu", 5 );

            // 재귀 방식 2 - 모든 하위 디렉토리 포함
            //ProcessDir( @"G:\Baidu" );

            // Stack을 이용한 다른 방식( 복잡하고 중첩된 디렉토리의 스택 오버 플로우 방지 )
            //TraverseTree( @"G:\Temp" );

            // 전체 디렉토리 지우기
            //DeleteSubDirectory( DeleteSubDirs );

            //Console.WriteLine( "폴더 지우기 완료" );


            SimpleGetDirectory();
            Console.ReadLine();
        }

        // 모든 하위 디렉토리의 파일
        private static void ProcessDir( string SourceDir )
        {
            if( 1 <= HowDeepToScan )
            {
                fileEntries = Directory.GetFiles( SourceDir );

                foreach( string fileName in fileEntries )
                {
                    using( StreamWriter fs = new StreamWriter( @"C:\Sub\allfile.txt", true ) )
                    {
                        fs.WriteLine( fileName );
                    }
                    // do something with fileName
                    //Console.WriteLine( fileName );
                }

                SubDirectoryEntries = Directory.GetDirectories( SourceDir );

                foreach( string SubDirectory in SubDirectoryEntries )
                    // Do not iterate through reparse points
                    if( ( File.GetAttributes( SubDirectory ) &
                         FileAttributes.ReparsePoint ) !=
                             FileAttributes.ReparsePoint )
                        ProcessDir( SubDirectory );
            }
        }

        // 재귀 단계 조정
        //private static void ProcessDir( string SourceDir, int ReCursionLevel )
        //{
        //    //if( ReCursionLevel <= HowDeepToScan )
        //    if(  ReCursionLevel <= HowDeepToScan )
        //    {
        //        // Process the list of files found in the directory. 
        //        fileEntries = Directory.GetFiles( SourceDir );

        //        foreach( string fileName in fileEntries )
        //        {
        //            using( StreamWriter fs = new StreamWriter( @"G:\Baidu\allfile.txt", true ) )
        //            {
        //                fs.WriteLine( fileName );
        //            }
        //            // do something with fileName
        //            //Console.WriteLine( fileName );
        //        }

        //        // Recurse into subdirectories of this directory.
        //        SubDirectoryEntries = Directory.GetDirectories( SourceDir );
        //        foreach( string SubDirectory in SubDirectoryEntries )
        //            // Do not iterate through reparse points
        //            if( ( File.GetAttributes( SubDirectory ) &
        //                 FileAttributes.ReparsePoint ) !=
        //                     FileAttributes.ReparsePoint )

        //                ProcessDir( SubDirectory, ReCursionLevel + 1 );
        //    }
        //}

        // 일반적으로 재귀 방식을 쓰지만 복잡하거나 중첩 규모가 크면 스택 오버 플로우 발생 가능성
        private static void TraverseTree( string SourceDir )
        {
            Stack<string> dirs = new Stack<string>( 20 );
            if( !Directory.Exists( SourceDir ) )
            {
                throw new DirectoryNotFoundException();
            }

            // 스택에 소스 경로 넣기( Push )
            dirs.Push( SourceDir );

            while( dirs.Count > 0 )
            {
                string CurrentDir = dirs.Pop();
                string[] SubDirs = null;

                try
                {
                    SubDirs = Directory.GetDirectories( CurrentDir );
                    foreach( var SubDir in SubDirs )
                    {
                        DeleteSubDirs.Push( SubDir.ToString() );
                        //Console.WriteLine(SubDir.ToString() );
                    }
                }
                catch( UnauthorizedAccessException e )
                {
                    Debug.WriteLine( e.Message );
                }

                string[] files = null;
                try
                {
                    files = Directory.GetFiles( CurrentDir );
                }
                catch( UnauthorizedAccessException e )
                {
                    Debug.WriteLine( e.Message );
                }

                foreach( string file in files )
                {
                    try
                    {
                        FileInfo fi = new FileInfo( file );
                        //Console.WriteLine( "{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime );
                    }
                    catch( FileNotFoundException e )
                    {
                        Debug.WriteLine( e.Message );
                    }
                }

                foreach( string SubDir in SubDirs )
                {
                    dirs.Push( SubDir );
                }
            }
        }

        private static void DeleteSubDirectory( Stack<string> SubDirs )
        {
            int Count = SubDirs.Count();
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

        private static void SimpleGetDirectory()
        {
            try
            {
                string dirPath = @"C:\Temp";

                List<string> dirs = new List<string>( Directory.EnumerateDirectories( dirPath ) );
                foreach( var dir in dirs )
                {
                    Console.WriteLine( "{0}", dir.Substring( dir.LastIndexOf( "\\" ) + 1 ) );
                }
                Console.WriteLine( "{0} directories found.", dirs.Count );
            }
            catch( UnauthorizedAccessException UAEx )
            {
                Console.WriteLine( UAEx.Message );
            }
            catch( PathTooLongException PathEx )
            {
                Console.WriteLine( PathEx.Message );
            }
        }

    }
}
