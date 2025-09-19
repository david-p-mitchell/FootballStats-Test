let playerActions = {};
playerActions.addPlayer = () => {
    openPlayerModal();
}

playerActions.editPlayer = (rowElement) => {
    const player = playersDataTable.row($(rowElement).closest('tr')).data();
    openPlayerModal(player);
};

playerActions.deletePlayer = async (rowElement) => {
    if (confirm("Are you sure you want to delete this player?")) {
        const player = playersDataTable.row($(rowElement).closest('tr')).data();
        console.log(player.playerId);
        await playerApi.delete(player.playerId);
        showToast('Player removed', 'success', 3000);
        playersDataTable.ajax.reload();
    }
};

// Wire up buttons
$('#playersTable tbody').on('click', '.edit-btn', function () {
    playerActions.editPlayer(this);
});

$('#playersTable tbody').on('click', '.delete-btn', async function () {
    await playerActions.deletePlayer(this);
});