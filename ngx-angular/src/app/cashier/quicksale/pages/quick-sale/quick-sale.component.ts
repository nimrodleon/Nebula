import { Component, Injector, OnInit } from "@angular/core";
import {
  faCircleLeft, faCircleUser, faCircleXmark, faCoins,
  faMagnifyingGlass, faTrashCan
} from "@fortawesome/free-solid-svg-icons";
import { ActivatedRoute, Router } from "@angular/router";
import {
  confirmExit,
  confirmTask,
  initializeSelect2Injector,
  isValidObjectId,
  toastError,
  toastSuccess
} from "app/common/interfaces";
import { QuickSaleConfig } from "app/cashier/interfaces";
import { CajaDiariaService } from "app/cashier/services";
import { Product } from "app/products/interfaces";
import { ProductService } from "app/products/services";
import { ComprobanteDto, ResponseCobrarModal } from "../../interfaces";
import { Contact } from "app/contact/interfaces";
import { FormsModule } from "@angular/forms";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { CashierContainerComponent } from "app/common/containers/cashier-container/cashier-container.component";
import { CurrencyPipe, DecimalPipe, NgForOf } from "@angular/common";
import { CobrarModalComponent } from "../../components/cobrar-modal/cobrar-modal.component";
import {
  SearchContactModalComponent
} from "app/common/contact/search-contact-modal/search-contact-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-quick-sale",
  standalone: true,
  imports: [
    FormsModule,
    FaIconComponent,
    CashierContainerComponent,
    CurrencyPipe,
    NgForOf,
    DecimalPipe,
    CobrarModalComponent,
    SearchContactModalComponent
  ],
  templateUrl: "./quick-sale.component.html"
})
export class QuickSaleComponent implements OnInit {
  faCircleLeft = faCircleLeft;
  faMagnifyingGlass = faMagnifyingGlass;
  faCircleUser = faCircleUser;
  faCircleXmark = faCircleXmark;
  faCoins = faCoins;
  faTrashCan = faTrashCan;
  // ====================================================================================================
  companyId: string = "";
  query: string = "";
  productos: Product[] = new Array<Product>();
  quickSaleConfig: QuickSaleConfig = new QuickSaleConfig();
  comprobanteDto: ComprobanteDto = new ComprobanteDto();
  subTotal: number = 0;
  mtoIgv: number = 0;
  totalCobrar: number = 0;
  // ====================================================================================================
  searchContactModal: any;
  cobrarModal: any;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private injector: Injector,
    private cajaDiariaService: CajaDiariaService,
    private productService: ProductService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      const id: string = params.get("id") || "";
      this.cajaDiariaService.getQuickSaleConfig(id)
        .subscribe(result => {
          this.quickSaleConfig = result;
          this.configurarCabecera(result);
        });
    });
    this.cargarProductos();
    this.searchContactModal = new bootstrap.Modal("#searchContactModal");
    this.cobrarModal = new bootstrap.Modal("#cobrarModal");
  }

  private configurarCabecera(config: QuickSaleConfig): void {
    this.comprobanteDto.cabecera.cajaDiariaId = config.cajaDiaria.id;
    this.comprobanteDto.cabecera.invoiceSerieId = config.cajaDiaria.invoiceSerieId;
    this.comprobanteDto.setCliente(config.contact);
  }

  public cargarProductos(): void {
    this.productService.lista(this.query)
      .subscribe(result => this.productos = result);
  }

  public salir(): void {
    if (this.comprobanteDto.detalle.length <= 0) {
      this.router.navigate([
        "/", this.companyId, "cashier", "detail", this.quickSaleConfig.cajaDiaria.id
      ]).then(() => console.log("salir"));
    } else {
      confirmExit().then(result => {
        if (result.isConfirmed) {
          this.router.navigate([
            "/", this.companyId, "cashier", "detail", this.quickSaleConfig.cajaDiaria.id
          ]).then(() => console.log("salir"));
        }
      });
    }
  }

  public calcularTotales(): void {
    this.subTotal = this.comprobanteDto.getValorVenta(this.quickSaleConfig.company);
    this.mtoIgv = this.comprobanteDto.getMtoIgv(this.quickSaleConfig.company);
    this.totalCobrar = this.comprobanteDto.getTotalCobrar();
  }

  public agregarProducto(product: Product): void {
    this.comprobanteDto.agregarDetalle(product, this.quickSaleConfig.cajaDiaria.warehouseId);
    this.calcularTotales();
  }

  public borrarItemDetalle(event: Event, productId: string): void {
    event.preventDefault();
    this.comprobanteDto.borrarItemDetalle(productId);
    this.calcularTotales();
  }

  public buscarCliente(event: Event): void {
    event.preventDefault();
    this.searchContactModal.show();
  }

  public seleccionarContacto(contact: Contact): void {
    this.comprobanteDto.setCliente(contact);
    this.searchContactModal.hide();
  }

  public cobrarPago(): void {
    if (this.comprobanteDto.detalle.length <= 0) {
      toastError("Debe existir al menos un Item para facturar!");
    } else {
      this.cobrarModal.show();
    }
  }

  public cancelarVenta(): void {
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.comprobanteDto = new ComprobanteDto();
        this.configurarCabecera(this.quickSaleConfig);
      }
    });
  }

  public imprimirTicket(data: ResponseCobrarModal): void {
    this.cobrarModal.hide();
    if (isValidObjectId(data.invoiceSaleId)) {
      this.comprobanteDto = new ComprobanteDto();
      this.configurarCabecera(this.quickSaleConfig);
      this.calcularTotales();
      if (data.imprimir) {
        this.router.navigate([
          "/", this.quickSaleConfig.company.id, "cashier", "ticket", data.invoiceSaleId
        ]).then(() => console.log(data.billingResponse.hash));
      } else {
        if (data.billingResponse?.success)
          toastSuccess(data.billingResponse.cdrDescription);
        else
          toastSuccess("La nota de venta ha sido registrado!");
      }
    }
  }

}
