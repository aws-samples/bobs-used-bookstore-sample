// Update total price based on number of books in cart
var prices = document.getElementsByName('prices');
var numbers = document.getElementsByClassName("numbers");
var totalprice = 0;
for (var i = 0; i < prices.length; i++) {

    var number = numbers[i].value;
    totalprice += prices[i].value * number;

}
document.getElementById('totalprice').innerHTML = totalprice;

for (var i = 0; i < numbers.length; i++) {
    numbers[i].onchange = updateTotalPrice;
}

function updateTotalPrice() {
    var totalprice = 0;
    for (var i = 0; i < prices.length; i++) {

        var number = numbers[i].value;
        totalprice += prices[i].value * number;

    }
    document.getElementById('totalprice').innerHTML = totalprice;
}