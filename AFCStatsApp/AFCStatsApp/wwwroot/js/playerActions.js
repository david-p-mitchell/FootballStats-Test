let playerActions = {};
playerActions.addPlayer = () => {
    openPlayerModal();
}

playerActions.editPlayer = (rowElement) => {
    const player = playersDataTable.row($(rowElement).closest('tr')).data();
    openPlayerModal(player);
};

playerActions.deletePlayer = async (rowElement) => {
    const player = playersDataTable.row($(rowElement).closest('tr')).data();
    openDeletePlayerModal(player);
};

// Wire up buttons
$('#playersTable tbody').on('click', '.edit-btn', function () {
    playerActions.editPlayer(this);
});

$('#playersTable tbody').on('click', '.delete-btn', async function () {
    await playerActions.deletePlayer(this);
});