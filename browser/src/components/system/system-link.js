import React, {Fragment} from 'react'
import {Col, Row} from 'react-bootstrap'
import RouterLink from '../common/router-link'
import {faAddressBook, faCog, faCoins, faLaptop, faUsers, faWarehouse} from '@fortawesome/free-solid-svg-icons'
import {useRouteMatch} from 'react-router-dom'

const SystemLink = () => {
	let match = useRouteMatch()

	return (
		<Fragment>
			<div className="bg-white w-100 m-2">
				<div className="p-4">
					<h4 className="text-uppercase">Ajustes del Sistema</h4>
					<div className="border rounded p-4">
						<Row>
							<Col md={4}>
								<RouterLink
									href={`${match.url}/general`} icon={faCog} title="General"
									detail="Ajustes Generales"/>
							</Col>
							<Col md={4}>
								<RouterLink
									href={`${match.url}/users`} icon={faUsers} title="Usuarios"
									detail="Lista de usuarios"/>
							</Col>
							<Col md={4}>
								<RouterLink
									href={`${match.url}/taxes`} icon={faCoins} title="Impuesto"
									detail="Lista de Impuestos"/>
							</Col>
							<Col md={4}>
								<RouterLink
									href={`${match.url}/contacts`} icon={faAddressBook} title="Contactos"
									detail="Libreta de direcciones"/>
							</Col>
							<Col md={4}>
								<RouterLink
									href={`${match.url}/devices-type`} icon={faLaptop} title="Tipo de Equipos"
									detail="Lista Tipo de Equipos"/>
							</Col>
							<Col md={4}>
								<RouterLink
									href={`${match.url}/warehouses`} icon={faWarehouse} title="Almacenes"
									detail="Lista de Almacenes"/>
							</Col>
						</Row>
					</div>
				</div>
			</div>
		</Fragment>
	)
}

export default SystemLink