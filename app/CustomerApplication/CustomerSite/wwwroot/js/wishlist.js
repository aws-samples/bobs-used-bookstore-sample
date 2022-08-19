var fruits = document.getElementsByName('fruits');
var numbers = document.getElementsByClassName("numbers");

var list = [];
for (var i = 0; i < fruits.length; i++) {
    fruits[i].onclick = count;
}
for (var i = 0; i < numbers.length; i++) {
    numbers[i].onclick = count;
}

function count() {
    var totalprice = 0;
    for (var i = 0; i < fruits.length; i++) {
        if (fruits[i].checked) {
            var number = fruits[i].nextSibling.nextSibling.value;
            totalprice += parseFloat(fruits[i].value) * number;
        }
    }
    document.getElementById('totalprice').innerHTML = totalprice;
}

function selectall() {
    var sports = document.getElementsByName("fruits");
    for (i = 0; i < sports.length; i++) {
        var sportname = sports[i];
        if (sportname.checked == true) {
            sportname.checked = false

        } else {
            sportname.checked = true;
        }

    }
}