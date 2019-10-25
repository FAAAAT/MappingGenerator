using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;

namespace MappingGenerator.Features.Refactorings.Mapping.MappingImplementors
{
    public class EqualsMethodImplementors : IMappingMethodImplementor
    {
        public bool CanImplement(IMethodSymbol methodSymbol)
        {
            return methodSymbol.Name == "Equals" && methodSymbol.IsOverride == true;
        }

        public IEnumerable<SyntaxNode> GenerateImplementation(IMethodSymbol methodSymbol, SyntaxGenerator generator, SemanticModel semanticModel)
        {
            var obj = methodSymbol.Parameters[0];
            var thisType = methodSymbol.ReceiverType;
            List<SyntaxNode> nodes = new List<SyntaxNode>();

            var typeCompareStatement = generator.ValueNotEqualsExpression(generator.InvocationExpression(generator.MemberAccessExpression(generator.ThisExpression(),"GetType")), generator.InvocationExpression(generator.MemberAccessExpression(generator.IdentifierName(obj.Name), "GetType")));
            var trueStatement = generator.ReturnStatement(generator.FalseLiteralExpression());

            nodes.Add(generator.IfStatement(typeCompareStatement, new[] { trueStatement }).WithAdditionalAnnotations(Formatter.Annotation));
            var localVariableObjName = "unboxedObj";
            nodes.Add(generator.LocalDeclarationStatement(localVariableObjName,generator.ConvertExpression(thisType,generator.IdentifierName(obj.Name))));

            var memberSymbols = ObjectHelper.GetFieldsAndPropertiesThatCanBeReadPublicly(thisType);
            foreach (ISymbol symbol in memberSymbols) {
                if (symbol.Kind == SymbolKind.Property)
                {
                    var mSymbol = symbol as IPropertySymbol;
                    if (ObjectHelper.IsSimpleType(mSymbol.Type))
                    {
                        nodes.Add(generator.IfStatement(generator.LogicalNotExpression(generator.ValueEqualsExpression(generator.MemberAccessExpression(generator.ThisExpression(), mSymbol.Name), generator.MemberAccessExpression(generator.IdentifierName(localVariableObjName), mSymbol.Name))), new[] { generator.ReturnStatement(generator.FalseLiteralExpression()) }));
                    }
                    else if (MappingHelper.IsCollection(mSymbol.Type)) {
                        var elementType = MappingHelper.GetElementType(mSymbol.Type);
                        if (ObjectHelper.IsSimpleType(elementType))
                        {
                            nodes.Add(generator.IfStatement(generator.LogicalNotExpression(generator.InvocationExpression(generator.IdentifierName("Enumerable.SequenceEqual"), generator.MemberAccessExpression(generator.ThisExpression(), mSymbol.Name), generator.MemberAccessExpression(generator.IdentifierName(localVariableObjName), mSymbol.Name))), new[] { generator.ReturnStatement(generator.FalseLiteralExpression()) }));
                        }
                        //is indexer support foreach? if not, must create the spec method to compare indexer.
                        else
                        {
                           // for collections element type is not valuetype, invoke their equals method to compare.
                        }
                    }
                    else
                    {
                        nodes.Add(generator.IfStatement(generator.LogicalNotExpression(generator.InvocationExpression(generator.MemberAccessExpression(generator.MemberAccessExpression(generator.ThisExpression(), mSymbol.Name), "Equals"), generator.MemberAccessExpression(generator.IdentifierName(localVariableObjName), mSymbol.Name))), new[] { generator.ReturnStatement(generator.FalseLiteralExpression()) }));
                    }

                }
                else if (symbol.Kind == SymbolKind.Field) {
                    var mSymbol = symbol as IFieldSymbol;
                    if (ObjectHelper.IsSimpleType(mSymbol.Type))
                    {
                        nodes.Add(generator.IfStatement(generator.LogicalNotExpression(generator.ValueEqualsExpression(generator.MemberAccessExpression(generator.ThisExpression(), mSymbol.Name), generator.MemberAccessExpression(generator.IdentifierName(localVariableObjName), mSymbol.Name))), new[] { generator.ReturnStatement(generator.FalseLiteralExpression()) }));
                    }
                    else if (MappingHelper.IsCollection(mSymbol.Type))
                    {
                        var elementType = MappingHelper.GetElementType(mSymbol.Type);
                        if (ObjectHelper.IsSimpleType(elementType))
                        {
                            nodes.Add(generator.IfStatement(generator.LogicalNotExpression(generator.InvocationExpression(generator.IdentifierName("Enumerable.SequenceEqual"), generator.MemberAccessExpression(generator.ThisExpression(), mSymbol.Name), generator.MemberAccessExpression(generator.IdentifierName(localVariableObjName), mSymbol.Name))), new[] { generator.ReturnStatement(generator.FalseLiteralExpression()) }));
                        }
                    }
                    else
                    {
                        nodes.Add(generator.IfStatement(generator.LogicalNotExpression(generator.InvocationExpression(generator.MemberAccessExpression(generator.MemberAccessExpression(generator.ThisExpression(), mSymbol.Name), "Equals"), generator.MemberAccessExpression(generator.IdentifierName(localVariableObjName), mSymbol.Name))), new[] { generator.ReturnStatement(generator.FalseLiteralExpression()) }));
                    }
                }
            }
            nodes.Add(generator.ReturnStatement(generator.TrueLiteralExpression()));
            return nodes;
        }
    }
}
