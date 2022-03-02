export class PagedResponse<Type> {
  constructor() {
    this.pageNumber = 0;
    this.pageSize = 0;
    this.firstPage = null;
    this.lastPage = null;
    this.totalPages = 0;
    this.totalRecords = 0;
    this.nextPage = null;
    this.previousPage = null;
    this.data = [];
  }

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
