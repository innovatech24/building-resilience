$(document).ready(function () {
    
    $(".rating").rateYo({
        precision: 0,
        readOnly: true,
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

