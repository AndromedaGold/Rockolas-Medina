$("#datepickerRockola").datepicker(
    {
        dateFormat: "dd/mm/yy",
        changeMonth:true,
        changeYear:true
    }
);

listar();

function listar() {
    $.get("Rockola/listarRockola", function (data) {
        crearListado(["ID","Placa","RAM","CPU","Fuente","HDD Cap","Monitor","Teclado","Fecha Fab","Habilitado"], data)
    })
}

var btnBuscar = document.getElementById("btnBuscar");
btnBuscar.onclick = function () {
    var nombre = document.getElementById("txtnombre").value;
    $.get("Rockola/buscarRockola/?nombre=" + nombre, function (data) {
        crearListado(["ID","Placa","RAM","CPU","Fuente","HDD Cap","Monitor","Teclado","Fecha Fab","Habilitado"],data);
    })     
}

var btnLimpiar = document.getElementById("btnLimpiar");
btnLimpiar.onclick = function () {
    listar();
}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tabla-rockola' class='table'>";
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

    $("#tabla-rockola").dataTable({
        searching: false
    });
}

function eliminar(id) {
    if (confirm("¿Desea eliminar el registro?") == 1) {
        var frm = new FormData();
        frm.append("IDrockola", id);
        $.ajax({
            type: "POST",
            url: "Rockola/eliminar",
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
        $.get("Rockola/llamarInfo/?id=" + id, function (data) {
            document.getElementById("txtIDrockola").value = data[0].IDrockola;
            document.getElementById("txtPlaca").value = data[0].motherb;
            document.getElementById("txtRam").value = data[0].ram;
            document.getElementById("txtCPU").value = data[0].procesador;
            document.getElementById("txtFuenteP").value = data[0].fuentep;
            document.getElementById("txtHDDCap").value = data[0].hddcapacidad;
            document.getElementById("txtMonitor").value = data[0].monitor;
            document.getElementById("txtTeclado").value = data[0].teclado;
            document.getElementById("datepickerRockola").value = data[0].fechaFabricacion;
            document.getElementById("txtHabilitado").value = data[0].habilitado;
        });
    }
}

function Agregar() {
    if (datosObligatorios() == true) {
        var frm = new FormData();
        var idrockola = document.getElementById("txtIDrockola").value;
        var motherb = document.getElementById("txtPlaca").value;
        var ram = document.getElementById("txtRam").value;
        var cpu = document.getElementById("txtCPU").value;
        var fuente = document.getElementById("txtFuenteP").value;
        var hdd = document.getElementById("txtHDDCap").value;
        var monitor = document.getElementById("txtMonitor").value;
        var teclado = document.getElementById("txtTeclado").value;
        var fecha = document.getElementById("datepickerRockola").value;
        var habilitado = document.getElementById("txtHabilitado").value;

        frm.append("IDrockola", idrockola);
        frm.append("motherb", motherb);
        frm.append("ram", ram);
        frm.append("procesador", cpu);
        frm.append("fuentep", fuente);
        frm.append("hddcapacidad", hdd);
        frm.append("monitor", monitor);
        frm.append("teclado", teclado);
        frm.append("fechaFabricacion", fecha);
        frm.append("habilitado", habilitado);

        if (confirm("¿Desea realizar la operacion?") == 1) {
            $.ajax({
                type: "POST",
                url: "Rockola/guardar",
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data != 0) {
                        listar();
                        alert("El dato se guardo correctamente");
                        document.getElementById("btnCancelar").click();
                    } else {
                        alert("Ocurrio un error");
                    }
                }
            });
        }
    } else {

    }
}
