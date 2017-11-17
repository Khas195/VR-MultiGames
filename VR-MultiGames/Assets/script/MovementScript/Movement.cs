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

        [SerializeField] protected float _MaxSpeed = 10f;
        [SerializeField] protected float GroundHeight = 1.25f;
        [SerializeField] protected float HeightOffset = 1f;
        [SerializeField] protected LayerMask GroundLayer;
        [SerializeField] protected Controller Controller;
        
        public bool IsGrounded { get; protected set; }
        public bool IsCrouch { get; protected set;  }
        public bool IsSprint { get; protected set; }

        public float MaxSpeed
        {
            get { return _MaxSpeed; }
        }
        
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
    }
}
