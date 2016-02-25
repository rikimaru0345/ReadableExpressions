namespace AgileObjects.ReadableExpressions.Translators
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Extensions;

    internal class TryCatchExpressionTranslator : ExpressionTranslatorBase
    {
        public TryCatchExpressionTranslator(IExpressionTranslatorRegistry registry)
            : base(registry, ExpressionType.Try)
        {
        }

        public override string Translate(Expression expression)
        {
            var tryCatchFinally = (TryExpression)expression;

            var tryBody = Registry.TranslateExpressionBody(tryCatchFinally.Body);
            var catchBlocks = string.Join(Environment.NewLine, tryCatchFinally.Handlers.Select(GetCatchBlock));
            var finallyBlock = GetFinallyBlock(tryCatchFinally.Finally);

            var tryCatchFinallyBlock = $@"
try{tryBody.WithBrackets()}
{catchBlocks}{finallyBlock}";

            return tryCatchFinallyBlock.TrimStart();
        }

        private string GetCatchBlock(CatchBlock catchBlock)
        {
            var catchBody = Registry.TranslateExpressionBody(catchBlock.Body);

            var exceptionClause = GetExceptionClause(catchBlock);

            var catchBodyBlock = catchBody
                .WithBrackets()
                .Replace($"throw {catchBlock.Variable.Name};", "throw;");

            return $"catch{exceptionClause}{catchBodyBlock}";
        }

        private static string GetExceptionClause(CatchBlock catchBlock)
        {
            var exceptionTypeName = catchBlock.Variable.Type.GetFriendlyName();

            if (ExceptionUsageFinder.IsVariableUsed(catchBlock))
            {
                return $" ({exceptionTypeName} {catchBlock.Variable.Name})";
            }

            if (catchBlock.Variable.Type != typeof(Exception))
            {
                return $" ({exceptionTypeName})";
            }

            return null;
        }

        private string GetFinallyBlock(Expression finallyBlock)
        {
            if (finallyBlock == null)
            {
                return null;
            }

            var finallyBlockBody = Registry
                .TranslateExpressionBody(finallyBlock)
                .WithBrackets();

            return "finally" + finallyBlockBody;
        }

        #region ExceptionUsageFinder

        private class ExceptionUsageFinder : ExpressionVisitor
        {
            private readonly CatchBlock _catchHandler;
            private bool _rethrowFound;
            private bool _usageFound;

            private ExceptionUsageFinder(CatchBlock catchHandler)
            {
                _catchHandler = catchHandler;
            }

            public static bool IsVariableUsed(CatchBlock catchHandler)
            {
                var visitor = new ExceptionUsageFinder(catchHandler);
                visitor.Visit(catchHandler.Body);

                return visitor._usageFound;
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                _rethrowFound = node.NodeType == ExpressionType.Throw;

                return base.VisitUnary(node);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!_rethrowFound)
                {
                    if (node == _catchHandler.Variable)
                    {
                        _usageFound = true;
                    }
                }
                else
                {
                    _rethrowFound = false;
                }

                return base.VisitParameter(node);
            }
        }

        #endregion
    }
}