using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace NM_asm_highlight
{
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
            DisplayName = "NM asm Keyword"; //human readable version of the name
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#3e3ef1");
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
            DisplayName = "NM asm Directive"; //human readable version of the name
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#2b91af");
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
            DisplayName = " NM asm Label"; //human readable version of the name
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#b514e3");
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
            DisplayName = "NM asm Data Registers"; //human readable version of the name
            ForegroundColor = Colors.DarkCyan;
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
            DisplayName = "NM asm Comment"; //human readable version of the name
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#006400");
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
            DisplayName = "NM asm Quote"; //human readable version of the name
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#a31515");
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
            DisplayName = "NM asm Number"; //human readable version of the name
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#2f4f4f");
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
            DisplayName = "NM asm custom label"; //human readable version of the name
         //   ForegroundColor = Colors.DarkRed; 
            ForegroundColor = (Color)ColorConverter.ConvertFromString("#d52828");
        }
    }
    #endregion //Format definition
}
