const API_URL = "http://localhost:5029/api/Events";
const AUTH_URL = "http://localhost:5029/api/Auth";

let hasAlertedExpiration = false;
let reservasGlobalesParaMapa = [];
let misEntradasGlobales = [];

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
            contenedor.innerHTML = '<p class="alert alert-info text-center">No hay eventos disponibles en este momento.</p>';
            return;
        }

        // Dibujamos cada evento
        eventos.forEach((evento) => {
    const isSoldOut = evento.isSoldOut;
    const soldOutClass = isSoldOut ? 'event-sold-out' : '';
    const btnText = isSoldOut ? 'Sin Stock' : 'Reservar Entradas';
    const disabledAttr = isSoldOut ? 'disabled' : '';

    const card = `
        <div class="col-md-4 mb-4 ${soldOutClass}">
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
                    <button class="btn w-100 " onclick="mostrarDetalles(${evento.id}, '${evento.name.replace(/'/g, "\\'")}')" ${disabledAttr}>
                        ${btnText}
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
                    Tuvimos un problemita conectando con el servidor. Por favor, intenta actualizar la página.
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
            
            // Auto-selección de Sector con disponibilidad
            let foundAvailable = false;
            for (const sector of sectores) {
                const resSeats = await fetch(`${API_URL}/${id}/sectors/${sector.id}/seats`);
                if (resSeats.ok) {
                    const asientos = await resSeats.json();
                    if (asientos.some(a => a.status === 0)) {
                        sectorSelect.value = sector.id;
                        await cargarAsientos(id, sector.id);
                        foundAvailable = true;
                        break;
                    }
                }
            }
            if (!foundAvailable) {
                document.getElementById('contenedor-asientos').innerHTML = '<p class="text-danger fw-bold mt-3">Agotado: No hay asientos disponibles en ningún sector.</p>';
            }
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
            contenedorAsientos.innerHTML = '<p class="text-warning text-center">No hay butacas configuradas para este sector.</p>';
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
            contenedorAsientos.innerHTML = `<p class="text-danger">Uy, tuvimos un problemita al cargar los asientos. Por favor, intenta de nuevo.</p>`;
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
    window.location.href = "index.html";
}

let pendingReservation = null;

async function reservarAsiento(eventId, seatId, seatNumber) {
    const sectorSelect = document.getElementById('sectorSelect');
    const sectorName = sectorSelect.options[sectorSelect.selectedIndex].text;
    const price = sectorSelect.options[sectorSelect.selectedIndex].getAttribute('data-price');

    pendingReservation = { eventId, seatId, seatNumber, sectorName, price };

    document.getElementById('confirmReserveBody').innerHTML = `¿Confirmas la reserva de la butaca <strong>${seatNumber}</strong> en <strong>${sectorName}</strong> por <strong>$${price}</strong>?<br><br><small class="text-danger fw-bold">Dispondrás de un tiempo límite unificado para completar el pago de tu carrito.</small>`;
    const confirmModal = new bootstrap.Modal(document.getElementById('confirmReserveModal'));
    confirmModal.show();
}

async function procesarReservaConfirmada() {
    if (!pendingReservation) return;
    const { eventId, seatId } = pendingReservation;
    
    const confirmModal = bootstrap.Modal.getInstance(document.getElementById('confirmReserveModal'));
    if (confirmModal) confirmModal.hide();

    const token = localStorage.getItem('jwt_token');
    if (!token) {
        alert("¡Hola! Para asegurar tus butacas, por favor inicia sesión o crea una cuenta rápido.");
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
            alert("Parece que tu sesión caducó. Por favor, vuelve a iniciar sesión para continuar.");
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
            alert(`Ups, no pudimos reservar esa butaca: ${errorMsg}\n\nVamos a actualizar el mapa para que veas qué lugares quedan libres.`);
            const sectorId = document.getElementById('sectorSelect').value;
            cargarAsientos(eventId, sectorId);
        }
    } catch (e) {
        console.error("Detalle técnico del error:", e);
        alert("Lo sentimos, ocurrió un problema al procesar tu solicitud. Por favor, inténtalo de nuevo en unos instantes.");
    }
}

let timerInterval = null;

async function cargarCarrito() {
    const token = localStorage.getItem('jwt_token');
    const cartPanel = document.getElementById('cart-panel');
    if (!token) {
        if (cartPanel) cartPanel.classList.add('d-none');
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
            
            reservasGlobalesParaMapa = reservas.map(r => ({
                ...r,
                eventId: Number(r.eventId ?? r.EventId ?? 0),
                sectorId: Number(r.sectorId ?? r.SectorId ?? 0),
                sectorName: r.sectorName || r.SectorName || 'Desconocido',
                seatId: String(r.seatId || r.SeatId || '')
            }));
            if (reservas.length === 0) {
                if (cartPanel) cartPanel.classList.add('d-none');
                clearInterval(timerInterval);

                if (window.location.pathname.includes("reserves.html")) {
                    window.location.href = "index.html";
                }
                return;
            }

            if (cartPanel) cartPanel.classList.remove('d-none');
            if (timerContainer) timerContainer.classList.remove('d-none');
            
            if (cartItems) cartItems.innerHTML = '';
            let total = 0;
            let minExpiresAt = new Date(reservas[0].expiresAt.endsWith('Z') ? reservas[0].expiresAt : reservas[0].expiresAt + 'Z');

            reservas.forEach(r => {
                total += r.price;
                const expDate = new Date(r.expiresAt.endsWith('Z') ? r.expiresAt : r.expiresAt + 'Z');
                if (expDate < minExpiresAt) minExpiresAt = expDate;

                if (cartItems) {
                    cartItems.innerHTML += `
                        <div class="cart-item d-flex flex-column justify-content-between">
                            <div>
                                <span class="badge bg-secondary mb-2">${r.sectorName}</span>
                                <div class="fs-5 text-white mb-1"><i class="bi bi-ticket-detailed me-2"></i>Fila ${r.row || '-'} / Asiento ${r.seatNumber}</div>
                            </div>
                            <div class="mt-3 text-center border-top border-secondary pt-3">
                                <div class="fs-4 fw-bold text-success mb-2">$${r.price}</div>
                                <button class="btn btn-sm btn-reserves btn-outline-danger w-100 rounded-pill fw-bold" onclick="eliminarReserva('${r.reservationId}')">
                                    <i class="bi bi-trash me-1"></i>Eliminar
                                </button>
                            </div>
                        </div>
                    `;
                }
            });

            if (cartTotal) cartTotal.innerText = `Total: $${total}`;
            iniciarTemporizador(minExpiresAt);
        }
    } catch (e) {
        console.error("Error al cargar el carrito:", e);
    }
}

function iniciarTemporizador(expiresAt) {
    clearInterval(timerInterval);
    const timerText = document.getElementById('timer-text');
    
    const actualizarTiempo = () => {
        const now = new Date();
        const diff = Math.floor((expiresAt - now) / 1000);
        
        if (diff > 0) {
            hasAlertedExpiration = false;
        }

        if (diff <= 0) {
            if (!hasAlertedExpiration) {
                hasAlertedExpiration = true;
                clearInterval(timerInterval);
                const timerContainer = document.getElementById('timer-container');
                if (timerContainer) timerContainer.classList.add('d-none');
                const cartPanel = document.getElementById('cart-panel');
                if (cartPanel && !timerContainer) cartPanel.classList.add('d-none');
                
                const cartItems = document.getElementById('cart-items');
                if (cartItems) cartItems.innerHTML = '';

                alert("¡Uy! Se acabó el tiempo de tu reserva. Las butacas han vuelto a estar disponibles, pero puedes volver a intentarlo.");
               
                obtenerEventos();
                if (currentEventId) {
                    const sectorId = document.getElementById('sectorSelect')?.value;
                    if (sectorId) cargarAsientos(currentEventId, sectorId);
                }
            }
            return;
        }
        
        const timerContainer = document.getElementById('timer-container');
        if (timerContainer) {
            if (diff <= 60) {
                timerContainer.classList.remove('alert-warning');
                timerContainer.classList.add('alert-danger');
            } else {
                timerContainer.classList.remove('alert-danger');
                timerContainer.classList.add('alert-warning');
            }
        } else {
            const cartPanel = document.getElementById('cart-panel');
            if (cartPanel) {
                if (diff <= 60) {
                    cartPanel.style.backgroundColor = '#dc3545'; // alert-danger
                } else {
                    cartPanel.style.backgroundColor = '#E84A5F'; // base color
                }
            }
        }

        const minutos = String(Math.floor(diff / 60)).padStart(2, '0');
        const segundos = String(diff % 60).padStart(2, '0');
        if (timerText) timerText.textContent = `${minutos}:${segundos}`;
    };

    actualizarTiempo(); // Ejecutamos la lógica inmediatamente para evitar el parpadeo
    timerInterval = setInterval(actualizarTiempo, 1000); // Programamos el ciclo
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
            alert("¡Qué bien! Tu pago se ha procesado correctamente. ¡Prepárate para disfrutar del evento!");
            cargarCarrito();
            if (currentEventId) {
                const sectorId = document.getElementById('sectorSelect')?.value;
                if (sectorId) cargarAsientos(currentEventId, sectorId);
            }
            window.location.href = "index.html"
        } else {
            alert("Uy, tuvimos un inconveniente con tu pago: " + (data.error || "Error desconocido."));
        }
    } catch (e) {
        alert("Parece que hay un problema de conexión. Por favor, revisa tu internet e inténtalo de nuevo.");
    }
}

async function eliminarReserva(reservationId) {
    if (!confirm("¿Deseas quitar esta butaca de tu reserva? Estará disponible para otras personas.")) return;

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
            alert("Lo sentimos, no pudimos quitar la butaca: " + (data.error || "Error desconocido"));
        }
    } catch (e) {
        console.error("Error al eliminar la reserva:", e);
        alert("Parece que hay un problema de conexión al intentar quitar la butaca.");
    }
}

// === Lógica para el Mapa de Solo Lectura ===
async function renderizarMapaSoloLectura(eventId, sectorId, contenedorId) {
    console.log(`Renderizando mapa solo lectura - EventId: ${eventId}, SectorId: ${sectorId}`);
    if (!eventId || !sectorId || eventId == 0 || sectorId == 0 || eventId === "undefined" || sectorId === "undefined") {
        document.getElementById(contenedorId).innerHTML = '<p class="text-warning text-center">Datos de evento o sector inválidos.</p>';
        return;
    }

    const contenedorAsientos = document.getElementById(contenedorId);
    contenedorAsientos.innerHTML = '<div class="spinner-border text-info" role="status"><span class="visually-hidden">Cargando mapa...</span></div>';
    
    try {
        const token = localStorage.getItem('jwt_token');
        const headers = token ? { 'Authorization': `Bearer ${token}` } : {};

        const [resSeats, resPending, resPaid] = await Promise.all([
            fetch(`${API_URL}/${eventId}/sectors/${sectorId}/seats`),
            fetch(`http://localhost:5029/api/v1/reservations/pending`, { headers }),
            fetch(`http://localhost:5029/api/v1/reservations/paid`, { headers })
        ]);

        if (!resSeats.ok) throw new Error("No se pudo cargar el mapa.");

        const asientos = await resSeats.json();
        const pending = resPending.ok ? await resPending.json() : [];
        const paid = resPaid.ok ? await resPaid.json() : [];

        const pendingIds = pending.map(p => String(p.seatId || p.SeatId || ''));
        const paidIds = paid.map(p => String(p.seatId || p.SeatId || ''));

        if (asientos.length === 0) {
            contenedorAsientos.innerHTML = '<p class="text-warning text-center">Aún no hay butacas en este sector.</p>';
            return;
        }

        let html = '<div class="seat-map-wrapper">';
        html += '<div class="stage-indicator">ESCENARIO</div>';
        html += '<div class="seat-map">';
        
        asientos.forEach(a => {
            let colorClass = 'seat-grey';
            const sId = String(a.id || a.Id || '');
            if (paidIds.includes(sId)) {
                colorClass = 'seat-pink';
            } else if (pendingIds.includes(sId)) {
                colorClass = 'seat-green';
            }
            const num = a.number ?? a.Number ?? '-';
            const row = a.row ?? a.Row ?? '-';

            html += `
            <button 
                class="btn ${colorClass} seat-btn seat-view-only" 
                title="Fila: ${row}, Asiento: ${num}"
            >
                ${num}
            </button>`;
        });

        html += '</div></div>';
        contenedorAsientos.innerHTML = html;

    } catch (error) {
        console.error(error);
        contenedorAsientos.innerHTML = `<p class="text-danger">Uy, tuvimos un problemita cargando el mapa. Por favor, intenta de nuevo.</p>`;
    }
}

async function abrirModalMisEntradas() {
    const modal = new bootstrap.Modal(document.getElementById('myTicketsModal'));
    modal.show();
    
    const eventSelect = document.getElementById('myTicketsEventSelect');
    const sectorSelect = document.getElementById('myTicketsSectorSelect');
    const contenedorAsientos = document.getElementById('contenedor-mis-asientos');

    eventSelect.innerHTML = '<option value="">Cargando información...</option>';
    sectorSelect.innerHTML = '<option value="">Seleccione un evento primero</option>';
    contenedorAsientos.innerHTML = '<p class="text-muted">Seleccione un evento y sector para ver el mapa de asientos.</p>';

    try {
        const token = localStorage.getItem('jwt_token');
        const headers = token ? { 'Authorization': `Bearer ${token}` } : {};

        const [resEvents, resPending, resPaid] = await Promise.all([
            fetch(API_URL),
            fetch(`http://localhost:5029/api/v1/reservations/pending`, { headers }),
            fetch(`http://localhost:5029/api/v1/reservations/paid`, { headers })
        ]);

        const eventos = resEvents.ok ? await resEvents.json() : [];
        const pending = resPending.ok ? await resPending.json() : [];
        const paid = resPaid.ok ? await resPaid.json() : [];

        misEntradasGlobales = [...pending, ...paid].map(t => ({
            ...t,
            eventId: Number(t.eventId ?? t.EventId ?? 0),
            sectorId: Number(t.sectorId ?? t.SectorId ?? 0),
            sectorName: t.sectorName || t.SectorName || 'Desconocido',
            seatId: String(t.seatId || t.SeatId || ''),
            expiresAt: t.expiresAt || t.ExpiresAt || null
        }));

        if (misEntradasGlobales.length === 0) {
            eventSelect.innerHTML = '<option value="">No posees entradas</option>';
            sectorSelect.innerHTML = '<option value="">No hay sectores</option>';
            return;
        }
        
        if (eventos.length === 0) {
            eventSelect.innerHTML = '<option value="">No hay eventos disponibles</option>';
            sectorSelect.innerHTML = '<option value="">No hay sectores</option>';
            return;
        }

        const userEventIds = [...new Set(misEntradasGlobales.map(t => t.eventId))];
        const userEvents = eventos.filter(e => userEventIds.includes(e.id));

        eventSelect.innerHTML = '';
        userEvents.forEach(e => {
            eventSelect.innerHTML += `<option value="${e.id}">${e.name}</option>`;
        });

        if (userEvents.length > 0) {
            const mostRecent = [...misEntradasGlobales].sort((a, b) => new Date(b.expiresAt || 0) - new Date(a.expiresAt || 0))[0];
            eventSelect.value = mostRecent && mostRecent.eventId ? mostRecent.eventId : userEvents[0].id;
            actualizarSectoresMisEntradas();
        }
    } catch (e) {
        eventSelect.innerHTML = '<option value="">Error al cargar la información</option>';
        console.error(e);
    }
}

function actualizarSectoresMisEntradas() {
    const eventId = document.getElementById('myTicketsEventSelect').value;
    const sectorSelect = document.getElementById('myTicketsSectorSelect');
    
    if (!eventId) {
        sectorSelect.innerHTML = '<option value="">Seleccione un evento primero</option>';
        document.getElementById('contenedor-mis-asientos').innerHTML = '<p class="text-muted">Seleccione un evento y sector para ver el mapa de asientos.</p>';
        return;
    }

    const entradasEvento = misEntradasGlobales.filter(t => String(t.eventId) === String(eventId));
    
    const sectoresUnicos = [];
    entradasEvento.forEach(t => {
        if (!sectoresUnicos.some(s => String(s.sectorId) === String(t.sectorId))) {
            sectoresUnicos.push({ sectorId: t.sectorId, sectorName: t.sectorName });
        }
    });

    sectorSelect.innerHTML = '';
    sectoresUnicos.forEach(s => {
        sectorSelect.innerHTML += `<option value="${s.sectorId}">${s.sectorName}</option>`;
    });

    if (sectoresUnicos.length > 0) {
        sectorSelect.value = sectoresUnicos[0].sectorId;
        cargarMapaMisEntradas();
    } else {
        sectorSelect.innerHTML = '<option value="">No hay sectores</option>';
        document.getElementById('contenedor-mis-asientos').innerHTML = '<p class="text-muted">No hay asientos disponibles en este evento.</p>';
    }
}

function cargarMapaMisEntradas() {
    const eventId = document.getElementById('myTicketsEventSelect').value;
    const sectorId = document.getElementById('myTicketsSectorSelect').value;
    if (eventId && sectorId) {
        renderizarMapaSoloLectura(eventId, sectorId, 'contenedor-mis-asientos');
    }
}

async function abrirModalMapaReserva() {
    const modal = new bootstrap.Modal(document.getElementById('readonlySeatMapModal'));
    modal.show();

    const sectorSelect = document.getElementById('readonlySectorSelect');
    sectorSelect.innerHTML = '<option value="">Elige un sector</option>';
    document.getElementById('contenedor-asientos-readonly').innerHTML = '<p class="text-muted">Seleccione un sector de sus reservas para ver el mapa.</p>';

    if (reservasGlobalesParaMapa.length > 0) {
        const sectoresUnicos = [];
        reservasGlobalesParaMapa.forEach(r => {
            if (r.eventId !== undefined && r.sectorId !== undefined) {
                if (!sectoresUnicos.some(s => String(s.sectorId) === String(r.sectorId) && String(s.eventId) === String(r.eventId))) {
                    sectoresUnicos.push({ sectorId: r.sectorId, eventId: r.eventId, sectorName: r.sectorName });
                }
            }
        });
        
        sectoresUnicos.forEach(s => {
            sectorSelect.innerHTML += `<option value="${s.eventId}-${s.sectorId}">${s.sectorName}</option>`;
        });
        
        if (sectoresUnicos.length > 0) {
            sectorSelect.value = `${sectoresUnicos[0].eventId}-${sectoresUnicos[0].sectorId}`;
            cargarMapaReserva();
        } else {
            sectorSelect.innerHTML = '<option value="">No hay sectores válidos</option>';
        }
    } else {
        sectorSelect.innerHTML = '<option value="">No tienes reservas en tu carrito</option>';
    }
}

function cargarMapaReserva() {
    const val = document.getElementById('readonlySectorSelect').value;
    if (val) {
        const [eventId, sectorId] = val.split('-');
        renderizarMapaSoloLectura(eventId, sectorId, 'contenedor-asientos-readonly');
    }
}

// Llamamos a la función
actualizarUI();
cargarCarrito();
obtenerEventos();