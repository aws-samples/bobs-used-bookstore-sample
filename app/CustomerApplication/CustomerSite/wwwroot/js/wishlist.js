// Update total price based on number of books in wishlist
var prices = document.getElementsByName('prices');
var numbers = document.getElementsByClassName("numbers");

var list = [];
for (var i = 0; i < prices.length; i++) {
    prices[i].onclick = updateTotalPrice;
}
for (var i = 0; i < numbers.length; i++) {
    numbers[i].onclick = updateTotalPrice;
}

function updateTotalPrice() {
    var totalprice = 0;
    for (var i = 0; i < prices.length; i++) {
        if (prices[i].checked) {
            var number = prices[i].nextSibling.nextSibling.value;
            totalprice += parseFloat(prices[i].value) * number;
        }
    }
    document.getElementById('totalprice').innerHTML = totalprice;
}

function selectall() {
    var prices = document.getElementsByName("prices");
    for (i = 0; i < prices.length; i++) {
        var price = prices[i];
        if (price.checked == true) {
            price.checked = false
        } else {
            price.checked = true;
        }
    }
}