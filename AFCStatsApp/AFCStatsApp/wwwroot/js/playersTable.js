$(document).ready(function () {
    $('#playersTable').DataTable({
        ajax: { url: '/Players/GetAll', dataSrc: '' },
        columns: [
            { data: 'playerName', orderable:false },
            { data: 'position', orderable: false },
            { data: 'jerseyNumber', orderable: false },
            { data: 'goalsScored' },
            { data: null, render: (data) => `<button class="btn btn-primary edit-btn">Edit</button>`, orderable: false }
        ],
        paging: false,
        scrollY: "600px",
        scrollCollapse: true, 
        searching: false,
        scrollable: true,
        ordering: true,
        order: []
    });
});
