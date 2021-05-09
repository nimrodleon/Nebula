import React from 'react'
import {Link} from 'react-router-dom'
import styled from 'styled-components'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'

const RouterLinkContainer = styled.div`
	margin: 4px;

	a {
		text-decoration: none;
	}
`

const RouterLinkContent = styled.div`
	display: flex;

	&:hover {
		background-color: #f4f6f8;
		border-radius: 4px;
		text-decoration: none;
	}
`

const RouterLinkIcon = styled.div`
	background-color: #919eab;
	padding: 4px 8px;
	border-radius: 2px;
	margin: 10px;
`

const RouterLink = (props) => {
	return (
		<RouterLinkContainer>
			<Link to={props.href}>
				<RouterLinkContent>
					<RouterLinkIcon>
						<FontAwesomeIcon
							icon={props.icon}
							size={'lg'}
							fixedWidth={true}
							style={{
								color: 'white',
								fontSize: '1.2rem',
							}}/>
					</RouterLinkIcon>
					<div className="w-100">
            <span style={{
							color: '#006fc6',
							fontWeight: 'bold',
							display: 'block'
						}}>{props.title}</span>
						<span style={{
							color: '#637381'
						}}>{props.detail}</span>
					</div>
				</RouterLinkContent>
			</Link>
		</RouterLinkContainer>
	)
}

export default RouterLink