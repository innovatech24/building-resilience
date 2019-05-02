
function validate() {
    
    // Initialize response
    res = false;

    // Start validation process
    validateEntry(res);

};

// Recursive function calling itself if the response is false
function validateEntry(validation) {

    // While validation is false
    if (!validation) {

        // Ask for the password
        var entry = prompt("Enter validation code:", "");

        // Validated with the code in the server. Execute the code after the ajax finish.
        $.when(validateEntryAjax(entry)).done(function (response) {

            // Call again with the new response
            validateEntry(response);
        });
    } else {
        // Make body visible and reload page
        bd.hidden = false;
        location.reload();
    };
}

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