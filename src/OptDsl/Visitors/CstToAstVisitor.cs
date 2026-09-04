using Antlr4.Runtime.Misc;
using OptDsl.Composites.Core;
using OptDsl.Composites.Tree;
using OptDsl.Composites.Values;

namespace OptDsl.Visitors;

public class CstToAstVisitor : OptDslBaseVisitor<AstElement>
{
    public override AstElement VisitTranslation_unit([NotNull] OptDslParser.Translation_unitContext context)
    {
        AstTranslationUnit astRoot = new(context.Start.Line);

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
        AstOptimizationBlock astOptimizationBlock = new(
            context.Start.Line,
            context.STRING().GetText().Trim('"')
        );

        foreach (var stmtCtx in context.statement())
        {
            var childNode = Visit(stmtCtx);
            if (childNode != null)
            {
                astOptimizationBlock.AddChild(childNode);
            }
        }

        return astOptimizationBlock;
    }

    public override AstElement VisitScopeBlock([NotNull] OptDslParser.ScopeBlockContext context)
    {
        AstScopeBlock astScopeBlock = new(
            context.Start.Line,
            context.scope().GetText()
        );

        foreach (var stmtCtx in context.statement())
        {
            var childNode = Visit(stmtCtx);
            if (childNode != null)
            {
                astScopeBlock.AddChild(childNode);
            }
        }

        return astScopeBlock;
    }

    public override AstElement VisitOptimizationStatement([NotNull] OptDslParser.OptimizationStatementContext context)
    {
        string optName = context.ID().GetText();
        var paramList = context.parameter_list();

        AstOptimizationStatement astOptimizationStatement = new(
            context.Start.Line,
            optName
        );

        if (paramList != null)
        {
            foreach (var param in paramList.param_assignment())
            {
                var node = Visit(param);
                astOptimizationStatement.AddChild(node);
            }
        }

        return astOptimizationStatement;
    }

    public override AstElement VisitParam_assignment([NotNull] OptDslParser.Param_assignmentContext context)
    {
        string paramName = context.ID().GetText();
        
        string valText = context.value().GetText();
        ParameterValue value = int.TryParse(valText, out int intVal)
            ? new ParameterValue(intVal)
            : new ParameterValue(bool.Parse(valText));

        return new AstParameterStatement(context.Start.Line, paramName, value);
    }
}
