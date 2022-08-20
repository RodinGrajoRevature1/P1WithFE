export const LoginAPI = async (username, password) => {
    return (await fetch(`/employees/login`, {
        method: 'POST',
        body: JSON.stringify({
            username,
            password
        }),
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const RegisterAPI = async (username, password, fname, lname, address, phone, managerID) => {
    return (await fetch(`/employees/register`, {
        method: 'POST',
        body: JSON.stringify({
            username,
            password,
            fname,
            lname,
            address,
            phone,
            managerID
        }),
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const SubmitTicketAPI = async (amount, description, type, fk_employeeID) => {
    return (await fetch(`${fk_employeeID}/tickets/submit-ticket`, {
        method: 'POST',
        body: JSON.stringify({
            amount,
            description,
            type,
            fk_employeeID
        }),
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const ProcessTicketAPI = async (ticketID, processingManagerID, newStatus) => {
    return (await fetch(`${ticketID}/process-ticket`, {
        method: 'PUT',
        body: JSON.stringify({
            ticketID,
            processingManagerID,
            newStatus
        }),
        headers: {
        'Content-Type': 'application/json',
        }
    })).json();
}

export const GetMyTicketsAPI = async (employeeID, filterStatusBy, filterTypeBy) => {
    return (await fetch(`${employeeID}/my-tickets?filterStatusBy=${filterStatusBy}&filterTypeBy=${filterTypeBy}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const ChangeEmployeeRoleAPI = async (roleChangingEmployeeID, processingManagerID, newManagerID, newRole) => {
    return (await fetch(`${roleChangingEmployeeID}/change-role`, {
        method: 'PUT',
        body: JSON.stringify({
            roleChangingEmployeeID,
            processingManagerID,
            newManagerID,
            newRole
        }),
        headers: {
          'Content-Type': 'application/json',
        }
    })).json();
}

export const UploadReceiptPhotoAPI = async (imageFile, ticketID) => {
    return (await fetch(`${ticketID}/upload/receipt-photo`, {
        method: 'PUT',
        body: JSON.stringify({
            imageFile,
            ticketID
        }),
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const GetReceiptPhotoAPI = async (ticketID) => {
    return (await fetch(`${ticketID}/receipt-photo`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const UploadEmployeePhotoAPI = async (imageFile, employeeID) => {
    return (await fetch(`${employeeID}/upload/photo`, {
        method: 'PUT',
        body: JSON.stringify({
            imageFile,
            employeeID
        }),
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const GetEmployeePhotoAPI = async (employeeID) => {
    return (await fetch(`${employeeID}/photo`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}

export const UpdateEmployeeInfoAPI = async (username, password, fname, lname, address, phone, employeeID) => {
    return (await fetch(`${employeeID}/update-info`, {
        method: 'PUT',
        body: JSON.stringify({
            employeeID,
            username,
            password,
            fname,
            lname,
            address,
            phone
        }),
        headers: {
            'Content-Type': 'application/json',
        }
    })).json();
}