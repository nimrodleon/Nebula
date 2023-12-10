from motor.motor_asyncio import AsyncIOMotorClient
from pymongo.database import Database as PyMongoDatabase


class Database:
    _client: AsyncIOMotorClient = None
    DEFAULT_DATABASE_NAME = "Nebula"

    @classmethod
    def get_client(cls) -> AsyncIOMotorClient:
        if cls._client is None:
            cls._client = AsyncIOMotorClient("mongodb://localhost:27017")
        return cls._client

    @classmethod
    def get_database(cls, db_name: str = None) -> PyMongoDatabase:
        if db_name is None:
            db_name = cls.DEFAULT_DATABASE_NAME
        return cls.get_client()[db_name]
