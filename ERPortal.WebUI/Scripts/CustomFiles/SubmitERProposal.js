import { YesNo, HydrocarbonType, StyleClass, HydrocarbonMethodProposed, ImplementationType, AlertColors, ReturnData } from './Types.js'; // Import Types.js File
import { GetUploadFilesData, alertModal, CompareTwoDates, CurrentDate, CompareTwoDatesByYear } from './CommenMethods.js';
import { StatusData } from './CommentClasses.js';

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
    DateOfLastComProd: '#ERApplications_DateOfLastCommercialProduction',
    TechnicallyCompatible: '#ERApplications_ERPilot_TechnicallyCompatible',
    PilotDesign: '#ERApplications_ERPilot_PilotDesign',
    PilotStartDate: '#ERApplications_ERPilot_PilotStartDate',
    PilotEndDate: '#ERApplications_ERPilot_PilotEndDate',
    FullFillStartDate: '#ERApplications_ERPilot_FullFillStartDate',
    FullFillEndDate: '#ERApplications_ERPilot_FullFillEndDate',
    BtnERAppEligibility: '#BtnERAppEligibility',
    ERAppSubmit: '#ERAppSubmit',
    ERScreeningInstituteId: '#ERApplications_ERScreeningDetail_ERScreeningInstituteId',
    EORTechniqueId: '#EORTechniqueId',
    EGRTechniqueId: '#EGRTechniqueId',
    UHCProductionMethodId: '#ERApplications_UHCProductionMethodId',
    operatorform: '#operatorform',


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
    DateOfLastCommProdDiv: $("#DateOfLastCommercialProductionDiv"),
    PilotDetailDiv: $("#PilotDetailDiv"),
    FullFilledDiv: $("#FullFilledDiv"),

};

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
    if ($(ElementsId.ERApplicationId).val() != "") {
        $('#disabledForm').prop('disabled', true)
        $(ElementsId.ERAppSubmit).hide();
        $('.statussuccess').empty().append('Application Ref. No. : ' + $(ElementsId.ERApplicationId).val()).removeClass('d-none');
        GetUploadFilesData('#UploadFilesData', $(ElementsId.ReportDocument).val());
        setTimeout(() => {
            $('.fileDelete').addClass('d-none');
        });
    }
    else {
        $('select option[value=""]').prop('selected', true);
        $(ElementsId.DateOfDiscovery).val('');

        DivId.FileDiv.removeClass('d-none');
    }



});


$(document).on('change', ElementsId.HydrocarbonType, ({ currentTarget }) => {
    ChangeHydrocarbonType(currentTarget);
});

$(document.body).on('change', ElementsId.HydrocarbonMethod, ({ currentTarget }) => {
    // console.log(currentTarget.selectedOptions[0].text);
    ChangeHydrocarbonMethod(currentTarget);
});

$(document).on('change', ElementsId.ImplementationType, ({ currentTarget }) => {
    ChangeImplementationType(currentTarget);
});

$(document).on('change', ElementsId.DateOfDiscovery, ({ currentTarget }) => {
    let st = ChangeDateOfDiscovery(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});
$(document).on('change', ElementsId.DateOfInitialComProd, ({ currentTarget }) => {
    let st = ChangeDateOfInitComProd(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
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
    let st = ChangeDateOfLastComProd(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});

$(document).on('change', ElementsId.FieldOIIP, ({ currentTarget }) => {
    let st = ChangeFieldOIIP(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg, AlertColors.Warning);
    return st.Status;
});
$(document).on('change', ElementsId.FieldGIIP, ({ currentTarget }) => {
    let st = ChangeFieldGIIP(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg, AlertColors.Warning);
    return st.Status;
});

$(document).on("change", ElementsId.FirstOrderScreening, ({ currentTarget }) => {
    let st = ChangeFirstOrderScreening(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg, AlertColors.Danger);
    return st.Status;
});

$(document).on("change", ElementsId.SecondOrderScreening, ({ currentTarget }) => {
    let st = ChangeSecondOrderScreening(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});

$(document).on("change", ElementsId.ThirdOrderScreening, ({ currentTarget }) => {
    let st = ChangeThirdOrderScreening(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});
$(document).on("change", ElementsId.PilotDesign, ({ currentTarget }) => {
    let st = ChangePilotDesign(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});

$(document).on("change", ElementsId.PilotEndDate, ({ currentTarget }) => {
    let st = ChangePilotEndDate(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});
$(document).on("change", ElementsId.FullFillStartDate, ({ currentTarget }) => {
    let st = ChangeFullFillStartDate(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});
$(document).on("change", ElementsId.FullFillEndDate, ({ currentTarget }) => {
    let st = ChangeFullFillEndDate(currentTarget);
    if (st.Msg.length > 0)
        alertModal(st.Msg);
    return st.Status;
});

$(document).on('click', ElementsId.BtnERAppEligibility, (e) => {
    e.preventDefault()
    CheckEligibilityToSubmit(e.currentTarget)
        .then(data => {
            if (data[1] == false) {
                console.log(data[0]);
                alertModal(data[0], AlertColors.Danger);
                $(ElementsId.ERAppSubmit).addClass('d-none');
                return false;
            }
            else {
                console.log(true);
                $(ElementsId.ERAppSubmit).removeClass('d-none');
                $(ElementsId.BtnERAppEligibility).addClass('d-none');
                return true;
            }
        }).catch(() => {
            alertModal("Something Is Wrong Try Again");
            return false;
        });
});
////////////////
//  Function  //
////////////////
const BtnEligibleOrSubmitVisible = () => {
    if ($(ElementsId.ERAppSubmit).hasClass('d-none') == false) {
        $(ElementsId.ERAppSubmit).addClass('d-none');
        $(ElementsId.BtnERAppEligibility).removeClass('d-none');
    }
};
const ChangeHydrocarbonType = (currentTarget) => {

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
            ReturnData.Msg = "";
            ReturnData.Status = true;
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
            ReturnData.Msg = "";
            ReturnData.Status = true;
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
            ReturnData.Msg = "Choose Type of Hydrocarbon";
            ReturnData.Status = false;
            break;
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeHydrocarbonMethod = (currentTarget) => {
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
            ReturnData.Msg = "";
            ReturnData.Status = true;
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
            ReturnData.Msg = "";
            ReturnData.Status = true;

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
            ReturnData.Msg = "Choose Type of Hydrocarbon Method Propose";
            ReturnData.Status = false;
            break;
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeImplementationType = (currentTarget) => {
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
            $(ElementsId.FieldOIIP).prop('disabled', false);
            $(ElementsId.FieldGIIP).prop('disabled', true);
            ReturnData.Msg = "";
            ReturnData.Status = true;
            break;
        case ImplementationType.EGR:
            DivId.EGRTechniquesDiv.fadeIn("slow");
            if (DivId.uhcProdnMethodDiv.is(":visible")) {
                DivId.uhcProdnMethodDiv.fadeOut("slow");
                DivId.uhcProdnMethodDiv.find('select option[value=""]').prop('selected', true);
            }
            $(ElementsId.FieldOIIP).prop('disabled', true);
            $(ElementsId.FieldGIIP).prop('disabled', false);
            ReturnData.Msg = "";
            ReturnData.Status = true;
            break;
        case ImplementationType.UHC:
            DivId.uhcProdnMethodDiv.fadeIn("slow");
            ReturnData.Msg = "";
            ReturnData.Status = true;
            return ReturnData;
            break;
        case "":
            ReturnData.Msg = "Choose Incentive Sought for Implementation";
            ReturnData.Status = false;
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
            ReturnData.Msg = "";
            ReturnData.Status = true;
            break;
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeDateOfDiscovery = (currentTarget) => {
    let datestatus = false;
    ReturnData.Msg = null;
    if (currentTarget.value != "" && currentTarget.value != undefined) {

        datestatus = CompareTwoDates(CurrentDate(), currentTarget.value);

        if (DivId.ImplementaionTypeDiv.find('select option[value="4"]:selected').val() == "4" && datestatus == true) {
            console.log(currentTarget.value);
            datestatus = CompareTwoDates("2018/10/10", currentTarget.value);
            console.log(datestatus);

            if (datestatus == false) {
                // currentTarget.value = "";
                ReturnData.Status = datestatus;
                ReturnData.Msg = "Not Eligible For Fiscal Incentive. Because Date Of Discovery > Date: 10/10/2018. For Choosing UHC Extraction";

            }
            else {
                ReturnData.Status = datestatus;
                ReturnData.Msg = "Eligible For Fiscal Incentive";

            }

        }
        else if (datestatus == true) {
            ReturnData.Status = datestatus;
            ReturnData.Msg = "Eligible For Fiscal Incentive";

        }
        else {

            ReturnData.Status = datestatus;
            ReturnData.Msg = "Date Of Discovery Greater Then Or Equal To Current Date.";
        }
    }
    else {
        ReturnData.Status = datestatus;
        ReturnData.Msg = "Date Of Discovery Is Empty";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeDateOfInitComProd = (currentTarget) => {
    let statusval = false;
    ReturnData.Msg = null;
    if (currentTarget.value != "") {
        statusval = CompareTwoDates(currentTarget.value, $(ElementsId.DateOfDiscovery).val());

        if (statusval == true) {

            if ($(ElementsId.HydrocarbonType).val() == HydrocarbonType.Conventional &&
                $(ElementsId.HydrocarbonMethod).val() == HydrocarbonMethodProposed.Oil &&
                $(ElementsId.ImplementationType).val() == ImplementationType.EOR) {
                let st = false;
                st = CompareTwoDatesByYear(currentTarget.value, CurrentDate(), 3);
                if (st == true) {
                    console.log(st);
                    ReturnData.Status = st;
                    ReturnData.Msg = "Project/Proposal Is Eligible For Availing Fiscal Incentive(s) as Entittled For EOR Method";

                }
                else {
                    // currentTarget.value = "";
                    ReturnData.Status = st;
                    ReturnData.Msg = "Project/Proposal Is Not Eligible For Availing Fiscal Incentive(s) as Entittled For EOR Method";

                }
            }
            console.log(statusval);
            ReturnData.Status = statusval;
            ReturnData.Msg = "";

        }
        else {
            //currentTarget.value = "";
            console.log(statusval);
            ReturnData.Status = statusval;
            ReturnData.Msg = "Date Of Initial Commercial Production > Date Of Discovery";

        }

    }
    else {
        ReturnData.Status = statusval;
        ReturnData.Msg = "Date of Commencement of Commercial Production Is Empty";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeDateOfLastComProd = (currentTarget) => {
    let d1 = currentTarget.value;
    let d2 = $(ElementsId.DateOfInitialComProd).val();
    let statusval = false;
    if (currentTarget.value != "") {
        statusval = CompareTwoDates(d1, d2);
        if (statusval == true) {
            statusval = CompareTwoDatesByYear(d1, d2, 3);
            if (statusval == false) {
                // currentTarget.value = "";
                ReturnData.Status = statusval;
                ReturnData.Msg = "Not Eligible For Fiscal Incentive. Because Date of most recent Commercial Production Greater Then Or Equal To 3 Years ( >= 3) From Date of Commencement of Commercial Production.";

            }
            else {
                ReturnData.Status = statusval;
                ReturnData.Msg = "Eligible For Fiscal Incentive";

            }
        }
        ReturnData.Status = statusval;
        ReturnData.Msg = "Date of most recent Commercial Production < Date of Commencement of Commercial Production.";

    }
    else {
        ReturnData.Status = statusval;
        ReturnData.Msg = "Date of most recent Commercial Production Is Empty.";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeFieldGIIP = (currentTarget) => {
    if (parseFloat(currentTarget.value) < 0.25 && parseFloat(currentTarget.value) > 0) {
        ReturnData.Status = false;
        ReturnData.Msg = "Pilot Phase Is Not Mandatory. Because If GIIP < 0.25 TCF.";

    }
    else if (parseInt(currentTarget.value) < 0) {
        currentTarget.value = 0;
        ReturnData.Status = false;
        ReturnData.Msg = "";

    }
    else if (parseFloat(currentTarget.value)) {
        ReturnData.Status = true;
        ReturnData.Msg = "Pilot Phase Is Mandatory. Because If GIIP < 0.25 TCF.";

    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Field GIIP Is Empty";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};

const ChangeFieldOIIP = (currentTarget) => {
    if (parseInt(currentTarget.value) < 25 && parseInt(currentTarget.value) > 0) {
        ReturnData.Status = false;
        ReturnData.Msg = "Pilot Phase Is Not Mandatory. Because If OIIP < 25 MMBBL";

    }
    else if (parseInt(currentTarget.value) < 0) {
        currentTarget.value = 0;
        ReturnData.Status = false;
        ReturnData.Msg = "";

    }
    else if (parseInt(currentTarget.value) >= 25) {
        ReturnData.Status = true;
        ReturnData.Msg = "Pilot Phase Is Mandatory. Because If OIIP < 25 MMBBL";

    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Field OIIP Is Empty";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeFirstOrderScreening = (currentTarget) => {
    if (currentTarget.value == YesNo.No) {
        //   DivId.FirstOrderScrTextDiv.removeClass('d-none');
        $(ElementsId.BtnERAppEligibility).addClass('d-none');
        currentTarget.value = "";
        ReturnData.Msg = "1st Order Screening Mandatory";
        ReturnData.Status = false;
    }
    else if (currentTarget.value == YesNo.Yes) {
        DivId.FirstOrderScrTextDiv.addClass('d-none').find('textarea').val('');
        $(ElementsId.BtnERAppEligibility).removeClass('d-none');
        ReturnData.Status = true;
        ReturnData.Msg = "";
    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Choose First Order Screening ";
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeSecondOrderScreening = (currentTarget) => {

    switch (currentTarget.value) {
        case YesNo.No:
            DivId.SecondOrderScrTextDiv.removeClass('d-none');
            if (DivId.SecondOrderScrTextDiv.find('textarea').val() == "") {
                ReturnData.Msg = "Comments Mandatory If No";
                ReturnData.Status = false;
            } else {
                ReturnData.Msg = "";
                ReturnData.Status = true;
            }
            break;
        case YesNo.Yes:
            DivId.SecondOrderScrTextDiv.addClass('d-none').find('textarea').val('');
            ReturnData.Status = true;
            ReturnData.Msg = "";
            break;
        default:
            DivId.SecondOrderScrTextDiv.addClass('d-none').find('textarea').val('');
            ReturnData.Status = false;
            ReturnData.Msg = "Choose Second Order Screening ";
            break;
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeThirdOrderScreening = (currentTarget) => {
    switch (currentTarget.value) {
        case YesNo.No:
            DivId.ThirdOrderScrTextDiv.removeClass('d-none');
            if (DivId.ThirdOrderScrTextDiv.find('textarea').val() == "") {
                ReturnData.Msg = "Third Order Screening Comments Mandatory If No";
                ReturnData.Status = false;
            }
            else {
                ReturnData.Msg = "";
                ReturnData.Status = true;
            }
            break;
        case YesNo.Yes:
            DivId.ThirdOrderScrTextDiv.addClass('d-none').find('textarea').val('');
            ReturnData.Status = true;
            ReturnData.Msg = "";
            break;
        default:
            DivId.ThirdOrderScrTextDiv.addClass('d-none').find('textarea').val('');
            ReturnData.Status = false;
            ReturnData.Msg = "Choose Third Order Screening ";
            break;
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangePilotDesign = (currentTarget) => {
    let FieldGIIP = $(ElementsId.FieldGIIP);
    let FieldOIIP = $(ElementsId.FieldOIIP);
    let st = ReturnData;
    switch (currentTarget.value) {
        case YesNo.No:
            if (parseInt(FieldOIIP.val()) > 0 && FieldOIIP.val().length > 0) {
                st = ChangeFieldOIIP(FieldOIIP[0]);
            }
            else if (parseFloat(FieldGIIP.val()) > 0 && FieldGIIP.val().length > 0) {
                st = ChangeFieldGIIP(FieldGIIP[0]);
            }
            else {
                st.Msg = "Hydrocarbon In Place : OIIP or GIIP Empty";
                st.Status = false;
            }
            break;
        case YesNo.Yes:
            st.Msg = "Pilot Detail Mandatory and Full Field Mandatory";
            st.Status = true;

            st.Mandatory.push("Pilot");
            st.Mandatory.push("FullField");

            $(DivId.PilotDetailDiv).addClass('RequirdData');
            $(DivId.FullFilledDiv).addClass('RequirdData');
            break;
        default:
            st.Msg = "Choose Pilot Design Carried Out";
            st.Status = false;
            $(DivId.PilotDetailDiv).removeClass('RequirdData');
            $(DivId.FullFilledDiv).removeClass('RequirdData');
            break;
    }

    if (st.Status == true) {
        st.Msg = "Pilot Mandatory and Full Field Mandatory";
        st.Mandatory.push("Pilot");
        st.Mandatory.push("FullField");
        $(DivId.PilotDetailDiv).addClass('RequirdData');
        $(DivId.FullFilledDiv).addClass('RequirdData');
    }
    else {
        st.Msg = "Pilot Mandatory and Full Field Is Optional";
        st.Mandatory.push("Pilot");
        $(DivId.PilotDetailDiv).addClass('RequirdData');
        $(DivId.FullFilledDiv).removeClass('RequirdData');
    }
    BtnEligibleOrSubmitVisible();
    return st;
};
const ChangePilotEndDate = (currentTarget) => {
    let PilotStartDate = $(ElementsId.PilotStartDate);
    if (currentTarget.value != "" && PilotStartDate.val() != "") {
        let st = CompareTwoDates(currentTarget.value, PilotStartDate.val());
        if (st == false) {
            ReturnData.Status = st;
            ReturnData.Msg = "Pilot Phase Commencement Date < Pilot Phase Culmination Date";

        }
        else {
            ReturnData.Status = st;
            ReturnData.Msg = "";

        }
    }
    else {
        ReturnData.Status = false
        ReturnData.Msg = "Pilot Phase Commencement Date Or Pilot Phase Culmination Date Are Empty";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeFullFillStartDate = (currentTarget) => {
    let PilotEndDate = $(ElementsId.PilotEndDate);
    if (currentTarget.value != "" && PilotEndDate.val() != "") {
        let st = CompareTwoDates(currentTarget.value, PilotEndDate.val());
        if (st == false) {
            ReturnData.Status = st;
            ReturnData.Msg = "Pilot Phase Culmination Date < Full Field Commencement Date";

        }
        else {
            ReturnData.Status = st;
            ReturnData.Msg = "";

        }
    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Pilot Phase Culmination Date or Full Field Commencement Date Are Empty";
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeFullFillEndDate = (currentTarget) => {
    let FullFillStartDate = $(ElementsId.FullFillStartDate);
    if (currentTarget.value != "" && FullFillStartDate.val() != "") {
        let st = CompareTwoDates(currentTarget.value, FullFillStartDate.val());
        if (st == false) {
            ReturnData.Status = st;
            ReturnData.Msg = "Full Field Commencement Date < Full Field Culmination Date";

        }
        else {
            ReturnData.Status = st;
            ReturnData.Msg = "";

        }
    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Full Field Culmination Date or Full Field Commencement Date Are Empty";
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;

};
const ChangeERScreeningIntitute = (currentTarget) => {
    if (currentTarget.value != "" || currentTarget.value == undefined) {
        ReturnData.Status = true;
        ReturnData.Msg = "";

    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Choose ER Screening Institute";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeEORTechnique = (currentTarget) => {
    if (currentTarget.value != "" || currentTarget.value == undefined) {
        ReturnData.Status = true;
        ReturnData.Msg = "";

    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Choose EOR Technique";

    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeEGRTechnique = (currentTarget) => {
    if (currentTarget.value != "" || currentTarget.value == undefined) {
        ReturnData.Status = true;
        ReturnData.Msg = "";
    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Choose EGR Technique";
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const ChangeUHCProductionMethod = (currentTarget) => {
    if (currentTarget.value != "" || currentTarget.value == undefined) {
        ReturnData.Status = true;
        ReturnData.Msg = "";
    }
    else {
        ReturnData.Status = false;
        ReturnData.Msg = "Choose UHC Production Methods.";
    }
    BtnEligibleOrSubmitVisible();
    return ReturnData;
};
const CheckEligibilityToSubmit = async (e) => {
    let arr = [];
    let printdata = "";
    let returndt = "";
    let status = true;

    if ($(ElementsId.HydrocarbonType + " option:selected").val() == "") {
        ReturnData.Msg = "Choose Type of Hydrocarbon";
        ReturnData.Status = false;
        arr.push(new StatusData(ReturnData));
    }
    if ($(ElementsId.HydrocarbonType + " option:selected").val() == HydrocarbonType.Conventional) {
        if ($(ElementsId.HydrocarbonMethod + " option:selected").val() == "") {
            ReturnData.Msg = "Choose Type of Hydrocarbon Method Propose";
            ReturnData.Status = false;
            arr.push(new StatusData(ReturnData));
        }
    }
    if ($(ElementsId.ImplementationType + " option:selected").val() != "") {
        if ($(ElementsId.ImplementationType + " option:selected").val() == ImplementationType.EOR) {
            arr.push(new StatusData(await ChangeEORTechnique($(ElementsId.EORTechniqueId)[0])));
        }
        if ($(ElementsId.ImplementationType + " option:selected").val() == ImplementationType.EGR) {
            arr.push(new StatusData(await ChangeEGRTechnique($(ElementsId.EGRTechniqueId)[0])));
        }
        if ($(ElementsId.ImplementationType + " option:selected").val() == ImplementationType.UHC) {
            arr.push(new StatusData(await ChangeUHCProductionMethod($(ElementsId.UHCProductionMethodId)[0])));
        }
    }
    else {
        ReturnData.Msg = "Choose Incentive Sought for Implementation";
        ReturnData.Status = false;
        arr.push(new StatusData(ReturnData));
    }


    arr.push(new StatusData(await ChangeDateOfDiscovery($(ElementsId.DateOfDiscovery)[0])));
    arr.push(new StatusData(await ChangeDateOfInitComProd($(ElementsId.DateOfInitialComProd)[0])));
    arr.push(new StatusData(await ChangePilotDesign($(ElementsId.PilotDesign)[0])));

    if ($(ElementsId.PresentlyUnderProduction).val() == YesNo.No) {
        arr.push(new StatusData(await ChangeDateOfLastComProd($(ElementsId.DateOfLastComProd)[0])));

    }
    if (DivId.PilotDetailDiv.hasClass("RequirdData")) {
        arr.push(new StatusData(await ChangePilotEndDate($(ElementsId.PilotEndDate)[0])));
    }
    if (DivId.FullFilledDiv.hasClass("RequirdData")) {
        arr.push(new StatusData(await ChangeFullFillStartDate($(ElementsId.FullFillStartDate)[0])));
        arr.push(new StatusData(await ChangeFullFillEndDate($(ElementsId.FullFillEndDate)[0])));
    }
    arr.push(new StatusData(await ChangeERScreeningIntitute($(ElementsId.ERScreeningInstituteId)[0])));
    arr.push(new StatusData(await ChangeFirstOrderScreening($(ElementsId.FirstOrderScreening)[0])));
    arr.push(new StatusData(await ChangeSecondOrderScreening($(ElementsId.SecondOrderScreening)[0])));
    arr.push(new StatusData(await ChangeThirdOrderScreening($(ElementsId.ThirdOrderScreening)[0])));

    for (let st of arr) {
        if (st.Status == false) {
            printdata += `<li>${st.Msg}</li>`;
            status = st.Status;
        }
    }

    returndt += printdata.length > 0 ? `<h5 class="text-primary">Mandatory Fields</h5><ol class="text-danger">${printdata}</ol>` : "";
    return [returndt, status];
};






