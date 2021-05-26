import React, {Fragment, useEffect, useState} from 'react'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../../common/util'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddShowDeviceTypeModal, LoadDeviceTypesAction} from '../../../actions/device_types'
import DeviceTypeItem from './device-type-item'
import DeviceTypeModalForm from './modal-form'

/**
 * Lista de Tipo de equipos.
 * @returns {JSX.Element}
 * @constructor
 */
const DeviceTypeList = () => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')

	useEffect(() => {
		handleLoadDeviceTypes()
		// eslint-disable-next-line
	}, [])

	// Variables de estado.
	const deviceTypes = useSelector(state => state.deviceTypes.deviceTypes)

	// Acciones redux.
	const handleAddDeviceType = () => dispatch(btnAddShowDeviceTypeModal())
	const handleLoadDeviceTypes = (page = 1, search = '') => dispatch(LoadDeviceTypesAction(page, search))

	// Formulario de búsqueda.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadDeviceTypes(1, search)
	}

	// Render Component.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Lista Tipo de Equipos</h4>
						<Button variant={'success'} onClick={handleAddDeviceType}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Tipo de Equipo</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
														placeholder="TIPO DE EQUIPO..." className="mr-2"/>
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
							<th>Nombre</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{deviceTypes && deviceTypes.map(data => <DeviceTypeItem key={data.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<DeviceTypeModalForm/>
		</Fragment>
	)
}

export default DeviceTypeList