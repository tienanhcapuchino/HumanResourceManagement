$(() => {
	getTotalEarnings();
	getVacationDays();
});

function getTotalEarnings() {
	$.ajax({
		url: "http://localhost:5247/api/Employees/get-total-earning",
		method: "GET",
		contentType: "application/json",
		success: function (response) {
			$("#earnings-value").empty();
			$("#earnings-value").append(`$ ${response}`);
		},
		error: function (xhr) {
			console.error(xhr.responseText);
		},
	});
}

function getVacationDays() {
	$.ajax({
		url: "http://localhost:5247/api/Employees/get-vacation-days",
		method: "GET",
		contentType: "application/json",
		success: function (response) {
			$("#vacation-value").empty();
			$("#excess-vacation-days-value").empty();

			$("#vacation-value").append(`${response.vacationDaysTaken} days`);
			$("#excess-vacation-days-value").append(
				`${response.excessVacationDays} days`
			);
		},
		error: function (xhr) {
			console.error(xhr.responseText);
		},
	});
}
