import React, {Fragment} from 'react'
import {useHistory} from 'react-router-dom'
import {useDispatch} from 'react-redux'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'
import {SwalConfirmDialog, TdBtn} from '../common/util'
import {DeleteOrderRepairAction, LoadOrderRepairsAction} from '../../actions/order_repair'
import moment from 'moment-timezone'

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

	// Detalle Orden de reparación.
	const handleDetail = (e) => {
		e.preventDefault()
		console.log(data)
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>
					{/*{data.OrderRepair.reception_date}*/}
					{moment(data.OrderRepair.created_at).tz('America/Lima').fromNow(true)}
				</td>
				<td>
					<a href={`!#${data.full_name}`} onClick={handleDetail}>{data.full_name}</a>
				</td>
				<td>{data.OrderRepair.device_info}</td>
				<td>{data.OrderRepair.status}</td>
				<td>{`${moment(data.OrderRepair.promised_date).format('YYYY-MM-DD')}, ${data.OrderRepair.promised_time}`}</td>
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