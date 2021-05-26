import React, {Fragment, useEffect, useState} from 'react'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../../common/util'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddShowTaxModal, LoadTaxesAction} from '../../../actions/taxes'
import TaxItem from './tax-item'
import TaxModalForm from './modal-form'

/**
 * Lista de Impuestos.
 * @returns {JSX.Element}
 * @constructor
 */
const TaxesList = () => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')

	useEffect(() => {
		handleLoadTaxes()
		// eslint-disable-next-line
	}, [])

	// Variables de estado.
	const taxes = useSelector(state => state.taxes.taxes)

	// Acciones redux.
	const handleAddTax = () => dispatch(btnAddShowTaxModal())
	const handleLoadTaxes = (page = 1, search = '') => dispatch(LoadTaxesAction(page, search))

	// Formulario buscar impuestos.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadTaxes(1, search)
	}

	// Render Component.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Lista de Impuestos</h4>
						<Button variant={'success'} onClick={handleAddTax}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Impuesto</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
														placeholder="IMPUESTO..." className="mr-2"/>
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
							<th>Nombre</th>
							<th>Valor</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{taxes && taxes.map(data => <TaxItem key={data.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<TaxModalForm/>
		</Fragment>
	)
}

export default TaxesList