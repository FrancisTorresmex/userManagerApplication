let userId = 0;

document.addEventListener("DOMContentLoaded", function () {
    //var updateUserModal = document.getElementById("updateUserModal");
    //updateUserModal.addEventListener("show.bs.modal", function (event) {
    //    var button = event.relatedTarget; // Obtiene el botón que desencadenó la apertura del modal
    //    userId = button.getAttribute("data-id");

    //});

    //Initialize modal updateUser
    let modalUpdateUser = document.getElementById('updateUserModal');
    modalUpdateUserInitialize = new bootstrap.Modal(modalUpdateUser);
});

function openModalUpdateUser(idUser) {        
    modalUpdateUserInitialize.show();
    getUserData(idUser);
    getAllRoles();
}

function getAllRoles() {
    functionFetch('/Roles/GetAllRoles/', {}, 'GET', successGetAllRoles);
}

function successGetAllRoles(data) {
    if (data.success) {
        let selectRoles = document.getElementById("selRoleUser");

        for (var i = 0; i < data.data.length; i++) {            
            var option = document.createElement('option');
            option.value = data.data[i].idRole; 
            option.text = data.data[i].name; 
            selectRoles.appendChild(option);
        }

    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}


function getUserData(idUser) {

    userId = idUser;    
    functionFetch("/Admin/GetUserData/?id=" + userId, {}, "GET", successGetUserData);
}

function successGetUserData(data) {
    if (data.success) {

        var userData = data.data;
        var userForm = document.getElementById("editUserForm");

        userForm.elements["Name"].value = userData.name;
        userForm.elements["LastName"].value = userData.lastName;
        userForm.elements["Phone"].value = userData.phone;
        userForm.elements["Email"].value = userData.email;
        userForm.elements["Role"].value = userData.idRole;
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}

function updateUser() {

    //form fields
    var editUserForm = document.getElementById("editUserForm");

    var name = editUserForm.elements["Name"].value;
    var lastName = editUserForm.elements["LastName"].value;
    var phone = editUserForm.elements["Phone"].value;
    var email = editUserForm.elements["Email"].value;
    var role = editUserForm.elements["Role"].value;

    var user = {
        IdUser: userId,
        Name: name,
        LastName: lastName,
        Phone: phone,
        Email: email,
        IdRole: role
    };

    if (userId == 0) //add user
        addUserData(user);
    else
        updateUserData(user);
    

}

function updateUserData(user) {
    functionFetch('/Admin/UpdateUser/', user, 'PUT', successUpdateUserData);
}

function successUpdateUserData(data) {

    if (data.success) {

        modalUpdateUserInitialize.hide();
        alertAnimatedCustom(data.message, 'success', 'User updated');

        var dataUserUpdated = data.data;

        var objUser = {
            IdUser: dataUserUpdated.idUser,
            LastName: dataUserUpdated.lastName,
            Name: dataUserUpdated.name,
            Email: dataUserUpdated.email,
            Phone: dataUserUpdated.phone,
            RoleName: dataUserUpdated.roleName,
            DateAdmision: dataUserUpdated.dateAdmision,
            InactiveDate: dataUserUpdated.inactiveDate,
            Status: dataUserUpdated.status,
        };

        updateRowDataTable(tblUsers, dataUserUpdated.idUser, objUser, true);
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}

//Add new user
function addUserData(user) {
    functionFetch('/Admin/AddUser/', user, 'POST', successAddUserData);
}

function successAddUserData(data) {
    if (data.success) {

        modalUpdateUserInitialize.hide();
        alertAnimatedCustom(data.message, 'success', 'User created successfully');

        var dataUserCreated = data.data;

        //get rol name local
        let rolTemp = document.getElementById("selRoleUser");
        let selectedIndex = rolTemp.selectedIndex;
        let rolrolNameTemp = rolTemp.options[selectedIndex].text;;

        var objUser = {
            IdUser: dataUserCreated.idUser,
            LastName: dataUserCreated.lastName,
            Name: dataUserCreated.name,
            Email: dataUserCreated.email,
            Phone: dataUserCreated.phone,
            RoleName: rolrolNameTemp,
            DateAdmision: dataUserCreated.dateAdmision,
            InactiveDate: dataUserCreated.inactiveDate,
            Status: dataUserCreated.status,
        };

        tblUsers.row.add(objUser).draw(false);
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}




