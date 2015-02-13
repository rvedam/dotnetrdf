/*
dotNetRDF is free and open source software licensed under the MIT License

-----------------------------------------------------------------------------

Copyright (c) 2009-2012 dotNetRDF Project (dotnetrdf-developer@lists.sf.net)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using VDS.RDF.Nodes;
using VDS.RDF.Query.Engine;
using VDS.RDF.Query.Expressions.Factories;

namespace VDS.RDF.Query.Expressions.Functions.Leviathan.Numeric
{
    /// <summary>
    /// Represents the Leviathan lfn:sqrt() function
    /// </summary>
    public class SquareRootFunction
        : BaseUnaryExpression
    {
        /// <summary>
        /// Creates a new Leviathan Square Root Function
        /// </summary>
        /// <param name="expr">Expression</param>
        public SquareRootFunction(IExpression expr)
            : base(expr) { }

        public override IExpression Copy(IExpression argument)
        {
            return new SquareRootFunction(argument);
        }

        /// <summary>
        /// Evaluates the expression
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override IValuedNode Evaluate(ISolution solution, IExpressionContext context)
        {
            IValuedNode temp = this.Argument.Evaluate(solution, context);
            if (temp == null) throw new RdfQueryException("Cannot square root a null");

            switch (temp.NumericType)
            {
                case EffectiveNumericType.Integer:
                case EffectiveNumericType.Decimal:
                case EffectiveNumericType.Float:
                case EffectiveNumericType.Double:
                    return new DoubleNode(Math.Sqrt(temp.AsDouble()));
                default:
                    throw new RdfQueryException("Cannot square a non-numeric argument");
            }
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public override string Functor
        {
            get
            {
                return LeviathanFunctionFactory.LeviathanFunctionsNamespace + LeviathanFunctionFactory.SquareRoot;
            }
        }
    }
}