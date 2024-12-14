$(document).ready(function () {
    $.ajax({
        url: '/admin/users/GetUserActive',
        method: 'GET',
        success: function (data) {
            $('#user-table').html(data)
        },
        error: function (e) {
            console.log(e.messageText)
        }
    })
})