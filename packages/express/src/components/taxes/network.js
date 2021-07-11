import express, {response} from 'express'
import {verifyToken} from '../middlewares'
import {TaxController} from './controller'

const router = express.Router()

// http://<HOST>/api/taxes
router.get('/', [verifyToken], getTaxes)

// Listar impuestos.
function getTaxes(req, res = response) {
  const {search = ''} = req.query
  TaxController.getTaxes(search).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/taxes/:id
router.get('/:id', [verifyToken], getTax)

// obtener impuesto por id.
function getTax(req, res = response) {
  TaxController.getTax(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/taxes
router.post('/', [verifyToken], addTax)

// registrar nuevo impuesto.
function addTax(req, res = response) {
  TaxController.addTax(req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/taxes/:id
router.put('/:id', [verifyToken], updateTax)

// actualizar impuesto por id.
function updateTax(req, res = response) {
  TaxController.updateTax(req.params.id, req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/taxes/:id
router.delete('/:id', [verifyToken], deleteTax)

// borrar impuesto.
function deleteTax(req, res = response) {
  TaxController.deleteTax(req.params.id).then(result => {
    res.json(result)
  })
}

export const taxRouter = router
