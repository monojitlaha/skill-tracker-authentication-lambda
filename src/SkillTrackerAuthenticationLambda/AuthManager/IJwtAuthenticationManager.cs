using SkillTrackerAuthenticationLambda.Model;

namespace SkillTrackerAuthenticationLambda.AuthManager
{
    public interface IJwtAuthenticationManager
    {
        Response Authenticate(string userName, string password);
    }
}
