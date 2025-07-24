// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    function loadNotifications() {
        $.ajax({
            url: '/Notification/GetUserNotifications',
            type: 'GET',
            success: function (notifications) {
                var unreadCount = notifications.filter(n => !n.isRead).length;
                if (unreadCount > 0) {
                    $('#notificationDot').removeClass('d-none');
                } else {
                    $('#notificationDot').addClass('d-none');
                }

                var $panelBody = $('#notificationPanel .panel-body');
                $panelBody.empty();
                if (notifications.length === 0) {
                    $panelBody.append('<p class="text-muted">No notifications.</p>');
                } else {
                    notifications.forEach(function (n) {
                        $panelBody.append(
                            `<div class="mb-2${n.isRead ? '' : ' fw-bold'}">
                                <div>${n.title || ''}</div>
                                <div class="small text-muted">${n.message}</div>
                                <div class="small text-secondary">${new Date(n.createdAt).toLocaleString()}</div>
                            </div>`
                        );
                    });
                }
            }
        });
    }

    // Load notifications on page load and when opening the panel
    loadNotifications();
    $('#notificationBell').on('click', loadNotifications);
});
