using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class ProcessTicketDTO
    {
        public Guid? TicketID {get; set;}
        public Guid? ProcessingManagerID {get; set;}
        public string? NewStatus {get; set;}

        public ProcessTicketDTO() {}

        public ProcessTicketDTO(
            Guid? ticketID,
            Guid? processingManagerID, 
            string? newStatus
        )
        {
            this.TicketID = ticketID;
            this.ProcessingManagerID = processingManagerID;
            this.NewStatus = newStatus;
        }
    }
}