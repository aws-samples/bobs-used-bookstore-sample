/**
 * Created by mosaddek on 12/27/17.
 */

;(function () {
    'use strict';

    var ChartBuilder = function () {
        var self = this,
            chartData, chartDataInitial, chartSchema, chartContext, chartConfig, chartLine,
            updateBlockInfo;

        chartData = {
            webVisit: {
                title: 'Web Visits',
                prevTotalDataPoints: 4000,
                dataPoints: [
                    { 'date': '2017-01-01T17:00:00.000Z', 'value': 570 },
                    { 'date': '2017-01-05T17:00:00.000Z', 'value': 525 },
                    { 'date': '2017-01-10T17:00:00.000Z', 'value': 560 },
                    { 'date': '2017-01-15T17:00:00.000Z', 'value': 550 },
                    { 'date': '2017-01-20T17:00:00.000Z', 'value': 555 },
                    { 'date': '2017-01-25T17:00:00.000Z', 'value': 780 },
                    { 'date': '2017-01-29T17:00:00.000Z', 'value': 560 },
                    { 'date': '2017-01-30T17:00:00.000Z', 'value': 560 },
                ],
            },
            fileDownload: {
                title: 'File Downloads',
                prevTotalDataPoints: 1300,
                dataPoints: [
                    { 'date': '2017-01-01T17:00:00.000Z', 'value': 110 },
                    { 'date': '2017-01-05T17:00:00.000Z', 'value': 130 },
                    { 'date': '2017-01-10T17:00:00.000Z', 'value': 105 },
                    { 'date': '2017-01-15T17:00:00.000Z', 'value': 215 },
                    { 'date': '2017-01-20T17:00:00.000Z', 'value': 85 },
                    { 'date': '2017-01-25T17:00:00.000Z', 'value': 120 },
                    { 'date': '2017-01-29T17:00:00.000Z', 'value': 135 },
                    { 'date': '2017-01-30T17:00:00.000Z', 'value': 110 },
                ],
            }
        };

        _.each(chartData, function (e) {
            e.value = _.reduce(e.dataPoints, function (p, f) { return p + f.value; }, 0);
            e.percentage = (e.value - e.prevTotalDataPoints) / e.prevTotalDataPoints * 100;
            e.status = (e.value > e.prevTotalDataPoints) ? 'up' : 'down';
        });

        chartDataInitial = chartData.webVisit;

        chartSchema = {
            labels: _.map(chartDataInitial.dataPoints, function (e, i) {
                return moment(e.date).format('D. MMM');
            }),
            datasets: [{
                fillColor: 'rgba(0,0,0,.1)',
                strokeColor: 'rgba(0,0,0,.5)',
                pointColor: 'transparent',
                pointStrokeColor: 'transparent',
                pointHighlightFill: '#fefefe',
                pointHighlightStroke: 'rgba(255,255,255,0.3)',
                data: _.map(chartDataInitial.dataPoints, function (e) {
                    return e.value;
                }),
            }]
        };

        chartConfig = {
            responsive: true,
            animationSteps: 30,
            scaleFontColor: '#fff',
            datasetStrokeWidth: 2,
            pointDotRadius: 4,
            pointDotStrokeWidth: 4,
            scaleShowVerticalLines: false,
            scaleLineColor: 'rgba(255,255,255,0.1)',
            scaleGridLineColor: "rgba(255,255,255,0.1)",
            scaleGridLineWidth: 1,
            customTooltips: function(tooltip) {
                var tooltipEl = $('.aws-tooltip'),
                    whichChart = $("[data-btn-chart].aws-active").attr('data-btn-chart'),
                    currentData, text;

                if (!tooltip) {
                    tooltipEl.css({ opacity: 0 });
                    return;
                }

                currentData = _.find(chartData[whichChart].dataPoints, function (e) {
                    return moment(e.date).format('D. MMM') == $.trim(tooltip.text.split(':')[0]);
                });

                tooltipEl.removeClass('above below aws-for-file');
                tooltipEl.addClass(tooltip.yAlign);

                text = accounting.formatNumber(currentData.value);

                if (chartData[whichChart].title.toLowerCase().indexOf('web') == -1)
                    tooltipEl.addClass('aws-for-file');

                tooltipEl.html([
                    '<span>' + moment(currentData.date).format("ddd, MMM DD, YYYY") + '</span>',
                    '<span>' + chartData[whichChart].title + ': <b>' + text + '</b></span>'
                ].join(''));

                tooltipEl.css({
                    opacity: 1,
                    left: tooltip.chart.canvas.offsetLeft + tooltip.x - (parseInt(tooltipEl.css('width').replace('px', ''), 0) / 2.0) + 'px',
                    top: tooltip.chart.canvas.offsetTop + tooltip.y - 68 + 'px',
                });
            }
        };

//        chartContext = $('.aws-chart canvas')[0].getContext('2d');

        updateBlockInfo = function (whichChartData, isOnInit) {
            isOnInit = (typeof isOnInit === String(undefined)) ? false : isOnInit;

            var $block1After, $block1Before,
                $block2After, $block2Before;

            $block1Before = $block1After = $('.aws-details .col-md-6:eq(0) .aws-block-info:eq(0)');
            $block2Before = $block2After = $('.aws-details .col-md-6:eq(1) .aws-block-info:eq(0)');

            if (!isOnInit) {
                $block1After.clone().appendTo($block1After.parent());
                $block2After.clone().appendTo($block2After.parent());

                $block1After = $block1After.next();
                $block2After = $block2After.next();
            }

            $block1After.find('h3 span').html([
                accounting.formatNumber(whichChartData.value),
                whichChartData.title.split(' ')[1]
            ].join(' '));

            $block2After.find('h3 span').html(
                accounting.formatNumber(whichChartData.percentage, 2)
            );

            if (whichChartData.hasOwnProperty('status'))
                $block2After.find('h3').attr('data-status', whichChartData.status);

            if (!isOnInit) {
                $block1Before.animate({
                    marginTop: -100
                }, 300, 'easeOutCubic', function () {
                    $block1Before.remove();
                });

                $block2Before.animate({
                    marginTop: -100
                }, 300, 'easeOutCubic', function () {
                    $block2Before.remove();
                });
            }
        };

        self.run = function () {
            //chartLine = new Chart(chartContext).Line(chartSchema, chartConfig);
           // updateBlockInfo(chartDataInitial, true);
        };

        self.registerEvent = function () {
            $('[data-btn-chart]').on('click', function () {
                var $self = $(this),
                    dataBtnChart = $self.attr('data-btn-chart'),
                    whichChartData = chartData[dataBtnChart],
                    targetDataPoints = whichChartData.dataPoints,
                    points = chartLine.datasets[0].points;

                if ($self.hasClass('aws-active'))
                    return;

                $self.closest('nav').find('.aws-active').removeClass('aws-active');
                $self.addClass('aws-active');

                for (var i in points) {
                    points[i].value = targetDataPoints[i].value;
                }

                chartLine.update();
                updateBlockInfo(whichChartData);
            });
        };
    };

    $(function () {
        var chartBuilder = new ChartBuilder();
        chartBuilder.run();
        chartBuilder.registerEvent();
    });
}());