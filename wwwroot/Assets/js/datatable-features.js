$(document).ready(function () {
    $('tbody').on('click', '.js-delete', function (e) {
        var currentForm = this; 
        e.preventDefault();
        bootbox.dialog({
            message: "¿Estas seguro que quieres eliminar este elemento?",
            title: "Confirmación",
            buttons: {
                no: {
                    label: 'No',
                    className: 'btn-danger',
                    callback: function () {
                        bootbox.hideAll();
                    }
                },
                yes: {
                    label: 'Si',
                    className: 'btn-success',
                    callback: function () {
                        currentForm.submit();
                    }
                }
            }
        });
    });

    $('tbody').on('click', '.js-approved', function (e) {
        var currentForm = this;
        e.preventDefault();
        bootbox.dialog({
            message: "¿Estas seguro que quieres modificar el estado de este elemento?",
            title: "Confirmación",
            buttons: {
                no: {
                    label: 'No',
                    className: 'btn-danger',
                    callback: function () {
                        bootbox.hideAll();
                    }
                },
                yes: {
                    label: 'Si',
                    className: 'btn-success',
                    callback: function () {
                        currentForm.submit();
                    }
                }
            }
        });
    });
    $('[data-toggle="tooltip"]').tooltip();
});