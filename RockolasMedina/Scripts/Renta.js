$("#datepickerRenta").datepicker(
    {
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true
    }
);

listar();
function listar() {
    $.get("Renta/listarRenta", function (data) {
        crearListado(["ID", "Cliente", "DPI", "Telefono", "Direccion", "Rockola", "Ruta", "Municipio", "Fecha Renta", "Habilitado"], data);
    })
}

var btnBuscar = document.getElementById("btnBuscar");
btnBuscar.onclick = function () {
    var id = document.getElementById("txtid").value;
    $.get("Renta/buscarRenta/?id=" + id, function (data) {
        crearListado(["ID", "Cliente", "DPI", "Telefono", "Direccion", "Rockola", "Ruta", "Municipio", "Fecha Renta", "Habilitado"], data);
    })
}

var btnLimpiar = document.getElementById("btnLimpiar");
btnLimpiar.onclick = function () {
    listar();
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
        $.get("Renta/llamarInfo/?id=" + id, function (data) {
            document.getElementById("txtIDrenta").value = data[0].IDrenta;
            document.getElementById("txtCliente").value = data[0].ClienteRentaNombre;
            document.getElementById("txtDPI").value = data[0].DPInumero;
            document.getElementById("txtTelefono").value = data[0].TelefonoCliente;
            document.getElementById("txtDireccion").value = data[0].Direccion;
            document.getElementById("txtRockola").value = data[0].IDrockola;
            document.getElementById("cboRutaPopup").value = data[0].IDruta;
            document.getElementById("txtMunicipio").value = data[0].IDmunicipio;
            document.getElementById("datepickerRenta").value = data[0].FechaRenta;
            document.getElementById("txtHabilitado").value = data[0].Habilitado;
        });
    }
}

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

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-renta' class='table'>";
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

    $("#tabla-renta").dataTable({
        searching: false
    });
}

function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIDrenta").value;
        var cliente = document.getElementById("txtCliente").value;
        var dpi = document.getElementById("txtDPI").value;
        var telefono = document.getElementById("txtTelefono").value;
        var direccion = document.getElementById("txtDireccion").value;
        var rockola = document.getElementById("txtRockola").value;
        var ruta = document.getElementById("cboRutaPopup").value;
        var municipio = document.getElementById("txtMunicipio").value;
        var fecha = document.getElementById("datepickerRenta").value;
        var habilitado = document.getElementById("txtHabilitado").value;

        frm.append("IDrenta", id);
        frm.append("ClienteRentaNombre", cliente);
        frm.append("DPInumero", dpi);
        frm.append("TelefonoCliente", telefono);
        frm.append("Direccion", direccion);
        frm.append("IDrockola", rockola);
        frm.append("cboRuta", ruta);
        frm.append("IDmunicipio", municipio);
        frm.append("FechaRenta", fecha);
        frm.append("Habilitado", habilitado);

        if (confirm("¿Desea realizar la operacion?") == 1) {
            $.ajax({
                type: "POST",
                url: "Renta/guardar",
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
                            alert("La renta ya existe");
                        }
                        alert("Ocurrio un error");
                    }
                }
            });
        }else {

        }
    }
}

function eliminar(id) {
    if (confirm("¿Desea eliminar el dato?") == 1) {
        $.get("Renta/eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                alert("El dato se eliminó correctamente");
                listar();
            }
        });
    }
}