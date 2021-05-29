import {Tax} from './model'

// Crud impuesto.
export class TaxStore {
  // Listar impuestos.
  static async getTaxes(query) {
    return Tax.find({name: {$regex: query}})
  }

  // Obtener impuesto por id.
  static async getTax(id) {
    return Tax.findById(id)
  }

  // registrar impuesto.
  static async addTax(data) {
    let _tax = new Tax(data)
    _tax.isdeleted = false
    await _tax.save()
    return _tax
  }

  // actualizar impuesto.
  static async updateTax(id, data) {
    return Tax.findByIdAndUpdate(id, data, {new: true})
  }

  // borrar impuesto.
  static async deleteTax(id) {
    let _tax = await this.getTax(id)
    _tax.isdeleted = false
    return this.updateTax(id, _tax)
  }
}
