/* 
    Initial js: Here will be the functions required when starting the application 
*/


//reusable fetch call
function functionFetch(url, data, methodType, funcSuccess) { //funcSuccess = return function, data = {object}

    var requestOptions = null;
    if (Object.keys(data).length > 0) {

        requestOptions = {
            method: methodType,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        };
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
