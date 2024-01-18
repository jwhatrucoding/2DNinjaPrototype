using Godot;

public partial class Player : CharacterBody2D
{
	private float _runSpeed = 750;
	private float _jumpSpeed = -1000;
	private float _gravity = 2500;
	private int _jump_counter = 0;
	private int _extra_jumps = 1;
	

	public override void _PhysicsProcess(double delta)
	{
		var velocity = Velocity;		
		velocity.X = 0;
		
		// Get input
		var right = Input.IsActionPressed("move_right");
		var left = Input.IsActionPressed("move_left");
		// For jump IsActionJustPressed is better and more precise than IsActionPressed
		var jump = Input.IsActionJustPressed("move_jump");
		
		// JUMP
		if (jump && _jump_counter < _extra_jumps){
			velocity.Y = _jumpSpeed;
			_jump_counter += 1;
			GD.Print(_jump_counter + " " + _extra_jumps);
		}
		// RIGHT
		if (right){
			velocity.X += _runSpeed;
		}
		// LEFT
		if (left){
			velocity.X -= _runSpeed;
		}
		// When on floor reset jump counter
		if (IsOnFloor()){
			_jump_counter = 0;
		}
		
		Velocity = velocity;
		// Get AnimatedSprites
		var animatedSprite2D = GetNode<AnimatedSprite2D>("Sprite2D");
		
		// If moving, play animation 
		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * _runSpeed;
			animatedSprite2D.Play();
		}
		// else stop
		else
		{
			animatedSprite2D.Stop();
		}
		// If walking play animation walking and flip player
		if (velocity.X != 0)
		{
			animatedSprite2D.Animation = "walking";
			animatedSprite2D.FlipV = false;
			// See the note below about boolean assignment.
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		
		velocity = Velocity;
		velocity.Y += _gravity * (float)delta;
		Velocity = velocity;
		MoveAndSlide();	
	}
}
