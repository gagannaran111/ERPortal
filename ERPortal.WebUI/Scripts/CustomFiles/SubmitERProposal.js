const OnSuccess = (response) => {
    //alertModal(response);

    //$('#operatorform').find('input,select,radio').prop('disabled', true);
    //$('#ERAppSubmit').hide();
    //$('.statussuccess').empty().append(response).removeClass('d-none');
    // setTimeout(function () {
    //        $('.fileDelete, .FileDiv').addClass('d-none');
    //    }, 500);

    console.log(response);
    if (response == "Success")
        window.location.href = "@Url.Action("Index","Operator")";
        else {
    alert("Fill Comments If You Select Order Screening : No");
    return false;
}
    }

const OnFailure = (response) => {
    console.log(response);
    alertModal("Try Again Error Occured.");
}
$(document).ready(() => {

    $.each($('#ERApplications_ImplementaionType option'), function (index, element) {
        if ($(this).val() == "1" || $(this).val() == "0") {
            $(this).addClass('oil');
        }
        else if ($(this).val() == "4" || $(this).val() == "") {

        }
        else {
            $(this).addClass('gas');
        }
    });


    $('[data-toggle="tooltip"]').tooltip();
    if ($('#ERApplications_AppId').val() != "") {
        $('#disabledForm').prop('disabled', true)
        //$('#operatorform').find('input,select,radio').prop('disabled', true);
        $('#ERAppSubmit').hide();
        $('.statussuccess').empty().append('Application Ref. No. : ' + $('#ERApplications_AppId').val()).removeClass('d-none');
        GetUploadFilesData('#UploadFilesData', $('#ReportDocument').val());
        setTimeout(() => {
            $('.fileDelete').addClass('d-none');
        });
    }
    else {

        $('#ERApplications_DateOfDiscovery').val('');
        $("#ERApplications_ImplementaionType option[value='']").attr('selected', true);
        $('#FileDiv').removeClass('d-none');
    }


    $(document.body).on('change', '#ERApplications_HydrocarbonType', ({ currentTarget }) => {

        console.log(currentTarget.selectedOptions[0].text);
        let selectedOption = currentTarget.selectedOptions[0].text;
        let actionDiv = $("#uhcProdnMethodDiv");

        if ("UHC" == selectedOption && actionDiv.is(":hidden")) {
            actionDiv.val('').fadeIn("slow");
            $('#ERApplications_FieldGIIP').prop('disabled', false);
            $('#ERApplications_FieldOIIP').prop('disabled', false);
            $('.oil').prop('hidden', false);
            $('.gas').prop('hidden', false);
        } else if (actionDiv.is(":visible")) {
            actionDiv.val('').fadeOut("slow");
            $("#uhcProdnMethodDiv option[value='']").attr('selected', true);
        }

        if ($('#ERApplications_HydrocarbonType option:selected').val() == '0') {
            $('.gas').prop('hidden', true);
            $('.oil').prop('hidden', false);
            $('#ERApplications_FieldGIIP').prop('disabled', true);
            $('#ERApplications_FieldGIIP').val('');
            $('#ERApplications_FieldOIIP').prop('disabled', false);
            $("#ERApplications_ImplementaionType option[value='']").attr('selected', true);
        }
        else if ($('#ERApplications_HydrocarbonType option:selected').val() == '1') {
            $('.oil').prop('hidden', true);
            $('.gas').prop('hidden', false);
            $('#ERApplications_FieldGIIP').prop('disabled', false);
            $('#ERApplications_FieldOIIP').prop('disabled', true);
            $('#ERApplications_FieldOIIP').val('');
            $("#ERApplications_ImplementaionType option[value='']").attr('selected', true);
        }
    });

    $(document).on("change", "input[name='ERApplications.ERScreeningStatus']", ({ currentTarget }) => {
        let actionDiv = $('#FileDiv');

        if (currentTarget.value == "True") {
            $("#opertor1").click();
            actionDiv.val('').fadeIn("slow");
        } else if (actionDiv.is(":visible")) {
            actionDiv.val('').fadeOut("slow");
            $("#FileDiv option[value='']").attr('selected', true);
        }
    });


    $('#formModal').on('show.bs.modal', (event) => {
        let button = $(event.relatedTarget); // Button that triggered the modal
        let targetPage = button.data('page'); // Extract page redirection from data-* attributes
        let modalTitle = button.data('title');// Extract Modal title from data-* attributes
        // Initiate an AJAX request here (and then updating in a callback)
        $("#formModalLabel").html("Add " + modalTitle);
        $.ajax({
            url: "/Operator/AjaxAdd?targetPage=" + targetPage,
            success: (result) => {
                $('#modalContent').html(result);
                if ($("#saveButton").is(":hidden")) {
                    $('#saveButton').show();
                }
            },
            error: () => {
                $('#modalContent').html('<div class="alert alert-danger" role="alert"> An Error has occured </div >');
                if ($("#saveButton").is(":visible")) {
                    $('#saveButton').fadeOut();
                }
            },
            fail: (xhr, textStatus, errorThrown) => {
                $('#modalContent').html('<div class="alert alert-danger" role="alert">Request Failed with error: ' + errorThrown + '</div > ');
                if ($("#saveButton").is(":visible")) {
                    $('#saveButton').fadeOut();
                }
            }
        });
    });
    $('#formDetailModal').on('show.bs.modal', (event) => {
        let button = $(event.relatedTarget); // Button that triggered the modal
        let targetPage = button.data('page'); // Extract page redirection from data-* attributes
        let modalTitle = button.data('title');// Extract Modal title from data-* attributes
        let RefId = button.attr('form-ref-id');
        // Initiate an AJAX request here (and then updating in a callback)
        $("#formModalLabelView").html(modalTitle);
        $.ajax({
            url: "/Operator/AjaxViewDetails?targetPage=" + targetPage + "&&RefId=" + RefId,
            success: (result) => {
                $('#modalContentView').html(result);
                if (targetPage == "ERScreeningDetailView") {

                    GetUploadFilesData('#DetailViewFile', $('#eRScreeningDetail_ReportDocumentPath').val());
                    setTimeout(() => {
                        $('.fileDelete').remove();
                    }, 500);
                }
            },
            error: () => {
                $('#modalContentView').html('<div class="alert alert-danger" role="alert"> An Error has occured </div >');

            },
            fail: (xhr, textStatus, errorThrown) => {
                $('#modalContentView').html('<div class="alert alert-danger" role="alert">Request Failed with error: ' + errorThrown + '</div > ');

            }
        });
    });

    $(document).on('submit', '#myForm', (e) => {
        // stop default form submission
        e.preventDefault();

        // if ($('#UploadFilesData').find('div').length != 0 && ) {

        let formUrl = $('#myForm').attr('action');

        $.ajax({
            url: formUrl,
            type: 'POST',
            data: $('#myForm').serialize(),
            success: (result) => {
                let res = result.split(",");
                if ('Success' == res[0]) {
                    if (res[2] == "ERScreeningDetail") {

                        $('#ERApplications_ERScreeningDetailId').val(res[1]);
                        erscreeninglinkshowhideonload();

                    }

                    $('#modalContent').html('<div class="alert alert-success" role="alert"> Successfully Added </div >');
                    if ($("#saveButton").is(":visible")) {
                        $('#saveButton').fadeOut();
                    }
                } else {
                    $('#modalContent').html(result);
                }
            },
            error: (result) => {

                $('#modalContent').html(result);
            },
            fail: (xhr, textStatus, errorThrown) => {
                $('#modalContent').html('<div class="alert alert-danger" role="alert">Request Failed with error: ' + errorThrown + '</div > ');
                if ($("#saveButton").is(":visible")) {
                    $('#saveButton').fadeOut();
                }
            }


        });
        // }
        //  else {
        //      return false;
        //  }

    });

    $(document).on("change", "input[name='ERApplications.PresentlyUnderProduction']", ({ currentTarget }) => {
        let actionDiv = $("#DateOfLastCommercialProductionDiv");
        console.log(currentTarget);
        if ($('#ERApplications_DateOfInitialCommercialProduction').val() != '') {
            if (currentTarget.value == "False") {
                actionDiv.val('').fadeIn("slow");
            } else if (currentTarget.value == "True") {
                CheckERScreeningEligibility();
                actionDiv.val('').fadeOut("slow");
                $('input[name="ERApplications.DateOfLastCommercialProduction"]').val('');
            }
        }
        else {
            alertModal("Select Date of Commencement of Commercial Production");
            currentTarget.checked = false;
        }
    });
    $(document).on('change', 'input[name="ERApplications.DateOfLastCommercialProduction"]', ({ currentTarget }) => {
        let d1 = $('#ERApplications_DateOfLastCommercialProduction');
        let d2 = $('#ERApplications_DateOfInitialCommercialProduction');

        let msg = "";
        if (currentTarget.value != "") {
            let statusval = CompareTwoDates(d1, d2);
            statusval == false ? alertModal(msg) : CheckERScreeningEligibility();
            return statusval;
        }
        else {
            return false;
        }
    });

    $(document).on('change', '#ERApplications_DateOfInitialCommercialProduction', ({ currentTarget }) => {
        let d1 = $('#ERApplications_DateOfInitialCommercialProduction');
        let d2 = $('#ERApplications_DateOfDiscovery');
        let msg = "(Date Of Initial Commercial Production > Date Of Discovery) Or (Date Of Initial Commercial Production Or Date Of Discovery Are Empty.)";
        if (currentTarget.value != "") {
            let statusval = CompareTwoDates(d1, d2);
            statusval == false ? alertModal(msg) : null;
            return statusval;
        }
        else {
            return false;
        }
    });

    $(document).on('change', 'input[name="ERApplications.FieldOIIP"]', () => {

        checkMandatoryPilot();
    });
    $(document).on('change', 'input[name="ERApplications.FieldGIIP"]', () => {

        checkMandatoryPilot();
    });
});

$(document).on('change', '#ERApplications_DateOfDiscovery', () => {
    CheckEligibleToFillERForm();
});
$(document).on('click', '.Querycommenttablink', function () {

    $.ajax({
        url: "@Url.Action("QueryCommentSummary", "Comment", new { appid = Model.ERApplications.Id })",
        type: 'POST',
        success: function (result) {
            @{ string[] arr = Session["UserData"] as string[]; }
            console.log(result);
            if (result.length == 0) {
                $("#QueryCommentTab .body-panel").html('<h3 class="text-danger ">Data Not Found</h3>');
            }
            else {
                var dd = "";
                var userid = "@arr[0]";
                var statuscheck = "";
                var counter = 1;
                var subject = "";
                var replybtn = "";
                var resolvedbtn = "";
                for (let element of result) {
                    if (element[0].Subject != subject) {
                        subject = element[0].Subject;
                        dd += "<div class=''><div class='alert alert-success h5'>Query : " + counter + " Subject : " + subject + "</div>";
                        counter++;
                    }
                    dd += "<div class='alert alert-light border border-danger'><p class='h5 float-right'>Query Between " + element[0].Sender.SenderName + " and " + element[0].Reciver.ReciverName + "</p><hr/>";
                    for (let f of element) {
                        let files = "";
                        for (let e of f.Files) {
                            files += '<a class="badge badge-success mr-2" href="' + e.FilePath + '" target="blank"><i class="fas fa-file-download"></i> ' + e.FileName + '</a>';
                        };

                        if (f.Status == "Query Rasied") {
                            statuscheck = "Query Rasied To " + f.Reciver.ReciverName + " by " + f.Sender.SenderName;

                        }
                        else if (f.Status == "Query Reply") {
                            statuscheck = "Query Reply To " + f.Reciver.ReciverName + " by " + f.Sender.SenderName;
                        }
                        else if (f.Status == "Query Forward") {
                            statuscheck = "Query Forward To " + f.Reciver.ReciverName + " by " + f.Sender.SenderName;
                        }
                        else {
                            statuscheck = "";
                        }
                        if (f.Reciver.ReciverId == userid || f.Sender.SenderId == userid) {
                            replybtn = '<button class="btn btn-sm btn-primary ml-2 BtnQueryReply" data-page="QueryCommentReply" data-query-parentid="' + f.QueryParentId + '" data-query-id="' + f.Id +
                                '" data-toggle="modal" data-target="#CommentModal" data-title="Query Reply"><i class="fas fa-reply"></i> Reply</button>';
                        }
                        else {
                            replybtn = "";
                        }


                        dd += ' <li class="left clearfix "><span class="chat_img pull-left">' +
                            '	<span class="chat_img"> <img src="../Content/img/user-profile.png" alt="User" style="width:50px"> </span>' +
                            '</span><div class="chat-body clearfix"><div class="header"><strong class="text-danger h5">' + statuscheck + '</strong> <small class="pull-right text-muted">' +
                            '<i class="fas fa-clock"></i> ' + ToChangeDateFormate(f.CreatedAt) + '</small></div><div><p><b>Subject :</b> ' + f.Subject + '</p></div>' +
                            '<p><b>Comments :</b> ' + f.Comments + '</p><div class="mb-2"><p>' + files + '</p ></div>' +
                            '<div class="float-right" > <button class="btn btn-sm btn-success BtnQueryForward" data-page="QueryCommentForward" data-query-parentid="' + f.QueryParentId + '" data-query-id="' + f.Id +
                            '" data-toggle="modal" data-target="#CommentModal" data-title="Query Forward"><i class="fas fa-forward"></i> Forward</button>' + replybtn +
                            '</div ></div ></li > ';

                        if (f.Sender.SenderId == userid && f.Status == "Query Rasied") {
                            resolvedbtn = '<div class="alert alert-light"><button class="btn btn-sm btn-info ml-2 BtnQueryResolved" data-page="QueryCommentResolved" data-query-parentid="' + f.QueryParentId + '" data-query-id="' + f.Id +
                                '" data-toggle="modal" data-target="#CommentModal" data-title="Query Resolved"><i class="fas fa-check"></i> Mark As Resolved</button></div>';
                        }
                        else {
                            resolvedbtn = "";
                        }
                    };
                    dd += resolvedbtn + "</div></div>";
                };

                //  var subject = "<div class='col alert alert-info'><h5 class=''>Subject : " + result[1].Subject + "</h4></div>"
                $("#QueryCommentTab .body-panel").html('<ul class="chat">' + dd + '</ul>');
            }
        },
        error: function () {

            alertModal("Something Went Wrong. Try Again Later");
        },
        fail: function (xhr, textStatus, errorThrown) {

            alertModal("Something Went Wrong. Try Again Later");
        }
    });

});

$(document).on("change", "input[name='ERApplications.ERScreeningDetail.FirstOrderScreening']", ({ currentTarget }) => {
    let firstodertext = $('.FirstOrderScrText');
    currentTarget.value == "False" ? firstodertext.removeClass('d-none') : firstodertext.addClass('d-none').find('textarea').val('');
});

$(document).on("change", "input[name='ERApplications.ERScreeningDetail.SecondOrderScreening']", ({ currentTarget }) => {
    let secondodertext = $('.SecondOrderScrText');
    currentTarget.value == "False" ? secondodertext.removeClass('d-none') : secondodertext.addClass('d-none').find('textarea').val('');
});

$(document).on("change", "input[name='ERApplications.ERScreeningDetail.ThirdOrderScreening']", ({ currentTarget }) => {
    console.log(currentTarget.value);
    let thirdodertext = $('.ThirdOrderScrText');
    currentTarget.value == "False" ? thirdodertext.removeClass('d-none') : thirdodertext.addClass('d-none').find('textarea').val('');
});

////////////////
//  Function  //
////////////////

const checkMandatoryPilot = () => {

    let FieldOIIP = $('#ERApplications_FieldOIIP');
    let FieldGIIP = $('#ERApplications_FieldGIIP');
    let pilotprodprofile = $('#ERApplications_PilotProductionProfile');
    let pilotdesign = $('input[name="ERApplications.PilotDesign"]:eq(0)');

    if (FieldGIIP.val() != '' && $('#ERApplications_HydrocarbonType option:selected').val() == '1') {

        if (parseFloat(FieldGIIP.val()) < 0.25) {
            $('.pilotmandatory').attr('data-original-title', 'Not Mandatory');
            pilotprodprofile.removeAttr('required');
            $('input[name="ERApplications.PilotDesign"]').removeAttr('disabled');
        }
        else {
            $('.pilotmandatory').attr('data-original-title', 'Mandatory');
            pilotprodprofile.attr('required', 'required');
            pilotdesign.prop("checked", true);
            $('input[name="ERApplications.PilotDesign"]').attr('disabled', 'disabled');
        }
    }
    else if (FieldOIIP.val() != '' && $('#ERApplications_HydrocarbonType option:selected').val() == '0') {
        if (parseFloat(FieldOIIP.val()) < 25) {
            $('.pilotmandatory').attr('data-original-title', 'Not Mandatory');
            pilotprodprofile.removeAttr('required');
            $('input[name="ERApplications.PilotDesign"]').removeAttr('disabled');
        }
        else {
            $('.pilotmandatory').attr('data-original-title', 'Mandatory');
            pilotprodprofile.attr('required', 'required');
            pilotdesign.prop("checked", true);
            $('input[name="ERApplications.PilotDesign"]').attr('disabled', 'disabled');
        }
    }
    else {
        $('.pilotmandatory').attr('data-original-title', 'If OIIP < 25 MMBBL Or GIIP < 0.25 TCF then Not Mandatory otherwise Mandatory');
    }


}
const CheckEligibleToFillERForm = () => {
    let dateofdiscoveryval = $('#ERApplications_DateOfDiscovery').val().split('/');
    let dateofdiscovery = $('#ERApplications_DateOfDiscovery');
    let currentdate = new Date();
    let diffYear = "";
    let diffmonth = "";
    let diffdate = "";
    let msg = "Date Of Discovery Less Then 3 Years. So You Cannot Fill ER Proposal";

    diffYear = currentdate.getFullYear() - parseInt(dateofdiscoveryval[0]);
    diffmonth = parseInt((currentdate.getMonth() + 1)) - parseInt(dateofdiscoveryval[1]);
    diffdate = parseInt(currentdate.getDate()) - parseInt(dateofdiscoveryval[2]);

    if (diffYear > 3) {
        alertModal('Eligible To Fill ER Proposal');
    }
    else if (diffYear == 3) {
        if (diffmonth >= 0) {
            if (diffdate < 0) {
                alertModal(msg);
                dateofdiscovery.val('');
            }
        }
        else {
            alertModal(msg);
            dateofdiscovery.val('');
        }
    }
    else {
        alertModal(msg);
        dateofdiscovery.val('');
    }

}
const CheckERScreeningEligibility = () => {

    let dateofinitial = $('#ERApplications_DateOfInitialCommercialProduction').val();
    let dateoflastsubmission = $('#ERApplications_DateOfLastCommercialProduction').val();
    let initialdatesplit = dateofinitial.split("/");
    let currentdate = new Date();
    let diffYear = "";
    let diffmonth = "";
    let diffdate = "";
    let statusval = true;
    if (dateoflastsubmission != '') {
        let lastdatesplit = dateoflastsubmission.split("/");
        diffYear = parseInt(lastdatesplit[0]) - parseInt(initialdatesplit[0]);
        diffmonth = parseInt(lastdatesplit[1]) - parseInt(initialdatesplit[1]);
        diffdate = parseInt(lastdatesplit[2]) - parseInt(initialdatesplit[2]);
    }
    else {

        diffYear = currentdate.getFullYear() - parseInt(initialdatesplit[0]);
        diffmonth = parseInt((currentdate.getMonth() + 1)) - parseInt(initialdatesplit[1]);
        diffdate = parseInt(currentdate.getDate()) - parseInt(initialdatesplit[2]);
    }
    if (diffYear > 3) {
        statusval = true;
        alertModal('ERScreening Details Mandatory');
    }
    else if (diffYear == 3) {
        if (diffmonth >= 0) {
            if (diffdate >= 0) {
                statusval = true;
                alertModal('ERScreening Details Mandatory');

            }
            else {
                statusval = false;
                alertModal('ERScreening Details Not Mandatory');

            }
        }
        else {
            statusval = false;
            alertModal('ERScreening Details Not Mandatory');
        }
    }
    else {
        statusval = false;
        alertModal('ERScreening Details Not Mandatory');
    }
    return statusval;
}

const CheckOrderScreening = (e) => {
    let RadFirstOrderId = $('#ERApplications_ERScreeningDetail_FirstOrderScreening');
    let RadSecondOrderId = $('#ERApplications_ERScreeningDetail_SecondOrderScreening');
    let RadThirdOrderId = $('#ERApplications_ERScreeningDetail_ThirdOrderScreening');

};
