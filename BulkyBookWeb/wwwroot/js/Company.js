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
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { data: "name", width: "15%" },
            { data: "streetAddress", width: "15%" },
            { data: "city", width: "15%" },
            { data: "state", width: "15%" },
            { data: "phoneNumber", width: "15%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                    <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i>Delete</a>
                    </div >`


                },
                width: "25%"
            },s

        ]

    });
}
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type : 'DELETE' ,
                success: function (data) {
                    // data is the response from the controller from the delete api call in json format
                    datatable.ajax.reload();
                    //toastr.success(data.message);
                }
            })
            Swal.fire(
                'Deleted!',
                'Your file has been deleted.',
                'success'
            )
        }
    })
}