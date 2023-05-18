
using MAUI1.User;
using MAUI1.User.Admin;
using MAUI1.User.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svg;
using System;
using System.Net.Http;
using System.Net.Sockets;

namespace MAUI1
{

    public static class TCPCLient
    {
        public static string accessTokenPath = $"{App.projectPersonalFolderPath}\\access_token.json";
        public static string TestToken;
        static TCPCLient()
        {
        }
        public static async Task<string> SendRegistrationQueryToServerAsync(UserModel user, int port = 8888, string uri = "127.0.0.1")
        {
            try
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                var response = "";
                string json = JsonConvert.SerializeObject(user, Formatting.Indented);
                File.WriteAllText($"{App.projectPersonalFolderPath}\\reg.json", json);
                await streamWriter.WriteLineAsync("REG");
                await streamWriter.FlushAsync();
                if (await streamReader.ReadLineAsync() == "1")
                {
                    using (FileStream fileStream = File.OpenRead($"{App.projectPersonalFolderPath}\\reg.json"))
                    {
                        var data = await File.ReadAllBytesAsync($"{App.projectPersonalFolderPath}\\reg.json");
                        await streamWriter.WriteLineAsync(fileStream.Length.ToString());
                        await streamWriter.FlushAsync();
                        binaryWriter.Write(data);
                    }
                }
                response += (await streamReader.ReadLineAsync());
                tcpClient.Close();
                return response;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Невозможно связаться с сервером: {ex.Message}", "OK");
                return "0";
            }
        }
        public static async Task<string> SendLoginQueryToServerAsync(string login, string password, int port = 8888, string uri = "127.0.0.1")
        {
            try
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                var response = "";

                var credentials = new
                {
                    login,
                    password
                };
                string json = JsonConvert.SerializeObject(credentials, Formatting.Indented);
                File.WriteAllText($"{App.projectPersonalFolderPath}\\login.json", json);

                await streamWriter.WriteLineAsync("LOGIN");
                await streamWriter.FlushAsync();

                if (await streamReader.ReadLineAsync() == "1")
                {
                    using (FileStream fileStream = File.OpenRead($"{App.projectPersonalFolderPath}\\login.json"))
                    {
                        var data = await File.ReadAllBytesAsync($"{App.projectPersonalFolderPath}\\login.json");
                        await streamWriter.WriteLineAsync(fileStream.Length.ToString());
                        await streamWriter.FlushAsync();
                        binaryWriter.Write(data);

                    }
                }
                if (await streamReader.ReadLineAsync() == "1")
                {
                    SaveToken(await streamReader.ReadLineAsync());
                    response += (await streamReader.ReadLineAsync());
                }
                else
                {
                    response = "Failed";
                }
                tcpClient.Close();
                return response;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Невозможно связаться с сервером: {ex.Message}", "OK");
                return "0";
            }
        }
        public static async Task<bool> SendTokenToServer(int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            await SendMessageToServerAsync(streamWriter, "test");
            if (await streamReader.ReadLineAsync() == "1")
            {
                var json = File.ReadAllText($"{App.projectPersonalFolderPath}\\access_token.json");
                JObject jsonObject = JObject.Parse(json);
                string tokenValue = (string)jsonObject["accessToken"];
                await SendMessageToServerAsync(streamWriter, tokenValue);
                var d = await streamReader.ReadLineAsync();
                return bool.Parse(d);
            }
            return false;
        }
        public static async Task<string> SendQueryToServer(string destination, int port = 8888, string uri = "127.0.0.1")
        {
            if (destination == "test")
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                await streamWriter.WriteLineAsync(destination);
                await streamWriter.FlushAsync();
                await streamWriter.WriteLineAsync(TestToken);
                await streamWriter.FlushAsync();
                var d = await streamReader.ReadLineAsync();
                return "";
            }
            else if (destination == "test1")
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);
                await streamWriter.WriteLineAsync(destination);
                await streamWriter.FlushAsync();
                TestToken = await streamReader.ReadLineAsync();
                return "";
            }
            return "";
        }
        public static async Task<UserModel[]> GetUsersFromServerAsync(int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            BinaryReader binaryReader = new BinaryReader(stream);

            await SendMessageToServerAsync(streamWriter, "GETUSERS");
            if(await streamReader.ReadLineAsync() == "1")
            {
                //var fileSize = await streamReader.ReadLineAsync();
                //using(FileStream fileStream = new FileStream($"{App.projectPersonalFolderPath}\\users.json", FileMode.Create, FileAccess.Write))
                //{
                //    var buffer = binaryReader.ReadBytes(int.Parse(fileSize));
                //    fileStream.Write(buffer);
                //}
                var tokenValue = GetAccessToken();
                if(tokenValue == null)
                {
                    tcpClient.Close();
                    return null;
                }
                await SendMessageToServerAsync(streamWriter, tokenValue);
                var resp = await streamReader.ReadLineAsync();
                if (resp == bool.TrueString)
                {
                    string json = streamReader.ReadLine();
                    var users = JsonConvert.DeserializeObject<UserModel[]>(json);
                    tcpClient.Close();
                    return users;
                }
            }
            tcpClient.Close();
            return null;
        }

        private static string GetAccessToken()
        {
            if (File.Exists(accessTokenPath))
            {
                var jsonToken = File.ReadAllText(accessTokenPath);
                JObject jsonObject = JObject.Parse(jsonToken);
                string tokenValue = (string)jsonObject["accessToken"];
                return tokenValue;
            }
            return null;
        }

        public static async Task<UserVM> GetAutorizationDataAsync(int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            BinaryReader binaryReader = new BinaryReader(stream);

            await SendMessageToServerAsync(streamWriter, "AUTH");
            if (await streamReader.ReadLineAsync() == "1")
            {
                var tokenValue = GetAccessToken();
                if (tokenValue == null)
                {
                    tcpClient.Close();
                    return null;
                }
                await SendMessageToServerAsync(streamWriter, tokenValue);
                var resp = await streamReader.ReadLineAsync();
                if (resp == bool.TrueString)
                {
                    string json = streamReader.ReadLine();
                    var user = JsonConvert.DeserializeObject<UserModel>(json);
                    UserVM userVM = new UserVM(user);
                    return userVM;
                }
            }
            tcpClient.Close();
            return null;

        }
        private static string SaveToken(string accessToken, int port = 8888, string uri = "127.0.0.1")
        {
            var token = new
            {
                accessToken,
            };
            string json = JsonConvert.SerializeObject(token, Formatting.Indented);
            File.WriteAllText($"{App.projectPersonalFolderPath}\\access_token.json", json);

            return "";
        }

        private static async Task<string> GetAccessTokenFromServer(int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            BinaryWriter binaryWriter = new BinaryWriter(stream);
            await streamWriter.WriteLineAsync("test1");
            await streamWriter.FlushAsync();

            var accessToken = await streamReader.ReadLineAsync();
            var token = new
            {
                accessToken,
            };
            string json = JsonConvert.SerializeObject(token, Formatting.Indented);
            File.WriteAllText($"{App.projectPersonalFolderPath}\\access_token.json", json);

            return "";
        }
        private static async Task SendMessageToServerAsync(StreamWriter streamWriter, string message)
        {
            await streamWriter.WriteLineAsync(message);
            await streamWriter.FlushAsync();
        }
        //public static async Task<string> SendQueryToServer(string destination, int port = 8888, string uri = "127.0.0.1")
        //{
        //    using TcpClient tcpClient = new TcpClient();
        //    await tcpClient.ConnectAsync(uri, port);
        //    var stream = tcpClient.GetStream();
        //    using var streamReader = new StreamReader(stream);
        //    using var streamWriter = new StreamWriter(stream);
        //    var response = "";
        //    try
        //    {
        //        foreach (var entry in entries)
        //        {
        //            await streamWriter.WriteLineAsync(entry);
        //            await streamWriter.FlushAsync();
        //        }
        //        response += (await streamReader.ReadLineAsync());
        //        tcpClient.Close();
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to pick the image: {ex.Message}", "OK");
        //        tcpClient.Close();
        //        return null;
        //    }
        //}
        public static void RegistrationAction()
        {

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








