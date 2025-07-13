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

  const usuario = {
    nombre: document.getElementById('crearNombre').value,
    email: document.getElementById('crearEmail').value,
    password: document.getElementById('crearPassword').value,
    privilegios: []
  };

  if (document.querySelector('input[name="crear"]').checked) usuario.privilegios.push("BOOKS_CREATE");
  if (document.querySelector('input[name="leer"]').checked) usuario.privilegios.push("BOOKS_LIST");
  if (document.querySelector('input[name="editar"]').checked) usuario.privilegios.push("BOOKS_UPDATE");
  if (document.querySelector('input[name="eliminar"]').checked) usuario.privilegios.push("BOOKS_DELETE");

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

  const privilegios = [];
  if (document.querySelector('input[name="editarCrear"]').checked) privilegios.push("BOOKS_CREATE");
  if (document.querySelector('input[name="editarLeer"]').checked) privilegios.push("BOOKS_LIST");
  if (document.querySelector('input[name="editarEditar"]').checked) privilegios.push("BOOKS_UPDATE");
  if (document.querySelector('input[name="editarEliminar"]').checked) privilegios.push("BOOKS_DELETE");

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
        <td>${user.privilegios?.join(', ') || 'Sin privilegios'}</td>
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

  const privs = user.privilegios || [];

  document.querySelector('input[name="editarCrear"]').checked = privs.includes("BOOKS_CREATE");
  document.querySelector('input[name="editarLeer"]').checked = privs.includes("BOOKS_LIST");
  document.querySelector('input[name="editarEditar"]').checked = privs.includes("BOOKS_UPDATE");
  document.querySelector('input[name="editarEliminar"]').checked = privs.includes("BOOKS_DELETE");
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