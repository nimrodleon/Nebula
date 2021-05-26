import React, {Fragment, useEffect, useState} from 'react'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../../common/util'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddShowUserModal, LoadUsersAction} from '../../../actions/users'
import UserItem from './user-item'
import UserModalForm from './modal-form'

/**
 * Lista de Usuarios.
 * @returns {JSX.Element}
 * @constructor
 */
const UserList = () => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')

	useEffect(() => {
		handleLoadUsers()
		// eslint-disable-next-line
	}, [])

	// Variables de estado.
	const users = useSelector(state => state.users.users)

	// Acciones redux.
	const handleAddUser = () => dispatch(btnAddShowUserModal())
	const handleLoadUsers = (page = 1, search = '') => dispatch(LoadUsersAction(page, search))

	// Buscar usuarios.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadUsers(1, search)
	}

	// Render Component.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Lista de Usuarios</h4>
						<Button variant={'success'} onClick={handleAddUser}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Usuario</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
														placeholder="USERNAME..." className="mr-2"/>
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
							<th>Nombres</th>
							<th>Username</th>
							<th>Permiso</th>
							<th></th>
							<th>Email</th>
							<th>Teléfono</th>
							<th>Estado</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{users && users.map(data => <UserItem key={data.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<UserModalForm/>
		</Fragment>
	)
}

export default UserList