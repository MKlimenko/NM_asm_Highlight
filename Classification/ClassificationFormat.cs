using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace NM_asm_Language
{
    #region Format definition
    /// <summary>
    /// Defines the editor format for the ookExclamation classification type. Text is colored BlueViolet
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_keyword")]
    [Name("nm_asm_keyword")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Keyword : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "exclamation" classification type
        /// </summary>
        public NM_asm_Keyword()
        {
            DisplayName = "NM asm Keyword"; //human readable version of the name
            ForegroundColor = Colors.Blue;
        }
    }

    /// <summary>
    /// Defines the editor format for the ookQuestion classification type. Text is colored Green
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_directive")]
    [Name("nm_asm_directive")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Directive : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "question" classification type
        /// </summary>
        public NM_asm_Directive()
        {
            DisplayName = "NM asm Directive"; //human readable version of the name
            ForegroundColor = Colors.DarkCyan;
        }
    }

    /// <summary>
    /// Defines the editor format for the ookPeriod classification type. Text is colored Orange
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_label")]
    [Name("nm_asm_label")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Label : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "period" classification type
        /// </summary>
        public NM_asm_Label()
        {
            DisplayName = " NM asm Label"; //human readable version of the name
            ForegroundColor = Colors.Coral;
        }
    }

    /// <summary>
    /// Defines the editor format for the ookPeriod classification type. Text is colored Orange
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_data_registers")]
    [Name("nm_asm_data_registers")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Data_registers : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "period" classification type
        /// </summary>
        public NM_asm_Data_registers()
        {
            DisplayName = "NM asm Data Registers"; //human readable version of the name
            ForegroundColor = Colors.DarkGreen;
        }
    }

    /// <summary>
    /// Defines the editor format for the ookPeriod classification type. Text is colored Orange
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_comment")]
    [Name("nm_asm_comment")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Comment : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "period" classification type
        /// </summary>
        public NM_asm_Comment()
        {
            DisplayName = "NM asm Comment"; //human readable version of the name
            ForegroundColor = Colors.Green;
        }
    }

    /// <summary>
    /// Defines the editor format for the ookPeriod classification type. Text is colored Orange
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_quote")]
    [Name("nm_asm_quote")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Quote : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "period" classification type
        /// </summary>
        public NM_asm_Quote()
        {
            DisplayName = "NM asm Quote"; //human readable version of the name
            ForegroundColor = Colors.Red;
        }
    }

    /// <summary>
    /// Defines the editor format for the ookPeriod classification type. Text is colored Orange
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "nm_asm_number")]
    [Name("nm_asm_number")]
    //this should be visible to the end user
    [UserVisible(false)]
    //set the priority to be after the default classifiers
    [Order(Before = Priority.Default)]
    internal sealed class NM_asm_Number : ClassificationFormatDefinition
    {
        /// <summary>
        /// Defines the visual format for the "period" classification type
        /// </summary>
        public NM_asm_Number()
        {
            DisplayName = "NM asm Number"; //human readable version of the name
            ForegroundColor = Colors.DarkGray;
        }
    }
    #endregion //Format definition
}
