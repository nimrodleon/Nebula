import java.io.BufferedReader
import java.io.File
import java.io.FileInputStream
import java.io.InputStreamReader
import java.sql.DriverManager

fun main(args: Array<String>) {
    // Llamada a la función para obtener la ruta del archivo
    val rutaArchivo = obtenerRutaArchivo("padron_reducido_ruc.txt")
    // Llamada a la función para leer el archivo CSV
    leerArchivoCSV(rutaArchivo)
}

fun obtenerRutaArchivo(nombreArchivo: String): String {
    val rutaBase = File("").absolutePath
    return "$rutaBase/$nombreArchivo"
}

fun leerArchivoCSV(archivo: String) {
    val fileInputStream = FileInputStream(archivo)
    val inputStreamReader = InputStreamReader(fileInputStream, Charsets.UTF_8)
    val bufferedReader = BufferedReader(inputStreamReader)

    // ruta de la base de datos.
    val dataBasePath = obtenerRutaArchivo("padron_reducido_ruc.db")

    // Conexión a la base de datos SQLite
    val connection = DriverManager.getConnection("jdbc:sqlite:$dataBasePath")

    // Crea la tabla en la base de datos
    val createTableQuery = """
        CREATE TABLE IF NOT EXISTS contribuyentes (
            ruc TEXT,
            dni TEXT,
            nombre TEXT,
            estado TEXT,
            condicion_domicilio TEXT,
            ubigeo TEXT,
            tipo_via TEXT,
            nombre_via TEXT,
            codigo_zona TEXT,
            tipo_zona TEXT,
            numero TEXT,
            interior TEXT,
            lote TEXT,
            departamento TEXT,
            manzana TEXT,
            kilometro TEXT
        )
    """

    connection.createStatement().use { statement ->
        statement.execute(createTableQuery)
    }

    val createIndexQuery = """
        CREATE INDEX IF NOT EXISTS idx_ruc ON contribuyentes (ruc);
        CREATE INDEX IF NOT EXISTS idx_dni ON contribuyentes (dni);
    """

    connection.createStatement().use { statement ->
        val queries = createIndexQuery.split(";").filter { it.isNotBlank() }
        for (query in queries) {
            statement.addBatch(query)
        }
        statement.executeBatch()
    }

    // Iniciar transacción
    connection.autoCommit = false

    // Lee el archivo CSV línea por línea
    var line: String? = bufferedReader.readLine()
    var primeraLinea = true // Variable para identificar la primera línea
    var batchSize = 0
    val batchSizeLimit = 1000 // Tamaño del lote de inserción

    val insertQuery = """
            INSERT INTO contribuyentes (
                ruc, dni, nombre, estado, condicion_domicilio, ubigeo,
                tipo_via, nombre_via, codigo_zona, tipo_zona, numero,
                interior, lote, departamento, manzana, kilometro
            ) VALUES (
                ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?
            )
        """

    // Crear una declaración preparada
    val preparedStatement = connection.prepareStatement(insertQuery)

    while (line != null) {
        if (primeraLinea) {
            primeraLinea = false
            line = bufferedReader.readLine()
            continue // Omitir el procesamiento de la primera línea
        }
        val datos = line.split("|")
        val ruc = datos[0]
        val nombre = datos[1]
        val estado = datos[2]
        val condicionDomicilio = datos[3]
        val ubigeo = datos[4]
        val tipoVia = datos[5]
        val nombreVia = datos[6]
        val codigoZona = datos[7]
        val tipoZona = datos[8]
        val numero = datos[9]
        val interior = datos[10]
        val lote = datos[11]
        val departamento = datos[12]
        val manzana = datos[13]
        val kilometro = datos[14]

        println("$ruc - $nombre")

        // Obtén el DNI según la lógica especificada
        val dni = when {
            ruc.startsWith("10") -> ruc.substring(2, 10)
            else -> "-"
        }

        // Asignar los valores a los marcadores de posición
        preparedStatement.setString(1, ruc)
        preparedStatement.setString(2, dni)
        preparedStatement.setString(3, nombre)
        preparedStatement.setString(4, estado)
        preparedStatement.setString(5, condicionDomicilio)
        preparedStatement.setString(6, ubigeo)
        preparedStatement.setString(7, tipoVia)
        preparedStatement.setString(8, nombreVia)
        preparedStatement.setString(9, codigoZona)
        preparedStatement.setString(10, tipoZona)
        preparedStatement.setString(11, numero)
        preparedStatement.setString(12, interior)
        preparedStatement.setString(13, lote)
        preparedStatement.setString(14, departamento)
        preparedStatement.setString(15, manzana)
        preparedStatement.setString(16, kilometro)

        // Agregar la declaración preparada al lote de inserción
        preparedStatement.addBatch()

        batchSize++

        // Ejecutar el lote de inserción si se alcanza el tamaño límite
        if (batchSize >= batchSizeLimit) {
            preparedStatement.executeBatch()
            batchSize = 0
        }

        line = bufferedReader.readLine()
    }

    // Ejecutar el lote de inserción final (si hay filas restantes en el lote)
    preparedStatement.executeBatch()

    // Confirmar la transacción
    connection.commit()

    // Cierra los recursos
    bufferedReader.close()
    connection.close()
}