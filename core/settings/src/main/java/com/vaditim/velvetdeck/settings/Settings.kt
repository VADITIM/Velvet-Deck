package com.vaditim.velvetdeck.settings

import android.content.Context
import android.content.SharedPreferences
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import java.util.Locale

// The few things the game remembers between evenings, as snapshot state so every screen follows a change at once.
object Settings {
    private lateinit var preferences: SharedPreferences

    var isRoleplayOn by mutableStateOf(true)
        private set
    var isVibrationOn by mutableStateOf(true)
        private set
    // Until the players choose, the phone's own language decides.
    var language by mutableStateOf(Language.ENGLISH)
        private set

    // The last couple's names and colours, so a second evening starts already set; 0 means the seat never chose one.
    var firstName by mutableStateOf("")
        private set
    var secondName by mutableStateOf("")
        private set
    var firstColor by mutableStateOf(0)
        private set
    var secondColor by mutableStateOf(0)
        private set

    fun init(context: Context) {
        preferences = context.getSharedPreferences("velvet-deck", Context.MODE_PRIVATE)
        isRoleplayOn = preferences.getBoolean(KEY_ROLEPLAY, true)
        isVibrationOn = preferences.getBoolean(KEY_VIBRATION, true)
        language = Language.of(preferences.getString(KEY_LANGUAGE, null)) ?: if (Locale.getDefault().language == Language.GERMAN.code) Language.GERMAN else Language.ENGLISH
        firstName = preferences.getString(KEY_FIRST_NAME, "").orEmpty()
        secondName = preferences.getString(KEY_SECOND_NAME, "").orEmpty()
        firstColor = preferences.getInt(KEY_FIRST_COLOR, 0)
        secondColor = preferences.getInt(KEY_SECOND_COLOR, 0)
    }

    fun setRoleplay(isOn: Boolean) {
        isRoleplayOn = isOn
        preferences.edit().putBoolean(KEY_ROLEPLAY, isOn).apply()
    }

    fun setVibration(isOn: Boolean) {
        isVibrationOn = isOn
        preferences.edit().putBoolean(KEY_VIBRATION, isOn).apply()
    }

    fun chooseLanguage(chosen: Language) {
        language = chosen
        preferences.edit().putString(KEY_LANGUAGE, chosen.code).apply()
    }

    fun rememberPlayers(firstName: String, firstColor: Int, secondName: String, secondColor: Int) {
        this.firstName = firstName
        this.secondName = secondName
        this.firstColor = firstColor
        this.secondColor = secondColor
        preferences.edit()
            .putString(KEY_FIRST_NAME, firstName)
            .putString(KEY_SECOND_NAME, secondName)
            .putInt(KEY_FIRST_COLOR, firstColor)
            .putInt(KEY_SECOND_COLOR, secondColor)
            .apply()
    }

    private const val KEY_ROLEPLAY = "roleplay"
    private const val KEY_VIBRATION = "vibration"
    private const val KEY_LANGUAGE = "language"
    private const val KEY_FIRST_NAME = "first-name"
    private const val KEY_SECOND_NAME = "second-name"
    private const val KEY_FIRST_COLOR = "first-color"
    private const val KEY_SECOND_COLOR = "second-color"
}
