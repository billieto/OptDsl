using System.Diagnostics;
using Antlr4.Runtime.Misc;
using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;
using OptDsl.Composites.Values;

namespace OptDsl.Visitors;

public class CstToAstVisitor : OptDslBaseVisitor<AstElement>
{
    public override AstElement VisitTranslation_unit([NotNull] OptDslParser.Translation_unitContext context)
    {
        AstTranslationUnit astRoot = new();

        foreach (var blockCtx in context.optimization_block())
        {
            var childNode = Visit(blockCtx);
            if (childNode != null)
            {
                astRoot.AddChild(childNode);
            }
        }

        return astRoot;
    }

    public override AstElement VisitOptimization_block([NotNull] OptDslParser.Optimization_blockContext context)
    {
        AstOptimizationBlock astOptimizationBlock = new(context.STRING().GetText().Trim('"'));

        foreach (var stmtCtx in context.statement_list().statement())
        {
            var childNode = Visit(stmtCtx.optimization_statement());
            if (childNode != null)
            {
                astOptimizationBlock.AddChild(childNode);
            }
        }

        return astOptimizationBlock;
    }

    public override AstElement VisitOptimization_statement([NotNull] OptDslParser.Optimization_statementContext context)
    {
        string optName = context.optimization().GetText();
        string paramName = context.parameter().GetText();
        ParameterValue paramValue;

        var valueCtx = context.value();
        if (valueCtx.INT() != null)
        {
            paramValue = int.Parse(valueCtx.INT().GetText());
        }
        else if (valueCtx.BOOL() != null)
        {
            paramValue = bool.Parse(valueCtx.BOOL().GetText());
        }
        else
        {
            throw new UnreachableException();
        }

        AstOptimizationStatement astOptimizationStatement = new(optName, paramName, paramValue);

        return astOptimizationStatement;
    }
}
