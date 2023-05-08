function loadDataTable(url,tradeUrl) {
    var dataTable = $('#tblData').DataTable({
        "ajax": { url: url },
        columns: [
            { data: 'stockSymbol', "width": "20%" },
            { data: 'stockName', "width": "45%" },
            {
                data: 'stockSymbol',
                "render": function (data) {
                    return `
                                                <div class="w-100 pt-2 btn-group" role="group">
                                                    <button onClick="showStockDetails('${data}')" class="btn btn-primary mx-2" style="cursor:pointer">
                                                                <i class="bi bi-info-circle me-2"></i> Details
                                                            </button>
                                                                    <a class="btn btn-primary mx-2" style="cursor:pointer" href="${tradeUrl}/${data}">
                                                                    <i class="bi bi-arrow-down-up me-2"></i> Trade
                                                    </a>
                                                </div>
                                            `;
                },
                "width": "35%"
            }
        ]
    });
}