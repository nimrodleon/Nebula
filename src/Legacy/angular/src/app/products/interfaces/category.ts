export class Category {
  constructor(
    public id: string | any = null,
    public companyId: string = "",
    public name: string = "") {
  }
}

export class CategoryDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public category: Category = new Category()) {
  }
}
