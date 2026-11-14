package com.vaditim.velvetdeck.settings

// The languages the game speaks; the code is what is remembered.
enum class Language(val code: String, val shortName: String) {
    ENGLISH("en", "EN"),
    GERMAN("de", "DE");

    companion object {
        fun of(code: String?): Language? = entries.firstOrNull { language -> language.code == code }
    }
}
