$(document).ready(function () {

    window.onbeforeunload = function () {
        return true;
    };

    // Create range of emotions
    var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];

    // Set div with rating
    $(".rating").emotionsRating({

        // Back ground emoticon
        bgEmotion: 'happy',

        // Set array of emoticons
        emotions: emotionsArray,

        // Everytime the value change, the hiden object binded to the model is updated
        onUpdate: function (value) {
            document.getElementById("MenteeFeedback").value = value;
        }
    });

});

// Just to omit the confirmation to leave the page when the create button is pressed
function btnSubmit() {
    window.onbeforeunload = null;
};