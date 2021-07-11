import {ContactStore} from './store'

// Lógica - contactos.
export class ContactController {
  // Listar contactos.
  static getContacts(query) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ContactStore.getContacts(query))
      } catch (err) {
        reject(err)
      }
    })
  }

  // obtener contacto por id.
  static getContact(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ContactStore.getContact(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // registrar contacto.
  static addContact(data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ContactStore.addContact(data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // actualizar contacto.
  static updateContact(id, data) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ContactStore.updateContact(id, data))
      } catch (err) {
        reject(err)
      }
    })
  }

  // borrar contacto.
  static deleteContact(id) {
    return new Promise((resolve, reject) => {
      try {
        resolve(ContactStore.deleteContact(id))
      } catch (err) {
        reject(err)
      }
    })
  }

  // listar contactos para select2.
  static getContactsWithSelect2(term) {
    return new Promise(async (resolve, reject) => {
      try {
        let data = {results: []}
        let _contacts = await ContactStore.getContacts(term)
        await _.forEach(_contacts, value => {
          data.results.push({id: value._id, text: value.fullName})
        })
        reject(data)
      } catch (err) {
        reject(err)
      }
    })
  }
}
