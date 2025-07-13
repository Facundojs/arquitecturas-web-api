const ApiUrl = "http://localhost:5188/api";

window.Api = {
  //login
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

  //index
  async getUsuarioActual() {
    const token = localStorage.getItem('token');
    if (!token) throw new Error('Token no encontrado');

    const response = await fetch(`${ApiUrl}/iam/me`, {
      method: 'GET',
      headers: {
        'Authorization': `Bearer ${token}`
      }
    });

    if (!response.ok) {
      throw new Error('Sesión expirada o token inválido');
    }

    const data = await response.json();
    return data;
  },

  //libros
  async getLibros() {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/libros`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al cargar libros');
    return await res.json();
  },

  async crearLibro(libro) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/libros`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(libro)
    });
    if (!res.ok) throw new Error('Error al crear libro');
  },

  async editarLibro(id, libro) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/libros/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(libro)
    });
    if (!res.ok) throw new Error('Error al actualizar libro');
  },

  async eliminarLibro(id) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/libros/${id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al eliminar libro');
  },

  //usuarios
  async getUsuarios() {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/usuarios`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al cargar usuarios');
    return await res.json();
  },

  async crearUsuario(usuario) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/usuarios`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(usuario)
    });
    if (!res.ok) throw new Error('Error al crear usuario');
  },

  async editarUsuario(id, usuario) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/usuarios/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(usuario)
    });
    if (!res.ok) throw new Error('Error al actualizar usuario');
  },

  async eliminarUsuario(id) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/usuarios/${id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al eliminar usuario');
  },

  //registros
  async registrarUsuario({ nombre, apellido, email, password }) {
    const response = await fetch(`${ApiUrl}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nombre, apellido, email, password })
    });

    if (!response.ok) {
      const data = await response.json().catch(() => ({}));
      throw new Error(data.message || 'Error al registrarse');
    }

    return await response.json();
  }
};