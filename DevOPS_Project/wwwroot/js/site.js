// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var dataTable;

$(document).ready(function () {

    if (window.location.pathname == "/Buildings" || window.location.pathname == "/Buildings/" || window.location.pathname == "/Buildings/Index" ) {
        loadDataTable();
    }
    else if (window.location.pathname == "/Users" || window.location.pathname == "/Users/" || window.location.pathname == "/Users/Index") {
        loadDataTableU();
    }
    else {
        loadDataTableR();
    }

})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Buildings/GetData"
        },
        "columns": [
            { "data": "buildingID", "width": "10%" },
            { "data": "buildingName", "width": "25%" },
            { "data": "syscreatedDt", "width": "25%" },
            {"data":"actions","width":"40%"}
        ]
    });

}
function loadDataTableU() {

    
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Users/GetData"
        },
        "columns": [
            { "data": "userID", "width": "10%" },
            { "data": "userName", "width": "25%" },
            { "data": "syscreatedDt", "width": "25%" },
            { "data": "actions", "width": "30%" }
        ]
    });

}
function loadDataTableR() {

    var id = $('#BId').val();
    

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Rooms/GetData/" + id
        },
        "columns": [
            { "data": "roomID", "width": "10%" },
            { "data": "isTR", "width": "20%" },
            { "data": "roomName", "width": "20%" },            
            { "data": "syscreatedDt", "width": "20%" },
            { "data": "actions", "width": "30%" }
        ]
    });

}