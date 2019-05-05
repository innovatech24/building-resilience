$(document).ready(function () {
    
    $(".rating").rateYo({
        precision: 0,
        onSet: function (rating, rateYoInstance) {
            $.ajax({
                url: '/DiaryEntries/ViewFeedback',
                type: 'POST',
                data: {
                    Id: parseInt(this.getAttribute("iid")),
                    rate: rating
                }
            });
        }
    });

});

