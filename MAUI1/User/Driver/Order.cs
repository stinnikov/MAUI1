using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User.Driver
{
    internal class Order : INotifyPropertyChanged
    {
        private DateTime _startTime;
        public int Id;
        public DateTime startTime { get; set; } //дата начала заказа
        public DateTime endTime { get; set; } //дата окончания заказа
        public int Client_Id { get; set; } //идентификатор клиента
        public string StartPoint { get; set; } //начальная точка
        public string EndPoint { get; set; } //конечная точка
        public float Price { get; set; } //цена
        public string Status { get; set; } //статус заказа
        public int Driver_Id { get; set; } //id виодителя который выполнил/яет заказ
                              //public string Tariff; //тариф
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public Order()
        {
        }
        public Order(DateTime start_Time, int client_Id, string start, string end, float price, DriverModel driver)
        {
            startTime = start_Time;
            Client_Id = client_Id;
            StartPoint = start;
            EndPoint = end;
            Price = price;
            Driver_Id = driver.Id;
        }
    }
}
