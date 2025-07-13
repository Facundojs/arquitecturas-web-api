window.addEventListener('DOMContentLoaded', async () => {
  const errorContainer = document.getElementById('error-message');
  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  try {
    const usuario = await window.Api.getUsuarioActual();

    if (usuario.privilegios?.includes('USERS_LIST')) {
      document.getElementById('usuariosCard').style.display = 'block';
    }

    document.querySelector('.bienvenida-texto').textContent = `Hola, ${usuario.nombre}`;
  } catch (error) {
    errorContainer.textContent = error.message;
    errorContainer.style.display = 'block';
    localStorage.removeItem('token');
    setTimeout(() => {
      window.location.href = 'login.html';
    }, 3000);
  }

  const logoutBtn = document.getElementById('logoutBtn');
  logoutBtn.addEventListener('click', () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    window.location.href = 'login.html';
  });
});