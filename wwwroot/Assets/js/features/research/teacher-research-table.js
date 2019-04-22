$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var pagination = $('#pagination');
    var dropdownActions = $('.actions');
    var spinner = $('.spinner');
    var table = $('tbody');
    var tbody = $('tbody');
    var defaultOpts = { totalPages: 1 };
    var query = { page: 1, isApproved: true };
    pagination.twbsPagination(defaultOpts);
    loadItems();

    function loadItems() {
        table.hide();
        spinner.show();
        $.ajax({
            url: '/api/teacher/research',
            data: query,
            success: function (data) {
                spinner.hide();
                table.show();
                tbody.empty();
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
                    <td class="id" hidden>${value.id}</td>
                    <td class="path" hidden>${value.researchPath}</td> 
                    <td>`;
                    $.each(value.teacherResearch, function (key, value) {
                        datax += '<div>' + value.name + '</div>';
                    });
                    datax += '</td>';
                    datax += '</tr>';
                });
                tbody.append(datax);

                var totalPages = Math.ceil(data.totalItems / 10);
                var currentPage = query.page;
                pagination.twbsPagination('destroy');
                pagination.twbsPagination($.extend({}, defaultOpts, {
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

    $(tbody).on('click', 'tr', function () {
        var id = $(this).find('.id').text();
        var path = $(this).find('.path').text();
        $('tr').removeClass('success');
        $(this).addClass('success');
        $.ajax({
            url: `/api/teacher/research/${id}/role`,
            success: function (role) {
                dropdownActions.empty();
                console.log(role);
                var authorized = (role == 'WRITTER') ? "none" : "disabled-item";
                var existPath = (path != 'null') ? "none" : "disabled-item";
                console.log(path);
                data = `
                <li class="${authorized}"><a href="/Research/Edit/${id}">Editar</a></li>
                <li class="${authorized}"><a href="/Research/Upload/${id}">Añadir PDF</a></li>
                <li class="${existPath}"><a target="_blank" href="${path}">Descargar</a></li>
                    <li class="${authorized}">
                        <form action="/Research/Delete/${id}" method="post" class="js-delete">
                            <button class="btn-block" type="submit">Eliminar</button>
                            <input name="__RequestVerificationToken" type="hidden" value="${token}">
                        </form>
                    </li>`

                dropdownActions.append(data);
                $(".disabled-item").remove();
            }
        });
    });

    $('#search-box').keypress(function (e) {
        if (e.keyCode == '13')
            search();
    });
    $('#search-btn').click(function () {
        search();
    });
    
    $("#isApproved").prop("checked", true);
    $('#isApproved').change(function () {
        query.page = 1;
        query.isApproved = $(this).prop("checked");
        loadItems();
        dropdownActions.empty();
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