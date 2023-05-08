using System.Net.Http;
using System.Net.Sockets;

namespace MAUI1
{
    public static class TCPCLient
    {
        static TCPCLient()
        {
        }
        public static async Task<string> SendQueryToServer(string[] entries, int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            var response = "";
            try
            {
                foreach (var entry in entries)
                {
                    await streamWriter.WriteLineAsync(entry);
                    await streamWriter.FlushAsync();
                }
                response += (await streamReader.ReadLineAsync());
                tcpClient.Close();
                return response;
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to pick the image: {ex.Message}", "OK");
                tcpClient.Close();
                return null;
            }
        }
        public static async Task<string> SendImage(byte[] fileBytes, long fileSize, int port = 8888, string uri = "192.168.0.36")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            using var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            using var binaryWriter = new BinaryWriter(stream);
            await streamWriter.WriteLineAsync("SAVEAVATAR");
            await streamWriter.FlushAsync();
            if (await streamReader.ReadLineAsync() == "1")
            {
                await streamWriter.WriteLineAsync(fileSize.ToString());
                await streamWriter.FlushAsync();
                binaryWriter.Write(fileBytes, 0, fileBytes.Length);
            }
            string response = "";
            response = await streamReader.ReadLineAsync();
            stream.Close();
            tcpClient.Close();
            return response;
        }
        public static async Task<string> ReceiveAvatar(int port = 8888, string uri = "192.168.0.36")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            using var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            BinaryReader binaryReader = new BinaryReader(stream);
            await streamWriter.WriteLineAsync("SENDAVATAR");
            await streamWriter.FlushAsync();
            var fileSize = await streamReader.ReadLineAsync();
            using (FileStream fileStream = new FileStream($"{App.projectPersonalFolderPath}\\avatar.png", FileMode.Create, FileAccess.Write))
            {
                var e = binaryReader.ReadBytes(Convert.ToInt32(fileSize));
                fileStream.Write(e, 0, e.Length);
            }
            string response = "";
            response = await streamReader.ReadLineAsync();
            stream.Close();
            tcpClient.Close();
            return response;
        }
    }
}
    







