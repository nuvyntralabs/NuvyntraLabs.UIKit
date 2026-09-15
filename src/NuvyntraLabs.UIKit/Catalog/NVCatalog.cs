namespace NuvyntraLabs.UIKit;

/// <summary>Public catalog. Tests assert these names; the sample gallery binds them.</summary>
public static class NVCatalog
{
    public static IReadOnlyList<Type> Foundation { get; } =
    [
        typeof(NVTheme), typeof(NVTokens), typeof(NVTypography), typeof(NVIcons),
        typeof(NVMotion), typeof(NVDensity), typeof(NVVisualState), typeof(NVAccessibility)
    ];

    public static IReadOnlyList<Type> Controls { get; } =
    [
        typeof(NVSurface), typeof(NVDivider), typeof(NVIcon), typeof(NVAvatar), typeof(NVBadge),
        typeof(NVSkeleton), typeof(NVEffects), typeof(NVElevation), typeof(NVOverlay),
        typeof(NVInteractiveViewer), typeof(NVSpacer), typeof(NVHighlight),
        typeof(NVButton), typeof(NVIconButton), typeof(NVToggleButton), typeof(NVDropDownButton),
        typeof(NVCheckBox), typeof(NVRadioButton), typeof(NVSwitch), typeof(NVChip),
        typeof(NVSegmentedControl), typeof(NVSpeechToTextButton),
        typeof(NVTextField), typeof(NVEditor), typeof(NVSearchBar), typeof(NVMaskedEntry),
        typeof(NVNumericEntry), typeof(NVNumericUpDown), typeof(NVOtpInput), typeof(NVAutoComplete),
        typeof(NVComboBox), typeof(NVPicker), typeof(NVDatePicker), typeof(NVTimePicker),
        typeof(NVDateTimePicker), typeof(NVTimeSpanPicker), typeof(NVTemplatedPicker),
        typeof(NVColorPicker), typeof(NVSlider), typeof(NVRangeSlider), typeof(NVCircularSlider),
        typeof(NVRangeSelector), typeof(NVSignaturePad), typeof(NVRating),
        typeof(NVProgressBar), typeof(NVCircularProgressBar), typeof(NVStepProgressBar),
        typeof(NVBusyIndicator), typeof(NVPullToRefresh), typeof(NVPopup), typeof(NVToast),
        typeof(NVBanner), typeof(NVEmptyView), typeof(NVTooltip),
        typeof(NVCard), typeof(NVAccordion), typeof(NVExpander), typeof(NVTabView),
        typeof(NVBottomSheet), typeof(NVNavigationDrawer), typeof(NVNavigationView),
        typeof(NVDockLayout), typeof(NVWrapLayout), typeof(NVGridSplitter), typeof(NVToolbar),
        typeof(NVBackdrop), typeof(NVCarousel), typeof(NVParallaxView), typeof(NVBottomNavigation),
        typeof(NVRadialMenu),
        typeof(NVCollectionView), typeof(NVDataGrid), typeof(NVTreeDataGrid), typeof(NVTreeView),
        typeof(NVDataForm), typeof(NVDataPager), typeof(NVKanban),
        typeof(NVChart), typeof(NVRadialGauge), typeof(NVLinearGauge), typeof(NVDigitalGauge),
        typeof(NVMap), typeof(NVBarcode), typeof(NVTreeMap),
        typeof(NVCalendar), typeof(NVScheduler),
        typeof(NVImageEditor), typeof(NVRichTextEditor), typeof(NVMarkdownViewer), typeof(NVPdfViewer),
        typeof(NVChat), typeof(NVAIPrompt), typeof(NVSmartPasteButton), typeof(NVDocxViewer),
        typeof(NVSpreadsheet), typeof(NVPromptInput),
        typeof(NVHeading), typeof(NVBodyText), typeof(NVCaptionText), typeof(NVImage), typeof(NVSafeArea),
        typeof(NVSectionHeader), typeof(NVFormSection), typeof(NVFloatingActionButton), typeof(NVDotIndicator),
        typeof(NVAppScaffold), typeof(NVDialog), typeof(NVActionSheet), typeof(NVMenu),
        typeof(NVListTile), typeof(NVSettingsTile), typeof(NVChipGroup), typeof(NVCheckList),
        typeof(NVGroupedList), typeof(NVIndexBar), typeof(NVSwipeTile), typeof(NVSelectionBar),
        typeof(NVSkeletonList), typeof(NVInfiniteFooter),
        typeof(NVEmailField), typeof(NVPhoneField), typeof(NVPasswordField), typeof(NVPasswordStrength),
        typeof(NVQuantityStepper), typeof(NVDateRangePicker), typeof(NVMonthYearPicker), typeof(NVFilterBar),
        typeof(NVTagInput), typeof(NVPinPad), typeof(NVCopyable), typeof(NVLink),
        typeof(NVCountryPicker), typeof(NVLanguagePicker), typeof(NVThemePicker),
        typeof(NVCurrencyLabel), typeof(NVCountdown), typeof(NVQuote), typeof(NVCodeBlock), typeof(NVBulletList),
        typeof(NVStatCard), typeof(NVTimeline), typeof(NVWizard), typeof(NVStickyBar), typeof(NVCartBar),
        typeof(NVPriceTag), typeof(NVVariantPicker), typeof(NVCouponField), typeof(NVTicket),
        typeof(NVTimeSlotPicker), typeof(NVSeatPicker),
        typeof(NVProfileHeader), typeof(NVFeedCard), typeof(NVComposer), typeof(NVBubble),
        typeof(NVTypingIndicator), typeof(NVStoryRing), typeof(NVReactionBar), typeof(NVNotificationRow),
        typeof(NVContactTile),
        typeof(NVImageGallery), typeof(NVLightbox), typeof(NVVideoPlayer), typeof(NVAudioPlayer),
        typeof(NVWebView), typeof(NVVoiceNote), typeof(NVWaveform), typeof(NVBeforeAfter),
        typeof(NVMasterDetail), typeof(NVRetryView), typeof(NVOfflineBanner), typeof(NVPermissionCard),
        typeof(NVForceUpdate), typeof(NVLockPad), typeof(NVBiometricGate), typeof(NVDashboardGrid),
        typeof(NVGantt), typeof(NVOrgChart)
    ];

    public static IReadOnlyList<Type> Helpers { get; } =
    [
        typeof(NVRadioGroup), typeof(NVFormField)
    ];

    public static IReadOnlyList<Type> Pages { get; } =
    [
        typeof(NVSignInView), typeof(NVSignUpView), typeof(NVForgotPasswordView), typeof(NVResetPasswordView),
        typeof(NVSocialSignInView), typeof(NVTabbedAuthView), typeof(NVProfileSetupView), typeof(NVWalkthroughView),
        typeof(NVCategoryView), typeof(NVCatalogView), typeof(NVProductHomeView), typeof(NVProductDetailView),
        typeof(NVCartView), typeof(NVWishlistView), typeof(NVCheckoutView), typeof(NVCardPaymentView),
        typeof(NVSavedCardsView), typeof(NVPaymentResultView), typeof(NVOrdersView), typeof(NVOrderHistoryView),
        typeof(NVArticleFeedView), typeof(NVArticleDetailView), typeof(NVMyArticlesView), typeof(NVReviewView),
        typeof(NVContactView), typeof(NVAboutView), typeof(NVFaqView), typeof(NVBookmarksView),
        typeof(NVInboxView), typeof(NVConversationView), typeof(NVSocialProfileView), typeof(NVPeopleListView),
        typeof(NVAuthorProfileView), typeof(NVHealthProfileView), typeof(NVChatProfileView),
        typeof(NVNavigationHubView), typeof(NVMediaLibraryView), typeof(NVPlaylistView), typeof(NVFileExplorerView),
        typeof(NVDocumentsView), typeof(NVSuggestionsView),
        typeof(NVStatusView), typeof(NVSettingsView), typeof(NVHelpView), typeof(NVNotificationsView),
        typeof(NVDeliveryTrackView), typeof(NVAddressBookView), typeof(NVBookingView), typeof(NVDashboardView),
        typeof(NVPinLockView), typeof(NVForceUpdateView), typeof(NVSearchResultsView), typeof(NVFilterSheetView),
        typeof(NVMediaPlayerView), typeof(NVSplitInboxView), typeof(NVOnboardingPermissionsView), typeof(NVOrderSummaryView)
    ];

    public static IReadOnlyList<string> ControlNames => Controls.Select(t => t.Name).ToArray();
    public static IReadOnlyList<string> PageNames => Pages.Select(t => t.Name).ToArray();
}
