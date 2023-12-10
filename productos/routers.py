from fastapi import APIRouter, HTTPException
from productos.service import ProductService
from productos.models import Product

product_router = APIRouter()
product_service = ProductService()


@product_router.post("/products/")
async def create_product(product: Product):
    return await product_service.create_product(product)


@product_router.get("/products/{product_id}")
async def read_product(product_id: str):
    result = await product_service.get_product(product_id)
    if result:
        return result
    raise HTTPException(status_code=404, detail="Product not found")


@product_router.put("/products/{product_id}")
async def update_product(product_id: str, updated_product: Product):
    return await product_service.update_product(product_id, updated_product)


@product_router.delete("/products/{product_id}")
async def delete_product(product_id: str):
    success = await product_service.delete_product(product_id)
    if success:
        return {"status": "success", "message": "Product deleted"}
    raise HTTPException(status_code=404, detail="Product not found")


@product_router.get("/products/")
async def get_all_products():
    return await product_service.get_all_products()
