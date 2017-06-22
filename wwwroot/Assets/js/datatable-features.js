$(document).ready(function () {
    $('#tableEntity').DataTable({
        "language": {
            "emptyTable": "No se encontraron elementos",
            "info": "_TOTAL_ elementos",
            "infoEmpty": "",
            "infoFiltered": "(filtrado de un total de _MAX_ entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ elementos",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron coincidencias",
            "paginate": {
                "first": "Primera",
                "last": "Ultima",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "aria": {
                "sortAscending": ": activar para ordenar columnas ascendentes",
                "sortDescending": ": activar para ordenar columnas descendentes"
            }
        },
        responsive: true
    });

    $('.js-delete').submit(function (e) {
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
    $('.js-approved').submit(function (e) {
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