import { ShowCommentModal, alertModal, ToChangeDateFormate } from "./CommenMethods.js";
import { UserRole, ShowHide } from "./Types.js";
const ElementId = {
    AppId: '#ERApplications_AppId',
    Id: '#ERApplications_Id',
    CommentTabLink: '#CommentTablTink',
    ERProposalTabLink: '#ERProposalTabLink',
    FowardAppFileStatus: '#ForwardApplication_FileStatus',
    ForwardApp: '#ForwardApp',
    GrantApp: '#GrantApp',
    UploadLetter: '#UploadLetter',
};
const AppUserDetail = {
    AppId: $(ElementId.Id).val(),
    CurrUserRole: $(ElementId.Id).attr('UserRole'),
};
const AjaxURL = {
    AjaxAdd: "/Comment/AjaxAdd",
    ApplicationSummary: "/Comment/ApplicationSummary",
    BtnCheckStatus: "/Comment/BtnCheckStatus"
};
$(document).ready(() => {
    BtnForwardCheckShowHideStatus();
    AppUserDetail.CurrUserRole == UserRole.CER ? BtnUploadLetterCheckShowHideStatus() : null;
});

$(document).on('click', ElementId.ForwardApp, ({ currentTarget }) => {
    let DatasetAttrVal = currentTarget.dataset;

    ShowCommentModal(AppUserDetail.AppId, DatasetAttrVal.title, AjaxURL.AjaxAdd, DatasetAttrVal.page, null);

    let list = AppUserDetail.CurrUserRole == UserRole.CER ? ["1", "2", "3", "4"]
        : AppUserDetail.CurrUserRole == UserRole.Coord ? ["1", "2", "4"] : ["0", "3", "4"];
    $(ElementId.FowardAppFileStatus + ' option').filter(function () {
        return $.inArray(this.value, list) !== -1
    }).remove();
    $(ElementId.FowardAppFileStatus).val('');
});
$(document).on('click', ElementId.UploadLetter, ({ currentTarget }) => {
    let DatasetAttrVal = currentTarget.dataset;
    ShowCommentModal(AppUserDetail.AppId, DatasetAttrVal.title, AjaxURL.AjaxAdd, DatasetAttrVal.page, null);

});
$(document).on('click', '.QueryComment', ({ currentTarget }) => {
    let DatasetAttrVal = currentTarget.dataset;
    ShowCommentModal(AppUserDetail.AppId, DatasetAttrVal.title, AjaxURL.AjaxAdd, DatasetAttrVal.page, null);

});
$(document).on('click', ElementId.GrantApp, ({ currentTarget }) => {
    let DatasetAttrVal = currentTarget.dataset;
    ShowCommentModal(AppUserDetail.AppId, DatasetAttrVal.title,AjaxURL.AjaxAdd, DatasetAttrVal.page, null);
});

$(document).on('click', ElementId.ERProposalTabLink, () => {
    BtnForwardCheckShowHideStatus();
    AppUserDetail.CurrUserRole == UserRole.CER ? BtnUploadLetterCheckShowHideStatus() : null;
});

$(document).on('click', ElementId.CommentTabLink, () => {

    $.ajax({
        url: AjaxURL.ApplicationSummary, //"@Url.Action("ApplicationSummary", "Comment", new { appid = Model.ERApplications.Id })",
        data: { appid: AppUserDetail.AppId },
        success: (result) => {

            console.log(result);
            if (result.length == 0) {
                $("#CommentTab .body-panel").html('<h3 class="text-danger ">Data Not Found</h3>');
            }
            else {
                let dd = "";
                let statuscheck = "";
                for (let ee of result) {
                    let arrayelement = ee.length;
                    let reciver = "";
                    let countelement = 0;
                    for (let element of ee) {
                        let files = "";
                        countelement++;

                        for (let e of element.Files) {
                            console.log(e);
                            files += '<a class="badge badge-info mr-2" href="../Content/Uploads/' + e.NewFileName + '" target="_blank"><i class="fas fa-file-download"></i> ' + e.FileName + '</a>';
                        };

                        reciver += arrayelement > 1 ? element.Recivername + "," : element.Recivername;

                        if (arrayelement == countelement) {
                            reciver = reciver.includes(',') == true ? reciver.substr(0, reciver.length - 1) : reciver;
                            switch (element.FileStatus) {
                                case 0:
                                    statuscheck = "Forward To " + reciver + " by " + element.Sendername;
                                    break;
                                case 1:
                                    statuscheck = "Recommended by " + element.Sendername;
                                    break;
                                case 2:
                                    statuscheck = "Comment Back To " + reciver + " by " + element.Sendername;
                                    break;
                                case 3:
                                    statuscheck = reciver + " Review Application Again. Comment Resolved By " + element.Sendername;
                                    break;
                                case 4:
                                    statuscheck = "Application Approval Letter Uploaded by " + element.Sendername;
                                    break;
                                default: statuscheck = "";
                                    break;
                            }
                            dd += `<li class="left clearfix "><span class=" pull-left">
                                    <span class=""><img src="../Content/img/user-profile.png" alt="User" style="width:40px"> </span>
                                    </span><div class="chat-body clearfix"><div class="header"><strong class="text-danger">${statuscheck}</strong> <small class="pull-right text-muted">
                                    <i class="fas fa-clock"></i> ${ToChangeDateFormate(element.CreatedAt)}</small></div>
                                    <p>${element.comments}</p><div class="mb-2"><p>${files}</p ></div></div ></li >`;
                        }
                    };
                };
                $("#CommentTab .body-panel").html('<ul class="chat">' + dd + '</ul>');
            }
        },
        error: () => {
            alertModal("Something Went Wrong. Try Again Later");
        },
        fail: (xhr, textStatus, errorThrown) => {
            alertModal("Something Went Wrong. Try Again Later");
        }
    });

});

$(document).on('change', ElementId.FowardAppFileStatus, () => {
    let value = $(ElementId.FowardAppFileStatus + ' option:selected').val();
    $('.ErrorComment').text('');
    if (value == "1" || value == "2" || value == "3") {
        $('.ReciverIdSelectList').addClass('d-none');
        $(".ReciverIdSelectList option:selected").prop("selected", false);
    }
    else {
        $('.ReciverIdSelectList').removeClass('d-none');
    }
})

// Create Functions
const BtnUploadLetterCheckShowHideStatus = () => {

    let sendData = { btnType: "BtnUploadLetter", appid: AppUserDetail.AppId };
    $.ajax({
        url: AjaxURL.BtnCheckStatus,//"@Url.Action("BtnCheckStatus", "Comment")",
        type: 'POST',
        data: sendData,
        async: false,
        success: (result) => {
            switch (result) {
                case ShowHide.Show:
                    $(ElementId.UploadLetter).removeClass('d-none');
                    break;
                case ShowHide.Hide:
                    $(ElementId.UploadLetter).addClass('d-none');
                    break;
                default: alertModal("Something Went Wrong. Try Again Later");
            }
        },
        error: () => {
            alertModal("Something Went Wrong. Try Again Later");
        },
        fail: (xhr, textStatus, errorThrown) => {
            alertModal("Something Went Wrong. Try Again Later");
        }
    });

}
const BtnForwardCheckShowHideStatus = () => {
    let sendData = { btnType: "btnForward", appid: AppUserDetail.AppId };
    $.ajax({
        url: AjaxURL.BtnCheckStatus, // "@Url.Action("BtnCheckStatus", "Comment")",
        type: 'POST',
        data: sendData,
        async: false,
        success: (result) => {
            switch (result) {
                case ShowHide.Show:
                    $(ElementId.ForwardApp).removeClass('d-none');
                    break;
                case ShowHide.Hide:
                    $(ElementId.ForwardApp).addClass('d-none');
                    break;
                default: alertModal("Something Went Wrong. Try Again Later");

            }
        },
        error: () => {
            alertModal("Something Went Wrong. Try Again Later");
        },
        fail: (xhr, textStatus, errorThrown) => {
            alertModal("Something Went Wrong. Try Again Later");
        }
    });
}


// Query Comment

//$(document).on('click', '.Querycommenttablink', function () {

//    $.ajax({
//        url: //"@Url.Action("QueryCommentSummary", "Comment", new { appid = Model.ERApplications.Id })",
//        type: 'POST',
//        success: function (result) {

//            console.log(result);
//            if (result.length == 0) {
//                $("#QueryCommentTab .body-panel").html('<h3 class="text-danger ">Data Not Found</h3>');
//            }
//            else {
//                //editor("#demo-editor-bootstrap");

//                //(parseInt(index) + counter) +
//                var dd = "";
//                var userid = "@arr[0]";
//                var statuscheck = "";
//                var counter = 1;
//                var subject = "";
//                var replybtn = "";
//                var resolvedbtn = "";
//                $.each(result, function (index, element) {
//                    if (element[0].Subject != subject) {
//                        subject = element[0].Subject;
//                        dd += "<div class=''><div class='alert alert-success h5'>Query : " + counter + " Subject : " + subject + "</div>";
//                        counter++;
//                    }
//                    dd += "<div class='alert alert-light border border-danger'><p class='h5 float-right'>Query Between " + element[0].Sender.SenderName + " and " + element[0].Reciver.ReciverName + "</p><hr/>";
//                    $.each(result[index], function (i, f) {
//                        var files = "";

//                        $.each(f.Files, function (j, e) {
//                            files += '<a class="badge badge-success mr-2" href="' + e.FilePath + '" target="blank"><i class="fas fa-file-download"></i> ' + e.FileName + '</a>';
//                        });
//                        if (f.Status == "Query Rasied") {
//                            statuscheck = "Query Rasied To " + f.Reciver.ReciverName + " by " + f.Sender.SenderName;

//                        }
//                        else if (f.Status == "Query Reply") {
//                            statuscheck = "Query Reply To " + f.Reciver.ReciverName + " by " + f.Sender.SenderName;
//                        }
//                        else if (f.Status == "Query Forward") {
//                            statuscheck = "Query Forward To " + f.Reciver.ReciverName + " by " + f.Sender.SenderName;
//                        }
//                        else {
//                            statuscheck = "";
//                        }

//                        if (f.Reciver.ReciverId == userid || f.Sender.SenderId == userid) {
//                            replybtn = '<button class="btn btn-sm btn-primary ml-2 BtnQueryReply" data-page="QueryCommentReply" data-query-parentid="' + f.QueryParentId + '" data-query-id="' + f.Id +
//                                '" data-toggle="modal" data-target="#CommentModal" data-title="Query Reply"><i class="fas fa-reply"></i> Reply</button>';
//                        }
//                        else {
//                            replybtn = "";
//                        }

//                        dd += ' <li class="left clearfix "><span class="chat_img pull-left">' +
//                            '	<span class="chat_img"> <img src="../Content/img/user-profile.png" alt="User" style="width:50px"> </span>' +
//                            '</span><div class="chat-body clearfix"><div class="header"><strong class="text-danger h5">' + statuscheck + '</strong> <small class="pull-right text-muted">' +
//                            '<i class="fas fa-clock"></i> ' + ToChangeDateFormate(f.CreatedAt) + '</small></div><div><p><b>Subject :</b> ' + f.Subject + '</p></div>' +
//                            '<p><b>Comments :</b> ' + f.Comments + '</p><div class="mb-2"><p>' + files + '</p ></div>' +
//                            '<div class="float-right" > <button class="btn btn-sm btn-success BtnQueryForward" data-page="QueryCommentForward" data-query-parentid="' + f.QueryParentId + '" data-query-id="' + f.Id +
//                            '" data-toggle="modal" data-target="#CommentModal" data-title="Query Forward"><i class="fas fa-forward"></i> Forward</button>' + replybtn +
//                            '</div ></div ></li > ';

//                        if (f.Sender.SenderId == userid && f.Status == "Query Rasied") {
//                            resolvedbtn = '<div class="alert alert-light"><button class="btn btn-sm btn-info ml-2 BtnQueryResolved" data-page="QueryCommentResolved" data-query-parentid="' + f.QueryParentId + '" data-query-id="' + f.Id +
//                                '" data-toggle="modal" data-target="#CommentModal" data-title="Query Resolved"><i class="fas fa-check"></i> Mark As Resolved</button></div>';
//                        }
//                        else {
//                            resolvedbtn = "";
//                        }
//                    });
//                    dd += resolvedbtn + "</div></div>";
//                });

//                //  var subject = "<div class='col alert alert-info'><h5 class=''>Subject : " + result[1].Subject + "</h4></div>"
//                $("#QueryCommentTab .body-panel").html('<ul class="chat">' + dd + '</ul>');
//            }
//        },
//        error: function () {

//            alertModal("Something Went Wrong. Try Again Later");
//        },
//        fail: function (xhr, textStatus, errorThrown) {

//            alertModal("Something Went Wrong. Try Again Later");
//        }
//    });

//});

//$(document).on('click', '.BtnQueryForward', function () {
//    ShowCommentModal("@Model.ERApplications.Id", $(this).attr('data-title'), "@Url.Action("AjaxAdd","Comment")", $(this).attr('data-page'), $(this).attr('data-query-id'));
//});
//$(document).on('click', '.BtnQueryReply', function () {
//    ShowCommentModal("@Model.ERApplications.Id", $(this).attr('data-title'), "@Url.Action("AjaxAdd","Comment")", $(this).attr('data-page'), $(this).attr('data-query-id'));
//});
//$(document).on('click', '.BtnQueryResolved', function () {
//    ShowCommentModal("@Model.ERApplications.Id", $(this).attr('data-title'), "@Url.Action("AjaxAdd","Comment")", $(this).attr('data-page'), $(this).attr('data-query-id'));
//});



