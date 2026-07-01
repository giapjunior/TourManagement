using DataAccess.DataAccess;

namespace Presentation
{
    public static class SessionManager
    {
        public static Account? CurrentAccount { get; set; }
        public static Customer? CurrentCustomer { get; set; }

        public static bool IsAdmin => CurrentAccount?.Role == "Admin";
        public static bool IsCustomer => CurrentAccount?.Role == "Customer";

        public static void Clear()
        {
            CurrentAccount = null;
            CurrentCustomer = null;
        }
    }
}
