/* 
    Initial js: Here will be the functions required when starting the application 
*/


//reusable fetch call
function functionFetch(url, data, methodType, funcSuccess) { //funcSuccess = return function, data = {object}

    var requestOptions = {
        method: methodType,
        headers: {
            'Content-Type': 'application/json',
            //'Authorization': 'Bearer ' + token
        },
         //body: JSON.stringify(data)
        };

    if (methodType !== 'GET') {
        requestOptions.body = JSON.stringify(data);
    }

    fetch(url, requestOptions)
        .then(function (response) {
            if (!response.ok) {
                /*throw new Error(response.statusText);*/
                alertAnimatedCustom(error, 'error', 'An error occurred');
            }

            return response.json();

        })
        .then(function (response) {
            funcSuccess(response);
        })
        .catch(function (error) {
            alertAnimatedCustom(error, 'error', 'An error occurred');
        });
}

//reusable: update datatable row
//isObject = true if you send an entire object, or false if you only want to update a part of that object
function updateRowDataTable(tbl, valueSearch, valueUpdated, updateAllObjectBool) {

    var row = tbl.row('tr[data-id="' + valueSearch + '"]');

    if (row) {

        if (updateAllObjectBool) {
            tbl.row('tr[data-id="' + valueSearch + '"]').data(valueUpdated).draw();
        }
        else {
            var rowData = row.data();
            for(var key in valueUpdated) {
                if (rowData.hasOwnProperty(key)) {
                    rowData[key] = valueUpdated[key];
                }
            }
            row.data(rowData).draw();
        }

        
        
    }
    else {
        alertAnimatedCustom('Row not found', 'error', 'An error occurred');
    }
}

//reusable: update all datatable
function updateDataTable(tbl, data) {
    tbl.api().clear().rows.add(data).draw();
}

//Alerts sweet alerts: Basic
function alertBasicCustom(message, icon, title) {
    swal.fire({
        icon: icon,
        title: title,
        text: message
    });
}

//Alerts sweet alerts: animation and basic
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


//Get user pages
function getUserPages() {

    functionFetch("/Pages/GetAllUserPages/", {}, "GET", successGetUserPages);
}

function successGetUserPages(data) {
    if (data.success) {

        var lstScreen = data.data;
        var menu = document.querySelector(".sidenav");
        lstScreen.forEach(function (item) {

            var li = document.createElement("li");
            var a = document.createElement("a");

            a.setAttribute("href", item.url);
            a.textContent = item.title;
            li.appendChild(a);
            menu.appendChild(li);
        });
    }
    else
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
}

getUserPages();
