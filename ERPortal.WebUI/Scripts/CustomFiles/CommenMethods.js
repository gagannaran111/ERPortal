import { PATTERNS } from './Types.js';

export const CurrentDate = () => {
    let currdate = new Date();
    let year = currdate.getFullYear();
    let month = currdate.getMonth() + 1;
    let date = currdate.getDate()
    return year + "/" + month + "/" + date;
};

export const alertModal = (msg,color) => {
    $('#btnalertmodal').click();
    $('#alertmodelheader').addClass(color);
    $('#modalContentAlert').html("<strong>" + msg + "</strong>");    
};

export const ToChangeDateFormate = (value) => {

    let results = PATTERNS.DateFormate.exec(value);
    let dt = new Date(parseFloat(results[1]));

    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
}
export const CompareTwoDates = (d1, d2) => {
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
export const GetUploadFilesData = (divId, refid) => {
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
                        txt += `<div class='badge badge-warning mr-2 mb-2'>
                    <a class='text-dark' href='@Url.Content("~/Content/UploadedFiles/")${file.FileName}' target='_blank'>${file.FileName}</a>
                    <a class='fileDelete small text-danger'data-fileid='${file.Id}' data-fileref='${file.FIleRef}' data-divid ='${divId}'  href='#'><i class='ml-1 fa fa-times'></i></a></div>`;
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