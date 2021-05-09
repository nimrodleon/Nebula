import React, {Fragment, useEffect} from 'react'
import NavbarHTML from '../common/navbarHTML'
import Sidebar from '../common/sidebar'
import {loggedIn, Wrap} from '../common/util'
import {Route, Switch, useHistory, useRouteMatch} from 'react-router-dom'
import Home from './index'
import System from '../system/system'
import Articles from '../articles'
import Repair from '../repair'

const Dashboard = () => {
	let match = useRouteMatch()
	let history = useHistory()

	useEffect(() => {
		if (!loggedIn()) {
			history.push('/login')
		}
	})

	// Render Component.
	return (
		<Fragment>
			<NavbarHTML/>
			<Wrap>
				<Sidebar/>
				<Switch>
					<Route path="/articles" component={Articles}/>
					<Route path="/repair" component={Repair}/>
					<Route path="/system" component={System}/>
					{/* # System default. */}
					<Route path={match.path} component={Home}/>
				</Switch>
			</Wrap>
		</Fragment>
	)
}

export default Dashboard