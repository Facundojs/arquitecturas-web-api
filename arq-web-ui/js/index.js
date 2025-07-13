window.addEventListener('DOMContentLoaded', async () => {
  const errorContainer = document.getElementById('error-message');
  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  try {
    const usuario = await window.Api.getUsuarioActual();

    if (usuario.rol === 'admin') {
      document.getElementById('usuariosCard').style.display = 'block';
    }

    document.querySelector('.header h1, .bienvenida-texto').textContent = `Hola, ${usuario.nombre}`;
  } catch (error) {
    errorContainer.textContent = error.message;
    errorContainer.style.display = 'block';
    localStorage.removeItem('token');
    setTimeout(() => {
      window.location.href = 'login.html';
    }, 3000);
  }
});