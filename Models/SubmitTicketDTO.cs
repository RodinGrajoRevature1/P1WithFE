using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class SubmitTicketDTO
    {
        public SubmitTicketDTO(decimal amount, string description, string type, Guid fk_employeeID)
        {
            this.Amount = amount;
            this.Description = description;
            this.Type = type;
            this.FK_EmployeeID = fk_employeeID;
        }

        public Guid? TicketID;
        public decimal Amount {get; set;}
        public string Description {get; set;}
        public string Type {get; set;}
        public Guid FK_EmployeeID {get; set;}
    }
}