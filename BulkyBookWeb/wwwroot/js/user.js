var datatable;
$(document).ready(function () {
    loadDataTable();
})
/* jevha html pagen fuly load hoto tevha loadDataTable() function call hoto 
$(document).ready(...) is waiting for the DOM to be ready, 
and then it's calling the loadDataTable() function, 
presumably to initialize and load data into a DataTable component or similar functionality

By using $(document).ready(), you ensure that 
your code runs at the appropriate time, after the DOM is ready for manipulation.*/
//var datatable;
function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { "data": "name", width: "15%" },
            { "data": "email", width: "15%" },
            { "data": "phoneNumber", width: "15%" },
            { "data": "company.name", width: "15%" },
            { "data": "role", width: "15%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/user/edit?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>`


                },
                width: "25%"
            },

        ]

    });
}
