using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace Skeleton {

    interface SkeletonController
    {
        void switchActive();

        bool getIsActive();

        LayerMask getGrabbable();

        LayerMask getGrabbableSolid();
    }

//}