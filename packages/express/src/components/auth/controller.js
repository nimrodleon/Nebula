import bcrypt from 'bcryptjs'
import jwt from 'jsonwebtoken'
import {UserStore} from './store'

const saltRounds = 10

// generate Password.
export function generatePassword(myTextPassword) {
  return new Promise((resolve, reject) => {
    bcrypt.genSalt(saltRounds, (err, salt) => {
      bcrypt.hash(myTextPassword, salt, (err, hash) => {
        if (err) {
          reject(err)
        }
        resolve(hash)
      })
    })
  })
}

// Lista de usuarios.
export function getUsers(query) {
  return new Promise((resolve, reject) => {
    try {
      resolve(UserStore.getUsers(query))
    } catch (err) {
      reject(err)
    }
  })
}

// Lista de usuarios activos.
export function getUsersActive(query) {
  return new Promise((resolve, reject) => {
    try {
      resolve(UserStore.getUsersActive(query))
    } catch (err) {
      reject(err)
    }
  })
}

// devolver usuario por id.
export function getUser(userId) {
  return new Promise((resolve, reject) => {
    try {
      resolve(UserStore.getUser(userId))
    } catch (err) {
      reject(err)
    }
  })
}

// registrar usuario.
export function createUser(data) {
  return new Promise((resolve, reject) => {
    generatePassword(data.password).then(hash => {
      data.password = hash
      try {
        resolve(UserStore.createUser(data))
      } catch (err) {
        reject(err)
      }
    }).catch(err => reject(err))
  })
}

// actualizar usuario.
export function updateUser(id, data) {
  return new Promise((resolve, reject) => {
    try {
      resolve(UserStore.updateUser(id, data))
    } catch (err) {
      reject(err)
    }
  })
}

// borrar usuario.
export function deleteUser(id) {
  return new Promise((resolve, reject) => {
    try {
      resolve(UserStore.deleteUser(id))
    } catch (err) {
      reject(err)
    }
  })
}

// Login de acceso => retorna un [token].
export function userLogin(userName, password) {
  return new Promise(async (resolve, reject) => {
    let _user = await UserStore.checkUserExist(userName)
    if (!_user) reject(new Error('El usuario no Existe'))
    if (_user.suspended) reject(new Error('Cuenta Suspendida'))
    bcrypt.compare(password, _user.password).then(result => {
      if (result === false) {
        reject(new Error('Contraseña Incorrecta'))
      } else {
        let token = jwt.sign({
          _id: _user._id,
        }, process.env.JWT_SECRET_KEY, {expiresIn: '24h'})
        resolve(token)
      }
    })
  })
}

// Cambiar contraseña del usuario.
export function passwordChange(userId, password) {
  return new Promise(async (resolve, reject) => {
    getUser(userId).then(currentUser => {
      if (!currentUser) {
        reject(new Error('El usuario no existe!'))
      }
      generatePassword(password).then(hash => {
        currentUser.password = hash
        try {
          resolve(UserStore.updateUser(userId, currentUser))
        } catch (err) {
          reject(err)
        }
      }).catch(err => reject(err))
    })
  })
}

// registrar cuenta super usuario.
export function createSuperUser() {
  return new Promise((resolve, reject) => {
    let userName = process.env.SUPER_USER_NAME
    UserStore.checkUserExist(userName).then(currentUser => {
      if (currentUser) {
        generatePassword(process.env.PASSWORD_SUPER_USER).then(hash => {
          currentUser.password = hash
          resolve(UserStore.updateUser(currentUser._id, currentUser))
        }).catch(err => reject(err))
      } else {
        generatePassword(process.env.PASSWORD_SUPER_USER).then(hash => {
          resolve(UserStore.createSuperUser(userName, hash))
        }).catch(err => reject(err))
      }
    })
  })
}

// cambiar estado del usuario.
export function changeStatusUserAccount(userId, status) {
  return new Promise((resolve, reject) => {
    UserStore.getUser(userId).then(currentUser => {
      currentUser.suspended = status
      try {
        resolve(UserStore.updateUser(userId, currentUser))
      } catch (err) {
        reject(err)
      }
    }).catch(err => reject(err))
  })
}