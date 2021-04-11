namespace Identity.API.Models.DeviceViewModels
{
    using Identity.API.Models.ConsentViewModels;

    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}