using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace Zero_To_FileWrite
{
    class Program
    {
        static void Main( string[] args )
        {
            //string TextString = "이것은 테스트";

            // UTF8 인코딩
            Encoding encode = Encoding.UTF8;

            // 배열에 Hex 값 넣기
            //byte[] _bytes = encode.GetBytes( TextString );

            // Hex 코드 출력
            //PrintHexBytes( _bytes );

            if( args.Length != 0 )
            {
                // Async Copy File
                //Program AsyncIo = new Program();
                //AsyncIo.File_Async_IO();
                //Console.WriteLine("Async Copy File Complete..\n\n");      


                if( ByteArrayToFile( args[ 0 ] ) )
                {
                    Console.WriteLine( "ByteArrayToFile 파일 쓰기 완료!\n\n" );
                }
                else
                {
                    Console.WriteLine( "파일 쓰기 실패" );
                }

                // 문자 헥사코드를 바이트 배열로 변환해서 이진파일로 쓰기
                //string hexString = "C0CCB0CDC0BA20C5D7BDBAC6AE";
                //File.WriteAllBytes( "HexWriteDemo.hex", StringToByteArray( hexString ) );


                //Console.WriteLine("저장할 문자를 입력 하세요.\n");
                // 파일 길이 설정( Byte )
                //FileStreamSetLengh();
            }
            else
            {
                // 사용법
                Usage();
            }
        }
        public static bool ByteArrayToFile( string _FileName )
        {
            try
            {
                using( FileStream _FileStream = new FileStream( _FileName, FileMode.Open, FileAccess.Write ) )
                {
                    //_FileStream.SetLength( 0 );
                    byte[] _ByteArray = new byte[ _FileStream.Length ];

                    // 바이트 블록 단위로 파일 쓰기( 바이트 블록, 시작 위치, Write Size )
                    _FileStream.Write( _ByteArray, 0, _ByteArray.Length );
                };
            }
            catch( Exception e )
            {
                Console.WriteLine( e.Message );
                return false;
            }
            return true;
        }

        public static byte[] StringToByteArray( string hex )
        {
            return Enumerable.Range( 0, hex.Length )
                             .Where( x => x % 2 == 0 )
                             .Select( x => Convert.ToByte( hex.Substring( x, 2 ), 16 ) )
                             .ToArray();
        }
        private static void PrintHexBytes( byte[] _bytes )
        {
            if( ( _bytes == null ) || ( _bytes.Length == 0 ) )
            {
                Console.WriteLine( "<none>" );
            }
            else
            {
                for( int i = 0; i < _bytes.Length; i++ )
                {
                    Console.Write( "{0:X2} ", _bytes[ i ] );
                }
            }
            Console.WriteLine( "\n\n" );
        }

        // 비동기로 파일 쓰기
        private async static void FileStreamSetLengh()
        {
            using( FileStream _FileStream = new FileStream( "SetLengthDemo.txt", FileMode.OpenOrCreate, FileAccess.Write ) )
            {
                using( StreamWriter _StreamWriter = new StreamWriter( _FileStream ) )
                {
                    // 파일 길이 설정( Byte )
                    _FileStream.SetLength( 4096 );
                    while( true )
                    {
                        string ReadString = Console.ReadLine();
                        if( ReadString.Length == 0 )
                            break;

                        await _StreamWriter.WriteAsync( ReadString );
                    }
                };
            };
        }
        // 비동기 방식으로 파일 읽고 다른 이름으로 파일 쓰기
        private async void File_Async_IO()
        {
            string UserDirectory = @"C:\Temp\Async_Directory\";

            using( StreamReader SourceReader = File.OpenText( UserDirectory + "BigFile.txt" ) )
            {
                using( StreamWriter DestinationWriter = File.CreateText( UserDirectory + "CopiedFile.txt" ) )
                {
                    await CopyFilesAsync( SourceReader, DestinationWriter );
                }
            }
            Console.WriteLine( "Async Write Complete...\n" );
        }
        private async Task CopyFilesAsync( StreamReader Source, StreamWriter Destination )
        {
            // 0x1000 - 4096 byte
            char[] buffer = new char[ 0x1000 ];
            int numRead;
            while( ( numRead = await Source.ReadAsync( buffer, 0, buffer.Length ) ) != 0 )
            {
                await Destination.WriteAsync( buffer, 0, numRead );
            }
        }
        public static void Usage()
        {
            Console.WriteLine( "사용 방법 :\n" );
            Console.WriteLine( "Zero_Write_File 삭제 파일 이름\n" );
            Console.WriteLine( "예: Zero_Write_File Test.exe" );
        }
        private static void Etc_Function()
        {
            object[] doubles = { 1.0, 2.0, 3.0 };
            IEnumerable<double> d = doubles.Cast<double>();
            Console.Write( d );

            ArrayList fruits = new ArrayList();
            fruits.Add( "3" );
            fruits.Add( "2" );
            fruits.Add( "1" );

            ArrayList fr = new ArrayList { 1, 2, 3 };

            List<string> StringList = new List<string> { "t", "a", "z" };

            IEnumerable<string> query1 =
                fruits.Cast<string>().OrderBy( fruit => fruit ).Select( fruit => fruit );
            foreach( string fruit in query1 )
            {
                Console.Write( fruit );
            }

            //
            IEnumerable<int> query2 =
                fr.Cast<int>().OrderBy( fru => fru ).Select( fru => fru );

            // 문자 숫자 배열을 정수 숫자 배열로 변경
            string[] StringInt = new string[] { "1", "2", "3", "4" };
            int[] IntConvert1 = Array.ConvertAll( StringInt, s => int.Parse( s ) );
            int[] IntConvert2 = Array.ConvertAll( StringInt, int.Parse );
            int[] IntConvert3 = StringInt.Select( int.Parse ).ToArray();

            // ArrayList 문자열 숫자를 정수형 배열로 변환하기
            int[] ArrayListToInt = new int[ fruits.Count ];
            for( int i = 0; i < ArrayListToInt.Length; i++ )
            {
                try
                {
                    ArrayListToInt[ i ] = int.Parse( fruits[ i ].ToString() );
                }
                catch( Exception )
                {
                }
            }
            //fruits.Cast<int>(qq).CopyTo( qq );

            var qe = fruits.Cast<int>().OrderBy( ff => ff ).Select( ff => ff );
            foreach( var item in qe )
            {
                Console.Write( item );
            }

            // The following code, without the cast, doesn't compile.
            //IEnumerable<string> query1 =
            //    fruits.OrderBy( fruit => fruit ).Select( fruit => fruit );
        }
    }
}
