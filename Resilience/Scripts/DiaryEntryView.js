
$(document).ready(function () {
    
    //highlightText("#entry", true);
    highlightText("#entry", true);

    // Create gauge plot in div 'gaugediv'
    window.gauge = createGauge(0, "gaugediv","Sentiment",true);

    // Set value of the plot to the sentiment score value
    updateGaugeValue(window.gauge, sentiment);

    // Create emoticon rating
    var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];
    $("#rating").emotionsRating({
        bgEmotion: 'happy',
        emotions: emotionsArray,
        disabled: true,
        count:1,
        initialRating: document.getElementById("rating").getAttribute("rate")
    });
});


// Checkbox to turn on and off the highlighting
$("#highlight").click(function () {

    // Get value of checkbox
    var chkbox = $(this)[0];

    // Highlight if true
    if (chkbox.checked == true) {
        highlightText("#entry", true);

    //Un-highlight otherwise 
    } else {
        $("#entry").highlightWithinTextarea('destroy');
    }

});

