import {Injectable} from "@angular/core";
import {MaterialService} from "./material.service";
import {accessDenied, deleteConfirm, deleteError} from "app/common/interfaces";
import {MaterialDetailService} from "./material-detail.service";
import {AjusteInventario, InventoryNotas, Material, Transferencia} from "../interfaces";
import {InventoryNotasService} from "./inventory-notas.service";
import {InventoryNotasDetailService} from "./inventory-notas-detail.service";
import {TransferenciaService} from "./transferencia.service";
import {TransferenciaDetailService} from "./transferencia-detail.service";
import {AjusteInventarioService} from "./ajuste-inventario.service";
import {AjusteInventarioDetailService} from "./ajuste-inventario-detail.service";
import {UserDataService} from "app/common/user-data.service";
import _ from "lodash";

@Injectable({
  providedIn: "root"
})
export class InventoryService {
  private _materials: Array<Material> = new Array<Material>();
  private _inventoryNotas: Array<InventoryNotas> = new Array<InventoryNotas>();
  private _transferencias: Array<Transferencia> = new Array<Transferencia>();
  private _ajusteInventarios: Array<AjusteInventario> = new Array<AjusteInventario>();

  constructor(
    private userDataService: UserDataService,
    private inventoryNotasService: InventoryNotasService,
    private inventoryNotasDetailService: InventoryNotasDetailService,
    private materialService: MaterialService,
    private materialDetailService: MaterialDetailService,
    private transferenciaService: TransferenciaService,
    private transferenciaDetailService: TransferenciaDetailService,
    private ajusteInventarioService: AjusteInventarioService,
    private ajusteInventarioDetailService: AjusteInventarioDetailService) {
  }

  public get ajusteInventarios(): Array<AjusteInventario> {
    return this._ajusteInventarios;
  }

  public getAjusteInventarios(year: string, month: string): void {
    this.ajusteInventarioService.index(year, month)
      .subscribe(result => this._ajusteInventarios = result);
  }

  public deleteAjusteInventario(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.ajusteInventarioDetailService.countDocuments(id)
            .subscribe(totalDocuments => {
              if (totalDocuments > 0) {
                deleteError().then(() => console.log(totalDocuments));
              } else {
                this.ajusteInventarioService.delete(id).subscribe(result => {
                  this._ajusteInventarios = _.filter(this._ajusteInventarios, (o: AjusteInventario) => o.id !== result.id);
                });
              }
            });
        }
      });
    }
  }

  public get transferencias(): Array<Transferencia> {
    return this._transferencias;
  }

  public getTransferencias(year: string, month: string): void {
    this.transferenciaService.index(year, month)
      .subscribe(result => this._transferencias = result);
  }

  public deleteTransferencia(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.transferenciaDetailService.countDocuments(id)
            .subscribe(totalDocuments => {
              if (totalDocuments > 0) {
                deleteError().then(() => console.log(totalDocuments));
              } else {
                this.transferenciaService.delete(id).subscribe(result => {
                  this._transferencias = _.filter(this._transferencias, (o: Transferencia) => o.id !== result.id);
                });
              }
            });
        }
      });
    }
  }

  public get inventoryNotas(): Array<InventoryNotas> {
    return this._inventoryNotas;
  }

  public getInventoryNotas(year: string, month: string): void {
    this.inventoryNotasService.index(year, month)
      .subscribe(result => this._inventoryNotas = result);
  }

  public deleteInventoryNotas(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.inventoryNotasDetailService.countDocuments(id)
            .subscribe(totalDocuments => {
              if (totalDocuments > 0) {
                deleteError().then(() => console.log(totalDocuments));
              } else {
                this.inventoryNotasService.delete(id).subscribe(result => {
                  this._inventoryNotas = _.filter(this._inventoryNotas, (o: InventoryNotas) => o.id !== result.id);
                });
              }
            });
        }
      });
    }
  }

  public get materials(): Array<Material> {
    return this._materials;
  }

  public getMaterials(year: string, month: string): void {
    this.materialService.index(year, month)
      .subscribe(result => this._materials = result);
  }

  public deleteMaterial(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.materialDetailService.countDocuments(id)
            .subscribe(totalDocuments => {
              if (totalDocuments > 0) {
                deleteError().then(() => console.log(totalDocuments));
              } else {
                this.materialService.delete(id).subscribe(result => {
                  this._materials = _.filter(this._materials, (o: Material) => o.id !== result.id);
                });
              }
            });
        }
      });
    }
  }

}
