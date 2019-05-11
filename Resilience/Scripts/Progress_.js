$(document).ready(function () {
    
    $.ajax({
        url: '/Options/DashboardData',
        type: 'POST',
        data: {
            id: menteeId
        },
        success: function (response) {

            var data = JSON.parse(response);
            console.log(data);

            // Feelings rating
            var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];
            $("#rating").emotionsRating({
                bgEmotion: 'happy',
                emotions: emotionsArray,
                disabled: true,
                initialRating: Math.round(data.diaries.AvgFeeling)
            });

            // Create gauge plot in div 'gaugediv'
            window.gauge = createGauge(0, "gaugediv","Average sentiment in reflective diaries",false);
            // Set value of the plot to the sentiment score value
            updateGaugeValue(window.gauge, data.diaries.AvgSentimentScore);


            // Timeseries for diary entry
            timeseriesplot("timeplotdiv",data.diaries.Entries,Title = "Feeling rating VS Sentiment score");

        }
    });
});

function timeseriesplot(obj,data,Title) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create(obj, am4charts.XYChart);

    // Increase contrast by taking evey second color
    chart.colors.step = 2;

    // Add data
    chart.data = data;
    console.log(generateChartData());

    // Create axes
    var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.renderer.minGridDistance = 50;

    // Create series
    function createAxisAndSeries(field, name, opposite, bullet, min,max) {
        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.min = min;
        valueAxis.max = max;
        valueAxis.strictMinMax = false;

        var series = chart.series.push(new am4charts.LineSeries());
        series.dataFields.valueY = field;
        series.dataFields.dateX = "date";
        series.strokeWidth = 2;
        series.yAxis = valueAxis;
        series.name = name;
        series.tooltipText = "{name}: [bold]{valueY}[/]";
        series.tensionX = 0.8;

        var interfaceColors = new am4core.InterfaceColorSet();

        switch (bullet) {
            case "triangle":
                var bullet = series.bullets.push(new am4charts.Bullet());
                bullet.width = 12;
                bullet.height = 12;
                bullet.horizontalCenter = "middle";
                bullet.verticalCenter = "middle";

                var triangle = bullet.createChild(am4core.Triangle);
                triangle.stroke = interfaceColors.getFor("background");
                triangle.strokeWidth = 2;
                triangle.direction = "top";
                triangle.width = 12;
                triangle.height = 12;
                break;
            case "rectangle":
                var bullet = series.bullets.push(new am4charts.Bullet());
                bullet.width = 10;
                bullet.height = 10;
                bullet.horizontalCenter = "middle";
                bullet.verticalCenter = "middle";

                var rectangle = bullet.createChild(am4core.Rectangle);
                rectangle.stroke = interfaceColors.getFor("background");
                rectangle.strokeWidth = 2;
                rectangle.width = 10;
                rectangle.height = 10;
                break;
            default:
                var bullet = series.bullets.push(new am4charts.CircleBullet());
                bullet.circle.stroke = interfaceColors.getFor("background");
                bullet.circle.strokeWidth = 2;
                break;
        }

        valueAxis.renderer.line.strokeOpacity = 1;
        valueAxis.renderer.line.strokeWidth = 2;
        valueAxis.renderer.line.stroke = series.stroke;
        valueAxis.renderer.labels.template.fill = series.stroke;
        valueAxis.renderer.opposite = opposite;
        valueAxis.renderer.grid.template.disabled = true;
    }

    createAxisAndSeries("feeling", "Feeling rate", false, "rectangle", 1, 5);
    createAxisAndSeries("score", "Sentiment Score", true, "circle",-1,1);
    //createAxisAndSeries("views", "Views", true, "triangle");

    // Add legend
    chart.legend = new am4charts.Legend();

    // Add cursor
    chart.cursor = new am4charts.XYCursor();

    //Add Title
    var title = chart.titles.create();
    title.text = Title;
    title.fontSize = 25;
    title.marginBottom = 0;

    // generate some random data, quite different range
    function generateChartData() {
        var chartData = [];
        var firstDate = new Date();
        firstDate.setDate(firstDate.getDate() - 100);
        firstDate.setHours(0, 0, 0, 0);

        var visits = 1600;
        var hits = 2900;
        var views = 8700;

        for (var i = 0; i < 15; i++) {
            // we create date objects here. In your data, you can have date strings
            // and then set format of your dates using chart.dataDateFormat property,
            // however when possible, use date objects, as this will speed up chart rendering.
            var newDate = new Date(firstDate);
            newDate.setDate(newDate.getDate() + i);

            visits += Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 10);
            hits += Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 10);
            views += Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 10);

            chartData.push({
                date: newDate,
                visits: visits,
                hits: hits,
                views: views
            });
        }
        return chartData;
    }
};