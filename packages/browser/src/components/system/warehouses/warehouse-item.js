import React, {Fragment} from 'react'
import {useDispatch} from 'react-redux'
import {btnEditShowWarehouseModal, DeleteWarehouseAction} from '../../../actions/warehouses'
import {SwalConfirmDialog, TdBtn} from '../../common/util'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'

/**
 * Item accesorio.
 * @param props
 * @returns {JSX.Element}
 * @constructor
 */
const WarehouseItem = (props) => {
	const {data} = props
	const dispatch = useDispatch()

	// Editar accesorio.
	const handleEdit = () => {
		dispatch(btnEditShowWarehouseModal(data))
	}

	// Borrar accesorio.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatch(DeleteWarehouseAction(data.id))
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.type}</td>
				<td>{data.name}</td>
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

export default WarehouseItem