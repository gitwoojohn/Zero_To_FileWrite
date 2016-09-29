using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MemoryMappingFiles
{
    class Program
    {
        // 1 GB
        private static int fileSizeMb = 3072; // 1024;
        private static string filePath = @"C:\Temp\MemoryMappingDemo.bin";

        static void Main( string[] args )
        {
            //using( StreamReader streamReader = new StreamReader( filePath ) )
            //{
            //    string line = streamReader.ReadToEnd();
            //}

            //// 좀 더 빠른 읽기
            //using( FileStream fs = File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
            //using( BufferedStream bs = new BufferedStream( fs ) )
            //using( StreamReader sr = new StreamReader( bs ) )
            //{
            //    string line;
            //    while( ( line = sr.ReadLine() ) != null )
            //    {

            //    }
            //}


            if( File.Exists( filePath ) == false )
            {
                BinaryFileCreate();
            }

            //MemoryMappingFile_2( filePath );
            MemoryMappingFile_3();


            //BinaryFileWrite();

            //int i = 0, j = 1;

            //long fileSize = new FileInfo( filePath ).Length;

            //double c = fileSize / 500000000;
            //double quotient = Math.Truncate( c );

            //for( ; i < quotient; i++ )
            //{
            //    MemoryMappingFile( filePath, i * 500000000, ( i + j ) * 500000000 );
            //    Thread.Sleep( 5000 );
            //}

            //long Start_Position = Convert.ToInt32( quotient ) * 500000000;
            //long Last_Position = fileSize % 500000000;


            //MemoryMappingFile( filePath, Start_Position, Last_Position );

        }

        private static void BinaryFileCreate()
        {
            const int blockSize = 1024 * 8;
            const int blocksPerMb = ( 1024 * 1024 ) / blockSize;

            int count = fileSizeMb * blocksPerMb;

            byte[] data = new byte[ blockSize ];
            Random rng = new Random();
            //using (FileStream streamWriter = File.OpenWrite(filePath))
            using( StreamWriter streamWriter = new StreamWriter( filePath ) )
            {
                // There 
                for( int i = 0; i < count; i++ )
                {
                    //rng.NextBytes( data );
                    streamWriter.BaseStream.Write( data, 0, data.Length );
                    //streamWriter.Write(data, 0, data.Length);

                }
            }
        }

        private static void BinaryFileWrite()
        {
            const int blockSize = 1024 * 8;
            //const int blocksPerMb = ( 1024 * 1024 ) / blockSize;
            int count = 0;  //fileSizeMb * blocksPerMb;

            using( FileStream streamWrite = File.OpenWrite( filePath ) )
            {
                count = ( int )streamWrite.Length / blockSize;
                //Debug.Write( streamWrite.Length );
            }


            byte[] data = new byte[ blockSize ];
            Random rng = new Random();
            using( FileStream streamWrite = File.OpenWrite( filePath ) )
            //using( StreamWriter streamWriter = new StreamWriter( filePath ) )
            {
                // There 
                for( int i = 0; i < count; i++ )
                {
                    //rng.NextBytes( data );
                    //streamWrite.BaseStream.Write( data, 0, data.Length );
                    streamWrite.Write( data, 0, data.Length );

                }
            }
        }

        private static void MemoryMappingFile_2( string filePath )
        {
            long length = new FileInfo( filePath ).Length;

            // Create the memory-mapped file.
            using( var mmf = MemoryMappedFile.CreateFromFile( filePath, FileMode.Open, "ImgA" ) )
            {
                // Create a random access view, from the 256th megabyte (the offset)
                // to the 768th megabyte (the offset plus length).
                using( var accessor = mmf.CreateViewAccessor() )
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

        private static void MemoryMappingFile_3()
        {
            using( var memFile = MemFile( filePath ) )
            using( var stream = memFile.CreateViewStream( 0L, 0L, MemoryMappedFileAccess.Read ) )
            {
                //do stuff with your stream
            }
        }
        public static MemoryMappedFile MemFile( string filePath )
        {
            return MemoryMappedFile.CreateFromFile(
                      //include a readonly shared stream
                      File.Open( filePath, FileMode.Open, FileAccess.Read, FileShare.Read ),
                      //not mapping to a name
                      null,
                      //use the file's actual size
                      0L,
                      //read only access
                      MemoryMappedFileAccess.Read,
                      //not configuring security
                      null,
                      //adjust as needed
                      HandleInheritability.None,
                      //close the previously passed in stream when done
                      false );

        }

        private static void MemoryMappingFile( string filePath, long startPosition, long endPosition )
        {
            long offset = startPosition;    //0x00000000 * startPosition; // offset 0
            long length = endPosition; //0x20000000 * endPosition; // 512 megabytes

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
