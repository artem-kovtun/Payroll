window.addEventListener('load', () => {
    $(".edit-service").click((e) => {
        var id = e.currentTarget.name;
        $.getJSON(`/services/${id}`, (data) => {
            $("#service-title").val(data.name);
            $("#service-description").val(data.description);
            $("#service-hours").val(data.hours);
            $("#service-id").val(data.serviceId);
        });
    });

    $(".copy-service").click((e) => {
        var id = e.currentTarget.name;
        $.getJSON(`/services/${id}`, (data) => {
            $("#service-title").val(data.name);
            $("#service-description").val(data.description);
            $("#service-hours").val(data.hours);
        });
    });
});