export interface PeopleDocType {
  id: string | null;
  description: string;
  sunatCode: string;
}

export function PeopleDocTypeDefaultValues(): PeopleDocType {
  return {
    id: null,
    description: '',
    sunatCode: ''
  };
}
