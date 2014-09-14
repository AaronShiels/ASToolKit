var app = angular.module('budgetTrackerApp', []);

app.value('Frequencies', {
    weekly: 'weekly',
    fortnightly: 'fortnightly',
    monthly: 'monthly',
    halfYearly: 'halfYearly',
    yearly: 'yearly'
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

app.factory('Income', function (Frequencies) {
    function Income(name, amount, frequency, approxNextDueDate) {
        this.name = name;
        this.amount = amount;
        this.frequency = frequency;
        this.nextDue = approxNextDueDate;
    }

    return (Income);
});