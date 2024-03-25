using System;

namespace uPalette.Generated
{
public enum ColorTheme
    {
        Default,
        NewTheme,
    }

    public static class ColorThemeExtensions
    {
        public static string ToThemeId(this ColorTheme theme)
        {
            switch (theme)
            {
                case ColorTheme.Default:
                    return "e9c81117-b7cb-4f69-8673-b747282a5fc3";
                case ColorTheme.NewTheme:
                    return "913ca696-7772-405e-9764-40cabcc3e128";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum ColorEntry
    {
        Primary,
        Black,
        White,
        Error,
        DateSelected,
        Text,
        Menu,
        BMI,
        Calories,
    }

    public static class ColorEntryExtensions
    {
        public static string ToEntryId(this ColorEntry entry)
        {
            switch (entry)
            {
                case ColorEntry.Primary:
                    return "ed16ed0e-dac1-49fb-a608-d88a993816b7";
                case ColorEntry.Black:
                    return "3d51e616-387d-4567-abbb-8a5780830867";
                case ColorEntry.White:
                    return "0aa5245d-b64d-41ec-9e06-85a45e3ac837";
                case ColorEntry.Error:
                    return "f9664b2f-be79-49f8-8adf-d8eab3f1af51";
                case ColorEntry.DateSelected:
                    return "e95d30b1-2e84-49c5-8d48-c23278bb8f11";
                case ColorEntry.Text:
                    return "e62d09f8-073c-41fa-92ec-62252f813220";
                case ColorEntry.Menu:
                    return "85c67598-67da-4d45-bb5f-fb3be32ca9a9";
                case ColorEntry.BMI:
                    return "525bdd84-1167-4ac2-b588-38d3d87e6555";
                case ColorEntry.Calories:
                    return "fc94772a-758d-4cf3-b875-8c0e0f7ac8dd";
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }

    public enum GradientTheme
    {
        Default,
    }

    public static class GradientThemeExtensions
    {
        public static string ToThemeId(this GradientTheme theme)
        {
            switch (theme)
            {
                case GradientTheme.Default:
                    return "f98ae734-27c2-4503-833a-4f5910332cd1";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum GradientEntry
    {
    }

    public static class GradientEntryExtensions
    {
        public static string ToEntryId(this GradientEntry entry)
        {
            switch (entry)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }

    public enum CharacterStyleTheme
    {
        Default,
    }

    public static class CharacterStyleThemeExtensions
    {
        public static string ToThemeId(this CharacterStyleTheme theme)
        {
            switch (theme)
            {
                case CharacterStyleTheme.Default:
                    return "ee785432-c643-4b21-9b05-8390c308c373";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum CharacterStyleEntry
    {
    }

    public static class CharacterStyleEntryExtensions
    {
        public static string ToEntryId(this CharacterStyleEntry entry)
        {
            switch (entry)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }

    public enum CharacterStyleTMPTheme
    {
        Default,
    }

    public static class CharacterStyleTMPThemeExtensions
    {
        public static string ToThemeId(this CharacterStyleTMPTheme theme)
        {
            switch (theme)
            {
                case CharacterStyleTMPTheme.Default:
                    return "f4f94bdb-2412-4a14-89b8-5f1c5703cd27";
                default:
                    throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
            }
        }
    }

    public enum CharacterStyleTMPEntry
    {
    }

    public static class CharacterStyleTMPEntryExtensions
    {
        public static string ToEntryId(this CharacterStyleTMPEntry entry)
        {
            switch (entry)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(entry), entry, null);
            }
        }
    }
}
