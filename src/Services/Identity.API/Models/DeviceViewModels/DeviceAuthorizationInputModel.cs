namespace Identity.API.Models.DeviceViewModels
{
    using Identity.API.Models.ConsentViewModels;

    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}