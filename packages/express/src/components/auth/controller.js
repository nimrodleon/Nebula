import bcrypt from 'bcrypt'
import jwt from 'jsonwebtoken'
import * as store from './store'

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
export function getUsers(status) {
  return new Promise((resolve, reject) => {
    try {
      resolve(store.getUsers(status))
    } catch (err) {
      reject(err)
    }
  })
}

// devolver usuario por id.
export function getUser(userId) {
  return new Promise((resolve, reject) => {
    try {
      resolve(store.getUser(userId))
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
        resolve(store.createUser(data))
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
      resolve(store.updateUser(id, data))
    } catch (err) {
      reject(err)
    }
  })
}

// borrar usuario.
export function deleteUser(id) {
  return new Promise((resolve, reject) => {
    try {
      resolve(store.deleteUser(id))
    } catch (err) {
      reject(err)
    }
  })
}

// Login de acceso => retorna un [token].
export function userLogin(userName, password) {
  return new Promise(async (resolve, reject) => {
    let _user = await store.getUserByUserName(userName)
    if (!_user) reject(new Error('El usuario no Existe'))
    if (_user.suspended) reject(new Error('Cuenta Suspendida'))
    bcrypt.compare(password, _user.password).then(result => {
      if (result === false) {
        reject(new Error('Contraseña Incorrecta'))
      } else {
        let exp = _user.isAdmin || _user.redes ? '10m' : '4h'
        let token = jwt.sign({
          _id: _user._id,
          isAdmin: _user.isAdmin,
          redes: _user.redes,
          caja: _user.caja,
        }, 'ias0SH23FN47L0ZciKN204BFfWwj6vNY', {expiresIn: exp})
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
          resolve(store.updateUser(userId, currentUser))
        } catch (err) {
          reject(err)
        }
      }).catch(err => reject(err))
    })
  })
}
