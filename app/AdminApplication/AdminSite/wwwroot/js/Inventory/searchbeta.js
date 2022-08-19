$(function () {
    $("#tags").autocomplete({
        source: '/Inventory/AutoSuggest'
    });
});