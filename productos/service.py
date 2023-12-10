from bson import ObjectId
from pymongo.collection import Collection
from common.database import Database
from productos.models import Product


class ProductService:
    def __init__(self):
        self.collection: Collection = Database.get_database()["Product"]

    async def create_product(self, product: Product):
        product_data = product.dict()
        result = await self.collection.insert_one(product_data)
        return str(result.inserted_id)

    async def get_product(self, product_id: str):
        product_data = await self.collection.find_one({"_id": ObjectId(product_id)})
        return product_data

    async def update_product(self, product_id: str, updated_product: Product):
        await self.collection.update_one(
            {"_id": ObjectId(product_id)},
            {"$set": updated_product.dict(exclude_unset=True)}
        )
        return True

    async def delete_product(self, product_id: str):
        result = await self.collection.delete_one({"_id": ObjectId(product_id)})
        return result.deleted_count > 0

    async def get_all_products(self):
        products_cursor = self.collection.find()
        products = [product async for product in products_cursor]
        return products
