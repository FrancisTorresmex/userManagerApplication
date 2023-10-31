
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

    fetch('Access/Login/', requestOptions)
        .then(function (response) {
            if (!response.ok) {                
                alertAnimatedCustom(error, 'error', 'An error occurred');
            }

            return response.text();

        })
        .then(function (response) {
            window.location.href = 'Admin/';
        })
        .catch(function (error) {
            alertAnimatedCustom(error, 'error', 'An error occurred');
        });
}
