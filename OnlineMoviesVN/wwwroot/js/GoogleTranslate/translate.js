$(document).ready(function () {
    // Kiểm tra xem cookie lang có tồn tại không
    let lang = $.cookie('lang') || 'vi';  // Nếu không có cookie lang, mặc định là 'vi'

    // Nếu không có cookie lang, tạo mới với giá trị mặc định là 'vi'
    if (!$.cookie('lang')) {
        $.cookie('lang', 'vi', { path: '/' });  // Tạo cookie lang với phạm vi toàn cục
    }

    // Hiển thị cờ ngôn ngữ tương ứng ngay khi tải trang
    let flagElement = $('.lang_show[data-lang="' + lang + '"]');

    // Ẩn tất cả các cờ ngôn ngữ và chỉ hiển thị cờ ngôn ngữ tương ứng
    $('.lang_show').hide();
    flagElement.show();

    // Cập nhật Google Translate với cả 2 ngôn ngữ (en và vi)
    new google.translate.TranslateElement({
        pageLanguage: 'vi',  // Ngôn ngữ mặc định của trang là 'vi'
        includedLanguages: 'en,vi',  // Các ngôn ngữ có sẵn là tiếng Anh và tiếng Việt
        layout: google.translate.TranslateElement.FloatPosition.TOP_LEFT
    }, 'google_translate_element');
});

// Hàm thay đổi ngôn ngữ và lưu vào cookie
function changelang(theLang) {
    $.cookie('lang', theLang, { path: '/' });  // Lưu ngôn ngữ vào cookie với phạm vi toàn cục
    $('.goog-te-combo').val(theLang)[0].dispatchEvent(new Event('change'));  // Cập nhật ngôn ngữ Google Translate
    let act = $('.lang_show[data-lang=' + theLang + ']').show();  // Hiển thị cờ tương ứng
    $('.lang_show').not(act).hide();  // Ẩn các cờ khác
}

// Hàm khởi tạo Google Translate khi trang được tải
function googleTranslateElementInit() {
    let defaultLang = $.cookie('lang') || 'vi';
    new google.translate.TranslateElement({
        pageLanguage: defaultLang,  // Ngôn ngữ mặc định của trang
        includedLanguages: 'en,vi',  // Các ngôn ngữ có sẵn là tiếng Anh và tiếng Việt
        layout: google.translate.TranslateElement.FloatPosition.TOP_LEFT
    }, 'google_translate_element');
}

// Khởi tạo Google Translate khi trang được tải
$(function () {
    let l = $.cookie('lang');
    if (l) {
        let act = $('.lang_show[data-lang=' + l + ']').show();
        $('.lang_show').not(act).hide();
    }
});

// Đảm bảo dropdown hoạt động khi người dùng thay đổi ngôn ngữ
$(".dropdown.notranslate").click(function () {
    $(this).toggleClass("open");
});
