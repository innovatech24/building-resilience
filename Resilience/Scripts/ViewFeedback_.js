
$(document).ready(function () {

    // Create emoticon rating
    var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];
    $("#rating").emotionsRating({
        bgEmotion: 'happy',
        emotions: emotionsArray,
        disabled: true,
        count: 1,
        initialRating: document.getElementById("rating").getAttribute("rate")
    });
});