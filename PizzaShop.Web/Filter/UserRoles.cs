namespace PizzaShop.Filter;

public static class UserRoles
{

    public const string Admin = "1";
        public const string Chef = "3";

        public const string Manager = "2";
        public static readonly List<string> All = new List<string> { "1", "2", "3" };
}
