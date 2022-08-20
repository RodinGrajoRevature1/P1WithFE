using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class Employee : IEmployee
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Fname { get; set; }
        public string? Lname { get; set; }
        private string? _role { get; set; } = "Employee";
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public long? Photo { get; set; }
        public Guid? EmployeeID { get; set; } = Guid.NewGuid();
        public Guid? ManagerID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public Employee() {}

        public Employee(
            Guid? employeeID,
            string? username, 
            string? password, 
            string? fname, 
            string? lname, 
            string? role,
            string? address, 
            string? phone, 
            long? photo, 
            Guid? managerID, 
            DateTime? dateCreated, 
            DateTime? dateModified
        )
        {
            this.EmployeeID = employeeID;
            this.Username = username;
            this.Password = password;
            this.Fname = fname;
            this.Lname = lname;
            this._role = role;
            this.Address = address;
            this.Phone = phone;
            if (photo!=null) this.Photo = photo;
            if (managerID!=null) this.ManagerID = managerID;
            if (dateCreated!=null) this.DateCreated = dateCreated;
            if (dateModified!=null) this.DateModified = dateModified;
        }

        public string? Role 
        {
            get 
            {
                return this._role;
            }
        }

        public void ChangeRole(string newRole)
        {
            this._role = newRole;
        }
    }
}