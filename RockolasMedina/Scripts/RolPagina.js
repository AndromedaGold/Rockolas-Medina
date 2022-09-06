listar();
function listar() {
    $.get("RolPagina/listarRol", function (data) {
        crearListado(["ID Rol","Nombre","Descripción"], data);
    });
}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-rolpagina' class='table'>";
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

    $("#tabla-rolpagina").dataTable({
        searching: false
    });
}

function borrarDatos() {
    var controles = document.getElementsByClassName("borrar");
    var ncontroles = controles.length;
    for (var i = 0; i < ncontroles; i++) {
        controles[i].value = "";
    }
}

var idRol;
function abrirModal(id) {
    idRol = id;
    var controlesObligatorios = document.getElementsByClassName("obligatorio");
    var ncontroles = controlesObligatorios.length;
    for (var i = 0; i < ncontroles; i++) {
        controlesObligatorios[i].parentNode.classList.remove("error");
    }

    $.get("RolPagina/listarPaginas", function (data) {

        var contenido = "<tbody>";
            for (var i = 0; i < data.length; i++) {
                contenido += "<tr>";
                contenido += "<td>";
                contenido += "<input class='checkbox' type='checkbox' id='" + data[i].IDpagina + "' />"
                contenido += "</td>";
                contenido += "<td>";
                contenido += data[i].Mensaje;
                contenido += "</td>";
                contenido += "</tr>";
            }
        contenido += "</tbody>";
        document.getElementById("tblPagina").innerHTML = contenido;
        if (id > 0) {
            obtenerRolPagina();
        }
    })
    if (id == 0) {
        borrarDatos();
    } 
}

function obtenerRolPagina() {
    $.get("RolPagina/obtenerRolPagina/?idRol=" + idRol, function (data) {
        var nregistros = data.length;
        for (var i = 0; i < nregistros; i++) {
            if (data[i].Habilitado == 1) {
                document.getElementById(data[i].IDpagina).checked = true;
            }
        }
    })

    $.get("RolPagina/obtenerRol/?idRol=" + idRol, function (data) {
        document.getElementById("txtIDrol").value = data.IDrol;
        document.getElementById("txtNombreRol").value = data.Nombre;
        document.getElementById("txtDescripcion").value = data.Descripcion;
    })
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
        var id = document.getElementById("txtIDrol").value;
        var rol = document.getElementById("txtNombreRol").value;
        var depto = document.getElementById("txtDescripcion").value;

        frm.append("IDrol", id);
        frm.append("Nombre", rol);
        frm.append("Descripcion", depto);
        frm.append("Habilitado", 1);

        var checkbox = document.getElementsByClassName("checkbox");
        var ncheckbox = checkbox.length;
        var dataEnviar = "";

        for (var i = 0; i < ncheckbox; i++) {
            if (checkbox[i].checked == true) {
                dataEnviar += checkbox[i].id;
                dataEnviar += "$";
            }
        }
        dataEnviar = dataEnviar.substring(0, dataEnviar.length - 1);
        frm.append("dataEnviar", dataEnviar);

        if (confirm("¿Desea guardar los cambios?") == 1) {
            $.ajax({
                type: "POST",
                url: "RolPagina/guardarDatos",
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 0) {
                        alert("Ocurrió un error");
                    } else {
                        alert("Se guardó correctamente");
                        document.getElementById("btnCancelar").click();
                        listar();
                    }
                }
            });
        }
    }
}

function eliminar(id) {
    if (confirm("¿Desea eliminar el dato?") == 1) {
        $.get("RolPagina/eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Ocurrio un error");
            } else {
                alert("El dato se eliminó correctamente");
                listar();
            }
        });
    }
}
