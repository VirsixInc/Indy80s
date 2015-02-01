#pragma strict

// The velocity at which drag should begin being applied.
var dragStartVelocity : float;
// The velocity at which drag should equal maxDrag.
var dragMaxVelocity : float;

// The maximum allowed velocity. The velocity will be clamped to keep
// it from exceeding this value. (Note: this value should be greater than
// or equal to dragMaxVelocity.)
var maxVelocity : float;

// The maximum drag to apply. This is the value that will
// be applied if the velocity is equal or greater
// than dragMaxVelocity. Between the start and max velocities,
// the drag applied will go from 0 to maxDrag, increasing
// the closer the velocity gets to dragMaxVelocity.
var maxDrag : float = 1.0;

// The original drag of the object, which we use if the velocity
// is below dragStartVelocity.
private var originalDrag : float;
// Cache the rigidbody to avoid GetComponent calls behind the scenes.
private var rb : Rigidbody;
// Cached values used in FixedUpdate
private var sqrDragStartVelocity : float;
private var sqrDragVelocityRange : float;
private var sqrMaxVelocity : float;

// Awake is called when the script instance is being loaded.
// For more info, see:
// http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.Awake.html
function Awake(){
	originalDrag = rigidbody.drag;
	rb = rigidbody;
	Initialize(dragStartVelocity, dragMaxVelocity, maxVelocity, maxDrag);
}

// Sets the threshold values and calculates cached variables used in FixedUpdate.
// Outside callers who wish to modify the thresholds should use this function. Otherwise,
// the cached values will not be recalculated.
function Initialize(dragStartVelocity : float, dragMaxVelocity : float, maxVelocity : float, maxDrag : float){
	this.dragStartVelocity = dragStartVelocity;
	this.dragMaxVelocity = dragMaxVelocity;
	this.maxVelocity = maxVelocity;
	this.maxDrag = maxDrag;

	// Calculate cached values
	sqrDragStartVelocity = dragStartVelocity * dragStartVelocity;
	sqrDragVelocityRange = (dragMaxVelocity * dragMaxVelocity) - sqrDragStartVelocity;
	sqrMaxVelocity = maxVelocity * maxVelocity;
}

// FixedUpdate is a built-in unity function that is called every fixed framerate frame.
// We use FixedUpdate instead of Update here because the docs recommend doing so when
// dealing with rigidbodies.
// For more info, see:
// http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.FixedUpdate.html
//
// We limit the velocity here to account for gravity and to allow the drag to be relaxed
// over time, even if no collisions are occurring.
function FixedUpdate(){
	var v = rb.velocity;
	// We use sqrMagnitude instead of magnitude for performance reasons.
	var vSqr = v.sqrMagnitude;

	if(vSqr > sqrDragStartVelocity){
		rigidbody.drag = Mathf.Lerp(originalDrag, maxDrag, Mathf.Clamp01((vSqr - sqrDragStartVelocity)/sqrDragVelocityRange));

		// Clamp the velocity, if necessary
		if(vSqr > sqrMaxVelocity){
			// Vector3.normalized returns this vector with a magnitude
			// of 1. This ensures that we're not messing with the
			// direction of the vector, only its magnitude.
			rb.velocity = v.normalized * maxVelocity;
		}
	} else {
		rb.drag = originalDrag;
	}
}

// Require a Rigidbody component to be attached to the same GameObject.
@script RequireComponent(Rigidbody)
