$("#datepickerReparacion").datepicker(
    {
        dateFormat: "dd/mm/yy",
        changeMonth:true,
        changeYear:true
    }
);

$.get("Corte/listarRuta", function (data) {
    llenarCombo(data, document.getElementById("cboRutaPopup"), true)
});

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

listar();

function listar() {
    $.get("Reparacion/listarReparacion", function (data) {
        crearListado(["ID","Parte","Ruta","Tecnico","Fecha Rep","Comentario","ID Rockola","Habilitado"], data);
    })
}

var nombreParte = document.getElementById("txtnombre");
nombreParte.onkeyup = function () {
    var nombre = document.getElementById("txtnombre").value;
    $.get("Reparacion/buscarReparacion/?nombreParte=" + nombre, function(data) {
        crearListado(["ID","Parte","Ruta","Tecnico","Fecha Rep","Comentario","ID Rockola","Habilitado"], data);
    });
}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-reparacion' class='table'>";
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

    $("#tabla-reparacion").dataTable({
        searching: false
    });
}

function eliminar(id) {
    if (confirm("¿Desea eliminar el registro?") == 1) {
        var frm = new FormData();
        frm.append("IDreparacion", id);
        $.ajax({
            type: "POST",
            url: "Reparacion/eliminarDatos",
            data: frm,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == 0) {
                    alert("Ocurrio un error");
                }
                else {
                    alert("El registro se eliminó correctamente");
                    listar();
                }
            }
        });
    }
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
        $.get("Reparacion/recuperarDatos/?id=" + id, function (data) {
            document.getElementById("txtIDreparacion").value = data[0].IDreparacion;
            document.getElementById("txtParte").value = data[0].ParteNombre;
            document.getElementById("cboRutaPopup").value = data[0].IDruta;
            document.getElementById("txtTecnico").value = data[0].IDtecnico;
            document.getElementById("datepickerReparacion").value = data[0].FechaReparacion;
            document.getElementById("txtComentario").value = data[0].Comentario;
            document.getElementById("txtRockola").value = data[0].IDrockola;
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

function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIDreparacion").value;
        var parte = document.getElementById("txtParte").value;
        var ruta = document.getElementById("cboRutaPopup").value;
        var tecnico = document.getElementById("txtTecnico").value;
        var fecha = document.getElementById("datepickerReparacion").value;
        var comentario = document.getElementById("txtComentario").value;
        var rockola = document.getElementById("txtRockola").value;
        var habilitado = document.getElementById("txtHabilitado").value;

        frm.append("IDreparacion", id);
        frm.append("ParteNombre", parte);
        frm.append("IDruta", ruta);
        frm.append("IDtecnico", tecnico);
        frm.append("FechaReparacion", fecha);
        frm.append("Comentario", comentario);
        frm.append("IDrockola", rockola);
        frm.append("Habilitado", habilitado);

        if (confirm("¿Desea guardar los cambios?") == 1) {
            $.ajax({
                type: "POST",
                url: "Reparacion/guardarDatos",
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
                            alert("Esa reparacion ya existe");
                        }
                        alert("Ocurrio un error");
                    }
                }
            });
        }
    } else {

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