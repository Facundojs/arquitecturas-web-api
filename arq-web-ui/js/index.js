window.addEventListener('DOMContentLoaded', () => {
  const token = localStorage.getItem('token');

  if (!token) {
    window.location.href = 'login.html';
    return;
  }

  const payload = JSON.parse(atob(token.split('.')[1]));

  if (payload.rol === 'admin') {
    document.getElementById('usuariosCard').style.display = 'block';
  }

  document.querySelector('.header h1').textContent = `Hola, ${payload.nombre || 'usuario'}`;
});

document.getElementById('logoutBtn').addEventListener('click', () => {
  localStorage.removeItem('token');
  window.location.href = 'login.html';
});