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
    const response = await fetch('https://tubackend.com/api/register', /* backeend!!!!!!!!!!!!!!!!! */{
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nombre, apellido, email, password })
    });

    if (response.ok) {
      alert('Usuario creado con éxito. Ahora podés iniciar sesión.');
      window.location.href = 'login.html';
    } else {
      const { message } = await response.json();
      alert(message || 'Error al registrarse.');
    }
  } catch (error) {
    console.error(error);
    alert('Error al conectar con el servidor.');
  }
});