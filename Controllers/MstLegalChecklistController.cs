using GenericController.Controllers;
using GenericController.DTOs;
using GenericController.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GenericController.Controllers
{
    [Route("api/[controller]")]
    public class MstLegalChecklistController : GenericMasterController<MstLegalChecklist, MstLegalChecklistDto>
    {
        public MstLegalChecklistController(
            IMediator mediator,
            ILogger<MstLegalChecklistController> logger)
            : base(mediator, logger)
        {
        }

        protected override object GetIdFromDto(MstLegalChecklistDto dto)
        {
            return dto.LegalChkId;
        }

        protected override System.Linq.Expressions.Expression<System.Func<MstLegalChecklist, bool>>? CreateFilterExpression(string filter)
        {
            // Example: filter by ScreenPoint or Recommendation
            return e => (e.ScreenPoint != null && e.ScreenPoint.Contains(filter)) ||
                       (e.Recommendation != null && e.Recommendation.Contains(filter));
        }
    }
}