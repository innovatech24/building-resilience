
$(document).ready(function () {
    window.gauge = createGauge(0, "gaugediv");

    updateGauge(window.gauge, "#inputest");

});


$("#highlight").click(function () {
    var chkbox = $(this)[0];

    if (chkbox.checked == true) {
        highlightText("#inputest", true);
    } else {
        highlightText("#inputest", false);
    }

});


$("#inputest").keypress(function (e) {
    if (e.key === ' ' || e.key === 'Spacebar') {
        updateGauge(window.gauge, "#inputest");
    };
});