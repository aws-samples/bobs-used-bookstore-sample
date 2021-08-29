function buttonFn(a) {

    var section_id = "#" + a.parentNode.previousElementSibling.id
    var newitem = a.previousElementSibling.value;
    var newOption = "<option value='" + newitem + "'>" + newitem + "</option>";
    $(newOption).insertBefore($(section_id + " option:last")); //add new option
    $(section_id).val(newitem);
    $("#" + a.parentNode.id).css("display", "none");
}
$(document).ready(function () {
    $(".dropdown").change(function () {
        var item = $(this).find("option:selected");
        var text = item.html(); //get the selected option text
        var div_id = "#" + this.nextElementSibling.id
        if (text == 'Others') {
            $(div_id).css("display", "block"); //display the add new dialog
        }
        else {
            $(div_id).css("display", "none");
        }

    });
});