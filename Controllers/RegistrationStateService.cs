using Microsoft.AspNetCore.Identity;

namespace SIT.Controller.Controllers
{
    public class RegistrationStateService
    {
        public bool RegistrationEnabled { get; private set; } = true;
        public PasswordOptions PasswordOptions { get; private set; } = new PasswordOptions();

        public void ToggleRegistration()
        {
            RegistrationEnabled = !RegistrationEnabled;
        }

        public void UpdatePasswordOptions(PasswordOptions options)
        {
            PasswordOptions = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}
