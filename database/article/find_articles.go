package article

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo/options"
	"sgc-server/database/db"
	"sgc-server/models"
	"time"
)

// FindArticles buscar artículos.
func FindArticles(page int64, search string) ([]*models.Article, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := db.MongoConnect.Database(db.Database)
	col := data.Collection(db.ArticleCollection)

	var results []*models.Article

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"is_deleted": false,
		"$or": []interface{}{
			bson.M{"name": bson.M{"$regex": `(?i)` + search}},
			bson.M{"bar_code": bson.M{"$regex": `(?i)` + search}},
		},
		// "name": bson.M{"$regex": `(?i)` + search},
	}

	cursor, err := col.Find(ctx, query, findOptions)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.Article
		err := cursor.Decode(&doc)
		if err != nil {
			return results, false
		}
		results = append(results, &doc)
	}

	err = cursor.Err()
	if err != nil {
		return results, false
	}
	_ = cursor.Close(ctx)
	return results, true
}
