using UnityEngine;

public class PlayerDrummer : MonoBehaviour
{
    [Header("Test Object")]
    [SerializeField] private SpriteRenderer upSpriteRenderer;
    [SerializeField] private SpriteRenderer downSpriteRenderer;
    [SerializeField] private SpriteRenderer leftSpriteRenderer;
    [SerializeField] private SpriteRenderer rightSpriteRenderer;

    [Header("Colors")]
    [SerializeField] private Color upColor;
    [SerializeField] private Color downColor;
    [SerializeField] private Color leftColor;
    [SerializeField] private Color rightColor;
    [SerializeField] private Color normalColor;

    private SpriteRenderer[] SpriteRenderers
    {
        get
        {
            return new SpriteRenderer[] {upSpriteRenderer, downSpriteRenderer, leftSpriteRenderer, rightSpriteRenderer};
        }
    }
    private Color[] Colors
    {
        get
        {
            return new Color[] { upColor, downColor, leftColor, rightColor };
        }
    }

    private PlayerControls playerActions;
    private PlayerControls.PlayerActions controls;

    private void Awake()
    {
        playerActions = new();
        controls = playerActions.Player;
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.ActionDirection.performed += ActionDirection;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.ActionDirection.performed -= ActionDirection;
    }

    private void ActionDirection(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Vector2DirectionToInt(currentDirection) == -1) return;

        Vector2 direction = obj.ReadValue<Vector2>();
        if (Vector2DirectionToInt(direction) == -1) return;

        SpriteRenderers[Vector2DirectionToInt(currentDirection)].color = Colors[Vector2DirectionToInt(direction)];
    }

    private void Update()
    {
        FaceDirection();
    }

    private Vector2 prevDirection;
    private Vector2 currentDirection;
    private void FaceDirection()
    {
        Vector2 directionalInput = controls.FaceDirection.ReadValue<Vector2>();

        if (directionalInput == prevDirection) return;
        
        foreach (SpriteRenderer sRend in SpriteRenderers)
        {
            sRend.color = normalColor;
        }

        prevDirection = directionalInput;

        if (directionalInput == Vector2.zero || (Mathf.Abs(directionalInput.x) != 1 && Mathf.Abs(directionalInput.y) != 1)) 
        {
            foreach (SpriteRenderer sRend in SpriteRenderers)
            {
                sRend.enabled = false;
                currentDirection = Vector2.zero;
            }
            return;
        }

        SpriteRenderers[Vector2DirectionToInt(prevDirection)].enabled = true;
        currentDirection = prevDirection;
        print(directionalInput);
    }

    private int Vector2DirectionToInt(Vector2 vector)
    {
        if (vector.x == 0 && vector.y == 1) return 0;
        else if (vector.x == 0 && vector.y == -1) return 1;
        else if (vector.x == -1 && vector.y == 0) return 2;
        else if (vector.x == 1 && vector.y == 0) return 3;
        else return -1;
    }
}
