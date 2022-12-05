using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] private CameraController camController;
    [SerializeField] private CatCharacterMovement catController;
    [SerializeField] private SkullCharacterController skull;
    [SerializeField] private SkeletonSansLegsController sansLegs;
    [SerializeField] private FullSkeletonController fullSkeleton;
    [SerializeField] private FMODUnity.StudioEventEmitter charSwitchEmitter;
    [SerializeField] private MusicManager musicManager;

    private bool isSkeleton;
    private SkeletonController skeletonController;

    // Start is called before the first frame update
    void Start()
    {

        isSkeleton = false;

        if (fullSkeleton != null) {
            skeletonController = (SkeletonController) fullSkeleton;
        } else if (sansLegs != null) {
            skeletonController = (SkeletonController) sansLegs;
        } else if (skull != null) {
            skeletonController = (SkeletonController) skull;
        } else {
            throw new Exception("No skeleton controller assigned.");
        }
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
        isSkeleton = !isSkeleton;
        catController.switchActive();
        skeletonController.switchActive();
        camController.switchActive();
        charSwitchEmitter.Play();
        musicManager.SwitchActiveCharacterMusic(isSkeleton);
    }
}
