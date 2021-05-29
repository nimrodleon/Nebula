import express, {response} from 'express'
import verifyToken from '../middlewares/verify-token'
import {WarehouseController} from './controller'

const router = express.Router()

// http://<HOST>/api/warehouses
router.get('/', [verifyToken], getWarehouses)

// Lista de almacenes.
function getWarehouses(req, res = response) {
  const {search = ''} = req.query
  WarehouseController.getWarehouses(search).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/warehouses/:id
router.get('/:id', [verifyToken], getWarehouse)

// obtener almacén por id
function getWarehouse(req, res = response) {
  WarehouseController.getWarehouse(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/warehouses
router.post('/', [verifyToken], addWarehouse)

// registrar almacén.
function addWarehouse(req, res = response) {
  WarehouseController.addWarehouse(req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/warehouses/:id
router.put('/:id', [verifyToken], updateWarehouse)

// Actualizar almacén.
function updateWarehouse(req, res = response) {
  WarehouseController.updateWarehouse(req.params.id, req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/warehouses/:id
router.delete('/:id', [verifyToken], deleteWarehouse)

// borrar almacén.
function deleteWarehouse(req, res = response) {
  WarehouseController.deleteWarehouse(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/warehouses/:type/select2/q
router.get('/:type/select2/q', [verifyToken], getWarehousesWithSelect2)

// Cargar almacenes con select2.
function getWarehousesWithSelect2(req, res = response) {
  const {term = ''} = req.query
  WarehouseController.getWarehousesWithSelect2(req.params.type, term).then(result => {
    res.json(result)
  })
}

export const warehouseRouter = router
