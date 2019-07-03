window.addEventListener("load", () => {
    $("#doc-date-pick-input").change((e) => {
        var date = e.currentTarget.value.replace(/-/g, '');
        $.getJSON(`https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode=USD&date=${date}`, (data) => {
            $("#rate-value")[0].innerHTML = data[0].rate.toFixed(2);
            updateTotalPay();
        })   
    })
    $(".doc-service-list-item").click((e) => {
        selectService(e.currentTarget);
        updateTotalPay();
    });

    $("#custom-usd-rate-input").change((e) => {
        updateTotalPay();
    });

    $("#highest-exchange-rate-button").click((e) => {
        var dateInput = $("#doc-date-pick-input");
        dateInput.prop("disabled", true);
        $.getJSON("/usdhighestexchangerate", (data) => {
            var date = data.exchangedate.substring(0,10).split(".").reverse().join("-");;
            dateInput.val(date);
            dateInput.trigger("change");
            dateInput.prop("disabled", false);
        });
    });


    $("#doc-date-pick-input").trigger("change");
});

var selectService = function (target) {
    if (!target.classList.contains("doc-service-selected")) {
        target.classList.add("doc-service-selected");
        target.children[3].value = true;
    }
    else {
        target.classList.remove("doc-service-selected");
        target.children[3].value = false;
    }
}


var updateTotalPay = function () {
    var OneUsd = $("#rate-value").html();
    var UsdRate = $("#custom-usd-rate-input").val();
    var checkedServices = $("input.check")

    var totalHours = 0
    for (var i = 0; i < checkedServices.length; i++) {
        if (checkedServices[i].value == "true") {
            totalHours += parseInt(checkedServices[i].parentElement.childNodes[11].childNodes[0].innerHTML);
        }
    }

    var pay = OneUsd * UsdRate * totalHours;
    debugger;
    $("#total-pay-value")[0].innerHTML = pay.toFixed(2);
}

function numberToDay(j) {
    return ('0' + j).slice(-2);
}