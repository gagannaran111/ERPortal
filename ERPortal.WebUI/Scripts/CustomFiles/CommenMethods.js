import { PATTERNS, DateDiff } from './Types.js';

export const CurrentDate = () => {
    let currdate = new Date();
    let year = currdate.getFullYear();
    let month = currdate.getMonth() + 1;
    let date = currdate.getDate()
    return year + "/" + month + "/" + date;
};

export const alertModal = (msg, color) => {
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
        DateDiff.diffYear = parseInt(date1[0]) - parseInt(date2[0]);
        DateDiff.diffmonth = parseInt(date1[1]) - parseInt(date2[1]);
        DateDiff.diffdate = parseInt(date1[2]) - parseInt(date2[2]);
        valnull = DateDiff.diffYear > 0 ? true
            : DateDiff.diffmonth > 0 && DateDiff.diffYear == 0 ? true
                : DateDiff.diffdate > 0 && DateDiff.diffmonth == 0 ? true : false;

        console.log(DateDiff.diffYear, DateDiff.diffmonth, DateDiff.diffdate, valnull);
    }
    else {
        return "ERROR";
    }
    return valnull;

}

export const CompareTwoDatesByYear = (d1, d2, y) => {
    let valnull = false;
    if (d1 != "" && d2 != "") {

        let date1 = d1.split('/');
        let date2 = d2.split('/');
        DateDiff.diffYear = parseInt(date1[0]) - parseInt(date2[0]);
        DateDiff.diffmonth = parseInt(date1[1]) - parseInt(date2[1]);
        DateDiff.diffdate = parseInt(date1[2]) - parseInt(date2[2]);

        valnull = DateDiff.diffYear > y ? true :
            DateDiff.diffYear < 0 ? false :
                DateDiff.diffmonth > 0 ? true :
                    DateDiff.diffmonth < 0 ? false :
                        DateDiff.diffdate > 0 ? true :
                            DateDiff.diffdate < 0 ? false : false;
        return valnull;

        //if (diffYear > 3) {
        //    valnull = true;
        //}
        //else if (diffYear == 3) {
        //    if (diffmonth > 0) {
        //        valnull = true;
        //    }
        //    else if (diffmonth == 0) {
        //        if (diffdate > 0) {
        //            valnull = true;
        //        }
        //        else
        //            valnull = false;
        //    }
        //    else {
        //        valnull = false;
        //    }
        //}
        //else {
        //    valnull = false;
        //}

    }
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
                    <a class='fileDelete small text-danger'data-fileid='${file.Id}' data-fileref='${file.FIleRef}' data-divid ='${divId}' href='#'>
                    <i class='ml-1 fa fa-times'></i>
                    </a></div>`;
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