export class ResponseData<Type> {
  constructor() {
    this.ok = false;
    this.data = undefined;
    this.msg = null;
  }

  ok: boolean;
  msg: string | null;
  data: Type | undefined;
}
