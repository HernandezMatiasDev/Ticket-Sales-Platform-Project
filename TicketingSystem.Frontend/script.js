const API_URL = "http://localhost:5029/api/Events";
const AUTH_URL = "http://localhost:5029/api/Auth";

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
                    <p class="text-muted"><i class="bi bi-calendar"></i> ${new Date(evento.eventDate || evento.date).toLocaleDateString()}</p>
                </div>
                <div class="card-footer bg-white border-0">
                    <button class="btn btn-success w-100 rounded-pill" onclick="mostrarDetalles(${evento.id}, '${evento.name.replace(/'/g, "\\'")}')">
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

let currentEventId = null;

function mostrarDetalles(id, nombre) {
    currentEventId = id;
    document.getElementById('seatMapModalLabel').innerText = `Asientos para: ${nombre}`;
    document.getElementById('sectorSelect').value = "1"; // Por defecto cargar VIP
    cargarAsientos(id, 1);
    
    const modal = new bootstrap.Modal(document.getElementById('seatMapModal'));
    modal.show();
}

document.getElementById('sectorSelect')?.addEventListener('change', (e) => {
    if (currentEventId) {
        cargarAsientos(currentEventId, e.target.value);
    }
});

async function cargarAsientos(eventId, sectorId) {
    const contenedorAsientos = document.getElementById('contenedor-asientos');
    contenedorAsientos.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div>';
    
    try {
        const res = await fetch(`${API_URL}/${eventId}/sectors/${sectorId}/seats`);
        if (!res.ok) throw new Error("Error al cargar los asientos");
        
        const asientos = await res.json();
        
        if (asientos.length === 0) {
            contenedorAsientos.innerHTML = '<p class="text-muted">No hay asientos configurados en este sector.</p>';
            return;
        }

        let html = '<div class="d-flex flex-wrap gap-2 justify-content-center">';
        asientos.forEach(a => {
            // status: 0 = Available, 1 = Reserved, 2 = Sold
            let colorClass = a.status === 0 ? 'btn-outline-success' : (a.status === 1 ? 'btn-warning' : 'btn-danger');
            let disabled = a.status !== 0 ? 'disabled' : '';
            
            html += `<button class="btn ${colorClass}" ${disabled} onclick="reservarAsiento(${eventId}, '${a.id}')" title="Fila: ${a.row || '-'}">
                        ${a.number}
                     </button>`;
        });
        html += '</div>';
        contenedorAsientos.innerHTML = html;
    } catch (error) {
        contenedorAsientos.innerHTML = `<p class="text-danger">Hubo un error al cargar el mapa de asientos.</p>`;
    }
}

// === Lógica de Autenticación ===

function actualizarUI() {
    const token = localStorage.getItem('jwt_token');
    const email = localStorage.getItem('user_email');
    const authButtons = document.getElementById('auth-buttons');
    const userMenu = document.getElementById('user-menu');
    const userEmailSpan = document.getElementById('user-email');

    if (token && email) {
        if (authButtons) authButtons.classList.add('d-none');
        if (userMenu) userMenu.classList.remove('d-none');
        if (userEmailSpan) userEmailSpan.textContent = email;
    } else {
        if (authButtons) authButtons.classList.remove('d-none');
        if (userMenu) userMenu.classList.add('d-none');
        if (userEmailSpan) userEmailSpan.textContent = '';
    }
}

document.getElementById('loginForm')?.addEventListener('submit', async (e) => {
    e.preventDefault();
    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;
    const msgDiv = document.getElementById('loginMessage');
    
    try {
        const res = await fetch(`${AUTH_URL}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        const data = await res.json();
        
        if (res.ok) {
            localStorage.setItem('jwt_token', data.token);
            localStorage.setItem('user_email', email);
            
            // Cerrar modal exitosamente
            const modalEl = document.getElementById('loginModal');
            const modal = bootstrap.Modal.getInstance(modalEl) || new bootstrap.Modal(modalEl);
            modal.hide();
            
            actualizarUI();
            document.getElementById('loginForm').reset();
            msgDiv.classList.add('d-none');
        } else {
            msgDiv.classList.remove('d-none');
            msgDiv.className = 'alert alert-danger mb-3';
            msgDiv.textContent = data.error || 'Error al iniciar sesión';
        }
    } catch (error) {
        console.error(error);
        msgDiv.classList.remove('d-none');
        msgDiv.className = 'alert alert-danger mb-3';
        msgDiv.textContent = 'Error de conexión con el servidor.';
    }
});

document.getElementById('registerForm')?.addEventListener('submit', async (e) => {
    e.preventDefault();
    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;
    const msgDiv = document.getElementById('registerMessage');
    
    try {
        const res = await fetch(`${AUTH_URL}/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        const data = await res.json();
        
        msgDiv.classList.remove('d-none');
        if (res.ok) {
            msgDiv.className = 'alert alert-success mb-3';
            msgDiv.textContent = data.message || 'Registro exitoso. Ahora puedes iniciar sesión.';
            document.getElementById('registerForm').reset();
        } else {
            msgDiv.className = 'alert alert-danger mb-3';
            // Procesar errores de validación de Identity si es que vienen en una lista
            msgDiv.textContent = data.error || (data.errors ? Object.values(data.errors).flat().join(', ') : 'Error al registrarse');
        }
    } catch (error) {
        console.error(error);
        msgDiv.classList.remove('d-none');
        msgDiv.className = 'alert alert-danger mb-3';
        msgDiv.textContent = 'Error de conexión con el servidor.';
    }
});

function cerrarSesion() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_email');
    actualizarUI();
}

async function reservarAsiento(eventId, seatId) {
    const token = localStorage.getItem('jwt_token');
    if (!token) {
        alert("Debes iniciar sesión para realizar una reserva.");
        const seatModal = bootstrap.Modal.getInstance(document.getElementById('seatMapModal'));
        if (seatModal) seatModal.hide();
        
        const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
        loginModal.show();
        return;
    }

    try {
        const res = await fetch(`http://localhost:5029/api/v1/events/${eventId}/seats/${seatId}/reservations`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const data = await res.json();
        
        if (res.ok) {
            alert("¡Reserva exitosa! Tienes 5 minutos para confirmar el pago.");
            const sectorId = document.getElementById('sectorSelect').value;
            cargarAsientos(eventId, sectorId); // Refrescar el mapa para que se desactive el botón
        } else {
            alert("Error al reservar: " + (data.error || "La butaca no está disponible."));
        }
    } catch (e) {
        alert("Error de conexión al procesar la reserva.");
    }
}

// Llamamos a la función
actualizarUI();
obtenerEventos();