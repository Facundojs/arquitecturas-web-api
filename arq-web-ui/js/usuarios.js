const userApi = 'https://tubackend.com/api/usuarios' /* backeend!!!!!!!!!!!!!!!!! */;
let token = localStorage.getItem('token');

if (!token) window.location.href = 'login.html';

const headers = {
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
};

window.addEventListener('DOMContentLoaded', cargarUsuarios);

document.getElementById('logoutBtn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'login.html';
});

document.getElementById('crearUsuarioForm').addEventListener('submit', async e => {
  e.preventDefault();
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

  const res = await fetch(userApi, {
    method: 'POST',
    headers,
    body: JSON.stringify(usuario)
  });

  if (res.ok) {
    alert('Usuario creado');
    e.target.reset();
    cargarUsuarios();
  } else {
    alert('Error al crear usuario');
  }
});

document.getElementById('editarUsuarioForm').addEventListener('submit', async e => {
  e.preventDefault();
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

  const res = await fetch(`${userApi}/${id}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(usuario)
  });

  if (res.ok) {
    alert('Usuario actualizado');
    cargarUsuarios();
  } else {
    alert('Error al actualizar');
  }
});

document.getElementById('eliminarUsuarioBtn').addEventListener('click', async () => {
  const id = document.getElementById('editarId').value;
  if (!confirm('¿Eliminar este usuario?')) return;

  const res = await fetch(`${userApi}/${id}`, {
    method: 'DELETE',
    headers
  });

  if (res.ok) {
    alert('Usuario eliminado');
    cargarUsuarios();
    document.getElementById('editarUsuarioForm').reset();
  } else {
    alert('Error al eliminar');
  }
});

async function cargarUsuarios() {
  const res = await fetch(userApi, { headers });
  const data = await res.json();
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
        .map(([key]) => key).join(', ')}</td>
    `;
    fila.addEventListener('click', () => seleccionarUsuario(user));
    tbody.appendChild(fila);
  });
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