function googleTranslateElementInit() {
    new google.translate.TranslateElement({ pageLanguage: 'vi', layout: google.translate.TranslateElement.FloatPosition.TOP_LEFT }, 'google_translate_element');
}
$(function () {
    let l = $.cookie('lang');
    if (l) {
        let act = $('.lang_show[data-lang=' + l + ']').show();
        $('.lang_show').not(act).hide();
    }
});
let languageChanged = false;
function changelang(theLang) {
    console.log(theLang);
    $.cookie('lang', theLang);
    if ($('#lang_select_modal').is(":visible")) {
        $('#lang_select_modal').modal('hide');
    }
    $('.goog-te-combo').val(theLang)[0].dispatchEvent(new Event('change'));
    if (sessionStorage.getItem('lang_selected')) {
        $('.goog-te-combo').val(theLang)[0].dispatchEvent(new Event('change'));
    }
    sessionStorage.setItem('lang_selected', true);
    let act = $('.lang_show[data-lang=' + theLang + ']').show();
    $('.lang_show').not(act).hide();
}

$(".dropdown.notranslate").click(function () {
    $(this).toggleClass("open");
});
var openLg = $(".dropdown.notranslate");
var clickShow = $('.dropdown-lgg');
function addClassLG() {
    openLg.classList.add('open');
}
clickShow.click(addClassLG);


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