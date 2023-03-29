﻿using System.Net.Sockets;

namespace MAUI1
{

    class TCPCLient
    {
        public TCPCLient()
        {
        }
        public async Task<string> SendQueryToServer(string[] entries, int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            string response = "";
            foreach (var entry in entries)
            {
                await streamWriter.WriteLineAsync(entry);
                await streamWriter.FlushAsync();
                response += await streamReader.ReadLineAsync();
            }
            if (response.All(item => item == '1'))
            {
                return "1";
            }
            else
                return "0";

        }
    }
}
    


