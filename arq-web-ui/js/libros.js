window.addEventListener('DOMContentLoaded', async () => {

  try {
    const usuario = await window.Api.getUsuarioActual();

    if (!usuario.privilegios?.includes('BOOKS_CREATE')) {
      document.getElementById("crearTitulo").setAttribute('disabled', true)
      document.getElementById("crearAutor").setAttribute('disabled', true)
      document.getElementById("crearDescripcion").setAttribute('disabled', true)
      document.getElementById("crearLibroBtn").setAttribute('disabled', true)
    }

    if (!usuario.privilegios?.includes('BOOKS_DELETE')) {
      document.getElementById("eliminarBtn").setAttribute('disabled', true)
    }

    if (usuario.privilegios?.includes('BOOKS_LIST')) {
      document.getElementById('tablaLibros').style.display = 'block';
    }

    if (!usuario.privilegios?.includes('BOOKS_UPDATE')) {
      document.getElementById("editarTitulo").setAttribute('disabled', true)
      document.getElementById("editarAutor").setAttribute('disabled', true)
      document.getElementById("editarDescripcion").setAttribute('disabled', true)
      document.getElementById("editarBtn").setAttribute('disabled', true)
    }

  } catch (error) {
    errorContainer.textContent = error.message;
    errorContainer.style.display = 'block';
    localStorage.removeItem('token');
    setTimeout(() => {
      window.location.href = 'login.html';
    });
  }

});


window.addEventListener('DOMContentLoaded', async () => {
  const errorContainer = document.getElementById('error-message');
  try {
    await window.Api.getUsuarioActual();
    cargarLibros();
  } catch (err) {
    errorContainer.textContent = err.message;
    errorContainer.style.display = 'block';
    localStorage.removeItem('token');
    setTimeout(() => window.location.href = 'login.html', 3000);
  }
});

document.getElementById('logoutBtn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'login.html';
});

document.getElementById('crearForm').addEventListener('submit', async e => {
  e.preventDefault();
  const errorContainer = document.getElementById('error-message');
  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  const nuevo = {
    Nombre: document.getElementById('crearTitulo').value,
    Autor: document.getElementById('crearAutor').value,
    Descripcion: document.getElementById('crearDescripcion').value,
    Categoria: "General"
  };

  try {
    await window.Api.crearLibro(nuevo);
    document.getElementById('crearForm').reset();
    cargarLibros();
  } catch (err) {
    errorContainer.textContent = err.message;
    errorContainer.style.display = 'block';
  }
});

document.getElementById('editarForm').addEventListener('submit', async e => {
  e.preventDefault();
  const errorContainer = document.getElementById('error-message');
  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  const id = document.getElementById('editarId').value;
  const modificado = {
    Nombre: document.getElementById('editarTitulo').value,
    Autor: document.getElementById('editarAutor').value,
    Descripcion: document.getElementById('editarDescripcion').value,
    Categoria: "General"
  };

  try {
    await window.Api.editarLibro(id, modificado);
    cargarLibros();
  } catch (err) {
    errorContainer.textContent = err.message;
    errorContainer.style.display = 'block';
  }
});

document.getElementById('eliminarBtn').addEventListener('click', async () => {
  const id = document.getElementById('editarId').value;
  if (!confirm('¿Seguro que querés eliminar este registro?')) return;

  const errorContainer = document.getElementById('error-message');
  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  try {
    await window.Api.eliminarLibro(id);
    document.getElementById('editarForm').reset();
    cargarLibros();
  } catch (err) {
    errorContainer.textContent = err.message;
    errorContainer.style.display = 'block';
  }
});

async function cargarLibros() {
  const errorContainer = document.getElementById('error-message');
  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  try {
    const data = await window.Api.getLibros();
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
    errorContainer.textContent = err.message;
    errorContainer.style.display = 'block';
  }
}

function seleccionarLibro(libro) {
  document.getElementById('editarId').value = libro.id;
  document.getElementById('editarTitulo').value = libro.name;
  document.getElementById('editarAutor').value = libro.author;
  document.getElementById('editarDescripcion').value = libro.description;
}