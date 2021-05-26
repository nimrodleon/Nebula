import React from 'react'
import ReactDOM from 'react-dom'
import App from './components/common/App'
import reportWebVitals from './reportWebVitals'
import * as jquery from 'jquery'
import 'select2/dist/js/select2'
import 'select2/dist/css/select2.css'
import '@ttskch/select2-bootstrap4-theme/dist/select2-bootstrap4.css'
import './bootstrap.scss'

window.jQuery = window.$ = jquery

ReactDOM.render(
	<React.StrictMode>
		<App/>
	</React.StrictMode>,
	document.getElementById('root')
)

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals()
