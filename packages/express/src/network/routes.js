import * as network from './network'

const routes = (server) => {
  server.use('/api/users', network.authRouter)
  server.use('/api/warehouses', network.warehouseRouter)
}

export default routes
