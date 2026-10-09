pluginManagement {
    includeBuild("build-logic")
    repositories {
        google()
        mavenCentral()
        gradlePluginPortal()
    }
}

dependencyResolutionManagement {
    repositoriesMode.set(RepositoriesMode.FAIL_ON_PROJECT_REPOS)
    repositories {
        google()
        mavenCentral()
    }
}

rootProject.name = "Velvet-Deck"

// The app joins the features, features stand on the core, and the core never reaches up (docs/ARCHITECTURE.md).
include(":app")
include(":core:settings")
include(":core:data")
include(":core:design")
include(":core:ui")
include(":feature:setup")
include(":feature:table")
include(":feature:options")
