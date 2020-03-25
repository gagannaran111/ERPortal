$(document).ready(() => {

    let CurrentTab = sessionStorage.getItem('CurrentTab');
    if (CurrentTab != null) {
        $('a[href="' + CurrentTab + '"]').click();
    }

    $('#formModal').on('show.bs.modal', (event) => {
        let button = $(event.relatedTarget); // Button that triggered the modal
        let targetPage = button.data('page'); // Extract page redirection from data-* attributes
        let modalTitle = button.data('title');// Extract Modal title from data-* attributes
        let btnval = button.data('value');
        // Initiate an AJAX request here (and then updating in a callback)
        $("#formModalLabel").html("Add " + modalTitle);
        $.ajax({
            url: "/Admin/AjaxAdd?targetPage=" + targetPage,
            data: { Id: btnval },
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


    $('.table').DataTable(
        {            
            scrollCollapse: true,
            paging: true,
            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        });

});

$(document).on('submit', '#myForm', (e) => {
    // stop default form submission
    e.preventDefault();
    let formUrl = $('#myForm').attr('action');
    $.ajax({
        url: formUrl,
        type: 'POST',
        data: $('#myForm').serialize(),
        success: (result) => {
            if ('Success' == result) {
                $('#modalContent').html('<div class="alert alert-success" role="alert"> Successfully Added </div >');
                if ($("#saveButton").is(":visible")) {
                    $('#saveButton').fadeOut();
                }
            } else {
                $('#modalContent').html(result);
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

$(document).on('click', '.DeleteData', ({ currentTarget }) => {
    let DatasetAttrVal = currentTarget.dataset;
    console.log(DatasetAttrVal);
    let target = DatasetAttrVal.target; //$(this).attr('data-target');
    let id = DatasetAttrVal.value;// $(this).attr('data-value');
    $.ajax({
        url: "/Admin/DeleteData", // "@Url.Action("DeleteData","Admin")",
        type: 'POST',
        data: { targetdata: target, Id: id },
        success: (result) => {
            alert(result);
        },
        error: () => {
        },
        fail: (xhr, textStatus, errorThrown) => { }

    });

});

$(document).on('click', '.nav-tabs a', ({ currentTarget }) => {
    console.log(currentTarget.hash);
    sessionStorage.setItem('CurrentTab', currentTarget.hash);
});
