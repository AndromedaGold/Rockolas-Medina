$("#datepickerVenta").datepicker(
    {
        dateFormat: "dd/mm/yy",
        changeMonth: true,
        changeYear: true
    }
);

listar();
function listar() {
    $.get("Venta/listarVentas", function (data) {
        crearListado(["ID", "Color", "Precio", "Nombre Cliente", "Fecha Venta", "IDrockola", "Tipo Rokola", "Habilitado"], data)
    });
}

$.get("Venta/listarTipo", function (data) {
    llenarCombo(data, document.getElementById("cboVenta"), true)
    llenarCombo(data, document.getElementById("cboTipoPopup"), true)
});

var btnBuscar = document.getElementById("btnBuscar");
btnBuscar.onclick = function () {
    var idtipo = document.getElementById("cboVenta").value;
    if (idtipo == "") {
        listar();
    } else
        $.get("Venta/filtrarTipo/?idTipo=" + idtipo, function (data) {
            crearListado(["ID", "Color", "Precio","Nombre Cliente","Fecha Venta","IDrockola","Tipo Rokola","Habilitado"], data)
        });
}

function borrar(id) {
    if (confirm("¿Desea eliminar el dato?") == 1) {
        $.get("Venta/borrar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrió un error");
            } else {
                alert("Se elimino correctamente");
                listar();
            }
        });
    }
}

function llenarCombo(data, control, primerElemento) {
    var contenido = "";
    if (primerElemento == true) {
        contenido += "<option value=''>--Seleccione--</option>";
    }
    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].ID + "'>";
        contenido += data[i].tipo;
        contenido += "</option>";
    }
    control.innerHTML = contenido;
}

var btnLimpiar = document.getElementById("btnLimpiar");
btnLimpiar.onclick = function () {
    listar();
}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-venta' class='table'>";
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

    $("#tabla-venta").dataTable({
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
        $.get("Venta/recuperarInfo/?id=" + id, function (data) {
            document.getElementById("txtIDventa").value = data[0].IDventa;
            document.getElementById("txtColor").value = data[0].Color;
            document.getElementById("txtPrecio").value = data[0].Precio;
            document.getElementById("txtClienteNom").value = data[0].NombreCliente;
            document.getElementById("datepickerVenta").value = data[0].fechaVen;
            document.getElementById("txtIDrockola").value = data[0].IDrockola;
            document.getElementById("cboTipoPopup").value = data[0].IDtiporockola;
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

function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIDventa").value;
        var color = document.getElementById("txtColor").value;
        var precio = document.getElementById("txtPrecio").value;
        var cliente = document.getElementById("txtClienteNom").value;
        var fecha = document.getElementById("datepickerVenta").value;
        var rockola = document.getElementById("txtIDrockola").value;
        var tipo = document.getElementById("cboTipoPopup").value;
        var habilitado = document.getElementById("txtHabilitado").value;

        frm.append("IDventa", id);
        frm.append("Color", color);
        frm.append("Precio", precio);
        frm.append("NombreCliente", cliente);
        frm.append("FechaVenta", fecha);
        frm.append("IDrockola", rockola);
        frm.append("IDtiporockola", tipo);
        frm.append("Habilitado", habilitado);

        if (confirm("¿Desea guardar los cambios?") == 1) {
            $.ajax({
                type: "POST",
                url: "Venta/guardarInfo",
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
                            alert("La venta ya existe");
                        }
                        alert("Ocurrio un error");
                    }
                }
            });
        }
    }
}
