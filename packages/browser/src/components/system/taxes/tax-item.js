import React, {Fragment} from 'react'
import {useDispatch} from 'react-redux'
import {btnEditShowTaxModal, DeleteTaxAction} from '../../../actions/taxes'
import {SwalConfirmDialog, TdBtn} from '../../common/util'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'

const TaxItem = (props) => {
	const {data} = props
	const dispatch = useDispatch()

	// Editar impuesto.
	const handleEdit = () => {
		dispatch(btnEditShowTaxModal(data))
	}

	// Borrar impuesto.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatch(DeleteTaxAction(data.id))
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.name}</td>
				<td>{data.value}</td>
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

export default TaxItem