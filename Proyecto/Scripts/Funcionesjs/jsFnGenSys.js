//function log(message) {
//    var client = new XMLHttpRequest();
//    client.open("POST", "/log");
//    client.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
//    client.send(message);
//}

// #region Funciones genericas
/*Función que permite seleccionar un tipo mensajes*/
var TipoFormatoDate = {
    Short_Date: "dd/MM/yyyy",
    ISO_Date: "yyyy-MM-dd",
    LongDate_Date: "MMM/dd/yyyy"
}

/*=Función que permite configurar todos los datetimepicker del aplicativo, establecer su fecha de inicio, establecer su fecha final y establecer de quien depende un datetimepicker para restringir su fecha*/
$.fn.calendarApp = function (options) {
    $(this).datepicker({
        dateFormat: 'dd/mm/yy',
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
        dayNames: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
        changeYear: true
    });
}

/*=Función que permite realizar busquedas en cajas de texto según su url y devuelve hasta 5 regitros que coincidan con lo descrito en la caja de texto para despues ser cargados los datos que se seleccionarón , funciona para web transportes en general=*/
function autoComplete(element, urlData, dataOrigin, selectIt, displaySel) {
    $.each(String(element).split(","), function (k, v) {
        $(v).autocomplete({
            maxLength: 10,
            //Envia la informacion
            source: function (request, response) {
                var args = {};
                //alert(dataOrigin.datos);
                if (!dataOrigin) args = { 'id': request.term }; else args = dataOrigin.datos;
                $.ajax({
                    url: urlData,
                    async: true,
                    cache: true,
                    type: "POST",
                    dataType: "json",
                    data: args,
                    success: function (data) {
                        //alert(typeof (data) === "object")
                        response(data);
                        //                        alert(typeof (data) === "object");
                        //(typeof (data) === "object" ? data : JSON.parse(data))
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item === null) {
                    $(this).val('');
                }
            },
            //    $.getJSON(urlData, { 'id': $(v).val(), 'rel': !dataOrigin || dataOrigin == null ? null : $(dataOrigin).val() }, function (data, status, xhr) {
            //        response(data);
            //    }
            //    );
            //},


            //Evento que se activa cuando se situa el mouse sobre el item del autocomplete
            /*focus: !selectIt || selectIt == null ? function (event, ui) {
                $.each(ui.item, function (key, val) {
                    if (typeof (val) != 'undefined') {
                        $(v).val(val);
                        return false;
                    }
                });
                return false;
            } : selectIt,*/
            delay: 1000,
            //Evento que se activa cuando se selecciona el item.
            select: !selectIt || selectIt == null ? function (event, ui) {
                $.each(ui.item, function (key, val) {
                    if (typeof (val) != 'undefined') {
                        $(v).val(val);
                        return false;
                    }
                });
                return false;
            } : selectIt,
        }).data("ui-autocomplete")._renderItem = !displaySel || selectIt == null ? function (ul, item) //Visualiza tres items por detecto.
        {
            if (!ul.hasClass('lst-autocomplete'))
                ul.addClass('lst-autocomplete');
            //Crea la variable tipo array que almacena las llaves de la consulta.
            var fields = new Array();

            //Recorre los campos y asigna los valores en el array
            $.each(item, function (k, v) {
                if (typeof (v) != 'undefined')
                    fields.push(v);
            });
            //Devuelve el listado de items que coincidan con el elemento.
            return $('<li class="ui-autocomplete-header" role="presentation"  style="font-weight:bold !important;">').append('<a title="Clic para escoger la opción">' + fields[0] + ' ' + fields[1] + '</a>').appendTo(ul);
        } : displaySel;

    });
}

//
/*Función que permite convertir una tabla en datatable*/
function tableToDataTable(agTable) {
    //options._table : Tabla que va a ser convertida a DataTable
    var _MENU_ = "<select><option value='5'>5</option><option value='10'>10</option><option value='15'>15</option><option value='20'>20</option><option value='30'>30</option><option value='40'>40</option><option value='50'>50</option></select>";
    agTable.DataTable({
        "retrieve": true,
        "language": {
            //"lengthMenu": [[5, 10, 15, 20], [5, 10, 15, 20]],
            "lengthMenu": "Mostrar de a " + _MENU_ + " registros por página",
            // //"Mostrar _MENU_ registros",
            //"sLengthMenu": "<select>" +
            //"<option value='3'>3</option>" +
            //"<option value='6'>6</option>" +
            //"<option value='9'>9</option>" +
            //"<option value='12'>12</option>" +
            //"</select>",
            "pageLength": 5,
            //"iDisplayLength": "6",
            "zeroRecords": "No se encontraron resultados",
            "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            //"infoFiltered": "(filtered from _MAX_ total records)",
            "infoFiltered": "(filtro de _MAX_ registro(s) en total)",
            "search": "Buscar:",
            "responsive": true,
            "paginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            }
        }
    });
    //tbdt.destroy();
}
/**/
/*Función que permite validar elementos vacio*/
function ValidarElemtsVacios(agElementos) {
    var CampoVacio = false;
    $.each(agElementos, function (index, value) {
        if (!CampoVacio) {
            CampoVacio = IsNullOrEmpty(value);
            if (CampoVacio)
                MsjApp(value, 1);
        }
    });
    return CampoVacio;
}
//

/*Función que permite validar fecha valida*/
function ValidarElemtsFecha(agElementos, agtipoFormato) {
    var FechaCorrecta = true;
    $.each(agElementos, function (index, value) {
        if (FechaCorrecta)
            FechaCorrecta = EsFecha(value, agtipoFormato == 1 ? TipoFormatoDate.Short_Date : TipoFormatoDate.ISO_Date);
    });
    return FechaCorrecta;
}
//

/*Función que permite validar uan fecha sea mayor que otra*/
function FechaIniMayorFechaFin(agFechaIni, agFechaFin) {
    var parts1 = $(agFechaIni).val().split("/");
    var parts2 = $(agFechaFin).val().split("/");
    var d1 = new Date(Number(parts1[2]), Number(parts1[1]) - 1, Number(parts1[0]));
    var d2 = new Date(Number(parts2[2]), Number(parts2[1]) - 1, Number(parts2[0]));
    //alert("fecIni==>" + d1);
    //alert("fecFin==>" + d2);
    //alert("fecIFin==>" + $(agFechaFin).val());
    //alert("fecIniPaseada==>" + Date.parse($(agFechaIni).val()));
    //alert("fecIFinPaseada==>" + Date.parse($(agFechaFin).val()));
    //alert(Date.parseExact($(agFechaIni).val(), "dd/MM/yyyy"));
    if (d1 > d2) {
        MsjApp(agFechaIni, 5, null, agFechaFin);
        return true;
    }
    else
        return false;
};
//

function EsFecha(agFecha, agTipoFormato) {
    //alert(agFecha+ "///"+agTipoFormato);
    var EsFechaOK = true;
    var FormatoFecha = agTipoFormato == "dd/MM/yyyy" ? /^(\d{1,2})(\/)(\d{1,2})(\/)(\d{4})$/ : /^(\d{4})(\-)(\d{1,2})(\-)(\d{1,2})$/;
    var matchArray = $(agFecha).val().match(FormatoFecha)

    if (matchArray == null)// format NOT ok?
    {
        EsFechaOK = false;
        MsjApp(agFecha, 4, agTipoFormato);
    }
    else {
        var mes = 0;
        var dia = 0;
        var anio = 0;
        if (agTipoFormato == "dd/MM/yyyy") {
            var FechaPar = $(agFecha).val().split("/");
            dia = parseInt(FechaPar[0]);// p@rse date into variables
            mes = parseInt(FechaPar[1]);
            anio = parseInt(FechaPar[2]);
        }

        if (mes < 1 || mes > 12) // check month range
            EsFechaOK = false;
        else if (dia < 1 || dia > 31)
            EsFechaOK = false;
        else if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && mes == 31)
            EsFechaOK = false;
        if (mes == 2) { // check for february 29th
            var isleap = (anio % 4 == 0 && (anio % 100 != 0 || anio % 400 == 0));
            if (dia > 29 || (dia == 29 && !isleap))
                EsFechaOK = false;
        }

        if (!EsFechaOK) 
            MsjApp(agFecha, 3);
    }
    return EsFechaOK;
}

function AnioDiferente(agFechaIni, agFechaFin)
{
    var dateFormat1 = $(agFechaIni).val().split("/");
    var dateFormat2 = $(agFechaFin).val().split("/");
    if (parseInt(dateFormat1[2]) != parseInt(dateFormat2[2]))
    {
        alert("Debe establecer el mismo año para poder realizar la consulta!");
        return true;
    }
    else
        return false;
}

//
/*Función que permite validar un elemento vacio*/
function IsNullOrEmpty(agElemento)
{
    var vacio = false; 
    if (agElemento.is("input") && ($(agElemento).val() == "" || $(agElemento).val().length <= 0 || $(agElemento).val() == null))
        vacio = true;
    else if (agElemento.is("label") && ($(agElemento).text == "" || $(agElemento).text.length <= 0 || $(agElemento).text == null))
        vacio = true;
    return vacio;
}

/*Función que permite envair via AJAX*/
function SendViaAjax(_type, _url, _data, _respuesta, _dataType, _eventTermino) {
    $.ajax({
        type: _type,
        url: _url,
        dataType: _dataType ? _dataType : null,
        data: _data,
        success: _respuesta,
        ajaxComplete: _eventTermino
    });
}

/*Función de autocompletar*/
//$.fn.autocompletar = function (agOptions) {
//    $(this).autocomplete({
//        source: function (request, response) {
//            var args = {};
//            if (!agOptions.datos) args = { 'id': request.term }; else args = agOptions.datos();
//            /*            $.post(options.url, args, function (data) {
//                            response(data);
//                        });*/
//            $.ajax({
//                url: agOptions.url,
//                async: true,
//                cache: true,
//                type: "POST",
//                dataType: "json",
//                //                contentType: "application/json",
//                data: args,
//                success: function (data) {
//                    //alert(JSON.stringify(data));
//                    response(data);
//                },
//            });

//        },
//        minLength: 0,
//        delay: 500,
//        select: !agOptions.onSelect ? null : agOptions.onSelect,
//        open: function () {
//            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
//            $(this).css('z-index', '9999999');
//        },
//        close: function () {
//            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
//            !agOptions.onSelect ? null : agOptions.onSelect();
//        }
//    });
//};
// #endregion

