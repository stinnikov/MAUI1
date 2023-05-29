using MAUI1.User.Client;
using MAUI1.User.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Order
{
    public enum OrderStatusType
    {
        Waiting,
        InProgress,
        CompletedWithQuestionMark,
        Completed,
        Cancelled,
        Unknown,
    }
    public class OrderModel
    {

        public int Id { get; set; }
        public DateTime StartingTime { get; set; } //дата начала заказа
        public DateTime EndingTime { get; set; } //дата окончания заказа
        public int Dispatcher_Id { get; set; }
        public string DispatcherPhoneNumber { get; set; }
        public int Client_Id { get; set; } //идентификатор клиента
        public string ClientPhoneNumber { get; set; }
        public string StartingPoint { get; set; } //начальная точка
        public string EndingPoint { get; set; } //конечная точка
        public float Price { get; set; } //цена
        public OrderStatusType Status { get; set; } //статус заказа
        public int? Driver_Id { get; set; } //id виодителя который выполнил/яет заказ
        public string? DriverPhoneNumber { get; set; }
        //public string Tariff; //тариф
        public OrderModel(string startingPoint, string endingPoint, UserModel client)
        {
            this.StartingPoint = startingPoint;
            this.EndingPoint = endingPoint;
            Client_Id = client.Id;
            ClientPhoneNumber = client.PhoneNumber;
        }
        public OrderModel(string startingPoint, string endingPoint, float price, UserModel client)
        {
            this.StartingPoint = startingPoint;
            this.EndingPoint = endingPoint;
            Client_Id = client.Id;
            ClientPhoneNumber = client.PhoneNumber;
            Price = price;
        }
        public OrderModel()
        {

        }
    }
}
