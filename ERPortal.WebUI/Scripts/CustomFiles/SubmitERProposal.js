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
        window.location.href = '/Operator/Index'; //"@Url.Action("Index","Operator")";
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
        $('select option[value=""]').prop('selected', true);
        $('#ERApplications_DateOfDiscovery').val('');

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
