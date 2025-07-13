if (!localStorage.getItem('token')) {
  window.location.href = 'login.html';
}

window.addEventListener('DOMContentLoaded', cargarLibros);

document.getElementById('logoutBtn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'login.html';
});

document.getElementById('crearForm').addEventListener('submit', async e => {
  e.preventDefault();
  const nuevo = {
    name: document.getElementById('crearTitulo').value,
    author: document.getElementById('crearAutor').value,
    description: document.getElementById('crearDescripcion').value,
    category: "General"
  };

  try {
    await window.Api.createBook(nuevo);
    alert('Libro creado con éxito');
    document.getElementById('crearForm').reset();
    cargarLibros();
  } catch (err) {
    alert(err.message);
  }
});

document.getElementById('editarForm').addEventListener('submit', async e => {
  e.preventDefault();
  const id = document.getElementById('editarId').value;
  const modificado = {
    name: document.getElementById('editarTitulo').value,
    author: document.getElementById('editarAutor').value,
    description: document.getElementById('editarDescripcion').value,
    category: "General"
  };

  try {
    await window.Api.updateBook(id, modificado);
    alert('Libro actualizado');
    cargarLibros();
  } catch (err) {
    alert(err.message);
  }
});

document.getElementById('eliminarBtn').addEventListener('click', async () => {
  const id = document.getElementById('editarId').value;
  if (!confirm('¿Seguro que querés eliminar este registro?')) return;

  try {
    await window.Api.deleteBook(id);
    alert('Libro eliminado');
    document.getElementById('editarForm').reset();
    cargarLibros();
  } catch (err) {
    alert(err.message);
  }
});

async function cargarLibros() {
  try {
    const data = await window.Api.getBooks();
    const tbody = document.querySelector('#tablaLibros tbody');
    tbody.innerHTML = '';

    data.forEach(libro => {
      const fila = document.createElement('tr');
      fila.innerHTML = `
        <td>${libro.id}</td>
        <td>${libro.name}</td>
        <td>${libro.author}</td>
        <td>${libro.description}</td>
      `;
      fila.addEventListener('click', () => seleccionarLibro(libro));
      tbody.appendChild(fila);
    });
  } catch (err) {
    alert(err.message);
  }
}

function seleccionarLibro(libro) {
  document.getElementById('editarId').value = libro.id;
  document.getElementById('editarTitulo').value = libro.name;
  document.getElementById('editarAutor').value = libro.author;
  document.getElementById('editarDescripcion').value = libro.description;
}