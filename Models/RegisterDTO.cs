using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class RegisterDTO
    {
        public RegisterDTO(string username, string password, string fname, string lname, string? address, string? phone, Guid? managerID)
        {
            this.Username = username;
            this.Password = password;
            this.Fname = fname;
            this.Lname = lname;
            this.Address = address;
            this.Phone = phone;
            this.ManagerID = managerID;
        }

        public string Username {get; set;}
        public string Password {get; set;}
        public string Fname {get; set;}
        public string Lname {get; set;}
        public string? Address {get; set;}
        public string? Phone {get; set;}
        public Guid? ManagerID {get; set;}
    }
}