// Permet de controller le _QuantityBox pour la page Book_Detail
// Incremente et decremente la valeur du QuantityBox.
// La valeur est entre 1 et 10.

var min = 1;
var max = 10;

function decrement() {
    var value = document.getElementById("quantity-box-text").value;

    if (value > min) {
        document.getElementById("quantity-box-text").value = --value;
    }
}

function increment() {
    var value = document.getElementById("quantity-box-text").value;
    
    if (value < max) {
        document.getElementById("quantity-box-text").value = ++value;
    }
}

function verifyValue() {
    var value = document.getElementById("quantity-box-text").value;

    if (isNaN(value)) {
        document.getElementById("quantity-box-text").value = 1;
    }
    else {
        if (value > 10) {
            document.getElementById("quantity-box-text").value = 10;
        }
        else if (value < 1) {
            document.getElementById("quantity-box-text").value = 1;
        }
    }
}