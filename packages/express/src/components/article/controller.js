import {ArticleStore} from './store'

// Lógica - artículos.
export class ArticleController {
  // Lista de artículos.
  static async getArticles(query) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ArticleStore.getArticles(query))
      } catch (err) {
        reject(err)
      }
    })
  }

  // obtener artículos por id.
  static async getArticle(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ArticleStore.getArticle(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // registrar artículo.
  static async addArticle(data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ArticleStore.addArticle(data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // actualizar artículo.
  static async updateArticle(id, data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ArticleStore.updateArticle(id, data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // borrar artículo.
  static async deleteArticle(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ArticleStore.deleteArticle(id))
      } catch (err) {
        reject(err)
      }
    })
  }
}
