using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{ 
    public static PlayerCamera Instance;
    public PlayerLocomotionManager playerLocomotionManager;

    [Header("Camera Settings")]
    public Camera cameraObject;
    private Vector3 _cameraVelocity;
    [SerializeField] private Transform cameraPivotTransform;
    [SerializeField] private float leftAndRightRotationSpeed = 100;
    [SerializeField] private float upAndDownRotationSpeed = 100;
    [SerializeField] private float minimumPivot = -30f; //THE LOWEST POINT YOU ARE ABLE TO LOOK DOWN
    [SerializeField] private float maximumPivot = 60f;  //THE HIGHEST POINT YOU ARE ABLE TO LOOK UP
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collideWithLayer;
    
    [Header("Camera values")]
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    [SerializeField] private float cameraSmoothSpeed = 1;
    private Vector3 _cameraObjectPosition; 
    private float _cameraZPosition;  
    private float _targetCameraZPosition;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if(playerLocomotionManager !=  null)
        {
            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();
        }
    }
    
    private void HandleFollowTarget()
    {
        var targetCameraPosition = Vector3.SmoothDamp(transform.position, playerLocomotionManager.transform.position, ref _cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotation()
    {
         //ROTATE LEFT AND RIGHT BASED ON HORIZONTAL ON THE MOUSE
        leftAndRightLookAngle += (InputHandler.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

        // ROTATE UP AND DOWN BASED ON VERTICAL ON THE MOUSE
        upAndDownLookAngle -= (InputHandler.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

        // CLAMP THE UP AND DOWN LOOK ANGLER BETWEEN MIN AND MAX
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);


        Vector3 cameraRotation = Vector3.zero;

        // ROTATE THIS GAME OBJECT LEFT AND RIGHT
        cameraRotation.y = leftAndRightLookAngle;
        var targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation; 


        // ROTATE THE PIVOT GAME OBJECT UP AND DOWN
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        _targetCameraZPosition = _cameraZPosition;

        // DIRECTION FOR THE COLLISION TO CHECK 
        var direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // WE CHECK IF THERE IS ANY OBJECT IN FRONT OF OUR DESIRED DIRECTION * 
        if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out var hit, Mathf.Abs(_targetCameraZPosition), collideWithLayer))
        {
            //IF THERE IS ANY OBJECT , WE GET OUR DISTANCE FROM IT
            Debug.DrawRay(cameraPivotTransform.position, direction * Mathf.Abs(_targetCameraZPosition), Color.green);
            var distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);

            // WE THEN EQUATE OUR DISTANCE FROM IT
            _targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }
        
        // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBSTRACT OUR COLLISION RADIUS
        if(Mathf.Abs(_targetCameraZPosition) < cameraCollisionRadius)
        {
            _targetCameraZPosition = -cameraCollisionRadius;
        }

        // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F SECONDS
        _cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = _cameraObjectPosition;
        
    }
}

