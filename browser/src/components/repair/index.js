import React, {Fragment} from 'react'
import {Route, Switch, useRouteMatch} from 'react-router-dom'
import RepairList from './repair-list'
import OrderRepair from './order-repair'

/**
 * Router base de reparaciones.
 * @returns {JSX.Element}
 * @constructor
 */
const Repair = () => {
	let match = useRouteMatch()

	// Render Component.
	return (
		<Fragment>
			<Switch>
				<Route path={`${match.path}/form`} component={OrderRepair}/>
				{/* # System default. */}
				<Route path={match.path} component={RepairList}/>
			</Switch>
		</Fragment>
	)
}

export default Repair