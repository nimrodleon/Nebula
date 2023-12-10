from fastapi import FastAPI
from productos.routers import product_router

app = FastAPI()
app.include_router(product_router, prefix="/productos", tags=["productos"])
