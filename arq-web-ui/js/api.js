const ApiUrl = "http://localhost:5188/api"

function getAuthHeaders() {
  const token = localStorage.getItem('token');
  return {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  };
}

window.Api = {
  //Login
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
  },

  //Registro
  async register(nombre, apellido, email, password) {
    const response = await fetch(`${ApiUrl}/iam/registro`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nombre, apellido, email, password })
    });

    if (!response.ok) {
      let msg = 'Error al registrarse.';
      try {
        const error = await response.json();
        msg = error.message || msg;
      } catch {}
      throw new Error(msg);
    }

    return await response.json().catch(() => ({}));
  },

  //Libros
  async getBooks() {
    const res = await fetch(`${ApiUrl}/books`, {
      headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Error al obtener libros');
    return await res.json();
  },

  async createBook(libro) {
    const res = await fetch(`${ApiUrl}/books`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify(libro)
    });
    if (!res.ok) throw new Error('Error al crear libro');
    return await res.json();
  },

  async updateBook(id, libro) {
    const res = await fetch(`${ApiUrl}/books/${id}`, {
      method: 'PUT',
      headers: getAuthHeaders(),
      body: JSON.stringify(libro)
    });
    if (!res.ok) throw new Error('Error al actualizar libro');
  },

  async deleteBook(id) {
    const res = await fetch(`${ApiUrl}/books/${id}`, {
      method: 'DELETE',
      headers: getAuthHeaders()
    });
    if (!res.ok) throw new Error('Error al eliminar libro');
  }
}