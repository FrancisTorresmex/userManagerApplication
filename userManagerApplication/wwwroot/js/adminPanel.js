let objectUserData = null;


$(document).ready(function () {
    createTableUsers();

});

function createTableUsers() {
    $("#userTable").dataTable({        
        "paging": true,
        "destroy": true,
        "order": [[0, 'asc'], [1, 'asc']],
        "searching": true,
        "lengthChange": true,
        "info": true,
        "responsive": true,
        "autoWidth": false,
        "processing": true,

    });
}

//function getUserData() {

//    var btnUpdateUser = document.getElementById("btnUpdateUser");
//    var userId = btnUpdateUser.getAttribute("data-id");

//    // Realizar una solicitud Ajax al controlador MVC para obtener datos del usuario
//    fetch("Admin/GetUserData/" + userId)
//        .then(function (response) {
//            if (response.ok) {
//                return response.json();
//            }
//            throw new Error("Error al obtener datos del usuario");
//        })
//        .then(function (userData) {
            
//            console.log(userData);

            

//        })
//        .catch(function (error) {
//            console.error(error);
//        });

//}

