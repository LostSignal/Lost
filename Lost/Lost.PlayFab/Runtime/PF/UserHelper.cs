﻿//-----------------------------------------------------------------------
// <copyright file="UserHelper.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_PLAYFAB_SDK

namespace Lost
{
    using System.Collections.Generic;
    using PlayFab.ClientModels;

    ////
    //// TODO [bgish]: Make sure to null out all the cached data when user is logged out
    ////
    public class UserHelper
    {
        private UserAccountInfo userAccountInfo;

        public UserTitleInfo TitleInfo { get; private set; }
        public string PlayFabId { get; private set; }
        public long PlayFabNumericId { get; private set; }
        public string DisplayName { get; private set; }
        public string FacebookId { get; private set; }
        public bool IsFacebookLinked => string.IsNullOrEmpty(this.FacebookId) == false;
        public string AvatarUrl { get; private set; }

        public UserHelper()
        {
            PF.PlayfabEvents.OnLoginResultEvent += PlayfabEvents_OnLoginResultEvent;
            PF.PlayfabEvents.OnUpdateAvatarUrlResultEvent += PlayfabEvents_OnUpdateAvatarUrlResultEvent;
            PF.PlayfabEvents.OnUpdateUserTitleDisplayNameResultEvent += PlayfabEvents_OnUpdateUserTitleDisplayNameResultEvent;
            PF.PlayfabEvents.OnLinkFacebookAccountResultEvent += PlayfabEvents_OnLinkFacebookAccountResultEvent;
            PF.PlayfabEvents.OnUnlinkFacebookAccountResultEvent += PlayfabEvents_OnUnlinkFacebookAccountResultEvent;
        }

        public UnityTask<bool> ChangeDisplayName(string newDisplayName)
        {
            return UnityTask<bool>.Run(this.ChangeDisplayNameCoroutine(newDisplayName));
        }

        public UnityTask<bool> ChangeDisplayNameWithPopup()
        {
            return UnityTask<bool>.Run(this.ChangeDisplayNameWithPopupCoroutine());
        }

        public bool HasFacebookPermission(string permission)
        {
            #if USING_FACEBOOK_SDK
            var facebookPermissions = Facebook.Unity.AccessToken.CurrentAccessToken?.Permissions;

            if (facebookPermissions != null)
            {
                foreach (var p in facebookPermissions)
                {
                    if (p == permission)
                    {
                        return true;
                    }
                }
            }
            #endif

            return false;
        }

        public bool HasFacebookPermissions(List<string> permissions)
        {
            #if USING_FACEBOOK_SDK
            if (permissions.IsNullOrEmpty() == false)
            {
                foreach (var permission in permissions)
                {
                    if (this.HasFacebookPermission(permission) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
            #else

            return false;

            #endif
        }

        private IEnumerator<bool> ChangeDisplayNameCoroutine(string newDisplayName)
        {
            var updateDisplayName = PF.Do(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = newDisplayName,
            });

            while (updateDisplayName.IsDone == false)
            {
                yield return default(bool);
            }

            // Early out if we got an error
            if (updateDisplayName.HasError)
            {
                PlayFabMessages.HandleError(updateDisplayName.Exception);
                yield return false;
                yield break;
            }

            // TODO [bgish]: Fire off DisplayName changed event?
            // Updating the display name
            this.DisplayName = newDisplayName;

            yield return true;
        }

        private IEnumerator<bool> ChangeDisplayNameWithPopupCoroutine()
        {
            var stringInputBox = PlayFabMessages.ShowChangeDisplayNameInputBox(PF.User.DisplayName);

            while (stringInputBox.IsDone == false)
            {
                yield return default(bool);
            }

            if (stringInputBox.Value == StringInputResult.Cancel)
            {
                yield return false;
                yield break;
            }

            var updateDisplayName = PF.Do(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = StringInputBox.Instance.Text,
            });

            while (updateDisplayName.IsDone == false)
            {
                yield return default(bool);
            }

            // Early out if we got an error
            if (updateDisplayName.HasError)
            {
                PlayFabMessages.HandleError(updateDisplayName.Exception);
                yield return false;
                yield break;
            }

            // TODO [bgish]: Fire off DisplayName changed event?
            // Updating the display name
            this.DisplayName = StringInputBox.Instance.Text;

            yield return true;
        }

        private void PlayfabEvents_OnLoginResultEvent(LoginResult result)
        {
            this.userAccountInfo = result?.InfoResultPayload?.AccountInfo;

            this.PlayFabId = this.userAccountInfo?.PlayFabId;
            this.TitleInfo = this.userAccountInfo?.TitleInfo;
            this.DisplayName = this.userAccountInfo?.TitleInfo?.DisplayName;
            this.FacebookId = this.userAccountInfo?.FacebookInfo?.FacebookId;
            this.AvatarUrl = this.userAccountInfo?.TitleInfo?.AvatarUrl;

            this.PlayFabNumericId = PF.ConvertPlayFabIdToLong(this.PlayFabId);

            // TODO [bgish]: Fire a AvatarUrlChanged event
            // TODO [bgish]: Fire a DisplayNameChanged event
            // TODO [bgish]: Fire a FacebookChanged event
        }

        private void PlayfabEvents_OnLinkFacebookAccountResultEvent(LinkFacebookAccountResult result)
        {
            #if USING_FACEBOOK_SDK
            this.FacebookId = PF.Login.FacebookLoginResult?.AccessToken?.UserId;
            #endif
        }

        private void PlayfabEvents_OnUnlinkFacebookAccountResultEvent(UnlinkFacebookAccountResult result)
        {
            this.FacebookId = null;
        }

        private void PlayfabEvents_OnUpdateAvatarUrlResultEvent(EmptyResponse result)
        {
            // TODO [bgish]: Update AvatarUrl and fire a AvatarUrlChanged event
        }

        private void PlayfabEvents_OnUpdateUserTitleDisplayNameResultEvent(UpdateUserTitleDisplayNameResult result)
        {
            // TODO [bgish]: Update DisplayName and fire a DisplayNameChanged event
        }
    }
}

#endif
