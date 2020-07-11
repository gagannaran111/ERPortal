import { PATTERNS, DateDiff } from './Types.js';

export const CurrentDate = () => {
    let currdate = new Date();
    let year = currdate.getFullYear();
    let month = currdate.getMonth() + 1;
    let date = currdate.getDate()
    return year + "/" + month + "/" + date;
};
const RemoveClass = (Id) => {
    let classArr = $(Id).attr("class").split(" ")
    $(Id).attr("class", "")
    for (let alertcolor of classArr) {         
        // some condition/filter
        if (alertcolor.substr(0, 5) != "alert") {
            $(Id).addClass(alertcolor);
        }        
    }
}
export const alertModal = (msg, color) => {
    $('#btnalertmodal').click();
    RemoveClass("#alertmodelheader");
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
            : DateDiff.diffYear < 0 ? false
                : DateDiff.diffmonth > 0 && DateDiff.diffYear == 0 ? true
                    : DateDiff.diffmonth < 0 ? false
                        : DateDiff.diffdate > 0 && DateDiff.diffmonth == 0 ? true : false;

        console.log(DateDiff.diffYear, DateDiff.diffmonth, DateDiff.diffdate, valnull);
    }
    else {
        return false;
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

        //valnull = DateDiff.diffYear > y ? true :
        //    dateDiff.diffYear == 3 ?
        //        DateDiff.diffmonth > 0 ? true :
        //            DateDiff.diffmonth == 0 ? false :
        //                DateDiff.diffdate > 0 ? true :
        //                    DateDiff.diffdate < 0 ? false : false;
        //return valnull;

        if (DateDiff.diffYear > 3) {
            valnull = true;
        }
        else if (DateDiff.diffYear == 3) {
            if (DateDiff.diffmonth > 0) {
                valnull = true;
            }
            else if (DateDiff.diffmonth == 0) {
                if (DateDiff.diffdate > 0) {
                    valnull = true;
                }
                else
                    valnull = false;
            }
            else {
                valnull = false;
            }
        }
        else {
            valnull = false;
        }

    }
    return valnull;
}
export const  GetUploadFilesData = (divId, refid) => {
    $(divId).html("No files attached");
    let sendData = { RefId: refid };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: "../Comment/GetUploadFiles",
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
                    <a class='text-dark' href='../Content/Uploads/${file.NewFileName}' target='_blank'>${file.FileName}</a>
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

export const ShowCommentModal = (appid, title, urlpath, targetpage, queryid) => {
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