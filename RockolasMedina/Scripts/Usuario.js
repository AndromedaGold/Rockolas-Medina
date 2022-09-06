listar();
function listar() {

    $.get("Usuario/listarUsuarios", function (data) {
        crearListado(["Id Usuario", "Nombre Completo", "Usuario", "Nombre Rol", "Tipo"], data);
    })
    $.get("Usuario/listarRol", function (data) {
        llenarCombo(data, document.getElementById("cboRol"), true);
    })
    $.get("Usuario/listarPersonas", function (data) {
        llenarCombo(data, document.getElementById("cboPersona"), true);
    })
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

function borrarDatos() {
    var controles = document.getElementsByClassName("borrar");
    var ncontroles = controles.length;
    for (var i = 0; i < ncontroles; i++) {
        controles[i].value = "";
    }
}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-usuario' class='table'>";
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

    $("#tabla-usuario").dataTable({
        searching: false
    });
}

function abrirModal(id) {
    var controlesObligatorio = document.getElementsByClassName("obligatorio");
    var ncontroles = controlesObligatorio.length;
    for (var i = 0; i < ncontroles; i++) {
        controlesObligatorio[i].parentNode.classList.remove("error");
    }
    if (id == 0) {
        document.getElementById("lblContraseña").style.display = "block";
        document.getElementById("txtContraseña").style.display = "block";
        document.getElementById("lblPersona").style.display = "block";
        document.getElementById("cboPersona").style.display = "block";
        borrarDatos();
    } else {
        document.getElementById("lblContraseña").style.display = "none";
        document.getElementById("txtContraseña").style.display = "none";
        document.getElementById("lblPersona").style.display = "none";
        document.getElementById("cboPersona").style.display = "none";
        document.getElementById("txtContraseña").value = "1";
        document.getElementById("cboPersona").value = "2";

        $.get("Usuario/recuperarDatos/?idUsuario=" + id, function (data) {
            document.getElementById("txtIDusuario").value = data.IDusuario;
            document.getElementById("txtNombre").value = data.Nombre;
            document.getElementById("cboRol").value = data.IDrol;

        })
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
    if (confirm("¿Desea eliminar el usuario?") == 1) {
        var frm = new FormData();
        frm.append("IDusuario", id);
        $.ajax({
            type: "POST",
            url: "Usuario/eliminar",
            data: frm,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == 0) {
                    alert("Ocurrio un error");
                }
                else {
                    alert("El usuario se eliminó correctamente");
                    listar();
                }
            }
        });
    }
}

function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIDusuario").value;
        var nombre = document.getElementById("txtNombre").value;
        var contra = document.getElementById("txtContraseña").value;
        var persona = document.getElementById("cboPersona").value;
        var rol = document.getElementById("cboRol").value;
        var nombrePersona = document.getElementById("cboPersona").options[document.getElementById("cboPersona").selectedIndex].text;

        frm.append("IDusuario", id);
        frm.append("Nombre", nombre);
        frm.append("Contraseña", contra);
        frm.append("ID", persona);
        frm.append("IDrol", rol);
        frm.append("nombreCompleto", nombrePersona);
        frm.append("Habilitado", 1);
      
        if (confirm("¿Desea guardar los cambios?") == 1) {
            $.ajax({
                type: "POST",
                url: "Usuario/guardarDatos",
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 1) {
                        alert("Se guardó correctamente");
                        document.getElementById("btnCancelar").click();
                        listar();
                    } else {
                        alert("Ocurrió un error");
                    }
                }
            });
        }
    } 
}