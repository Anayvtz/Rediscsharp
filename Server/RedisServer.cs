using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace rediscsharp
{
    class RedisLiteServer
    {
        private const int Port = 6379;  // Redis default port
        private static readonly string Host = "127.0.0.1"; // Localhost

        static void Main(string[] args)
        {
            // Start the Redis Lite server
            Console.WriteLine("Starting Redis Lite server...");
            StartServer();
        }

        private static void StartServer()
        {
            var listener = new TcpListener(IPAddress.Parse(Host), Port);
            listener.Start();
            Console.WriteLine($"Listening on {Host}:{Port}...");

            while (true)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    // Create a new thread for each incoming client
                    var clientThread = new Thread(new ConnClient(client).HandleClient);
                    clientThread.Start(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }

        
    }
}
