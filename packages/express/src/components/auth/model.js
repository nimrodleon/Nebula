import {model, Schema} from 'mongoose'

export const User = model('User',
  new Schema({
    name: String,
    userName: String,
    password: String,
    avatar: String,
    isAdmin: Boolean,
    redes: Boolean,
    caja: Boolean,
    suspended: {
      type: Boolean,
      default: false
    },
    isDeleted: {
      type: Boolean,
      default: false
    }
  }))
