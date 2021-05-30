import express, {response} from 'express'
import verifyToken from '../middlewares/verify-token'
import {ContactController} from './controller'

const router = express.Router()

// http://<HOST>/api/contacts
router.get('/', [verifyToken], getContacts)

// Listar contactos.
function getContacts(req, res = response) {
  const {search = ''} = req.query
  ContactController.getContacts(search).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/contacts/:id
router.get('/:id', [verifyToken], getContact)

// Obtener contacto por id.
function getContact(req, res = response) {
  ContactController.getContact(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/contacts
router.post('/', [verifyToken], addContact)

// registrar contacto.
function addContact(req, res = response) {
  ContactController.addContact(req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/contacts/:id
router.put('/:id', [verifyToken], updateContact)

// actualizar contacto.
function updateContact(req, res = response) {
  ContactController.updateContact(req.params.id, req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/contacts/:id
router.delete('/:id', [verifyToken], deleteContact)

// borrar contacto.
function deleteContact(req, res = response) {
  ContactController.deleteContact(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/contacts/select2/q
router.get('/select2/q', [verifyToken], getContactsWithSelect2)

// listar contactos para select2.
function getContactsWithSelect2(req, res = response) {
  const {term = ''} = req.query
  ContactController.getContactsWithSelect2(term).then(result => {
    res.json(result)
  })
}

export const contactRouter = router
