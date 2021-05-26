import React, {Fragment} from 'react'
import {Route, Switch, useRouteMatch} from 'react-router-dom'
import SystemLink from './system-link'
import ContactList from './contact'
import TaxesList from './taxes'
import UserList from './auth-user'
import DeviceTypeList from './device-type'
import WarehousesList from './warehouses'
import GeneralConfig from './general'

const System = () => {
	let match = useRouteMatch()
	return (
		<Fragment>
			<Switch>
				<Route path={`${match.path}/general`} component={GeneralConfig}/>
				<Route path={`${match.path}/users`} component={UserList}/>
				<Route path={`${match.path}/taxes`} component={TaxesList}/>
				<Route path={`${match.path}/contacts`} component={ContactList}/>
				<Route path={`${match.path}/devices-type`} component={DeviceTypeList}/>
				<Route path={`${match.path}/warehouses`} component={WarehousesList}/>
				{/* # System default. */}
				<Route path={match.path} component={SystemLink}/>
			</Switch>
		</Fragment>
	)
}

export default System