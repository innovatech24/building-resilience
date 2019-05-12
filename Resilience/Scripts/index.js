
$(document).ready(function () {
    // Get open datasets
    $.ajax({
        url: '/Home/GetDatasets',
        type: 'GET',
        success: function (response) {
            datasets = JSON.parse(response);
            console.log(datasets);


            barplot("region_plot", datasets.Employment_region, "Employment by Region in Victoria - 2018");

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


    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
        modal2.style.display = "none";
        modal3.style.display = "none";
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
