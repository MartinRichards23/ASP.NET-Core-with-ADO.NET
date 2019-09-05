function ToggleVisibility(chkBoxId, elementId) {
    var chkBox = document.getElementById(chkBoxId);
    var panel = document.getElementById(elementId);

    if (chkBox.checked) {
        panel.style.display = '';
    }
    else {
        panel.style.display = 'none';
    }
}

function AnimateVisibility(chkBoxId, elementId) {
    var chkBox = document.getElementById(chkBoxId);
    var panel = document.getElementById(elementId);

    if (chkBox.checked) {
        $(panel).fadeIn(200);
    }
    else {

        $(panel).fadeOut(200);
    }
}

function ClearLog() {
    $.ajax({
        type: "POST",
        url: '/Manage/ClearLog',
        data: {},
        success: function () {

            location.reload();
        }
    });
}

function AskDeletePaymentMethod(id) {
    BootstrapDialog.show({
        title: 'Confirm',
        message: 'Are you sure you want to delete this payment method?',
        type: BootstrapDialog.TYPE_WARNING,
        buttons: [{
            label: 'Delete',
            autospin: true,
            action: function (dialog) {
                document.getElementById('deleteForm_' + id).submit();
                dialog.close();
            }
        }, {
            label: 'Cancel',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
}

function submitForm(id){
    document.getElementById(id).submit();
}

function getQueryVariable(variable) {

    var query = window.location.search.substring(1);
    var vars = query.split("&");

    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");

        if (pair[0] == variable) {

            var value = pair[1];

            return decodeURIComponent(value);
        }
    }

    return null;
}