import {Contact} from '../contact/model'
import {OrderRepair} from './model'

// CRUD - Orden de reparación.
export class OrderRepairStore {
  // Lista de ordenes de reparación.
  static async getOrderRepairs(query) {
    return Contact.aggregate([
      {
        $match: {
          fullName: {$regex: query}
        }
      },
      {
        $lookup: {
          from: 'contacts',
          localField: '_id',
          foreignField: 'clientId',
          as: 'OrderRepair'
        }
      },
      {
        $project: {
          fullName: true,
          OrderRepair: {
            $filter: {
              input: '$OrderRepair',
              as: 'rep',
              cond: {
                $and: [
                  {$eq: {'$$rep.equipoEntregado': false}},
                  {$eq: {'$$rep.isDeleted': false}}
                ]
              }
            }
          }
        }
      },
      {$unwind: '$OrderRepair',},
      {$sort: {'OrderRepair.receptionDate': -1}},
    ])
  }

  // Obtener orden de reparación por id.
  static async getOrderRepair(id) {
    return OrderRepair.findById(id)
  }

  // registrar orden de reparación.
  static async addOrderRepair(data) {
    let _orderRepair = new OrderRepair(data)
    await _orderRepair.save()
    return _orderRepair
  }

  // actualizar orden de reparación.
  static async updateOrderRepair(id, data) {
    return OrderRepair.findByIdAndUpdate(id, data, {new: true})
  }

  // borrar orden de reparación.
  static async deleteOrderRepair(id) {
    let _orderRepair = await this.getOrderRepair(id)
    _orderRepair.isDeleted = true
    return this.updateOrderRepair(id, _orderRepair)
  }
}
