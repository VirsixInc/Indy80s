#pragma strict

var AICar : GameObject;
var brakingPower : float = 20;
var enginePower : float = 20;

function Start() {
	//This is where the script finds the AI car and plugs it into the variable of this script.
	AICar = GameObject.FindGameObjectWithTag("AI");
}


// Hit the brakes when the AI enters the trigger
function OnTriggerEnter (other : Collider) {

	AICar.GetComponent.< AICar_Script >().BrakePower = (brakingPower);
	AICar.GetComponent.< AICar_Script >().EngineTorque = (enginePower);
}

function OnTriggerExit (other : Collider) {

	AICar.GetComponent.< AICar_Script >().BrakePower = 0;
	AICar.GetComponent.< AICar_Script >().EngineTorque = (Mathf.Lerp(enginePower, 600, Time.deltaTime));

}