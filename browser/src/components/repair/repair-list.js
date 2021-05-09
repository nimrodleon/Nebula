import React, {Fragment} from 'react'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../common/util'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'

/**
 * Lista de reparaciones.
 * @returns {JSX.Element}
 * @constructor
 */
const RepairList = ({history}) => {

	// Agregar orden de reparación.
	const handleAddOrderRepair = () => {
		history.push('/repair/form')
	}

	// Render Component.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Equipos en Taller</h4>
						<Button variant={'success'} onClick={handleAddOrderRepair}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Orden Reparación</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														placeholder="CLIENTES..." className="mr-2"/>
							<Button variant={'dark'} type={'submit'}>
								<FontAwesomeIcon icon={faSearch}/>
								<span className="text-uppercase ml-1">Buscar</span>
							</Button>
						</Form>
					</WrapHeaderItem>
				</WrapHeader>
				<WrapBody>
					<Table striped={true} size={'sm'} className="mb-0">
						<thead className="thead-light">
						<tr className="text-uppercase">
							<th>Orden</th>
							<th>Fecha</th>
							<th>Cliente</th>
							<th>Tipo de Equipo</th>
							<th>Serie</th>
							<th>Prometido</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
		</Fragment>
	)
}

export default RepairList