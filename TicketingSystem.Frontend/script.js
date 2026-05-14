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
                <div class="card-header  text-black">
                    <small class="text-uppercase card-header">Evento Próximo</small>
                </div>
                <div class="card-body text-center">
                    <h3 class="card-title text-primary">${evento.name}</h3>
                    <p class="badge">${evento.venue}</p>
                    <p class="text-muted"><i class="bi bi-calendar"></i> ${new Date(evento.eventDate || evento.date).toLocaleDateString()}</p>
                </div>
                <div class="card-footer border-0">
                    <button class="btn w-100 " onclick="mostrarDetalles(${evento.id}, '${evento.name.replace(/'/g, "\\'")}')">
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


async function mostrarDetalles(id, nombre) {
    currentEventId = id;
    document.getElementById('seatMapModalLabel').innerText = `Asientos para: ${nombre}`;
    
    const sectorSelect = document.getElementById('sectorSelect');
    sectorSelect.innerHTML = '<option value="">Cargando sectores...</option>';

    try {
        const res = await fetch(`${API_URL}/${id}/sectors`);
        if (!res.ok) throw new Error("Error al cargar los sectores");
        
        const sectores = await res.json();
        sectorSelect.innerHTML = '';
        
        if (sectores.length > 0) {
            sectores.forEach(sector => {
                sectorSelect.innerHTML += `<option value="${sector.id}" data-price="${sector.price}">${sector.name}</option>`;
            });
            
            // Cargar asientos del primer sector por defecto
            cargarAsientos(id, sectorSelect.value);
        } else {
            sectorSelect.innerHTML = '<option value="">No hay sectores disponibles</option>';
            document.getElementById('contenedor-asientos').innerHTML = '<p class="text-muted">No hay asientos disponibles.</p>';
        }
    } catch (error) {
        console.error("Hubo un error cargando sectores:", error);
        sectorSelect.innerHTML = '<option value="">Error al cargar sectores</option>';
    }

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

        let html = '<div class="seat-map-wrapper">';
        html += '<div class="stage-indicator">ESCENARIO</div>';
        html += '<div class="seat-map">';
        asientos.forEach(a => {
            // status: 0 = Available, 1 = Reserved, 2 = Sold
            let colorClass = a.status === 0 ? 'btn-outline-success' : (a.status === 1 ? 'btn-warning' : 'btn-danger');
            let disabled = a.status !== 0 ? 'disabled' : '';
            
        html += `
        <button 
            class="btn ${colorClass} seat-btn" 
            ${disabled}
            onclick="reservarAsiento(${eventId}, '${a.id}', ${a.number})"
            title="Fila: ${a.row || '-'}, Asiento: ${a.number}"
        >
            ${a.number}
        </button>`;

        });
        html += '</div></div>';
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
            
            window.location.reload();
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
    window.location.reload();
}

async function reservarAsiento(eventId, seatId, seatNumber) {
    const sectorSelect = document.getElementById('sectorSelect');
    const sectorName = sectorSelect.options[sectorSelect.selectedIndex].text;
    const price = sectorSelect.options[sectorSelect.selectedIndex].getAttribute('data-price');

    if (!confirm(`¿Seguro que quieres seleccionar la silla ${seatNumber} del sector ${sectorName} por $${price}?`)) return;

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
        
        if (res.status === 401) {
            alert("Tu sesión ha expirado. Por favor, inicia sesión nuevamente.");
            cerrarSesion();
            
            const seatModal = bootstrap.Modal.getInstance(document.getElementById('seatMapModal'));
            if (seatModal) seatModal.hide();
            
            const loginModal = new bootstrap.Modal(document.getElementById('loginModal'));
            loginModal.show();
            
            return;
        }

        const text = await res.text();
        const data = text ? JSON.parse(text) : {};
        
        if (res.ok) {
            const selectElement = document.getElementById('sectorSelect');
            const sectorId = selectElement.value;
            cargarAsientos(eventId, sectorId);
            cargarCarrito(); // Refrescar el carrito persistente
        } else {
            const errorMsg = data.error || "La butaca no está disponible.";
            alert(`No se pudo realizar la reserva: ${errorMsg}\n\nEl mapa de asientos se actualizará para reflejar la disponibilidad más reciente.`);
            const sectorId = document.getElementById('sectorSelect').value;
            cargarAsientos(eventId, sectorId);
        }
    } catch (e) {
        console.error("Detalle técnico del error:", e);
        alert("Error crítico al procesar la petición. Presiona F12 y revisa la consola para ver el detalle exacto.");
    }
}

let timerInterval = null;

async function cargarCarrito() {
    const token = localStorage.getItem('jwt_token');
    const cartPanel = document.getElementById('cart-panel');
    if (!token) {
        cartPanel.classList.add('d-none');
        clearInterval(timerInterval);
        return;
    }

    try {
        const res = await fetch(`http://localhost:5029/api/v1/reservations/pending`, {
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (res.ok) {
            const reservas = await res.json();
            const cartItems = document.getElementById('cart-items');
            const cartTotal = document.getElementById('cart-total');
            const timerContainer = document.getElementById('timer-container');
            
            if (reservas.length === 0) {
                cartPanel.classList.add('d-none');
                clearInterval(timerInterval);
                return;
            }

            cartPanel.classList.remove('d-none');
            timerContainer.classList.remove('d-none');
            cartItems.innerHTML = '';
            let total = 0;
            let minExpiresAt = new Date(reservas[0].expiresAt.endsWith('Z') ? reservas[0].expiresAt : reservas[0].expiresAt + 'Z');

            reservas.forEach(r => {
                total += r.price;
                const expDate = new Date(r.expiresAt.endsWith('Z') ? r.expiresAt : r.expiresAt + 'Z');
                if (expDate < minExpiresAt) minExpiresAt = expDate;

                cartItems.innerHTML += `
                    <div class="cart-item">
                        <strong>Sector:</strong> ${r.sectorName} <br>
                        <strong>Fila:</strong> ${r.row} - <strong>Asiento:</strong> ${r.seatNumber} <br>
                        <span class="text-success">$${r.price}</span>
                        <button class="btn btn-sm btn-outline-danger mt-2 w-100" onclick="eliminarReserva('${r.reservationId}')">Eliminar del carrito</button>
                    </div>
                `;
            });

            cartTotal.innerText = `Total: $${total}`;
            iniciarTemporizador(minExpiresAt);
        }
    } catch (e) {
        console.error("Error cargando carrito:", e);
    }
}

function iniciarTemporizador(expiresAt) {
    clearInterval(timerInterval);
    const timerText = document.getElementById('timer-text');
    
    timerInterval = setInterval(() => {
        const now = new Date();
        const diff = Math.floor((expiresAt - now) / 1000);
        
        if (diff <= 0) {
            clearInterval(timerInterval);
            document.getElementById('timer-container').classList.add('d-none');
            alert("El tiempo de alguna de tus reservas ha expirado.");
            cargarCarrito(); // Recargar para eliminar las expiradas de la vista
            if (currentEventId) {
                const sectorId = document.getElementById('sectorSelect')?.value;
                if (sectorId) cargarAsientos(currentEventId, sectorId);
            }
            return;
        }
        
        const timerContainer = document.getElementById('timer-container');
        if (diff <= 60) {
            timerContainer.classList.remove('alert-warning');
            timerContainer.classList.add('alert-danger');
        } else {
            timerContainer.classList.remove('alert-danger');
            timerContainer.classList.add('alert-warning');
        }

        const minutos = String(Math.floor(diff / 60)).padStart(2, '0');
        const segundos = String(diff % 60).padStart(2, '0');
        timerText.textContent = `${minutos}:${segundos}`;
    }, 1000);
}

async function procesarPago() {
    const token = localStorage.getItem('jwt_token');
    if (!token) return;

    try {
        const res = await fetch(`http://localhost:5029/api/v1/reservations/pay`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });
        const data = await res.json();

        if (res.ok) {
            alert("¡Compra exitosa! Que disfrutes tu evento.");
            cargarCarrito();
            if (currentEventId) {
                const sectorId = document.getElementById('sectorSelect')?.value;
                if (sectorId) cargarAsientos(currentEventId, sectorId);
            }
        } else {
            alert("No se pudo procesar el pago: " + (data.error || "Error desconocido."));
        }
    } catch (e) {
        alert("Error de red al intentar procesar el pago.");
    }
}

async function eliminarReserva(reservationId) {
    if (!confirm("¿Estás seguro de que deseas eliminar esta silla de tu carrito? Quedará disponible para otros usuarios.")) return;

    const token = localStorage.getItem('jwt_token');
    if (!token) return;

    try {
        const res = await fetch(`http://localhost:5029/api/v1/events/0/seats/0/reservations/${reservationId}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (res.ok) {
            cargarCarrito();
            if (currentEventId) {
                const sectorId = document.getElementById('sectorSelect').value;
                if (sectorId) cargarAsientos(currentEventId, sectorId);
            }
        } else {
            const data = await res.json();
            alert("Error al eliminar la reserva: " + (data.error || "Error desconocido"));
        }
    } catch (e) {
        console.error("Error al eliminar la reserva:", e);
        alert("Error de red al intentar eliminar la reserva.");
    }
}

// Llamamos a la función
actualizarUI();
cargarCarrito();
obtenerEventos();