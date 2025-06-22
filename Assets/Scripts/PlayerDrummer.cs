using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerDrummer : MonoBehaviour
{
    private Animator anim;

    private PlayerControls playerActions;
    private PlayerControls.PlayerActions controls;

    private Direction facingDirection = Direction.East;

    private void Awake()
    {
        playerActions = new();
        controls = playerActions.Player;

        TryGetComponent(out anim);
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

        facingDirection = direction;

        Vector3 euler = transform.localEulerAngles;
        euler.z = facingDirection switch
        {
            Direction.North => 90,
            Direction.East => 0,
            Direction.South => 270,
            Direction.West => 180,
            _ => throw new NotImplementedException(),
        };
        transform.localEulerAngles = euler;
    }

    public void ActionDirection(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 directionalInput = obj.ReadValue<Vector2>();
        if (Mathf.Abs(directionalInput.x) != 1 && Mathf.Abs(directionalInput.y) != 1) return;

        NoteType actionType = Vector2ToAction(directionalInput);

        switch (actionType)
        {
            case NoteType.swat:
                anim.SetTrigger("Swat");
                break;
            case NoteType.stomp:
                anim.SetTrigger("Stomp");
                break;
            case NoteType.spray:
                anim.SetTrigger("Spray");
                break;
            case NoteType.poke:
                anim.SetTrigger("Poke");
                break;
        };

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
        if (vector == Vector2.up) return NoteType.spray;
        else if (vector == Vector2.down) return NoteType.stomp;
        else if (vector == Vector2.left) return NoteType.swat;
        else if (vector == Vector2.right) return NoteType.poke;
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
