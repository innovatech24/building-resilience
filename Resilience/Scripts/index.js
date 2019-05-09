
function validate() {
    
    // Ask for the password
    var entry = prompt("Enter validation code:", "");

    // Validated with the code in the server. Execute the code after the ajax finish.
    $.when(validateEntryAjax(entry)).done(function (response) {
        
        // Call again with the new response
        if (!response) {
            location.reload();
        } else {
            // Make body visible and reload page
            bd.hidden = false;
            location.reload();
        };
    });

};

// Function to call controller
function validateEntryAjax(text) {

    return $.ajax({
        url: '/Home/Validate',
        type: 'POST',
        data: {
            entry: text
        }
    });

};