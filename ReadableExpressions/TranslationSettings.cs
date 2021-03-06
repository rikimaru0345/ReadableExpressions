﻿namespace AgileObjects.ReadableExpressions
{
    using System;
#if NET35
    using Microsoft.Scripting.Ast;
#else
    using System.Linq.Expressions;
#endif


    /// <summary>
    /// Provides configuration options to control aspects of source-code string generation.
    /// </summary>
    public class TranslationSettings
    {
        internal static readonly TranslationSettings Default = new TranslationSettings();

        internal TranslationSettings()
        {
            UseImplicitGenericParameters = true;
        }

        /// <summary>
        /// Fully qualify Type names with their namespace.
        /// </summary>
        public TranslationSettings UseFullyQualifiedTypeNames
        {
            get
            {
                FullyQualifyTypeNames = true;
                return this;
            }
        }

        internal bool FullyQualifyTypeNames { get; private set; }

        /// <summary>
        /// Always specify generic parameter arguments explicitly in &lt;pointy braces&gt;
        /// </summary>
        public TranslationSettings UseExplicitGenericParameters
        {
            get
            {
                UseImplicitGenericParameters = false;
                return this;
            }
        }

        internal bool UseImplicitGenericParameters { get; private set; }

        /// <summary>
        /// Annotate a Quoted Lambda Expression with a comment indicating that it has 
        /// been Quoted.
        /// </summary>
        public TranslationSettings ShowQuotedLambdaComments
        {
            get
            {
                CommentQuotedLambdas = true;
                return this;
            }
        }

        internal bool DoNotCommentQuotedLambdas => !CommentQuotedLambdas;

        internal bool CommentQuotedLambdas { get; set; }

        /// <summary>
        /// Name anonymous types using the given <paramref name="nameFactory"/> instead of the
        /// default method.
        /// </summary>
        /// <param name="nameFactory">The factory method to execute to retrieve the name for an anonymous type.</param>
        public TranslationSettings NameAnonymousTypesUsing(Func<Type, string> nameFactory)
        {
            AnonymousTypeNameFactory = nameFactory;
            return this;
        }

        internal Func<Type, string> AnonymousTypeNameFactory { get; private set; }
				
		/// <summary>
		/// Also convert calls to property accessors (get_Name(), set_Name()) to normal property syntax.
		/// </summary>
		public TranslationSettings ConvertPropertyMethods
		{
			get
			{
				ConvertPropertyMethodsToSimpleSyntax = true;
				return this;
			}
		}

        internal bool ConvertPropertyMethodsToSimpleSyntax { get; set; }


		/// <summary>
        /// Name constant expressions using the given <paramref name="nameFactory"/> instead of the default method.
        /// </summary>
        /// <param name="nameFactory">The factory method to execute to retrieve the name for the constant.</param>
        public TranslationSettings NameConstantsUsing(Func<ConstantExpression, string> nameFactory)
        {
            ConstantExpressionNameFactory = nameFactory;
            return this;
        }

        internal Func<ConstantExpression, string> ConstantExpressionNameFactory { get; private set; }
    }
}
