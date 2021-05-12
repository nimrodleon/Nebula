import React, {Fragment} from 'react'
import {useHistory} from 'react-router-dom'
import {useDispatch} from 'react-redux'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'
import {SwalConfirmDialog, TdBtn} from '../common/util'
import {DeleteOrderRepairAction, LoadOrderRepairsAction} from '../../actions/order_repair'

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
		history.push(`/repair/form/${data.OrderRepair.id}`)
	}

	// Borrar Orden de reparación.
	const handleDelete = () => {
		SwalConfirmDialog().then(async (result) => {
			if (result.isConfirmed) {
				await dispatch(DeleteOrderRepairAction(data.OrderRepair.id))
				dispatch(LoadOrderRepairsAction(1, ''))
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.OrderRepair.reception_date}</td>
				<td><a href="#">{data.full_name}</a></td>
				<td>{data.OrderRepair.device_info}</td>
				<td>{data.OrderRepair.status}</td>
				<td>{data.OrderRepair.promised_date}, {data.OrderRepair.promised_time}</td>
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