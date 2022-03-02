export class Contact {
  constructor(
    public id: any = undefined,
    public document: string = '',
    public docType: string = '0:SIN DEFINIR',
    public name: string = '',
    public address: string = '',
    public phoneNumber: string = '',
    public email: string = '',) {
  }
}
