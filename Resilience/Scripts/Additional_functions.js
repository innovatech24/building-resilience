
// Creates Gauge plot. Receives initial value for the gauge and the div id where to put it. It returns an object with the chart and the hand.
function createGauge(value, name_div) {

    // Themes begin
    am4core.useTheme(am4themes_kelly);
    am4core.useTheme(am4themes_animated);
    // Themes end

    // create chart
    var chart = am4core.create(name_div, am4charts.GaugeChart);
    chart.hiddenState.properties.opacity = 0; // this makes initial fade in effect


    chart.innerRadius = -50;

    var axis = chart.xAxes.push(new am4charts.ValueAxis());
    axis.min = -1;
    axis.max = 1;
    axis.strictMinMax = true;
    axis.renderer.grid.template.stroke = new am4core.InterfaceColorSet().getFor("background");
    axis.renderer.grid.template.strokeOpacity = 0.3;
    axis.renderer.labels.template.disabled = true;

    var colorSet = new am4core.ColorSet();

    var range0 = axis.axisRanges.create();
    range0.value = -1;
    range0.endValue = -0.25;
    range0.axisFill.fillOpacity = 1;
    range0.axisFill.fill = colorSet.getIndex(4);
    range0.axisFill.zIndex = - 1;

    var range1 = axis.axisRanges.create();
    range1.value = -0.25;
    range1.endValue = 0.25;
    range1.axisFill.fillOpacity = 1;
    range1.axisFill.fill = colorSet.getIndex(0);
    range1.axisFill.zIndex = -1;

    var range2 = axis.axisRanges.create();
    range2.value = 0.25;
    range2.endValue = 1;
    range2.axisFill.fillOpacity = 1;
    range2.axisFill.fill = am4core.color("green");
    range2.axisFill.zIndex = -1;

    var legend = new am4charts.Legend();
    legend.isMeasured = false;
    legend.y = am4core.percent(100);
    legend.verticalCenter = "bottom";
    legend.parent = chart.chartContainer;
    legend.data = [{
        "name": "Negative",
        "fill": chart.colors.getIndex(4)
    }, {
        "name": "Positive",
        "fill": am4core.color("green")
    }];

    var title = chart.titles.create();
    title.text = "Sentiment";
    title.fontSize = 25;
    title.marginBottom = 0;

    hand = chart.hands.push(new am4charts.ClockHand());
    hand.showValue(0, 0, am4core.ease.cubicOut);

    return ({ "chart": chart, "hand": hand });
};

// Filter records by type. Return list of text of the type specified
function get_text(records, type) {

    texts = [];

    for (var i in records) {

        var item = records[i];
        if (item.type == type) {
            texts.push(item.text);
        };

    };
    return (texts);
};

// Change value of the hand given an specific text object in teh page. Receives a gauge object (from createGauge) and the html text object id
function updateGauge(gauge_, textObj) {

    $.ajax({
        url: '/Sentiment/SentimentScore',
        type: 'POST',
        data: {
            input: $(textObj).val()
        },
        success: function (response) {

            gauge_.chart.setTimeout(() => { gauge_.hand.showValue(response, 1000, am4core.ease.cubicOut) }, 500);
        }
    });
}

// Hightight negative and positive sentences on an html text object given a boolean 'doit'
function highlightText(textObj, doit) {

    if (doit) {
        $.ajax({
            url: '/Sentiment/SentimentMinMax',
            type: 'POST',
            data: {
                input: $(textObj).val()
            },
            success: function (response) {

                sent = JSON.parse(response);

                $(textObj).highlightWithinTextarea({
                    highlight: [
                        {
                            highlight: get_text(sent, "min"),
                            className: 'red'
                        },
                        {
                            highlight: get_text(sent, "max"),
                            className: 'green'
                        }
                    ]
                });

            }
        });
    }
    else {
        $(textObj).HighlightWithinTextarea('destroy');
    };

}

// Change value of the gauge given a fixed value
function updateGaugeValue(gauge_, val) {
    gauge_.chart.setTimeout(() => { gauge_.hand.showValue(val, 1000, am4core.ease.cubicOut) }, 500);
}