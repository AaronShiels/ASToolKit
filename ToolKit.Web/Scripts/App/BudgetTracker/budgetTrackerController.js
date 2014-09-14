app.controller('budgetTrackerController', function ($scope, Income, Bill, Cost, Frequencies) {
    $scope.today = new Date();
    $scope.balance = 900;
    $scope.cycle = Frequencies.weekly;
    $scope.cycleShort = function () {
        switch ($scope.cycle) {
            case Frequencies.weekly:
                return 'wk';
            case Frequencies.fortnightly:
                return 'fn';
            case Frequencies.monthly:
                return 'mth';
            case Frequencies.halfYearly:
                return '6m';
            case Frequencies.yearly:
                return 'yr';
            default:
                return '???'
        }
    };


    $scope.incomes = [
        new Income('Collection House Ltd', 800, Frequencies.fortnightly, new Date(2014,09,17))
    ];
    $scope.bills = [
        new Bill('Phone',70, Frequencies.monthly, new Date(2014,9,20)),
        new Bill('Rent',150, Frequencies.weekly, new Date(2014,9,16))
    ];
    $scope.costs = [
        new Cost('Food', 80, Frequencies.weekly, new Date(2014, 9, 20)),
        new Cost('Fuel', 60, Frequencies.monthly, new Date(2014, 9, 16))
    ];
});

