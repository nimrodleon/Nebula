import React, {Fragment} from 'react'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faLeaf, faUserCircle} from '@fortawesome/free-solid-svg-icons'
import {Link, useHistory} from 'react-router-dom'
import {Nav, Navbar} from 'react-bootstrap'

const NavbarHTML = () => {
	let history = useHistory()

	// Salir del Sistema.
	const handleLogout = () => {
		localStorage.removeItem('token')
		history.push('/login')
	}

	// Render Component.
	return (
		<Fragment>
			<Navbar bg="white">
				<Navbar.Brand href="/">
					<div className="font-weight-bold">
						<FontAwesomeIcon icon={faLeaf} size={'lg'} fixedWidth={true} className="text-success"/>
						<span className="ml-1">SGT-Pre-alpha1</span>
					</div>
				</Navbar.Brand>
				<Nav className="ml-auto">
					<Link to="#" style={{textDecoration: 'none'}}>
						<div className="d-flex align-items-center text-dark">
							<span className="text-uppercase mr-1">SUPER</span>
							<span className="pr-2" style={{fontSize: '1.25rem'}}>
                <FontAwesomeIcon icon={faUserCircle}/>
              </span>
						</div>
					</Link>
					<button onClick={handleLogout} className="btn btn-danger btn-sm">
						<span className="text-uppercase mx-2">Salir</span>
					</button>
				</Nav>
			</Navbar>
		</Fragment>
	)
}

export default NavbarHTML