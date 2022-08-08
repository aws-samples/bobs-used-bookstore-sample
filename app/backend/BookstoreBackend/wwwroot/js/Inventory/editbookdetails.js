var onValChange = args => {
    localStorage.setItem(args.id, args.value)
};

var checkAndUpdate = elements => {
    for (tag of elements) {
        const val = localStorage.getItem(tag.id)
        if (val) {
            document.getElementById(tag.id).value = val;
        }
    }
  }

$(document).ready(function () {
    var tagNames = ["input", "select", "textarea"]
    for (tagName of tagNames) {
        checkAndUpdate(document.getElementsByTagName(tagName))
    }

    $('.custom-file-input').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});