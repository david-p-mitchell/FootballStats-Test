window.playerHelpers = {
    getEmptyPlayer: function () {
        return { playerId: 0, playerName: "", position: "", jerseyNumber: 0, goalsScored: 0 };
    },
    validateJerseyNumber: function (num, allPlayers, currentPlayerId) {
        const exists = allPlayers.find(p => p.jerseyNumber === num && p.playerId !== currentPlayerId);
        if (!Number.isInteger(num) || num <= 0 || num >= 100) return "Jersey number must be 1-99";
        if (exists) return `${exists.playerName} already has that number`;
        return "";
    }
};
