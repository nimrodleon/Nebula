import _ from 'lodash'
import express, {response} from 'express'
import * as controller from './controller'
import {verifyToken} from '../middlewares'

const router = express.Router()

// http://<HOST>/api/users/login
router.post('/login', [], loginAccess)

// Login de acceso al sistema.
function loginAccess(req, res = response) {
  let {userName, password} = req.body
  console.log(userName, password)
  controller.userLogin(userName, password).then(token => {
    res.json({token: token})
  }).catch(err => {
    console.error('[loginAccess]', err.message)
    res.status(400).json({
      ok: false,
      msg: 'Usuario y/o Contraseña Invalida!'
    })
  })
}

// http://<HOST>/api/users/register_super_user
router.post('/register_super_user', [], registerSuperUser)

// registrar super usuario.
function registerSuperUser(req, res = response) {
  controller.createSuperUser().then(result => {
    res.json(result)
  }).catch(err => {
    console.error('[registerSuperUser]', err.message)
    res.status(400).json({ok: false})
  })
}

// http://<HOST>/api/users/
router.get('/', [verifyToken], getUsers)

// Lista de usuarios.
function getUsers(req, res = response) {
  const {search = ''} = req.query
  controller.getUsers(search).then(result => {
    res.json(result)
  }).catch(err => {
    console.error('[getUsers]', err)
  })
}

// http://<HOST>/api/users/:id
router.get('/:id', [verifyToken], getUser)

// obtener usuario por id.
function getUser(req, res = response) {
  controller.getUser(req.params.id).then(result => {
    res.json(result)
  }).catch(err => {
    console.error('[getUser]', err)
  })
}

// http://<HOST>/api/users/
router.post('/', [
  verifyToken

], createUser)

// registrar usuario.
function createUser(req, res = response) {
  controller.createUser(req.body).then(result => {
    res.status(201).json(result)
  }).catch(err => {
    console.error('[createUser]', err.message)
  })
}

// http://<HOST>/api/users/:id
router.put('/:id', [verifyToken], updateUser)

// editar usuario por id.
function updateUser(req, res = response) {
  controller.updateUser(req.params.id, req.body).then(result => {
    res.json(result)
  }).catch(err => {
    console.error('[updateUser]', err.message)
  })
}

// http://<HOST>/api/users/:id
router.delete('/:id', [verifyToken], deleteUser)

// borrar usuario por id.
function deleteUser(req, res = response) {
  controller.deleteUser(req.params.id).then(result => {
    res.json(result)
  }).catch(err => {
    console.error('[deleteUser]', err)
  })
}

// http://<HOST>/api/users/change_status/:id/:status
router.patch('/change_status/:id/:status', [verifyToken], userChangeStatus)

// cambiar estado del usuario.
function userChangeStatus(req, res = response) {
  controller.changeStatusUserAccount(req.params.id, req.params.status).then(() => {
    res.status(200).send()
  }).catch(err => {
    console.error('[userChangeStatus]', err)
  })
}

// http://<HOST>/api/users/password_change/:id
router.patch('/password_change/:id', [verifyToken], userPasswordChange)

// cambiar contraseña del usuario.
function userPasswordChange(req, res = response) {
  const {password} = req.body
  controller.passwordChange(req.params.id, password).then(() => {
    res.status(200).send()
  }).catch(err => {
    console.error('[userPasswordChange]', err)
  })
}

// http://<HOST>/api/users/select2/q
router.get('/select2/q', [verifyToken], getUsersWithSelect2)

// listar usuarios con select2.
function getUsersWithSelect2(req, res = response) {
  let {term = ''} = req.query
  controller.getUsersActive(term).then(async (result) => {
    let data = {results: []}
    await _.forEach(result, value => {
      data.results.push({id: value._id, text: value.fullName})
    })
    res.json(data)
  }).catch(err => {
    console.error('[getUsersWithSelect2]', err)
  })
}

// exportar rutas auth.
export const authRouter = router
