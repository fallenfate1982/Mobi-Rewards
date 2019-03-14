///<reference path="/scripts/knockout-2.3.0.js"/>

var promotion = function () {
    self = this;
    self.promotionData = ko.observableArray([]);
    self.filteredData = ko.observableArray([]);

    self.isApproved = ko.observable(false);
    self.isApproved.subscribe(function (newValue) {
        self.isApproved(newValue);
        self.doFilter();
    });

    self.isPaid = ko.observable(false);
    self.isPaid.subscribe(function (newValue) {
        self.isPaid(newValue);
        self.doFilter();
    });

    self.keyword = ko.observable("");
    self.keyword.subscribe(function (newValue) {
        self.keyword(newValue);
        self.doFilter();
    });

    self.showCoupons = ko.observable(false);
    self.showCouponsClick = function () {
        $("#" + this.PromotionId).toggle();
    };
    //Filter based on selection
    self.doFilter = function () {
        var ar = [];
        ko.utils.arrayForEach(self.promotionData(), function (item) {
            if ((
                self.keyword() == ""
                || item.PromotionTitle.toLowerCase().indexOf(self.keyword().toLowerCase()) != -1
                || item.PromotionName.toLowerCase().indexOf(self.keyword().toLowerCase()) != -1)
                && item.IsApproved == self.isApproved()
                && item.IsPaid == self.isPaid()
                ) {
                ar.push(item);
            }
        });

        self.filteredData([]);
        self.filteredData(ar);
    };

    //Load initial data
    self.loadData = function (vm) {
        $.ajax({
            type: "GET",
            dataType: "json",
            url: "/api/promotions",
            success: function (data) {
                vm.promotionData(data);
                vm.filteredData(data);
                ko.applyBindings(vm);
            },
            error: function (error) {
                var jsonValue = jQuery.parseJSON(error.responseText);
                //alert(jsonValue);
                //jError('An error has occurred while saving the new part source: ' + jsonValue, { TimeShown: 3000 });
            }
        });
    };
}

