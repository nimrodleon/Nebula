import {response} from 'express'
import {validationResult} from 'express-validator'

// middleware validar campos.
export const validate = (req, res = response, next) => {
  const errors = validationResult(req)
  if (!errors.isEmpty()) {
    return res.status(400).json(errors)
  }
  next()
}
