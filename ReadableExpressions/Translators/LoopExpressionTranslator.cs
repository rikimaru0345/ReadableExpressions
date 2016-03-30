namespace AgileObjects.ReadableExpressions.Translators
{
    using System.Linq.Expressions;
    using Formatting;

    internal class LoopExpressionTranslator : ExpressionTranslatorBase
    {
        public LoopExpressionTranslator(IExpressionTranslatorRegistry registry)
            : base(registry, ExpressionType.Loop)
        {
        }

        public override string Translate(Expression expression)
        {
            var loop = (LoopExpression)expression;

            var loopBodyBlock = Registry.TranslateExpressionBody(loop.Body);

            return $"while (true){loopBodyBlock.WithBrackets()}";
        }
    }
}