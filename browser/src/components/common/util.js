import React, {Fragment} from 'react'
import styled from 'styled-components'
import Swal from 'sweetalert2'

// Comprueba la existencia de token.
export const loggedIn = () => {
	return !!localStorage.getItem('token')
}

// Wrap contenedor principal.
export const Wrap = styled.div`
	display: flex;
	min-height: calc(100vh - 56px);
	background-color: #e5e7eb;
`

// WrapBody contenedor de la data.
export const WrapContainer = styled.div`
	display: flex;
	flex-direction: column;
	width: 100%;
`

// WrapHeader contenedor header de la aplicación.
export const WrapHeader = (props) => {
	return (
		<Fragment>
			<div className="bg-white p-3 m-2">
				<div className="d-flex justify-content-between">{props.children}</div>
			</div>
		</Fragment>
	)
}

// WrapHeaderItem item del WrapHeader.
export const WrapHeaderItem = styled.div`
	display: flex;
	align-items: center;
	width: 50%;
`

// WrapBody contenedor body.
export const WrapBody = styled.div`
	background-color: white;
	padding: 8px;
	margin: 0 8px;
`

// Td para botones de tablas.
export const TdBtn = styled.td`
	min-width: 82px;
	width: 82px;`

// Swal para confirmar preguntas.
export const SwalConfirmDialog = () => {
	return Swal.fire({
		title: '¿Estás seguro?',
		text: '¡No podrás revertir esto!',
		icon: 'warning',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Sí, confirmar!',
		cancelButtonText: 'Cancelar'
	})
}