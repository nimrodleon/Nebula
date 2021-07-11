import express, {response} from 'express'
import {verifyToken} from '../middlewares'
import {ArticleController} from './controller'

const router = express.Router()

// http://<HOST>/api/articles
router.get('/', [verifyToken], getArticles)

// Listar artículos.
function getArticles(req, res = response) {
  const {search = ''} = req.query
  ArticleController.getArticles(search).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/articles/:id
router.get('/:id', [verifyToken], getArticle)

// obtener artículo por id.
function getArticle(req, res = response) {
  ArticleController.getArticle(req.params.id).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/articles
router.post('/', [verifyToken], addArticle)

// registrar artículo.
function addArticle(req, res = response) {
  ArticleController.addArticle(req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/articles/:id
router.put('/:id', [verifyToken], updateArticle)

// actualizar artículo.
function updateArticle(req, res = response) {
  ArticleController.updateArticle(req.params.id, req.body).then(result => {
    res.json(result)
  })
}

// http://<HOST>/api/articles/:id
router.delete('/:id', [verifyToken], deleteArticle)

// borrar artículo.
function deleteArticle(req, res = response) {
  ArticleController.deleteArticle(req.params.id).then(result => {
    res.json(result)
  })
}

export const articleRouter = router
