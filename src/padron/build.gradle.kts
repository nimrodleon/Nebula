plugins {
    kotlin("jvm") version "1.8.21"
    application
}

group = "org.example"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    implementation("org.xerial:sqlite-jdbc:3.34.0")
    testImplementation(kotlin("test"))
}

tasks.test {
    useJUnitPlatform()
}

kotlin {
    jvmToolchain(11)
}

application {
    mainClass.set("MainKt")
}

tasks.jar {
    duplicatesStrategy = DuplicatesStrategy.INCLUDE
    manifest {
        attributes["Main-Class"] = "MainKt"
    }
    // Configuración para incluir todas las dependencias en el JAR
    from(configurations.runtimeClasspath.get().map { if (it.isDirectory) it else zipTree(it) })
    from(tasks.getByName<org.jetbrains.kotlin.gradle.tasks.KotlinCompile>("compileKotlin").destinationDirectory)
}
