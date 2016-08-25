using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.Collections.Generic;
using Microsoft.Win32;

namespace NM_asm_highlight
{
    public enum VsTheme
    {
        Unknown = 0,
        Light,
        Dark,
        Blue
    }
    
    public class ThemeCheck
    {

        private static readonly IDictionary<string, VsTheme> Themes = new Dictionary<string, VsTheme>()
        {
            { "de3dbbcd-f642-433c-8353-8f1df4370aba", VsTheme.Light },
            { "1ded0138-47ce-435e-84ef-9ec1f439b749", VsTheme.Dark },
            { "a4d6a176-b948-4b29-8c66-53c97a1ed7d0", VsTheme.Blue }
        };
        
        public static VsTheme GetCurrentTheme()
        {
            string themeId = GetThemeId();
            if (string.IsNullOrWhiteSpace(themeId) == false)
            {
                VsTheme theme;
                if (Themes.TryGetValue(themeId, out theme))
                {
                    return theme;
                }
            }

            return VsTheme.Unknown;
        }

        public static string GetThemeId()
        {
            string[] vstypes = { "14.0" };
            string keyName = string.Format(@"Software\Microsoft\VisualStudio\{0}\ApplicationPrivateSettings\Microsoft\VisualStudio", vstypes[0]);


            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key != null)
                {
                    var keyText = (string)key.GetValue("ColorTheme", string.Empty);

                    if (!string.IsNullOrEmpty(keyText))
                    {
                        var keyTextValues = keyText.Split('*');
                        if (keyTextValues.Length > 2)
                        {
                            return keyTextValues[2];
                        }
                    }
                }
            }

            return null;
        }
    }



    #region Format definition
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_keyword")]
    [Name("nm_asm_keyword")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Keyword : ClassificationFormatDefinition
    {
        public NM_asm_Keyword()
        {
            DisplayName = "NM asm Keyword";
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#569cd6");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#1616ff");
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_directive")]
    [Name("nm_asm_directive")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Directive : ClassificationFormatDefinition
    {
        public NM_asm_Directive()
        {
            DisplayName = "NM asm Directive"; var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#49c6ad");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#2b91af");
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_label")]
    [Name("nm_asm_label")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Label : ClassificationFormatDefinition
    {
        public NM_asm_Label()
        {
            DisplayName = "NM asm Label";
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#b572e3");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#b514e3");
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_data_registers")]
    [Name("nm_asm_data_registers")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Data_registers : ClassificationFormatDefinition
    {
        public NM_asm_Data_registers()
        {
            DisplayName = "NM asm Data Registers";
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = Colors.DarkCyan;
            }
            else
            {
                ForegroundColor = Colors.DarkCyan;
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_comment")]
    [Name("nm_asm_comment")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Comment : ClassificationFormatDefinition
    {
        public NM_asm_Comment()
        {
            DisplayName = "NM asm Comment";
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#51a448");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#006400");
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_quote")]
    [Name("nm_asm_quote")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Quote : ClassificationFormatDefinition
    {
        public NM_asm_Quote()
        {
            DisplayName = "NM asm Quote";
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#d39670");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#a31515");
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_number")]
    [Name("nm_asm_number")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Number : ClassificationFormatDefinition
    {
        public NM_asm_Number()
        {
            DisplayName = "NM asm Number";
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#b5cea8");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#2f4f4f");
            }
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_custom_label")]
    [Name("nm_asm_custom_label")]
    [UserVisible(false)]
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Custom_Label: ClassificationFormatDefinition
    {
        public NM_asm_Custom_Label()
        {
            DisplayName = "NM asm custom label";
            //   ForegroundColor = Colors.DarkRed; 
            var current_theme = ThemeCheck.GetCurrentTheme();
            if (current_theme.ToString().ToLower() == "dark")
            {
                //Dark coloring
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#d37941");
            }
            else
            {
                ForegroundColor = (Color)ColorConverter.ConvertFromString("#d52828");
            }
        }
    }
    #endregion //Format definition
}
