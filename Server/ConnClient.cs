using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace rediscsharp
{
    class ConnClient
    {
        private TcpClient _tcpClient;
        private StreamReader _reader;
        private StreamWriter _writer;

        // Constructor to initialize with TcpClient
        public ConnClient(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            var stream = _tcpClient.GetStream();
            _reader = new StreamReader(stream, Encoding.ASCII);
            _writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
        }

        // Method to handle the client communication
        public void HandleClient()
        {
            Console.WriteLine("Client connected.");

            try
            {
                while (true)
                {
                    // Read the command from the client
                    var command = _reader.ReadLine();
                    if (string.IsNullOrEmpty(command))
                        break;

                    Console.WriteLine($"Received command: {command}");

                    // Handle the PING command
                    if (command.Equals("PING", StringComparison.OrdinalIgnoreCase))
                    {
                        _writer.WriteLine("+PONG");
                    }
                    else
                    {
                        // Unknown command handling (optional)
                        _writer.WriteLine("-ERR unknown command");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                _tcpClient.Close();
                Console.WriteLine("Client disconnected.");
            }
        }
    }
}
