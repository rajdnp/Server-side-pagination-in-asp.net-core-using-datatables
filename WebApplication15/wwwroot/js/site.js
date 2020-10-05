$(document).ready(function () {
    $("#sampleDataTable").DataTable({
        "ajax": {
            "url": "/Home/GetData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "name": "id" },
            { "data": "email", "name": "email" },
            { "data": "sendgridEvent", "name": "sendgridEvent" }
        ],

        "serverSide": "true",
        "pageLength": 10,
        "scrollX": true,
        "rowReorder": true,
        "order": [0, "desc"],
        "displayStart": 0,
        "processing": "true",
        "language": {
            "processing": "processing... please wait"
        }

    });
});