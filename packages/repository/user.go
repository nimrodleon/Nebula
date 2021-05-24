package repository

import (
	"context"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo/options"
	"golang.org/x/crypto/bcrypt"
	"sgc-server/packages/config"
	"sgc-server/packages/models"
	"time"
)

// GetUsers buscar usuarios.
func GetUsers(page int64, search string) ([]*models.User, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	var results []*models.User

	findOptions := options.Find()
	findOptions.SetSkip((page - 1) * 20)
	findOptions.SetLimit(20)

	query := bson.M{
		"full_name":  bson.M{"$regex": `(?i)` + search},
		"permission": bson.M{"$in": bson.A{"ROLE_ADMIN", "ROLE_CASH", "ROLE_USER"}},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query, findOptions)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.User
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

// GetUser retorno un usuario.
func GetUser(ID string) (models.User, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	var doc models.User
	objID, _ := primitive.ObjectIDFromHex(ID)

	filter := bson.M{
		"_id": objID,
	}

	err := col.FindOne(ctx, filter).Decode(&doc)

	if err != nil {
		return doc, err
	}
	return doc, nil
}

// GetActiveUsers retorna la lista de usuarios activos.
func GetActiveUsers(search string) ([]*models.User, bool) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	var results []*models.User

	query := bson.M{
		"suspended":  false,
		"full_name":  bson.M{"$regex": `(?i)` + search},
		"permission": bson.M{"$in": bson.A{"ROLE_ADMIN", "ROLE_CASH", "ROLE_USER"}},
		"is_deleted": false,
	}

	cursor, err := col.Find(ctx, query)
	if err != nil {
		return results, false
	}

	for cursor.Next(ctx) {
		var doc models.User
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

// AddUser agregar usuario.
func AddUser(doc models.User) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	doc.Suspended = false
	doc.IsDeleted = false

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.Hex(), true, nil
}

// CreateUser es la parada final para insertar datos de usuario.
func CreateUser(doc models.User) (string, bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	doc.Password, _ = UserPasswordEncrypt(doc.Password)

	result, err := col.InsertOne(ctx, doc)
	if err != nil {
		return "", false, err
	}
	objID, _ := result.InsertedID.(primitive.ObjectID)
	return objID.String(), true, nil
}

// UpdateUser actualizar usuario.
func UpdateUser(doc models.User, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	arrData := make(map[string]interface{})
	if len(doc.FullName) > 0 {
		arrData["full_name"] = doc.FullName
	}
	if len(doc.Address) > 0 {
		arrData["address"] = doc.Address
	}
	if len(doc.PhoneNumber) > 0 {
		arrData["phone_number"] = doc.PhoneNumber
	}
	if len(doc.Permission) > 0 {
		arrData["permission"] = doc.Permission
	}
	if len(doc.Email) > 0 {
		arrData["email"] = doc.Email
	}
	if len(doc.Avatar) > 0 {
		arrData["avatar"] = doc.Avatar
	}

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}

// UserPasswordChange Cambia la contraseña de un usuario.
func UserPasswordChange(passwordRaw string, ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	arrData := make(map[string]interface{})
	arrData["password"], _ = UserPasswordEncrypt(passwordRaw)

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}

// DeleteUser borrar usuario.
func DeleteUser(ID string) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	arrData := make(map[string]interface{})
	arrData["is_deleted"] = true

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}

// ChangeStatusUserAccount activa o suspende la cuenta de usuario
// enviar status => TRUE para suspender la cuenta y status => FALSE para activar.
func ChangeStatusUserAccount(ID string, status bool) (bool, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	arrData := make(map[string]interface{})
	arrData["suspended"] = status

	updateString := bson.M{
		"$set": arrData,
	}

	objID, _ := primitive.ObjectIDFromHex(ID)
	filter := bson.M{"_id": bson.M{"$eq": objID}}

	_, err := col.UpdateOne(ctx, filter, updateString)
	if err != nil {
		return false, err
	}
	return true, nil
}

// CheckUserExist recibe userName de parámetro y
// chequea si ya está en la base de datos.
func CheckUserExist(userName string) (models.User, bool, string) {
	ctx, cancel := context.WithTimeout(context.Background(), 15*time.Second)
	defer cancel()

	data := config.MongoConnect.Database(config.Database)
	col := data.Collection(config.UserCollection)

	condition := bson.M{"user_name": userName}

	var result models.User

	err := col.FindOne(ctx, condition).Decode(&result)
	ID := result.ID.Hex()
	if err != nil {
		return result, false, ID
	}
	return result, true, ID
}

// UserLoginIntent realiza el chequeo de login a la BD.
func UserLoginIntent(userName string, password string) (models.User, bool) {
	user, found, _ := CheckUserExist(userName)
	if found == false {
		return user, false
	}

	passwordBytes := []byte(password)
	passwordDb := []byte(user.Password)
	err := bcrypt.CompareHashAndPassword(passwordDb, passwordBytes)
	if err != nil {
		return user, false
	}

	return user, true
}

// UserPasswordEncrypt rutina para encriptar contraseñas.
func UserPasswordEncrypt(passwordRaw string) (string, error) {
	bytes, err := bcrypt.GenerateFromPassword([]byte(passwordRaw), 8)
	return string(bytes), err
}
