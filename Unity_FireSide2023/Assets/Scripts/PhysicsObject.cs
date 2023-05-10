#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
public abstract class PhysicsObject : MonoBehaviour
{
    // Just create another script and make it a child of this class like  " public class Player_Controller : Character_Controller "
    // You can then modify all behaviour values and use CharacterLookAt, CharacterMove and CharacterJump to control the character
    // the class implements a function called OnGroundContact that is like OnCollisionEnter but only when the player hits any surface
    // You can use OnGroundContact to play a sound or particles and use the Vector3 contactPoint for 3d sounds or to place the particle effect there

    public float f_clamp = 1f;
    // Rigidbody
    public Rigidbody Rb { get; private set; }

    [Header("Rigidbody Settings")]
    public string layer;
    private int layerMask;
    public float gravity = 10f;
    public float mass = 1f;
    public float drag = 0f;
    public float angDrag = 0.05f;
    // Capsule Collider
    public CapsuleCollider Col { get; private set; }



    [Header("Capsule Collider Settings")]
    public Vector3 center = Vector3.zero;
    public float radius = .5f;
    public float lenght = 1f;

    [Header("Physics Behaviour Settings")]
    public float alignHeight = .5f;
    public float alignHeightPuffer = 1f;
    public float alignHeightOffset = -.5f;
    public float alignHeightSpringStrenght = 50f;
    public float alignHeightSpringDamper = 25f;
    public float alignRotationSpringStrenght = 50f;
    public float alignRotationSpringDamp = 2.5f;

    // private forces in fixed update
    private Vector3 AdditionalForce;
    private Vector3 jumpForce;
    private Vector3 AdditionalTorque;

    // Limit the velocity to prevent bugs
    private float maxVelocity = 100000;

    // public getter variables
    public bool IsGrounded { get; private set; }
    public bool WasInAir { get; private set; }
    public bool HasLanded { get; private set; }

    private void Awake()
    {
        Rb = AddRigidBody(mass, drag, angDrag);
        Col = AddCapsuleCollider(center, radius, lenght);
        layerMask = ~LayerMask.GetMask(layer);

    }

    private void FixedUpdate()
    {
        if (Rb == null || Col == null)
            return;
        // gravity
        Rb.AddForce(gravity * Rb.mass * Vector3.down);
        
        UpdateUprightRotation(); // spring forces to upright rotation and height
        UpdateRightHeight();
        
        Rb.AddForce(AdditionalForce); // Additional Forces for look at and move functions
        Rb.AddTorque(AdditionalTorque); // Additional Forces for look at and move functions
        
        Rb.AddForce(jumpForce); // Add Jump Force

        // transition jumpForce back to 0
        jumpForce = jumpForce.magnitude > 0 ? (Vector3.zero - jumpForce) * Time.fixedDeltaTime : Vector3.zero;
        // check for the first ground contact and trigger the
        // OnGroundContact function with the contact position that
        // a child class can use to play sounds or trigger particle system
        CheckForGroundContact();
    }


    // Child movement functions
    public void CharacterLookAt(Vector3 pos)
    {
        // Zero out the y position and check if the Vector is not 0,0,0 because this debugs an error
        pos.y = 0;

        if (pos != Vector3.zero)
        {
            // Get the shortest rotation path from players current rotation and the target rotation
            Quaternion currentRot = Quaternion.LookRotation(transform.forward, Vector3.up);
            Quaternion nextRot = Quaternion.LookRotation(pos, Vector3.up);
            Quaternion targetRot = ShortestRotation(currentRot, nextRot);

            // Outputs the angle to the target rotation
            targetRot.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);
            rotAxis.Normalize();
            float rotRadians = rotDegrees * Mathf.Deg2Rad;


            // Apply the additional torque (this variable comes from the character controller parent)
            Vector3 neededTorque = (rotAxis * -rotRadians) * (1 / Time.fixedDeltaTime);
            AdditionalTorque = neededTorque;
        }
    }
    public void CharacterMove(Vector3 dir)
    {
        Vector3 clampedVel = Vector3.ClampMagnitude(Rb.velocity, f_clamp);
        Vector3 neededForce = (dir - clampedVel) * (1 / Time.fixedDeltaTime);
        neededForce.y = 0;
        Debug.LogWarning($"force: {neededForce}, dir: {dir}, vel: {Rb.velocity}");

        AdditionalForce = neededForce;
    }
    public void CharacterJump(float height)
    {
        // Zero out the current velocity to have the same jump everytime
        Rb.velocity = Vector3.zero;
        // Calc the up force needed to perform the jump
        float neededForce = height * (1 / Time.fixedDeltaTime);
        // Set the jumpForce to the calulated value
        jumpForce = Vector3.up * neededForce;
    }

    // Balancing movement functions
    private void UpdateUprightRotation()
    {
        // Find the next rotation direction based on the current rotation and the upright rotation
        Quaternion currentRot = transform.rotation;
        Quaternion uprightRot = Quaternion.Euler(Vector3.up * transform.rotation.eulerAngles.y);
        Quaternion goalRot = ShortestRotation(currentRot, uprightRot);

        goalRot.ToAngleAxis(out float rotDegrees, out Vector3 rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        Vector3 force = (rotAxis * (rotRadians * -alignRotationSpringStrenght)) - (Rb.angularVelocity * alignRotationSpringDamp);
        Rb.AddTorque(force);
    }
    private void UpdateRightHeight()
    {
        // Define the start and end Position of the downwards linecast for ground check
        Vector3 startPos = transform.position + Vector3.down * alignHeightOffset;
        Vector3 endPos = transform.position + Vector3.down * (alignHeightOffset + alignHeight + alignHeightPuffer);

        IsGrounded = Physics.Linecast(startPos, endPos, out RaycastHit hit, layerMask,QueryTriggerInteraction.Ignore);

        if (IsGrounded && jumpForce.y == 0f)
        {
            float rayDirVel = Vector3.Dot(Vector3.down, Rb.velocity); // How far away is the current velocity direction from the downwards direction
            float x = hit.distance - alignHeight; // How far is the player to the ground

            // the force is simply the distance to the ground 
            // since its always returning a force  the regulate the player to the specific location it need be dampened
            // which is done by just subtracting the force amount with the amount how much the player is already falling
            // both the force and the dampen are regulated with mulitpliers -- so genius
            float springForce = (x * (alignHeightSpringStrenght * gravity)) - (rayDirVel * alignHeightSpringDamper);
            // apply that downward force
            Rb.AddForce(Vector3.down * springForce);
        }
    }

    // Ground check
    private void CheckForGroundContact()
    {
        if (!WasInAir && !IsGrounded)
        {
            WasInAir = true;
            HasLanded = false;
        }
        else if (WasInAir && IsGrounded && !HasLanded)
        {
            HasLanded = true;
            WasInAir = false;

            Vector3 startPos = transform.position + Vector3.down * alignHeightOffset;
            Vector3 endPos = transform.position + Vector3.down * (alignHeightOffset + alignHeight + alignHeightPuffer);
            Physics.Linecast(startPos, endPos, out RaycastHit hit);

            OnGroundContact(hit.point);
        }
    }
    // For fx or audio in child
    public abstract void OnGroundContact(Vector3 contactPoint);

    // For creating the capsule collider and rigidbody
    private CapsuleCollider AddCapsuleCollider(Vector3 center, float radius, float height)
    {
        // Get existing capsule collider or create one and apply values
        CapsuleCollider col = GetComponent<CapsuleCollider>() != null ? GetComponent<CapsuleCollider>() : this.gameObject.AddComponent<CapsuleCollider>();
        col.isTrigger = false;
        col.center = center;
        col.radius = radius;
        col.height = height/3;
        col.direction = 2;

        col.material = new PhysicMaterial
        {
            dynamicFriction = 0,
            staticFriction =0,
            bounciness = 0,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Minimum
        };
        return col;
    }
    private Rigidbody AddRigidBody(float mass, float drag, float angDrag)
    {
        // Get existing rigidbody or create one and apply values
        Rigidbody rb = GetComponent<Rigidbody>() != null ? GetComponent<Rigidbody>() : this.gameObject.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = drag;
        rb.angularDrag = angDrag;
        rb.maxAngularVelocity = maxVelocity;
        rb.maxDepenetrationVelocity = maxVelocity;
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.constraints = RigidbodyConstraints.None;
        return rb;
    }

    // For visualisation in editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawWireCapsule(transform.position + center, transform.rotation * Quaternion.Euler(new(-90,0,0)), radius, lenght, Color.green);;
        Debug.DrawLine(transform.position + Vector3.down * alignHeightOffset, transform.position + Vector3.down * alignHeightOffset + Vector3.down * alignHeight, Color.red);
        Debug.DrawLine(transform.position + Vector3.down * (alignHeight + alignHeightOffset + alignHeightPuffer), transform.position + Vector3.down * alignHeightOffset + Vector3.down * alignHeight, Color.yellow);
    }
#endif

    // Extention functions
    public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default(Color))
    {
        if (_color != default(Color))
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

        }
    }
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
            return a * Quaternion.Inverse(Multiply(b, -1));

        else return a * Quaternion.Inverse(b);
    }
    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}
