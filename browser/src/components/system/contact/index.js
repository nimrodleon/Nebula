import React, {Fragment, useEffect, useState} from 'react'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../../common/util'
import ContactModalForm from './modal-form'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddShowContactModal, LoadContactsAction} from '../../../actions/contacts'
import ContactItem from './contact-item'

/**
 * Libreta de direcciones.
 * @returns {JSX.Element}
 * @constructor
 */
const ContactList = () => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')

	useEffect(() => {
		handleLoadContacts()
		// eslint-disable-next-line 
	}, [])

	// variables de estado.
	const contacts = useSelector(state => state.contacts.contacts)

	// Acciones redux.
	// botón agregar contacto.
	const handleAddContact = () => dispatch(btnAddShowContactModal())
	const handleLoadContacts = (page = 1, search = '') => dispatch(LoadContactsAction(page, search))

	// Buscar contactos.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadContacts(1, search)
		// console.log(search)
	}

	// Render Component ContactList.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Libreta de direcciones</h4>
						<Button variant={'success'} onClick={handleAddContact}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Contacto</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
														placeholder="CONTACTO..." className="mr-2"/>
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
							<th>Tipo</th>
							<th>Documento</th>
							<th>Nombres</th>
							<th>Dirección</th>
							<th>Teléfono</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{contacts && contacts.map(data =>
							<ContactItem key={data.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<ContactModalForm/>
		</Fragment>
	)
}

export default ContactList