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
        private MemDb _memDb;

        // Constructor to initialize with TcpClient
        public ConnClient(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            var stream = _tcpClient.GetStream();
            _reader = new StreamReader(stream, Encoding.ASCII);
            _writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
            _memDb = new MemDb();
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

                    (string? cmds,string remains)=Dsrlz.dsrlz_str(command);
                    if (string.IsNullOrEmpty(cmds) == false)
                    {
                        if (cmds.Equals("PING", StringComparison.OrdinalIgnoreCase))
                        {
                            string pong=Srlz.srlz_simple_str("PONG");
                            _writer.WriteLine(pong);
                        }
                        else
                        {
                            // Unknown command handling (optional)
                            _writer.WriteLine($"-ERR unknown command {cmds}");
                        }
                    }
                    else
                    {
                        (List<string>? cmda,string remaina)=Dsrlz.dsrlz_arr(command);
                        if (cmda != null)
                        {
                            if (cmda[0].Equals("ECHO", StringComparison.OrdinalIgnoreCase))
                            {
                                for (int i = 1; i < cmda.Count; ++i)
                                {
                                    string echo = Srlz.srlz_simple_str(cmda[i]);
                                    _writer.WriteLine(echo);
                                }

                            }
                            else if (cmda[0].Equals("SET", StringComparison.OrdinalIgnoreCase))
                            {
                                string key = cmda[1];
                                string value = cmda[2];
                                _memDb.Set(key, value);
                                string ok = Srlz.srlz_simple_str("OK");
                                _writer.WriteLine(ok);
                            }
                            else if (cmda[0].Equals("GET", StringComparison.OrdinalIgnoreCase))
                            {
                                string key = cmdsa[1];

                                string value = _memDb.Get(key);
                                string okval = Srlz.srlz_simple_str(value);
                                _writer.WriteLine(okval);
                            }
                            else
                            {
                                // Unknown command handling (optional)
                                _writer.WriteLine($"-ERR unknown command {cmda[0]}");
                            }
                        }
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
