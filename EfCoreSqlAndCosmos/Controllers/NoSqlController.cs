﻿// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.BooksCommon;
using ServiceLayer.BooksNoSql;
using ServiceLayer.BooksSql;
using ServiceLayer.BooksSql.Dtos;
using ServiceLayer.Logger;

namespace EfCoreSqlAndCosmos.Controllers
{
    public class NoSqlController : BaseTraceController
    {
        public async Task<IActionResult> Index (SortFilterPageOptions options, [FromServices] IListNoSqlBooksService service)
        {
            var output = await (await service.SortFilterPageAsync(options)).ToListAsync();
            SetupTraceInfo();
            return View(new BookListNoSqlCombinedDto(options, output));              
        }


        /// <summary>
        /// This provides the filter search dropdown content
        /// </summary>
        /// <param name="options"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFilterSearchContent    
            (SortFilterPageOptions options, [FromServices]IBookNoSqlFilterDropdownService service)         
        {

            var traceIdent = HttpContext.TraceIdentifier; 
            return Json(                            
                new TraceIndentGeneric<IEnumerable<DropdownTuple>>(
                traceIdent,
                service.GetFilterDropDownValues(options.FilterBy)));            
        }

    }
}
