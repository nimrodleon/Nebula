import * as network from './network'

const routes = (server) => {
  server.use('/api/users', network.authRouter)
  server.use('/api/warehouses', network.warehouseRouter)
  server.use('/api/taxes', network.taxRouter)
  server.use('/api/articles', network.articleRouter)
  server.use('/api/contacts', network.contactRouter)
}

export default routes
