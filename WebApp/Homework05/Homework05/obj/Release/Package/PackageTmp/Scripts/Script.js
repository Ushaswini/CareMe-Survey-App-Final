
$(document).ready(function () {

    var tokenKey = 'accessToken';

    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });

    $("#logout").click(function (e) {
        e.preventDefault();

        
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'POST',
            url: '/api/Account/Logout',
            headers: headers
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            // self.user('');
            window.location.href = yourApp.Urls.homeScreenUrl;
            sessionStorage.removeItem(tokenKey);
            $("#sidebar-wrapper").css('display', 'none');
            $("#lblGreetings").css('display', 'none');

        }).fail(showError);
    })
});