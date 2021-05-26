import React, {Fragment} from 'react'
import {Route, Switch, useRouteMatch} from 'react-router-dom'
import ArticlesList from './articles-list'

/**
 * Router base de artículos.
 * @returns {JSX.Element}
 * @constructor
 */
const Articles = () => {
	let match = useRouteMatch()

	return (
		<Fragment>
			<Switch>
				{/* # System default. */}
				<Route path={match.path} component={ArticlesList}/>
			</Switch>
		</Fragment>
	)
}

export default Articles