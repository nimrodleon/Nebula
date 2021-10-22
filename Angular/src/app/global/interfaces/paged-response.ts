export interface PagedResponse<Type> {
  pageNumber: number;
  pageSize: number;
  firstPage: string | null;
  lastPage: string | null;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: Array<Type>;
}
