import {model, Schema} from 'mongoose'

// definición  del schema usuarios.
const userSchema = new Schema({
  fullName: String,
  address: String,
  phoneNumber: String,
  userName: {
    type: String,
    required: true,
    unique: true
  },
  password: String,
  permission: {
    type: String,
    default: 'ROLE_USER'
  },
  email: {
    type: String,
    required: true,
    unique: true
  },
  suspended: {
    type: Boolean,
    default: false
  },
  isDeleted: {
    type: Boolean,
    default: false
  }
})

// definición del modelo usuario.
export const User = model('User', userSchema)
