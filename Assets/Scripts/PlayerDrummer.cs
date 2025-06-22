using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDrummer : MonoBehaviour
{
    [Header("Test Object")]
    [SerializeField] private SpriteRenderer northSpriteRenderer;
    [SerializeField] private SpriteRenderer southSpriteRenderer;
    [SerializeField] private SpriteRenderer eastSpriteRenderer;
    [SerializeField] private SpriteRenderer westSpriteRenderer;

    [Header("Colors")]
    [SerializeField] private Color northColor = Color.green;
    [SerializeField] private Color southColor = Color.blue;
    [SerializeField] private Color eastColor = Color.red;
    [SerializeField] private Color westColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.grey;

    private SpriteRenderer[] SpriteRenderers => new SpriteRenderer[] { northSpriteRenderer, southSpriteRenderer, eastSpriteRenderer, westSpriteRenderer };
    private Color[] Colors => new Color[] { northColor, southColor, eastColor, westColor };

    private PlayerControls playerActions;
    private PlayerControls.PlayerActions controls;

    private Direction facingDirection = Direction.North;

    private void Awake()
    {
        playerActions = new();
        controls = playerActions.Player;

        ActivateSpriteRenderer(facingDirection);
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.ActionDirection.performed += ActionDirection;
        controls.FaceDirection.performed += FaceDirection;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.ActionDirection.performed -= ActionDirection;
        controls.FaceDirection.performed -= FaceDirection;
    }

    private void FaceDirection(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 directionalInput = obj.ReadValue<Vector2>();

        if (Mathf.Abs(directionalInput.x) != 1 && Mathf.Abs(directionalInput.y) != 1) return;
        
        Direction direction = Vector2ToDirection(directionalInput);
        if (direction == facingDirection) return;

        ActivateSpriteRenderer(direction);
        facingDirection = direction;
    }

    private void ActivateSpriteRenderer(Direction direction)
    {
        foreach (SpriteRenderer sRend in SpriteRenderers)
        {
            sRend.enabled = false;
        }

        _ = direction switch
        {
            Direction.North => northSpriteRenderer.enabled = true,
            Direction.South => southSpriteRenderer.enabled = true,
            Direction.West => westSpriteRenderer.enabled = true,
            Direction.East => eastSpriteRenderer.enabled = true,
            _ => throw new NotImplementedException(),
        };
    }

    public void ActionDirection(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 directionalInput = obj.ReadValue<Vector2>();
        if (Mathf.Abs(directionalInput.x) != 1 && Mathf.Abs(directionalInput.y) != 1) return;

        NoteType actionType = Vector2ToAction(directionalInput);

        LaneManager.Instance.CheckHit(facingDirection, actionType);
    }

    private int DirectionToInt(Direction dir) => dir switch
    {
        Direction.North => 0,
        Direction.South => 1,
        Direction.East => 2,
        Direction.West => 3,
        _ => throw new System.NotImplementedException(),
    };

    private Direction Vector2ToDirection(Vector2 vector) 
    {
        if (vector == Vector2.up) return Direction.North;
        else if (vector == Vector2.down) return Direction.South;
        else if (vector == Vector2.left) return Direction.West;
        else if (vector == Vector2.right) return Direction.East;
        else throw new System.NotImplementedException();
    }

    private NoteType Vector2ToAction(Vector2 vector)
    {
        if (vector == Vector2.up) return NoteType.Spray;
        else if (vector == Vector2.down) return NoteType.Stomp;
        else if (vector == Vector2.left) return NoteType.Swat;
        else if (vector == Vector2.right) return NoteType.Bat;
        else throw new System.NotImplementedException();
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}
