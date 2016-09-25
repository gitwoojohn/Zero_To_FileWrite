using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSubString
{
    class Program
    {
        static void Main( string[] args )
        {
            // Substring( int32 )
            Ex_01();


            // Substring( int32, int32 )
            Ex_02();
        }

        private static void Ex_01()
        {
            string[] info = { "Name: Felica Walker", "Title: Mz.", "Age: 47", "Location: Paris", "Gender: F" };
            int found = 0;

            Console.WriteLine( "The initial values in the array are:" );
            foreach( string s in info )
                Console.WriteLine( s );

            Console.WriteLine( "{0}We want to retrieve only the key information. That is:", Environment.NewLine );

            foreach( string s in info )
            {
                found = s.IndexOf( ":" );
                Console.WriteLine( s.Substring( found + 1 ) );
            }
        }

        private static void Ex_02()
        {
            string SourceString = "abc";
            // test1 true
            bool test1 = SourceString.Substring( 2, 1 ).Equals( "c" );
            Console.WriteLine( $"Test1 is {test1}" );

            // test2 true
            bool test2 = string.IsNullOrEmpty( SourceString.Substring( 3, 0 ) );
            Console.WriteLine( $"Test2 is {test2}" );

            try
            {
                string str3 = SourceString.Substring( 3, 1 );
            }
            // 인스턴스 내에 없는 위치이거나 길이가 0 보다 작을때
            catch( ArgumentOutOfRangeException e )
            {
                Console.WriteLine( "\n예외 발생 : " );
                Console.WriteLine( e.Message );
            }
        }
    }
}
