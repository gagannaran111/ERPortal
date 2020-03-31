import { YesNo, HydrocarbonType, StyleClass, HydrocarbonMethodProposed, ImplementationType, AlertColors } from './Types.js'; // Import Types.js File
import { GetUploadFilesData, alertModal, CompareTwoDates, CurrentDate, CompareTwoDatesByYear } from './CommenMethods.js';

const ElementsId = {
    ERApplicationId: '#ERApplications_AppId',
    DateOfInitialComProd: '#ERApplications_DateOfInitialCommercialProduction',
    DateOfDiscovery: '#ERApplications_DateOfDiscovery',
    ReportDocument: '#ReportDocument',
    FieldGIIP: '#ERApplications_FieldGIIP',
    FieldOIIP: '#ERApplications_FieldOIIP',
    ImplementationType: '#ERApplications_ImplementaionType',
    HydrocarbonType: '#ERApplications_HydrocarbonType',
    HydrocarbonMethod: '#ERApplications_HydrocarbonMethod',
    FirstOrderScreening: '#ERApplications_ERScreeningDetail_FirstOrderScreening',
    SecondOrderScreening: '#ERApplications_ERScreeningDetail_SecondOrderScreening',
    ThirdOrderScreening: '#ERApplications_ERScreeningDetail_ThirdOrderScreening',
    PresentlyUnderProduction: '#ERApplications_PresentlyUnderProduction',
    DateOfLastComProd: '#ERApplications_DateOfLastCommercialProduction'
};

const DivId = {
    uhcProdnMethodDiv: $("#uhcProdnMethodDiv"),
    MethodProposedDiv: $("#MethodProposedDiv"),
    ImplementaionTypeDiv: $("#ImplementaionTypeDiv"),
    EORTechniquesDiv: $("#EORTechniquesDiv"),
    EGRTechniquesDiv: $("#EGRTechniquesDiv"),
    HydrocarbonTypeChangeDiv: $("#HydrocarbonTypeChangeDiv"),
    UploadFileDataDiv: $('#UploadFilesData'),
    FileDiv: $('#FileDiv'),
    FirstOrderScrTextDiv: $("#FirstOrderScrTextDiv"),
    SecondOrderScrTextDiv: $("#SecondOrderScrTextDiv"),
    ThirdOrderScrTextDiv: $("#ThirdOrderScrTextDiv"),
    DateOfLastCommProdDiv: $("#DateOfLastCommercialProductionDiv")
};

const OnSuccess = (response) => {

    console.log(response);
    if (response == "Success")
        window.location.href = '/Operator/Index';
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
            $(this).addClass(StyleClass.OIL);
        }
        else if ($(this).val() == "4") {
            $(this).addClass(StyleClass.UHC);
        }
        else if ($(this).val() == "2" || $(this).val() == "3") {
            $(this).addClass(StyleClass.GAS);
        }
        else {

        }
    });
    DivId.ImplementaionTypeDiv.find('select option').prop('hidden', true);

    $('[data-toggle="tooltip"]').tooltip();
    if ($('#ERApplications_AppId').val() != "") {
        $('#disabledForm').prop('disabled', true)
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



});


$(document).on('change', ElementsId.HydrocarbonType, ({ currentTarget }) => {

    switch (currentTarget.value) {
        case HydrocarbonType.UnConventional:
            //  DivId.uhcProdnMethodDiv.fadeIn("slow");
            DivId.MethodProposedDiv.fadeOut("slow");

            DivId.HydrocarbonTypeChangeDiv.find('select option[value=""]').prop('selected', true);
            DivId.ImplementaionTypeDiv.find('select option').prop('hidden', false);

            if (DivId.EGRTechniquesDiv.is(":visible")) {
                DivId.EGRTechniquesDiv.fadeOut("slow");
                DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            if (DivId.EORTechniquesDiv.is(":visible")) {
                DivId.EORTechniquesDiv.fadeOut("slow")
                DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            break;
        case HydrocarbonType.Conventional:
            DivId.MethodProposedDiv.fadeIn("slow");
            DivId.uhcProdnMethodDiv.fadeOut("slow");
            DivId.EGRTechniquesDiv.fadeOut("slow");
            DivId.EORTechniquesDiv.fadeOut("slow");
            DivId.HydrocarbonTypeChangeDiv.find('select option[value=""]').prop('selected', true);

            DivId.MethodProposedDiv.find('select option[value="2"]').addClass('d-none');
            DivId.ImplementaionTypeDiv.find('select option').prop('hidden', true);

            if (DivId.EGRTechniquesDiv.is(":visible")) {
                DivId.EGRTechniquesDiv.fadeOut("slow");
                DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            if (DivId.EORTechniquesDiv.is(":visible")) {
                DivId.EORTechniquesDiv.fadeOut("slow")
                DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }

            break;
        default:
            DivId.HydrocarbonTypeChangeDiv.find('select option[value=""]').prop('selected', true);
            DivId.ImplementaionTypeDiv.find('select option').prop('hidden', true);


            if (DivId.uhcProdnMethodDiv.is(":visible"))
                DivId.uhcProdnMethodDiv.fadeOut("slow");
            if (DivId.MethodProposedDiv.is(":visible"))
                DivId.MethodProposedDiv.fadeOut("slow");
            if (DivId.EGRTechniquesDiv.is(":visible"))
                DivId.EGRTechniquesDiv.fadeOut("slow");
            if (DivId.EORTechniquesDiv.is(":visible"))
                DivId.EORTechniquesDiv.fadeOut("slow");

            break;
    }
});

$(document.body).on('change', ElementsId.HydrocarbonMethod, ({ currentTarget }) => {
    // console.log(currentTarget.selectedOptions[0].text);
    switch (currentTarget.value) {
        case HydrocarbonMethodProposed.Oil:
            $('.' + StyleClass.GAS).prop('hidden', true);
            $('.' + StyleClass.OIL).prop('hidden', false);
            $('.' + StyleClass.UHC).prop('hidden', true);

            DivId.ImplementaionTypeDiv.find('select option[value=""]').prop('selected', true);
            if (DivId.EGRTechniquesDiv.is(":visible")) {
                DivId.EGRTechniquesDiv.fadeOut("slow");
                DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            if (DivId.EORTechniquesDiv.is(":visible")) {
                DivId.EORTechniquesDiv.fadeOut("slow")
                DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }

            $('#ERApplications_FieldGIIP').prop('disabled', true);
            $('#ERApplications_FieldGIIP').val('');
            $('#ERApplications_FieldOIIP').prop('disabled', false);
            break;
        case HydrocarbonMethodProposed.Gas:
            $('.' + StyleClass.OIL).prop('hidden', true);
            $('.' + StyleClass.GAS).prop('hidden', false);
            $('.' + StyleClass.UHC).prop('hidden', true);

            DivId.ImplementaionTypeDiv.find('select option[value=""]').prop('selected', true);
            if (DivId.EGRTechniquesDiv.is(":visible")) {
                DivId.EGRTechniquesDiv.fadeOut("slow");
                DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            if (DivId.EORTechniquesDiv.is(":visible")) {
                DivId.EORTechniquesDiv.fadeOut("slow")
                DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            $('#ERApplications_FieldGIIP').prop('disabled', false);
            $('#ERApplications_FieldOIIP').prop('disabled', true);
            $('#ERApplications_FieldOIIP').val('');
            break;
        default:
            DivId.ImplementaionTypeDiv.find('select option').prop('hidden', true);
            DivId.ImplementaionTypeDiv.find('select option[value=""]').prop('selected', true);

            if (DivId.EGRTechniquesDiv.is(":visible")) {
                DivId.EGRTechniquesDiv.fadeOut("slow");
                DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            if (DivId.EORTechniquesDiv.is(":visible")) {
                DivId.EORTechniquesDiv.fadeOut("slow")
                DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            break;
    }
});

$(document).on('change', ElementsId.ImplementationType, ({ currentTarget }) => {
    if (DivId.EGRTechniquesDiv.is(":visible")) {
        DivId.EGRTechniquesDiv.fadeOut("slow");
        DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
    }
    if (DivId.EORTechniquesDiv.is(":visible")) {
        DivId.EORTechniquesDiv.fadeOut("slow")
        DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
    }
    switch (currentTarget.value) {

        case ImplementationType.EOR:
            DivId.EORTechniquesDiv.fadeIn("slow");
            if (DivId.uhcProdnMethodDiv.is(":visible")) {
                DivId.uhcProdnMethodDiv.fadeOut("slow");
                DivId.uhcProdnMethodDiv.find('select option[value=""]').prop('selected', true);
            }
            break;
        case ImplementationType.EGR:
            DivId.EGRTechniquesDiv.fadeIn("slow");
            if (DivId.uhcProdnMethodDiv.is(":visible")) {
                DivId.uhcProdnMethodDiv.fadeOut("slow");
                DivId.uhcProdnMethodDiv.find('select option[value=""]').prop('selected', true);
            }
            break;
        case ImplementationType.UHC:
            DivId.uhcProdnMethodDiv.fadeIn("slow");
            break;
        default:
            if (DivId.EGRTechniquesDiv.is(":visible")) {
                DivId.EGRTechniquesDiv.fadeOut("slow");
                DivId.EGRTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            if (DivId.EORTechniquesDiv.is(":visible")) {
                DivId.EORTechniquesDiv.fadeOut("slow")
                DivId.EORTechniquesDiv.find('select option[value=""]').prop('selected', true);
            }
            break;
    }
});

//switch (currentTarget.value) {
//    case "0":
//        break;
//    case "2":
//        break;
//    default:
//        break;
//}
// DateOfDiscovery

$(document).on('change', ElementsId.DateOfDiscovery, ({ currentTarget }) => {

    let datestatus = false;

    if (currentTarget.value.length == "10") {
        datestatus = CompareTwoDates(CurrentDate(), currentTarget.value) == false ?
            alertModal("Date Of Discovery Greater Then Or Equal To  Current Date.", AlertColors.Warning) : true;

        if (DivId.ImplementaionTypeDiv.find('select option[value="4"]:selected').val() == "4" && datestatus == true) {
            console.log(currentTarget.value);
            datestatus = CompareTwoDates("2018/10/10", currentTarget.value);
            console.log(datestatus);

            if (datestatus == false) {
                currentTarget.value = "";
                alertModal("Not Eligible For Fiscal Incentive. Because Date Of Discovery > Date: 10/10/2018. For Choosing UHC Extraction", AlertColors.Info);

            }
            else {
                alertModal("Eligible For Fiscal Incentive", AlertColors.Success);
            }

        }
        else if (datestatus == true) {
            alertModal("Eligible For Fiscal Incentive", AlertColors.Success);
        }
        else {
            currentTarget.value = "";
        }
    }
    //CheckEligibleToFillERForm();
});
$(document).on('change', ElementsId.DateOfInitialComProd, ({ currentTarget }) => {
    let statusval = false;
    if (currentTarget.value != "") {
        statusval = CompareTwoDates(currentTarget.value, $(ElementsId.DateOfDiscovery).val());
        console.log(statusval + " dd");
        if (statusval == true) {

            if ($(ElementsId.HydrocarbonType).val() == HydrocarbonType.Conventional &&
                $(ElementsId.HydrocarbonMethod).val() == HydrocarbonMethodProposed.Oil &&
                $(ElementsId.ImplementationType).val() == ImplementationType.EOR) {
                let st = false;
                st = CompareTwoDatesByYear(currentTarget.value, CurrentDate(), 3);
                if (st == true) {
                    console.log(st);
                    alertModal("Project/Proposal Is Eligible For Availing Fiscal Incentive(s) as Entittled For EOR Method")
                    return st;
                }
                else {
                    currentTarget.value = "";
                    console.log(st);
                    alertModal("Project/Proposal Is Not Eligible For Availing Fiscal Incentive(s) as Entittled For EOR Method");
                    return st;
                }
            }
            console.log(statusval);
            return statusval;
        }
        else {
            currentTarget.value = "";
            alertModal('Date Of Initial Commercial Production > Date Of Discovery');
            console.log(statusval);
            return statusval;
        }

    }
});

$(document).on("change", ElementsId.PresentlyUnderProduction, ({ currentTarget }) => {

    console.log(currentTarget);
    if ($(ElementsId.DateOfInitialComProd).val() != '') {
        if (currentTarget.value == YesNo.No) {
            DivId.DateOfLastCommProdDiv.fadeIn("slow");
        } else if (currentTarget.value == YesNo.Yes) {
            // CheckERScreeningEligibility();
            DivId.DateOfLastCommProdDiv.fadeOut("slow");
            $(ElementsId.DateOfLastComProd).val('');
        }
    }
    else {
        alertModal("Select Date of Commencement of Commercial Production", AlertColors.Warning);
        currentTarget.value = '';
    }
});

$(document).on('change', ElementsId.DateOfLastComProd, ({ currentTarget }) => {
    let d1 = currentTarget.value;
    let d2 = $(ElementsId.DateOfInitialComProd).val();
    let statusval = false;
    if (currentTarget.value != "") {
        statusval = CompareTwoDates(d1, d2);
        if (statusval == true) {
            statusval = CompareTwoDatesByYear(d1, d2, 3);
            if (statusval == false) {
                currentTarget.value = "";
                alertModal("Not Eligible For Fiscal Incentive")
                return statusval;
            }
            else {
                alertModal("Eligible For Fiscal Incentive");
                return statusval;
            }
        }
        return statusval;
    }
    else {
        return statusval;
    }
});

$(document).on('change', ElementsId.FieldOIIP, () => {

    checkMandatoryPilot();
});
$(document).on('change', ElementsId.FieldGIIP, () => {

    //  checkMandatoryPilot();
});

$(document).on("change", ElementsId.FirstOrderScreening, ({ currentTarget }) => {
    console.log(currentTarget);
    currentTarget.value == YesNo.No ? DivId.FirstOrderScrTextDiv.removeClass('d-none')
        : DivId.FirstOrderScrTextDiv.addClass('d-none').find('textarea').val('');
});

$(document).on("change", ElementsId.SecondOrderScreening, ({ currentTarget }) => {
    currentTarget.value == YesNo.No ? DivId.SecondOrderScrTextDiv.removeClass('d-none')
        : DivId.SecondOrderScrTextDiv.addClass('d-none').find('textarea').val('');
});

$(document).on("change", ElementsId.ThirdOrderScreening, ({ currentTarget }) => {
    console.log(currentTarget.value);
    currentTarget.value == YesNo.No ? DivId.ThirdOrderScrTextDiv.removeClass('d-none')
        : DivId.ThirdOrderScrTextDiv.addClass('d-none').find('textarea').val('');
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
    let diffYear = 0;
    let diffmonth = 0;
    let diffdate = 0;
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
    let diffYear = 0;
    let diffmonth = 0;
    let diffdate = 0;
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

const CheckEligibilityToSubmitForm = () => {


};





