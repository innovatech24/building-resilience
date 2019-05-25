
// Create range of emotions
var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];

$(document).ready(function () {

  
    showMessage("completionTask", message);
 

    // Set div with rating
    $(".ratingEnabled").emotionsRating({

        // Back ground emoticon
        bgEmotion: 'happy',

        // Set array of emoticons
        emotions: emotionsArray,

        onUpdate: function (value) {
            $(this.element).attr("rate", value);

            iid = $(this.element).attr("iid");

            $.ajax({
                url: '/Goals/SetGoalMenteeRate',
                type: 'POST',
                data: {
                    Id: iid,
                    rate: value
                }
            });
        }

    });

    // Get list of rating divs
    var ratingDis = document.getElementsByClassName("ratingDisabled");

    // Loop over rating divs
    for (var i = 0; i < ratingDis.length; i++) {

        // Get element
        element = ratingDis.item(i);
        buildDisabledRating(element);
    };


});

function buildDisabledRating(element){
    // Configure rating on each element

    $(element).emotionsRating({
        bgEmotion: 'happy',
        emotions: emotionsArray,
        disabled: true,
        count:1,
        initialRating: element.getAttribute("rate")
    });
};

function showMessage(obj, messageObj) {
    if (messageObj != "") {

        // Convert message from the server into json and get div element
        var message = JSON.parse(messageObj.replace(/&quot;/g, '"'));
        var element = document.getElementById(obj);

        // Modify div name with message data
        element.classList.add("alert-" + message.Type);
        var s = document.createElement("strong");
        s.appendChild(document.createTextNode(message.Title));
        element.appendChild(s);
        element.appendChild(document.createTextNode(" " + message.Message));
        element.hidden = false;    
    };
};
