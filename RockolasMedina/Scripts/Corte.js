//funcion para mostrar el calendario
$("#datepickerCorte").datepicker(
    {
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true
    }
);
//funcion para listar 
listar();
function listar() {
    $.get("Corte/listarCortes", function (data) {
        crearListado(["ID","Cort/Parcial","Cort/Final","Efectivo","Fecha Corte","Rockola","Ruta","Tecnico","Habilitado"], data);
    });
}
//funcion para mostrar combos de rutas
$.get("Corte/listarRuta", function (data) {
    llenarCombo(data, document.getElementById("cboRuta"), true)
    llenarCombo(data, document.getElementById("cboRutaPopup"), true)
    
});

var btnBuscar = document.getElementById("btnBuscar");
btnBuscar.onclick = function () {
    var idruta = document.getElementById("cboRuta").value;
    if (idruta == "") {
        listar();
    } else 
    $.get("Corte/filtrarRuta/?idruta=" + idruta, function (data) {
        crearListado(["ID","Cort/Parcial","Cort/Final","Efectivo","Fecha Corte","Rockola","Ruta","Tecnico","Habilitado"], data);
    });
}

var btnLimpiar = document.getElementById("btnLimpiar");
btnLimpiar.onclick = function () {
    listar();
}

function llenarCombo(data, control, primerElemento) {
    var contenido = "";
    if (primerElemento == true) {
        contenido += "<option value=''>--Seleccione--</option>";
    }
    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].ID + "'>";
        contenido += data[i].Nombre;
        contenido += "</option>";
    }
    control.innerHTML = contenido;
}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-corte' class='table'>";
    contenido += "<thead>";

    contenido += "<tr>";
    for (var i = 0; i < arrayColumnas.length; i++) {
        contenido += "<td>";
        contenido += arrayColumnas[i];
        contenido += "</td>";
    }
    //Añadimos columna para las acciones
    contenido += "<td>Acciones</td>";
    contenido += "</tr>";

    contenido += "</thead>";
    var llaves = Object.keys(data[0]);
    contenido += "<tbody>";
    for (var i = 0; i < data.length; i++) {
        contenido += "<tr>";
        for (var j = 0; j < llaves.length; j++) {
            var valorLlaves = llaves[j];
            contenido += "<td>";
            contenido += data[i][valorLlaves];
            contenido += "</td>";
        }
        var llaveID = llaves[0];
        //Añadir iconos
        contenido += "<td>"
        contenido += "<button class='badge rounded-pill bg-success' onclick='abrirModal(" + data[i][llaveID] + ")' data-toggle='modal' data-target='#myModal'><i class='glyphicon glyphicon-edit'>Editar</i></ button> "
        contenido += " <button class='badge rounded-pill bg-danger' onclick='eliminar(" + data[i][llaveID] + ")'><i class='glyphicon glyphicon-trash'></i>Borrar</button>"
        contenido += "</td>"

        contenido += "</tr>";
    }
    contenido += "</tbody>";
    contenido += "</table>";

    document.getElementById("tabla").innerHTML = contenido;

    $("#tabla-corte").dataTable({
        searching: false
    });
}

function abrirModal(id) {
    var controlesObligatorios = document.getElementsByClassName("obligatorio");
    var ncontroles = controlesObligatorios.length;
    for (var i = 0; i < ncontroles; i++) {
        controlesObligatorios[i].parentNode.classList.remove("error");
    }
    if (id == 0) {
        borrarDatos();
    } else {
        $.get("Corte/recuperarInformacion/?id=" + id, function (data) {
            document.getElementById("txtIDcorte").value = data[0].IDcorte;
            document.getElementById("txtCParcial").value = data[0].ContadorParcial;
            document.getElementById("txtCFinal").value = data[0].ContadorFinal;
            document.getElementById("txtEfectivo").value = data[0].Efectivo;
            document.getElementById("datepickerCorte").value = data[0].Fecha;
            document.getElementById("txtRockola").value = data[0].IDrockola;
            document.getElementById("cboRutaPopup").value = data[0].IDruta;
            document.getElementById("txtTecnico").value = data[0].IDtecnico;
            document.getElementById("txtHabilitado").value = data[0].Habilitado;
        });     
    }
}

function borrarDatos() {
    var controles = document.getElementsByClassName("borrar");
    var ncontroles = controles.length;
    for (var i = 0; i < ncontroles; i++) {
        controles[i].value = "";
    }
}

function datosObligatorios() {
    var exito = true;
    var controlesObligatorios = document.getElementsByClassName("obligatorio");
    var ncontroles = controlesObligatorios.length;
    for (var i = 0; i < ncontroles; i++) {
        if (controlesObligatorios[i].value == "") {
            exito = false;
            controlesObligatorios[i].parentNode.classList.add("error");
        } else {
            controlesObligatorios[i].parentNode.classList.remove("error");
        }
    }
    return exito;
}



function eliminar(id) {
    if (confirm("¿Desea eliminar el elemento?") == 1) {
        $.get("Corte/eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                alert("Se elimino correctamente");
                listar();
            }
        });
    }
}


function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIDcorte").value;
        var cparcial = document.getElementById("txtCParcial").value;
        var cfinal = document.getElementById("txtCFinal").value;
        var efectivo = document.getElementById("txtEfectivo").value;
        var fecha = document.getElementById("datepickerCorte").value;
        var rockola = document.getElementById("txtRockola").value;
        var ruta = document.getElementById("cboRutaPopup").value;
        var tecnico = document.getElementById("txtTecnico").value;
        var habilitado = document.getElementById("txtHabilitado").value;

        frm.append("IDcorte", id);
        frm.append("ContadorParcial", cparcial);
        frm.append("ContadorFinal", cfinal);
        frm.append("Efectivo", efectivo);
        frm.append("FechaCorte", fecha);
        frm.append("IDrockola", rockola);
        frm.append("IDruta", ruta);
        frm.append("IDtecnico", tecnico);
        frm.append("habilitado", habilitado);

        if (confirm("¿Desea guardar los cambios?") == 1){
            $.ajax({
                type: "POST", 
                url: "Corte/guardar", 
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 1) {
                        alert("El dato se guardó correctamente");
                        listar();
                        document.getElementById("btnCancelar").click();                       
                    } else {
                        if (data == -1) {
                            alert("El corte a esa rockola ya existe");
                        }
                        alert("Ocurrio un error");
                    }
                }
            });
        }
    }
}
