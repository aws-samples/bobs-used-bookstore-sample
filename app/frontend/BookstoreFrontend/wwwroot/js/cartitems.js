var fruits = document.getElementsByName('fruits');
var numbers = document.getElementsByClassName("numbers");
var totalprice = 0;
for (var i = 0; i < fruits.length; i++) {

    var number = numbers[i].value;
    totalprice += fruits[i].value * number;

}
document.getElementById('totalprice').innerHTML = totalprice;

for (var i = 0; i < numbers.length; i++) {
    numbers[i].onclick = count;
}

function count() {
    var totalprice = 0;
    for (var i = 0; i < fruits.length; i++) {

        var number = numbers[i].value;
        totalprice += fruits[i].value * number;

    }
    document.getElementById('totalprice').innerHTML = totalprice;
}