$(function () {
    RegularNull("#Title,#Des,#Context", "1");
    Addbtn();
    GetKindEditor();
});


function Addbtn() {
    $(document).on("click", "#EditBtn", function () {

        if (Regular()) {
            $("#Controller").submit();

        }
    });
}
function GetKindEditor() {
    var editor = KindEditor.create('#Context', {
        allowFileManager: true,
        width: 750,
        items: [
           'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
    'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
    'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
    'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
    'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
    'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
    'flash', 'media', 'insertfile', 'table', 'hr', 'emoticons', 'baidumap', 'pagebreak',
    'anchor', 'link', 'unlink', '|', 'about'
        ],
        afterBlur: function () { this.sync(); },
        afterCreate: function () { this.html($('#ArtContent').val()); }
    });
}