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
using System.Globalization;
using System.Linq;
using VDS.RDF.Nodes;
using VDS.RDF.Query.Engine;
using VDS.RDF.Specifications;

namespace VDS.RDF.Query.Expressions.Functions.XPath.Cast
{
    /// <summary>
    /// Class representing an XPath Decimal Cast Function
    /// </summary>
    public class DecimalCast
        : BaseCast
    {
        /// <summary>
        /// Creates a new XPath Decimal Cast Function Expression
        /// </summary>
        /// <param name="expr">Expression to be cast</param>
        public DecimalCast(IExpression expr)
            : base(expr) { }

        public override IExpression Copy(IExpression argument)
        {
            return new DecimalCast(argument);
        }

        /// <summary>
        /// Casts the Value of the inner Expression to a Decimal
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override IValuedNode Evaluate(ISolution solution, IExpressionContext context)
        {
            IValuedNode n = this.Argument.Evaluate(solution, context);

            if (n == null)
            {
                throw new RdfQueryException("Cannot cast a Null to a xsd:decimal");
            }

            switch (n.NodeType)
            {
                case NodeType.Blank:
                case NodeType.GraphLiteral:
                case NodeType.Uri:
                    throw new RdfQueryException("Cannot cast a Blank/URI/Graph Literal Node to a xsd:decimal");

                case NodeType.Literal:
                    if (n is DecimalNode) return n;
                    //See if the value can be cast
                    INode lit = n;
                    if (lit.DataType != null)
                    {
                        string dt = lit.DataType.ToString();
                        if (XmlSpecsHelper.IntegerDataTypes.Contains(dt))
                        {
                            //Already an integer type so valid as a xsd:decimal
                            decimal d;
                            if (Decimal.TryParse(lit.Value, NumberStyles.Any ^ NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out d))
                            {
                                //Parsed OK
                                return new DecimalNode(d);
                            }
                            else
                            {
                                throw new RdfQueryException("Invalid lexical form for xsd:decimal");
                            }
                        }
                        else if (dt.Equals(XmlSpecsHelper.XmlSchemaDataTypeDateTime))
                        {
                            //DateTime cast forbidden
                            throw new RdfQueryException("Cannot cast a xsd:dateTime to a xsd:decimal");
                        }
                        else
                        {
                            decimal d;
                            if (Decimal.TryParse(lit.Value, NumberStyles.Any ^ NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out d))
                            {
                                //Parsed OK
                                return new DecimalNode(d);
                            }
                            else
                            {
                                throw new RdfQueryException("Cannot cast the value '" + lit.Value + "' to a xsd:decimal");
                            }
                        }
                    }
                    else
                    {
                        decimal d;
                        if (Decimal.TryParse(lit.Value, NumberStyles.Any ^ NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out d))
                        {
                            //Parsed OK
                            return new DecimalNode(d);
                        }
                        else
                        {
                            throw new RdfQueryException("Cannot cast the value '" + lit.Value + "' to a xsd:decimal");
                        }
                    }
                default:
                    throw new RdfQueryException("Cannot cast an Unknown Node to a xsd:decimal");
            }
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public override string Functor
        {
            get
            {
                return XmlSpecsHelper.XmlSchemaDataTypeDecimal;
            }
        }
    }
}
