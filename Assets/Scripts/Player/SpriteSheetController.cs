using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class SpriteSheetController : MonoBehaviour
    {

        [SerializeField] Material baseMaterial;
        [SerializeField] int animationFramesPerSecond=12;
        [SerializeField] Vector2 spriteSheetSize = new Vector2(8,8);
        [SerializeField] int totalFramesPerAnimation = 6;

        private Coroutine spriteAnimationsCoroutine;
        private bool animate=true;
        private AnimationState animationState = AnimationState.Down;
        private string textureName = "_MainTex";

        private float AnimationDuration => 1f / animationFramesPerSecond;
        private float SpriteFrameSizeX => 1f / spriteSheetSize.x;
        private float SpriteFrameSizeY => 1f / spriteSheetSize.y;

        public enum AnimationState
        {
            Left = 0,
            Right = 1,
            Up=2,
            Down=3,
        }
        private void OnEnable()
        {
            PlayerInputHandler.OnInputMovement += ChangeSpriteSheetAnimation;
      
         }
        private void OnDisable()
        {
            PlayerInputHandler.OnInputMovement -= ChangeSpriteSheetAnimation;
            RestartSpriteSheet();
        }
        private void ChangeSpriteSheetAnimation(Vector2 direction)
        {

            if (direction == Vector2.zero)
            {
                StopAnimation();
                return;
            }

            if (direction.y > 0.1)
                animationState = AnimationState.Up;
            else if (direction.y < -0.1)
                animationState = AnimationState.Down;
            else if (direction.x > 0.1)
                animationState = AnimationState.Right;
            else if (direction.x < -0.1)
                animationState = AnimationState.Left;

            StartAnimation();
        }

        private void StopAnimation()
        {
            animate= false;
            if(spriteAnimationsCoroutine!=null)
            {
                StopCoroutine(spriteAnimationsCoroutine);
                spriteAnimationsCoroutine = null;
            }

            //Take animation spriteSheet to the IDLE animations
            Vector2 newOffset = new Vector2((int)animationState*SpriteFrameSizeX,
                                            (spriteSheetSize.y-1) * SpriteFrameSizeY);
            baseMaterial.SetTextureOffset(textureName, newOffset);
           
        }

        private void StartAnimation()
        {
            StopAnimation();
            animate = true;
            spriteAnimationsCoroutine = StartCoroutine(AnimateSprites());
       
        }

        IEnumerator AnimateSprites()
        {
            Vector2 newOffset;
            var yOffset = (int)animationState;
        

            //Animation loop
            while (animate)
            {
               
                yield return new WaitForSeconds(AnimationDuration);

                var actualTextureOffset = baseMaterial.GetTextureOffset(textureName);
                newOffset = new Vector2(actualTextureOffset.x + SpriteFrameSizeX, yOffset * SpriteFrameSizeY);

                if (newOffset.x >= SpriteFrameSizeX * totalFramesPerAnimation)
                     newOffset = new Vector2(0, yOffset * SpriteFrameSizeY);

                baseMaterial.SetTextureOffset(textureName, newOffset);

            }
        }


        //Take the player to the IDLE POSE
        public void RestartSpriteSheet()
        {
            Vector2 newOffset = new Vector2(SpriteFrameSizeX *3,
                                    SpriteFrameSizeX * (spriteSheetSize.y-1));

            baseMaterial.SetTextureOffset(textureName, newOffset);

        }
    }
}


