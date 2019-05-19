$(document).ready(function () {
    
    $.ajax({
        url: '/Options/DashboardData',
        type: 'POST',
        data: {
            id: menteeId
        },
        success: function (response) {

            var data = JSON.parse(response);

            // Feelings rating
            var emotionsArray = ['crying', 'disappointed', 'meh', 'happy', 'smile'];
            $("#rating").emotionsRating({
                bgEmotion: 'happy',
                emotions: emotionsArray,
                emotionSize: 40,
                count:1,
                disabled: true,
                initialRating: Math.round(data.diaries.AvgFeeling)
            });

            // Create gauge plot in div 'gaugediv'
            //window.gauge = createGauge(0, "gaugediv","",true);
            // Set value of the plot to the sentiment score value
            //updateGaugeValue(window.gauge, data.diaries.AvgSentimentScore);

            if (data.diaries.NumDiaries > 0) {
                // Timeseries for diary entry
                timeseriesplot("timeplotdiv", data.diaries.Entries, Title = "");
            };

            if (data.goals.OpenGoals + data.goals.CloseGoals > 0) {
                // Stacked bar plot of goals
                stackedbarplot("bardiv", data.goals.Goals, "");
            };

            //Information overview divs
            document.getElementById("openGoals").appendChild(document.createTextNode(data.goals.OpenGoals));
            document.getElementById("closeGoals").appendChild(document.createTextNode(data.goals.CloseGoals));
            document.getElementById("nDiaries").appendChild(document.createTextNode(data.diaries.NumDiaries));
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

    createAxisAndSeries("feeling", "Mood scale", false, "rectangle", 0, 5);
    //createAxisAndSeries("score", "Sentiment Score", true, "circle",-1,1);
    //createAxisAndSeries("views", "Views", true, "triangle");

    // Add legend
    chart.legend = new am4charts.Legend();

    // Add cursor
    chart.cursor = new am4charts.XYCursor();

    //Add Title
    if (Title != "") {
        var title = chart.titles.create();
        title.text = Title;
        title.fontSize = 25;
        title.marginBottom = 0;
    };
    
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

function stackedbarplot(obj, data, Title){
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create(obj, am4charts.XYChart);

    // Add data
    chart.data = data;
    /*
    chart.data = [{
        "year": "2003",
        "europe": 2.5,
        "namerica": 2.5,
        "asia": 2.1,
        "lamerica": 1.2,
        "meast": 0.2,
        "africa": 0.1
    }, {
        "year": "2004",
        "europe": 2.6,
        "namerica": 2.7,
        "asia": 2.2,
        "lamerica": 1.3,
        "meast": 0.3,
        "africa": 0.1
    }, {
        "year": "2005",
        "europe": 2.8,
        "namerica": 2.9,
        "asia": 2.4,
        "lamerica": 1.4,
        "meast": 0.3,
        "africa": 0.1
    }];
    */

    // Create axes
    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "GoalName";
    categoryAxis.title.text = "Goals";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.cellStartLocation = 0.1;
    categoryAxis.renderer.cellEndLocation = 0.9;

    var label = categoryAxis.renderer.labels.template;
    label.wrap = true;
    label.maxWidth = 170;

    var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.title.text = "Tasks";

    // Create series
    function createSeries(field, name, stacked,color) {
        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.valueX = field;
        series.dataFields.categoryY = "GoalName";
        series.name = name;
        series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
        series.stacked = stacked;
        series.columns.template.width = am4core.percent(95);

        series.fill = am4core.color(color);
        series.stroke = am4core.color("white");
    }

    createSeries("completedTasks", "Completed (on time)", false,"#74db7e");
    createSeries("delayedCompletedTasks", "Completed (late)", true,"orange");
    createSeries("delayedTasks", "Overdue", true, "#ed5c4b");
    createSeries("pendingTasks", "Ongoing", true, "#4ba4ed");

    // Add legend
    chart.legend = new am4charts.Legend();

    //Add Title
    if (Title != "") {
        var title = chart.titles.create();
        title.text = Title;
        title.fontSize = 25;
        title.marginBottom = 0;
    };
};