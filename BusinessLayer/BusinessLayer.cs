using ProjectOneModels;
using RepoLayer;

namespace BusinessLayer;
public class Bus
{
    private Repo _repo = new Repo();
    private Employee? _loggedIn = null;
    public Employee? LoggedIn
    {
        get
        {
            return _loggedIn;
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        Employee? e = await this._repo.GetEmployeeByUsernameAsync(username);
        if (e != null && e.Password == password)
        {
            _loggedIn = e;
            return true;
        }
        else return false;
    }

    public async Task<Employee?> RegisterNewUserAsync(RegisterDTO r)
    {
        Guid id = Guid.NewGuid();

        if(await this._repo.InsertNewEmployeeAsync(r, id))
        {
            Employee? e = await this._repo.GetEmployeeByIDAsync(id);
            return e;
        }

        return null;
    }

    public async Task<Ticket?> SubmitTicketAsync(SubmitTicketDTO s)
    {
        s.TicketID = Guid.NewGuid();
        Ticket t = new Ticket(s);

        if(await this._repo.InsertNewTicketAsync(t))
        {
            return await this._repo.GetTicketByIDAsync(s.TicketID);
        }

        return null;
    }

    public async Task<bool> ProcessTicketAsync(ProcessTicketDTO p)
    {
        Manager? m = new Manager((await this._repo.GetEmployeeByIDAsync(p.ProcessingManagerID)), null);
        Ticket? t = await this._repo.GetTicketByIDAsync(p.TicketID);

        if(m.Role != "Manager" || t?.Status != "Pending")
        {
            return false;
        }

        if(p.NewStatus == "Approved")
        {
            t.Approve();
        }
        else if(p.NewStatus == "Denied")
        {
            t.Deny();
        }
        else
        {
            return false;
        }

        // Update ticket in database
        bool isSuccess = await this._repo.UpdateTicketByIDAsync(t, m.EmployeeID);
        return isSuccess;
    }

    public async Task<List<Ticket>?> GetMyTicketsAsync(Guid? employeeID, string? filterStatusBy, string? filterTypeBy)
    {
        Employee? e = await this._repo.GetEmployeeByIDAsync(employeeID);
        
        if (e == null) return null;

        List<Ticket>? tickets = await this._repo.GetTicketsByEmployeeID(employeeID, filterStatusBy, filterTypeBy);

        return tickets;
    }

    public async Task<bool> PromoteEmployee(Guid? promotingEmployeeID, Guid? processingManagerID, Guid? newManagerID)
    {
        Employee? promotingEmployee = await this._repo.GetEmployeeByIDAsync(promotingEmployeeID);
        Manager? processingManager = new Manager(await this._repo.GetEmployeeByIDAsync(processingManagerID), null);
        Manager? newManager = new Manager(await this._repo.GetEmployeeByIDAsync(newManagerID), null);

        if(processingManager.Role != "Manager")
        {
            return false;
        }

        Manager? promotedEmployee = processingManager.PromoteEmployee(promotingEmployee, newManager);

        bool isSuccess = await this._repo.UpdateEmployeeRoleByIDAsync(promotedEmployee.EmployeeID, promotedEmployee.ManagerID, "Manager");
        return isSuccess;
    }

    public async Task<bool> DemoteManager(Guid? demotingManagerID, Guid? processingManagerID, Guid? newManagerID)
    {
        Manager? demotingManager = new Manager(await this._repo.GetEmployeeByIDAsync(demotingManagerID), newManagerID);
        Manager? processingManager = new Manager(await this._repo.GetEmployeeByIDAsync(processingManagerID), null);
        Manager? newManager = new Manager(await this._repo.GetEmployeeByIDAsync(newManagerID), null);

        if(processingManager.Role != "Manager")
        {
            return false;
        }

        Employee? demotedManager = processingManager.DemoteManager(demotingManager, newManager);

        bool isSuccess = await this._repo.UpdateEmployeeRoleByIDAsync(demotedManager?.EmployeeID, demotedManager?.ManagerID, "Employee");

        return isSuccess;
    }

    public async Task<bool> UploadReceiptPhoto(Stream file, Guid ticketID)
    {
        Ticket? updatedTicket = await this._repo.GetTicketByIDAsync(ticketID);

        if(updatedTicket == null) return false;

        using BinaryReader reader = new BinaryReader(file);

        byte[] photo = reader.ReadBytes((int)file.Length);

        bool isSuccess = await this._repo.UpdateTicketPhotoAsync(photo, ticketID);   

        return isSuccess;
    }

    public async Task<byte[]?> GetReceiptPhoto(Guid ticketID)
    {
        byte[]? photo = await this._repo.GetReceiptPhoto(ticketID);

        if(photo == null) return null;

        return photo;
    }

    public async Task<bool> UploadEmployeePhoto(Stream file, Guid employeeID)
    {
        Employee? updatedEmployee = await this._repo.GetEmployeeByIDAsync(employeeID);

        using BinaryReader reader = new BinaryReader(file);

        byte[] photo = reader.ReadBytes((int)file.Length);

        if(updatedEmployee == null) return false;

        bool isSuccess = await this._repo.UpdateEmployeePhotoAsync(photo, employeeID);   

        return isSuccess;
    }

    public async Task<byte[]?> GetEmployeePhoto(Guid employeeID)
    {
        byte[]? photo = await this._repo.GetEmployeePhoto(employeeID);

        if(photo == null) return null;

        return photo;
    }

    public async Task<UpdateEmployeeDTO?> UpdateEmployeeInfo(UpdateEmployeeDTO ueDTO, Guid employeeID)
    {
        Employee? employeeOld = await this._repo.GetEmployeeByIDAsync(employeeID);

        if (employeeOld == null) return null;

        if (ueDTO.Username == null) ueDTO.Username = employeeOld.Username;
        if (ueDTO.Password == null) ueDTO.Password = employeeOld.Password;
        if (ueDTO.Fname == null) ueDTO.Fname= employeeOld.Fname;
        if (ueDTO.Lname == null) ueDTO.Lname = employeeOld.Lname;
        if (ueDTO.Address == null) ueDTO.Address = employeeOld.Address;
        if (ueDTO.Phone == null) ueDTO.Phone = employeeOld.Phone;

        return await this._repo.UpdateEmployeeInfo(ueDTO, employeeID);
    }
}
