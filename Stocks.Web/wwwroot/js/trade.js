// Setup WebHook
function setupWebhook(symbol) { 
    fetch("/api/finnhub/token").then(response => response.text()).then(token => {
        const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
        socket.addEventListener('open', (event) => {
            socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': symbol }))
        });

        socket.addEventListener('message', (event) => {
            return; //disabled for the time being
            if (event.data.type == "error") {
                $("#price").text(event.data.msg);
                return;
            }
            const data = JSON.parse(event.data);
            if (data.type === 'trade') {
                console.log(data.data);
                const price = data.data[0].p;
                const timeStamp = data.data[0].t;

                $("#price").text(price.toFixed(2)); //mini display price
                $(".current-price").text("$" + price.toFixed(2)); //mini display price
                // TODO: Handle percentage change and daily gain/loss
            }
        });
        window.addEventListener('beforeunload', (event) => {
            socket.close();
        });
    });
}