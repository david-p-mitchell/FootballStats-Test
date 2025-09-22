const { useState, useEffect } = React;

function DeletePlayerModal({ show, onClose, playerModel, playerApi, playersDataTable }) {
    const [player, setPlayer] = useState(playerModel);

    useEffect(() => {
        setPlayer(playerModel);
    }, [playerModel, show]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log(player);
        try {
            let data = await playerApi.delete(player.playerId);
            
            if (data.success) {
                showToast('Player removed', 'success', 3000);
                onClose();
                if (playersDataTable) playersDataTable.ajax.reload();
            } else {
                showToast(data.errors || 'Failed to save player', 'error', 3000);
            }
        } catch (err) {
            console.error(err);
            showToast('Error saving player', 'error', 3000);
        }
    };

    if (!show) return null;

    return (
        <div className="modal show d-block" tabIndex="-1">
            <div className="modal-dialog">
                <form onSubmit={handleSubmit} className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">Delete Player</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">
                        <p> Are you sure you want to delete {" "}
                            {player ? player.playerName : "this player"}?</p>
                    </div>
                    <div className="modal-footer">
                        <button type="submit" className="btn btn-primary">Yes</button>
                        <button type="button" className="btn btn-secondary" onClick={onClose}>No</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// Root + state
const deletePlayermodalRoot = document.getElementById('deletePlayerModalRoot');
const deleteModalRootInstance = ReactDOM.createRoot(deletePlayermodalRoot);

let deletePlayerModalState = { show: false, playerModel: null };

function renderDeleteModal() {
    deleteModalRootInstance.render(
        <DeletePlayerModal
            show={deletePlayerModalState.show}
            playerModel={deletePlayerModalState.playerModel}
            onClose={() => {
                deletePlayerModalState.show = false;
                renderDeleteModal();
            }}
            playerApi={playerApi}
            playersDataTable={playersDataTable}
        />
    );
}

window.openDeletePlayerModal = function (playerModel = null) {
    console.log(playerModel);
    deletePlayerModalState = { show: true, playerModel };
    renderDeleteModal();
};

// initial render
renderDeleteModal();
