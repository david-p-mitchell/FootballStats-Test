let matchesDataTable;
let retryCount = 0;
const maxRetries = 3;

$(function () {

    // Helper function: format date as "Wed 3rd July"
    function formatMatchDate(dateString) {
        const date = new Date(dateString);

        const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        const months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

        const dayName = days[date.getDay()];
        const dayNum = date.getDate();
        const monthName = months[date.getMonth()];

        const ordinal = (n) => {
            if (n > 3 && n < 21) return 'th';
            switch (n % 10) {
                case 1: return 'st';
                case 2: return 'nd';
                case 3: return 'rd';
                default: return 'th';
            }
        };

        return `${dayName} ${dayNum}${ordinal(dayNum)} ${monthName}`;
    }

    // Helper function: format time as "2:30 PM"
    function formatMatchTime(dateString, use12Hour = true) {
        const date = new Date(dateString);
        let hours = date.getHours();
        const minutes = date.getMinutes().toString().padStart(2, '0');

        if (use12Hour) {
            const ampm = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12 || 12;
            return `${hours}:${minutes} ${ampm}`;
        } else {
            return `${hours.toString().padStart(2, '0')}:${minutes}`;
        }
    }

    matchesDataTable = $('#matchesTable').DataTable({
        headerCallback: function (thead, data, start, end, display) {
            $(thead).remove(); // removes thead entirely
        },
        ajax: {
            url: 'api/matches/teams/57',
            dataSrc: '',
            error: function (xhr, status, error) {
                if (xhr.status === 500 && retryCount < maxRetries) {
                    retryCount++;
                    showToast('Failed to load matches data. Retrying', 'warning', 3000);
                    setTimeout(() => {
                        matchesDataTable.ajax.reload(null, false); // false keeps current paging
                    }, 1000);
                } else if (retryCount >= maxRetries) {
                    showToast(`Failed to load matches data after ${retryCount} retries.`, 'error', 3000);
                }
            }
        },
        columns: [
            {
                width: '75px',
                data: 'homeTeam.crest',

                orderable: false,
                render: (data) => `<img src=${data} style="height:75px;" />`
            },
            {
                data: 'homeTeam.name',
                orderable: false,
                className: 'dt-center v-center',
                render: (data) => `<p>${data}</p>`
            },
            {
                width:'200px',
                data: null,
                orderable: false,
                render: (data) => {
                    if (data.status === "FINISHED") {
                        return `<div style="text-align:center; " class="team-img">
                    <img src="${data.area.flag}" style="height:20px;" />
                    <div>
                        <p style="font-size:0.95em;">${data.score.fullTime.home} - ${data.score.fullTime.away}</p>
                        <p style="margin-bottom: 0;">HT:</p>
                        <p style="font-size:0.65em;">${data.score.halfTime.home} - ${data.score.halfTime.away}</p>
                    </div>
                </div>`;
                    } else {
                        const dateStr = formatMatchDate(data.utcDate);
                        const timeStr = formatMatchTime(data.utcDate);
                        return `<div style="text-align:center; " class="team-img">
                    <img src="${data.area.flag}" style="height:20px;" />
                    <div>
                        <p>${dateStr}</p>
                        <p>${timeStr}</p>
                    </div>
                </div>`;
                    }
                }
            },
            {
                data: 'awayTeam.name',
                orderable: false,
                className: 'dt-center v-center',
                render: (data) => `<p>${data}</p>`
            },
            {
                width: '75px',
                data: 'awayTeam.crest',
                orderable: false,
                render: (data) => `<img src=${data} style="height:75px;" />`
            },
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
