package com.vaditim.velvetdeck.table

import androidx.compose.foundation.gestures.Orientation
import androidx.compose.foundation.gestures.draggable
import androidx.compose.foundation.gestures.rememberDraggableState
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.BoxWithConstraints
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.size
import androidx.compose.runtime.Composable
import androidx.compose.runtime.derivedStateOf
import androidx.compose.runtime.getValue
import androidx.compose.runtime.key
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.layout.onSizeChanged
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.min
import com.vaditim.velvetdeck.game.Face
import com.vaditim.velvetdeck.game.Table
import com.vaditim.velvetdeck.vas.pressable
import kotlin.math.PI
import kotlin.math.sin

// The card in the middle of the table and the one waiting under it. A tap turns the top card over; once it is read, a swipe either way (or a tap) sends it to the other player, and a joker lifts it away upward.
@Composable
fun CardStage(table: Table, dealer: Dealer, modifier: Modifier = Modifier) {
    BoxWithConstraints(
        modifier.onSizeChanged { size ->
            dealer.width = size.width.toFloat().coerceAtLeast(1f)
            dealer.height = size.height.toFloat().coerceAtLeast(1f)
        },
    ) {
        val cardWidth = min(min(maxWidth - 40.dp, maxHeight * CARD_RATIO), 400.dp)
        val cardModifier = Modifier.align(Alignment.Center).size(cardWidth, cardWidth / CARD_RATIO)

        // The next card waits underneath, smaller and dimmed, and grows into place as the top one is swiped away (components/23 §3).
        table.next?.let { next ->
            key(next) {
                CardFace(
                    next.kind,
                    cardModifier.graphicsLayer {
                        // Hidden while the top card turns: it narrows to an edge mid-turn and would show the next card's kind.
                        val turned = dealer.flip.value
                        if (turned > 0f && turned < 1f) {
                            alpha = 0f
                            return@graphicsLayer
                        }
                        val departure = dealer.departure
                        val scale = WAITING_SCALE + (1f - WAITING_SCALE) * departure
                        scaleX = scale
                        scaleY = scale
                        alpha = WAITING_ALPHA + (1f - WAITING_ALPHA) * departure
                    },
                )
            }
        }

        val card = table.card ?: return@BoxWithConstraints
        Box(
            cardModifier
                .graphicsLayer {
                    translationX = dealer.drag
                    translationY = dealer.rise
                    rotationZ = dealer.drag / dealer.width * LEAN_DEGREES
                }
                .pressable({ if (table.face == Face.FRONT) dealer.turnOver() else dealer.pass() }, pressedScale = 0.98f)
                .draggable(
                    rememberDraggableState { delta -> dealer.dragBy(delta) },
                    Orientation.Horizontal,
                    enabled = table.face == Face.BACK,
                    onDragStarted = { dealer.startDrag() },
                    onDragStopped = { velocity -> dealer.release(velocity) },
                ),
        ) {
            val isBackShown by remember { derivedStateOf { dealer.flip.value > 0.5f } }
            Box(
                Modifier
                    .fillMaxSize()
                    .graphicsLayer {
                        val turned = dealer.flip.value
                        rotationY = turned * 180f
                        cameraDistance = CAMERA_DISTANCE * density
                        // The card lifts off the table a little while it turns, and is laid back down as it lands.
                        val lift = 1f + FLIP_LIFT * sin(turned * PI.toFloat())
                        scaleX = lift
                        scaleY = lift
                    },
            ) {
                key(card) {
                    if (isBackShown) CardBack(card, Modifier.graphicsLayer { rotationY = 180f }) else CardFace(card.kind)
                }
            }
        }
    }
}

const val CARD_RATIO = 0.68f
private const val WAITING_SCALE = 0.92f
private const val WAITING_ALPHA = 0.45f
private const val LEAN_DEGREES = 14f
private const val CAMERA_DISTANCE = 18f
private const val FLIP_LIFT = 0.06f
