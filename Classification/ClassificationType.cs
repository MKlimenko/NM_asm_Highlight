using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace NM_asm_Language
{
    internal static class OrdinaryClassificationDefinition
    {
        #region Type definition

        /// <summary>
        /// Defines the "ookExclamation" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("nm_asm_keyword")]
        internal static ClassificationTypeDefinition NM_asm_Keyword = null;

        /// <summary>
        /// Defines the "ookQuestion" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("nm_asm_directive")]
        internal static ClassificationTypeDefinition NM_asm_Directive = null;

        /// <summary>
        /// Defines the "ookPeriod" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("nm_asm_label")]
        internal static ClassificationTypeDefinition NM_asm_Label = null;

        /// <summary>
        /// Defines the "ookPeriod" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("nm_asm_data_registers")]
        internal static ClassificationTypeDefinition NM_asm_Data_registers = null;

        #endregion
    }
}
