using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// This class provides a way to automatically connect your Unity messages (OnMouseDown/Up, etc)
/// to the easy touch plugin, so it handles multi touch (unlike Unity's messages).
/// This way, if you have started your project by using simple unity messages system but want to switch to a
/// proper multi touch handling system, you don't have to change your whole project's code to use the event-based system
/// in Easy Touch.
/// 
/// After having imported and setup Easy Touch plugin:
/// 1- Add this component to EasyTouch's game object in the scene
/// 2- Rename all your OnMouse<Down/Up/Drag> functions as OnETMouse<Down/Up/Drag>
/// 3- Add a Gesture parameter to those functions 
/// 4- Replace all uses of Input.mousePosition in the code of those functions with gesture.position
/// 
/// </summary>
public class EasyTouchFacade : MonoBehaviour {

	void OnEnable () 
	{
		EasyTouch.On_SimpleTap += HandleOn_SimpleTap;
		EasyTouch.On_TouchStart += HandleOn_TouchStart;
		EasyTouch.On_TouchUp += HandleOn_TouchUp;
		EasyTouch.On_Drag += HandleOn_Drag;
	}
	
	void OnDisable () 
	{
		EasyTouch.On_SimpleTap -= HandleOn_SimpleTap;
		EasyTouch.On_TouchStart -= HandleOn_TouchStart;
		EasyTouch.On_TouchUp -= HandleOn_TouchUp;
		EasyTouch.On_Drag -= HandleOn_Drag;
	}
    
    void HandleOn_SimpleTap (Gesture gesture)
	{
		if (gesture.pickedObject != null)
			gesture.pickedObject.SendMessage("OnETMouseUpAsButton", gesture, SendMessageOptions.DontRequireReceiver);
	}
	
	void HandleOn_TouchStart (Gesture gesture)
	{
		if (gesture.pickedObject != null)
			gesture.pickedObject.SendMessage("OnETMouseDown", gesture, SendMessageOptions.DontRequireReceiver);
	}
	
	void HandleOn_TouchUp (Gesture gesture)
	{
		if (gesture.pickedObject != null)
			gesture.pickedObject.SendMessage("OnETMouseUp", gesture, SendMessageOptions.DontRequireReceiver);
	}
	
	void HandleOn_Drag (Gesture gesture)
	{
		if (gesture.pickedObject != null)
			gesture.pickedObject.SendMessage("OnETMouseDrag", gesture, SendMessageOptions.DontRequireReceiver);
	}
}
