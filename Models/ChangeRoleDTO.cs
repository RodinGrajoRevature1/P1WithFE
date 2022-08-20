using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectOneModels
{
    public class ChangeRoleDTO
    {
        public Guid? RoleChangingEmployeeID { get; set; }
        public Guid? ProcessingManagerID { get; set; }
        public Guid? NewManagerID { get; set; }
        public string? NewRole { get; set; }

        public ChangeRoleDTO() {}

        public ChangeRoleDTO(Guid? roleChangingEmployeeID, Guid? processingManagerID, Guid? newManagerID, string? newRole)
        {
            this.RoleChangingEmployeeID = roleChangingEmployeeID;
            this.ProcessingManagerID = processingManagerID;
            this.NewManagerID = newManagerID;
            this.NewRole = newRole;
        }
    }
}