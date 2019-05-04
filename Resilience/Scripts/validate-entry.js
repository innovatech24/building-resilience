$.ajax({
    url: '/Home/ValidateEntry',
    type: 'POST',
    success: function (validated) {
        if (!validated) {
            window.location.replace("/");
        };
    }
});