import {model, Schema} from 'mongoose'
import moment from 'moment-timezone'

// definición  del schema orden de reparación.
const orderRepairSchema = new Schema({
  receptionDate: {
    type: Date,
    default: moment().utc().toDate(),
  },
  memberUserId: {
    type: Schema.Types.ObjectId,
    ref: 'User'
  },
  clientId: {
    type: Schema.Types.ObjectId,
    ref: 'Contact'
  },
  warehouseId: {
    type: Schema.Types.ObjectId,
    ref: 'Warehouse'
  },
  deviceInfo: String,
  failure: String,
  promisedDate: {
    type: Date,
    required: true
  },
  PromisedHour: {
    type: String,
    required: true
  },
  technicalUserId: {
    type: Schema.Types.ObjectId,
    ref: 'User'
  },
  // Estado de la reparación.
  // enum['SR': 'SIN REVISAR', 'ER': 'EN REPARACIÓN', 'RT': 'REPARACIÓN TERMINADA']
  Status: {
    type: String,
    default: 'SR',
    enum: ['SR', 'ER', 'RT']
  },
  equipoEntregado: {
    type: Boolean,
    default: false
  },
  isDeleted: {
    type: Boolean,
    default: false
  }
})

// orderRepairSchema.methods.toJSON = function () {
//   const {receptionDate, ...orderRepair} = this.toObject()
//   orderRepair.receptionDate = moment(receptionDate).tz('America/Lima').toDate()
// }

// definición del modelo orden de reparación.
export const OrderRepair = model('OrderRepair', orderRepairSchema)
