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
    fetch("Admin/GetUserData/" + userId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            return response.json();
        })
        .then(function (userData) {

            var userForm = document.getElementById("editUserForm");
            userForm.elements["Name"].value = userData.name;
            userForm.elements["LastName"].value = userData.lastName;
            userForm.elements["Phone"].value = userData.phone;
            userForm.elements["Email"].value = userData.email;
            userForm.elements["Role"].value = userData.idRole;
        })
        .catch(function (error) {
            console.error(error);
        });
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

    var requestOptions = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json', 
        },
        body: JSON.stringify(user)
    };

    fetch("Admin/UpdateUser/", requestOptions)
        .then(function (response) {
            if (!response.ok) {
                throw new Error(response.statusText);    
            }
            return response.json();
            
        })
        .then(function (userData) {

            modalUpdateUserInitialize.hide();

            alertAnimatedCustom(userData.message, 'success', 'User updated');


        })
        .catch(function (error) {
            console.error(error);
        });

}

function alertBasicCustom(message, icon, title) {
    swal.fire({
        icon: icon,
        title: title,
        text: message
    });
}

function alertAnimatedCustom(message, icon = 'success', title = 'Alert') {
    Swal.fire({
        title: title,
        text: message,
        icon: icon,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    })
}


