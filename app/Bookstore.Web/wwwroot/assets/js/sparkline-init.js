var Script = function () {

    $("#sparkline_1").sparkline([87, 109, 111, 95, 120, 99, 87, 100, 67, 75, 65, 87], {
        type: 'bar',
        height: '32',
        barWidth: 5,
        barSpacing: 2,
        barColor: '#36a2f5'
    });

    $("#sparkline_2").sparkline([102, 109, 90, 120, 70, 99, 110, 80, 87, 50, 65, 74], {
        type: 'bar',
        height: '32',
        barWidth: 5,
        barSpacing: 2,
        barColor: '#A768F3'
    });


    $("#sparkline_3").sparkline([99, 110, 80, 102, 109, 120, 87, 74, 112, 54, 76, 90, 43], {
        type: 'bar',
        height: '32',
        barWidth: 5,
        barSpacing: 2,
        barColor: '#FF518A'
    });


}();
