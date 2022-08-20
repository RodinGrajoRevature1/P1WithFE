using dotenv.net;
using System.Data.SqlClient;
using ProjectOneModels;

namespace RepoLayer;

public class Repo
{
    private static readonly SqlConnection _conn = Repo._Connect();
    private static SqlConnection _Connect()
    {
        DotEnv.Load();
        IDictionary<string, string> envVars = DotEnv.Read();
        return new SqlConnection(envVars["CONNSTRING"]);
    }

    public void Test()
    {     
        SqlCommand command = new SqlCommand("SELECT * FROM dbo.Employee", _conn);
        _conn.Open();
        SqlDataReader myReader = command.ExecuteReader();

        while (myReader.Read())
        {
            Console.WriteLine($"\t{myReader.GetInt32(0)}\t{myReader.GetString(1)}");
        }

        _conn.Close();
    }

    public async Task<Employee?> GetEmployeeByUsernameAsync(string username)
    {
        int bufferSize = 100;
        byte[] outByte = new byte[bufferSize];

        SqlCommand? command = new SqlCommand("SELECT * FROM dbo.Employee WHERE Username = @username", _conn);
        command.Parameters.AddWithValue("@username", username);
        _conn.Open();

        SqlDataReader? ret = await command.ExecuteReaderAsync();
        if (ret.Read())
        {
            Guid? managerID = null;

            if(!(await ret.IsDBNullAsync(8)))
            {
                managerID = ret.GetGuid(8);
            }

            Employee e = new Employee(
                ret.GetGuid(0),
                ret.GetString(1),
                ret.GetString(2),
                ret.GetString(3),
                ret.GetString(4),
                ret.GetString(5),
                ret.GetString(6),
                ret.GetString(7),
                // ret.GetBytes(8, 0, outByte, 0, bufferSize),
                null,
                managerID,
                ret.GetDateTime(10),
                ret.GetDateTime(11)
            );

            _conn.Close();
            return e;
        }
        else
        {
            _conn.Close();
            return null;
        }
    }

    public async Task<Employee?> GetEmployeeByIDAsync(Guid? employeeID)
    {
        int bufferSize = 100;
        byte[] outByte = new byte[bufferSize];

        SqlCommand command = new SqlCommand("SELECT * FROM dbo.Employee WHERE EmployeeID = @employeeID", _conn);
        command.Parameters.AddWithValue("@employeeID", employeeID);
        _conn.Open();

        SqlDataReader? ret = await command.ExecuteReaderAsync();
        if (ret.Read())
        {
            Guid? managerID = null;

            if(!(await ret.IsDBNullAsync(9)))
            {
                managerID = ret.GetGuid(9);
            }

            Employee e = new Employee(
                ret.GetGuid(0),
                ret.GetString(1),
                ret.GetString(2),
                ret.GetString(3),
                ret.GetString(4),
                ret.GetString(5),
                ret.GetString(6),
                ret.GetString(7),
                // ret.GetBytes(8, 0, outByte, 0, bufferSize),
                null,
                managerID,
                ret.GetDateTime(10),
                ret.GetDateTime(11)
            );

            _conn.Close();
            return e;
        }
        else
        {
            _conn.Close();
            return null;
        }
    }

    public async Task<List<Ticket>?> GetTicketsByEmployeeID(Guid? employeeID, string? filterStatusBy, string? filterTypeBy)
    {
        List<Ticket> tickets = new List<Ticket>();
        int bufferSize = 100;
        byte[] outByte = new byte[bufferSize];
        SqlCommand command;

        if(filterStatusBy==null && filterTypeBy==null)
        {
            command = new SqlCommand("SELECT * FROM Ticket WHERE FK_EmployeeID = @employeeID", _conn);
            command.Parameters.AddWithValue("@employeeID", employeeID);        
        } 
        else if(filterTypeBy==null)
        {
            command = new SqlCommand("SELECT * FROM Ticket WHERE FK_EmployeeID = @employeeID AND Status = @filterStatusBy ", _conn);
            command.Parameters.AddWithValue("@employeeID", employeeID);  
            command.Parameters.AddWithValue("@filterStatusBy", filterStatusBy);  
        }
        else if(filterStatusBy==null)
        {
            command = new SqlCommand("SELECT * FROM Ticket WHERE FK_EmployeeID = @employeeID AND Type = @filterTypeBy ", _conn);
            command.Parameters.AddWithValue("@employeeID", employeeID);  
            command.Parameters.AddWithValue("@filterTypeBy", filterTypeBy);  
        }
        else
        {
            command = new SqlCommand("SELECT * FROM Ticket WHERE FK_EmployeeID = @employeeID AND Status = @filterStatusBy AND Type = @filterTypeBy", _conn);
            command.Parameters.AddWithValue("@employeeID", employeeID);  
            command.Parameters.AddWithValue("@filterStatusBy", filterStatusBy);  
            command.Parameters.AddWithValue("@filterTypeBy", filterTypeBy);  
        }

        _conn.Open();

        SqlDataReader? ret = await command.ExecuteReaderAsync();

        while(ret.Read())
        {
            long? receiptData = null;
            Guid? processingManagerData = null;
            DateTime? processDateData = null;

            if(!(await ret.IsDBNullAsync(5))) 
            {
                receiptData = ret.GetBytes(5, 0, outByte, 0, bufferSize);
            }

            if(!(await ret.IsDBNullAsync(7)))
            {
                processingManagerData = ret.GetGuid(7);
            }

            if(!(await ret.IsDBNullAsync(10))) 
            {
                processDateData = ret.GetDateTime(10);
            }

            Ticket t = new Ticket(
                ret.GetGuid(0),
                (decimal)ret.GetSqlMoney(1),
                ret.GetString(2),
                ret.GetString(3),
                ret.GetString(4),
                receiptData,
                ret.GetGuid(6),
                processingManagerData,
                ret.GetDateTime(8),
                ret.GetDateTime(9),
                processDateData
            );

            tickets.Add(t);
        }

        _conn.Close();

        return tickets;
    }

    public async Task<UpdateEmployeeDTO?> UpdateEmployeeInfo(UpdateEmployeeDTO ueDTO, Guid employeeID)
    {
        using SqlCommand command = new SqlCommand("UPDATE Employee SET Username = @username, Password = @password, Fname = @fname, Lname = @lname, Address = @address, Phone = @phone WHERE EmployeeID = @employeeID", _conn);
        command.Parameters.AddWithValue("@employeeID", employeeID);
        command.Parameters.AddWithValue("@username", ueDTO.Username);
        command.Parameters.AddWithValue("@password", ueDTO.Password);
        command.Parameters.AddWithValue("@fname", ueDTO.Fname);
        command.Parameters.AddWithValue("@lname", ueDTO.Lname);
        command.Parameters.AddWithValue("@address", ueDTO.Address);
        command.Parameters.AddWithValue("@phone", ueDTO.Phone);

        _conn.Open();
        int ret = await command.ExecuteNonQueryAsync(); 
        _conn.Close();

        if(ret < 1) return null;

        return ueDTO;
    }

    public async Task<byte[]?> GetEmployeePhoto(Guid employeeID)
    {
        using SqlCommand command = new SqlCommand("SELECT Photo FROM Employee WHERE EmployeeID = @employeeID", _conn);
        command.Parameters.AddWithValue("@employeeID", employeeID);
        
        _conn.Open();
        byte[]? photoData = (byte[]?)(await command.ExecuteScalarAsync());
        _conn.Close();

        return photoData;
    }

    public async Task<bool> UpdateEmployeePhotoAsync(byte[] photo, Guid employeeID)
    {
        using SqlCommand command = new SqlCommand("UPDATE [dbo].[Employee] SET Photo = @photo WHERE EmployeeID = @employeeID", _conn);
        command.Parameters.AddWithValue("@photo", photo);
        command.Parameters.AddWithValue("@employeeID", employeeID);

        _conn.Open();
        bool ret = (await command.ExecuteNonQueryAsync()) > 0;
        _conn.Close();

        return ret;
    }

    public async Task<byte[]?> GetReceiptPhoto(Guid ticketID)
    {
        using SqlCommand command = new SqlCommand("SELECT Receipt FROM Ticket Where TicketID = @ticketID", _conn);
        command.Parameters.AddWithValue("@ticketID", ticketID);
        
        _conn.Open();
        byte[]? photoData = (byte[]?)(await command.ExecuteScalarAsync());
        _conn.Close();

        return photoData;
    }

    public async Task<bool> UpdateTicketPhotoAsync(byte[] photo, Guid ticketID)
    {
        using SqlCommand command = new SqlCommand("UPDATE [dbo].[Ticket] SET Receipt = @photo WHERE TicketID = @ticketID", _conn);
        command.Parameters.AddWithValue("@photo", photo);
        command.Parameters.AddWithValue("@ticketID", ticketID);

        _conn.Open();
        bool ret = (await command.ExecuteNonQueryAsync()) > 0;
        _conn.Close();

        return ret;
    }

    public async Task<Ticket?> GetTicketByIDAsync(Guid? ticketID)
    {
        int bufferSize = 100;
        byte[] outByte = new byte[bufferSize];

        SqlCommand command = new SqlCommand("SELECT * FROM dbo.Ticket WHERE TicketID = @ticketID", _conn);
        command.Parameters.AddWithValue("@ticketID", ticketID);
        _conn.Open();

        SqlDataReader? ret = await command.ExecuteReaderAsync();
        if (ret.Read())
        {
            long? receiptData = null;
            Guid? processingManagerData = null;
            DateTime? processDateData = null;

            if(!(await ret.IsDBNullAsync(5))) 
            {
                receiptData = ret.GetBytes(5, 0, outByte, 0, bufferSize);
            }

            if(!(await ret.IsDBNullAsync(7)))
            {
                processingManagerData = ret.GetGuid(7);
            }

            if(!(await ret.IsDBNullAsync(10))) 
            {
                processDateData = ret.GetDateTime(10);
            }

            
            Ticket t = new Ticket
            (
                ret.GetGuid(0),
                (decimal)ret.GetSqlMoney(1),
                ret.GetString(2),
                ret.GetString(3),
                ret.GetString(4),
                receiptData,
                ret.GetGuid(6),
                processingManagerData,
                ret.GetDateTime(8),
                ret.GetDateTime(9),
                processDateData
            );
            _conn.Close();
            return t;
        }
        else
        {
            _conn.Close();
            return null;
        }
    }

    public async Task<bool> InsertNewEmployeeAsync(RegisterDTO r, Guid id)
    {
        using (SqlCommand command = new SqlCommand("INSERT INTO Employee (EmployeeID, Username, Password, Fname, Lname, Role, Address, Phone, ManagerID) VALUES (@employeeID, @username, @password, @fname, @lname, @role, @address, @phone, @managerID)", _conn))
        {
            command.Parameters.AddWithValue("@employeeID", id);
            command.Parameters.AddWithValue("@username", r.Username);
            command.Parameters.AddWithValue("@password", r.Password);
            command.Parameters.AddWithValue("@fname", r.Fname);
            command.Parameters.AddWithValue("@lname", r.Lname);
            command.Parameters.AddWithValue("@role", "Employee");
            command.Parameters.AddWithValue("@address", r.Address);
            command.Parameters.AddWithValue("@phone", r.Phone);
            command.Parameters.AddWithValue("@managerID", r.ManagerID);
            _conn.Open();
            bool ret = (await command.ExecuteNonQueryAsync()) == 1;
            _conn.Close();
            return ret;
        };
    }

    public async Task<bool> InsertNewTicketAsync(Ticket t)
    {
        using (SqlCommand command = new SqlCommand("INSERT INTO Ticket (TicketID, Amount, Description, Status, Type, FK_EmployeeID) VALUES (@ticketID, @amount, @description, @status, @type, @employeeID)", _conn))
        {
            command.Parameters.AddWithValue("@ticketID", t.TicketID);
            command.Parameters.AddWithValue("@amount", t.Amount);
            command.Parameters.AddWithValue("@description", t.Description);
            command.Parameters.AddWithValue("@status", t.Status);
            command.Parameters.AddWithValue("@type", t.Type);
            command.Parameters.AddWithValue("@employeeID", t.FK_EmployeeID);
            _conn.Open();
            bool ret = (await command.ExecuteNonQueryAsync()) == 1;
            _conn.Close();
            return ret;
        }
    }

    public async Task<bool> UpdateTicketByIDAsync(Ticket t, Guid? processingManagerID)
    {
        using (SqlCommand command = new SqlCommand("UPDATE [dbo].[Ticket] SET Status = @status, FK_ProcessingManagerID = @processingManagerID WHERE TicketID = @ticketID", _conn))
        {
            command.Parameters.AddWithValue("@status", t.Status);
            command.Parameters.AddWithValue("@processingManagerID", processingManagerID);
            command.Parameters.AddWithValue("@ticketID", t.TicketID);
            _conn.Open();
            bool ret = (await command.ExecuteNonQueryAsync()) > 0;
            _conn.Close();
            return ret;
        }
    }

    public async Task<bool> UpdateEmployeeRoleByIDAsync(Guid? employeeID, Guid? newManagerID, string? newRole)
    {
        using (SqlCommand command = new SqlCommand("UPDATE [dbo].[Employee] SET Role = @newRole, ManagerID = @newManager WHERE EmployeeID = @employeeID", _conn))
        {
            command.Parameters.AddWithValue("@newRole", newRole);
            command.Parameters.AddWithValue("@newManager", newManagerID);
            command.Parameters.AddWithValue("@employeeID", employeeID);
            _conn.Open();
            bool ret = (await command.ExecuteNonQueryAsync()) > 0;
            _conn.Close();
            return ret;
        }
    }
}

