(function () {
    window.onload = function () {
        var preloader = document.querySelector('.cs-page-loading');
        preloader.classList.remove('active');
    };
})();
$(document).ajaxSend(function () {
    $(".cs-page-loading").addClass("ajax_active");
});
$(document).ajaxComplete(function () {
    $(".cs-page-loading").removeClass("ajax_active");
});

function ajaxLoading(loading = "on") {
    if (loading == "on") {
        $(".cs-page-loading").addClass("ajax_active");
    }
    else {
        $(".cs-page-loading").removeClass("ajax_active");
    }
}