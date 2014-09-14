var app = angular.module('budgetTrackerApp', []);

app.value('Frequencies', {
    weekly: 'Weekly',
    fortnightly: 'Fortnightly',
    monthly: 'Monthly',
    halfYearly: 'Half yearly',
    yearly: 'Yearly'
});

app.factory('Bill', function (Frequencies) {
    function Bill(name, amount, frequency, approxNextDueDate) {
        this.name = name;
        this.amount = amount;
        this.frequency = frequency;
        this.nextDue = approxNextDueDate;
    }

    return (Bill);
});

app.factory('Cost', function (Frequencies) {
    function Cost(name, amount, frequency, resets) {
        this.name = name;
        this.amount = amount;
        this.frequency = frequency;
        this.resets = resets;
    }

    return (Cost);
});

app.factory('Income', function (Frequencies) {
    function Income(name, amount, frequency, approxNextDueDate) {
        this.name = name;
        this.amount = amount;
        this.frequency = frequency;
        this.nextDue = approxNextDueDate;
    }

    return (Income);
});