using System;
using System.Diagnostics;
using System.Reflection;

using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="PropertyInfo"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInfoAssertions :
        MemberInfoAssertions<PropertyInfo, PropertyInfoAssertions>
    {
        public PropertyInfoAssertions(PropertyInfo propertyInfo)
        {
            Subject = propertyInfo;
        }

        /// <summary>
        /// Asserts that the selected property is virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeVirtual(
            string because = "", params object[] reasonArgs)
        {
            string failureMessage = "Expected property " +
                                    GetDescriptionFor(Subject) +
                                    " to be virtual{reason}, but it is not virtual.";

            Execute.Assertion
                .ForCondition(!IsGetterNonVirtual(Subject))
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property has a setter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeWritable(
            string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CanWrite)
                .BecauseOf(because, reasonArgs)
                .FailWith(
                    "Expected {context:property} {0} to have a setter{reason}.",
                    Subject);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property returns a specified type.
        /// </summary>
        /// <param name="propertyType">The expected type of the property.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> Return(Type propertyType,
            string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(Subject.PropertyType == propertyType)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected Type of property " + Subject.Name + " to be {0}{reason}, but it is {1}.", propertyType, Subject.PropertyType);


            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        internal static bool IsGetterNonVirtual(PropertyInfo property)
        {
            MethodInfo getter = property.GetGetMethod(true);
            return MethodInfoAssertions.IsNonVirtual(getter);
        }

        internal static string GetDescriptionFor(PropertyInfo property)
        {
            string propTypeName = property.PropertyType.Name;
            return String.Format("{0} {1}.{2}", propTypeName,
                property.DeclaringType, property.Name);
        }

        internal override string SubjectDescription
        {
            get { return GetDescriptionFor(Subject); }
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "property"; }
        }
    }
}