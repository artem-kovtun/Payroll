window.addEventListener("load", () => {
    $(".doc-service-list-item").click((e) => {
        selectService(e.currentTarget);
    });
});

var selectService = function (target) {
    if (!target.classList.contains("doc-service-selected")) {
        target.classList.add("doc-service-selected");
        target.children[2].value = true;
    }
    else {
        target.classList.remove("doc-service-selected");
        target.children[2].value = false;
    }
}