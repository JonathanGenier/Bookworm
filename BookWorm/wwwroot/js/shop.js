// Ajoute le produit cliquer dans le chariot d'achat.
// Les produits afficher dans la page index et shop.
$(document).ready(function () {
    $('.addToCartBtn').click(function (e) {
        $.ajax({
            method: "Get",
            url: "/Shop/AddToCart",
            data: { bookId: e.target.id },
            success: function (data) {

                // À implementer
                // Devra donner un signe à l'utilisateur que 
                // son livre à été ajouté à son chariot d'achat.
            }
        });
    });
});