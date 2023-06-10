
using CommunityToolkit.Maui.Core.Extensions;
using MAUI1.User;
using MAUI1.User.Admin;
using MAUI1.User.Client;
using MAUI1.User.Dispatcher;
using MAUI1.User.Driver;
using MAUI1.User.Order;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svg;
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace MAUI1
{

    public static class TCPCLient
    {
        public static string accessTokenPath = $"{App.projectPersonalFolderPath}\\access_token.json";
        public static string TestToken;
        public const string URI = "192.168.0.101";
        public const int PORT = 8888;
        static TCPCLient()
        {
        }
        public static async Task<string> CreateUserRegistrationQueryAsync(UserModel user, int port = PORT,string uri = URI)
        {
            try
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                //proverka
                await SendMessageToServerAsync(streamWriter, "REG");
                var userJson = JsonConvert.SerializeObject(user);
                await SendMessageToServerAsync(streamWriter, userJson);
                var response = await streamReader.ReadLineAsync();
                tcpClient.Close();
                return response;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Невозможно связаться с сервером: {ex.Message}", "OK");
                return "0";
            }
        }
        public static async Task<object> PollServerData(UserModel userDetails, int port = 8888, string uri = URI)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            await SendMessageToServerAsync(streamWriter, "GETDATA");

            var token = GetAccessToken();
            await SendMessageToServerAsync(streamWriter, token);

            var response = await streamReader.ReadLineAsync();

            if(response == "1")
            {
                if (userDetails.UserType == UserType.Client)
                {
                    var orderJson = await streamReader.ReadLineAsync();
                    var driverJson = await streamReader.ReadLineAsync();
                    OrderModel order = null;
                    UserModel driver = null;
                    if (orderJson != "")
                    {
                        order = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                        if (driverJson != "")
                        {
                            driver = JsonConvert.DeserializeObject<UserModel>(driverJson);
                            tcpClient.Close();
                            return new List<object> { order, driver };
                        }
                        tcpClient.Close();
                        return new List<object> { order,driver };
                    }
                    tcpClient.Close();
                    return new List<object> { order, driver };
                }
                else if (userDetails.UserType == UserType.Driver)
                {
                    var orderJson = await streamReader.ReadLineAsync();
                    var clientJson = await streamReader.ReadLineAsync();
                    OrderModel order = null;
                    UserModel client = null;
                    if (orderJson != "")
                    {
                        order = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                        if (clientJson != "")
                        {
                            client = JsonConvert.DeserializeObject<UserModel>(clientJson);
                            tcpClient.Close();
                            return new List<object> { order, client };
                        }
                        tcpClient.Close();
                        return new List<object> { order, client };
                    }
                    tcpClient.Close();
                    return new List<object> { order, client };
                }
                else if (userDetails.UserType == UserType.Dispatcher)
                {
                    var ordersJson = await streamReader.ReadLineAsync();
                    var ordersUsersJson = await streamReader.ReadLineAsync();
                    var driversJson = await streamReader.ReadLineAsync();
                    object[] dispatcherDataArray = new object[2];
                    tcpClient.Close();
                    if (ordersJson != "")
                    {
                        OrderModel[] orders;
                        List<OrderViewModel> ovms = new List<OrderViewModel>();
                        if (ordersJson != "")
                        {
                            orders = JsonConvert.DeserializeObject<OrderModel[]>(ordersJson);
                        }
                        else
                        {
                            orders = new OrderModel[] { };
                        }
                        UserModel[] ordersUsers; 
                        if (ordersUsersJson != "")
                        {
                            ordersUsers = JsonConvert.DeserializeObject<UserModel[]>(ordersUsersJson);
                        }
                        else
                        {
                            ordersUsers = new UserModel[] { };
                        }
                        foreach (var element in orders)
                        {
                            var client = ordersUsers.FirstOrDefault(item => item.PhoneNumber == element.ClientPhoneNumber);
                            var driver = ordersUsers.FirstOrDefault(item => item.PhoneNumber == element.DriverPhoneNumber);
                            if (client != null)
                            {
                                var clientVM = new ClientViewModel(client);
                                if (driver != null)
                                {
                                    var driverVM = new DriverViewModel(driver);
                                    var ovm = new OrderViewModel(element, clientVM, driverVM);
                                    ovms.Add(ovm);
                                }
                                else
                                {
                                    var ovm = new OrderViewModel(element, clientVM);
                                    ovms.Add(ovm);
                                }
                                dispatcherDataArray[0] = ovms;
                            }
                        }
                        if (driversJson != "")
                        {
                            var drivers = JsonConvert.DeserializeObject<UserModel[]>(driversJson);
                            var dvms = new List<DriverViewModel>();
                            foreach (var driver in drivers)
                            {
                                dvms.Add(new DriverViewModel(driver));
                            }
                            dispatcherDataArray[1] = dvms;
                        }
                        return dispatcherDataArray;
                    }
                }
                else if(userDetails.UserType == UserType.Administrator)
                {
                    var usersJson = await streamReader.ReadLineAsync();
                    if (usersJson != "")
                    {
                        var users = JsonConvert.DeserializeObject<UserModel[]>(usersJson);
                        return users;
                    }
                }
            }
            tcpClient.Close();
            return null;
        }
        public static async void SendDriverAnswerToServer(bool answer, int port = 8888, string uri = URI)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            await SendMessageToServerAsync(streamWriter, "DRIVERANSWER");
            var token = GetAccessToken();
            await SendMessageToServerAsync(streamWriter, token);
            var response = await streamReader.ReadLineAsync();
            if (response == "1")
            {
                await SendMessageToServerAsync(streamWriter, answer.ToString());
            }
        }
        public static async void CreateDriverOrderRequest(string driverPhone, string clientPhone, int port = PORT, string uri = URI)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            await SendMessageToServerAsync(streamWriter, "CREATEDRIVERORDERREQUEST");
            var token = GetAccessToken();
            await SendMessageToServerAsync(streamWriter, token);
            var response = await streamReader.ReadLineAsync();
            if (response == "1")
            {
                await SendMessageToServerAsync(streamWriter, driverPhone);
                await SendMessageToServerAsync(streamWriter, clientPhone);
            }
        }
        public static async Task<bool> CreateOrderRequest(OrderModel order, int port = PORT, string uri = URI)
        {
            try
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                await SendMessageToServerAsync(streamWriter, "CREATEORDER");
                var token = GetAccessToken();
                await SendMessageToServerAsync(streamWriter, token);
                var response = await streamReader.ReadLineAsync();
                if (response == "1")
                {
                    var orderJson = JsonConvert.SerializeObject(order);
                    await SendMessageToServerAsync(streamWriter, orderJson);
                    response = await streamReader.ReadLineAsync();
                    tcpClient.Close();
                    return bool.Parse(response);
                }
                return false;
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public static async Task<(double, double)> GetUserLocation(string userPhoneNumber, int port = PORT, string uri = URI)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            await SendMessageToServerAsync(streamWriter, "GETUSERLOCATION");
            var token = GetAccessToken();
            await SendMessageToServerAsync(streamWriter, token);
            if (await streamReader.ReadLineAsync() == "1")
            {
                await SendMessageToServerAsync(streamWriter, userPhoneNumber);
                var longitude = await streamReader.ReadLineAsync();
                if (longitude != "close")
                {
                    var latitude = await streamReader.ReadLineAsync();
                    if (latitude != "close")
                    {
                        tcpClient.Close();
                        return (double.Parse(longitude), double.Parse(latitude));
                    }
                    else
                    {
                        tcpClient.Close();
                        return (0, 0);
                    }
                }
                else
                {
                    tcpClient.Close();
                    return (0, 0);
                }
            }
            if (await streamReader.ReadLineAsync() == "close")
            {
                tcpClient.Close();
            }
            return (0, 0);
        }
        public static async Task<UserVM> SendLoginQueryToServerAsync(string login, string password, int port = PORT, string uri = URI)
        {
            try
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                BinaryWriter binaryWriter = new BinaryWriter(stream);

                var credentials = new
                {
                    login,
                    password
                };
                string json = JsonConvert.SerializeObject(credentials);

                await SendMessageToServerAsync(streamWriter, "LOGIN");

                await SendMessageToServerAsync(streamWriter, json);
                if (await streamReader.ReadLineAsync() == "1")
                {
                    var token = await streamReader.ReadLineAsync();
                    SaveToken(token);
                    var userJson = await streamReader.ReadLineAsync();
                    if (userJson != null)
                    {
                        var user = JsonConvert.DeserializeObject<UserModel>(userJson);
                        if (user.UserType == UserType.Client)
                        {
                            ClientViewModel clientViewModel = new ClientViewModel(user);
                            var orderJson = await streamReader.ReadLineAsync();
                            if (orderJson != "")
                            {
                                var order = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                                var driverJson = await streamReader.ReadLineAsync();
                                if (driverJson != "")
                                {
                                    var driver = JsonConvert.DeserializeObject<UserModel>(driverJson);
                                    var driverVM = new DriverViewModel(driver);
                                    OrderViewModel orderViewModel = new OrderViewModel(order, clientViewModel, driverVM);
                                }
                                else
                                {
                                    OrderViewModel orderViewModel = new OrderViewModel(order, clientViewModel);
                                }
                            }
                            return clientViewModel;
                        }
                        else if (user.UserType == UserType.Driver)
                        {
                            DriverViewModel driverViewModel = new DriverViewModel(user);
                            var orderJson = await streamReader.ReadLineAsync();
                            if (orderJson != "")
                            {
                                var order = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                                var clientJson = await streamReader.ReadLineAsync();
                                if (clientJson != "")
                                {
                                    var client = JsonConvert.DeserializeObject<UserModel>(clientJson);
                                    ClientViewModel clientViewModel = new ClientViewModel(client);
                                    OrderViewModel orderViewModel = new OrderViewModel(order, clientViewModel, driverViewModel);
                                }
                            }
                            return driverViewModel;
                        }
                        //TODO:реализовать дисечтера
                        else if (user.UserType == UserType.Dispatcher)
                        {
                            var ordersJson = await streamReader.ReadLineAsync();
                            var ordersUsersJson = await streamReader.ReadLineAsync();
                            var driversJson = await streamReader.ReadLineAsync();
                            tcpClient.Close();
                            TaxiDispatcherModel tdispatcher = new TaxiDispatcherModel(user.FirstName, user.LastName, user.PhoneNumber, user.Email, user.Password);
                            if (ordersJson != "")
                            {
                                OrderModel[] orders;
                                List<OrderViewModel> ovms = new List<OrderViewModel>();
                                if (ordersJson != "")
                                {
                                    orders = JsonConvert.DeserializeObject<OrderModel[]>(ordersJson);
                                }
                                else
                                {
                                    orders = new OrderModel[] { };
                                }
                                UserModel[] ordersUsers;
                                if (ordersUsersJson != "")
                                {
                                    ordersUsers = JsonConvert.DeserializeObject<UserModel[]>(ordersUsersJson);
                                }
                                else
                                {
                                    ordersUsers = new UserModel[] { };
                                }
                                foreach (var element in orders)
                                {
                                    var client = ordersUsers.FirstOrDefault(item => item.PhoneNumber == element.ClientPhoneNumber);
                                    var driver = ordersUsers.FirstOrDefault(item => item.PhoneNumber == element.DriverPhoneNumber);
                                    if (client != null)
                                    {
                                        var clientVM = new ClientViewModel(client);
                                        if (driver != null)
                                        {
                                            var driverVM = new DriverViewModel(driver);
                                            var ovm = new OrderViewModel(element, clientVM, driverVM);
                                            ovms.Add(ovm);
                                        }
                                        else
                                        {
                                            var ovm = new OrderViewModel(element, clientVM);
                                            ovms.Add(ovm);
                                        }
                                    }
                                }
                                tdispatcher.OrdersCollection = ovms.ToObservableCollection();
                            }
                            if (driversJson != "")
                            {
                                var drivers = JsonConvert.DeserializeObject<UserModel[]>(driversJson);
                                var dvms = new List<DriverViewModel>();
                                foreach (var driver in drivers)
                                {
                                    dvms.Add(new DriverViewModel(driver));
                                }
                                tdispatcher.Drivers = dvms.ToObservableCollection();
                            }
                            TaxiDispatcherViewModel dispatcherVM = new TaxiDispatcherViewModel(tdispatcher);
                            return dispatcherVM;
                        }

                        else if (user.UserType == UserType.Administrator)
                        {
                            AdminVM adminVM = new AdminVM(user);
                            var usersJson = await streamReader.ReadLineAsync();
                            var users = JsonConvert.DeserializeObject<UserModel[]>(usersJson);
                            if (users != null)
                            {
                                foreach (var element in users)
                                {
                                    UserVM userVM = new UserVM(element);
                                    adminVM.Users.Add(userVM);
                                }
                                return adminVM;
                            }
                        }
                    }
                }
                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                }
                return null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", $"Невозможно связаться с сервером: {ex.Message}", "OK");
                return null;
            }
        }

        public static async Task<UserVM> GetAutorizationDataAsync(int port = PORT, string uri = URI)
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            BinaryReader binaryReader = new BinaryReader(stream);

            await SendMessageToServerAsync(streamWriter, "AUTH");
            var token = GetAccessToken();
            await SendMessageToServerAsync(streamWriter, token);
            if (await streamReader.ReadLineAsync() == "1")
            {
                var userJson = await streamReader.ReadLineAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(userJson);
                if (user.UserType == UserType.Client)
                {
                    ClientViewModel clientViewModel = new ClientViewModel(user);
                    var orderJson = await streamReader.ReadLineAsync();
                    if (orderJson != "")
                    {
                        var order = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                        var driverJson = await streamReader.ReadLineAsync();
                        if (driverJson != "")
                        {
                            var driver = JsonConvert.DeserializeObject<UserModel>(driverJson);
                            var driverVM = new DriverViewModel(driver);
                            OrderViewModel orderViewModel = new OrderViewModel(order, clientViewModel, driverVM);
                        }
                        else
                        {
                            OrderViewModel orderViewModel = new OrderViewModel(order, clientViewModel);
                        }
                    }
                    return clientViewModel;
                }
                else if (user.UserType == UserType.Driver)
                {
                    DriverViewModel driverViewModel = new DriverViewModel(user);
                    var orderJson = await streamReader.ReadLineAsync();
                    if (orderJson != "")
                    {
                        var order = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                        var clientJson = await streamReader.ReadLineAsync();
                        if (clientJson != "")
                        {
                            var client = JsonConvert.DeserializeObject<UserModel>(clientJson);
                            ClientViewModel clientViewModel = new ClientViewModel(client);
                            OrderViewModel orderViewModel = new OrderViewModel(order, clientViewModel, driverViewModel);
                        }
                    }
                    return driverViewModel;
                }
                //TODO:реализовать дисечтера
                    else if (user.UserType == UserType.Dispatcher)
                    {
                        var ordersJson = await streamReader.ReadLineAsync();
                        var ordersUsersJson = await streamReader.ReadLineAsync();
                        var driversJson = await streamReader.ReadLineAsync();
                        tcpClient.Close();
                        TaxiDispatcherModel tdispatcher = new TaxiDispatcherModel(user.FirstName, user.LastName, user.PhoneNumber, user.Email, user.Password);
                        if (ordersJson != "")
                        {
                            OrderModel[] orders;
                            List<OrderViewModel> ovms = new List<OrderViewModel>();
                            if (ordersJson != null)
                            {
                                orders = JsonConvert.DeserializeObject<OrderModel[]>(ordersJson);
                            }
                            else
                            {
                                orders = new OrderModel[] { };
                            }
                            UserModel[] ordersUsers;
                            if (ordersUsersJson != "")
                            {
                                ordersUsers = JsonConvert.DeserializeObject<UserModel[]>(ordersUsersJson);
                            }
                            else
                            {
                                ordersUsers = new UserModel[] { };
                            }
                            foreach (var element in orders)
                            {
                                var client = ordersUsers.FirstOrDefault(item => item.PhoneNumber == element.ClientPhoneNumber);
                                var driver = ordersUsers.FirstOrDefault(item => item.PhoneNumber == element.DriverPhoneNumber);
                                if (client != null)
                                {
                                    var clientVM = new ClientViewModel(client);
                                    if (driver != null)
                                    {
                                        var driverVM = new DriverViewModel(driver);
                                        var ovm = new OrderViewModel(element, clientVM, driverVM);
                                        ovms.Add(ovm);
                                    }
                                    else
                                    {
                                        var ovm = new OrderViewModel(element, clientVM);
                                        ovms.Add(ovm);
                                    }
                                }
                            }
                            tdispatcher.OrdersCollection = ovms.ToObservableCollection();
                        }
                        if (driversJson != "")
                        {
                            var drivers = JsonConvert.DeserializeObject<UserModel[]>(driversJson);
                            var dvms = new List<DriverViewModel>();
                            foreach (var driver in drivers)
                            {
                                dvms.Add(new DriverViewModel(driver));
                            }
                            tdispatcher.Drivers = dvms.ToObservableCollection();
                        }
                        TaxiDispatcherViewModel dispatcherVM = new TaxiDispatcherViewModel(tdispatcher);
                        return dispatcherVM;
                    
                }

                else if (user.UserType == UserType.Administrator)
                {
                    AdminVM adminVM = new AdminVM(user);
                    var usersJson = await streamReader.ReadLineAsync();
                    var users = JsonConvert.DeserializeObject<UserModel[]>(usersJson);
                    if (users != null)
                    {
                        foreach (var element in users)
                        {
                            UserVM userVM = new UserVM(element);
                            adminVM.Users.Add(userVM);
                        }
                        return adminVM;
                    }
                }
            }
            tcpClient.Close();
            return null;
        }
        public static async void RefreshLocationDataOnServer(double longitude, double latitude, int port = PORT, string uri = URI)
        {
            try
            {
                using TcpClient tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri, port);
                var stream = tcpClient.GetStream();
                using var streamReader = new StreamReader(stream);
                using var streamWriter = new StreamWriter(stream);
                await SendMessageToServerAsync(streamWriter, "REFRESHLOC");
                var token = GetAccessToken();
                await SendMessageToServerAsync(streamWriter, token);
                var response = await streamReader.ReadLineAsync();
                if (response == "1")
                {
                    await SendMessageToServerAsync(streamWriter, longitude.ToString());
                    await SendMessageToServerAsync(streamWriter, latitude.ToString());
                    response = await streamReader.ReadLineAsync();
                }
                if (response == null)
                {
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        public static async void SendLocationToServer(double longitude, double latitude, TcpClient tcpClient)
        {
            NetworkStream networkStream = tcpClient.GetStream();
            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            await SendMessageToServerAsync(streamWriter, "SENDLOCATION");
            var token = GetAccessToken();
            await SendMessageToServerAsync(streamWriter, token);
            if (await streamReader.ReadLineAsync() == "1")
            {
                await SendMessageToServerAsync(streamWriter, longitude.ToString());
                await SendMessageToServerAsync(streamWriter, latitude.ToString());
            }
        }



        public static async Task<(bool, string)> SendTokenToServerAsync(int port = 8888, string uri = "127.0.0.1")
        {
            using TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(uri, port);
            var stream = tcpClient.GetStream();
            using var streamReader = new StreamReader(stream);
            using var streamWriter = new StreamWriter(stream);
            await SendMessageToServerAsync(streamWriter, "CHECKTOKEN");
            if (await streamReader.ReadLineAsync() == "1")
            {
                var json = GetAccessToken();
                JObject jsonObject = JObject.Parse(json);
                string tokenValue = (string)jsonObject["accessToken"];
                await SendMessageToServerAsync(streamWriter, tokenValue);
                bool isTokenValid = bool.Parse(await streamReader.ReadLineAsync());
                if (isTokenValid)
                {
                    var userType = await streamReader.ReadLineAsync();
                    return (isTokenValid, userType);
                }
                return (isTokenValid, "Unknown");
            }
            return (false, "Unknown");
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
            if (await streamReader.ReadLineAsync() == "1")
            {
                //var fileSize = await streamReader.ReadLineAsync();
                //using(FileStream fileStream = new FileStream($"{App.projectPersonalFolderPath}\\users.json", FileMode.Create, FileAccess.Write))
                //{
                //    var buffer = binaryReader.ReadBytes(int.Parse(fileSize));
                //    fileStream.Write(buffer);
                //}
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
                    var users = JsonConvert.DeserializeObject<UserModel[]>(json);
                    tcpClient.Close();
                    return users;
                }
            }
            tcpClient.Close();
            return null;
        }

        public static string GetAccessToken()
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


        private static void SaveToken(string accessToken, int port = 8888, string uri = "127.0.0.1")
        {
            var token = new
            {
                accessToken,
            };
            string json = JsonConvert.SerializeObject(token, Formatting.Indented);
            File.WriteAllText(accessTokenPath, json);
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








