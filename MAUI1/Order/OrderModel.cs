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
    public enum OrderStatus
    {
        Waiting,
        InProgress,
        CompletedWithQuestionMark,
        Completed,
        Cancelled,
        PreCancelled,
    }
    public class OrderModel
    {
        
        public int Id { get; set; }
        public DateTime StartTime { get; set; } //дата начала заказа
        public DateTime EndTime { get; set; } //дата окончания заказа
        public UserModel Client { get; set; } //идентификатор клиента
        public string StartPoint { get; set; } //начальная точка
        public string EndPoint { get; set; } //конечная точка
        public float Price { get; set; } //цена
        public OrderStatus Status { get; set; } //статус заказа
        public UserModel Driver { get; set; } //id виодителя который выполнил/яет заказ
                                           //public string Tariff; //тариф
        public OrderModel()
        {
        }
        public OrderModel(DateTime start_Time, string start, string end, float price)
        {
            StartTime = start_Time;
            StartPoint = start;
            EndPoint = end;
            Price = price;
        }
    }
}
