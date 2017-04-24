/*!
 * index view javascript functions
 */
function toggleAllCheckBoxes() {
    if (isAllCheckBoxesChecked() == true) {
        $("input:checkbox").each(function (index, elem) {
            $(elem).prop("checked", false);
        });
    } else {
        $("input:checkbox").each(function (index, elem) {
            $(elem).prop("checked", true);
        });
    }
}
function isAllCheckBoxesChecked() {
    var result = true;
    $("input:checkbox").each(function (index, elem) {
        if ($(elem).is(":checked") == false) { result = false; }
    })
    return result;
}
function viewReport(url, timeout, error, link, loader) {
    error.slideUp(500);
    link.hide();
    loader.fadeIn(500);
    $.ajax({
        url: url,
        type: "GET",
        timeout: timeout,
        success: function (data) {
            window.open(data.Url);
            link.show();
            loader.hide();
        },
        error: function () {
            link.show();
            loader.hide();
            error.slideDown(500);
        }
    });
}
function storePrinterFriendlyData(printerFriendlyUrl, appEngineTimeout) {
    if (printerFriendlyUrl.length > 0) {
        window.localStorage.setItem("printerFriendlyUrl", printerFriendlyUrl);
    };
    if (appEngineTimeout.length > 0) {
        window.localStorage.setItem("appEngineTimeout", appEngineTimeout);
    };
}
function getPrinterFriendlyUrl() {
    return window.localStorage.getItem("printerFriendlyUrl");
}
function getAppEngineTimeout() {
    return window.localStorage.getItem("appEngineTimeout");
}

