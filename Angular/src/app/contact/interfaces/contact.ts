export class Contact {
  constructor(
    public id: number | any = null,
    public document: string = '',
    public docType: number = 0,
    public name: string = '',
    public address: string = '',
    public phoneNumber: string = '',
    public email: string = '',) {
  }
}
