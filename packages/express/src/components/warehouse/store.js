import {Warehouse} from './model'

// Crud Almacenes.
export class WarehouseStore {
  // Lista de almacenes.
  static async getWarehouses(query) {
    return Warehouse.find({name: {$regex: query}, isDeleted: false})
  }

  // Obtener almacén por id.
  static async getWarehouse(id) {
    return Warehouse.findById(id)
  }

  // Lista de almacenes por tipo.
  static async getWarehousesByType(type, query) {
    return Warehouse.find({name: {$regex: query}, typeWarehouse: type, isDeleted: false})
  }

  // Agregar nuevo almacén.
  static async addWarehouse(data) {
    let _warehouse = new Warehouse(data)
    _warehouse.isDeleted = false
    await _warehouse.save()
    return _warehouse
  }

  // Actualizar almacén.
  static async updateWarehouse(id, data) {
    return Warehouse.findByIdAndUpdate(id, data, {new: true})
  }

  // borrar almacén.
  static async deleteWarehouse(id) {
    let _warehouse = await this.getWarehouse(id)
    _warehouse.isDeleted = false
    return await this.updateWarehouse(id, _warehouse)
  }
}
