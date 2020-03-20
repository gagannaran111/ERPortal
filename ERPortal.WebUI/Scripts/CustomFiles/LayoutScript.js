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

$(document).on('click', '.ViewComment', function (e) {
    e.preventDefault();
    var comText = $(this).attr('commenttext');
    var comType = $(this).attr('commentType');
    var comdate = $(this).attr('commentdate');
    $('#formModalLabelView').html('View Comment');
    $('#modalContentView').html('<div class=row><div class="col-lg-6 text-success font-weight-bold">Type : ' + comType + ' </div><div class="col-lg-6 text-success font-weight-bold">Date : ' + comdate + ' </div><div class="col-sm-12 mt-3 border-top "><h6 class="text-info">Comment :</h6> <p class="mt-3">' + comText + '</p></div></div>');
    $('#btnCommentViewModal').click();
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

const STATUSERROR = { ERROR: "ERROR", NOCOMMENTREC: "NCR", QUERYREPSUCC: "Query Reply Successfully" };

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
const handleImageUpload = event => {
    const files = event.target.files
    const formData = new FormData()
    formData.append('myFile', files[0])

    fetch('/saveImage', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            console.log(data.path)
        })
        .catch(error => {
            console.error(error)
        })
}

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
        url: "/Comment/LoadUploadFile", //"@Url.Action("LoadUploadFile","Comment")",
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
        url: "/Comment/RemoveUploadFile",  //'@Url.Action("RemoveUploadFile", "Comment")',
        data: JSON.stringify(sendData),
        datatype: 'json',
        async: false,
        success: (data) => {
            // alertModal(data);
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
        url: "/Comment/GetUploadFiles", //'@Url.Action("GetUploadFiles", "Comment")',
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
    let pattern = /Date\(([^)]+)\)/;
    let results = pattern.exec(value);
    let dt = new Date(parseFloat(results[1]));

    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
}
const CompareTwoDates = (d1, d2) => {

    if (d1.val() != "" && d2.val() != "") {
        let date1 = d1.val().split('/');
        let date2 = d2.val().split('/');
        let diffYear = "";
        let diffmonth = "";
        let diffdate = "";
        let valnull = false;
        diffYear = date1[0] - parseInt(date2[0]);
        diffmonth = parseInt((date1[1])) - parseInt(date2[1]);
        diffdate = parseInt(date1[2]) - parseInt(date2[2]);
       // valnull = diffYear < 0 ? true: diffmonth > 0 ? true : diffdate > 0 ? true : false;

        if (diffYear > 0) {
            if (diffmonth > 0) {
                if (diffdate > 0) {
                    valnull = true;
                }
                else {
                    valnull = false;
                }
            }
            else {
                valnull = false;
            }
        }
        else {
            valnull = false;
        }
        console.log(diffYear, diffmonth, diffdate);


        if (valnull == false) {
            d1.val('');
            d2.val('');
            return valnull;
        }
        else {
            return valnull;
        }

    }
    else {
        valnull = d1.val() == '' ? false : d2.val() == '' ? false : true;
        d1.val() != '' ? d1.val('') : d2.val('');
        return valnull;
    }
}

const editor = (textareaid) => {
    $(textareaid).Editor({

        "insert_img": false,
        "insert_link": false,
        "togglescreen": false,
        "source": false
    });

}
$(document).ajaxStart(() => {
    $(".containerloader").removeClass('d-none');
    console.log('loaderstart');
});
$(document).ajaxComplete(() => {
    $(".containerloader").addClass('d-none');
    console.log('loadercomplete');
});
