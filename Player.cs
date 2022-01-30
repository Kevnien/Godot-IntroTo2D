using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    private int Speed = 400;

    public Vector2 ScreenSize;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewportRect().Size;
        // Hide();
        // var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        // animatedSprite.hide();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var velocity = Vector2.Zero;
        if (Input.IsActionPressed("move_right"))
        {
            velocity.x++;
        } 
        if (Input.IsActionPressed("move_left"))
        {
            velocity.x--;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.y--;
        }
        if (Input.IsActionPressed("move_down"))
        {
            velocity.y++;
        }

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        Position += velocity * delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, ScreenSize.x),
            y: Mathf.Clamp(Position.y, 0, ScreenSize.y)
        );

        if (velocity.x != 0)
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipV = false;
            animatedSprite.FlipH = velocity.x < 0;
        }
        else if (velocity.y != 0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipH = false;
            animatedSprite.FlipV = velocity.y > 0;
        }
    }

    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        Hide(); // Player disappears after being hit.
        EmitSignal(nameof(Hit));
        // Must be deferred as we can't change physics properties on a physics callback.
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        // Show();
        // var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        // animatedSprite.show();
        // var button = GetNode<Button>("Button");
        // button.hide();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    public void _on_Button_pressed()
    {
        Start(new Vector2(x :ScreenSize.x / 2, y: ScreenSize.y / 2));
    }
}
