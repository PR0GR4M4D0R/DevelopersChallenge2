using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankExtract.UI.Web.Models
{
    /// <summary>
    /// Contains the data for a move.
    /// </summary>
    public class Extract
    {
        public Extract()
        {
            Movements = new List<Movement>();
        }

        /// <summary>
        /// List of performed moves.
        /// </summary>
        [DataMember]
        public List<Movement> Movements { get; set; }

        /// <summary>
        /// Selected files on the form.
        /// </summary>
        [DataMember]
        public List<HttpPostedFileBase> Files { get; set; }

        /// <summary>
        /// Checks if there is movement equal to the parameter object.
        /// </summary>
        /// <param name="obj">Data for comparison.</param>
        /// <returns>True if there is any movement identical to the parameter object.</returns>
        public bool ExistMovementEquals(Movement obj)
        {
            return Movements.Any(x => x.Type == obj.Type
                                   && x.Description == obj.Description
                                   && x.DateMovement == obj.DateMovement
                                   && x.Value == obj.Value) ;
        }
    }
}