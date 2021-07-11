import {User} from './model'

export class UserStore {
  // Lista de Usuarios.
  static async getUsers(query = '') {
    return User.find({
      fullName: {$regex: query},
      permission: {$in: ['ROLE_ADMIN', 'ROLE_USER']},
      isDeleted: false
    })
  }

  // obtener usuario por id.
  static async getUser(id) {
    return User.findById(id)
  }

  // lista de usuarios activos.
  static async getUsersActive(query = '') {
    return User.find({
      fullName: {$regex: query},
      permission: {$in: ['ROLE_ADMIN', 'ROLE_USER']},
      suspended: false,
      isDeleted: false
    })
  }

  // registrar usuario.
  static async createUser(data) {
    const _user = new User(data)
    _user.suspended = false
    _user.isDeleted = false
    return _user.save()
  }

  // actualizar usuario.
  static async updateUser(id, data) {
    return User.findByIdAndUpdate(id, data, {new: true})
  }

  // borrar usuario.
  static async deleteUser(id) {
    let _user = await this.getUser(id)
    _user.isDeleted = true
    return this.updateUser(id, _user)
  }

  // chequear si existe en la base de datos.
  static async checkUserExist(userName = '') {
    return User.findOne({userName: userName})
  }

  // registrar cuenta super usuario.
  static async createSuperUser(userName, passwordHash) {
    let _user = new User({
      fullName: 'SUPER_USER',
      userName: userName,
      password: passwordHash,
      permission: 'ROLE_SUPER',
      email: 'super@local.pe',
      suspended: false
    })
    return this.createUser(_user)
  }
}
