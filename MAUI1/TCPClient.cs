﻿using System.Net.Sockets;

namespace MAUI1
{

    static class TCPCLient
    {
        static TCPCLient()
        {
        }
        public static async Task<List<string>> SendQueryToServer(string[] entries, int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            List<string> response = new List<string>();
            foreach (var entry in entries)
            {
                await streamWriter.WriteLineAsync(entry);
                await streamWriter.FlushAsync();
                
            }
            response.Add(await streamReader.ReadLineAsync());
            return response;
        }
    }
}
    



