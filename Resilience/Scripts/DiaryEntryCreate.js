$(document).ready(function () {

    /*$(".rating").rateYo({
        precision: 0,
        onSet: function (rating, rateYoInstance) {
            document.getElementById("MenteeFeedback").value = rating;

        }
    });*/

    var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];

    $(".rating").emotionsRating({

        bgEmotion: 'happy',
        emotions: emotionsArray,
        onUpdate: function (value) {
            document.getElementById("MenteeFeedback").value = value;
        }
    });

});