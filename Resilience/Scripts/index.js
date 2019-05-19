
$(document).ready(function () {
    // Get open datasets
    $.ajax({
        url: '/Home/GetDatasets',
        type: 'GET',
        success: function (response) {
            datasets = JSON.parse(response);
            console.log(datasets);

            // Employment in victoria 2018 by region
            barplot("region_plot", datasets.Employment_region, "Employment by Region in Victoria - 2018");
            // Employment by industry 2018 vs projection 2023
            barplotcompound("industry_plot", datasets.Industry_empl_2018, "Employment projection by 2023");
            //Unemployment time series
            linetrendplot("history_plot", datasets.Hist_Vic_empl, "Trend of unemployment rate in Victoria");
        },
        errlr: function (response) { console.log(response);}
    });
    // REGION PLOT
    // Get the modal
    var modal = document.getElementById('myModal');

    // Get the button that opens the modal
    var btn = document.getElementById("regionPlot");

    // When the user clicks on the button, open the modal 
    btn.onclick = function () {
        modal.style.display = "block";
    }

    // INDUSTRY EMPLOYMENT
    // Get the modal
    var modal2 = document.getElementById('myModal2');

    // Get the button that opens the modal
    var btn2 = document.getElementById("industryPlot");

    // When the user clicks on the button, open the modal 
    btn2.onclick = function () {
        modal2.style.display = "block";
    }

    // HISTORY EMPLOYMENT VIC
    // Get the modal
    var modal3 = document.getElementById('myModal3');

    // Get the button that opens the modal
    var btn3 = document.getElementById("histEmpl");

    // When the user clicks on the button, open the modal 
    btn3.onclick = function () {
        modal3.style.display = "block";
    }

    // WHY RESILIENCE SECTION
    // Get the modal
    var modal4 = document.getElementById('whyresilience');

    // Get the button that opens the modal
    var btn4 = document.getElementById("whyRes");

    // When the user clicks on the button, open the modal 
    btn4.onclick = function () {
        modal4.style.display = "block";
    }

    // HOW CAN WE HELP SECTION
    // Get the modal
    var modal5 = document.getElementById('howcanwehelp');

    // Get the button that opens the modal
    var btn5 = document.getElementById("howCanWeHelp");

    // When the user clicks on the button, open the modal 
    btn5.onclick = function () {
        modal5.style.display = "block";
    }

    // CHALLENGES SECTION
    // Get the modal
    var modal6 = document.getElementById('challengesPopup');

    // Get the button that opens the modal
    var btn6 = document.getElementById("challenges");

    // When the user clicks on the button, open the modal 
    btn6.onclick = function () {
        modal6.style.display = "block";
    }

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
        modal2.style.display = "none";
        modal3.style.display = "none";
        modal4.style.display = "none";
        modal5.style.display = "none";
        modal6.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
        if (event.target == modal2) {
            modal2.style.display = "none";
        }
        if (event.target == modal3) {
            modal3.style.display = "none";
        }
        if (event.target == modal4) {
            modal4.style.display = "none";
        }
        if (event.target == modal5) {
            modal5.style.display = "none";
        }
        if (event.target == modal6) {
            modal6.style.display = "none";
        }
    }

});

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

function barplot(obj,data,Title) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create(obj, am4charts.XYChart);

    // Add data
    chart.data = data;

    // Create axes

    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Vic_region";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;
    /*
    categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
        if (target.dataItem && target.dataItem.index & 2 == 2) {
            return dy + 25;
        }
        return dy;
    });*/

    var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueX = "Employment";
    series.dataFields.categoryY = "Vic_region";
    series.name = "Employment";
    series.columns.template.tooltipText = "{categoryY}: [bold]{valueX}[/]";
    series.columns.template.fillOpacity = .8;

    var columnTemplate = series.columns.template;
    columnTemplate.strokeWidth = 2;
    columnTemplate.strokeOpacity = 1;

    if (Title != "") {
        var title = chart.titles.create();
        title.text = Title;
        title.fontSize = 25;
        title.marginBottom = 0;
    };
};

function barplotcompound(obj, data, Title) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create(obj, am4charts.XYChart);

    // Add data
    chart.data = data;

    // Create axes
    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Industry";
    categoryAxis.title.text = "Industry";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.cellStartLocation = 0.1;
    categoryAxis.renderer.cellEndLocation = 0.9;

    var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;
    valueAxis.title.text = "Employment ('000)";

    // Create series
    function createSeries(field, name, stacked) {
        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.valueX = field;
        series.dataFields.categoryY = "Industry";
        series.name = name;
        series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
        series.stacked = stacked;
        series.columns.template.width = am4core.percent(95);
    }

    

    createSeries("Employment_2023_000", "2023", false);
    createSeries("Employment_2018_000", "2018", false);

    // Add legend
    chart.legend = new am4charts.Legend();
    if (Title != "") {
        var title = chart.titles.create();
        title.text = Title;
        title.fontSize = 25;
        title.marginBottom = 0;
    };

};

function linetrendplot(obj, data, Title) {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create(obj, am4charts.XYChart);

    // Enable chart cursor
    chart.cursor = new am4charts.XYCursor();
    chart.cursor.lineX.disabled = true;
    chart.cursor.lineY.disabled = true;

    // Enable scrollbar
    chart.scrollbarX = new am4core.Scrollbar();

    // Add data
    chart.data = data;

    // Create axes
    
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.ticks.template.disabled = true;
    categoryAxis.renderer.line.opacity = 0;
    categoryAxis.renderer.grid.template.disabled = true;
    categoryAxis.renderer.minGridDistance = 40;
    categoryAxis.dataFields.category = "year";
    categoryAxis.startLocation = 0.4;
    categoryAxis.endLocation = 0.6;
    categoryAxis.title.text = "Year";
    //var valueAxis2 = chart.xAxes.push(new am4charts.ValueAxis());
    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

    // Create series
    var series = chart.series.push(new am4charts.LineSeries());
    series.tooltipText = "{year}\n[bold font-size: 17px]Rate: {valueY}[/]";
    series.dataFields.valueY = "unemployment_rate";
    series.dataFields.categoryX = "year";
    series.strokeDasharray = 3;
    series.strokeWidth = 2
    series.strokeOpacity = 0.3;
    series.strokeDasharray = "3,3"

    var bullet = series.bullets.push(new am4charts.CircleBullet());
    bullet.strokeWidth = 2;
    bullet.stroke = am4core.color("#fff");
    bullet.setStateOnChildren = true;
    bullet.propertyFields.fillOpacity = "opacity";
    bullet.propertyFields.strokeOpacity = "opacity";

    var hoverState = bullet.states.create("hover");
    hoverState.properties.scale = 1.7;

    function createTrendLine(data) {
        var trend = chart.series.push(new am4charts.LineSeries());
        trend.dataFields.valueY = "unemployment_rate";
        trend.dataFields.categoryX = "year";
        trend.strokeWidth = 2
        trend.stroke = trend.fill = am4core.color("#c00");
        trend.data = data;

        var bullet = trend.bullets.push(new am4charts.CircleBullet());
        bullet.tooltipText = "{year}\n[bold font-size: 17px]Trend: {valueY}[/]";
        bullet.strokeWidth = 2;
        bullet.stroke = am4core.color("#fff")
        bullet.circle.fill = trend.stroke;

        var hoverState = bullet.states.create("hover");
        hoverState.properties.scale = 1.7;

        return trend;
    };

    createTrendLine([
        { "year": "1990", "unemployment_rate": 8.4 },
        { "year": "2015", "unemployment_rate": 6.1 }
    ]);

    if (Title != "") {
        var title = chart.titles.create();
        title.text = Title;
        title.fontSize = 25;
        title.marginBottom = 0;
    };

}