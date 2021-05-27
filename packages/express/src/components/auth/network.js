import express, {response} from 'express'
import * as controller from './controller'
import verifyToken from './verify-token'

const router = express.Router()

// Login de acceso al sistema.
// http://<HOST>/api/users/login
router.post('/login', [], loginAccess)

function loginAccess(req, res = response) {
  let {userName, password} = req.body
  controller.userLogin(userName, password).then(token => {
    res.json({token: token})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// registrar super usuario.
// http://<HOST>/api/users/register_super_user
router.post('/register_super_user', [], registerSuperUser)

function registerSuperUser(req, res = response) {
  controller.createSuperUser().then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// Lista de usuarios.
// http://<HOST>/api/users/
router.get('/', [verifyToken], getUsers)

function getUsers(req, res = response) {
  controller.getUsers(req.query.search).then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// obtener usuario por id.
// http://<HOST>/api/users/:id
router.get('/:id', [verifyToken], getUser)

function getUser(req, res = response) {
  controller.getUser(req.params.id).then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// registrar usuario.
// http://<HOST>/api/users/
router.post('/', [verifyToken], createUser)

function createUser(req, res = response) {
  controller.createUser(req.body).then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// editar usuario por id.
// http://<HOST>/api/users/:id
router.put('/:id', [verifyToken], updateUser)

function updateUser(req, res = response) {
  controller.updateUser(req.params.id, req.body).then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// borrar usuario por id.
// http://<HOST>/api/users/:id
router.delete('/:id', [verifyToken], deleteUser)

function deleteUser(req, res = response) {
  controller.deleteUser(req.params.id).then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// cambiar estado del usuario.
// http://<HOST>/api/users/change_status/:id/:status
router.patch('/change_status/:id/:status', [verifyToken], userChangeStatus)

function userChangeStatus(req, res = response) {
  controller.changeStatusUserAccount(req.params.id, req.params.status).then(result => {
    res.json({data: result})
  }).catch(err => {
    res.status(500).json({err: err})
  })
}

// cambiar contraseña del usuario.
// http://<HOST>/api/users/password_change/:id
router.patch('/password_change/:id', [verifyToken], userPasswordChange)

function userPasswordChange(req, res = response) {

}

export default router
