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
            url: '/api/teacher/articles',
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
                    <td>${value.magazine}</td>
                    <td>${value.editionDate}</td>
                    <td>${value.issn}</td>
                    <td>`;
                    $.each(value.teacherArticles, function(key, value){
                        datax +='<p>' + value.name + '</p>';
                    });
                    datax +='</td>';
                    datax += `<td>
                        <a href="/Article/Edit/${value.id}" class="btn btn-sm btn-info"
                        data-toggle="tooltip" title="Editar"><i class="fa fa-pencil" aria-hidden="true"></i></a>
                        <a href="${value.articlePath}" class="btn btn-sm btn-basic" data-toggle="tooltip" title="Ver"><i class="fa fa-eye" aria-hidden="true"></i></a>                        
                        <form action="/Article/Delete/${value.id}" method="post" class="js-delete inline">
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
                    first: 'Primera',
                    last: 'Última',
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