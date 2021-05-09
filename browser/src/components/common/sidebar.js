import React, {Fragment} from 'react'
import styled from 'styled-components'
import {Link} from 'react-router-dom'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faBox, faCogs, faTools} from '@fortawesome/free-solid-svg-icons'

const SidebarContainer = styled.div`
	width: 100px;
	background-color: #282c34;
	min-height: 100%;
	border-right: 4px solid #09d3ac;
`

const SidebarItem = styled.div`
	text-align: center;

	a {
		display: block;
		padding-top: 15px;
		color: white;
		text-decoration: none;

		&:hover {
			background-color: #09d3ac;
			color: #282c34;
		}
	}

	svg {
		font-size: 40px;
	}
`

const SidebarItemText = styled.span`
	display: block;
	font-size: 14px;
	text-transform: uppercase;
	font-weight: bold;
	padding: 4px;
`

const Sidebar = () => {
	return (
		<Fragment>
			<SidebarContainer>
				<SidebarItem>
					<Link to="/repair">
						<FontAwesomeIcon icon={faTools}/>
						<SidebarItemText>Taller</SidebarItemText>
					</Link>
				</SidebarItem>
				<SidebarItem>
					<Link to="/articles">
						<FontAwesomeIcon icon={faBox}/>
						<SidebarItemText>Artículos</SidebarItemText>
					</Link>
				</SidebarItem>
				<SidebarItem>
					<Link to="/system">
						<FontAwesomeIcon icon={faCogs}/>
						<SidebarItemText>Sistema</SidebarItemText>
					</Link>
				</SidebarItem>
			</SidebarContainer>
		</Fragment>
	)
}

export default Sidebar