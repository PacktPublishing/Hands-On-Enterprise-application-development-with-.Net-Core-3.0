// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.custom-file input').change(function() {
    var $el = $(this),
    files = $el[0].files,
    label = files[0].name;
    if (files.length > 1) {
        label = label + " and " + String(files.length - 1) + " more files"
    }
    $el.next('.custom-file-label').html(label);
});