namespace SIT.Controller.Controllers
{
    public class RegistrationStateService
    {
        public bool RegistrationEnabled { get; private set; } = true;

        public void ToggleRegistration()
        {
            RegistrationEnabled = !RegistrationEnabled;
        }
    }
}
