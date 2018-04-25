$(document).ready(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var $pagination = $('#pagination');
    var $spinner = $('.spinner');
    var $table = $('tbody');
    var $tbody = $('tbody');
    var defaultOpts = { totalPages: 1 };
    var query = { page: 1 , isApproved: true};
    $pagination.twbsPagination(defaultOpts);
    loadItems();

    function loadItems() {
        $table.hide();
        $spinner.show();
        $.ajax({
            url: '/api/admin/magazines',
            data: query,
            success: function (data) {
                $spinner.hide();
                $table.show();
                $tbody.empty();
                var datax = '';
                $.each(data.items, function (key, value) {
                datax +=
                `<tr>
                    <td>${value.title}</td>
                    <td>${value.issn}</td>
                    <td>${value.index}</td>
                    <td>${value.edition}</td>
                    <td>${value.editionDate}</td>
                    <td>${value.editor}</td>
                    <td>`;
                    $.each(value.teacherMagazines, function(key, value){
                        datax +='<p>' + value.name + '</p>';
                    });
                    datax +='</td>';
                    datax += '<td> <a href="'+ value.magazinePath +'">Descargar</a></td>';
                    datax += '<td>';
                                if(value.isApproved === true)
                                    datax += '<span class="label label-success">Aprobado</span>';
                                else
                                    datax += '<span class="label label-warning">No aprobado</span>';
                    datax += '</td>';
                    datax += `<td>
                        <a href="/Magazine/Edit/${value.id}" class="btn btn-sm btn-info"
                        data-toggle="tooltip" title="Editar"><i class="fa fa-pencil" aria-hidden="true"></i></a>

                        <form action="/Magazine/Delete/${value.id}" method="post" class="js-delete inline">
                            <button type="submit" class="btn btn-sm btn-danger" data-toggle="tooltip" title="Eliminar">
                                <i class="fa fa-trash-o" aria-hidden="true"></i>
                            </button>
                            <input name="__RequestVerificationToken" type="hidden" value="${token}">
                        </form>
                        <form action="/Magazine/IsApproved/${value.id}" method="post" class="js-approved inline">
                            <button type="submit" class="btn btn-sm btn-warning" data-toggle="tooltip" title="No aprobar">
                                <i class="fa fa-exchange" aria-hidden="true"></i>
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
    function search(){
        var searchTerm = $('#search-box').val();
        query.page = 1;
        query.searchTerm = searchTerm;
        loadItems();
    };
    $('#search-box').keypress(function (e) {
       if(e.keyCode == '13') 
       search();
    });
    $('#search-btn').click(function () {
       search();
    });
    $('#isApproved').change(function(){
        query.page = 1;
        query.isApproved = $(this).prop("checked");
        loadItems();
    });
});