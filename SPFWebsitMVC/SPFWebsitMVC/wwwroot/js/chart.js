// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function drawChart() {
    var data = new google.visualization.DataTable();
    data.addColumn('number', 'Day');
    data.addColumn('number', 'Weight');

    $.ajax({
        url: 'https://localhost:44391/api/DayWeights/GetChartDataByClientId/' + $('#chartClientId').val(),
        dataType: 'json',
        type: 'get',
        contentType: 'application/json',
        success: function (results) {
            results.forEach(function (item, index) {
                var dict = {
                    Id: item.dayWeightId,
                    Day: item.day,
                    Weight: item.weight,
                    ClientId: item.clientId,
                    Client: item.client
                };
                data.addRow([dict.Day, dict.Weight]);
            });
        },
        error: function (jqXhr, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

    var options = {
        chart: {
            title: 'Weight',
            subtitle: 'in lbs'
        },
        width: 900,
        height: 500
    };

    var chart = new google.charts.Line(document.getElementById('linechart_material'));

    chart.draw(data, google.charts.Line.convertOptions(options));
}
