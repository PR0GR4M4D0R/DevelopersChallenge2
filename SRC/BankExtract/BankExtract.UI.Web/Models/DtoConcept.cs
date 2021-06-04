using System.Runtime.Serialization;

namespace BankExtract.UI.Web.Models
{
    /// <summary>
    /// Contains the data for a concept.
    /// </summary>
    /// <typeparam name="T">Type of code.</typeparam>
    public class DtoConcept<T>
    {
        /// <summary>
        /// Gets or sets the concept code.
        /// </summary>
        /// <value>
        /// Concept code.
        /// </value>
        [DataMember]
        public T Code { get; set; }

        /// <summary>
        /// Gets or sets the concept description.
        /// </summary>
        /// <value>
        /// Description of the concept.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Concatenates the concept code and description information.
        /// </summary>
        /// <returns>Return the code and description concatenated.</returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
