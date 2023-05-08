function LoadPopularStocks(url, containerId) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            document.getElementById(containerId).innerHTML = result;
        }
    });
}

function drawChart(dataArray,containerId) {
    var data = google.visualization.arrayToDataTable(dataArray);

    var options = {
        backgroundColor: 'transparent',
        legend: 'none',
        width: 600,
        height: 400,
        pieSliceText: 'percentage',
        colors: ['#0598d8', '#f97263'],
        chartArea: {
            left: "3%",
            top: "3%",
            height: "94%"
        }
    };

    var chart = new google.visualization.PieChart(document.getElementById(containerId));
    chart.draw(data, options);
}

function LoadOrderDetails(parentClass,bodyClass,symbol,url) {
    var body = document.getElementsByClassName(bodyClass)[0];
    body.innerHTML = `
                            <div class="spinner-border" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
                             `
    var modal = document.getElementsByClassName(parentClass)[0];
    modal.style.display = "block";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: symbol },
        success: function (result) {
            body.innerHTML = result;
        }
    });
}