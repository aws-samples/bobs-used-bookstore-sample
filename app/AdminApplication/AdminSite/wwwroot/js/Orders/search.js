function updateFilterValue(filterValue, filterValueData) {
    var text = document.getElementById('idFilterValue').value;
    var reg = new RegExp(filterValue, "g");

    var reg2 = new RegExp(" " + filterValueData, "g");
    var data = document.getElementById("filterValueString").value;

    if (text.includes(filterValue)) {
        text = text.replace(reg, '');
        data = data.replace(reg2, '');
    }
    else {
        text = text.concat(filterValue);

        data = data.concat(" " + filterValueData);
    }

    document.getElementById('idFilterValue').value = text;
    document.getElementById("filterValueString").value = data;
}