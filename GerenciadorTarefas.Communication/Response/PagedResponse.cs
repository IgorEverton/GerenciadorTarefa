using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Communication.Response
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int PagerNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }


        public PagedResponse(IEnumerable<T> data, int pagerNumber, int pageSize, int totalCount)
        {
            PagerNumber = pagerNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);
            TotalCount = totalCount;
            Data = data;
        }
    }
}
