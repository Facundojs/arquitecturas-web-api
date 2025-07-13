const apiURL = 'https://tubackend.com/api/libros'; /* backeend!!!!!!!!!!!!!!!!! */
let token = localStorage.getItem('token');

if (!token) window.location.href = 'login.html';

const headers = {
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
};

window.addEventListener('DOMContentLoaded', cargarLibros);

document.getElementById('logoutBtn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'login.html';
});

document.getElementById('crearForm').addEventListener('submit', async e => {
  e.preventDefault();
  const nuevo = {
    titulo: document.getElementById('crearTitulo').value,
    autor: document.getElementById('crearAutor').value,
    descripcion: document.getElementById('crearDescripcion').value
  };

  const res = await fetch(apiURL, {
    method: 'POST',
    headers,
    body: JSON.stringify(nuevo)
  });

  if (res.ok) {
    alert('Libro creado con éxito');
    document.getElementById('crearForm').reset();
    cargarLibros();
  } else {
    alert('Error al crear');
  }
});

document.getElementById('editarForm').addEventListener('submit', async e => {
  e.preventDefault();
  const id = document.getElementById('editarId').value;
  const modificado = {
    titulo: document.getElementById('editarTitulo').value,
    autor: document.getElementById('editarAutor').value,
    descripcion: document.getElementById('editarDescripcion').value
  };

  const res = await fetch(`${apiURL}/${id}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify(modificado)
  });

  if (res.ok) {
    alert('Libro actualizado');
    cargarLibros();
  } else {
    alert('Error al actualizar');
  }
});

document.getElementById('eliminarBtn').addEventListener('click', async () => {
  const id = document.getElementById('editarId').value;
  if (!confirm('¿Seguro que querés eliminar este registro?')) return;

  const res = await fetch(`${apiURL}/${id}`, {
    method: 'DELETE',
    headers
  });

  if (res.ok) {
    alert('Libro eliminado');
    cargarLibros();
    document.getElementById('editarForm').reset();
  } else {
    alert('Error al eliminar');
  }
});

async function cargarLibros() {
  const res = await fetch(apiURL, { headers });
  const data = await res.json();
  const tbody = document.querySelector('#tablaLibros tbody');
  tbody.innerHTML = '';

  data.forEach(libro => {
    const fila = document.createElement('tr');
    fila.innerHTML = `
      <td>${libro.id}</td>
      <td>${libro.titulo}</td>
      <td>${libro.autor}</td>
      <td>${libro.descripcion}</td>
    `;
    fila.addEventListener('click', () => seleccionarLibro(libro));
    tbody.appendChild(fila);
  });
}

function seleccionarLibro(libro) {
  document.getElementById('editarId').value = libro.id;
  document.getElementById('editarTitulo').value = libro.titulo;
  document.getElementById('editarAutor').value = libro.autor;
  document.getElementById('editarDescripcion').value = libro.descripcion;
}