

function confirmLogout(event) {
    event.preventDefault();  // Ngừng hành động mặc định (chuyển trang)

    // Hiển thị SweetAlert để xác nhận đăng xuất
    Swal.fire({
        title: 'Bạn có chắc muốn đăng xuất?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Đăng xuất',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/Account/Login/Logout';
        }
    });
}

