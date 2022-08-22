// Enable autocomplete
$(function () {
    $("#tags").autocomplete({
        source: '/Inventory/AutoSuggest'
    });
});