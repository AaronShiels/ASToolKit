app.controller('budgetTrackerController', function ($scope, Income, Bill, Frequencies) {
    $scope.today = new Date();
    $scope.incomes = [
        new Income('Collection House Ltd', 800, Frequencies.fortnightly, new Date(2014,09,17))
    ];
    $scope.bills = [
        new Bill('Phone',70, Frequencies.monthly, new Date(2014,9,20)),
        new Bill('Rent',150, Frequencies.weekly, new Date(2014,9,16))
    ];
});

