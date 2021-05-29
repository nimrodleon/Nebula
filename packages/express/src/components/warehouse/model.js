import {model, Schema} from 'mongoose'

// definición  del schema almacén.
const warehouseSchema = new Schema({
  typeWarehouse: String,
  name: String,
  isDeleted: {
    type: Boolean,
    default: false
  }
})

// definición del modelo usuario.
export const Warehouse = model('Warehouse', warehouseSchema)
