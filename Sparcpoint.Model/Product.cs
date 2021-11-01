namespace Sparcpoint.Model
{
    public class Product
    {
        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product image uris 
        /// </summary>
        public string ImageUris { get; set; }

        /// <summary>
        /// Valid Skus
        /// </summary>
        public string ValidSkus { get; set; }

        /// <summary>
        /// Category instance id
        /// </summary>
        public int CategoryId { get; set; }
    }
}
