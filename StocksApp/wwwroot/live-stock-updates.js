const stockSymbol = document.getElementById("StockSymbol").textContent;
const stockPrice = document.getElementById('StockPrice');

fetch("/api/finnhub/token").then(response => response.text()).then(token => {
    const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
    socket.addEventListener('open', (event) => {
        socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }));
    });

    socket.addEventListener('message', (event) => {
        const data = JSON.parse(event.data);
        if (data.type === 'trade') {
            const price = data.data[0].p;
            stockPrice.textContent = price.toFixed(2);
        }
    });
    window.addEventListener('beforeunload', (event) => {
        socket.close();
    });
});