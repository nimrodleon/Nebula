import {model, Schema} from 'mongoose'

// definición  del schema contactos.
const contactSchema = new Schema({
  typeDoc: String,
  document: String,
  fullName: String,
  address: String,
  phoneNumber: String,
  email: String,
  isDeleted: {
    type: Boolean,
    default: false
  }
})

// definición del modelo contactos.
export const Contact = model('Contact', contactSchema)
