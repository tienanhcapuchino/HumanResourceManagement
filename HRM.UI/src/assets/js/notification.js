$(function () {
    $(document).ajaxStart(() => {
        $("#loadingSpinner").show();
    });
    $(document).ajaxStop(() => {
        $("#loadingSpinner").hide();
    });
    $.ajax({
        url: "http://localhost:5085/api/notification/getall",
        method: "GET",
        contentType: "application/json",
        success: function (response) {
            $("#notification-bell").empty();
            let notificationHtml = `<h6 class="p-3 mb-0">Notifications</h6>`;
            notificationHtml += response.datas
                .map((notify) => {
                    return `<div class="dropdown-divider"></div>
                <a class="dropdown-item preview-item">
                    <div class="preview-thumbnail">
                    <div class="preview-icon bg-success">
                        <i class="mdi mdi-calendar"></i>
                    </div>
                    </div>
                    <div class="preview-item-content d-flex align-items-start flex-column justify-content-center">
                    <h6 class="preview-subject font-weight-normal mb-1">${notify.content}</h6>
                    <p class="text-gray ellipsis mb-0">${notify.publishedTime}</p>
                    </div>
                </a>`;
                })
                .join("");
            notificationHtml += `<div class="dropdown-divider"></div>`;

            $("#hiring-anni-value").empty();
            $("#hiring-anni-value").append(`${response.totalAnniveralDays} people`);

            $("#notification-bell").append(notificationHtml);
        },
        error: function (xhr) {
            console.error(xhr.responseText);
        },
    });
});
