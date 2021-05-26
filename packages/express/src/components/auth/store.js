import {User} from './model'

// Listar usuarios.
export async function getUsers(suspended = false) {
  return User.find({suspended: suspended})
}

// devolver un usuario por id.
export async function getUser(id) {
  return User.findById(id)
}

// devolver  usuario por userName.
export async function getUserByUserName(userName) {
  return User.findOne({userName: userName})
}

// registrar usuario.
export async function createUser(data) {
  const _user = new User(data)
  return _user.save()
}

// actualizar usuario.
export async function updateUser(id, data) {
  return User.findByIdAndUpdate(id, data, {new: true})
}

// borrar usuario.
export async function deleteUser(id) {
  let _user = await getUser(id)
  _user.isDeleted = true
  return updateUser(id, _user)
}
