import {TaxStore} from './store'

// Controlador impuestos.
export class TaxController {
  // Listar impuestos.
  static getTaxes(query) {
    return new Promise((resolve, reject) => {
      try {
        resolve(TaxStore.getTaxes(query))
      } catch (err) {
        reject(err)
      }
    })
  }

  // Obtener impuesto por id.
  static getTax(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(TaxStore.getTax(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // registrar impuesto.
  static addTax(data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(TaxStore.addTax(data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // actualizar impuesto.
  static updateTax(id, data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(TaxStore.updateTax(id, data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // borrar impuesto.
  static deleteTax(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(TaxStore.deleteTax(id))
      } catch (err) {
        reject(err)
      }
    })
  }
}
