$(document).ready(function () {

    highlightText(".entry", true);

    window.gauge = createGauge(0, "gaugediv");

    //var sentiment = @Html.Raw(Json.Encode(Model.DiaryEntries.SentimentScore));
    //updateGauge(window.gauge, "#inputest");   

    updateGaugeValue(window.gauge, sentiment);

    //updateGaugeValue(window.gauge, parseFloat(sentiment));
});


$("#highlight").click(function () {

    var chkbox = $(this)[0];

    highlightText(".entry", chkbox.checked);
    
});

