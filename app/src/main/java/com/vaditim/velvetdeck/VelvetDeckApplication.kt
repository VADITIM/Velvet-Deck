package com.vaditim.velvetdeck

import android.app.Application
import com.vaditim.velvetdeck.settings.Settings

class VelvetDeckApplication : Application() {
    override fun onCreate() {
        super.onCreate()
        Settings.init(this)
    }
}
