namespace OneJevelsCompany.Web.Routing
{
    public static class RouteNames
    {
        public static class Admin
        {
            public const string Dashboard = "Admin.Dashboard";
            public const string NewInvoice = "Admin.NewInvoice";
            public const string Invoices = "Admin.Invoices";
            public const string PurchaseQueue = "Admin.PurchaseQueue";
            public const string ComponentsList = "Admin.Components.List";
            public const string Jewel = "Admin.Jewels";
            public const string DesignOrders = "Admin.DesignOrders";
            public const string ReadyDesignOrders = "Admin.ReadyDesignOrders";

            public static class Jewels
            {
                public const string New = "Admin.Jewels.New";
                public const string Edit = "Admin.Jewels.Edit";
            }
        }

        public static class Components
        {
            public const string New = "Admin.Components.New";
            public const string Edit = "Admin.Components.Edit";
        }

        public static class Categories
        {
            public const string Index = "Admin.Categories.Index";
            public const string New = "Admin.Categories.New";
            public const string Edit = "Admin.Categories.Edit";
            public const string Create = "Admin.Categories.Create";
            public const string Delete = "Admin.Categories.Delete";
        }

        public static class Cart
        {
            public const string View = "Cart.View";
            public const string Update = "Cart.Update";
            public const string Remove = "Cart.Remove";
            public const string Clear = "Cart.Clear";
            public const string AddCustomRecipe = "Cart.AddCustomRecipe";
        }

        public static class Checkout
        {
            public const string Index = "Checkout.Index";
            public const string Create = "Checkout.CreateOrder";
            public const string Success = "Checkout.Success";
        }

        public static class Shop
        {
            public const string Collections = "Shop.Collections";
            public const string BuildGet = "Shop.Build.GET";
            public const string BuildPost = "Shop.Build.POST";
            public const string AddReady = "Shop.AddReady";
            public const string Details = "Shop.Details";
            public const string ConfigureGet = "Shop.Configure.GET";
            public const string ConfigurePost = "Shop.Configure.POST";
            public const string DesignGet = "Shop.Design.GET";
            public const string DesignPost = "Shop.Design.POST";
            public const string SubmitDesign = "Shop.SubmitDesign";
            public const string DesignSubmitted = "Shop.DesignSubmitted";
            public const string DesignsGallery = "Shop.DesignsGallery";
        }

        public static class AdminSetup
        {
            public const string Elevate = "AdminSetup.Elevate";
        }

        public static class Home
        {
            public const string Index = "Home.Index";
            public const string About = "Home.About";
        }

        public static class Account
        {
            public const string Login = "Account.Login";
            public const string LoginPost = "Account.Login.POST";
            public const string Register = "Account.Register";
            public const string RegisterPost = "Account.Register.POST";
            public const string Logout = "Account.Logout";
            public const string LogoutGet = "Account.Logout.GET";
            public const string AccessDenied = "Account.AccessDenied";
        }
    }
}
