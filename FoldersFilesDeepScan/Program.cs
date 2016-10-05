using System;
using System.Collections.Generic;
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

        static void Main( string[] args )
        {
            //ProcessDir( @"G:\Baidu", 5 );
            ProcessDir( @"G:\Baidu" );
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
                    using( StreamWriter fs = new StreamWriter( @"G:\Baidu\allfile.txt", true ) )
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
    }
}
