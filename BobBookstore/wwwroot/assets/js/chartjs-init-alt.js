

var ctx = document.getElementById('myChart-light').getContext('2d');
var chart = new Chart(ctx, {
    // The type of chart we want to create
    type: 'line',

    // The data for our dataset
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [{
            label: "My First dataset",
            backgroundColor: 'rgba(167,104,243,.2)',
            borderColor: 'rgba(167,104,243,1)',
            data: [0, 20, 9, 25, 15, 25,18]
        }]


    },

    // Configuration options go here
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },

        scales: {
            xAxes: [{
                display: false
            }],
            yAxes: [{
                display: false
            }]
        },
        elements: {
            line: {
                tension: 0.00001,
                //tension: 0.4,
                borderWidth: 1
            },
            point: {
                radius: 4,
                hitRadius: 10,
                hoverRadius: 4
            }
        }
    }
});



var ctx = document.getElementById('myChart-tow-light').getContext('2d');
var chart = new Chart(ctx, {
    // The type of chart we want to create
    type: 'line',

    // The data for our dataset
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [{
            label: "My First dataset",
            backgroundColor: 'rgba(54,162,245,.2)',
            borderColor: 'rgba(54,162,245,1)',
            //data: [6.06, 82.2, -22.11, 21.53, -21.47, 73.61, -53.75, -60.32]
            data: [70, 45, 65, 50, 65, 35, 50]
        }]


    },

    // Configuration options go here
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                display: false
            }],
            yAxes: [{
                display: false
            }]
        },
        elements: {
            line: {
                //tension: 0.00001,
                tension: 0.4,
                borderWidth: 1
            },
            point: {
                radius: 4,
                hitRadius: 10,
                hoverRadius: 4
            }
        }
    }
});


var ctx = document.getElementById('myChart3-light').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'bar',
    data: {
        labels: ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q"],
        datasets: [{
            label: '# of Votes',
            data: [58, 80, 44, 76, 54, 50, 45, 90, 57, 48, 54, 49, 63, 77, 67, 83, 95],
            backgroundColor: [
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)',
                'rgba(234,196,89,.6)'

            ],
            //borderColor: [
            //    'rgba(255,99,132,1)',
            //    'rgba(54, 162, 235, 1)',
            //    'rgba(255, 206, 86, 1)',
            //    'rgba(75, 192, 192, 1)',
            //    'rgba(153, 102, 255, 1)',
            //    'rgba(255, 159, 64, 1)'
            //],
            borderWidth: 0
        }]
    },
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                display: false
            }],
            yAxes: [{
                display: false
            }]
        }

    }
});




var ctx = document.getElementById('myChart4-light').getContext('2d');
var chart = new Chart(ctx, {
    // The type of chart we want to create
    type: 'line',

    // The data for our dataset
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: [{
            label: "My First dataset",
            backgroundColor: 'rgb(255,255,255,0)',
            //backgroundColor: 'rgba(167,104,243,.2)',
            borderColor: 'rgba(255,81,138,1)',
            data: [6.06, 82.2, -22.11, 21.53, -21.47, 73.61, -53.75, -60.32]
        }]


    },

    // Configuration options go here
    options: {
        maintainAspectRatio: false,
        legend: {
            display: false
        },
        scales: {
            xAxes: [{
                gridLines: {
                    color: 'transparent',
                    zeroLineColor: 'transparent'
                },
                ticks: {
                    fontSize: 2,
                    fontColor: 'transparent'
                }

            }],
            yAxes: [{
                display: false,
                ticks: {
                    display: false
                    //min: Math.min.apply(Math, data.datasets[0].data) - 5,
                    //max: Math.max.apply(Math, data.datasets[0].data) + 5
                }
            }]
        },
        elements: {
            line: {
                tension: 0.00001,
                //tension: 0.4,
                borderWidth: 1
            },
            point: {
                radius: 4,
                hitRadius: 10,
                hoverRadius: 4
            }
        }
    }
});

