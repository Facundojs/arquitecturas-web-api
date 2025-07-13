const ApiUrl = "http://localhost:5188/api";

class Api {
  //login
  static async login(email, password) {
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

  //index
  static async getUsuarioActual() {
    const token = localStorage.getItem('token');
    if (!token) throw new Error('Token no encontrado');

    const response = await fetch(`${ApiUrl}/iam/perfil`, {
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
  }

  //libros
  static async getLibros() {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/books`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al cargar libros');
    return await res.json();
  }

  static async crearLibro(libro) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/books`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(libro)
    });
    if (!res.ok) throw new Error('Error al crear libro');
  }

  static async editarLibro(id, libro) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/books/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(libro)
    });
    if (!res.ok) throw new Error('Error al actualizar libro');
  }

  static async eliminarLibro(id) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/books/${id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al eliminar libro');
  }

  //usuarios
  static async getUsuarios() {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/users`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al cargar usuarios');
    return await res.json();
  }

  static async crearUsuario(usuario) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/users`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(usuario)
    });

    if (!res.ok) {
      const data = await res.json().catch(() => ({}));
      throw new Error(data.message || 'Error al crear usuario');
    }
  }

  static async editarUsuario(id, usuario) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/users/${id}`, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify(usuario)
    });
    if (!res.ok) throw new Error('Error al actualizar usuario');
  }

  static async eliminarUsuario(id) {
    const token = localStorage.getItem('token');
    const res = await fetch(`${ApiUrl}/users/${id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Error al eliminar usuario');
  }
}

window.Api = Api