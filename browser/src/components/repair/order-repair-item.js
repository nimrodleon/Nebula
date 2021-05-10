import React, {Fragment} from 'react'
import {useHistory} from 'react-router-dom'
import {useDispatch} from 'react-redux'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'
import {SwalConfirmDialog, TdBtn} from '../common/util'
import {DeleteOrderRepairAction} from '../../actions/order_repair'

/**
 * Item orden de reparación.
 * @param props
 * @returns {JSX.Element}
 * @constructor
 */
const OrderRepairItem = ({data}) => {
	let history = useHistory()
	const dispatch = useDispatch()

	// Editar Orden de reparación.
	const handleEdit = () => {
		history.push(`/repair/form/${data.id}`)
	}

	// Borrar Orden de reparación.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatch(DeleteOrderRepairAction(data.id))
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.reception_date}</td>
				<td>{data.client_id}</td>
				<td>{data.device_info}</td>
				<td>{data.status}</td>
				<td>{data.promised_date}, {data.promised_time}</td>
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

export default OrderRepairItem