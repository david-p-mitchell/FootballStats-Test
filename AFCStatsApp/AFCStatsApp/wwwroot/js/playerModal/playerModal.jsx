const { useState, useEffect } = React;

function PlayerModal({ show, handleClose, onSave, initialData, positions }) {
    const [player, setPlayer] = useState({
        playerId: 0,
        playerName: "",
        position: "",
        jerseyNumber: 0,
        goalsScored: 0
    });

    useEffect(() => {
        if (initialData) setPlayer(initialData);
        else setPlayer({ playerId: 0, playerName: "", position: "", jerseyNumber: 0, goalsScored: 0 });
    }, [initialData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setPlayer(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        await onSave(player);
    };

    if (!show) return null;

    return (
        <div className="modal show d-block" tabIndex="-1">
            <div className="modal-dialog">
                <form onSubmit={handleSubmit} className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{player.playerId ? "Edit" : "Add"} Player</h5>
                        <button type="button" className="btn-close" onClick={handleClose}></button>
                    </div>
                    <div className="modal-body">
                        <input type="hidden" name="playerId" value={player.playerId} />
                        <div className="mb-3">
                            <label className="form-label">Player Name</label>
                            <input type="text" name="playerName" className="form-control" value={player.playerName} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Position</label>
                            <select name="position" className="form-select" value={player.position} onChange={handleChange} required>
                                <option value="">-- Select Position --</option>
                                {positions.map(pos => <option key={pos.value} value={pos.value}>{pos.text}</option>)}
                            </select>
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Jersey Number</label>
                            <input type="number" name="jerseyNumber" className="form-control" value={player.jerseyNumber} onChange={handleChange} required />
                        </div>
                        <div className="mb-3">
                            <label className="form-label">Goals Scored</label>
                            <input type="number" name="goalsScored" className="form-control" value={player.goalsScored} onChange={handleChange} />
                        </div>
                    </div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" onClick={handleClose}>Close</button>
                        <button type="submit" className="btn btn-primary">Save</button>
                    </div>
                </form>
            </div>
        </div>
    );
}

// Global function to open modal
window.openPlayerModal = function (initialData) {
    const positions = [
        { value: 'Goalkeeper', text: 'Goalkeeper' },
        { value: 'Defender', text: 'Defender' },
        { value: 'Midfielder', text: 'Midfielder' },
        { value: 'Forward', text: 'Forward' }
    ];

    function handleClose() {
        ReactDOM.render(<div></div>, document.getElementById('playerModalRoot'));
    }

    function getAntiForgeryToken() {
        return document.querySelector('input[name="__RequestVerificationToken"]').value;
    }

    async function handleSave(player) {
        const isNew = player.playerId == 0;
        const url = isNew ? '/api/players/add' : '/api/players/update'; // adjust update endpoint if needed
        const method = 'POST';
        const successMessage = isNew ? 'Player Added' : 'Player Updated';
        console.log(player);
        try {
            const res = await fetch(url, {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': getAntiForgeryToken()
                },
                body: JSON.stringify(player)
            });

            let data;
            try {
                data = await res.json(); // try to parse JSON
            } catch (err) {
                // If parsing fails (empty or invalid JSON), fallback to text
                const text = await res.text();
                data = { errors: text || 'Unknown error' };
            }

            // Handle success
            if (data.success) {
                showToast(successMessage, 'success', 3000);
                handleClose();
            } else {
                showToast(data.errors || 'Failed to save player', 'error', 3000);
            }

        } catch (err) {
            console.error('Error saving player:', err);
            showToast('Error saving player', 'error', 3000);
        }
    }

    ReactDOM.render(
        <PlayerModal show={true} handleClose={handleClose} onSave={handleSave} positions={positions} initialData={initialData} />,
        document.getElementById('playerModalRoot')
    );
}