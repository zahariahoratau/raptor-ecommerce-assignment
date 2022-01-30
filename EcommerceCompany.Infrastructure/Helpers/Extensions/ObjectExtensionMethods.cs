namespace EcommerceCompany.Infrastructure.Helpers.Extensions
{
    public static class ObjectExtensionMethods
    {
        public static void ThrowIfNull<T>(this T? @this) where T : class?
        {
            if (@this is null)
                throw new ArgumentNullException($"{nameof(@this)} was null");
        }
    }
}
