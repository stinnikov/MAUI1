﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1.User
{
    public enum UserType
    {
        Client,
        Driver,
        Dispatcher,
        Administrator,
    }
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public int Age
        {
            get
            {
                var age = DateTime.Today.Year - DateOfBirth.Year;
                if (DateOfBirth > DateTime.Today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }
        public UserType UserType { get; set; }
        [Column(TypeName = "date")]
        public DateTime Created { get; set; }
        public UserModel(string firstName, string lastName, string phoneNumber, string email, string password, UserType userType)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            UserType = userType;
        }
    }
}

