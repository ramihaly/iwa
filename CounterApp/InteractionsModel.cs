namespace Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class InteractionsModel
    {
        /// <summary>
        /// Org Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public int Sent { get; set; }

        [DataMember]
        public int Delivered { get; set; }
    }
}