import {model, Schema} from 'mongoose'

// definición  del schema impuestos.
const taxSchema = new Schema({
  name: String,
  value: Number,
  isDeleted: {
    type: Boolean,
    default: false
  }
})

// definición del modelo impuesto.
export const Tax = model('Tax', taxSchema)
