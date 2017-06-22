$(document).ready(function () {

    var vm = {
        teacherIds: []
    };

    $("#teachers").ready(function () {
        $(".list-group-item").each(function () {          
            var id = $(this).attr('id');
            vm.teacherIds.push(id);
        });
    });
 
    $('.input-group.date').datepicker({
        format: 'yyyy-mm-dd',
        endDate: '1d',
        todayBtn: "linked",
        autoclose: true,
        todayHighlight: true
    }).on('change', function (e) {
        $('#EditionDate').focus();
        $('#PublishedDate').focus();
    });

    var teachers = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,

        remote: {
            url: '/api/teachers/search/%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#teacher').typeahead({
        minLength: 1,
        highlight: true
    }, {
            name: 'teachers',
            display: 'name',
            source: teachers
        
        }).on("typeahead:select", function (e, teacher) {

            if (vm.teacherIds.length === 0) {
                vm.teacherIds.push(teacher.id);
                $("#teachers").append("<div> <div class='list-group-item' id=" + teacher.id + "> " + teacher.name + " </div> <input name='TeacherIds' hidden value=" + teacher.id + "></input> </div > ");
                $("#teacher").typeahead("val", "");
            }
            function checkUser() {
                for (var id in vm.teacherIds) {
                    if (teacher.id === vm.teacherIds[id])
                        return false;
                }
                return true;
            }
            if (checkUser()) {
                vm.teacherIds.push(teacher.id);
                $("#teachers").append("<div> <div class='list-group-item' id=" + teacher.id + ">" + teacher.name + "</div> <input name='TeacherIds' hidden value=" + teacher.id + "></input> </div > ");
                $("#teacher").typeahead("val", "");

            }
        });

    $("#teachers").on('click', '.list-group-item', function () {
        var id = $(this).attr('id');
        var box = this;
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
                        var l = vm.teacherIds.length;
                        $(box).parent('div').remove();
                        for (var i = 0; i < l; i++) {
                            if (id === vm.teacherIds[i])
                                vm.teacherIds.splice(i, 1);
                        }
                    }

                }
            }
        });
        
    });
});