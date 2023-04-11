const stockSymbol = document.getElementById("StockSymbol").value;

fetch("/api/finnhub/token").then(response => response.text()).then(token => {
    const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
    socket.addEventListener('open', (event) => {
        socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }));
    });

    socket.addEventListener('message', (event) => {
        if (event.data.type == "error") {
            $(".price").text(event.data.msg);
            return; 
        }
        const data = JSON.parse(event.data);
        if (data.type === 'trade') {
            const price = data.data[0].p;
            const timeStamp = data.data[0].t;

            $(".price").text(price.toFixed(2)); //big display price
            $("#price").val(price.toFixed(2)); //input hidden price
        }
    });
    window.addEventListener('beforeunload', (event) => {
        socket.close();
    });
});