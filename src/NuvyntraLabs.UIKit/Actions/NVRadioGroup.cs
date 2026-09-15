namespace NuvyntraLabs.UIKit;

/// <summary>Applies one <see cref="GroupName"/> to nested <see cref="NVRadioButton"/> children.</summary>
public class NVRadioGroup : VerticalStackLayout
{
    public static readonly BindableProperty GroupNameProperty = BindableProperty.Create(
        nameof(GroupName), typeof(string), typeof(NVRadioGroup), "nv-radio",
        propertyChanged: OnGroupChanged);

    public NVRadioGroup()
    {
        Spacing = NVTokens.Space2;
        ChildAdded += (_, _) => ApplyGroup();
    }

    public string GroupName
    {
        get => (string)GetValue(GroupNameProperty);
        set => SetValue(GroupNameProperty, value);
    }

    static void OnGroupChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NVRadioGroup group)
        {
            group.ApplyGroup();
        }
    }

    void ApplyGroup()
    {
        foreach (var child in Children)
        {
            if (child is NVRadioButton radio && string.IsNullOrEmpty(radio.GroupName))
            {
                radio.GroupName = GroupName;
            }
        }
    }
}
