// Verifie si le mot de passe entré est valide.
function validatePassword() {
    let password = $("#password").val();
    var testPassed = true;

    // Verifie la longueur du mot de passe
    if (password.length >= 8) {
        $("#phCharacters").css("color", "green");
    } else {
        $("#phCharacters").css("color", "red");
        testPassed = false;
    }

    // Verifie si le mot de passe contient un chiffre
    if (/\d/.test(password)) {
        $("#phNumber").css("color", "green");
    } else {
        $("#phNumber").css("color", "red");
        testPassed = false;
    }

    // Verifie si le mot de passe contient des symboles. 
    var format = /[ `!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?~]/;
    if (format.test(password)) {
        $("#phSymbol").css("color", "green");
    } else {
        $("#phSymbol").css("color", "red");
        testPassed = false;
    }

    // Verifie si le mot de passe contient une lettre majuscule
    var format = /[A-Z]/;
    if (format.test(password)) {
        $("#phUppercase").css("color", "green");
    } else {
        $("#phUppercase").css("color", "red");
        testPassed = false;
    }

    if (testPassed) {
        $("#cfrmPassword").prop("disabled", false);
    } else {
        $("#cfrmPassword").prop("disabled", true);
    }

    confirmPassword();
    return testPassed;
}

// Cette fonction verifie si les deux mot de passe entrees sont identiques.
function confirmPassword() {

    if ($("#password").val() == $("#cfrmPassword").val()) {
        hideAlert("#cfrmPasswordAlert");
        return true;
    } else {
        showAlert("#cfrmPasswordAlert");
        return false;
    } 
}

// Cette fonction valide tous les attributs lorsqu'on créer un nouveau compte utilisateur.
function validateNewAccount() {
    var valid = true;

    // Verifie si le nom de l'utilisateur n'est pas vide.
    if ($("#username").val().trim().length > 0) {
        hideAlert("#usernameAlert");
    } else {
        showAlert("#usernameAlert");
        valid = false;
    } 

    // Verifie le mot de passe entree.
    if (validatePassword()) {
        hideAlert("#passwordAlert");
    } else {
        showAlert("#passwordAlert");
        valid = false;
    }

    // Confirme si les mots de passe sont identiques.
    if (confirmPassword()) {
        hideAlert("#cfrmPasswordAlert");
    } else {
        showAlert("#cfrmPasswordAlert");
        valid = false;
    }
    
    // Verifie si le prenom n'est pas vide.
    if ($("#firstName").val().trim().length > 0) {
        hideAlert("#firstNameAlert");
    } else {
        showAlert("#firstNameAlert");
        valid = false;
    } 

    // Verifie si le nom n'est pas vide.
    if ($("#lastName").val().trim().length > 0) {
        hideAlert("#lastNameAlert");
    } else {
        showAlert("#lastNameAlert");
        valid = false;
    } 

    // Verifie si le nom de le courriel est d'un bon format.
    let format = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (format.test($("#email").val())) {
        hideAlert("#emailAlert");
    } else {
        showAlert("#emailAlert");
        valid = false;
    }

    // Verifie si les polices sont cochés.
    if ($("#termsOfService").prop("checked") == true) {
        hideAlert("#checkboxAlert");
    } else {
        showAlert("#checkboxAlert");
        valid = false;
    }

    if ($("#privacyPolicy").prop("checked") == true) {
        hideAlert("#checkboxAlert");
    } else {
        showAlert("#checkboxAlert");
        valid = false;
    }

    // Si tous est vérifie, on encrypte le mot de passe.
    if (valid == true) {
        encrypt("#password");
    }

    return valid;
}

// Affiche une alerte lorsqu'un attribut n'est pas valide.
function showAlert(id) {
    $(id).css("display", "flex");
    $(id).css("visibility", "visible");
}

// Cache une alerte affiché lorsqu'un attribut devient valide.
function hideAlert(id) {    
    $(id).css("display", "none");
    $(id).css("visibility", "hidden");
}

// Cette fonction valide le login.
function validateLogin() {
    var valid = true;

    // Verifie si le nom d'utilisateur n'est pas vide.
    if ($("#username").val().trim().length > 0) {
        hideAlert("#usernameAlert");
    } else {
        showAlert("#usernameAlert");
        valid = false;
    } 

    // Verifie si le mot de passe n'est pas vide.
    if ($("#password").val().trim().length > 0) {
        hideAlert("#passwordAlert");
    } else {
        showAlert("#passwordAlert");
        valid = false;
    } 

    // Si tous les attributs sont valide, on encrypte le mot de passe.
    if (valid == true) {
        encrypt("#password");
    }

    return valid;
}

// Encrypt un attribut (principalement un mot de passe).
function encrypt(id) {
    let plainTextPassword = CryptoJS.enc.Utf8.parse($(id).val());
    var key = CryptoJS.enc.Utf8.parse('5v8y/B?E(H+MbQeT');
    var iv = CryptoJS.enc.Utf8.parse('bQeThWmZq4t7w9z$');

    var encryptedPassword = CryptoJS.AES.encrypt(plainTextPassword, key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });   

    $("#encrypted").val(encryptedPassword);
}