using Microsoft.AspNetCore.Identity;

namespace SIT.Controller.Controllers
{

    /// <summary>
    /// Manages the registration state and password options for the application, including enabling or disabling registration
    /// and updating password configuration settings.
    /// </summary>
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
