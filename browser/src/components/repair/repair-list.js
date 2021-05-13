import React, {Fragment, useEffect, useState} from 'react'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../common/util'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddOrderRepair, LoadOrderRepairsAction} from '../../actions/order_repair'
import OrderRepairItem from './order-repair-item'
import OrderRepairDetail from './order-repair-detail'

/**
 * Lista de reparaciones.
 * @returns {JSX.Element}
 * @constructor
 */
const RepairList = ({history}) => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')

	useEffect(() => {
		handleLoadOrderRepairs()
		// eslint-disable-next-line
	}, [])

	// Variables de estado.
	const orderRepairs = useSelector(state => state.orderRepair.orderRepairs)

	// Acciones redux.
	const handleLoadOrderRepairs = (page = 1, search = '') => dispatch(LoadOrderRepairsAction(page, search))

	// Agregar orden de reparación.
	const handleAddOrderRepair = async () => {
		await dispatch(btnAddOrderRepair())
		history.push('/repair/form')
	}

	// Formulario de Búsqueda.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadOrderRepairs(1, search)
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
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
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
							<th>Fecha</th>
							<th>Cliente</th>
							<th>Información Equipo</th>
							<th>Estado</th>
							<th>Prometido</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{orderRepairs && orderRepairs.map(data => <OrderRepairItem key={data.OrderRepair.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<OrderRepairDetail/>
		</Fragment>
	)
}

export default RepairList