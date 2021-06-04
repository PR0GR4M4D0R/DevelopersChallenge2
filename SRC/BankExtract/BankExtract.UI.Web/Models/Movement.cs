using BankExtract.UI.Web.Utils;
using System;
using System.Runtime.Serialization;

namespace BankExtract.UI.Web.Models
{
    /// <summary>
    /// Movement performed.
    /// </summary>
    public class Movement
    {
        /// <summary>
        /// Drive type.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Date of movement.
        /// </summary>
        [DataMember]
        public DateTime? DateMovement { get; set; }

        /// <summary>
        /// Move value.
        /// </summary>
        [DataMember]
        public decimal? Value { get; set; }

        /// <summary>
        /// Description of the drive.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Drive bench.
        /// </summary>
        [DataMember]
        public DtoConcept<int?> Bank { get; set; }

        /// <summary>
        /// Checks that the movement data is all filled out.
        /// </summary>
        /// <returns>True if all data is filled.</returns>
        public bool PopulatedMovement()
        {
            return !string.IsNullOrEmpty(Type)
                   && !string.IsNullOrEmpty(Description)
                   && DateMovement.EhDataValida()
                   && Value.HasValue
                   && Value != 0;
        }
    }
}