namespace AgileObjects.ReadableExpressions.UnitTests
{
    using System;
#if !NET35
    using System.Linq.Expressions;
    using Xunit;
#else
    using Microsoft.Scripting.Ast;
    using Fact = NUnit.Framework.TestAttribute;

    [NUnit.Framework.TestFixture]
#endif
    public class WhenTranslatingLogicalOperations : TestClassBase
    {
        [Fact]
        public void ShouldTranslateAnAndOperation()
        {
            var bothBoolsAreTheSame = CreateLambda((bool b1, bool b2) => b1 && b2);

            var translated = ToReadableString(bothBoolsAreTheSame.Body);

            translated.ShouldBe("b1 && b2");
        }

        [Fact]
        public void ShouldTranslateABitwiseAndOperation()
        {
            var bitwiseAnd = CreateLambda((bool b1, bool b2) => b1 & b2);

            var translated = ToReadableString(bitwiseAnd.Body);

            translated.ShouldBe("b1 & b2");
        }

        [Fact]
        public void ShouldTranslateAnOrOperation()
        {
            var eitherBoolsIsTrue = CreateLambda((bool b1, bool b2) => b1 || b2);

            var translated = ToReadableString(eitherBoolsIsTrue.Body);

            translated.ShouldBe("b1 || b2");
        }

        [Fact]
        public void ShouldTranslateABitwiseOrOperation()
        {
            var bitwiseOr = CreateLambda((bool b1, bool b2) => b1 | b2);

            var translated = ToReadableString(bitwiseOr.Body);

            translated.ShouldBe("b1 | b2");
        }

        [Fact]
        public void ShouldTranslateABitwiseExclusiveOrOperation()
        {
            var bitwiseExclusiveOr = CreateLambda((bool b1, bool b2) => b1 ^ b2);

            var translated = ToReadableString(bitwiseExclusiveOr.Body);

            translated.ShouldBe("b1 ^ b2");
        }

        [Fact]
        public void ShouldTranslateABitwiseLeftShiftOperation()
        {
            var bitwiseLeftShift = CreateLambda((int i1, int i2) => i1 << i2);

            var translated = ToReadableString(bitwiseLeftShift.Body);

            translated.ShouldBe("i1 << i2");
        }

        [Fact]
        public void ShouldTranslateABitwiseRightShiftOperation()
        {
            var bitwiseRightShift = CreateLambda((int i1, int i2) => i1 >> i2);

            var translated = ToReadableString(bitwiseRightShift.Body);

            translated.ShouldBe("i1 >> i2");
        }

        [Fact]
        public void ShouldTranslateAUnaryPlusOperation()
        {
            var intVariable = Expression.Variable(typeof(int), "i");
            var unaryPlus = Expression.UnaryPlus(intVariable);

            var translated = ToReadableString(unaryPlus);

            translated.ShouldBe("+i");
        }

        [Fact]
        public void ShouldTranslateAOnesComplementOperation()
        {
            var intVariable = Expression.Variable(typeof(int), "i");
            var onesComplement = Expression.OnesComplement(intVariable);

            var translated = ToReadableString(onesComplement);

            translated.ShouldBe("~i");
        }

        [Fact]
        public void ShouldTranslateACoalesceOperation()
        {
            var oneOrTwo = CreateLambda((bool? b1, bool b2) => b1 ?? b2);

            var translated = ToReadableString(oneOrTwo.Body);

            translated.ShouldBe("b1 ?? b2");
        }

        [Fact]
        public void ShouldTranslateAConditionalOperation()
        {
            var whatSize = CreateLambda((int i) => (i < 8) ? "Too small" : "Too big");

            var translated = ToReadableString(whatSize.Body);

            translated.ShouldBe("(i < 8) ? \"Too small\" : \"Too big\"");
        }

        [Fact]
        public void ShouldTranslateAnIsTypeExpression()
        {
            var objectIsDisposable = CreateLambda((object o) => o is IDisposable);

            var translated = ToReadableString(objectIsDisposable.Body);

            translated.ShouldBe("o is IDisposable");
        }

        [Fact]
        public void ShouldTranslateAValueTypeTypeEqualExpression()
        {
            var intVariable = Expression.Variable(typeof(int), "i");
            var intIsLong = Expression.TypeEqual(intVariable, typeof(long));

            var translated = ToReadableString(intIsLong);

            translated.ShouldBe("false");
        }

        [Fact]
        public void ShouldTranslateANullableValueTypeTypeEqualExpression()
        {
            var nullableLongVariable = Expression.Variable(typeof(long?), "l");
            var nullableLongIsNullableLong = Expression.TypeEqual(nullableLongVariable, typeof(long?));

            var translated = ToReadableString(nullableLongIsNullableLong);

            translated.ShouldBe("l != null");
        }

        [Fact]
        public void ShouldTranslateAConstantTypeEqualExpression()
        {
            var intConstant = Expression.Constant(123, typeof(int));
            var intConstantIsInt = Expression.TypeEqual(intConstant, typeof(int));

            var translated = ToReadableString(intConstantIsInt);

            translated.ShouldBe("true");
        }

        [Fact]
        public void ShouldTranslateAnObjectTypeEqualExpression()
        {
            var objectVariable = Expression.Variable(typeof(object), "o");
            var objectIsString = Expression.TypeEqual(objectVariable, typeof(string));

            var translated = ToReadableString(objectIsString);

            translated.ShouldBe("(o != null) && (o.GetType() == typeof(string))");
        }

        [Fact]
        public void ShouldTranslateAnIsTrueExpression()
        {
            var boolVariable = Expression.Variable(typeof(bool), "b");
            var boolIsTrue = Expression.IsTrue(boolVariable);

            var translated = ToReadableString(boolIsTrue);

            translated.ShouldBe("b");
        }

        [Fact]
        public void ShouldTranslateAnIsFalseExpression()
        {
            var boolVariable = Expression.Variable(typeof(bool), "b");
            var boolIsFalse = Expression.IsFalse(boolVariable);

            var translated = ToReadableString(boolIsFalse);

            translated.ShouldBe("!b");
        }
    }
}