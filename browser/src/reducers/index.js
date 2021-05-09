import {combineReducers} from 'redux'
import taxesReducer from './taxes'
import contactsReducer from './contacts'
import articlesReducer from './articles'
import warehousesReducer from './warehouses'
import deviceTypesReducer from './device_types'
import usersReducer from './users'
import orderRepairReducer from './order_repair'

export default combineReducers({
	contacts: contactsReducer,
	taxes: taxesReducer,
	articles: articlesReducer,
	warehouses: warehousesReducer,
	deviceTypes: deviceTypesReducer,
	users: usersReducer,
	orderRepair: orderRepairReducer
})
