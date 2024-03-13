using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BusinessInterface
{
    /// <summary>
    /// paging result will be encapsulate in to this kind object
    /// </summary>
    public class PagingData<T> where T : class
    {

        /// <summary>
        /// total count
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// current page 
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        ///  count per page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// data in current page
        /// </summary>
        public List<T>? DataList { get; set; }

        public string? SearchString { get; set; }

    }
}
