$(document).ready(function () {
    $(function () {
        $("#AcademicBodyId").ready(function () {
            $('#ResearchLineId').empty();
            $.ajax({
                type: "POST",
                url: 'api/Research/GetResearchLines',
                datatype: "Json",
                data: {
                    id: $('#AcademicBodyId').val()
                },
                success: function (data) {
                    $.each(data, function (index, item) {
                        var id = $(".ResearchLineId").attr('id');
                        if (item.id == id) {
                            $('#ResearchLineId').append('<option value="' + item.id + '" selected>' + item.name + '</option>');
                        }
                        else {
                            $('#ResearchLineId').append('<option value="' + item.id + '" >' + item.name + '</option>');
                        }
                    });
                }
            });
        });

        $("#AcademicBodyId").change(function () {
            $('#ResearchLineId').empty();
            $.ajax({
                type: "POST",
                url: 'api/Research/GetResearchLines',
                datatype: "Json",
                data: { id: $('#AcademicBodyId').val() },
                success: function (data) {
                    $.each(data, function (index, item) {
                        $('#ResearchLineId').append('<option value="' + item.id + '">' + item.name + '</option>');
                    });
                }
            });
        });
    });
});