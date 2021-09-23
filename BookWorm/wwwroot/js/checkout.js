$(document).ready(function () {

    /* Lorsque la page est charger, on va chercher les informations
     * de la première addresse du dropdown pour le shipping */
    $('#dropDownShipping').ready(function () {

        if ($('#startId').val() != -1) {
            $.ajax({
                method: "Get",
                url: "/Shop/GetAddress",
                data: { addressId: $('#startId').val() },
                success: function (data) {
                    var address = JSON.parse(data);
                    
                    // On change les valeurs du html pour l'adresse selectionner
                    $('#shippingLocalAddress').text(address.LocalAddress);
                    $('#shippingApartment').text(address.Apartment);
                    $('#shippingPostalCode').text(address.PostalCode);
                    $('#shippingCity').text(address.City);
                    $('#shippingRegion').text(address.RegionName);
                    $('#shippingCountry').text(address.CountryName);
                    $('#taxPercentage').val(address.Region.Tax);

                    // On met a jour le prix total
                    updateTotal();
                }
            });
        }
    });

    /* Lorsque la page est charger, on va chercher les informations
     * de la première addresse du dropdown pour le billing */
    $('#dropDownBilling').ready(function () {

        if ($('#startId').val() != -1) {
            $.ajax({
                method: "Get",
                url: "/Shop/GetAddress",
                data: { addressId: $('#startId').val() },
                success: function (data) {
                    var address = JSON.parse(data);

                    // On change les valeurs du html pour l'adresse selectionner
                    $('#billingLocalAddress').text(address.LocalAddress);
                    $('#billingApartment').text(address.Apartment);
                    $('#billingPostalCode').text(address.PostalCode);
                    $('#billingCity').text(address.City);
                    $('#billingRegion').text(address.RegionName);
                    $('#billingCountry').text(address.CountryName);
                }
            });
        }
    });

    /* Lorsque le la valeur du dropdown est changer pour le shipping*/
    $('#dropDownShipping').change(function () {
        $.ajax({
            method: "Get",
            url: "/Shop/GetAddress",
            data: { addressId: $(this).val() },
            success: function (data) {
                var address = JSON.parse(data);

                // On change les valeurs du html pour l'adresse selectionner
                $('#shippingLocalAddress').text(address.LocalAddress);
                $('#shippingApartment').text(address.Apartment);
                $('#shippingPostalCode').text(address.PostalCode);
                $('#shippingCity').text(address.City);
                $('#shippingRegion').text(address.RegionName);
                $('#shippingCountry').text(address.CountryName); 
                $('#taxPercentage').val(address.Region.Tax);

                // On met a jour le prix total
                updateTotal();
            }
        });
    });

    /* Lorsque le la valeur du dropdown est changer pour le billing */
    $('#dropDownBilling').change(function () {
        $.ajax({
            method: "Get",
            url: "/Shop/GetAddress",
            data: { addressId: $(this).val() },
            success: function (data) {
                var address = JSON.parse(data);

                // On change les valeurs du html pour l'adresse selectionner
                $('#billingLocalAddress').text(address.LocalAddress);
                $('#billingApartment').text(address.Apartment);
                $('#billingPostalCode').text(address.PostalCode);
                $('#billingCity').text(address.City);
                $('#billingRegion').text(address.RegionName);
                $('#billingCountry').text(address.CountryName);
            }
        });
    });

    // Met à jour le grand total dépendant de la taxes de la région (Province/État)
    function updateTotal() {
        var subTotal = $('#subtotal').val();
        var shipping = $('#shipping').val();
        var handling = $('#handling').val();
        var taxVal = $('#taxPercentage').val();

        // Total avant-taxes
        var total = (+subTotal + +shipping + +handling);

        // Montant de taxes en $
        var taxesTotal = (+total * +taxVal);
        taxesTotal = +taxesTotal.toFixed(2);

        // Total apres-taxes
        var grandTotal = (+total + +taxesTotal);
        grandTotal = +grandTotal.toFixed(2);

        $('#taxAmount').text("$" + taxesTotal);
        $('#grandTotal').text("$" + grandTotal);
        $('#paypalTax').val(taxesTotal); 
    }
});

// Avant de proceder au paiement, on verifie que l'adresse est valide.
function payementSubmit() {

    if ($('#shippingLocalAddress').text() == "Please add an address." ||
        $('#billingLocalAddress').text() == "Please add an address." ) {
        return false;
    }

    return true;
}
