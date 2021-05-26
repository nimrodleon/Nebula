import React, {Fragment} from 'react'
import {useDispatch} from 'react-redux'
import {btnEditShowArticleModal, DeleteArticleAction} from '../../actions/articles'
import {SwalConfirmDialog, TdBtn} from '../common/util'
import {Button} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons'

/**
 * Item individual del artículo.
 * @returns {JSX.Element}
 * @constructor
 */
const ArticleItem = (props) => {
	const {data} = props
	const dispatch = useDispatch()

	// Editar artículo.
	const handleEdit = () => {
		dispatch(btnEditShowArticleModal(data))
	}

	// Borrar artículo.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatch(DeleteArticleAction(data.id))
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.type}</td>
				<td>{data.bar_code}</td>
				<td>{data.name}</td>
				<td>{data.price_1}</td>
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

export default ArticleItem