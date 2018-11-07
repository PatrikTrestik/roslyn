﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis.FlowAnalysis;

namespace Microsoft.CodeAnalysis.Operations
{
    /// <summary>
    /// Represents an operation that creates a pointer value by taking the address of a reference.
    /// </summary>
    internal abstract partial class BaseAddressOfOperation : Operation, IAddressOfOperation
    {
        protected BaseAddressOfOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.AddressOf, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Reference != null)
                {
                    yield return Reference;
                }
            }
        }
        /// <summary>
        /// Addressed reference.
        /// </summary>
        public abstract IOperation Reference { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitAddressOf(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitAddressOf(this, argument);
        }
    }
    /// <summary>
    /// Represents an operation that creates a pointer value by taking the address of a reference.
    /// </summary>
    internal sealed partial class AddressOfOperation : BaseAddressOfOperation, IAddressOfOperation
    {
        public AddressOfOperation(IOperation reference, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Reference = SetParentOperation(reference, this);
        }

        public override IOperation Reference { get; }
    }
/// <summary>
/// Represents an operation that creates a pointer value by taking the address of a reference.
/// </summary>
internal abstract class LazyAddressOfOperation : BaseAddressOfOperation, IAddressOfOperation
{
    private IOperation _lazyReferenceInterlocked;

    public LazyAddressOfOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateReference();

    public override IOperation Reference
    {
        get
        {
            if (_lazyReferenceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyReferenceInterlocked, CreateReference(), null);
                VerifyParentOperation(this, _lazyReferenceInterlocked);
            }

            return _lazyReferenceInterlocked;
        }
    }
}

    /// <summary>
    /// Represents C# nameof and VB NameOf expression.
    /// </summary>
    internal abstract partial class BaseNameOfOperation : Operation, INameOfOperation
    {
        protected BaseNameOfOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.NameOf, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Argument != null)
                {
                    yield return Argument;
                }
            }
        }
        /// <summary>
        /// Argument to name of expression.
        /// </summary>
        public abstract IOperation Argument { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitNameOf(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitNameOf(this, argument);
        }
    }
    /// <summary>
    /// Represents C# nameof and VB NameOf expression.
    /// </summary>
    internal sealed partial class NameOfOperation : BaseNameOfOperation, INameOfOperation
    {
        public NameOfOperation(IOperation argument, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Argument = SetParentOperation(argument, this);
        }

        public override IOperation Argument { get; }
    }
/// <summary>
/// Represents C# nameof and VB NameOf expression.
/// </summary>
internal abstract class LazyNameOfOperation : BaseNameOfOperation, INameOfOperation
{
    private IOperation _lazyArgumentInterlocked;

    public LazyNameOfOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateArgument();

    public override IOperation Argument
    {
        get
        {
            if (_lazyArgumentInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyArgumentInterlocked, CreateArgument(), null);
                VerifyParentOperation(this, _lazyArgumentInterlocked);
            }

            return _lazyArgumentInterlocked;
        }
    }
}

    /// <summary>
    /// Represents C# throw expression.
    /// </summary>
    internal abstract partial class BaseThrowOperation : Operation, IThrowOperation
    {
        protected BaseThrowOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Throw, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Exception != null)
                {
                    yield return Exception;
                }
            }
        }
        /// <summary>
        /// Expression.
        /// </summary>
        public abstract IOperation Exception { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitThrow(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitThrow(this, argument);
        }
    }
    /// <summary>
    /// Represents C# throw expression.
    /// </summary>
    internal sealed partial class ThrowOperation : BaseThrowOperation, IThrowOperation
    {
        public ThrowOperation(IOperation exception, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Exception = SetParentOperation(exception, this);

        }

        public override IOperation Exception { get; }
    }
/// <summary>
/// Represents C# throw expression.
/// </summary>
internal abstract class LazyThrowOperation : BaseThrowOperation, IThrowOperation
{
    private IOperation _lazyExceptionInterlocked;

    public LazyThrowOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateException();

    public override IOperation Exception
    {
        get
        {
            if (_lazyExceptionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyExceptionInterlocked, CreateException(), null);
                VerifyParentOperation(this, _lazyExceptionInterlocked);
            }

            return _lazyExceptionInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an argument in a method invocation.
    /// </summary>
    internal abstract partial class BaseArgumentOperation : Operation, IArgumentOperation
    {
        protected BaseArgumentOperation(ArgumentKind argumentKind, IParameterSymbol parameter, IConvertibleConversion inConversionOpt, IConvertibleConversion outConversionOpt, SemanticModel semanticModel, SyntaxNode syntax, bool isImplicit) :
                    base(OperationKind.Argument, semanticModel, syntax, type: null, constantValue: default, isImplicit: isImplicit)
        {
            ArgumentKind = argumentKind;
            Parameter = parameter;
            InConversionConvertibleOpt = inConversionOpt;
            OutConversionConvertibleOpt = outConversionOpt;
        }
        /// <summary>
        /// Kind of argument.
        /// </summary>
        public ArgumentKind ArgumentKind { get; }
        /// <summary>
        /// Parameter the argument matches.
        /// </summary>
        public IParameterSymbol Parameter { get; }

        internal IConvertibleConversion InConversionConvertibleOpt { get; }
        internal IConvertibleConversion OutConversionConvertibleOpt { get; }
        public CommonConversion InConversion => InConversionConvertibleOpt?.ToCommonConversion() ?? Identity();
        public CommonConversion OutConversion => OutConversionConvertibleOpt?.ToCommonConversion() ?? Identity();

        private static CommonConversion Identity()
        {
            return new CommonConversion(exists: true, isIdentity: true, isNumeric: false, isReference: false, methodSymbol: null, isImplicit: true);
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }
        /// <summary>
        /// Value supplied for the argument.
        /// </summary>
        public abstract IOperation Value { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitArgument(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitArgument(this, argument);
        }
    }

    internal sealed partial class ArgumentOperation : BaseArgumentOperation
    {
        public ArgumentOperation(IOperation value, ArgumentKind argumentKind, IParameterSymbol parameter, IConvertibleConversion inConversionOpt, IConvertibleConversion outConversionOpt, SemanticModel semanticModel, SyntaxNode syntax, bool isImplicit) :
            base(argumentKind, parameter, inConversionOpt, outConversionOpt, semanticModel, syntax, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }

        public override IOperation Value { get; }
    }

internal abstract class LazyArgumentOperation : BaseArgumentOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyArgumentOperation(ArgumentKind argumentKind, IConvertibleConversion inConversionOpt, IConvertibleConversion outConversionOpt, IParameterSymbol parameter, SemanticModel semanticModel, SyntaxNode syntax, bool isImplicit) : base(argumentKind, parameter, inConversionOpt, outConversionOpt, semanticModel, syntax, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents the creation of an array instance.
    /// </summary>
    internal abstract partial class BaseArrayCreationOperation : Operation, IArrayCreationOperation
    {
        protected BaseArrayCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ArrayCreation, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var dimensionSize in DimensionSizes)
                {
                    if (dimensionSize != null)
                    {
                        yield return dimensionSize;
                    }
                }
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }
        /// <summary>
        /// Sizes of the dimensions of the created array instance.
        /// </summary>
        public abstract ImmutableArray<IOperation> DimensionSizes { get; }
        /// <summary>
        /// Values of elements of the created array instance.
        /// </summary>
        public abstract IArrayInitializerOperation Initializer { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitArrayCreation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitArrayCreation(this, argument);
        }
    }

    /// <summary>
    /// Represents the creation of an array instance.
    /// </summary>
    internal sealed partial class ArrayCreationOperation : BaseArrayCreationOperation, IArrayCreationOperation
    {
        public ArrayCreationOperation(ImmutableArray<IOperation> dimensionSizes, IArrayInitializerOperation initializer, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            DimensionSizes = SetParentOperation(dimensionSizes, this);
            Initializer = SetParentOperation(initializer, this);
        }

        public override ImmutableArray<IOperation> DimensionSizes { get; }
        public override IArrayInitializerOperation Initializer { get; }
    }

/// <summary>
/// Represents the creation of an array instance.
/// </summary>
internal abstract class LazyArrayCreationOperation : BaseArrayCreationOperation, IArrayCreationOperation
{
    private ImmutableArray<IOperation> _lazyDimensionSizesInterlocked;
    private IArrayInitializerOperation _lazyInitializerInterlocked;

    public LazyArrayCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateDimensionSizes();
    protected abstract IArrayInitializerOperation CreateInitializer();

    public override ImmutableArray<IOperation> DimensionSizes
    {
        get
        {
            if (_lazyDimensionSizesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyDimensionSizesInterlocked, CreateDimensionSizes(), default);
                VerifyParentOperation(this, _lazyDimensionSizesInterlocked);
            }

            return _lazyDimensionSizesInterlocked;
        }
    }

    public override IArrayInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a reference to an array element.
    /// </summary>
    internal abstract partial class BaseArrayElementReferenceOperation : Operation, IArrayElementReferenceOperation
    {
        protected BaseArrayElementReferenceOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ArrayElementReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (ArrayReference != null)
                {
                    yield return ArrayReference;
                }

                foreach (var index in Indices)
                {
                    if (index != null)
                    {
                        yield return index;
                    }
                }
            }
        }
        /// <summary>
        /// Array to be indexed.
        /// </summary>
        public abstract IOperation ArrayReference { get; }
        /// <summary>
        /// Indices that specify an individual element.
        /// </summary>
        public abstract ImmutableArray<IOperation> Indices { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitArrayElementReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitArrayElementReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a reference to an array element.
    /// </summary>
    internal sealed partial class ArrayElementReferenceOperation : BaseArrayElementReferenceOperation, IArrayElementReferenceOperation
    {
        public ArrayElementReferenceOperation(IOperation arrayReference, ImmutableArray<IOperation> indices, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            ArrayReference = SetParentOperation(arrayReference, this);
            Indices = SetParentOperation(indices, this);
        }

        public override IOperation ArrayReference { get; }
        public override ImmutableArray<IOperation> Indices { get; }
    }

/// <summary>
/// Represents a reference to an array element.
/// </summary>
internal abstract class LazyArrayElementReferenceOperation : BaseArrayElementReferenceOperation, IArrayElementReferenceOperation
{
    private IOperation _lazyArrayReferenceInterlocked;
    private ImmutableArray<IOperation> _lazyIndicesInterlocked;

    public LazyArrayElementReferenceOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateArrayReference();
    protected abstract ImmutableArray<IOperation> CreateIndices();

    public override IOperation ArrayReference
    {
        get
        {
            if (_lazyArrayReferenceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyArrayReferenceInterlocked, CreateArrayReference(), null);
                VerifyParentOperation(this, _lazyArrayReferenceInterlocked);
            }

            return _lazyArrayReferenceInterlocked;
        }
    }

    public override ImmutableArray<IOperation> Indices
    {
        get
        {
            if (_lazyIndicesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyIndicesInterlocked, CreateIndices(), default);
                VerifyParentOperation(this, _lazyIndicesInterlocked);
            }

            return _lazyIndicesInterlocked;
        }
    }
}

    /// <summary>
    /// Represents the initialization of an array instance.
    /// </summary>
    internal abstract partial class BaseArrayInitializerOperation : Operation, IArrayInitializerOperation
    {
        protected BaseArrayInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ArrayInitializer, semanticModel, syntax, type: null, constantValue: constantValue, isImplicit: isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var elementValue in ElementValues)
                {
                    if (elementValue != null)
                    {
                        yield return elementValue;
                    }
                }
            }
        }
        /// <summary>
        /// Values to initialize array elements.
        /// </summary>
        public abstract ImmutableArray<IOperation> ElementValues { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitArrayInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitArrayInitializer(this, argument);
        }
    }

    /// <summary>
    /// Represents the initialization of an array instance.
    /// </summary>
    internal sealed partial class ArrayInitializerOperation : BaseArrayInitializerOperation, IArrayInitializerOperation
    {
        public ArrayInitializerOperation(ImmutableArray<IOperation> elementValues, SemanticModel semanticModel, SyntaxNode syntax, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, constantValue, isImplicit)
        {
            ElementValues = SetParentOperation(elementValues, this);
        }

        public override ImmutableArray<IOperation> ElementValues { get; }
    }

/// <summary>
/// Represents the initialization of an array instance.
/// </summary>
internal abstract class LazyArrayInitializerOperation : BaseArrayInitializerOperation, IArrayInitializerOperation
{
    private ImmutableArray<IOperation> _lazyElementValuesInterlocked;

    public LazyArrayInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateElementValues();

    public override ImmutableArray<IOperation> ElementValues
    {
        get
        {
            if (_lazyElementValuesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyElementValuesInterlocked, CreateElementValues(), default);
                VerifyParentOperation(this, _lazyElementValuesInterlocked);
            }

            return _lazyElementValuesInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an base type of assignment expression.
    /// </summary>
    internal abstract partial class AssignmentOperation : Operation, IAssignmentOperation
    {
        protected AssignmentOperation(OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                if (Target != null)
                {
                    yield return Target;
                }
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }
        /// <summary>
        /// Target of the assignment.
        /// </summary>
        public abstract IOperation Target { get; }
        /// <summary>
        /// Value to be assigned to the target of the assignment.
        /// </summary>
        public abstract IOperation Value { get; }
    }

    /// <summary>
    /// Represents a simple assignment expression.
    /// </summary>
    internal abstract partial class BaseSimpleAssignmentOperation : AssignmentOperation, ISimpleAssignmentOperation
    {
        public BaseSimpleAssignmentOperation(bool isRef, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.SimpleAssignment, semanticModel, syntax, type, constantValue, isImplicit)
        {
            IsRef = isRef;
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitSimpleAssignment(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitSimpleAssignment(this, argument);
        }

        /// <summary>
        /// Is this a ref assignment
        /// </summary>
        public bool IsRef { get; }
    }

    /// <summary>
    /// Represents a simple assignment expression.
    /// </summary>
    internal sealed partial class SimpleAssignmentOperation : BaseSimpleAssignmentOperation, ISimpleAssignmentOperation
    {
        public SimpleAssignmentOperation(IOperation target, bool isRef, IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(isRef, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = SetParentOperation(target, this);
            Value = SetParentOperation(value, this);
        }
        public override IOperation Target { get; }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents a simple assignment expression.
/// </summary>
internal abstract class LazySimpleAssignmentOperation : BaseSimpleAssignmentOperation, ISimpleAssignmentOperation
{
    private IOperation _lazyTargetInterlocked;
    private IOperation _lazyValueInterlocked;

    public LazySimpleAssignmentOperation(bool isRef, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(isRef, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateTarget();
    protected abstract IOperation CreateValue();

    public override IOperation Target
    {
        get
        {
            if (_lazyTargetInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTargetInterlocked, CreateTarget(), null);
                VerifyParentOperation(this, _lazyTargetInterlocked);
            }

            return _lazyTargetInterlocked;
        }
    }

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a deconstruction assignment expression.
    /// </summary>
    internal abstract partial class BaseDeconstructionAssignmentOperation : AssignmentOperation, IDeconstructionAssignmentOperation
    {
        public BaseDeconstructionAssignmentOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DeconstructionAssignment, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDeconstructionAssignment(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDeconstructionAssignment(this, argument);
        }
    }

    /// <summary>
    /// Represents a deconstruction assignment expression.
    /// </summary>
    internal sealed partial class DeconstructionAssignmentOperation : BaseDeconstructionAssignmentOperation, IDeconstructionAssignmentOperation
    {
        public DeconstructionAssignmentOperation(IOperation target, IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = SetParentOperation(target, this);
            Value = SetParentOperation(value, this);
        }
        public override IOperation Target { get; }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents a deconstruction assignment expression.
/// </summary>
internal abstract class LazyDeconstructionAssignmentOperation : BaseDeconstructionAssignmentOperation, IDeconstructionAssignmentOperation
{
    private IOperation _lazyTargetInterlocked;
    private IOperation _lazyValueInterlocked;

    public LazyDeconstructionAssignmentOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateTarget();
    protected abstract IOperation CreateValue();

    public override IOperation Target
    {
        get
        {
            if (_lazyTargetInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTargetInterlocked, CreateTarget(), null);
                VerifyParentOperation(this, _lazyTargetInterlocked);
            }

            return _lazyTargetInterlocked;
        }
    }

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a declaration expression in C#.
    /// Unlike a regular variable declaration, this operation represents an "expression" declaring a variable.
    /// For example,
    ///   1. "var (x, y)" is a deconstruction declaration expression with variables x and y.
    ///   2. "(var x, var y)" is a tuple expression with two declaration expressions.
    ///   3. "M(out var x);" is an invocation expression with an out "var x" declaration expression.
    /// </summary>
    internal abstract partial class BaseDeclarationExpressionOperation : Operation, IDeclarationExpressionOperation
    {
        public BaseDeclarationExpressionOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DeclarationExpression, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Expression != null)
                {
                    yield return Expression;
                }
            }
        }
        /// <summary>
        /// Underlying expression.
        /// </summary>
        public abstract IOperation Expression { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDeclarationExpression(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDeclarationExpression(this, argument);
        }
    }

    /// <summary>
    /// Represents a declaration expression in C#.
    /// Unlike a regular variable declaration, this operation represents an "expression" declaring a variable.
    /// For example,
    ///   1. "var (x, y)" is a deconstruction declaration expression with variables x and y.
    ///   2. "(var x, var y)" is a tuple expression with two declaration expressions.
    ///   3. "M(out var x);" is an invocation expression with an out "var x" declaration expression.
    /// </summary>
    internal sealed partial class DeclarationExpressionOperation : BaseDeclarationExpressionOperation, IDeclarationExpressionOperation
    {
        public DeclarationExpressionOperation(IOperation expression, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Expression = SetParentOperation(expression, this);
        }
        public override IOperation Expression { get; }
    }

/// <summary>
/// Represents a declaration expression in C#.
/// Unlike a regular variable declaration, this operation represents an "expression" declaring a variable.
/// For example,
///   1. "var (x, y)" is a deconstruction declaration expression with variables x and y.
///   2. "(var x, var y)" is a tuple expression with two declaration expressions.
///   3. "M(out var x);" is an invocation expression with an out "var x" declaration expression.
/// </summary>
internal abstract class LazyDeclarationExpressionOperation : BaseDeclarationExpressionOperation, IDeclarationExpressionOperation
{
    private IOperation _lazyExpressionInterlocked;

    public LazyDeclarationExpressionOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateExpression();

    public override IOperation Expression
    {
        get
        {
            if (_lazyExpressionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyExpressionInterlocked, CreateExpression(), null);
                VerifyParentOperation(this, _lazyExpressionInterlocked);
            }

            return _lazyExpressionInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an await expression.
    /// </summary>
    internal abstract partial class BaseAwaitOperation : Operation, IAwaitOperation
    {
        protected BaseAwaitOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Await, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
            }
        }
        /// <summary>
        /// Awaited expression.
        /// </summary>
        public abstract IOperation Operation { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitAwait(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitAwait(this, argument);
        }
    }

    /// <summary>
    /// Represents an await expression.
    /// </summary>
    internal sealed partial class AwaitOperation : BaseAwaitOperation, IAwaitOperation
    {
        public AwaitOperation(IOperation operation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operation = SetParentOperation(operation, this);
        }

        public override IOperation Operation { get; }
    }

/// <summary>
/// Represents an await expression.
/// </summary>
internal abstract class LazyAwaitOperation : BaseAwaitOperation, IAwaitOperation
{
    private IOperation _lazyOperationInterlocked;

    public LazyAwaitOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();

    public override IOperation Operation
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an operation with two operands that produces a result with the same type as at least one of the operands.
    /// </summary>
    internal abstract partial class BaseBinaryOperation : Operation, IBinaryOperation
    {
        protected BaseBinaryOperation(BinaryOperatorKind operatorKind, bool isLifted, bool isChecked, bool isCompareText,
                                               IMethodSymbol operatorMethod, IMethodSymbol unaryOperatorMethod, SemanticModel semanticModel,
                                               SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Binary, semanticModel, syntax, type, constantValue, isImplicit)
        {
            OperatorKind = operatorKind;
            IsLifted = isLifted;
            IsChecked = isChecked;
            IsCompareText = isCompareText;
            OperatorMethod = operatorMethod;
            UnaryOperatorMethod = unaryOperatorMethod;
        }
        /// <summary>
        /// Kind of binary operation.
        /// </summary>
        public BinaryOperatorKind OperatorKind { get; }
        /// <summary>
        /// Operator method used by the operation, null if the operation does not use an operator method.
        /// </summary>
        public IMethodSymbol OperatorMethod { get; }

        /// <summary>
        /// True/False operator method used for short circuiting.
        /// https://github.com/dotnet/roslyn/issues/27598 tracks exposing this information through public API
        /// </summary>
        public IMethodSymbol UnaryOperatorMethod { get; }

        /// <summary>
        /// <see langword="true"/> if this is a 'lifted' binary operator.  When there is an
        /// operator that is defined to work on a value type, 'lifted' operators are
        /// created to work on the <see cref="System.Nullable{T}"/> versions of those
        /// value types.
        /// </summary>
        public bool IsLifted { get; }
        /// <summary>
        /// <see langword="true"/> if overflow checking is performed for the arithmetic operation.
        /// </summary>
        public bool IsChecked { get; }
        /// <summary>
        /// <see langword="true"/> if the comparison is text based for string or object comparison in VB.
        /// </summary>
        public bool IsCompareText { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (LeftOperand != null)
                {
                    yield return LeftOperand;
                }
                if (RightOperand != null)
                {
                    yield return RightOperand;
                }
            }
        }
        /// <summary>
        /// Left operand.
        /// </summary>
        public abstract IOperation LeftOperand { get; }
        /// <summary>
        /// Right operand.
        /// </summary>
        public abstract IOperation RightOperand { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitBinaryOperator(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitBinaryOperator(this, argument);
        }
    }

    /// <summary>
    /// Represents an operation with two operands that produces a result with the same type as at least one of the operands.
    /// </summary>
    internal sealed partial class BinaryOperation : BaseBinaryOperation, IBinaryOperation
    {
        public BinaryOperation(BinaryOperatorKind operatorKind, IOperation leftOperand, IOperation rightOperand, bool isLifted, bool isChecked, bool isCompareText,
                                        IMethodSymbol operatorMethod, IMethodSymbol unaryOperatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type,
                                        Optional<object> constantValue, bool isImplicit) :
            base(operatorKind, isLifted, isChecked, isCompareText, operatorMethod, unaryOperatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LeftOperand = SetParentOperation(leftOperand, this);
            RightOperand = SetParentOperation(rightOperand, this);
        }

        public override IOperation LeftOperand { get; }
        public override IOperation RightOperand { get; }
    }

/// <summary>
/// Represents an operation with two operands that produces a result with the same type as at least one of the operands.
/// </summary>
internal abstract class LazyBinaryOperation : BaseBinaryOperation, IBinaryOperation
{
    private IOperation _lazyLeftOperandInterlocked;
    private IOperation _lazyRightOperandInterlocked;

    public LazyBinaryOperation(BinaryOperatorKind operatorKind, bool isLifted, bool isChecked, bool isCompareText, IMethodSymbol operatorMethod, IMethodSymbol unaryOperatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(operatorKind, isLifted, isChecked, isCompareText, operatorMethod, unaryOperatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateLeftOperand();
    protected abstract IOperation CreateRightOperand();

    public override IOperation LeftOperand
    {
        get
        {
            if (_lazyLeftOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyLeftOperandInterlocked, CreateLeftOperand(), null);
                VerifyParentOperation(this, _lazyLeftOperandInterlocked);
            }

            return _lazyLeftOperandInterlocked;
        }
    }

    public override IOperation RightOperand
    {
        get
        {
            if (_lazyRightOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyRightOperandInterlocked, CreateRightOperand(), null);
                VerifyParentOperation(this, _lazyRightOperandInterlocked);
            }

            return _lazyRightOperandInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an operation with two operands that produces a result with the same type as at least one of the operands.
    /// </summary>
    internal abstract class BaseTupleBinaryOperation : Operation, ITupleBinaryOperation
    {
        public BaseTupleBinaryOperation(BinaryOperatorKind operatorKind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit)
            : base(OperationKind.TupleBinary, semanticModel, syntax, type, constantValue, isImplicit)
        {
            OperatorKind = operatorKind;
        }
        /// <summary>
        /// Kind of binary operation.
        /// </summary>
        public BinaryOperatorKind OperatorKind { get; }
        /// <summary>
        /// Left operand.
        /// </summary>
        public abstract IOperation LeftOperand { get; }
        /// <summary>
        /// Right operand.
        /// </summary>
        public abstract IOperation RightOperand { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (LeftOperand != null)
                {
                    yield return LeftOperand;
                }
                if (RightOperand != null)
                {
                    yield return RightOperand;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitTupleBinaryOperator(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitTupleBinaryOperator(this, argument);
        }
    }

    /// <summary>
    /// Represents an operation with two operands that produces a result with the same type as at least one of the operands.
    /// </summary>
    internal sealed class TupleBinaryOperation : BaseTupleBinaryOperation, ITupleBinaryOperation
    {
        public TupleBinaryOperation(BinaryOperatorKind operatorKind, IOperation leftOperand, IOperation rightOperand, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit)
            : base(operatorKind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LeftOperand = SetParentOperation(leftOperand, this);
            RightOperand = SetParentOperation(rightOperand, this);
        }

        public override IOperation LeftOperand { get; }
        public override IOperation RightOperand { get; }
    }

/// <summary>
/// Represents an operation with two operands that produces a result with the same type as at least one of the operands.
/// </summary>
internal abstract class LazyTupleBinaryOperation : BaseTupleBinaryOperation, ITupleBinaryOperation
{
    private IOperation _lazyLeftOperandInterlocked;
    private IOperation _lazyRightOperandInterlocked;

    public LazyTupleBinaryOperation(BinaryOperatorKind operatorKind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(operatorKind, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateLeftOperand();
    protected abstract IOperation CreateRightOperand();

    public override IOperation LeftOperand
    {
        get
        {
            if (_lazyLeftOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyLeftOperandInterlocked, CreateLeftOperand(), null);
                VerifyParentOperation(this, _lazyLeftOperandInterlocked);
            }

            return _lazyLeftOperandInterlocked;
        }
    }

    public override IOperation RightOperand
    {
        get
        {
            if (_lazyRightOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyRightOperandInterlocked, CreateRightOperand(), null);
                VerifyParentOperation(this, _lazyRightOperandInterlocked);
            }

            return _lazyRightOperandInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a block scope.
    /// </summary>
    internal abstract partial class BaseBlockOperation : Operation, IBlockOperation
    {
        protected BaseBlockOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Block, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
        }

        /// <summary>
        /// Local declarations contained within the block.
        /// </summary>
        public ImmutableArray<ILocalSymbol> Locals { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var operation in Operations)
                {
                    if (operation != null)
                    {
                        yield return operation;
                    }
                }
            }
        }
        /// <summary>
        /// Statements contained within the block.
        /// </summary>
        public abstract ImmutableArray<IOperation> Operations { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitBlock(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitBlock(this, argument);
        }
    }

    /// <summary>
    /// Represents a block scope.
    /// </summary>
    internal sealed partial class BlockOperation : BaseBlockOperation, IBlockOperation
    {
        public BlockOperation(ImmutableArray<IOperation> operations, ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operations = SetParentOperation(operations, this);
        }

        public override ImmutableArray<IOperation> Operations { get; }
    }

/// <summary>
/// Represents a block scope.
/// </summary>
internal abstract class LazyBlockOperation : BaseBlockOperation, IBlockOperation
{
    private ImmutableArray<IOperation> _lazyOperationsInterlocked;

    public LazyBlockOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateOperations();

    public override ImmutableArray<IOperation> Operations
    {
        get
        {
            if (_lazyOperationsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyOperationsInterlocked, CreateOperations(), default);
                VerifyParentOperation(this, _lazyOperationsInterlocked);
            }

            return _lazyOperationsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# goto, break, or continue statement, or a VB GoTo, Exit ***, or Continue *** statement
    /// </summary>
    internal sealed partial class BranchOperation : Operation, IBranchOperation
    {
        public BranchOperation(ILabelSymbol target, BranchKind branchKind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Branch, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = target;
            BranchKind = branchKind;
        }
        /// <summary>
        /// Label that is the target of the branch.
        /// </summary>
        public ILabelSymbol Target { get; }
        /// <summary>
        /// Kind of the branch.
        /// </summary>
        public BranchKind BranchKind { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitBranch(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitBranch(this, argument);
        }
    }

    /// <summary>
    /// Represents a clause of a C# case or a VB Case.
    /// </summary>
    internal abstract partial class BaseCaseClauseOperation : Operation, ICaseClauseOperation
    {
        protected BaseCaseClauseOperation(CaseKind caseKind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.CaseClause, semanticModel, syntax, type, constantValue, isImplicit)
        {
            CaseKind = caseKind;
        }
        /// <summary>
        /// Kind of the clause.
        /// </summary>
        public CaseKind CaseKind { get; }

        public abstract ILabelSymbol Label { get; }
    }

    /// <summary>
    /// Represents a C# catch or VB Catch clause.
    /// </summary>
    internal abstract partial class BaseCatchClauseOperation : Operation, ICatchClauseOperation
    {
        protected BaseCatchClauseOperation(ITypeSymbol exceptionType, ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.CatchClause, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ExceptionType = exceptionType;
            Locals = locals;
        }
        /// <summary>
        /// Type of the exception handled by the catch clause.
        /// </summary>
        public ITypeSymbol ExceptionType { get; }
        /// <summary>
        /// Locals declared by the <see cref="ExceptionDeclarationOrExpression"/> and/or <see cref="Filter"/> clause.
        /// </summary>
        public ImmutableArray<ILocalSymbol> Locals { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (ExceptionDeclarationOrExpression != null)
                {
                    yield return ExceptionDeclarationOrExpression;
                }
                if (Filter != null)
                {
                    yield return Filter;
                }
                if (Handler != null)
                {
                    yield return Handler;
                }
            }
        }
        /// <summary>
        /// Optional source for exception. This could be any of the following operation:
        /// 1. Declaration for the local catch variable bound to the caught exception (C# and VB) OR
        /// 2. Type expression for the caught expression type (C#) OR
        /// 3. Null, indicating no expression (C#)
        /// 4. Reference to an existing local or parameter (VB) OR
        /// 5. An error expression (VB)
        /// </summary>
        public abstract IOperation ExceptionDeclarationOrExpression { get; }
        /// <summary>
        /// Filter expression to be executed to determine whether to handle the exception.
        /// </summary>
        public abstract IOperation Filter { get; }
        /// <summary>
        /// Body of the exception handler.
        /// </summary>
        public abstract IBlockOperation Handler { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitCatchClause(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitCatchClause(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# catch or VB Catch clause.
    /// </summary>
    internal sealed partial class CatchClauseOperation : BaseCatchClauseOperation, ICatchClauseOperation
    {
        public CatchClauseOperation(IOperation exceptionDeclarationOrExpression, ITypeSymbol exceptionType, ImmutableArray<ILocalSymbol> locals, IOperation filter, IBlockOperation handler, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(exceptionType, locals, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ExceptionDeclarationOrExpression = SetParentOperation(exceptionDeclarationOrExpression, this);
            Filter = SetParentOperation(filter, this);
            Handler = SetParentOperation(handler, this);
        }

        public override IBlockOperation Handler { get; }
        public override IOperation Filter { get; }
        public override IOperation ExceptionDeclarationOrExpression { get; }
    }

/// <summary>
/// Represents a C# catch or VB Catch clause.
/// </summary>
internal abstract class LazyCatchClauseOperation : BaseCatchClauseOperation, ICatchClauseOperation
{
    private IOperation _lazyExceptionDeclarationOrExpressionInterlocked;
    private IOperation _lazyFilterInterlocked;
    private IBlockOperation _lazyHandlerInterlocked;

    public LazyCatchClauseOperation(ITypeSymbol exceptionType, ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(exceptionType, locals, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateExceptionDeclarationOrExpression();
    protected abstract IOperation CreateFilter();
    protected abstract IBlockOperation CreateHandler();

    public override IOperation ExceptionDeclarationOrExpression
    {
        get
        {
            if (_lazyExceptionDeclarationOrExpressionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyExceptionDeclarationOrExpressionInterlocked, CreateExceptionDeclarationOrExpression(), null);
                VerifyParentOperation(this, _lazyExceptionDeclarationOrExpressionInterlocked);
            }

            return _lazyExceptionDeclarationOrExpressionInterlocked;
        }
    }

    public override IOperation Filter
    {
        get
        {
            if (_lazyFilterInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyFilterInterlocked, CreateFilter(), null);
                VerifyParentOperation(this, _lazyFilterInterlocked);
            }

            return _lazyFilterInterlocked;
        }
    }

    public override IBlockOperation Handler
    {
        get
        {
            if (_lazyHandlerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyHandlerInterlocked, CreateHandler(), null);
                VerifyParentOperation(this, _lazyHandlerInterlocked);
            }

            return _lazyHandlerInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an assignment expression that includes a binary operation.
    /// </summary>
    internal abstract partial class BaseCompoundAssignmentOperation : AssignmentOperation, ICompoundAssignmentOperation
    {
        protected BaseCompoundAssignmentOperation(IConvertibleConversion inConversionConvertible, IConvertibleConversion outConversionConvertible, BinaryOperatorKind operatorKind, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.CompoundAssignment, semanticModel, syntax, type, constantValue, isImplicit)
        {
            OperatorKind = operatorKind;
            IsLifted = isLifted;
            IsChecked = isChecked;
            OperatorMethod = operatorMethod;
            InConversionConvertible = inConversionConvertible;
            OutConversionConvertible = outConversionConvertible;
        }
        /// <summary>
        /// Kind of binary operation.
        /// </summary>
        public BinaryOperatorKind OperatorKind { get; }
        /// <summary>
        /// <see langword="true"/> if this assignment contains a 'lifted' binary operation.
        /// </summary>
        public bool IsLifted { get; }
        /// <summary>
        /// <see langword="true"/> if overflow checking is performed for the arithmetic operation.
        /// </summary>
        public bool IsChecked { get; }
        /// <summary>
        /// Operator method used by the operation, null if the operation does not use an operator method.
        /// </summary>
        public IMethodSymbol OperatorMethod { get; }
        internal IConvertibleConversion InConversionConvertible { get; }
        internal IConvertibleConversion OutConversionConvertible { get; }
        public CommonConversion InConversion => InConversionConvertible.ToCommonConversion();
        public CommonConversion OutConversion => OutConversionConvertible.ToCommonConversion();

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitCompoundAssignment(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitCompoundAssignment(this, argument);
        }
    }

    internal sealed partial class CompoundAssignmentOperation : BaseCompoundAssignmentOperation
    {
        public CompoundAssignmentOperation(IOperation target, IOperation value, IConvertibleConversion inConversionConvertible, IConvertibleConversion outConversionConvertible, BinaryOperatorKind operatorKind, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(inConversionConvertible, outConversionConvertible, operatorKind, isLifted, isChecked, operatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = SetParentOperation(target, this);
            Value = SetParentOperation(value, this);
        }

        public override IOperation Target { get; }
        public override IOperation Value { get; }
    }

internal abstract class LazyCompoundAssignmentOperation : BaseCompoundAssignmentOperation
{
    private IOperation _lazyTargetInterlocked;
    private IOperation _lazyValueInterlocked;

    public LazyCompoundAssignmentOperation(IConvertibleConversion inConversionConvertible, IConvertibleConversion outConversionConvertible, BinaryOperatorKind operatorKind, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(inConversionConvertible, outConversionConvertible, operatorKind, isLifted, isChecked, operatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateTarget();
    protected abstract IOperation CreateValue();

    public override IOperation Target
    {
        get
        {
            if (_lazyTargetInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTargetInterlocked, CreateTarget(), null);
                VerifyParentOperation(this, _lazyTargetInterlocked);
            }

            return _lazyTargetInterlocked;
        }
    }

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an expression that includes a ? or ?. conditional access instance expression.
    /// </summary>
    internal abstract partial class BaseConditionalAccessOperation : Operation, IConditionalAccessOperation
    {
        protected BaseConditionalAccessOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ConditionalAccess, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
                if (WhenNotNull != null)
                {
                    yield return WhenNotNull;
                }
            }
        }
        /// <summary>
        /// Expression that is conditionally accessed.
        /// </summary>
        public abstract IOperation Operation { get; }
        /// <summary>
        /// Expression to be evaluated if the conditional instance is non null.
        /// </summary>
        public abstract IOperation WhenNotNull { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitConditionalAccess(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitConditionalAccess(this, argument);
        }
    }

    /// <summary>
    /// Represents an expression that includes a ? or ?. conditional access instance expression.
    /// </summary>
    internal sealed partial class ConditionalAccessOperation : BaseConditionalAccessOperation, IConditionalAccessOperation
    {
        public ConditionalAccessOperation(IOperation whenNotNull, IOperation operation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            WhenNotNull = SetParentOperation(whenNotNull, this);
            Operation = SetParentOperation(operation, this);
        }

        public override IOperation Operation { get; }
        public override IOperation WhenNotNull { get; }
    }

/// <summary>
/// Represents an expression that includes a ? or ?. conditional access instance expression.
/// </summary>
internal abstract class LazyConditionalAccessOperation : BaseConditionalAccessOperation, IConditionalAccessOperation
{
    private IOperation _lazyWhenNotNullInterlocked;
    private IOperation _lazyOperationInterlocked;

    public LazyConditionalAccessOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();
    protected abstract IOperation CreateWhenNotNull();

    public override IOperation Operation
    {
        get
        {
            if (_lazyWhenNotNullInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyWhenNotNullInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyWhenNotNullInterlocked);
            }

            return _lazyWhenNotNullInterlocked;
        }
    }

    public override IOperation WhenNotNull
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateWhenNotNull(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }
}

    /// <summary>
    /// Represents the value of a conditionally-accessed expression within an expression containing a conditional access.
    /// </summary>
    internal sealed partial class ConditionalAccessInstanceOperation : Operation, IConditionalAccessInstanceOperation
    {
        public ConditionalAccessInstanceOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ConditionalAccessInstance, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitConditionalAccessInstance(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitConditionalAccessInstance(this, argument);
        }
    }

    /// <summary>
    /// Represents a conditional operation with:
    ///  1. <see cref="IConditionalOperation.Condition"/> to be tested,
    ///  2. <see cref="IConditionalOperation.WhenTrue"/> operation to be executed when <see cref="IConditionalOperation.Condition"/> is true and
    ///  3. <see cref="IConditionalOperation.WhenFalse"/> operation to be executed when the <see cref="IConditionalOperation.Condition"/> is false.
    /// For example, a C# ternary expression "a ? b : c" or a VB "If(a, b, c)" expression is represented as a conditional operation whose resulting value is the result of the expression.
    /// Additionally, a C# "if else" statement or VB "If Then Else" is also is represented as a conditional operation, but with a dropped or no result value.
    /// </summary>
    internal abstract partial class BaseConditionalOperation : Operation, IConditionalOperation
    {
        protected BaseConditionalOperation(bool isRef, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Conditional, semanticModel, syntax, type, constantValue, isImplicit)
        {
            IsRef = isRef;
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Condition != null)
                {
                    yield return Condition;
                }
                if (WhenTrue != null)
                {
                    yield return WhenTrue;
                }
                if (WhenFalse != null)
                {
                    yield return WhenFalse;
                }
            }
        }
        /// <summary>
        /// Condition to be tested.
        /// </summary>
        public abstract IOperation Condition { get; }
        /// <summary>
        /// Value evaluated if the Condition is true.
        /// </summary>
        public abstract IOperation WhenTrue { get; }
        /// <summary>
        /// Value evaluated if the Condition is false.
        /// </summary>
        public abstract IOperation WhenFalse { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitConditional(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitConditional(this, argument);
        }

        /// <summary>
        /// Is result a managed reference
        /// </summary>
        public bool IsRef { get; }
    }

    /// <summary>
    /// Represents a conditional operation with:
    ///  1. <see cref="IConditionalOperation.Condition"/> to be tested,
    ///  2. <see cref="IConditionalOperation.WhenTrue"/> operation to be executed when <see cref="IConditionalOperation.Condition"/> is true and
    ///  3. <see cref="IConditionalOperation.WhenFalse"/> operation to be executed when the <see cref="IConditionalOperation.Condition"/> is false.
    /// For example, a C# ternary expression "a ? b : c" or a VB "If(a, b, c)" expression is represented as a conditional operation whose resulting value is the result of the expression.
    /// Additionally, a C# "if else" statement or VB "If Then Else" is also is represented as a conditional operation, but with a dropped or no result value.
    /// </summary>
    internal sealed partial class ConditionalOperation : BaseConditionalOperation, IConditionalOperation
    {
        public ConditionalOperation(IOperation condition, IOperation whenTrue, IOperation whenFalse, bool isRef, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(isRef, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Condition = SetParentOperation(condition, this);
            WhenTrue = SetParentOperation(whenTrue, this);
            WhenFalse = SetParentOperation(whenFalse, this);
        }

        public override IOperation Condition { get; }
        public override IOperation WhenTrue { get; }
        public override IOperation WhenFalse { get; }
    }

/// <summary>
/// Represents a conditional operation with:
///  1. <see cref="IConditionalOperation.Condition"/> to be tested,
///  2. <see cref="IConditionalOperation.WhenTrue"/> operation to be executed when <see cref="IConditionalOperation.Condition"/> is true and
///  3. <see cref="IConditionalOperation.WhenFalse"/> operation to be executed when the <see cref="IConditionalOperation.Condition"/> is false.
/// For example, a C# ternary expression "a ? b : c" or a VB "If(a, b, c)" expression is represented as a conditional operation whose resulting value is the result of the expression.
/// Additionally, a C# "if else" statement or VB "If Then Else" is also is represented as a conditional operation, but with a dropped or no result value.
/// </summary>
internal abstract class LazyConditionalOperation : BaseConditionalOperation, IConditionalOperation
{
    private IOperation _lazyConditionInterlocked;
    private IOperation _lazyWhenTrueInterlocked;
    private IOperation _lazyWhenFalseInterlocked;

    public LazyConditionalOperation(bool isRef, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(isRef, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateCondition();
    protected abstract IOperation CreateWhenTrue();
    protected abstract IOperation CreateWhenFalse();

    public override IOperation Condition
    {
        get
        {
            if (_lazyConditionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyConditionInterlocked, CreateCondition(), null);
                VerifyParentOperation(this, _lazyConditionInterlocked);
            }

            return _lazyConditionInterlocked;
        }
    }

    public override IOperation WhenTrue
    {
        get
        {
            if (_lazyWhenTrueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyWhenTrueInterlocked, CreateWhenTrue(), null);
                VerifyParentOperation(this, _lazyWhenTrueInterlocked);
            }

            return _lazyWhenTrueInterlocked;
        }
    }

    public override IOperation WhenFalse
    {
        get
        {
            if (_lazyWhenFalseInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyWhenFalseInterlocked, CreateWhenFalse(), null);
                VerifyParentOperation(this, _lazyWhenFalseInterlocked);
            }

            return _lazyWhenFalseInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a conversion operation.
    /// </summary>
    internal abstract partial class BaseConversionOperation : Operation, IConversionOperation
    {
        protected BaseConversionOperation(IConvertibleConversion convertibleConversion, bool isTryCast, bool isChecked, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Conversion, semanticModel, syntax, type, constantValue, isImplicit)
        {
            IsTryCast = isTryCast;
            IsChecked = isChecked;
            ConvertibleConversion = convertibleConversion;
        }

        internal IConvertibleConversion ConvertibleConversion { get; }
        public CommonConversion Conversion => ConvertibleConversion.ToCommonConversion();
        public bool IsTryCast { get; }
        public bool IsChecked { get; }
        public IMethodSymbol OperatorMethod => Conversion.MethodSymbol;
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operand != null)
                {
                    yield return Operand;
                }
            }
        }
        /// <summary>
        /// Value to be converted.
        /// </summary>
        public abstract IOperation Operand { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitConversion(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitConversion(this, argument);
        }
    }

    internal sealed partial class ConversionOperation : BaseConversionOperation
    {
        public ConversionOperation(IOperation operand, IConvertibleConversion convertibleConversion, bool isTryCast, bool isChecked, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(convertibleConversion, isTryCast, isChecked, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operand = SetParentOperation(operand, this);
        }

        public override IOperation Operand { get; }
    }

internal abstract class LazyConversionOperation : BaseConversionOperation
{
    private IOperation _lazyOperandInterlocked;

    public LazyConversionOperation(IConvertibleConversion convertibleConversion, bool isTryCast, bool isChecked, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(convertibleConversion, isTryCast, isChecked, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperand();

    public override IOperation Operand
    {
        get
        {
            if (_lazyOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperandInterlocked, CreateOperand(), null);
                VerifyParentOperation(this, _lazyOperandInterlocked);
            }

            return _lazyOperandInterlocked;
        }
    }
}

    /// <remarks>
    /// This interface is reserved for implementation by its associated APIs. We reserve the right to
    /// change it in the future.
    /// </remarks>
    internal sealed partial class DefaultValueOperation : Operation, IDefaultValueOperation
    {
        public DefaultValueOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DefaultValue, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDefaultValue(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDefaultValue(this, argument);
        }
    }

    /// <summary>
    /// Represents an empty statement.
    /// </summary>
    internal sealed partial class EmptyOperation : Operation, IEmptyOperation
    {
        public EmptyOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Empty, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitEmpty(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitEmpty(this, argument);
        }
    }

    /// <summary>
    /// Represents a VB End statement.
    /// </summary>
    internal sealed partial class EndOperation : Operation, IEndOperation
    {
        public EndOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.End, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitEnd(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitEnd(this, argument);
        }
    }

    /// <summary>
    /// Represents a binding of an event.
    /// </summary>
    internal abstract partial class BaseEventAssignmentOperation : Operation, IEventAssignmentOperation
    {
        protected BaseEventAssignmentOperation(bool adds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.EventAssignment, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Adds = adds;
        }

        /// <summary>
        /// True for adding a binding, false for removing one.
        /// </summary>
        public bool Adds { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (EventReference != null)
                {
                    yield return EventReference;
                }
                if (HandlerValue != null)
                {
                    yield return HandlerValue;
                }
            }
        }

        /// <summary>
        /// Instance used to refer to the event being bound.
        /// </summary>
        public abstract IOperation EventReference { get; }

        /// <summary>
        /// Handler supplied for the event.
        /// </summary>
        public abstract IOperation HandlerValue { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitEventAssignment(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitEventAssignment(this, argument);
        }
    }

    /// <summary>
    /// Represents a binding of an event.
    /// </summary>
    internal sealed partial class EventAssignmentOperation : BaseEventAssignmentOperation, IEventAssignmentOperation
    {
        public EventAssignmentOperation(IOperation eventReference, IOperation handlerValue, bool adds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(adds, semanticModel, syntax, type, constantValue, isImplicit)
        {
            EventReference = SetParentOperation(eventReference, this);
            HandlerValue = SetParentOperation(handlerValue, this);
        }

        public override IOperation EventReference { get; }
        public override IOperation HandlerValue { get; }
    }

/// <summary>
/// Represents a binding of an event.
/// </summary>
internal abstract class LazyEventAssignmentOperation : BaseEventAssignmentOperation, IEventAssignmentOperation
{
    private IOperation _lazyEventReferenceInterlocked;
    private IOperation _lazyHandlerValueInterlocked;

    public LazyEventAssignmentOperation(bool adds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(adds, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateEventReference();
    protected abstract IOperation CreateHandlerValue();

    public override IOperation EventReference
    {
        get
        {
            if (_lazyEventReferenceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyEventReferenceInterlocked, CreateEventReference(), null);
                VerifyParentOperation(this, _lazyEventReferenceInterlocked);
            }

            return _lazyEventReferenceInterlocked;
        }
    }

    public override IOperation HandlerValue
    {
        get
        {
            if (_lazyHandlerValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyHandlerValueInterlocked, CreateHandlerValue(), null);
                VerifyParentOperation(this, _lazyHandlerValueInterlocked);
            }

            return _lazyHandlerValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a reference to an event.
    /// </summary>
    internal abstract partial class BaseEventReferenceOperation : BaseMemberReferenceOperation, IEventReferenceOperation
    {
        public BaseEventReferenceOperation(IEventSymbol @event, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(@event, OperationKind.EventReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        /// <summary>
        /// Referenced event.
        /// </summary>
        public IEventSymbol Event => (IEventSymbol)Member;

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Instance != null)
                {
                    yield return Instance;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitEventReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitEventReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a reference to an event.
    /// </summary>
    internal sealed partial class EventReferenceOperation : BaseEventReferenceOperation, IEventReferenceOperation
    {
        public EventReferenceOperation(IEventSymbol @event, IOperation instance, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(@event, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Instance = SetParentOperation(instance, this);
        }
        public override IOperation Instance { get; }
    }

/// <summary>
/// Represents a reference to an event.
/// </summary>
internal abstract class LazyEventReferenceOperation : BaseEventReferenceOperation, IEventReferenceOperation
{
    private IOperation _lazyInstanceInterlocked;

    public LazyEventReferenceOperation(IEventSymbol @event, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(@event, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInstance();

    public override IOperation Instance
    {
        get
        {
            if (_lazyInstanceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInstanceInterlocked, CreateInstance(), null);
                VerifyParentOperation(this, _lazyInstanceInterlocked);
            }

            return _lazyInstanceInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# or VB statement that consists solely of an expression.
    /// </summary>
    internal abstract partial class BaseExpressionStatementOperation : Operation, IExpressionStatementOperation
    {
        protected BaseExpressionStatementOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ExpressionStatement, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
            }
        }
        /// <summary>
        /// Expression of the statement.
        /// </summary>
        public abstract IOperation Operation { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitExpressionStatement(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitExpressionStatement(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# or VB statement that consists solely of an expression.
    /// </summary>
    internal sealed partial class ExpressionStatementOperation : BaseExpressionStatementOperation, IExpressionStatementOperation
    {
        public ExpressionStatementOperation(IOperation operation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operation = SetParentOperation(operation, this);
        }

        public override IOperation Operation { get; }
    }

/// <summary>
/// Represents a C# or VB statement that consists solely of an expression.
/// </summary>
internal abstract class LazyExpressionStatementOperation : BaseExpressionStatementOperation, IExpressionStatementOperation
{
    private IOperation _lazyOperationInterlocked;

    public LazyExpressionStatementOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();

    public override IOperation Operation
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an initialization of a local variable.
    /// </summary>
    internal abstract partial class BaseVariableInitializerOperation : SymbolInitializer, IVariableInitializerOperation
    {
        public BaseVariableInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.VariableInitializer, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitVariableInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitVariableInitializer(this, argument);
        }

        ImmutableArray<ILocalSymbol> ISymbolInitializerOperation.Locals => ImmutableArray<ILocalSymbol>.Empty;
    }

    /// <summary>
    /// Represents an initialization of a local variable.
    /// </summary>
    internal sealed partial class VariableInitializerOperation : BaseVariableInitializerOperation, IVariableInitializerOperation
    {
        public VariableInitializerOperation(IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents an initialization of a local variable.
/// </summary>
internal abstract class LazyVariableInitializerOperation : BaseVariableInitializerOperation, IVariableInitializerOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyVariableInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an initialization of a field.
    /// </summary>
    internal abstract partial class BaseFieldInitializerOperation : SymbolInitializer, IFieldInitializerOperation
    {
        public BaseFieldInitializerOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<IFieldSymbol> initializedFields, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
            InitializedFields = initializedFields;
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }

        /// <summary>
        /// Initialized fields. There can be multiple fields for Visual Basic fields declared with As New.
        /// </summary>
        public ImmutableArray<IFieldSymbol> InitializedFields { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFieldInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFieldInitializer(this, argument);
        }
    }

    /// <summary>
    /// Represents an initialization of a field.
    /// </summary>
    internal sealed partial class FieldInitializerOperation : BaseFieldInitializerOperation, IFieldInitializerOperation
    {
        public FieldInitializerOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<IFieldSymbol> initializedFields, IOperation value, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, initializedFields, kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents an initialization of a field.
/// </summary>
internal abstract class LazyFieldInitializerOperation : BaseFieldInitializerOperation, IFieldInitializerOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyFieldInitializerOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<IFieldSymbol> initializedFields, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, initializedFields, kind, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a reference to a field.
    /// </summary>
    internal abstract partial class BaseFieldReferenceOperation : BaseMemberReferenceOperation, IFieldReferenceOperation
    {
        public BaseFieldReferenceOperation(IFieldSymbol field, bool isDeclaration, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(field, OperationKind.FieldReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
            IsDeclaration = isDeclaration;
        }
        /// <summary>
        /// Referenced field.
        /// </summary>
        public IFieldSymbol Field => (IFieldSymbol)Member;
        public bool IsDeclaration { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Instance != null)
                {
                    yield return Instance;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFieldReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFieldReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a reference to a field.
    /// </summary>
    internal sealed partial class FieldReferenceOperation : BaseFieldReferenceOperation, IFieldReferenceOperation
    {
        public FieldReferenceOperation(IFieldSymbol field, bool isDeclaration, IOperation instance, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(field, isDeclaration, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Instance = SetParentOperation(instance, this);
        }
        public override IOperation Instance { get; }
    }

/// <summary>
/// Represents a reference to a field.
/// </summary>
internal abstract class LazyFieldReferenceOperation : BaseFieldReferenceOperation, IFieldReferenceOperation
{
    private IOperation _lazyInstanceInterlocked;

    public LazyFieldReferenceOperation(IFieldSymbol field, bool isDeclaration, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(field, isDeclaration, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInstance();

    public override IOperation Instance
    {
        get
        {
            if (_lazyInstanceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInstanceInterlocked, CreateInstance(), null);
                VerifyParentOperation(this, _lazyInstanceInterlocked);
            }

            return _lazyInstanceInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# fixed statement.
    /// </summary>
    internal abstract partial class BaseFixedOperation : Operation, IFixedOperation
    {
        protected BaseFixedOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            // https://github.com/dotnet/roslyn/issues/21281
            // base(OperationKind.Fixed, semanticModel, syntax, type, constantValue)
            base(OperationKind.None, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Variables != null)
                {
                    yield return Variables;
                }
                if (Body != null)
                {
                    yield return Body;
                }
            }
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }

        /// <summary>
        /// Variables to be fixed.
        /// </summary>
        public abstract IVariableDeclarationGroupOperation Variables { get; }
        /// <summary>
        /// Body of the fixed, over which the variables are fixed.
        /// </summary>
        public abstract IOperation Body { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFixed(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFixed(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# fixed statement.
    /// </summary>
    internal sealed partial class FixedOperation : BaseFixedOperation, IFixedOperation
    {
        public FixedOperation(ImmutableArray<ILocalSymbol> locals, IVariableDeclarationGroupOperation variables, IOperation body, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Variables = SetParentOperation(variables, this);
            Body = SetParentOperation(body, this);
        }

        public override IVariableDeclarationGroupOperation Variables { get; }
        public override IOperation Body { get; }
    }

/// <summary>
/// Represents a C# fixed statement.
/// </summary>
internal abstract class LazyFixedOperation : BaseFixedOperation, IFixedOperation
{
    private IVariableDeclarationGroupOperation _lazyVariablesInterlocked;
    private IOperation _lazyBodyInterlocked;

    public LazyFixedOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IVariableDeclarationGroupOperation CreateVariables();
    protected abstract IOperation CreateBody();

    public override IVariableDeclarationGroupOperation Variables
    {
        get
        {
            if (_lazyVariablesInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyVariablesInterlocked, CreateVariables(), null);
                VerifyParentOperation(this, _lazyVariablesInterlocked);
            }

            return _lazyVariablesInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# 'foreach' statement or a VB 'For Each' statement.
    /// </summary>
    internal abstract partial class BaseForEachLoopOperation : LoopOperation, IForEachLoopOperation
    {
        public BaseForEachLoopOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, ForEachLoopOperationInfo info,
                                        SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(LoopKind.ForEach, locals, continueLabel, exitLabel, OperationKind.Loop, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Info = info;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Collection != null)
                {
                    yield return Collection;
                }
                if (LoopControlVariable != null)
                {
                    yield return LoopControlVariable;
                }
                if (Body != null)
                {
                    yield return Body;
                }
                foreach (var expression in NextVariables)
                {
                    if (expression != null)
                    {
                        yield return expression;
                    }
                }
            }
        }

        public virtual ForEachLoopOperationInfo Info { get; }

        /// <summary>
        /// Optional loop control variable in VB that refers to the operation for declaring a new local variable or reference an existing variable or an expression.
        /// This field is always null for C#.
        /// </summary>
        public abstract IOperation LoopControlVariable { get; }
        /// <summary>
        /// Collection value over which the loop iterates.
        /// </summary>
        public abstract IOperation Collection { get; }
        /// <summary>
        /// Optional list of comma separate operations to execute at loop bottom for VB.
        /// This list is always empty for C#.
        /// </summary>
        public abstract ImmutableArray<IOperation> NextVariables { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitForEachLoop(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitForEachLoop(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# 'foreach' statement or a VB 'For Each' statement.
    /// </summary>
    internal sealed partial class ForEachLoopOperation : BaseForEachLoopOperation, IForEachLoopOperation
    {
        public ForEachLoopOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, IOperation loopControlVariable,
                                    IOperation collection, ImmutableArray<IOperation> nextVariables, IOperation body, ForEachLoopOperationInfo info,
                                    SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, continueLabel, exitLabel, info, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LoopControlVariable = SetParentOperation(loopControlVariable, this);
            Collection = SetParentOperation(collection, this);
            NextVariables = SetParentOperation(nextVariables, this);
            Body = SetParentOperation(body, this);
        }

        public override IOperation LoopControlVariable { get; }
        public override IOperation Collection { get; }
        public override ImmutableArray<IOperation> NextVariables { get; }
        public override IOperation Body { get; }
    }

/// <summary>
/// Represents a C# 'foreach' statement or a VB 'For Each' statement.
/// </summary>
internal abstract class LazyForEachLoopOperation : BaseForEachLoopOperation, IForEachLoopOperation
{
    private IOperation _lazyLoopControlVariableInterlocked;
    private IOperation _lazyCollectionInterlocked;
    private ImmutableArray<IOperation> _lazyNextVariablesInterlocked;
    private IOperation _lazyBodyInterlocked;

    public LazyForEachLoopOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, ForEachLoopOperationInfo info, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, continueLabel, exitLabel, info, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateLoopControlVariable();
    protected abstract IOperation CreateCollection();
    protected abstract ImmutableArray<IOperation> CreateNextVariables();
    protected abstract IOperation CreateBody();

    public override IOperation LoopControlVariable
    {
        get
        {
            if (_lazyLoopControlVariableInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyLoopControlVariableInterlocked, CreateLoopControlVariable(), null);
                VerifyParentOperation(this, _lazyLoopControlVariableInterlocked);
            }

            return _lazyLoopControlVariableInterlocked;
        }
    }

    public override IOperation Collection
    {
        get
        {
            if (_lazyCollectionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyCollectionInterlocked, CreateCollection(), null);
                VerifyParentOperation(this, _lazyCollectionInterlocked);
            }

            return _lazyCollectionInterlocked;
        }
    }

    public override ImmutableArray<IOperation> NextVariables
    {
        get
        {
            if (_lazyNextVariablesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyNextVariablesInterlocked, CreateNextVariables(), default);
                VerifyParentOperation(this, _lazyNextVariablesInterlocked);
            }

            return _lazyNextVariablesInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# 'for' loop statement.
    /// </summary>
    internal abstract partial class BaseForLoopOperation : LoopOperation, IForLoopOperation
    {
        public BaseForLoopOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<ILocalSymbol> conditionLocals,
            ILabelSymbol continueLabel, ILabelSymbol exitLabel, SemanticModel semanticModel,
            SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(LoopKind.For, locals, continueLabel, exitLabel, OperationKind.Loop, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ConditionLocals = conditionLocals;
        }

        public ImmutableArray<ILocalSymbol> ConditionLocals { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var before in Before)
                {
                    if (before != null)
                    {
                        yield return before;
                    }
                }
                if (Condition != null)
                {
                    yield return Condition;
                }
                if (Body != null)
                {
                    yield return Body;
                }
                foreach (var atLoopBottom in AtLoopBottom)
                {
                    if (atLoopBottom != null)
                    {
                        yield return atLoopBottom;
                    }
                }
            }
        }
        /// <summary>
        /// List of operations to execute before entry to the loop. This comes from the first clause of the for statement.
        /// </summary>
        public abstract ImmutableArray<IOperation> Before { get; }
        /// <summary>
        /// Condition of the loop. This comes from the second clause of the for statement.
        /// </summary>
        public abstract IOperation Condition { get; }
        /// <summary>
        /// List of operations to execute at the bottom of the loop. This comes from the third clause of the for statement.
        /// </summary>
        public abstract ImmutableArray<IOperation> AtLoopBottom { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitForLoop(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitForLoop(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# 'for' loop statement.
    /// </summary>
    internal sealed partial class ForLoopOperation : BaseForLoopOperation, IForLoopOperation
    {
        public ForLoopOperation(ImmutableArray<IOperation> before, IOperation condition, ImmutableArray<IOperation> atLoopBottom, ImmutableArray<ILocalSymbol> locals, ImmutableArray<ILocalSymbol> conditionLocals,
            ILabelSymbol continueLabel, ILabelSymbol exitLabel, IOperation body, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, conditionLocals, continueLabel, exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Before = SetParentOperation(before, this);
            Condition = SetParentOperation(condition, this);
            AtLoopBottom = SetParentOperation(atLoopBottom, this);
            Body = SetParentOperation(body, this);
        }

        public override ImmutableArray<IOperation> Before { get; }
        public override IOperation Condition { get; }
        public override ImmutableArray<IOperation> AtLoopBottom { get; }
        public override IOperation Body { get; }
    }

/// <summary>
/// Represents a C# 'for' loop statement.
/// </summary>
internal abstract class LazyForLoopOperation : BaseForLoopOperation, IForLoopOperation
{
    private ImmutableArray<IOperation> _lazyBeforeInterlocked;
    private IOperation _lazyConditionInterlocked;
    private ImmutableArray<IOperation> _lazyAtLoopBottomInterlocked;
    private IOperation _lazyBodyInterlocked;

    public LazyForLoopOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<ILocalSymbol> conditionLocals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, conditionLocals, continueLabel, exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateBefore();
    protected abstract IOperation CreateCondition();
    protected abstract ImmutableArray<IOperation> CreateAtLoopBottom();
    protected abstract IOperation CreateBody();

    public override ImmutableArray<IOperation> Before
    {
        get
        {
            if (_lazyBeforeInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyBeforeInterlocked, CreateBefore(), default);
                VerifyParentOperation(this, _lazyBeforeInterlocked);
            }

            return _lazyBeforeInterlocked;
        }
    }

    public override IOperation Condition
    {
        get
        {
            if (_lazyConditionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyConditionInterlocked, CreateCondition(), null);
                VerifyParentOperation(this, _lazyConditionInterlocked);
            }

            return _lazyConditionInterlocked;
        }
    }

    public override ImmutableArray<IOperation> AtLoopBottom
    {
        get
        {
            if (_lazyAtLoopBottomInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyAtLoopBottomInterlocked, CreateAtLoopBottom(), default);
                VerifyParentOperation(this, _lazyAtLoopBottomInterlocked);
            }

            return _lazyAtLoopBottomInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a VB 'For To' loop statement.
    /// </summary>
    internal abstract partial class BaseForToLoopOperation : LoopOperation, IForToLoopOperation
    {
        public BaseForToLoopOperation(ImmutableArray<ILocalSymbol> locals, bool isChecked,
                                      (ILocalSymbol LoopObject, ForToLoopOperationUserDefinedInfo UserDefinedInfo) info,
                                      ILabelSymbol continueLabel, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(LoopKind.ForTo, locals, continueLabel, exitLabel, OperationKind.Loop, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Debug.Assert(info.LoopObject == null || info.LoopObject.Type.SpecialType == SpecialType.System_Object);

            IsChecked = isChecked;
            Info = info;
        }

        public bool IsChecked { get; }

        public (ILocalSymbol LoopObject, ForToLoopOperationUserDefinedInfo UserDefinedInfo) Info { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (LoopControlVariable != null)
                {
                    yield return LoopControlVariable;
                }
                if (InitialValue != null)
                {
                    yield return InitialValue;
                }
                if (LimitValue != null)
                {
                    yield return LimitValue;
                }
                if (StepValue != null)
                {
                    yield return StepValue;
                }
                if (Body != null)
                {
                    yield return Body;
                }
                foreach (var expression in NextVariables)
                {
                    if (expression != null)
                    {
                        yield return expression;
                    }
                }
            }
        }
        /// <summary>
        /// Loop control variable refers to the operation for declaring a new local variable or reference an existing variable or an expression.
        /// </summary>
        public abstract IOperation LoopControlVariable { get; }

        /// <summary>
        /// Operation for setting the initial value of the loop control variable. This comes from the expression between the 'For' and 'To' keywords.
        /// </summary>
        public abstract IOperation InitialValue { get; }

        /// <summary>
        /// Operation for the limit value of the loop control variable. This comes from the expression after the 'To' keyword.
        /// </summary>
        public abstract IOperation LimitValue { get; }

        /// <summary>
        /// Optional operation for the step value of the loop control variable. This comes from the expression after the 'Step' keyword.
        /// </summary>
        public abstract IOperation StepValue { get; }

        /// <summary>
        /// Optional list of comma separated next variables at loop bottom.
        /// </summary>
        public abstract ImmutableArray<IOperation> NextVariables { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitForToLoop(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitForToLoop(this, argument);
        }
    }

    /// <summary>
    /// Represents a VB 'For To' loop statement.
    /// </summary>
    internal sealed partial class ForToLoopOperation : BaseForToLoopOperation, IForToLoopOperation
    {
        public ForToLoopOperation(ImmutableArray<ILocalSymbol> locals, bool isChecked,
                                  (ILocalSymbol LoopObject, ForToLoopOperationUserDefinedInfo UserDefinedInfo) info,
                                  ILabelSymbol continueLabel, ILabelSymbol exitLabel, IOperation loopControlVariable,
                                  IOperation initialValue, IOperation limitValue, IOperation stepValue, IOperation body,
                                  ImmutableArray<IOperation> nextVariables, SemanticModel semanticModel, SyntaxNode syntax,
                                  ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, isChecked, info, continueLabel, exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LoopControlVariable = SetParentOperation(loopControlVariable, this);
            InitialValue = SetParentOperation(initialValue, this);
            LimitValue = SetParentOperation(limitValue, this);
            StepValue = SetParentOperation(stepValue, this);
            Body = SetParentOperation(body, this);
            NextVariables = SetParentOperation(nextVariables, this);
        }

        public override IOperation LoopControlVariable { get; }
        public override IOperation InitialValue { get; }
        public override IOperation LimitValue { get; }
        public override IOperation StepValue { get; }
        public override IOperation Body { get; }
        public override ImmutableArray<IOperation> NextVariables { get; }
    }

/// <summary>
/// Represents a VB 'For To' loop statement.
/// </summary>
internal abstract class LazyForToLoopOperation : BaseForToLoopOperation, IForToLoopOperation
{
    private IOperation _lazyLoopControlVariableInterlocked;
    private IOperation _lazyInitialValueInterlocked;
    private IOperation _lazyLimitValueInterlocked;
    private IOperation _lazyStepValueInterlocked;
    private IOperation _lazyBodyInterlocked;
    private ImmutableArray<IOperation> _lazyNextVariablesInterlocked;

    public LazyForToLoopOperation(ImmutableArray<ILocalSymbol> locals, bool isChecked, (ILocalSymbol LoopObject, ForToLoopOperationUserDefinedInfo UserDefinedInfo) info, ILabelSymbol continueLabel, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, isChecked, info, continueLabel, exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateLoopControlVariable();
    protected abstract IOperation CreateInitialValue();
    protected abstract IOperation CreateLimitValue();
    protected abstract IOperation CreateStepValue();
    protected abstract IOperation CreateBody();
    protected abstract ImmutableArray<IOperation> CreateNextVariables();

    public override IOperation LoopControlVariable
    {
        get
        {
            if (_lazyLoopControlVariableInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyLoopControlVariableInterlocked, CreateLoopControlVariable(), null);
                VerifyParentOperation(this, _lazyLoopControlVariableInterlocked);
            }

            return _lazyLoopControlVariableInterlocked;
        }
    }

    public override IOperation InitialValue
    {
        get
        {
            if (_lazyInitialValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitialValueInterlocked, CreateInitialValue(), null);
                VerifyParentOperation(this, _lazyInitialValueInterlocked);
            }

            return _lazyInitialValueInterlocked;
        }
    }

    public override IOperation LimitValue
    {
        get
        {
            if (_lazyLimitValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyLimitValueInterlocked, CreateLimitValue(), null);
                VerifyParentOperation(this, _lazyLimitValueInterlocked);
            }

            return _lazyLimitValueInterlocked;
        }
    }

    public override IOperation StepValue
    {
        get
        {
            if (_lazyStepValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyStepValueInterlocked, CreateStepValue(), null);
                VerifyParentOperation(this, _lazyStepValueInterlocked);
            }

            return _lazyStepValueInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }

    public override ImmutableArray<IOperation> NextVariables
    {
        get
        {
            if (_lazyNextVariablesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyNextVariablesInterlocked, CreateNextVariables(), default);
                VerifyParentOperation(this, _lazyNextVariablesInterlocked);
            }

            return _lazyNextVariablesInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an increment expression.
    /// </summary>
    internal abstract partial class BaseIncrementOrDecrementOperation : Operation, IIncrementOrDecrementOperation
    {
        public BaseIncrementOrDecrementOperation(bool isDecrement, bool isPostfix, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(isDecrement ? OperationKind.Decrement : OperationKind.Increment, semanticModel, syntax, type, constantValue, isImplicit)
        {
            IsPostfix = isPostfix;
            IsLifted = isLifted;
            IsChecked = isChecked;
            OperatorMethod = operatorMethod;
        }
        /// <summary>
        /// <see langword="true"/> if this is a postfix expression.
        /// <see langword="false"/> if this is a prefix expression.
        /// </summary>
        public bool IsPostfix { get; }
        /// <summary>
        /// <see langword="true"/> if this is a 'lifted' increment operator.  When there is an
        /// operator that is defined to work on a value type, 'lifted' operators are
        /// created to work on the <see cref="System.Nullable{T}"/> versions of those
        /// value types.
        /// </summary>
        public bool IsLifted { get; }
        /// <summary>
        /// <see langword="true"/> if overflow checking is performed for the arithmetic operation.
        /// </summary>
        public bool IsChecked { get; }
        /// <summary>
        /// Operator method used by the operation, null if the operation does not use an operator method.
        /// </summary>
        public IMethodSymbol OperatorMethod { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Target != null)
                {
                    yield return Target;
                }
            }
        }
        /// <summary>
        /// Target of the assignment.
        /// </summary>
        public abstract IOperation Target { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitIncrementOrDecrement(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitIncrementOrDecrement(this, argument);
        }
    }

    /// <summary>
    /// Represents an increment expression.
    /// </summary>
    internal sealed partial class IncrementOrDecrementOperation : BaseIncrementOrDecrementOperation, IIncrementOrDecrementOperation
    {
        public IncrementOrDecrementOperation(bool isDecrement, bool isPostfix, bool isLifted, bool isChecked, IOperation target, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(isDecrement, isPostfix, isLifted, isChecked, operatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = SetParentOperation(target, this);
        }

        public override IOperation Target { get; }
    }

/// <summary>
/// Represents an increment expression.
/// </summary>
internal abstract class LazyIncrementOrDecrementOperation : BaseIncrementOrDecrementOperation, IIncrementOrDecrementOperation
{
    private IOperation _lazyTargetInterlocked;

    public LazyIncrementOrDecrementOperation(bool isDecrement, bool isPostfix, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(isDecrement, isPostfix, isLifted, isChecked, operatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateTarget();

    public override IOperation Target
    {
        get
        {
            if (_lazyTargetInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTargetInterlocked, CreateTarget(), null);
                VerifyParentOperation(this, _lazyTargetInterlocked);
            }

            return _lazyTargetInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# this or base expression, or a VB Me, MyClass, or MyBase expression.
    /// </summary>
    internal sealed partial class InstanceReferenceOperation : Operation, IInstanceReferenceOperation
    {
        public InstanceReferenceOperation(InstanceReferenceKind referenceKind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.InstanceReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ReferenceKind = referenceKind;
        }
        public InstanceReferenceKind ReferenceKind { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitInstanceReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitInstanceReference(this, argument);
        }
    }

    /// <remarks>
    /// Represents an interpolated string expression.
    /// </remarks>
    internal abstract partial class BaseInterpolatedStringOperation : Operation, IInterpolatedStringOperation
    {
        protected BaseInterpolatedStringOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.InterpolatedString, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var part in Parts)
                {
                    if (part != null)
                    {
                        yield return part;
                    }
                }
            }
        }
        /// <summary>
        /// Constituent parts of interpolated string, each of which is an <see cref="IInterpolatedStringContentOperation"/>.
        /// </summary>
        public abstract ImmutableArray<IInterpolatedStringContentOperation> Parts { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitInterpolatedString(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitInterpolatedString(this, argument);
        }
    }

    /// <remarks>
    /// Represents an interpolated string expression.
    /// </remarks>
    internal sealed partial class InterpolatedStringOperation : BaseInterpolatedStringOperation, IInterpolatedStringOperation
    {
        public InterpolatedStringOperation(ImmutableArray<IInterpolatedStringContentOperation> parts, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Parts = SetParentOperation(parts, this);
        }

        public override ImmutableArray<IInterpolatedStringContentOperation> Parts { get; }
    }

/// <remarks>
/// Represents an interpolated string expression.
/// </remarks>
internal abstract class LazyInterpolatedStringOperation : BaseInterpolatedStringOperation, IInterpolatedStringOperation
{
    private ImmutableArray<IInterpolatedStringContentOperation> _lazyPartsInterlocked;

    public LazyInterpolatedStringOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IInterpolatedStringContentOperation> CreateParts();

    public override ImmutableArray<IInterpolatedStringContentOperation> Parts
    {
        get
        {
            if (_lazyPartsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyPartsInterlocked, CreateParts(), default);
                VerifyParentOperation(this, _lazyPartsInterlocked);
            }

            return _lazyPartsInterlocked;
        }
    }
}

    /// <remarks>
    /// Represents a constituent string literal part of an interpolated string expression.
    /// </remarks>
    internal abstract partial class BaseInterpolatedStringTextOperation : Operation, IInterpolatedStringTextOperation
    {
        protected BaseInterpolatedStringTextOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.InterpolatedStringText, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Text != null)
                {
                    yield return Text;
                }
            }
        }
        /// <summary>
        /// Text content.
        /// </summary>
        public abstract IOperation Text { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitInterpolatedStringText(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitInterpolatedStringText(this, argument);
        }
    }

    /// <remarks>
    /// Represents a constituent string literal part of an interpolated string expression.
    /// </remarks>
    internal sealed partial class InterpolatedStringTextOperation : BaseInterpolatedStringTextOperation, IInterpolatedStringTextOperation
    {
        public InterpolatedStringTextOperation(IOperation text, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Text = SetParentOperation(text, this);
        }

        public override IOperation Text { get; }
    }

/// <remarks>
/// Represents a constituent string literal part of an interpolated string expression.
/// </remarks>
internal abstract class LazyInterpolatedStringTextOperation : BaseInterpolatedStringTextOperation, IInterpolatedStringTextOperation
{
    private IOperation _lazyTextInterlocked;

    public LazyInterpolatedStringTextOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateText();

    public override IOperation Text
    {
        get
        {
            if (_lazyTextInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTextInterlocked, CreateText(), null);
                VerifyParentOperation(this, _lazyTextInterlocked);
            }

            return _lazyTextInterlocked;
        }
    }
}

    /// <remarks>
    /// Represents a constituent interpolation part of an interpolated string expression.
    /// </remarks>
    internal abstract partial class BaseInterpolationOperation : Operation, IInterpolationOperation
    {
        protected BaseInterpolationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Interpolation, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Expression != null)
                {
                    yield return Expression;
                }
                if (Alignment != null)
                {
                    yield return Alignment;
                }
                if (FormatString != null)
                {
                    yield return FormatString;
                }
            }
        }
        /// <summary>
        /// Expression of the interpolation.
        /// </summary>
        public abstract IOperation Expression { get; }
        /// <summary>
        /// Optional alignment of the interpolation.
        /// </summary>
        public abstract IOperation Alignment { get; }
        /// <summary>
        /// Optional format string of the interpolation.
        /// </summary>
        public abstract IOperation FormatString { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitInterpolation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitInterpolation(this, argument);
        }
    }

    /// <remarks>
    /// Represents a constituent interpolation part of an interpolated string expression.
    /// </remarks>
    internal sealed partial class InterpolationOperation : BaseInterpolationOperation, IInterpolationOperation
    {
        public InterpolationOperation(IOperation expression, IOperation alignment, IOperation formatString, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Expression = SetParentOperation(expression, this);
            Alignment = SetParentOperation(alignment, this);
            FormatString = SetParentOperation(formatString, this);
        }

        public override IOperation Expression { get; }
        public override IOperation Alignment { get; }
        public override IOperation FormatString { get; }
    }

/// <remarks>
/// Represents a constituent interpolation part of an interpolated string expression.
/// </remarks>
internal abstract class LazyInterpolationOperation : BaseInterpolationOperation, IInterpolationOperation
{
    private IOperation _lazyExpressionInterlocked;
    private IOperation _lazyAlignmentInterlocked;
    private IOperation _lazyFormatStringInterlocked;

    public LazyInterpolationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateExpression();
    protected abstract IOperation CreateAlignment();
    protected abstract IOperation CreateFormatString();

    public override IOperation Expression
    {
        get
        {
            if (_lazyExpressionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyExpressionInterlocked, CreateExpression(), null);
                VerifyParentOperation(this, _lazyExpressionInterlocked);
            }

            return _lazyExpressionInterlocked;
        }
    }

    public override IOperation Alignment
    {
        get
        {
            if (_lazyAlignmentInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyAlignmentInterlocked, CreateAlignment(), null);
                VerifyParentOperation(this, _lazyAlignmentInterlocked);
            }

            return _lazyAlignmentInterlocked;
        }
    }

    public override IOperation FormatString
    {
        get
        {
            if (_lazyFormatStringInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyFormatStringInterlocked, CreateFormatString(), null);
                VerifyParentOperation(this, _lazyFormatStringInterlocked);
            }

            return _lazyFormatStringInterlocked;
        }
    }
}

    /// <remarks>
    /// This interface is reserved for implementation by its associated APIs. We reserve the right to
    /// change it in the future.
    /// </remarks>
    internal abstract partial class BaseInvalidOperation : Operation, IInvalidOperation
    {
        protected BaseInvalidOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Invalid, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitInvalid(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitInvalid(this, argument);
        }
    }

    /// <remarks>
    /// This interface is reserved for implementation by its associated APIs. We reserve the right to
    /// change it in the future.
    /// </remarks>
    internal sealed partial class InvalidOperation : BaseInvalidOperation, IInvalidOperation
    {
        public InvalidOperation(ImmutableArray<IOperation> children, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            // we don't allow null children.
            Debug.Assert(children.All(o => o != null));
            Children = SetParentOperation(children, this);
        }
        public override IEnumerable<IOperation> Children { get; }
    }

    /// <remarks>
    /// This interface is reserved for implementation by its associated APIs. We reserve the right to
    /// change it in the future.
    /// </remarks>
    internal abstract class LazyInvalidOperation : BaseInvalidOperation, IInvalidOperation
    {
        private ImmutableArray<IOperation> _lazyChildrenInterlocked;

        public LazyInvalidOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        protected abstract ImmutableArray<IOperation> CreateChildren();

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (_lazyChildrenInterlocked.IsDefault)
                {
                    ImmutableInterlocked.InterlockedCompareExchange(ref _lazyChildrenInterlocked, CreateChildren(), default);
                    VerifyParentOperation(this, _lazyChildrenInterlocked);
                }

                return _lazyChildrenInterlocked;
            }
        }
    }

    /// <summary>
    /// Represents a C# or VB method invocation.
    /// </summary>
    internal abstract partial class BaseInvocationOperation : Operation, IInvocationOperation
    {
        protected BaseInvocationOperation(IMethodSymbol targetMethod, bool isVirtual, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Invocation, semanticModel, syntax, type, constantValue, isImplicit)
        {
            TargetMethod = targetMethod;
            IsVirtual = isVirtual;
        }
        /// <summary>
        /// Method to be invoked.
        /// </summary>
        public IMethodSymbol TargetMethod { get; }
        /// <summary>
        /// True if the invocation uses a virtual mechanism, and false otherwise.
        /// </summary>
        public bool IsVirtual { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Instance != null)
                {
                    yield return Instance;
                }
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
            }
        }
        /// <summary>
        /// 'This' or 'Me' instance to be supplied to the method, or null if the method is static.
        /// </summary>
        public abstract IOperation Instance { get; }
        /// <summary>
        /// Arguments of the invocation, excluding the instance argument. Arguments are in evaluation order.
        /// </summary>
        /// <remarks>
        /// If the invocation is in its expanded form, then params/ParamArray arguments would be collected into arrays.
        /// Default values are supplied for optional arguments missing in source.
        /// </remarks>
        public abstract ImmutableArray<IArgumentOperation> Arguments { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitInvocation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitInvocation(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# or VB method invocation.
    /// </summary>
    internal sealed partial class InvocationOperation : BaseInvocationOperation, IInvocationOperation
    {
        public InvocationOperation(IMethodSymbol targetMethod, IOperation instance, bool isVirtual, ImmutableArray<IArgumentOperation> arguments, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(targetMethod, isVirtual, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Instance = SetParentOperation(instance, this);
            Arguments = SetParentOperation(arguments, this);
        }

        public override IOperation Instance { get; }
        public override ImmutableArray<IArgumentOperation> Arguments { get; }
    }

/// <summary>
/// Represents a C# or VB method invocation.
/// </summary>
internal abstract class LazyInvocationOperation : BaseInvocationOperation, IInvocationOperation
{
    private IOperation _lazyInstanceInterlocked;
    private ImmutableArray<IArgumentOperation> _lazyArgumentsInterlocked;

    public LazyInvocationOperation(IMethodSymbol targetMethod, bool isVirtual, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(targetMethod, isVirtual, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInstance();
    protected abstract ImmutableArray<IArgumentOperation> CreateArguments();

    public override IOperation Instance
    {
        get
        {
            if (_lazyInstanceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInstanceInterlocked, CreateInstance(), null);
                VerifyParentOperation(this, _lazyInstanceInterlocked);
            }

            return _lazyInstanceInterlocked;
        }
    }

    public override ImmutableArray<IArgumentOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a VB raise event statement.
    /// </summary>
    internal abstract partial class BaseRaiseEventOperation : Operation, IRaiseEventOperation
    {
        protected BaseRaiseEventOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.RaiseEvent, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (EventReference != null)
                {
                    yield return EventReference;
                }
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
            }
        }
        /// <summary>
        /// Reference to the event to be raised.
        /// </summary>
        public abstract IEventReferenceOperation EventReference { get; }
        public abstract ImmutableArray<IArgumentOperation> Arguments { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitRaiseEvent(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitRaiseEvent(this, argument);
        }
    }

    /// <summary>
    /// Represents a VB raise event statement.
    /// </summary>
    internal sealed partial class RaiseEventOperation : BaseRaiseEventOperation, IRaiseEventOperation
    {
        public RaiseEventOperation(IEventReferenceOperation eventReference, ImmutableArray<IArgumentOperation> arguments, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            EventReference = SetParentOperation(eventReference, this);
            Arguments = SetParentOperation(arguments, this);
        }

        public override IEventReferenceOperation EventReference { get; }
        public override ImmutableArray<IArgumentOperation> Arguments { get; }
    }

/// <summary>
/// Represents a VB raise event statement.
/// </summary>
internal abstract class LazyRaiseEventOperation : BaseRaiseEventOperation, IRaiseEventOperation
{
    private IEventReferenceOperation _lazyEventReferenceInterlocked;
    private ImmutableArray<IArgumentOperation> _lazyArgumentsInterlocked;

    public LazyRaiseEventOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IEventReferenceOperation CreateEventReference();
    protected abstract ImmutableArray<IArgumentOperation> CreateArguments();

    public override IEventReferenceOperation EventReference
    {
        get
        {
            if (_lazyEventReferenceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyEventReferenceInterlocked, CreateEventReference(), null);
                VerifyParentOperation(this, _lazyEventReferenceInterlocked);
            }

            return _lazyEventReferenceInterlocked;
        }
    }

    public override ImmutableArray<IArgumentOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an expression that tests if a value is of a specific type.
    /// </summary>
    internal abstract partial class BaseIsTypeOperation : Operation, IIsTypeOperation
    {
        protected BaseIsTypeOperation(ITypeSymbol typeOperand, bool isNegated, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.IsType, semanticModel, syntax, type, constantValue, isImplicit)
        {
            TypeOperand = typeOperand;
            IsNegated = isNegated;
        }
        /// <summary>
        /// Type for which to test.
        /// </summary>
        public ITypeSymbol TypeOperand { get; }
        /// <summary>
        /// Flag indicating if this is an "is not" type expression.
        /// True for VB "TypeOf ... IsNot ..." expression.
        /// False, otherwise.
        /// </summary>
        public bool IsNegated { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (ValueOperand != null)
                {
                    yield return ValueOperand;
                }
            }
        }
        /// <summary>
        /// Value to test.
        /// </summary>
        public abstract IOperation ValueOperand { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitIsType(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitIsType(this, argument);
        }
    }

    /// <summary>
    /// Represents an expression that tests if a value is of a specific type.
    /// </summary>
    internal sealed partial class IsTypeOperation : BaseIsTypeOperation, IIsTypeOperation
    {
        public IsTypeOperation(IOperation valueOperand, ITypeSymbol typeOperand, bool isNegated, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(typeOperand, isNegated, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ValueOperand = SetParentOperation(valueOperand, this);
        }

        public override IOperation ValueOperand { get; }
    }

/// <summary>
/// Represents an expression that tests if a value is of a specific type.
/// </summary>
internal abstract class LazyIsTypeOperation : BaseIsTypeOperation, IIsTypeOperation
{
    private IOperation _lazyOperandInterlocked;

    public LazyIsTypeOperation(ITypeSymbol isType, bool isNotTypeExpression, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(isType, isNotTypeExpression, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValueOperand();

    public override IOperation ValueOperand
    {
        get
        {
            if (_lazyOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperandInterlocked, CreateValueOperand(), null);
                VerifyParentOperation(this, _lazyOperandInterlocked);
            }

            return _lazyOperandInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# or VB label statement.
    /// </summary>
    internal abstract partial class BaseLabeledOperation : Operation, ILabeledOperation
    {
        protected BaseLabeledOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Labeled, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Label = label;
        }
        /// <summary>
        ///  Label that can be the target of branches.
        /// </summary>
        public ILabelSymbol Label { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
            }
        }
        /// <summary>
        /// Statement that has been labeled.
        /// </summary>
        public abstract IOperation Operation { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitLabeled(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitLabeled(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# or VB label statement.
    /// </summary>
    internal sealed partial class LabeledOperation : BaseLabeledOperation, ILabeledOperation
    {
        public LabeledOperation(ILabelSymbol label, IOperation operation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(label, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operation = SetParentOperation(operation, this);
        }

        public override IOperation Operation { get; }
    }

/// <summary>
/// Represents a C# or VB label statement.
/// </summary>
internal abstract class LazyLabeledOperation : BaseLabeledOperation, ILabeledOperation
{
    private IOperation _lazyOperationInterlocked;

    public LazyLabeledOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(label, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();

    public override IOperation Operation
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }
}

    internal abstract partial class BaseAnonymousFunctionOperation : Operation, IAnonymousFunctionOperation
    {
        protected BaseAnonymousFunctionOperation(IMethodSymbol symbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.AnonymousFunction, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Symbol = symbol;
        }
        public IMethodSymbol Symbol { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Body != null)
                {
                    yield return Body;
                }
            }
        }
        public abstract IBlockOperation Body { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitAnonymousFunction(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitAnonymousFunction(this, argument);
        }
    }

    internal sealed partial class AnonymousFunctionOperation : BaseAnonymousFunctionOperation, IAnonymousFunctionOperation
    {
        public AnonymousFunctionOperation(IMethodSymbol symbol, IBlockOperation body, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(symbol, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Body = SetParentOperation(body, this);
        }

        public override IBlockOperation Body { get; }
    }

internal abstract class LazyAnonymousFunctionOperation : BaseAnonymousFunctionOperation, IAnonymousFunctionOperation
{
    private IBlockOperation _lazyBodyInterlocked;

    public LazyAnonymousFunctionOperation(IMethodSymbol symbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(symbol, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IBlockOperation CreateBody();

    public override IBlockOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    internal sealed class FlowAnonymousFunctionOperation : Operation, IFlowAnonymousFunctionOperation
    {
        public readonly ControlFlowGraphBuilder.Context Context;
        public readonly IAnonymousFunctionOperation Original;

        public FlowAnonymousFunctionOperation(in ControlFlowGraphBuilder.Context context, IAnonymousFunctionOperation original, bool isImplicit) :
            base(OperationKind.FlowAnonymousFunction, semanticModel: null, original.Syntax, original.Type, original.ConstantValue, isImplicit)
        {
            Context = context;
            Original = original;
        }
        public IMethodSymbol Symbol => Original.Symbol;
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFlowAnonymousFunction(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFlowAnonymousFunction(this, argument);
        }
    }

    internal abstract partial class BaseDelegateCreationOperation : Operation, IDelegateCreationOperation
    {
        public BaseDelegateCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DelegateCreation, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Target != null)
                {
                    yield return Target;
                }
            }
        }
        public abstract IOperation Target { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDelegateCreation(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDelegateCreation(this, argument);
        }
    }

    internal sealed partial class DelegateCreationOperation : BaseDelegateCreationOperation
    {
        public DelegateCreationOperation(IOperation target, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = SetParentOperation(target, this);
        }

        public override IOperation Target { get; }
    }

internal abstract class LazyDelegateCreationOperation : BaseDelegateCreationOperation
{
    private IOperation _lazyTargetInterlocked;

    public LazyDelegateCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateTarget();

    public override IOperation Target
    {
        get
        {
            if (_lazyTargetInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTargetInterlocked, CreateTarget(), null);
                VerifyParentOperation(this, _lazyTargetInterlocked);
            }

            return _lazyTargetInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a dynamic access to a member of a class, struct, or module.
    /// </summary>
    internal abstract partial class BaseDynamicMemberReferenceOperation : Operation, IDynamicMemberReferenceOperation
    {
        protected BaseDynamicMemberReferenceOperation(string memberName, ImmutableArray<ITypeSymbol> typeArguments, ITypeSymbol containingType, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DynamicMemberReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
            MemberName = memberName;
            TypeArguments = typeArguments;
            ContainingType = containingType;
        }

        /// <summary>
        /// Name of the member.
        /// </summary>
        public string MemberName { get; }
        /// <summary>
        /// Type arguments.
        /// </summary>
        public ImmutableArray<ITypeSymbol> TypeArguments { get; }
        /// <summary>
        /// The containing type of this expression. In C#, this will always be null.
        /// </summary>
        public ITypeSymbol ContainingType { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Instance != null)
                {
                    yield return Instance;
                }
            }
        }
        /// <summary>
        /// Instance used to bind the member reference.
        /// </summary>
        public abstract IOperation Instance { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDynamicMemberReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDynamicMemberReference(this, argument);
        }

    }

    /// <summary>
    /// Represents a dynamic access to a member of a class, struct, or module.
    /// </summary>
    internal sealed partial class DynamicMemberReferenceOperation : BaseDynamicMemberReferenceOperation, IDynamicMemberReferenceOperation
    {
        public DynamicMemberReferenceOperation(IOperation instance, string memberName, ImmutableArray<ITypeSymbol> typeArguments, ITypeSymbol containingType, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(memberName, typeArguments, containingType, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Instance = SetParentOperation(instance, this);
        }

        public override IOperation Instance { get; }
    }

/// <summary>
/// Represents a dynamic access to a member of a class, struct, or module.
/// </summary>
internal abstract class LazyDynamicMemberReferenceOperation : BaseDynamicMemberReferenceOperation, IDynamicMemberReferenceOperation
{
    private IOperation _lazyInstanceInterlocked;

    public LazyDynamicMemberReferenceOperation(string memberName, ImmutableArray<ITypeSymbol> typeArguments, ITypeSymbol containingType, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(memberName, typeArguments, containingType, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInstance();

    public override IOperation Instance
    {
        get
        {
            if (_lazyInstanceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInstanceInterlocked, CreateInstance(), null);
                VerifyParentOperation(this, _lazyInstanceInterlocked);
            }

            return _lazyInstanceInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a textual literal numeric, string, etc. expression.
    /// </summary>
    internal sealed partial class LiteralOperation : Operation, ILiteralOperation
    {
        public LiteralOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Literal, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitLiteral(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitLiteral(this, argument);
        }
    }

    /// <summary>
    /// Represents a reference to a declared local variable.
    /// </summary>
    internal sealed partial class LocalReferenceOperation : Operation, ILocalReferenceOperation
    {
        public LocalReferenceOperation(ILocalSymbol local, bool isDeclaration, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.LocalReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Local = local;
            IsDeclaration = isDeclaration;
        }
        /// <summary>
        /// Referenced local variable.
        /// </summary>
        public ILocalSymbol Local { get; }
        public bool IsDeclaration { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitLocalReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitLocalReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# lock or a VB SyncLock statement.
    /// </summary>
    internal abstract partial class BaseLockOperation : Operation, ILockOperation
    {
        protected BaseLockOperation(ILocalSymbol lockTakenSymbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Lock, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LockTakenSymbol = lockTakenSymbol;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (LockedValue != null)
                {
                    yield return LockedValue;
                }
                if (Body != null)
                {
                    yield return Body;
                }
            }
        }
        /// <summary>
        /// Expression producing a value to be locked.
        /// </summary>
        public abstract IOperation LockedValue { get; }
        /// <summary>
        /// Body of the lock, to be executed while holding the lock.
        /// </summary>
        public abstract IOperation Body { get; }
        public ILocalSymbol LockTakenSymbol { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitLock(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitLock(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# lock or a VB SyncLock statement.
    /// </summary>
    internal sealed partial class LockOperation : BaseLockOperation, ILockOperation
    {
        public LockOperation(IOperation lockedValue, IOperation body, ILocalSymbol lockTakenSymbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(lockTakenSymbol, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LockedValue = SetParentOperation(lockedValue, this);
            Body = SetParentOperation(body, this);
        }

        public override IOperation LockedValue { get; }
        public override IOperation Body { get; }
    }

/// <summary>
/// Represents a C# lock or a VB SyncLock statement.
/// </summary>
internal abstract class LazyLockOperation : BaseLockOperation, ILockOperation
{
    private IOperation _lazyLockedValueInterlocked;
    private IOperation _lazyBodyInterlocked;

    public LazyLockOperation(ILocalSymbol lockTakenSymbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(lockTakenSymbol, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateLockedValue();
    protected abstract IOperation CreateBody();

    public override IOperation LockedValue
    {
        get
        {
            if (_lazyLockedValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyLockedValueInterlocked, CreateLockedValue(), null);
                VerifyParentOperation(this, _lazyLockedValueInterlocked);
            }

            return _lazyLockedValueInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# while, for, foreach, or do statement, or a VB While, For, For Each, or Do statement.
    /// </summary>
    internal abstract partial class LoopOperation : Operation, ILoopOperation
    {
        protected LoopOperation(LoopKind loopKind, ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            LoopKind = loopKind;
            Locals = locals;
            ContinueLabel = continueLabel;
            ExitLabel = exitLabel;
        }
        /// <summary>
        /// Kind of the loop.
        /// </summary>
        public LoopKind LoopKind { get; }
        /// <summary>
        /// Declarations local to the loop.
        /// </summary>
        public ImmutableArray<ILocalSymbol> Locals { get; }
        public ILabelSymbol ContinueLabel { get; }
        public ILabelSymbol ExitLabel { get; }
        /// <summary>
        /// Body of the loop.
        /// </summary>
        public abstract IOperation Body { get; }
    }

    /// <summary>
    /// Represents a reference to a member of a class, struct, or interface.
    /// </summary>
    internal abstract partial class BaseMemberReferenceOperation : Operation, IMemberReferenceOperation
    {
        protected BaseMemberReferenceOperation(ISymbol member, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Member = member;
        }
        /// <summary>
        /// Instance of the type. Null if the reference is to a static/shared member.
        /// </summary>
        public abstract IOperation Instance { get; }

        /// <summary>
        /// Referenced member.
        /// </summary>
        public ISymbol Member { get; }
    }

    /// <summary>
    /// Represents a reference to a method other than as the target of an invocation.
    /// </summary>
    internal abstract partial class BaseMethodReferenceOperation : BaseMemberReferenceOperation, IMethodReferenceOperation
    {
        public BaseMethodReferenceOperation(IMethodSymbol method, bool isVirtual, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(method, OperationKind.MethodReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
            IsVirtual = isVirtual;
        }
        /// <summary>
        /// Referenced method.
        /// </summary>
        public IMethodSymbol Method => (IMethodSymbol)Member;

        /// <summary>
        /// Indicates whether the reference uses virtual semantics.
        /// </summary>
        public bool IsVirtual { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Instance != null)
                {
                    yield return Instance;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitMethodReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitMethodReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a reference to a method other than as the target of an invocation.
    /// </summary>
    internal sealed partial class MethodReferenceOperation : BaseMethodReferenceOperation, IMethodReferenceOperation
    {
        public MethodReferenceOperation(IMethodSymbol method, bool isVirtual, IOperation instance, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(method, isVirtual, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Instance = SetParentOperation(instance, this);
        }
        /// <summary>
        /// Instance of the type. Null if the reference is to a static/shared member.
        /// </summary>
        public override IOperation Instance { get; }
    }

/// <summary>
/// Represents a reference to a method other than as the target of an invocation.
/// </summary>
internal abstract class LazyMethodReferenceOperation : BaseMethodReferenceOperation, IMethodReferenceOperation
{
    private IOperation _lazyInstanceInterlocked;

    public LazyMethodReferenceOperation(IMethodSymbol method, bool isVirtual, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(method, isVirtual, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInstance();

    public override IOperation Instance
    {
        get
        {
            if (_lazyInstanceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInstanceInterlocked, CreateInstance(), null);
                VerifyParentOperation(this, _lazyInstanceInterlocked);
            }

            return _lazyInstanceInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a null-coalescing expression.
    /// </summary>
    internal abstract partial class BaseCoalesceOperation : Operation, ICoalesceOperation
    {
        protected BaseCoalesceOperation(IConvertibleConversion convertibleValueConversion, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Coalesce, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ConvertibleValueConversion = convertibleValueConversion;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
                if (WhenNull != null)
                {
                    yield return WhenNull;
                }
            }
        }
        /// <summary>
        /// Value to be unconditionally evaluated.
        /// </summary>
        public abstract IOperation Value { get; }
        /// <summary>
        /// Value to be evaluated if <see cref="Value"/> evaluates to null/Nothing.
        /// </summary>
        public abstract IOperation WhenNull { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitCoalesce(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitCoalesce(this, argument);
        }

        // https://github.com/dotnet/roslyn/issues/27599 tracks adding extension methods that expose language specific conversion information
        internal IConvertibleConversion ConvertibleValueConversion { get; }
        public CommonConversion ValueConversion => ConvertibleValueConversion.ToCommonConversion();
    }

    /// <summary>
    /// Represents a null-coalescing expression.
    /// </summary>
    internal sealed partial class CoalesceOperation : BaseCoalesceOperation, ICoalesceOperation
    {
        public CoalesceOperation(IOperation value, IOperation whenNull, IConvertibleConversion convertibleValueConversion, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(convertibleValueConversion, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
            WhenNull = SetParentOperation(whenNull, this);
        }

        public override IOperation Value { get; }
        public override IOperation WhenNull { get; }
    }

/// <summary>
/// Represents a null-coalescing expression.
/// </summary>
internal abstract class LazyCoalesceOperation : BaseCoalesceOperation, ICoalesceOperation
{
    private IOperation _lazyValueInterlocked;
    private IOperation _lazyWhenNullInterlocked;

    public LazyCoalesceOperation(IConvertibleConversion convertibleValueConversion, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(convertibleValueConversion, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();
    protected abstract IOperation CreateWhenNull();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }

    public override IOperation WhenNull
    {
        get
        {
            if (_lazyWhenNullInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyWhenNullInterlocked, CreateWhenNull(), null);
                VerifyParentOperation(this, _lazyWhenNullInterlocked);
            }

            return _lazyWhenNullInterlocked;
        }
    }
}

    internal abstract partial class BaseCoalesceAssignmentOperation : Operation, ICoalesceAssignmentOperation
    {
        protected BaseCoalesceAssignmentOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.CoalesceAssignment, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public abstract IOperation Target { get; }
        public abstract IOperation Value { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Target != null)
                {
                    yield return Target;
                }
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitCoalesceAssignment(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitCoalesceAssignment(this, argument);
        }
    }

    internal sealed class CoalesceAssignmentOperation : BaseCoalesceAssignmentOperation
    {
        public CoalesceAssignmentOperation(IOperation target, IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Target = SetParentOperation(target, this);
            Value = SetParentOperation(value, this);
        }

        public override IOperation Target { get; }
        public override IOperation Value { get; }
    }

internal abstract class LazyCoalesceAssignmentOperation : BaseCoalesceAssignmentOperation
{
    private IOperation _lazyTargetInterlocked;
    private IOperation _lazyWhenNullInterlocked;

    public LazyCoalesceAssignmentOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateTarget();
    protected abstract IOperation CreateValue();

    public override IOperation Target
    {
        get
        {
            if (_lazyTargetInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyTargetInterlocked, CreateTarget(), null);
                VerifyParentOperation(this, _lazyTargetInterlocked);
            }

            return _lazyTargetInterlocked;
        }
    }

    public override IOperation Value
    {
        get
        {
            if (_lazyWhenNullInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyWhenNullInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyWhenNullInterlocked);
            }

            return _lazyWhenNullInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a new/New expression.
    /// </summary>
    internal abstract partial class BaseObjectCreationOperation : Operation, IObjectCreationOperation
    {
        protected BaseObjectCreationOperation(IMethodSymbol constructor, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ObjectCreation, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Constructor = constructor;
        }
        /// <summary>
        /// Constructor to be invoked on the created instance.
        /// </summary>
        public IMethodSymbol Constructor { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }
        /// <summary>
        /// Object or collection initializer, if any.
        /// </summary>
        public abstract IObjectOrCollectionInitializerOperation Initializer { get; }
        /// <summary>
        /// Arguments of the invocation, excluding the instance argument. Arguments are in evaluation order.
        /// </summary>
        /// <remarks>
        /// If the invocation is in its expanded form, then params/ParamArray arguments would be collected into arrays.
        /// Default values are supplied for optional arguments missing in source.
        /// </remarks>
        public abstract ImmutableArray<IArgumentOperation> Arguments { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitObjectCreation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitObjectCreation(this, argument);
        }
    }

    /// <summary>
    /// Represents a new/New expression.
    /// </summary>
    internal sealed partial class ObjectCreationOperation : BaseObjectCreationOperation, IObjectCreationOperation
    {
        public ObjectCreationOperation(IMethodSymbol constructor, IObjectOrCollectionInitializerOperation initializer, ImmutableArray<IArgumentOperation> arguments, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(constructor, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Initializer = SetParentOperation(initializer, this);
            Arguments = SetParentOperation(arguments, this);
        }

        public override IObjectOrCollectionInitializerOperation Initializer { get; }
        public override ImmutableArray<IArgumentOperation> Arguments { get; }
    }

/// <summary>
/// Represents a new/New expression.
/// </summary>
internal abstract class LazyObjectCreationOperation : BaseObjectCreationOperation, IObjectCreationOperation
{
    private IObjectOrCollectionInitializerOperation _lazyInitializerInterlocked;
    private ImmutableArray<IArgumentOperation> _lazyArgumentsInterlocked;

    public LazyObjectCreationOperation(IMethodSymbol constructor, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(constructor, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IObjectOrCollectionInitializerOperation CreateInitializer();
    protected abstract ImmutableArray<IArgumentOperation> CreateArguments();

    public override IObjectOrCollectionInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }

    public override ImmutableArray<IArgumentOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# or VB new/New anonymous object creation expression.
    /// </summary>
    internal abstract partial class BaseAnonymousObjectCreationOperation : Operation, IAnonymousObjectCreationOperation
    {
        protected BaseAnonymousObjectCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.AnonymousObjectCreation, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var initializer in Initializers)
                {
                    if (initializer != null)
                    {
                        yield return initializer;
                    }
                }
            }
        }
        /// <summary>
        /// Explicitly-specified member initializers.
        /// </summary>
        public abstract ImmutableArray<IOperation> Initializers { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitAnonymousObjectCreation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitAnonymousObjectCreation(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# or VB new/New anonymous object creation expression.
    /// </summary>
    internal sealed partial class AnonymousObjectCreationOperation : BaseAnonymousObjectCreationOperation, IAnonymousObjectCreationOperation
    {
        public AnonymousObjectCreationOperation(ImmutableArray<IOperation> initializers, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Initializers = SetParentOperation(initializers, this);
        }

        public override ImmutableArray<IOperation> Initializers { get; }
    }

/// <summary>
/// Represents a C# or VB new/New anonymous object creation expression.
/// </summary>
internal abstract class LazyAnonymousObjectCreationOperation : BaseAnonymousObjectCreationOperation, IAnonymousObjectCreationOperation
{
    private ImmutableArray<IOperation> _lazyInitializersInterlocked;

    public LazyAnonymousObjectCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateInitializers();

    public override ImmutableArray<IOperation> Initializers
    {
        get
        {
            if (_lazyInitializersInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyInitializersInterlocked, CreateInitializers(), default);
                VerifyParentOperation(this, _lazyInitializersInterlocked);
            }

            return _lazyInitializersInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an argument value that has been omitted in an invocation.
    /// </summary>
    internal sealed partial class OmittedArgumentOperation : Operation, IOmittedArgumentOperation
    {
        public OmittedArgumentOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.OmittedArgument, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitOmittedArgument(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitOmittedArgument(this, argument);
        }
    }

    /// <summary>
    /// Represents an initialization of a parameter at the point of declaration.
    /// </summary>
    internal abstract partial class BaseParameterInitializerOperation : SymbolInitializer, IParameterInitializerOperation
    {
        public BaseParameterInitializerOperation(ImmutableArray<ILocalSymbol> locals, IParameterSymbol parameter, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
            Parameter = parameter;
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }

        /// <summary>
        /// Initialized parameter.
        /// </summary>
        public IParameterSymbol Parameter { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitParameterInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitParameterInitializer(this, argument);
        }
    }

    /// <summary>
    /// Represents an initialization of a parameter at the point of declaration.
    /// </summary>
    internal sealed partial class ParameterInitializerOperation : BaseParameterInitializerOperation, IParameterInitializerOperation
    {
        public ParameterInitializerOperation(ImmutableArray<ILocalSymbol> locals, IParameterSymbol parameter, IOperation value, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, parameter, kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents an initialization of a parameter at the point of declaration.
/// </summary>
internal abstract class LazyParameterInitializerOperation : BaseParameterInitializerOperation, IParameterInitializerOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyParameterInitializerOperation(ImmutableArray<ILocalSymbol> locals, IParameterSymbol parameter, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, parameter, kind, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a reference to a parameter.
    /// </summary>
    internal sealed partial class ParameterReferenceOperation : Operation, IParameterReferenceOperation
    {
        public ParameterReferenceOperation(IParameterSymbol parameter, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ParameterReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Parameter = parameter;
        }
        /// <summary>
        /// Referenced parameter.
        /// </summary>
        public IParameterSymbol Parameter { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitParameterReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitParameterReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a parenthesized expression.
    /// </summary>
    internal abstract partial class BaseParenthesizedOperation : Operation, IParenthesizedOperation
    {
        protected BaseParenthesizedOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Parenthesized, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operand != null)
                {
                    yield return Operand;
                }
            }
        }
        /// <summary>
        /// Operand enclosed in parentheses.
        /// </summary>
        public abstract IOperation Operand { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitParenthesized(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitParenthesized(this, argument);
        }
    }

    /// <summary>
    /// Represents a parenthesized expression.
    /// </summary>
    internal sealed partial class ParenthesizedOperation : BaseParenthesizedOperation, IParenthesizedOperation
    {
        public ParenthesizedOperation(IOperation operand, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operand = SetParentOperation(operand, this);
        }

        public override IOperation Operand { get; }
    }

/// <summary>
/// Represents a parenthesized expression.
/// </summary>
internal abstract class LazyParenthesizedOperation : BaseParenthesizedOperation, IParenthesizedOperation
{
    private IOperation _lazyOperandInterlocked;

    public LazyParenthesizedOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperand();

    public override IOperation Operand
    {
        get
        {
            if (_lazyOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperandInterlocked, CreateOperand(), null);
                VerifyParentOperation(this, _lazyOperandInterlocked);
            }

            return _lazyOperandInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a general placeholder when no more specific kind of placeholder is available.
    /// A placeholder is an expression whose meaning is inferred from context.
    /// </summary>
    internal sealed partial class PlaceholderOperation : Operation, IPlaceholderOperation
    {
        public PlaceholderOperation(PlaceholderKind placeholderKind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            // https://github.com/dotnet/roslyn/issues/21294
            // base(OperationKind.Placeholder, semanticModel, syntax, type, constantValue, isImplicit)
            base(OperationKind.None, semanticModel, syntax, type, constantValue, isImplicit)
        {
            PlaceholderKind = placeholderKind;
        }

        public PlaceholderKind PlaceholderKind { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitPlaceholder(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitPlaceholder(this, argument);
        }
    }

    /// <summary>
    /// Represents an initialization of a property.
    /// </summary>
    internal abstract partial class BasePropertyInitializerOperation : SymbolInitializer, IPropertyInitializerOperation
    {
        public BasePropertyInitializerOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<IPropertySymbol> initializedProperties, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
            InitializedProperties = initializedProperties;
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }

        /// <summary>
        /// Initialized properties. There can be multiple properties for Visual Basic 'WithEvents' declaration with AsNew clause.
        /// </summary>
        public ImmutableArray<IPropertySymbol> InitializedProperties { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitPropertyInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitPropertyInitializer(this, argument);
        }
    }

    /// <summary>
    /// Represents an initialization of a property.
    /// </summary>
    internal sealed partial class PropertyInitializerOperation : BasePropertyInitializerOperation, IPropertyInitializerOperation
    {
        public PropertyInitializerOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<IPropertySymbol> initializedProperties, IOperation value, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, initializedProperties, kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents an initialization of a property.
/// </summary>
internal abstract class LazyPropertyInitializerOperation : BasePropertyInitializerOperation, IPropertyInitializerOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyPropertyInitializerOperation(ImmutableArray<ILocalSymbol> locals, ImmutableArray<IPropertySymbol> initializedProperties, OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, initializedProperties, kind, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a reference to a property.
    /// </summary>
    internal abstract partial class BasePropertyReferenceOperation : BaseMemberReferenceOperation, IPropertyReferenceOperation
    {
        protected BasePropertyReferenceOperation(IPropertySymbol property, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(property, OperationKind.PropertyReference, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        /// <summary>
        /// Referenced property.
        /// </summary>
        public IPropertySymbol Property => (IPropertySymbol)Member;
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Instance != null)
                {
                    yield return Instance;
                }
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
            }
        }
        /// <summary>
        /// Arguments of the invocation, excluding the instance argument. Arguments are in evaluation order.
        /// </summary>
        /// <remarks>
        /// If the invocation is in its expanded form, then params/ParamArray arguments would be collected into arrays.
        /// Default values are supplied for optional arguments missing in source.
        /// </remarks>
        public abstract ImmutableArray<IArgumentOperation> Arguments { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitPropertyReference(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitPropertyReference(this, argument);
        }
    }

    /// <summary>
    /// Represents a reference to a property.
    /// </summary>
    internal sealed partial class PropertyReferenceOperation : BasePropertyReferenceOperation, IPropertyReferenceOperation
    {
        public PropertyReferenceOperation(IPropertySymbol property, IOperation instance, ImmutableArray<IArgumentOperation> arguments, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(property, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Instance = SetParentOperation(instance, this);
            Arguments = SetParentOperation(arguments, this);
        }
        public override IOperation Instance { get; }
        public override ImmutableArray<IArgumentOperation> Arguments { get; }
    }

/// <summary>
/// Represents a reference to a property.
/// </summary>
internal abstract class LazyPropertyReferenceOperation : BasePropertyReferenceOperation, IPropertyReferenceOperation
{
    private IOperation _lazyInstanceInterlocked;
    private ImmutableArray<IArgumentOperation> _lazyArgumentsInterlocked;

    public LazyPropertyReferenceOperation(IPropertySymbol property, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(property, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInstance();
    protected abstract ImmutableArray<IArgumentOperation> CreateArguments();

    public override IOperation Instance
    {
        get
        {
            if (_lazyInstanceInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInstanceInterlocked, CreateInstance(), null);
                VerifyParentOperation(this, _lazyInstanceInterlocked);
            }

            return _lazyInstanceInterlocked;
        }
    }

    public override ImmutableArray<IArgumentOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents Case x To y in VB.
    /// </summary>
    internal abstract partial class BaseRangeCaseClauseOperation : BaseCaseClauseOperation, IRangeCaseClauseOperation
    {
        public BaseRangeCaseClauseOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(CaseKind.Range, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public sealed override ILabelSymbol Label => null;

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (MinimumValue != null)
                {
                    yield return MinimumValue;
                }
                if (MaximumValue != null)
                {
                    yield return MaximumValue;
                }
            }
        }
        /// <summary>
        /// Minimum value of the case range.
        /// </summary>
        public abstract IOperation MinimumValue { get; }
        /// <summary>
        /// Maximum value of the case range.
        /// </summary>
        public abstract IOperation MaximumValue { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitRangeCaseClause(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitRangeCaseClause(this, argument);
        }
    }

    /// <summary>
    /// Represents Case x To y in VB.
    /// </summary>
    internal sealed partial class RangeCaseClauseOperation : BaseRangeCaseClauseOperation, IRangeCaseClauseOperation
    {
        public RangeCaseClauseOperation(IOperation minimumValue, IOperation maximumValue, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            MinimumValue = SetParentOperation(minimumValue, this);
            MaximumValue = SetParentOperation(maximumValue, this);
        }

        public override IOperation MinimumValue { get; }
        public override IOperation MaximumValue { get; }
    }

/// <summary>
/// Represents Case x To y in VB.
/// </summary>
internal abstract class LazyRangeCaseClauseOperation : BaseRangeCaseClauseOperation, IRangeCaseClauseOperation
{
    private IOperation _lazyMinimumValueInterlocked;
    private IOperation _lazyMaximumValueInterlocked;

    public LazyRangeCaseClauseOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateMinimumValue();
    protected abstract IOperation CreateMaximumValue();

    public override IOperation MinimumValue
    {
        get
        {
            if (_lazyMinimumValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyMinimumValueInterlocked, CreateMinimumValue(), null);
                VerifyParentOperation(this, _lazyMinimumValueInterlocked);
            }

            return _lazyMinimumValueInterlocked;
        }
    }

    public override IOperation MaximumValue
    {
        get
        {
            if (_lazyMaximumValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyMaximumValueInterlocked, CreateMaximumValue(), null);
                VerifyParentOperation(this, _lazyMaximumValueInterlocked);
            }

            return _lazyMaximumValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents Case Is op x in VB.
    /// </summary>
    internal abstract partial class BaseRelationalCaseClauseOperation : BaseCaseClauseOperation, IRelationalCaseClauseOperation
    {
        public BaseRelationalCaseClauseOperation(BinaryOperatorKind relation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(CaseKind.Relational, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Relation = relation;
        }

        public sealed override ILabelSymbol Label => null;

        /// <summary>
        /// Relational operator used to compare the switch value with the case value.
        /// </summary>
        public BinaryOperatorKind Relation { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }
        /// <summary>
        /// Case value.
        /// </summary>
        public abstract IOperation Value { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitRelationalCaseClause(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitRelationalCaseClause(this, argument);
        }
    }

    /// <summary>
    /// Represents Case Is op x in VB.
    /// </summary>
    internal sealed partial class RelationalCaseClauseOperation : BaseRelationalCaseClauseOperation, IRelationalCaseClauseOperation
    {
        public RelationalCaseClauseOperation(IOperation value, BinaryOperatorKind relation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(relation, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }

        public override IOperation Value { get; }
    }

/// <summary>
/// Represents Case Is op x in VB.
/// </summary>
internal abstract class LazyRelationalCaseClauseOperation : BaseRelationalCaseClauseOperation, IRelationalCaseClauseOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyRelationalCaseClauseOperation(BinaryOperatorKind relation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(relation, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# return or a VB Return statement.
    /// </summary>
    internal abstract partial class BaseReturnOperation : Operation, IReturnOperation
    {
        protected BaseReturnOperation(OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Debug.Assert(kind == OperationKind.Return
                      || kind == OperationKind.YieldReturn
                      || kind == OperationKind.YieldBreak);
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (ReturnedValue != null)
                {
                    yield return ReturnedValue;
                }
            }
        }
        /// <summary>
        /// Value to be returned.
        /// </summary>
        public abstract IOperation ReturnedValue { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitReturn(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitReturn(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# return or a VB Return statement.
    /// </summary>
    internal sealed partial class ReturnOperation : BaseReturnOperation, IReturnOperation
    {
        public ReturnOperation(OperationKind kind, IOperation returnedValue, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ReturnedValue = SetParentOperation(returnedValue, this);
        }

        public override IOperation ReturnedValue { get; }
    }

/// <summary>
/// Represents a C# return or a VB Return statement.
/// </summary>
internal abstract class LazyReturnOperation : BaseReturnOperation, IReturnOperation
{
    private IOperation _lazyReturnedValueInterlocked;

    public LazyReturnOperation(OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(kind, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateReturnedValue();

    public override IOperation ReturnedValue
    {
        get
        {
            if (_lazyReturnedValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyReturnedValueInterlocked, CreateReturnedValue(), null);
                VerifyParentOperation(this, _lazyReturnedValueInterlocked);
            }

            return _lazyReturnedValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents case x in C# or Case x in VB.
    /// </summary>
    internal abstract partial class BaseSingleValueCaseClauseOperation : BaseCaseClauseOperation, ISingleValueCaseClauseOperation
    {
        public BaseSingleValueCaseClauseOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(CaseKind.SingleValue, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Label = label;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }
        /// <summary>
        /// Case value.
        /// </summary>
        public abstract IOperation Value { get; }
        public override ILabelSymbol Label { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitSingleValueCaseClause(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitSingleValueCaseClause(this, argument);
        }
    }

    /// <summary>
    /// Represents case x in C# or Case x in VB.
    /// </summary>
    internal sealed partial class SingleValueCaseClauseOperation : BaseSingleValueCaseClauseOperation, ISingleValueCaseClauseOperation
    {
        public SingleValueCaseClauseOperation(ILabelSymbol label, IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(label, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }

        public override IOperation Value { get; }
    }

/// <summary>
/// Represents case x in C# or Case x in VB.
/// </summary>
internal abstract class LazySingleValueCaseClauseOperation : BaseSingleValueCaseClauseOperation, ISingleValueCaseClauseOperation
{
    private IOperation _lazyValueInterlocked;

    public LazySingleValueCaseClauseOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(label, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents default case in C# or Case Else in VB.
    /// </summary>
    internal sealed partial class DefaultCaseClauseOperation : BaseCaseClauseOperation, IDefaultCaseClauseOperation
    {
        public DefaultCaseClauseOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(CaseKind.Default, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Label = label;
        }
        public override ILabelSymbol Label { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDefaultCaseClause(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDefaultCaseClause(this, argument);
        }
    }

    /// <summary>
    /// Represents a SizeOf expression.
    /// </summary>
    internal sealed partial class SizeOfOperation : Operation, ISizeOfOperation
    {
        public SizeOfOperation(ITypeSymbol typeOperand, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.SizeOf, semanticModel, syntax, type, constantValue, isImplicit)
        {
            TypeOperand = typeOperand;
        }
        /// <summary>
        /// Type operand.
        /// </summary>
        public ITypeSymbol TypeOperand { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitSizeOf(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitSizeOf(this, argument);
        }
    }

    /// <summary>
    /// Represents a VB Stop statement.
    /// </summary>
    internal sealed partial class StopOperation : Operation, IStopOperation
    {
        public StopOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Stop, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitStop(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitStop(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# case or VB Case statement.
    /// </summary>
    internal abstract partial class BaseSwitchCaseOperation : Operation, ISwitchCaseOperation
    {
        protected BaseSwitchCaseOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.SwitchCase, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var clause in Clauses)
                {
                    if (clause != null)
                    {
                        yield return clause;
                    }
                }
                foreach (var body in Body)
                {
                    if (body != null)
                    {
                        yield return body;
                    }
                }
            }
        }
        /// <summary>
        /// Clauses of the case. For C# there is one clause per case, but for VB there can be multiple.
        /// </summary>
        public abstract ImmutableArray<ICaseClauseOperation> Clauses { get; }
        /// <summary>
        /// Statements of the case.
        /// </summary>
        public abstract ImmutableArray<IOperation> Body { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitSwitchCase(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitSwitchCase(this, argument);
        }

        /// <summary>
        /// Optional combined logical condition that accounts for all <see cref="Clauses"/>.
        /// An instance of <see cref="IPlaceholderOperation"/> with kind <see cref="PlaceholderKind.SwitchOperationExpression"/>
        /// is used to refer to the <see cref="ISwitchOperation.Value"/> in context of this expression.
        /// It is not part of <see cref="Children"/> list and likely contains duplicate nodes for
        /// nodes exposed by <see cref="Clauses"/>, like <see cref="ISingleValueCaseClauseOperation.Value"/>,
        /// etc.
        /// Never set for C# at the moment.
        /// </summary>
        public abstract IOperation Condition { get; }
    }

    /// <summary>
    /// Represents a C# case or VB Case statement.
    /// </summary>
    internal sealed partial class SwitchCaseOperation : BaseSwitchCaseOperation, ISwitchCaseOperation
    {
        public SwitchCaseOperation(ImmutableArray<ILocalSymbol> locals, IOperation condition, ImmutableArray<ICaseClauseOperation> clauses, ImmutableArray<IOperation> body, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Clauses = SetParentOperation(clauses, this);
            Condition = SetParentOperation(condition, null);
            Body = SetParentOperation(body, this);
        }

        public override ImmutableArray<ICaseClauseOperation> Clauses { get; }
        public override IOperation Condition { get; }
        public override ImmutableArray<IOperation> Body { get; }
    }

/// <summary>
/// Represents a C# case or VB Case statement.
/// </summary>
internal abstract class LazySwitchCaseOperation : BaseSwitchCaseOperation, ISwitchCaseOperation
{
    private ImmutableArray<ICaseClauseOperation> _lazyClausesInterlocked;
    private IOperation _lazyConditionInterlocked;
    private ImmutableArray<IOperation> _lazyBodyInterlocked;

    public LazySwitchCaseOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<ICaseClauseOperation> CreateClauses();
    protected abstract IOperation CreateCondition();
    protected abstract ImmutableArray<IOperation> CreateBody();

    public override ImmutableArray<ICaseClauseOperation> Clauses
    {
        get
        {
            if (_lazyClausesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyClausesInterlocked, CreateClauses(), default);
                VerifyParentOperation(this, _lazyClausesInterlocked);
            }

            return _lazyClausesInterlocked;
        }
    }

    public override IOperation Condition
    {
        get
        {
            if (_lazyConditionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyConditionInterlocked, CreateCondition(), null);
                VerifyParentOperation(null, _lazyConditionInterlocked);
            }

            return _lazyConditionInterlocked;
        }
    }

    public override ImmutableArray<IOperation> Body
    {
        get
        {
            if (_lazyBodyInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyBodyInterlocked, CreateBody(), default);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# switch or VB Select Case statement.
    /// </summary>
    internal abstract partial class BaseSwitchOperation : Operation, ISwitchOperation
    {
        protected BaseSwitchOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Switch, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
            ExitLabel = exitLabel;
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
                foreach (var @case in Cases)
                {
                    if (@case != null)
                    {
                        yield return @case;
                    }
                }
            }
        }
        public ILabelSymbol ExitLabel { get; }
        /// <summary>
        /// Value to be switched upon.
        /// </summary>
        public abstract IOperation Value { get; }
        /// <summary>
        /// Cases of the switch.
        /// </summary>
        public abstract ImmutableArray<ISwitchCaseOperation> Cases { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitSwitch(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitSwitch(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# switch or VB Select Case statement.
    /// </summary>
    internal sealed partial class SwitchOperation : BaseSwitchOperation, ISwitchOperation
    {
        public SwitchOperation(ImmutableArray<ILocalSymbol> locals, IOperation value, ImmutableArray<ISwitchCaseOperation> cases, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
            Cases = SetParentOperation(cases, this);
        }

        public override IOperation Value { get; }
        public override ImmutableArray<ISwitchCaseOperation> Cases { get; }
    }

/// <summary>
/// Represents a C# switch or VB Select Case statement.
/// </summary>
internal abstract class LazySwitchOperation : BaseSwitchOperation, ISwitchOperation
{
    private IOperation _lazyValueInterlocked;
    private ImmutableArray<ISwitchCaseOperation> _lazyCasesInterlocked;

    public LazySwitchOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();
    protected abstract ImmutableArray<ISwitchCaseOperation> CreateCases();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }

    public override ImmutableArray<ISwitchCaseOperation> Cases
    {
        get
        {
            if (_lazyCasesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyCasesInterlocked, CreateCases(), default);
                VerifyParentOperation(this, _lazyCasesInterlocked);
            }

            return _lazyCasesInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an initializer for a field, property, or parameter.
    /// </summary>
    internal abstract partial class SymbolInitializer : Operation
    {
        protected SymbolInitializer(OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(kind, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public abstract IOperation Value { get; }
    }

    /// <summary>
    /// Represents a C# try or a VB Try statement.
    /// </summary>
    internal abstract partial class BaseTryOperation : Operation, ITryOperation
    {
        protected BaseTryOperation(ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Try, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ExitLabel = exitLabel;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Body != null)
                {
                    yield return Body;
                }
                foreach (var @catch in Catches)
                {
                    if (@catch != null)
                    {
                        yield return @catch;
                    }
                }
                if (Finally != null)
                {
                    yield return Finally;
                }
            }
        }
        public ILabelSymbol ExitLabel { get; }
        /// <summary>
        /// Body of the try, over which the handlers are active.
        /// </summary>
        public abstract IBlockOperation Body { get; }
        /// <summary>
        /// Catch clauses of the try.
        /// </summary>
        public abstract ImmutableArray<ICatchClauseOperation> Catches { get; }
        /// <summary>
        /// Finally handler of the try.
        /// </summary>
        public abstract IBlockOperation Finally { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitTry(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitTry(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# try or a VB Try statement.
    /// </summary>
    internal sealed partial class TryOperation : BaseTryOperation, ITryOperation
    {
        public TryOperation(IBlockOperation body, ImmutableArray<ICatchClauseOperation> catches, IBlockOperation finallyHandler, ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Body = SetParentOperation(body, this);
            Catches = SetParentOperation(catches, this);
            Finally = SetParentOperation(finallyHandler, this);
        }

        public override IBlockOperation Body { get; }
        public override ImmutableArray<ICatchClauseOperation> Catches { get; }
        public override IBlockOperation Finally { get; }
    }

/// <summary>
/// Represents a C# try or a VB Try statement.
/// </summary>
internal abstract class LazyTryOperation : BaseTryOperation, ITryOperation
{
    private IBlockOperation _lazyBodyInterlocked;
    private ImmutableArray<ICatchClauseOperation> _lazyCatchesInterlocked;
    private IBlockOperation _lazyFinallyHandlerInterlocked;

    public LazyTryOperation(ILabelSymbol exitLabel, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(exitLabel, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IBlockOperation CreateBody();
    protected abstract ImmutableArray<ICatchClauseOperation> CreateCatches();
    protected abstract IBlockOperation CreateFinally();

    public override IBlockOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }

    public override ImmutableArray<ICatchClauseOperation> Catches
    {
        get
        {
            if (_lazyCatchesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyCatchesInterlocked, CreateCatches(), default);
                VerifyParentOperation(this, _lazyCatchesInterlocked);
            }

            return _lazyCatchesInterlocked;
        }
    }

    public override IBlockOperation Finally
    {
        get
        {
            if (_lazyFinallyHandlerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyFinallyHandlerInterlocked, CreateFinally(), null);
                VerifyParentOperation(this, _lazyFinallyHandlerInterlocked);
            }

            return _lazyFinallyHandlerInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a tuple expression.
    /// </summary>
    internal abstract partial class BaseTupleOperation : Operation, ITupleOperation
    {
        protected BaseTupleOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, ITypeSymbol naturalType, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Tuple, semanticModel, syntax, type, constantValue, isImplicit)
        {
            NaturalType = naturalType;
        }

        /// <summary>
        /// Natural type of the tuple, or null if tuple doesn't have a natural type.
        /// Natural type can be different from <see cref="IOperation.Type"/> depending on the
        /// conversion context, in which the tuple is used.
        /// </summary>
        public ITypeSymbol NaturalType { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var element in Elements)
                {
                    if (element != null)
                    {
                        yield return element;
                    }
                }
            }
        }
        /// <summary>
        /// Elements for tuple expression.
        /// </summary>
        public abstract ImmutableArray<IOperation> Elements { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitTuple(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitTuple(this, argument);
        }
    }

    /// <summary>
    /// Represents a tuple expression.
    /// </summary>
    internal sealed partial class TupleOperation : BaseTupleOperation, ITupleOperation
    {
        public TupleOperation(ImmutableArray<IOperation> elements, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, ITypeSymbol naturalType, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, naturalType, constantValue, isImplicit)
        {
            Elements = SetParentOperation(elements, this);
        }

        public override ImmutableArray<IOperation> Elements { get; }
    }

/// <summary>
/// Represents a tuple expression.
/// </summary>
internal abstract class LazyTupleOperation : BaseTupleOperation, ITupleOperation
{
    private ImmutableArray<IOperation> _lazyElementsInterlocked;

    public LazyTupleOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, ITypeSymbol naturalType, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, naturalType, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateElements();

    public override ImmutableArray<IOperation> Elements
    {
        get
        {
            if (_lazyElementsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyElementsInterlocked, CreateElements(), default);
                VerifyParentOperation(this, _lazyElementsInterlocked);
            }

            return _lazyElementsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a TypeOf expression.
    /// </summary>
    internal sealed partial class TypeOfOperation : Operation, ITypeOfOperation
    {
        public TypeOfOperation(ITypeSymbol typeOperand, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.TypeOf, semanticModel, syntax, type, constantValue, isImplicit)
        {
            TypeOperand = typeOperand;
        }
        /// <summary>
        /// Type operand.
        /// </summary>
        public ITypeSymbol TypeOperand { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitTypeOf(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitTypeOf(this, argument);
        }
    }

    /// <summary>
    /// Represents a type parameter object creation expression, i.e. new T(), where T is a type parameter with new constraint.
    /// </summary>
    internal abstract partial class BaseTypeParameterObjectCreationOperation : Operation, ITypeParameterObjectCreationOperation
    {
        public BaseTypeParameterObjectCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.TypeParameterObjectCreation, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }
        /// <summary>
        /// Object or collection initializer, if any.
        /// </summary>
        public abstract IObjectOrCollectionInitializerOperation Initializer { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitTypeParameterObjectCreation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitTypeParameterObjectCreation(this, argument);
        }
    }

    /// <summary>
    /// Represents a type parameter object creation expression, i.e. new T(), where T is a type parameter with new constraint.
    /// </summary>
    internal sealed partial class TypeParameterObjectCreationOperation : BaseTypeParameterObjectCreationOperation, ITypeParameterObjectCreationOperation
    {
        public TypeParameterObjectCreationOperation(IObjectOrCollectionInitializerOperation initializer, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Initializer = SetParentOperation(initializer, this);
        }
        public override IObjectOrCollectionInitializerOperation Initializer { get; }
    }

/// <summary>
/// Represents a type parameter object creation expression, i.e. new T(), where T is a type parameter with new constraint.
/// </summary>
internal abstract class LazyTypeParameterObjectCreationOperation : BaseTypeParameterObjectCreationOperation, ITypeParameterObjectCreationOperation
{
    private IObjectOrCollectionInitializerOperation _lazyInitializerInterlocked;

    public LazyTypeParameterObjectCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IObjectOrCollectionInitializerOperation CreateInitializer();

    public override IObjectOrCollectionInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }
}

    /// <remarks>
    /// Represents a dynamically bound expression that can have argument names or refkinds.
    /// </remarks>
    internal abstract partial class HasDynamicArgumentsExpression : Operation
    {
        protected HasDynamicArgumentsExpression(OperationKind operationKind, ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(operationKind, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ArgumentNames = argumentNames;
            ArgumentRefKinds = argumentRefKinds;
        }

        /// <summary>
        /// Optional argument names for named arguments.
        /// </summary>
        public ImmutableArray<string> ArgumentNames { get; }
        /// <summary>
        /// Optional argument ref kinds.
        /// </summary>
        public ImmutableArray<RefKind> ArgumentRefKinds { get; }
        /// <summary>
        /// Dynamically bound arguments, excluding the instance argument.
        /// </summary>
        public abstract ImmutableArray<IOperation> Arguments { get; }
    }

    /// <remarks>
    /// Represents a dynamically bound new/New expression.
    /// </remarks>
    internal abstract partial class BaseDynamicObjectCreationOperation : HasDynamicArgumentsExpression, IDynamicObjectCreationOperation
    {
        public BaseDynamicObjectCreationOperation(ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DynamicObjectCreation, argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }
        /// <summary>
        /// Object or collection initializer, if any.
        /// </summary>
        public abstract IObjectOrCollectionInitializerOperation Initializer { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDynamicObjectCreation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDynamicObjectCreation(this, argument);
        }
    }

    /// <remarks>
    /// Represents a dynamically bound new/New expression.
    /// </remarks>
    internal sealed partial class DynamicObjectCreationOperation : BaseDynamicObjectCreationOperation, IDynamicObjectCreationOperation
    {
        public DynamicObjectCreationOperation(ImmutableArray<IOperation> arguments, ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, IObjectOrCollectionInitializerOperation initializer, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Arguments = SetParentOperation(arguments, this);
            Initializer = SetParentOperation(initializer, this);
        }
        public override ImmutableArray<IOperation> Arguments { get; }
        public override IObjectOrCollectionInitializerOperation Initializer { get; }
    }

/// <remarks>
/// Represents a dynamically bound new/New expression.
/// </remarks>
internal abstract class LazyDynamicObjectCreationOperation : BaseDynamicObjectCreationOperation, IDynamicObjectCreationOperation
{
    private ImmutableArray<IOperation> _lazyArgumentsInterlocked;
    private IObjectOrCollectionInitializerOperation _lazyInitializerInterlocked;

    public LazyDynamicObjectCreationOperation(ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateArguments();
    protected abstract IObjectOrCollectionInitializerOperation CreateInitializer();

    public override ImmutableArray<IOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }

    public override IObjectOrCollectionInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }
}

    /// <remarks>
    /// Represents a dynamically bound invocation expression in C# and late bound invocation in VB.
    /// </remarks>
    internal abstract partial class BaseDynamicInvocationOperation : HasDynamicArgumentsExpression, IDynamicInvocationOperation
    {
        public BaseDynamicInvocationOperation(ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DynamicInvocation, argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
            }
        }
        /// <summary>
        /// Dynamically or late bound expression.
        /// </summary>
        public abstract IOperation Operation { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDynamicInvocation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDynamicInvocation(this, argument);
        }
    }

    /// <remarks>
    /// Represents a dynamically bound invocation expression in C# and late bound invocation in VB.
    /// </remarks>
    internal sealed partial class DynamicInvocationOperation : BaseDynamicInvocationOperation, IDynamicInvocationOperation
    {
        public DynamicInvocationOperation(IOperation operation, ImmutableArray<IOperation> arguments, ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operation = SetParentOperation(operation, this);
            Arguments = SetParentOperation(arguments, this);
        }
        public override IOperation Operation { get; }
        public override ImmutableArray<IOperation> Arguments { get; }
    }

/// <remarks>
/// Represents a dynamically bound invocation expression in C# and late bound invocation in VB.
/// </remarks>
internal abstract class LazyDynamicInvocationOperation : BaseDynamicInvocationOperation, IDynamicInvocationOperation
{
    private IOperation _lazyOperationInterlocked;
    private ImmutableArray<IOperation> _lazyArgumentsInterlocked;

    public LazyDynamicInvocationOperation(ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();
    protected abstract ImmutableArray<IOperation> CreateArguments();

    public override IOperation Operation
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }

    public override ImmutableArray<IOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }
}

    /// <remarks>
    /// Represents a dynamic indexer expression in C#.
    /// </remarks>
    internal abstract partial class BaseDynamicIndexerAccessOperation : HasDynamicArgumentsExpression, IDynamicIndexerAccessOperation
    {
        public BaseDynamicIndexerAccessOperation(ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DynamicIndexerAccess, argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
                foreach (var argument in Arguments)
                {
                    if (argument != null)
                    {
                        yield return argument;
                    }
                }
            }
        }
        /// <summary>
        /// Dynamically indexed expression.
        /// </summary>
        public abstract IOperation Operation { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDynamicIndexerAccess(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDynamicIndexerAccess(this, argument);
        }
    }

    /// <remarks>
    /// Represents a dynamic indexer expression in C#.
    /// </remarks>
    internal sealed partial class DynamicIndexerAccessOperation : BaseDynamicIndexerAccessOperation, IDynamicIndexerAccessOperation
    {
        public DynamicIndexerAccessOperation(IOperation operation, ImmutableArray<IOperation> arguments, ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operation = SetParentOperation(operation, this);
            Arguments = SetParentOperation(arguments, this);
        }
        public override IOperation Operation { get; }
        public override ImmutableArray<IOperation> Arguments { get; }
    }

/// <remarks>
/// Represents a dynamic indexer expression in C#.
/// </remarks>
internal abstract class LazyDynamicIndexerAccessOperation : BaseDynamicIndexerAccessOperation, IDynamicIndexerAccessOperation
{
    private IOperation _lazyOperationInterlocked;
    private ImmutableArray<IOperation> _lazyArgumentsInterlocked;

    public LazyDynamicIndexerAccessOperation(ImmutableArray<string> argumentNames, ImmutableArray<RefKind> argumentRefKinds, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(argumentNames, argumentRefKinds, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();
    protected abstract ImmutableArray<IOperation> CreateArguments();

    public override IOperation Operation
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }

    public override ImmutableArray<IOperation> Arguments
    {
        get
        {
            if (_lazyArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyArgumentsInterlocked, CreateArguments(), default);
                VerifyParentOperation(this, _lazyArgumentsInterlocked);
            }

            return _lazyArgumentsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an operation with one operand.
    /// </summary>
    internal abstract partial class BaseUnaryOperation : Operation, IUnaryOperation
    {
        protected BaseUnaryOperation(UnaryOperatorKind unaryOperationKind, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Unary, semanticModel, syntax, type, constantValue, isImplicit)
        {
            OperatorKind = unaryOperationKind;
            IsLifted = isLifted;
            IsChecked = isChecked;
            OperatorMethod = operatorMethod;
        }
        /// <summary>
        /// Kind of unary operation.
        /// </summary>
        public UnaryOperatorKind OperatorKind { get; }
        /// <summary>
        /// Operator method used by the operation, null if the operation does not use an operator method.
        /// </summary>
        public IMethodSymbol OperatorMethod { get; }
        /// <summary>
        /// <see langword="true"/> if this is a 'lifted' binary operator.  When there is an
        /// operator that is defined to work on a value type, 'lifted' operators are
        /// created to work on the <see cref="System.Nullable{T}"/> versions of those
        /// value types.
        /// </summary>
        public bool IsLifted { get; }
        /// <summary>
        /// <see langword="true"/> if overflow checking is performed for the arithmetic operation.
        /// </summary>
        public bool IsChecked { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operand != null)
                {
                    yield return Operand;
                }
            }
        }
        /// <summary>
        /// Single operand.
        /// </summary>
        public abstract IOperation Operand { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitUnaryOperator(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitUnaryOperator(this, argument);
        }
    }

    /// <summary>
    /// Represents an operation with one operand.
    /// </summary>
    internal sealed partial class UnaryOperation : BaseUnaryOperation, IUnaryOperation
    {
        public UnaryOperation(UnaryOperatorKind unaryOperationKind, IOperation operand, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(unaryOperationKind, isLifted, isChecked, operatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operand = SetParentOperation(operand, this);
        }

        public override IOperation Operand { get; }
    }

/// <summary>
/// Represents an operation with one operand.
/// </summary>
internal abstract class LazyUnaryOperation : BaseUnaryOperation, IUnaryOperation
{
    private IOperation _lazyOperandInterlocked;

    public LazyUnaryOperation(UnaryOperatorKind unaryOperationKind, bool isLifted, bool isChecked, IMethodSymbol operatorMethod, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(unaryOperationKind, isLifted, isChecked, operatorMethod, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperand();

    public override IOperation Operand
    {
        get
        {
            if (_lazyOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperandInterlocked, CreateOperand(), null);
                VerifyParentOperation(this, _lazyOperandInterlocked);
            }

            return _lazyOperandInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# using or VB Using statement.
    /// </summary>
    internal abstract partial class BaseUsingOperation : Operation, IUsingOperation
    {
        protected BaseUsingOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Using, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Locals = locals;
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Resources != null)
                {
                    yield return Resources;
                }
                if (Body != null)
                {
                    yield return Body;
                }
            }
        }

        /// <summary>
        /// Declaration introduced or resource held by the using.
        /// </summary>
        public abstract IOperation Resources { get; }

        /// <summary>
        /// Body of the using, over which the resources of the using are maintained.
        /// </summary>
        public abstract IOperation Body { get; }

        /// <summary>
        /// Locals declared within the <see cref="Resources"/> with scope spanning across this entire <see cref="IUsingOperation"/>.
        /// </summary>
        public ImmutableArray<ILocalSymbol> Locals { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitUsing(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitUsing(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# using or VB Using statement.
    /// </summary>
    internal sealed partial class UsingOperation : BaseUsingOperation, IUsingOperation
    {
        public UsingOperation(IOperation resources, IOperation body, ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Resources = SetParentOperation(resources, this);
            Body = SetParentOperation(body, this);
        }

        public override IOperation Resources { get; }
        public override IOperation Body { get; }
    }

/// <summary>
/// Represents a C# using or VB Using statement.
/// </summary>
internal abstract class LazyUsingOperation : BaseUsingOperation, IUsingOperation
{
    private IOperation _lazyResourcesInterlocked;
    private IOperation _lazyBodyInterlocked;

    public LazyUsingOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateResources();
    protected abstract IOperation CreateBody();

    public override IOperation Resources
    {
        get
        {
            if (_lazyResourcesInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyResourcesInterlocked, CreateResources(), null);
                VerifyParentOperation(this, _lazyResourcesInterlocked);
            }

            return _lazyResourcesInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }
}

    internal abstract partial class BaseVariableDeclaratorOperation : Operation, IVariableDeclaratorOperation
    {
        protected BaseVariableDeclaratorOperation(ILocalSymbol symbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.VariableDeclarator, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Symbol = symbol;
        }

        public ILocalSymbol Symbol { get; }
        /// <summary>
        /// Optional initializer of the variable.
        /// </summary>
        public abstract IVariableInitializerOperation Initializer { get; }
        public abstract ImmutableArray<IOperation> IgnoredArguments { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var arg in IgnoredArguments)
                {
                    yield return arg;
                }
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitVariableDeclarator(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitVariableDeclarator(this, argument);
        }
    }

    /// <summary>
    /// Represents a local variable declaration.
    /// </summary>
    internal sealed partial class VariableDeclaratorOperation : BaseVariableDeclaratorOperation
    {
        public VariableDeclaratorOperation(ILocalSymbol symbol, IVariableInitializerOperation initializer, ImmutableArray<IOperation> ignoredArguments, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(symbol, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Initializer = SetParentOperation(initializer, this);
            IgnoredArguments = SetParentOperation(ignoredArguments, this);
        }

        public override IVariableInitializerOperation Initializer { get; }
        public override ImmutableArray<IOperation> IgnoredArguments { get; }
    }

/// <summary>
/// Represents a local variable declaration.
/// </summary>
internal abstract class LazyVariableDeclaratorOperation : BaseVariableDeclaratorOperation
{
    private IVariableInitializerOperation _lazyInitializerInterlocked;
    private ImmutableArray<IOperation> _lazyIgnoredArgumentsInterlocked;

    public LazyVariableDeclaratorOperation(ILocalSymbol symbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(symbol, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IVariableInitializerOperation CreateInitializer();
    protected abstract ImmutableArray<IOperation> CreateIgnoredArguments();

    public override IVariableInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }

    public override ImmutableArray<IOperation> IgnoredArguments
    {
        get
        {
            if (_lazyIgnoredArgumentsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyIgnoredArgumentsInterlocked, CreateIgnoredArguments(), default);
                VerifyParentOperation(this, _lazyIgnoredArgumentsInterlocked);
            }

            return _lazyIgnoredArgumentsInterlocked;
        }
    }
}

    internal abstract partial class BaseVariableDeclarationOperation : Operation, IVariableDeclarationOperation
    {
        protected BaseVariableDeclarationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.VariableDeclaration, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public abstract ImmutableArray<IVariableDeclaratorOperation> Declarators { get; }

        /// <summary>
        /// Optional initializer of the variable.
        /// </summary>
        public abstract IVariableInitializerOperation Initializer { get; }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var declaration in Declarators)
                {
                    yield return declaration;
                }

                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitVariableDeclaration(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitVariableDeclaration(this, argument);
        }
    }

    internal sealed partial class VariableDeclarationOperation : BaseVariableDeclarationOperation
    {
        public VariableDeclarationOperation(ImmutableArray<IVariableDeclaratorOperation> declarations, IVariableInitializerOperation initializer, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Declarators = SetParentOperation(declarations, this);
            Initializer = SetParentOperation(initializer, this);
        }

        public override ImmutableArray<IVariableDeclaratorOperation> Declarators { get; }
        public override IVariableInitializerOperation Initializer { get; }
    }

internal abstract class LazyVariableDeclarationOperation : BaseVariableDeclarationOperation
{
    private ImmutableArray<IVariableDeclaratorOperation> _lazyDeclaratorsInterlocked;
    private IVariableInitializerOperation _lazyInitializerInterlocked;

    public LazyVariableDeclarationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IVariableDeclaratorOperation> CreateDeclarators();
    protected abstract IVariableInitializerOperation CreateInitializer();

    public override ImmutableArray<IVariableDeclaratorOperation> Declarators
    {
        get
        {
            if (_lazyDeclaratorsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyDeclaratorsInterlocked, CreateDeclarators(), default);
                VerifyParentOperation(this, _lazyDeclaratorsInterlocked);
            }

            return _lazyDeclaratorsInterlocked;
        }
    }

    public override IVariableInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a local variable declaration statement.
    /// </summary>
    internal abstract partial class BaseVariableDeclarationGroupOperation : Operation, IVariableDeclarationGroupOperation
    {
        protected BaseVariableDeclarationGroupOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.VariableDeclarationGroup, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var declaration in Declarations)
                {
                    if (declaration != null)
                    {
                        yield return declaration;
                    }
                }
            }
        }
        /// <summary>
        /// Variables declared by the statement.
        /// </summary>
        public abstract ImmutableArray<IVariableDeclarationOperation> Declarations { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitVariableDeclarationGroup(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitVariableDeclarationGroup(this, argument);
        }
    }

    /// <summary>
    /// Represents a local variable declaration statement.
    /// </summary>
    internal sealed partial class VariableDeclarationGroupOperation : BaseVariableDeclarationGroupOperation, IVariableDeclarationGroupOperation
    {
        public VariableDeclarationGroupOperation(ImmutableArray<IVariableDeclarationOperation> declarations, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Declarations = SetParentOperation(declarations, this);
        }

        public override ImmutableArray<IVariableDeclarationOperation> Declarations { get; }
    }

/// <summary>
/// Represents a local variable declaration statement.
/// </summary>
internal abstract class LazyVariableDeclarationGroupOperation : BaseVariableDeclarationGroupOperation, IVariableDeclarationGroupOperation
{
    private ImmutableArray<IVariableDeclarationOperation> _lazyDeclarationsInterlocked;

    public LazyVariableDeclarationGroupOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IVariableDeclarationOperation> CreateDeclarations();

    public override ImmutableArray<IVariableDeclarationOperation> Declarations
    {
        get
        {
            if (_lazyDeclarationsInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyDeclarationsInterlocked, CreateDeclarations(), default);
                VerifyParentOperation(this, _lazyDeclarationsInterlocked);
            }

            return _lazyDeclarationsInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a while or do while loop.
    /// <para>
    /// Current usage:
    ///  (1) C# 'while' and 'do while' loop statements.
    ///  (2) VB 'While', 'Do While' and 'Do Until' loop statements.
    /// </para>
    /// </summary>
    internal abstract partial class BaseWhileLoopOperation : LoopOperation, IWhileLoopOperation
    {
        public BaseWhileLoopOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, bool conditionIsTop, bool conditionIsUntil, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(LoopKind.While, locals, continueLabel, exitLabel, OperationKind.Loop, semanticModel, syntax, type, constantValue, isImplicit)
        {
            ConditionIsTop = conditionIsTop;
            ConditionIsUntil = conditionIsUntil;
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (ConditionIsTop)
                {
                    if (Condition != null)
                    {
                        yield return Condition;
                    }
                }
                if (Body != null)
                {
                    yield return Body;
                }
                if (!ConditionIsTop)
                {
                    if (Condition != null)
                    {
                        yield return Condition;
                    }
                }
                if (IgnoredCondition != null)
                {
                    yield return IgnoredCondition;
                }
            }
        }
        /// <summary>
        /// Condition of the loop.
        /// </summary>
        public abstract IOperation Condition { get; }
        /// <summary>
        /// True if the <see cref="Condition"/> is evaluated at start of each loop iteration.
        /// False if it is evaluated at the end of each loop iteration.
        /// </summary>

        public bool ConditionIsTop { get; }

        /// <summary>
        /// True if the loop has 'Until' loop semantics and the loop is executed while <see cref="Condition"/> is false.
        /// </summary>

        public bool ConditionIsUntil { get; }
        /// <summary>
        /// Additional conditional supplied for loop in error cases, which is ignored by the compiler.
        /// For example, for VB 'Do While' or 'Do Until' loop with syntax errors where both the top and bottom conditions are provided.
        /// The top condition is preferred and exposed as <see cref="Condition"/> and the bottom condition is ignored and exposed by this property.
        /// This property should be null for all non-error cases.
        /// </summary>
        public abstract IOperation IgnoredCondition { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitWhileLoop(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitWhileLoop(this, argument);
        }
    }

    /// <summary>
    /// Represents a while or do while loop.
    /// <para>
    /// Current usage:
    ///  (1) C# 'while' and 'do while' loop statements.
    ///  (2) VB 'While', 'Do While' and 'Do Until' loop statements.
    /// </para>
    /// </summary>
    internal sealed partial class WhileLoopOperation : BaseWhileLoopOperation, IWhileLoopOperation
    {
        public WhileLoopOperation(IOperation condition, IOperation body, IOperation ignoredCondition, ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, bool conditionIsTop, bool conditionIsUntil, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(locals, continueLabel, exitLabel, conditionIsTop, conditionIsUntil, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Condition = SetParentOperation(condition, this);
            Body = SetParentOperation(body, this);
            IgnoredCondition = SetParentOperation(ignoredCondition, this);
        }
        public override IOperation Condition { get; }
        public override IOperation Body { get; }
        public override IOperation IgnoredCondition { get; }
    }

/// <summary>
/// Represents a while or do while loop.
/// <para>
/// Current usage:
///  (1) C# 'while' and 'do while' loop statements.
///  (2) VB 'While', 'Do While' and 'Do Until' loop statements.
/// </para>
/// </summary>
internal abstract class LazyWhileLoopOperation : BaseWhileLoopOperation, IWhileLoopOperation
{
    private IOperation _lazyConditionInterlocked;
    private IOperation _lazyBodyInterlocked;
    private IOperation _lazyIgnoredConditionInterlocked;

    public LazyWhileLoopOperation(ImmutableArray<ILocalSymbol> locals, ILabelSymbol continueLabel, ILabelSymbol exitLabel, bool conditionIsTop, bool conditionIsUntil, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(locals, continueLabel, exitLabel, conditionIsTop, conditionIsUntil, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateCondition();
    protected abstract IOperation CreateBody();
    protected abstract IOperation CreateIgnoredCondition();

    public override IOperation Condition
    {
        get
        {
            if (_lazyConditionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyConditionInterlocked, CreateCondition(), null);
                VerifyParentOperation(this, _lazyConditionInterlocked);
            }

            return _lazyConditionInterlocked;
        }
    }

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }

    public override IOperation IgnoredCondition
    {
        get
        {
            if (_lazyIgnoredConditionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyIgnoredConditionInterlocked, CreateIgnoredCondition(), null);
                VerifyParentOperation(this, _lazyIgnoredConditionInterlocked);
            }

            return _lazyIgnoredConditionInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a VB With statement.
    /// </summary>
    internal abstract partial class BaseWithOperation : Operation, IWithOperation
    {
        protected BaseWithOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            // https://github.com/dotnet/roslyn/issues/22005
            // base(OperationKind.With, semanticModel, syntax, type, constantValue, isImplicit)
            base(OperationKind.None, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
                if (Body != null)
                {
                    yield return Body;
                }
            }
        }
        /// <summary>
        /// Body of the with.
        /// </summary>
        public abstract IOperation Body { get; }
        /// <summary>
        /// Value to whose members leading-dot-qualified references within the with body bind.
        /// </summary>
        public abstract IOperation Value { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitWith(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitWith(this, argument);
        }
    }

    /// <summary>
    /// Represents a VB With statement.
    /// </summary>
    internal sealed partial class WithOperation : BaseWithOperation, IWithOperation
    {
        public WithOperation(IOperation body, IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Body = SetParentOperation(body, this);
            Value = SetParentOperation(value, this);
        }

        public override IOperation Body { get; }
        public override IOperation Value { get; }
    }

/// <summary>
/// Represents a VB With statement.
/// </summary>
internal abstract class LazyWithOperation : BaseWithOperation, IWithOperation
{
    private IOperation _lazyBodyInterlocked;
    private IOperation _lazyValueInterlocked;

    public LazyWithOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateBody();
    protected abstract IOperation CreateValue();

    public override IOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a local function statement.
    /// </summary>
    internal abstract partial class BaseLocalFunctionOperation : Operation, ILocalFunctionOperation
    {
        protected BaseLocalFunctionOperation(IMethodSymbol symbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.LocalFunction, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Symbol = symbol;
        }
        /// <summary>
        /// Local function symbol.
        /// </summary>
        public IMethodSymbol Symbol { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Body != null)
                {
                    yield return Body;
                }
                if (IgnoredBody != null)
                {
                    yield return IgnoredBody;
                }
            }
        }

        /// <summary>
        /// Body of the local function.
        /// </summary>
        public abstract IBlockOperation Body { get; }
        public abstract IBlockOperation IgnoredBody { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitLocalFunction(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitLocalFunction(this, argument);
        }
    }

    /// <summary>
    /// Represents a local function statement.
    /// </summary>
    internal sealed partial class LocalFunctionOperation : BaseLocalFunctionOperation, ILocalFunctionOperation
    {
        public LocalFunctionOperation(IMethodSymbol symbol, IBlockOperation body, IBlockOperation ignoredBody, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(symbol, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Body = SetParentOperation(body, this);
            IgnoredBody = SetParentOperation(ignoredBody, this);
        }

        public override IBlockOperation Body { get; }
        public override IBlockOperation IgnoredBody { get; }
    }

/// <summary>
/// Represents a local function statement.
/// </summary>
internal abstract class LazyLocalFunctionOperation : BaseLocalFunctionOperation, ILocalFunctionOperation
{
    private IBlockOperation _lazyBodyInterlocked;
    private IBlockOperation _lazyIgnoredBodyInterlocked;

    public LazyLocalFunctionOperation(IMethodSymbol symbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(symbol, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IBlockOperation CreateBody();
    protected abstract IBlockOperation CreateIgnoredBody();

    public override IBlockOperation Body
    {
        get
        {
            if (_lazyBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBodyInterlocked, CreateBody(), null);
                VerifyParentOperation(this, _lazyBodyInterlocked);
            }

            return _lazyBodyInterlocked;
        }
    }

    public override IBlockOperation IgnoredBody
    {
        get
        {
            if (_lazyIgnoredBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyIgnoredBodyInterlocked, CreateIgnoredBody(), null);
                VerifyParentOperation(this, _lazyIgnoredBodyInterlocked);
            }

            return _lazyIgnoredBodyInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# constant pattern.
    /// </summary>
    internal abstract partial class BaseConstantPatternOperation : Operation, IConstantPatternOperation
    {
        protected BaseConstantPatternOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ConstantPattern, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
            }
        }
        /// <summary>
        /// Constant value of the pattern.
        /// </summary>
        public abstract IOperation Value { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitConstantPattern(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitConstantPattern(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# constant pattern.
    /// </summary>
    internal sealed partial class ConstantPatternOperation : BaseConstantPatternOperation, IConstantPatternOperation
    {
        public ConstantPatternOperation(IOperation value, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
        }

        public override IOperation Value { get; }
    }

/// <summary>
/// Represents a C# constant pattern.
/// </summary>
internal abstract class LazyConstantPatternOperation : BaseConstantPatternOperation, IConstantPatternOperation
{
    private IOperation _lazyValueInterlocked;

    public LazyConstantPatternOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# declaration pattern.
    /// </summary>
    internal sealed partial class DeclarationPatternOperation : Operation, IDeclarationPatternOperation
    {
        public DeclarationPatternOperation(ISymbol declaredSymbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.DeclarationPattern, semanticModel, syntax, type, constantValue, isImplicit)
        {
            DeclaredSymbol = declaredSymbol;
        }
        /// <summary>
        /// Symbol declared by the pattern.
        /// </summary>
        public ISymbol DeclaredSymbol { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDeclarationPattern(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDeclarationPattern(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# pattern case clause.
    /// </summary>
    internal abstract partial class BasePatternCaseClauseOperation : BaseCaseClauseOperation, IPatternCaseClauseOperation
    {
        protected BasePatternCaseClauseOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(CaseKind.Pattern, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Label = label;
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Pattern != null)
                {
                    yield return Pattern;
                }
                if (Guard != null)
                {
                    yield return Guard;
                }
            }
        }
        /// <summary>
        /// Pattern associated with case clause.
        /// </summary>
        public abstract IPatternOperation Pattern { get; }
        /// <summary>
        /// Guard expression associated with the pattern case clause.
        /// </summary>
        public abstract IOperation Guard { get; }
        public override ILabelSymbol Label { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitPatternCaseClause(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitPatternCaseClause(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# pattern case clause.
    /// </summary>
    internal sealed partial class PatternCaseClauseOperation : BasePatternCaseClauseOperation, IPatternCaseClauseOperation
    {
        public PatternCaseClauseOperation(ILabelSymbol label, IPatternOperation pattern, IOperation guardExpression, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(label, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Pattern = SetParentOperation(pattern, this);
            Guard = SetParentOperation(guardExpression, this);
        }

        public override IPatternOperation Pattern { get; }
        public override IOperation Guard { get; }
    }

/// <summary>
/// Represents a C# pattern case clause.
/// </summary>
internal abstract class LazyPatternCaseClauseOperation : BasePatternCaseClauseOperation, IPatternCaseClauseOperation
{
    private IPatternOperation _lazyPatternInterlocked;
    private IOperation _lazyGuardExpressionInterlocked;

    public LazyPatternCaseClauseOperation(ILabelSymbol label, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(label, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IPatternOperation CreatePattern();
    protected abstract IOperation CreateGuard();

    public override IPatternOperation Pattern
    {
        get
        {
            if (_lazyPatternInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyPatternInterlocked, CreatePattern(), null);
                VerifyParentOperation(this, _lazyPatternInterlocked);
            }

            return _lazyPatternInterlocked;
        }
    }

    public override IOperation Guard
    {
        get
        {
            if (_lazyGuardExpressionInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyGuardExpressionInterlocked, CreateGuard(), null);
                VerifyParentOperation(this, _lazyGuardExpressionInterlocked);
            }

            return _lazyGuardExpressionInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# is pattern expression. For example, "x is int i".
    /// </summary>
    internal abstract partial class BaseIsPatternOperation : Operation, IIsPatternOperation
    {
        protected BaseIsPatternOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.IsPattern, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Value != null)
                {
                    yield return Value;
                }
                if (Pattern != null)
                {
                    yield return Pattern;
                }
            }
        }
        /// <summary>
        /// Expression.
        /// </summary>
        public abstract IOperation Value { get; }
        /// <summary>
        /// Pattern.
        /// </summary>
        public abstract IPatternOperation Pattern { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitIsPattern(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitIsPattern(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# is pattern expression. For example, "x is int i".
    /// </summary>
    internal sealed partial class IsPatternOperation : BaseIsPatternOperation, IIsPatternOperation
    {
        public IsPatternOperation(IOperation value, IPatternOperation pattern, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Value = SetParentOperation(value, this);
            Pattern = SetParentOperation(pattern, this);
        }

        public override IOperation Value { get; }
        public override IPatternOperation Pattern { get; }
    }

/// <summary>
/// Represents a C# is pattern expression. For example, "x is int i".
/// </summary>
internal abstract class LazyIsPatternOperation : BaseIsPatternOperation, IIsPatternOperation
{
    private IOperation _lazyValueInterlocked;
    private IPatternOperation _lazyPatternInterlocked;

    public LazyIsPatternOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateValue();
    protected abstract IPatternOperation CreatePattern();

    public override IOperation Value
    {
        get
        {
            if (_lazyValueInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyValueInterlocked, CreateValue(), null);
                VerifyParentOperation(this, _lazyValueInterlocked);
            }

            return _lazyValueInterlocked;
        }
    }

    public override IPatternOperation Pattern
    {
        get
        {
            if (_lazyPatternInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyPatternInterlocked, CreatePattern(), null);
                VerifyParentOperation(this, _lazyPatternInterlocked);
            }

            return _lazyPatternInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# or VB object or collection initializer expression.
    /// </summary>
    internal abstract partial class BaseObjectOrCollectionInitializerOperation : Operation, IObjectOrCollectionInitializerOperation
    {
        protected BaseObjectOrCollectionInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ObjectOrCollectionInitializer, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var initializer in Initializers)
                {
                    if (initializer != null)
                    {
                        yield return initializer;
                    }
                }
            }
        }
        /// <summary>
        /// Object member or collection initializers.
        /// </summary>
        public abstract ImmutableArray<IOperation> Initializers { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitObjectOrCollectionInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitObjectOrCollectionInitializer(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# or VB object or collection initializer expression.
    /// </summary>
    internal sealed partial class ObjectOrCollectionInitializerOperation : BaseObjectOrCollectionInitializerOperation, IObjectOrCollectionInitializerOperation
    {
        public ObjectOrCollectionInitializerOperation(ImmutableArray<IOperation> initializers, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Initializers = SetParentOperation(initializers, this);
        }

        public override ImmutableArray<IOperation> Initializers { get; }
    }

/// <summary>
/// Represents a C# or VB object or collection initializer expression.
/// </summary>
internal abstract class LazyObjectOrCollectionInitializerOperation : BaseObjectOrCollectionInitializerOperation, IObjectOrCollectionInitializerOperation
{
    private ImmutableArray<IOperation> _lazyInitializersInterlocked;

    public LazyObjectOrCollectionInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IOperation> CreateInitializers();

    public override ImmutableArray<IOperation> Initializers
    {
        get
        {
            if (_lazyInitializersInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyInitializersInterlocked, CreateInitializers(), default);
                VerifyParentOperation(this, _lazyInitializersInterlocked);
            }

            return _lazyInitializersInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a C# or VB member initializer expression within an object initializer expression.
    /// </summary>
    internal abstract partial class BaseMemberInitializerOperation : Operation, IMemberInitializerOperation
    {
        protected BaseMemberInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.MemberInitializer, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (InitializedMember != null)
                {
                    yield return InitializedMember;
                }
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }
        /// <summary>
        /// Initialized member.
        /// </summary>
        public abstract IOperation InitializedMember { get; }

        /// <summary>
        /// Member initializer.
        /// </summary>
        public abstract IObjectOrCollectionInitializerOperation Initializer { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitMemberInitializer(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitMemberInitializer(this, argument);
        }
    }

    /// <summary>
    /// Represents a C# or VB member initializer expression within an object initializer expression.
    /// </summary>
    internal sealed partial class MemberInitializerOperation : BaseMemberInitializerOperation, IMemberInitializerOperation
    {
        public MemberInitializerOperation(IOperation initializedMember, IObjectOrCollectionInitializerOperation initializer, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            InitializedMember = SetParentOperation(initializedMember, this);
            Initializer = SetParentOperation(initializer, this);
        }

        public override IOperation InitializedMember { get; }
        public override IObjectOrCollectionInitializerOperation Initializer { get; }
    }

/// <summary>
/// Represents a C# or VB member initializer expression within an object initializer expression.
/// </summary>
internal abstract class LazyMemberInitializerOperation : BaseMemberInitializerOperation, IMemberInitializerOperation
{
    private IOperation _lazyInitializedMemberInterlocked;
    private IObjectOrCollectionInitializerOperation _lazyInitializerInterlocked;

    public LazyMemberInitializerOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateInitializedMember();
    protected abstract IObjectOrCollectionInitializerOperation CreateInitializer();

    public override IOperation InitializedMember
    {
        get
        {
            if (_lazyInitializedMemberInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializedMemberInterlocked, CreateInitializedMember(), null);
                VerifyParentOperation(this, _lazyInitializedMemberInterlocked);
            }

            return _lazyInitializedMemberInterlocked;
        }
    }

    public override IObjectOrCollectionInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }
}

    /// <summary>
    /// Represents an unrolled/lowered query expression in C# and VB.
    /// For example, for the query expression "from x in set where x.Name != null select x.Name", the Operation tree has the following shape:
    ///   ITranslatedQueryExpression
    ///     IInvocationExpression ('Select' invocation for "select x.Name")
    ///       IInvocationExpression ('Where' invocation for "where x.Name != null")
    ///         IInvocationExpression ('From' invocation for "from x in set")
    /// </summary>
    internal abstract partial class BaseTranslatedQueryOperation : Operation, ITranslatedQueryOperation
    {
        protected BaseTranslatedQueryOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.TranslatedQuery, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }
        /// <summary>
        /// Underlying unrolled expression.
        /// </summary>
        public abstract IOperation Operation { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Operation != null)
                {
                    yield return Operation;
                }
            }
        }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitTranslatedQuery(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitTranslatedQuery(this, argument);
        }
    }

    /// <summary>
    /// Represents an unrolled/lowered query expression in C# and VB.
    /// For example, for the query expression "from x in set where x.Name != null select x.Name", the Operation tree has the following shape:
    ///   ITranslatedQueryExpression
    ///     IInvocationExpression ('Select' invocation for "select x.Name")
    ///       IInvocationExpression ('Where' invocation for "where x.Name != null")
    ///         IInvocationExpression ('From' invocation for "from x in set")
    /// </summary>
    internal sealed partial class TranslatedQueryOperation : BaseTranslatedQueryOperation, ITranslatedQueryOperation
    {
        public TranslatedQueryOperation(IOperation operation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operation = SetParentOperation(operation, this);
        }
        public override IOperation Operation { get; }
    }

/// <summary>
/// Represents an unrolled/lowered query expression in C# and VB.
/// For example, for the query expression "from x in set where x.Name != null select x.Name", the Operation tree has the following shape:
///   ITranslatedQueryExpression
///     IInvocationExpression ('Select' invocation for "select x.Name")
///       IInvocationExpression ('Where' invocation for "where x.Name != null")
///         IInvocationExpression ('From' invocation for "from x in set")
/// </summary>
internal abstract class LazyTranslatedQueryOperation : BaseTranslatedQueryOperation, ITranslatedQueryOperation
{
    private IOperation _lazyOperationInterlocked;

    public LazyTranslatedQueryOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperation();

    public override IOperation Operation
    {
        get
        {
            if (_lazyOperationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperationInterlocked, CreateOperation(), null);
                VerifyParentOperation(this, _lazyOperationInterlocked);
            }

            return _lazyOperationInterlocked;
        }
    }
}

    internal sealed partial class FlowCaptureReferenceOperation : Operation, IFlowCaptureReferenceOperation
    {
        public FlowCaptureReferenceOperation(int id, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue) :
            base(OperationKind.FlowCaptureReference, semanticModel: null, syntax: syntax, type: type, constantValue: constantValue, isImplicit: true)
        {
            Id = new CaptureId(id);
        }

        public FlowCaptureReferenceOperation(CaptureId id, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue) :
            base(OperationKind.FlowCaptureReference, semanticModel: null, syntax: syntax, type: type, constantValue: constantValue, isImplicit: true)
        {
            Id = id;
        }

        public CaptureId Id { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFlowCaptureReference(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFlowCaptureReference(this, argument);
        }
    }

    internal sealed partial class FlowCaptureOperation : Operation, IFlowCaptureOperation
    {
        public FlowCaptureOperation(int id, SyntaxNode syntax, IOperation value) :
            base(OperationKind.FlowCapture, semanticModel: null, syntax: syntax, type: null, constantValue: default, isImplicit: true)
        {
            Debug.Assert(value != null);
            Id = new CaptureId(id);
            Value = SetParentOperation(value, this);
        }

        public CaptureId Id { get; }
        public IOperation Value { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                yield return Value;
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFlowCapture(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFlowCapture(this, argument);
        }
    }

    internal sealed partial class IsNullOperation : Operation, IIsNullOperation
    {
        public IsNullOperation(SyntaxNode syntax, IOperation operand, ITypeSymbol type, Optional<object> constantValue) :
            base(OperationKind.IsNull, semanticModel: null, syntax: syntax, type: type, constantValue: constantValue, isImplicit: true)
        {
            Debug.Assert(operand != null);
            Operand = SetParentOperation(operand, this);
        }

        public IOperation Operand { get; }
        public override IEnumerable<IOperation> Children
        {
            get
            {
                yield return Operand;
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitIsNull(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitIsNull(this, argument);
        }
    }

    internal sealed partial class CaughtExceptionOperation : Operation, ICaughtExceptionOperation
    {
        public CaughtExceptionOperation(SyntaxNode syntax, ITypeSymbol type) :
            base(OperationKind.CaughtException, semanticModel: null, syntax: syntax, type: type, constantValue: default, isImplicit: true)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                return Array.Empty<IOperation>();
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitCaughtException(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitCaughtException(this, argument);
        }
    }

    internal sealed class StaticLocalInitializationSemaphoreOperation : Operation, IStaticLocalInitializationSemaphoreOperation
    {
        public StaticLocalInitializationSemaphoreOperation(ILocalSymbol local, SyntaxNode syntax, ITypeSymbol type) :
            base(OperationKind.StaticLocalInitializationSemaphore, semanticModel: null, syntax, type, constantValue: default, isImplicit: true)
        {
            Local = local;
        }

        public ILocalSymbol Local { get; }

        public override IEnumerable<IOperation> Children
        {
            get => Array.Empty<IOperation>();
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitStaticLocalInitializationSemaphore(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitStaticLocalInitializationSemaphore(this, argument);
        }
    }

    internal abstract class BaseMethodBodyBaseOperation : Operation, IMethodBodyBaseOperation
    {
        protected BaseMethodBodyBaseOperation(OperationKind kind, SemanticModel semanticModel, SyntaxNode syntax) :
            base(kind, semanticModel, syntax, type: null, constantValue: default, isImplicit: false)
        {
        }

        public abstract IBlockOperation BlockBody { get; }
        public abstract IBlockOperation ExpressionBody { get; }
        public abstract override IEnumerable<IOperation> Children { get; }
    }

    internal abstract class BaseMethodBodyOperation : BaseMethodBodyBaseOperation, IMethodBodyOperation
    {
        protected BaseMethodBodyOperation(SemanticModel semanticModel, SyntaxNode syntax) :
            base(OperationKind.MethodBody, semanticModel, syntax)
        {
        }

        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                IBlockOperation blockBody = BlockBody;
                if (blockBody != null)
                {
                    yield return blockBody;
                }

                IBlockOperation expressionBody = ExpressionBody;
                if (expressionBody != null)
                {
                    yield return expressionBody;
                }
            }
        }

        public sealed override void Accept(OperationVisitor visitor)
        {
            visitor.VisitMethodBodyOperation(this);
        }

        public sealed override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitMethodBodyOperation(this, argument);
        }
    }

    internal sealed class MethodBodyOperation : BaseMethodBodyOperation
    {
        public MethodBodyOperation(SemanticModel semanticModel, SyntaxNode syntax, IBlockOperation blockBody, IBlockOperation expressionBody) :
            base(semanticModel, syntax)
        {
            BlockBody = SetParentOperation(blockBody, this);
            ExpressionBody = SetParentOperation(expressionBody, this);
        }

        public override IBlockOperation BlockBody { get; }
        public override IBlockOperation ExpressionBody { get; }
    }

internal abstract class LazyMethodBodyOperation : BaseMethodBodyOperation
{
    private IBlockOperation _lazyBlockBodyInterlocked;
    private IBlockOperation _lazyExpressionBodyInterlocked;

    public LazyMethodBodyOperation(SemanticModel semanticModel, SyntaxNode syntax) : base(semanticModel, syntax)
    {
    }

    protected abstract IBlockOperation CreateBlockBody();
    protected abstract IBlockOperation CreateExpressionBody();

    public override IBlockOperation BlockBody
    {
        get
        {
            if (_lazyBlockBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBlockBodyInterlocked, CreateBlockBody(), null);
                VerifyParentOperation(this, _lazyBlockBodyInterlocked);
            }

            return _lazyBlockBodyInterlocked;
        }
    }

    public override IBlockOperation ExpressionBody
    {
        get
        {
            if (_lazyExpressionBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyExpressionBodyInterlocked, CreateExpressionBody(), null);
                VerifyParentOperation(this, _lazyExpressionBodyInterlocked);
            }

            return _lazyExpressionBodyInterlocked;
        }
    }
}

    internal abstract class BaseConstructorBodyOperation : BaseMethodBodyBaseOperation, IConstructorBodyOperation
    {
        protected BaseConstructorBodyOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax) :
            base(OperationKind.ConstructorBody, semanticModel, syntax)
        {
            Locals = locals;
        }

        public ImmutableArray<ILocalSymbol> Locals { get; }
        public abstract IOperation Initializer { get; }

        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                IOperation initializer = Initializer;
                if (initializer != null)
                {
                    yield return initializer;
                }

                IBlockOperation blockBody = BlockBody;
                if (blockBody != null)
                {
                    yield return blockBody;
                }

                IBlockOperation expressionBody = ExpressionBody;
                if (expressionBody != null)
                {
                    yield return expressionBody;
                }
            }
        }

        public sealed override void Accept(OperationVisitor visitor)
        {
            visitor.VisitConstructorBodyOperation(this);
        }

        public sealed override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitConstructorBodyOperation(this, argument);
        }
    }

    internal sealed class ConstructorBodyOperation : BaseConstructorBodyOperation
    {
        public ConstructorBodyOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax,
                                        IOperation initializer, IBlockOperation blockBody, IBlockOperation expressionBody) :
            base(locals, semanticModel, syntax)
        {
            Initializer = SetParentOperation(initializer, this);
            BlockBody = SetParentOperation(blockBody, this);
            ExpressionBody = SetParentOperation(expressionBody, this);
        }

        public override IOperation Initializer { get; }
        public override IBlockOperation BlockBody { get; }
        public override IBlockOperation ExpressionBody { get; }
    }

internal abstract class LazyConstructorBodyOperation : BaseConstructorBodyOperation
{
    private IOperation _lazyInitializerInterlocked;
    private IBlockOperation _lazyBlockBodyInterlocked;
    private IBlockOperation _lazyExpressionBodyInterlocked;

    public LazyConstructorBodyOperation(ImmutableArray<ILocalSymbol> locals, SemanticModel semanticModel, SyntaxNode syntax) : base(locals, semanticModel, syntax)
    {
    }

    protected abstract IOperation CreateInitializer();
    protected abstract IBlockOperation CreateBlockBody();
    protected abstract IBlockOperation CreateExpressionBody();

    public override IOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }

    public override IBlockOperation BlockBody
    {
        get
        {
            if (_lazyBlockBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyBlockBodyInterlocked, CreateBlockBody(), null);
                VerifyParentOperation(this, _lazyBlockBodyInterlocked);
            }

            return _lazyBlockBodyInterlocked;
        }
    }

    public override IBlockOperation ExpressionBody
    {
        get
        {
            if (_lazyExpressionBodyInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyExpressionBodyInterlocked, CreateExpressionBody(), null);
                VerifyParentOperation(this, _lazyExpressionBodyInterlocked);
            }

            return _lazyExpressionBodyInterlocked;
        }
    }
}

    internal sealed class DiscardOperation : Operation, IDiscardOperation
    {
        public DiscardOperation(IDiscardSymbol discardSymbol, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.Discard, semanticModel, syntax, type, constantValue, isImplicit)
        {
            DiscardSymbol = discardSymbol;
        }

        public IDiscardSymbol DiscardSymbol { get; }

        public override IEnumerable<IOperation> Children => Array.Empty<IOperation>();

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitDiscardOperation(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitDiscardOperation(this, argument);
        }
    }

    /// <summary>
    /// Represents a standalone VB query Aggregate operation with more than one item in Into clause.
    /// </summary>
    internal abstract partial class BaseAggregateQueryOperation : Operation, IAggregateQueryOperation
    {
        protected BaseAggregateQueryOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.None, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Group != null)
                {
                    yield return Group;
                }
                if (Aggregation != null)
                {
                    yield return Aggregation;
                }
            }
        }

        /// <summary>
        /// See BoundAggregateClause node in VB compiler.
        /// </summary>
        public abstract IOperation Group { get; }

        /// <summary>
        /// See BoundAggregateClause node in VB compiler.
        /// </summary>
        public abstract IOperation Aggregation { get; }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitAggregateQuery(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitAggregateQuery(this, argument);
        }
    }

    /// <summary>
    /// Represents a standalone VB query Aggregate operation with more than one item in Into clause.
    /// </summary>
    internal sealed partial class AggregateQueryOperation : BaseAggregateQueryOperation
    {
        public AggregateQueryOperation(IOperation group, IOperation aggregation, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Group = SetParentOperation(group, this);
            Aggregation = SetParentOperation(aggregation, this);
        }

        public override IOperation Group { get; }
        public override IOperation Aggregation { get; }
    }

/// <summary>
/// Represents a standalone VB query Aggregate operation with more than one item in Into clause.
/// </summary>
internal abstract class LazyAggregateQueryOperation : BaseAggregateQueryOperation
{
    private IOperation _lazyGroupInterlocked;
    private IOperation _lazyAggregationInterlocked;

    public LazyAggregateQueryOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateGroup();
    protected abstract IOperation CreateAggregation();

    public override IOperation Group
    {
        get
        {
            if (_lazyGroupInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyGroupInterlocked, CreateGroup(), null);
                VerifyParentOperation(this, _lazyGroupInterlocked);
            }

            return _lazyGroupInterlocked;
        }
    }

    public override IOperation Aggregation
    {
        get
        {
            if (_lazyAggregationInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyAggregationInterlocked, CreateAggregation(), null);
                VerifyParentOperation(this, _lazyAggregationInterlocked);
            }

            return _lazyAggregationInterlocked;
        }
    }
}

    /// <summary>
    /// Represents a creation of an instance of a NoPia interface, i.e. new I(), where I is an embedded NoPia interface.
    /// </summary>
    internal abstract partial class BaseNoPiaObjectCreationOperation : Operation, INoPiaObjectCreationOperation
    {
        public BaseNoPiaObjectCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.None, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public override IEnumerable<IOperation> Children
        {
            get
            {
                if (Initializer != null)
                {
                    yield return Initializer;
                }
            }
        }
        /// <summary>
        /// Object or collection initializer, if any.
        /// </summary>
        public abstract IObjectOrCollectionInitializerOperation Initializer { get; }
        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitNoPiaObjectCreation(this);
        }
        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitNoPiaObjectCreation(this, argument);
        }
    }

    /// <summary>
    /// Represents a creation of an instance of a NoPia interface, i.e. new I(), where I is an embedded NoPia interface.
    /// </summary>
    internal sealed partial class NoPiaObjectCreationOperation : BaseNoPiaObjectCreationOperation
    {
        public NoPiaObjectCreationOperation(IObjectOrCollectionInitializerOperation initializer, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Initializer = SetParentOperation(initializer, this);
        }
        public override IObjectOrCollectionInitializerOperation Initializer { get; }
    }

/// <summary>
/// Represents a creation of an instance of a NoPia interface, i.e. new I(), where I is an embedded NoPia interface.
/// </summary>
internal abstract class LazyNoPiaObjectCreationOperation : BaseNoPiaObjectCreationOperation
{
    private IObjectOrCollectionInitializerOperation _lazyInitializerInterlocked;

    public LazyNoPiaObjectCreationOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IObjectOrCollectionInitializerOperation CreateInitializer();

    public override IObjectOrCollectionInitializerOperation Initializer
    {
        get
        {
            if (_lazyInitializerInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyInitializerInterlocked, CreateInitializer(), null);
                VerifyParentOperation(this, _lazyInitializerInterlocked);
            }

            return _lazyInitializerInterlocked;
        }
    }
}

    internal abstract class BaseFromEndIndexOperation : Operation, IFromEndIndexOperation
    {
        protected BaseFromEndIndexOperation(bool isLifted, bool isImplicit, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, IMethodSymbol symbol) :
                    base(OperationKind.FromEndIndex, semanticModel, syntax, type, constantValue: default, isImplicit: isImplicit)
        {
            IsLifted = isLifted;
            Symbol = symbol;
        }

        public abstract IOperation Operand { get; }
        public bool IsLifted { get; }
        public IMethodSymbol Symbol { get; }

        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                IOperation operand = Operand;
                if (operand != null)
                {
                    yield return operand;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitFromEndIndexOperation(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitFromEndIndexOperation(this, argument);
        }
    }

    internal sealed class FromEndIndexOperation : BaseFromEndIndexOperation
    {
        public FromEndIndexOperation(bool isLifted, bool isImplicit, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, IOperation operand, IMethodSymbol symbol) :
                    base(isLifted, isImplicit, semanticModel, syntax, type, symbol)
        {
            Operand = Operation.SetParentOperation(operand, this);
        }

        public override IOperation Operand { get; }
    }

internal abstract class LazyFromEndIndexOperation : BaseFromEndIndexOperation
{
    private IOperation _operandInterlocked;

    public LazyFromEndIndexOperation(bool isLifted, bool isImplicit, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, IMethodSymbol symbol) : base(isLifted, isImplicit, semanticModel, syntax, type, symbol)
    {
    }

    protected abstract IOperation CreateOperand();

    public override IOperation Operand
    {
        get
        {
            if (_operandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _operandInterlocked, CreateOperand(), null);
                VerifyParentOperation(this, _operandInterlocked);
            }

            return _operandInterlocked;
        }
    }
}

    internal abstract class BaseRangeOperation : Operation, IRangeOperation
    {
        protected BaseRangeOperation(bool isLifted, bool isImplicit, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, IMethodSymbol symbol) :
                    base(OperationKind.Range, semanticModel, syntax, type, constantValue: default, isImplicit: isImplicit)
        {
            IsLifted = isLifted;
            Method = symbol;
        }

        public abstract IOperation LeftOperand { get; }
        public abstract IOperation RightOperand { get; }
        public bool IsLifted { get; }
        public IMethodSymbol Method { get; }

        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                IOperation leftOperand = LeftOperand;
                if (leftOperand != null)
                {
                    yield return leftOperand;
                }

                IOperation rightOperand = RightOperand;
                if (rightOperand != null)
                {
                    yield return rightOperand;
                }
            }
        }

        public override void Accept(OperationVisitor visitor)
        {
            visitor.VisitRangeOperation(this);
        }

        public override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitRangeOperation(this, argument);
        }
    }

    internal sealed class RangeOperation : BaseRangeOperation
    {
        public RangeOperation(bool isLifted, bool isImplicit, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, IOperation leftOperand, IOperation rightOperand, IMethodSymbol symbol) :
                    base(isLifted, isImplicit, semanticModel, syntax, type, symbol)
        {
            LeftOperand = Operation.SetParentOperation(leftOperand, this);
            RightOperand = Operation.SetParentOperation(rightOperand, this);
        }

        public override IOperation LeftOperand { get; }
        public override IOperation RightOperand { get; }
    }

internal abstract class LazyRangeOperation : BaseRangeOperation
{
    private IOperation _leftOperandInterlocked;
    private IOperation _rightOperandInterlocked;

    public LazyRangeOperation(bool isLifted, bool isImplicit, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, IMethodSymbol symbol) : base(isLifted, isImplicit, semanticModel, syntax, type, symbol)
    {
    }

    protected abstract IOperation CreateLeftOperand();
    protected abstract IOperation CreateRightOperand();

    public override IOperation LeftOperand
    {
        get
        {
            if (_leftOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _leftOperandInterlocked, CreateLeftOperand(), null);
                VerifyParentOperation(this, _leftOperandInterlocked);
            }

            return _leftOperandInterlocked;
        }
    }

    public override IOperation RightOperand
    {
        get
        {
            if (_rightOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _rightOperandInterlocked, CreateRightOperand(), null);
                VerifyParentOperation(this, _rightOperandInterlocked);
            }

            return _rightOperandInterlocked;
        }
    }
}

    internal abstract class BaseReDimOperation : Operation, IReDimOperation
    {
        protected BaseReDimOperation(bool preserve, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ReDim, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Preserve = preserve;
        }

        public abstract ImmutableArray<IReDimClauseOperation> Clauses { get; }
        public bool Preserve { get; }

        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                foreach (var clause in Clauses)
                {
                    yield return clause;
                }
            }
        }

        public sealed override void Accept(OperationVisitor visitor)
        {
            visitor.VisitReDim(this);
        }

        public sealed override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitReDim(this, argument);
        }
    }

    internal sealed class ReDimOperation : BaseReDimOperation
    {
        public ReDimOperation(ImmutableArray<IReDimClauseOperation> clauses, bool preserve, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(preserve, semanticModel, syntax, type, constantValue, isImplicit)
        {
            Clauses = SetParentOperation(clauses, this);
        }

        public override ImmutableArray<IReDimClauseOperation> Clauses { get; }
    }

internal abstract class LazyReDimOperation : BaseReDimOperation
{
    private ImmutableArray<IReDimClauseOperation> _lazyClausesInterlocked;

    public LazyReDimOperation(bool preserve, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(preserve, semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract ImmutableArray<IReDimClauseOperation> CreateClauses();

    public override ImmutableArray<IReDimClauseOperation> Clauses
    {
        get
        {
            if (_lazyClausesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyClausesInterlocked, CreateClauses(), default);
                VerifyParentOperation(this, _lazyClausesInterlocked);
            }

            return _lazyClausesInterlocked;
        }
    }
}

    internal abstract class BaseReDimClauseOperation : Operation, IReDimClauseOperation
    {
        protected BaseReDimClauseOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(OperationKind.ReDimClause, semanticModel, syntax, type, constantValue, isImplicit)
        {
        }

        public abstract IOperation Operand { get; }
        public abstract ImmutableArray<IOperation> DimensionSizes { get; }

        public sealed override IEnumerable<IOperation> Children
        {
            get
            {
                Debug.Assert(Operand != null);
                yield return Operand;

                foreach (var index in DimensionSizes)
                {
                    Debug.Assert(index != null);
                    yield return index;
                }
            }
        }

        public sealed override void Accept(OperationVisitor visitor)
        {
            visitor.VisitReDimClause(this);
        }

        public sealed override TResult Accept<TArgument, TResult>(OperationVisitor<TArgument, TResult> visitor, TArgument argument)
        {
            return visitor.VisitReDimClause(this, argument);
        }
    }

    internal sealed class ReDimClauseOperation : BaseReDimClauseOperation
    {
        public ReDimClauseOperation(IOperation operand, ImmutableArray<IOperation> dimensionSizes, SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) :
            base(semanticModel, syntax, type, constantValue, isImplicit)
        {
            Operand = SetParentOperation(operand, this);
            DimensionSizes = SetParentOperation(dimensionSizes, this);
        }

        public override IOperation Operand { get; }
        public override ImmutableArray<IOperation> DimensionSizes { get; }
    }

internal abstract class LazyReDimClauseOperation : BaseReDimClauseOperation
{
    private IOperation _lazyOperandInterlocked;
    private ImmutableArray<IOperation> _lazyDimensionSizesInterlocked;

    public LazyReDimClauseOperation(SemanticModel semanticModel, SyntaxNode syntax, ITypeSymbol type, Optional<object> constantValue, bool isImplicit) : base(semanticModel, syntax, type, constantValue, isImplicit)
    {
    }

    protected abstract IOperation CreateOperand();
    protected abstract ImmutableArray<IOperation> CreateDimensionSizes();

    public override IOperation Operand
    {
        get
        {
            if (_lazyOperandInterlocked is null)
            {
                Interlocked.CompareExchange(ref _lazyOperandInterlocked, CreateOperand(), null);
                VerifyParentOperation(this, _lazyOperandInterlocked);
            }

            return _lazyOperandInterlocked;
        }
    }

    public override ImmutableArray<IOperation> DimensionSizes
    {
        get
        {
            if (_lazyDimensionSizesInterlocked.IsDefault)
            {
                ImmutableInterlocked.InterlockedCompareExchange(ref _lazyDimensionSizesInterlocked, CreateDimensionSizes(), default);
                VerifyParentOperation(this, _lazyDimensionSizesInterlocked);
            }

            return _lazyDimensionSizesInterlocked;
        }
    }
}
}
