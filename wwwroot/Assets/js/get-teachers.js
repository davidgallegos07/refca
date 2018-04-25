$(document).ready(function () {
    var teacherIds = [];
    // get Teachers from model
    $("#teachers").ready(function () {
        $(".list-group-item").each(function () {
            var id = $(this).attr('id');
            teacherIds.push(id);
        });
    });

    // check user
    function checkUser(teacherId) {
        for (var id in teacherIds) {
            if (teacherId === teacherIds[id]) {
                return true;
            }
        }
        return false;
    }
    // find user
    function findUser(teacherId) {
        for (var id in teacherIds) {
            if (teacherId === teacherIds[id]) {
                return id;
            }
        }
        return null;
    }
    // datepicker
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

            if (!checkUser(teacher.id)) {
                teacherIds.push(teacher.id);
                $("#teachers").append("<div> <div class='list-group-item' id=" + teacher.id + ">" + teacher.name + "</div> <input name='TeacherIds' hidden value=" + teacher.id + "></input> </div > ");
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
                        var teacherIdx = findUser(id);
                        if (teacherIdx != null) {
                            teacherIds.splice(teacherIdx, 1);
                        }
                        $(box).parent('div').remove();
                    }
                }
            }
        });
    });
});