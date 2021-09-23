// Utiliser dans UserAccount.
// Change les valeurs du dropdown region dependant du pays choisis.
$(document).ready(function () {
    $('#dropDownCountry').change(function () {
        $.ajax({
            method: "Get",
            url: "/Account/GetRegions",
            data: { countryName: $(this).val() },
            success: function (data) {


                $('#dropDownRegion').empty();
                $('#dropDownRegion').append(data);
            }
        });
    });
});

