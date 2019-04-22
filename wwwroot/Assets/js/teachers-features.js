$(document).ready(function () {
    if ($("#SNI").is(":checked")) {
        $("#Level").show();
    }
    else {
        $("#Level").hide();
    }
    $("#SNI").click(function () {
        $("#Level").toggle();
    });

    $("#js-cancel-action").click(function () {
        bootbox.dialog({
            message: "¿Estás seguro que quieres eliminar este elemento?",
            title: "Confirmación",
            buttons: {
                no: {
                    label: 'No',
                    className: 'btn-success',
                    callback: function () {
                        bootbox.hideAll();
                    }
                },
                yes: {
                    label: 'Si',
                    className: 'btn-danger',
                    callback: function () {

                    }

                }
            }
        });

    });

});