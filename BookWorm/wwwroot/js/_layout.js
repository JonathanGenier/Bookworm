
// Genere une date qui est afficher dans le footer de la page.
function getTodayDate() {
    var month = "";
    var date = new Date();

    switch (date.getMonth()) {
        case 0: month = "Janvier"; break;
        case 1: month = "Février"; break;
        case 2: month = "Mars"; break;
        case 3: month = "Avril"; break;
        case 4: month = "Mai"; break;
        case 5: month = "Juin"; break;
        case 6: month = "Juillet"; break;
        case 7: month = "Août"; break;
        case 8: month = "Septembre"; break;
        case 9: month = "Octobre"; break;
        case 10: month = "Novembre"; break;
        case 11: month = "Décembre"; break;
    }

    document.getElementById("date").innerHTML =
        date.getDate() + " " + month + " " + date.getFullYear();
}

