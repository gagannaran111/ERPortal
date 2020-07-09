import { ToChangeDateFormate } from './CommenMethods.js';
import { statusDeck, filterData } from './Types.js';
$(document).ready(() => {

    UserTableData();
});
let table;
const UserTableData = () => {
    $.ajax({
        url: "../DGH/DGHDashboard", //"@Url.Action("DGHDashboard","DGH")",
        type: 'POST',
        async: false,
        success: (result) => {

            console.log(result);
            let dd = "";
            let status = "";
            let newappcount = 0;
            let approvalappcount = 0;
            let rejectappcount = 0;
            let underproccessingappcount = 0;
            let pendingappcount = 0;
            let totalappcount = 0;
            for (let e of result) {
                var files = "";
                if (e.ApprovalLetter != null) {
                    for (let e1 of e.ApprovalLetter) {

                        files += '<a class="btn btn-sm btn-primary text-truncate" style="max-width: 120px;" href="../Content/UploadedFiles/' + e1.FileName + '" target="_blank"><i class="fas fa-file-download"></i> ' + e1.FileName + '</a>';
                    };
                }
                status = e.Status == "AP" ? "<kbd class='bg-success'>" + statusDeck.statusAP + "</kbd>"
                    : e.Status == "NA" ? "<kbd class='bg-dark'>" + statusDeck.statusPWM + "</kbd>"
                        : e.Status == "PWM" ? "<kbd class='bg-dark'>" + statusDeck.statusPWM + "</kbd>"
                            : e.Status == "UP" ? "<kbd class='bg-info'>" + statusDeck.statusUP + "</kbd>"
                                : "<kbd class='bg-Warning'>Revert Back</kbd>";

                e.Status == "AP" ? approvalappcount++
                    : e.Status == "NA" ? newappcount++
                        : e.Status == "PWM" ? pendingappcount++
                            : e.Status == "UP" ? underproccessingappcount++
                                : 0;
                totalappcount = approvalappcount + newappcount + pendingappcount + underproccessingappcount;
                dd += `<tr><td><a href='../DGH/AppRecToDGH?appid=${e.AppId}'>${e.AppId}</a></td>
                        <td> ${e.FieldType.Type}</td><td>${e.FieldName}</td>
                        <td>${e.Organisation.Name}</td><td>${ToChangeDateFormate(e.CreatedAt)}</td>
                        <td>${files}</td><td>${status}</td></tr>`;
            };
            $("#usertable tbody").html(dd);
            $('.newapp').html(newappcount);
            $('.underproccessingapp').html(underproccessingappcount);
            $('.rejectapp').html(rejectappcount);
            $('.approvedapp').html(approvalappcount);
            $('.pendingwithmeapp').html(pendingappcount);
            $('.Allapp').html(totalappcount);
            table = $('#usertable').DataTable(
                {
                    scrollY: '50vh',
                    scrollCollapse: true,
                    paging: true,
                    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    //processing: true,
                    // deferLoading: 5
                })
        },
        error: () => {
            alertModal("Something Went Wrong. Try Again Later");
        },
        fail: (xhr, textStatus, errorThrown) => {
            alertModal("Something Went Wrong. Try Again Later");
        }
    })
};

$(document).on('click', '.card-deck .card', ({ currentTarget }) => {
    let DatasetAttrVal = currentTarget.dataset;
    filterData.statustext = DatasetAttrVal.val;
    filterData.type = "Status";
    table.draw();
});
$.fn.dataTable.ext.search.push((settings, data, dataIndex) => {
    switch (filterData.type) {
        case "Status":
           let Status = data[6];
            filterData.statustext = filterData.statustext == statusDeck.statusAll ? "" : filterData.statustext;
            return Status == filterData.statustext || filterData.statustext == "" ? true : false;
        default: return false;
    }

})

