
const { useState, useEffect } = React;

function PlayerModal({ show, onClose, playerModel, playerApi, playersDataTable }) {
    
    const [player, setPlayer] = useState(playerModel || {});
    const [errors, setErrors] = useState({});
    const [allPlayers, setAllPlayers] = useState([]);

    useEffect(() => {
        setPlayer(playerModel || { playerId: 0, playerName: "", position: "", jerseyNumber: 0, goalsScored: 0 });
        setErrors({});
    }, [playerModel, show]);

    useEffect(() => {
        fetch('/api/players/getAll')
            .then(res => res.json())
            .then(data => setAllPlayers(data))
            .catch(console.error);
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setPlayer(prev => ({ ...prev, [name]: value }));
        if (name === "playerName") setErrors(prev => ({ ...prev, playerName: value ? '' : 'Player must have a name' }));
        if (name === "jerseyNumber") validateJerseyNumber(value);
    };

    const validateJerseyNumber = (value) => {
        const num = Number(value);
        if (!Number.isInteger(num) || num <= 0 || num >= 100) {
            setErrors(prev => ({ ...prev, jerseyNumber: "Jersey number must be 1-99" }));
            return;
        }
        const exists = allPlayers.find(p => p.jerseyNumber === num && p.playerId !== player.playerId);
        setErrors(prev => ({ ...prev, jerseyNumber: exists ? `${exists.playerName} already has that number` : '' }));
    };

    const submitDisabled = Object.values(errors).some(err => err) || !player.playerName || player.jerseyNumber <= 0 || player.jerseyNumber >= 100;

    const handleSubmit = async (e) => {
        e.preventDefault();
        const isNew = player.playerId === 0;
        
        try {
            let data;
            if (isNew) {
                data = await playerApi.add(player);
            }
            else {
                data = await playerApi.update(player);
            }

            if (data.success) {
                showToast(isNew ? 'Player Added' : 'Player Updated', 'success', 3000);
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
                        <h5 className="modal-title">{player.playerId ? "Edit" : "Add"} Player</h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">
                        <input type="hidden" name="playerId" value={player.playerId} />
                        <div className="mb-3">
                            <label className="form-label">Player Name</label>
                            <input type="text" name="playerName" className="form-control" value={player.playerName} onChange={handleChange} />
                            {errors.playerName && <p className="text-danger mt-2">{errors.playerName}</p>}
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Position</label>
                            <select name="position" className="form-select" value={player.position} onChange={handleChange}>
                                <option value="">-- Select Position --</option>
                                {['Goalkeeper', 'Defender', 'Midfielder', 'Forward'].map(p => <option key={p} value={p}>{p}</option>)}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Jersey Number</label>
                            <input type="number" name="jerseyNumber" className="form-control" value={player.jerseyNumber} onChange={handleChange} max="99" />
                            {errors.jerseyNumber && <p className="text-danger mt-2">{errors.jerseyNumber}</p>}
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Goals Scored</label>
                            <input type="number" name="goalsScored" className="form-control" value={player.goalsScored} onChange={handleChange} />
                        </div>
                    </div>
                    <div className="modal-footer">
                        <button type="submit" className="btn btn-primary" disabled={submitDisabled}>Save</button>
                        <button type="button" className="btn btn-secondary" onClick={onClose}>Close</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

const modalRoot = document.getElementById('playerModalRoot');
const modalRootInstance = ReactDOM.createRoot(modalRoot);
let modalState = { show: false, playerModel: null };

function renderModal() {
    modalRootInstance.render(
        <PlayerModal
            show={modalState.show}
            playerModel={modalState.playerModel}
            onClose={() => { modalState.show = false; renderModal(); }}
            playerApi={playerApi}
            playersDataTable={playersDataTable }
        />
    );
}

window.openPlayerModal = function (playerModel = null) {
    modalState = { show: true, playerModel };
    renderModal();
};

renderModal();
