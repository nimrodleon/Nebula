import {Contact} from './model'

// CRUD - contactos.
export class ContactStore {
  // Listar contactos.
  static async getContacts(query) {
    return Contact.find({fullName: {$regex: query}, isDeleted: false})
  }

  // obtener contacto por id.
  static async getContact(id) {
    return Contact.findById(id)
  }

  // registrar contacto.
  static async addContact(data) {
    let _contact = new Contact(data)
    _contact.isDeleted = false
    await _contact.save()
    return _contact
  }

  // actualizar contacto.
  static async updateContact(id, data) {
    return Contact.findByIdAndUpdate(id, data, {new: true})
  }

  // borrar contacto.
  static async deleteContact(id) {
    let _contact = await this.getContact(id)
    _contact.isDeleted = true
    return this.updateContact(id, _contact)
  }
}
