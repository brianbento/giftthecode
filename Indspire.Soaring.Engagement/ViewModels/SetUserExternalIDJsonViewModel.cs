// Copyright (c) Team Agility. All rights reserved.

namespace Indspire.Soaring.Engagement.ViewModels
{
    public class SetUserExternalIDJsonViewModel : JsonBaseViewModel
    {
        public SetUserExternalIDJsonViewModel()
        {
            this.ResponseData = new SetUserExternalIDResponseData();
        }

        public SetUserExternalIDResponseData ResponseData { get; set; }
    }
}
