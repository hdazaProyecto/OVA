///<summary>
///Función de seguridad para que en los inputs se pueda escribir letras o números segun 
/// la cadena que se coloque cuando se utiliza la función. 
///</summary> 
$.fn.onlyNumbers = function (cadena) {
    //    $(this).off('keypress'); se suprime para que funcione el porcentaje de seguro y no tome mas de 100
    $(this).on({
        keypress: function (e) {
            var key = e.which,
                keye = e.keyCode,
                tecla = String.fromCharCode(key).toLowerCase(),
                letras = cadena;
            if (letras.indexOf(tecla) == -1 && keye != 9 && (key == 37 || keye != 37) && (keye != 39 || key == 39) && keye != 8 && (keye != 46 || key == 46) || key == 161) {
                e.preventDefault();
            }
        }
    });
};

function aditionalSecurity() {
    $('.decimal').focusout(function () {
        if ($(this).val().split(' ').join('').length == 0 || isNaN($(this).val())) {
            $(this).val(parseFloat(0).toFixed(4));
        }
        else {
            $(this).val(parseFloat($(this).val()).toFixed(4));
        }
    });

    $('.int').focusout(function () {
        if ($(this).val().split(' ').join('').length == 0 || isNaN($(this).val())) {
            $(this).val(0);
        }
    });

    $('.money, .moneyStatic').focusout(function () {

        //if ($(this).val().split(' ').join('').length == 0 || isNaN($(this).val())) {
        //    $(this).val(parseFloat(0).toFixed(2).replace(".",","));
        //}
        //else {
        //    $(this).val(numberWithseparator($(this).val(), "."));
        //}

        if ($(this).val().split(' ').join('').length == 0 || isNaN($(this).val())) {
            $(this).val(parseFloat(0).toFixed(2));
        }
        else
            $(this).val(parseFloat($(this).val()).toFixed(2));
    });

    $('.hourTransComp, .minutesTransComp').focusout(function () {
        if ($(this).val().split(' ').join('').length == 0 || isNaN($(this).val())) {
            if ($(this).attr("id") == "txtAddHourTrans")
                $(this).val(HoraActual("hh"));
            else if ($(this).attr("id") == "txtAddMinutesTrans")
                $(this).val(HoraActual("mm"));
        }
    });


}
$.fn.porcentaje = function () {
    $(this).keypress(function (e) {
        if ($(this).val() + String.fromCharCode(e.keyCode) > 100) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
    });
}

function ConfigHourDocTrans(horas, minutos) {
    horas.keypress(function (e) {
        if ($(this).val() + String.fromCharCode(e.keyCode) > 23) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
    });

    minutos.keypress(function (e) {
        if ($(this).val() + String.fromCharCode(e.keyCode) > 59) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
    });

    $(".hour").focusout(function () {
        if ($(this).val().split(' ').join('').length == 0 || isNaN($(this).val())) {
            $(this).val("00");
        }
        else if ($(this).val() <= 9 && $(this).val().length == 1) {
            var numero = $(this).val();
            $(this).val("0".concat(numero));
        }
        else {
            $(this).val($(this).val());
        }
    });
}

function securityMouse() {

    document.oncontextmenu = function () {
        $('#section-content').ModalDialog('Mensaje de seguridad', 'Clic derecho no está permitido por seguridad, en la aplicación', 'warning');
        return false;
    }
    document.onkeydown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123) {
            return false;
        }
    }
}