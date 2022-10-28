using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private CameraController camController;
    [SerializeField] private CatCharacterMovement catController;
    [SerializeField] private SkullCharacterController skullController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool spaceDown = Input.GetButtonDown("Jump");

        if (spaceDown) {
            switchActive();
        }
    }

    private void switchActive() {
        catController.switchActive();
        skullController.switchActive();
        camController.switchActive();
    }
}
