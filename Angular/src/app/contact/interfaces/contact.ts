export interface Contact {
  id: number | null | undefined;
  document: string;
  peopleDocTypeId: string;
  peopleDocType: undefined | any;
  name: string;
  address: string;
  phoneNumber: string;
  email: string;
}
