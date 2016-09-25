using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WriteAllLines_Demo
{
    /*    
        WriteAllLines 예제 ( 추가 : 현재 문화권을 알아내고 프로그램 실행 시간동안 변경 )

        File.WriteAllLines 메서드 (String, IEnumerable<String>)        

        public static void WriteAllLines(
                string path,
                IEnumerable<string> contents
        )
    */

    class Program
    {
        static string dataPath = @"C:\Temp\TimeStamps.txt";
        static string stringWordPath = @"C:\Temp\strWord.txt";
        static void Main( string[] args )
        {
            //CurrentCultureInfo();
            CreateSampleFile();

            // 7월의 토요일과 일요일만 추출
            var JulyWeekends = from line in File.ReadLines( dataPath )
                               where ( line.StartsWith( "Saturday" ) ||
                               line.StartsWith( "Sunday" ) ) &
                               line.Contains( "July" )
                               select line;

            // 추출 데이터 쓰기( 기존 파일 덮어 쓰기 )
            File.WriteAllLines( @"C:\temp\selectedDays.txt", JulyWeekends );

            // 3월의 월요일 추가 추출
            var MarchMondays = from line in File.ReadLines( dataPath )
                               where line.StartsWith( "Monday" ) &&
                               line.Contains( "March" )
                               select line;

            // March의 Monday만을 선택해서 파일 추가모드로 쓰기
            File.AppendAllLines( @"C:\temp\selectedDays.txt", MarchMondays );

            // -----------------------------------------------------------------------
            // 배열 전체 파일쓰기
            string[] stringWord = new[] { "Hello", "Hi", "guys"};
            File.WriteAllLines( stringWordPath, stringWord );

            //-----------------------------------------------------------------------
            // 이진 파일 읽고, 쓰기
            BinaryWriter_Demo();

            //BitConverter_Demo();

            //BinaryWriter_Demo_1();

            //WriteCharacters();

            //UnmanagedMemoryStream();
            // Press any key exit ....
            Console.ReadLine();
        }
        // unsafe 코드
        unsafe private static void UnmanagedMemoryStream()
        {
            // Create some data to read and write.
            // 읽고 쓰기할 유니코드 배열 데이터 생성
            byte[] message = UnicodeEncoding.Unicode.GetBytes( "Here is some data." );

            // Allocate a block of unmanaged memory and return an IntPtr object.	
            // 배열 데이터 메모리 할당
            IntPtr memIntPtr = Marshal.AllocHGlobal( message.Length );

            // Get a byte pointer from the IntPtr object.
            // IntPtr 개체로 부터 byte 포인터 구하기
            byte* memBytePtr = ( byte* )memIntPtr.ToPointer();

            // Create an UnmanagedMemoryStream object using a pointer to unmanaged memory.
            // 비관리 메모리 개체 포인터를 사용해서  비관리 메모리 스트림 생성
            UnmanagedMemoryStream writeStream = 
                new UnmanagedMemoryStream( memBytePtr, message.Length, message.Length, FileAccess.Write );

            // Write the data.
            // 데이터 쓰기
            writeStream.Write( message, 0, message.Length );

            // Close the stream.
            // 스트림 닫기
            writeStream.Close();

            // Create another UnmanagedMemoryStream object using a pointer to unmanaged memory.
            // 비관리 메모리 개체 포인터를 사용해서 읽기위한 다른 비관리 메모리 스트림 생성
            UnmanagedMemoryStream readStream = 
                new UnmanagedMemoryStream( memBytePtr, message.Length, message.Length, FileAccess.Read );

            // Create a byte array to hold data from unmanaged memory.
            // 비관리 메모리로 부터 데이터를 구하기 위해서 바이트 배열 생성
            byte[] outMessage = new byte[ message.Length ];

            // Read from unmanaged memory to the byte array.
            // 비관리 메모리부터 바이트 배열에 읽기
            readStream.Read( outMessage, 0, message.Length );

            // Close the stream.
            // 스트림 닫기
            readStream.Close();

            // Display the data to the console.
            // 콘솔에 읽어온 데이터 쓰기
            Console.WriteLine( UnicodeEncoding.Unicode.GetString( outMessage ) );

            // Free the block of unmanaged memory.
            // 비관리 메모리 해제하기
            Marshal.FreeHGlobal( memIntPtr );
        }
        static async void WriteCharacters()
        {
            using( StreamWriter writer = File.CreateText( @"C:\Temp\NewFile.txt" ) )
            {
                await writer.WriteLineAsync( "First line of example" );
                await writer.WriteLineAsync( "and second line" );
            }
        }
        // 이진 파일 쓰고, 읽기
        private static void BinaryWriter_Demo()
        {
            const string file_name = @"C:\Temp\Test_Binrary.dat";

            // 이진 파일 쓰기
            using( FileStream _FileStream = new FileStream( file_name, FileMode.Create, FileAccess.Write ) )
            {
                using( BinaryWriter _BinaryWriter = new BinaryWriter( _FileStream ) )
                {
                    for( int i = 1; i < 11; i++ )
                    {
                        _BinaryWriter.Write( i );
                    }
                }
            }
            // 이진 파일 읽기
            using( FileStream _FileStream = new FileStream( file_name, FileMode.Open, FileAccess.Read ) )
            {
                using( BinaryReader _BinaryReader = new BinaryReader( _FileStream ) )
                {
                    // Int32 - 4Byte ( _FileStream.Length / 4 )
                    for( int i = 0; i < ( _FileStream.Length / 4 ); i++ )
                    {
                        // 4바이트 정수를 읽고 4칸 앞으로 이동
                        Console.Write( "\t" + _BinaryReader.ReadInt32() );
                    }
                }
            }
        }
        private static void BinaryWriter_Demo_1()
        {
            const string fileName = @"C:\Temp\AppSettings.dat";

            // 설정값 쓰고, 읽기
            using( BinaryWriter writer = new BinaryWriter( File.Open( fileName, FileMode.Create ) ) )
            {
                writer.Write( 1.250F );
                writer.Write( @"C:\Temp" );
                writer.Write( 10 );
                writer.Write( true );
            }

            float aspectRatio;
            string tempDirectory;
            int autoSaveTime;
            bool showStatusBar;

            using( BinaryReader reader = new BinaryReader( File.Open( fileName, FileMode.Open ) ) )
            {
                aspectRatio = reader.ReadSingle();
                tempDirectory = reader.ReadString();
                autoSaveTime = reader.ReadInt32();
                showStatusBar = reader.ReadBoolean();
            }

            Console.WriteLine( "Aspect ratio set to: " + aspectRatio );
            Console.WriteLine( "Temp directory is: " + tempDirectory );
            Console.WriteLine( "Auto save time set to: " + autoSaveTime );
            Console.WriteLine( "Show status bar: " + showStatusBar );

        }
        // 비트 컨버터
        private static void BitConverter_Demo()
        {
            // Define an array of integers.
            int[] values = { 0, 15, -15, 0x100000,  -0x100000, 1000000000,
                         -1000000000, int.MinValue, int.MaxValue };

            // Convert each integer to a byte array.
            Console.WriteLine("\n\n");
            Console.WriteLine( "{0,16}{1,10}{2,17}", "Integer", "Endian", "Byte Array" );
            Console.WriteLine( "{0,16}{1,10}{2,17}", "---", "------", "-----------" );
            foreach( var value in values )
            {
                byte[] byteArray = BitConverter.GetBytes( value );
                Console.WriteLine( "{0,16}{1,10}{2,17}", value,
                                  BitConverter.IsLittleEndian ? "Little" : " Big",
                                  BitConverter.ToString( byteArray ) );
            }
        }

        // 샘플용 파일 생성
        static void CreateSampleFile()
        {
            DateTime TimeStamp = new DateTime( 1700, 1, 1 );
            using( StreamReader sr = new StreamReader( stringWordPath ) )
            {
                string line;
                //while( ( line = sr.ReadLine()) != null )
                while( (line = sr.ReadLine()) != null)
                {
                    Console.WriteLine( line );
                }
            }

            using( StreamWriter sw = new StreamWriter( dataPath ) )
            {
                for( int i = 0; i < 500; i++ )
                {
                    DateTime TS1 = TimeStamp.AddYears( i );
                    DateTime TS2 = TS1.AddMonths( i );
                    DateTime TS3 = TS2.AddDays( i );

                    // 문화권을 영미권으로 변경 ( 변경 하지 않으면 대한민국 ko로 자동설정 - 출력 : 월요일 7월 )
                    Thread.CurrentThread.CurrentCulture = new CultureInfo( "en" );
                    sw.WriteLine( TS3.ToLongDateString() );
                }
            }
        }
        // 현재 지역을 알아내고 다른 지역을 설정 하는 예제
        static void CurrentCultureInfo()
        {
            // 각 문화권의 통용 화폐 변수
            double value = 1634.92;

            // 이 코드는 지원 안 됨.
            //CultureInfo.CurrentCulture = new CultureInfo( "fr-CA" );

            // 현재 문화권을 알아내고 그 문화권에 맞는 통화를 출력
            Console.WriteLine( "Current Culture: {0}", CultureInfo.CurrentCulture.Name );
            Console.WriteLine( "{0:C2}\n", value );

            // 원하는 문화권으로 변경 ( 영어 문화권으로 변경 )
            Thread.CurrentThread.CurrentCulture = new CultureInfo( "en" );
            // 변경한 문화권 출력
            Console.WriteLine( "Current Culture: {0}", CultureInfo.CurrentCulture.Name );
            // 변경한 문화권 통화로 변수 출력
            Console.WriteLine( "{0:C2}", value );

        }
    }
}
/*

**** 쓰기 ****
WriteAllLines, StreamWriter, BinaryWriter, StringWriter(Async), TextWriter


**** 읽기 ****
WriteAllReadLine, StreamReader, BinaryReader, StringReader(Async), TextReader

*/