import jwt from 'jsonwebtoken'

// Middleware para verificar el Token de acceso.
const verifyToken = (req, res, next) => {
  if (!req.headers.authorization) {
    return res.status(401).send('Unauthorized request')
  }
  const token = req.headers.authorization.split(' ')[1]
  if (token === null) {
    return res.status(401).send('Unauthorized request')
  }
  try {
    const decoded = jwt.verify(token, 'ias0SH23FN47L0ZciKN204BFfWwj6vNY')
    req.userId = decoded._id
    req.isAdmin = decoded.isAdmin
    req.redes = decoded.redes
    req.caja = decoded.caja
    next()
  } catch (err) {
    return res.status(401).send('Invalid token')
  }
}

export default verifyToken
