export class Contact {
  id: number | null | undefined;
  document: string;
  typeDoc: string;
  name: string;
  address: string;
  phoneNumber1: string;
  phoneNumber2: string;
  email: string;

  constructor() {
    this.id = null;
    this.document = '';
    this.typeDoc = '';
    this.name = '';
    this.address = '';
    this.phoneNumber1 = '';
    this.phoneNumber2 = '';
    this.email = '';
  }
}
