$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var $pagination = $('#pagination');
    var $spinner = $('.spinner');
    var $table = $('tbody');
    var $tbody = $('tbody');
    var defaultOpts = { totalPages: 1 };
    var query = { page: 1, isApproved: true };
    $pagination.twbsPagination(defaultOpts);
    loadItems();

    function loadItems() {
        $table.hide();
        $spinner.show();
        $.ajax({
            url: '/api/teacher/research',
            data: query,
            success: function (data) {
                $spinner.hide();
                $table.show();
                $tbody.empty();
                var datax = '';
                $.each(data.items, function (key, value) {
                    datax +=
                        `<tr>
                    <td>${value.code}</td>
                    <td>${value.title}</td>
                    <td>${value.knowledgeArea.name}</td>
                    <td>${value.researchType}</td>
                    <td>${value.sector}</td>
                    <td>${value.researchDuration}</td>
                    <td>${value.academicBody.name}</td>
                    <td>${value.researchLine.name}</td>                    
                    <td>`;
                    $.each(value.teacherResearch, function (key, value) {
                        datax += '<p>' + value.name + '</p>';
                    });
                    datax += '</td>';
                    datax += '<td> <a href="' + value.researchPath + '">Descargar</a></td>';
                    datax += '<td>';
                    if (value.isApproved === true)
                        datax += '<span class="label label-success">Aprobado</span>';
                    else
                        datax += '<span class="label label-warning">No aprobado</span>';
                    datax += '</td>';
                    datax += `<td>
                        <a href="/Research/Edit/${value.id}" class="btn btn-sm btn-info"
                        data-toggle="tooltip" title="Editar"><i class="fa fa-pencil" aria-hidden="true"></i></a>

                        <form action="/Research/Delete/${value.id}" method="post" class="js-delete inline">
                            <button type="submit" class="btn btn-sm btn-danger" data-toggle="tooltip" title="Eliminar">
                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                            </button>
                            <input name="__RequestVerificationToken" type="hidden" value="${token}">
                        </form>
                        </td>`;
                    datax += '</tr>';
                });
                $tbody.append(datax);

                var totalPages = Math.ceil(data.totalItems / 10);
                var currentPage = query.page;
                $pagination.twbsPagination('destroy');
                $pagination.twbsPagination($.extend({}, defaultOpts, {
                    startPage: currentPage,
                    totalPages: totalPages,
                    first: 'primera',
                    last: 'ultima',
                    prev: '<span aria-hidden="true">&laquo;</span>',
                    next: '<span aria-hidden="true">&raquo;</span>',
                    initiateStartPageClick: false,
                    onPageClick: function (event, page) {
                        query.page = page;
                        loadItems();
                    }
                }));
            }
        });
    };
    function search() {
        var searchTerm = $('#search-box').val();
        query.page = 1;
        query.searchTerm = searchTerm;
        loadItems();
    };
    $('#search-box').keypress(function (e) {
        if (e.keyCode == '13')
            search();
    });
    $('#search-btn').click(function () {
        search();
    });
    $('#isApproved').change(function () {
        query.page = 1;
        query.isApproved = $(this).prop("checked");
        loadItems();
    });
    $.ajax({
        url: 'api/academicBodies',
        success: function (data) {
            $.each(data, function (index, item) {
                $('#academicBodyId').append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        }
    });
    $.ajax({
        url: 'api/researchLines',
        success: function (data) {
            $.each(data, function (index, item) {
                $('#researchLineId').append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        }
    });
    $.ajax({
        url: 'api/knowledgeAreas',
        success: function (data) {
            $.each(data, function (index, item) {
                $('#knowledgeAreaId').append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        }
    });
    $('#academicBodyId').change(function () {
        query.page = 1;
        query.educationProgramId = $(this).val();
        loadItems();
    });
    $('#researchLineId').change(function () {
        query.page = 1;
        query.researchLineId = $(this).val();
        loadItems();
    });
    $('#knowledgeAreaId').change(function () {
        query.page = 1;
        query.knowledgeAreaId = $(this).val();
        loadItems();
    });
});