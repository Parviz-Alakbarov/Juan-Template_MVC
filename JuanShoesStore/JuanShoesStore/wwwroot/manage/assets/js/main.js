$(function () {

    $(".confirmDelete").on('click', function (e) {
        e.preventDefault();


        let Name = $(this).parent().siblings().eq(2).text();
        let Url = $(this).attr("href");

        Swal.fire({
            title: `Are you sure to delete ${Name}?`,
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                fetch(Url);

                Swal.fire(
                    'Deleted!',
                    `${Name} has been deleted.`,
                    'success'

                ).then(function () { window.location.replace(Url + "/../../"); })
            }

        })
    });

   


})