# Alternative Smooth Player
This is a very basic Project Arrhythmia mod that attempts to fix the player visual being capped to 50 FPS due to the way Rigidbody works in Unity.
> **WARNING:** as of now, this mod seems to scale down the player's hitbox to 0.75 of its size. This is a mistake and will be fixed soon. 

## Usage
- Press the F key to enable/disable the fix.
- Press the V key to enable/disable the visual representation of the player's physics.

## In-game Implementation (TODO)
1. Remove the `DelayTracker` component from the player.
2. Add the following code to the bottom of the `Update` method of the `VGPlayer` class:
  ```cs
  const float rotateSpeed = 40f;
  const float followSpeed = 10f;
  const float edgeSpeed = 25f;
		
  var positionDelta = (Vector3)this.internalVelocity;
  var rotationDelta = Quaternion.Euler(0f, 0f, this.Player_Rigidbody.transform.eulerAngles.z);
		
  var visual = this.transform.Find("Player");
  visual.position += positionDelta * Time.deltaTime;
  visual.rotation = Quaternion.Lerp(visual.rotation, rotationDelta, Time.deltaTime * rotateSpeed);
  ```
3. Add the following code to the bottom of the `LateUpdate` method of the `VGPlayer` class:
  ```cs
  const float rotateSpeed = 40f;
  const float followSpeed = 10f;
  const float edgeSpeed = 25f;

  var visual = this.transform.Find("Player");
  visual.position = Vector3.Lerp(visual.position, this.Player_Rigidbody.transform.position, Time.deltaTime * followSpeed);

  var vector = this.ObjectCamera.WorldToViewportPoint(visual.position);
  var edgeOffset = VGPlayer.EDGE_OFFSET;
  var num = (float)Screen.height / Screen.width * edgeOffset;

  if (vector.x < num || vector.x > 1f - num || vector.y < edgeOffset || vector.y > 1f - edgeOffset)
  {
    vector.x = Mathf.Clamp(vector.x, num, 1f - num);
    vector.y = Mathf.Clamp(vector.y, edgeOffset, 1f - edgeOffset);
    visual.position = Vector3.Lerp(visual.position, this.ObjectCamera.ViewportToWorldPoint(vector), Time.deltaTime * edgeSpeed);
  }
  ```
