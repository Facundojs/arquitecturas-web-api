window.addEventListener('DOMContentLoaded', async () => {
  const errorContainer = document.getElementById('error-message');
  try {
    await window.Api.getUsuarioActual();
    cargarUsuarios();
  } catch (err) {
    mostrarError(err.message);
    localStorage.removeItem('token');
    setTimeout(() => window.location.href = 'login.html', 3000);
  }
});

document.getElementById('logoutBtn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'login.html';
});

document.getElementById('crearUsuarioForm').addEventListener('submit', async e => {
  e.preventDefault();
  limpiarError();

  const nombre = document.getElementById('crearNombre').value;
  const email = document.getElementById('crearEmail').value;
  const password = document.getElementById('crearPassword').value;

  const privilegios = {
    crear: document.querySelector('input[name="crear"]').checked,
    leer: document.querySelector('input[name="leer"]').checked,
    editar: document.querySelector('input[name="editar"]').checked,
    eliminar: document.querySelector('input[name="eliminar"]').checked
  };

  const usuario = { nombre, email, password, privilegios };

  try {
    await window.Api.crearUsuario(usuario);
    e.target.reset();
    cargarUsuarios();
  } catch (err) {
    mostrarError(err.message);
  }
});

document.getElementById('editarUsuarioForm').addEventListener('submit', async e => {
  e.preventDefault();
  limpiarError();

  const id = document.getElementById('editarId').value;
  const nombre = document.getElementById('editarNombre').value;
  const email = document.getElementById('editarEmail').value;

  const privilegios = {
    crear: document.querySelector('input[name="editarCrear"]').checked,
    leer: document.querySelector('input[name="editarLeer"]').checked,
    editar: document.querySelector('input[name="editarEditar"]').checked,
    eliminar: document.querySelector('input[name="editarEliminar"]').checked
  };

  const usuario = { nombre, email, privilegios };

  try {
    await window.Api.editarUsuario(id, usuario);
    cargarUsuarios();
  } catch (err) {
    mostrarError(err.message);
  }
});

document.getElementById('eliminarUsuarioBtn').addEventListener('click', async () => {
  const id = document.getElementById('editarId').value;
  if (!confirm('¿Eliminar este usuario?')) return;

  limpiarError();

  try {
    await window.Api.eliminarUsuario(id);
    document.getElementById('editarUsuarioForm').reset();
    cargarUsuarios();
  } catch (err) {
    mostrarError(err.message);
  }
});

async function cargarUsuarios() {
  limpiarError();

  try {
    const data = await window.Api.getUsuarios();
    const tbody = document.querySelector('#tablaUsuarios tbody');
    tbody.innerHTML = '';

    data.forEach(user => {
      const fila = document.createElement('tr');
      fila.innerHTML = `
        <td>${user.id}</td>
        <td>${user.nombre}</td>
        <td>${user.email}</td>
        <td>${Object.entries(user.privilegios)
          .filter(([_, val]) => val)
          .map(([key]) => key)
          .join(', ')}</td>
      `;
      fila.addEventListener('click', () => seleccionarUsuario(user));
      tbody.appendChild(fila);
    });
  } catch (err) {
    mostrarError(err.message);
  }
}

function seleccionarUsuario(user) {
  document.getElementById('editarId').value = user.id;
  document.getElementById('editarNombre').value = user.nombre;
  document.getElementById('editarEmail').value = user.email;

  document.querySelector('input[name="editarCrear"]').checked = user.privilegios.crear;
  document.querySelector('input[name="editarLeer"]').checked = user.privilegios.leer;
  document.querySelector('input[name="editarEditar"]').checked = user.privilegios.editar;
  document.querySelector('input[name="editarEliminar"]').checked = user.privilegios.eliminar;
}

function mostrarError(msg) {
  const el = document.getElementById('error-message');
  el.textContent = msg;
  el.style.display = 'block';
}

function limpiarError() {
  const el = document.getElementById('error-message');
  el.textContent = '';
  el.style.display = 'none';
}