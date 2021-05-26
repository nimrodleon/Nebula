import axios from 'axios'

const configURL = {
	development: 'http://localhost:8084/api/',
	production: '/api/',
}

export const baseURL = configURL.development

// Load token from localStorage.
const token = localStorage.getItem('token') || ''
export const accessToken = token.length > 0 ? `Bearer ${token}` : token

// Set config defaults when creating the instance.
const instance = axios.create({baseURL: baseURL})
instance.defaults.headers.common['Authorization'] = accessToken

export default instance