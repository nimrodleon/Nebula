import React, {Fragment, useEffect} from 'react'
import {useHistory} from 'react-router-dom'
import {Button, Card, Form} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faLeaf, faSignInAlt} from '@fortawesome/free-solid-svg-icons'
import styles from './login.module.scss'
import {useForm} from 'react-hook-form'
import axios from '../../axios'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import {loggedIn} from '../common/util'

const mySwal = withReactContent(Swal)

/**
 * Login del Sistema.
 * @returns {JSX.Element}
 * @constructor
 */
const Login = () => {
	let history = useHistory()
	const {register, handleSubmit} = useForm()

	useEffect(() => {
		if (loggedIn()) {
			history.push('/')
		}
	})

	// Submit del formulario Login.
	const handleLogin = (f) => {
		axios.post('auth/login', f).then(({data}) => {
			localStorage.setItem('token', data.token)
			window.location.reload()
		}).catch(err => {
			mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Usuario y/o Contraseña Incorrecta!'
			}).then(() => {
				console.log(err.response.data)
			})
		})
	}

	// Render Component.
	return (
		<Fragment>
			<div className="d-flex align-items-center justify-content-center"
					 style={{height: '100vh'}}>
				<div className={styles.CardContainer}>
					<Card className={styles.BackgroundCard}>
						<Card.Body>
							<Card.Title>
								<h4 className="text-center">
                  <span className="text-success">
                    <FontAwesomeIcon icon={faLeaf}/>
                  </span>
									<span className="ml-1">SGT-Pre-alpha1</span>
								</h4>
							</Card.Title>
							<Form onSubmit={handleSubmit(handleLogin)}>
								<Form.Group>
									<Form.Label>
										<span className="text-uppercase">Nombre Usuario</span>
									</Form.Label>
									<Form.Control type="text" {...register('user_name', {required: true})}
																size={'lg'} placeholder="Username"/>
								</Form.Group>
								<Form.Group>
									<Form.Label>
										<span className="text-uppercase">Contraseña</span>
									</Form.Label>
									<Form.Control type="password" {...register('password', {required: true})}
																size={'lg'} placeholder="**********"/>
								</Form.Group>
								<Form.Group>
									<Button type={'submit'} variant={'success'} size={'lg'} block>
										<FontAwesomeIcon icon={faSignInAlt} size={'lg'} fixedWidth={true}/>
										<span className="text-uppercase ml-1">Acceso al Sistema</span>
									</Button>
								</Form.Group>
							</Form>
							<Card.Text className="text-center">
								elaborado por <a href="https://twitter.com/nleonc14"
																 target="_blank" rel="noreferrer" className="text-decoration-none">@nleonc14</a>
							</Card.Text>
						</Card.Body>
					</Card>
				</div>
			</div>
		</Fragment>
	)
}

export default Login