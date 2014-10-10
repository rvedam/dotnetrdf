﻿using System.Collections.Generic;
using VDS.RDF.Query.Grouping;
using VDS.RDF.Specifications;

namespace VDS.RDF.Query.Expressions.Aggregates
{
    public class CountAllAggregate
        : BaseAggregate
    {
        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string Functor
        {
            get { return SparqlSpecsHelper.SparqlKeywordCount; }
        }

        public override IAccumulator CreateAccumulator()
        {
            return new CountAllAccumulator();
        }

        public override IExpression Copy()
        {
            return new CountAllAggregate();
        }

        public override IExpression Copy(IEnumerable<IExpression> arguments)
        {
            return Copy();
        }
    }
}
