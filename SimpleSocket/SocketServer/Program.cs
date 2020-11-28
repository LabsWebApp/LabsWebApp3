using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 6666);
            
            server.Start();
            Console.WriteLine("server.Start " + server.LocalEndpoint);
            
            

            for (;;)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("AcceptTcpClient " + client);

                var stream = client.GetStream();

                string msg = "Hello";
                Encoding encode = Encoding.ASCII;
                byte[] buffer = encode.GetBytes(msg);

                stream.Write(buffer, 0, buffer.Length);
                
                stream.Flush();
                client.Close(); 
                Console.WriteLine("client closed");

                if (Console.ReadKey().KeyChar==(char)ConsoleKey.E) break;
            }

            server.Stop();
            Console.WriteLine("server stopped");
            Console.ReadLine();
        }
    }
}
