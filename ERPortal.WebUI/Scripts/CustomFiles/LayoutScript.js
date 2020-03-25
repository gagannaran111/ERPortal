const STATUSERROR = { ERROR: "ERROR", NOCOMMENTREC: "NCR", QUERYREPSUCC: "Query Reply Successfully" };
const PATTERNS = { DateFormate: /Date\(([^)]+)\)/ };

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

$(document).on("click", ".GeneratePDF", function (e) {
    e.preventDefault();
    let cssfile = "";
    cssfile = "/Content/css/PrintingData.css";
    $(".container-fluid").printThis({
        importCSS: true,
        importStyle: true,
        printContainer: false,
        copyTagClasses: true,
        pageTitle: "",
        loadCSS: cssfile
    });
});


$('#btnCommentSave').click(() => {
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

$(document).on('click', '.UploadFile', (e) => {
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
        url: "/Comment/LoadUploadFile",
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

$(document).on("click", '.fileDelete', (e) => {
    e.preventDefault();
    let DatasetAttrVal = e.currentTarget.dataset;
    let divId = DatasetAttrVal.divid;
    let fileid = DatasetAttrVal.fileid;
    let refid = DatasetAttrVal.fileref;
    let sendData = { FileId: fileid };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: "/Comment/RemoveUploadFile",
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

const alertModal = (msg) => {
    $('#btnalertmodal').click();
    $('#modalContentAlert').html("<strong>" + msg + "</strong>");
}

const GetUploadFilesData = (divId, refid) => {
    $(divId).html("No files attached");
    let sendData = { RefId: refid };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: "/Comment/GetUploadFiles",
        data: JSON.stringify(sendData),
        datatype: 'json',
        async: false,
        success: (data) => {
            if (data) {
                console.log(data);
                let txt = "";
                if (data.length > 0) {
                    for (let file of data) {
                        txt += `<div class='badge badge-warning mr-2 mb-2'><a class='text-dark' href='@Url.Content("~/Content/UploadedFiles/")${file.FileName}' target='_blank'>${file.FileName}</a><a class='fileDelete small text-danger'data-fileid='${file.Id}' data-fileref='${file.FIleRef}' data-divid ='${divId}'  href='#'><i class='ml-1 fa fa-times'></i></a></div>`;
                    }
                    $(divId).html(txt);
                    $(divId).fadeIn("slow");
                }
            }
        },
        error: () => {
            alertModal("Something Went Wrong. Try Again");
        },
    })
}
const ShowCommentModal = (appid, title, urlpath, targetpage, queryid) => {
    $('#modalContentComment').empty();
    let senddata = { appid: appid, targetPage: targetpage, queryId: queryid };
    $.ajax({
        url: urlpath,
        data: senddata,
        async: false,
        success: (result) => {
            console.log(result);
            $('#CommentModalLabel').text(title);
            $('#modalContentComment').html(result);
            if ($("#btnCommentSave").is(":hidden")) {
                $('#btnCommentSave').show();
            }
            //  editor('#modalContentComment textarea');
        },
        error: () => {
            $('#modalContentComment').html('<div class="alert alert-danger" role="alert"> An Error has occured </div >');

        },
        fail: (xhr, textStatus, errorThrown) => {
            $('#modalContentComment').html('<div class="alert alert-danger" role="alert">Request Failed with error: ' + errorThrown + '</div > ');

        }
    });

}
const ToChangeDateFormate = (value) => {
    
    let results = PATTERNS.DateFormate.exec(value);
    let dt = new Date(parseFloat(results[1]));

    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
}
const CompareTwoDates = (d1, d2) => {
    let valnull = false;
    
    if (d1 != "" && d2 != "") {
        let date1 = d1.split('/');
        let date2 = d2.split('/');
        let diffYear = 0;
        let diffmonth = 0;
        let diffdate = 0;
        diffYear = parseInt(date1[0]) - parseInt(date2[0]);
        diffmonth = parseInt(date1[1]) - parseInt(date2[1]);
        diffdate = parseInt(date1[2]) - parseInt(date2[2]);
        valnull = diffYear > 0 ? true : diffmonth > 0 && diffYear == 0 ? true : diffdate > 0 && diffmonth == 0 ? true : false;
        console.log(diffYear, diffmonth, diffdate, valnull);
    }
    else {
        return "ERROR";
    }
    return valnull;
   
}

$(document).ajaxStart(() => {
    $(".containerloader").removeClass('d-none');
    console.log('loaderstart');
});
$(document).ajaxComplete(() => {
    $(".containerloader").addClass('d-none');
    console.log('loadercomplete');
});
