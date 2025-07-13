document.querySelector('#loginForm').addEventListener('submit', onSubmit);

async function onSubmit(e) {
  e.preventDefault();
  const email = document.getElementById('email').value.trim();
  const password = document.getElementById('password').value.trim();
  const errorContainer = document.getElementById('error-message');

  errorContainer.style.display = 'none';
  errorContainer.textContent = '';

  try {
    const { token, refreshToken } = await window.Api.login(email, password);
    localStorage.setItem('token', token);
    localStorage.setItem('refreshToken', refreshToken);
    window.location.href = 'index.html';
  } catch (error) {
    errorContainer.textContent = error.message;
    errorContainer.style.display = 'block';
  }
}