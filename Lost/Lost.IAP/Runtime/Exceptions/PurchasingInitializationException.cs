﻿//-----------------------------------------------------------------------
// <copyright file="PurchasingInitializationException.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_UNITY_PURCHASING && !UNITY_XBOXONE

namespace Lost
{
    using UnityEngine.Purchasing;

    public class PurchasingInitializationException : System.Exception
    {
        public InitializationFailureReason FailureReason { get; private set; }

        public PurchasingInitializationException(InitializationFailureReason failureReason)
        {
            this.FailureReason = failureReason;
        }
    }
}

#endif
