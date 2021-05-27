import jwt from 'jsonwebtoken'
import {response} from 'express'
import {getUser} from './controller'

// Middleware para verificar el Token de acceso.
const verifyToken = (req, res = response, next) => {
  if (!req.headers.Authorization) {
    return res.status(401).json({
      msg: 'Solicitud no autorizada'
    })
  }
  const token = req.headers.Authorization.split(' ')[1]
  if (token === null) {
    return res.status(401).json({
      msg: 'Solicitud no autorizada'
    })
  }
  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET_KEY)
    getUser(decoded._id).then(currentUser => {
      if (!currentUser) {
        return res.status(401).json({
          msg: 'Token invalido'
        })
      } else {
        req.currentUser = currentUser
        next()
      }
    }).catch(err => {
      console.log(err)
    })
  } catch (err) {
    return res.status(401).json({
      msg: 'Token invalido'
    })
  }
}

export default verifyToken
