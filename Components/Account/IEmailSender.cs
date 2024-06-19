namespace SIT.Controller.Components.Account
{
    public interface IEmailSender<TUser>
    {
        Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink);
        Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink);
        Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode);
    }
}
