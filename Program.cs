using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace rediscsharp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("hello world");
            (string? str,string remains)=Dsrlz.dsrlz_str("$3\r\nlen\r\n");
            Console.WriteLine($"str is {str} and remain is {remains}");
            (List<string>? arr, string remainl) = Dsrlz.dsrlz_arr("*2\r\n:23\r\n$3\r\nyou\r\n");
            Console.WriteLine($"remainl is {remainl}");
            for (int i = 0; arr != null && i < arr?.Count; ++i)
            {
                Console.WriteLine($"list item {i} is {arr[i]}");
            }
        }
    }
}
