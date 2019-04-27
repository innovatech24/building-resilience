
$(document).ready(function () {
    highlightText(".entry", true);
    window.gauge = createGauge(0, "gaugediv");

    //updateGauge(window.gauge, "#inputest");
    updateGaugeValue(window.gauge, parseFloat($(".sentiment", this).text().trim()));

});


$("#highlight").click(function () {
    var chkbox = $(this)[0];

    if (chkbox.checked == true) {
        highlightText(".entry", true);
    } else {
        highlightText(".entry", false);
    }

});