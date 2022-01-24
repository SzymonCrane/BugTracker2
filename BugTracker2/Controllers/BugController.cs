using BugTracker2.Interfaces;
using BugTracker2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BugTracker2.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BugController : ControllerBase
    {
        private IBugRepository _bugRepository { get; set; }

        public BugController(IBugRepository bugRepository)
        {
            _bugRepository = bugRepository;
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateAsync(Bug data)
        //{
            
        //    try {
        //        if (ModelState.IsValid)
        //        {
        //            await _dataContext.Bugs.AddAsync(data);
        //            await _dataContext.SaveChangesAsync();
        //            return Ok(data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ex");
        //    }
        //    return new JsonResult("Something went wrong.") { StatusCode = 500 };
        //}
        /// <summary>
        /// Endpoint that will get you a list of available bugs.
        /// </summary>
        /// <param name="count">Number of bugs per page</param>
        /// <param name="page">Page number</param>
        /// <returns> </returns>
        //[HttpGet]
        //public async Task<ActionResult> GetAll([FromQuery] int count,[FromQuery] int page)
        //{
        //    var bugs = await _dataContext.Bugs.Skip((page - 1) * count).Take(count).ToListAsync();
        //    if (count <= 0)
        //    {
        //        throw new ArgumentException("Invalid count.", nameof(count));
        //    }
        //    return Ok(bugs);

        //}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var bug = await _bugRepository.GetBugById(id);
            if (bug is null)
            {
                return NotFound();
            }
            return Ok(bug);
        }
        //[producesresponsetype(statuscodes.status200ok)]
        //[producesresponsetype(statuscodes.status404notfound)]
        //[httpdelete("{id}")]
        //public async task<iactionresult> deleteasync(int id)
        //{
        //    var bug = await _bugrepository.getbugbyid(id);
        //    if (bug is null)
        //    {
        //        return notfound();
        //    }
        //    _datacontext.bugs.remove(bug);
        //    await _datacontext.savechangesasync();
        //    return ok(bug);
        //}
        //[httpput("{id}")]
        //public async task<iactionresult> updateasync(int id, bug bug)
        //{
        //    // change to automapper
        //    if (bug.id != id)
        //    {
        //        return badrequest();
        //    }
        //    var bugtoupdate = await _datacontext.bugs.firstordefaultasync(x => x.id == id);
        //    if (bug is null)
        //    {
        //        return notfound();
        //    }
        //    bugtoupdate.name = bug.name;
        //    bugtoupdate.description = bug.description;
        //    await _datacontext.savechangesasync();
        //    return nocontent();
        //}
        //[httppatch("{id}")]
        //public async task<iactionresult> patchasync(int id, jsonpatchdocument<bug> jsonpatchdocument)
        //{
        //    if (jsonpatchdocument is null || !jsonpatchdocument.operations.any() || !modelstate.isvalid)
        //    {
        //        return badrequest(modelstate);
        //    }
        //    var bug = await _datacontext.bugs.firstordefaultasync(x => x.id == id);
        //    if (bug is null)
        //    {
        //        return notfound();
        //    }
        //    jsonpatchdocument.applyto(bug);
        //    await _datacontext.savechangesasync();
        //    return nocontent();
        //}
    }
}
