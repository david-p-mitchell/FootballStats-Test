let playersDataTable;
let retryCount = 0;
const maxRetries = 3;
$(function () {
    playersDataTable = $('#playersTable').DataTable({
        ajax: {
            url: 'api/players/getAll',
            dataSrc: '',
            error: function (xhr, status, error) {
                if (xhr.status === 500 && retryCount < maxRetries) {
                    retryCount++;
                    showToast('Failed to load players data. Retrying', 'warning', 3000);
                    setTimeout(() => {
                        playersDataTable.ajax.reload(null, false); // false keeps current paging
                    }, 1000);
                } else if (retryCount >= maxRetries){
                    showToast(`Failed to load players data after ${retryCount} retries.`,'error', 3000);
                }
            }
        },
        columns: [
            { data: 'playerName', orderable:false },
            { data: 'position', orderable: false },
            { data: 'jerseyNumber', orderable: false },
            { data: 'goalsScored' },
            {
                data: null,
                render: (data) =>
                `
                    <button class="btn btn-primary edit-btn">Edit</button>
                    <button class="btn btn-danger delete-btn">Delete</button>
                `,
                orderable: false
            }
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


