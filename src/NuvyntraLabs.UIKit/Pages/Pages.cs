namespace NuvyntraLabs.UIKit;

public abstract class NVPageRecipe : ThemeAwareView
{
    protected NVPageRecipe(string title, string body, params View[] extras)
    {
        var stack = new VerticalStackLayout { Spacing = NVTokens.Space3 };
        stack.Children.Add(new NVToolbar { Title = title });
        stack.Children.Add(new NVCard { Title = title, Body = body });
        foreach (var extra in extras)
        {
            stack.Children.Add(extra);
        }
        Content = stack;
        ApplyTheme();
    }

    protected override void ApplyTheme() { }
}

public class NVSignInView : NVPageRecipe
{
    public NVSignInView() : base("Sign in", "Email and password",
        new NVTextField { Label = "Email" },
        new NVTextField { Label = "Password", IsPassword = true },
        new NVButton { Text = "Continue", Variant = NVButtonVariant.Filled }) { }
}

public class NVSignUpView : NVPageRecipe
{
    public NVSignUpView() : base("Sign up", "Create an account",
        new NVDataForm
        {
            Fields =
            [
                NVFormField.For("Name", typeof(string)),
                NVFormField.For("Email", typeof(string))
            ]
        }) { }
}

public class NVForgotPasswordView : NVPageRecipe
{
    public NVForgotPasswordView() : base("Forgot password", "Reset link",
        new NVTextField { Label = "Email" },
        new NVEmptyView { Title = "Check your inbox", Reason = NVStatusReason.Generic },
        new NVButton { Text = "Send link" }) { }
}

public class NVResetPasswordView : NVPageRecipe
{
    public NVResetPasswordView() : base("Reset password", "OTP then new secret",
        new NVOtpInput { Length = 6 },
        new NVTextField { Label = "New password", IsPassword = true }) { }
}

public class NVSocialSignInView : NVPageRecipe
{
    public NVSocialSignInView() : base("Social sign in", "Icon row + form",
        new HorizontalStackLayout
        {
            Spacing = NVTokens.Space2,
            Children = { new NVIconButton { Kind = NVIconKind.User }, new NVIconButton { Kind = NVIconKind.Chat } }
        },
        new NVSignInView()) { }
}

public class NVTabbedAuthView : NVPageRecipe
{
    public NVTabbedAuthView() : base("Auth", "Tabbed sign-in / sign-up",
        new NVTabView { Tabs = new List<string> { "Sign in", "Sign up" } }) { }
}

public class NVProfileSetupView : NVPageRecipe
{
    public NVProfileSetupView() : base("Profile setup", "Avatar + form",
        new NVAvatar { Initials = "NV" },
        new NVDataForm { Fields = [NVFormField.For("Display name", typeof(string))] }) { }
}

public class NVWalkthroughView : NVPageRecipe
{
    public NVWalkthroughView() : base("Walkthrough", "Carousel intro",
        new NVCarousel { Items = new List<string> { "Welcome", "Lumina", "Ready" } },
        new NVButton { Text = "Next" }) { }
}

public class NVCategoryView : NVPageRecipe { public NVCategoryView() : base("Categories", "Browse tiles", new NVCardsView { Items = Demo.Items("Apparel", "Home") }) { } }
public class NVCatalogView : NVPageRecipe { public NVCatalogView() : base("Catalog", "Product grid", new NVCollectionView { Items = Demo.Items("Lamp", "Chair"), LayoutMode = NVLayoutMode.Tile }) { } }
public class NVProductHomeView : NVPageRecipe { public NVProductHomeView() : base("Store", "Home merchandising", new NVCarousel { Items = new List<string> { "Hero", "New" } }) { } }
public class NVProductDetailView : NVPageRecipe { public NVProductDetailView() : base("Product", "Detail + rating", new NVRating { Value = 4 }, new NVButton { Text = "Add to cart" }) { } }
public class NVCartView : NVPageRecipe { public NVCartView() : base("Cart", "Line items", new NVCollectionView { Items = Demo.Items("Lamp × 1") }, new NVButton { Text = "Checkout" }) { } }
public class NVWishlistView : NVPageRecipe { public NVWishlistView() : base("Wishlist", "Saved goods", new NVCollectionView { Items = Demo.Items("Chair") }) { } }
public class NVCheckoutView : NVPageRecipe { public NVCheckoutView() : base("Checkout", "Address + pay", new NVDataForm { Fields = [NVFormField.For("Address", typeof(string))] }) { } }
public class NVCardPaymentView : NVPageRecipe { public NVCardPaymentView() : base("Card payment", "Masked PAN", new NVMaskedEntry { Label = "Card", Mask = "0000 0000 0000 0000" }) { } }
public class NVSavedCardsView : NVPageRecipe { public NVSavedCardsView() : base("Saved cards", "Stored instruments", new NVCollectionView { Items = Demo.Items("Visa ••42") }) { } }
public class NVPaymentResultView : NVPageRecipe { public NVPaymentResultView() : base("Payment", "Result", new NVEmptyView { Title = "Paid", Reason = NVStatusReason.Generic }) { } }
public class NVOrdersView : NVPageRecipe { public NVOrdersView() : base("Orders", "Open tickets", new NVCollectionView { Items = Demo.Items("Order #1042") }) { } }
public class NVOrderHistoryView : NVPageRecipe { public NVOrderHistoryView() : base("History", "Past orders", new NVCollectionView { Items = Demo.Items("Order #1001") }) { } }

public class NVArticleFeedView : NVPageRecipe { public NVArticleFeedView() : base("Feed", "Articles", new NVCollectionView { Items = Demo.Items("Aurora notes") }) { } }
public class NVArticleDetailView : NVPageRecipe { public NVArticleDetailView() : base("Article", "Long read", new NVMarkdownViewer { Markdown = "# Lumina\nWarm paper." }) { } }
public class NVMyArticlesView : NVPageRecipe { public NVMyArticlesView() : base("My articles", "Author queue", new NVCollectionView { Items = Demo.Items("Draft") }) { } }
public class NVReviewView : NVPageRecipe { public NVReviewView() : base("Review", "Stars + text", new NVRating { Value = 5 }, new NVEditor { Label = "Thoughts" }) { } }
public class NVContactView : NVPageRecipe { public NVContactView() : base("Contact", "Reach us", new NVTextField { Label = "Message" }, new NVButton { Text = "Send" }) { } }
public class NVAboutView : NVPageRecipe { public NVAboutView() : base("About", "NuvyntraLabs.UIKit 1.0") { } }
public class NVFaqView : NVPageRecipe { public NVFaqView() : base("FAQ", "Accordion answers", new NVAccordion { Title = "What is Lumina?", IsExpanded = true }) { } }
public class NVBookmarksView : NVPageRecipe { public NVBookmarksView() : base("Bookmarks", "Saved reads", new NVCollectionView { Items = Demo.Items("Saved") }) { } }

public class NVInboxView : NVPageRecipe { public NVInboxView() : base("Inbox", "Threads", new NVCollectionView { Items = Demo.Items("Lumina", "Studio") }) { } }
public class NVConversationView : NVPageRecipe { public NVConversationView() : base("Conversation", "Chat", new NVChat { Messages = [new NVChatMessage { Author = "Lumina", Text = "Hello" }] }) { } }
public class NVSocialProfileView : NVPageRecipe { public NVSocialProfileView() : base("Profile", "Social", new NVAvatar { Initials = "NP" }, new NVButton { Text = "Follow" }) { } }
public class NVPeopleListView : NVPageRecipe { public NVPeopleListView() : base("People", "Directory", new NVCollectionView { Items = Demo.Items("Ada", "Lin") }) { } }
public class NVAuthorProfileView : NVPageRecipe { public NVAuthorProfileView() : base("Author", "Byline", new NVAvatar { Initials = "AP" }) { } }
public class NVHealthProfileView : NVPageRecipe { public NVHealthProfileView() : base("Health", "Vitals", new NVGauge { Value = 72 }) { } }
public class NVChatProfileView : NVPageRecipe { public NVChatProfileView() : base("Chat profile", "Presence", new NVAvatar { Initials = "CH", StatusOn = true }) { } }

public class NVNavigationHubView : NVPageRecipe { public NVNavigationHubView() : base("Hub", "Destinations", new NVNavigationView()) { } }
public class NVMediaLibraryView : NVPageRecipe { public NVMediaLibraryView() : base("Library", "Media", new NVCollectionView { Items = Demo.Items("Clip 1"), LayoutMode = NVLayoutMode.Tile }) { } }
public class NVPlaylistView : NVPageRecipe { public NVPlaylistView() : base("Playlist", "Tracks", new NVCollectionView { Items = Demo.Items("Track A") }) { } }
public class NVFileExplorerView : NVPageRecipe { public NVFileExplorerView() : base("Files", "Tree", new NVTreeView { Roots = [new NVTreeNode { Title = "Documents", IsExpanded = true, Children = { new NVTreeNode { Title = "Notes" } } }] }) { } }
public class NVDocumentsView : NVPageRecipe { public NVDocumentsView() : base("Documents", "PDF", new NVPdfViewer { Title = "Brief.pdf" }) { } }
public class NVSuggestionsView : NVPageRecipe { public NVSuggestionsView() : base("Suggestions", "AI chips", new NVAIPrompt()) { } }

public class NVStatusView : NVPageRecipe { public NVStatusView() : base("Status", "Empty / error", new NVEmptyView { Reason = NVStatusReason.Offline }) { } }
public class NVSettingsView : NVPageRecipe { public NVSettingsView() : base("Settings", "Toggles", new NVSwitch { Text = "Dark", IsOn = false }) { } }
public class NVHelpView : NVPageRecipe { public NVHelpView() : base("Help", "Support", new NVFaqView()) { } }
public class NVNotificationsView : NVPageRecipe { public NVNotificationsView() : base("Notifications", "Inbox", new NVBanner { Text = "Welcome to Lumina", Tone = NVBannerTone.Info }) { } }
public class NVDeliveryTrackView : NVPageRecipe { public NVDeliveryTrackView() : base("Delivery", "Track", new NVStepProgressBar { Steps = new List<string> { "Packed", "Ship", "Delivered" }, Index = 1 }) { } }
public class NVAddressBookView : NVPageRecipe { public NVAddressBookView() : base("Addresses", "Book", new NVCollectionView { Items = Demo.Items("Home", "Studio") }) { } }
public class NVBookingView : NVPageRecipe { public NVBookingView() : base("Booking", "Calendar", new NVCalendar(), new NVButton { Text = "Reserve" }) { } }
public class NVDashboardView : NVPageRecipe
{
    public NVDashboardView() : base("Dashboard", "Charts + cards",
        new NVChart
        {
            Series =
            [
                new NVChartSeries
                {
                    Title = "Aurora",
                    Kind = NVChartSeriesKind.Bar,
                    Points = [new NVChartPoint { Category = "Mon", Value = 4 }, new NVChartPoint { Category = "Tue", Value = 7 }]
                }
            ]
        },
        new NVGauge { Value = 64 },
        new NVCard { Title = "Preset", Body = "Seven sample presets share this layout." }) { }
}

public class NVPinLockView : NVPageRecipe { public NVPinLockView() : base("PIN lock", "Unlock the app", new NVLockPad()) { } }
public class NVForceUpdateView : NVPageRecipe { public NVForceUpdateView() : base("Update", "Blocking store prompt", new NVForceUpdate()) { } }
public class NVSearchResultsView : NVPageRecipe { public NVSearchResultsView() : base("Search", "Results", new NVSearchBar(), new NVFilterBar(), new NVCollectionView { Items = Demo.Items("Aurora notes") }) { } }
public class NVFilterSheetView : NVPageRecipe { public NVFilterSheetView() : base("Filters", "Sheet", new NVFilterBar(), new NVDateRangePicker(), new NVStickyBar { Text = "Apply" }) { } }
public class NVMediaPlayerView : NVPageRecipe { public NVMediaPlayerView() : base("Player", "AV chrome", new NVVideoPlayer(), new NVAudioPlayer()) { } }
public class NVSplitInboxView : NVPageRecipe { public NVSplitInboxView() : base("Inbox", "Master-detail", new NVMasterDetail()) { } }
public class NVOnboardingPermissionsView : NVPageRecipe { public NVOnboardingPermissionsView() : base("Permissions", "Rationale", new NVPermissionCard { Title = "Notifications" }, new NVPermissionCard { Title = "Location" }) { } }
public class NVOrderSummaryView : NVPageRecipe { public NVOrderSummaryView() : base("Summary", "Sticky pay bar", new NVCollectionView { Items = Demo.Items("Lamp") }, new NVCartBar { Total = 42 }) { } }

static class Demo
{
    public static List<NVListItem> Items(params string[] titles) =>
        titles.Select(t => new NVListItem { Title = t, Subtitle = "Lumina" }).ToList();
}
