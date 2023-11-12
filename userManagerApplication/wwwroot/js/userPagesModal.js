document.addEventListener("DOMContentLoaded", function () {
    
}); 

function getUserAccessPages() {
    document.getElementById('editUserPagesForm').reset();
    functionFetch('/Pages/GetAllUserAndAccessPages/?idUser=' + userId, {}, 'GET', successGetUserAccessPages);
}

function successGetUserAccessPages(data) {
    if (data.success) {

        var container = document.getElementById("containerUserPagesCheck");
        container.innerHTML = '';

        data.data.forEach(function (element) {
            var div = document.createElement("div");
            div.classList.add("form-check", "form-switch");

            var input = document.createElement("input");
            input.classList.add("form-check-input");
            input.type = "checkbox";
            input.id = element.id;
            input.checked = element.userAccess;

            var label = document.createElement("label");
            label.classList.add("form-check-label");
            label.htmlFor = element.id;
            label.textContent = element.title;

            div.appendChild(input);
            div.appendChild(label);
            container.appendChild(div);
        });
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}


document.getElementById("btnUpdateUserPages").addEventListener("click", function () {

    var lstChecks = getUserPagesChecks();
    functionFetch('/Pages/UpdateUserPages/', lstChecks, 'PUT', successUpdateUserAccessPages);

});

function successUpdateUserAccessPages(data) {
    if (data.success) {
        modalUserPagesInitialize.hide();
        alertAnimatedCustom(data.message, 'success', 'successfully updated');
    }
    else {
        alertAnimatedCustom(data.message, 'error', 'An error occurred');
    }
}

function getUserPagesChecks() {
    var contianer = document.getElementById("containerUserPagesCheck");
    var checkboxes = contianer.querySelectorAll("input[type='checkbox']");
    var values = [];

    checkboxes.forEach(function (checkbox) {

        if (checkbox.checked) {
            values.push({
                IdUser: userId,
                IdScreen: checkbox.id
            });
        }        
    });

    if (values.length === 0) {
        values.push({ IdUser: userId, IdScreen: -1 });
    }

    return values;
}