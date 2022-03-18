export class ResponseData<Type> {
  constructor(
    public ok: boolean = false,
    public data: Type | undefined = undefined,
    public msg: string = '') {
  }
}
