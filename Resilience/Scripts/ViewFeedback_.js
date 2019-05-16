$(document).ready(function () {

    // Get list of rating divs
    var rates = document.getElementsByClassName("rating");

    // Define range of emoticons
    var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];
    
    // Loop over rating divs
    for (var i = 0; i < rates.length; i++) {

        // Get element
        element = rates.item(i);

        // Configure rating on each element
        $(element).emotionsRating({
            bgEmotion: 'happy',
            emotions: emotionsArray,
            disabled: true,
            count: 1,
            initialRating: element.getAttribute("rate")
        });
    };

});

