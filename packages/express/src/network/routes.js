import authRouter from '../components/auth/network'

const routes = (server) => {
  server.use('/api/users', authRouter)
}

export default routes
