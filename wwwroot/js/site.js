// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready( function () {
    $('#gamesTable').DataTable({
        pageLength: 100
        // https://datatables.net/reference/option/
    });

    $("#btnExcel").click(function(e) {
        e.preventDefault();
        // var t = '@GetAntiXsrfRequestToken()';
        $.ajax({
                 url: "/Home/ExcelExport",
                //  headers:
                //  {
                //      "RequestVerificationToken": t
                //  },
                 type: "POST",
                //  data: { data: 'foo2' },
        }).done(function(data) {
                console.log(data);
        }).fail(function(a, v, e) {
                alert(e);
        });
    });
} );