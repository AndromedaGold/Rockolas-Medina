listar();
function listar() {
    $.get("Municipio/listarMunicipio", function (data) {
        crearListado(["ID","Municipio","Departamento","Ruta","Habilitado"], data);
    })
}

var nombreDepto = document.getElementById("txtdepto");
nombreDepto.onkeyup = function () {
    var depto = document.getElementById("txtdepto").value;
    $.get("Municipio/buscarDepartamento/?depto=" + depto, function (data) {
        crearListado(["ID", "Municipio", "Departamento", "Ruta", "Habilitado"], data);
    });
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

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-municipio' class='table'>";
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

    $("#tabla-municipio").dataTable({
        searching: false
    });
}

function eliminar(id) {
    if (confirm("¿Desea eliminar el dato?") == 1) {
        $.get("Municipio/eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                alert("El dato se eliminó correctamente");
                listar();
            }
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

function abrirModal(id) {
    var controlesObligatorios = document.getElementsByClassName("obligatorio");
    var ncontroles = controlesObligatorios.length;
    for (var i = 0; i < ncontroles; i++) {
        controlesObligatorios[i].parentNode.classList.remove("error");
    }
    if (id == 0) {
        borrarDatos();
    } else {
        $.get("Municipio/recuperarInfo/?id=" + id, function (data) {
            document.getElementById("txtIDmunicipio").value = data[0].IDmunicipio;
            document.getElementById("txtMunicipio").value = data[0].Nombre;
            document.getElementById("txtDepto").value = data[0].Departamento;
            document.getElementById("cboRutaPopup").value = data[0].IDruta;          
            document.getElementById("txtHabilitado").value = data[0].Habilitado;
        });
    }
}

function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIDmunicipio").value;
        var muni = document.getElementById("txtMunicipio").value;
        var depto = document.getElementById("txtDepto").value;
        var ruta = document.getElementById("cboRutaPopup").value;
        var habilitado = document.getElementById("txtHabilitado").value;

        frm.append("IDmunicipio", id);
        frm.append("Nombre", muni);
        frm.append("Departamento", depto);
        frm.append("IDruta", ruta);       
        frm.append("Habilitado", habilitado);

        if (confirm("¿Desea guardar los cambios?") == 1) {
            $.ajax({
                type: "POST",
                url: "Municipio/guardarDatos",
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
                            alert("Ese Municipio ya existe");
                        }
                        alert("Ocurrio un error");
                    }
                }
            });
        }
    } else {

    }
}