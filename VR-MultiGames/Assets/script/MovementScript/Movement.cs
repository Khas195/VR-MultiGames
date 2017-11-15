using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using script.ControllerScript;
using UnityEngine;

namespace script.MovementScript
{
    public abstract class Movement : MonoBehaviour
    {
        protected Movement(bool isGrounded=false, bool isCrouch=false, 
            bool isSprint=false)
        {
            IsGrounded = isGrounded;
            IsCrouch = isCrouch;
            IsSprint = isSprint;
        }

        public float GroundHeight = 1.25f;
        public float HeightOffset = 1f;
        public LayerMask GroundLayer;
        public bool IsGrounded { get; protected set; }
        public bool IsCrouch { get; protected set;  }
        public bool IsSprint { get; protected set; }
        [SerializeField] protected Controller Controller;
        
        public abstract void Move(Vector3 direction);
        public abstract void Jump(float scale);

        private void Awake()
        {
            if (Controller == null)
            {
                Controller = GetComponent<Controller>();
            }
        }

        protected void Start()
        {
            GroundCheck();
        }

        protected void OnEnable()
        {
            if (Controller && !Controller.AllowMultipleMovements && IsOtherInstanceEnabled())
            {
                enabled = false;
            }
            
            GroundCheck();
            IsCrouch = IsSprint = false;
        }

        protected void GroundCheck()
        {
            if (Controller == null)
                return;
            
            Vector3 origin = Controller.transform.position;
            origin.y += HeightOffset;

            if (Physics.Raycast(origin, Vector3.down, HeightOffset + GroundHeight, GroundLayer))
            {
                IsGrounded = true;
            }
            else
            {
                IsGrounded = false;
            }
        }
        
        public void SetController(Controller controller)
        {
            Controller = controller;
        }

        public void SetCrouch(bool isCrouch)
        {
            IsCrouch = isCrouch;
        }

        public void SetSprint(bool isSprint)
        {
            IsSprint = isSprint;
        }

        private bool IsOtherInstanceEnabled()
        {
            var movementList = Controller.GetComponents<Movement>().ToList();
            return movementList.Where(movement => !GameObject.ReferenceEquals(movement, this)).
                Any(movement => movement.enabled);
        }
    }
}
