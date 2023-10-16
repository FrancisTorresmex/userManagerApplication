let userId = 0;
let modalUpdateUserInitialize = null;

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
}

function getUserData(idUser) {

    userId = idUser;    
    functionFetch('Admin/GetUserData/' + userId, {}, 'GET', successGetUserData);
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

    updateUserData(user);

}

function updateUserData(user) {
    functionFetch('Admin/UpdateUser/', user, 'PUT', successUpdateUserData);
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

        updateRowTableUser(tblUsers, dataUserUpdated.idUser, objUser);
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}

function updateRowTableUser(tbl, valueSearch, valueUpdated) {

    var row = tbl.row('tr[data-id="' + valueSearch + '"]');

    if (row) {
        tbl.row('tr[data-id="' + valueSearch + '"]').data(valueUpdated).draw();
    }
    else {
        alertAnimatedCustom('Row not found', 'error', 'An error occurred');        
    }
}




