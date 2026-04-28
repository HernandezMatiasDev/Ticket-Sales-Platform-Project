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

// Llamamos a la función
actualizarUI();
obtenerEventos();