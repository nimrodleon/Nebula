import React, {Fragment} from 'react'
import {SwalConfirmDialog, TdBtn} from '../../common/util'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'
import {useDispatch} from 'react-redux'
import {btnEditShowContactModal, DeleteContactAction} from '../../../actions/contacts'

const ContactItem = (props) => {
	const {data} = props
	const dispatch = useDispatch()

	// Acciones del redux.
	const dispatchEditContactModal = (contact) => dispatch(btnEditShowContactModal(contact))
	const dispatchDeleteContactModal = (id) => dispatch(DeleteContactAction(id))

	// botón editar contacto.
	const handleEdit = () => {
		dispatchEditContactModal(data)
	}

	// botón borrar contacto.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatchDeleteContactModal(data.id)
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.type_doc}</td>
				<td>{data.document}</td>
				<td>{data.full_name}</td>
				<td>{data.address}</td>
				<td>{data.phone_number}</td>
				<TdBtn>
					<Button variant={'primary'} onClick={handleEdit} className="mr-2" size={'sm'}>
						<FontAwesomeIcon icon={faEdit}/>
					</Button>
					<Button variant={'danger'} onClick={handleDelete} size={'sm'}>
						<FontAwesomeIcon icon={faTrashAlt}/>
					</Button>
				</TdBtn>
			</tr>
		</Fragment>
	)
}

export default ContactItem