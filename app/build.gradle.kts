plugins {
    alias(libs.plugins.android.application)
    alias(libs.plugins.kotlin.android)
    alias(libs.plugins.kotlin.compose)
}

android {
    namespace = "com.vaditim.velvetdeck"
    compileSdk = 36

    defaultConfig {
        applicationId = "com.vaditim.velvetdeck"
        // Android 12 is the floor because every press and the timer's end are made of the vibrator's composed primitives.
        minSdk = 31
        targetSdk = 36
        versionCode = 1
        versionName = "2.0.0"
    }

    signingConfigs {
        // Debug builds only: one committed key, so a local debug build installs over the last one.
        getByName("debug") {
            storeFile = file("debug.keystore")
            storePassword = "android"
            keyAlias = "androiddebugkey"
            keyPassword = "android"
        }
        // The release key never enters the repository, which is public; CI writes it out from the repository's secrets and points these variables at it.
        create("release") {
            System.getenv("RELEASE_KEYSTORE")?.let { path ->
                storeFile = file(path)
                storePassword = System.getenv("RELEASE_KEYSTORE_PASSWORD")
                keyAlias = System.getenv("RELEASE_KEY_ALIAS")
                keyPassword = System.getenv("RELEASE_KEY_PASSWORD")
            }
        }
    }

    buildTypes {
        // The build to install: shrunk and optimised by R8, signed with the release key once its secrets exist (the committed debug key until then).
        release {
            isMinifyEnabled = true
            isShrinkResources = true
            proguardFiles(getDefaultProguardFile("proguard-android-optimize.txt"))
            signingConfig = signingConfigs.getByName(if (System.getenv("RELEASE_KEYSTORE") != null) "release" else "debug")
        }
    }

    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }

    buildFeatures {
        compose = true
    }
}

dependencies {
    implementation(project(":core:settings"))
    implementation(project(":core:data"))
    implementation(project(":core:design"))
    implementation(project(":core:ui"))
    implementation(project(":feature:setup"))
    implementation(project(":feature:table"))
    implementation(project(":feature:options"))

    implementation(libs.androidx.core.ktx)
    implementation(libs.androidx.activity.compose)
    implementation(libs.androidx.lifecycle.runtime.compose)

    implementation(platform(libs.androidx.compose.bom))
    implementation(libs.androidx.compose.ui)
    implementation(libs.androidx.compose.foundation)
    implementation(libs.androidx.compose.animation)
    implementation(libs.haze)
}

kotlin {
    jvmToolchain(17)
}
