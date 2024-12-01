$('#DateScheduled').datetimepicker();
$('#DateCreated').datetimepicker();

showChooseRelated = function () {
    var popup = window.open('/Articles/ChooseRelated', "popUpChooseRelated", 'width=640px,height=480px');
    var myVal = '';
    popup.onbeforeunload = function () {
        //write code for unload page -> pass variable to session
        console.log('unload choose page -> call controller to refresh partial create page');
    }
};

function readURL(input) {
    console.log('xx1');
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image_upload_preview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#FileUpload").change(function () {
    console.log('xx2');
    readURL(this);
});

//CKEDITOR.basePath = '@baseHref/Scripts/ckeditor/';

//CKEDITOR.replace('OriginalContent', {
//    filebrowserImageBrowseUrl: '/Articles/UploadPartial',
//    filebrowserImageUploadUrl: '/Articles/UploadNow',
//    removeDialogTabs: 'link:upload;image:upload'
//});


function updateValue(id, value) {
    console.log('xx3');
    var dialog = CKEDITOR.dialog.getCurrent();
    if (dialog != null) {
        dialog.setValueOf('info', 'txtUrl', value);
    }
    else {
        console.log('xx123');
        document.getElementById('cke_Extend_textInput_ID').value = value;
    }
}

//HÀM KINH KHỦNG
function CkEditorURLTransfer(imageUrl, callUrl) {
    var functionCode = getParameterByName('CKEditorFuncNum', callUrl);

    //cái CKEditorFuncNum=0 ở cái link popup
    console.log(imageUrl);
    console.log('xx4');

    CKEDITOR.tools.callFunction(functionCode, imageUrl);
}

//get param cho cái link là thằng call hàm để chọn ảnh trong bài viết
//từ param => function code để đưa link vào hộp select ảnh
function getParameterByName(name, href) {
    console.log('xx5');
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(href);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function PreviewImage(inputId) {
    console.log('xx6');
    var link = $('#' + inputId).val();
    if (link == '') {
        link = 'http://placehold.it/100x100';
    }
    $('#image_upload_preview').attr('src', link);
}