import React, {Fragment} from 'react'
import {Provider} from 'react-redux'
import {BrowserRouter as Router, Route, Switch} from 'react-router-dom'
import store from '../../store'
import Dashboard from '../home/dashboard'
import Login from '../login/login'

const App = () => {
	return (
		<Fragment>
			<Router>
				<Provider store={store}>
					<Switch>
						<Route path="/login" component={Login}/>
						<Route path="/" component={Dashboard}/>
					</Switch>
				</Provider>
			</Router>
		</Fragment>
	)
}

export default App
