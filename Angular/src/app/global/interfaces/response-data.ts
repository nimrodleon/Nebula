export interface ResponseData<Type> {
  ok: boolean;
  msg: string | null;
  data: Type;
}
