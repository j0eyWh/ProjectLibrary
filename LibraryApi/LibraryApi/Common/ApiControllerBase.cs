using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DataGateway.Repository;

namespace LibraryApi.Common
{
    public class ApiControllerBase : ApiController
    {
        private UnitOfWork _unitOfWork;
        public UnitOfWork UnitOfWork => _unitOfWork ?? (_unitOfWork = new UnitOfWork());
    }
}