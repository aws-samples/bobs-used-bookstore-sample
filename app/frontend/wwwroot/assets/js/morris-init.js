
// area chart

Morris.Area({
    element: 'area-chart',
    behaveLikeLine: true,
    gridEnabled: false,
    gridLineColor: '#e5ebf8',
    axes: true,
    fillOpacity:.7,
    data: [
        {period: '2015 Q1', iphone: 2666, ipad: null, itouch: 2647},
        {period: '2015 Q2', iphone: 15278, ipad: 4294, itouch: 2441},
        {period: '2015 Q3', iphone: 4912, ipad: 1969, itouch: 2501},
        {period: '2015 Q4', iphone: 3767, ipad: 3597, itouch: 5689},
        {period: '2016 Q1', iphone: 6810, ipad: 13914, itouch: 2293},
        {period: '2016 Q2', iphone: 5670, ipad: 4293, itouch: 1881},
        {period: '2016 Q3', iphone: 4820, ipad: 23795, itouch: 1588},
        {period: '2016 Q4', iphone: 15073, ipad: 5967, itouch: 5175},
        {period: '2017 Q1', iphone: 10687, ipad: 4460, itouch: 2028},
        {period: '2017 Q2', iphone: 8432, ipad: 5713, itouch: 1791}
    ],
    lineColors:['#FF518A','#FFEA80','#36a2f5'],
    xkey: 'period',
    ykeys: ['iphone', 'ipad', 'itouch'],
    labels: ['iPhone', 'iPad', 'iPod Touch'],
    pointSize: 4,
    lineWidth: 1,
    hideHover: 'auto'

});



// bar chart

Morris.Bar({
    element: 'bar-chart',
    data: [
        {x: '2015 Q1', y: 2, z: 4, a: 3},
        {x: '2015 Q2', y: 2, z: null, a: 1},
        {x: '2015 Q3', y: 0, z: 2, a: 4},
        {x: '2015 Q4', y: 2, z: 4, a: 3}
    ],
    xkey: 'x',
    ykeys: ['y', 'z', 'a'],
    labels: ['Y', 'Z', 'A'],
    gridLineColor: '#e5ebf8',
    barColors:['#36a2f5','#A768F3','#eac459']

});


var day_data = [
    {"elapsed": "I", "value": 8},
    {"elapsed": "II", "value": 34},
    {"elapsed": "III", "value": 53},
    {"elapsed": "IV", "value": 12},
    {"elapsed": "V", "value": 13},
    {"elapsed": "VI", "value": 22},
    {"elapsed": "VII", "value": 5},
    {"elapsed": "VIII", "value": 26},
    {"elapsed": "IX", "value": 12},
    {"elapsed": "X", "value": 19}
];

// line chart

Morris.Line({
    element: 'line-chart',
    data: day_data,
    xkey: 'elapsed',
    ykeys: ['value'],
    labels: ['value'],
    gridLineColor: '#e5ebf8',
    lineColors:['#FF518A'],
    parseTime: false,
    lineWidth: 1
});


// area line chart

Morris.Area({
    element: 'area-line-chart',
    behaveLikeLine: false,
    data: [
        {x: '2017 Q1', y: 5, z: 3},
        {x: '2017 Q2', y: 3, z: 3},
        {x: '2017 Q3', y: 5, z: 5},
        {x: '2017 Q4', y: 4, z: 3},
        {x: '2017 Q5', y: 3, z: 2}
    ],
    xkey: 'x',
    ykeys: ['y', 'z'],
    labels: ['Y', 'Z'],
    gridLineColor: '#e5ebf8',
    lineColors:['#eac459','#A768F3'],
    pointSize: 4,
    lineWidth: 1

});


// donut chart

Morris.Donut({
    element: 'donut-chart',
    data: [
        {value: 60, label: 'Apple', formatted: 'at least 55%' },
        {value: 25, label: 'Orange', formatted: 'approx. 25%' },
        {value: 5, label: 'Banana', formatted: 'approx. 10%' },
        {value: 10, label: 'Long title chart', formatted: 'at most 10%' }
    ],
    backgroundColor: '#fff',
    labelColor: '#53505F',
    gridLineColor: '#e5ebf8',
    colors: [
        '#A768F3','#36a2f5','#34bfa3','#eac459'
    ],
    formatter: function (x, data) { return data.formatted; }
});



