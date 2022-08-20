using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class Ticket : ITicket
    {
        public Guid? TicketID { get; set; } = Guid.NewGuid();
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        private string? _status = "Pending";
        public string? Type { get; set; }
        public long? Receipt { get; set; }
        public Guid? FK_EmployeeID { get; set; }
        public Guid? FK_ProcessingManagerID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateProcessed { get; set; }
        
        public Ticket() {}
        public Ticket(
            Guid? ticketID,
            decimal? amount, 
            string? description, 
            string? type, 
            string? status,
            long? receipt,
            Guid? employeeID, 
            Guid? processingManagerID,
            DateTime? dateCreated, 
            DateTime? dateModified, 
            DateTime? dateProcessed
        )
        {
            this.TicketID = ticketID;
            this.Amount = amount;
            this.Description = description;
            this.Type = type;
            this._status = status;
            this.Receipt = receipt;
            this.FK_EmployeeID = employeeID;
            this.FK_ProcessingManagerID = processingManagerID;
            if (dateCreated!=null) this.DateCreated = dateCreated;
            if (dateModified!=null) this.DateModified = dateModified;
            if (dateProcessed!=null) this.DateProcessed = dateProcessed; 
        }

        public Ticket(SubmitTicketDTO s)
        {
            this.TicketID = s.TicketID;
            this.Amount = s.Amount;
            this.Description = s.Description;
            this.Type = s.Type;
            this.FK_EmployeeID = s.FK_EmployeeID;
        }

        public string? Status
        {
            get 
            {
                return this._status;
            }
        }
        public void Approve()
        {
            this._status = "Approved";
        }

        public void Deny()
        {
            this._status = "Denied";
        }
    }
}