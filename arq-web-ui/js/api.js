const ApiUrl = "http://localhost:5188/api"

window.Api = {
  async login(email, password) {
    const response = await fetch(`${ApiUrl}/iam/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ email, password })
    });

    if (!response.ok) {
      throw new Error('Credenciales incorrectas');
    }

    const data = await response.json();
    return data;
  }
}