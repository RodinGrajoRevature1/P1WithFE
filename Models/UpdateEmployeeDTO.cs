using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class UpdateEmployeeDTO
    {
        public UpdateEmployeeDTO(){}

        public UpdateEmployeeDTO(string? username, string? password, string? fname, string? lname, string? address, string? phone)
        {
            Username = username;
            Password = password;
            Fname = fname;
            Lname = lname;
            Address = address;
            Phone = phone;
        }

        public string? Username {get; set;}
        public string? Password {get; set;}
        public string? Fname {get; set;}
        public string? Lname {get; set;}
        public string? Address {get; set;}
        public string? Phone {get; set;}
    }
}