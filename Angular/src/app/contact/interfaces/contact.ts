export class Contact {
  id: number | null | undefined;
  document: string;
  peopleDocTypeId: string;
  peopleDocType: undefined | any;
  name: string;
  address: string;
  phoneNumber1: string;
  phoneNumber2: string;
  email: string;

  constructor() {
    this.id = null;
    this.document = '';
    this.peopleDocTypeId = '';
    this.peopleDocType = undefined;
    this.name = '';
    this.address = '';
    this.phoneNumber1 = '';
    this.phoneNumber2 = '';
    this.email = '';
  }
}
