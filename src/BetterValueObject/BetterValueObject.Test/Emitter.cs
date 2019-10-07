namespace BetterValueObject.Test
{
    using System;
    using BetterValueObject.Emitter;
    using BetterValueObject.Emitter.Exceptions;
    using FluentAssertions;
    using Xunit;

    public interface IHaveOnlyGetOnlyProperties
    {
        int Number { get; }
        string Name { get; }
    }

    public interface IHaveASetProperty
    {
        int Number { get; }
        string Name { get; set; }
    }

    public interface IHaveAMethod
    {
        int Number { get; }
        string Name { get; }
        void TryMe();
    }

    public class TypeEmitterSpecs
    {
        private readonly TypeEmitter sut;

        public TypeEmitterSpecs()
            => sut = new TypeEmitter();

        [Fact]
        public void Emit_WithAnInterfaceWithOnlyGetOnlyProperties_ShouldReturnClassTypeImplementingInterfaceAndIEquatable()
        {
            // Arrange
            var targetType = typeof(IHaveOnlyGetOnlyProperties);

            //Act
            var emittedType = sut.Implement(targetType);

            //Assert
            emittedType.Should().NotBeNull();
            emittedType.Should().Implement(targetType);
            emittedType.Should().Implement(typeof(IEquatable<IHaveOnlyGetOnlyProperties>));
        }

        [Fact]
        public void Emit_WithAnInterfaceWithSetProperties_ShouldFail()
        {
            // Arrange
            var targetType = typeof(IHaveASetProperty);

            //Act
            var exception = Record.Exception(() => sut.Implement(targetType));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<MutateableTypeNotAllowedException>();
        }

        [Fact]
        public void Emit_WithAnInterfaceWithAnyMethod_ShouldFail()
        {
            // Arrange
            var targetType = typeof(IHaveAMethod);

            //Act
            var exception = Record.Exception(() => sut.Implement(targetType));

            //Assert
            exception.Should().NotBeNull();
            exception.Should().BeOfType<TypeWithUnsupportedMethodsException>();
        }

        [Fact]
        public void Emit_WithAnInterfaceWithOnlyGetOnlyProperties_ShouldReturnTypeWithDefaultCtor()
        {
            // Arrange
            var targetType = typeof(IHaveOnlyGetOnlyProperties);

            //Act
            var emittedType = sut.Implement(targetType);

            //Assert
            emittedType.Should().NotBeNull();
            emittedType.Should().HaveConstructor(new Type[0]);
            emittedType.Should().HaveConstructor(new Type[0]).Which.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void Emit_WithAnInterfaceWithOnlyGetOnlyProperties_ShouldReturnTypeWithAllTheProperties()
        {
            // Arrange
            var targetType = typeof(IHaveOnlyGetOnlyProperties);

            //Act
            var emittedType = sut.Implement(targetType);

            //Assert
            emittedType.Should().NotBeNull();
            foreach (var property in targetType.Properties())
            {
                emittedType.Should().HaveProperty(property.PropertyType, property.Name);
            }
        }

        [Fact]
        public void Emit_WithAnInterfaceWithOnlyGetOnlyProperties_ShouldReturnTypeWithAnOverridenEqualsAndImplementIEquatable()
        {
            // Arrange
            var targetType = typeof(IHaveOnlyGetOnlyProperties);

            //Act
            var emittedType = sut.Implement(targetType);

            //Assert
            emittedType.Should().NotBeNull();
            emittedType.Should().HaveMethod("Equals", new[] {typeof(object)})
                .Which.DeclaringType.Should().Be(emittedType);
            emittedType.Should().HaveMethod("Equals", new[] {targetType});
            emittedType.Should().HaveMethod("op_Equality", new[] {targetType, targetType});
            emittedType.Should().HaveMethod("op_Inequality", new[] {targetType, targetType});
        }

        [Fact]
        public void Emit_WithAnInterfaceWithOnlyGetOnlyProperties_ShouldReturnTypeWithAnOverridenGetHashCode()
        {
            // Arrange
            var targetType = typeof(IHaveOnlyGetOnlyProperties);

            //Act
            var emittedType = sut.Implement(targetType);

            //Assert
            emittedType.Should().NotBeNull();
            emittedType.Should().HaveMethod("GetHashCode", new Type[0]);
        }
    }
}
