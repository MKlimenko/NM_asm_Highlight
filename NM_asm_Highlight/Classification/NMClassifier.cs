﻿namespace NM_asm_highlight
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITaggerProvider))]
    [ContentType("nm_asm")]
    [TagType(typeof(ClassificationTag))]
    internal sealed class NMClassifierProvider : ITaggerProvider
    {

        [Export]
        [Name("nm_asm")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition NMContentType = null;

        [Export]
        [FileExtension(".asm")]
        [ContentType("nm_asm")]
        internal static FileExtensionToContentTypeDefinition NMFileType = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        [Import]
        internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {

            ITagAggregator<NM_TokenTag> NMTagAggregator = 
                                            aggregatorFactory.CreateTagAggregator<NM_TokenTag>(buffer);

            return new NMClassifier(NMTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
        }
    }

    internal sealed class NMClassifier : ITagger<ClassificationTag>
    {
        readonly ITagAggregator<NM_TokenTag> _aggregator;
        readonly IDictionary<NM_TokenTypes, IClassificationType> _NM_Types;
  
        /// <summary>
        /// Construct the classifier and define search tokens
        /// </summary>
        internal NMClassifier(ITagAggregator<NM_TokenTag> NMTagAggregator, 
                               IClassificationTypeRegistryService typeService)
        {
            _aggregator = NMTagAggregator;
            _NM_Types = new Dictionary<NM_TokenTypes, IClassificationType>
            {
                [NM_TokenTypes.NM_Keyword] = typeService.GetClassificationType("nm_asm_keyword"),
                [NM_TokenTypes.NM_directive] = typeService.GetClassificationType("nm_asm_directive"),
                [NM_TokenTypes.NM_label] = typeService.GetClassificationType("nm_asm_label"),
                [NM_TokenTypes.NM_data_registers] = typeService.GetClassificationType("nm_asm_data_registers"),
                [NM_TokenTypes.NM_comment] = typeService.GetClassificationType("nm_asm_comment"),
                [NM_TokenTypes.NM_quote] = typeService.GetClassificationType("nm_asm_quote"),
                [NM_TokenTypes.NM_number] = typeService.GetClassificationType("nm_asm_number"),
                [NM_TokenTypes.NM_custom_label] = typeService.GetClassificationType("nm_asm_custom_label"),
                [NM_TokenTypes.NM_Macro] = typeService.GetClassificationType("nm_asm_macro")
            };

        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Search the given span for any instances of classified tags
        /// </summary>
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
                yield return 
                    new TagSpan<ClassificationTag>(tagSpans[0],
                                                   new ClassificationTag(_NM_Types[tagSpan.Tag.type]));
            }
        }
    }
}
