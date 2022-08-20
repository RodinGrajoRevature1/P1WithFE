using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class Manager : Employee
    {
        public Manager(Employee? employee, Guid? newManager) : base(
            employee?.EmployeeID,
            employee?.Username, 
            employee?.Password, 
            employee?.Fname, 
            employee?.Lname, 
            employee?.Role,
            employee?.Address, 
            employee?.Phone, 
            employee?.Photo,
            newManager, 
            employee?.DateCreated, 
            employee?.DateModified
        )
        {
            this.ChangeRole("Manager");
        }
        
        public Manager PromoteEmployee(Employee? employee, Manager? newManager)
        {
            Manager manager = new Manager(employee, newManager?.EmployeeID);
            manager.ChangeRole("Manager");
            return manager;
        }

        public Employee? DemoteManager(Manager? manager, Manager? newManager)
        {

            if(manager?.Role == "Manager")
            {
                Employee employee = new Employee(
                    manager.EmployeeID,
                    manager.Username, 
                    manager.Password, 
                    manager.Fname, 
                    manager.Lname,
                    manager.Role,
                    manager.Address, 
                    manager.Phone, 
                    manager.Photo, 
                    newManager?.EmployeeID, 
                    manager.DateCreated, 
                    manager.DateModified
                );

                employee.ChangeRole("Employee");
                return employee;
            } 
            else
            {
                return null;
            }
        }
    }
}