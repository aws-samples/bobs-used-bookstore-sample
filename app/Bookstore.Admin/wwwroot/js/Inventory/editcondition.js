// Populate page with current book condition to edit
var text;
function LoadText() {
    text = document.getElementById('list').value;
    $('#actual').val()
    return text;
}

$(document).ready(function () {
    $('#exampleModal').on('shown.bs.modal', function () {
        var metrics_key = $('.list').val();
        $('#exampleModal input[name="name"]').val(metrics_key);
    })
});

