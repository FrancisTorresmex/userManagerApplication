let objectUserData = null;
let tblUsers = null;

$(document).ready(function () {
    createTableUsers();

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
            { data: 'Status' },
            {
                data: null,
                render: function (data, type, full, meta) {
                    var userId = data.IdUser;
                    var userStatus = data.Status;
                    return '<button type="button" class="btn btn-primary" data-id="' + userId + '" id="btnUpdateUser" onclick="openModalUpdateUser(' + userId + ')">Edit</button>' +
                        '<button class="btn btn-info" id="btnInacivateUser" onclick="inactiveActiveUser(\'' + userId + '\', \'' + userStatus + '\')">' + (data.Status == "Inactive" ? "Inactive" : "Active") + '</button>';
                }
            }
        ]
    });
}

function inactiveActiveUser(idUser, status) {

    var model = {
        IdUser: idUser,
        Status: status
    };

    functionFetch("Admin/InactiveUser/", model, "PUT", successInactiveActiveUser)
}

function successInactiveActiveUser(data) {

    if (data.success) {

        //update rows and cells
        //btn status
        var btn = document.getElementById("btnInacivateUser");
        var row = tblUsers.row(btn.closest('tr'));
        var cellBtn = tblUsers.cell(row, 8);
        var currentText = btn.textContent;
        var newText = (currentText === "Inactive" ? "Active" : "Inactive");
        btn.textContent = newText;
        cellBtn.data(newText).draw();

        //tr status text
        var statusTxt = document.getElementById("statusTd");
        var rowStatus = tblUsers.row(statusTxt.closest('tr'));
        var cellStatus = tblUsers.cell(rowStatus, 8);
        statusTxt.textContent = newText;
        cellStatus.data(newText).draw();
    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}
