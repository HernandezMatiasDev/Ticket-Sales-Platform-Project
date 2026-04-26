const API_URL = "http://localhost:5029/api/Events";

// Esta función se ejecuta apenas carga la página
async function obtenerEventos() 
{
    try {
        const respuesta = await fetch(API_URL);
        
        if (!respuesta.ok) {
            throw new Error("Error en la respuesta de la red");
        }

        const eventos = await respuesta.json();
        const contenedor = document.getElementById('contenedor-eventos');

        if (!contenedor) {
            console.error("No se encontró el elemento 'contenedor-eventos' en el HTML");
            return;
        }
        
        // Limpiamos el mensaje de "Cargando..."
        contenedor.innerHTML = '';

        if (eventos.length === 0) {
            contenedor.innerHTML = '<p class="alert alert-warning">No hay eventos creados todavía.</p>';
            return;
        }

        // Dibujamos cada evento
        eventos.forEach((evento) => {
    const card = `
        <div class="col-md-4 mb-4">
            <div class="card h-100 shadow border-0">
                <div class="card-header bg-dark text-white">
                    <small class="text-uppercase">Evento Próximo</small>
                </div>
                <div class="card-body text-center">
                    <h3 class="card-title text-primary">${evento.name}</h3>
                    <p class="badge bg-secondary">${evento.venue}</p>
                    <p class="text-muted"><i class="bi bi-calendar"></i> ${new Date(evento.date).toLocaleDateString()}</p>
                </div>
                <div class="card-footer bg-white border-0">
                    <button class="btn btn-success w-100 rounded-pill" onclick="mostrarDetalles('${evento.name}')">
                        Reservar Entradas
                    </button>
                </div>
            </div>
        </div>
    `;
    contenedor.innerHTML += card;
    console.log("¡He recibido datos del backend!", eventos);

});


    } catch (error) {
        console.error("Hubo un error:", error);
        const contenedor = document.getElementById('contenedor-eventos');
        if (contenedor) {
            contenedor.innerHTML = `
                <div class="alert alert-danger">
                    Error al conectar con el servidor. <br>
                    Detalle: ${String(error)}
                </div>`;
        }
    }
}

function mostrarDetalles(nombre) {
    alert("¡Has seleccionado: " + nombre + "! \nAquí es donde JavaScript pedirá los asientos a la API en el siguiente paso.");
}


// Llamamos a la función
obtenerEventos();