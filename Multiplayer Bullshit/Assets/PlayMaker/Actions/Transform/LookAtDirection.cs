// (c) Copyright HutongGames, LLC 2021. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Transform)]
    [Tooltip("Rotates a Game Object so its forward vector points in the specified Direction.")]
    public class LookAtDirection : ComponentAction<Transform>
    {
        [RequiredField]
        [Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        [Tooltip("The direction to look at.")]
        public FsmVector3 targetDirection;

        [Tooltip("Keep this vector pointing up as the GameObject rotates.")]
        public FsmVector3 upVector;

        [Tooltip("Perform in LateUpdate. This can help eliminate jitters in some situations.")]
        public bool lateUpdate;

        public override void Reset()
        {
            gameObject = null;
            targetDirection = new FsmVector3 { UseVariable = true };
            upVector = new FsmVector3 { UseVariable = true };
            lateUpdate = true;
        }

        public override void OnPreprocess()
        {
            Fsm.HandleLateUpdate = lateUpdate;
        }

        public override void OnUpdate()
        {
            if (!lateUpdate)
            {
                DoLookAtDirection();
            }
        }

        public override void OnLateUpdate()
        {
            if (lateUpdate)
            {
                DoLookAtDirection();
            }
        }

        private void DoLookAtDirection()
        {
            if (targetDirection.IsNone) return;

            if (!UpdateCacheAndTransform(Fsm.GetOwnerDefaultTarget(gameObject)))
                return;

            cachedTransform.rotation = Quaternion.LookRotation(targetDirection.Value, upVector.IsNone ? Vector3.up : upVector.Value);
        }
    }
}