import { STATUSERROR } from './Types.js';
import { GetUploadFilesData, alertModal } from './CommenMethods.js';
const ClassName = {
    GeneratePDF: ".GeneratePDF",
    UploadFile: ".UploadFile",
    FileDelete: ".fileDelete"
};
const AjaxURL = {
    UploadFile: "../Comment/LoadUploadFile",
    FileDelete: "../Comment/RemoveUploadFile",
};
$(document).ready(() => {

    $(".datetimetext").datetimepicker({
        format: 'Y/m/d',
        timepicker: false,
        scrollMonth: false,
        scrollInput: false
    });
    $(".sidebar-dropdown > a").click(function () {
        $(".sidebar-submenu").slideUp(200);
        if ($(this).parent().hasClass("active")) {
            $(".sidebar-dropdown").removeClass("active");
            $(this).parent().removeClass("active");
        }
        else {
            $(".sidebar-dropdown").removeClass("active");
            $(this).next(".sidebar-submenu").slideDown(200);
            $(this).parent().addClass("active");
        }
    });
    $("#close-sidebar").click(() => {
        $(".page-wrapper").removeClass("toggled");
    });
    $("#show-sidebar").click(() => {
        $(".page-wrapper").addClass("toggled");
    });
});

$(document).on("click", ClassName.GeneratePDF, (e) => {
    e.preventDefault();
    let cssfile = "";
    cssfile = "../Content/css/PrintingData.css";
    $(".container-fluid").printThis({
        importCSS: true,
        importStyle: true,
        printContainer: false,
        copyTagClasses: true,
        pageTitle: "",
        loadCSS: cssfile
    });
});


$(document).on('click', '#btnCommentSave', () => {
    let form = $('#modalContentComment').find('form');
    let formUrl = form.attr('action');

    let DataText = $('#modalContentComment').find('#Comment_Text').val();
    let msg = 'Something Went Wrong. Try Again.';

    if (DataText.length > 0) {
        msg = 'Something Went Wrong. Try Again.';
        $.ajax({
            url: formUrl,
            type: 'POST',
            data: form.serialize(),
            async: false,
            success: (result) => {
                console.log(result);
                switch (result) {
                    case STATUSERROR.ERROR:
                        $('.ErrorComment').text(msg);
                        break;
                    case STATUSERROR.QUERYREPSUCC:
                        $('.Querycommenttablink').click();
                        break;
                    case STATUSERROR.NOCOMMENTREC:
                        $('.ErrorComment').text("There Have No Comment Recieved To Resolve");
                        break;
                    default:
                        $('#modalContentComment').html(result);
                        if ($("#btnCommentSave").is(":visible")) {
                            $('#btnCommentSave').fadeOut();
                        }
                        window.location.reload();
                        break;
                }
            },
            error: (result) => {

                $('#modalContentComment').html(result);
            },
            fail: (xhr, textStatus, errorThrown) => {
                $('#modalContentComment').html('<div class="alert alert-danger" role="alert">Request Failed with error: ' + errorThrown + '</div > ');

            }
        });
    }
    else {
        msg = 'Text Field Not Empty. Enter Some Text To Save';
        $('.ErrorComment').text(msg);
        return false;
    }
});

$(document).on('click', ClassName.UploadFile, (e) => {
    e.preventDefault();
    let DatasetAttrVal = e.currentTarget.dataset;
    let refid = DatasetAttrVal.filerefid;
    let divid = DatasetAttrVal.divid;
    let inputfile = DatasetAttrVal.inputfile;
    let file = $(inputfile).get(0).files[0];
    let sendData = new FormData();
    sendData.append("file", file);
    sendData.append("RefId", refid);
    //var sendData = function () {
    //       var data = new FormData();
    //       data.append("file", file);
    //       data.append("RefId", refid);
    //       return data;
    //   }();
    $.ajax({
        url: AjaxURL.UploadFile,
        type: 'POST',
        data: sendData,
        contentType: false,
        processData: false,
        async: false,
        success: (result) => {
            if (result == "File upload Success") {
                $("input[type='file']").val("");
                GetUploadFilesData("#" + divid, refid);
            }
        },
        error: () => {
            alert("Something Went Wrong. Try Again");
        },
        fail: (xhr, textStatus, errorThrown) => {
        }
    });
});

$(document).on("click", ClassName.FileDelete, (e) => {
    e.preventDefault();
    let DatasetAttrVal = e.currentTarget.dataset;
    let divId = DatasetAttrVal.divid;
    let fileid = DatasetAttrVal.fileid;
    let refid = DatasetAttrVal.fileref;
    let sendData = { FileId: fileid };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: AjaxURL.FileDelete,
        data: JSON.stringify(sendData),
        datatype: 'json',
        async: false,
        success: (data) => {
            GetUploadFilesData(divId, refid);
        },
        error: () => {
            alertModal("Something Went Wrong. Try Again");
        },
    });

});
$(document).ajaxStart(() => {
    $(".containerloader").removeClass('d-none');
    console.log('loaderstart');
});
$(document).ajaxComplete(() => {
    $(".containerloader").addClass('d-none');
    console.log('loadercomplete');
});
