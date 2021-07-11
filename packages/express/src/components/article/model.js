import {model, Schema} from 'mongoose'

// definición  del schema artículo.
const articleSchema = new Schema({
  name: String,
  typeArticle: String,
  barCode: String,
  taxId: {
    type: Schema.Types.ObjectId,
    ref: 'Tax'
  },
  price1: Number,
  price2: Number,
  remark: String,
  isDeleted: {
    type: Boolean,
    default: false
  }
})

// definición del modelo artículo.
export const Article = model('Article', articleSchema)
