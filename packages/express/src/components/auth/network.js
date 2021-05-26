import express from 'express'
import verifyToken from './verifyToken'
import * as controller from './controller'

const router = express.Router()

// Lista de usuarios.
router.get('/', verifyToken, async (req, res) => {
  const status = req.query.status || false
  controller.getUsers(status).then(result => {
    res.json(result)
  }).catch(err => {
    res.status(500).json(err)
  })
})

// retornar usuario para editar.
router.get('/:id/edit', verifyToken, async (req, res) => {
  controller.getUser(req.params.id).then(result => {
    res.json(result)
  }).catch(err => {
    res.status(500).json(err)
  })
})

// devolver la información del usuario autenticado.
router.get('/profile', verifyToken, async (req, res) => {
  controller.getUser(req.userId).then(result => {
    res.json(result)
  }).catch(err => {
    res.status(500).json(err)
  })
})

// permisos del usuario administrador.
router.get('/is-admin', verifyToken, async (req, res) => {
  res.json(req.isAdmin)
})

// permisos del usuario redes.
router.get('/is-redes', verifyToken, async (req, res) => {
  res.json(req.redes)
})

// permisos del usuario caja.
router.get('/is-caja', verifyToken, async (req, res) => {
  res.json(req.caja)
})

// registrar usuario.
router.post('/', verifyToken, async (req, res) => {
  controller.createUser(req.body).then(result => {
    res.json(result)
  }).catch(err => {
    res.status(500).json(err)
  })
})

// actualizar datos del usuario.
router.patch('/:id', verifyToken, async (req, res) => {
  controller.updateUser(req.params.id, req.body).then(result => {
    res.json(result)
  }).catch(err => {
    res.status(500).json(err)
  })
})

// dependencia de documentos.
router.get('/:id/dependency', verifyToken, async (req, res) => {
  res.status(200).json(0)
})

// borrar usuarios.
router.delete('/:id', verifyToken, async (req, res) => {
  if (!req.isAdmin) {
    return res.status(500).send('Unauthorized request')
  } else {
    controller.deleteUser(req.params.id).then(result => {
      if (!result) {
        res.status(404).send('No item found')
      }
      res.status(200).send()
    }).catch(err => {
      res.status(500).json(err)
    })
  }
})

// Login de acceso.
router.post('/login', async (req, res) => {
  const {userName, password} = req.body
  controller.userLogin(userName, password).then(token => {
    res.status(200).json(token)
  }).catch(err => {
    res.status(500).json(err)
  })
})

// Cambiar contraseña.
router.post('/:id/change-password', verifyToken, async (req, res) => {
  const {current} = req.body
  controller.passwordChange(req.params.id, current).then(() => {
    res.status(200).send()
  }).catch(err => {
    res.status(500).json(err)
  })
})

export default router
