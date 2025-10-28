using GenericController.Controllers;
using GenericController.DTOs;
using GenericController.Models;
using GenericController.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenericController.Controllers
{
    [Route("api/[controller]")]
    public class MstMarketingNoteController : GenericMasterController<MstMarketingNote, MstMarketingNoteDto>
    {
        public MstMarketingNoteController(
            IGenericService<MstMarketingNote, MstMarketingNoteDto> service,
            ILogger<MstMarketingNoteController> logger)
            : base(service, logger)
        {
        }

        protected override object GetIdFromDto(MstMarketingNoteDto dto)
        {
            return dto.NoteCode;
        }

        protected override System.Linq.Expressions.Expression<System.Func<MstMarketingNote, bool>>? CreateFilterExpression(string filter)
        {
            // Example: filter by NoteCode or Description
            return e => e.NoteCode.Contains(filter) || (e.Description != null && e.Description.Contains(filter));
        }
    }
}