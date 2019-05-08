
$(document).ready(function () {
    showMessage("addMessage", message);
   
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
            setTimeout(() => { window.location.href ="../Options/Mentor"; }, 1500);
        };
    };
};