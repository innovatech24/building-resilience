

$(document).ready(function () {
    showMessage("addMentorMessage", message);

    $.ajax({
        url: '/Users/getUser',
        type: 'POST',
        data: {
            id: uid
        },
        success: function (response) {

            var user = JSON.parse(response);
            
            if (user.Mentor != null) {
                var element = document.getElementById("MentorInfo");
                
                var p1 = document.createElement("strong");
                p1.appendChild(document.createTextNode(user.Mentor.FirstName + " " + user.Mentor.LastName + " - " + user.Mentor.EmailAddress))
                element.appendChild(p1);
                element.hidden = false;
            }
        }
    });
});

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

        // If success redirect to mentees options
        if (message.Type == "success") {
            setTimeout(() => { window.location.href ="../Options/Mentee"; }, 1500);
        };
    };
};