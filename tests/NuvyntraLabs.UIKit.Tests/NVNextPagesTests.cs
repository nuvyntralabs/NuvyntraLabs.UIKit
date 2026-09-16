namespace NuvyntraLabs.UIKit.Tests;

[Collection("UIKit")]
public class NVNextPagesTests
{
    public static TheoryData<Type> NextRecipes =>
    [
        typeof(NVInvoiceView),
        typeof(NVReceiptView),
        typeof(NVCompareView),
        typeof(NVStoreLocatorView),
        typeof(NVSubscriptionView),
        typeof(NVWhatsNewView),
        typeof(NVConflictResolveView),
        typeof(NVCallView),
        typeof(NVAddressFormView)
    ];

    [Theory]
    [MemberData(nameof(NextRecipes))]
    public void Recipe_instantiates_with_null_binding_context(Type type)
    {
        var view = (ContentView)Activator.CreateInstance(type)!;
        view.BindingContext = null;
        Assert.Null(view.BindingContext);
        Assert.NotNull(view.Content);
    }
}
