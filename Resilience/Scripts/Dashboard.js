jQuery(".tile-content").click(function (e) {     
    window.location = jQuery(this).find("a").attr("href");
    return false;
});