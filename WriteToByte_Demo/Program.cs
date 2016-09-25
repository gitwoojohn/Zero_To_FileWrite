using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WriteToByte_Demo
{
    class Program
    {
        private static FileStream _FileStream;

        static void Main( string[] args )
        {
            // 이것은 테스트 Hex Code
            string HexString1 = "C0CCB0CDC0BA20C5D7BDBAC6AE";
            string HexString2 = "이것은 테스트";
            byte[] byteString = new byte[ HexString2.Length ];
            HexStringToWrite( "HexStringWrite1.Txt", byteString );


            //File.WriteAllBytes( "output.dat", StringToByteArray( HexString1 ) );
            StringToByteArray( HexString1 );
        }
        private static void HexStringToWrite( string _FileName, byte[] _ByteArray )
        {
            try
            {
                _FileStream = new FileStream( _FileName, FileMode.Create, FileAccess.Write );
                _FileStream.Write( _ByteArray, 0, _ByteArray.Length );

            }
            catch( FileNotFoundException e )
            {
                Console.WriteLine( e.Message );
            }
            catch( UnauthorizedAccessException e )
            {
                Console.WriteLine( e.Message );
            }
            finally
            {
                _FileStream.Close();
            }
        }
        public static void StringToByteArray( string hex )
        {
            // 계산해서 바로 리턴 
            // public static byte[] StringToByteArray( string hex )
            //return file = Enumerable.Range( 0, hex.Length )
            //  .Where( x => x % 2 == 0 )
            //  .Select( x => Convert.ToByte( hex.Substring( x, 2 ), 16 ) )
            //  .ToArray();


            //  C0 CC B0 CD C0 BA 20 C5 D7 BD BA C6 AE  - 26개
            //  0  1  2  4  6  8  10 12 14 16 18 20 22  - 13개
            // Enumerable.Range : 지정된 순차 정수 생성(hex 26개) 0, 1, 2, ...... 24, 25, 26
            // Where  :           0 부터 나머지를 구하면 13개 0, 2, 4, .... 22, 24, 26
            // Select :           분리된 13개의 2자리 Hex값들을 정수로 변환  
            // (0, 2), (2, 2), (4, 2), (6, 2) ....
            // 구한 값들을 배열로 변환
            var file = Enumerable.Range( 0, hex.Length )
                .Where( x => x % 2 == 0 )
                .Select( x => Convert.ToByte( hex.Substring( x, 2 ), 16 ) )
                .ToArray();

            // 파일 쓰기
            File.WriteAllBytes( "output.dat", file );
        }
        // List<byte> 구현
        //public static List<byte> StringToListArray( string hex )
        //{
        //    // 계산해서 바로 리턴 
        //    return Enumerable.Range( 0, hex.Length )
        //      .Where( x => x % 2 == 0 )
        //      .Select( x => Convert.ToByte( hex.Substring( x, 2 ), 16 ) )
        //      .ToList();
        //}  
    }
}
