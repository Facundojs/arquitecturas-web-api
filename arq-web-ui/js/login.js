document.querySelector('#loginForm').addEventListener('submit', onSubmit);

async function onSubmit(e) {
  e.preventDefault();
  const email = document.getElementById('email').value.trim();
  const password = document.getElementById('password').value.trim();

  try {
    const { token, refreshToken } = await window.Api.login(email, password);
    localStorage.setItem('token', token);
    localStorage.setItem('refreshToken', refreshToken);
    window.location.href = 'index.html';
  } catch {
    alert(error.message);
  }
}