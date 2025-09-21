let playerApi = {};


async function request(url, player = null, method = 'POST') {
    try {
        const options = {
            method,
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        };

        if (player) {
            options.body = JSON.stringify(player);
        }

        const res = await fetch(url, options);

        if (!res.ok && method !== "DELETE") {
            const text = await res.text();
            throw new Error(`${method} request failed: ${res.status} - ${text}`);
        }

        return await res.json();
    } catch (err) {
        console.error(`Error in ${method} request:`, err);
        throw err;
    }
}
playerApi.getAll = async (url) => {
    return await request(url, null, 'GET');
};

// Define API methods
playerApi.add = async (player) => {
    return await request('/api/players/add', player, 'POST');
};

playerApi.update = async (player) => {
    return await request('/api/players/update', player, 'PUT');
};

playerApi.delete = async (playerId) => {
    const options = {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        }
    };
    await fetch(`/api/players/delete/${playerId}`, options);
};