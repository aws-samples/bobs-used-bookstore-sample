var Script = function () {

// easy pie chart

    $('.ep_1').easyPieChart({
        animate: 1000,
        size: 100,
        barColor:'#36a2f5'
    });

    $('.ep_2').easyPieChart({
        animate: 1000,
        size: 100,
        barColor:'#A768F3'
    });

    $('.ep_3').easyPieChart({
        animate: 1000,
        size: 100,
        barColor:'#FF518A'
    });

    $('.ep_4').easyPieChart({
        barColor: function(percent) {
            percent /= 100;
            return "rgb(" + Math.round(255 * (1-percent)) + ", " + Math.round(255 * percent) + ", 0)";
        },
        trackColor: '#eeeeee',
        barColor:'#A768F3',
        scaleColor: false,
        lineCap: 'butt',
        lineWidth: 5,
        animate: 1000,
        size: 100
    });

    $('.ep_5').easyPieChart({
        barColor: function(percent) {
            percent /= 100;
            return "rgb(" + Math.round(255 * (1-percent)) + ", " + Math.round(255 * percent) + ", 0)";
        },
        trackColor: '#eeeeee',
        barColor:'#36a2f5',
        scaleColor: false,
        lineCap: 'butt',
        lineWidth: 5,
        animate: 1000,
        size: 100
    });



    $('.ep_6').easyPieChart({
        barColor: function(percent) {
            percent /= 100;
            return "rgb(" + Math.round(255 * (1-percent)) + ", " + Math.round(255 * percent) + ", 0)";
        },
        trackColor: '#eeeeee',
        barColor:'#eac459',
        scaleColor: false,
        lineCap: 'butt',
        lineWidth: 5,
        animate: 1000,
        size: 100
    });





}();