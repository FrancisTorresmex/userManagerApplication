let objectUserData = null;
let tblUsers = null;
let modalUpdateUserInitialize = null;
let idUserTemp = 0;

$(document).ready(function () {
    createTableUsers();
});

document.addEventListener("DOMContentLoaded", function () {

    let modalUpdateUser = document.getElementById('updateUserModal');
    modalUpdateUserInitialize = new bootstrap.Modal(modalUpdateUser);
});

function createTableUsers() {
    tblUsers = $("#userTable").DataTable({        
        "paging": true,
        "destroy": true,
        "order": [[0, 'asc'], [1, 'asc']],
        "searching": true,
        "lengthChange": true,
        "info": true,
        "responsive": true,
        "autoWidth": false,
        "processing": true,
        columns: [
            { data: 'IdUser' },
            { data: 'Name' },
            { data: 'LastName' },
            { data: 'Email' },
            { data: 'Phone' },
            { data: 'RoleName' },
            { data: 'DateAdmision' },
            { data: 'InactiveDate' },
            { data: 'StatusName' },
            {
                data: null,
                render: function (data, type, full, meta) {
                    var userId = data.IdUser;
                    var userStatus = data.statusName;
                    return '<button type="button" class="btn btn-primary" data-id="' + userId + '" id="btnUpdateUser" onclick="openModalUpdateUser(' + userId + ')">Edit</button>' +
                        '<button class="btn btn-info" data-id="' + userId + '"  id="btnInacivateUser' + userId + '" onclick="inactiveActiveUser(\'' + userId + '\', \'' + userStatus + '\')">' + (data.StatusName == "Inactive" ? "Active" : "Inactive") + '</button>';
                }
            }
        ]
    });
}

//open modal addUser
document.getElementById("btnCreateUser").addEventListener('click', function () {
    userId = 0;
    modalUpdateUserInitialize.show();
    getAllRoles();
    document.getElementById('editUserForm').reset();
});

function inactiveActiveUser(idUser, status) {

    idUserTemp = idUser;
    let contraryStatus = status == 'Active' ? 'Inactive' : 'Active';

    var model = {
        IdUser: idUser,
        Status: contraryStatus
    };

    functionFetch("/Admin/InactiveUser/", model, "PUT", successInactiveActiveUser);
}

function successInactiveActiveUser(data) {

    if (data.success) {

        var row = tblUsers.row('tr[data-id="' + idUserTemp + '"]');
        var rowData = row.data();

        var btn = document.getElementById("btnInacivateUser" + idUserTemp);
        btn.textContent = rowData.StatusName === "Inactive" ? "Active" : "Inactive";

        var status = (rowData.StatusName === "Inactive" ? "Active" : "Inactive");
        var objRows = {
            Status: status
        };
        updateRowDataTable(tblUsers, idUserTemp, objRows, false);
        
    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}
