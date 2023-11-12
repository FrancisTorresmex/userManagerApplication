var idUser = 0;

function login() {

    var loginForm = document.getElementById("userLogin");

    var email = loginForm.elements["Email"].value;
    var password = loginForm.elements["Password"].value;

    var obj = {
        email: email,
        password: password
    };

    requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(obj)
    };

    fetch('/Access/Login/', requestOptions)
        .then(function (response) {
            if (!response.ok) {                
                alertAnimatedCustom(error, 'error', 'An error occurred');
            }

            return response.json();

        })
        .then(function (response) {
            /*window.location.href = 'Admin/';*/

            if (!response.success) {
                alertAnimatedCustom(response.error, 'error', 'An error occurred');
            } else {
                window.location.href = response.data;
            }

            
        })
        .catch(function (error) {
            alertAnimatedCustom(error, 'error', 'An error occurred');
        });
}
