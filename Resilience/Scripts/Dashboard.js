
// Create range of emotions
var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];

$(document).ready(function () {

    $("#goalTitle").click(function () {
        window.location.href = "Goals/MentorView/" + $(this).getAttribute("iid");
    });

    // Get list of rating divs
    var ratingDis = document.getElementsByClassName("ratingDisabled");

    // Loop over rating divs
    for (var i = 0; i < ratingDis.length; i++) {

        // Get element
        element = ratingDis.item(i);
        buildDisabledRating(element);
    };

    $(".linkbox").click(function (e) {
        
        if (e.target.className != "apending") {
            window.location.href = this.getAttribute("ref") + this.getAttribute("iid");
        }
    });

});

function buildDisabledRating(element) {
    // Configure rating on each element

    $(element).emotionsRating({
        bgEmotion: 'happy',
        emotions: emotionsArray,
        disabled: true,
        count: 1,
        initialRating: element.getAttribute("rate"),
        emotionSize:20
    });
};

/*
jQuery(".card").click(function (e) {     
    //window.location = jQuery(this).find("a").attr("href");
    return false;
});*/