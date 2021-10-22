export interface Contact {
  id: number | null;
  document: string;
  typeDoc: string;
  name: string;
  address: string;
  phoneNumber1: string;
  phoneNumber2: string;
  email: string;
}

export function ContactDefaultValues(): Contact {
  return {
    id: null,
    document: '',
    typeDoc: '',
    name: '',
    address: '',
    phoneNumber1: '',
    phoneNumber2: '',
    email: ''
  };
}
