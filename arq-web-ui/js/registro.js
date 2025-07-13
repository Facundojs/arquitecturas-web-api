document.getElementById('registroForm').addEventListener('submit', async (e) => {
  e.preventDefault();

  const nombre = document.getElementById('nombre').value.trim();
  const apellido = document.getElementById('apellido').value.trim();
  const email = document.getElementById('email').value.trim();
  const password = document.getElementById('password').value;
  const confirmar = document.getElementById('confirmar').value;

  if (!nombre || !apellido || !email || !password || !confirmar) {
    alert('Por favor, completá todos los campos.');
    return;
  }

  if (password !== confirmar) {
    alert('Las contraseñas no coinciden.');
    return;
  }

  try {
    await window.Api.register(nombre, apellido, email, password);
    alert('Usuario creado con éxito. Ahora podés iniciar sesión.');
    window.location.href = 'login.html';
  } catch (error) {
    alert(error.message);
  }
});