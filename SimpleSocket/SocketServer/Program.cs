using System;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 4010);
            
            server.Start();
            Console.WriteLine("server.Start " + server.LocalEndpoint);
            
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("AcceptTcpClient " + client);

            var stream = client.GetStream();
            byte[] buffer = {55, 56};
            stream.Write(buffer , 0, buffer.Length);

            client.Close();
            Console.WriteLine("client closed");
            server.Stop();
            Console.WriteLine("server stopped");
            
            Console.ReadLine();
        }
    }
}
