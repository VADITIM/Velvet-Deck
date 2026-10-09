plugins {
    alias(libs.plugins.vados.android.library)
}

dependencies {
    implementation(project(":core:settings"))
    implementation(project(":core:data"))
    implementation(project(":core:design"))
    implementation(libs.haze)
    implementation(libs.androidx.compose.animation)
}
