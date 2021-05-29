import _ from 'lodash'
import {WarehouseStore} from './store'

// controlador de almacenes.
export class WarehouseController {
  // Lista de almacenes.
  static getWarehouses(query) {
    return new Promise((resolve, reject) => {
      try {
        resolve(WarehouseStore.getWarehouses(query))
      } catch (err) {
        reject(err)
      }
    })
  }

  // obtener almacenes por id.
  static getWarehouse(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(WarehouseStore.getWarehouse(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // obtener almacenes por tipo.
  static getWarehousesByType(type, query) {
    return new Promise((resolve, reject) => {
      try {
        resolve(WarehouseStore.getWarehousesByType(type, query))
      } catch (err) {
        reject(err)
      }
    })
  }

  // registrar almacén.
  static addWarehouse(data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(WarehouseStore.addWarehouse(data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // actualizar almacén.
  static updateWarehouse(id, data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(WarehouseStore.updateWarehouse(id, data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // borrar almacén.
  static deleteWarehouse(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(WarehouseStore.deleteWarehouse(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // cargar almacenes con select2.
  static getWarehousesWithSelect2(type, term) {
    return new Promise(async (resolve, reject) => {
      try {
        let _warehouses = await WarehouseStore.getWarehousesByType(type, term)
        let data = {results: []}
        await _.forEach(_warehouses, value => {
          data.results.push({id: value._id, text: value.name})
        })
        resolve(data)
      } catch (err) {
        reject(err)
      }
    })
  }
}
