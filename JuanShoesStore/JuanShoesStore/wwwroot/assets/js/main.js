$(function () {
    $(document).on('click', '#shoe-quick-view', function (e) {
        e.preventDefault();

        fetch($(this).attr("href"))
            .then(response => response.text())
            .then(data => {
                $("#quick_view .modal-dialog-centered").html(data)
                $("#quick_view").modal(true);
            })
    })

    $(document).on('click', "#shoe-addTo-basket", function (e) {
        e.preventDefault();

        let Url = $(this).attr("href");

        fetch(Url)
            .then(response => response.text())
            .then(data => {
                    $('.minicart-content-box').html(data)
            })
            .then(function () {
                $('#basket-notification').text($('#basket-items-count').val());
            })

    }); 


    $(document).on('click', "#remove-from-basket", function (e) {
        e.preventDefault();

        let Url = $(this).attr("href");

        fetch(Url)
            .then(response => response.text())
            .then(data => {
                $('.minicart-content-box').html(data)
            })
            .then(function () {
                $('#basket-notification').text($('#basket-items-count').val());
            })

    });


})