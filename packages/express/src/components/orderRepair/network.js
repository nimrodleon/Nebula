import express, {response} from 'express'
import {verifyToken} from '../middlewares'
import {OrderRepairController} from './controller'

const router = express.Router()

// http://<HOST>/api/order_repairs
router.get('/', [verifyToken], getOrderRepairs)

// Listar ordenes de reparación.
function getOrderRepairs(req, res = response) {
  const {search = ''} = req.query
  OrderRepairController.getOrderRepairs(search).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/order_repairs/:id
router.get('/:id', [verifyToken], getOrderRepair)

// obtener orden de reparación por id.
function getOrderRepair(req, res = response) {
  OrderRepairController.getOrderRepair(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/order_repairs
router.post('/', [verifyToken], addOrderRepair)

// registrar orden de reparación.
function addOrderRepair(req, res = response) {
  OrderRepairController.addOrderRepair(req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/order_repairs/:id
router.put('/:id', [verifyToken], updateOrderRepair)

// actualizar orden de reparación.
function updateOrderRepair(req, res = response) {
  OrderRepairController.updateOrderRepair(req.params.id, req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/order_repairs/:id
router.delete('/:id', [verifyToken], deleteOrderRepair)

// borrar orden de reparación.
function deleteOrderRepair(req, res = response) {
  OrderRepairController.deleteOrderRepair(req.params.id).then(result => {
    res.json(result)
  })
}

export const orderRepairRouter = router
