from pydantic import BaseModel
from typing import Optional


class Product(BaseModel):
    id: Optional[str] = None
    company_id: str = ""
    description: str = ""
    category: str = ""
    barcode: str = "-"
    igv_sunat: str = "Gravado"
    precio_venta_unitario: float
    type: str = ""
    und_medida: str = ""
