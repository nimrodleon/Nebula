import React, {Fragment, useEffect} from 'react'

const Home = ({history}) => {
	useEffect(() => {
		history.push('/repair')
	})
	
	// Render Component.
	return (
		<Fragment>
			<h1>Home</h1>
		</Fragment>
	)
}

export default Home