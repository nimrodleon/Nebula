import React, {Fragment} from 'react'
import {useDispatch} from 'react-redux'
import {btnEditShowDeviceTypeModal, DeleteDeviceTypeAction} from '../../../actions/device_types'
import {SwalConfirmDialog, TdBtn} from '../../common/util'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'

/**
 * Item tipos de equipo.
 * @param props
 * @returns {JSX.Element}
 * @constructor
 */
const DeviceTypeItem = (props) => {
	const {data} = props
	const dispatch = useDispatch()

	// Editar tipo de equipo.
	const handleEdit = () => {
		dispatch(btnEditShowDeviceTypeModal(data))
	}

	//Borrar tipo de equipo.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatch(DeleteDeviceTypeAction(data.id))
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
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

export default DeviceTypeItem