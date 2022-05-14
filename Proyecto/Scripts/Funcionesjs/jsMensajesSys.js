// #region Funciones de mensajes
/*Función que permite seleccionar un tipo mensajes*/
var TipeMsj = {
    WARNING: 1,
    WARNINGYORN: 2,
    INFO: 3,
    QUESTION: 4,
    QUESTIONYORN: 5
}
//

/*Función que permite mmostrar un tipo de mensajes*/
function MsjApp(agElement, agTypeMsj, agFormato, agElement2) {
    switch (agTypeMsj) {
        //case 1:
        //    myAlert("Campo Nulo", "El campo '" + agElement.attr("title").toUpperCase() + "', NO puede contener valores nulos!", true, false, TipeMsj.WARNING);
        //    break;
        //case 2:
        //    myAlert("Valor NO Permitido", "El campo '" + agElement.attr("title").toUpperCase() + "', NO puede contener valores númericos menores que cero!", true, false, TipeMsj.WARNING);
        //    break;
        //case 3:
        //    myAlert("Fecha NO Permitida", "El campo '" + agElement.attr("title").toUpperCase() + "', requiere un valor de tipo FECHA!", true, false, TipeMsj.WARNING);
        //    break;
        //case 4:
        //    myAlert("Fecha NO Permitida", "El campo '" + agElement.attr("title").toUpperCase() + "', requiere un formato de tipo '" + agFormato + "'!", true, false, TipeMsj.WARNING);
        //    break;

        case 1:
            alert("El campo '" + agElement.attr("title").toUpperCase() + "', NO puede contener valores nulos!");
            break;
        case 2:
            alert("El campo '" + agElement.attr("title").toUpperCase() + "', NO puede contener valores númericos menores que cero!");
            break;
        case 3:
            alert("El campo '" + agElement.attr("title").toUpperCase() + "', requiere un valor de tipo FECHA!");
            break;
        case 4:
            alert("El campo '" + agElement.attr("title").toUpperCase() + "', requiere un formato de tipo '" + agFormato + "'!");
            break;
        case 5:
            alert("El campo '" + agElement.attr("title").toUpperCase() + "', NO puede ser mayor '" + agElement2.attr("title").toUpperCase() + "'!");
            break;
    }
}
//

/*Función que permite crea el mensaje*/
//function myAlert(agTitle, agMensaje, agbtnOK, agbtnYes, agTipoMsj) {
//    var IconMsj = agTipoMsj == 1 ? '<i class="fa fa-exclamation-triangle align-middle"></i>' : "";
//    var BgColorMsj = agTipoMsj == 1 ? "bg-warning" : "";
//    var botonOK = agbtnOK && agTipoMsj == 1 ? '<button type="button" class="btn-warning" onclick="fnDeleteModal()">OK</button>' : '';

//    var Modal = '<div id="myAlert" class="modal" role="dialog">\
//    <div class="modal-dialog modal-dialog-centered" role="document">\
//        <div class="modal-content">\
//            <div class="modal-header '+BgColorMsj+'">\
//                <h5 class="modal-title" id="myAlertTitle">'+ agTitle + '</h5>\
//                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
//                        <span aria-hidden="true">&times;</span>\
//                    </button>\
//            </div>\
//            <!-- dialog body -->\
//            <div class="modal-body">'+IconMsj + " " +agMensaje+'</div>\
//            <div class="modal-footer">' + botonOK
//            '</div>\
//        </div>\
//        </div>\
//    </div>';
//    $('body').append(Modal);
//    $("#myAlert").modal("show");
//}

//function fnDeleteModal() {
//    $('.modal').modal('hide');
//    $('.modal-backdrop').remove() // removes the grey overlay.
//    //$('#myAlert.in:visible').modal('hide');
//}

//$('.modal').on('hidden', function () {
//    $("#myAlert").remove();
//});
