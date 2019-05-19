
// Create range of emotions
var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];

$(document).ready(function () {


    // Get list of rating divs
    var ratingDis = document.getElementsByClassName("ratingDisabled");

    // Loop over rating divs
    for (var i = 0; i < ratingDis.length; i++) {

        // Get element
        element = ratingDis.item(i);
        buildDisabledRating(element);
    };


});

function buildDisabledRating(element) {
    // Configure rating on each element

    $(element).emotionsRating({
        bgEmotion: 'happy',
        emotions: emotionsArray,
        disabled: true,
        count: 1,
        initialRating: element.getAttribute("rate")
    });
};
