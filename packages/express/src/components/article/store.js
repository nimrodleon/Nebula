import {Article} from './model'

// CRUD - artículos.
export class ArticleStore {
  // Lista de artículos.
  static async getArticles(query) {
    return Article.find({name: {$regex: query}, isDeleted: false})
  }

  // Obtener artículo por id.
  static async getArticle(id) {
    return Article.findById(id)
  }

  // registrar artículo.
  static async addArticle(data) {
    let _article = new Article(data)
    _article.isDeleted = false
    await _article.save()
    return _article
  }

  // actualizar artículo.
  static async updateArticle(id, data) {
    return Article.findByIdAndUpdate(id, data, {new: true})
  }

  // borrar artículo.
  static async deleteArticle(id) {
    let _article = await this.getArticle(id)
    _article.isDeleted = true
    return this.updateArticle(id, _article)
  }
}
