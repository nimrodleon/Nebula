import {OrderRepairStore} from './store'

// Lógica - Orden de reparación.
export class OrderRepairController {
  // Listar ordenes de reparación.
  static async getOrderRepairs(query) {
    return new Promise((resolve, reject) => {
      try {
        resolve(OrderRepairStore.getOrderRepairs(query))
      } catch (err) {
        reject(err)
      }
    })
  }

  // obtener orden de reparación por id.
  static async getOrderRepair(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(OrderRepairStore.getOrderRepair(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // registrar orden de reparación.
  static async addOrderRepair(data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(OrderRepairStore.addOrderRepair(data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // actualizar orden de reparación.
  static async updateOrderRepair(id, data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(OrderRepairStore.updateOrderRepair(id, data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // borrar orden de reparación.
  static async deleteOrderRepair(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(OrderRepairStore.deleteOrderRepair(id))
      } catch (err) {
        reject(err)
      }
    })
  }
}
