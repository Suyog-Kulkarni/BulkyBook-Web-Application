var datatable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        loadDataTable("all");
    }
})
/* jevha html pagen fuly load hoto tevha loadDataTable() function call hoto 
$(document).ready(...) is waiting for the DOM to be ready, 
and then it's calling the loadDataTable() function, 
presumably to initialize and load data into a DataTable component or similar functionality

By using $(document).ready(), you ensure that 
your code runs at the appropriate time, after the DOM is ready for manipulation.*/
//var datatable;
function loadDataTable(status) {
    datatable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { "data": "id", width: "5%" },
            { "data": "name", width: "20%" },
            { "data": "phoneNumber", width: "20%" },
            { "data": "applicationUser.email", width: "20%" },
            { "data": "orderStatus", width: "10%" },
            { "data": "orderTotal", width: "10%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div >`


                },
                width: "10%"
            },

        ]

    });
}

/*
    when the page is loaded, the loadDataTable() function is called, and the status parameter is passed to it. 
The status parameter is used to determine which orders to display in the table. 
The status parameter is passed to the getall action method in the OrderController.
initially the status parameter is set to all, so all orders are displayed in the table.
and then the loadDataTable() function is called again when the status changes.
the query string is displayed when the button is clicked and the status changes.

code is written in the loadDataTable() function to display the query string in the browser's address bar.
code is not working not sure why.
help me to fix this issue.
please help me to fix this issue.


*/