using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class SimpleVelocityLimiter : MonoBehaviour {
	
	public float dragStartVelocity;
	// The velocity at which drag should equal maxDrag.
	public float dragMaxVelocity;
	
	// The maximum allowed velocity. (Note: this value should be greater than
	// or equal to dragMaxVelocity.)
	public float maxVelocity;
	
	// The maximum drag to apply. This is the value that will
	// be applied if the velocity is equal or greater
	// than dragMaxVelocity. Between the start and max velocities,
	// the drag applied will go from 0 to maxDrag, increasing
	// the closer the velocity gets to dragMaxVelocity.
	public float maxDrag = 1.0f;
	
	// The original drag of the object, which we use if the velocity
	// is below dragStartVelocity.
	private float originalDrag;

	private float sqrDragStartVelocity;
	private float sqrDragVelocityRange;
	private float sqrMaxVelocity;

	void Awake() {
		originalDrag = rigidbody.drag;
		Initialize(dragStartVelocity, dragMaxVelocity, maxVelocity, maxDrag);
	}
	
	// Sets the threshold values and calculates cached variables used in FixedUpdate.
	// Outside callers who wish to modify the thresholds should use this function. Otherwise,
	// the cached values will not be recalculated.
	void Initialize(float p_dragStartVelocity, float p_dragMaxVelocity, float p_maxVelocity, float p_maxDrag){
		dragStartVelocity = p_dragStartVelocity;
		dragMaxVelocity = p_dragMaxVelocity;
		maxVelocity = p_maxVelocity;
		maxDrag = p_maxDrag;
		
		// Calculate cached values
		sqrDragStartVelocity = dragStartVelocity * dragStartVelocity;
		sqrDragVelocityRange = (dragMaxVelocity * dragMaxVelocity) - sqrDragStartVelocity;
		sqrMaxVelocity = maxVelocity * maxVelocity;
	}

	// We limit the velocity here to account for gravity and to allow the drag to be relaxed
	// over time, even if no collisions are occurring.
	void FixedUpdate(){
		Vector3 v = rigidbody.velocity;
		// We use sqrMagnitude instead of magnitude for performance reasons.
		float vSqr = v.sqrMagnitude;
		
		if(vSqr > sqrDragStartVelocity){
			rigidbody.drag = Mathf.Lerp(originalDrag, maxDrag, Mathf.Clamp01((vSqr - sqrDragStartVelocity)/sqrDragVelocityRange));
			
			// Clamp the velocity, if necessary
			if(vSqr > sqrMaxVelocity){
				rigidbody.velocity = v.normalized * maxVelocity;
			}
		} else {
			rigidbody.drag = originalDrag;
		}
	}
}
