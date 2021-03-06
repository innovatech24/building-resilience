﻿
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
        count:1,
        initialRating: element.getAttribute("rate")
    });
};

// Set div with rating
$(".ratingEnabled").emotionsRating({

    // Back ground emoticon
    bgEmotion: 'happy',
    count: 5,
    // Set array of emoticons
    emotions: emotionsArray,

    onUpdate: function (value) {
        $(this.element).attr("rate", value);

        iid = $(this.element).attr("iid");

        $.ajax({
            url: '/Exercises/setExerciseMenteeRate',
            type: 'POST',
            data: {
                Id: iid,
                rate: value
            }
        });
    }

});