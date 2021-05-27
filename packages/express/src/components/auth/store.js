import {User} from './model'

// Lista de Usuarios.
export async function getUsers(query = '') {
  return User.find({
    fullName: {$regex: query},
    permission: {$in: ['ROLE_ADMIN', 'ROLE_USER']},
    isDeleted: false
  })
}

// obtener usuario por id.
export async function getUser(id) {
  return User.findById(id)
}

// lista de usuarios activos.
export async function getUsersActive(query = '') {
  return User.find({
    fullName: {$regex: query},
    permission: {$in: ['ROLE_ADMIN', 'ROLE_USER']},
    suspended: false,
    isDeleted: false
  })
}

// registrar usuario.
export async function createUser(data) {
  const _user = new User(data)
  _user.suspended = false
  _user.isDeleted = false
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

// chequear si existe en la base de datos.
export async function checkUserExist(userName = '') {
  return User.findOne({userName: userName})
}

// registrar cuenta super usuario.
export async function createSuperUser(userName, passwordHash) {
  let _user = new User({
    fullName: 'SUPER_USER',
    userName: userName,
    password: passwordHash,
    permission: 'ROLE_SUPER',
    suspended: false
  })
  return createUser(_user)
}
