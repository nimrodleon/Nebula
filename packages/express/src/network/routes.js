import * as network from './network'

const routes = (server) => {
  server.use('/api/users', network.authRouter)
}

export default routes
